using System;
using Microsoft.Xna.Framework;

namespace VikingEngine
{
    struct IntervalIntV3 : IBinaryIOobj
    {
        public IntVector3 Min;
        public IntVector3 Max;
        public static readonly IntervalIntV3 Zero = new IntervalIntV3(IntVector3.Zero, IntVector3.Zero);

        public IntervalIntV3(IntVector3 minAndMax)
        {
            Min = minAndMax; Max = minAndMax;
        }
        public IntervalIntV3(IntVector3 min, IntVector3 max)
        {
            Min = min; Max = max;
        }
        public IntervalIntV3(IntVector3 center, int radius)
        {
            Min = center - radius; Max = center + radius;
        }
        public IntVector3 Add
        {
            get
            {
                IntVector3 result = Max;
                result.X -= Min.X;
                result.Y -= Min.Y;
                result.Z -= Min.Z;
                return result;
            }
            set
            {
                Max = Min + value;
            }
        }

        public IntVector3 Size
        {
            get
            {
                return Max - Min + 1;
            }
            set
            {
                Max = Min + value - 1;
            }
        }

        public void AddRadius(int radius)
        {
            Min -= radius;
            Max += radius;
        }

        public int AddX
        {
            get
            {
                return Max.X - Min.X;

            }
            set
            {
                Max.X = Min.X + value;
            }
        }
        public int AddY
        {
            get
            {
                return Max.Y - Min.Y;

            }
            set
            {
                Max.Y = Min.Y + value;
            }
        }
        public int AddZ
        {
            get
            {
                return Max.Z - Min.Z;

            }
            set
            {
                Max.Z = Min.Z + value;
            }
        }

        public void AddValue(IntVector3 add)
        {
            Min.Add(add);
            Max.Add(add);
        }

        public Vector3 Center
        {
            get
            {
                return Min.Vec + Add.Vec * PublicConstants.Half;
            }
        }
        public IntVector3 keepValueInMyBounds(IntVector3 value, bool rollOver)
        {
            if (rollOver)
            {
                value.X = Bound.SetRollover(value.X, Min.X, Max.X);
                value.Y = Bound.SetRollover(value.Y, Min.Y, Max.Y);
                value.Z = Bound.SetRollover(value.Z, Min.Z, Max.Z);
            }
            else
            {
                if (value.X < Min.X) { value.X = Min.X; }
                else if (value.X > Max.X) { value.X = Max.X; }

                if (value.Y < Min.Y) { value.Y = Min.Y; }
                else if (value.Y > Max.Y) { value.Y = Max.Y; }

                if (value.Z < Min.Z) { value.Z = Min.Z; }
                else if (value.Z > Max.Z) { value.Z = Max.Z; }
            }
            return value;
        }

        public IntervalIntV3 keepValueInMyBounds(IntervalIntV3 value)
        {
            value.Min = keepValueInMyBounds(value.Min, false);
            value.Max = keepValueInMyBounds(value.Max, false);

            return value;
        }

        public IntervalIntV3(Range x, Range y, Range z)
        {
            Min = new IntVector3(x.Min, y.Min, z.Min);
            Max = new IntVector3(x.Max, y.Max, z.Max);
        }

        /// <summary>
        /// Make sure the max is the larger value to min
        /// </summary>
        public void UpdateValue()
        {
            if (Max.X < Min.X)
            {
                int store = Max.X;
                Max.X = Min.X;
                Min.X = store;
            }
            if (Max.Y < Min.Y)
            {
                int store = Max.Y;
                Max.Y = Min.Y;
                Min.Y = store;
            }
            if (Max.Z < Min.Z)
            {
                int store = Max.Z;
                Max.Z = Min.Z;
                Min.Z = store;
            }
        }

        public bool pointInBounds(IntVector3 point)
        {
            if (Min.X <= point.X && Max.X >= point.X)
            {
                if (Min.Y <= point.Y && Max.Y >= point.Y)
                {
                    if (Min.Z <= point.Z && Max.Z >= point.Z)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static IntervalIntV3 FromTwoPoints(IntVector3 point1, IntVector3 point2)
        {
            IntervalIntV3 result = IntervalIntV3.Zero;
            if (point1.X < point2.X)
            {
                result.Min.X = point1.X;
                result.Max.X = point2.X;
            }
            else
            {
                result.Min.X = point2.X;
                result.Max.X = point1.X;
            }
            if (point1.Y < point2.Y)
            {
                result.Min.Y = point1.Y;
                result.Max.Y = point2.Y;
            }
            else
            {
                result.Min.Y = point2.Y;
                result.Max.Y = point1.Y;
            }
            if (point1.Z < point2.Z)
            {
                result.Min.Z = point1.Z;
                result.Max.Z = point2.Z;
            }
            else
            {
                result.Min.Z = point2.Z;
                result.Max.Z = point1.Z;
            }

            return result;
        }

        public void NetworkWriteByte(System.IO.BinaryWriter w)
        {
            Min.NetworkWriteByte(w);
            Max.NetworkWriteByte(w);
        }
        public void write(System.IO.BinaryWriter w)
        {
            Min.write(w);
            Max.write(w);
        }
        public void read(System.IO.BinaryReader r)
        {
            Min.read(r);
            Max.read(r);
        }

        public static bool operator ==(IntervalIntV3 value1, IntervalIntV3 value2)
        {
            return value1.Min == value2.Min && value1.Max == value2.Max;
        }
        public override bool Equals(object obj)
        {
            if (obj is IntervalIntV3)
            {
                return this == (IntervalIntV3)obj;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return (17 * Min.GetHashCode()) ^ (13 * Max.GetHashCode());
        }
        public static bool operator !=(IntervalIntV3 value1, IntervalIntV3 value2)
        {
            return value1.Min != value2.Min || value1.Max != value2.Max;
        }

        public static IntervalIntV3 FromStream(System.IO.BinaryReader r)
        {
            IntervalIntV3 result = IntervalIntV3.Zero;
            result.read(r);
            return result;
        }

        public static IntervalIntV3 NetworkReadByte(System.IO.BinaryReader r)
        {
            return new IntervalIntV3(IntVector3.FromByteSzStream(r), IntVector3.FromByteSzStream(r));
        }

        public override string ToString()
        {
            return "Min" + Min.ToString() + ", Max" + Max.ToString();
        }
    }
    struct ColorRange
    {
        public Vector3 Min;
        public Vector3 Add;

        public ColorRange(Color min, Color max)
        {
            this.Min = min.ToVector3();
            this.Add = max.ToVector3() - Min;
        }

        public Color MinCol
        {
            get { return new Color(Min); }
        }
        public Color MaxCol
        {
            get { return new Color(Min + Add); }
        }


        public void Brighter(int add)
        {
            Min.X += add;
            Min.Y += add;
            Min.Z += add;
            Add.X += add;
            Add.Y += add;
            Add.Z += add;
        }

        public static ColorRange FromRadius(Color center, byte radius)
        {
            ColorRange result = new ColorRange();
            result.Min = center.ToVector3() - new Vector3(radius);
            result.Add = new Vector3(radius * PublicConstants.Twice);

            return result;
        }
        public static ColorRange FromPercentRadius(Color center, float radius)
        {
            return FromRadius(center, (byte)(radius * byte.MaxValue));
        }
        /// <summary>
        /// Checks if the min+add might break the byte range
        /// </summary>
        /// <returns>the safe add value</returns>
        static byte safeCheck(byte min, byte add)
        {
            if (min + add > byte.MaxValue)
            {
                return (byte)(byte.MaxValue - min);
            }
            return add;
        }
        public Color GetRandom()
        {
            return new Color(
                Min.X + Ref.rnd.Float(Add.X),
                Min.Y + Ref.rnd.Float(Add.Y),
                Min.Z + Ref.rnd.Float(Add.Z));
        }

        public Color GetRandomPercentPos()
        {
            return PercentPos((float)Ref.rnd.Double());
        }

        public Color PercentPos(float percent)
        {
            return new Color(
                Min.X + Add.X * percent,
                Min.Y + Add.Y * percent,
                Min.Z + Add.Z * percent);

        }

        public override string ToString()
        {
            return "Min: " + MinCol.ToString() + " - Max: " + MaxCol.ToString();
        }
    }
}
