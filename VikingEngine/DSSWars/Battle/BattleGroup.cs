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
        const int StandardGridRadius = 25;

        int index;
        List<AbsMapObject> members;
        Vector2 center;
        Rotation1D rotation;

        IntVector2 gridTopLeft;
        Grid2D<BattleGridNode> grid;

        public BattleGroup(AbsMapObject m1, AbsMapObject m2) 
        {
            members = new List<AbsMapObject> 
            { 
                m1, m2
            };

            m2.battleGroup = this;

            index = DssRef.state.battles.Add(this);

            center = VectorExt.V3XZtoV2(m2.position + m1.position) / 2f;
            Vector2 diff = VectorExt.V3XZtoV2(m2.position - m1.position);
            rotation = Rotation1D.FromDirection(diff);
            rotation.radians %= MathExt.TauOver4;

            createGrid();

            placeSoldiers(m1);
            placeSoldiers(m2);

            Ref.update.AddSyncAction(new SyncAction(debugVisuals));
        }

        void createGrid()
        {
            gridTopLeft = new IntVector2(-StandardGridRadius);
            grid = new Grid2D<BattleGridNode>(StandardGridRadius * 2 + 1);
            grid.LoopBegin();
            while (grid.LoopNext())
            {
                grid.LoopValueSet(new BattleGridNode()
                {
                    worldPos = gridPosToWp(grid.LoopPosition + gridTopLeft),
                });
            }
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

                bool reversingForwardDirection = rotation.AngleDifference(army.rotation.radians) > MathExt.TauOver4;

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

        void placeGroupInNode(SoldierGroup group, IntVector2 nodePos)
        {
            group.battleGridPos = nodePos;
            var node = getNode(group.battleGridPos);
            group.battleWp = node.worldPos;
            node.group = group;
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
            grid.TryGet(pos - gridTopLeft, out node);

            return node;
        }
    }

    class BattleGridNode
    { 
        public SoldierGroup group = null;
        public Vector3 worldPos;
    }
}
