using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VikingEngine.Engine
{

    struct UInt12_flag4
    {
        private ushort _data;

        // Bits 0-3 (Flags)
        public bool Flag1
        {
            get => (_data & 0x0001) != 0;
            set => _data = (ushort)(value ? (_data | 0x0001) : (_data & ~0x0001));
        }
        public bool Flag2
        {
            get => (_data & 0x0002) != 0;
            set => _data = (ushort)(value ? (_data | 0x0002) : (_data & ~0x0002));
        }
        public bool Flag3
        {
            get => (_data & 0x0004) != 0;
            set => _data = (ushort)(value ? (_data | 0x0004) : (_data & ~0x0004));
        }
        public bool Flag4
        {
            get => (_data & 0x0008) != 0;
            set => _data = (ushort)(value ? (_data | 0x0008) : (_data & ~0x0008));
        }

        // Bits 4-15 (12-bit integer)
        public ushort Value12Bit
        {
            get => (ushort)(_data >> 4);
            set => _data = (ushort)((_data & 0x000F) | ((value & 0x0FFF) << 4));
        }

        public UInt12_flag4(bool f1, bool f2, bool f3, bool f4, ushort value12Bit)
        {
            _data = 0;
            Flag1 = f1;
            Flag2 = f2;
            Flag3 = f3;
            Flag4 = f4;
            Value12Bit = value12Bit;
        }

        /// <summary>
        /// Safely adds a value to the 12-bit integer, clamping it between 0 and 4095 to prevent overflow.
        /// </summary>
        public void SafeAdd(int amount)
        {
            int newValue = Value12Bit + amount;

            // Clamp to 12-bit limits (0x000 to 0xFFF)
            if (newValue > 4095) newValue = 4095;
            else if (newValue < 0) newValue = 0;

            Value12Bit = (ushort)newValue;
        }

        /// <summary>
        /// Writes the exact 2 bytes to a stream.
        /// </summary>
        public void Write(BinaryWriter writer)
        {
            writer.Write(_data);
        }

        /// <summary>
        /// Reads the 2 bytes from a stream and reconstructs the struct.
        /// </summary>
        public static UInt12_flag4 Read(BinaryReader reader)
        {
            UInt12_flag4 pd = new UInt12_flag4();
            pd._data = reader.ReadUInt16();
            return pd;
        }
    }
}
