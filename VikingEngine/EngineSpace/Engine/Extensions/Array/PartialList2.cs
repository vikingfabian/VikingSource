using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine
{
    using System;

    /// <summary>
    /// Storable index range [StartIndex, EndIndex) into a parent List2.
    /// Can be stored as a field in a class.
    /// </summary>
    public struct PartialList2<T>
    {
        public int StartIndex;
        public int ExEndIndex;

        public int Count => ExEndIndex - StartIndex;

        public PartialList2(int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex < startIndex)
                throw new ArgumentOutOfRangeException();

            StartIndex = startIndex;
            ExEndIndex = endIndex;
        }

        public PartialList2(int startIndex)
            : this(startIndex, startIndex)
        {
        }

        public PartialList2(List2<T> parent)
            : this(parent.Count)
        {
        }
        public PartialList2(ref List2<T> parent, T item1)
            : this(parent.Count)
        {
            parent.Add(item1);
            ExEndIndex += 1;
        }
        public PartialList2(ref List2<T> parent, T item1, T item2)
            : this(parent.Count)
        {
            parent.Add(item1);
            parent.Add(item2);
            ExEndIndex += 2;
        }
        public PartialList2(ref List2<T> parent, T item1, T item2, T item3)
            : this(parent.Count)
        {
            parent.Add(item1);
            parent.Add(item2);
            parent.Add(item3);
            ExEndIndex += 3;
        }
        public PartialList2(ref List2<T> parent, T item1, T item2, T item3, T item4)
            : this(parent.Count)
        {
            parent.Add(item1);
            parent.Add(item2);
            parent.Add(item3);
            parent.Add(item4);
            ExEndIndex += 4;
        }
        public PartialList2(ref List2<T> parent, T item1, T item2, T item3, T item4, T item5)
            : this(parent.Count)
        {
            parent.Add(item1);
            parent.Add(item2);
            parent.Add(item3);
            parent.Add(item4);
            parent.Add(item5);
            ExEndIndex += 5;
        }

        // Local index get / set / ref
        public T Get(ref List2<T> parent, int localIndex)
        {
            ValidateLocalIndex(localIndex);
            return parent.Array[StartIndex + localIndex];
        }

        public T First(List2<T> parent)
        {
            if (ExEndIndex <= StartIndex)
            {
                return default;
            }
            return parent.Array[StartIndex];
        }

        public void Set(ref List2<T> parent, int localIndex, T value)
        {
            ValidateLocalIndex(localIndex);
            parent.Array[StartIndex + localIndex] = value;
        }

        public ref T Ref(ref List2<T> parent, int localIndex)
        {
            ValidateLocalIndex(localIndex);
            return ref parent[StartIndex + localIndex];
        }

        // Zero-allocation view for [] indexing and foreach (including "foreach (ref var x in ...)")
        public Span<T> AsSpan(ref List2<T> parent)
        {
            if (Count <= 0)
                return Span<T>.Empty;

            return new Span<T>(parent.Array, StartIndex, Count);
        }

        public void Add(ref List2<T> parent, T item)
        {
            Insert(ref parent, Count, item);
        }

        public void Insert(ref List2<T> parent, int localIndex, T item)
        {
            if ((uint)localIndex > (uint)Count)
                throw new ArgumentOutOfRangeException(nameof(localIndex));

            int parentIndex = StartIndex + localIndex;
            int oldParentCount = parent.Count;

            parent.Add(default);

            if (parentIndex < oldParentCount)
            {
                System.Array.Copy(parent.Array, parentIndex, parent.Array, parentIndex + 1, oldParentCount - parentIndex);
            }

            parent.Array[parentIndex] = item;
            ExEndIndex++;
        }

        public void RemoveAt(ref List2<T> parent, int localIndex)
        {
            ValidateLocalIndex(localIndex);
            parent.RemoveAt(StartIndex + localIndex);
            ExEndIndex--;
        }

        // Swaps with the last element of THIS slice rather than the end of the parent list
        public void RemoveAtSwapBack(ref List2<T> parent, int localIndex)
        {
            ValidateLocalIndex(localIndex);
            int parentIndex = StartIndex + localIndex;
            int lastInSlice = ExEndIndex - 1;

            if (parentIndex < lastInSlice)
            {
                parent.Array[parentIndex] = parent.Array[lastInSlice];
            }

            parent.RemoveAt(lastInSlice);
            ExEndIndex--;
        }

        public bool Remove(ref List2<T> parent, T item)
        {
            int localIndex = IndexOf(ref parent, item);
            if (localIndex >= 0)
            {
                RemoveAt(ref parent, localIndex);
                return true;
            }
            return false;
        }

        public void Clear(ref List2<T> parent)
        {
            int removeCount = Count;
            if (removeCount <= 0) return;

            int trailingCount = parent.Count - ExEndIndex;
            if (trailingCount > 0)
            {
                System.Array.Copy(parent.Array, ExEndIndex, parent.Array, StartIndex, trailingCount);
            }

            int newParentCount = parent.Count - removeCount;
            System.Array.Clear(parent.Array, newParentCount, removeCount);
            parent.Count = newParentCount;
            ExEndIndex = StartIndex;
        }

        public int IndexOf(ref List2<T> parent, T item)
        {
            if (parent.Array == null || Count <= 0) return -1;
            int found = System.Array.IndexOf(parent.Array, item, StartIndex, Count);
            return found >= 0 ? found - StartIndex : -1;
        }

        public bool Contains(ref List2<T> parent, T item) => IndexOf(ref parent, item) >= 0;

        public bool HasValue() => ExEndIndex > StartIndex;
        public bool IsEmpty() => ExEndIndex <= StartIndex;

        private void ValidateLocalIndex(int localIndex)
        {
            if ((uint)localIndex >= (uint)Count)
                throw new ArgumentOutOfRangeException(nameof(localIndex));
        }
    }

    public struct PartialListCounter<T>
    {
        public int currentIndex;
        List2<T> parent;
        PartialList2 <T> partialList;

        public PartialListCounter(List2<T> parent, PartialList2<T> partialList)
        {
            this.parent = parent;
            this.partialList = partialList;
            //if (partialList.Count > 0)
            //{
                currentIndex = partialList.StartIndex - 1;
            //}
            //else
            //{
            //    currentIndex = parent.Count;
            //}
        }

        public bool Next(out T value)
        {
            currentIndex++;
            if (currentIndex < partialList.ExEndIndex)
            {
                value = parent.Array[currentIndex];
                return true;
            }

            value = default;
            return false;
        }
    }
}