using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.LootFest.Map;

namespace VikingEngine.DSSWars.Map.Map2
{

    struct CityPlacementData
    {
        public int myIndex;
        public IntVector2 pos;

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
        public List<CityPlacementData> cities;

        PcgRandom rnd = new PcgRandom();

        public void generateCities(Map2GenerateSettings generateSettings, NodeMap nodeMap, IconWorldData icon)
        {

            List<IntVector2> cityNodePositions = new List<IntVector2>();

            ForXYLoop loop = new ForXYLoop(nodeMap.nodeGrid.Size);
            while (loop.Next())
            {
                if (nodeMap.nodeGrid.Get(loop.Position))
                {
                    cityNodePositions.Add(loop.Position);
                    if (rnd.Chance(0.5))
                    {
                        cityNodePositions.Add(loop.Position);
                    }
                }
            }

            int numCities = MathExt.MultiplyInt(cityNodePositions.Count, 0.5);
            cities = new List<CityPlacementData>(numCities);

            const int CompareValueCount = 8;
            PlacementValue[] placementValues = new PlacementValue[CompareValueCount];
            const int CheckRadius = NodeMap.NodePixWidth * 2;
            citiesOnNodes = new Grid2D_L<FlatArray_Three<CityPlacementData>>(nodeMap.nodeGrid.Size);

            for (int i = 0; i < numCities; i++)
            {
                IntVector2 rndNodePos = arraylib.RandomListMemberPop(cityNodePositions, rnd);
                Rectangle2 nodearea = Rectangle2.FromCenterTileAndRadius(rndNodePos, 3);

                Parallel.For(0, CompareValueCount, i =>
                {
                    PlacementValue placementValue = new PlacementValue();
                    IntVector2 tryPos = new IntVector2(NodeMap.start + NodeMap.NodePixWidth / 2) + rndNodePos * NodeMap.NodePixWidth + new IntVector2(rnd.Plus_Minus(CheckRadius), rnd.Plus_Minus(CheckRadius));

                     
                    if (icon.iconGrid.TryGet(tryPos, out var tile) &&
                        tile.groundY >= Map2Generator.Height_LowGround && tile.groundY <= Map2Generator.Height_MountainStart)
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
                    CityPlacementData city = new CityPlacementData(){ myIndex = cities.Count, pos = cityPos };
                    cities.Add(city);
                    citiesOnNodes.GetRef(rndNodePos).Add(city);
                }
               
            }
        }

        public void scaleUp16x()
        {
            for (int i = 0; i < cities.Count; i++)
            {
                var c = cities[i];
                c.pos *= 16;
                cities[i] = c;
            }
        }
    }
}
