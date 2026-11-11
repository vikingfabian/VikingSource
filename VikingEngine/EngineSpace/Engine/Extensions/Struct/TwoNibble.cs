using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.EngineSpace.Engine.Extensions.Struct
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace VikingEngine
    {
        public struct TwoNibble : IBinaryIOobj
        {
            public static readonly TwoNibble Zero = new TwoNibble(byte.MinValue);
            public static readonly TwoNibble Max = new TwoNibble(byte.MaxValue);

            public byte byteValue;

            public byte Value1
            {
                get { return Get(0); }
                set { Set(0, value); }
            }

            public byte Value2
            {
                get { return Get(1); }
                set { Set(1, value); }
            }

            public TwoNibble(byte value1, byte value2 = 0)
            {
                // Mask each value to 4 bits (0-15) and pack them into a single byte
                byteValue = (byte)((value1 & 0x0F) | ((value2 & 0x0F) << 4));
            }

            public TwoNibble(byte byteValue)
            {
                this.byteValue = byteValue;
            }

            public byte Get(int index)
            {
                if (index == 0)
                    return (byte)(byteValue & 0x0F);
                if (index == 1)
                    return (byte)((byteValue >> 4) & 0x0F);

                return 0;
            }

            public void Get(out byte value1)
            {
                value1 = Get(0);
            }

            public void Get(out byte value1, out byte value2)
            {
                value1 = Get(0);
                value2 = Get(1);
            }

            public void Set(int index, byte value)
            {
                if (index == 0)
                {
                    // Clear the lower 4 bits, then set them to the new value
                    byteValue = (byte)((byteValue & 0xF0) | (value & 0x0F));
                }
                else if (index == 1)
                {
                    // Clear the upper 4 bits, then set them to the new value
                    byteValue = (byte)((byteValue & 0x0F) | ((value & 0x0F) << 4));
                }
            }

            public void Set_Safe(int index, byte value)
            {
                if (index == 0 || index == 1)
                {
                    Set(index, value);
                }
            }

            public static byte GetNibble(byte byteValue, int index)
            {
                if (index == 0)
                    return (byte)(byteValue & 0x0F);
                if (index == 1)
                    return (byte)((byteValue >> 4) & 0x0F);

                return 0;
            }

            public static byte SetNibble(byte byteValue, int index, byte value)
            {
                if (index == 0)
                {
                    byteValue = (byte)((byteValue & 0xF0) | (value & 0x0F));
                }
                else if (index == 1)
                {
                    byteValue = (byte)((byteValue & 0x0F) | ((value & 0x0F) << 4));
                }

                return byteValue;
            }

            public void write(System.IO.BinaryWriter w)
            {
                w.Write(byteValue);
            }

            public void read(System.IO.BinaryReader r)
            {
                byteValue = r.ReadByte();
            }

            public TwoNibble(System.IO.BinaryReader r)
            {
                byteValue = 0;
                read(r);
            }

            public static TwoNibble FromStream(System.IO.BinaryReader r)
            {
                TwoNibble result = TwoNibble.Zero;
                result.read(r);
                return result;
            }

            public override string ToString()
            {
                return "{" + Get(0).ToString() + ", " + Get(1).ToString() + "}";
            }
        }
    }
}
