using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map.Map2
{
    class WorldData2
    {
        public Grid2D<Tile2> tileGrid;

        public WorldData2(MapSize size)
        { 
            IntVector2 tileSz = WorldData.SizeDimentions(size) * 8;

            tileGrid = new Grid2D<Tile2>(tileSz);
        }

    }
}
