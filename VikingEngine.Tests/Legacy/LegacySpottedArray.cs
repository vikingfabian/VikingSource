using System;
using System.Collections.Generic;

namespace VikingEngine.Tests.Legacy
{
    /// <summary>
    /// Snapshot of the legacy SpottedArray before Phase 4 TrimExcess.
    /// In the legacy implementation, Array grows by doubling and never shrinks, retaining peak capacity permanently.
    /// </summary>
    class LegacySpottedArray<T>
    {
        public int mostLeftFreePosition = 0;
        public int SpottedLength;
        public int Count;
        public T[] Array;

        public LegacySpottedArray(int length = 4)
        {
            Array = new T[length];
        }

        public int Add(T obj)
        {
            int placementIndex = NextAvailableIndex();
            if (placementIndex >= Array.Length)
            {
                adjustLength();
            }

            Array[placementIndex] = obj;
            SpottedLength = Math.Max(SpottedLength, placementIndex + 1);
            ++Count;
            mostLeftFreePosition = placementIndex + 1;
            return placementIndex;
        }

        public int NextAvailableIndex()
        {
            int result = mostLeftFreePosition;
            if (result >= Array.Length)
            {
                return Array.Length;
            }
            else
            {
                while (Array[result] != null)
                {
                    ++result;
                    if (result >= Array.Length)
                    {
                        return Array.Length;
                    }
                }
            }
            return result;
        }

        public void RemoveAt(int index)
        {
            --Count;
            Array[index] = default(T)!;
            mostLeftFreePosition = Math.Min(mostLeftFreePosition, index);
            updateSpottedLength();
        }

        void updateSpottedLength()
        {
            while (SpottedLength > 0 && Array[SpottedLength - 1] == null)
            {
                --SpottedLength;
            }
        }

        public void adjustLength()
        {
            int toLength = Math.Max(Array.Length * 2, 8);
            T[] newArray = new T[toLength];
            System.Array.Copy(this.Array, 0, newArray, 0, this.Array.Length);
            this.Array = newArray;
        }
    }
}
