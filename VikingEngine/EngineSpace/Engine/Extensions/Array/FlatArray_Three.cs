using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    using System;

    /// <summary>
    /// A complete garbage-free fixed array of up to 3 items.
    /// </summary>
    public struct FlatArray_Three<T> where T : struct/*, IEquatable<T>*/
    {
        public const int Capacity = 3;
        public int count;
        public T value1;
        public T value2;
        public T value3;

        public FlatArray_Three() { }
        public FlatArray_Three(T value1)
        {
            this.value1 = value1;
            count = 1;
        }
        public FlatArray_Three(T value1, T value2)
        {
            this.value1 = value1;
            this.value2 = value2;
            count = 2;
        }
        public FlatArray_Three(T value1, T value2, T value3) 
        {
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
            count = 3;
        }

        public void Add(T v)
        {
            switch (count)
            {
                case 0: value1 = v; break;
                case 1: value2 = v; break;
                case 2: value3 = v; break;
                default: throw new InvalidOperationException("FlatArray_Three is full.");
            }
            count++;
        }
        public void TryAdd(T v)
        {
            switch (count)
            {
                case 0: value1 = v; break;
                case 1: value2 = v; break;
                case 2: value3 = v; break;
                default: return;
            }
            count++;
        }

        public void TryAddIfNotContains(T v)
        {
            if (count > 0 && value1.Equals(v)) return;
            if (count > 1 && value2.Equals(v)) return;
            if (count > 2 && value3.Equals(v)) return;
            
            switch (count)
            {
                case 0: value1 = v; break;
                case 1: value2 = v; break;
                case 2: value3 = v; break;
                default: return;
            }
            count++;
        }

        public T this[int index]
        {
            readonly get
            {
                return index switch
                {
                    0 => value1,
                    1 => value2,
                    2 => value3,
                    _ => throw new IndexOutOfRangeException()
                };
            }
            set
            {
                switch (index)
                {
                    case 0: value1 = value; break;
                    case 1: value2 = value; break;
                    case 2: value3 = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public readonly bool Contains(T value)
        {
            if (count > 0 && value1.Equals(value)) return true;
            if (count > 1 && value2.Equals(value)) return true;
            if (count > 2 && value3.Equals(value)) return true;

            return false;
        }
        public readonly bool Contains_GetIndex(T value, out int index)
        {
            if (count > 0 && value1.Equals(value))
            { index = 0; return true; }

            if (count > 1 && value2.Equals(value)) 
            { index = 1; return true; }

            if (count > 2 && value3.Equals(value)) 
            { index = 2; return true; }

            index = -1;
            return false;
        }

        public void Clear()
        {
            count = 0;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            // Shift elements down to fill the gap
            if (index == 0)
            {
                value1 = value2;
                value2 = value3;
            }
            else if (index == 1)
            {
                value2 = value3;
            }

            count--;

            // Clear the obsolete trailing value to release references
            // (Not strictly necessary for unmanaged structs, but good practice)
            switch (count)
            {
                case 0: value1 = default; break;
                case 1: value2 = default; break;
                case 2: value3 = default; break;
            }
        }

        public void Remove(T value)
        {
            // Find the index and remove it
            if (count > 0 && value1.Equals(value)) { RemoveAt(0); return; }
            if (count > 1 && value2.Equals(value)) { RemoveAt(1); return; }
            if (count > 2 && value3.Equals(value)) { RemoveAt(2); return; }
        }

        /// <summary>
        /// Removes and returns the last element in the array.
        /// </summary>
        public T PullLast()
        {
            if (count == 0)
                throw new InvalidOperationException("FlatArray_Three is empty.");

            count--;
            T result;

            switch (count)
            {
                case 0:
                    result = value1;
                    break;
                case 1:
                    result = value2;
                    break;
                case 2:
                    result = value3;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return result;
        }

        public bool TryPullLast(out T result)
        {
            if (count == 0)
            {
                result = default;
                return false;
            }

            count--;

            switch (count)
            {
                case 0:
                    result = value1;
                    break;
                case 1:
                    result = value2;
                    break;
                case 2:
                    result = value3;
                    break;
                default:
                    result = default;
                    return false;
            }

            return true;
        }
    }
}
