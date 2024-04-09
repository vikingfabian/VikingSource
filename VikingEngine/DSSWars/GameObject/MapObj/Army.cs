using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.Xsl;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Data;
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

        const int GroupsWidth_Size1 = 5;
        const int GroupsWidth_Size2 = 7;
        const int GroupsWidth_Size3 = 9;
        const int GroupsWidth_Size4 = 11;

        const int Size1Capacity = GroupsWidth_Size1 * GroupsWidth_Size1;
        const int Size2Capacity = GroupsWidth_Size2 * GroupsWidth_Size2;
        const int Size3Capacity = GroupsWidth_Size3 * GroupsWidth_Size3;


        const LootFest.VoxelModelName OverviewBannerModelName = LootFest.VoxelModelName.armystand;

        //public ArmyAi ai;
        public SpottedArray<SoldierGroup> groups = new SpottedArray<SoldierGroup>(32);
        //public SpottedArrayCounter<SoldierGroup> groupsCounter;

        protected Graphics.AbsVoxelObj overviewBanner;
       
        public Rotation1D rotation = Rotation1D.D180.Add(Ref.rnd.Plus_MinusF(0.8f));
        public float soldierRadius = 0.5f;
        BoundingSphere bound;
        //public int index;
        public static int NextId = 0;
        public int id;
       
        public int soldiersCount = 0;
        public int upkeep;
        public float transportSpeedLand = AbsSoldierData.StandardWalkingSpeed;
        public float transportSpeedSea = AbsSoldierData.StandardShipSpeed;
        public bool isShip = false;

        public float terrainSpeedMultiplier = 1.0f;
        public IntVector2 positionBeforeBattle;
        string name;

        public Vector2 cullingTopLeft, cullingBottomRight;
        //IntVector2 nextGroupPlacement = IntVector2.Zero;

        public Army(Faction faction, IntVector2 startPosition)
        {
            id = ++NextId;
            name = Data.NameGenerator.ArmyName();
            position = WP.ToWorldPos(startPosition);
            tilePos = startPosition;

            init(faction);
        }

        public Army()
        { }

        void init(Faction faction)
        {
            bound = new BoundingSphere(Vector3.Zero, 0.5f);
            asynchCullingUpdate(1f);
            faction.AddArmy(this);
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            WP.writePosXZ(w, position);

            w.Write((ushort)groups.Count);
            var groupsC = groups.counter();
            while (groupsC.Next())
            { 
                groupsC.sel.writeGameState(w);
            }
        }
        public void readGameState(Faction faction, System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
        {
            WP.readPosXZ(r, out position, out tilePos);

            int groupsCount = r.ReadUInt16();
            for (int i = 0; i < groupsCount; i++)
            {
                SoldierGroup group = new SoldierGroup(this, r, version, pointers);               
            }

            init(faction);
        }

        public void writeNet(System.IO.BinaryWriter w)
        {

        }
        public void readNet(System.IO.BinaryReader r)
        {

        }

        public override string TypeName()
        {
            return "Army" + " (" + TextLib.IndexToString( parentArrayIndex) +   ")";//return "Army" + parentArrayIndex.ToString();
        }

        public override string Name()
        {
            return name;//return "Army" + parentArrayIndex.ToString();
        }

        public override void toHud(Display.ObjectHudArgs args)
        {
            base.toHud(args);

            if (args.player.hud.detailLevel == Display.HudDetailLevel.Minimal)
            {
                if (args.gui.menuState.Count == 0)
                {
                    args.content.Add(new RichBoxImage(SpriteName.WarsGroupIcon));
                    args.content.Add(new RichBoxText(groups.Count.ToString()));
                    args.content.space();
                    args.content.Add(new RichBoxImage(SpriteName.WarsStrengthIcon));
                    args.content.Add(new RichBoxText(string.Format(HudLib.OneDecimalFormat, strengthValue)));
                    args.content.space();
                    args.content.Add(new RichBoxImage(SpriteName.rtsUpkeepTime));
                    args.content.Add(new RichBoxText(TextLib.LargeNumber(upkeep)));
                }
            }
            else
            {
                int count = 0;

                var groupsCounter= groups.counter();
                while (groupsCounter.Next())
                {
                    count += groupsCounter.sel.soldiers.Count;
                }
                if (args.gui.menuState.Count == 0)
                {
                    args.content.icontext(SpriteName.WarsGroupIcon, "Group count: " + groups.Count);
                    args.content.icontext(SpriteName.WarsSoldierIcon,"Total count: " + TextLib.LargeNumber(count));
                    args.content.icontext(SpriteName.WarsStrengthIcon,"Strength rating: " + string.Format(HudLib.OneDecimalFormat, strengthValue));
                    args.content.icontext(SpriteName.rtsUpkeepTime,"Upkeep: " + TextLib.LargeNumber(upkeep));
                    if (PlatformSettings.DevBuild)
                    {
                        args.content.text("Id: " + id.ToString());
                    }
                }
            }
            if (args.selected && faction == args.player.faction)
            {                
                new Display.ArmyMenu(args.player, this, args.content);
            }
        }

        public ArmyStatus Status()
        {
            ArmyStatus status = new ArmyStatus();
            var groupsCounter = groups.counter();
            while (groupsCounter.Next())
            {
                ++status.typeCount[(int)groupsCounter.sel.type];
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
                var status = Status().getTypeCounts();
                foreach (var kv in status)
                {
                    tradeSoldiersAction(ref otherArmy, kv.Key, kv.Value);
                }
            }
        }

        public void tradeSoldiersAction(ref Army toArmy, UnitType type, int count)
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

        public void tradeSoldiersTo(UnitType type, int count, Army toArmy)
        {
            var groupsCounter = groups.counter();

            while (groupsCounter.Next())
            {
                if (groupsCounter.sel.type == type)
                {
                    groupsCounter.sel.army = toArmy;
                    toArmy.AddSoldierGroup(groupsCounter.sel);
                    if (groupsCounter.sel.groupObjective == SoldierGroup.GroupObjective_FollowArmyObjective)
                    {
                        groupsCounter.sel.groupObjective = SoldierGroup.GroupObjective_ReGrouping;
                    }
                    groupsCounter.RemoveAtCurrent();

                    if (--count <= 0)
                    {
                        break;
                    }
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

            toArmy.refreshPositions(false);
            toArmy.onArmyMerge();
        }

        public void disbandSoldiersAction(UnitType type, int count)
        {
            var groupsCounter = groups.counter();
            while (groupsCounter.Next())
            {
                if (groupsCounter.sel.type == type)
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
                    soldiersDeserted += group.soldiers.Count;
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
            if (!InBattle())
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
                hoverAndSelectInfo(guiModels, player.playerData.localPlayerIndex);
            }
        }

        override public bool rayCollision(Ray ray)
        {
            float? distance = ray.Intersects(bound);
            return distance.HasValue;
        }

        public override void selectionFrame(bool hover, Selection selection)
        {
            selection.frameModel.Position = position;
             selection.frameModel.position.Y += 0.05f;
            selection.frameModel.Scale = new Vector3(0.6f);

            selection.frameModel.LoadedMeshType = hover ? LoadedMesh.SelectCircleDotted : LoadedMesh.SelectCircleSolid;
            //frameModel.SetSpriteName(hover ? SpriteName.LittleUnitSelectionDotted : SpriteName.WhiteCirkle);
        }

        

        virtual public void update()
        {
            if (id == 1391)
            {
                lib.DoNothing();
            }
            updateDetailLevel();

            if (inRender)
            {
                updateMembers(Ref.DeltaGameTimeMs, true);
            }

            if (groups.Count == 0)
            {
                DeleteMe(DeleteReason.EmptyGroup, true);
            }
        }

        void updateMembers(float time, bool fullUpdate)
        {
            if (id == 1)
            {
                lib.DoNothing();
            }
            if (groups.Count > 0)
            {
                Vector3 armyCenter = Vector3.Zero;
                int armyCenterCount = 0;
                var groupsC = groups.counter();
                SoldierGroup centerGuy = null;
                while (groupsC.Next())
                {
                    groupsC.sel.update(time, fullUpdate);
                    if (groupsC.sel.armyLocalPlacement.Y == 0)
                    {
                        armyCenter += groupsC.sel.position;
                        ++armyCenterCount;
                        if (groupsC.sel.armyLocalPlacement.X == 0)
                        {
                            centerGuy = groupsC.sel;
                        }
                    }
                }

                if (centerGuy != null)
                {
                    var newPosition = centerGuy.position;
                    if (newPosition.X > 1 && newPosition.Z > 1)
                    { 
                        position=newPosition;
                    }
                }
                else if (armyCenterCount > 0)
                {
                    var newPosition = armyCenter / armyCenterCount;
                    if (newPosition.X > 1 && newPosition.Z > 1)
                    {
                        position = newPosition;
                    }
                }

                if (overviewBanner != null && fullUpdate)
                {
                    updateModelsPosition();
                    overviewBanner.Frame = isShip ? 1 : 0;
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
            int width = groupsWidth();

            IntVector2 nextGroupPlacementIndex = IntVector2.Zero;

            refreshPositionsFor(ArmyPlacement.Front, ref nextGroupPlacementIndex, width, onPurchase);
            refreshPositionsFor(ArmyPlacement.Mid, ref nextGroupPlacementIndex, width, onPurchase);
            refreshPositionsFor(ArmyPlacement.Back, ref nextGroupPlacementIndex, width, onPurchase);
        }

        public int groupsWidth()
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

            return width;
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


        public void refreshPositionsFor(ArmyPlacement armyPlacement, ref IntVector2 nextGroupPlacementIndex, int groupsWidth, bool onPurchase)
        {
            var groupsC = groups.counter();

            while (groupsC.Next())
            {
                var soldier = groupsC.sel.FirstSoldier();
                if (soldier != null)
                {
                    if (soldier.data.ArmyFrontToBackPlacement == armyPlacement)
                    {
                        IntVector2 result = nextGroupPlacementIndex;
                        result.X = TogglePlacementX(nextGroupPlacementIndex.X);// PlacementX[result.X];

                        nextGroupPlacementIndex.Grindex_Next(groupsWidth);
                        groupsC.sel.SetArmyPlacement(result, onPurchase); //behöver en wake up alert
                                                                          
                    }
                }
            }
        }

        protected override void setInRenderState()
        {              
            if (inRender)
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

            var groupsCounter = groups.counter();
            while (groupsCounter.Next())
            {
                var soldiers = groupsCounter.sel.soldiers.counter();
                while (soldiers.Next())
                {
                    soldiers.sel.setDetailLevel(inRender);
                }
            }
        }

        public void asynchGameObjectsUpdate(float time)
        {
            if (groups.Count > 0)
            {
                int count = 0;
                int shipCount = 0;
                double speedbonus = 0;
                float totalStrength = 0;
                int dps;

                Vector2 minpos = VectorExt.V2Max;
                Vector2 maxpos = VectorExt.V2Min;


                Map.Tile tile;
                if (DssRef.world.tileGrid.TryGet(tilePos, out tile))
                { 
                    terrainSpeedMultiplier = tile.TerrainSpeedMultiplier(isShip);
                }
                
                var battleGroup_sp = battleGroup;
                bool inBattle = battleGroup_sp != null && battleGroup_sp.battleState == Battle.BattleState.Battle;
                bool notBattle = !inBattle;

                var groupsC = groups.counter();

               
                while (groupsC.Next())
                {
                    var unitData = DssRef.unitsdata.Get(groupsC.sel.type);
                    groupsC.sel.asynchUpdate();
                    count += groupsC.sel.soldiers.Count;
                    groupsC.sel.setBattleWalkingSpeed();

                    if (groupsC.sel.IsShip())
                    {
                        ++shipCount;
                        dps = unitData.DPS_sea();

                        if (notBattle)                       
                        {
                            speedbonus += unitData.ArmySpeedBonusSea;
                            groupsC.sel.walkSpeed = transportSpeedSea;
                        }
                    }
                    else
                    {
                        dps = unitData.DPS_land();

                        if (notBattle)
                        {
                            speedbonus += unitData.ArmySpeedBonusLand;
                            groupsC.sel.walkSpeed = transportSpeedLand;
                        }
                    }

                    if (groupsC.sel.position.X < minpos.X)
                    {
                        minpos.X = groupsC.sel.position.X;
                    }
                    if (groupsC.sel.position.X > maxpos.X)
                    { 
                        maxpos.X = groupsC.sel.position.X;
                    }

                    if (groupsC.sel.position.Y < minpos.Y)
                    {
                        minpos.Y = groupsC.sel.position.Y;
                    }
                    if (groupsC.sel.position.X > maxpos.Y)
                    {
                        maxpos.Y = groupsC.sel.position.X;
                    }

                    totalStrength += (dps + unitData.basehealth * AllUnits.HealthToStrengthConvertion) * groupsC.sel.soldiers.Count;
                }
                
                isShip = shipCount > groups.Count / 2;
                soldierRadius = MathExt.SquareRootF(count) / 20f;
                strengthValue = count;
                soldiersCount = count;
                tilePos = WP.ToTilePos(position);
                speedbonus /= groups.Count;
                if (speedbonus < 0)
                {
                    speedbonus *= 0.5;
                }
                speedbonus += 1;
                transportSpeedLand = Convert.ToSingle(AbsSoldierData.StandardWalkingSpeed * speedbonus);
                transportSpeedSea = Convert.ToSingle(AbsSoldierData.StandardShipSpeed * speedbonus);
                collectBattles_asynch();

                strengthValue = totalStrength / AllUnits.AverageGroupStrength;
            }
        }

        public void asynchSleepObjectsUpdate(float time)
        {
            if (!inRender)
            {
                updateMembers(time * Ref.GameTimeSpeed, false);
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
                var battle = battles.First();
                if (battle != null && battle.faction.player.IsPlayer())
                {
                    DssRef.achieve.UnlockAchievement(AchievementIndex.viking_naval);
                }
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
        }

        public void onNewModel(LootFest.VoxelModelName name, VoxelModel master)
        {
            if (overviewBanner != null)
            {
                DSSWars.Faction.SetNewMaster(name, OverviewBannerModelName, overviewBanner, master);
            }

            if (inRender)
            {
                var groupsC = groups.counter();
                while (groupsC.Next())
                {
                    groupsC.sel.onNewModel(name, master);
                }
            }
        }

        public void setWalkNode(IntVector2 area,
            bool nextIsFootTransform, bool nextIsShipTransform)
        {
            if (battleGroup != null)
            {
                return;
            }

            if (id == 786)
            { 
                lib.DoNothing();
            }
            Vector2 diff = WP.ToWorldPosXZ(area);
            diff.X -= position.X;
            diff.Y -= position.Z;
            rotation = Rotation1D.FromDirection(diff);

            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.setWalkNode(area, nextIsFootTransform, nextIsShipTransform);                
            }
        }

        public override void setFaction(Faction faction)
        {
            base.setFaction(faction);
            faction.AddArmy(this);
            
        }

        public override void OnNewOwner()
        {
            if (inRender)
            {
                inRender = false;
                setInRenderState();
                inRender = true;
                setInRenderState();
            }
        }

        public override bool isMelee()
        {
            throw new NotImplementedException();
        }

        public override bool defeatedBy(Faction attacker)
        {
            return isDeleted;
        }

        public override bool aliveAndBelongTo(Faction faction)
        {
            return !isDeleted;
        }

        public override void OnBattleJoin(BattleGroup group)
        {
            base.OnBattleJoin(group);

            var groupsC = groups.counter();
            while (groupsC.Next())
            {   
                groupsC.sel.battleQueTime = 0;
                groupsC.sel.prevBattleGridPos = IntVector2.MinValue;
            }
        }

        public override void ExitBattleGroup()
        {
            base.ExitBattleGroup();

            refreshPositions(false);
            Ai_EnterPeaceEvent();

            bool refreshArmyPos = IdleObjetive();

            var groupsC = groups.counter();
            while (groupsC.Next())
            {   
                if (refreshArmyPos)
                {
                    groupsC.sel.bumpWalkToNode(tilePos);
                }

                groupsC.sel.EnterPeaceEvent();
            }
        }
        
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

        override public Army GetArmy() { return this; }

        public override GameObjectType gameobjectType()
        {
            return GameObject.GameObjectType.Army;
        }


        public override string ToString()
        {
            return "Army" + parentArrayIndex.ToString() + ", " + faction.ToString();
        }

        public bool Is(int index, int faction)
        {
            return this.parentArrayIndex == index && this.faction.parentArrayIndex == faction;
        }
    }
    enum ArmyPlacement
    { 
        Front,
        Mid,
        Back,        
    }
}
