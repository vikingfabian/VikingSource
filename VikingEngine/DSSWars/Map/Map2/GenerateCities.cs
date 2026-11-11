using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.LootFest.Map;

namespace VikingEngine.DSSWars.Map.Map2
{

    struct CityPlacementData
    {
        public int myIndex;
        public IntVector2 pos;
       
        public FlatArray_Three<IntVector2> secondary;
    }

    struct PlacementValue
    {
        public IntVector2 pos;

        public bool success;
        public float value;
    }

    class GenerateCities
    {
        public Grid2D_L<FlatArray_Three<CityPlacementData>> citiesOnNodes;
        PcgRandom rnd = new PcgRandom();

        public void generateCities(Map2GenerateSettings generateSettings, NodeMap nodeMap, IconWorldData icon)
        {
            List<Task> secondaryCitiesProcess = new List<Task>(128);


            try
            {
                

                List<IntVector2> cityNodePositions = new List<IntVector2>();

                IntVector2 CityGridSz = generateSettings.mapScale.Size(true) / generateSettings.SpaceBetweenCities;
                Rectangle2 area = new Rectangle2(IntVector2.One, CityGridSz - 2);
                ForXYLoop loop = new ForXYLoop(area);
                while (loop.Next())
                {
                    cityNodePositions.Add(loop.Position);
                    //if (rnd.Chance(0.5))
                    //{
                    //    cityNodePositions.Add(loop.Position);
                    //}
                }

                int numCities = MathExt.MultiplyInt(cityNodePositions.Count, conv.FromPercentage(generateSettings.cityPlacementPerc) /*0.25*/);
                icon.cities = new List<CityPlacementData>(numCities);

                const int CompareValueCount = 8;
                PlacementValue[] placementValues = new PlacementValue[CompareValueCount];
                int CheckRadius = generateSettings.SpaceBetweenCities;
                citiesOnNodes = new Grid2D_L<FlatArray_Three<CityPlacementData>>(CityGridSz);

                for (int i = 0; i < numCities; i++)
                {
                    IntVector2 rndNodePos = arraylib.RandomListMemberPop(cityNodePositions, rnd);
                    Rectangle2 nodearea = Rectangle2.FromCenterTileAndRadius(rndNodePos, 3);

                    Parallel.For(0, CompareValueCount, i =>
                    {
                        try
                        {
                            PlacementValue placementValue = new PlacementValue();
                            IntVector2 tryPos = rndNodePos * generateSettings.SpaceBetweenCities + new IntVector2(rnd.Int(CheckRadius), rnd.Int(CheckRadius));


                            if (allowedCityPlacement(tryPos))
                            {
                                //find closest city

                                float closest = float.MaxValue;

                                ForXYLoop nodeLoop = new ForXYLoop(nodearea);
                                while (nodeLoop.Next())
                                {
                                    if (citiesOnNodes.TryGet(nodeLoop.Position, out var cities))
                                    {
                                        for (int cityIndex = 0; cityIndex < cities.count; cityIndex++)
                                        {
                                            float distance = (cities[cityIndex].pos - tryPos).Length();
                                            if (distance < closest)
                                            {
                                                closest = distance;
                                            }
                                        }
                                    }
                                }

                                if (closest >= generateSettings.minCitySpacing)
                                {
                                    placementValue.pos = tryPos;
                                    placementValue.success = true;
                                    placementValue.value = closest;
                                }
                            }

                            placementValues[i] = placementValue;
                        }
                        catch (Exception e)
                        {
                            lib.DoNothing();
                        }
                    });

                    float bestPlacementValue = float.MinValue;
                    int bestPlacementIndex = -1;

                    for (int pIx = 0; pIx < placementValues.Length; pIx++)
                    {
                        if (placementValues[pIx].success)
                        {
                            if (placementValues[pIx].value > bestPlacementValue)
                            {
                                bestPlacementValue = placementValues[pIx].value;
                                bestPlacementIndex = pIx;
                            }
                        }
                    }

                    if (bestPlacementIndex >= 0)
                    {
                        IntVector2 cityPos = placementValues[bestPlacementIndex].pos;
                        CityPlacementData city = new CityPlacementData() { myIndex = icon.cities.Count, pos = cityPos };
                        icon.cities.Add(city);
                        citiesOnNodes.GetRef(rndNodePos).Add(city);

                        secondaryCitiesProcess.Add(Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                //Find secondary city placements
                                int Checks = Ref.rnd.Int(4, 8);

                                Rotation1D dir = Rotation1D.Random();
                                for (int i = 0; i < Checks; ++i)
                                {
                                    if (Ref.rnd.Chance(0.8))
                                    {
                                        IntVector2 pos = cityPos + new IntVector2(dir.Direction(Ref.rnd.Plus_MinusF(1f) + generateSettings.secondaryCitySpacing));
                                        if (allowedCityPlacement(pos))
                                        {
                                            city.secondary.Add(pos);
                                            if (city.secondary.IsFull())
                                            {
                                                break;
                                            }
                                        }
                                        dir.Add(MathExt.Tau / Checks);
                                    }
                                }

                                icon.cities[city.myIndex] = city;
                            }
                            catch (Exception e)
                            {
                                lib.DoNothing();
                            }
                        }));
                    }
                }
             }
            catch (Exception e)
            {
                lib.DoNothing();
            }
            lib.DoNothing();


            //Check for overlapping secondary cities
           
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await Task.WhenAll(secondaryCitiesProcess);

                    for (int i = 0; i < icon.cities.Count; ++i)
                    {
                        var primary = icon.cities[i];

                        //Check collisions with both main and secondary cities
                        for (int secIx = primary.secondary.count - 1; secIx >= 0; secIx--)
                        {
                            if (tooClose(primary.secondary[secIx], i - 1))
                            {
                                primary.secondary.RemoveAt(secIx);
                            }
                        }

                        icon.cities[i] = primary;

                        bool tooClose(IntVector2 pos, int maxSecondayIndex)
                        {
                            const int MinDistance = 16;

                            Rectangle2 nodearea = Rectangle2.FromCenterTileAndRadius(pos, 3);
                            ForXYLoop nodeLoop = new ForXYLoop(nodearea);
                            while (nodeLoop.Next())
                            {
                                if (citiesOnNodes.TryGet(nodeLoop.Position, out var cities))
                                {
                                    for (int cityIndex = 0; cityIndex < cities.count; cityIndex++)
                                    {
                                        var city = cities[cityIndex];
                                        float distance = (city.pos - pos).Length();
                                        if (distance < MinDistance)
                                        {
                                            return true;
                                        }

                                        if (city.myIndex > maxSecondayIndex && city.secondary.count > 0)
                                        {
                                            for (int citySecIndex = 0; citySecIndex < city.secondary.count; citySecIndex++)
                                            {
                                                distance = (city.secondary[citySecIndex] - pos).Length();
                                                if (distance < MinDistance)
                                                {
                                                    return true;
                                                }
                                            }
                                        }

                                        
                                    }
                                }
                            }
                            return false;
                        }
                    }

                }
                catch (Exception e)
                {
                    lib.DoNothing();
                }

        });
            

            bool allowedCityPlacement(IntVector2 pos)
            {
                if (icon.iconGrid.TryGet(pos, out var tile) &&
                    tile.WithinMayBuildHeight())
                {
                    foreach (var dir in IntVector2.Dir8Array)
                    {
                        if (!(icon.iconGrid.TryGet(pos + dir, out var ntile) &&
                            ntile.WithinMayBuildHeight()))
                        {
                            return false;
                        }
                    }

                    int ringCount = 0;
                    ForXYEdgeLoop loop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(pos, 2));
                    while (loop.Next())
                    {
                        if (icon.iconGrid.TryGet(loop.Position, out var ntile) &&
                            ntile.WithinMayBuildHeight())
                        {
                            ringCount++;
                        }
                    }

                    return ringCount > 9;
                }

                return false;
            }
        }

        public void scaleUp16x(IconWorldData icon)
        {
            for (int i = 0; i < icon.cities.Count; i++)
            {
                var c = icon.cities[i];
                c.pos *= 16;
                c.secondary.value1 *= 16;
                c.secondary.value2 *= 16;
                c.secondary.value3 *= 16;

                icon.cities[i] = c;
            }
        }
    }
}
