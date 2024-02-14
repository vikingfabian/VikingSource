using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

//xna
using VikingEngine.EngineSpace.Maths;

namespace VikingEngine //VECTOR
{

    /// <summary>
    /// Wrapper to take care of both 2D and 3D speed, Y is up
    /// </summary>
    struct Velocity
    {
        public static readonly Velocity Zero = new Velocity();
        public Vector3 Value;
        
        public void SetValue_Safe(Vector3 value)
        {
            this.Value = value;
            if (PlatformSettings.ViewErrorWarnings)
            {
                if (float.IsNaN(value.X) ||
                    float.IsNaN(value.Y) ||
                    float.IsNaN(value.Z))
                {
                    throw new Exception("velocity Nan value");
                }
            }
        }

        public Velocity(Vector3 startSpeed)
        {

            Value = startSpeed;
            setMaxValue();
        }
        public Velocity(Vector2 startSpeed)
        {
            Value = new Vector3(startSpeed.X, 0, startSpeed.Y);
            setMaxValue();
        }

        void setMaxValue()
        { 
            const float Max = 100;
            Value.X = Bound.MaxAbs(Value.X, Max);
            Value.Y = Bound.MaxAbs(Value.Y, Max);
            Value.Z = Bound.MaxAbs(Value.Z, Max);

            //if (Math.Abs(Value.X) > Max)
            //{
            //    Value
            //}
        }


        public Velocity(Rotation1D dir, float length)
            :this()
        {
            Set(dir, length);
        }

        /// <summary>
        /// The speed in 2D, seen from above
        /// </summary>
        public Vector2 PlaneValue
        {
            get
            {
                return new Vector2(Value.X, Value.Z);
            }
            set
            {
                Value.X = value.X;
                Value.Z = value.Y;
            }
        }

        //public void SetPlaneValue(Vector3 valueXY)
        //{
        //    Value.X = valueXY.X;
        //    Value.Z = valueXY.Y;
        //}
        public void Set(Vector2 valueXZ)
        {
            Value.X = valueXZ.X;
            Value.Z = valueXZ.Y;
        }
        public void SetPlaneValue(Velocity value)
        {
            Value.X = value.Value.X;
            Value.Z = value.Value.Z;
        }

        public float PlaneX
        {
            get { return Value.X; }
            set { Value.X = value; }
        }
        public float PlaneY
        {
            get { return Value.Z; }
            set { Value.Z = value; }
        }
        public float Y
        {
            get { return Value.Y; }
            set { Value.Y = value; }
        }

        public bool ZeroPlaneSpeed { get { return Value.X == 0 && Value.Z == 0; } }

        public float PlaneLength()
        {
            return VectorExt.Length(Value.X, Value.Z);
        }
        public void SetLength(float length)
        {
            if (Value != Vector3.Zero)
            {
                Value.Normalize();
                Value *= length;
            }
        }
        //public float PlaneLength()
        //{
        //    return Value.Length();
        //}

        public void Read(System.IO.BinaryReader r)
        {
            Value = new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
        }
        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(Value.X); w.Write(Value.Y); w.Write(Value.Z);
        }

        /// <summary>
        /// Write the plane speed angle in one byte 
        /// </summary>
        public void WriteByteDir(System.IO.BinaryWriter w)
        {
            w.Write(this.Rotation1D().ByteDir);
        }


        public void Update(float time, Graphics.AbsVoxelObj img)
        {
            img.position += Value * time;
        }
        public void Update(float time, Graphics.AbsVoxelObj img, float gravity)
        {
            Value.Y += gravity * time;
            img.position += Value * time;
        }
        public void PlaneUpdate(float time, Graphics.AbsVoxelObj img)
        {
            img.position.X += Value.X * time;
            img.position.Z += Value.Z * time;
        }
        public Vector3 Update(float time, Vector3 startPos)
        {
            return startPos + Value * time;
        }

        public Vector3 Update(float time, Vector3 startPos, float gravity)
        {
            Value.Y += gravity * time;
            return startPos + Value * time;
        }

        public static Velocity operator *(Velocity speed, float value)
        {
            speed.Value *= value;
            return speed;
        }
        public static Velocity operator *(float value, Velocity speed)
        {
            speed.Value *= value;
            return speed;
        }

        
        public void Add(Rotation1D dir, float length)
        {
            Add(lib.AngleToV2(dir.Radians, length));
        }
        public void Add(Vector2 planeSpeed)
        {
            Value.X += planeSpeed.X;
            Value.Z += planeSpeed.Y;
        }
        public void MultiplyPlane(float multi)
        {
            Value.X *= multi;
            Value.Z *= multi;
        }

        public void Set(Rotation1D dir, float length)
        {
            PlaneValue = lib.AngleToV2(dir.Radians, length);
        }

        /// <param name="dir">direction in radians</param>
        public void Set(float dir, float length)
        {
            PlaneValue = lib.AngleToV2(dir, length);
        }

        /// <returns>Reached max speed</returns>
        public bool SetMaxPlaneSpeed(float maxSpeed)
        {
            float l = VectorExt.Length(Value.X, Value.Z);
            if (l > maxSpeed)
            {
                float multi = 1 / l * maxSpeed;
                Value.X *= multi;
                Value.Z *= multi;
                return true;
            }
            return l == maxSpeed;
        }

        public Rotation1D Rotation1D()
        {
            return new Rotation1D(lib.V2ToAngle(PlaneValue));
        }


        public float Radians()
        {
            return lib.V2ToAngle(PlaneValue);
        }

        public void SetZeroPlaneSpeed()
        {
            Value.X = 0; Value.Z = 0;
        }
        public void SetZero()
        {
            Value = Vector3.Zero;
        }
        public override string ToString()
        {
            return "velocity " + Value.ToString();
        }
    }

    public struct ByteVector2
    {
        public byte X;
        public byte Y;

        public ByteVector2(byte unit)
        { X = unit; Y = unit; }
        public ByteVector2(byte x, byte y)
        { X = x; Y = y; }
        public ByteVector2(float x, float y)
        { X = (byte)x; Y = (byte)y; }
        public ByteVector2 Multiply(float multi)
        {
            X = (byte)(multi * X);
            Y = (byte)(multi * Y);
            return this;
        }
        public Vector2 Vec
        {
            get
            {
                Vector2 ret = Vector2.Zero;
                ret.X = X;
                ret.Y = Y;
                return ret;

            }
            set
            {
                X = (byte)value.X;
                Y = (byte)value.Y;
            }
        }
        public byte[] Array
        {
            get
            {
                return new byte[2] { X, Y };
            }
            set
            {
                X = value[0];
                Y = value[1];
            }
        }
        public static ByteVector2 Zero = new ByteVector2(0, 0);

        public static ByteVector2 One = new ByteVector2(1, 1);

        public static ByteVector2 FromVec2(Vector2 value)
        {
            return new ByteVector2((byte)value.X, (byte)value.Y);
        }

        public Dir4 ToFacing4Dir()
        {
            if (X == 0)
            {
                if (Y < 0)
                {
                    return Dir4.N;
                }
                else
                {
                    return Dir4.S;
                }
            }
            else
            {
                if (X < 0)
                {
                    return Dir4.W;
                }
                else
                {
                    return Dir4.E;
                }
            }
        }
        public void Add(ByteVector2 add)
        {
            X += add.X;
            Y += add.Y;
        }
        public void Square(byte side)
        { X = side; Y = side; }
        public void set(byte x, byte y)
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
                    X = (byte)array[0];
                    Y = (byte)array[1];
                }
            }
        }


        public override string ToString()
        {
            return "{" + X.ToString() + "," + Y.ToString() + "}";
        }
        public override bool Equals(object obj)
        {
            ByteVector2 vec = (ByteVector2)obj;

            return vec.X == this.X && vec.Y == this.Y;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(ByteVector2 value1, ByteVector2 value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }
        public static bool operator !=(ByteVector2 value1, ByteVector2 value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }
        public void WriteStream(System.IO.BinaryWriter w)
        {
            w.Write(X); w.Write(Y);
        }
        public void ReadStream(System.IO.BinaryReader r)
        {
            X = r.ReadByte();
            Y = r.ReadByte();
        }
        public static ByteVector2 FromStream(System.IO.BinaryReader r)
        {
            return new ByteVector2(r.ReadByte(), r.ReadByte());
        }
    }
    public struct ShortVector2 : IBinaryIOobj
    {
        public short X;
        public short Y;
        public ShortVector2(short unit)
        { X = unit; Y = unit; }
        public ShortVector2(int x, int y)
        { X = (short)x; Y = (short)y; }
        public ShortVector2(short x, short y)
        { X = x; Y = y; }
        public ShortVector2(IntVector2 fromVec)
        { X = (short)fromVec.X; Y = (short)fromVec.Y; }
        public static readonly ShortVector2 Zero = new ShortVector2(0, 0);
        public static readonly ShortVector2 One = new ShortVector2(1, 1);
        public static readonly ShortVector2 NegativeOne = new ShortVector2(-1, -1);
        public Vector2 Vec
        {
            get
            {
                Vector2 ret = Vector2.Zero;
                ret.X = X;
                ret.Y = Y;
                return ret;

            }
            set
            {
                X = (short)value.X;
                Y = (short)value.Y;
            }
        }
        public IntVector2 IntV2
        {
            get
            {
                IntVector2 ret = IntVector2.Zero;
                ret.X = X;
                ret.Y = Y;
                return ret;

            }
            set
            {
                X = (short)value.X;
                Y = (short)value.Y;
            }
        }
        public static ShortVector2 operator +(ShortVector2 value1, ShortVector2 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }
        public static ShortVector2 operator -(ShortVector2 value1, ShortVector2 value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }
        public static ShortVector2 operator -(ShortVector2 value1, int value2)
        {
            value1.X -= (short)value2;
            value1.Y -= (short)value2;
            return value1;
        }
        public static bool operator ==(ShortVector2 value1, ShortVector2 value2)
        {
            return value1.X == value2.X && value1.Y == value2.Y;
        }
        public static bool operator !=(ShortVector2 value1, ShortVector2 value2)
        {
            return value1.X != value2.X || value1.Y != value2.Y;
        }
        public void Add(IntVector2 add)
        {
            X = (short)(X + add.X);
            Y = (short)(Y + add.Y);

        }
        public override bool Equals(object obj)
        {
            ShortVector2 other = (ShortVector2)obj;
            return other.X == X && other.Y == Y;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return "{" + X.ToString() + "," + Y.ToString() + "}";
        }
        public float Length()
        {
            return Vec.Length();
        }
        /// <summary>
        /// Returns the longest side
        /// </summary>
        public short SideLength()
        {
            return Math.Abs(Math.Abs(X) > Math.Abs(Y) ? X : Y);
        }
        public short SideLength(ShortVector2 point)
        {
            ShortVector2 diff = new ShortVector2(point.X - X, point.Y - Y);
            return diff.SideLength();
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(X); w.Write(Y);
        }
        public void read(System.IO.BinaryReader r)
        {
            X = r.ReadInt16();
            Y = r.ReadInt16();
        }
        public static ShortVector2 FromStream(System.IO.BinaryReader r)
        {
            ShortVector2 result = ShortVector2.Zero;
            result.read(r);
            return result;
        }
    }


    
   

    public struct ByteVector3 : IBinaryIOobj
    {
        public byte X;
        public byte Y;
        public byte Z;

        public ByteVector3(byte x, byte y, byte z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public ByteVector3(float x, float y, float z)
        {
            this.X = (byte)x;
            this.Y = (byte)y;
            this.Z = (byte)z;
        }
        public static readonly ByteVector3 Zero = new ByteVector3(0, 0, 0);
        public static readonly ByteVector3 One = new ByteVector3(1, 1, 1);

        public IntVector3 IntVec
        {
            get
            {
                return new IntVector3(X, Y, Z);
            }
        }
        public Vector3 Vec
        {
            get
            {
                Vector3 ret = Vector3.Zero;
                ret.X = X;
                ret.Y = Y;
                ret.Z = Z;
                return ret;

            }
            set
            {
                X = (byte)value.X;
                Y = (byte)value.Y;
                Z = (byte)value.Z;
            }
        }
        public byte[] Array
        {
            get
            {
                return new byte[3] { X, Y, Z };
            }
            set
            {
                X = value[0];
                Y = value[1];
                Z = value[2];
            }
        }
        public byte GetArrayIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return X;
                case 1:
                    return Y;
                case 2:
                    return Z;

            }
            throw new IndexOutOfRangeException("ByteV3 GetArrayIndex");
        }

        public void SetArrayIndex(int index, byte value)
        {
            switch (index)
            {
                case 0:
                    X = value;
                    break;
                case 1:
                    Y = value;
                    break;
                case 2:
                    Z = value;
                    break;
            }
        }
        
        public void Cube(byte side)
        { X = side; Y = side; Z = side; }
        public void write(System.IO.BinaryWriter w)
        {
            w.Write(X); w.Write(Y); w.Write(Z);
        }
        public void read(System.IO.BinaryReader r)
        {
            X = r.ReadByte();
            Y = r.ReadByte();
            Z = r.ReadByte();
        }
        public static ByteVector3 FromStream(System.IO.BinaryReader r)
        {
            ByteVector3 result = ByteVector3.Zero;
            result.read(r);
            return result;
        }
        public override string ToString()
        {
            return "{X:" + X.ToString() + " Y:" + Y.ToString() + " Z:" + Z.ToString() + "}";
        }
    }

    /// <summary>
    /// Three signed bytes, X, Y and Z. Each -128 to 127
    /// </summary>
    public struct SByteVector3 : IBinaryIOobj
    {
        public sbyte X;
        public sbyte Y;
        public sbyte Z;

        public SByteVector3(sbyte x, sbyte y, sbyte z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public SByteVector3(float x, float y, float z)
        {
            this.X = (sbyte)x;
            this.Y = (sbyte)y;
            this.Z = (sbyte)z;
        }
        public static readonly SByteVector3 Zero = new SByteVector3(0, 0, 0);
        public static readonly SByteVector3 One = new SByteVector3(1, 1, 1);

        public IntVector3 IntVec
        {
            get
            {
                return new IntVector3(X, Y, Z);
            }
        }
        public Vector3 Vec
        {
            get
            {
                Vector3 ret = Vector3.Zero;
                ret.X = X;
                ret.Y = Y;
                ret.Z = Z;
                return ret;

            }
            set
            {
                X = (sbyte)value.X;
                Y = (sbyte)value.Y;
                Z = (sbyte)value.Z;
            }
        }
        public sbyte[] Array
        {
            get
            {
                return new sbyte[3] { X, Y, Z };
            }
            set
            {
                X = value[0];
                Y = value[1];
                Z = value[2];
            }
        }
        public SByte GetArrayIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return X;
                case 1:
                    return Y;
                case 2:
                    return Z;

            }
            throw new IndexOutOfRangeException("ByteV3 GetArrayIndex");
        }

        public void SetArrayIndex(int index, SByte value)
        {
            switch (index)
            {
                case 0:
                    X = value;
                    break;
                case 1:
                    Y = value;
                    break;
                case 2:
                    Z = value;
                    break;
            }
        }
        public void Cube(sbyte side)
        { X = side; Y = side; Z = side; }
        public void write(System.IO.BinaryWriter w)
        {
            w.Write(X); w.Write(Y); w.Write(Z);
        }
        public void read(System.IO.BinaryReader r)
        {
            X = r.ReadSByte();
            Y = r.ReadSByte();
            Z = r.ReadSByte();
        }
        public static SByteVector3 FromStream(System.IO.BinaryReader r)
        {
            SByteVector3 result = SByteVector3.Zero;
            result.read(r);
            return result;
        }
        public override string ToString()
        {
            return "{X:" + X.ToString() + " Y:" + Y.ToString() + " Z:" + Z.ToString() + "}";
        }
    }
}
