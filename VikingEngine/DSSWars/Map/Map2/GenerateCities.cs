using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.LootFest.Map;

namespace VikingEngine.DSSWars.Map.Map2
{

    

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
            try
            {
                const int CityGridPixSz = 8;

                List<IntVector2> cityNodePositions = new List<IntVector2>();

                IntVector2 CityGridSz = icon.iconGrid.Size / CityGridPixSz;
                Rectangle2 area = new Rectangle2(IntVector2.One, CityGridSz - 2);
                ForXYLoop loop = new ForXYLoop(area);
                while (loop.Next())
                {
                    cityNodePositions.Add(loop.Position );
                    //if (rnd.Chance(0.5))
                    //{
                    //    cityNodePositions.Add(loop.Position);
                    //}
                }

                int numCities = MathExt.MultiplyInt(cityNodePositions.Count, 0.25);
                icon.cities = new List<CityPlacementData>(numCities);

                const int CompareValueCount = 8;
                PlacementValue[] placementValues = new PlacementValue[CompareValueCount];
                const int CheckRadius = CityGridPixSz;
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
                            IntVector2 tryPos = rndNodePos * CityGridPixSz + new IntVector2(rnd.Int(CheckRadius), rnd.Int(CheckRadius));


                            if (icon.iconGrid.TryGet(tryPos, out var tile) &&
                                tile.groundY >= MapHeight2.LowGroundY && tile.groundY <= MapHeight2.MountainStartY)
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
                    }


                }
             }
            catch (Exception e)
            {
                lib.DoNothing();
            }
            lib.DoNothing();
        }

        public void scaleUp16x(IconWorldData icon)
        {
            for (int i = 0; i < icon.cities.Count; i++)
            {
                var c = icon.cities[i];
                c.pos *= 16;
                icon.cities[i] = c;
            }
        }
    }
}
