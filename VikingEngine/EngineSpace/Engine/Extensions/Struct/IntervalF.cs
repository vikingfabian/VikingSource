using System;
using Microsoft.Xna.Framework;

namespace VikingEngine
{
    public struct IntervalF
    {
        public static readonly IntervalF Zero = new IntervalF(0, 0);

        public float Min;
        public float Max;

        public float Length
        {
            get { return Max - Min; }
        }

        public IntervalF(float range)
        {
            Min = -range; Max = range;
        }

        public static IntervalF FromPercentDiff(float center, float percentDiff)
        {
            return new IntervalF(center * (1 - percentDiff), center * (1 + percentDiff));
        }

        public static IntervalF FromCenter(float center, float range)
        {
            return new IntervalF(center - range, center + range);
        }

        public static IntervalF NoInterval(float value)
        {
            return new IntervalF(value, value);
        }

        public IntervalF(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float RandomGaussianPoint(PcgRandom prng)
        {
            return MathHelper.Clamp(prng.GaussianDistribution(Min + (Max - Min) / 2, (Max - Min) / 6), Min, Max);
        }

        public bool IsOverlapping(IntervalF other)
        {
            if (Min < other.Min)
            {
                if (other.Min < Max)
                {
                    return true;
                }
            }
            else
            {
                if (Min < other.Max)
                {
                    return true;
                }
            }

            return false;
        }

        public IntervalF FindOverlap(IntervalF other)
        {
            return new IntervalF(Math.Max(Min, other.Min), Math.Min(Max, other.Max));
        }

        public bool IsWithinRange(float value)
        {
            if (Min <= Max)
                return (Min <= value && value <= Max);
            return (Max <= value && value <= Min);
        }
        public float SetBounds(float value)
        {
            if (value < Min) { value = Min; }
            else if (value > Max) { value = Max; }
            return value;
        }
        public float Center
        {
            get { return Min + (Max - Min) * PublicConstants.Half; }
        }

        public float BytePercentPosition(System.IO.BinaryReader r)
        {
            return BytePercentPosition(r.ReadByte());
        }
        public float BytePercentPosition(byte pos)
        {
            return Min + Difference * ((float)pos / byte.MaxValue);
        }

        public byte GetValueBytePercentPos(float value)
        {
            return (byte)(GetValuePercentPos(value) * byte.MaxValue);
        }

        /// <summary>
        /// Remap a value from this interval to another
        /// </summary>
        public void RemapValue(ref float value, IntervalF other)
        {
            value = MathExt.Lerp(other.Min, other.Max, GetValuePercentPos(value));
        }

        /// <summary>
        /// Remap a value from this interval to another
        /// </summary>
        public float RemapValue(float value, IntervalF other)
        {
            return MathExt.Lerp(other.Min, other.Max, GetValuePercentPos(value));
        }

        /// <summary>
        /// Where the value is relative to the range min/max
        /// </summary>
        /// <returns>A percent value 0 to 1</returns>
        public float GetValuePercentPos(float value)
        {
            return (value - Min) / Difference;
        }

        /// <summary>
        /// Where the value is relative to the range min/max
        /// </summary>
        /// <returns>A percent value 0 to 1</returns>
        public float GetValuePercentPosClamped(float value)
        {
            return MathHelper.Clamp((value - Min) / Difference, 0, 1);
        }

        /// <param name="percent">A value between 0 and 1</param>
        /// <returns>a position in the range from percent</returns>
        public float GetFromPercent(float percent)
        {
            return Min + Difference * percent;
        }
        public float GetFromPercent(Percent percent)
        {
            return this.GetFromPercent(percent.Value);
        }
        /// <summary>
        /// Multiplies both the min and max value by the selected factor
        /// </summary>
        public void Multiply(float value)
        {
            Min *= value; Max *= value;
        }

        public static IntervalF operator *(IntervalF value1, float value2)
        {
            value1.Min *= value2;
            value1.Max *= value2;
            return value1;
        }
        public static IntervalF operator +(IntervalF value1, float value2)
        {
            value1.Min += value2;
            value1.Max += value2;
            return value1;
        }
        public float GetRandom()
        {
            return Ref.rnd.Float(Min, Max);
        }

        public float GetRandom(PcgRandom rnd)
        {
            return rnd.Float(Min, Max);
        }

        public float Difference
        {
            get { return Max - Min; }
        }
        public void Add(float add)
        {
            Min += add;
            Max += add;
        }
        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(Min);
            w.Write(Max);
        }
        public void Read(System.IO.BinaryReader r)
        {
            Min = r.ReadSingle();
            Max = r.ReadSingle();
        }
        public static IntervalF FromReader(System.IO.BinaryReader r)
        {
            IntervalF result = new IntervalF();
            result.Read(r);
            return result;
        }

        public override string ToString()
        {
            return "Range { " + Min.ToString() + " to " + Max.ToString() + " }";
        }
    }
}
