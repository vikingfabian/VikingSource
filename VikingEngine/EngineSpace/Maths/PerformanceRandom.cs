using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.EngineSpace.Maths
{
    
        class PerformanceRandom : AbsRandom
        {
            private const int ARRAY_SIZE = 100; // Replaces hardcoded 100
            private int index = 0;
            private readonly float[] values;
            private readonly int[] intValues;
            private readonly uint[] uintValues;
            private readonly bool[] boolValues;
            private readonly byte[] byteValues;
            private readonly ushort[] ushortValues;

            public PerformanceRandom()
            {
                // Generate shuffled float values (1f to 0.01f)
                values = Enumerable.Range(0, ARRAY_SIZE)
                                   .Select(i => 1f - (i * 0.01f))
                                   .ToArray();
                Shuffle(values);

                // Generate other precomputed random values
                intValues = Enumerable.Range(0, ARRAY_SIZE).ToArray();
                Shuffle(intValues);

                uintValues = intValues.Select(i => (uint)i).ToArray();
                Shuffle(uintValues);

                boolValues = Enumerable.Range(0, ARRAY_SIZE).Select(_ => new Random().Next(2) == 1).ToArray();
                Shuffle(boolValues);

                byteValues = Enumerable.Range(0, ARRAY_SIZE).Select(_ => (byte)new Random().Next(256)).ToArray();
                Shuffle(byteValues);

                ushortValues = Enumerable.Range(0, ARRAY_SIZE).Select(_ => (ushort)new Random().Next(ushort.MaxValue)).ToArray();
                Shuffle(ushortValues);
            }

            private void Shuffle<T>(T[] array)
            {
                Random rng = new Random();
                for (int i = array.Length - 1; i > 0; i--)
                {
                    int j = rng.Next(i + 1);
                    (array[i], array[j]) = (array[j], array[i]); // Swap
                }
            }

            // Explicit methods to avoid boxing
            public override float Float()
            {
                if (++index >= ARRAY_SIZE) index = 0;
                return values[index];
            }

            public override int Int()
            {
                if (++index >= ARRAY_SIZE) index = 0;
                return intValues[index];
            }

            public override int Int(int exMax)
            {
                if (++index >= ARRAY_SIZE) index = 0;
                return intValues[index] % exMax;
            }

            public override int Int(int min, int exMax)
            {
                if (++index >= ARRAY_SIZE) index = 0;
                return min + (intValues[index] % (exMax - min));
            }

            public override uint Uint()
            {
                if (++index >= ARRAY_SIZE) index = 0;
                return uintValues[index];
            }

            public override uint Uint(uint exMax)
            {
                if (++index >= ARRAY_SIZE) index = 0;
                return uintValues[index] % exMax;
            }

            public override bool Bool()
            {
                if (++index >= ARRAY_SIZE) index = 0;
                return boolValues[index];
            }

            public override byte Byte()
            {
                if (++index >= ARRAY_SIZE) index = 0;
                return byteValues[index];
            }

            public override ushort Ushort()
            {
                if (++index >= ARRAY_SIZE) index = 0;
                return ushortValues[index];
            }
        }

    
}
