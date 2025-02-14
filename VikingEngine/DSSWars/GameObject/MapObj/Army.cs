﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Xsl;
using Valve.Steamworks;
//using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.HeroQuest.Data;
using VikingEngine.ToGG.HeroQuest.Data.Condition;

namespace VikingEngine.DSSWars.GameObject
{
    partial class Army : AbsMapObject
    {
        public const float MaxTradeDistance = 3;

        const int GroupsWidth_Size1 = 2;
        const int GroupsWidth_Size2 = 4;
        const int GroupsWidth_Size3 = 6;
        const int GroupsWidth_Size4 = 8;

        static readonly int Size1Capacity = MathExt.Square(GroupsWidth_Size1 * 3);
        static readonly int Size2Capacity = MathExt.Square(GroupsWidth_Size2 * 3);
        static readonly int Size3Capacity = MathExt.Square(GroupsWidth_Size3 * 3);


        const LootFest.VoxelModelName OverviewBannerModelName = LootFest.VoxelModelName.armystand;

        //public ArmyAi ai;
        public SpottedArray<SoldierGroup> groups = new SpottedArray<SoldierGroup>(32);
        //public SpottedArrayCounter<SoldierGroup> groupsCounter;

        protected Graphics.AbsVoxelObj overviewBanner;

        public Rotation1D rotation = Rotation1D.D180.Add(Ref.rnd.Plus_MinusF(0.8f));
        public float soldierRadius = 0.5f;
        BoundingSphere bound;
        
        public int id;
       
        public int soldiersCount = 0;
        //public int upkeep;
        public float transportSpeedLand = DssConst.Men_StandardWalkingSpeed;
        public float transportSpeedSea = DssConst.Men_StandardShipSpeed;
        public bool isShip = false;

        public float terrainSpeedMultiplier = 1.0f;
        //public IntVector2 positionBeforeBattle;
        //string name;
        ObjectName name = new ObjectName();

        static readonly Vector2 CamCullingRadius = new Vector2(DssVar.SoldierGroup_Spacing * 1.4f);
        public Vector2 cullingTopLeft, cullingBottomRight;
        bool isIdle = true;

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
                return faction.payMoney(cost, false, null);
            }
            else
            {
                gold -= cost;
                return true;
            }
        }

        void init(Faction faction)
        {
            bound = new BoundingSphere(Vector3.Zero, 0.5f);
            asynchCullingUpdate(1f, DssRef.state.culling.cullingStateA);
            faction.AddArmy(this);
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(Debug.Ushort_OrCrash(id));
            name.write(w);
            WP.writePosXZ(w, position);

            w.Write((ushort)groups.Count);
            var groupsC = groups.counter();
            while (groupsC.Next())
            { 
                groupsC.sel.writeGameState(w);
            }

            writeAiState(w);

            w.Write(food);

            w.Write((byte)tagBack);
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
            if (tagBack != CityTagBack.NONE)
            {
                w.Write((ushort)tagArt);
            }
        }
        public void readGameState(Faction faction, System.IO.BinaryReader r, int subVersion, ObjectPointerCollection pointers)
        {
            this.faction = faction;

            id = r.ReadUInt16();
            name.read(r, subVersion);
            if (!name.custom)
            {
                name.name = Data.NameGenerator.ArmyName(id);
            }

            WP.readPosXZ(r, out position, out tilePos);

            int groupsCount = r.ReadUInt16();
            for (int i = 0; i < groupsCount; i++)
            {
                SoldierGroup group = new SoldierGroup(this, r, subVersion, pointers);               
            }

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
        }

        public void writeNet(System.IO.BinaryWriter w)
        {

        }
        public void readNet(System.IO.BinaryReader r)
        {

        }

        override public void tagSprites(out SpriteName back, out SpriteName art)
        {
            back = Data.CityTag.BackSprite(tagBack);
            art = Data.CityTag.ArtSprite(tagArt);
        }

        public override string TypeName()
        {
            return DssRef.lang.UnitType_Army + " (" + parentArrayIndex.ToString() +   ")";//return "Army" + parentArrayIndex.ToString();
        }

        public override void TypeIcon(RichBoxContent content)
        {
            content.Add(new RbImage(SpriteName.WarsUnitIcon_Soldier));
            tagToHud(content);
        }
        //public override SpriteName TypeIcon()
        //{
        //    return SpriteName.WarsUnitIcon_Soldier;
        //}

        public override string Name(out bool mayEdit)
        {
            mayEdit = faction.player.IsLocalPlayer();
            return name.name;
        }

        protected override void NameEditEvent(string result, object tag)
        {
            name.setCustom(result);
        }

        public override void toHud(ObjectHudArgs args)
        {
            base.toHud(args);

            if (args.player.hud.detailLevel == Display.HudDetailLevel.Minimal)
            {
                //if (args.gui.menuState.Count == 0)
                //{
                args.content.Add(new RbImage(SpriteName.WarsGroupIcon));
                args.content.Add(new RbText(groups.Count.ToString()));
                args.content.space();
                args.content.Add(new RbImage(SpriteName.WarsStrengthIcon));
                args.content.Add(new RbText(TextLib.OneDecimal(strengthValue)));
                args.content.space();
                args.content.Add(new RbImage(SpriteName.rtsUpkeepTime));
                //args.content.Add(new RichBoxText(TextLib.LargeNumber(upkeep)));
                //}
            }
            else
            {

                //if (args.gui.menuState.Count == 0)
                //{

                //}
                //var groupsCounter = groups.counter();
                //while (groupsCounter.Next())
                //{
                //    count += groupsCounter.sel.soldierCount;
                ////}
                //if (args.gui.menuState.Count == 0)
                //{
                //    //HudLib.ItemCount(args.content, SpriteName.WarsGroupIcon, DssRef.lang.Hud_SoldierGroupsCount, groups.Count.ToString());
                //    args.content.icontext(SpriteName.WarsGroupIcon, string.Format(DssRef.lang.Hud_SoldierGroupsCount, groups.Count));
                //    args.content.icontext(SpriteName.WarsSoldierIcon, string.Format(DssRef.lang.Hud_SoldierCount, TextLib.LargeNumber(soldiersCount)));
                //    args.content.icontext(SpriteName.WarsStrengthIcon, string.Format(DssRef.lang.Hud_StrengthRating, TextLib.OneDecimal(strengthValue)));
                //    //args.content.icontext(SpriteName.rtsUpkeepTime,string.Format(DssRef.lang.Hud_Upkeep ,TextLib.LargeNumber(upkeep)));
                //    args.content.text(string.Format(DssRef.lang.ArmyHud_Food_Reserves_X, TextLib.LargeNumber((int)food)));
                //    args.content.space();
                //    HudLib.InfoButton(args.content, new RbAction(() =>
                //    {
                //        RichBoxContent content = new RichBoxContent();
                //        HudLib.Description(content, DssRef.lang.Info_ArmyFood);
                //        args.player.hud.tooltip.create(args.player, content, true);
                //    }));
                //    args.content.text(string.Format(DssRef.lang.ArmyHud_Food_Upkeep_X, TextLib.OneDecimal(foodUpkeep)));
                //    args.content.space();
                //    HudLib.PerSecondInfo(args.player, args.content, false);

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
            args.content.Add(new RbImage(SpriteName.WarsResource_Food));
            args.content.space();
            args.content.Add(new RbText(string.Format(DssRef.lang.ArmyHud_Food_Reserves_X, TextLib.LargeNumber((int)food))));
            args.content.space();
            HudLib.InfoButton(args.content, new RbTooltip_Text(DssRef.lang.Info_ArmyFood));
            //    () =>
            //{
            //    RichBoxContent content = new RichBoxContent();
            //    HudLib.Description(content, DssRef.lang.Info_ArmyFood);
            //    args.player.hud.tooltip.create(args.player, content, true);
            //}));
            args.content.newLine();
            args.content.Add(new RbImage(SpriteName.WarsResource_FoodSub));
            args.content.space();
            args.content.Add(new RbText( string.Format(DssRef.lang.ArmyHud_Food_Upkeep_X, TextLib.OneDecimal(foodUpkeep))));
            args.content.space();
            HudLib.PerSecondInfo(args.player, args.content, false);

            args.content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.ArmyHud_Food_Costs_X, TextLib.OneDecimal(foodCosts_import.displayValue_sec)));
            args.content.space();
            HudLib.PerSecondInfo(args.player, args.content, true);

            if (PlatformSettings.DevBuild)
            {
                args.content.text("Id: " + id.ToString());
            }
        }



        public void toGroupHud(RichBoxContent content)
        {
            string name = Name(out _);

            if (name != null)
            {
                content.text(name).overrideColor = Color.LightYellow;
                content.newLine();
            }

            content.Add(new RbBeginTitle());
            content.Add(GetFaction().FlagTextureToHud());
            content.Add(new RbText(TypeName()));

            content.Add(new RbImage(SpriteName.WarsStrengthIcon));
            content.Add(new RbText(TextLib.OneDecimal(strengthValue)));
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

        public void AddSoldierGroup(SoldierGroup group) 
        {
            //Hitta en plats bland alla grupper
            //group.armyPosition = nextPlacement();
            group.parentArrayIndex = groups.Add(group);
            group.army = this;
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
            int count = MathExt.MultiplyInt(Ref.rnd.Double(0.2, 0.4), groups.Count);
            int soldiersDeserted = 0;

            for (int i = 0; i < count; i++)
            {
                var group = groups.PullRandom_Safe(Ref.rnd);
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
            //var groupsCounter = groups.counter();
            //while (groupsCounter.Next())
            //{   
            //    groupsCounter.sel.deleteGroup(true);
            //    groupsCounter.sel.onDisband(false);
            //}

            DeleteMe( DeleteReason.Disband, true);
        }

        public void remove(SoldierGroup group)
        {
            Debug.CrashIfThreaded();
            groups.RemoveAt_EqualSafeCheck(group, group.parentArrayIndex);
            //if (!InBattle())
            {
                refreshPositions(false);
            }
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

        public override void selectionFrame(bool hover, Selection selection)
        {
            selectionFramePlacement(out var pos, out var scale);

            selection.OneFrameModel(false, pos, scale, hover, false);

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
                updateMembers(Ref.DeltaGameTimeMs, true);               
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

        void updateMembers(float time, bool fullUpdate)
        {
            //if (id == 1)
            //{
            //    lib.DoNothing();
            //}
            if (groups.Count > 0)
            {
                if (fullUpdate || !isIdle)
                {
                    Vector3 armyCenter = Vector3.Zero;
                    int armyCenterCount = 0;
                    var groupsC = groups.counter();
                    //SoldierGroup centerGuy = null;
                    //SoldierGroup mostCenterGuy 

                    while (groupsC.Next())
                    {
                        groupsC.sel.update(time, fullUpdate);
                        Vector3 goalOffset = groupsC.sel.goalWp - armyGoalCenterWp;
                        armyCenter += groupsC.sel.position - goalOffset;
                        ++armyCenterCount;

                        //if (groupsC.sel.armyGridPlacement2.Y == 0)
                        //{                            
                        //    if (groupsC.sel.armyGridPlacement2.X == 0)
                        //    {
                        //        centerGuy = groupsC.sel;
                        //    }
                        //}
                    }
                    if (IdleObjetive())
                    {
                        //position.X = armyGoalCenterWp.X;
                        //position.Z = armyGoalCenterWp.Z;

                    }
                    else if (armyCenterCount > 0)
                    {
                        var newPosition = armyCenter / armyCenterCount;

                        DssRef.world.unitBounds.KeepPointInsideBound_TilePositionXZref(ref newPosition);
                        position = newPosition;

                        //if (newPosition.X > 1 && newPosition.Z > 1)
                        //{
                        //    position.X = newPosition.X;
                        //    position.Z = newPosition.Z;
                        //}

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
            refreshGroupPlacements2(tilePos, false, false);
        }

        public void startInOnePoint()
        {
            Task.Factory.StartNew(() =>
            {
                var groupsC = groups.counter();
                while (groupsC.Next())
                {
                    groupsC.sel.setArmyPlacement2(position, true);
                }
            });
            
        }

        public void autoColumnWidth()
        {
            int width;

            if (groups.Count > Size3Capacity)
            {
                width = GroupsWidth_Size4;
            }
            else if (groups.Count > Size2Capacity)
            {
                width = GroupsWidth_Size3;
            }
            else if (groups.Count > Size1Capacity)
            {
                width = GroupsWidth_Size2;
            }
            else
            {
                width = GroupsWidth_Size1;
            }

            armyColumnWidth = width;
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
                //var soldiers = groupsCounter.sel.soldiers.counter();
                //while (soldiers.Next())
                //{
                //    soldiers.sel.setDetailLevel(inRender_detailLayer);
                //}
            }
        }

        public void asynchGameObjectsUpdate(float time, bool oneMinute)
        {
            if (debugTagged)
            {
                lib.DoNothing();
            }


            if (groups.Count > 0)
            {
                int count = 0;
                int shipCount = 0;
                double speedbonus = 0;
                float totalStrength = 0;
                int dps;
                bool allGropsAreIdle = true;

                Vector2 minpos = VectorExt.V2Max;
                Vector2 maxpos = VectorExt.V2Min;

                if (DssRef.world.tileGrid.TryGet(tilePos, out Map.Tile tile))
                { 
                    terrainSpeedMultiplier = tile.TerrainSpeedMultiplier(isShip);
                }
                
                //var battleGroup_sp = battleGroup;
                //bool inBattle = battleGroup_sp != null && battleGroup_sp.battleState == Battle.BattleState.Battle;
                //bool notBattle = !inBattle;

                var groupsC = groups.counter();

                while (groupsC.Next())
                {
                   

                        count += groupsC.sel.soldierCount;
                        groupsC.sel.setBattleWalkingSpeed();

                        allGropsAreIdle &= groupsC.sel.state == GroupState.Idle; //.allInduvidualsAreIdle;
                        int health;

                        if (groupsC.sel.isShip)
                        {
                            ++shipCount;
                            dps = groupsC.sel.soldierData.DPS_sea();

                            //if (notBattle)                       
                            {
                                speedbonus += groupsC.sel.soldierConscript.conscript.armySpeedBonus(false);//unitProfile.ArmySpeedBonusSea;
                                groupsC.sel.walkSpeed = transportSpeedSea;
                            }

                            //var first = groupsC.sel.FirstSoldier();
                            //if (first != null)
                            //{
                            //TODO ship health
                            health = groupsC.sel.soldierData.basehealth;
                            //}
                        }
                        else
                        {
                            dps = groupsC.sel.soldierData.DPS_land();

                            //if (notBattle)
                            //{
                            speedbonus += groupsC.sel.soldierConscript.conscript.armySpeedBonus(true);//unitProfile.ArmySpeedBonusLand;
                            groupsC.sel.walkSpeed = transportSpeedLand;
                            //}

                            health = groupsC.sel.soldierData.basehealth;
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

                        totalStrength += (dps + health * AllUnits.HealthToStrengthConvertion) * groupsC.sel.soldierCount;
                    
                }
                
                isIdle = allGropsAreIdle && IdleObjetive();
                isShip = shipCount > groups.Count / 2;
                soldierRadius = MathExt.SquareRootF(count) / 20f;
                this.strengthValue = count;
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
                collectBattles_asynch();

                strengthValue = totalStrength / AllUnits.AverageGroupStrength;

                cullingTopLeft = minpos - CamCullingRadius;
                cullingBottomRight = maxpos + CamCullingRadius;
            }

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
                        faction.gainMoney(gold - goldCarryCapacity, onCity);
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
                    updateMembers(time * Ref.GameTimeSpeed, false);
                }
            }
        }


        public void asyncNearObjectsUpdate()
        {
            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.asynchNearObjectsUpdate();
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

            refreshGroupPlacements2(nextNodeTilePos, false, false);

            
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
    }
    enum ArmyPlacement
    { 
        Front,
        Mid,
        Back,        
    }
}
