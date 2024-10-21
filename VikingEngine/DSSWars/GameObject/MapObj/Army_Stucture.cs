using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    partial class Army
    {
        //*center, left, right body, left/right flank / scout, front, body, second, behind
        
        int nextLeftRightOnFrontRow = 0;
        int nextLeftRightOnBodyRow = 0;
        int nextLeftRightOnSecondRow = 0;
        int nextLeftRightOnBehindRow = 0;

        public int armyColumnWidth = 2;

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

        void refreshGroupPlacements2(IntVector2 walkToTilePos, bool onPurchase)
        {
            ArmyPlacementGrid placementGrid = new ArmyPlacementGrid();

            var groupsC = groups.counter();

            while (groupsC.Next())
            {
                placementGrid.add(groupsC.sel);
            }

            placementGrid.calcPositions(this, walkToTilePos);

            //if (onPurchase)
            //{ //måste optimeras
            //    groupsC.Reset();
            //    while (groupsC.Next())
            //    {
            //        groupsC.sel.groupObjective = SoldierGroup.GroupObjective_FindArmyPlacement;
            //    }
            //}
        }
    }

    class ArmyPlacementGrid
    {
        //public const int Row_Scout = -1;
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

        public void add(SoldierGroup group)
        {
            grid[group.armyGridPlacement2.X + PosXAdd, group.armyGridPlacement2.Y + PosYAdd].add(group);

        }

        public void calcPositions(Army army, IntVector2 walkToPos)
        {
            if (army.groups.Count == 0) return;

           // Debug.Log("--- Calc posotions for " + army.TypeName());

            //army.position
            //army.rotation
            //army.armyColumnWidth

            Vector2 relPos = new Vector2();
            Vector2 centerWp = walkToPos.Vec;

            column(Col_Center + PosXAdd, relPos, out float largestWidth, -0.5f);

            Vector2 relPosLeft = new Vector2(-largestWidth * 0.5f - DssVar.SoldierGroup_GridExtraSpacing, 0);
            Vector2 relPosRight = new Vector2(largestWidth * 0.5f + DssVar.SoldierGroup_GridExtraSpacing, 0);

            column(Col_LeftBody + PosXAdd, relPosLeft, out largestWidth, -1f);
            relPosLeft.X -= largestWidth + DssVar.SoldierGroup_GridExtraSpacing;

            column(Col_RightBody + PosXAdd, relPosRight, out largestWidth, 0f);
            relPosRight.X += largestWidth + DssVar.SoldierGroup_GridExtraSpacing;

            column(Col_LeftFlank + PosXAdd, relPosLeft, out largestWidth, -1f);

            column(Col_RightFlank + PosXAdd, relPosRight, out largestWidth, 0f);

            void column(int colindex, Vector2 _relPos, out float largestWidth, float centerPan)
            {
                largestWidth = 0;
                Vector2 cellSize;
                grid[colindex, 0].nextPlacement(centerWp, army.rotation, _relPos, centerPan, army.armyColumnWidth, out cellSize, true);

                for (int row = 1; row < RowsCount; row++)
                {
                    grid[colindex, row].nextPlacement(centerWp, army.rotation, _relPos, centerPan, army.armyColumnWidth, out cellSize, false);
                    largestWidth = lib.LargestValue(largestWidth, cellSize.X);

                    _relPos.Y += cellSize.Y;
                }

                comumnWidth[colindex] = largestWidth;
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

        public void nextPlacement(Vector2 centerWp, Rotation1D armyRotation, Vector2 relativePosition, float centerPan, int armyColumnWidth, out Vector2 cellSize, bool frontRow)
        {
            if (groups.Count > 0)
            {
                //Debug.Log($"####place grid x{x} y{y}");

                if (centerPan < 0)
                {
                    lib.DoNothing();
                }
                int cols = Bound.Min(lib.SmallestValue(groups.Count, armyColumnWidth), 1);
                int rows = Bound.Min( (int)Math.Ceiling(groups.Count / (double)cols), 1);

                cellSize = new Vector2(cols * DssVar.SoldierGroup_Spacing, rows * DssVar.SoldierGroup_Spacing);

                //Adjust center
                //this.relativePosition.X += cellSize.X * centerPan;

                Vector2 topleft = relativePosition;
                topleft.X += cellSize.X * centerPan + DssVar.SoldierGroup_Spacing * 0.5f;//cellSize.X * 0.5f + DssVar.SoldierGroup_Spacing * 0.5f;

                if (frontRow)
                {
                    topleft.Y -= cellSize.Y;
                }

                int colX = 0;
                int rowY = 0;
                foreach (SoldierGroup group in groups)
                {
                    Vector2 localPos = new Vector2(
                        topleft.X + colX * DssVar.SoldierGroup_Spacing,
                        topleft.Y + rowY * DssVar.SoldierGroup_Spacing);

                    if (localPos.Y < 0.38f)
                    {
                        lib.DoNothing();
                    }
                    localPos = lib.RotatePointAroundCenter(Vector2.Zero, localPos,  armyRotation.radians);
                    group.setArmyPlacement2(VectorExt.V2toV3XZ(localPos + centerWp));

                   // Debug.Log($"cell colx{colX} rowy{rowY}");
                   // Debug.Log($"localPos {localPos}, wp {group.goalWp}");

                    if (++colX >= cols)
                    {
                        colX = 0;
                        rowY++;
                    }
                }
            }
            else
            {
                cellSize = new Vector2(DssVar.SoldierGroup_Spacing);
            }
        }
    }
}
