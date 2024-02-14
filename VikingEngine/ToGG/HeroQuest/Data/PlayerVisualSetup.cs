using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Network;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class PlayerVisualSetup
    {
        public KillMarkVisuals killmark;
        public HqUnitType unit;

        public PlayerVisualSetup()
        {
            unit = HqUnitType.Num_None;
        }

        public PlayerVisualSetup(HqUnitType unit)
        {
            this.unit = unit;
        }

        public void netWriteStatus(System.IO.BinaryWriter w)
        {
            //var w = Ref.netSession.BeginWritingPacket(PacketType.hqPlayerVisualSetup, PacketReliability.Reliable);
            w.Write((byte)unit);
        }

        public void netReadStatus(System.IO.BinaryReader r)
        {
            unit = (HqUnitType)r.ReadByte();
        }
    }
}
