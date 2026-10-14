using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    using System;

    /// <summary>
    /// A complete garbage-free fixed array of up to 8 items.
    /// </summary>
    public struct FlatArray_Eight<T> where T : struct
    {
        public FlatArray_Eight(T value1)
        {
            this.value1 = value1;
            this.count = 1;
        }
        public FlatArray_Eight(T value1, T value2)
        {
            this.value1 = value1;
            this.value2 = value2;
            this.count = 2;
        }
        public FlatArray_Eight(T value1, T value2, T value3)
        {
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
            this.count = 3;
        }
        public FlatArray_Eight(T value1, T value2, T value3, T value4)
        {
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
            this.value4 = value4;
            this.count = 4;
        }
        public FlatArray_Eight(T value1, T value2, T value3, T value4, T value5, T value6, T value7, T value8) 
        { 
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
            this.value4 = value4;
            this.value5 = value5;
            this.value6 = value6;
            this.value7 = value7;
            this.value8 = value8;
            this.count = 8;
        }

        public int count;
        public T value1;
        public T value2;
        public T value3;
        public T value4;
        public T value5;
        public T value6;
        public T value7;
        public T value8;

        public void Add(T v)
        {
            switch (count)
            {
                case 0: value1 = v; break;
                case 1: value2 = v; break;
                case 2: value3 = v; break;
                case 3: value4 = v; break;
                case 4: value5 = v; break;
                case 5: value6 = v; break;
                case 6: value7 = v; break;
                case 7: value8 = v; break;
                default: throw new InvalidOperationException("FlatArray is full.");
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
                    3 => value4,
                    4 => value5,
                    5 => value6,
                    6 => value7,
                    7 => value8,
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
                    case 3: value4 = value; break;
                    case 4: value5 = value; break;
                    case 5: value6 = value; break;
                    case 6: value7 = value; break;
                    case 7: value8 = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public bool InBounds(int index)
        {
            return index >= 0 && index < count;
        }
    }
}
