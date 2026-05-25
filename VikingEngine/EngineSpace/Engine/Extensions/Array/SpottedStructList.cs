using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.LootFest.GO.Gadgets;

namespace VikingEngine
{
    
    struct StructListId
    { 
        public int index;
        public int version;

        public StructListId(int index, int version)
        {
            this.index = index;
            this.version = version;
        }
    }

    interface ISpottedStructListMember
    {
        int Version { get; set; }
        bool HasValue { get; set; }

        /// <summary>
        /// Only used if the item is in a chain of items
        /// </summary>
        StructListId NextChainId();
    }

    struct ExampleSpotStruct : ISpottedStructListMember
    {
        public bool HasValue { get; set; }
        public int Version { get; set; }

        public StructListId NextChainId() 
        {
            throw new NotImplementedException();
        }

    }

    //GC optimized list that exposes the array for ref usage
    //Example: ref WorkerStatus status = ref workerStatuses.array[i];
    struct SpottedStructList<T> where T : struct, ISpottedStructListMember
    {
        public T[] array;
        public int Count;
        public int mostLeftFreePosition = 0;
        /// <summary>
        /// The used part of the array, can be greater than the member count
        /// </summary>
        public int SpottedLength;
        public int Capacity => array.Length;

        public SpottedStructList(int initialCapacity)
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

        public SpottedStructList()
            : this(16)
        {
        }

        public void Init(int initialCapacity)
        {
            array = new T[initialCapacity];
        }

        //public void Add(T item)
        //{
        //    if (Count >= array.Length)
        //        Resize();

        //    array[Count++] = item;
        //}
        public StructListId Add(T item)
        {
            int placementIndex = NextAvailableIndex();
            if (placementIndex >= array.Length)
                Resize();

            SpottedLength = lib.LargestValue(SpottedLength, placementIndex + 1);
            ++Count;
            mostLeftFreePosition = placementIndex + 1;
            
            //return placementIndex;
            int version = array[placementIndex].Version + 1;
            item.Version = version;
            array[placementIndex] = item;

            return new StructListId(placementIndex, version);
        }

        public int NextAvailableIndex()
        {
            int result = mostLeftFreePosition;

            if (result >= array.Length)
            { return array.Length; }
            else
            {
                while (array[result].HasValue)
                {
                    ++result;
                    if (result >= array.Length)
                    {
                        return array.Length;
                    }
                }
            }

            return result;
        }

        //public StructListId Add_ReturnId(T item)
        //{
        //    if (Count >= array.Length)
        //        Resize();

        //    //StructListId id = new StructListId() { index = Count };
        //    int index = Count++;
        //    int version = array[index].Version + 1;
        //    item.Version = version;
        //    array[index] = item;

        //    return new StructListId(index, version);
        //}

        public T Get(StructListId id)
        {
            T item = array[id.index];
            if (item.Version == id.version && item.HasValue)
            { 
                return item;
            }
            return default;
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
        /// Removes the element at the index and shifts subsequent elements down.
        /// Preserves order.
        /// </summary>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count) return; // Or throw ArgumentOutOfRangeException

            array[index].HasValue = false;
            array[index].Version++;
            mostLeftFreePosition = lib.SmallestValue(mostLeftFreePosition, index);

            --Count;
            updateSpottedLength();
        }

        void updateSpottedLength()
        {
            while (SpottedLength > 0 && !array[SpottedLength - 1].HasValue)
            {
                --SpottedLength;
            }
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
