
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;

namespace VikingEngine.DSSWars.Map.Map2
{
    /// <summary>
    /// Convert generated icon map to the full map data and post process terrain
    /// </summary>
    class IconMapConverter
    {
        public WorldData world;

        public void ConvertIcon(IconMapGenerator generator)
        {
            world = new WorldData(generator.iconWorldScaledUp.metaData, generator.generateSettings);

            Parallel.For(0, generator.iconWorldScaledUp.iconGrid.Size.X, x =>
            {
                for (int y = 0; y < generator.iconWorldScaledUp.iconGrid.Size.Y; y++)
                {
                    var genTile = generator.iconWorldScaledUp.iconGrid.Get(x, y);
                    MapTile1_1 mapTile = new MapTile1_1()
                    {
                        heightValue = MapHeight2.HeightY_Interval.GetValueBytePercentPos_WithBound(genTile.groundY),
                    };

                    mapTile.mainTerrain = mapTile.heightValue > MapHeight2.WaterPlaneHeight ? TerrainMainType.DefaultLand : TerrainMainType.DefaultSea;
                    world.subTileGrid.Set(x, y, mapTile);

                }
            });

            Parallel.For(0, world.tileGrid.Size.X, x =>
            {
                int tileX = x * SumTile4_4.TileWidth + 1;
                for (int y = 0; y < world.tileGrid.Size.Y; y++)
                {
                    int tileY = y * SumTile4_4.TileWidth + 1;

                    var genTile = generator.iconWorldScaledUp.iconGrid.Get(tileX, tileY);

                    SumTile4_4 sumTile = new SumTile4_4()
                    {
                        biom1 = genTile.biom1,
                        biom2 = genTile.biom2,
                        secondBiomWeight = (byte)(genTile.secondBiomWeight * byte.MaxValue),
                        biomColorHeight = (byte)(MapHeight2.HeightY_Interval.GetValueBytePercentPos_WithBound(genTile.groundY) / MapHeight2.ColorLayerHeight),
                        IsFullTile = genTile.WithinMayBuildHeight(),
                    };
                    world.tileGrid.Set(x, y, sumTile);
                }
            });

            //Find full tiles
            Task.Factory.StartNew(() =>
            {

            });

            CityType defaultSize;
            switch (DssRef.storage.ruleset_instance.factionStartSize)
            {
                default:
                case FactionStartSize.Settler:
                    defaultSize = CityType.Campsite;
                    break;
                case FactionStartSize.OneCity:
                    defaultSize = CityType.Village;
                    break;
                case FactionStartSize.Full:
                    defaultSize = CityType.Town;
                    break;

            }

            foreach (var cityPos in generator.iconWorldScaledUp.cities)
            {
                City c = new City(world.cities.Count, cityPos.pos, defaultSize, world) { primaryCity = true };                
                world.cities.Add(c);

                if (DssRef.storage.ruleset_instance.factionStartSize == FactionStartSize.Full)
                {
                    int pullCount = Math.Min(cityPos.secondary.count, 2);
                    for (int i = 0; i < pullCount; ++i)
                    {
                        var secondPos = cityPos.secondary.PullRandom();

                        City c2 = new City(world.cities.Count, secondPos, CityType.Village, world);
                        world.cities.Add(c2);
                        c.naturalExpansion.Add(c2.myIndex);
                    } 
                }
            }
        }

        public IconWorldData ConvertBackToIcon(WorldData world)
        {
            const int ScaleDown = 16;
            IconWorldData icon = new IconWorldData(world.metaData);
            //world = new WorldData(generator.iconWorldScaledUp.metaData, generator.generateSettings);

            ForXYLoop mapLoop = new ForXYLoop(world.subTileGrid.Size / ScaleDown);
            //mapLoop.stepLength = ScaleDown;

            while (mapLoop.Next())
            {
                var tile = world.GetCombinedTile(mapLoop.Position * ScaleDown);

                icon.iconGrid.Set(mapLoop.Position, tile.GenTile());
            }

            icon.cities = new List<CityPlacementData>(world.cities.Count);
            var citiesC = world.cities.counter();
            while(citiesC.Next())//foreach (var c in world.cities)
            {
                icon.cities.Add(new CityPlacementData() { myIndex = citiesC.sel.myIndex, pos = citiesC.sel.maptilePos / ScaleDown });
            }

            return icon;
           
        }
    }
}
