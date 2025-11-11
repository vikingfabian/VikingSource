using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    using System;
    using System.Runtime.CompilerServices;



    /// <summary>
    /// A garbage-free counter for iterating over an entity’s subarray.
    /// </summary>
    struct EcsStaticArrayCounter
    {
        EcsStaticArray array;
        int end;
        public int index;

        public EcsStaticArrayCounter(EcsStaticArray array, int entityIndex, int count)
        {
            this.array = array;
            array.LoopSpan(entityIndex, count, out index, out end);
        }

         public bool Next(out int value)
        {
            index++;
            if (index < end)
            {
                value = array.array[index];
                return true;
            }

            value = int.MinValue;
            return false;
        }

        public bool Next<T>(List<T> list, out T value)
        {
            index++;
            if (index < end)
            {
                int idx = array.array[index];
                value = list[idx];
                return true;
            }

            value = default;
            return false;
        }
    }
    struct EcsStaticArray
    {
        // Large shared flat array
        public int[] array;
        int arrayLength;

        public EcsStaticArray(int arrayLength, int entityCount)
        {
            this.arrayLength = arrayLength;
            array = new int[arrayLength * entityCount];
        }

        /// <summary>
        /// Adds a value to this entity’s subarray.
        /// The entity’s count is passed by ref and incremented.
        /// </summary>
        public void Add(int entityIndex, ref int count, int value, bool containCheck = false)
        {            
            if (count >= arrayLength)
                throw new InvalidOperationException("Entity subarray is full.");

            int start = entityIndex * arrayLength;

            if (containCheck)
            { 
                int exEnd = start + count;
                for (int i = start; i < exEnd; i++)
                {
                    if (array[i] == value)
                    {
                        //Already contains value, abort
                        return;
                    }
                }
            }
           
            array[start + count] = value;
            count++;
        }

        public void LoopSpan(int entityIndex, int count, out int start, out int exEnd)
        {
            start = entityIndex * arrayLength;
            exEnd = start + count;
        }

    }

}
