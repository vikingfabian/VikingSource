using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Core.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Work;
using VikingEngine.LootFest.Map;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.DSSWars.Map.MapProcess
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

            int cityradius = city.citySumTileArea.size.SideLength() / 2;
            for (int radius = 2; radius <= cityradius; ++radius)
            {
                ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.maptilePos, radius));

                while (cirkleLoop.Next())
                {
                    if (DssRef.world.GetMapAndSumTile_Safe(cirkleLoop.Position, out var mapTile, out var sumTile))/*.maptileBounds.IntersectTilePoint(cirkleLoop.Position)*/
                    {
                        if (sumTile.pcity.cityIndex == city.myIndex && mapTile.IsLand())
                        {
                            topleft = WP.ToSubTilePos_TopLeft(cirkleLoop.Position);
                            subTileLoop = new ForXYLoop(topleft, topleft + Map.MapData.MapTile1_1.ModelScale_Inv_MaxIndex);

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
                                            
                                            subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.SmallStones, 1);
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
            int maxRadius = city.citySumTileArea.size.SideLength();
            for (int radius = 0; radius < maxRadius; ++radius)
            {
                ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.maptilePos, radius));

                while (cirkleLoop.Next())
                {
                    //if (DssRef.world.tileBounds.IntersectTilePoint(cirkleLoop.Position))
                    if (DssRef.world.GetMapAndSumTile_Safe(cirkleLoop.Position, out var mapTile, out var sumTile))
                    {
                        //var tile = DssRef.world.tileGrid.Get(cirkleLoop.Position);
                        if (sumTile.pcity.cityIndex == city.myIndex && mapTile.IsLand())
                        {
                            topleft = WP.ToSubTilePos_TopLeft(cirkleLoop.Position);
                            subTileLoop = new ForXYLoop(topleft, topleft + Map.MapData.MapTile1_1.ModelScale_Inv_MaxIndex);

                            while (subTileLoop.Next())
                            {
                                MapTile1_1 subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);

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
            int maxRadius = city.citySumTileArea.size.SideLength();
            for (int radius = 0; radius < maxRadius; ++radius)
            {
                ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.maptilePos, radius));

                while (cirkleLoop.Next())
                {
                    if (DssRef.world.GetMapAndSumTile_Safe(cirkleLoop.Position, out var mapTile, out var sumTile))
                    {
                        var tile = DssRef.world.tileGrid.Get(cirkleLoop.Position);
                        if (tile.pcity.cityIndex == city.myIndex /*&& tile.MayBuild()*/)
                        {
                            topleft = WP.ToSubTilePos_TopLeft(cirkleLoop.Position);
                            subTileLoop = new ForXYLoop(topleft, topleft + Map.MapData.MapTile1_1.ModelScale_Inv_MaxIndex);

                            while (subTileLoop.Next())
                            {
                                MapTile1_1 subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);

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

            int maxRadius = city.citySumTileArea.size.SideLength();
            for (int radius = 0; radius < maxRadius; ++radius)
            {
                //ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.tilePos, radius));
                edgeRandomizer.start(Rectangle2.FromCenterTileAndRadius(city.maptilePos, radius));
                while (edgeRandomizer.Next())
                {
                    //if (DssRef.world.tileBounds.IntersectTilePoint(edgeRandomizer.Position))
                    //{
                        //if (DssRef.world.tileGrid.TryGet(edgeRandomizer.Position, out SumTile4_4 tile))
                        //{
                        if (DssRef.world.GetMapAndSumTile_Safe(edgeRandomizer.Position, out var mapTile, out var sumTile))
                        {
                            if (sumTile.pcity.cityIndex == city.myIndex && mapTile.IsLand())
                            {
                                IntVector2 topleft = edgeRandomizer.Position;
                                ForXYLoop subTileLoop = new ForXYLoop(topleft, topleft + Map.MapData.MapTile1_1.ModelScale_Inv_MaxIndex);

                                while (subTileLoop.Next())
                                {
                                    MapTile1_1 subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);
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
                    //}
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
            int armySupply = 0;

            Rectangle2 emptyArea = Rectangle2.Zero;
            buildingPosition = new BuildingPosition();
            BuildingStructure buildingStructure = new BuildingStructure() { gotUpdate = true };
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
            ForXYLoop loop = new ForXYLoop(city.citySumTileArea);
            while (loop.Next())
            {
                topleft = WP.SumTileToMaptile(loop.Position);
                if (world.GetMapAndSumTile_Safe(topleft, out var mapTile, out var sumTile) &&
                    sumTile.pcity.cityIndex == city.myIndex &&
                    mapTile.IsLand())
                {
                    
                    subTileLoop = new ForXYLoop(topleft, topleft + Map.MapData.SumTile4_4.TileWidth);

                    while (subTileLoop.Next())
                    {
                        MapTile1_1 subTile = world.subTileGrid.Get(subTileLoop.Position);

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
                                    case TerrainSubFoilType.TreeSoft:
                                    case TerrainSubFoilType.TreeHard:
                                    case TerrainSubFoilType.DryWood:
                                        if (foil == TerrainSubFoilType.DryWood || subTile.terrainAmount >= TerrainContent.TreeReadySize)
                                        {
                                            ++terrainStructure.resourceCount_wood;
                                            ++fuelSpots;
                                            Trees.Add(subTileLoop.Position);
                                        }
                                        break;

                                    case TerrainSubFoilType.LargeStones:
                                    case TerrainSubFoilType.SmallStones:
                                        ++terrainStructure.resourceCount_stone;
                                        Stones.Add(subTileLoop.Position);
                                        break;

                                }
                                break;
                            case TerrainMainType.Farm:
                                switch ((TerrainSubFarmType)subTile.subTerrain)
                                {  
                                    case TerrainSubFarmType.TreeApple:
                                    case TerrainSubFarmType.TreeBanana:
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
                                        BuildingPosition.Set(ref buildingPosition.Orchard_pos, ref subTileLoop.Position); 
                                        //buildingPosition.Orchard_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubFarmType.WheatFarm:
                                    case TerrainSubFarmType.WheatFarmUpgraded:
                                        ++buildingStructure.WheatFarm_count;
                                        ++foodspots;
                                        farming(ref subTile);
                                        BuildingPosition.Set(ref buildingPosition.WheatFarm_pos, ref subTileLoop.Position); 
                                        //buildingPosition.WheatFarm_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubFarmType.LinenFarm:
                                    case TerrainSubFarmType.LinenFarmUpgraded:
                                        ++buildingStructure.LinenFarm_count;
                                        farming(ref subTile);
                                        BuildingPosition.Set(ref buildingPosition.LinenFarm_pos, ref subTileLoop.Position); 
                                        //buildingPosition.LinenFarm_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubFarmType.RapeSeedFarm:
                                    case TerrainSubFarmType.RapeSeedFarmUpgraded:
                                        ++buildingStructure.RapeSeedFarm_count;
                                        //#if DEBUG
                                        //                                        if (buildingStructure.RapeSeedFarm_count >= 8)
                                        //                                        {
                                        //                                            lib.DoNothing();
                                        //                                        }
                                        //#endif
                                        ++fuelSpots;
                                        farming(ref subTile);
                                        BuildingPosition.Set(ref buildingPosition.RapeSeedFarm_pos, ref subTileLoop.Position); 
                                        //buildingPosition.RapeSeedFarm_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubFarmType.HempFarm:
                                    case TerrainSubFarmType.HempFarmUpgraded:
                                        ++buildingStructure.HempFarm_count;
                                        ++fuelSpots;
                                        farming(ref subTile);
                                        BuildingPosition.Set(ref buildingPosition.HempFarm_pos, ref subTileLoop.Position); 
                                        //buildingPosition.HempFarm_pos = subTileLoop.Position; //Old
                                        break;


                                }
                                break;
                            case TerrainMainType.Mine:
                                Mines.Add(subTileLoop.Position);

                                var mineType = (TerrainMineType)subTile.subTerrain;
                                switch (mineType)
                                {
                                    case TerrainMineType.BogIron:
                                        ++terrainStructure.mineCount_bogIron;
                                        //if (BogIron.Count < workerCount)
                                        //{
                                        BogIron.Add(subTileLoop.Position);
                                        //}
                                        break;

                                    case TerrainMineType.ClayPit:
                                        ++terrainStructure.resourceCount_clay;
                                        //if (ClayPit.Count < workerCount)
                                        //{
                                        ClayPit.Add(subTileLoop.Position);
                                        //}
                                        break;
                                    case TerrainMineType.SaltPit:
                                        break;
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
                                    case TerrainBuildingType.Logistics:
                                        buildingStructure.buildingLevel_logistics = subTile.terrainAmount;
                                        BuildingPosition.Set(ref buildingPosition.GovernentBuildings_pos, ref subTileLoop.Position);
                                        break;

                                    case TerrainBuildingType.ManorLord:
                                        buildingStructure.manorLord = true;
                                        DssRef.state.hasManorLords = true;
                                        BuildingPosition.Set(ref buildingPosition.GovernentBuildings_pos, ref subTileLoop.Position);
                                        break;

                                    case TerrainBuildingType.GreatHall:
                                        buildingStructure.greatHall = true;
                                        BuildingPosition.Set(ref buildingPosition.GovernentBuildings_pos, ref subTileLoop.Position);
                                        break;

                                    case TerrainBuildingType.Economics:
                                        buildingStructure.economics = true;
                                        BuildingPosition.Set(ref buildingPosition.GovernentBuildings_pos, ref subTileLoop.Position);
                                        break;

                                    case TerrainBuildingType.TingsHouse:
                                        buildingStructure.tingshouse = true;
                                        BuildingPosition.Set(ref buildingPosition.GovernentBuildings_pos, ref subTileLoop.Position);
                                        break;

                                    case TerrainBuildingType.ExpansionHall_Lvl1:
                                        ++buildingStructure.ExpansionHall_count;
                                        buildingStructure.ExpansionHall_LowestLevel = Math.Min(buildingStructure.ExpansionHall_LowestLevel, 1);
                                        
                                        break;
                                    case TerrainBuildingType.ExpansionHall_Lvl2:
                                        ++buildingStructure.ExpansionHall_count;
                                        buildingStructure.ExpansionHall_LowestLevel = Math.Min(buildingStructure.ExpansionHall_LowestLevel, 2);

                                        break;
                                    case TerrainBuildingType.ExpansionHall_Lvl3:
                                        ++buildingStructure.ExpansionHall_count;
                                        buildingStructure.ExpansionHall_LowestLevel = Math.Min(buildingStructure.ExpansionHall_LowestLevel, 3);

                                        break;
                                    case TerrainBuildingType.WorkerTent:
                                        ++buildingStructure.TentHuts_count;
                                        BuildingPosition.Set(ref buildingPosition.WorkerHuts_pos, ref subTileLoop.Position); 
                                        housingCount_Workers += DssConst.HousingCount_WorkerTent;
                                        break;

                                    case TerrainBuildingType.WorkerHut:
                                        ++buildingStructure.WorkerHuts_count;
                                        BuildingPosition.Set(ref buildingPosition.WorkerHuts_pos, ref subTileLoop.Position); 
                                        housingCount_Workers += DssConst.HousingCount_WorkerHut;
                                        break;

                                    case TerrainBuildingType.WorkerHutLarge:
                                        ++buildingStructure.WorkerHuts_Large_count;
                                        BuildingPosition.Set(ref buildingPosition.WorkerHuts_pos, ref subTileLoop.Position); 
                                        housingCount_Workers += DssConst.HousingCount_WorkerHutLarge;
                                        break;

                                    case TerrainBuildingType.ServiceMenHouse_Small:
                                        ++buildingStructure.ServiceMenHouse_count;
                                        serviceMenHousing += DssConst.HousingCount_ServiceHouse_Small;
                                        BuildingPosition.Set(ref buildingPosition.ServiceHouse_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.ServiceMenHouse_Large:
                                        ++buildingStructure.ServiceMenHouse_Large_count;
                                        serviceMenHousing += DssConst.HousingCount_ServiceHouse_Large;
                                        BuildingPosition.Set(ref buildingPosition.ServiceHouse_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.GuardHouse_Small:
                                        ++buildingStructure.GuardOffice_count;
                                        BuildingPosition.Set(ref buildingPosition.GuardHouse_pos, ref subTileLoop.Position);                                        
                                        break;
                                    case TerrainBuildingType.GuardHouse_Large:
                                        ++buildingStructure.GuardOffice_Large_count;
                                        BuildingPosition.Set(ref buildingPosition.GuardHouse_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.SoldiersQuarter_Small:
                                        ++buildingStructure.SoldiersQuarter_Small_count;
                                        BuildingPosition.Set(ref buildingPosition.SoldiersQuarter_pos, ref subTileLoop.Position); 
                                        armySupply += DssConst.SoldiersQuarter_Small_Supply;
                                        break;

                                    case TerrainBuildingType.SoldiersQuarter_Large:
                                        ++buildingStructure.SoldiersQuarter_Large_count;
                                        BuildingPosition.Set(ref buildingPosition.SoldiersQuarter_pos, ref subTileLoop.Position); 
                                        armySupply += DssConst.SoldiersQuarter_Large_Supply;
                                        break;

                                    case TerrainBuildingType.Postal:
                                    case TerrainBuildingType.PostalLevel2:
                                    case TerrainBuildingType.PostalLevel3:
                                        ++buildingStructure.Postal_count;
                                        BuildingPosition.Set(ref buildingPosition.Postal_pos, ref subTileLoop.Position); 
                                        //buildingPosition.Postal_pos = subTileLoop.Position; //Old
                                        break;



                                    case TerrainBuildingType.Recruitment:
                                    case TerrainBuildingType.RecruitmentLevel2:
                                    case TerrainBuildingType.RecruitmentLevel3:
                                        ++buildingStructure.Recruitment_count;
                                        BuildingPosition.Set(ref buildingPosition.Recruitment_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.ImmigrationTent:
                                        ++buildingStructure.ImmigrationTent_count;
                                        BuildingPosition.Set(ref buildingPosition.ImmigrationTent_pos, ref subTileLoop.Position); 
                                        break;
                                    

                                    case TerrainBuildingType.SoldierBarracks:
                                        ++buildingStructure.SoldierBarracks_count;
                                        BuildingPosition.Set(ref buildingPosition.SoldierBarracks_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.ArcherBarracks:
                                        ++buildingStructure.ArcherBarracks_count;
                                        BuildingPosition.Set(ref buildingPosition.ArcherBarracks_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.WarmachineBarracks:
                                        ++buildingStructure.WarmachineBarracks_count;
                                        BuildingPosition.Set(ref buildingPosition.WarmachineBarracks_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.GunBarracks:
                                        ++buildingStructure.GunBarracks_count;
                                        BuildingPosition.Set(ref buildingPosition.GunBarracks_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.CannonBarracks:
                                        ++buildingStructure.CannonBarracks_count;
                                        BuildingPosition.Set(ref buildingPosition.CannonBarracks_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.Tavern:
                                        ++buildingStructure.Tavern_count;
                                        FoodSpots_workupdate.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Tavern_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.Storehouse:
                                        ++buildingStructure.Storehouse_count;
                                        StoragePoints_workupdate.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Storehouse_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.Carpenter:
                                        ++buildingStructure.Carpenter_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Carpenter_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.Brewery:
                                        ++buildingStructure.Brewery_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Brewery_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.Work_CoalPit:
                                        ++buildingStructure.CoalPit_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.CoalPit_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.Work_Cook:
                                        ++buildingStructure.Cook_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Cook_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.Work_Bench:
                                        ++buildingStructure.WorkBench_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.WorkBench_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.Work_Smith:
                                        ++buildingStructure.Smith_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Smith_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.Smelter:
                                        ++buildingStructure.Smelter_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Smelter_pos, ref subTileLoop.Position); 
                                         break;

                                    case TerrainBuildingType.Foundry:
                                        ++buildingStructure.Foundry_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Foundry_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.Armory:
                                        ++buildingStructure.Armory_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Armory_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.Chemist:
                                        ++buildingStructure.Chemist_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Chemist_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.Gunmaker:
                                        ++buildingStructure.Gunmaker_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Gunmaker_pos, ref subTileLoop.Position); 
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
                                        BuildingPosition.Set(ref buildingPosition.Noblehouse_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.Embassy:
                                        ++buildingStructure.Embassy_count;
                                        BuildingPosition.Set(ref buildingPosition.Embassy_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.School:
                                        ++buildingStructure.School_count;
                                        BuildingPosition.Set(ref buildingPosition.School_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.ResearchCenter:
                                        ++buildingStructure.ResearchCenter_count;
                                        BuildingPosition.Set(ref buildingPosition.ResearchCenter_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.BookPress:
                                        ++buildingStructure.BookPress_count;
                                        BuildingPosition.Set(ref buildingPosition.BookPress_pos, ref subTileLoop.Position); 
                                       break;
                                    
                                    case TerrainBuildingType.WaterResovoir:
                                        ++buildingStructure.WaterResovoir_count;
                                        BuildingPosition.Set(ref buildingPosition.Water_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.GoldDeliveryLevel1:
                                    case TerrainBuildingType.GoldDeliveryLevel2:
                                    case TerrainBuildingType.GoldDeliveryLevel3:
                                        ++buildingStructure.GoldDelivery_count;
                                        BuildingPosition.Set(ref buildingPosition.GoldDelivery_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.Bank:
                                        ++buildingStructure.Bank_count;
                                        BuildingPosition.Set(ref buildingPosition.Bank_pos, ref subTileLoop.Position); 
                                        break;
                                    case TerrainBuildingType.CoinMinter:
                                        ++buildingStructure.CoinMinter_count;
                                        CoinMinting.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.CoinMinter_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.Pottery:
                                        ++buildingStructure.Pottery_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Pottery_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.DryingPan:
                                        ++buildingStructure.DryingPan_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.DryingPan_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.Butcher:
                                        ++buildingStructure.Butcher_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Butcher_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.Smoker:
                                        ++buildingStructure.Smoker_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Smoker_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.Dryer:
                                        ++buildingStructure.Dryer_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.Dryer_pos, ref subTileLoop.Position); 
                                       break;

                                    case TerrainBuildingType.ShieldMaker:
                                        ++buildingStructure.ShieldMaker_count;
                                        CraftStation.Add(subTileLoop.Position);
                                        BuildingPosition.Set(ref buildingPosition.ShieldMaker_pos, ref subTileLoop.Position); 
                                        break;

                                    // --- Storage ---

                                    case TerrainBuildingType.MaterialStorage:
                                        ++buildingStructure.MaterialStorage_count;
                                        BuildingPosition.Set(ref buildingPosition.MaterialStorage_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.FoodStorage:
                                        ++buildingStructure.FoodStorage_count;
                                        BuildingPosition.Set(ref buildingPosition.FoodStorage_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.WeaponStorage:
                                        ++buildingStructure.WeaponStorage_count;
                                        BuildingPosition.Set(ref buildingPosition.WeaponStorage_pos, ref subTileLoop.Position); 
                                       break;

                                    case TerrainBuildingType.ArmorStorage:
                                        ++buildingStructure.ArmorStorage_count;
                                        BuildingPosition.Set(ref buildingPosition.ArmorStorage_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.AnimalStorage:
                                        ++buildingStructure.AnimalStorage_count;
                                        BuildingPosition.Set(ref buildingPosition.AnimalStorage_pos, ref subTileLoop.Position); 
                                        break;

                                    case TerrainBuildingType.Cesspit:
                                        ++buildingStructure.CessPit_count;
                                        break;

                                    case TerrainBuildingType.TrappersHut:
                                        ++buildingStructure.TrapperHut_count;
                                        BuildingPosition.Set(ref buildingPosition.TrapperHut_pos, ref subTileLoop.Position); 
                                        TrapperHuts.Add(subTileLoop.Position);
                                        break;

                                    

                                }


                                break;

                            case TerrainMainType.AnimalPen:
                                switch ((TerrainSubAnimalPenType)subTile.subTerrain)
                                {
                                    case TerrainSubAnimalPenType.FowlPen:
                                        ++buildingStructure.FowlPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.FowlGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }

                                        BuildingPosition.Set(ref buildingPosition.FowlPen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.FowlPen_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainSubAnimalPenType.BoarPen:
                                        ++buildingStructure.BoarPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.BoarGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.BoarPen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.BoarPen_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainSubAnimalPenType.HenPen:
                                        ++buildingStructure.HenPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.HenGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }

                                        BuildingPosition.Set(ref buildingPosition.HenPen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.HenPen_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainSubAnimalPenType.PigPen:
                                        ++buildingStructure.PigPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.PigGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.PigPen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.PigPen_pos = subTileLoop.Position; //Old
                                        break;
                                    case TerrainSubAnimalPenType.FowlHabitat:
                                        ++terrainStructure.wildAnimalCount_Fowl;
                                        if (subTile.terrainAmount >= TerrainContent.FowlGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;
                                    case TerrainSubAnimalPenType.BoarHabitat:
                                        ++terrainStructure.wildAnimalCount_Boar;
                                        if (subTile.terrainAmount >= TerrainContent.BoarGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;
                                    case TerrainSubAnimalPenType.DogHabitat:
                                        ++terrainStructure.wildAnimalCount_Dog;
                                        if (subTile.terrainAmount >= TerrainContent.DogGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;
                                    case TerrainSubAnimalPenType.OxHabitat:
                                        ++terrainStructure.wildAnimalCount_Ox;
                                        if (subTile.terrainAmount >= TerrainContent.OxenGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;
                                    case TerrainSubAnimalPenType.PonyHabitat:
                                        ++terrainStructure.wildAnimalCount_Pony;
                                        if (subTile.terrainAmount >= TerrainContent.PonyGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;

                                    case TerrainSubAnimalPenType.WolfHabitat:
                                        ++terrainStructure.wildAnimalCount_Wolf;
                                        if (subTile.terrainAmount >= TerrainContent.WolfGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;

                                    case TerrainSubAnimalPenType.CatHabitat:
                                        ++terrainStructure.wildAnimalCount_Cat;
                                        if (subTile.terrainAmount >= TerrainContent.WildCatGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;

                                    case TerrainSubAnimalPenType.ElephantHabitat:
                                        ++terrainStructure.wildAnimalCount_Elephant;
                                        if (subTile.terrainAmount >= TerrainContent.ElephantGrowth.harvestReady)
                                        {
                                            WildAnimals.Add(subTileLoop.Position);
                                        }
                                        break;
                                    case TerrainSubAnimalPenType.OxenPen:
                                        ++buildingStructure.OxenPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.OxenGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.OxenPen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.OxenPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.KineOxenPen:
                                        ++buildingStructure.KineOxenPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.KineOxenGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.KineOxenPen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.KineOxenPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.DogCage:
                                        ++buildingStructure.DogCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.DogGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.DogCage_pos, ref subTileLoop.Position); 
                                        //buildingPosition.DogCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.HoundCage:
                                        ++buildingStructure.HoundCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.HoundGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.HoundCage_pos, ref subTileLoop.Position); 
                                        //buildingPosition.HoundCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.PonyPen:
                                        ++buildingStructure.PonyPen_count;
                                        if (subTile.terrainAmount >= TerrainContent.PonyGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.PonyPen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.PonyPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.HorsePen:
                                        ++buildingStructure.HorsePen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount > TerrainContent.HorseGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.HorsePen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.HorsePen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.WarHorsePen:
                                        ++buildingStructure.WarHorsePen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.WarHorseGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WarHorsePen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.WarHorsePen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.DraftHorsePen:
                                        ++buildingStructure.DraftHorsePen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.DraftHorseGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.DraftHorsePen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.DraftHorsePen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.WildPigPen:
                                        ++buildingStructure.WildPigPen_count;
                                        if (subTile.terrainAmount >= TerrainContent.WildPigGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WildPigPen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.WildPigPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.WildHogPen:
                                        ++buildingStructure.WildHogPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.WildHogGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WildHogPen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.WildHogPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.WarHogPen:
                                        ++buildingStructure.WarHogPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.WarHogGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WarHogPen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.WarHogPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.StagHogPen:
                                        ++buildingStructure.StagHogPen_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.StagHogGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.StagHogPen_pos, ref subTileLoop.Position); 
                                        //buildingPosition.StagHogPen_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.WolfCage:
                                        ++buildingStructure.WolfCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.WolfGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WolfCage_pos, ref subTileLoop.Position); 
                                        //buildingPosition.WolfCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.WargCage:
                                        ++buildingStructure.WargCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.WargGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WargCage_pos, ref subTileLoop.Position); 
                                        //buildingPosition.WargCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.AlphaWargCage:
                                        ++buildingStructure.AlphaWargCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.AlphaWargGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.AlphaWargCage_pos, ref subTileLoop.Position); 
                                        //buildingPosition.AlphaWargCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.WildCatCage:
                                        ++buildingStructure.WildCatCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.WildCatGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WildCatCage_pos, ref subTileLoop.Position); 
                                        //buildingPosition.WildCatCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.LionCage:
                                        ++buildingStructure.LionCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.LionGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.LionCage_pos, ref subTileLoop.Position); 
                                        //buildingPosition.LionCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.WarLionCage:
                                        ++buildingStructure.WarLionCage_count;
                                        if (subTile.terrainAmount >= TerrainContent.WarLionGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WarLionCage_pos, ref subTileLoop.Position); 
                                        //buildingPosition.WarLionCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.ElephantCage:
                                        ++buildingStructure.ElephantCage_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.ElephantGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.ElephantCage_pos, ref subTileLoop.Position); 
                                        //buildingPosition.ElephantCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.WarElephantCage:
                                        ++buildingStructure.WarElephantCage_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.WarElephantGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.WarElephantCage_pos, ref subTileLoop.Position); 
                                        //buildingPosition.WarElephantCage_pos = subTileLoop.Position; //Old
                                        break;

                                    case TerrainSubAnimalPenType.OliphantCage:
                                        ++buildingStructure.OliphantCage_count;
                                        ++foodspots;
                                        if (subTile.terrainAmount >= TerrainContent.OliphantGrowth.harvestReady)
                                        {
                                            AnimalPens.Add(subTileLoop.Position);
                                        }
                                        BuildingPosition.Set(ref buildingPosition.OliphantCage_pos, ref subTileLoop.Position); 
                                        //buildingPosition.OliphantCage_pos = subTileLoop.Position; //Old
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

            void farming(ref MapTile1_1 subTile)
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
                //Faction faction = city.pfaction.GetFaction();

                foreach (var pos in AnimalPens)
                {
                    EditSubTile editValue = new EditSubTile(city.pfaction, false, pos, new MapTile1_1() { terrainAmount = 1 }, false, true, false);
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
