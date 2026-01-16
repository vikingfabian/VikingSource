using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    /// <summary>
    /// Iterator-style counter for SpottedIntArray. 
    /// Works like an enumerator but without allocations.
    /// </summary>
    struct SpottedPointerArrayCounter
    {
        int selIndex;
        //public SpottedPointerArray array;
        public int sel;

        public SpottedPointerArrayCounter(/*SpottedPointerArray array*/)
            //: this()
        {
            //this.array = array;
            Reset();
        }

        /// <summary>
        /// Move to next valid element (skips empty slots).
        /// To be used in a while-loop.
        /// </summary>
        public bool Next(ref SpottedPointerArray array)
        {
            int value = array.NextIteration(ref selIndex);
            sel = value;
            return sel != SpottedPointerArray.NullPointer;
        }

        public bool Next<T>(ref SpottedPointerArray array, List<T> objects, out T selected)
        {
            int value = array.NextIteration(ref selIndex);
            sel = value;
            if (sel != SpottedPointerArray.NullPointer)
            {
                selected = objects[value];
                return true;
            }
            else
            { 
                selected = default(T);
                return false;
            }
        }

        /// <summary>
        /// Returns true if there’s still a valid current selection.
        /// </summary>
        public bool HasMore()
        {
            return selIndex < 0 || sel != SpottedPointerArray.NullPointer;
        }

        /// <summary>
        /// Move to the next element, wrapping to start if end reached.
        /// </summary>
        public bool Prev_Rollover(ref SpottedPointerArray array)
        {
            if (array.Count == 0)
                return false;

            sel = array.NextIteration(ref selIndex);
            if (sel == SpottedPointerArray.NullPointer)
            {
                selIndex = SpottedPointerArray.NullPointer;
                sel = array.NextIteration(ref selIndex);
            }
            return sel != SpottedPointerArray.NullPointer;
        }

        /// <summary>
        /// Move to the previous element, wrapping to end if start reached.
        /// </summary>
        public bool Next_Rollover(ref SpottedPointerArray array)
        {
            if (array.Count == 0)
                return false;

            sel = array.PrevIteration(ref selIndex);
            if (sel == SpottedPointerArray.NullPointer)
            {
                selIndex = array.Array.Length;
                sel = array.PrevIteration(ref selIndex);
            }
            return sel != SpottedPointerArray.NullPointer;
        }

        //public void Reset(SpottedPointerArray array)
        //{
        //    this.array = array;
        //    Reset();
        //}

        public void Reset()
        {
            selIndex = SpottedPointerArray.NullPointer;
            sel = SpottedPointerArray.NullPointer;
        }

        public void RemoveAtCurrent(ref SpottedPointerArray array)
        {
            if (selIndex >= 0 && selIndex < array.SpottedLength)
                array.RemoveAt(selIndex);
        }

        public int GetFromIndex(int index, ref SpottedPointerArray array)
        {
            if (index >= 0 && index < array.Array.Length)
                return array.Array[index];
            return SpottedPointerArray.NullPointer;
        }

        public SpottedPointerArrayCounter Clone(ref SpottedPointerArray array)
        {
            SpottedPointerArrayCounter clone = this;
            clone.Reset();
            return clone;
        }

        public int CurrentIndex => selIndex;

        public int GetSelection => sel;
    }


    /// <summary>
    /// A compact, integer-only version of SpottedArray that uses -1 to represent an empty slot.
    /// Thread-safe for adding/removing as long as items themselves are not modified concurrently.
    /// </summary>
    struct SpottedPointerArray
    {
        public const int NullPointer = -1;

        public int mostLeftFreePosition = 0;
        /// <summary>
        /// The used part of the array, can be greater than the member count
        /// </summary>
        public int SpottedLength;
        public int Count;
        public int[] Array;

        public SpottedPointerArray()
            : this(4)
        { }

        public SpottedPointerArray(int length)
        {
            Array = new int[length];
            for (int i = 0; i < Array.Length; ++i)
                Array[i] = NullPointer;
        }

        /// <returns>At array index</returns>
        public int Add(int value)
        {
            int placementIndex = NextAvailableIndex();
            if (placementIndex >= Array.Length)
            {
                AdjustLength();
            }

            Array[placementIndex] = value;
            SpottedLength = Math.Max(SpottedLength, placementIndex + 1);
            ++Count;

            mostLeftFreePosition = placementIndex + 1;
            return placementIndex;
        }

        public int NextAvailableIndex()
        {
            int result = mostLeftFreePosition;

            if (result >= Array.Length)
                return Array.Length;

            while (Array[result] != NullPointer)
            {
                ++result;
                if (result >= Array.Length)
                    return Array.Length;
            }

            return result;
        }

        public bool AddIfNotExists(int value)
        {
            if (!Array.Contains(value))
            {
                Add(value);
                return true;
            }
            return false;
        }

        public void Add(SpottedPointerArray other)
        {
            int index = NullPointer;
            int member;
            do
            {
                member = other.NextIteration(ref index);
                if (member != NullPointer)
                    Add(member);
            } while (member != NullPointer);
        }

        public void HardSet(int value, int arrayIndex)
        {
            while (arrayIndex >= Array.Length)
            {
                AdjustLength();
            }

            if (arrayIndex >= SpottedLength)
            {
                SpottedLength = arrayIndex + 1;
                Count++;
            }
            else if (Array[arrayIndex] == NullPointer)
            {
                Count++;
            }
            Array[arrayIndex] = value;
        }

        public void Remove(int value)
        {
            for (int i = 0; i < SpottedLength; ++i)
            {
                if (Array[i] == value)
                {
                    RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveAt(int index)
        {
            if (Array[index] != NullPointer)
            {
                --Count;
                Array[index] = NullPointer;
                mostLeftFreePosition = Math.Min(mostLeftFreePosition, index);
                UpdateSpottedLength();
            }
        }

        public int Get(int position)
        {
            for (int i = 0; i < Array.Length; ++i)
            {
                if (Array[i] != NullPointer)
                {
                    if (position <= 0)
                        return Array[i];
                    --position;
                }
            }
            throw new IndexOutOfRangeException("SpottedIntArray get index");
        }

        public int GetIndex_Safe(int index)
        {
            if (index >= 0 && index < Array.Length)
            {
                return Array[index];
            }
            return NullPointer;
        }

        public int PullIndex_Safe(int index)
        {
            if (index >= 0 && index < Array.Length)
            {
                int value = Array[index];
                if (value != NullPointer)
                {
                    --Count;
                    Array[index] = NullPointer;
                    mostLeftFreePosition = Math.Min(mostLeftFreePosition, index);
                    UpdateSpottedLength();

                    return value;
                }
            }

            return NullPointer;
        }

        void UpdateSpottedLength()
        {
            while (SpottedLength > 0 && Array[SpottedLength - 1] == NullPointer)
            {
                --SpottedLength;
            }
        }

        public void AdjustLength(int minLength = 8)
        {
            int[] newArray = new int[Math.Max(Array.Length * 2, minLength)];
            Array.CopyTo(newArray, 0);
            for (int i = Array.Length; i < newArray.Length; ++i)
                newArray[i] = NullPointer;
            Array = newArray;
        }

        public int PrevIteration(ref int i)
        {
            --i;
            for (; i >= 0; --i)
            {
                int result = Array[i];
                if (result != NullPointer)
                    return result;
            }

            return NullPointer;
        }

        public int NextIteration(ref int i)
        {
            ++i;
            for (; i < SpottedLength; ++i)
            {
                int result = Array[i];
                if (result != NullPointer)
                    return result;
            }

            return NullPointer;
        }

        public int First()
        {
            if (Count > 0)
            {
                foreach (var m in Array)
                {
                    if (m != NullPointer)
                        return m;
                }
            }

            return NullPointer;
        }

        public bool Contains(int value)
        {
            return Array.Contains(value);
        }

        public void Clear()
        {
            if (Count > 0)
            {
                Count = 0;
                for (int i = 0; i < SpottedLength; ++i)
                {
                    Array[i] = NullPointer;
                }
                mostLeftFreePosition = 0;
                SpottedLength = 0;
            }
        }

        public int UpdateCount()
        {
            int result = 0;
            for (int i = 0; i < SpottedLength; ++i)
            {
                if (Array[i] != NullPointer) ++result;
            }
            Count = result;
            return result;
        }

        public int this[int index]
        {
            get { return Array[index]; }
            set
            {
                if (Array[index] == NullPointer && value != NullPointer)
                    ++Count;
                else if (Array[index] != NullPointer && value == NullPointer)
                    --Count;

                Array[index] = value;
                SpottedLength = Math.Max(SpottedLength, index + 1);
            }
        }

        public List<T> toList<T>(List<T> pullFromList)
        {
            List<T> list = new List<T>(Count);
            
            for (int i = 0; i < SpottedLength; ++i)
            {
                int pointer = Array[i];
                if (pointer != NullPointer)
                {
                    list.Add(pullFromList[pointer]);
                }
            }

            return list;
        }


        public T GetRandom<T>(AbsRandom rnd, List<T> pullFromList)
        {
            int pointer = GetRandom(rnd);
            if (pointer != NullPointer)
            {
                return pullFromList[pointer];
            }
            return default(T);
        }

        public int GetRandom(AbsRandom rnd)
        {
            if (Count <= 0)
            {
                return NullPointer;
            }

            //First try just a random position
            int maxTrials = 16;
            while (--maxTrials > 0)
            {
                int ix = rnd.Int(SpottedLength);
                if (Array[ix] != NullPointer)
                { return Array[ix]; }
            }

            //Loop until found
            int start = rnd.Int(SpottedLength);
            for (int i = start; i < SpottedLength; ++i)
            {
                if (Array[i] != NullPointer)
                { return Array[i]; }
            }

            for (int i = 0; i < start; ++i)
            {
                if (Array[i] != NullPointer)
                { return Array[i]; }
            }

            return NullPointer;
        }

        public void write_ushort_compressed(System.IO.BinaryWriter w)
        { 
            w.Write((ushort)Count);

            if (Count > 0)
            {
                int realcount = 0;
                for (int i = 0; i < Array.Length; ++i)
                {
                    int pointer = Array[i];
                    if (pointer != NullPointer)
                    {
                        w.Write((ushort)pointer);
                        realcount++;
                        if (realcount >= Count)
                        { 
                            break;
                        }
                    }
                }

                realcount -= Count;
                for (int i = 0; i < realcount; ++i)
                {
                    w.Write(ushort.MaxValue);
                }
            }
        }
        public void read_ushort_compressed(System.IO.BinaryReader r)
        {
            int readCount = r.ReadUInt16();
            if (readCount > Array.Length)
            {
                Array = new int[readCount];
            }

            for (int i = 0; i < readCount; ++i)
            {
                int pointer = r.ReadUInt16();
                if (pointer != ushort.MaxValue)
                {
                    Add(pointer);
                }                
            }
        }

        public void read_ushort(System.IO.BinaryReader r)
        {
            Count = r.ReadUInt16();
            if (Count > Array.Length)
            {
                Array = new int[Count];
            }

            for (int i = 0; i < Count; ++i)
            { 
                int pointer = r.ReadUInt16();
                if (pointer == ushort.MaxValue)
                {
                    pointer = NullPointer;
                }
                else
                {
                    SpottedLength = i + 1;
                }
                Array[i] = pointer;
            }

            for (int i = SpottedLength; i < Array.Length; ++i)
            {
                Array[i] = NullPointer;
            }
        }

        public bool HasMembers => Count > 0;

        public bool Empty => Count == 0;
    }

}
