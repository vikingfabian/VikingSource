using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;
using VikingEngine.PJ.CarBall;

namespace VikingEngine.DSSWars.GameObject
{
    partial class Army
    {
        //*center, left, right body, left/right flank / scout, front, body, second, behind
        public const int MinColumnWidth = 2;
        public const int MaxColumnWidth = 8;

        //int nextLeftRightOnFrontRow = 0;
        int nextLeftRightOnBodyRow = 0;
        int nextLeftRightOnSecondRow = 0;
        int nextLeftRightOnBehindRow = 0;

        public int armyColumnWidth = MinColumnWidth;
        public Vector3 armyGoalCenterWp;
        public float armyGoalRotation;

        public IntVector2 nextArmyPlacement(int row)
        {
            IntVector2 armyLocalPlacement2 = IntVector2.Zero;
            armyLocalPlacement2.Y = row;
            switch (armyLocalPlacement2.Y)
            {
                case ArmyPlacementGrid.Row_Body:
                    armyLocalPlacement2.X = nextLeftRight(ref nextLeftRightOnBodyRow);
                    break;
                case ArmyPlacementGrid.Row_Second:
                    armyLocalPlacement2.X = nextLeftRight(ref nextLeftRightOnSecondRow);
                    break;
                case ArmyPlacementGrid.Row_Behind:
                    armyLocalPlacement2.X = nextLeftRight(ref nextLeftRightOnBehindRow);
                    break;
            }

            return armyLocalPlacement2;

            int nextLeftRight(ref int rowLeftRight)
            {
                int result = rowLeftRight;
                if (--rowLeftRight <= -2)
                {
                    rowLeftRight = 1;
                }
                return result;
            }
        }

        public void armyColumnWidthClick(int w)
        {
            armyColumnWidth = w;
            refreshGroupPlacements2(tilePos, false, false);
        }

        void refreshGroupPlacements2(IntVector2 walkToTilePos, bool resetCommand, bool teleport, bool async = true)
        {
            if (async)
            {
                Task.Factory.StartNew(() =>
                {
                    execute();
                });
            }
            else
            {
                execute();
            }

            void execute()
            {
                if (faction.player.IsAi())
                {
                    autoColumnWidth();
                }

                ArmyPlacementGrid placementGrid = ArmyPlacementGrid.PoolGet();
                {
                    var groupsC = groups.counter();

                    while (groupsC.Next())
                    {
                        placementGrid.add(groupsC.sel);
                    }

                    placementGrid.calcPositions(this, walkToTilePos, walkGoalAsShip, resetCommand, teleport);
                }
                ArmyPlacementGrid.PoolReturn(placementGrid);
            }
        }

    }

    class ArmyPlacementGrid
    {
        static ConcurrentStack<ArmyPlacementGrid> Pool = new ConcurrentStack<ArmyPlacementGrid>();

        public static ArmyPlacementGrid PoolGet()
        {
            if (Pool.TryPop(out ArmyPlacementGrid grid))
            {
                return grid;
            }
            else
            {
                return new ArmyPlacementGrid();
            }
        }

        public static void PoolReturn(ArmyPlacementGrid grid)
        {
            // Reset the node to a default state
            grid.recycle();
            Pool.Push(grid);
        }

        public const int Row_Front = -1;
        public const int Row_Body = 0;
        public const int Row_Second = 1;
        public const int Row_Behind = 2;
        public const int RowsCount = 4;

        public const int Col_Center = 0;
        const int Col_LeftBody = -1;
        const int Col_RightBody = 1;
        const int Col_LeftFlank = -2;
        const int Col_RightFlank = 2;
        public const int ColsCount = 5;

        public const int PosXAdd = 2;
        public const int PosYAdd = 1;

        ArmyPlacementCell[,] grid;
        float[] comumnWidth;
        public ArmyPlacementGrid()
        {
            grid = new ArmyPlacementCell[ColsCount, RowsCount];
            ForXYLoop loop = new ForXYLoop(new IntVector2(ColsCount, RowsCount));
            while (loop.Next())
            {
                grid[loop.Position.X, loop.Position.Y] = new ArmyPlacementCell();
            }
            comumnWidth = new float[ColsCount];
        }

        public void recycle()
        {
            ForXYLoop loop = new ForXYLoop(new IntVector2(ColsCount, RowsCount));
            while (loop.Next())
            {
                grid[loop.Position.X, loop.Position.Y].recycle();
            }
        }

        public void add(SoldierGroup group)
        {
            grid[group.armyGridPlacement2.X + PosXAdd, group.armyGridPlacement2.Y + PosYAdd].add(group);

        }

        public void calcPositions(Army army, IntVector2 walkToPos, bool endAsShip, bool resetCommand, bool teleport)
        {
            List<SoldierGroup> failedPlacements = new List<SoldierGroup>();

            if (army.groups.Count == 0) return;

            // Debug.Log("--- Calc posotions for " + army.TypeName());

            //army.position
            //army.rotation
            //army.armyColumnWidth
            int columnWidth = /*columnStructure ? 1 :*/ army.armyColumnWidth;

            Vector2 relPos = new Vector2();
            Vector2 centerWp = walkToPos.Vec;
            army.armyGoalCenterWp = VectorExt.V2toV3XZ(centerWp);
            //float endRotation;
            Vector2 diff = centerWp - VectorExt.V3XZtoV2(army.position);
            if (VectorExt.HasValue(diff))
            {
                army.armyGoalRotation = lib.V2ToAngle(diff);
            }
            else
            {
                army.armyGoalRotation = army.rotation.Radians;
            }

            column(Col_Center + PosXAdd, relPos, out Vector2 finalCenter, out float largestWidth, -0.5f, endAsShip);

            Vector2 relPosLeft = new Vector2(-largestWidth * 0.5f - DssVar.SoldierGroup_GridExtraSpacing, 0);
            Vector2 relPosRight = new Vector2(largestWidth * 0.5f + DssVar.SoldierGroup_GridExtraSpacing, 0);



            column(Col_LeftBody + PosXAdd, relPosLeft, out Vector2 finalLeft, out largestWidth, -1f, endAsShip);
            relPosLeft.X -= largestWidth + DssVar.SoldierGroup_GridExtraSpacing;

            column(Col_RightBody + PosXAdd, relPosRight, out Vector2 finalRight, out largestWidth, 0f, endAsShip);
            relPosRight.X += largestWidth + DssVar.SoldierGroup_GridExtraSpacing;

            column(Col_LeftFlank + PosXAdd, relPosLeft, out _, out largestWidth, -1f, endAsShip);

            column(Col_RightFlank + PosXAdd, relPosRight, out _, out largestWidth, 0f, endAsShip);


            int leftColX = 0, centerColX = 0, rightColX = 0;

           

            foreach (var group in failedPlacements)
            {
                const float MaxDistance = 0.5f;
                float maxLeftY = finalLeft.Y + MaxDistance;
                float maxCenterY = finalLeft.Y + MaxDistance;
                float maxRightY = finalLeft.Y + MaxDistance;

                for (int depth = 0; depth <= 10; depth++)
                {
                    //if (depth == 10)
                    //{
                    //    //total fail
                    //    group.setArmyPlacement2(VectorExt.V2toV3XZ(centerWp));

                    //}
                    //else
                    //{
                        if (group.armyGridPlacement2.X < 0)
                        {
                            //go left to right
                            if (nextExtraColumnPos(group, ref finalLeft, ref leftColX))
                            {
                                break;
                            }

                            if (nextExtraColumnPos(group, ref finalCenter, ref centerColX))
                            {
                                break;
                            }

                            if (nextExtraColumnPos(group, ref finalRight, ref rightColX))
                            {
                                break;
                            }
                        }
                        else if (group.armyGridPlacement2.X > 0)
                        {
                            //go right to left
                            if (nextExtraColumnPos(group, ref finalRight, ref rightColX))
                            {
                                break;
                            }

                            if (nextExtraColumnPos(group, ref finalCenter, ref centerColX))
                            {
                                break;
                            }

                            if (nextExtraColumnPos(group, ref finalLeft, ref leftColX))
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (nextExtraColumnPos(group, ref finalCenter, ref centerColX))
                            {
                                break;
                            }

                            if (nextExtraColumnPos(group, ref finalRight, ref rightColX))
                            {
                                break;
                            }

                            if (nextExtraColumnPos(group, ref finalLeft, ref leftColX))
                            {
                                break;
                            }
                        }

                    if (finalLeft.Y >= maxLeftY || finalCenter.Y >= maxCenterY || finalRight.Y >= maxRightY)
                    {
                        return;
                    }
                }
                    
                //}
            }

            void column(int colindex, Vector2 _relPos, out Vector2 finalPos, out float largestWidth, float centerPan, bool endAsShip)
            {
                finalPos.X = _relPos.X;
                largestWidth = 0;
                Vector2 cellSize;
                grid[colindex, 0].nextPlacement(centerWp, army.armyGoalRotation, _relPos, centerPan, columnWidth, 
                    out cellSize, out _, true, endAsShip, resetCommand, teleport, failedPlacements);

                for (int row = 1; row < RowsCount; row++)
                {
                    grid[colindex, row].nextPlacement(centerWp, army.armyGoalRotation, _relPos, centerPan, columnWidth, 
                        out cellSize, out float adjLeft, false, endAsShip, resetCommand, teleport, failedPlacements);
                    largestWidth = lib.LargestValue(largestWidth, cellSize.X);
                    finalPos.X = adjLeft;
                    _relPos.Y += cellSize.Y + DssVar.SoldierGroup_GridExtraSpacing;
                }

                comumnWidth[colindex] = largestWidth;

                finalPos.Y = _relPos.Y + cellSize.Y + DssVar.SoldierGroup_GridExtraSpacing * 2f;
            }

            bool nextExtraColumnPos(SoldierGroup group, ref Vector2 finalPos, ref int colX)
            {
                bool success = ArmyPlacementCell.ExtraPlacement(centerWp, army.armyGoalRotation, finalPos, columnWidth, ref colX, out Vector3 goalWp);

                if (!success || colX >= columnWidth)
                {
                    colX = 0;
                    finalPos.Y += DssVar.SoldierGroup_Spacing;
                }
                if (success)
                {
                    group.setArmyPlacement2(goalWp, resetCommand, teleport);
                }
                return success;
            }
        }
    }

    //OPTIMERA, rotera en vektor för X och Y för alla beräkningar
    class ArmyPlacementCell
    {
        //Vector2 relativePosition;
        List<SoldierGroup> groups = new List<SoldierGroup>();
        public void add(SoldierGroup group)
        {
            groups.Add(group);
        }

        public void recycle()
        {
            groups.Clear();
        }

        public static bool ExtraPlacement(Vector2 centerWp, float endRotation, Vector2 relativePosition, int armyColumnWidth, ref int currentColX, out Vector3 goalWp)
        {

            Vector2 topleft = relativePosition;

            for (int colX = currentColX ; colX < armyColumnWidth; colX++)
            {
                Vector2 localPos = topleft;
                localPos.X += colX * DssVar.SoldierGroup_Spacing ;

                localPos = lib.RotatePointAroundCenter(Vector2.Zero, localPos, endRotation);
                goalWp = VectorExt.V2toV3XZ(localPos + centerWp);
                IntVector2 subTilePos = WP.ToSubTilePos(goalWp);
                if (DssRef.world.subTileGrid.TryGet(subTilePos, out SubTile subTile))
                {
                    if (subTile.mainTerrain != Map.TerrainMainType.DefaultSea)
                    {
                        currentColX = colX + 1;
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            currentColX = 0;
            goalWp = Vector3.Zero;
            return false;
        }

        public void nextPlacement(Vector2 centerWp, float endRotation, Vector2 relativePosition, float centerPan, int armyColumnWidth, 
            out Vector2 cellSize, out float adjLeft, bool frontRow, bool endAsShip, bool resetCommand, bool teleport, List<SoldierGroup> failedPlacements)
        {
            if (centerPan < 0)
            {
                lib.DoNothing();
            }
            int cols = Bound.Min(lib.SmallestValue(groups.Count, armyColumnWidth), 1);
            int rows = Bound.Min((int)Math.Ceiling(groups.Count / (double)cols), 1);

            cellSize = new Vector2(cols * DssVar.SoldierGroup_Spacing, rows * DssVar.SoldierGroup_Spacing);

            //Adjust center
            //this.relativePosition.X += cellSize.X * centerPan;

            Vector2 topleft = relativePosition;
            topleft.X += cellSize.X * centerPan + DssVar.SoldierGroup_Spacing * 0.5f;//cellSize.X * 0.5f + DssVar.SoldierGroup_Spacing * 0.5f;
            adjLeft = topleft.X;


            if (groups.Count > 0)
            {
                //Debug.Log($"####place grid x{x} y{y}");

                
                if (frontRow)
                {
                    topleft.Y -= cellSize.Y + DssVar.SoldierGroup_GridExtraSpacing;
                }

                int colX = 0;
                int rowY = 0;
                foreach (SoldierGroup group in groups)
                {
                    Vector2 localPos = new Vector2(
                        topleft.X + colX * DssVar.SoldierGroup_Spacing,
                        topleft.Y + rowY * DssVar.SoldierGroup_Spacing);

                    //if (localPos.Y < 0.38f)
                    //{
                    //    lib.DoNothing();
                    //}
                    localPos = lib.RotatePointAroundCenter(Vector2.Zero, localPos,  endRotation);
                    Vector3 goalWp = VectorExt.V2toV3XZ(localPos + centerWp);
                    IntVector2 subTilePos = WP.ToSubTilePos(goalWp);
                    var subTile = DssRef.world.subTileGrid.Get(subTilePos);
                    if ((subTile.mainTerrain == Map.TerrainMainType.DefaultSea) != endAsShip)
                    {
                        failedPlacements.Add(group);
                    }
                    //else
                    //{
                        group.setArmyPlacement2(goalWp, resetCommand, teleport);
                    //}

                    if (++colX >= cols)
                    {
                        colX = 0;
                        rowY++;
                    }
                }
            }
            //else
            //{
            //    cellSize = new Vector2(DssVar.SoldierGroup_Spacing);
            //}
        }

        public override string ToString()
        {
            return "ArmyPlacementCell, count: " + groups.Count.ToString();
        }
    }
}
