using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine
{
    public static class VectorExt
    {
        public static readonly Vector2 V2NegOne = new Vector2(-1f);
        public static readonly Vector2 V2NegHalf = new Vector2(-0.5f);
        public static readonly Vector2 V2Half = new Vector2(0.5f);
        public static readonly Vector3 V3Half = new Vector3(0.5f);
        public static readonly Vector3 V3NegOne = new Vector3(-1f);

        public static readonly Vector2 V2HalfX = new Vector2(0.5f, 0f);
        public static readonly Vector2 V2HalfY = new Vector2(0f, 0.5f);
        public static readonly Vector2 V2NegHalfX = new Vector2(-0.5f, 0f);
        public static readonly Vector2 V2NegHalfY = new Vector2(0f, -0.5f);


        public static float Dot(this Vector3 op1, Vector3 op2) { return Vector3.Dot(op1, op2); }
        public static Vector3 Cross(this Vector3 op1, Vector3 op2) { return Vector3.Cross(op1, op2); }

        public static float GetDim(this Vector3 v, int dim)
        {
            switch (dim)
            {
                case 0: return v.X;
                case 1: return v.Y;
                case 2: return v.Z;
                default: throw new NotSupportedException();
            }
        }

        public static float GetDim(this Vector3 v, Dimensions dim)
        {
            switch (dim)
            {
                case Dimensions.X: return v.X;
                case Dimensions.Y: return v.Y;
                case Dimensions.Z: return v.Z;
                default: throw new NotSupportedException();
            }
        }

        public static void SetDim(this Vector3 v, int dim, float value)
        {
            switch (dim)
            {
                case 0: v.X = value; return;
                case 1: v.Y = value; return;
                case 2: v.Z = value; return;
                default: throw new NotSupportedException();
            }
        }

        public static Vector3 SetDim(Vector3 vector, Dimensions dim, float value)
        {
            switch (dim)
            {
                case Dimensions.X: vector.X = value; break;
                case Dimensions.Y: vector.Y = value; break;
                case Dimensions.Z: vector.Z = value; break;
                default: throw new NotSupportedException();
            }

            return vector;
        }

        public static float GetDim(this Vector2 v, int dim)
        {
            if (dim == 0)
                return v.X;
            return v.Y;
        }
        public static void SetDim(this Vector2 v, int dim, float x)
        {
            if (dim == 0)
                v.X = x;
            v.Y = x;
        }

        public static Vector3 AddToDimention(Vector3 value, float add, Dimensions d)
        {
            switch (d)
            {
                case Dimensions.X: value.X += add; return value;
                case Dimensions.Y: value.Y += add; return value;
                case Dimensions.Z: value.Z += add; return value;
            }
            throw new Exception();
        }

        public static Vector2 SetX(Vector2 value, float x)
        {
            value.X = x;
            return value;
        }
        public static Vector2 SetY(Vector2 value, float y)
        {
            value.Y = y;
            return value;
        }

        public static Vector3 SetY(Vector3 value, float y)
        {
            value.Y = y;
            return value;
        }

        public static IntVector2 AddX(IntVector2 value, int add)
        {
            value.X += add;
            return value;
        }

        public static IntVector2 AddY(IntVector2 value, int add)
        {
            value.Y += add;
            return value;
        }

        public static Vector2 AddX(Vector2 value, float add)
        {
            value.X += add;
            return value;
        }

        public static IntVector2 Add(IntVector2 value, int x, int y)
        {
            value.X += x;
            value.Y += y;
            return value;
        }

        public static Vector2 Add(Vector2 value, float add)
        {
            value.X += add;
            value.Y += add;
            return value;
        }
        public static Vector2 AddY(Vector2 value, float add)
        {
            value.Y += add;
            return value;
        }

        public static Vector3 AddX(Vector3 value, float add)
        {
            value.X += add;
            return value;
        }
        public static Vector3 AddY(Vector3 value, float add)
        {
            value.Y += add;
            return value;
        }
        public static Vector3 AddZ(Vector3 value, float add)
        {
            value.Z += add;
            return value;
        }

        public static Vector3 AddXZ(Vector3 value, Vector2 xz)
        {
            value.X += xz.X;
            value.Z += xz.Y;

            return value;
        }

        public static Vector3 AddXZ(Vector3 value, IntVector2 xz)
        {
            value.X += xz.X;
            value.Z += xz.Y;

            return value;
        }

        public static Vector3 AddXZ(Vector3 value, float x, float z)
        {
            value.X += x;
            value.Z += z;

            return value;
        }

        public static IntVector3 AddXZ(IntVector3 valV3, IntVector2 valXZ)
        {
            valV3.X += valXZ.X;
            valV3.Z += valXZ.Y;
            return valV3;
        }

        public static Vector2 AddAlongVectorX(Vector2 value, float add)
        {
            if (value.X > 0)
            {
                value.X += add;
            }
            else
            {
                value.X -= add;
            }
            return value;
        }

        public static float MaxDimValue(this Vector3 v)
        {
            return Math.Max(Math.Max(v.X, v.Y), v.Z);
        }
        public static float PlaneXZLengthSq(this Vector3 v)
        {
            return v.X * v.X + v.Z * v.Z;
        }
        public static float PlaneXZLength(this Vector3 v)
        {
            return MathExt.SquareRootF(v.X * v.X + v.Z * v.Z);
        }
        public static Vector2 PlaneXZVec(this Vector3 v)
        {
            return new Vector2(v.X, v.Z);
        }

        public static Vector2 V2(float val)
        {
            Vector2 ret = Vector2.Zero;
            ret.X = val; ret.Y = val;
            return ret;
        }
        public static Vector3 V3(float val)
        {
            Vector3 ret = Vector3.Zero;
            ret.X = val; ret.Y = val; ret.Z = val;
            return ret;
        }

        public static Vector3 V3FromWidthAndHeight(float xz, float y)
        {
            return new Vector3(xz, y, xz);
        }

        public static Vector3 V3FromXZ(Vector2 xz, float y)
        {
            return new Vector3(xz.X, y, xz.Y);
        }

        public static Vector3 V3FromXY(Vector2 xy, float z)
        {
            return new Vector3(xy.X, xy.Y, z);
        }

        public static Vector3 V3FromXY(float xy, float z)
        {
            return new Vector3(xy, xy, z);
        }

        public static Vector2 V2FromX(float x)
        {
            Vector2 result = Vector2.Zero;
            result.X = x;
            return result;
        }
        public static Vector2 V2FromY(float y)
        {
            Vector2 result = Vector2.Zero;
            result.Y = y;
            return result;
        }

        public static Vector3 V3FromX(float x)
        {
            Vector3 result = Vector3.Zero;
            result.X = x;
            return result;
        }
        public static Vector3 V3FromY(float y)
        {
            Vector3 result = Vector3.Zero;
            result.Y = y;
            return result;
        }
        public static Vector3 V3FromZ(float z)
        {
            Vector3 result = Vector3.Zero;
            result.Z = z;
            return result;
        }

        public static Vector3 V2toV3XY(Vector2 pos)
        {
            Vector3 ret = Vector3.Zero;
            ret.X = pos.X;
            ret.Y = pos.Y;
            return ret;
        }
        public static Vector3 V2toV3XZ(Vector2 pos)
        {
            Vector3 ret = Vector3.Zero;
            ret.X = pos.X;
            ret.Z = pos.Y;
            return ret;
        }
        public static Vector3 V2toV3XZ(Vector2 v2, float y)
        {
            Vector3 ret = Vector3.Zero;
            ret.X = v2.X;
            ret.Y = y;
            ret.Z = v2.Y;
            return ret;
        }

        public static Vector2 V3XYtoV2(Vector3 pos)
        {
            Vector2 ret = Vector2.Zero;
            ret.X = pos.X;
            ret.Y = pos.Y;
            return ret;
        }

        public static Vector2 V3XZtoV2(Vector3 pos)
        {
            Vector2 ret = Vector2.Zero;
            ret.X = pos.X;
            ret.Y = pos.Z;
            return ret;
        }
        public static IntVector2 V3XZtoV2(IntVector3 pos)
        {
            IntVector2 ret = IntVector2.Zero;
            ret.X = pos.X;
            ret.Y = pos.Z;
            return ret;
        }
        public static IntVector3 V2toV3XZ(IntVector2 pos)
        {
            IntVector3 ret = IntVector3.Zero;
            ret.X = pos.X;
            ret.Z = pos.Y;
            return ret;
        }
        public static IntVector3 V2toV3XZ(IntVector2 pos, int y)
        {
            IntVector3 ret = IntVector3.Zero;
            ret.X = pos.X;
            ret.Z = pos.Y;
            ret.Y = y;
            return ret;
        }

        public static Vector3 MoveTowardsPoint(Vector3 value, Vector3 goalPoint, float movelength)
        {
            return value + SafeNormalizeV3(goalPoint - value) * movelength;
        }

        public static bool HasValue(Vector2 value)
        {
            return value.X != 0 || value.Y != 0;
        }
        public static bool HasValue(Vector3 value)
        {
            return value.X != 0 || value.Y != 0 || value.Z != 0;
        }
        public static Vector2 SetMaxLength(Vector2 value, float maxLength)
        {
            float length = value.Length();
            if (length > maxLength)
            {
                //value.Normalize();
                //value *= maxLength;
                value *= 1f / length * maxLength;
            }

            return value;
        }

        public static Vector3 SetMaxLength(Vector3 value, float maxLength)
        {
            float length = value.Length();
            if (length > maxLength)
            {
                //value.Normalize();
                //value *= maxLength;
                value *= 1f / length * maxLength;
            }

            return value;
        }

        public static Vector2 SetLength(Vector2 value, float length)
        {
            value.Normalize();
            return value * length;
        }

        public static Vector3 SetLength(Vector3 value, float length)
        {
            value.Normalize();
            return value * length;
        }

        public static bool Corrupted(Vector2 value)
        {
            return float.IsNaN(value.X) || float.IsNaN(value.Y);
        }
        public static bool Corrupted(Vector3 value)
        {
            return float.IsNaN(value.X) || float.IsNaN(value.Y) || float.IsNaN(value.Z);
        }

        public static Vector2 SetScale(Vector2 value, float scale, Dimensions onSide)
        {
            if (onSide == Dimensions.X)
            {
                value.Y = scale / value.X * value.Y;
                value.X = scale;
            }
            else
            {
                value.X = scale / value.Y * value.X;
                value.Y = scale;
            }

            return value;
        }

        public static Vector2 Normalize(Vector2 vector, out float length)
        {
            length = vector.Length();
            vector.X /= length;
            vector.Y /= length;
            return vector;
        }

        public static Vector3 Normalize(Vector3 vector, out float length)
        {
            length = vector.Length();
            vector.X /= length;
            vector.Y /= length;
            vector.Z /= length;
            return vector;
        }

        public static float Length(Vector2 vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }
        public static float Length(float x, float y)
        {
            return (float)Math.Sqrt(x * x + y * y);
        }
        public static float Length(float x, float y, float z)
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public static float SideLength(float x, float y)
        {
            x = Math.Abs(x);
            y = Math.Abs(y);
            return x > y ? x : y;
        }

        public static float SideLength(float x, float y, float z)
        {
            x = Math.Abs(x);
            y = Math.Abs(y);
            z = Math.Abs(z);

            if (x > y)
            {
                return x > z ? x : z;
            }
            return y > z ? y : z;
        }

        public static float SideLength(Vector2 value)
        {
            value.X = Math.Abs(value.X);
            value.Y = Math.Abs(value.Y);

            return value.X > value.Y ? value.X : value.Y;
        }

        public static float SideLength(Vector2 value1, Vector2 value2)
        {
            float x = Math.Abs(value2.X - value1.X);
            float y = Math.Abs(value2.Y - value1.Y);

            return x > y ? x : y;
        }

        public static float SideLength(Vector3 value)
        {
            value.X = Math.Abs(value.X);
            value.Y = Math.Abs(value.Y);
            value.Z = Math.Abs(value.Z);

            if (value.X > value.Y)
            {
                return value.X > value.Z ? value.X : value.Z;
            }
            return value.Y > value.Z ? value.Y : value.Z;
        }

        public static float SideLength(Vector3 from, Vector3 to)
        {
            return SideLength(to.X - from.X, to.Y - from.Y, to.Z - from.Z);
        }

        public static float SideLength_XZ(Vector3 from, Vector3 to)
        {
            return SideLength(to.X - from.X, to.Z - from.Z);
        }

        public static float SideLength_XZ(IntVector3 from, IntVector3 to)
        {
            return SideLength(to.X - from.X, to.Z - from.Z);
        }

        

        /// <summary>
        /// Cirkle of vectors pointing outwards
        /// </summary>
        /// <returns></returns>
        public static Vector2[] CircleOfDirections(int count, float angleStart, float length)
        {
            Vector2[] result = new Vector2[count];

            float step = MathExt.Tau / count;
            Rotation1D angle = new Rotation1D(angleStart);

            for (int i = 0; i < count; ++i)
            {
                result[i] = angle.Direction(length);
                angle.Add(step);
            }

            return result;
        }

        public static Vector2 RotateVector(Vector2 vector, float rotation)
        {
            Vector2 result = new Vector2(
                (float)(Math.Cos(rotation) * vector.X - Math.Sin(rotation) * vector.Y),
                (float)(Math.Sin(rotation) * vector.X + Math.Cos(rotation) * vector.Y));
            return result;
        }

        public static Vector2 RotateVector(Vector2 vector, float rotation, Vector2 addToOrigo)
        {
            return new Vector2(
                (float)(Math.Cos(rotation) * vector.X - Math.Sin(rotation) * vector.Y) + addToOrigo.X,
                (float)(Math.Sin(rotation) * vector.X + Math.Cos(rotation) * vector.Y) + addToOrigo.Y);
        }

        public static Vector3 RotateVector(Vector3 vector, float rotation)
        {
            if ((vector.X == 0 && vector.Z == 0) || rotation == 0) return vector;

            return new Vector3(
                (float)(Math.Cos(rotation) * vector.X - Math.Sin(rotation) * vector.Z),
                vector.Y,
                (float)(Math.Sin(rotation) * vector.X + Math.Cos(rotation) * vector.Z));
        }

        public static Vector2 RotateVector90DegreeLeft(Vector2 vector)
        {
            return new Vector2(vector.Y, -vector.X);
        }
        public static Vector2 RotateVector90DegreeRight(Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }

        public static IntVector2 RotateVector90DegreeLeft(IntVector2 vector)
        {
            return new IntVector2(vector.Y, -vector.X);
        }
        public static IntVector2 RotateVector90DegreeRight(IntVector2 vector)
        {
            return new IntVector2(-vector.Y, vector.X);
        }

        public static Vector3 RotateVector90DegreeLeftXZ(Vector3 vector)
        {
            return new Vector3(vector.Z, vector.Y, -vector.X);
        }
        public static Vector3 RotateVector90DegreeRightXZ(Vector3 vector)
        {
            return new Vector3(-vector.Z, vector.Y, vector.X);
        }

        public static IntVector3 RotateVector90DegreeLeftXZ(IntVector3 vector)
        {
            return new IntVector3(vector.Z, vector.Y, -vector.X);
        }
        public static IntVector3 RotateVector90DegreeRightXZ(IntVector3 vector)
        {
            return new IntVector3(-vector.Z, vector.Y, vector.X);
        }

        public static Vector2 SafeNormalizeV2(Vector2 value)
        {
            if (value.X == 0 && value.Y == 0) return Vector2.Zero;
            value.Normalize();
            return value;
        }
        public static Vector3 SafeNormalizeV3(Vector3 value)
        {
            if (value.X == 0 && value.Y == 0 && value.Z == 0) return Vector3.Zero;
            value.Normalize();
            return value;
        }

        public static Vector2 Round(Vector2 value)
        {
            value.X = Convert.ToInt32(value.X);
            value.Y = Convert.ToInt32(value.Y);
            return value;
        }
    }
}
