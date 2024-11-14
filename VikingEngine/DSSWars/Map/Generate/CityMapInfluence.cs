using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.Map;

namespace VikingEngine.DSSWars.Map
{
    class CityMapInfluence
    {
        Grid2D<Influence> inflenceMap;
        List<MapCity> cities;

        //public void reset(WorldData world)
        //{
        //    for (int i = 0; i < world.cities.Count; ++i)
        //    {
        //        world.cities[i].parentArrayIndex = i;
        //        world.cities[i].neighborCities.Clear();
        //        world.cities[i].areaSize = 0;
        //    }

        //    world.tileGrid.LoopBegin();
        //    while (world.tileGrid.LoopNext())
        //    {
        //        var tile = world.tileGrid.LoopValueGet();
        //        tile.removeCity();

        //    }
        //}
        public void generate(WorldData world) 
        {
            inflenceMap = new Grid2D<Influence>(world.Size);
            inflenceMap.LoopBegin();
            while (inflenceMap.LoopNext())
            {
                inflenceMap.LoopValueSet(new Influence());
            }

            cities = new List<MapCity>(world.cities.Count);
            //List<MapCity> complete = new List<MapCity>(world.cities.Count);

            foreach (var c in world.cities)
            {
                cities.Add(new MapCity(c, inflenceMap));
            }

            int loopCount = 0;
            while (cities.Count > 0)
            {
                for (int i = cities.Count - 1; i >= 0; --i)
                {
                    if (cities[i].next(inflenceMap, world))
                    {
                        //complete.Add(cities[i]);
                        cities.RemoveAt(i);
                    }
                }

                if (++loopCount > 10000)
                {
                    throw new EndlessLoopException("CityMapInfluence");
                }
            }

            cleanUpEdges(world);

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


            inflenceMap.LoopBegin();
            while (inflenceMap.LoopNext())
            {
                if (inflenceMap.LoopValueGet().city == null)
                {
                    throw new Exception();
                }

                var city = inflenceMap.LoopValueGet().city;
                var tile = world.tileGrid.Get(inflenceMap.LoopPosition);
                {
                    tile.CityIndex = city.parentArrayIndex;
                }
                world.tileGrid.Set(inflenceMap.LoopPosition, tile);
                
                var r = inflenceMap.LoopPosition.SideLength(city.tilePos);
                if (city.cityTileRadius < r)
                { 
                    city.cityTileRadius = r;
                }
            }
        }

        void cleanUpEdges(WorldData world)
        {
            Rectangle2 area = inflenceMap.Area;
            area.AddRadius(-1);
            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                if (world.tileGrid.Get(loop.Position).IsLand())
                {

                    Dictionary<int, int> cityInfluence = new Dictionary<int, int>();
                    var inf = inflenceMap.Get(loop.Position);
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

        class Influence
        {
            public City city = null;
            public int influence = 0;
            public bool locked = false;
        }

        class MapCity
        {
            GameObject.City city;
            ForXYEdgeLoop loop;
            int radius = 1;

            public MapCity(GameObject.City city, Grid2D<Influence> inflenceMap)
            { 
                this.city = city;
                //Workforce är ca 300
                int startInfluence = 20000 + city.workForceMax * 200;
                Rectangle2 startArea = Rectangle2.FromCenterTileAndRadius(city.tilePos, 1);
                ForXYLoop startloop = new ForXYLoop(startArea);
                while(startloop.Next())
                {
                    var inf = inflenceMap.Get(startloop.Position);
                    inf.city = city;
                    inf.influence = startInfluence;
                    inf.locked = true;  
                }

                loop = new ForXYEdgeLoop(startArea);
            }

            /// <returns>Is complete</returns>
            public bool next(Grid2D<Influence> inflenceMap, WorldData world)
            { 
                bool madeInflence = false;

                while (loop.Next())
                {
                    Influence inf;
                    if (inflenceMap.TryGet(loop.Position, out inf))
                    {
                        if (inf.city == city)
                        {
                            int influence = inf.influence;
                            int support = 0;
                            //Collect supporting tiles (avoid thin areas)
                            foreach (var dir in IntVector2.Dir8Array)
                            {
                                Influence adjInf;
                                var npos = loop.Position + dir;
                                if (inflenceMap.TryGet(npos, out adjInf))
                                {
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
                                Influence adjInf;
                                var npos = loop.Position + dir;
                                if (inflenceMap.TryGet(npos, out adjInf))
                                {
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
                    loop.ExpandRadius();
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
