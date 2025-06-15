using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Xsl;
using Valve.Steamworks;
using VikingEngine.DebugExtensions;

//using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Players;
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
        public float foodUpkeep = 0;

        public float foodBuffer_minutes = 2f;
        public float friendlyAreaFoodBuffer_minutes = 5f;

        public MinuteStats foodCosts_import = new MinuteStats();
        public MinuteStats foodCosts_blackmarket = new MinuteStats();

        public CityTagBack tagBack = CityTagBack.NONE;
        public ArmyTagArt tagArt = ArmyTagArt.None;

        public int goldCarryCapacity = 0;
        public int gold = 0;

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
        }

        public Army()
        { }

        public bool payMoney(int cost)
        {
            if (DssRef.storage.centralGold)
            {
                return faction.payGold(cost, false, null);
            }
            else
            {
                gold -= cost;
                return true;
            }
        }

        void init(Faction faction, int overrideIx = -1)
        {
            bound = new BoundingSphere(Vector3.Zero, 0.5f);
            asynchCullingUpdate(1f, DssRef.state.culling.cullingStateA);
            faction.AddArmy(this, overrideIx);
        }

        public static void NetWriteArmy(System.IO.BinaryWriter w, Army army)
        {
            w.Write((ushort)army.faction.parentArrayIndex);
            w.Write((ushort)army.parentArrayIndex);

            army.writeNet(w);
        }
        public static void NetReadArmy(System.IO.BinaryReader r)
        {
            int factionIx = r.ReadUInt16();
            var faction = DssRef.world.factions.Array[factionIx];
            
            int armyIx = r.ReadUInt16();
            Army army = faction.armies.GetIndex_Safe(armyIx);
            bool needInit = false;
            if (army == null)
            { 
                army = new Army();
                army.faction = faction;
                faction.armies.HardSet(army, armyIx);
                needInit = true;
            }

            army.readNet(r, needInit);

            if (needInit)
            {
                army.init(faction, armyIx);
            }

            army.net_onUpdate();
        }

        public static void NetWriteGroup(System.IO.BinaryWriter w, SoldierGroup group)
        {
            w.Write((ushort)group.parentArrayIndex);
            group.writeNet(w);
        }

        public static bool NetReadGroup(System.IO.BinaryReader r, Army army)
        {
            int index = r.ReadUInt16();
            if (index != ushort.MaxValue)
            {
                var group = army.groups.GetIndex_Safe(index);
                bool needInit = false;
                if (group == null)
                {
                    needInit = true;
                    if (army.IsCity())
                    {
                        group = new GuardGroup(army);
                    }
                    else
                    {
                        group = new SoldierGroup(army);
                    }
                    army.groups.HardSet(group, index);
                }

                group.readNet(r, needInit);
                group.net_onUpdate();
                return true;
            }
            else
            { 
                return false;
            }
        }


        public void writeNet(System.IO.BinaryWriter w)
        {
            WP.WritePosXZPercentU16(w, position);
            //WP.writePosXZ(w, position);
            //net_writeGroups(w);
        }
        public void readNet(System.IO.BinaryReader r, bool needInit)
        {
            WP.ReadPosXZPercentU16(r, out position, out tilePos);
            //WP.readPosXZ(r, out position, out tilePos);
            position.Y = DssRef.world.tileGrid.Get(tilePos).GroundY_aboveWater();   
            
            //net_readGroups(r);
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

        public void net_updateclient(bool playerDetailView)
        {
            if (inRender_overviewLayer)
            {
                updateModelsPosition();
                overviewBanner.Frame = isShip ? 1 : 0;

                if (lastNetUpdate.secPassed(30))
                {
                    inRender_overviewLayer = false;
                    setInRenderState();
                }
                
            }

            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.net_updateclient(playerDetailView);
            }
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(Debug.Ushort_OrCrash(id));
            name.write(w);
            WP.WritePosXZPercentU16(w, position);

            writeGroups(w);

            writeAiState(w);

            w.Write(food);

            w.Write((byte)tagBack);
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
            if (tagBack != CityTagBack.NONE)
            {
                w.Write((ushort)tagArt);
            }

            Debug.WriteCheck(w);
        }

        
        public void readGameState(Faction faction, System.IO.BinaryReader r, int subVersion, ObjectPointerCollection pointers)
        {
            this.faction = faction;

            //if (faction.player.IsLocalPlayer())
            //{
            //    lib.DoNothing();
            //}

            id = r.ReadUInt16();
            name.read(r, subVersion);
            if (!name.custom)
            {
                name.name = Data.NameGenerator.ArmyName(id);
            }

            if (subVersion < 62)
            {
                WP.readPosXZ_old(r, out position, out tilePos);
            }
            else
            {
                WP.ReadPosXZPercentU16(r, out position, out tilePos);
            }

            readGroups(r, subVersion, pointers);

            init(faction);
            refreshPositions(true);
            position.Y = DssRef.world.tileGrid.Get(tilePos).GroundY_aboveWater();

            readAiState(r, subVersion, pointers);

            food = r.ReadSingle();
            
            tagBack = (CityTagBack)r.ReadByte();
            if (tagBack != CityTagBack.NONE)
            {
                tagArt = (ArmyTagArt)r.ReadUInt16();
            }

            if (subVersion >= 62)
            { 
                Debug.ReadCheck(r);
            }
        }


        override public void tagSprites(out SpriteName back, out SpriteName art)
        {
            back = Data.CityTag.BackSprite(tagBack);
            art = Data.CityTag.ArtSprite(tagArt);
        }

        public override string TypeName()
        {
            return DssRef.lang.UnitType_Army + " (" + parentArrayIndex.ToString() + ")";
        }

        public override void TypeIcon(RichBoxContent content)
        {
            content.Add(new RbImage(SpriteName.WarsArmy));
            tagToHud(content);
        }

        public override string Name(out bool mayEdit)
        {
            mayEdit = faction.player.IsLocalPlayer();
            return name.name;
        }

        protected override void NameEditEvent(string result, object tag)
        {
            name.setCustom(result);
        }


        void ArmyPresentationHud(ObjectHudArgs args, bool tooltip)
        {
            nameToHud(args.content, !tooltip);

            args.content.Add(new RbBeginTitle(tooltip ? 2 : 1));
            if (!tagToHud(args.content))
            {
                args.content.Add(GetFaction().FlagTextureToHud());
            }
            args.content.space(0.5f);
            args.content.Add(new RbImage(SpriteName.WarsArmy));
            args.content.space(0.5f);
            args.content.Add(new RbText(DssRef.lang.UnitType_Army, tooltip ? HudLib.TitleColor_TypeName : HudLib.TitleColor_Head));

            args.content.space(1);
            args.content.Add(new RbText(string.Format(DssRef.lang.UnitId, parentArrayIndex), HudLib.SecondaryTextColor));

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
            {
                HudLib.ItemCount(args.content, SpriteName.WarsResource_Food, DssRef.lang.Resource_TypeName_Food, TextLib.OneDecimal(food));
            }

            args.content.newLine();
            args.content.Add(new RbImage(SpriteName.WarsStrengthIcon));
            args.content.Add(new RbText(TextLib.OneDecimal(strengthValue)));

            args.content.newLine();
            args.content.Add(new RbImage(SpriteName.WarsGroupIcon));
            args.content.space(1);


            var typeCounts = Status().getTypeCounts_Sorted(faction);

            foreach (var kv in typeCounts)
            {
                args.content.Add(new RbText(kv.Value.ToString()));
                args.content.Add(new RbImage(AllUnits.UnitFilterIcon(kv.Key)));
                args.content.space(2);
            }

        }
        public override void toHud(ObjectHudArgs args)
        {
            //base.toHud(args);
            ArmyPresentationHud(args, false);

            //if (args.player.hud.detailLevel == Display.HudDetailLevel.Minimal)
            //{
            //    //if (args.gui.menuState.Count == 0)
            //    //{
            //    args.content.Add(new RbImage(SpriteName.WarsGroupIcon));
            //    args.content.Add(new RbText(groups.Count.ToString()));
            //    args.content.space();
            //    args.content.Add(new RbImage(SpriteName.WarsStrengthIcon));
            //    args.content.Add(new RbText(TextLib.OneDecimal(strengthValue)));
            //    args.content.space();
            //    args.content.Add(new RbImage(SpriteName.rtsUpkeepTime));
            //    //args.content.Add(new RichBoxText(TextLib.LargeNumber(upkeep)));
            //    //}
            //}
            //else
            //{
                    if (faction == args.player.faction)
                    {
                        new Display.ArmyMenu(args.player, this, args.content);
                    }
                    else
                    {
                        basicInfoHud(args);
                    }
            //}
        }

        public void basicInfoHud(ObjectHudArgs args)
        {
            //int count = 0;

            //var groupsCounter = groups.counter();
            //while (groupsCounter.Next())
            //{
            //    count += groupsCounter.sel.soldiers.Count;
            //}

            //HudLib.ItemCount(args.content, SpriteName.WarsGroupIcon, DssRef.lang.Hud_SoldierGroupsCount, groups.Count.ToString());
            args.content.icontext(SpriteName.WarsGroupIcon, string.Format(DssRef.lang.Hud_SoldierGroupsCount, groups.Count));
            args.content.icontext(SpriteName.WarsSoldierIcon, string.Format(DssRef.lang.Hud_SoldierCount, TextLib.LargeNumber(soldiersCount)));
            args.content.icontext(SpriteName.WarsStrengthIcon, string.Format(DssRef.lang.Hud_StrengthRating, TextLib.OneDecimal(strengthValue)));
            //args.content.icontext(SpriteName.rtsUpkeepTime,string.Format(DssRef.lang.Hud_Upkeep ,TextLib.LargeNumber(upkeep)));
            args.content.newLine();

            if (DssRef.state.PlayType() == GameState.PlayStateType.Play)
            {
                foodToHud(args, true);
                //args.content.Add(new RbImage(SpriteName.WarsResource_Food));
                //args.content.space();
                //args.content.Add(new RbText(string.Format(DssRef.lang.ArmyHud_Food_Reserves_X, TextLib.LargeNumber((int)food))));

                //args.content.space();
                //HudLib.InfoButton(args.content, new RbTooltip_Text(DssRef.lang.Info_ArmyFood));

                //args.content.newLine();
                //args.content.Add(new RbImage(SpriteName.WarsResource_FoodSub));
                //args.content.space();
                //args.content.Add(new RbText(string.Format(DssRef.lang.ArmyHud_Food_Upkeep_X, TextLib.OneDecimal(foodUpkeep))));
                //args.content.space();
                //HudLib.PerSecondInfo(args.player, args.content, false);

                //args.content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.ArmyHud_Food_Costs_X, TextLib.OneDecimal(foodCosts_import.displayValue_gold_sec)));
                //args.content.space();
                //HudLib.PerSecondInfo(args.player, args.content, true);
            }
            
            
            //    () =>
            //{
            //    RichBoxContent content = new RichBoxContent();
            //    HudLib.Description(content, DssRef.lang.Info_ArmyFood);
            //    args.player.hud.tooltip.create(args.player, content, true);
            //}));
            

            if (PlatformSettings.DevBuild)
            {
                args.content.text("Id: " + id.ToString());
            }
        }

        void foodToHud(ObjectHudArgs args, bool mayInteract)
        {
            args.content.Add(new RbImage(SpriteName.WarsResource_Food));
            args.content.space();
            args.content.Add(new RbText(string.Format(DssRef.lang.ArmyHud_Food_Reserves_X, TextLib.LargeNumber((int)food))));

            if (mayInteract)
            {
                args.content.space();
                HudLib.InfoButton(args.content, new RbTooltip_Text(DssRef.lang.Info_ArmyFood));
            }

            args.content.newLine();
            args.content.Add(new RbImage(SpriteName.WarsResource_FoodSub));
            args.content.space();
            args.content.Add(new RbText(string.Format(DssRef.lang.ArmyHud_Food_Upkeep_X, TextLib.OneDecimal(foodUpkeep))));
            args.content.space();
            HudLib.PerSecondInfo(args.player, args.content, false);

            args.content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.ArmyHud_Food_Costs_X, TextLib.OneDecimal(foodCosts_import.displayValue_gold_sec)));
            args.content.space();
            HudLib.PerSecondInfo(args.player, args.content, true);
        }



        public void toGroupHud(RichBoxContent content)
        {
            //string name = Name(out _);

            //if (name != null)
            //{
            //    content.text(name).overrideColor = Color.LightYellow;
            //    content.newLine();
            //}

            //content.Add(new RbBeginTitle());

            RichBoxContent buttonContent = new RichBoxContent();

            buttonContent.Add(GetFaction().FlagTextureToHud());
            buttonContent.space(0.5f);
            buttonContent.Add(new RbText(DssRef.lang.UnitType_Army, HudLib.TitleColor_TypeName));

            buttonContent.space(0.5f);
            buttonContent.Add(new RbText(string.Format(DssRef.lang.UnitId, parentArrayIndex), HudLib.SecondaryTextColor));


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

        

        public void mergeArmies(Army otherArmy)
        {
            //This army will be removed

            if (otherArmy != null && otherArmy != this)
            {
                var status = Status().getTypeCounts(faction);
                foreach (var kv in status)
                {
                    tradeSoldiersAction(ref otherArmy, kv.Key, kv.Value);
                }
            }
        }

        public void tradeSoldiersAction(ref Army toArmy, UnitFilterType type, int count)
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
                toArmy = faction.NewArmy(onTile);
            }

            tradeSoldiersTo(type, count, toArmy);
        }

        public void tradeSoldiersTo(UnitFilterType type, int count, Army toArmy)
        {
            float startGroupCount = groups.Count;
            var groupsCounter = groups.counter();

            while (groupsCounter.Next())
            {
                if (groupsCounter.sel.soldierConscript.filterType() == type)
                {
                    groupsCounter.sel.army = toArmy;
                    toArmy.AddSoldierGroup(groupsCounter.sel);
                    //if (groupsCounter.sel.groupObjective == SoldierGroup.GroupObjective_FollowArmyObjective)
                    //{
                    //    groupsCounter.sel.groupObjective = SoldierGroup.GroupObjective_ReGrouping;
                    //}
                    groupsCounter.RemoveAtCurrent();

                    if (--count <= 0)
                    {
                        break;
                    }
                }
            }

            int transportGold;

            if (groups.Count <= 0)
            {
                transportGold = gold;
                DeleteMe(DeleteReason.EmptyGroup, true);
            }
            else
            {
                float percMove = (startGroupCount - groups.Count) / startGroupCount;
                transportGold = Convert.ToInt32(gold * percMove);
                refreshPositions(false);
            }

            gold -= transportGold;
            toArmy.gold += transportGold;
            toArmy.refreshPositions(false);
            toArmy.onArmyMerge();
        }

        public void disbandSoldiersAction(UnitFilterType type, int count)
        {
            var groupsCounter = groups.counter();
            while (groupsCounter.Next())
            {
                if (groupsCounter.sel.soldierConscript.filterType() == type)
                {
                    groupsCounter.sel.DeleteMe(DeleteReason.Disband, false);
                    //groupsCounter.sel.onDisband(false);
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
            int count = MathExt.MultiplyInt(Ref.peRnd.Double(0.2, 0.4), groups.Count);
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

            //if (faction.player.IsPlayer())
            //{
            //    faction.player.GetLocalPlayer().statistics.SoldiersDeserted += soldiersDeserted;
            //}

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


        public void disbandArmyAction()
        {
            DeleteMe( DeleteReason.Disband, true);
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
            if (player.faction == faction)
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

            //selection.frameModel.Position = pos;
            //selection.frameModel.Scale = scale;

            //selection.frameModel.LoadedMeshType = hover ? LoadedMesh.SelectCircleDotted : LoadedMesh.SelectCircleSolid;
            //frameModel.SetSpriteName(hover ? SpriteName.LittleUnitSelectionDotted : SpriteName.WhiteCirkle);
        }

        public void selectionFramePlacement(out Vector3 pos, out Vector3 scale)
        {
            pos = position;
            pos.Y += 0.05f;
            scale = new Vector3(0.6f);
        }

        virtual public void update()
        {
            updateArmyMovement(Ref.DeltaGameTimeMs);

            if (debugTagged || id == -1)
            {
                lib.DoNothing();
            }
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
                DeleteMe(DeleteReason.EmptyGroup, true);
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

                    if (!IdleObjetive() && armyCenterCount > 0)
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
            overviewBanner.position = VectorExt.AddY(position, 0.04f);
            bound.Center = overviewBanner.position;
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
                        groupsC.sel.setArmyPlacement2(position, false, true);
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

        protected override void setInRenderState()
        {              
            if (inRender_overviewLayer)
            {
                if (overviewBanner == null)
                {
                    overviewBanner = faction.AutoLoadModelInstance(
                        OverviewBannerModelName, 1f);
                    overviewBanner.AddToRender(DrawGame.TerrainLayer);

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
            if (groups.Count > 0)
            {
                int count = 0;
                int shipCount = 0;
                double speedbonus = 0;
                float totalStrength = 0;
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

                    allGropsAreIdle &= groupsC.sel.state == GroupState.Idle;
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

                    totalStrength += AllUnits.GroupStrengh(groupsC.sel.soldierCount, ref groupsC.sel.soldierData, !groupsC.sel.isShip);//(dps + health * AllUnits.HealthToStrengthConvertion) * groupsC.sel.soldierCount;

                }

                army_isIdle = allGropsAreIdle && IdleObjetive();
                isShip = shipCount > groups.Count / 2;
                soldierRadius = MathExt.SquareRootF(count) / 20f;
                //this.strengthValue = count;
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

                cullingTopLeft = minpos - CamCullingRadius;
                cullingBottomRight = maxpos + CamCullingRadius;
            }

        }

        public void asynchGameObjectsUpdate(float time, bool oneMinute)
        {
            if (debugTagged)
            {
                lib.DoNothing();
            }

            async_SoldiersUpdate(time, oneMinute);
            
            if (oneMinute)
            {
                foodCosts_import.minuteUpdate();
                foodCosts_blackmarket.minuteUpdate();
            }


            if (!DssRef.storage.centralGold)
            {
                var onCity = DssRef.world.tileGrid.Get(tilePos).City();

                if (onCity.faction == faction)
                {
                    if (gold < goldCarryCapacity)
                    {
                        gold += faction.payMoney_MuchAsPossible(goldCarryCapacity - gold, onCity);
                    }
                    else if (gold > goldCarryCapacity)
                    {
                        faction.addGold(gold - goldCarryCapacity, onCity);
                        gold = goldCarryCapacity;
                    }
                }
            }
        }

        override public void asynchCullingUpdate(float time, bool bStateA)
        {
            DssRef.state.culling.InRender_Asynch(ref enterRender_overviewLayer_async, ref enterRender_detailLayer_async, bStateA, ref cullingTopLeft, ref cullingBottomRight);
        }

        public void asynchSleepObjectsUpdate(float time)
        {
            if (!inRender_detailLayer)
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
        }


       

        public bool targetsFaction(AbsMapObject otherObj)
        {
            return attackTarget != null &&
                attackTarget.faction == otherObj.faction;
        }

        public override void DeleteMe(DeleteReason reason, bool removeFromParent)
        {
            isDeleted = true;
            Debug.CrashIfThreaded();

            if (reason == DeleteReason.EmptyGroup &&
                isShip && 
                faction.grouptype == FactionGroupType.Nordic)
            {
                //var battle = battles.First();
                //if (battle != null && battle.faction.player.IsPlayer())
                //{
                //    DssRef.achieve.UnlockAchievement(AchievementIndex.viking_naval);
                //}
            }

            var counter = groups.counter();
            while (counter.Next())
            {
                counter.sel.DeleteMe(reason, false);
            }

            overviewBanner?.DeleteMe();

            if (removeFromParent)
            {
                faction.remove(this);
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
            //if (battleGroup != null)
            //{
            //    return;
            //}

            //if (id == 786)
            //{ 
            //    lib.DoNothing();
            //}
            Vector2 diff = WP.ToWorldPosXZ(nextNodeTilePos);
            diff.X -= position.X;
            diff.Y -= position.Z;

            rotation.radians = lib.V2ToAngle(diff);

            nextNodePos = nextNodeTilePos;

            refreshGroupPlacements2(nextNodeTilePos, false, false, false);

            
            //var groupsC = groups.counter();
            //while (groupsC.Next())
            //{
            //    groupsC.sel.setWalkNode(area, nextIsFootTransform, nextIsShipTransform);                
            //}
        }

        public override void setFaction(Faction faction)
        {
            base.setFaction(faction);
            faction.AddArmy(this);
            
        }

        public override void OnNewOwner()
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

        public override bool defeatedBy(Faction attacker)
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
        
        public Vector3 leadingPosition()
        {
            var leader = groups.First();
            if (leader != null)
            {
                return leader.position;
            }
            else
            {
                return WP.ToWorldPos(tilePos);
            }
        }

        public void hungerDeserters()
        {
            //Gain a portion of deserters on all armies
            int totalDeserters = desertSoldiers();

            if (totalDeserters > 0 &&
                faction.player.IsLocalPlayer() && 
                faction.player.GetLocalPlayer().hud.messages.freeSpace())
            {
                faction.player.GetLocalPlayer().hud.messages.Add("Deserters!", "Hungry soldiers are deserting from your armies");
                faction.player.GetLocalPlayer().statistics.SoldiersDeserted += totalDeserters;
            }
        }

        override public Army GetArmy() { return this; }

        public override GameObjectType gameobjectType()
        {
            return GameObject.GameObjectType.Army;
        }


        public override string ToString()
        {
            return DssRef.lang.UnitType_Army + parentArrayIndex.ToString() + ", " + faction.ToString();
        }

        public bool Is(int index, int faction)
        {
            return this.parentArrayIndex == index && this.faction.parentArrayIndex == faction;
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
