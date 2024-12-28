using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.ToGG.ToggEngine.Map;
using VikingEngine.DSSWars.Work;
using VikingEngine.DSSWars.Build;

namespace VikingEngine.DSSWars.Map
{
    class CityStructure
    {
        public static readonly CityStructure WorkInstance = new CityStructure();
        public static readonly CityStructure AutomationInstance = new CityStructure();

        public List<IntVector2> FoodSpots_workupdate = new List<IntVector2>(4);
        public List<IntVector2> StoragePoints_workupdate = new List<IntVector2>(4);
        public List<IntVector2> Trees = new List<IntVector2>(20);
        public List<IntVector2> Stones = new List<IntVector2>(20);
        public List<SubTileWork> Farms = new List<SubTileWork>(20);
        //public List<IntVector2> FarmGather = new List<IntVector2>(20);
        public List<IntVector2> AnimalPens = new List<IntVector2>(20);
        public List<IntVector2> BogIron = new List<IntVector2>(20);
        public List<IntVector2> Mines = new List<IntVector2>(20);
        public List<IntVector2> CraftStation = new List<IntVector2>(20);
        public List<IntVector2> CoinMinting = new List<IntVector2>(2);
        public List<IntVector2> EmptyLand = new List<IntVector2>(2);
        public List<IntVector2> ResourceOnGround = new List<IntVector2>(20);

        public List<IntVector2> WoodCutter = new List<IntVector2>(20);
        public List<IntVector2> StoneCutter = new List<IntVector2>(20);

        //int nobelHouseCount = 0;
        public int fuelSpots = 0;
        public int foodspots = 0;
        //public int logisticsLevel = 0;
        public bool newCity = true;

        public int mineCount_iron = 0;
        public int mineCount_tin = 0;
        public int mineCount_copper = 0;
        public int mineCount_lead = 0;
        public int mineCount_silver = 0;
        public int mineCount_gold = 0;
        public int mineCount_mithril = 0;
        public int mineCount_sulfur = 0;
        public int mineCount_coal = 0;
        public BuildingPosition buildingPosition;


        public void setupTutorialMap(City city)
        {
            IntVector2 topleft;
            ForXYLoop subTileLoop;

            int wood = 4;
            int stone = 2;

            for (int radius = 2; radius <= city.cityTileRadius; ++radius)
            {
                ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.tilePos, radius));

                while (cirkleLoop.Next())
                {
                    if (DssRef.world.tileBounds.IntersectTilePoint(cirkleLoop.Position))
                    {
                        var tile = DssRef.world.tileGrid.Get(cirkleLoop.Position);
                        if (tile.CityIndex == city.parentArrayIndex && tile.IsLand())
                        {
                            topleft = WP.ToSubTilePos_TopLeft(cirkleLoop.Position);
                            subTileLoop = new ForXYLoop(topleft, topleft + WorldData.TileSubDivitions_MaxIndex);

                            while (subTileLoop.Next())
                            {
                                var subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);

                                switch (subTile.mainTerrain)
                                {
                                    
                                    case TerrainMainType.Destroyed:
                                    case TerrainMainType.DefaultLand:
                                        if (wood > 0)
                                        {
                                            --wood;
                                            subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.DryWood, TerrainContent.TreeReadySize);
                                            DssRef.world.subTileGrid.Set(subTileLoop.Position, subTile);
                                        }
                                        else 
                                        {
                                            
                                            subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.Stones, 1);
                                            DssRef.world.subTileGrid.Set(subTileLoop.Position, subTile);
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

                update(city, workerCount);
            }
        }

        public bool find(City city, TerrainMainType main, int sub, out IntVector2 position)
        {
            IntVector2 topleft;
            ForXYLoop subTileLoop;
            for (int radius = 0; radius <= city.cityTileRadius; ++radius)
            {
                ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.tilePos, radius));

                while (cirkleLoop.Next())
                {
                    if (DssRef.world.tileBounds.IntersectTilePoint(cirkleLoop.Position))
                    {
                        var tile = DssRef.world.tileGrid.Get(cirkleLoop.Position);
                        if (tile.CityIndex == city.parentArrayIndex && tile.IsLand())
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

        public void update(City city, int workerCount, int emptyLandExpansions = 2)
        {
            //int emptyLandExpansions = 2;

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
            Mines.Clear();
            CraftStation.Clear();
            CoinMinting.Clear();
            EmptyLand.Clear();
            ResourceOnGround.Clear();
            WoodCutter.Clear();
            StoneCutter.Clear();
            //nobelHouseCount = 0;
            //int coalPitCount = 0;
            fuelSpots = 0;
            foodspots = 0;
            //int logisticsLevel = 0;
            //int waterReservoirs = 0;
            mineCount_iron = 0;
            mineCount_tin = 0;
            mineCount_copper = 0;
            mineCount_lead = 0;
            mineCount_silver = 0;
            mineCount_gold = 0;
            mineCount_mithril = 0;
            mineCount_sulfur = 0;
            mineCount_coal = 0;

            Rectangle2 emptyArea= Rectangle2.Zero;
            buildingPosition = new BuildingPosition();
            BuildingStructure buildingStructure = new BuildingStructure();

            IntVector2 cityHall = WP.ToSubTilePos_Centered(city.tilePos);
            FoodSpots_workupdate.Add(cityHall);
            StoragePoints_workupdate.Add(cityHall);

            //Cirkle outward from city to find resources
            for (int radius = 0; radius <= city.cityTileRadius; ++radius)
            {
                ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.tilePos, radius));

                while (cirkleLoop.Next())
                {
                    if (DssRef.world.tileBounds.IntersectTilePoint(cirkleLoop.Position))
                    {
                        var tile = DssRef.world.tileGrid.Get(cirkleLoop.Position);
                        if (tile.CityIndex == city.parentArrayIndex && tile.IsLand())
                        {
                            topleft = WP.ToSubTilePos_TopLeft(cirkleLoop.Position);
                            subTileLoop = new ForXYLoop(topleft, topleft + WorldData.TileSubDivitions_MaxIndex);

                            while (subTileLoop.Next())
                            {
                                SubTile subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);

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
                                                if (/*Trees.Count < workerCount &&*/
                                                    (foil == TerrainSubFoilType.DryWood || subTile.terrainAmount >= TerrainContent.TreeReadySize))
                                                {
                                                    ++fuelSpots;
                                                    Trees.Add(subTileLoop.Position);
                                                }
                                                break;

                                            case Map.TerrainSubFoilType.StoneBlock:
                                            case Map.TerrainSubFoilType.Stones:
                                                //if (Stones.Count < workerCount)
                                                //{
                                                    Stones.Add(subTileLoop.Position);
                                                //}
                                                break;

                                            case TerrainSubFoilType.WheatFarm:
                                            case TerrainSubFoilType.WheatFarmUpgraded:
                                                ++buildingStructure.WheatFarm_count;
                                                ++foodspots;
                                                farming(ref subTile);
                                                buildingPosition.WheatFarm_pos = subTileLoop.Position;
                                                break;

                                            case TerrainSubFoilType.LinenFarm:
                                            case TerrainSubFoilType.LinenFarmUpgraded:
                                                ++buildingStructure.LinenFarm_count;
                                                farming(ref subTile);
                                                buildingPosition.LinenFarm_pos = subTileLoop.Position;
                                                break;

                                            case TerrainSubFoilType.RapeSeedFarm:
                                            case TerrainSubFoilType.RapeSeedFarmUpgraded:
                                                ++buildingStructure.RapeSeedFarm_count;
                                                ++fuelSpots;
                                                farming(ref subTile);
                                                buildingPosition.RapeSeedFarm_pos = subTileLoop.Position;
                                                break;

                                            case TerrainSubFoilType.HempFarm:
                                            case TerrainSubFoilType.HempFarmUpgraded:
                                                ++buildingStructure.HempFarm_count;
                                                ++fuelSpots;
                                                farming(ref subTile);
                                                buildingPosition.HempFarm_pos = subTileLoop.Position;
                                                break;

                                            case TerrainSubFoilType.BogIron:
                                                if (BogIron.Count < workerCount)
                                                {
                                                    BogIron.Add(subTileLoop.Position);
                                                }
                                                break;
                                        }

                                        break;

                                    case TerrainMainType.Mine:
                                        Mines.Add(subTileLoop.Position);

                                        var mineType = (TerrainMineType)subTile.subTerrain;
                                        switch (mineType)
                                        {
                                            case TerrainMineType.Coal:
                                                ++mineCount_coal;
                                                break;
                                            case TerrainMineType.IronOre:
                                                ++mineCount_iron;
                                                break;
                                            case TerrainMineType.TinOre:
                                                ++mineCount_tin;
                                                break;
                                            case TerrainMineType.CopperOre:
                                                ++mineCount_copper;
                                                break;
                                            case TerrainMineType.Sulfur:
                                                ++mineCount_sulfur;
                                                break;
                                            case TerrainMineType.LeadOre:
                                                ++mineCount_lead;
                                                break;
                                            case TerrainMineType.SilverOre:
                                                ++mineCount_silver;
                                                break;
                                            case TerrainMineType.GoldOre:
                                                ++mineCount_gold;
                                                break;
                                            case TerrainMineType.Mithril:
                                                ++mineCount_mithril;
                                                break;
                                        }
                                        break;

                                    case TerrainMainType.Building:
                                        var building = (TerrainBuildingType)subTile.subTerrain;

                                        switch (building)
                                        {
                                            case TerrainBuildingType.WorkerHut:
                                                ++buildingStructure.WorkerHuts_count;

                                                buildingPosition.WorkerHuts_pos = subTileLoop.Position;
                                                break;

                                            case TerrainBuildingType.HenPen:
                                                ++buildingStructure.HenPen_count;
                                                ++foodspots;
                                                if (subTile.terrainAmount > TerrainContent.HenReady)
                                                {
                                                    AnimalPens.Add(subTileLoop.Position);
                                                }

                                                buildingPosition.HenPen_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.PigPen:
                                                ++buildingStructure.PigPen_count;
                                                ++foodspots;
                                                if (subTile.terrainAmount > TerrainContent.PigReady)
                                                {
                                                    AnimalPens.Add(subTileLoop.Position);
                                                }

                                                buildingPosition.PigPen_pos = subTileLoop.Position;
                                                break;

                                            case TerrainBuildingType.Postal:
                                            case TerrainBuildingType.PostalLevel2:
                                            case TerrainBuildingType.PostalLevel3:
                                                ++buildingStructure.Postal_count;
                                                buildingPosition.Postal_pos = subTileLoop.Position;
                                                break;

                                            case TerrainBuildingType.Recruitment:
                                            case TerrainBuildingType.RecruitmentLevel2:
                                            case TerrainBuildingType.RecruitmentLevel3:
                                                ++buildingStructure.Recruitment_count;
                                                buildingPosition.Recruitment_pos = subTileLoop.Position;
                                                break;

                                            case TerrainBuildingType.SoldierBarracks:
                                                ++buildingStructure.SoldierBarracks_count;
                                                buildingPosition.SoldierBarracks_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.ArcherBarracks:
                                                ++buildingStructure.ArcherBarracks_count;
                                                buildingPosition.ArcherBarracks_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.WarmashineBarracks:
                                                ++buildingStructure.WarmashineBarracks_count;
                                                buildingPosition.WarmashineBarracks_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.KnightsBarracks:
                                                ++buildingStructure.KnightsBarracks_count;
                                                buildingPosition.KnightsBarracks_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.GunBarracks:
                                                ++buildingStructure.GunBarracks_count;
                                                buildingPosition.GunBarracks_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.CannonBarracks:
                                                ++buildingStructure.CannonBarracks_count;
                                                buildingPosition.CannonBarracks_pos = subTileLoop.Position;
                                                break;

                                            case TerrainBuildingType.Tavern:
                                                FoodSpots_workupdate.Add(subTileLoop.Position);
                                                buildingPosition.Tavern_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.Storehouse:
                                                StoragePoints_workupdate.Add(subTileLoop.Position);
                                                buildingPosition.Storehouse_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.Carpenter:
                                                ++buildingStructure.Carpenter_count;
                                                CraftStation.Add(subTileLoop.Position);
                                                buildingPosition.Carpenter_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.Brewery:
                                                ++buildingStructure.Brewery_count;
                                                CraftStation.Add(subTileLoop.Position);
                                                buildingPosition.Brewery_pos = subTileLoop.Position;
                                                break;
                                            
                                            case TerrainBuildingType.Work_CoalPit:
                                                ++buildingStructure.CoalPit_count;
                                                CraftStation.Add(subTileLoop.Position);
                                                buildingPosition.CoalPit_pos = subTileLoop.Position;
                                                break;

                                            case TerrainBuildingType.Work_Cook:
                                                ++buildingStructure.Cook_count;
                                                CraftStation.Add(subTileLoop.Position);
                                                buildingPosition.Cook_pos = subTileLoop.Position;
                                                break;

                                            case TerrainBuildingType.Work_Bench:
                                                ++buildingStructure.WorkBench_count;
                                                CraftStation.Add(subTileLoop.Position);
                                                buildingPosition.WorkBench_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.Work_Smith:
                                                ++buildingStructure.Smith_count;
                                                CraftStation.Add(subTileLoop.Position);
                                                buildingPosition.Smith_pos = subTileLoop.Position;
                                                break;

                                            case TerrainBuildingType.Smelter:
                                                ++buildingStructure.Smelter_count;
                                                CraftStation.Add(subTileLoop.Position);
                                                buildingPosition.Smelter_pos = subTileLoop.Position;
                                                break;

                                            case TerrainBuildingType.Foundry:
                                                ++buildingStructure.Foundry_count;
                                                CraftStation.Add(subTileLoop.Position);
                                                buildingPosition.Foundry_pos = subTileLoop.Position;
                                                break;

                                            case TerrainBuildingType.Armory:
                                                ++buildingStructure.Armory_count;
                                                CraftStation.Add(subTileLoop.Position);
                                                buildingPosition.Armory_pos = subTileLoop.Position;
                                                break;

                                            case TerrainBuildingType.Chemist:
                                                ++buildingStructure.Chemist_count;
                                                CraftStation.Add(subTileLoop.Position);
                                                buildingPosition.Chemist_pos = subTileLoop.Position;
                                                break;

                                            case TerrainBuildingType.Gunmaker:
                                                ++buildingStructure.Gunmaker_count;
                                                CraftStation.Add(subTileLoop.Position);
                                                buildingPosition.Gunmaker_pos = subTileLoop.Position;
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
                                                ++buildingStructure.Nobelhouse_count;
                                                buildingPosition.Nobelhouse_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.Embassy:
                                                ++buildingStructure.Embassy_count;
                                                buildingPosition.Embassy_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.School:
                                                ++buildingStructure.School_count;
                                                buildingPosition.School_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.Logistics:
                                                buildingStructure.buildingLevel_logistics = subTile.terrainAmount;
                                                break;
                                            case TerrainBuildingType.WaterResovoir:
                                                ++buildingStructure.WaterResovoir_count;
                                                buildingPosition.WaterResovoir_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.GoldDeliveryLevel1:
                                            case TerrainBuildingType.GoldDeliveryLevel2:
                                            case TerrainBuildingType.GoldDeliveryLevel3:
                                                ++buildingStructure.GoldDelivery_count;
                                                buildingPosition.GoldDelivery_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.Bank:
                                                ++buildingStructure.Bank_count;
                                                buildingPosition.Bank_pos = subTileLoop.Position;
                                                break;
                                            case TerrainBuildingType.CoinMinter:
                                                ++buildingStructure.CoinMinter_count;
                                                CoinMinting.Add(subTileLoop.Position);
                                                buildingPosition.CoinMinter_pos = subTileLoop.Position;
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
                                                EmptyLand.Add(subTileLoop.Position);
                                                emptyArea.includeTileAndRadius(subTileLoop.Position, 3);
                                            }                                             
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            //Complete
            city.buildingStructure = buildingStructure;

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

        public bool MayAutoBuildHere(City city, IntVector2 subTilePos)
        {
            switch (DssRef.world.subTileGrid.Get(subTilePos).mainTerrain)
            {
                case TerrainMainType.Destroyed:
                case TerrainMainType.DefaultLand:
                    //{ 
                       return DssRef.world.tileGrid.Get(WP.SubtileToTilePos(subTilePos)).CityIndex == city.parentArrayIndex;
                    //}
                    //break;
            }

            return false;
        }

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
