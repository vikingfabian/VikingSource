
namespace VikingEngine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// More optimized and feature rich list, make sure to always pass as "ref" since its a struct!
    /// </summary>
    public struct List2<T> : IEnumerable<T>
    {
        public T[] Array;
        //public T sel = default;
        public int selIndex = -1;
        public int Count = 0;

        // Handles the null array scenario
        public int Capacity => Array?.Length ?? 0;

        public List2(int capacity)
        {
            Array = new T[capacity];
        }

        public T PullIndex(int index)
        {
            T result = Array[index];
            RemoveAt(index);
            return result;
        }

        public void SelectRandom()
        {
            selIndex = Ref.rnd.Int(Count);
        }

        // 1. Classic RemoveAt: Shifts all subsequent elements left
        public void RemoveAt(int index)
        {
            ValidateIndex(index);
            Count--;
            if (index < Count)
            {
                System.Array.Copy(Array, index + 1, Array, index, Count - index);
            }
            Array[Count] = default;
        }

        // 2. Performant RemoveAt: Swaps the last element into the removed slot
        public void RemoveAtSwapBack(int index)
        {
            ValidateIndex(index);
            Count--;
            if (index < Count)
            {
                Array[index] = Array[Count];
            }
            Array[Count] = default;
        }

        // 3. Locked Index RemoveAt: Replaces with default, leaves Count intact so indices don't shift
        public void RemoveAtPreserveIndex(int index)
        {
            ValidateIndex(index);
            Array[index] = default;
            // Note: Count is not decremented to maintain array alignment
        }

        // Selection Methods
        //public void Next()
        //{
        //    int idx = IndexOf(sel);
        //    if (idx >= 0 && idx < Count - 1)
        //    {
        //        sel = Array[idx + 1];
        //    }
        //}

        //public void Previous()
        //{
        //    int idx = IndexOf(sel);
        //    if (idx > 0)
        //    {
        //        sel = Array[idx - 1];
        //    }
        //}

        public T Sel()
        {
            return Array[selIndex];
        }
        public T Sel_safe()
        {
            if (selIndex >= 0 && selIndex < Count)
            {
                return Array[selIndex];
            }
            return default;
        }
        public ref T SelRef()
        {
            return ref Array[selIndex];
        }

        // Standard List Features
        public ref T this[int index]
        {
            get
            {
                ValidateIndex(index);
                return ref Array[index];
            }
        }

        public void Add(T item)
        {
            EnsureCapacity(Count + 1);
            Array[Count++] = item;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public void Clear()
        {
            if (Array != null)
            {
                System.Array.Clear(Array, 0, Count);
            }
            Count = 0;
            selIndex = -1;
        }

        public bool Contains(T item) => IndexOf(item) >= 0;

        public int IndexOf(T item)
        {
            if (Array == null) return -1;
            return System.Array.IndexOf(Array, item, 0, Count);
        }

        private void EnsureCapacity(int min)
        {
            if (Array == null)
            {
                Array = new T[min == 0 ? 4 : min];
                return;
            }

            if (Array.Length < min)
            {
                int newCapacity = Array.Length == 0 ? 4 : Array.Length * 2;
                if (newCapacity < min) newCapacity = min;

                T[] newArray = new T[newCapacity];
                System.Array.Copy(Array, newArray, Count);
                Array = newArray;
            }
        }

        private void ValidateIndex(int index)
        {
            if ((uint)index >= (uint)Count)
                throw new ArgumentOutOfRangeException(nameof(index));
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return Array[i];
            }
        }

        public bool HasSelection()
        {
            return selIndex >= 0;
        }
        public void UnSelect()
        {
            selIndex = -1;
        }
        public bool HasValue()
        {
            return Count > 0;
        }

        public bool IsEmpty()
        {
            return Count == 0;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
