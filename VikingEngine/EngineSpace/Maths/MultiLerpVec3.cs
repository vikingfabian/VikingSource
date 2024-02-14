using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.EngineSpace.Maths
{
    class MultiLerpVec3
    {
        List<Vector3> stops;

        public MultiLerpVec3()
        {
            stops = new List<Vector3>();
        }

        public void Add(Vector3 val)
        {
            stops.Add(val);
        }

        public Vector3 GetAt(float t)
        {
            if (t == 0)
            {
                return stops[0];
            }
            else if (t == 1)
            {
                return stops[stops.Count - 1];
            }

            float stopF = t * stops.Count;
            int stop = (int)stopF;
            t = stopF - stop;
            if (stop == stops.Count - 1)
                return stops[stops.Count - 1];
            return (1 - t) * stops[stop] + t * stops[stop + 1];
        }

        public float FindT(Vector3 value)
        {
            for (int i = 0; i < stops.Count - 1; ++i)
            {
                Vector3 low = stops[i];
                Vector3 high = stops[i + 1];

                IntervalF xI = new IntervalF(low.X, high.X);
                IntervalF yI = new IntervalF(low.Y, high.Y);
                IntervalF zI = new IntervalF(low.Z, high.Z);

                if (xI.IsWithinRange(value.X) &&
                    yI.IsWithinRange(value.Y) &&
                    zI.IsWithinRange(value.Z))
                {
                    float t;
                    if (low.X != high.X)
                    {
                        t = xI.GetValuePercentPos(value.X);
                    }
                    else if (low.Y != high.Y)
                    {
                        t = yI.GetValuePercentPos(value.Y);
                    }
                    else
                    {
                        t = zI.GetValuePercentPos(value.Z);
                    }

                    return ((float)i + t) / (float)(stops.Count);
                }
            }

            return 0f;
        }
    }
}
