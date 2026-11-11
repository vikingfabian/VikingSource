using System;
using System.Collections.Generic;
using System.Drawing;
using VikingEngine.DSSWars.Map.Map2;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.MapModels;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Map.MapProcess
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

        public void asyncCityProduce(ref MapTile1_1 subtile)
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
        public const byte OrchardSproutMaxSize = 6;
        public const byte OrchardPlucked = OrchardSproutMaxSize + 1;
        public const byte OrchardWatered = OrchardPlucked + 1;
        public const byte OrchardReady = OrchardPlucked + 6;
        public const byte OrchardMax = OrchardReady + 1;


        public const byte SproutMaxSize = 5;
        public const byte TreeMaxSize = 100;
        public const byte TreeReadySize = 50;

        //public const int DryWoodSize = 20;

        public const byte FarmCulture_Empty = 0;
        public const byte FarmCulture_MaxSize = 5;        
        public const byte FarmCulture_ReadySize = FarmCulture_MaxSize - 1;
        public const byte FarmCulture_HalfSize = FarmCulture_ReadySize / 2;

        public const byte DryingSaltPanMax = 5;
        public const byte DryingSaltPanReady = DryingSaltPanMax -1;


        // Birds
        public static readonly AnimalPenGrowth FowlGrowth = new AnimalPenGrowth(
            maxSize: 3, maxCount: 4, harvestCount: 2);
        // Birds
        public static readonly AnimalPenGrowth HenGrowth = new AnimalPenGrowth(
            maxSize: 3, maxCount: 6, harvestCount: 3);


        public static readonly AnimalPenGrowth BoarGrowth = new AnimalPenGrowth(
            maxSize: 3, maxCount: 5, harvestCount: 3);

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


        public const int DefaultMineAmount = 10;
        public const int DryingSaltAmount = 8;
        public const int MineAmount_Coal = 20;

        public void asyncTerrainGrowthAndProduce(IntVector2 pos, ref MapTile1_1 subtile)
        {
            switch (subtile.mainTerrain)
            {
                case TerrainMainType.Farm:
                    switch ((TerrainSubFarmType)subtile.subTerrain)
                    {
                        case TerrainSubFarmType.TreeApple:
                        case TerrainSubFarmType.TreeBanana:
                            if (subtile.terrainAmount < OrchardMax &&
                                subtile.terrainAmount != OrchardPlucked)
                            {
                                subtile.terrainAmount++;
                            }
                            break;
                        default:
                            if (subtile.terrainAmount > FarmCulture_Empty &&
                                subtile.terrainAmount < FarmCulture_MaxSize)
                            {
                                subtile.terrainAmount++;
                            }
                            break;
                    }
                    break;
                case TerrainMainType.Foil:
                    switch ((TerrainSubFoilType)subtile.subTerrain)
                    {
                        case TerrainSubFoilType.TreeSoft:
                        case TerrainSubFoilType.TreeHard:
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

                                    var npos = pos + rndDir;
                                    if (DssRef.world.subTileGrid.InBounds(npos))
                                    {
                                        ref var ntile = ref DssRef.world.subTileGrid.GetRef(npos);
                                        if (ntile.mainTerrain == TerrainMainType.DefaultLand)
                                        {
                                            TerrainSubFoilType sprout = (TerrainSubFoilType)subtile.subTerrain == TerrainSubFoilType.TreeSoft ? TerrainSubFoilType.TreeSoftSprout : TerrainSubFoilType.TreeHardSprout;
                                            ntile.SetType(TerrainMainType.Foil, (int)sprout, 1);
                                        }
                                    }

                                }
                            }
                            break;
                        
                        case TerrainSubFoilType.TreeHardSprout:
                            {
                                if (++subtile.terrainAmount > SproutMaxSize)
                                {
                                    subtile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.TreeHard, 1);
                                }
                            }
                            break;
                    }
                    break;
                case TerrainMainType.AnimalPen:

                    switch ((TerrainSubAnimalPenType)subtile.subTerrain)
                    {
                        case TerrainSubAnimalPenType.BoarHabitat:
                        case TerrainSubAnimalPenType.BoarPen:
                            BoarGrowth.asyncCityProduce(ref subtile);
                            break;

                        case TerrainSubAnimalPenType.FowlHabitat:
                            FowlGrowth.asyncCityProduce(ref subtile);
                            break;

                        case TerrainSubAnimalPenType.FowlPen:
                            const int FowlEggGroupCount = 4;
                            spawnEggs(FowlEggGroupCount, ref subtile);
                            FowlGrowth.asyncCityProduce(ref subtile);
                            break;

                        case TerrainSubAnimalPenType.PigPen:
                            PigGrowth.asyncCityProduce(ref subtile);
                            break;

                        case TerrainSubAnimalPenType.HenPen:
                            const int EggGroupCount = 5;

                            spawnEggs(EggGroupCount, ref subtile);
                           
                            HenGrowth.asyncCityProduce(ref subtile);
                            break;

                        case TerrainSubAnimalPenType.OxHabitat:
                        case TerrainSubAnimalPenType.OxenPen:
                            OxenGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.KineOxenPen:
                            KineOxenGrowth.asyncCityProduce(ref subtile);
                            break;

                        case TerrainSubAnimalPenType.DogHabitat:
                        case TerrainSubAnimalPenType.DogCage:
                            DogGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.HoundCage:
                            HoundGrowth.asyncCityProduce(ref subtile);
                            break;

                        case TerrainSubAnimalPenType.PonyHabitat:
                        case TerrainSubAnimalPenType.PonyPen:
                            PonyGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.HorsePen:
                            HorseGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.WarHorsePen:
                            WarHorseGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.DraftHorsePen:
                            DraftHorseGrowth.asyncCityProduce(ref subtile);
                            break;

                        case TerrainSubAnimalPenType.WildPigPen:
                            WildPigGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.WildHogPen:
                            WildHogGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.WarHogPen:
                            WarHogGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.StagHogPen:
                            StagHogGrowth.asyncCityProduce(ref subtile);
                            break;

                        case TerrainSubAnimalPenType.WolfHabitat:
                        case TerrainSubAnimalPenType.WolfCage:
                            WolfGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.WargCage:
                            WargGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.AlphaWargCage:
                            AlphaWargGrowth.asyncCityProduce(ref subtile);
                            break;

                        case TerrainSubAnimalPenType.CatHabitat:
                        case TerrainSubAnimalPenType.WildCatCage:
                            WildCatGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.LionCage:
                            LionGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.WarLionCage:
                            WarLionGrowth.asyncCityProduce(ref subtile);
                            break;

                        case TerrainSubAnimalPenType.ElephantHabitat:
                        case TerrainSubAnimalPenType.ElephantCage:
                            ElephantGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.WarElephantCage:
                            WarElephantGrowth.asyncCityProduce(ref subtile);
                            break;
                        case TerrainSubAnimalPenType.OliphantCage:
                            OliphantGrowth.asyncCityProduce(ref subtile);
                            break;

                       
                    }

                    void spawnEggs(int eggGroupCount, ref MapTile1_1 subtile)
                    {
                        //const int EggGroupCount = 5;

                        if (subtile.terrainAmount > 0)
                        {
                            if (Ref.rnd.Chance(1.0 / eggGroupCount))
                            {
                                DssRef.state.resources.addItem(
                                    new ItemResource(
                                        ItemResourceType.Egg,
                                        //1,
                                        //subtile.terrainAmount * 4,
                                        (ushort)(subtile.terrainAmount * eggGroupCount)),
                                    ref subtile.collectionPointer);
                            }
                        }
                    }
                    break;
                case TerrainMainType.Building:
                    switch ((TerrainBuildingType)subtile.subTerrain)
                    {
                        case TerrainBuildingType.DryingPan:
                            if (subtile.terrainAmount < DryingSaltPanMax)
                            {
                                subtile.terrainAmount++;
                            }
                            break;
                    }
                    break;
            }
        }

        //public void asyncCityProduce(IntVector2 pos, ref MapTile1_1 subtile)
        //{
            
        //}

        const int TerrainCutOff = MapHeight2.MountainStarHeight;
        const int BeachCutOff = MapHeight2.WaterPlaneHeight + 6;

        public static void createSubTileContent(int x, int y, 
           
            ref CombinedTile cTile, 
            WorldData world, 
            EngineSpace.Maths.SimplexNoise2D noiseMap,
            List<IntVector2> mineLocations,
            List<IntVector2> animalSpawns)
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

                    if (groundType.mineSpawn && 
                        cTile.sumTile.biomColorHeight >= BiomHeightColor.MineHeightStart &&
                        world.rnd.Chance(0.008))
                    {
                        mineLocations.Add(new IntVector2(x, y));
                    }

                    if (groundType.animalSpawn && world.rnd.Chance(0.012))
                    {
                        animalSpawns.Add(new IntVector2(x, y));
                    }
                }
                else
                { //Mountain 
                    cTile.mapTile.groundType = world.rnd.Chance(0.1)? GroundType.Rubble : GroundType.Rock;
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

/*
                if (world.rnd.Chance(0.6))
                {                    
                    
                    if (stonenoise > 0.1)
                    {
                        
                        if (cTile.sumTile.biomColorHeight >= BiomHeightColor.MineHeightStart)
                        {
                            var rndMine = world.rnd.Double();
                            
                            if (rndMine < 0.008)
                            {
                                cTile.mapTile.SetType(TerrainMainType.Mine, 0, 1);
                                mineLocations.Add(new IntVector2(x, y));
                                return;
                            }
                        }
                        else
                        {
                            var rndBog = world.rnd.Double();
                            if (rndBog < 0.003)
                            {
                                cTile.mapTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.BogIron, 1);
                                return;
                            }
                            else if (rndBog < 0.008)
                            {
                                cTile.mapTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.ClayPit, 1);
                                return;
                            }
                        }

                        if (stonenoise > 0.6f)
                        {
                            cTile.mapTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.StoneBlock, 1);
                            return;
                        }
                    }
                    if (stonenoise < -0.5f)
                    {
                        cTile.mapTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.Stones, 1);
                        return;
                    }

                    float herbnoise = noiseMap.OctaveNoise2D(4, 0.8f, 5, x, -y);
                    if (herbnoise > 0.6f)
                    {
                        cTile.mapTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.Herbs, 1);
                        return;
                    }
                    if (herbnoise < -0.5f)
                    {
                        animalSpawn();
                        cTile.mapTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.Bush, 1);
                        return;
                    }

                    float grassnoise = noiseMap.OctaveNoise2D(4, 0.8f, 5, -x, -y);
                    if (grassnoise > 0.5f)
                    {
                        animalSpawn();
                        cTile.mapTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.TallGrass, 1);
                        return;
                    }
                }

                
                var percTree = biomHeight.percTree * biom.percTree;
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

                        cTile.mapTile.SetType(TerrainMainType.Foil, (int)treeType, size);
                    }

                }

                void animalSpawn()
                {
                    //if (distanceToCity > 3)
                    //{
                        var rndWildAnimal = world.rnd.Double();
                        if (rndWildAnimal < 0.012)
                        {
                            animalSpawns.Add(new IntVector2(x, y));
                        }
                    //}
                }
*/
            
        }


    }

    
}
