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
        List<CityPlacementData> cities;

        PcgRandom rnd = new PcgRandom();

        void generateCities(Map2GenerateSettings generateSettings, NodeMap nodeMap, IconWorldData icon)
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
                IntVector2 check = arraylib.RandomListMemberPop(cityNodePositions, rnd);
                Rectangle2 nodearea = Rectangle2.FromCenterTileAndRadius(check, 2);

                Parallel.For(0, CompareValueCount, i =>
                {
                    PlacementValue placementValue = new PlacementValue();
                    IntVector2 tryPos = new IntVector2(NodeMap.start + NodeMap.NodePixWidth / 2) + check * NodeMap.NodePixWidth + new IntVector2(rnd.Plus_Minus(CheckRadius), rnd.Plus_Minus(CheckRadius));

                    var tile = icon.iconGrid.Get(tryPos);
                    if (tile.groundY >= Map2Generator.Height_LowGround && tile.groundY <= Map2Generator.Height_MountainStart)
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
                            placementValue.success = true;
                        }
                    }

                    placementValues[i] = placementValue;
                });
                //switch (DssRef.storage.ruleset.factionStartSize)
                //{
                //    case FactionStartSize.Full:
                //        generateSettings.percentageUnclaimed = 0.25f;
                //        generateCityType(CityType.Capital, numHeadCities, HeadCityNeededFreeRadius, generateSettings);
                //        generateCityType(CityType.Town, numHeadCities * 2, 9, generateSettings);
                //        generateCityType(CityType.Village, numHeadCities * 4, 8, generateSettings);
                //        break;
                //    case FactionStartSize.OneCity:
                //        generateSettings.percentageUnclaimed = 0.85f;
                //        generateCityType(CityType.Village, numHeadCities * 8, 8, generateSettings);
                //        break;
                //    case FactionStartSize.Settler:
                //        generateSettings.percentageUnclaimed = 0.85f;
                //        generateCityType(CityType.Campsite, numHeadCities * 8, 8, generateSettings);
                //        break;
                //}

                //float storyPlacementScale = 1f;
                //if (world.Size.Area() > WorldData.SizeDimentions(MapSize.Medium).Area())
                //{
                //    storyPlacementScale = (float)WorldData.SizeDimentions(MapSize.Medium).Area() / world.Size.Area();
                //}
                //world.Init_CityComponents(world.cities.Count);
                //foreach (City city in world.cities)
                //{
                //    city.generateCultureAndEconomy(world, storyPlacementScale, cityCultureCollection);
                //}
            }
        }
    }
}
