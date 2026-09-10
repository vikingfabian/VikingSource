using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace VikingEngine.DSSWars.Map
{
    class WorldMapTiles
    {
        public Grid2D_L<SumTile4_4> tileGrid;
        public Grid2D_L<MapTile_> subTileGrid;

        public WorldMapTiles(IntVector2 tileSize)
        {
            subTileGrid = new Grid2D_L<MapTile_>(tileSize);
            tileGrid = new Grid2D_L<SumTile4_4>(tileSize / 4);
        }
    }
}
