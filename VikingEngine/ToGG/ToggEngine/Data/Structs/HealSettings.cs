using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG
{
    struct HealSettings
    {
        public HealType type;
        public int heal;

        public HealSettings(int heal, HealType type)
        {
            this.heal = heal;
            this.type = type;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)type);
            w.Write((byte)heal);
        }

        public void read(System.IO.BinaryReader r)
        {
            type = (HealType)r.ReadByte();
            heal = r.ReadByte();
        }
    }

    enum HealType
    {
        Nature,
        Magic,
        Will,
        Dark,
    }
}
