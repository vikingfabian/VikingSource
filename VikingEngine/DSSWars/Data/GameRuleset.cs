using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Data
{
    struct GameRuleset
    {
        public static readonly GameModeMainType[] AvailableModes = [GameModeMainType.FullStory, GameModeMainType.QuickBoss, GameModeMainType.QuickMatch, GameModeMainType.Sandbox, GameModeMainType.Peaceful, GameModeMainType.Spectator];
        public static readonly TwoInts[] QuickBossOptions_Time_Difficulty = [new TwoInts(3, 100), new TwoInts(5, 50), new TwoInts(8, 25)];


        public MapSize mapSize = MapSize.Medium;
        public bool centralGold = true;
        public FactionStartSize factionStartSize = FactionStartSize.OneCity;
        public int QuickBossTimeOption = 1;

        public GameRuleset() 
        { }

        const int Version = 2;
        public void write(System.IO.BinaryWriter w)
        { 
            w.Write(Version);
            w.Write((int)mapSize);
            w.Write(centralGold);
            w.Write((byte)factionStartSize);
            w.Write((byte)QuickBossTimeOption);
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
            if (version >= 2)
            {
                QuickBossTimeOption = r.ReadByte();
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
