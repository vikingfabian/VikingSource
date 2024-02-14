using System;
using Microsoft.Xna.Framework;

namespace VikingEngine
{
    //struct RangeIntV2
    //{
    //    public IntVector2 Min;
    //    public IntVector2 Max;

    //    public RangeIntV2(IntVector2 min, IntVector2 max)
    //    {
    //        this.Min = min;
    //        this.Max = max;
    //    }

    //    public void Update()
    //    {
    //        IntVector2 min = new IntVector2(
    //            lib.SmallestValue(Min.X, Max.X),
    //            lib.SmallestValue(Min.Y, Max.Y));
    //        IntVector2 max = new IntVector2(
    //            lib.LargestValue(Min.X, Max.X),
    //            lib.LargestValue(Min.Y, Max.Y));

    //        Min = min;
    //        Max = max;
    //    }

    //    public bool Interect(IntVector2 point)
    //    {
    //        return point.X >= Min.X && point.X <= Max.X &&
    //            point.Y >= Min.Y && point.Y <= Max.Y;
    //    }

    //    public IntVector2 Add
    //    {
    //        get
    //        {
    //            return Max - Min;
    //        }
    //    }

    //    public override string ToString()
    //    {
    //        return "Range Min" + Min.ToString() + " Max" + Max.ToString();
    //    }
    //}
    
    
    public struct Range
    {
        public static readonly Range Zero = new Range();

        public int Min;
        public int Max;

        public Range(int range)
        { Min = -range; Max = range; }
        public Range(int min, int max)
        { Min = min; Max = max; }

        public int GetRandom()
        {
            return Ref.rnd.Interval(Min, Max);
        }
        public int GetRandom(PcgRandom rnd)
        {
            return rnd.Interval(Min, Max);
        }
        public static Range FromRadius(int center, int radius)
        {
            return new Range(center - radius, center + radius);
        }
        public int Difference
        {
            get { return Max - Min; }
        }
        public bool IsWithinRange(int value)
        {
            if (value < Min) { return false; }
            else if (value > Max) { return false; }
            return true;
        }
        public void Add(int add)
        {
            Min += add;
            Max += add;
        }

        public void WriteShort(System.IO.BinaryWriter w)
        {
            w.Write((short)Min);
            w.Write((short)Max);
        }
        public void ReadShort(System.IO.BinaryReader r)
        {
            Min = r.ReadInt16();
            Max = r.ReadInt16();
        }
        public static Range FromShortStream(System.IO.BinaryReader r)
        {
            Range result = new Range();
            result.ReadShort(r);
            return result;
        }
        public static Range operator +(Range value1, int value2)
        {
            value1.Min += value2;
            value1.Max += value2;
            return value1;
        }
        public static Range operator -(Range value1, int value2)
        {
            value1.Min -= value2;
            value1.Max -= value2;
            return value1;
        }

        public int GetFromPercent(float percent)
        {
            int result = Convert.ToInt32(Min + (Max - Min) * percent);
            return result;
        }

        public int SetBounds(int value)
        {
            if (value < Min) { value = Min; }
            else if (value > Max) { value = Max; }
            return value;
        }
        public override string ToString()
        {
            return "Range { "+ Min.ToString() +" to " + Max.ToString() + " }";
        }

        public string ToString(string seperator)
        {
            return Min.ToString() + seperator + Max.ToString();
        }

        public bool IsEdge(int value)
        {
            return value == Min || value == Max;
        }
    }


    

}
