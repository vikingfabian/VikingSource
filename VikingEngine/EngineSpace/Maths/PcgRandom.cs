using Microsoft.Xna.Framework;
using System;

//Craig Martin

namespace VikingEngine
{
    public class PcgRandom
    {
        const double UintDiv = 1.0 / uint.MaxValue;

        ulong state;
        ulong inc;
        ulong seed;

        public ulong Seed
        {
            get
            {
                return seed;
            }
            set
            {
                SetSeed(value);
            }
        }


        public PcgRandom()
        {
            SetSeed(new Random().Next(int.MaxValue));//0x853c49e6748fea9bUL);
        }

        public PcgRandom(int seed)
        {
            SetSeed(seed);
        }

        public PcgRandom(ulong seed)
        {
            SetSeed(seed);
        }

        public PcgRandom(ulong seed, ulong initSeq)
        {
            SetSeed(seed, initSeq);
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(seed);
        }
        public void read(System.IO.BinaryReader r)
        {
            SetSeed(r.ReadUInt32());
        }

        public void SetSeed(int seed)
        {
            SetSeed((ulong)seed);
        }

        public void SetSeed(ulong seed, ulong initSeq = 0xda3e39cb94b95bdbUL)
        {
            this.seed = seed;

            state = 0U;
            inc = (initSeq << 1) | 1UL;
            Int();
            state += seed;
            Int();
        }

        public int Int()
        {
            int result = (int)Uint();
            if (result < 0) result = -result;
            return result;
        }

        public ushort Ushort()
        {
            return (ushort)Uint();
        }

        public int Int(int exMax)
        {
            int result = (int)Uint((uint)exMax);
            if (result < 0) result = -result;
            return result;
        }

        /// <summary>
        /// Higher probability for small values
        /// </summary>
        public int Int_HighToLowProbability1(int exMax)
        {
            uint max = (uint)exMax;
            uint val1 = Uint(max);
            uint val2 = Uint(max);

            int result = (int)Math.Min(val1, val2);
            if (result < 0) result = -result;
            return result;
        }

        /// <summary>
        /// Higher probability for small values
        /// </summary>
        public int Int_HighToLowProbabilityHalf(int exMax)
        {
            uint max = (uint)exMax;
            uint val1 = Uint(max);

            if (Double() < 0.5)
            {
                uint val2 = Uint(max);

                int result = (int)Math.Min(val1, val2);
                if (result < 0) result = -result;
                return result;
            }
            else
            {
                return (int)val1;
            }
        }

        public int Int(int min, int exMax)
        {
            return Int(exMax - min) + min;
        }

        public int Interval(int min, int incMax)
        {
            return Int(incMax + 1 - min) + min;
        }

        public byte Byte()
        {
            return (byte)Uint(byte.MaxValue);
        }

        public uint Uint()
        {
            ulong oldState = state;
            state = oldState * 6364136223846793005UL + inc;
            uint xorShifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
            int rot = (int)(oldState >> 59);
            uint result = (xorShifted >> rot) | (xorShifted << ((-rot) & 31));
            return result;
        }

        public uint Uint(uint exMax)
        {
            if (exMax == 0) exMax = 1;
            uint threshold = (uint)((0x100000000UL - exMax) % exMax);
            for (;;)
            {
                uint r = Uint();
                if (r >= threshold)
                {
                    return r % exMax;
                }
            }
        }

        public uint Uint(uint min, uint exMax)
        {
            return Uint(exMax - min) + min;
        }

        

        public double Double()
        {
            return Uint() * UintDiv;/// (double)uint.MaxValue;
        }

        public double Double(double min, double exMax)
        {
            double diff = exMax - min;
            var result = Uint();
            return (result * UintDiv * diff) + min;/// (double)uint.MaxValue) * diff + min;
        }

        public float Float()
        {
            return (float)(Uint() * UintDiv);/// (double)uint.MaxValue);
        }

        public float Float(float exMax)
        {
            return (float)(Uint() * UintDiv * exMax);/// (double)uint.MaxValue) * exMax;
        }

        public float Float(float min, float exMax)
        {
            float diff = exMax - min;
            var result = Uint();
            return (float)(result * UintDiv * diff) + min;/// (double)uint.MaxValue) * diff + min;
        }

        public bool Chance(double chance)
        {
            return Double() <= chance;
        }
        public bool Chance(int percent)
        {
            return Double() <= percent * 0.01;
        }

        public bool RandomChanceTime(double seconds)
        {
            return (int)(Double() * (seconds * 60)) == 0;
        }

        public int Plus_Minus(int range)
        {
            return (int)Uint((uint)(range * 2 +1)) - range;
        }

        public float Plus_MinusF(float range)
        {
            return (float)(-range + 2.0 * range * Double());
        }

        public float Plus_MinusPercent(float center, float percRange)
        {
            return center + Plus_MinusF(percRange * center);
        }

        public int Plus_MinusPercent(int center, float percRange)
        {
            return center + Plus_Minus(Convert.ToInt32(percRange * center));
        }

        /// <summary>
        /// Square shaped random 3D position
        /// </summary>
        public Vector3 Vector3_Sq(Vector3 center, float range)
        {
            center.X += (float)(-range + Double() * 2.0 * range);
            center.Y += (float)(-range + Double() * 2.0 * range);
            center.Z += (float)(-range + Double() * 2.0 * range);
            return center;
        }

        /// <summary>
        /// Square shaped random 3D position
        /// </summary>
        public Vector3 Vector3_Sq(Vector3 range)
        {
            return new Vector3(
                (float)(-range.X + Double() * 2.0 * range.X),
                (float)(-range.Y + Double() * 2.0 * range.Y),
                (float)(-range.Z + Double() * 2.0 * range.Z)
                );
        }

        /// <summary>
        /// Shpere shaped random 3D position, WARNING not tested
        /// </summary>
        public Vector3 Vector3_Sph()
        {
            Vector3 result = new Vector3(
                (float)(Double() * 2.0 - 1.0),
                (float)(Double() * 2.0 - 1.0),
                (float)(Double() * 2.0 - 1.0));

            float l = result.Length();
            if (l > 1f)
            {
                result /= l;
            }

            return result;

            //double a1 = Double() * Math.PI;
            //double a2 = Double() * Math.PI * 2.0;
            //return new Vector3((float)(Math.Sin(a1) * Math.Cos(a2)),
            //                   (float)(Math.Sin(a1) * Math.Sin(a2)),
            //                   (float)(Math.Cos(a1)));
        }

        public Vector2 vector2_cirkle(float length = 1f)
        {
            return lib.AngleToV2((float)(Math.PI * 2.0 * Double()), length);
        }

        public Vector2 vector2(Vector2 exMax)
        {
            return new Vector2(
                (float)(Uint() * UintDiv * exMax.X),
                (float)(Uint() * UintDiv * exMax.Y));
        }

        public Vector2 vector2(float exMaxX, float exMaxY)
        {
            return new Vector2(
                (float)(Uint() * UintDiv * exMaxX),
                (float)(Uint() * UintDiv * exMaxY));
        }

        const uint UnitMidValue = uint.MaxValue / 2;
        public bool Bool()
        {
            return Uint() > UnitMidValue;
        }

        public int LeftRight()
        {
            return Uint() > UnitMidValue ? 1 : -1;
        }

        public float Rotation()
        {
            return (float)(Math.PI * 2.0 * Double());
        }

        public override string ToString()
        {
            return string.Format("[State: {0}; Seed: {1}]", state, seed);
        }

        //FABIAN ENGINE SPECIFIC - all below
        public Dir4 Dir4()
        {
            return (Dir4)Uint(4);
        }

        public Dir8 Dir8()
        {
            return (Dir8)Uint(8);
        }

        public IntVector2 IntVector2_Tile(IntVector2 exMax)
        {
            return new IntVector2(Uint((uint)exMax.X), Uint((uint)exMax.Y));
        }

        // Note the standard deviation parameter - do not pass variance.
        public float GaussianDistribution(float mean, float stdDev)
        {
            double u1 = Double();
            double u2 = Double();

            // Normal(0,1)
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(u2 * MathHelper.TwoPi);

            // Normal(mean, stdDev^2)
            return (float)(mean + stdDev * randStdNormal);
        }
    }
}
