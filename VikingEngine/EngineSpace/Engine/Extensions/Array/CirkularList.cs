using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    struct ListCirkleCounter<T>
    {
        List<T> list;
        int startIndex;
        public int currentIndex;
        public ListCirkleCounter(List<T> list, int startIndex)
        {
            this.list = list;
            this.startIndex = startIndex;
            currentIndex = startIndex;
        }

        /// <summary>
        /// Do ... while (Next());
        /// </summary>
        /// <returns></returns>
        public bool Next()
        {
            currentIndex++;
            if (currentIndex >= list.Count)
                currentIndex = 0;

            return currentIndex != startIndex;
        }

        public T CurrentValue {  get {  return list[currentIndex]; } }
    }

    /// <summary>
    /// List with floating start index, good performance when pulling/adding at the start of the array
    /// </summary>
    class CirkularList<T>
    {
        int startPosition = 0;
        public int Length;
        public T[] Array;

        public CirkularList()
            : this(4)
        { }
        public CirkularList(int length)
        {
            Array = new T[length];
        }

        int publicToLocalIndex(int publicIndex)
        {
            int ix = startPosition + publicIndex;

            if (ix >= Array.Length)
            { ix %= Array.Length; }
            else
            {
                while (ix < 0)
                {
                    ix += Array.Length;
                }
            }

            return ix;
        }

        public T Get(int cirkleIndex)
        {
            return Array[publicToLocalIndex(cirkleIndex)];
        }

        public void AddLast(T value, bool overrideTail = false)
        {
            //Search up the next empty spot
            if (Length == Array.Length)
            {
                if (overrideTail)
                {
                    removeFirst();
                }
                else
                {
                    adjustLength();
                }
            }
            Array[publicToLocalIndex(Length)] = value;
            ++Length;
        }

        public void AddFirst(T value)
        {
            //Search up the next empty spot
            if (Length == Array.Length)
            {
                adjustLength();
            }
            --startPosition;
            if (startPosition < 0)
            {
                startPosition += Array.Length;
            }
            Array[startPosition] = value;
            ++Length;
        }

        public T PullFist()
        {
            if (Length <= 0) return default(T);
            T result = Array[startPosition];
            Array[startPosition] = default(T);
            removeFirst();
            return result;
        }

        public void removeFirst()
        {
            ++startPosition;
            --Length;
            if (startPosition >= Array.Length)
                startPosition -= Array.Length;
        }

        public T PullLast()
        {
            if (Length <= 0) return default(T);
            int ix = publicToLocalIndex(Length - 1);
            T result = Array[ix];
            Array[ix] = default(T);
            --Length;
            return result;
        }
        
        void adjustLength()
        {
            T[] newArray = new T[Array.Length * 2];
            for (int i = 0; i < Array.Length; ++i)
            {
                newArray[i] = Array[publicToLocalIndex(i)];
            }
            startPosition = 0;
            Array = newArray;
        }

        public void Clear()
        {
            for (int i = 0; i < Length; ++i)
            {
                Array[publicToLocalIndex(i)] = default(T);
            }
        }

        public static void Test()
        {
            CirkularList<int> testList = new CirkularList<int>();
            for (int i = 0; i < 8; ++i)
            {
                testList.AddLast(i);
            }

            testList.AddFirst(-1);

            
            if (testList.PullLast() != 7)
                throw new Exception();


            if (testList.PullFist() != -1)
                throw new Exception();

            if (testList.PullLast() != 6)
                throw new Exception();


            if (testList.PullFist() != 0)
                throw new Exception();


        }
    }
}
