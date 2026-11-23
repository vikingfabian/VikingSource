using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Core.DSSWars.Map.MapData;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.EngineSpace.Maths;

namespace VikingEngine.Core.DSSWars.Map.Generate
{
    class GenerateMapTileContent
    {
        WorldData world;
        List<IntVector2> mineLocations = new List<IntVector2>(1024);
        List<IntVector2> animalSpawns = new List<IntVector2>(1024);
        SimplexNoise2D noiseMap;
        int mineLocationsCount;
        public GenerateMapTileContent(WorldData world)
        {
            this.world = world;
            noiseMap = new SimplexNoise2D(world.metaData.worldId.seed);
        }

        public void placeTerrain()
        {
            mineLocationsCount = 0;
            Parallel.For(0, world.subTileGrid.Size.X, x =>
            {
                for (int y = 0; y < world.subTileGrid.Size.Y; y++)
                {
                    var ctile = world.GetCombinedTile(new IntVector2(x, y));//new MapTile1_1(tiletype, subType, rndColor, topY);
                    createSubTileContent(x, y, ref ctile, world, noiseMap);
                    world.subTileGrid.Set(x, y, ctile.mapTile);
                }
            });

            Task.Factory.StartNew(() =>
            {
                try
                {
                    world.mineLocations = new MapData.MineLocations(world, mineLocationsCount);
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
            });

           
        }

        const int TerrainCutOff = MapHeight2.MountainStarHeight;
        const int BeachCutOff = MapHeight2.WaterPlaneHeight + 6;

        void createSubTileContent(int x, int y,

            ref CombinedTile cTile,
            WorldData world,
            SimplexNoise2D noiseMap)
        {
            if (cTile.mapTile.IsLand())
            {
                if (cTile.mapTile.heightValue <= BeachCutOff)
                {
                    if (waterNoiseBuild())
                    {
                        cTile.mapTile.groundType = GroundType.Default;
                    }
                    else
                    {
                        cTile.sumTile.GetBiom(out BiomHeightColor biomHeight, out Biom biom);
                        cTile.mapTile.groundType = biom.beachGround.groundType;
                    }
                }
                else if (cTile.mapTile.heightValue < TerrainCutOff)
                {
                    cTile.sumTile.GetBiom(out BiomHeightColor biomHeight, out Biom biom);

                    var groundType = biom.GetTileGroundType(x, y, noiseMap);
                    groundType.set(world.rnd, ref cTile);

                    if (groundType.mineSpawn )
                    {
                        if (cTile.sumTile.biomColorHeight >= BiomHeightColor.MineHeightStart)
                        {
                            if (world.rnd.Chance(0.008))
                            {
                                mineLocationsCount++;
                                cTile.mapTile.SetType(TerrainMainType.Mine, (int)TerrainMineType.PointOfInterest, MineLocations.DeepMine);
                            }
                        }
                        else
                        {
                            if (world.rnd.Chance(0.003))
                            {
                                mineLocationsCount++;
                                cTile.mapTile.SetType(TerrainMainType.Mine, (int)TerrainMineType.PointOfInterest, MineLocations.ShallowMine);
                            }
                        }
                    }

                    if (groundType.animalSpawn && world.rnd.Chance(0.012))
                    {
                        //animalSpawns.Add(new IntVector2(x, y));
                    }
                }
                else
                { //Mountain 
                    cTile.mapTile.groundType = world.rnd.Chance(0.1) ? GroundType.Rubble : GroundType.Rock;
                }
            }
            else
            { //Water
                if (waterNoiseBuild())
                {
                    cTile.mapTile.groundType = GroundType.Default;
                }
                else
                {
                    cTile.mapTile.groundType = GroundType.Mud;
                }
            }

            bool waterNoiseBuild()
            {
                float waterBottomNoise = noiseMap.OctaveNoise2D(2, 0.8f, 5, x, y);
                return waterBottomNoise > 0;
            }


        }
    }
}
