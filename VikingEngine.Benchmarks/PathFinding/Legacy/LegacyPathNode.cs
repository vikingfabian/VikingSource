using System;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.Benchmarks.Pathfinding.Legacy
{
    struct LegacyPathNode
    {
        const float MoveCostStraight = 10f;
        const float MoveCostDiagonal = 14f;

        public static readonly LegacyPathNode Empty = new LegacyPathNode();

        public float Value;

        /// <summary>
        /// Distance to goal
        /// </summary>
        public float Heuristic;
        float moveCost;

        public IntVector2 Position;
        public IntVector2 PreviousPosition;

        public bool HasValue;
        public bool closed;
        public bool waterTile;
        public bool ship;

        int dir8;

        public LegacyPathNode(IntVector2 pos, int dir8, bool ship)
        {
            this.Position = pos;
            this.dir8 = dir8;
            this.ship = ship;
            HasValue = true;
            closed = true;

            moveCost = 0;
            Value = 0;
            PreviousPosition = pos;
            waterTile = ship;
        }

        public LegacyPathNode(IntVector2 pos, int dir8, WorldData world, LegacyPathNode parent, IntVector2 goalPos, bool endAsShip)
        {
            this.Position = pos;
            this.dir8 = dir8;
            this.PreviousPosition = parent.Position;
            closed = false;

            moveCost = lib.IsEven(dir8) ? MoveCostStraight : MoveCostDiagonal;
            if (dir8 == parent.dir8)
            { //Bonus for keeping direction
                moveCost -= 1f;
            }

            Tile tile = world.tileGrid.Get(pos);
            waterTile = tile.IsWater();

            if (waterTile != parent.waterTile)
            {
                if (waterTile == endAsShip)
                {//wanted convert
                    moveCost -= 2;
                }
                else
                {
                    moveCost += MoveCostStraight * 16;
                }
            }
            ship = this.waterTile;

            moveCost *= tile.TroupWalkingDistance(ship);

            moveCost += parent.moveCost;

            //Value = moveCost + (Math.Abs(pos.X - goalPos.X) + Math.Abs(pos.Y - goalPos.Y)) * MoveCostStraight;
            // Octile distance formula: 
            // 10 * (dx + dy) + (14 - 2 * 10) * min(dx, dy)
            //goalPos.Length()
            //int dx = Math.Abs(pos.X - goalPos.X);
            //int dy = Math.Abs(pos.Y - goalPos.Y);
            //Heuristic = (MoveCostStraight * (dx + dy)) + ((MoveCostDiagonal - 2 * MoveCostStraight) * Math.Min(dx, dy));
            Heuristic = (pos - goalPos).Length() * MoveCostStraight;

            const float DistanceToGoalWeight = 1.5f;
            Heuristic *= DistanceToGoalWeight;
            this.Value = moveCost + Heuristic;

            HasValue = true;
        }
    }
}
