using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;

namespace VikingEngine.DSSWars.Map.Map2
{
    /// <summary>
    /// Convert generated icon map to the full map data and post process terrain
    /// </summary>
    class Map2Builder
    {
        public WorldData world;

        public void ConvertIcon(Map2Generator generator)
        {
            world = new WorldData(generator.world.metaData, generator.generateSettings);

            Parallel.For(0, generator.iconWorld.iconGrid.Size.X, x =>
            {
                for (int y = 0; y < generator.iconWorld.iconGrid.Size.Y; y++)
                {
                    var genTile = generator.world.tileGrid.Get(x, y);
                    MapTile1_1 mapTile = new MapTile1_1()
                    {
                        heightValue = MapHeight2.HeightY_Interval.GetValueBytePercentPos_WithBound(genTile.groundY),
                    };

                    mapTile.mainTerrain = mapTile.heightValue > MapHeight2.WaterPlaneHeight ? TerrainMainType.DefaultLand : TerrainMainType.DefaultSea;
                }
            });

            Parallel.For(0, world.tileGrid.Size.X, x =>
            {
                int tileX = x * SumTile4_4.TileWidth + 1;
                for (int y = 0; y < world.tileGrid.Size.Y; y++)
                {
                    int tileY = y * SumTile4_4.TileWidth + 1;

                    var genTile = generator.world.tileGrid.Get(tileX, tileY);
                    SumTile4_4 sumTile = new SumTile4_4()
                    {
                        biom1 = genTile.biom1,
                        biom2 = genTile.biom2,
                        secondaryBiomStrength = (byte)(genTile.secondBiomWeight * byte.MaxValue),
                        biomColorHeight = (byte)(MapHeight2.HeightY_Interval.GetValueBytePercentPos_WithBound(genTile.groundY) / MapHeight2.ColorLayerHeight)
                    };
                }
            });
        }
    }
}
