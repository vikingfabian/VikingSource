using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapProcess;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.LootFest.Data;
using VikingEngine.PJ.Tanks;

namespace VikingEngine.DSSWars.Map.Map2
{
    class Map2PostGenerate
    {
        public bool citiesComplete = false;
        GenerateRegion region = new GenerateRegion();
        CityCultureCollection cityCultureCollection = new CityCultureCollection();
        WorldData world;
        VikingEngine.EngineSpace.Maths.SimplexNoise2D noiseMap;
        public Map2PostGenerate(WorldData world) 
        { 
            this.world = world;
            noiseMap = new EngineSpace.Maths.SimplexNoise2D(world.metaData.worldId.seed);
        }

        public async void citiesAndFactionsSetup(Map2GenerateSettings generateSettings)
        {
            cityAreaClaim();

            factionStartAreas(world.metaData.mapSize,
                DssRef.storage.ruleset.factionStartSize != FactionStartSize.Full,
                generateSettings);

            placeCityBuildings();
        }

        public async void cityAreaClaim()
        {
            List<Task> tasks = new List<Task>();

            foreach (var c in world.cities)
            {
                City city = c;
                
                // Start the task and add it to the list
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    try
                    {
                        CityMapClaim2.CityClaim(city);
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }

                }));
            }

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);
        }

        public async void placeCityBuildings()
        {
            CityTemplateCollection templateCollection = new CityTemplateCollection();

            // Create a list to hold the tasks
            List<Task> tasks = new List<Task>();

            foreach (var c in world.cities)
            {
                City city = c;


                // Start the task and add it to the list
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    try
                    {
                        city.createBuildingSubtiles(world, templateCollection);
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }

                }));
            }

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);
        }

        void factionStartAreas(MapSize mapSize, bool oneCity, Map2GenerateSettings generateSettings)
        {
            int goalWorkForce = DssConst.HeadCityStartMaxWorkForce + DssConst.LargeCityStartMaxWorkForce + DssConst.SmallCityStartMaxWorkForce;

            if (mapSize >= MapSize.Epic)
            {
                goalWorkForce += DssConst.HeadCityStartMaxWorkForce;
            }
            else if (mapSize >= MapSize.Huge)
            {
                goalWorkForce += DssConst.LargeCityStartMaxWorkForce;
            }

            bool useRandomEmpires = mapSize >= MapSize.Medium;
            IntervalF randomEmpiresSizeMulti = new IntervalF(1.5f, 2f + (mapSize - MapSize.Medium));

            //if (DssRef.difficulty.setting_gameMode == GameModeMainType.QuickMatch)
            //{
            //    namedFactionsOnMap_QuickMatch(DssRef.difficulty.QuickMatchPlayerStartSize(), oneCity);
            //}
            //else
            {
                namedFactionsOnMap(goalWorkForce, oneCity);
            }
            //var last = world.cities.Last();

            foreach (City c in world.cities)
            {
                if (c.pfaction.IsEmpty() && c.cityType > CityType.UnClaimed)
                {
                    int size = goalWorkForce;
                    bool rndEmpire = useRandomEmpires && world.rnd.Chance(0.25);
                    if (rndEmpire)
                    {
                        size = MathExt.MultiplyInt(randomEmpiresSizeMulti.GetRandom(world.rnd), size);
                    }

                    size = MathExt.MultiplyInt(size, 1.0 - generateSettings.percentageUnclaimed);

                    //region.Reset((int)size);
                    var faction = new Faction(world, FactionType.DefaultAi);
                    int regionCurrentWorkforce = region.GetStartFactionRegion(size, oneCity, c, world, faction);


                    if ((regionCurrentWorkforce >= size && !rndEmpire) || oneCity)
                    {
                        faction.availableForPlayer = true;
                    }
                }
            }

            if (world.factions.Count > DssLib.RtsMaxFactions)
            {
                throw new Exception("RtsMaxFactions");
            }
        }

        public void placeTerrain()
        {
            List<IntVector2> mineLocations = new List<IntVector2>(1024);
            List<IntVector2> animalSpawns = new List<IntVector2>(1024);

            Parallel.For(0, world.subTileGrid.Size.X, x =>
            {
                for (int y = 0; y < world.subTileGrid.Size.Y; y++)
                {
                    var ctile = world.GetCombinedTile(new IntVector2(x, y));//new MapTile1_1(tiletype, subType, rndColor, topY);
                    TerrainContent.createSubTileContent(x, y, ref ctile, world, noiseMap, mineLocations, animalSpawns);
                    world.subTileGrid.Set(x, y, ctile.mapTile);
                }
            });
        }
        void bindTilesToCities()
        {

            // figure out which tile is closest to which city, version 2
            new CityMapInfluence().generate(world);
            /*

            //calc what tiles are in border to eachother
            Rectangle2 area = world.tileGrid.Area;
            area.AddRadius(-1);

            ForXYLoop loop = new ForXYLoop(area);

            while (loop.Next())
            {
                SumTile4_4 t = world.tileGrid.Get(loop.Position);
                if (t.IsLand())
                {
                    //if (!arraylib.InBound(world.cities, t.CityIndex)) 
                    //{
                    //    lib.DoNothing();
                    //}

                    City owner = world.cities[t.CityIndex];
                    int borderCity = -1;

                    for (int dirIx = 0; dirIx < IntVector2.Dir4Array.Length; ++dirIx)
                    {
                        IntVector2 dir = IntVector2.Dir4Array[dirIx];
                        SumTile4_4 neighbor = world.tileGrid.Get(dir.X + loop.Position.X, dir.Y + loop.Position.Y);
                        bool land = neighbor.IsLand();
                        if (neighbor.CityIndex != owner.myIndex)
                        {
                            t.AddBorder(dirIx, land ? neighbor.CityIndex : SumTile4_4.SeaBorder);
                            borderCity = neighbor.CityIndex;
                        }
                    }

                    if (t.BorderCount > 0)
                    {
                        if (!arraylib.InBound(world.cities, borderCity))
                        {
                            lib.DoNothing();
                        }
                        owner.AddNeighborCity(world, borderCity);
                    }

                    world.tileGrid.Set(loop.Position, t);
                }
            }

            */
        }

        public void generateCityBuildings()
        {
            //this.world = world;
            world.rnd = new PcgRandom(world.metaData.worldId.seed);

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    //GenerateRoads roads = new GenerateRoads();

                    //if (loadMeta == null)
                    {
                        CityTemplateCollection templateCollection = new CityTemplateCollection();

                        // Create a list to hold the tasks
                        List<Task> tasks = new List<Task>();

                        foreach (var c in world.cities)
                        {
                            City city = c;


                            // Start the task and add it to the list
                            tasks.Add(Task.Factory.StartNew(() =>
                            {
                                try
                                {
                                    city.createBuildingSubtiles(world, templateCollection);
                                }
                                catch (Exception ex)
                                {
                                    BlueScreen.ThreadException = ex;
                                }

                            }));
                        }

                        // Wait for all tasks to complete
                        await Task.WhenAll(tasks);

                        tasks.Clear();

                        /*
                        foreach (var c in world.cities)
                        {
                            // Start the task and add it to the list
                            tasks.Add(Task.Factory.StartNew(() =>
                            {
                                try
                                {
                                    if (!abort)
                                    {
                                        roads.fromCity(world, c);
                                    }
                                }
                                catch (Exception ex)
                                {
#if DEBUG
                                    BlueScreen.ThreadException = ex;
#endif
                                }

                            }));
                        }

                        // Wait for all tasks to complete
                        await Task.WhenAll(tasks);
                        */
                    }
                    citiesComplete = true;
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
            });
        }
        /*
        void factionStartAreas(MapSize mapSize, bool oneCity, Map2GenerateSettings generateSettings)
        {
            int goalWorkForce = DssConst.HeadCityStartMaxWorkForce + DssConst.LargeCityStartMaxWorkForce + DssConst.SmallCityStartMaxWorkForce;

            if (mapSize >= MapSize.Epic)
            {
                goalWorkForce += DssConst.HeadCityStartMaxWorkForce;
            }
            else if (mapSize >= MapSize.Huge)
            {
                goalWorkForce += DssConst.LargeCityStartMaxWorkForce;
            }

            bool useRandomEmpires = mapSize >= MapSize.Medium;
            IntervalF randomEmpiresSizeMulti = new IntervalF(1.5f, 2f + (mapSize - MapSize.Medium));

            //if (DssRef.difficulty.setting_gameMode == GameModeMainType.QuickMatch)
            //{
            //    namedFactionsOnMap_QuickMatch(DssRef.difficulty.QuickMatchPlayerStartSize(), oneCity);
            //}
            //else
            //{
                namedFactionsOnMap(goalWorkForce, oneCity);
            //}
            //var last = world.cities.Last();

            foreach (City c in world.cities)
            {
                if (c.pfaction.IsEmpty() && c.cityType > CityType.UnClaimed)
                {
                    int size = goalWorkForce;
                    bool rndEmpire = useRandomEmpires && world.rnd.Chance(0.25);
                    if (rndEmpire)
                    {
                        size = MathExt.MultiplyInt(randomEmpiresSizeMulti.GetRandom(world.rnd), size);
                    }

                    size = MathExt.MultiplyInt(size, 1.0 - generateSettings.percentageUnclaimed);

                    //region.Reset((int)size);
                    var faction = new Faction(world, FactionType.DefaultAi);
                    int regionCurrentWorkforce = region.GetStartFactionRegion(size, oneCity, c, world, faction);


                    if ((regionCurrentWorkforce >= size && !rndEmpire) || oneCity)
                    {
                        faction.availableForPlayer = true;
                    }
                }
            }

            if (world.factions.Count > DssLib.RtsMaxFactions)
            {
                throw new Exception("RtsMaxFactions");
            }
        }
        */
        void namedFactionsOnMap(int standardWorkForce, bool oneCity)
        {
            bool bFullStory = DssRef.difficulty.setting_gameMode == GameModeMainType.FullStory;
            if (bFullStory)
            {
                var faction = new Faction(world, FactionType.DarkFollower);

                int size = MathExt.MultiplyInt(3, standardWorkForce);
                region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.DarkLands), world, faction);

            }

            if (bFullStory)
            {
                var faction = new Faction(world, FactionType.UnitedKingdom);

                int size = MathExt.MultiplyInt(5, standardWorkForce);

                region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.WestKingdom), world, faction);

            }

            {
                var faction = new Faction(world, FactionType.GreenWood);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.LargeGreen), world, faction);

            }

            if (world.metaData.mapSize >= MapSize.Medium)
            {
                {
                    var faction = new Faction(world, FactionType.DyingMonger);

                    int size = MathExt.MultiplyInt(2, standardWorkForce);

                    region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.DryEast), world, faction);

                }
                {
                    var faction = new Faction(world, FactionType.DyingHate);

                    int size = MathExt.MultiplyInt(2, standardWorkForce);

                    region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.DryEast), world, faction);

                }
                {
                    var faction = new Faction(world, FactionType.DyingDestru);

                    int size = MathExt.MultiplyInt(2, standardWorkForce);

                    region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.DryEast), world, faction);

                }

            }

            {
                var faction = new Faction(world, FactionType.EasternEmpire);

                int size = MathExt.MultiplyInt(3, standardWorkForce);

                region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.DryEast), world, faction);

            }

            {
                var faction = new Faction(world, FactionType.NordicRealm);

                int size = MathExt.MultiplyInt(2, standardWorkForce);

                region.GetStartFactionRegion(size, false, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);

            }


            if (DateTime.Now.Month == 12 || PlatformSettings.DebugLevel == BuildDebugLevel.Dev)
            {
                var faction = new Faction(world, FactionType.Tomten);

                int size = MathExt.MultiplyInt(0.5, standardWorkForce);

                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);

            }

            {
                var faction = new Faction(world, FactionType.BearClaw);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);
                //region.ApplyFaction(BearClaw);
            }

            {
                var faction = new Faction(world, FactionType.NordicSpur);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);
                //region.ApplyFaction(NordicSpur);
            }

            {
                var faction = new Faction(world, FactionType.IceRaven);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.NorthSea), world, faction);
                //region.ApplyFaction(IceRaven);
            }

            {
                var faction = new Faction(world, FactionType.DragonSlayer);

                int size = MathExt.MultiplyInt(1.5, standardWorkForce);

                region.GetStartFactionRegion(size, oneCity, randomCity(), world, faction);
                //region.ApplyFaction(DragonSlayer);
            }



            {
                var faction = new Faction(world, FactionType.BramblebrookHill);
                int size = MathExt.MultiplyInt(0.3, standardWorkForce);
                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.LargeGreen), world, faction);
            }
            {
                var faction = new Faction(world, FactionType.Tumblehill);
                int size = MathExt.MultiplyInt(0.3, standardWorkForce);
                region.GetStartFactionRegion(size, oneCity, collection_pullNextCity(cityCultureCollection.LargeGreen), world, faction);
            }
        }
        City collection_pullNextCity(List<City> collection)
        {
            while (collection.Count > 0)
            {
                var city = arraylib.RandomListMemberPop(collection, world.rnd);
                if (city.pfaction.IsEmpty() && city.cityType > CityType.UnClaimed)
                {
                    return city;
                }
            }

            return randomCity();

        }

        City randomCity()
        {
            int ix = world.rnd.Int(world.cities.Count);

            while (world.cities[ix].pfaction.HasValue() || world.cities[ix].cityType == CityType.UnClaimed)
            {
                ix++;
                if (ix >= world.cities.Count)
                {
                    ix = 0;
                }
            }

            return world.cities[ix];
        }

    }
}
