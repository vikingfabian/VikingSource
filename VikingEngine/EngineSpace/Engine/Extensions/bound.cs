using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine
{
    static class Bound
    {
        public static bool IsWithin(int value, int min, int max)
        {
            return value >= min && value <= max;
        }
        public static bool IsWithin(float value, float min, float max)
        {
            return value >= min && value <= max;
        }
        public static bool IsWithin(float value, IntervalF range)
        {
            return value >= range.Min && value <= range.Max;
        }
        public static float Set(float value, IntervalF range)
        {
            return Set(value, range.Min, range.Max);
        }
        public static float Set(float value, float min, float max)
        {
            if (value < min) { value = min; }
            else if (value > max) { value = max; }
            return value;
        }

        public static void Set(ref float value, float min, float max)
        {
            if (value < min) { value = min; }
            else if (value > max) { value = max; }
        }

        public static void Set(ref int value, int min, int max)
        {
            if (value < min) { value = min; }
            else if (value > max) { value = max; }
        }

        public static byte Byte(int value)
        {
            if (value < byte.MinValue)
                return byte.MinValue;
            if (value > byte.MaxValue)
                return byte.MaxValue;

            return (byte)value;
        }

        public static byte Byte_OutIsMaxVal(int value)
        {
            if (value < byte.MinValue || value > byte.MaxValue)
                return byte.MaxValue;

            return (byte)value;
        }

        public static int Min(int value, int min)
        {
            if (value < min) { return min; }
            return value;
        }

        public static void Min(ref int value, int min)
        {
            if (value < min)
            {
                value = min;
            }
        }

        public static int Max(int value, int max)
        {
            if (value > max) { return max; }
            return value;
        }
        public static float Max(float value, float max)
        {
            if (value > max) { return max; }
            return value;
        }

        public static void Max(ref float value, float max)
        {
            if (value > max)
            {
                value = max;
            }
        }

        public static void Max(ref int value, int max)
        {
            if (value > max)
            {
                value = max;
            }
        }

        public static float Min(float value, float min)
        {
            if (value < min) { return min; }
            return value;
        }
        public static float MinAbs(float value, float min)
        {
            if (Math.Abs(value) < Math.Abs(min))
            {
                if (value >= 0)
                {
                    return Math.Abs(min);
                }
                else
                {
                    return -Math.Abs(min);
                }
            }
            return value;
        }

        public static float MaxAbs(float value, float max)
        {
            if (Math.Abs(value) > Math.Abs(max))
                return Math.Abs(max) * (value > 0 ? 1 : -1);
            return value;
        }
        public static int MaxAbs(int value, int max)
        {
            if (Math.Abs(value) > Math.Abs(max))
                return Math.Abs(max) * (value > 0 ? 1 : -1);
            return value;
        }        
        public static int Set(int value, Range bounds)
        {
            return Set(value, bounds.Min, bounds.Max);
        }
        public static int Set(int value, int lowerBound, int upperBound)
        {
            if (value < lowerBound) { value = lowerBound; }
            else if (value > upperBound) { value = upperBound; }
            return value;
        }

        public static int ExMax(int index, int exMax)
        {
            if (index < 0) { return 0; }
            else if (index >= exMax) { return exMax - 1; }
            return index;
        }

        public static IntVector2 Set(IntVector2 value, IntVector2 min, IntVector2 max)
        {
            if (value.X < min.X) { value.X = min.X; }
            else if (value.X > max.X) { value.X = max.X; }

            if (value.Y < min.Y) { value.Y = min.Y; }
            else if (value.Y > max.Y) { value.Y = max.Y; }

            return value;
        }
        public static double SetRollover(double value, double min, double max)
        {
            if (value < min)
            {
                value = value - min + 1 + max;
            }
            else if (value > max)
            {
                value = value - max - 1 + min;
            }
            return value;
        }
        public static double SetRollover(double value, IntervalF bounds)
        {
            if (value < bounds.Min)
            {
                value = value - bounds.Min + 1 + bounds.Max;
            }
            else if (value > bounds.Max)
            {
                value = value - bounds.Max + bounds.Min;
            }
            return value;
        }
        public static float SetRollover(float value, float min, float max)
        {
            if (value < min)
            {
                value = value - min + 1 + max;
            }
            else if (value > max)
            {
                value = value - max - 1 + min;
            }
            return value;
        }
        public static float SetRollover(float value, IntervalF bounds)
        {
            if (value < bounds.Min)
            {
                value = value - bounds.Min + 1 + bounds.Max;
            }
            else if (value > bounds.Max)
            {
                value = value - bounds.Max + bounds.Min;
            }
            return value;
        }
        public static int SetRollover(int value, int min, int max)
        {
            if (value < min)
            {
                value = value - min + 1 + max;
            }
            else if (value > max)
            {
                value = value - max - 1 + min;
            }
            return value;
        }
        public static int SetRollover(int value, int max)
        {
            if (value > max)
            {
                value -= max + 1;
            }
            return value;
        }
    }
}
