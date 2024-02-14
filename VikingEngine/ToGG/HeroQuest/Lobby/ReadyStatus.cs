using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Lobby
{
    struct ReadyStatus
    {
        public bool ready;
        public bool mapLoaded;
        public bool joined;

        public void write(System.IO.BinaryWriter w)
        {
            EightBit status = new EightBit(ready, mapLoaded);
            status.write(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            EightBit status = new EightBit(r);
            status.Get(out ready, out mapLoaded);
        }

        public bool IsReady => joined && ready && mapLoaded;
    }
}
