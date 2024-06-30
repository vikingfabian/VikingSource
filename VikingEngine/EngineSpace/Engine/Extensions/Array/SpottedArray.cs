using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    interface ISpottedArrayCounter<T>
    {
        bool Next();
        T GetSelection { get; }
        void Reset();
        void RemoveAtCurrent();
        T GetFromIndex(int index);
        ISpottedArrayCounter<T> IClone();
        int CurrentIndex { get; }
    }

    interface ISpottedArrayMember
    {
        int SpottedArrayMemberIndex { get; set; }
        bool SpottedArrayUseIndex { get; }
    }

    struct SpottedArrayCounter<T> : ISpottedArrayCounter<T>
    {
        int selIndex;
        public SpottedArray<T> array;
        public T sel;

        public SpottedArrayCounter(SpottedArray<T> array)
            :this()
        {
            this.array = array;
            Reset();
        }

        /// <summary>
        /// To be used in a while-loop
        /// </summary>
        public bool Next()
        {
            var safePointer1 = array.NextIteration(ref selIndex);
            sel = safePointer1;
            return sel != null;
        }

        public bool Prev_Rollover()
        {
            if (array.Count == 0)
            {
                return false;
            }

            sel = array.NextIteration(ref selIndex);
            if (sel == null)
            {
                selIndex = -1;
                sel = array.NextIteration(ref selIndex);
            }
            return sel != null;
        }

        public bool Next_Rollover()
        {
            if (array.Count == 0) 
            { 
                return false;
            }

            sel = array.PrevIteration(ref selIndex);
            if (sel == null)
            {
                selIndex = array.Array.Length;
                sel = array.PrevIteration(ref selIndex);
            }
            return sel != null;
        }

        public void Reset(SpottedArray<T> array)
        {
            this.array = array;
            Reset();
        }

        public void Reset()
        {
            this.selIndex = -1;
            sel = default(T);
        }
        public void RemoveAtCurrent()
        {
            array.RemoveAt(selIndex);
        }
        public T GetFromIndex(int index)
        {
            return array.Array[index];
        }

        public SpottedArrayCounter<T> Clone()
        {
            SpottedArrayCounter<T> clone = this;
            clone.Reset();
            return clone;
        }

        public ISpottedArrayCounter<T> IClone()
        {
            SpottedArrayCounter<T> clone = this;
            clone.Reset();
            return clone;
        }

        public int CurrentIndex { get { return this.selIndex; } }
        public T GetSelection { get { return sel; } }
    }

    struct SpottedArrayTypeCounter<T> : ISpottedArrayCounter<T>
    {
        int index;
        public SpottedArray<T> array;
        public T sel;
        Type type;

        public SpottedArrayTypeCounter(SpottedArray<T> array, Type type)
            : this()
        {
            this.type = type;
            this.array = array;
            Reset();
        }

        /// <summary>
        /// To be used in a while-loop
        /// </summary>
        public bool Next()
        {
            while (true)
            {
                sel = array.NextIteration(ref index);

                if (sel == null)
                {
                    return false;
                }
                else if (sel.GetType() == type)
                {
                    return true;
                }
            }
        }

        public void Reset(SpottedArray<T> array)
        {
            this.array = array;
            Reset();
        }

        public void Reset()
        {
            this.index = -1;
            sel = default(T);
        }
        public void RemoveAtCurrent()
        {
            array.RemoveAt(index);
        }
        public T GetFromIndex(int index)
        {
            return array.Array[index];
        }

        public ISpottedArrayCounter<T> IClone()
        {
            var clone = this;
            clone.Reset();
            return clone;
        }
        public int CurrentIndex { get { return this.index; } }
        public T GetSelection { get { return sel; } }
    }


    struct SpottedArrayDoubleCounter<T> : ISpottedArrayCounter<T>
    {
        int index;
        bool firstArray;
        public SpottedArray<T> array1;
        public SpottedArray<T> array2;

        T currentObject;

        public SpottedArrayDoubleCounter(SpottedArray<T> array1, SpottedArray<T> array2)
            : this()
        {
            this.array1 = array1;
            this.array2 = array2;
            Reset();
        }

        /// <summary>
        /// To be used in a while-loop
        /// </summary>
        public bool Next()
        {
            if (firstArray)
            {
                currentObject = array1.NextIteration(ref index);
                if (index == array1.SpottedLength - 1)
                {
                    firstArray = false;
                    index = -1;
                }
            }
            else
            {
                currentObject = array2.NextIteration(ref index);
            }
            return currentObject != null;
        }

        public T GetSelection
        {
            get { return currentObject; }
        }

        public void Reset()
        {
            firstArray = true;
            this.index = -1;
            currentObject = default(T);
        }
        public void RemoveAtCurrent()
        {
            if (firstArray) 
                array1.RemoveAt(index);
            else
                array2.RemoveAt(index);
        }
        public T GetFromIndex(int index)
        {
            if (index < array1.Array.Length)
                return array1.Array[index];
            else
                return array2.Array[index - array1.Array.Length];
        }
        public ISpottedArrayCounter<T> IClone()
        {
            SpottedArrayDoubleCounter<T> clone = this;
            clone.Reset();
            return clone;
        }

        public int CurrentIndex { get { 
            if (firstArray)
                return this.index; 
            else
                return this.index + array1.Array.Length;
        } }
    }



    /// <summary>
    /// Will search up the next free spot to place a member, members will never move around due to thread safety
    /// </summary>
    class SpottedArray<T>
    {
        public int mostLeftFreePosition = 0;
        /// <summary>
        /// The used part of the array, can be greater than the member count
        /// </summary>
        public int SpottedLength;
        public int Count;
        public T[] Array;

        public SpottedArray()
            : this(4)
        { }
        public SpottedArray(int length)
        {
            Array = new T[length];
        }

        /// <returns>At array index</returns>
        public int Add(T obj)
        {
            int placementIndex = NextAvailableIndex();
            if (placementIndex >= Array.Length)
            {
                adjustLength();
            }

            Array[placementIndex] = obj;
            SpottedLength = lib.LargestValue(SpottedLength, placementIndex + 1);
            if (obj is ISpottedArrayMember)
                ((ISpottedArrayMember)obj).SpottedArrayMemberIndex = placementIndex;
            ++Count;

            mostLeftFreePosition = placementIndex + 1;
            return placementIndex;
        }

        public int NextAvailableIndex()
        {
            int result = mostLeftFreePosition;

            if (result >= Array.Length)
            { return Array.Length; }
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

        public bool AddIfNotExists(T obj)
        {
            if (!Array.Contains(obj))
            { 
                Add(obj);
                return true;
            }
            return false;
        }

        public void Add(SpottedArray<T> array)
        {
            int index = -1;
            T member;
            do
            {
                member = array.NextIteration(ref index);
                Add(member);
            } while (member != null);
        }

        public void HardSet(T obj, int arrayIndex)
        {
            while (arrayIndex >= Array.Length)
            {
                adjustLength();
            }

            if (arrayIndex >= SpottedLength)
            {
                SpottedLength = arrayIndex + 1;
                Count++;
            }
            else if (Array[arrayIndex] == null)
            {
                Count++;
            }
            Array[arrayIndex] = obj;
        }

        public void Remove(T obj)
        {
            if (obj != null)
            {
                if (obj is ISpottedArrayMember)
                {
                    ISpottedArrayMember member = (ISpottedArrayMember)obj;
                    if (member.SpottedArrayUseIndex)
                    {
                        int memberIx = member.SpottedArrayMemberIndex;
                        if (memberIx < Array.Length && Array[memberIx] != null && Array[memberIx].Equals(obj))
                            RemoveAt(memberIx);

                        return;
                    }
                }

                RemoveNotMember(obj);
            }
        }

        public void RemoveNotMember(T obj)
        {
            for (int i = 0; i < SpottedLength; ++i)
            {
                if (Array[i] != null && Array[i].Equals(obj))
                {
                    RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveAt(int index)
        {
            --Count;
            Array[index] = default(T);
            mostLeftFreePosition = lib.SmallestValue(mostLeftFreePosition, index);
            updateSpottedLength();
        }

        public void RemoveAt_EqualSafeCheck(T obj, int index)
        {
            if (obj.Equals(Array[index]))
            {
                --Count;
                Array[index] = default(T);
                mostLeftFreePosition = lib.SmallestValue(mostLeftFreePosition, index);
                updateSpottedLength();
            }
            else
            { 
                lib.DoNothing();
            }
        }

        //public void RemoveAt_EqualSafeCheck_OrCrash(T obj, int index)
        //{
        //    if (obj.Equals(Array[index]))
        //    {
        //        --Count;
        //        Array[index] = default(T);
        //        mostLeftFreePosition = lib.SmallestValue(mostLeftFreePosition, index);
        //        updateSpottedLength();
        //    }
        //}

        public T Get(int position)
        {
            for (int i = 0; i < Array.Length; ++i)
            {
                if (Array[i] != null)
                {
                    if (position <= 0)
                        return Array[i];
                    --position;
                }
            }
            throw new IndexOutOfRangeException("SpottedArray get index");
        }

        public T GetIndex_Safe(int index)
        {
            if (index > 0 && index < Array.Length)
            {
                return Array[index];
            }
            return default(T);
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
            T[] newArray = new T[Array.Length * 2];
            for (int i = 0; i < Array.Length; ++i)
            {
                newArray[i] = Array[i];
            }
            Array = newArray;
        }

        public T PrevIteration(ref int i)
        {
            --i;
            for (; i >=0; --i)
            {
                T result = Array[i];
                if (result != null)
                    return result;
            }

            return default(T);
        }

        public T NextIteration(ref int i)
        {
            ++i;
            for (; i < SpottedLength; ++i)
            {
                T result = Array[i];
                if (result != null)
                    return result;
            }

            return default(T);
        }

        public T First()
        {
            if (Count > 0)
            {
                foreach (var m in Array)
                {
                    if (m != null)
                    {
                        return m;
                    }
                }
            }

            return default(T);
        }

        public T GetRandomUnsafe(PcgRandom rnd)
        {
            if (Count <= 0)
            {
                return default(T);
            }

            //First try a classic random
            for (int i = 0; i < 3; ++i)
            {
                int ix = rnd.Int(Array.Length);

                if (Array[ix] != null)
                { return Array[ix]; }
            }

            int maxTrials = 100;
            while (--maxTrials > 0)
            {
                int ix = rnd.Int(SpottedLength);
                if (Array[ix] != null)
                { return Array[ix]; }
            }

            return default(T);
        }

        public T GetRandom(PcgRandom rnd)
        {
            if (Count <= 0)
            {
                throw new Exception("Spotted array GetRandom from empty list");
            }

            //First try a classic random
            for (int i = 0; i < 3; ++i)
            {
                int ix = rnd.Int(Array.Length);

                if (Array[ix] != null)
                { return Array[ix]; }
            }

            while (true)
            {
                int ix = rnd.Int(SpottedLength);
                if (Array[ix] != null)
                { return Array[ix]; }
            }
           
        }

        public T GetRandomSafe(PcgRandom rnd)
        {
            int ix = rnd.Int(Count);
            for (int i = 0; i < SpottedLength; ++i)
            {
                if (Array[i] != null)
                {
                    if (ix <= 0) return Array[i];
                    --ix;
                }
            }
            return default(T);
        }

        public T PullRandom_Safe(PcgRandom rnd)
        {
            int ix = rnd.Int(Count);
            for (int i = 0; i < SpottedLength; ++i)
            {
                if (Array[i] != null)
                {
                    if (ix <= 0)
                    {
                        T result = Array[i];
                        RemoveAt(i);
                        return result;
                    }
                    --ix;
                }
            }
            return default(T);
        }

        public bool Contains(T value)
        {
            return Array.Contains(value);
        }

        public void Clear()
        {
            Count = 0;
            for (int i = 0; i < SpottedLength; ++i)
            {
                Array[i] = default(T);
            }
        }
        public int updateCount()
        {
            int result = 0;
            for (int i = 0; i < SpottedLength; ++i)
            {
                if (Array[i] != null) ++result;
            }
            Count = result;
            return result;
        }

        public T this[int index] 
        {
            get { return Array[index]; } 
            set { Array[index] = value; } 
        }

        public List<T> toList()
        {
            List<T> list = new List<T>(this.Count);
            SpottedArrayCounter<T> counter = new SpottedArrayCounter<T>(this);
            while (counter.Next())
            {
                list.Add(counter.sel);
            }

            return list;
        }

        public void toList(ref List<T> list)
        {
            SpottedArrayCounter<T> counter = new SpottedArrayCounter<T>(this);
            while (counter.Next())
            {
                list.Add(counter.sel);
            }
        }

        public SpottedArrayCounter<T> counter()
        {
            return new SpottedArrayCounter<T>(this);
        }

        public bool HasMembers => Count > 0;

        public bool Empty => Count == 0;
    }
}
