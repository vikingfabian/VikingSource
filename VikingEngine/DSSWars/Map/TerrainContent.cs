using System;
using System.Drawing;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.DSSWars.Map
{
    class TerrainContent
    {
        public const int SproutMaxSize = 5;
        public const int TreeMaxSize = 100;
        public const int TreeReadySize = 50;

        //public const int DryWoodSize = 20;

        public const int FarmCulture_Empty = 0;
        public const int FarmCulture_MaxSize = 5;        
        public const int FarmCulture_ReadySize = FarmCulture_MaxSize - 1;
        public const int FarmCulture_HalfSize = FarmCulture_ReadySize / 2;

        public const int PigMaxSize = 3;
        public const int PigMaxCount = 4;
        const int PigMaxTotal = PigMaxSize * PigMaxCount;
        public const int PigReady = PigMaxSize * 3;

        public const int HenMaxSize = 2;
        public const int HenMaxCount = 6;
        const int HenMaxTotal = HenMaxSize * HenMaxCount;
        public const int HenReady = HenMaxSize * 3;

        public const int MineAmount = 10;

        public void asyncFoilGroth(IntVector2 pos, SubTile subtile)
        {
            Map.TerrainSubFoilType foilType = (Map.TerrainSubFoilType)subtile.subTerrain;
            switch (foilType)
            {
                case Map.TerrainSubFoilType.TreeSoft:
                case Map.TerrainSubFoilType.TreeHard:
                    {
                        if (subtile.terrainAmount < TreeMaxSize)
                        {
                            subtile.terrainAmount++;
                            DssRef.world.subTileGrid.Set(pos, subtile);
                        }

                        if (Ref.rnd.Chance(0.2) && subtile.terrainAmount > 20 && subtile.terrainAmount < 90)
                        {
                            IntVector2 rndDir = arraylib.RandomListMember(IntVector2.Dir8Array);
                            if (Ref.rnd.Chance(0.2))
                            {
                                rndDir *= 2;
                            }
                            Map.SubTile ntile;
                            var npos = pos + rndDir;
                            if (DssRef.world.subTileGrid.TryGet(npos, out ntile))
                            {
                                if (ntile.mainTerrain == Map.TerrainMainType.DefaultLand)
                                {
                                    Map.TerrainSubFoilType sprout = foilType == Map.TerrainSubFoilType.TreeSoft ? Map.TerrainSubFoilType.TreeSoftSprout : Map.TerrainSubFoilType.TreeHardSprout;
                                    ntile.SetType(Map.TerrainMainType.Foil, (int)sprout, 1);

                                    DssRef.world.subTileGrid.Set(npos, ntile);
                                }
                            }

                        }
                    }
                    break;

                case Map.TerrainSubFoilType.TreeHardSprout:
                    {
                        if (++subtile.terrainAmount > SproutMaxSize)
                        {
                            subtile.SetType(Map.TerrainMainType.Foil, (int)Map.TerrainSubFoilType.TreeHard, 1);
                        }

                        DssRef.world.subTileGrid.Set(pos, subtile);
                    }
                    break;

                case TerrainSubFoilType.WheatFarm:
                case TerrainSubFoilType.LinenFarm:
                    if (subtile.terrainAmount > FarmCulture_Empty && 
                        subtile.terrainAmount < FarmCulture_MaxSize)
                    {
                        subtile.terrainAmount++;
                        DssRef.world.subTileGrid.Set(pos, subtile);
                    }
                    break;
            }
        }

        public void asyncCityProduce(IntVector2 pos, SubTile subtile)
        {
            Map.TerrainBuildingType buildingType = (Map.TerrainBuildingType)subtile.subTerrain;
            switch (buildingType)
            {
                case TerrainBuildingType.PigPen:
                    if (subtile.terrainAmount < PigMaxTotal)
                    {
                        subtile.terrainAmount++;
                        DssRef.world.subTileGrid.Set(pos, subtile);
                    }
                    break;
                case TerrainBuildingType.HenPen:
                    const int EggGroupCount = 5;

                    if (subtile.terrainAmount > 0)
                    {
                        if (Ref.rnd.Chance(1.0 / EggGroupCount))
                        {
                            DssRef.state.resources.addItem(
                                new ItemResource(
                                    ItemResourceType.Egg,
                                    1,
                                    subtile.terrainAmount * 4,
                                    subtile.terrainAmount * EggGroupCount),
                                ref subtile.collectionPointer);
                        }
                    }
                    if (subtile.terrainAmount < HenMaxTotal)
                    {
                        subtile.terrainAmount++;
                    }
                    DssRef.world.subTileGrid.Set(pos, subtile);
                    break;
            }
        }

        public static void createSubTileContent(int x, int y, 
            float distanceToCity,
            Tile tile,
            Height height,
            Biom biom,
            ref IntervalF mudRadius,
            ref SubTile subTile, 
            WorldData world, 
            VikingEngine.EngineSpace.Maths.SimplexNoise2D noiseMap)
        {
            if (tile.IsLand() && !height.isMountainPeek)
            {
                if (distanceToCity <= mudRadius.Max)
                {
                    if (distanceToCity <= mudRadius.Min || world.rnd.Chance(0.5))
                    {
                        subTile.SetType(TerrainMainType.Destroyed, 0, 1);
                        return;
                    }
                }

                if (world.rnd.Chance(0.6))
                {
                    float stonenoise = noiseMap.OctaveNoise2D(4, 0.8f, 5, -x, y);

                    if (stonenoise > 0.1)
                    {
                        if (tile.heightLevel >= Height.MineHeightStart)
                        {
                            var rndMine = world.rnd.Double();
                            if (rndMine < 0.001)
                            {
                                subTile.SetType(TerrainMainType.Mine, (int)TerrainMineType.GoldOre, 1);
                                return;
                            }
                            else if (rndMine < 0.002)
                            {
                                subTile.SetType(TerrainMainType.Mine, (int)TerrainMineType.Coal, 1);
                                return;
                            }
                            else if (rndMine < 0.005)
                            {
                                subTile.SetType(TerrainMainType.Mine, (int)TerrainMineType.IronOre, 1);
                                return;
                            }
                        }

                        if (stonenoise > 0.6f)
                        {
                            subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.StoneBlock, 1);
                            return;
                        }
                    }
                    if (stonenoise < -0.5f)
                    {
                        subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.Stones, 1);
                        return;
                    }

                    float herbnoise = noiseMap.OctaveNoise2D(4, 0.8f, 5, x, -y);
                    if (herbnoise > 0.6f)
                    {
                        subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.Herbs, 1);
                        return;
                    }
                    if (herbnoise < -0.5f)
                    {
                        subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.Bush, 1);
                        return;
                    }

                    float grassnoise = noiseMap.OctaveNoise2D(4, 0.8f, 5, -x, -y);
                    if (grassnoise > 0.5f)
                    {
                        subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.TallGrass, 1);
                        return;
                    }
                }


                var percTree = height.percTree * biom.percTree;
                if (percTree > 0)
                {
                    float treenoise = noiseMap.OctaveNoise2D_Normal(4, 0.75f, 1, x, y);

                    if (treenoise < percTree && treenoise < world.rnd.Double(percTree * 2f))
                    {
                        int size = (int)((1.0 - Math.Min(treenoise, world.rnd.Double())) * TreeMaxSize);
                                                
                        TerrainSubFoilType treeType;
                        if (world.rnd.Chance(biom.percDryWood))
                        {
                            treeType = TerrainSubFoilType.DryWood;
                            size /= 3;
                        }
                        else if (world.rnd.Chance(biom.percSoftTree))
                        {
                            treeType = TerrainSubFoilType.TreeSoft;
                        }
                        else
                        {
                            treeType = TerrainSubFoilType.TreeHard;
                        }

                        subTile.SetType(TerrainMainType.Foil, (int)treeType, size);
                    }

                }
            }
        }


    }

    
}
