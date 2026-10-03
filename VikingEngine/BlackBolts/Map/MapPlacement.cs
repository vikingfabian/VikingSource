using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.BlackBolts.Map
{
    struct MapPlacement
    {
        public IntVector2 tilePos;
        public Dir4 direction;

        public MapPlacement(IntVector2 tilePos, Dir4 direction)
        {
            this.tilePos = tilePos;
            this.direction = direction;
        }

        public MapPlacement ForwardPos()
        {
           return new MapPlacement( ) { tilePos = tilePos +  IntVector2.FromDir4(direction), direction = direction };
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
    }
}
