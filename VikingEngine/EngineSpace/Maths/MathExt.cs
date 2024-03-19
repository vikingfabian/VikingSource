using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    /// <summary>
    /// Math by Martin Grönlund with short name.
    /// </summary>
    static class MathExt
    {
        public const float Tau = MathHelper.TwoPi;
        public const float TauOver2 = MathHelper.Pi;
        public const float TauOver4 = MathHelper.PiOver2;
        public const float TauOver8 = MathHelper.PiOver4;
        public const float TauOver16 = MathHelper.Pi / 8f;
        public const float TauThreeQuarter = MathHelper.TwoPi * 0.75f;

        /* Constants */
        public const float SqrtOf2 = 1.41421356237f;

        /* Methods */
        /// <summary>
        /// l <= x <= r
        /// </summary>
        public static bool InII(int l, int x, int r)
        {
            return l <= x && x <= r;
        }
        /// <summary>
        /// l <= x < r
        /// </summary>
        public static bool InIE(int l, int x, int r)
        {
            return l <= x && x < r;
        }
        /// <summary>
        /// l < x <= r
        /// </summary>
        public static bool InEI(int l, int x, int r)
        {
            return l < x && x <= r;
        }
        /// <summary>
        /// l < x < r
        /// </summary>
        public static bool InEE(int l, int x, int r)
        {
            return l < x && x < r;
        }

        /// <summary>
        /// Cosine interpolation
        /// </summary>
        public static Vector3 Cerp(Vector3 v1, Vector3 v2, float t)
        {
            t = (float)(1f - Math.Cos(t * MathHelper.Pi)) * 0.5f;
            return v1 * (1 - t) + v2 * t;
        }

        public static int Pow(int value, double power)
        {
            return Convert.ToInt32(Math.Pow(value, power));
        }

        /// <summary>
        /// Linear interpolation
        /// </summary>
        public static Vector3 Lerp(Vector3 v1, Vector3 v2, float t)
        {
            return v1 * (1 - t) + v2 * t;
        }

        /// <summary>
        /// Cosine interpolation
        /// </summary>
        public static Vector2 Cerp(Vector2 v1, Vector2 v2, float t)
        {
            t = (float)(1f - Math.Cos(t * MathHelper.Pi)) * 0.5f;
            return v1 * (1 - t) + v2 * t;
        }

        /// <summary>
        /// Linear interpolation
        /// </summary>
        public static Vector2 Lerp(Vector2 v1, Vector2 v2, float t)
        {
            return v1 * (1 - t) + v2 * t;
        }

        /// <summary>
        /// Cosine interpolation
        /// </summary>
        public static Color Cerp(Color c1, Color c2, float t)
        {
            return new Color(Cerp(c1.ToVector3(), c2.ToVector3(), t));
        }

        /// <summary>
        /// Cosine interpolation
        /// </summary>
        public static float Cerp(float x1, float x2, float t)
        {
            t = (float)(1f - Math.Cos(t * MathHelper.Pi)) * 0.5f;
            return x1 * (1 - t) + x2 * t;
        }

        /// <summary>
        /// Linear interpolation
        /// </summary>
        public static Color Lerp(Color c1, Color c2, float t)
        {
            return new Color(Lerp(c1.ToVector3(), c2.ToVector3(), t));
        }

        public static float Lerp(float a, float b, float t)
        {
            return a * (1 - t) + b * t;
        }

        public static ushort Lerp(ushort a, ushort b, float t)
        {
            return (ushort)(a * (1 - t) + b * t);
        }

        public static float SquareRootF(float x)
        {
            return (float)Math.Sqrt((double)x);
        }


        /// <returns>value^2</returns>
        public static float Square(float value)
        {
            return value * value;
        }
        /// <returns>value^2</returns>
        public static int Square(int value)
        {
            return value * value;
        }

        public static float MinimumDistance(IntVector2 to, List<IntVector2> from)
        {
            float minDistance = float.MaxValue;

            foreach (IntVector2 v in from)
            {
                float distance = (to - v).LengthSquared();

                if (distance < minDistance)
                    minDistance = distance;
            }

            return SquareRootF(minDistance);
        }

        /// <summary>
        /// 1 if above, 0 else.
        /// </summary>
        public static float AboveOmega(float x, float omega)
        {
            if (x >= omega)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Important! Only use for values in range [-pi,pi]
        /// </summary>
        public static float FastSin(float x)
        {
            if (x < 0)
                return 1.27323954f * x + 0.405284735f * x * x;
            else
                return 1.27323954f * x - 0.405284735f * x * x;
        }

        static double NthRoot(double x, int N)
        {
            return Math.Pow(x, 1.0 / N);
        }

        public static float Norm(IntVector2 x, int norm)
        {
            if (norm != 1)
                return (float)NthRoot(Math.Pow(x.X, norm) + Math.Pow(x.Y, norm), norm);
            else
                return Math.Abs(x.X) + Math.Abs(x.Y);
        }

        public static float Norm(Vector2 x, int norm)
        {
            if (norm != 1)
                return (float)NthRoot(Math.Pow(x.X, norm) + Math.Pow(x.Y, norm), norm);
            else
                return Math.Abs(x.X) + Math.Abs(x.Y);
        }

        public static float Norm(IntVector3 x, int norm)
        {
            if (norm != 1)
                return (float)NthRoot(Math.Pow(x.X, norm) + Math.Pow(x.Y, norm) + Math.Pow(x.Z, norm), norm);
            else
                return Math.Abs(x.X) + Math.Abs(x.Y) + Math.Abs(x.Z);
        }

        public static float Norm(Vector3 x, int norm)
        {
            if (norm != 1)
                return (float)NthRoot(Math.Pow(x.X, norm) + Math.Pow(x.Y, norm) + Math.Pow(x.Z, norm), norm);
            else
                return Math.Abs(x.X) + Math.Abs(x.Y) + Math.Abs(x.Z);
        }

        public static float Ceiling(float value)
        {
            return (float)Math.Ceiling(value);
        }

        public static int CeilingToInt(float value)
        {
            return (int)Math.Ceiling(value);
        }

        public static float Floor(float x)
        {
            return (float)((int)x);
        }

        public static float Log2(float x)
        {
            return (float)Math.Log(x, 2);
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        public static float V2Cross(Vector2 u, Vector2 v)
        {
            return u.X * v.Y - u.Y * v.X;
        }

        public static bool NearZero(float x)
        {
            return x < 1e-10f;
        }

        public static int MultiplyInt(double multiply, int value)
        {
            return Convert.ToInt32(value * multiply);
        }

        public static IntVector2 Max(IntVector2 a, IntVector2 b)
        {
            return new IntVector2(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
        }

        /// <summary>
        /// Calculates the minimum distance of the circle with radius sqrt(rSq)
        /// from the origin if it is to fit into a circle sector with angle
        /// width halfSectorAngle * 2.
        /// </summary>
        public static float PlaceCircleInSector(float rSq, float halfSectorAngle)
        {
            float projectionSq = (float)Math.Pow(Math.Cos(halfSectorAngle), 2);
            float t = (float)Math.Sqrt((rSq * projectionSq) / (1 - projectionSq));
            return (float)Math.Sqrt(Math.Pow(t, 2) + rSq);
        }

        public static float Cosf(float angle)
        {
            return (float)Math.Cos(angle);
        }

        public static float Sinf(float angle)
        {
            return (float)Math.Sin(angle);
        }

        public static float Atan2f(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }

        public static float Round(float value)
        {
            return Convert.ToInt32(value);
        }

        public static float RoundAndEven(float value)
        {
            int result = Convert.ToInt32(value);
            if (lib.IsOdd(result))
            {
                result -= 1;
            }
            return result;
        }

        /// <summary>
        /// Computes the unnormalized inward normal of side A in the triangle with sides OA,OB,AB
        /// </summary>
        public static Vector2 Tri_In(Vector2 A, Vector2 B)
        {
            // Ax(BxC)=B(A|C)-C(A|B)
            // Ax(BxA)=B(A|A)-A(A|B), in from A
            return B * Vector2.Dot(A, A) - A * Vector2.Dot(A, B);
        }

        /// <summary>
        /// Computes the unnormalized outward normal of side A in the triangle with sides OA,OB,AB
        /// </summary>
        public static Vector2 Tri_Out(Vector2 A, Vector2 B)
        {
            // Ax(BxC)=B(A|C)-C(A|B)
            // Ax(AxB)=A(A|B)-B(A|A), out from A
            return A * Vector2.Dot(A, B) - B * Vector2.Dot(A, A);
        }

        public static float FlipAngleX(float angle)
        {
            angle = NormalizeAngle(angle);

            angle = MathHelper.TwoPi - angle;

            return angle;
        }

        public static float FlipAngleY(float angle)
        {
            angle = NormalizeAngle(angle);

            if (angle < MathHelper.Pi)
            {
                angle = MathHelper.Pi - angle;
            }
            else
            {
                angle = MathHelper.TwoPi - angle + MathHelper.Pi;
            }
            return angle;
        }

        public static float SetAngleXdir(float angle, bool positiveDir)
        {
            angle = NormalizeAngle(angle);

            if (angle < MathHelper.TwoPi != positiveDir)
            {
                angle = FlipAngleX(angle);
            }

            return angle;
        }
        public static float SetAngleYdir(float angle, bool positiveDir)
        {
            angle = NormalizeAngle(angle);

            if ((angle > MathExt.TauOver4 && angle < MathExt.TauThreeQuarter) != positiveDir)
            {
                angle = FlipAngleY(angle);
            }

            return angle;
        }


        /// <summary>
        /// Keeps angle between 0 - Two Pi
        /// </summary>
        public static float NormalizeAngle(float angle)
        {
            if (angle < 0)
            {
                //angle = (angle % MathHelper.TwoPi) + MathHelper.TwoPi;

                int backRevolutions = (int)(-angle / MathHelper.TwoPi);
                return angle + MathHelper.TwoPi * (backRevolutions + 1);
            }
            else
            {
                //float test = (float)Math.IEEERemainder(angle, MathHelper.TwoPi);
                //float test2 = Remainder(angle, MathHelper.TwoPi);
                //float test = (float)Decimal.Remainder(angle, MathHelper.TwoPi);
                //return angle % MathHelper.TwoPi;
                //int revolutions = ((int)(angle / MathHelper.TwoPi));
                return angle - ((int)(angle / MathHelper.TwoPi)) * MathHelper.TwoPi;
            }
        }

        static float Remainder(float value, float divisor)
        {
            return value - ((int)(value / divisor)) * divisor;
        }

        public static int DivRest(int value, int divisor, out int rest)
        {
            int result = value / divisor;
            rest = value - result * divisor;
            return result;
        }

        public static int DivRest(ref int valueAndRest, int divisor)
        {
            int result = valueAndRest / divisor;
            valueAndRest -= result * divisor;
            return result;
        }

        public static int Div_Ceiling(double value, double divide)
        { 
            return (int)Math.Ceiling(value/divide);
        }

        /// <summary>
        /// Calculates the sum of a series of purchases.
        /// </summary>
        /// <param name="initialValue">The initial purchase amount.</param>
        /// <param name="increase">The increase in purchase amount for each subsequent purchase.</param>
        /// <param name="repeate">The total number of purchases.</param>
        /// <returns>The total sum of all purchases.</returns>
        public static double SumOfLinearIncreases(double initialValue, double increase, int repeate)
        {
            return repeate / 2.0 * (2 * initialValue + (repeate - 1) * increase);
        }
    }
}
