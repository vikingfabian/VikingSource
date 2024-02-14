using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.BlockMap;

namespace VikingEngine.LootFest
{
    static class LfRef
    {
        public static Map.World world;
        public static Map.WorldChunks chunks;
        public static Data.VoxModelAutoLoad modelLoad;

        public static LevelsManager levels2;
        public static PlayState gamestate;
        public static SpawnDirector spawner;
        public static Players.PlayerStorageGroup storage;
        public static StaticList<GO.PlayerCharacter.AbsHero> LocalHeroes;
        public static StaticList<GO.PlayerCharacter.AbsHero> AllHeroes;

        public static Data.Progress progress;
        public static Data.Images Images = new Data.Images();
        public static BoundManager bounds;
        public static LootFest.NetLobby2 net;
        public static bool WorldHost;
#if PCGAME
        public static Data.GameStats stats;
#endif

        public static BlockmapCollection blockmaps = new BlockmapCollection();

        public static void ClearRAM()
        {
            world = null;
            chunks = null;
            modelLoad = null;
            levels2 = null;
            gamestate = null;
            spawner = null;
            LocalHeroes = null;
            AllHeroes = null;
            bounds = null;
            blockmaps = null;
            //Ref.draw.heightmap = null;
            //storage.clearRAM();
            for (int i = 0; i < 4; ++i)
            {
                Engine.XGuide.GetPlayer(i).Tag = null;
            }

            GC.Collect();
        }
    }
}
