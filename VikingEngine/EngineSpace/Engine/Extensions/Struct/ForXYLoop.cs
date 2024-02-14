using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    struct ForXYLoop
    {
        IntVector2 min;
        IntVector2 max;
        IntVector2 nextPos;
        public IntVector2 Position;

        bool done;
        public bool Done
        { get { return done; } }

        public ForXYLoop(Rectangle2 area)
            : this(area.pos, area.BottomRight - 1)
        { }
        public ForXYLoop(IntVector2 size)
            : this(IntVector2.Zero, size - 1)
        { }
        public ForXYLoop(IntVector2 min, IntVector2 max)
            : this()
        {
            this.min = min;
            this.max = max;
            
            Reset();
        }

        public void Reset()
        {
            done = false;
            nextPos = min;
        }

        public IntVector2 Next_Old()
        {
            IntVector2 result = nextPos;
            nextPos.X++;
            if (nextPos.X > max.X)
            {
                nextPos.X = min.X;
                nextPos.Y++;
                if (nextPos.Y > max.Y)
                    done = true;
            }

            return result;
        }
        /// <summary>
        /// run as while(Next())
        /// </summary>
        public bool Next()
        {
            bool result = !done;
            Position = nextPos;
            nextPos.X++;
            if (nextPos.X > max.X)
            {
                nextPos.X = min.X;
                nextPos.Y++;
                if (nextPos.Y > max.Y)
                    done = true;
            }

            return result;
        }

        public bool AtRight
        {
            get { return Position.X == max.X; }
        }
        public bool AtBottom
        {
            get { return Position.Y == max.Y; }
        }
        public bool AtEdge
        {
            get
            {
                return Position.X == min.X || Position.X == max.X ||
                    Position.Y == min.Y || Position.Y == max.Y;
            }
        }
    }
}
