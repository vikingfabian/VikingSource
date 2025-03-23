using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.HeroQuest.Display;

namespace VikingEngine.EngineSpace.Maths
{
    
    public class PerformanceRandom : AbsRandom
    {
        private const int VALUE_SIZE = 224; // Replaces hardcoded 100
        private int index = 0;
        private readonly float[] percValuesF;

        public PerformanceRandom()
        {
            Random rng = new Random();
            // Generate shuffled float values (1f to 0.01f)
            const float Step = 1f / VALUE_SIZE;

            //Larger than VALUE_SIZE for thread safety
            percValuesF = new float[256];

            for (int i = 0; i < VALUE_SIZE; ++i)
            {
                percValuesF[i] = 1f - Step - (i * Step);
            }

            for (int i = VALUE_SIZE; i < percValuesF.Length; ++i)
            {
                percValuesF[i] = 0.5f;
            }

            Shuffle(percValuesF, rng);

        }

        private void Shuffle<T>(T[] array, Random rng)
        {
                
            for (int i = VALUE_SIZE - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]); // Swap
            }
        }

            // Explicit methods to avoid boxing
        public float PercentF()
        {
            if (++index >= VALUE_SIZE) index = 0;
            return percValuesF[index];
        }

        public override float Rotation()
        {
            if (++index >= VALUE_SIZE) index = 0;
            return MathHelper.TwoPi * percValuesF[index];
        }

        public override bool Chance(double chance)
        {
            if (++index >= VALUE_SIZE) index = 0;
            return percValuesF[index] < chance;
        }

        public bool Chance_CheckForZero(float chance)
        {
            if (chance<= 0)
                return false;

            if (++index >= VALUE_SIZE) index = 0;
            return percValuesF[index] < chance;
        }

        public override bool Chance(int percent)
        {
            if (++index >= VALUE_SIZE) index = 0;
            return percValuesF[index] < percent * MathExt.OnePercentage;
        }

        public override float Float()
        {
            if (++index >= VALUE_SIZE) index = 0;
            return percValuesF[index] * float.MaxValue;
        }
        public override float Float(float exMax)
        {
            if (++index >= VALUE_SIZE) index = 0;
            return percValuesF[index] * exMax;
        }

        public override float Float(float min, float exMax)
        {
            if (++index >= VALUE_SIZE) index = 0;
            return min + percValuesF[index] * (exMax - min);
        }
        public override float Plus_MinusF(float range)
        {
            if (++index >= VALUE_SIZE) index = 0;
            return range - percValuesF[index] * range * 2f;
        }

        /// <summary>
        /// Square shaped random 3D position
        /// </summary>
        override public Vector3 Vector3_Sq(Vector3 center, float range)
        {
            if (index+3 >= VALUE_SIZE) index = 0;
            
            center.X += range - percValuesF[index] * range * 2f;
            center.Y += range - percValuesF[index + 1] * range * 2f;
            center.Z += range - percValuesF[index + 2] * range * 2f;

            index += 3;
            return center;
        }

        public Vector3 Vector3_SqXZ(Vector3 center, float range)
        {
            if (index + 2 >= VALUE_SIZE) index = 0;

            center.X += range - percValuesF[index] * range * 2f;
            center.Z += range - percValuesF[index + 1] * range * 2f;

            index += 2;
            return center;
        }

        public override double Double()
        {
            if (++index >= VALUE_SIZE) index = 0;
            return percValuesF[index] * double.MaxValue;
        }

        public override double Double(double exMax)
        {
            return PercentF() * exMax;
        }

        public override double Double(double min, double exMax)
        {
            return min + PercentF() * (exMax - min);
        }

        public override int Int()
        {
            if (++index >= VALUE_SIZE) index = 0;
            return (int)(percValuesF[index] * int.MaxValue);
        }

        public override int Int(int exMax)
        {
            if (++index >= VALUE_SIZE) index = 0;
            return (int)(percValuesF[index] * exMax);
        }

        public override int Int(int min, int exMax)
        {
            if (++index >= VALUE_SIZE) index = 0;
            return min + (int)(percValuesF[index] * (exMax - min));
        }

        //public override uint Uint()
        //{
        //    if (++index >= ARRAY_SIZE) index = 0;
        //    return uintValues[index];
        //}

        //public override uint Uint(uint exMax)
        //{
        //    if (++index >= ARRAY_SIZE) index = 0;
        //    return uintValues[index] % exMax;
        //}

        public override bool Bool()
        {
            if (++index >= VALUE_SIZE) index = 0;
            return percValuesF[index] > 0.5f;
        }

        public override byte Byte()
        {
            if (++index >= VALUE_SIZE) index = 0;
            return (byte)(percValuesF[index] * byte.MaxValue);
        }

        public override ushort Ushort()
        {
            if (++index >= VALUE_SIZE) index = 0;
            return (ushort)(percValuesF[index] * ushort.MaxValue);
        }
    }

    
}
