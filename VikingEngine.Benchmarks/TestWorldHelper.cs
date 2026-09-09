using System;
using System.Collections.Generic;
using VikingEngine;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.Benchmarks.Pathfinding
{
    static class TestWorldHelper
    {
        public static void SetupFlatWorld(
            int width, int height,
            byte landHeight = Height.MinLandHeight,
            HashSet<IntVector2>? waterTiles = null)
        {
            Tile.Init();

            var world = new WorldData();
            world.refreshSize(new IntVector2(width, height));

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pos = new IntVector2(x, y);
                    var tile = new Tile();

                    if (waterTiles != null && waterTiles.Contains(pos))
                    {
                        tile.heightLevel = Height.DeepWaterHeight;
                    }
                    else
                    {
                        tile.heightLevel = landHeight;
                    }

                    world.tileGrid.Set(pos, tile);
                }
            }

            DssRef.world = world;
        }
    }
}
