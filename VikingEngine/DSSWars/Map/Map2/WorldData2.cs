using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map.Map2
{
    struct WorldMetaData2
    {
        public ushort seed;

        public WorldMetaData2()
        { 
            seed = Ref.rnd.Ushort();
        }
    }
    struct CityPlacementData
    {
        public int myIndex;
        public IntVector2 pos;

    }

    

    class WorldData2
    {
        public WorldMetaData2 metaData2;
        public Grid2D_L<GenTile> tileGrid;
        public PcgRandom rnd;

        public WorldData2(IconWorldData icon)
        {
            metaData2 = icon.metaData2;

            rnd = new PcgRandom(metaData2.seed);

            IntVector2 tileSz = icon.iconGrid.Size * 16;
            
        }

    }
}
