using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    using System;
    using System.Runtime.CompilerServices;

    struct FlatArrayCounter_EightInt
    {
        public int index;
        public readonly int length;

        public FlatArrayCounter_EightInt(int length)
        {
            this.index = -1;
            this.length = length;
        }

        public bool MoveNext()
        {
            index++;
            return index < length;
        }

        public void Reset()
        {
            index = -1;
        }
    }

    /// <summary>
    /// A complete garbage-free fixed array of up to 8 integers.
    /// </summary>
    struct FlatArray_EightInt
    {
        public int count;
        public int value1;
        public int value2;
        public int value3;
        public int value4;
        public int value5;
        public int value6;
        public int value7;
        public int value8;

        public void Add(int v)
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
                default: throw new InvalidOperationException("FlatArray_EightInt is full.");
            }
            count++;
        }

        public int this[int index]
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

        public FlatArrayCounter_EightInt GetCounter()
        {
            return new FlatArrayCounter_EightInt(count);
        }
    }

}
