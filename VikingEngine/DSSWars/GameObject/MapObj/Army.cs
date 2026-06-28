using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Xsl;

using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.EngineSpace.Graphics.In3D;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.HeroQuest.Data;
using VikingEngine.ToGG.HeroQuest.Data.Condition;

namespace VikingEngine.DSSWars.GameObject
{
    partial class Army : AbsArmy
    {
        public const float MaxTradeDistance = 3;

        const LootFest.VoxelModelName OverviewBannerModelName = LootFest.VoxelModelName.armystand;

        protected Graphics.AbsVoxelObj overviewBanner;

       
        public float soldierRadius = 0.5f;
        BoundingSphere bound;
        
        public int id;
       
        
        //public int upkeep;
        public float transportSpeedLand = DssConst.Men_StandardWalkingSpeed;
        public float transportSpeedSea = DssConst.Men_StandardShipSpeed;
        public bool isShip = false;

        public float terrainSpeedMultiplier = 1.0f;
       
        ObjectName name = new ObjectName();

        static readonly Vector2 CamCullingRadius = new Vector2(DssVar.SoldierGroup_Spacing * 1.4f);
        public Vector2 cullingTopLeft, cullingBottomRight;

        public float food = 0;
        public float conservedFood = 0;
        public SoldierUpkeep totalUpkeep = new SoldierUpkeep();
        public int missingUpkeepSeconds = 0;
        public float foodBuffer_minutes = 2f;

        public MinuteStats foodCosts_import = new MinuteStats();
        public MinuteStats foodCosts_blackmarket = new MinuteStats();

        public int goldCarryCapacity = 0;
        
        public Army(Faction faction, IntVector2 startPosition)
        {
            id = ++DssRef.state.NextArmyId;
            name.name = Data.NameGenerator.ArmyName(id);
            position = WP.ToMapPos(startPosition);
            tilePos = startPosition;
            cullingTopLeft = tilePos.Vec;
            cullingBottomRight = cullingTopLeft;
            nextNodePos = tilePos;
            setMaxFood();

            init(faction);
            postInit(faction);
        }

        public Army()
        {
            id = ++DssRef.state.NextArmyId;
        }

        public void init(Faction faction, int overrideIx = -1)
        {
#if DEBUG
            if (faction == null || faction.isDeleted)
            {
                throw new Exception();
            }
#endif
            faction.AddArmy(this, overrideIx);
            bound = new BoundingSphere(Vector3.Zero, 0.5f);
        }
        
        void postInit(Faction faction)
        {
            asynchCullingUpdate(1f, DssRef.state.culling.cullingStateA);   
        }

        override public bool lowFood()
        {
            return food + conservedFood <= 10;
        }

        public static void NetFullArmyStatus(Army army, Network.PacketReliability reliability )
        {
            var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssArmyStatus, reliability, out var packet);
            {
                Army.NetWriteArmy(w, army);
                army.lastNetUpdate.setNow();
            }
            packet.EndWrite_Asynch();

            int packetCount = 1;
            army.netWriteGroups(reliability, ref packetCount);
        }

        public static void NetWriteArmy(System.IO.BinaryWriter w, Army army)
        {
            Net.ObjectId.NetWriteMapObjId(w, army);
            
            army.writeNet(w);
        }

        public static void NetReadArmy(System.IO.BinaryReader r)
        {
            if (Net.ObjectId.NetReadMapObjId(r, out Faction faction, true, true, out AbsArmy mapObj, out bool needInit))
            {
                Army army = mapObj.GetArmy();
                army.readNet(r, needInit);

                if (needInit)
                {
                    army.postInit(faction);
                }

                army.net_onUpdate();
            }
        }        

        public void writeNet(System.IO.BinaryWriter w)
        {
            name.write(w);
            if (!name.custom)
            {
                w.Write((ushort)id);
            }

            WP.WritePosXZPercentU16(w, position);

            writeAiState(w);

            writeResources(w);
        }

        public void readNet(System.IO.BinaryReader r, bool needInit)
        {
            name.read(r, int.MaxValue);
            if (!name.custom)
            {
                int nameId = r.ReadUInt16();
                if (name.name == null)
                {
                    name.name = Data.NameGenerator.ArmyName(nameId);
                }
            }

            WP.ReadPosXZPercentU16(r, out position, out tilePos);            
            position.Y = DssRef.world.tileGrid.Get(tilePos).GroundY_aboveWater();

            readAiState(r, int.MaxValue, null);

            readResources(r);
        }

        public void net_onUpdate()
        {
            lastNetUpdate.setNow();
            if (!inRender_overviewLayer)
            {
                inRender_overviewLayer = true;
                setInRenderState();
            }
        }

        

        void writeResources(System.IO.BinaryWriter w)
        {
            w.Write(food);
            w.Write(conservedFood);
            money.write(w);
        }

        public void readResources(System.IO.BinaryReader r)
        {
            food = r.ReadSingle();
            conservedFood = r.ReadSingle();

            money.read(r);
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(id);
            name.write(w);
            WP.WritePosXZPercentU16(w, position);

            writeSoldierGroups(w);

            writeAiState(w);

            writeResources(w);
            Tag.write(w);
            
            w.Write((byte)armyColumnWidth);

            Debug.WriteCheck(w);
        }

        


        public void readGameState(Faction faction, System.IO.BinaryReader r, int subVersion, ObjectPointerCollection pointers)
        {
            this.factionIndex = faction.myIndex;

            if (subVersion < 105)
            {
                id = r.ReadUInt16();
            }
            else
            {
                id = r.ReadInt32();
            }
            name.read(r, subVersion);
            if (!name.custom)
            {
                name.name = Data.NameGenerator.ArmyName(id);
            }
                        
            WP.ReadPosXZPercentU16(r, out position, out tilePos);
            
            readSoldierGroups(r, subVersion, pointers);

            init(faction);
            postInit(faction);
            refreshPositions(true);
            position.Y = DssRef.world.tileGrid.Get(tilePos).GroundY_aboveWater();

            readAiState(r, subVersion, pointers);

            readResources(r);

            Tag.read(r, subVersion);
            //tagBack = (CityTagBack)r.ReadByte();

            //if (tagBack != CityTagBack.NONE)
            //{
            //    tagArt = (TagArt)r.ReadUInt16();
            //}

            armyColumnWidth = r.ReadByte();
            

            Debug.ReadCheck(r);
            
        }


        //override public void tagSprites(out SpriteName back, out SpriteName art)
        //{
        //    back = Data.TagLib.BackSprite(tagBack);
        //    art = Data.TagLib.ArtSprite(tagArt);
        //}

        public override string TypeName()
        {
            return DssRef.lang.UnitType_Army + " (" + myIndex.ToString() + ")";
        }

        public override void TypeIcon(RichBoxContent content)
        {
            content.Add(new RbImage(SpriteName.WarsArmy));
            content.hspace();
            tagToHud(content);
            content.hspace();
        }

        public override string Name(out bool mayEdit)
        {
            var faction = GetFaction();
            mayEdit = faction != null && faction.player.IsLocalPlayer();
            return name.name;
        }

        public override void NameEditEvent(string result, object tag)
        {
            name.setCustom(result);
        }

        void ArmyPresentationHud(ObjectHudArgs args, bool tooltip)
        {
            nameToHud(args.content, !tooltip);

            args.content.Add(new RbBeginTitle(tooltip ? 2 : 1));
            if (!tagToHud(args.content))
            {
                var faction = GetFaction();
                if (faction != null)
                {
                    args.content.Add(faction.FlagTextureToHud());
                }
            }
            args.content.space(0.5f);
            args.content.Add(new RbImage(SpriteName.WarsArmy));
            args.content.space(0.5f);
            args.content.Add(new RbText(DssRef.lang.UnitType_Army, tooltip ? HudLib.TitleColor_TypeName : HudLib.TitleColor_Head));

            args.content.space(1);

            IndexToHud(args.content);

            args.content.newLine();
            ownerToHud(args, !tooltip); 
        }

        public void groupTooltip(RichBoxContent content, object tag)
        {
            toTooltip(new ObjectHudArgs() { content = content });
        }

        public override void toTooltip(ObjectHudArgs args)
        {
            ArmyPresentationHud(args, true);

            //if (food < foodUpkeep * 2)
            if (!GetCasual())
            {
                HudLib.ItemCount(args.content, SpriteName.WarsResource_Food, DssRef.lang.Resource_TypeName_Food, TextLib.OneDecimal(food));
                HudLib.ItemCount(args.content, SpriteName.WarsResource_ConservedFood, DssRef.lang.Resource_TypeName_ConservedFood, TextLib.OneDecimal(conservedFood));
            }

            args.content.newLine();
            args.content.Add(new RbImage(SpriteName.WarsStrengthIcon));
            args.content.hspace();
            args.content.Add(new RbText(TextLib.OneDecimal(strengthValue)));

            args.content.space(2);
            args.content.Add(new RbImage(SpriteName.WarsMobilityIcon));
            args.content.hspace();
            args.content.Add(new RbText(TextLib.OneDecimal(mobilityValue)));

            args.content.newLine();
            args.content.Add(new RbImage(SpriteName.WarsGroupIcon));
            args.content.space(1);


            var typeCounts = Status().getTypeCounts_Sorted(GetFaction());

            foreach (var kv in typeCounts)
            {
                args.content.Add(new RbText(kv.Value.ToString()));
                args.content.Add(new RbImage(AllUnits.UnitFilterIcon(kv.Key)));
                args.content.space(2);
            }

        }
        public override void toHud(ObjectHudArgs args)
        {
            debugTagButton(args.content);

            ArmyPresentationHud(args, false);

            if (factionIndex == args.player.faction.myIndex)
                    {
                        new Interface.ArmyMenu(args.player, this, args.content);
                    }
                    else
                    {
                        basicInfoHud(args);
                    }
        }

        public void basicInfoHud(ObjectHudArgs args)
        {
            HudLib.LabelAndText(args.content, SpriteName.WarsSoldierGroup, DssRef.lang.Hud_SoldierGroupsCount, groups.Count.ToString());
            HudLib.LabelAndText(args.content, SpriteName.WarsSoldierMan, DssRef.lang.Hud_SoldierCount, TextLib.LargeNumber(soldiersCount));
            HudLib.LabelAndText(args.content, SpriteName.WarsStrengthIcon, DssRef.lang.Hud_StrengthRating, TextLib.OneDecimal(strengthValue));
            HudLib.LabelAndText(args.content, SpriteName.WarsMobilityIcon, DssRef.lang.Conscript_Mobility, TextLib.OneDecimal(mobilityValue));
            args.content.newLine();

            if (DssRef.state.PlayType() == GameState.PlayStateType.Play)
            {
                foodAndUpkeepToHud(args, true);
            }
            
            if (PlatformSettings.DevBuild)
            {
                args.content.text("Unique Id: " + id.ToString());
            }
        }

        void foodAndUpkeepToHud(ObjectHudArgs args, bool mayInteract)
        {
            args.content.newLine();
            args.content.Add(new RbImage(SpriteName.rtsUpkeepTime));
            args.content.space();
            args.content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCount, DssRef.lang.Hud_Upkeep, TextLib.TwoDecimal(totalUpkeep.copper * Money.CopperToGold))));
            args.content.space();
            HudLib.PerSecondInfo(args.player, args.content, false);

            if (!GetPlayer().profile.casualControls)
            {
                args.content.newLine();
                args.content.Add(new RbImage(SpriteName.WarsResource_FoodSub));
                args.content.space();
                args.content.Add(new RbText(string.Format(DssRef.lang.ArmyHud_Food_Upkeep_X, TextLib.TwoDecimal(totalUpkeep.food))));
                args.content.space();
                HudLib.PerSecondInfo(args.player, args.content, false);

                args.content.newLine();
                args.content.Add(new RbImage(SpriteName.WarsResource_Food));
                args.content.space();
                args.content.Add(new RbText(string.Format(DssRef.lang.ArmyHud_Food_Reserves_X, TextLib.LargeNumber((int)food))));

                getFoodGoalBuffer(out float bufferGoalFood, out float bufferGoalConservedFood);
                args.content.Add(new RbText(" / "+ TextLib.LargeNumber((int)bufferGoalFood), HudLib.SecondaryTextColor));

                if (mayInteract)
                {
                    args.content.space();
                    HudLib.InfoButton(args.content, new RbTooltip((RichBoxContent content, object tag)=> {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.Info_ArmyFood1));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.Info_ArmyFood2));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.Info_ArmyFood3));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.Info_ArmyFood4));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.Info_ArmyFood5));


                    }));
                }

                args.content.newLine();
                args.content.Add(new RbImage(SpriteName.WarsResource_ConservedFood));
                args.content.space();
                args.content.Add(new RbText(DssRef.lang.Resource_ConservedFood_Reserves +": " + TextLib.LargeNumber((int)conservedFood)));
                args.content.Add(new RbText(" / " + TextLib.LargeNumber((int)bufferGoalConservedFood), HudLib.SecondaryTextColor));

                args.content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.ArmyHud_Food_Costs_X, TextLib.TwoDecimal(foodCosts_import.displayValue_gold_sec + foodCosts_blackmarket.displayValue_gold_sec)));
                args.content.space();
                HudLib.PerSecondInfo(args.player, args.content, true);
            }
        }

        public void toGroupHud(RichBoxContent content)
        {
            RichBoxContent buttonContent = new RichBoxContent();

            buttonContent.Add(GetFaction().FlagTextureToHud());
            buttonContent.space(0.5f);
            buttonContent.Add(new RbText(DssRef.lang.UnitType_Army, HudLib.TitleColor_TypeName));

            buttonContent.space(0.5f);
            buttonContent.Add(new RbText(string.Format(DssRef.lang.UnitId, myIndex), HudLib.SecondaryTextColor));


            buttonContent.Add(new RbImage(SpriteName.WarsStrengthIcon));
            buttonContent.Add(new RbText(TextLib.OneDecimal(strengthValue)));

            content.Add(new ArtButton(RbButtonStyle.HoverArea, buttonContent,
                null, new RbTooltip(groupTooltip)));
        }

        public ArmyStatus Status()
        {
            ArmyStatus status = new ArmyStatus();
            var groupsCounter = groups.counter();

            while (groupsCounter.Next())
            {
                ++status.typeCount[(int)groupsCounter.sel.soldierConscript.filterType()];
            }

            return status;
        }

        public bool HasSettler(out SoldierGroup settlerUnit)
        {
            settlerUnit = null;
            var groupsCounter = groups.counter();

            while (groupsCounter.Next())
            {
                if (groupsCounter.sel.soldierConscript.conscript.weapon == ItemResourceType.Settler)
                {
                    settlerUnit = groupsCounter.sel;
                    return true;
                }
            }

            return false;
        }

        public void mergeArmies(AbsArmy otherArmy)
        {
            //This army will be removed

            if (otherArmy != null && otherArmy != this)
            {
                var status = Status().getTypeCounts(GetFaction());
                foreach (var kv in status)
                {
                    tradeSoldiersAction(ref otherArmy, kv.Key, kv.Value);
                }
            }
        }

        public void tradeSoldiersAction(ref AbsArmy toArmy, UnitFilterType type, int count)
        {
            if (
                toArmy != null &&
                (WP.birdDistance(this, toArmy) > (MaxTradeDistance + 1) || toArmy.isDeleted)
                )
            {
                //Army is no longer available
                toArmy = null;
            }

            if (toArmy == null)
            {
                IntVector2 onTile = DssRef.world.GetFreeTile(tilePos);
                toArmy = GetFaction().NewArmy(onTile);
            }

            tradeSoldiersTo(type, count, toArmy);
        }

        public void tradeSoldiersTo(UnitFilterType type, int count, AbsArmy toArmy)
        {
            float startGroupCount = groups.Count;
            var groupsCounter = groups.counter();

            while (groupsCounter.Next())
            {
                if (groupsCounter.sel.soldierConscript.filterType() == type)
                {
                    groupsCounter.sel.army = new WeakReference<AbsArmy>(toArmy);
                    toArmy.AddSoldierGroup(groupsCounter.sel);
                    groupsCounter.RemoveAtCurrent();

                    if (--count <= 0)
                    {
                        break;
                    }
                }
            }

            Money transportGold;

            if (groups.Count <= 0)
            {
                transportGold = money;
                DeleteMe(DeleteReason.EmptyGroup, true);
            }
            else
            {
                float percMove = (startGroupCount - groups.Count) / startGroupCount;
                transportGold = new Money(money.copper * percMove);
                refreshPositions(false);
            }

            money -= transportGold;

            var army = toArmy as Army;
            army.money += transportGold;
            army.refreshPositions(false);
            army.onArmyMerge();
        }
        public void disbandArmyAction()
        {
            DeleteMe(DeleteReason.Disband, true);
        }

        public void disbandSoldiersAction(UnitFilterType type, int count)
        {
            var groupsCounter = groups.counter();
            while (groupsCounter.Next())
            {
                if (groupsCounter.sel.soldierConscript.filterType() == type)
                {
                    groupsCounter.sel.DeleteMe(DeleteReason.Disband, false);
                    groupsCounter.RemoveAtCurrent();

                    if (--count <= 0)
                    {
                        break;
                    }
                }
            }

            if (groups.Count <= 0)
            {
                this.DeleteMe(DeleteReason.EmptyGroup, true);
            }
            else
            {
                refreshPositions(false);
            }
        }

        public int desertSoldiers()
        {
            int count = Bound.Min( MathExt.MultiplyInt(Ref.peRnd.Double(0.2, 0.4), groups.Count), 2);
            int soldiersDeserted = 0;

            for (int i = 0; i < count; i++)
            {
                var group = groups.PullRandom_Safe(Ref.peRnd);
                if (group != null)
                {
                    soldiersDeserted += group.soldierCount;
                    group.DeleteMe(DeleteReason.Desert, false);                    
                }
            }

            if (groups.Count <= 0)
            {
                DeleteMe(DeleteReason.EmptyGroup, true);
            }
            else
            {
                refreshPositions(false);
            }

            return soldiersDeserted;
        }


        

        public override void remove(SoldierGroup group)
        {
            base.remove(group);
            refreshPositions(false);
        }

        public void OnSoldierPurchaseCompleted()
        {
            refreshPositions(true);  
            
        }

        public override void selectionGui(Players.LocalPlayer player, ImageGroup guiModels)
        {
            if (player.faction.myIndex == factionIndex)
            {
                hoverAndSelectInfo(player, guiModels);
            }
        }

        override public bool rayCollision(Ray ray)
        {
            float? distance = ray.Intersects(bound);
            return distance.HasValue;
        }

        public override bool rectangleCollision(ScreenToSpaceRectangleBound rectangle)
        {
            return rectangle.Intersects(bound);
        }

        public override void selectionFrame(LocalPlayer player, bool hover, Selection selection)
        {
            selectionFramePlacement(out var pos, out var scale);

            selection.groupModels_terrian.OneFrameModel(pos, scale, hover, false);
        }

        public void selectionFramePlacement(out Vector3 pos, out Vector3 scale)
        {
            pos = position;
            pos.Y += 0.05f;
            scale = new Vector3(0.6f);
        }

        float emptyDeleteDelay = 3000;

        virtual public void update()
        {
            if (debugTagged || id == -1)
            {
                lib.DoNothing();
            }

            updateArmyMovement(Ref.DeltaGameTimeMs);

            updateDetailLevel();
            
            if (inRender_detailLayer)
            {
                updateArmyMembers(Ref.DeltaGameTimeMs, true);               
            }
            if (inRender_overviewLayer)
            {
                if (overviewBanner != null)
                {
                    updateModelsPosition();
                    overviewBanner.Frame = isShip ? 1 : 0;
                }
            }
            updateWorkerUnits();

            if (groups.Count == 0)
            {
                emptyDeleteDelay -= Ref.DeltaTimeMs;
                if (emptyDeleteDelay <= 0)
                {
                    DeleteMe(DeleteReason.EmptyGroup, true);
                }
            }
        }

        public void net_updateclient(bool playerDetailView)
        {
            //if (inRender_overviewLayer)
            //{
            //    updateModelsPosition();
            //    overviewBanner.Frame = isShip ? 1 : 0;

            //    if (lastNetUpdate.secPassed(30))
            //    {
            //        inRender_overviewLayer = false;
            //        setInRenderState();
            //    }
            //}
            updateArmyMovement(Ref.DeltaGameTimeMs);

            updateDetailLevel();
            if (inRender_detailLayer)
            {
                updateArmyMembers(Ref.DeltaGameTimeMs, true);
            }
            if (inRender_overviewLayer)
            {
                if (overviewBanner != null)
                {
                    updateModelsPosition();
                    overviewBanner.Frame = isShip ? 1 : 0;
                }
            }

            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.net_updateclient(playerDetailView);
            }
        }


        void updateArmyMovement(float time)
        {
            bool inPointMode = false; //for later opt, all groups are removed for perfromance

            if (!IdleObjetive())
            {
                if (inPointMode)
                {
                    Vector2 dir = Vector2.Zero;
                    dir.X = nextNodePos.X - position.X;
                    dir.Y = nextNodePos.Y - position.Z;

                    float l = dir.Length();

                    if (l > 0.04f)
                    {
                        var tile = DssRef.world.tileGrid.Get(tilePos);
                        float speed = tile.TerrainSpeedMultiplier(out bool isLand);
                        speed *= isLand ? transportSpeedLand : transportSpeedSea;

                        dir.Normalize();
                        //rotation.radians = lib.V2ToAngle(dir);
                        Vector2 move = speed * time * dir;
                        position.X += move.X;
                        position.Z += move.Y;

                        position.Y = tile.GroundY_aboveWater();

                        IntVector2 newtilepos = new IntVector2(position.X, position.Z);
                        if (tilePos != newtilepos)
                        {
                            tilePos = newtilepos;
                        }
                    }

                }
                else //Object mode
                {   
                    
                    Vector3 goalDiff = armyGoalCenterWp - position;
                    float l = VectorExt.PlaneXZLength(goalDiff);

                    if (l < 0.1f)
                    {
                        clearObjective();
                    }
                    else
                    {
                        rotation.radians = lib.V2ToAngle_normalized_unsafe(goalDiff.X / l, goalDiff.Z / l);
                    }
                } 
            }
        }

        void updateArmyMembers(float time, bool fullUpdate)
        {
            if (fullUpdate)
            {
                lib.DoNothing();
            }

            if (groups.Count > 0)
            {
                if (fullUpdate || !army_isIdle)
                {
                    Vector3 armyCenter = Vector3.Zero;
                    int armyCenterCount = 0;
                    var groupsC = groups.counter();

                    while (groupsC.Next())
                    {
                        groupsC.sel.update(time, fullUpdate);
                        Vector3 goalOffset = groupsC.sel.goalWp - armyGoalCenterWp;
                        armyCenter += groupsC.sel.position - goalOffset;
                        ++armyCenterCount;
                    }

                    if ((!IdleObjetive() || Ref.peRnd.ChanceF(0.05f)) && armyCenterCount > 0 && armyCenter.X > 1) 
                    {
                        var newPosition = armyCenter / armyCenterCount;

                        DssRef.world.unitBounds.KeepPointInsideBound_TilePositionXZref(ref newPosition);
                        position = newPosition;

                        tilePos = new IntVector2(position.X, position.Z);
                        var tile = DssRef.world.tileGrid.Get(tilePos);
                        position.Y = tile.GroundY_aboveWater();

                    }
                }

                aiUpdate(fullUpdate);
            }
        }

        virtual public void updateModelsPosition()
        {
            if (overviewBanner != null)
            {
                var tile = DssRef.world.tileGrid.Get(tilePos);
                overviewBanner.position = VectorExt.AddY(position, tile.GroundY_aboveWater());
                bound.Center = overviewBanner.position;
            }
        }

        public void refreshPositions(bool onPurchase)
        {
            refreshGroupPlacements2(tilePos, false, false, false);
        }

        public void startInOnePoint()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var groupsC = groups.counter();
                    while (groupsC.Next())
                    {
                        groupsC.sel.setArmyPlacement2(position, false, true, true);
                    }
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
                
            });
            
        }

       

        //static readonly int[] PlacementX = new int[] { 0, 1, -1, 2, -2, 3, -3, 4, -4, 5, -5 };

        /// <returns>Sequence of 0, 1, -1, 2, -2, 3, -3, 4, -4, 5, -5</returns>
        public static int TogglePlacementX(int index)
        {
            if (index == 0)
            {
                return 0;
            }

            int half = (index + 1) / 2;
            if (half * 2 > index)
            {
                return -half;
            }
            return half;
        }


        //public void refreshPositionsFor(ArmyPlacement armyPlacement, ref IntVector2 nextGroupPlacementIndex, int groupsWidth, bool onPurchase)
        //{
        //    var groupsC = groups.counter();

        //    while (groupsC.Next())
        //    {
        //        var soldier = groupsC.sel.FirstSoldier();
        //        if (soldier != null)
        //        {
        //            if (soldier.soldierData.ArmyFrontToBackPlacement == armyPlacement)
        //            {
        //                IntVector2 result = nextGroupPlacementIndex;
        //                result.X = TogglePlacementX(nextGroupPlacementIndex.X);// PlacementX[result.X];

        //                nextGroupPlacementIndex.Grindex_Next(groupsWidth);
        //                groupsC.sel.SetArmyPlacement(result, onPurchase); //behöver en wake up alert
                                                                          
        //            }
        //        }
        //    }
        //}

        public override void setInRenderState()
        {              
            if (inRender_overviewLayer)
            {
                if (overviewBanner == null && HasPlayer())
                {
                    overviewBanner = GetFaction_NoChecks().AutoLoadModelInstance(
                        OverviewBannerModelName, 1f);
                    overviewBanner.AddToRender(DrawGame.MidLayer);

                    updateModelsPosition();
                }
            }
            else
            {
                if (overviewBanner != null)
                {
                    overviewBanner.DeleteMe();
                    overviewBanner = null;
                }
            }

            setWorkersInRenderState();

            var groupsCounter = groups.counter();
            while (groupsCounter.Next())
            {
                groupsCounter.sel.setDetailLevel(inRender_detailLayer);
            }
        }

        protected void async_SoldiersUpdate(float time, bool oneMinute)
        {
            //if (debugTagged)
            //{
            //    lib.DoNothing();
            //}

            if (!HasAliveFaction())
            {
                lib.DoNothing();
            }

            if (groups.Count > 0)
            {
                int count = 0;
                int shipCount = 0;
                double speedbonus = 0;
                float totalStrength = 0;
                float totalMobility = 0;
                FindMinValue lowestMobility = new FindMinValue(false);

                //int dps;
                bool allGropsAreIdle = true;

                Vector2 minpos = VectorExt.V2Max;
                Vector2 maxpos = VectorExt.V2Min;

                if (DssRef.world.tileGrid.TryGet(tilePos, out Map.Tile tile))
                {
                    terrainSpeedMultiplier = tile.TerrainSpeedMultiplier(isShip);
                }

                var groupsC = groups.counter();

                while (groupsC.Next())
                {
                    count += groupsC.sel.soldierCount;
                    //groupsC.sel.setBattleWalkingSpeed();

                    allGropsAreIdle &= groupsC.sel.HasIdleState();
                    //int health;

                    if (groupsC.sel.isShip)
                    {
                        ++shipCount;
                        //dps = groupsC.sel.soldierData.DPS_sea();

                        
                        speedbonus += groupsC.sel.soldierConscript.conscript.armySpeedBonus(false);
                        groupsC.sel.walkSpeed_peace = transportSpeedSea;
                        
                        //TODO ship health
                        //health = groupsC.sel.soldierData.basehealth;
                    }
                    else
                    {
                        //dps = groupsC.sel.soldierData.DPS_land();
                        speedbonus += groupsC.sel.soldierConscript.conscript.armySpeedBonus(true);
                        groupsC.sel.walkSpeed_peace = transportSpeedLand;

                        //health = groupsC.sel.soldierData.basehealth;
                    }

                    if (groupsC.sel.position.X < minpos.X)
                    {
                        minpos.X = groupsC.sel.position.X;
                    }
                    if (groupsC.sel.position.X > maxpos.X)
                    {
                        maxpos.X = groupsC.sel.position.X;
                    }

                    if (groupsC.sel.position.Z < minpos.Y)
                    {
                        minpos.Y = groupsC.sel.position.Z;
                    }
                    if (groupsC.sel.position.Z > maxpos.Y)
                    {
                        maxpos.Y = groupsC.sel.position.Z;
                    }

                    totalStrength += groupsC.sel.strengthValue();/*AllUnits.GroupStrengh(groupsC.sel.soldierCount, ref groupsC.sel.soldierData, !groupsC.sel.isShip)*/;//(dps + health * AllUnits.HealthToStrengthConvertion) * groupsC.sel.soldierCount;
                    float mobility = groupsC.sel.mobilityValue();
                    totalMobility += mobility;
                    lowestMobility.Next(mobility);
                }

                army_isIdle = allGropsAreIdle && IdleObjetive();
                isShip = shipCount > groups.Count / 2;
                soldierRadius = MathExt.SquareRootF(count) / 20f;
                soldiersCount = count;

                //Endbart ändra när arme är i rörelse, måste följa center person
                //tilePos = WP.ToTilePos(position);
                speedbonus /= groups.Count;
                if (speedbonus < 0)
                {
                    speedbonus *= 0.5;
                }
                speedbonus += 1;
                transportSpeedLand = Convert.ToSingle(DssConst.Men_StandardWalkingSpeed * speedbonus);
                transportSpeedSea = Convert.ToSingle(DssConst.Men_StandardShipSpeed * speedbonus);

                strengthValue = totalStrength; // AllUnits.AverageGroupStrength;

                if (totalStrength > ArmySizeLeaderBoard.SizeUploaded)
                {
                    ArmySizeLeaderBoard.SizeUploaded = totalStrength;
                    Ref.update.AddSyncAction(new SyncAction(() =>
                    {
                        new ArmySizeLeaderBoard(ArmySizeLeaderBoard.SizeUploaded, soldiersCount);
                    }));
                }

                if (groups.Count < 2)
                {
                    mobilityValue = totalMobility;
                }
                else
                { 
                    mobilityValue = 0.2f * totalMobility / groups.Count + 0.8f * lowestMobility.minValue;
                }

                cullingTopLeft = minpos - CamCullingRadius;
                cullingBottomRight = maxpos + CamCullingRadius;
            }

        }

        public void asynchGameObjectsUpdate(float time, bool oneMinute)
        {
            async_SoldiersUpdate(time, oneMinute);

            if (IsNetHosted)
            {
                if (oneMinute)
                {
                    foodCosts_import.minuteUpdate();
                    foodCosts_blackmarket.minuteUpdate();
                }

                if (!DssRef.storage.gameRuleset.centralGold && time > 0)
                {
                    var onCity = DssRef.world.tileGrid.Get(tilePos).City();

                    if (onCity.factionIndex == factionIndex)
                    {
                        var faction = GetFaction();
                        if (faction != null)
                        {
                            if (money.GetGold() < goldCarryCapacity)
                            {
                                money.AddGold(onCity.money.payGold_MuchAsPossible(goldCarryCapacity - money.GetGold()));
                            }
                            else if (money.GetGold() > goldCarryCapacity)
                            {
                                faction.addGold(money.GetGold() - goldCarryCapacity, onCity);
                                money.SetGold(goldCarryCapacity);
                            }
                        }
                    }
                }
            }
        }

        override public void asynchCullingUpdate(float time, bool bStateA)
        {
            //if (this.debugTagged)
            //{
            //    lib.DoNothing();
            //}
            DssRef.state.culling.InRender_Asynch(ref enterRender_overviewLayer_async, ref enterRender_detailLayer_async, bStateA, ref cullingTopLeft, ref cullingBottomRight);

            if (DssRef.state.LocalHost().unitsPixelTexture != null)
            {
                foreach (var p in DssRef.state.localPlayers)
                {
                    p.unitsPixelTexture.asynch_AddArmy(this);
                }
            }
        }

        public void asynchSleepObjectsUpdate(float time)
        {
            if (!inRender_detailLayer)
            {
                if (IsNetHosted)
                {
                    if (objective == ArmyObjective.TeleportAttack)
                    {
                        //Wait to jump
                        if (DssRef.state.culling.outsidePlayerAttension(tilePos))
                        {
                            if (Ref.TotalGameTimeSec >= teleportTime)
                            {
                                Ai_Finalize_Attack();
                            }
                        }
                        else
                        {

                            //Cancel
                            Order_Attack(attackTarget);
                        }
                    }
                    else if (objective == ArmyObjective.TeleportMove)
                    {
                        //Wait to jump
                        if (DssRef.state.culling.outsidePlayerAttension(tilePos))
                        {
                            if (Ref.TotalGameTimeSec >= teleportTime)
                            {
                                Ai_Finalize_Move();
                            }
                        }
                        else
                        {
                            //Cancel
                            Order_MoveTo(walkGoal);
                        }
                    }
                    else
                    {
                        updateArmyMembers(time * Ref.GameTimeSpeed, false);
                    }
                }
                else
                {
                    //Net client update
                    updateArmyMembers(time * Ref.GameTimeSpeed, false);
                }
            }
        }


       

        public bool targetsFaction(AbsMapObject otherObj)
        {
            return attackTarget != null &&
                attackTarget.factionIndex == otherObj.factionIndex;
        }

        public override void DeleteMe(DeleteReason reason, bool removeFromParent)
        {
            if (isDeleted)
            {
                return;
            }

            if (IsNetHosted)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.DssDeleteArmy, Network.PacketReliability.Reliable);
                Net.ObjectId.NetWriteMapObjId(w, this);

            }

            isDeleted = true;
            Debug.CrashIfThreaded();

            if (reason == DeleteReason.EmptyGroup &&
                isShip &&
                GetFaction().factiontype == FactionType.SouthHara &&
                myIndex == 0)
            {
                DssRef.achieve.UnlockAchievement_onAny_100(AchievementIndex.early_hara_any, AchievementIndex.early_hara_100);
            }

            var counter = groups.counter();
            while (counter.Next())
            {
                counter.sel.DeleteMe(reason, false);
            }

            overviewBanner?.DeleteMe();

            if (removeFromParent)
            {
                GetFaction()?.remove(this);
            }

            if (workerUnits != null)
            {
                foreach (var m in workerUnits)
                {
                    m.DeleteMe();
                } 
            }
        }

        public void onNewModel(LootFest.VoxelModelName name, VoxelModel master)
        {
            if (overviewBanner != null)
            {
                DSSWars.Faction.SetNewMaster(name, OverviewBannerModelName, overviewBanner, master);
            }

            if (inRender_detailLayer)
            {
                var groupsC = groups.counter();
                while (groupsC.Next())
                {
                    groupsC.sel.onNewModel(name, master);
                }
            }
        }

        public void setWalkNode(IntVector2 nextNodeTilePos, bool finalNode,
            bool nextIsFootTransform, bool nextIsShipTransform)
        {
            Vector2 diff = WP.ToWorldPosXZ(nextNodeTilePos);
            diff.X -= position.X;
            diff.Y -= position.Z;

            rotation.radians = lib.V2ToAngle(diff);

            nextNodePos = nextNodeTilePos;

            refreshGroupPlacements2(nextNodeTilePos, false, false, false);
        }

        public override void setFaction(Faction newFaction, bool duringStartup, bool convert, ConvertReason convertReason, bool netShare)
        {
            base.setFaction(newFaction, duringStartup, false, convertReason, netShare);
            
            newFaction.AddArmy(this);
            
        }

        public override void OnNewOwner(Faction newFaction, bool convert, ConvertReason convertReason)
        {
            if (inRender_detailLayer)
            {
                inRender_detailLayer = false;
                setInRenderState();
                inRender_detailLayer = true;
                setInRenderState();
            }

            if (inRender_overviewLayer)
            {
                inRender_overviewLayer = false;
                setInRenderState();
                inRender_overviewLayer = true;
                setInRenderState();
            }
        }

        public override bool defeatedBy(int attackerFaction)
        {
            return isDeleted;
        }

        public override bool aliveAndBelongTo(int faction)
        {
            return !isDeleted;
        }

        //public override void OnBattleJoin(BattleGroup group)
        //{
        //    base.OnBattleJoin(group);

        //    var groupsC = groups.counter();
        //    while (groupsC.Next())
        //    {   
        //        groupsC.sel.battleQueTime = 0;
        //        groupsC.sel.prevBattleGridPos = IntVector2.MinValue;
        //    }
        //}

        //public override void ExitBattleGroup()
        //{
        //    base.ExitBattleGroup();

        //    refreshPositions(false);
        //    Ai_EnterPeaceEvent();

        //    bool refreshArmyPos = IdleObjetive();

        //    var groupsC = groups.counter();
        //    while (groupsC.Next())
        //    {   
        //        if (refreshArmyPos)
        //        {
        //            groupsC.sel.bumpWalkToNode(tilePos);
        //        }

        //        groupsC.sel.EnterPeaceEvent();
        //    }
        //}
        
        //public Vector3 leadingPosition()
        //{
        //    var leader = groups.First();
        //    if (leader != null)
        //    {
        //        return leader.position;
        //    }
        //    else
        //    {
        //        return WP.ToWorldPos(tilePos);
        //    }
        //}

        public void hungerDeserters()
        {
            //Gain a portion of deserters on all armies
            int totalDeserters = desertSoldiers();

            if (totalDeserters > 0)
            {
                var faction = GetFaction();

                if (faction != null && faction.player.IsLocalPlayer())
                {
                    var player = faction.player.GetLocalPlayer();
                    if (player.hud.messages.freeSpace())
                    {
                        player.hud.messages.Add(DssRef.lang.EventMessage_DesertersTitle, player.profile.casualControls? 
                            DssRef.lang.EventMessage_DesertersText_Money : DssRef.lang.EventMessage_DesertersText_Food);
                        player.statistics.SoldiersDeserted += totalDeserters;
                    }
                }
            }
        }

        override public Army GetAbsArmy() { return this; }
        override public Army GetArmy() { return this; }

        public override GameObjectType gameobjectType()
        {
            return GameObject.GameObjectType.Army;
        }


        public override string ToString()
        {
            return DssRef.lang.UnitType_Army + myIndex.ToString() + ", " + GetFaction().ToString();
        }

        public bool Is(int index, int faction)
        {
            return this.myIndex == index && factionIndex == faction;
        }

        public override bool CanMenuFocus()
        {
            return true;
        }


        public override bool IsArmy()
        {
            return true;
        }
        public override bool IsCity()
        {
            return false;
        }
    }
    enum ArmyPlacement
    { 
        Front,
        Mid,
        Back,        
    }
}
