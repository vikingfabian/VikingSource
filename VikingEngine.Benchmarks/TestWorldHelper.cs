using System;
using System.Collections.Generic;
using VikingEngine;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.Path3;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.Benchmarks.Pathfinding
{
    static class TestWorldHelper
    {
        public static void SetupFlatWorld(
            int width, int height,
            byte landHeight = MapHeight2.WaterPlaneHeight + 1,
            HashSet<IntVector2>? waterTiles = null)
        {
            var world = new WorldData();
            var size = new IntVector2(width, height);
            world.refreshSize(size);
            world.tileGrid = new Grid2D_L<SumTile4_4>(size);
            world.moveLayer0 = new Grid2D_L<MoveCost>(size);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pos = new IntVector2(x, y);
                    var subTile = new MapTile1_1(TerrainMainType.DefaultLand, (int)TerrainDefaultLandType.Flat, landHeight)
                    {
                        heightValue = landHeight
                    };

                    if (waterTiles != null && waterTiles.Contains(pos))
                    {
                        subTile = new MapTile1_1(TerrainMainType.DefaultSea, 0, 0)
                        {
                            heightValue = 0
                        };
                    }

                    world.subTileGrid.Set(pos, subTile);
                    world.moveLayer0.Set(pos, subTile.GetMoveCost());
                }
            }

            DssRef.world = world;
        }
    }
}
