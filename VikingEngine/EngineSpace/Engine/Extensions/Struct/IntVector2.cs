using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace VikingEngine
{
    public struct IntVector2 : IBinaryIOobj, IComparable
    {
        public static readonly IntVector2[] Dir4Array = new IntVector2[] {
            new IntVector2(0, -1),
            new IntVector2(1, 0),
            new IntVector2(0, 1),
            new IntVector2(-1, 0) };

        public static readonly IntVector2[] AllDiagonalsArray = new IntVector2[] {
            new IntVector2(1, -1),
            new IntVector2(1, 1),
            new IntVector2(-1, 1),
            new IntVector2(-1, -1) };

        public static readonly IntVector2[] Dir8Array = new IntVector2[] {
            new IntVector2(0, -1),new IntVector2(1, -1),
            new IntVector2(1, 0), new IntVector2(1, 1),
            new IntVector2(0, 1), new IntVector2(-1, 1),
            new IntVector2(-1, 0),new IntVector2(-1, -1),  };

        public int X;
        public int Y;

        public IntVector2(int unit)
        { X = unit; Y = unit; }
        public IntVector2(Vector2 v2)
        { X = Convert.ToInt32(v2.X); Y = Convert.ToInt32(v2.Y); }
        public IntVector2(int x, int y)
        { X = x; Y = y; }
        public IntVector2(float x, float y)
        { X = Convert.ToInt32(x); Y = Convert.ToInt32(y); }

        public int GetDim(Dimensions dim)
        {
            if (dim == Dimensions.X)
                return X;
            else if (dim == Dimensions.Y)
                return Y;
            return 0;
        }

        public int GetDim(Dir4 dir)
        {
            return GetDim(lib.Dir4ToDimensionsXY(dir));
        }

        public void MaxDim(Dimensions dim, int setIfBigger)
        {
            if (dim == Dimensions.X)
                X = Math.Max(X, setIfBigger);
            else
                Y = Math.Max(Y, setIfBigger);
        }

        public void MinDim(Dimensions dim, int setIfSmaller)
        {
            if (dim == Dimensions.X)
                X = Math.Min(X, setIfSmaller);
            else
                Y = Math.Min(Y, setIfSmaller);
        }

        public void MaxDim(Dimensions dim, IntVector2 setIfBigger)
        {
            if (dim == Dimensions.X)
                X = Math.Max(X, setIfBigger.X);
            else
                Y = Math.Max(Y, setIfBigger.Y);
        }

        public void MinDim(Dimensions dim, IntVector2 setIfSmaller)
        {
            if (dim == Dimensions.X)
                X = Math.Min(X, setIfSmaller.X);
            else
                Y = Math.Min(Y, setIfSmaller.Y);
        }

        public void SetDim(Dimensions dim, int value)
        {
            if (dim == Dimensions.X)
                X = value;
            else if (dim == Dimensions.Y)
                Y = value;
        }

        public void SetDim(Dimensions dim, IntVector2 sameDim)
        {
            if (dim == Dimensions.X)
                X = sameDim.X;
            else if (dim == Dimensions.Y)
                Y = sameDim.Y;
        }

        public void SetDim(Dir4 dir, int value)
        {
            SetDim(lib.Dir4ToDimensionsXY(dir), value);
        }

        public void AddDim(Dimensions dim, int value)
        {
            if (dim == Dimensions.X)
                X += value;
            else
                Y += value;
        }

        public void AddDim(Dimensions dim, IntVector2 sameDim)
        {
            if (dim == Dimensions.X)
                X += sameDim.X;
            else
                Y += sameDim.Y;
        }

        public void SubDim(Dimensions dim, int value)
        {
            if (dim == Dimensions.X)
                X -= value;
            else
                Y -= value;
        }

        public void SubDim(Dimensions dim, IntVector2 sameDim)
        {
            if (dim == Dimensions.X)
                X -= sameDim.X;
            else
                Y -= sameDim.Y;
        }

        public void AddDim(Dir4 dir, int value)
        {
            AddDim(lib.Dir4ToDimensionsXY(dir), value);
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);//return Vec.Length();
        }

        public double Length64()
        {
            return Math.Sqrt(X * X + Y * Y);//return Vec.Length();
        }

        public float Length(IntVector2 toPoint)
        {
            int x = toPoint.X - X;
            int y = toPoint.Y - Y;

            return (float)Math.Sqrt(x * x + y * y);
        }

        public float LengthSquared()
        {
            return Vec.LengthSquared();
        }

        public int SideLength()
        {
            return Math.Abs(Math.Abs(X) > Math.Abs(Y) ? X : Y);
        }

        public int SideLength(IntVector2 comparedTo)
        {
            int diffX = Math.Abs(comparedTo.X - X);
            int diffY = Math.Abs(comparedTo.Y - Y);

            return diffX > diffY ? diffX : diffY;
        }

        public int ManhattanDistance()
        {
            return Math.Abs(X) + Math.Abs(Y);
        }

        public IntVector2 Multiply(float multi)
        {
            X = (int)multi * X;
            Y = (int)multi * Y;
            return this;
        }

        /// <summary>
        /// The center pos from the selected side from a box 
        /// </summary>
        public IntVector2 GetMidSide(Dir4 dir)
        {
            switch (dir)
            {
                case Dir4.N:
                    return new IntVector2(X / 2, 0);
                case Dir4.S:
                    return new IntVector2(X / 2, Y - 1);
                case Dir4.W:
                    return new IntVector2(0, Y / 2);
                case Dir4.E:
                    return new IntVector2(X - 1, Y / 2);
            }
            throw new IndexOutOfRangeException();
        }
        public Vector2 Vec
        {
            get
            {
                return new Vector2(X, Y);
            }
            set
            {
                X = (int)value.X;
                Y = (int)value.Y;
            }
        }
        public ShortVector2 ShortVec
        {
            get
            {
                ShortVector2 ret = ShortVector2.Zero;
                ret.X = (short)X;
                ret.Y = (short)Y;
                return ret;

            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public Dir4 MajorDirection
        {
            get
            {
                if (Math.Abs(X) > Math.Abs(Y))
                {
                    if (X < 0)
                        return Dir4.W;
                    else
                        return Dir4.E;
                }
                else
                {
                    if (Y < 0)
                        return Dir4.N;
                    else
                        return Dir4.S;
                }
            }
        }
        /// <summary>
        /// A unit vector along the cardinal direction of greatest amplitude.
        /// </summary>
        public IntVector2 MajorDirectionVec
        {
            get
            {
                if (Math.Abs(X) > Math.Abs(Y))
                {
                    if (X < 0)
                        return new IntVector2(-1, 0);
                    else
                        return new IntVector2(1, 0);
                }
                else
                {
                    if (Y < 0)
                        return new IntVector2(0, -1);
                    else
                        return new IntVector2(0, 1);
                }
            }
        }
        //public System.Drawing.Point Point
        //{
        //    get
        //    {
        //        return new System.Drawing.Point(X, Y);
        //    }
        //}
        public int[] Array
        {
            get
            {
                return new int[2] { X, Y };
            }
            set
            {
                X = value[0];
                Y = value[1];
            }
        }
        public static readonly IntVector2 Zero = new IntVector2(0);
        public static readonly IntVector2 One = new IntVector2(1);
        public static readonly IntVector2 NegativeOne = new IntVector2(-1);
        public static readonly IntVector2 Left = new IntVector2(-1, 0);
        public static readonly IntVector2 Right = new IntVector2(1, 0);
        public static readonly IntVector2 PositiveY = new IntVector2(0, 1);
        public static readonly IntVector2 NegativeY = new IntVector2(0, -1);
        public static readonly IntVector2 MinValue = new IntVector2(int.MinValue, int.MinValue);

        public IntVector2 SwapXY()
        {
            return new IntVector2(Y, X);
        }

        public IntVector2 Normal()
        {
            int l = SideLength();
            return new IntVector2(X / l, Y / l);
        }
        public IntVector2 Normal_RoundUp()
        {
            float l = SideLength();
            return new IntVector2(X / l, Y / l);
        }
        
        /// <summary>
        /// Using geometric similarity
        /// </summary>
        public Vector2 SimilarVectorFromWidth(float width)
        {
            return new Vector2(width, width / X * Y);
        }

        /// <summary>
        /// Using geometric similarity
        /// </summary>
        public Vector2 SimilarVectorFromHeight(float height)
        {
            return new Vector2(height / Y * X, height);
        }

        public bool IsZero()
        {
            return X == 0 && Y == 0;
        }
        public bool HasValue()
        {
            return X != 0 || Y != 0;
        }

        public bool IsOrthogonal()
        {
            return (X != 0 && Y == 0) ||
                (Y != 0 && X == 0);
        }
        public bool IsDiagonal()
        {
            return X != 0 && Math.Abs(X) == Math.Abs(Y);
        }

        public bool IsOrthogonalOrDiagonal()
        {
            return IsOrthogonal() || IsDiagonal();
        }

            /// <summary>
            /// Inverts the position around a chosen centerpoint
            /// </summary>
            public IntVector2 mirrorTilePos(IntVector2 center)
        {
            return new IntVector2(center.X * 2 - X, center.Y * 2 - Y);
        }

        public static IntVector2 FromVec2(Vector2 value)
        {
            return new IntVector2((int)value.X, (int)value.Y);
        }

        public static IntVector2 FromDir4(int dir)
        {
            return Dir4Array[dir];
        }
        public static IntVector2 FromDir4(Dir4 dir)
        {
            return Dir4Array[(int)dir];
        }
        public static IntVector2 FromDir8(Dir8 dir)
        {
            return Dir8Array[(int)dir];
        }
       
        /// </summary>
        public void Add(int add)
        {
            X += add;
            Y += add;
        }

        public void Add(int addX, int addY)
        {
            X += addX;
            Y += addY;
        }

        public void Add(IntVector2 add)
        {
            X += add.X;
            Y += add.Y;
        }
        public void Add(ShortVector2 add)
        {
            X += add.X;
            Y += add.Y;
        }
        public void Sub(int sub)
        {
            X -= sub;
            Y -= sub;
        }
        public void Sub(IntVector2 sub)
        {
            X -= sub.X;
            Y -= sub.Y;
        }
        public void Sub(ShortVector2 sub)
        {
            X -= sub.X;
            Y -= sub.Y;
        }
        public void Square(int side)
        { X = side; Y = side; }
        public void set(int x, int y)
        { X = x; Y = y; }

        public string Script
        {
            get
            {
                return Convert.ToString(X) + SaveData.Dimension + Convert.ToString(Y);
            }
            set
            {
                if (value == TextLib.EmptyString)
                { X = 0; Y = 0; }
                else
                {
                    List<float> array = lib.StingDimentions(value);
                    X = (int)array[0];
                    Y = (int)array[1];
                }
            }
        }

        public static IntVector2 RotateVector(IntVector2 value, IntVector2 forward_negY)
        {
            IntVector2 result = value;
            if (forward_negY.X < 0)
            {//left
                result.X = value.Y;
                result.Y = -value.X;
            }
            else if (forward_negY.X > 0)
            {//right
                result.X = -value.Y;
                result.Y = value.X;
            }
            else if (forward_negY.Y > 0)
            {//down
                result.X = -value.X;
                result.Y = -value.Y;
            }

            return result;
        }

        public static IntVector2 RotateVector_D4(IntVector2 value, int steps)
        {
            if (steps < 0)
                steps += 4;
            else if (steps >= 4)
                steps -= 4;

            IntVector2 result = value;
            switch (steps)
            {
                case 1:
                    result.X = -value.Y;
                    result.Y = value.X;
                    break;
                case 2:
                    result.X = -value.X;
                    result.Y = -value.Y;
                    break;
                case 3:
                    result.X = value.Y;
                    result.Y = -value.X;
                    break;

            }
            return result;
        }


        public override string ToString()
        {
            return "{" + X.ToString() + "," + Y.ToString() + "}";
        }
        public string ToString(string divideChar)
        {
            return X.ToString() + divideChar + Y.ToString();
        }
        public string ToXYString()
        {
            return "X:" + X.ToString() + " Y:" + Y.ToString();
        }

        public static IntVector2 operator /(IntVector2 value1, int value2)
        {
            value1.X /= value2;
            value1.Y /= value2;
            return value1;
        }
        public static IntVector2 operator %(IntVector2 value1, int value2)
        {
            value1.X %= value2;
            value1.Y %= value2;
            return value1;
        }
        public static IntVector2 operator *(IntVector2 lhs, IntVector2 rhs)
        {
            lhs.X *= rhs.X;
            lhs.Y *= rhs.Y;
            return lhs;
        }
        public static IntVector2 operator *(IntVector2 value1, int value2)
        {
            value1.X *= value2;
            value1.Y *= value2;
            return value1;
        }
        public static IntVector2 operator *(IntVector2 value1, float value2)
        {
            value1.X *= (int)value2;
            value1.Y *= (int)value2;
            return value1;
        }
        public static IntVector2 operator +(IntVector2 value1, int value2)
        {
            value1.X += value2;
            value1.Y += value2;
            return value1;
        }
        public static IntVector2 operator -(IntVector2 value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }
        public static IntVector2 operator -(IntVector2 value1, int value2)
        {
            value1.X -= value2;
            value1.Y -= value2;
            return value1;
        }
        public static IntVector2 operator +(IntVector2 value1, IntVector2 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }
        public static IntVector2 operator -(IntVector2 value1, IntVector2 value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }
#if PCGAME
        public System.Drawing.Point Point
        {
            get
            {
                return new System.Drawing.Point(this.X, this.Y);
            }
        }
#endif

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(int x, int y)
        {
            return x == this.X && y == this.Y;
        }

        public override bool Equals(object obj)
        {
            IntVector2 vec = (IntVector2)obj;

            return vec.X == this.X && vec.Y == this.Y;
        }
        public static bool operator ==(IntVector2 value1, IntVector2 value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }
        public static bool operator !=(IntVector2 value1, IntVector2 value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }

        public int Area()
        { return X * Y; }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(X); w.Write(Y);
        }
        public void read(System.IO.BinaryReader r)
        {
            X = r.ReadInt32();
            Y = r.ReadInt32();
        }

        public void writeByte(System.IO.BinaryWriter w)
        {
            w.Write((byte)X); w.Write((byte)Y);
        }

        public void readByte(System.IO.BinaryReader r)
        {
            X = r.ReadByte(); Y = r.ReadByte();
        }

        public void writeUshort(System.IO.BinaryWriter w)
        {
            w.Write((ushort)X); w.Write((ushort)Y);
        }

        public void readUshort(System.IO.BinaryReader r)
        {
            X = r.ReadUInt16(); Y = r.ReadUInt16();
        }

        public void writeShort(System.IO.BinaryWriter w)
        {
            w.Write((short)X); w.Write((short)Y);
        }

        public void readShort(System.IO.BinaryReader r)
        {
            X = r.ReadInt16(); Y = r.ReadInt16();
        }


        public void writeSByte(System.IO.BinaryWriter w)
        {
            w.Write((sbyte)X); w.Write((sbyte)Y);
        }
        public void readSByte(System.IO.BinaryReader r)
        {
            X = r.ReadSByte(); Y = r.ReadSByte();
        }

        public static IntVector2 FromRead(System.IO.BinaryReader r)
        {
            IntVector2 result = IntVector2.Zero;
            result.read(r);
            return result;
        }

        public static IntVector2 FromReadByte(System.IO.BinaryReader r)
        {
            IntVector2 result = IntVector2.Zero;
            result.readByte(r);
            return result;
        }

        public int CompareTo(object obj)
        {
            IntVector2 other = (IntVector2)obj;

            if (X == other.X)
            {
                if (Y == other.Y)
                {
                    return 0;
                }
                else if (Y < other.Y)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else if (X < other.X)
            {
                return -2;
            }
            else
            {
                return 2;
            }
        }

        public void Grindex_Next(int rowCount)
        {
            if (++X >= rowCount)
            {
                X = 0;
                ++Y;
            }

        }

        public void Grindex_NewRow()
        {
            if (X > 0)
            {
                X = 0;
                ++Y;
            }

        }
    }
    
}
