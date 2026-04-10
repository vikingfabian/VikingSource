using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.EngineSpace
{


    struct ExampleStruct1
    {
        public bool isDeleted;
    }
    struct ExampleStruct2
    {
        public bool isDeleted;
    }


    //GC optimized list that exposes the array for ref usage
    //Example: ref WorkerStatus status = ref workerStatuses.array[i];
    struct StructList<T> where T : struct
    {
        public T[] array;
        public int Count;

        //public int Count => _count;
        public int Capacity => array.Length;

        public StructList(int initialCapacity)
        {
            if (initialCapacity == 0)
            {
                array = null;
            }
            else
            {
                array = new T[initialCapacity];
            }
            Count = 0;
        }

        public StructList()
            : this(16)
        {
        }

        public void Init(int initialCapacity)
        {
            array = new T[initialCapacity];
        }

        public void Add(T item)
        {
            if (Count >= array.Length)
                Resize();

            array[Count++] = item;
        }

        public ref T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();

                return ref array[index]; //ref access!
            }
        }

        public void Clear()
        {
            Count = 0;
        }

        public void Resize()
        {
            Resize(array.Length * 2);
        }

        public void Resize(int newSize)
        {
            Array.Resize(ref array, newSize);
        }

        /// <summary>
        /// places the last element in the empty spot
        /// </summary>
        public void RemoveAtSwapBack(int index)
        {
            if (index < 0 || index >= Count) return;
            Count--;
            array[index] = array[Count];
        }

        /// <summary>
        /// Removes the element at the index and shifts subsequent elements down.
        /// Preserves order.
        /// </summary>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count) return; // Or throw ArgumentOutOfRangeException

            for (int i = index; i < Count - 1; i++)
            {
                array[i] = array[i + 1];
            }

            Count--;
        }

        public static bool Example1_FindNextAlive(StructList<ExampleStruct1> array, ref int index)
        {
            while (index < array.Count)
            {
                if (array[index].isDeleted)
                    ++index;
                else
                    return true;
            }

            return false;
        }
        public static bool Example2_FindNextAlive(StructList<ExampleStruct2> array, ref int index)
        {
            while (index < array.Count)
            {
                if (array[index].isDeleted)
                    ++index;
                else
                    return true;
            }

            return false;
        }

        public bool InBound_List(int index)
        {
            return index >= 0 && index < Count;
        }
        public bool InBound_Array(int index)
        {
            return index >= 0 && index < array.Length;
        }
    }    
}
