using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.BlackBolts.Map
{
    struct MapPlacement
    {
        public float groundY;
        public IntVector2 tilePos;
        public Dir4 direction;

        public MapPlacement(IntVector2 tilePos, Dir4 direction)
        {
            this.tilePos = tilePos;
            this.direction = direction;
        }

        public void refreshGroundY()
        {
            groundY = BlackRef.mapData.GetTile(tilePos).groundY();
        }

        public MapPlacement ForwardPos()
        {
            return new MapPlacement() { tilePos = tilePos + IntVector2.FromDir4(direction), direction = direction };
        }
        public MapPlacement LeftPos()
        {
            Dir4 leftDir = lib.SumFacingAngles(direction, Dir4.W);
            return new MapPlacement() { tilePos = tilePos + IntVector2.FromDir4(leftDir), direction = leftDir };
        }
        public MapPlacement RightPos()
        {
            Dir4 rightDir = lib.SumFacingAngles(direction, Dir4.E);
            return new MapPlacement() { tilePos = tilePos + IntVector2.FromDir4(rightDir), direction = rightDir };
        }

        public MapPlacement TurnAroundPos()
        {
            MapPlacement result = this;
            result.FlipDir();
            return result;
        }
        public void FlipDir()
        {
            direction = lib.Rotate(direction, 2);
        }

        public void Rotate(int dir)
        {
            direction = lib.Rotate(direction, dir);
        }

        public bool Equals(MapPlacement other)
        {
            return tilePos.Equals(other.tilePos) && direction == other.direction;
        }

        public override bool Equals(object obj)
        {
            return obj is MapPlacement other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(tilePos, direction);
        }

        public static bool operator ==(MapPlacement left, MapPlacement right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MapPlacement left, MapPlacement right)
        {
            return !left.Equals(right);
        }
    }
}
