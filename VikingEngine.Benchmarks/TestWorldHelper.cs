using System;
using System.Collections.Generic;
using VikingEngine;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.Benchmarks.Pathfinding
{
    static class TestWorldHelper
    {
        public static void SetupFlatWorld(
            int width, int height,
            byte landHeight = ColorHeight.MinLandHeight,
            HashSet<IntVector2>? waterTiles = null)
        {
            SumTile4_4.Init();

            var world = new WorldData();
            world.refreshSize(new IntVector2(width, height));

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pos = new IntVector2(x, y);
                    var tile = new SumTile4_4();

                    if (waterTiles != null && waterTiles.Contains(pos))
                    {
                        tile.heightLevel = ColorHeight.DeepWaterHeight;
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
