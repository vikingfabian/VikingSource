using System;
using Microsoft.Xna.Framework;

namespace VikingEngine
{
    struct IntervalVector3
    {
        public Vector3 Min;
        public Vector3 Add;
        public Vector3 Max
        {
            get { return Min + Add; }
            set { Add = value - Min; }
        }
        public void AddRadius(float radius)
        {
            float twice = radius * PublicConstants.Twice;
            Min.X -= radius;
            Min.Y -= radius;
            Min.Z -= radius;

            Add.X += twice;
            Add.Y += twice;
            Add.Z += twice;
        }
        public IntervalVector3(Vector3 min, Vector3 max)
        {
            Min = min;
            Add = max;
            Add.X -= Min.X;
            Add.Y -= Min.Y;
            Add.Z -= Min.Z;
        }
        public static IntervalVector3 FromRadius(Vector3 center, float radius)
        {
            Vector3 min = center;
            min.X -= radius;
            min.Y -= radius;
            min.Z -= radius;

            IntervalVector3 ret = new IntervalVector3();
            ret.Min = min;
            ret.Add = VectorExt.V3(radius * 2);

            return ret;
        }
        public Vector3 GetRandom()
        {
            Vector3 ret = Min;
            ret.X += Ref.rnd.Float(Add.X);
            ret.Y += Ref.rnd.Float(Add.Y);
            ret.Z += Ref.rnd.Float(Add.Z);
            return ret;
        }
        public Vector3 PercentPosition(float percent)
        {
            return Min + Add * percent;
        }
        public override string ToString()
        {
            return "Min" + Min.ToString() + ", Add" + Add.ToString();
        }
        public static IntervalVector3 Zero = new IntervalVector3(Vector3.Zero, Vector3.Zero);

    }
}
