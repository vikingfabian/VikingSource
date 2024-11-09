//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using VikingEngine.DataStream;
//using VikingEngine.DSSWars.Data;
//using VikingEngine.DSSWars.GameObject;
//using VikingEngine.DSSWars.Map;
//using VikingEngine.LootFest.Players;
//using VikingEngine.ToGG.MoonFall;

//namespace VikingEngine.DSSWars.Battle
//{
//    class BattleGroup :AbsGameObject
//    {
//        const int StandardGridRadius = 20;
//        //const float MaxQueTime = 5000;        
//        SpottedArray<AbsMapObject> members;
//        //SpottedArrayCounter<AbsMapObject> membersC;
//        Vector2 center;
//        Rotation1D rotation;

//        IntVector2 gridTopLeft;
//        Grid2D<BattleGridNode> grid;
//        Time preparationTime = new Time(10, TimeUnit.Seconds);
//        float nextAiOrderTime = 0;
//        float checkIdleTime = 800;
//        public BattleState battleState = BattleState.Prepare;
        
//        //bool[] playerJoined = null;
//        List<Faction> factions = new List<Faction>(4);
//        float ingameTimeStamp;
//        public BattleGroup(AbsMapObject m1, AbsMapObject m2) 
//        {
//            //playerJoined = new bool[DssRef.state.localPlayers.Count];
//            ingameTimeStamp = Ref.TotalGameTimeSec;
//            members = new SpottedArray<AbsMapObject>(4);
//            //membersC = new SpottedArrayCounter<AbsMapObject>(members);

//            parentArrayIndex = DssRef.state.battles.Add(this);

//            center = VectorExt.V3XZtoV2(m2.position + m1.position) / 2f;
//            Vector2 diff = VectorExt.V3XZtoV2(m2.position - m1.position);
//            rotation = Rotation1D.FromDirection(diff);
//            rotation.radians %= MathExt.TauOver4;

//            createGrid();

//            addPart(m1);
//            addPart(m2);
//        }

//        public BattleGroup(System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
//        {
//            readGameState(r, version, pointers);
//        }

//        public void writeGameState(System.IO.BinaryWriter w)
//        {
//            w.Write((ushort)members.Count);
//            var membersC = members.counter();
//            while (membersC.Next())
//            {
//                new BattleMemberObjectPointer(w, membersC.sel);
//            }

//            SaveLib.WriteVector(w, center);
//            w.Write(rotation.ByteDir);
//        }
       
//        public void readGameState(System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
//        {

//        }
//        public void addPointer(AbsGameObject objectPointer)
//        {
//            addPart((AbsMapObject)objectPointer, false);
//        }

//        public SpottedArrayCounter<AbsMapObject> MembersCounter()
//        {
//            return members.counter();
//        }

//        public bool addPart(AbsMapObject m, bool atStart = true)
//        {
//            if (battleState == BattleState.End)
//            { return false; }

//            bool newMember = false;

//            if (members.AddIfNotExists(m))
//            {
//                bool newFaction = !factions.Contains(m.faction);
//                m.OnBattleJoin(this);
//                newMember = true;

//                if (m.faction.player.IsPlayer())
//                {
//                    var player = m.faction.player.GetLocalPlayer();
//                    //int ix = player.playerData.localPlayerIndex;
//                    if (newFaction)
//                    {
//                        Ref.update.AddSyncAction(new SyncAction2Arg<BattleGroup, AbsMapObject>( player.enterBattle,this, m));//.battles.Add(this);
//                    }
//                }

//                if (newFaction)
//                {
//                    factions.Add(m.faction);
//                }

//                if (atStart)
//                {
//                    placeSoldiers(m);
//                }
//            }

//            return newMember;
//        }

//        void createGrid()
//        {
//            gridTopLeft = new IntVector2(-StandardGridRadius);
//            grid = new Grid2D<BattleGridNode>(StandardGridRadius * 2 + 1);
//        }

//        void placeSoldiers(AbsMapObject aArmy)
//        {
//            var army = aArmy.GetArmy();
//            if (army != null)
//            {
//                army.positionBeforeBattle = army.tilePos;
//                IntVector2 armyGridPos = WpToGridPos(army.position.X, army.position.Z);
//                army.battleGridForward = -armyGridPos.MajorDirectionVec;
//                army.battleDirection = rotation;

                
//                IntVector2 invertY = armyGridPos;
//                invertY.Y = -invertY.Y;
//                army.battleDirection.Add(-(float)invertY.MajorDirection * MathExt.TauOver4);

//                bool reversingForwardDirection = Math.Abs(army.battleDirection.AngleDifference(army.rotation.radians)) > MathExt.TauOver4;

//                int width = army.groupsWidth();
//                var groupsC = army.groups.counter();
//                IntVector2 nextGroupPlacementIndex = IntVector2.Zero;

//                for (ArmyPlacement armyPlacement = ArmyPlacement.Front; armyPlacement <= ArmyPlacement.Back; armyPlacement++)
//                {
//                    groupsC.Reset();

//                    while (groupsC.Next())
//                    {
//                        var soldier = groupsC.sel.FirstSoldier();
//                        if (soldier != null)
//                        {
//                            if (soldier.soldierData.ArmyFrontToBackPlacement == armyPlacement)
//                            {
//                                IntVector2 result = nextGroupPlacementIndex;
//                                result.X = Army.TogglePlacementX(nextGroupPlacementIndex.X);// PlacementX[result.X];

//                                if (reversingForwardDirection)
//                                { 
//                                    result.X = -result.X;
//                                }

//                                nextGroupPlacementIndex.Grindex_Next(width);

//                                placeGroupInNode(groupsC.sel, IntVector2.RotateVector(result, army.battleGridForward) + armyGridPos);

//                            }
//                        }
//                    }
//                }
//            }
//        }

//        const float AiOrderTime = 600;

//        public bool async_update(float time)
//        {
//            float gameTime = (Ref.TotalGameTimeSec - ingameTimeStamp) * TimeExt.SecondToMs;
//            ingameTimeStamp = Ref.TotalGameTimeSec;
//            switch (battleState)
//            { 
//                case BattleState.Prepare:
//                    chainAdjacentArmies();

//                    checkIdleTime -= gameTime;

//                    if (checkIdleTime <= 0)
//                    {
//                        checkIdleTime = 200;

//                        if (allIdle())
//                        {
//                            battleState = BattleState.Battle;
//                        }
//                    }

//                    if (preparationTime.CountDown(gameTime))
//                    {
//                        battleState = BattleState.Battle;
//                    }
//                    break;

//                case BattleState.Battle:
//                    nextAiOrderTime -= gameTime;
//                    if (nextAiOrderTime < 0)
//                    {
//                        bool inBattle = refreshAiOrders_hasBattle();

//                        if (inBattle)
//                        {
//                            refreshGroupsWalkPath(gameTime);

//                            nextAiOrderTime = AiOrderTime;
//                        }
//                        else
//                        {
//                            battleState = BattleState.End;
                            
//                        }
//                    }
//                    break;

//                case BattleState.End:
//                    ExitBattle();
//                    return true;
//            }

//            return false;
//        }

//        bool allIdle()
//        {
//            var membersC = MembersCounter();

//            while (membersC.Next())
//            {
//                if (membersC.sel.gameobjectType() == GameObjectType.Army)
//                {
//                    var army = membersC.sel.GetArmy();

//                    var groupsC = army.groups.counter();
//                    while (groupsC.Next())
//                    {
//                        if (!groupsC.sel.groupIsIdle)
//                        {
//                            return false;
//                        }
//                    }
//                }
//            }

//            return true;
//        }

//        void chainAdjacentArmies()
//        {
//            List<Faction> factions = new List<Faction>(2);

//            var membersC = members.counter();
//            while (membersC.Next())
//            {
//                factions.AddIfMissing(membersC.sel.faction);
//            }

//            membersC.Reset();
//            while (membersC.Next())
//            {
//                List<AbsMapObject> nearMapObjects = DssRef.world.unitCollAreaGrid.BattleGroupNearMapObjects(
//                    membersC.sel.tilePos, factions);

//                foreach (var obj in nearMapObjects)
//                { 
//                    addPart(obj);
//                }
//            }
//        }

//        bool refreshAiOrders_hasBattle()
//        {
//            bool hasBattle = false;

//            //todo replace with player orders
//            var membersC = MembersCounter();

//            while (membersC.Next())
//            {
//                if (membersC.sel.gameobjectType() == GameObjectType.Army)
//                {
//                    if (membersC.sel.defeated())
//                    {
//                        membersC.sel.battleGroup =null;
//                        membersC.RemoveAtCurrent();
//                    }
//                    else
//                    {
//                        var army = membersC.sel.GetArmy();
//                        var groupsC = army.groups.counter();
//                        while (groupsC.Next())
//                        {
//                            hasBattle |= groupsC.sel.asynchFindBattleTarget(this);
//                        }
//                    }
//                }
//                else
//                {
//                    var city = membersC.sel.GetCity();
//                    city.asynchFindBattleTarget();
//                }
               
//            }

//            return hasBattle;
//        }

//        void refreshGroupsWalkPath(float gametime)
//        {
//            /*
//             * Försök att alltid hålla formation
//             * 1. Refresh av alla enheters position i grid
//             * 2. Ge path till de med helt rak move först, och blocka rutan framför sig
//             * 3. Flytta alla andra, gör en kort flank sökning eller köa
//             */

//            clearGrid();

//            refreshUnitsGridPositions();

//            var membersC = MembersCounter();

//            while (membersC.Next())
//            {
//                var army = membersC.sel.GetArmy();
//                if (army != null)
//                {
//                    var groupsC = army.groups.counter();
//                    while (groupsC.Next())
//                    {
//                        groupsC.sel.battleQueTime += AiOrderTime;
//                        walkPath(groupsC.sel, true, gametime);
//                    }
//                }
//            }

//            membersC.Reset();

//            while (membersC.Next())
//            {
//                var army = membersC.sel.GetArmy();
//                if (army != null)
//                {
//                    var groupsC = army.groups.counter();
//                    while (groupsC.Next())
//                    {
//                        if (!groupsC.sel.battleWalkPath)
//                        {
//                            walkPath(groupsC.sel, false, gametime);
//                        }
//                    }
//                }
//            }
//        }

//        void clearGrid()
//        {
//            grid.LoopBegin();
//            while (grid.LoopNext())
//            {
//                grid.LoopValueGet()?.clear();
//            }
//        }

//        void refreshUnitsGridPositions()
//        {
//            var membersC = MembersCounter();
//            while (membersC.Next())
//            {
//                if (membersC.sel.gameobjectType() == GameObjectType.Army)
//                {
//                    var army = membersC.sel.GetArmy();

//                    var groupsC = army.groups.counter();
//                    while (groupsC.Next())
//                    {
//                        groupsC.sel.battleWalkPath = false;
//                        IntVector2 prev = groupsC.sel.battleGridPos;
//                        groupsC.sel.battleGridPos = WpToGridPos(groupsC.sel.position.X, groupsC.sel.position.Z);

//                        if (groupsC.sel.battleGridPos != prev)
//                        {
//                            groupsC.sel.prevBattleGridPos = prev;
//                        }

//                        getNode(groupsC.sel.battleGridPos).add(groupsC.sel);
//                    }
//                }
//                else
//                { 
//                    var city = membersC.sel.GetCity();
//                    city.battleGridPos = WpToGridPos(city.position.X, city.position.Z);
//                }
//            }
//        }

//        void walkPath(SoldierGroup group, bool straightOnly, float time)
//        {
//            if (group.debugTagged)
//            { 
//                 lib.DoNothing();
//            }

//            if (group.attacking_soldierGroupOrCity != null &&
//                !group.attackState)
//            {
//                IntVector2 diff = group.attacking_soldierGroupOrCity.battleGridPos - group.battleGridPos;
//                if (diff.HasValue())
//                {
//                    var nDiff = diff.Normal();
//                    IntVector2 next = group.battleGridPos + nDiff;

//                    if (straightOnly)
//                    {                       
//                        if (diff.IsOrthogonal())
//                        {
//                            tryWalkToNode(group, next);
//                            //applyWalkNode(group, next);
//                        }
//                    }
//                    else
//                    {
//                        //Walk towards enemy
//                        if (!tryWalkToNode(group, next))
//                        {
//                            //Try left and right turn
//                            IntVector2 left = group.battleGridPos + VectorExt.RotateVector45DegreeLeft_Normal(nDiff);
//                            BattleGridNode leftNode;
//                            float leftValue = flankToNodeValue(group, left, out leftNode);

//                            IntVector2 right = group.battleGridPos + VectorExt.RotateVector45DegreeRight_Normal(nDiff);
//                            BattleGridNode rightNode;
//                            float rightValue = flankToNodeValue(group, right, out rightNode);

//                            if (leftValue > rightValue && leftValue > 0)
//                            {
//                                applyWalkToNode(group, leftNode);
//                            }
//                            else if (rightValue > 0)
//                            {
//                                applyWalkToNode(group, rightNode);
//                            }
                            
//                            //if (!tryWalkToNode(group, next))
//                            //{
//                            //    next = group.battleGridPos + VectorExt.RotateVector45DegreeRight_Normal(nDiff);
//                            //    tryWalkToNode(group, next);

//                            //    //on fail, the unit will que up
//                            //}
//                        }
//                    }
//                }
//            }

//        }

//        float flankToNodeValue(SoldierGroup group, IntVector2 next, out BattleGridNode goalNode)
//        {
//            goalNode = getNode(next);

//            if (goalNode.blockedByFriendly(group) || goalNode.blocked)
//            {
//                return 0;
//            }
//            else
//            {
//                float value = 4;

//                if (next == group.prevBattleGridPos)
//                {
//                    value = -1;
//                }

//                var currentNode = getNode(group.battleGridPos);
//                float currentDist = VectorExt.SideLength_XZ(group.army.position, currentNode.worldPos);
//                float nextDist = VectorExt.SideLength_XZ(group.army.position, goalNode.worldPos);

//                value += nextDist - currentDist; //Moving away from center is more points

//                if (group.isShip != goalNode.water)
//                {
//                    value = group.battleQueTime < DssLib.BattleMaxQueTimeMs? - 1: -10;//-= 1;
//                }

//                return value;
//            }
//        }

//        bool tryWalkToNode(SoldierGroup group, IntVector2 next)
//        {
//            var goalNode = getNode(next);

//            if (goalNode.blockedByFriendly(group) || goalNode.blocked)
//            {
//                return false;
//            }
//            else if (group.isShip != goalNode.water)
//            {
//                if (group.battleQueTime < DssLib.BattleMaxQueTimeMs)
//                {
//                    return false;
//                }
//            }

            

//            return applyWalkToNode(group, goalNode);
            
//        }

//        private bool applyWalkToNode(SoldierGroup group, BattleGridNode goalNode)
//        {

//            if (group.debugTagged)
//            {
//                lib.DoNothing();
//            }
//            //Apply
//            group.battleQueTime = 0;
//            group.battleWalkPath = true;
//            var currentNode = getNode(group.battleGridPos);
//            currentNode.remove(group);
//            goalNode.add(group);

//            group.setBattleNode(goalNode.worldPos);

//            if (group.isShip != goalNode.water)
//            {
//                Tile tile;
//                if (DssRef.world.tileGrid.TryGet(group.tilePos, out tile))
//                {
//                    if (tile.IsWater() == goalNode.water)
//                    {
//                        if (!group.inShipTransform)
//                        {
//                            group.inShipTransform = true;
//                            new ShipTransform(group, true);
//                        }
//                    }
//                }
//            }

//            return true;
//        }

//        void applyWalkNode(SoldierGroup group, IntVector2 next)
//        {
//            getNode(group.battleGridPos).remove(group);
//            var goalNode = getNode(next);
//            goalNode.add(group);

//            group.goalWp = goalNode.worldPos;
//        }

//        void placeGroupInNode(SoldierGroup group, IntVector2 nodePos)
//        {
//            group.battleGridPos = nodePos;
//            var node = getNode(group.battleGridPos);
//            group.goalWp = node.worldPos;
//            node.add(group);
//        }

//        void debugVisuals()
//        {
//            Rectangle2 area = Rectangle2.FromCenterTileAndRadius(IntVector2.Zero, 5);
//            ForXYLoop loop = new ForXYLoop(area);

//            while (loop.Next())
//            {
//                bool outsideBounds;
//                Vector3 pos = gridPosToWp(loop.Position, out outsideBounds);

//                Graphics.Mesh dot = new Graphics.Mesh(LoadedMesh.cube_repeating,
//                        pos,
//                        new Vector3(0.05f), Graphics.TextureEffectType.Flat,
//                        SpriteName.WhiteArea, loop.Position == IntVector2.Zero? Color.Blue : Color.Red, false);
              
//                dot.AddToRender(DrawGame.UnitDetailLayer);
//            }
//        }

//        /// <returns>V3 world position</returns>
//        Vector3 gridPosToWp(IntVector2 gridPos, out bool outsideBounds)
//        {
//            Vector2 offset = VectorExt.RotateVector(
//                new Vector2(
//                    gridPos.X * DssVar.SoldierGroup_Spacing, 
//                    gridPos.Y * DssVar.SoldierGroup_Spacing), 
//                rotation.radians);

//            Vector3 result = new Vector3(
//                center.X + offset.X,
//                0,
//                center.Y + offset.Y);

//            if (DssRef.world.unitBounds.IntersectPoint(result.X, result.Z))
//            {
//                result.Y = DssRef.world.SubTileHeight(result);
//                outsideBounds = false;
//            }
//            else
//            {
//                outsideBounds = true;
//            }
//            return result;  
//        }

//        IntVector2 WpToGridPos(float worldX, float worldZ)
//        {
//            float offsetX = worldX - center.X;
//            float offsetY = worldZ - center.Y;
            
//            Vector2 rotatedBackOffset = VectorExt.RotateVector(new Vector2(offsetX, offsetY), -rotation.radians);

//            var result = new IntVector2( 
//                rotatedBackOffset.X / DssVar.SoldierGroup_Spacing, 
//                rotatedBackOffset.Y / DssVar.SoldierGroup_Spacing);
//            return result;  
//        }

//        BattleGridNode getNode(IntVector2 pos)
//        {
//            BattleGridNode node;
//            IntVector2 localPos = pos - gridTopLeft;
//            if (grid.TryGet(localPos, out node))
//            {
//                if (node == null)
//                {
//                    bool outsideBounds;
//                    Vector3 wp = gridPosToWp(pos, out outsideBounds);
//                    node = new BattleGridNode(wp, outsideBounds);
//                    grid.Set(localPos, node);
//                }
//            }
//            else
//            {
//                //expand size
//                Rectangle2 area = new Rectangle2(IntVector2.Zero, grid.Size);
//                var minAdd = area.LengthToClosestTileEdge(localPos);

//                IntVector2 addRadius = new IntVector2( Math.Max(minAdd * 2, 10));
//                grid.ExpandSize(addRadius * 2, addRadius);
//                gridTopLeft -= addRadius;

//                return getNode(pos);
//            }

//            return node;
//        }

//        void ExitBattle()
//        {
//            if (members.Count == 0)
//            { return; }

//            List<City> cities = new List<City>(2);
//            Dictionary<int, float> cityDominationStrength = new Dictionary<int, float>(4);

//            var membersC = MembersCounter();
//            while (membersC.Next())
//            {
//                if (membersC.sel.gameobjectType() == GameObjectType.City)
//                {
//                    cities.Add(membersC.sel.GetCity());
//                }

//                if (cityDominationStrength.ContainsKey(membersC.sel.faction.parentArrayIndex))
//                {
//                    cityDominationStrength[membersC.sel.faction.parentArrayIndex] += membersC.sel.strengthValue;
//                }
//                else
//                {
//                    cityDominationStrength.Add(membersC.sel.faction.parentArrayIndex, membersC.sel.strengthValue);
//                }

//                membersC.sel.ExitBattleGroup();
//            }

//            int strongestFaction = -1;
//            float strongest = float.MinValue;

//            foreach (var kv in cityDominationStrength)
//            {
//                if (kv.Value > strongest)
//                {
//                    strongestFaction = kv.Key;
//                    strongest = kv.Value;
//                }
//            }

//            var dominatingFaction = DssRef.world.factions.Array[strongestFaction];

//            if (cities.Count > 0)
//            {
//                foreach (var c in cities)
//                {
//                    if (DssRef.diplomacy.InWar(c.faction, dominatingFaction))
//                    {
//                        if (c.faction.player.IsPlayer())
//                        {
//                            ++c.faction.player.GetLocalPlayer().statistics.CitiesLost;
//                        }
//                        if (dominatingFaction.player.IsPlayer())
//                        {
//                            ++dominatingFaction.player.GetLocalPlayer().statistics.CitiesCaptured;
//                        }

//                        Ref.update.AddSyncAction(new SyncAction1Arg<Faction>(c.setFaction, dominatingFaction));
//                    }
//                }
//            }

//            for (int i = 0; i < factions.Count; ++i)
//            {
//                var f = factions[i];

//                bool winner = f == dominatingFaction || !DssRef.diplomacy.InWar(f, dominatingFaction);

//                if (f.player.IsPlayer())
//                {
//                    var p = f.player.GetLocalPlayer();
//                    p.battles.Remove(this);
//                    if (winner)
//                    {
//                        p.statistics.BattlesWon++;
//                    }
//                    else
//                    {
//                        p.statistics.BattlesLost++;
//                    }
//                }
//            }
//        }

//        public override Faction GetFaction()
//        {
//            throw new NotImplementedException();
//        }

//        public override IntVector2 TilePos()
//        {
//            return IntVector2.FromVec2(center);
//        }

//        public override Vector3 WorldPos()
//        {
//            return VectorExt.V3FromXZ(center, 0);
//        }

//        public override string TypeName()
//        {
//            return DssRef.lang.Hud_Battle + " (" + parentArrayIndex.ToString() + ")"; //return "Battle (" + TextLib.IndexToString(parentArrayIndex) + ")";
//        }

//        public override GameObjectType gameobjectType()
//        {
//            return GameObjectType.Battle;
//        }

//        public override bool aliveAndBelongTo(Faction faction)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    class BattleGridNode
//    { 
//        public SoldierGroup group1 = null;
//        public SoldierGroup group2 = null;
//        public Vector3 worldPos;
//        public bool water;
//        public bool blocked;

//        public BattleGridNode(Vector3 worldPos, bool outsideBounds)
//        {
//            this.worldPos = worldPos;
//            blocked = outsideBounds;
//            Map.Tile tile;
//            if (DssRef.world.tileGrid.TryGet(new IntVector2(worldPos.X, worldPos.Z), out tile))
//            {
//                water = tile.IsWater();
//            }
//        }

//        public BattleGridNode(Vector3 worldPos, bool outsideBounds, 
//            System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
//            :this(worldPos, outsideBounds)
//        {
//            readGameState(r, version, pointers);
//        }

//        public void writeGameState(System.IO.BinaryWriter w)
//        {

//        }
//        public void readGameState(System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
//        {

//        }

//        public void add(SoldierGroup group)
//        {
//            if (group1 == null)
//            {
//                group1 = group;
//            }
//            else
//            {
//                group2 = group;
//            }
//        }

//        public void remove(SoldierGroup group)
//        {
//            if (group1 == group)
//            {
//                group1 = null;
//            }
//            else if (group2 == group)
//            {
//                group2 = null;
//            }
//        }

//        public bool blockedByFriendly(SoldierGroup group)
//        {
//            if (group1 != null && group1.army.faction == group.army.faction)
//            { 
//                return true;
//            }
//            if (group2 != null && group2.army.faction == group.army.faction)
//            {
//                return true;
//            }

//            return false;
//        }

//        public void clear()
//        {
//            group1 = null;
//            group2 = null;
//        }
               
//    }

//    enum BattleState
//    { 
//        Prepare,
//        Battle,
//        End,
//    }
//}
