using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    /// <summary>
    /// Used for obiect culling out side an area
    /// </summary>
    struct Intvector2MinMax
    {
        public IntVector2 min, max;

        public Intvector2MinMax(IntVector2 center) 
        {
            min = center;
            max = center;
        }

        public Intvector2MinMax(IntVector2 min, IntVector2 max)
        {
           this.min = min;
           this.max = max;
        }

        public void Combine(Intvector2MinMax other)
        {
            Next(ref other.min);
            Next(ref other.max);
        }

        public void Next(ref IntVector2 pos)
        {
            if (pos.X < min.X)
            {
                min.X = pos.X;
            }
            if (pos.X > max.X)
            {
                max.X = pos.X;
            }

            if (pos.Y < min.Y)
            {
                min.Y = pos.Y;
            }
            if (pos.Y > max.Y)
            {
                max.Y = pos.Y;
            }
        }
    }
}
