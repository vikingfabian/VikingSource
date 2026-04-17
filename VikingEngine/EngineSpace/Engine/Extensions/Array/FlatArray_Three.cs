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
    public struct FlatArray_Three<T> where T : struct, IEquatable<T>
    {
        public const int Capacity = 3;
        public int count;
        public T value1;
        public T value2;
        public T value3;

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
    }
}
