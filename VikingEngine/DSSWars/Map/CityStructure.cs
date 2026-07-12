using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Work;
using VikingEngine.LootFest.Map;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.DSSWars.Map
{
    class CityStructure
    {
        public static readonly CityStructure WorkInstance = new CityStructure();
        public static readonly CityStructure AutomationInstance = new CityStructure();
        ForXYEdgeLoopRandomPicker edgeRandomizer = new ForXYEdgeLoopRandomPicker();

        public List<IntVector2> FoodSpots_workupdate = new List<IntVector2>(4);
        public List<IntVector2> StoragePoints_workupdate = new List<IntVector2>(4);
        public List<IntVector2> Trees = new List<IntVector2>(20);
        public List<IntVector2> Stones = new List<IntVector2>(20);
        public List<SubTileWork> Farms = new List<SubTileWork>(20);
        //public List<IntVector2> FarmGather = new List<IntVector2>(20);
        public List<IntVector2> AnimalPens = new List<IntVector2>(20);
        public List<IntVector2> BogIron = new List<IntVector2>(20);
        public List<IntVector2> ClayPit = new List<IntVector2>(20);
        public List<IntVector2> Mines = new List<IntVector2>(20);
        public List<IntVector2> CraftStation = new List<IntVector2>(20);
        public List<IntVector2> CoinMinting = new List<IntVector2>(2);
        //public List<IntVector2> EmptyLand = new List<IntVector2>(2);
        public List<IntVector2> ResourceOnGround = new List<IntVector2>(20);

        public List<IntVector2> WoodCutter = new List<IntVector2>(20);
        public List<IntVector2> StoneCutter = new List<IntVector2>(20);

        public List<IntVector2> WildAnimals = new List<IntVector2>(8);
        public List<IntVector2> TrapperHuts = new List<IntVector2>(8);


        //int nobelHouseCount = 0;
        public int fuelSpots = 0;
        public int foodspots = 0;
        //public int logisticsLevel = 0;
        public bool newCity = true;

        public BuildingPosition buildingPosition;
        

        public void setupTutorialMap(City city)
        {
            IntVector2 topleft;
            ForXYLoop subTileLoop;

            int wood = 4;
            int stone = 2;

            int cityradius = city.cityTileArea.size.SideLength() / 2;
            for (int radius = 2; radius <= cityradius; ++radius)
            {
                ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.tilePos, radius));

                while (cirkleLoop.Next())
                {
                    if (DssRef.world.tileBounds.IntersectTilePoint(cirkleLoop.Position))
                    {
                        var tile = DssRef.world.tileGrid.Get(cirkleLoop.Position);
                        if (tile.CityIndex == city.myIndex && tile.IsLand())
                        {
                            topleft = WP.ToSubTilePos_TopLeft(cirkleLoop.Position);
                            subTileLoop = new ForXYLoop(topleft, topleft + WorldData.TileSubDivitions_MaxIndex);

                            while (subTileLoop.Next())
                            {
                                ref var subTile = ref DssRef.world.subTileGrid.GetRef(subTileLoop.Position);

                                switch (subTile.mainTerrain)
                                {
                                    
                                    case TerrainMainType.Destroyed:
                                    case TerrainMainType.DefaultLand:
                                        if (wood > 0)
                                        {
                                            --wood;
                                            subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.DryWood, TerrainContent.TreeReadySize);
                                            //DssRef.world.subTileGrid.Set(subTileLoop.Position, subTile);
                                        }
                                        else 
                                        {
                                            
                                            subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.Stones, 1);
                                            //DssRef.world.subTileGrid.Set(subTileLoop.Position, subTile);
                                            if (--stone <= 0)
                                            {
                                                return;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }

        }

        public void updateIfNew(City city, int workerCount)
        {
            if (newCity)
            {
                newCity = false;

                update(DssRef.world, city, workerCount);
            }
        }

        public static bool Find(City city, TerrainMainType main, int sub, out IntVector2 position)
        {
            IntVector2 topleft;
            ForXYLoop subTileLoop;
            int maxRadius = city.cityTileArea.size.SideLength();
            for (int radius = 0; radius < maxRadius; ++radius)
            {
                ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.tilePos, radius));

                while (cirkleLoop.Next())
                {
                    if (DssRef.world.tileBounds.IntersectTilePoint(cirkleLoop.Position))
                    {
                        var tile = DssRef.world.tileGrid.Get(cirkleLoop.Position);
                        if (tile.CityIndex == city.myIndex && tile.IsLand())
                        {
                            topleft = WP.ToSubTilePos_TopLeft(cirkleLoop.Position);
                            subTileLoop = new ForXYLoop(topleft, topleft + WorldData.TileSubDivitions_MaxIndex);

                            while (subTileLoop.Next())
                            {
                                SubTile subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);

                                if (subTile.mainTerrain == main && subTile.subTerrain == sub)
                                { 
                                    position = subTileLoop.Position;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            position = IntVector2.Zero;
            return false;
        }

        public static bool FindEmpty(City city, out IntVector2 position)
        {
            IntVector2 topleft;
            ForXYLoop subTileLoop;
            int maxRadius = city.cityTileArea.size.SideLength();
            for (int radius = 0; radius < maxRadius; ++radius)
            {
                ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.tilePos, radius));

                while (cirkleLoop.Next())
                {
                    if (DssRef.world.tileBounds.IntersectTilePoint(cirkleLoop.Position))
                    {
                        var tile = DssRef.world.tileGrid.Get(cirkleLoop.Position);
                        if (tile.CityIndex == city.myIndex && tile.MayBuild())
                        {
                            topleft = WP.ToSubTilePos_TopLeft(cirkleLoop.Position);
                            subTileLoop = new ForXYLoop(topleft, topleft + WorldData.TileSubDivitions_MaxIndex);

                            while (subTileLoop.Next())
                            {
                                SubTile subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);

                                if (subTile.mainTerrain == TerrainMainType.DefaultLand ||
                                    subTile.mainTerrain == TerrainMainType.Destroyed)
                                {
                                    position = subTileLoop.Position;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            position = IntVector2.Zero;
            return false;
        }

        public bool NextEmptyLand(City city, int addSpaces, out IntVector2 freeSubTilePos)
        {
            freeSubTilePos = IntVector2.Zero;

            int maxRadius = city.cityTileArea.size.SideLength();
            for (int radius = 0; radius < maxRadius; ++radius)
            {
                //ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.tilePos, radius));
                edgeRandomizer.start(Rectangle2.FromCenterTileAndRadius(city.tilePos, radius));
                while (edgeRandomizer.Next())
                {
                    if (DssRef.world.tileBounds.IntersectTilePoint(edgeRandomizer.Position))
                    {
                        if (DssRef.world.tileGrid.TryGet(edgeRandomizer.Position, out Tile tile))
                        {
                            if (tile.CityIndex == city.myIndex && tile.IsLand())
                            {
                                IntVector2 topleft = WP.ToSubTilePos_TopLeft(edgeRandomizer.Position);
                                ForXYLoop subTileLoop = new ForXYLoop(topleft, topleft + WorldData.TileSubDivitions_MaxIndex);

                                while (subTileLoop.Next())
                                {
                                    SubTile subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);
                                    switch (subTile.mainTerrain)
                                    {
                                        case TerrainMainType.Destroyed:
                                        case TerrainMainType.DefaultLand:
                                            freeSubTilePos = subTileLoop.Position;
                                            if (--addSpaces <= 0)
                                            {   
                                                return true;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            
            return freeSubTilePos.X > 0;
        }

        public void update(WorldData world, City city, int workerCount, int emptyLandExpansions = 2)
        {
            //#if DEBUG
            //            Debug.CrashIfMainThread();
            //            //int emptyLandExpansions = 2;
            //#endif
            IntVector2 topleft;
            ForXYLoop subTileLoop;
            FoodSpots_workupdate.Clear();
            StoragePoints_workupdate.Clear();
            Trees.Clear();
            Stones.Clear();
            Farms.Clear();
            //FarmGather.Clear();
            AnimalPens.Clear();
            BogIron.Clear();
            ClayPit.Clear();
            Mines.Clear();
            CraftStation.Clear();
            CoinMinting.Clear();
            //EmptyLand.Clear();
            ResourceOnGround.Clear();
            WoodCutter.Clear();
            StoneCutter.Clear();
            //nobelHouseCount = 0;
            //int coalPitCount = 0;
            fuelSpots = 0;
            foodspots = 0;

            WildAnimals.Clear();
            TrapperHuts.Clear();


            int serviceMenHousing = 0;
            int housingCount_Workers = 0;

            Rectangle2 emptyArea = Rectangle2.Zero;
            buildingPosition = new BuildingPosition();
            BuildingStructure buildingStructure = new BuildingStructure();
            TerrainStructure terrainStructure = new TerrainStructure();

            //IntVector2 cityHall = WP.ToSubTilePos_Centered(city.tilePos);
            FoodSpots_workupdate.Add(city.citySquareSubtilePos);
            StoragePoints_workupdate.Add(city.citySquareSubtilePos);

            //Cirkle outward from city to find resources
            //for (int radius = 0; radius <= city.cityTileRadius; ++radius)
            //{
            //    ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.tilePos, radius));

            //    while (cirkleLoop.Next())
            //    {
            //        if (DssRef.world.tileBounds.IntersectTilePoint(cirkleLoop.Position))
            //        {
            ForXYLoop loop = new ForXYLoop(city.cityTileArea);
            while (loop.Next())
            {

                if (world.tileGrid.TryGet(loop.Position, out var tile) &&
                    tile.CityIndex == city.myIndex &&
                    tile.IsLand())
                {
                    topleft = WP.ToSubTilePos_TopLeft(loop.Position);
                    subTileLoop = new ForXYLoop(topleft, topleft + WorldData.TileSubDivitions_MaxIndex);

                    while (subTileLoop.Next())
                    {
                        SubTile subTile = world.subTileGrid.Get(subTileLoop.Position);

                        if (subTile.collectionPointer >= 0)
                        {
                            //if (ResourceOnGround.Count < workerCount)
                            //{ 
                            ResourceOnGround.Add(subTileLoop.Position);
                            //}
                        }

                        switch (subTile.mainTerrain)
                        {

                            case TerrainMainType.Foil:
                                var foil = (TerrainSubFoilType)subTile.subTerrain;

                                switch (foil)
                                {
                                    case Map.TerrainSubFoilType.TreeSoft:
                                    case Map.TerrainSubFoilType.TreeHard:
                                    case Map.TerrainSubFoilType.DryWood:
                                        if (foil == TerrainSubFoilType.DryWood || subTile.terrainAmount >= TerrainContent.TreeReadySize)
                                        {
                                            ++terrainStructure.resourceCount_wood;
                                            ++fuelSpots;
                                            Trees.Add(subTileLoop.Position);
                                        }
                                        break;

                                    case Map.TerrainSubFoilType.StoneBlock:
                                    case Map.TerrainSubFoilType.Stones:
                                        ++terrainStructure.resourceCount_stone;
                                        Stones.Add(subTileLoop.Position);
                                        break;

                                    case TerrainSubFoilType.TreeApple:
                                    case TerrainSubFoilType.TreeBanana:
                                        ++buildingStructure.Orchard_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount == TerrainContent.OrchardPlucked)
                                        {
                                            Farms.Add(new SubTileWork(subTileLoop.Position, WorkType.Plant));
                                        }
                                        else if (subTile.terrainAmount >= TerrainContent.OrchardReady)
                                        {
                                            Farms.Add(new SubTileWork(subTileLoop.Position, WorkType.GatherFoil));
                                        }
                                        BuildingPosition.Set(ref buildingPosition.Orchard_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Orchard_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubFoilType.WheatFarm:
                                    case TerrainSubFoilType.WheatFarmUpgraded:
                                        ++buildingStructure.WheatFarm_count;
                                        ++foodspots;
                                        farming(ref subTile);
                                        BuildingPosition.Set(ref buildingPosition.WheatFarm_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WheatFarm_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubFoilType.LinenFarm:
                                    case TerrainSubFoilType.LinenFarmUpgraded:
                                        ++buildingStructure.LinenFarm_count;
                                        farming(ref subTile);
                                        BuildingPosition.Set(ref buildingPosition.LinenFarm_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.LinenFarm_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubFoilType.RapeSeedFarm:
                                    case TerrainSubFoilType.RapeSeedFarmUpgraded:
                                        ++buildingStructure.RapeSeedFarm_count;
                                        //#if DEBUG
                                        //                                        if (buildingStructure.RapeSeedFarm_count >= 8)
                                        //                                        {
                                        //                                            lib.DoNothing();
                                        //                                        }
                                        //#endif
                                        ++fuelSpots;
                                        farming(ref subTile);
                                        BuildingPosition.Set(ref buildingPosition.RapeSeedFarm_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.RapeSeedFarm_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubFoilType.HempFarm:
                                    case TerrainSubFoilType.HempFarmUpgraded:
                                        ++buildingStructure.HempFarm_count;
                                        ++fuelSpots;
                                        farming(ref subTile);
                                        BuildingPosition.Set(ref buildingPosition.HempFarm_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.HempFarm_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubFoilType.BogIron:
                                        ++terrainStructure.mineCount_bogIron;
                                        //if (BogIron.Count < workerCount)
                                        //{
                                        BogIron.Add(subTileLoop.Position);
                                        //}
                                        break;

                                    case TerrainSubFoilType.ClayPit:
                                        ++terrainStructure.resourceCount_clay;
                                        //if (ClayPit.Count < workerCount)
                                        //{
                                        ClayPit.Add(subTileLoop.Position);
                                        //}
                                        break;
                                }
                                break;
                            case TerrainMainType.Mine:
                                Mines.Add(subTileLoop.Position);

                                var mineType = (TerrainMineType)subTile.subTerrain;
                                switch (mineType)
                                {
                                    case TerrainMineType.Coal:
                                        ++terrainStructure.mineCount_coal;
                                        break;
                                    case TerrainMineType.IronOre:
                                        ++terrainStructure.mineCount_iron;
                                        break;
                                    case TerrainMineType.TinOre:
                                        ++terrainStructure.mineCount_tin;
                                        break;
                                    case TerrainMineType.CopperOre:
                                        ++terrainStructure.mineCount_copper;
                                        break;
                                    case TerrainMineType.Sulfur:
                                        ++terrainStructure.mineCount_sulfur;
                                        break;
                                    case TerrainMineType.LeadOre:
                                        ++terrainStructure.mineCount_lead;
                                        break;
                                    case TerrainMineType.SilverOre:
                                        ++terrainStructure.mineCount_silver;
                                        break;
                                    case TerrainMineType.GoldOre:
                                        ++terrainStructure.mineCount_gold;
                                        break;
                                    case TerrainMineType.Salt:
                                        ++terrainStructure.mineCount_salt;
                                        break;
                                    case TerrainMineType.StoneBlock:
                                        ++terrainStructure.mineCount_stoneblock;
                                        break;
                                    case TerrainMineType.Mithril:
                                        ++terrainStructure.mineCount_mithril;
                                        break;
                                }
                                break;
                            case TerrainMainType.Building:
                                var building = (TerrainBuildingType)subTile.subTerrain;

                                switch (building)
                                {
                                    case TerrainBuildingType.WorkerTent:
                                        ++buildingStructure.TentHuts_count;
                                        BuildingPosition.Set(ref buildingPosition.WorkerHuts_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WorkerHuts_pos = subTileLoop.Position; //Old
                                        housingCount_Workers += DssConst.HousingCount_WorkerTent;
                                        break;
                                    case TerrainBuildingType.WorkerHut:
                                        ++buildingStructure.WorkerHuts_count;
                                        BuildingPosition.Set(ref buildingPosition.WorkerHuts_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WorkerHuts_pos = subTileLoop.Position; //Old
                                        housingCount_Workers += DssConst.HousingCount_WorkerHut;
                                        break;
                                    case TerrainBuildingType.WorkerHutLarge:
                                        ++buildingStructure.WorkerHuts_Large_count;
                                        BuildingPosition.Set(ref buildingPosition.WorkerHuts_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WorkerHuts_pos = subTileLoop.Position; //Old
                                        housingCount_Workers += DssConst.HousingCount_WorkerHutLarge;
                                        break;

                                    case TerrainBuildingType.ServiceMenHouse_small:
                                        ++buildingStructure.ServiceMenHouse_count;
                                        serviceMenHousing += DssConst.HousingCount_ServiceHouse_Small;
                                        BuildingPosition.Set(ref buildingPosition.ServiceHouse_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.ServiceHouse_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.ServiceMenHouse_Large:
                                        ++buildingStructure.ServiceMenHouse_Large_count;
                                        serviceMenHousing += DssConst.HousingCount_ServiceHouse_Large;
                                        BuildingPosition.Set(ref buildingPosition.ServiceHouse_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.ServiceHouse_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.GuardHouse_Small:
                                        ++buildingStructure.GuardOffice_count;
                                        BuildingPosition.Set(ref buildingPosition.GuardHouse_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.GuardHouse_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.GuardHouse_Large:
                                        ++buildingStructure.GuardOffice_Large_count;
                                        BuildingPosition.Set(ref buildingPosition.GuardHouse_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.GuardHouse_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.FowlPen:
                                        ++buildingStructure.FowlPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.FowlGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }

                                        BuildingPosition.Set(ref buildingPosition.FowlPen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.FowlPen_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.BoarPen:
                                        ++buildingStructure.BoarPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.BoarGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.BoarPen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.BoarPen_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.HenPen:
                                        ++buildingStructure.HenPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.HenGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }

                                        BuildingPosition.Set(ref buildingPosition.HenPen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.HenPen_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.PigPen:
                                        ++buildingStructure.PigPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.PigGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.PigPen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.PigPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Postal:
                                    case TerrainBuildingType.PostalLevel2:
                                    case TerrainBuildingType.PostalLevel3:
                                        ++buildingStructure.Postal_count;
                                        BuildingPosition.Set(ref buildingPosition.Postal_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Postal_pos = subTileLoop.Position; //Old
                                        break;



                                    case TerrainBuildingType.Recruitment:
                                    case TerrainBuildingType.RecruitmentLevel2:
                                    case TerrainBuildingType.RecruitmentLevel3:
                                        ++buildingStructure.Recruitment_count;
                                        BuildingPosition.Set(ref buildingPosition.Recruitment_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Recruitment_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.ImmigrationTent:
                                        ++buildingStructure.ImmigrationTent_count;
                                        BuildingPosition.Set(ref buildingPosition.ImmigrationTent_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.ImmigrationTent_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.OxenPen:
                                        ++buildingStructure.OxenPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.OxenGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.OxenPen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.OxenPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.KineOxenPen:
                                        ++buildingStructure.KineOxenPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.KineOxenGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.KineOxenPen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.KineOxenPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.DogCage:
                                        ++buildingStructure.DogCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.DogGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.DogCage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.DogCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.HoundCage:
                                        ++buildingStructure.HoundCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.HoundGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.HoundCage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.HoundCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.PonyPen:
                                        ++buildingStructure.PonyPen_count;
                                        if (subTile.terrainAmount >= TerrainContent.PonyGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.PonyPen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.PonyPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.HorsePen:
                                        ++buildingStructure.HorsePen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount > TerrainContent.HorseGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.HorsePen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.HorsePen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.WarHorsePen:
                                        ++buildingStructure.WarHorsePen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.WarHorseGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WarHorsePen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WarHorsePen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.DraftHorsePen:
                                        ++buildingStructure.DraftHorsePen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.DraftHorseGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.DraftHorsePen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.DraftHorsePen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.WildPigPen:
                                        ++buildingStructure.WildPigPen_count;
                                        if (subTile.terrainAmount >= TerrainContent.WildPigGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WildPigPen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WildPigPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.WildHogPen:
                                        ++buildingStructure.WildHogPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.WildHogGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WildHogPen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WildHogPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.WarHogPen:
                                        ++buildingStructure.WarHogPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.WarHogGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WarHogPen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WarHogPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.StagHogPen:
                                        ++buildingStructure.StagHogPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.StagHogGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.StagHogPen_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.StagHogPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.WolfCage:
                                        ++buildingStructure.WolfCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.WolfGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WolfCage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WolfCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.WargCage:
                                        ++buildingStructure.WargCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.WargGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WargCage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WargCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.AlphaWargCage:
                                        ++buildingStructure.AlphaWargCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.AlphaWargGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.AlphaWargCage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.AlphaWargCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.WildCatCage:
                                        ++buildingStructure.WildCatCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.WildCatGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WildCatCage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WildCatCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.LionCage:
                                        ++buildingStructure.LionCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.LionGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.LionCage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.LionCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.WarLionCage:
                                        ++buildingStructure.WarLionCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.WarLionGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WarLionCage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WarLionCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.ElephantCage:
                                        ++buildingStructure.ElephantCage_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.ElephantGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.ElephantCage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.ElephantCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.WarElephantCage:
                                        ++buildingStructure.WarElephantCage_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.WarElephantGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WarElephantCage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WarElephantCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.OliphantCage:
                                        ++buildingStructure.OliphantCage_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.OliphantGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.OliphantCage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.OliphantCage_pos = subTileLoop.Position; //Old
                                        break;

                                    //case TerrainBuildingType.Postal:
                                    //case TerrainBuildingType.PostalLevel2:
                                    //case TerrainBuildingType.PostalLevel3:
                                    //    ++buildingStructure.Postal_count;
                                    //    BuildingPosition.Set(ref buildingPosition.Postal_pos, ref subTileLoop.Position); //New
                                    //    //buildingPosition.Postal_pos = subTileLoop.Position; //Old
                                    //    break;

                                    case TerrainBuildingType.SoldierBarracks:
                                        ++buildingStructure.SoldierBarracks_count;
                                        BuildingPosition.Set(ref buildingPosition.SoldierBarracks_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.SoldierBarracks_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.ArcherBarracks:
                                        ++buildingStructure.ArcherBarracks_count;
                                        BuildingPosition.Set(ref buildingPosition.ArcherBarracks_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.ArcherBarracks_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.WarmachineBarracks:
                                        ++buildingStructure.WarmachineBarracks_count;
                                        BuildingPosition.Set(ref buildingPosition.WarmachineBarracks_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WarmachineBarracks_pos = subTileLoop.Position; //Old
                                        break;
                                    //case TerrainBuildingType.KnightsBarracks:
                                    //    ++buildingStructure.KnightsBarracks_count;
                                    //    BuildingPosition.Set(ref buildingPosition.KnightsBarracks_pos, ref subTileLoop.Position); //New
                                    //    //buildingPosition.KnightsBarracks_pos = subTileLoop.Position; //Old
                                    //    break;
                                    case TerrainBuildingType.GunBarracks:
                                        ++buildingStructure.GunBarracks_count;
                                        BuildingPosition.Set(ref buildingPosition.GunBarracks_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.GunBarracks_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.CannonBarracks:
                                        ++buildingStructure.CannonBarracks_count;
                                        BuildingPosition.Set(ref buildingPosition.CannonBarracks_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.CannonBarracks_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Tavern:
                                        ++buildingStructure.Tavern_count;
                                        FoodSpots_workupdate.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Tavern_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Tavern_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.Storehouse:
                                        ++buildingStructure.Storehouse_count;
                                        StoragePoints_workupdate.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Storehouse_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Storehouse_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.Carpenter:
                                        ++buildingStructure.Carpenter_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Carpenter_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Carpenter_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.Brewery:
                                        ++buildingStructure.Brewery_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Brewery_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Brewery_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Work_CoalPit:
                                        ++buildingStructure.CoalPit_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.CoalPit_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.CoalPit_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Work_Cook:
                                        ++buildingStructure.Cook_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Cook_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Cook_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Work_Bench:
                                        ++buildingStructure.WorkBench_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.WorkBench_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WorkBench_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.Work_Smith:
                                        ++buildingStructure.Smith_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Smith_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Smith_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Smelter:
                                        ++buildingStructure.Smelter_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Smelter_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Smelter_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Foundry:
                                        ++buildingStructure.Foundry_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Foundry_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Foundry_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Armory:
                                        ++buildingStructure.Armory_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Armory_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Armory_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Chemist:
                                        ++buildingStructure.Chemist_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Chemist_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Chemist_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Gunmaker:
                                        ++buildingStructure.Gunmaker_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Gunmaker_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Gunmaker_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.WoodCutter:
                                        ++buildingStructure.WoodCutter_count;
                                        WoodCutter.Add(subTileLoop.Position);
                                        break;
                                    case TerrainBuildingType.StoneCutter:
                                        ++buildingStructure.StoneCutter_count;
                                        StoneCutter.Add(subTileLoop.Position);
                                        break;

                                    case TerrainBuildingType.Nobelhouse:
                                        ++buildingStructure.Noblehouse_count;
                                        BuildingPosition.Set(ref buildingPosition.Noblehouse_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Noblehouse_pos = subTileLoop.Position; //Old

                                        break;
                                    case TerrainBuildingType.Embassy:
                                        ++buildingStructure.Embassy_count;
                                        BuildingPosition.Set(ref buildingPosition.Embassy_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Embassy_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.School:
                                        ++buildingStructure.School_count;
                                        BuildingPosition.Set(ref buildingPosition.School_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.School_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.ResearchCenter:
                                        ++buildingStructure.ResearchCenter_count;
                                        BuildingPosition.Set(ref buildingPosition.ResearchCenter_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.ResearchCenter_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.BookPress:
                                        ++buildingStructure.BookPress_count;
                                        BuildingPosition.Set(ref buildingPosition.BookPress_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.BookPress_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.Logistics:
                                        buildingStructure.buildingLevel_logistics = subTile.terrainAmount;
                                        break;
                                    case TerrainBuildingType.ManorLord:
                                        buildingStructure.manorLord = true;
                                        DssRef.state.hasManorLords = true;
                                        break;
                                    case TerrainBuildingType.GreatHall:
                                        buildingStructure.greatHall = true;
                                        break;
                                    case TerrainBuildingType.WaterResovoir:
                                        ++buildingStructure.WaterResovoir_count;
                                        BuildingPosition.Set(ref buildingPosition.WaterResovoir_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WaterResovoir_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.GoldDeliveryLevel1:
                                    case TerrainBuildingType.GoldDeliveryLevel2:
                                    case TerrainBuildingType.GoldDeliveryLevel3:
                                        ++buildingStructure.GoldDelivery_count;
                                        BuildingPosition.Set(ref buildingPosition.GoldDelivery_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.GoldDelivery_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.Bank:
                                        ++buildingStructure.Bank_count;
                                        BuildingPosition.Set(ref buildingPosition.Bank_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Bank_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainBuildingType.CoinMinter:
                                        ++buildingStructure.CoinMinter_count;
                                        CoinMinting.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.CoinMinter_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.CoinMinter_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Pottery:
                                        ++buildingStructure.Pottery_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Pottery_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Pottery_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.DryingPan:
                                        ++buildingStructure.DryingPan_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.DryingPan_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.DryingPan_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Butcher:
                                        ++buildingStructure.Butcher_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Butcher_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Butcher_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Smoker:
                                        ++buildingStructure.Smoker_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Smoker_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Smoker_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Dryer:
                                        ++buildingStructure.Dryer_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Dryer_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.Dryer_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.ShieldMaker:
                                        ++buildingStructure.ShieldMaker_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.ShieldMaker_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.ShieldMaker_pos = subTileLoop.Position; //Old
                                        break;

                                    // --- Storage ---

                                    case TerrainBuildingType.MaterialStorage:
                                        ++buildingStructure.MaterialStorage_count;
                                        BuildingPosition.Set(ref buildingPosition.MaterialStorage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.MaterialStorage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.FoodStorage:
                                        ++buildingStructure.FoodStorage_count;
                                        BuildingPosition.Set(ref buildingPosition.FoodStorage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.FoodStorage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.WeaponStorage:
                                        ++buildingStructure.WeaponStorage_count;
                                        BuildingPosition.Set(ref buildingPosition.WeaponStorage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.WeaponStorage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.ArmorStorage:
                                        ++buildingStructure.ArmorStorage_count;
                                        BuildingPosition.Set(ref buildingPosition.ArmorStorage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.ArmorStorage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.AnimalStorage:
                                        ++buildingStructure.AnimalStorage_count;
                                        BuildingPosition.Set(ref buildingPosition.AnimalStorage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.AnimalStorage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.Cesspit:
                                        ++buildingStructure.CessPit_count;
                                        //BuildingPosition.Set(ref buildingPosition.AnimalStorage_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.AnimalStorage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainBuildingType.TrappersHut:
                                        ++buildingStructure.TrapperHut_count;
                                        BuildingPosition.Set(ref buildingPosition.TrapperHut_pos, ref subTileLoop.Position); //New
                                        //buildingPosition.TrapperHut_pos = subTileLoop.Position; //Old
                                        TrapperHuts.Add(subTileLoop.Position);
                                        break;

                                    case TerrainBuildingType.FowlHabitat:
                                        ++terrainStructure.wildAnimalCount_Fowl;
                                        if (subTile.terrainAmount >= TerrainContent.FowlGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;
                                    case TerrainBuildingType.BoarHabitat:
                                        ++terrainStructure.wildAnimalCount_Boar;
                                        if (subTile.terrainAmount >= TerrainContent.BoarGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;
                                    case TerrainBuildingType.DogHabitat:
                                        ++terrainStructure.wildAnimalCount_Dog;
                                        if (subTile.terrainAmount >= TerrainContent.DogGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;
                                    case TerrainBuildingType.OxHabitat:
                                        ++terrainStructure.wildAnimalCount_Ox;
                                        if (subTile.terrainAmount >= TerrainContent.OxenGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;
                                    case TerrainBuildingType.PonyHabitat:
                                        ++terrainStructure.wildAnimalCount_Pony;
                                        if (subTile.terrainAmount >= TerrainContent.PonyGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;

                                    case TerrainBuildingType.WolfHabitat:
                                        ++terrainStructure.wildAnimalCount_Wolf;
                                        if (subTile.terrainAmount >= TerrainContent.WolfGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;

                                    case TerrainBuildingType.CatHabitat:
                                        ++terrainStructure.wildAnimalCount_Cat;
                                        if (subTile.terrainAmount >= TerrainContent.WildCatGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;

                                    case TerrainBuildingType.ElephantHabitat:
                                        ++terrainStructure.wildAnimalCount_Elephant;
                                        if (subTile.terrainAmount >= TerrainContent.ElephantGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;

                                }


                                break;
                            case TerrainMainType.Destroyed:
                            case TerrainMainType.DefaultLand:
                                if (emptyLandExpansions > 0)
                                {
                                    if (emptyArea.size.X == 0)
                                    {
                                        emptyArea = Rectangle2.FromCenterTileAndRadius(subTileLoop.Position, 3);
                                    }
                                    else if (!emptyArea.IntersectTilePoint(subTileLoop.Position))
                                    {
                                        --emptyLandExpansions;
                                        //EmptyLand.Add(subTileLoop.Position);
                                        emptyArea.includeTileAndRadius(subTileLoop.Position, 3);
                                    }
                                }
                                break;
                            case TerrainMainType.Wall:
                                ++buildingStructure.wallCount;
                                break;
                        }
                    }
                }
            }

            checkPenUpkeep(city);

            buildingStructure.SuggestedTrapperPos = IntVector2.Zero;

            foreach (var pos in WildAnimals)
            {
                bool inTrapperRange = false;
                foreach (var trapPos in TrapperHuts)
                {
                    if (pos.SideLength(trapPos) <= DssConst.TrapperHutRadius)
                    {
                        AnimalPens.Add(pos);
                        inTrapperRange = true;
                        break;
                    }
                }

                if (!inTrapperRange && Ref.peRnd.ChanceF(0.5f))
                {
                    buildingStructure.SuggestedTrapperPos = pos + arraylib.RandomListMember(IntVector2.Dir4Array) * Ref.rnd.Int(2, 4);
                }

            }

            city.buildingStructure = buildingStructure;
            city.terrainStructure = terrainStructure;

            if (city.HousingCount_Workers != housingCount_Workers && housingCount_Workers > 0)
            {
                city.HousingCount_Workers = housingCount_Workers;
            }

            void farming(ref SubTile subTile)
            {
                if (subTile.terrainAmount == TerrainContent.FarmCulture_Empty)
                {
                    Farms.Add(new SubTileWork(subTileLoop.Position, WorkType.Plant));
                }
                else if (subTile.terrainAmount >= TerrainContent.FarmCulture_ReadySize)
                {
                    Farms.Add(new SubTileWork(subTileLoop.Position, WorkType.GatherFoil));
                }
            }
        }

        void checkPenUpkeep(City city)
        {
            if (!city.PenUpkeep_IsPayed)
            {
                Faction faction = city.pfaction.GetFaction();

                foreach (var pos in AnimalPens)
                {
                    EditSubTile editValue = new EditSubTile(faction, false, pos, new SubTile() { terrainAmount = 1 }, false, true, false);
                    editValue.Submit();
                }
            }
        }
        //public bool MayAutoBuildHere(City city, IntVector2 subTilePos)
        //{
        //    if (DssRef.world.subTileGrid.TryGet(subTilePos, out var subtile))
        //    {
        //        switch (subtile.mainTerrain)
        //        {
        //            case TerrainMainType.Destroyed:
        //            case TerrainMainType.DefaultLand:
        //                var tile = DssRef.world.tileGrid.Get(WP.SubtileToTilePos(subTilePos));
        //                return tile.MayBuild() && tile.CityIndex == city.myIndex;

        //        }
        //    }
        //    return false;
        //}

        public IntVector2 eatPosition(IntVector2 workerSubtile)
        { 
            int closestDist = int.MaxValue;
            IntVector2 result = IntVector2.MinValue;

            foreach (var pos in FoodSpots_workupdate)
            { 
                int dist = workerSubtile.SideLength(pos);
                if (dist < closestDist)
                { 
                    closestDist = dist;
                    result = pos;
                }
            }

            return result;
        }

        public IntVector2 storePosition(IntVector2 workerSubtile)
        {
            int closestDist = int.MaxValue;
            IntVector2 result = IntVector2.MinValue;

            foreach (var pos in StoragePoints_workupdate)
            {
                int dist = workerSubtile.SideLength(pos);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    result = pos;
                }
            }

            return result;
        }

        public bool inBonusRadius(IntVector2 pos, List<IntVector2> bonusLocations, int radius)
        {
            foreach (var loc in bonusLocations)
            {
                if (pos.SideLength(loc) <= radius)
                { return true; }
            }

            return false;
        }
    }

    struct SubTileWork
    {
        public IntVector2 subtile;
        public WorkType workType;

        public SubTileWork(IntVector2 subtile, WorkType workType)
        { 
            this.subtile = subtile;
            this.workType = workType;
        }
    }
}
