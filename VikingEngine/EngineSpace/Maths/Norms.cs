using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.EngineSpace.Maths
{
    public interface INorm
    {
        float Norm(float x);
        float Norm(Vector2 x);
        float Norm(Vector3 x);
    }

    public struct MaxNorm : INorm
    {
        /* Interface Methods */
        public float Norm(float x)
        {
            return Math.Abs(x);
        }
        public float Norm(Vector2 x)
        {
            return Math.Max(Math.Abs(x.X), Math.Abs(x.Y));
        }
        public float Norm(Vector3 x)
        {
            return Math.Max(Math.Max(Math.Abs(x.X), Math.Abs(x.Y)), Math.Abs(x.Z));
        }
    }

    public struct EuclideanNorm : INorm
    {
        /* Interface Methods */
        public float Norm(float x)
        {
            return Math.Abs(x);
        }
        public float Norm(Vector2 x)
        {
            return (float)Math.Sqrt(x.X * x.X + x.Y * x.Y);
        }
        public float Norm(Vector3 x)
        {
            return (float)Math.Sqrt(x.X * x.X + x.Y * x.Y + x.Z * x.Z);
        }
    }

    public struct EuclideanSquaredNorm : INorm
    {
        /* Interface Methods */
        public float Norm(float x)
        {
            return x * x;
        }
        public float Norm(Vector2 x)
        {
            return x.X * x.X + x.Y * x.Y;
        }
        public float Norm(Vector3 x)
        {
            return x.X * x.X + x.Y * x.Y + x.Z * x.Z;
        }
    }

    public struct ManhattanNorm : INorm
    {
        /* Interface Methods */
        public float Norm(float x)
        {
            return Math.Abs(x);
        }
        public float Norm(Vector2 x)
        {
            return Math.Abs(x.X) + Math.Abs(x.Y);
        }
        public float Norm(Vector3 x)
        {
            return Math.Abs(x.X) + Math.Abs(x.Y) + Math.Abs(x.Z);
        }
    }

    public struct PNorm : INorm
    {
        /* Fields */
        float p;

        /* Constructors */
        public PNorm(float p)
        {
            if (p < 1f)
                throw new NotSupportedException("p must be greater than or equal to 1");
            this.p = p;
        }

        /* Interface Methods */
        public float Norm(float x)
        {
            return Math.Abs(x);
        }
        public float Norm(Vector2 x)
        {
            return (float)Math.Pow(Math.Pow(Math.Abs(x.X), p) + Math.Pow(Math.Abs(x.Y), p), 1f / p);
        }
        public float Norm(Vector3 x)
        {
            return (float)Math.Pow(Math.Pow(Math.Abs(x.X), p) + Math.Pow(Math.Abs(x.Y), p) + Math.Pow(Math.Abs(x.Z), p), 1f / p);
        }
    }
}
