using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.LootFest.Map;

namespace VikingEngine.DSSWars.Map
{
    class CityMapInfluence
    {
        Grid2D<Influence> inflenceMap;
        MapCity[] cities;

        public bool generate(WorldData world) 
        {
            inflenceMap = new Grid2D<Influence>(world.Size);
            //inflenceMap.LoopBegin();
            //while (inflenceMap.LoopNext())
            //{
            //    inflenceMap.LoopValueSet(new Influence());
            //}

            cities = new MapCity[world.cities.Count];//new List<MapCity>(world.cities.Count);

            int verticalDivitions = world.Size.X / (Generate.GenerateMap.HeadCityNeededFreeRadius * 4);
            
            bool result = Task.Run(async ()=> {

                List<Task> tasks = new List<Task>(verticalDivitions);

                const int InitDivitions = 8;
                int cityCountDiv = world.cities.Count / 8;
                for (int i = 0; i < InitDivitions; ++i)
                { 
                    int start = i * cityCountDiv;
                    int end_plusone = start + cityCountDiv;
                    if (i == InitDivitions - 1)
                    {
                        end_plusone = world.cities.Count;
                    }

                    tasks.Add(Task.Run(() =>
                    {
                        for (int cityIx = start; cityIx < end_plusone; cityIx++)
                        {
                            cities[cityIx] = new MapCity(world.cities[cityIx], inflenceMap);
                        }
                    }));
                }

                //foreach (var c in world.cities)
                //{
                //    cities.Add(new MapCity(c, inflenceMap));
                //}

                await Task.WhenAll(tasks);
                tasks.Clear();

                //Optimized by diviting the world in vertical stripes, and running even, then odd

                int loopCount = 0;

                bool activeCities = false;

                //while (cities.Count > 0)
                do
                {
                    activeCities = false;

                    for (int evenOdd = 0; evenOdd < 2; evenOdd++)
                    {
                        for (int verticalStripeIx = 0; verticalStripeIx < verticalDivitions; verticalStripeIx++)
                        {
                            if (lib.IsEven(evenOdd) == lib.IsEven(verticalStripeIx))
                            {
                                int ix = verticalStripeIx;
                                tasks.Add(Task.Run(() =>
                                {
                                    Rectangle2 area = cityArea(ix, verticalDivitions, true);

                                    for (int cityIx = 0; cityIx < world.cities.Count; cityIx++)
                                    {
                                        if (cities[cityIx].active)
                                        {
                                            activeCities = true;

                                            if (area.IntersectTilePoint(cities[cityIx].city.tilePos))
                                            {
                                                cities[cityIx].next(inflenceMap, world);
                                            }
                                        }
                                    }

                                }));
                            }
                        }
                        await Task.WhenAll(tasks);
                        tasks.Clear();
                    }
                    //for (int i = cities.Count - 1; i >= 0; --i)
                    //{
                    //    if (cities[i].next(inflenceMap, world))
                    //    {
                    //        cities.RemoveAt(i);
                    //    }
                    //}

                    if (++loopCount > 10000)
                    {
                        throw new EndlessLoopException("CityMapInfluence");
                    }
                } while (activeCities);

                

                debugLog();

                for (int evenOdd = 0; evenOdd < 2; evenOdd++)
                {
                    for (int verticalStripeIx = 0; verticalStripeIx < verticalDivitions; verticalStripeIx++)
                    {
                        if (lib.IsEven(evenOdd) == lib.IsEven(verticalStripeIx))
                        {
                            int ix = verticalStripeIx;
                            tasks.Add(Task.Run(() =>
                            {
                                Rectangle2 area = cityArea(ix, verticalDivitions, true);

                                cleanUpEdges(world, area);

                            }));
                        }
                    }
                    await Task.WhenAll(tasks);
                    tasks.Clear();
                }


                for (int evenOdd = 0; evenOdd < 2; evenOdd++)
                {
                    for (int verticalStripeIx = 0; verticalStripeIx < verticalDivitions; verticalStripeIx++)
                    {
                        if (lib.IsEven(evenOdd) == lib.IsEven(verticalStripeIx))
                        {
                            int ix = verticalStripeIx;
                            tasks.Add(Task.Run(() =>
                            {
                                Rectangle2 area = cityArea(ix, verticalDivitions, false);

                                bindTiles(world, area);

                            }));
                        }
                    }
                    await Task.WhenAll(tasks);
                    tasks.Clear();
                }


                Rectangle2 cityArea(int part, int divitions, bool insertEdges)
                {
                    Rectangle2 area = new Rectangle2();
                    int widthChunk = world.Size.X / divitions;
                    area.X = part * widthChunk;
                    area.Width = widthChunk;

                    if (part == 0 && insertEdges)
                    {
                        area.AddToLeftSide(-1);
                    }
                    else if (part == divitions - 1)
                    {
                        //last
                        if (insertEdges)
                        {
                            area.SetRight(world.Size.X - 1, true);
                        }
                        else
                        {
                            area.SetRight(world.Size.X, true);
                        }
                    }

                    if (insertEdges)
                    {
                        area.Y = 1;
                        area.Height = world.Size.Y - 2;
                    }
                    else
                    {
                        area.Y = 0;
                        area.Height = world.Size.Y;
                    }
                    return area;
                }

                return true;
            }).Result;

            return result;
        }

        void debugLog()
        {
            const bool LogInfluence = false;
            if (LogInfluence)
            {
                for (int y = 0; y < inflenceMap.Height; ++y)
                {
                    StringBuilder line = new StringBuilder();
                    for (int x = 0; x < inflenceMap.Width; ++x)
                    {
                        line.Append(string.Format("{0:D6}", inflenceMap.array[x, y].city.ToString()));
                        line.Append(',');
                    }

                    System.Diagnostics.Debug.WriteLine(line.ToString());
                }
            }
        }

        void cleanUpEdges(WorldData world, Rectangle2 area)
        {
            //Rectangle2 area = inflenceMap.Area;
            //area.AddRadius(-1);
            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                if (world.tileGrid.Get(loop.Position).IsLand())
                {
                    Dictionary<int, int> cityInfluence = new Dictionary<int, int>();
                    ref var inf = ref inflenceMap.GetRef(loop.Position);
                    cityInfluence.Add(inf.city.parentArrayIndex, 1);

                    int mostInfluence = 1;
                    int mostInfluenceCity = inf.city.parentArrayIndex;

                    foreach (var dir in IntVector2.Dir8Array)
                    {
                        IntVector2 npos = loop.Position + dir;
                        if (world.tileGrid.Get(npos).IsLand())
                        {
                            var city = inflenceMap.Get(npos).city;
                            if (cityInfluence.ContainsKey(city.parentArrayIndex))
                            {
                                ++cityInfluence[city.parentArrayIndex];
                            }
                            else
                            {
                                cityInfluence.Add(city.parentArrayIndex, 1);
                            }
                        }
                    }

                    foreach (var kv in cityInfluence)
                    {
                        if (kv.Value > mostInfluence)
                        {
                            mostInfluence = kv.Value;
                            mostInfluenceCity = kv.Key;
                        }
                    }

                    inf.city = world.cities[ mostInfluenceCity];
                }
            }
        }

        void bindTiles(WorldData world, Rectangle2 area)
        {
            //End by binding tiles to cities
            //inflenceMap.LoopBegin();
            ForXYLoop loop = new ForXYLoop(area);
            Debug.Log("bindTiles " + area.ToString());
            while (loop.Next())
            {
                var city = inflenceMap.Get(loop.Position).city;
                if (city == null)
                {
                    throw new Exception();
                }
                
                var tile = world.tileGrid.Get(loop.Position);
                tile.CityIndex = city.parentArrayIndex;
                world.tileGrid.Set(loop.Position, tile);

                var r = loop.Position.SideLength(city.tilePos);
                if (city.cityTileRadius < r)
                {
                    city.cityTileRadius = r;
                }
            }
        }

        struct Influence
        {
            public City city;
            public int influence;
            public bool locked;
        }

        class MapCity
        {
            public GameObject.City city;
            ForXYEdgeLoop edgeloop;
            int radius = 1;
            public bool active = true;

            public MapCity(GameObject.City city, Grid2D<Influence> inflenceMap)
            { 
                this.city = city;
                //Workforce är ca 300
                int startInfluence = 20000 + city.HousingCount_Workers * 200;
                Rectangle2 startArea = Rectangle2.FromCenterTileAndRadius(city.tilePos, 1);
                ForXYLoop startloop = new ForXYLoop(startArea);
                while(startloop.Next())
                {
                    ref var inf = ref inflenceMap.GetRef(startloop.Position);
                    inf.city = city;
                    inf.influence = startInfluence;
                    inf.locked = true;  
                }

                edgeloop = new ForXYEdgeLoop(startArea);
            }

            /// <returns>Is complete</returns>
            public bool next(Grid2D<Influence> inflenceMap, WorldData world)
            { 
                //Will loop the outer edge of the city influence, and end by expanding one tile
                //All cities will expand this way until they meet eachother

                bool madeInflence = false;

                while (edgeloop.Next())
                {
                    //Influence inf;
                    if (inflenceMap.InBounds(edgeloop.Position))
                    {
                        ref var inf = ref inflenceMap.GetRef(edgeloop.Position);
                        if (inf.city == city)
                        {
                            int influence = inf.influence;
                            int support = 0;
                            //Collect supporting tiles (avoid thin areas)
                            foreach (var dir in IntVector2.Dir8Array)
                            {
                                //Influence adjInf;
                                var npos = edgeloop.Position + dir;
                                if (inflenceMap.InBounds(npos))//, out adjInf))
                                {
                                    ref var adjInf = ref inflenceMap.GetRef(npos);
                                    if (adjInf.city == city)
                                    {
                                        support++;
                                    }
                                }
                            }

                            if (support == 0)
                            {
                                influence /= 64;
                            }
                            else if (support == 1)
                            {
                                influence /= 16;
                            }
                            else if (support == 2)
                            {
                                influence /= 2;
                            }

                            foreach (var dir in IntVector2.Dir8Array)
                            {
                                //Influence adjInf;
                                var npos = edgeloop.Position + dir;
                                if (inflenceMap.InBounds(npos))//, out adjInf))
                                {
                                    ref var adjInf = ref inflenceMap.GetRef(npos);
                                    if (!adjInf.locked)
                                    {
                                        double length = (npos-city.tilePos).Length64();
                                        int cost = world.tileGrid.Get(npos).heightSett().influenceCost + adjInf.influence;
                                        cost += Convert.ToInt32(length * length) * 10;

                                        if (adjInf.city == null || cost < influence)
                                        {
                                            adjInf.city = city;
                                            adjInf.influence = influence - Math.Max(cost, 0);
                                            madeInflence = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (madeInflence)
                {
                    edgeloop.ExpandRadius();
                    return false;
                }
                else
                {
                    active = false;
                    return true;
                }
            }
        }
    }
}
