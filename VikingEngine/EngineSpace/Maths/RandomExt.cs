//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VikingEngine.EngineSpace.Maths;

//namespace VikingEngine
//{
//    public class PcgRandom
//    {
//        public Random random;
//        public int Seed { get; private set; }

//        public PcgRandom()
//        {
//            // I don't understand why they didn't expose the time-dependent seed for getting
//            Random rnd = new Random();
//            Seed = rnd.Next();
//            random = new Random(Seed);
//        }

//        public PcgRandom(int seed)
//        {
//            this.Seed = seed;
//            random = new Random(seed);
//        }

//        public bool ChanceSuccess(double chance)
//        {
//            if (chance <= 0)
//            {
//                return false;
//            }
//            return random.NextDouble() <= chance;
//        }

//        public float Float(float min, float max)
//        {
//            return Mgth.Lerp(min, max, (float)random.NextDouble());
//        }

//        //public Rectangle2 RectInRect(Rectangle2 rect, IntVector2 size)
//        //{
//        //    IntVector2 diff = rect.size - size;

//        //    if (diff.X < 0|| diff.Y < 0)
//        //        return new Rectangle2(IntVector2.Zero);

//        //    return new Rectangle2(rect.pos + new IntVector2(Int(diff.X), Int(diff.Y)), size);
//        //}

//        public float Float(float max)
//        {
//            return Mgth.Lerp(0, max, (float)random.NextDouble());
//        }

//        public float Float()
//        {
//            return (float)random.NextDouble();
//        }

//        /// <summary>
//        /// [min,max)
//        /// </summary>
//        /// <param name="min">Inclusive lower bound</param>
//        /// <param name="exMax">Exclusive upper bound</param>
//        public int Int(int min, int exMax)
//        {
//            return random.Next(min, exMax);
//        }

//        /// <summary>
//        /// [0,max)
//        /// </summary>
//        /// <param name="count">Exclusive upper bound</param>
//        public int Int(int count)
//        {
//            return random.Next(count);
//        }

//        public float Plus_Minus(int range)
//        {
//            return random.Next(-range, range + 1);
//        }
//        public float Plus_MinusF(float range)
//        {
//            return -range + (float)(2.0 * range * random.NextDouble());
//        }

//        public int Int()
//        {
//            return random.Next();
//        }

//        public bool Bool()
//        {
//            return random.NextDouble() >= 0.5;
//        }

//        public int Dir()
//        {
//            return random.NextDouble() >= 0.5? 1 : -1;
//        }

//        public IntVector2 intV2TilePos(IntVector2 size)
//        {
//            return new IntVector2(random.Next(size.X), random.Next(size.Y));
//        }

//        // Note the standard deviation parameter - do not pass variance.
//        public float GaussianDistribution(float mean, float stdDev)
//        {
//            double u1 = random.NextDouble();
//            double u2 = random.NextDouble();

//            // Normal(0,1)
//            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
//                         Math.Sin(u2 * MathHelper.TwoPi);
            
//            // Normal(mean, stdDev^2)
//            return (float)(mean + stdDev * randStdNormal);
//        }

//        /// <summary>
//        /// Returns a random float from a gaussian distribution set to fit
//        /// between the min and the max. The value is bounded to [min, max].-
//        /// </summary>
//        /// <param name="min">This value should be the smaller of the two.</param>
//        /// <param name="max">This value should be the greater of the two.</param>
//        public float GaussianInInterval(float min, float max)
//        {
//            float halfDist = (max - min) * 0.5f;
//            return MathHelper.Clamp(GaussianDistribution(min + halfDist, halfDist / 3), min, max);
//        }

//        //public Vector2 Direction2()
//        //{
//        //    double angle = random.NextDouble() * MathHelper.TwoPi;
//        //    return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
//        //}

//        //public Vector2 Direction2(float length)
//        //{
//        //    double angle = random.NextDouble() * MathHelper.TwoPi;
//        //    return new Vector2((float)Math.Cos(angle) * length, (float)Math.Sin(angle) * length);
//        //}

//        //public Vector3 Direction3()
//        //{
//        //    float a1 = Float(MathHelper.Pi);
//        //    float a2 = Float(MathHelper.TwoPi);
//        //    return new Vector3((float)(Math.Sin(a1) * Math.Cos(a2)),
//        //                       (float)(Math.Sin(a1) * Math.Sin(a2)),
//        //                       (float)(Math.Cos(a1)));
//        //}

//        public Dir4 Dir4()
//        {
//            return (Dir4)random.Next(4);
//        }
//    }
//}
