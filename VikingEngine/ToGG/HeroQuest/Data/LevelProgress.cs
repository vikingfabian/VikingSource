using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class LevelProgress
    {
        public EightBit runeKeys;

        public LevelProgress()
        {
            runeKeys = new EightBit();
        }

        public void AddRuneKey(int rune)
        {
            runeKeys.Set(rune, true);
            netWrite();
            refreshMap();
        }

        void netWrite()
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqLevelProgress, Network.PacketReliability.Reliable);
            runeKeys.write(w);
        }

        public void netRead(System.IO.BinaryReader r)
        {
            EightBit runeKeysRead = new EightBit();
            runeKeysRead.read(r);

            runeKeys.bitArray |= runeKeysRead.bitArray;

            refreshMap();
        }

        void refreshMap()
        {
            hqRef.events.SendToMap(ToGG.Data.EventType.LevelProgress, null);
        }
    }
}
