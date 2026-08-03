using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map.Map2
{
    class WorldData2
    {
        public ushort seed = Ref.rnd.Ushort();
        public Grid2D_L<Map.SubTile> iconGrid;
        public Grid2D_L<Map.SubTile> tileGrid;
        public PcgRandom rnd;

        public WorldData2(MapSize size)
        { 
            rnd = new PcgRandom(seed);

            IntVector2 tileSz = WorldData.SizeDimentions(size) * 8 * 2;

            iconGrid = new Grid2D_L<Map.SubTile>(tileSz / 16);
            //tileGrid = new Grid2D<Tile2>(tileSz);
            
        }

    }
}
