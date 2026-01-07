using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.LootFest.GO.Gadgets;

namespace VikingEngine
{
    public class ArrayExposedList<T>
    {
        public T[] array;
        public int count;

        public ArrayExposedList(int capacity)
        {
            array = new T[capacity];
            count = 0;
        }

        public ArrayExposedList(List<T> list)
        {
            array = list.ToArray();
            count = list.Count;
        }

        public void Add(T item)
        {
            if (count >= array.Length)
            {
                Array.Resize(ref array, array.Length * 2); // Resize if needed
            }
            array[count++] = item;
        }

        public void AddRange(List<T> list)
        {
            EnsureCapacity(count + list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                array[count++] = list[i];
            }
        }

        public void setLenght(int length)
        {
            EnsureCapacity(length);
            count = length;
        }

        private void EnsureCapacity(int requiredCapacity)
        {
            if (requiredCapacity > array.Length)
            {
                int newCapacity = Math.Max(array.Length * 2, requiredCapacity);
                Array.Resize(ref array, newCapacity);
            }
        }

        public void Clear()
        {
            count = 0;
        }
    }

}
