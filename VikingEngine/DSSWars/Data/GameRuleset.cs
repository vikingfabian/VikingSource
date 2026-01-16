using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Data
{
    struct GameRuleset
    {        
        public MapSize mapSize = MapSize.Medium;
        public bool centralGold = true;
        public FactionStartSize factionStartSize = FactionStartSize.OneCity;

        public GameRuleset() 
        { }

        const int Version = 1;
        public void write(System.IO.BinaryWriter w)
        { 
            w.Write(Version);
            w.Write((int)mapSize);
            w.Write(centralGold);
            w.Write((byte)factionStartSize);
        }
        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
            mapSize = (MapSize)r.ReadInt32();
            centralGold = r.ReadBoolean();
            if (version >= 1)
            {
                factionStartSize = (FactionStartSize)r.ReadByte();
            }
        }
        public void defaultGameSettings()
        {
            mapSize = MapSize.Medium;
            centralGold = true;
            factionStartSize = FactionStartSize.Full;
        }
        public void demoSetup()
        {
            mapSize = MapSize.Medium;
            centralGold = true;
            factionStartSize = FactionStartSize.Full;
        }
    }
}
