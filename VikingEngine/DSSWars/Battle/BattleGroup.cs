using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Battle
{
    class BattleGroup
    {
        const int StandardGridRadius = 20;

        int index;
        SpottedArray<AbsMapObject> members;
        SpottedArrayCounter<AbsMapObject> membersC;
        Vector2 center;
        Rotation1D rotation;

        IntVector2 gridTopLeft;
        Grid2D<BattleGridNode> grid;
        Time preparationTime = new Time(10, TimeUnit.Seconds);
        float nextAiOrderTime = 0;
        public bool battleState = false;

        public BattleGroup(AbsMapObject m1, AbsMapObject m2) 
        {
            members = new SpottedArray<AbsMapObject>(4);
            membersC = new SpottedArrayCounter<AbsMapObject>(members);

            members.Add(m1);
            members.Add(m2);
          
            m2.battleGroup = this;

            index = DssRef.state.battles.Add(this);

            center = VectorExt.V3XZtoV2(m2.position + m1.position) / 2f;
            Vector2 diff = VectorExt.V3XZtoV2(m2.position - m1.position);
            rotation = Rotation1D.FromDirection(diff);
            rotation.radians %= MathExt.TauOver4;

            createGrid();

            placeSoldiers(m1);
            placeSoldiers(m2);
        }

        public SpottedArrayCounter<AbsMapObject> MembersCounter()
        {
            return membersC.Clone();
        }

        public void addPart(AbsMapObject m)
        {
            m.battleGroup = this;
            members.Add(m);
            placeSoldiers(m);
        }

        void createGrid()
        {
            gridTopLeft = new IntVector2(-StandardGridRadius);
            grid = new Grid2D<BattleGridNode>(StandardGridRadius * 2 + 1);
            //grid.LoopBegin();
            //while (grid.LoopNext())
            //{
            //    grid.LoopValueSet(new BattleGridNode()
            //    {
            //        worldPos = gridPosToWp(grid.LoopPosition + gridTopLeft),
            //    });
            //}
        }

        void placeSoldiers(AbsMapObject aArmy)
        {
            var army = aArmy.GetArmy();
            if (army != null)
            {
                IntVector2 armyGridPos = WpToGridPos(army.position.X, army.position.Z);
                army.battleGridForward = -armyGridPos.MajorDirectionVec;
                army.battleDirection = rotation;

                
                IntVector2 invertY = armyGridPos;
                invertY.Y = -invertY.Y;
                army.battleDirection.Add(-(float)invertY.MajorDirection * MathExt.TauOver4);

                bool reversingForwardDirection = Math.Abs(army.battleDirection.AngleDifference(army.rotation.radians)) > MathExt.TauOver4;

                int width = army.groupsWidth();
                var groupsC = army.groups.counter();
                IntVector2 nextGroupPlacementIndex = IntVector2.Zero;

                for (ArmyPlacement armyPlacement = ArmyPlacement.Front; armyPlacement <= ArmyPlacement.Back; armyPlacement++)
                {
                    groupsC.Reset();

                    while (groupsC.Next())
                    {
                        var soldier = groupsC.sel.FirstSoldier();
                        if (soldier != null)
                        {
                            if (soldier.data.ArmyFrontToBackPlacement == armyPlacement)
                            {
                                IntVector2 result = nextGroupPlacementIndex;
                                result.X = Army.TogglePlacementX(nextGroupPlacementIndex.X);// PlacementX[result.X];

                                if (reversingForwardDirection)
                                { 
                                    result.X = -result.X;
                                }

                                nextGroupPlacementIndex.Grindex_Next(width);

                                placeGroupInNode(groupsC.sel, IntVector2.RotateVector(result, army.battleGridForward) + armyGridPos);

                            }
                        }
                    }
                }

            }
        }

        public bool async_update(float time)
        {
            if (battleState)
            {
                nextAiOrderTime-= time;
                if (nextAiOrderTime < 0)
                {
                    bool inBattle = refreshAiOrders_hasBattle();

                    if (inBattle)
                    {
                        refreshGroupsWalkPath();

                        nextAiOrderTime = 600;
                    }
                    else
                    {
                        DeleteMe();
                        return true;
                    }
                }
            }
            else if (preparationTime.CountDown())
            {
                battleState = true;
            }

            return false;
        }

        bool refreshAiOrders_hasBattle()
        {
            bool hasBattle = false;

            //todo replace with player orders
            membersC.Reset();

            while (membersC.Next())
            {
                if (membersC.sel.defeated())
                {
                    membersC.RemoveAtCurrent();
                }
                else
                {
                    var army = membersC.sel.GetArmy();
                    if (army != null)
                    {
                        var groupsC = army.groups.counter();
                        while (groupsC.Next())
                        {
                            hasBattle |= groupsC.sel.asynchFindBattleTarget(this);
                        }
                    }
                }
            }

            return hasBattle;
        }

        void refreshGroupsWalkPath()
        {
            /*
             * Försök att alltid hålla formation
             * 1. Refresh av alla enheters position i grid
             * 2. Ge path till de med helt rak move först, och blocka rutan framför sig
             * 3. Flytta alla andra, gör en kort flank sökning eller köa
             */

            clearGrid();

            refreshUnitsGridPositions();

            membersC.Reset();

            while (membersC.Next())
            {
                var army = membersC.sel.GetArmy();
                if (army != null)
                {
                    var groupsC = army.groups.counter();
                    while (groupsC.Next())
                    {
                        walkPath(groupsC.sel, true);
                    }
                }
            }

            membersC.Reset();

            while (membersC.Next())
            {
                var army = membersC.sel.GetArmy();
                if (army != null)
                {
                    var groupsC = army.groups.counter();
                    while (groupsC.Next())
                    {
                        if (!groupsC.sel.battleWalkPath)
                        {
                            walkPath(groupsC.sel, false);
                        }
                    }
                }
            }

        }

        void clearGrid()
        {
            grid.LoopBegin();
            while (grid.LoopNext())
            {
                grid.LoopValueGet()?.clear();
            }
        }

        void refreshUnitsGridPositions()
        {
            membersC.Reset();
            while (membersC.Next())
            {
                var army = membersC.sel.GetArmy();
                if (army != null)
                {
                    var groupsC = army.groups.counter();
                    while (groupsC.Next())
                    {
                        groupsC.sel.battleWalkPath = false;
                        groupsC.sel.battleGridPos = WpToGridPos(groupsC.sel.position.X, groupsC.sel.position.Z);
                        getNode(groupsC.sel.battleGridPos).add(groupsC.sel);
                    }
                }
            }
        }

        void walkPath(SoldierGroup group, bool straightOnly)
        {
            if (group.attacking_soldierGroupOrCity != null &&
                !group.attackState)
            {
                IntVector2 diff = group.attacking_soldierGroupOrCity.battleGridPos - group.battleGridPos;
                if (diff.HasValue())
                {
                    var nDiff = diff.Normal();
                    IntVector2 next = group.battleGridPos + nDiff;

                    if (straightOnly)
                    {                       
                        if (diff.IsOrthogonal())
                        {
                            tryWalkToNode(group, next);
                            //applyWalkNode(group, next);
                        }
                    }
                    else
                    {
                        //Walk towards enemy
                        if (!tryWalkToNode(group, next))
                        {
                            //Try left and right turn
                            next = group.battleGridPos + VectorExt.RotateVector45DegreeLeft_Normal(nDiff);
                            if (!tryWalkToNode(group, next))
                            {
                                next = group.battleGridPos + VectorExt.RotateVector45DegreeRight_Normal(nDiff);
                                tryWalkToNode(group, next);

                                //on fail, the unit will que up
                            }
                        }
                    }
                }
            }

        }

        bool tryWalkToNode(SoldierGroup group, IntVector2 next)
        {
            var goalNode = getNode(next);

            if (goalNode.blockedByFriendly(group))
            {
                return false;
            }
            else
            {
                //Apply
                group.battleWalkPath = true;
                getNode(group.battleGridPos).remove(group);
                goalNode.add(group);

                group.battleWp = goalNode.worldPos;

                return true;
            }
        }

        void applyWalkNode(SoldierGroup group, IntVector2 next)
        {
            getNode(group.battleGridPos).remove(group);
            var goalNode = getNode(next);
            goalNode.add(group);

            group.battleWp = goalNode.worldPos;
        }

        void placeGroupInNode(SoldierGroup group, IntVector2 nodePos)
        {
            group.battleGridPos = nodePos;
            var node = getNode(group.battleGridPos);
            group.battleWp = node.worldPos;
            node.add(group);
        }

        void debugVisuals()
        {
            Rectangle2 area = Rectangle2.FromCenterTileAndRadius(IntVector2.Zero, 5);
            ForXYLoop loop = new ForXYLoop(area);

            while (loop.Next())
            {
                Vector3 pos = gridPosToWp(loop.Position);

                Graphics.Mesh dot = new Graphics.Mesh(LoadedMesh.cube_repeating,
                        pos,
                        new Vector3(0.05f), Graphics.TextureEffectType.Flat,
                        SpriteName.WhiteArea, loop.Position == IntVector2.Zero? Color.Blue : Color.Red, false);
              
                dot.AddToRender(DrawGame.UnitDetailLayer);
            }
        }

        /// <returns>V3 world position</returns>
        Vector3 gridPosToWp(IntVector2 gridPos)
        {
            Vector2 offset = VectorExt.RotateVector(
                new Vector2(
                    gridPos.X * SoldierGroup.GroupSpacing, 
                    gridPos.Y * SoldierGroup.GroupSpacing), 
                rotation.radians);

            Vector3 result = new Vector3(
                center.X + offset.X,
                0,
                center.Y + offset.Y);

            result.Y = DssRef.world.SubTileHeight(result);

            return result;  
        }

        IntVector2 WpToGridPos(float worldX, float worldZ)
        {
            float offsetX = worldX - center.X;
            float offsetY = worldZ - center.Y;
            
            Vector2 rotatedBackOffset = VectorExt.RotateVector(new Vector2(offsetX, offsetY), -rotation.radians);

            var result = new IntVector2( 
                rotatedBackOffset.X / SoldierGroup.GroupSpacing, 
                rotatedBackOffset.Y / SoldierGroup.GroupSpacing);
            return result;  
        }

        BattleGridNode getNode(IntVector2 pos)
        {
            BattleGridNode node;
            IntVector2 localPos = pos - gridTopLeft;
            if (grid.TryGet(localPos, out node))
            {
                if (node == null)
                {
                    node = new BattleGridNode()
                    {
                        worldPos = gridPosToWp(pos),
                    };

                    grid.Set(localPos, node);
                }
            }
            else
            {
                //expand size
                //Rectangle2//LengthToClosestTileEdge
                Rectangle2 area = new Rectangle2(IntVector2.Zero, grid.Size);
                var minAdd = area.LengthToClosestTileEdge(localPos);

                IntVector2 addRadius = new IntVector2( Math.Max(minAdd * 2, 10));
                grid.ExpandSize(addRadius * 2, addRadius);
                gridTopLeft -= addRadius;

                return getNode(pos);
            }

            return node;
        }

        void DeleteMe()
        {
            membersC.Reset();
            while (membersC.Next())
            {
                membersC.sel.ExitBattleGroup();
            }
        }
    }

    class BattleGridNode
    { 
        public SoldierGroup group1 = null;
        public SoldierGroup group2 = null;
        public Vector3 worldPos;

        public void add(SoldierGroup group)
        {
            if (group1 == null)
            {
                group1 = group;
            }
            else
            {
                group2 = group;
            }
        }

        public void remove(SoldierGroup group)
        {
            if (group1 == group)
            {
                group1 = null;
            }
            else if (group2 == group)
            {
                group2 = null;
            }
        }

        public bool blockedByFriendly(SoldierGroup group)
        {
            if (group1 != null && group1.army.faction == group.army.faction)
            { 
                return true;
            }
            if (group2 != null && group2.army.faction == group.army.faction)
            {
                return true;
            }

            return false;
        }

        public void clear()
        {
            group1 = null;
            group2 = null;
        }
    }
}
