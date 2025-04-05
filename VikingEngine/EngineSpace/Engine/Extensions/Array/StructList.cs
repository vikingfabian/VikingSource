using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.EngineSpace
{

    //GC optimized list that exposes the array for ref usage
    //Example: ref WorkerStatus status = ref workerStatuses.array[i];
    public struct StructList<T> where T : unmanaged
    {
        public T[] array;
        public int _count;

        public int Count => _count;
        public int Capacity => array.Length;

        public StructList(int initialCapacity)
        {
            array = new T[initialCapacity];
            _count = 0;
        }

        public void Add(T item)
        {
            if (_count >= array.Length)
                Resize(array.Length * 2);

            array[_count++] = item;
        }

        public ref T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();

                return ref array[index]; // ✅ ref access!
            }
        }

        public void Clear()
        {
            _count = 0;
        }

        //public T[] RawArray => _array;

        private void Resize(int newSize)
        {
            Array.Resize(ref array, newSize);
        }
    }
}
