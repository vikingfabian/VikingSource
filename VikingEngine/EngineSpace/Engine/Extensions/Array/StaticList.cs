using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    /// <summary>
    /// Same properties as a list except it cant change length due to thread safety
    /// </summary>
    class StaticList<T>
    {
        public T[] Array;
        public int Count = 0;
        public int PreviousCount = 0;
        public int usingIndex = 0;

        public bool resizeable = false;

        public StaticList(int length, bool resizeable = false)
        {
            this.resizeable = resizeable;
            Array = new T[length];
        }

        public bool TryAdd(T obj)
        {
            if (Count < Array.Length)
            {
                Add(obj);
                return true;
            }
            return false;
        }

        public int Add(T obj)
        {
            int index = Count;
            Array[index] = obj;
            ++Count;

            if (resizeable && Count == Array.Length)
            {
                T[] array2 = new T[Count * 2];
                for (int i = 0; i < Count; ++i)
                {
                    array2[i] = Array[i];
                }

                Array = array2;
            }
            return index;
        }
        public void AddRange(List<T> array)
        {
            foreach (T obj in array)
            {
                Add(obj);
            }
        }

        public void AddRangeToList(List<T> list)
        {
            for (int i = 0; i < Count; ++i)
            {
                list.Add(Array[i]);
            }
        }

        public T PullLast()
        {
            if (Count > 0 && Count <= Array.Length)
            {
                --Count;
                int pullIndex = Count;
                if (pullIndex >= 0 && pullIndex < Array.Length)
                {
                    T result = Array[Count];
                    Array[Count] = default(T);
                    return result;
                }
            }
            return default(T);
        }

        public T UseNext()
        {
            if (Count > 0 && usingIndex < Count)
            {
                return Array[usingIndex++];
            }
            return default(T);
        }
        
        public T this[int index] { get { return Array[index]; } set { Array[index] = value; } }

        public T NextIteration(ref int i)
        {
            if (i >= Array.Length) return default(T);
            return Array[i++];
        }

        public bool NextIsNull()
        {
            return Count + 1 == Array.Length || Array[Count] == null;
        }

        public T RecycleNext()
        {
            return Array[Count++];
        }

        public void Clear()
        {
            Count = 0;
            for (int i = 0; i < Array.Length; ++i)
            {
                Array[i] = default(T);
            }
        }

        public void Remove(T obj)
        {
            for (int i = Count -1; i >= 0; --i)
            {
                if (Array[i].Equals(obj))
                {
                    RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveAt(int index)
        {
            //Move all member, after the index, one step left
            for (int i = index + 1; i < Count; ++i)
            {
                Array[i - 1] = Array[i];
            }
            Array[Count -1] = default(T);
            --Count;
        }

        public void QuickClear()
        {
            PreviousCount = Count;
            Count = 0;
        }

        virtual public T[] ToArray()
        {
            T[] result = new T[Count];
            for (int i = 0; i < Count; ++i)
            {
                result[i] = Array[i];
            }

            return result;
        }

        public bool Contains(T value)
        {
            return Array.Contains(value);
        }

        public T GetRandom()
        {
            if (Count <= 0)
                return default(T);
            return Array[Ref.rnd.Int(Count)];
        }

        public bool IsFull { get { return Count >= Array.Length; } }
        public int UnusedArrayLength { get { return Array.Length - Count; } }
        public int UseNextLeft { get { return Count - usingIndex; } }

        public StaticList_PreviousCounter<T> PrevCounter()
        {
            return new StaticList_PreviousCounter<T>(this);
        }

        public List<T> previousToList()
        {
            List<T> result = new List<T>(Count);
            var counter = new StaticList_PreviousCounter<T>(this);
            while (counter.Next())
            {
                result.Add(counter.sel);
            }

            return result;
        }
    }

    class StaticCountingList<T> : StaticList<T>
    {
        public T CurrentMember;
        int currentIndex;
        public int CountingLength { get { return currentIndex; } }

        public StaticCountingList(int length)
            :base(length)
        { }

        public void FillArrayWith(T defaultVal)
        {
            for (int i = 0; i < Array.Length; ++i)
            {
                Array[i] = defaultVal;
            }
        }

        public void ResetCounting()
        {
            currentIndex = 0;
            CurrentMember = Array[0];
        }

        public void NextIndex()
        {
            Array[currentIndex] = CurrentMember;
            CurrentMember = Array[++currentIndex];
        }

        public void SetCurrent(T obj)
        {
            CurrentMember = obj;
        }
        public void SetCurrentAndMoveNext(T obj)
        {
            Array[currentIndex++] = obj;
        }

        override public T[] ToArray()
        {
            T[] result = new T[currentIndex];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = Array[i];
            }

            return result;
        }
    }

    struct StaticList_PreviousCounter<T>
    {
        public T sel;
        public int selIndex;
        StaticList<T> list;

        public StaticList_PreviousCounter(StaticList<T> list)
        {
            sel = default(T);
            selIndex = -1;
            this.list = list;
        }

        public void Reset()
        {
            selIndex = -1;
            sel = default(T);
        }

        public bool Next()
        {
            ++selIndex;
            for (; selIndex < list.PreviousCount; ++selIndex)
            {
                sel = list[selIndex];
                if (sel != null)
                    return true;
            }

            return false;
        }

        
    }
}
