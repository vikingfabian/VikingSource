using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.BlockMap
{
    struct CollectItem
    {
        public int goalCount, collectedCount;
        public SpriteName collectDisplayIcon;

        public CollectItem(int goalCount, SpriteName icon)
        {
            this.goalCount = goalCount;
            this.collectedCount = 0;
            this.collectDisplayIcon = icon;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)collectedCount);
        }

        public void read(System.IO.BinaryReader r)
        {
            collectedCount = r.ReadByte();
        }
    }
}
