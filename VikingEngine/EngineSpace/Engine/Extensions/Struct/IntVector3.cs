using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace VikingEngine
{
    public struct IntVector3 : IBinaryIOobj
    {
        public int X;
        public int Y;
        public int Z;

        public IntVector3(CubeFace face)
        {
            X = 0;
            Y = 0;
            Z = 0;
            switch (face)
            {
                case CubeFace.Xnegative:
                    X = -1;
                    break;
                case CubeFace.Xpositive:
                    X = 1;
                    break;
                case CubeFace.Ynegative:
                    Y = -1;
                    break;
                case CubeFace.Ypositive:
                    Y = 1;
                    break;
                case CubeFace.Znegative:
                    Z = -1;
                    break;
                case CubeFace.Zpositive:
                    Z = 1;
                    break;
            }
        }
        public IntVector3(int x, int y, int z)
        {
            X = x; Y = y; Z = z;
        }
        public IntVector3(Vector3 roundedDown)
        {
            X = (int)roundedDown.X;
            Y = (int)roundedDown.Y;
            Z = (int)roundedDown.Z;
        }
        public IntVector3(int side)
            : this(side, side, side)
        { }

        public float Length
        {
            get
            {
                return VectorExt.Length(X, Y, Z);
            }
        }

        public IntVector2 XZplaneCoords
        {
            get
            {
                return new IntVector2(X, Z);
            }
            set
            {
                X = value.X;
                Z = value.Y;
            }
        }

        public void AddXZ(IntVector2 add)
        {
            X += add.X;
            Z += add.Y;
        }
        public void AddXZ(IntVector3 add)
        {
            X += add.X;
            Z += add.Z;
        }

        public IntVector2 XZ
        {
            get { return new IntVector2(X, Z); }
            set { X = value.X; Z = value.Y; }

        }

        public bool IsInFrontOf(Dir4 forward, IntVector3 other)
        {
            IntVector3 diff = this - other;
            switch (forward)
            {
                case Dir4.N: return diff.X == 0 && Z <= other.Z;
                case Dir4.S: return diff.X == 0 && Z >= other.Z;
                case Dir4.E: return diff.Z == 0 && X >= other.X;
                case Dir4.W: return diff.Z == 0 && X <= other.X;
                default: return false;
            }
        }

        public int GetDimension(Dimensions d)
        {
            switch (d)
            {
                case Dimensions.X:
                    return X;
                case Dimensions.Y:
                    return Y;
                case Dimensions.Z:
                    return Z;

            }
            throw new NotImplementedException("Get intV3");
        }
        public int GetDimensionXZ(Dir4 facing)
        {
            return GetDimension(lib.Facing4DirToDimensionsXZ(facing));
        }
        public void SetDimension(Dimensions d, int value)
        {
            switch (d)
            {
                case Dimensions.X:
                    X = value;
                    break;
                case Dimensions.Y:
                    Y = value;
                    break;
                case Dimensions.Z:
                    Z = value;
                    break;

            }
        }
        public void SetDimensionXZ(Dir4 facing, int value)
        {
            SetDimension(lib.Facing4DirToDimensionsXZ(facing), value);
        }
        public void AddDimension(Dimensions d, int add)
        {
            switch (d)
            {
                case Dimensions.X:
                    X += add;
                    break;
                case Dimensions.Y:
                    Y += add;
                    break;
                case Dimensions.Z:
                    Z += add;
                    break;

            }
        }

        public void AddSide(CubeFace side, int add)
        {
            switch (side)
            {
                case CubeFace.Xpositive: X += add; return;
                case CubeFace.Ypositive: Y += add; return;
                case CubeFace.Zpositive: Z += add; return;

                case CubeFace.Xnegative: X -= add; return;
                case CubeFace.Ynegative: Y -= add; return;
                case CubeFace.Znegative: Z -= add; return;
            }
        }

        public void SubtractSide(Dir4 side, int sub)
        {
            switch (side)
            {
                case Dir4.N: Z += sub; return;
                case Dir4.S: Z -= sub; return;
                case Dir4.E: X -= sub; return;
                case Dir4.W: X += sub; return;
                default: return;
            }
        }

        public void AddSide(Dir4 side, int add)
        {
            switch (side)
            {
                case Dir4.N: Z -= add; return;
                case Dir4.S: Z += add; return;
                case Dir4.E: X += add; return;
                case Dir4.W: X -= add; return;
                default: return;
            }
        }

        public static IntVector3 FromV3(Vector3 value)
        {
            return new IntVector3(Convert.ToInt32(value.X), Convert.ToInt32(value.Y), Convert.ToInt32(value.Z));
        }

        public static IntVector3 FromHeight(int y)
        {
            return new IntVector3(0, y, 0);
        }

        public ByteVector3 ByteVector
        {
            get
            {
                return new ByteVector3((byte)X, (byte)Y, (byte)Z);
            }
        }
        public SByteVector3 SByteVector
        {
            get
            {
                return new SByteVector3((sbyte)X, (sbyte)Y, (sbyte)Z);
            }
        }

        public Vector3 Vec
        {
            get
            {
                Vector3 result = Vector3.Zero;
                result.X = X;
                result.Y = Y;
                result.Z = Z;
                return result;

            }
            set
            {
                X = (int)value.X;
                Y = (int)value.Y;
                Z = (int)value.Z;
            }
        }
        public int[] Array
        {
            get
            {
                return new int[3] { X, Y, Z };
            }
            set
            {
                X = value[0];
                Y = value[1];
                Z = value[2];
            }
        }
        public static readonly IntVector3 Zero = new IntVector3(0, 0, 0);
        public static readonly IntVector3 One = new IntVector3(1, 1, 1);
        public static readonly IntVector3 NegativeOne = new IntVector3(-1, -1, -1);
        public static readonly IntVector3 PlusX = new IntVector3(1, 0, 0);
        public static readonly IntVector3 PlusZ = new IntVector3(0, 0, 1);
        public static readonly IntVector3 PlusY = new IntVector3(0, 1, 0);
        public static readonly IntVector3 NegativeX = new IntVector3(-1, 0, 0);
        public static readonly IntVector3 NegativeZ = new IntVector3(0, 0, -1);
        public static readonly IntVector3 NegativeY = new IntVector3(0, -1, 0);

        public void MoveXZ(Dir4 direction, int amount)
        {
            int sign = 1;
            if (direction == Dir4.N || direction == Dir4.W)
            {
                sign = -1;
            }
            SetDimensionXZ(direction, GetDimensionXZ(direction) + sign * amount);
        }
        public void Add(IntVector3 other)
        {
            this.X += other.X;
            this.Y += other.Y;
            this.Z += other.Z;
        }
        public static bool operator ==(IntVector3 value1, IntVector3 value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z;
        }
        public static bool operator !=(IntVector3 value1, IntVector3 value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y || value1.Z != value2.Z;
        }
        //public static bool operator >(IntVector3 value1, IntVector3 value2)
        //{
        //    return value1.X > value2.X || value1.Y != value2.Y || value1.Z != value2.Z;
        //}
        public static IntVector3 operator -(IntVector3 value)
        {
            value.X = -value.X;
            value.Y = -value.Y;
            value.Z = -value.Z;
            return value;
        }
        public static IntVector3 operator -(IntVector3 value1, IntVector3 value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            value1.Z -= value2.Z;
            return value1;
        }
        public static IntVector3 operator +(IntVector3 value1, IntVector3 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            value1.Z += value2.Z;
            return value1;
        }
        public static IntVector3 operator -(IntVector3 value1, int value2)
        {
            value1.X -= value2;
            value1.Y -= value2;
            value1.Z -= value2;
            return value1;
        }
        public static IntVector3 operator +(IntVector3 value1, int value2)
        {
            value1.X += value2;
            value1.Y += value2;
            value1.Z += value2;
            return value1;
        }
        public static IntVector3 operator *(IntVector3 value1, int value2)
        {
            value1.X *= value2;
            value1.Y *= value2;
            value1.Z *= value2;
            return value1;
        }
        public static IntVector3 operator /(IntVector3 value1, int value2)
        {
            value1.X /= value2;
            value1.Y /= value2;
            value1.Z /= value2;
            return value1;
        }
        public IntVector3 Sub(IntVector3 other)
        {
            this.X -= other.X;
            this.Y -= other.Y;
            this.Z -= other.Z;
            return this;
        }

        public bool IsZero { get { return X == 0 && Y == 0 && Z == 0; } }

        public bool HasValue()
        {
            return X != 0 || Y != 0 || Z != 0;
        }
        //public void One()
        //{
        //    X = 1; Y = 1; Z = 1;
        //}
        public void Cube(int side)
        { X = side; Y = side; Z = side; }
        public void Set(int x, int y, int z)
        { X = x; Y = y; Z = z; }
        public void Set(Dimensions d, int value)
        {
            switch (d)
            {
                case Dimensions.X:
                    X = value;
                    break;
                case Dimensions.Y:
                    Y = value;
                    break;
                case Dimensions.Z:
                    Z = value;
                    break;

            }

        }




        public void NetworkWriteByte(System.IO.BinaryWriter w)
        {
#if PCGAME
            if (X > byte.MaxValue || Y > byte.MaxValue || Z > byte.MaxValue)
                throw new OverflowException("Intvector3 to network byte");
#endif
            w.Write((byte)X); w.Write((byte)Y); w.Write((byte)Z);
        }
        public static IntVector3 FromByteSzStream(System.IO.BinaryReader r)
        {
            return new IntVector3(r.ReadByte(), r.ReadByte(), r.ReadByte());
        }

        public void WriteByteStream(System.IO.BinaryWriter w)
        {
            w.Write((byte)X); w.Write((byte)Y); w.Write((byte)Z);
        }
        public void ReadByteStream(System.IO.BinaryReader r)
        {
            X = r.ReadByte(); Y = r.ReadByte(); Z = r.ReadByte();
        }

        public void WriteUshortStream(System.IO.BinaryWriter w)
        {
            w.Write((ushort)X); w.Write((ushort)Y); w.Write((ushort)Z);
        }
        public void ReadUshortStream(System.IO.BinaryReader r)
        {
            X = r.ReadUInt16(); Y = r.ReadUInt16(); Z = r.ReadUInt16();
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(X); w.Write(Y); w.Write(Z);
        }
        public void read(System.IO.BinaryReader r)
        {
            X = r.ReadInt32(); Y = r.ReadInt32(); Z = r.ReadInt32();
        }
        public static IntVector3 FromStream(System.IO.BinaryReader r)
        {
            IntVector3 result = IntVector3.Zero;
            result.read(r);
            return result;
        }
        public string Text
        {
            set
            {
                string txt = TextLib.EmptyString;
                int[] val = Array;
                int pos = 0;
                foreach (char c in value)
                {
                    if (c == ',')
                    {
                        val[pos] = Convert.ToInt16(txt);
                        pos++;
                        txt = TextLib.EmptyString;
                        if (pos == 3)
                        { break; }
                    }
                    else if (c == ' ')
                    {
                        //Ignore
                    }
                    else
                    {
                        txt += c;
                    }
                }
            }
            get
            {
                return Convert.ToString(X) + "," + Convert.ToString(Y) + "," + Convert.ToString(Z);
            }
        }
        public override string ToString()
        {
            return "{X:" + X.ToString() + " Y:" + Y.ToString() + " Z:" + Z.ToString() + "}";
        }
        public string ToString(string divideChar)
        {
            return X.ToString() + divideChar + Y.ToString() + divideChar + Z.ToString();
        }

        public int LargestSideLength()
        {
            return lib.LargestValue(X, Y, Z);
        }

        public int LargestSideLength_Abs()
        {
            return Math.Abs(lib.LargestAbsoluteValue(X, Y, Z));
        }


        public override bool Equals(object obj)
        {
            IntVector3 other = (IntVector3)obj;
            return this.X == other.X &&
                this.Y == other.Y &&
                this.Z == other.Z;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void ClampAll(int min, int max)
        {
            X = MathExt.Clamp(X, min, max);
            Y = MathExt.Clamp(Y, min, max);
            Z = MathExt.Clamp(Z, min, max);
        }
    }
}
