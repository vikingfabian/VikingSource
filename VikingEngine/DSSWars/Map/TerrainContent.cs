using System;
using System.Collections.Generic;
using System.Drawing;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Map
{
    struct AnimalPenGrowth
    {
        public int maxSize;
        public int maxCount;
        public int maxTotal;

        public int harvestReady;

        public AnimalPenGrowth(int maxSize, int maxCount, int harvestCount)
        { 
            this.maxSize = maxSize;
            this.maxCount = maxCount;
            maxTotal = maxSize * maxCount;
            harvestReady = maxSize * harvestCount;
        }

        public void asyncCityProduce(ref SubTile subtile)
        {            
            if (subtile.terrainAmount < maxTotal)
            {
                subtile.terrainAmount++;
            }
        }

        public int visualCount(int terrainAmount)
        {
            return (terrainAmount + maxSize - 1) / maxSize;
        }
    }

    class TerrainContent
    {
        public const int OrchardSproutMaxSize = 6;
        public const int OrchardPlucked = OrchardSproutMaxSize + 1;
        public const int OrchardWatered = OrchardPlucked + 1;
        public const int OrchardReady = OrchardPlucked + 6;
        public const int OrchardMax = OrchardReady + 1;


        public const int SproutMaxSize = 5;
        public const int TreeMaxSize = 100;
        public const int TreeReadySize = 50;

        //public const int DryWoodSize = 20;

        public const int FarmCulture_Empty = 0;
        public const int FarmCulture_MaxSize = 5;        
        public const int FarmCulture_ReadySize = FarmCulture_MaxSize - 1;
        public const int FarmCulture_HalfSize = FarmCulture_ReadySize / 2;

        public const int DryingSaltPanMax = 5;
        public const int DryingSaltPanReady = DryingSaltPanMax -1;


        // Birds
        public static readonly AnimalPenGrowth HenGrowth = new AnimalPenGrowth(
            maxSize: 3, maxCount: 6, harvestCount: 3);

        // Livestock (Pigs)
        public static readonly AnimalPenGrowth PigGrowth = new AnimalPenGrowth(
            maxSize: 4, maxCount: 4, harvestCount: 3);

        // Livestock (Cattle)
        public static readonly AnimalPenGrowth OxenGrowth = new AnimalPenGrowth(
            maxSize: 5, maxCount: 3, harvestCount: 2);

        public static readonly AnimalPenGrowth KineOxenGrowth = new AnimalPenGrowth(
            maxSize: 6, maxCount: 3, harvestCount: 2);

        // Canines (Domestic)
        public static readonly AnimalPenGrowth DogGrowth = new AnimalPenGrowth(
            maxSize: 3, maxCount: 5, harvestCount: 3);

        public static readonly AnimalPenGrowth HoundGrowth = new AnimalPenGrowth(
            maxSize: 3, maxCount: 5, harvestCount: 3);

        // Equines
        public static readonly AnimalPenGrowth PonyGrowth = new AnimalPenGrowth(
            maxSize: 4, maxCount: 4, harvestCount: 2);

        public static readonly AnimalPenGrowth HorseGrowth = new AnimalPenGrowth(
            maxSize: 5, maxCount: 3, harvestCount: 2);

        public static readonly AnimalPenGrowth WarHorseGrowth = new AnimalPenGrowth(
            maxSize: 6, maxCount: 2, harvestCount: 1);

        public static readonly AnimalPenGrowth DraftHorseGrowth = new AnimalPenGrowth(
            maxSize: 6, maxCount: 2, harvestCount: 1);

        // Wild Hogs (Evolution Line)
        public static readonly AnimalPenGrowth WildPigGrowth = new AnimalPenGrowth(
            maxSize: 4, maxCount: 4, harvestCount: 3);

        public static readonly AnimalPenGrowth WildHogGrowth = new AnimalPenGrowth(
            maxSize: 5, maxCount: 3, harvestCount: 2);

        public static readonly AnimalPenGrowth WarHogGrowth = new AnimalPenGrowth(
            maxSize: 6, maxCount: 2, harvestCount: 1);

        public static readonly AnimalPenGrowth StagHogGrowth = new AnimalPenGrowth(
            maxSize: 7, maxCount: 2, harvestCount: 1);

        // Wolves (Evolution Line)
        public static readonly AnimalPenGrowth WolfGrowth = new AnimalPenGrowth(
            maxSize: 4, maxCount: 4, harvestCount: 2);

        public static readonly AnimalPenGrowth WargGrowth = new AnimalPenGrowth(
            maxSize: 6, maxCount: 3, harvestCount: 1);

        public static readonly AnimalPenGrowth AlphaWargGrowth = new AnimalPenGrowth(
            maxSize: 7, maxCount: 2, harvestCount: 1);

        // Felines
        public static readonly AnimalPenGrowth WildCatGrowth = new AnimalPenGrowth(
            maxSize: 3, maxCount: 5, harvestCount: 3);

        public static readonly AnimalPenGrowth LionGrowth = new AnimalPenGrowth(
            maxSize: 6, maxCount: 2, harvestCount: 1);

        public static readonly AnimalPenGrowth WarLionGrowth = new AnimalPenGrowth(
            maxSize: 7, maxCount: 2, harvestCount: 1);

        // Heavy Animals
        public static readonly AnimalPenGrowth ElephantGrowth = new AnimalPenGrowth(
            maxSize: 8, maxCount: 1, harvestCount: 1);

        public static readonly AnimalPenGrowth WarElephantGrowth = new AnimalPenGrowth(
            maxSize: 9, maxCount: 1, harvestCount: 1);

        public static readonly AnimalPenGrowth OliphantGrowth = new AnimalPenGrowth(
            maxSize: 10, maxCount: 1, harvestCount: 1);

        public static readonly AnimalPenGrowth Pheasant = new AnimalPenGrowth(
           maxSize: 1, maxCount: 2, harvestCount: int.MaxValue);

        //public const int PigMaxSize = 4;
        //public const int PigMaxCount = 4;
        //const int PigMaxTotal = PigMaxSize * PigMaxCount;
        //public const int PigReady = PigMaxSize * 3;

        //public const int HenMaxSize = 3;
        //public const int HenMaxCount = 6;
        //const int HenMaxTotal = HenMaxSize * HenMaxCount;
        //public const int HenReady = HenMaxSize * 3;

        public const int DefaultMineAmount = 10;
        public const int DryingSaltAmount = 8;
        public const int MineAmount_Coal = 20;

        public void asyncFoilGroth(IntVector2 pos, ref SubTile subtile)
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
                            //Map.SubTile ntile;
                            var npos = pos + rndDir;
                            if (DssRef.world.subTileGrid.InBounds(npos))//.TryGet(npos, out ntile))
                            {
                                ref var ntile = ref DssRef.world.subTileGrid.GetRef(npos);
                                if (ntile.mainTerrain == Map.TerrainMainType.DefaultLand)
                                {
                                    Map.TerrainSubFoilType sprout = foilType == Map.TerrainSubFoilType.TreeSoft ? Map.TerrainSubFoilType.TreeSoftSprout : Map.TerrainSubFoilType.TreeHardSprout;
                                    ntile.SetType(Map.TerrainMainType.Foil, (int)sprout, 1);

                                    //DssRef.world.subTileGrid.Set(npos, ntile);
                                }
                            }

                        }
                    }
                    break;
                case TerrainSubFoilType.TreeApple:
                case TerrainSubFoilType.TreeBanana:
                    if (subtile.terrainAmount < OrchardMax &&
                        subtile.terrainAmount != OrchardPlucked)
                    {
                        subtile.terrainAmount++;
                    }
                    break;
                case Map.TerrainSubFoilType.TreeHardSprout:
                    {
                        if (++subtile.terrainAmount > SproutMaxSize)
                        {
                            subtile.SetType(Map.TerrainMainType.Foil, (int)Map.TerrainSubFoilType.TreeHard, 1);
                        }
                    }
                    break;

                case TerrainSubFoilType.WheatFarm:
                case TerrainSubFoilType.LinenFarm:
                case TerrainSubFoilType.RapeSeedFarm:
                case TerrainSubFoilType.HempFarm:
                case TerrainSubFoilType.WheatFarmUpgraded:
                case TerrainSubFoilType.LinenFarmUpgraded:
                case TerrainSubFoilType.RapeSeedFarmUpgraded:
                case TerrainSubFoilType.HempFarmUpgraded:
                    if (subtile.terrainAmount > FarmCulture_Empty && 
                        subtile.terrainAmount < FarmCulture_MaxSize)
                    {
                        subtile.terrainAmount++;
                    }
                    break;

               
            }
        }

        public void asyncCityProduce(IntVector2 pos, ref SubTile subtile)
        {
            Map.TerrainBuildingType buildingType = (Map.TerrainBuildingType)subtile.subTerrain;
            switch (buildingType)
            {
                

                case TerrainBuildingType.PigPen:
                    PigGrowth.asyncCityProduce(ref subtile);
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
                    HenGrowth.asyncCityProduce(ref subtile);
                    //DssRef.world.subTileGrid.Set(pos, subtile);
                    break;

                case TerrainBuildingType.OxenPen:
                    OxenGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.KineOxenPen:
                    KineOxenGrowth.asyncCityProduce(ref subtile);
                    break;

                case TerrainBuildingType.DogCage:
                    DogGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.HoundCage:
                    HoundGrowth.asyncCityProduce(ref subtile);
                    break;

                case TerrainBuildingType.PonyPen:
                    PonyGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.HorsePen:
                    HorseGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.WarHorsePen:
                    WarHorseGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.DraftHorsePen:
                    DraftHorseGrowth.asyncCityProduce(ref subtile);
                    break;

                case TerrainBuildingType.WildPigPen:
                    WildPigGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.WildHogPen:
                    WildHogGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.WarHogPen:
                    WarHogGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.StagHogPen:
                    StagHogGrowth.asyncCityProduce(ref subtile);
                    break;

                case TerrainBuildingType.WolfCage:
                    WolfGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.WargCage:
                    WargGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.AlphaWargCage:
                    AlphaWargGrowth.asyncCityProduce(ref subtile);
                    break;

                case TerrainBuildingType.WildCatCage:
                    WildCatGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.LionCage:
                    LionGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.WarLionCage:
                    WarLionGrowth.asyncCityProduce(ref subtile);
                    break;

                case TerrainBuildingType.ElephantCage:
                    ElephantGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.WarElephantCage:
                    WarElephantGrowth.asyncCityProduce(ref subtile);
                    break;
                case TerrainBuildingType.OliphantCage:
                    OliphantGrowth.asyncCityProduce(ref subtile);
                    break;

                case TerrainBuildingType.DryingPan:
                    if (subtile.terrainAmount < DryingSaltPanMax)
                    {
                        subtile.terrainAmount++;
                    }
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
            VikingEngine.EngineSpace.Maths.SimplexNoise2D noiseMap,
            List<IntVector2> mineLocations)
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
                            //if (rndMine < 0.001)
                            //{
                            //    subTile.SetType(TerrainMainType.Mine, (int)TerrainMineType.GoldOre, 1);
                            //    return;
                            //}
                            //else if (rndMine < 0.002)
                            //{
                            //    subTile.SetType(TerrainMainType.Mine, (int)TerrainMineType.Coal, 1);
                            //    return;
                            //}
                            //else 
                            if (rndMine < 0.008)
                            {
                                subTile.SetType(TerrainMainType.Mine, 0, 1);//(int)TerrainMineType.IronOre, 1);
                                mineLocations.Add(new IntVector2(x, y));
                                return;
                            }
                        }
                        else
                        {
                            var rndBog = world.rnd.Double();
                            if (rndBog < 0.003)
                            {
                                subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.BogIron, 1);
                                return;
                            }
                            else if (rndBog < 0.008)
                            {
                                subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.ClayPit, 1);
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
