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

    class IconWorldData
    {
        public WorldMetaData2 metaData2;
        public Grid2D_L<GenTile> iconGrid;
        public PcgRandom rnd;

        public IconWorldData(IntVector2 iconSize)
        {
            metaData2 = new WorldMetaData2();

            rnd = new PcgRandom(metaData2.seed);

            iconGrid = new Grid2D_L<GenTile>(iconSize);

            IntVector2 tileSz = iconGrid.Size * 16;

        }

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
