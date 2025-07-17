using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.DSSWars.Map.Generate
{
    class GenerateRoads
    {
        //public GenerateRoads()
        //{ 

        //}
        const float RoadHeight = -Height.DefaultGroundYoffset;

        public void fromCity(WorldData world, City city)
        {
            DssRef.world = world;
            PcgRandom rnd = new PcgRandom(world.metaData.seed * city.myIndex);




            //if (city.cityType == CityType.Capital)
            //{
            foreach (var n in city.neighborCities)
            {
                //Low index cities track to higher
                if (city.myIndex < n)
                {

                    //if (city.parentArrayIndex == 139 || n == 139) 
                    //&&
                    //if (city.parentArrayIndex == 80 && n == 139)
                    //{
                    //    lib.DoNothing();
                    //}

                    //Debug.Log($"From {city.parentArrayIndex} to {n}");
                    var nCity = world.cities[n];

                    double chance = (city.cityType == CityType.Capital || nCity.cityType == CityType.Capital) ? 0.6 : 0.2f;

                    if (rnd.Chance(chance) &&
                       (nCity.factionIndex == city.factionIndex || rnd.Chance(0.1)))
                    {

                        if (city.myIndex == 139 || n == 139)
                        {
                            lib.DoNothing();
                            Debug.Log($"From {city.myIndex} to {n}");
                        }

                        PathFinding largePath = new PathFinding();
                        var largePathResult = largePath.FindPath(-1, city.tilePos, (int)conv.ToDir8(nCity.tilePos - city.tilePos), nCity.tilePos, false);
                        largePathResult.nodes.Add(new PathNodeResult(city.tilePos, false));

                        IntVector2 subTilePos = nCity.cityHallSubtilePos;

                        foreach (var node in largePathResult.nodes)
                        {
                            IntVector2 centerSubTile = WP.ToSubTilePos_Centered(node.position);

                            while (subTilePos.SideLength(centerSubTile) > 2)
                            {
                                IntVector2 dir = centerSubTile - subTilePos;
                                subTilePos += dir.Normal_Ceiling();

                                placeOnSubTile(subTilePos);

                                placeOnSubTile(VectorExt.AddX(subTilePos, 1));
                                placeOnSubTile(VectorExt.AddY(subTilePos, 1));

                                //foreach (var tileDir in IntVector2.Dir8Array)
                                //{
                                //    placeOnSubTile(subTilePos + tileDir);

                                //}
                            }
                        }


                        //RoadPathFinding roadPath = new RoadPathFinding();
                        //var path = roadPath.FindPath(city.cityHallSubtilePos, nCity.cityHallSubtilePos);
                        //if (path != null)
                        //{
                        //    foreach (var node in path.nodes)
                        //    {
                        //        if (!node.ship)
                        //        {
                        //            ref SubTile subTile = ref world.subTileGrid.array[node.position.X, node.position.Y];

                        //            bool canBuild = false;
                        //            switch (subTile.mainTerrain)
                        //            {
                        //                case TerrainMainType.Destroyed:
                        //                case TerrainMainType.DefaultLand:
                        //                    canBuild = true;
                        //                    break;

                        //                case TerrainMainType.Foil:
                        //                    switch (subTile.GetFoilType())
                        //                    {
                        //                        case TerrainSubFoilType.TreeHardSprout:
                        //                        case TerrainSubFoilType.TreeSoftSprout:
                        //                        case TerrainSubFoilType.TreeHard:
                        //                        case TerrainSubFoilType.TreeSoft:
                        //                        case TerrainSubFoilType.DryWood:
                        //                            canBuild = true;
                        //                            break;
                        //                    }
                        //                    break;
                        //            }

                        //            if (canBuild)
                        //            {
                        //                subTile.SetType(TerrainMainType.Road, 0, 1);
                        //                subTile.groundY += RoadHeight;
                        //            }
                        //        }
                        //    }
                        //}

                        void placeOnSubTile(IntVector2 pos)
                        {
                            ref SubTile subTile = ref world.subTileGrid.array[pos.X, pos.Y];

                            bool canBuild = false;
                            switch (subTile.mainTerrain)
                            {
                                case TerrainMainType.Destroyed:
                                case TerrainMainType.DefaultLand:
                                    canBuild = true;
                                    break;

                                case TerrainMainType.Foil:
                                    switch (subTile.GetFoilType())
                                    {
                                        case TerrainSubFoilType.TreeHardSprout:
                                        case TerrainSubFoilType.TreeSoftSprout:
                                        case TerrainSubFoilType.TreeHard:
                                        case TerrainSubFoilType.TreeSoft:
                                        case TerrainSubFoilType.DryWood:
                                            canBuild = true;
                                            break;
                                    }
                                    break;
                            }

                            if (canBuild)
                            {
                                subTile.SetType(TerrainMainType.Road, 0, 1);
                                subTile.groundY += RoadHeight;
                            }
                        }
                    }
                }
            }
            //}
        }
    }
}
