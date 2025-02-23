﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.ToGG.ToggEngine.Map;

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
        public List<IntVector2> EmptyLand = new List<IntVector2>(2);
        public List<IntVector2> ResourceOnGround = new List<IntVector2>(20);
        int nobelHouseCount = 0;
        public int fuelSpots = 0;
        public int foodspots = 0;
        //public int logisticsLevel = 0;
        public bool newCity = true;

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
            EmptyLand.Clear();
            ResourceOnGround.Clear();
            nobelHouseCount = 0;
            int coalPitCount = 0;
            fuelSpots = 0;
            foodspots = 0;
            int logisticsLevel = 0;

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
                                                if (Trees.Count < workerCount &&
                                                    (foil == TerrainSubFoilType.DryWood || subTile.terrainAmount >= TerrainContent.TreeReadySize))
                                                {
                                                    ++fuelSpots;
                                                    Trees.Add(subTileLoop.Position);
                                                }
                                                break;

                                            case Map.TerrainSubFoilType.StoneBlock:
                                            case Map.TerrainSubFoilType.Stones:
                                                if (Stones.Count < workerCount)
                                                {
                                                    Stones.Add(subTileLoop.Position);
                                                }
                                                break;

                                            case TerrainSubFoilType.WheatFarm:
                                                ++foodspots;
                                                farming(ref subTile);
                                                break;

                                            case TerrainSubFoilType.LinenFarm:
                                                farming(ref subTile);
                                                break;

                                            case TerrainSubFoilType.RapeSeedFarm:
                                            case TerrainSubFoilType.HempFarm:
                                                ++fuelSpots;
                                                farming(ref subTile);
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
                                        break;

                                    case TerrainMainType.Building:
                                        var building = (TerrainBuildingType)subTile.subTerrain;

                                        switch (building)
                                        {
                                            case TerrainBuildingType.HenPen:
                                                ++foodspots;
                                                if (subTile.terrainAmount > TerrainContent.HenReady)
                                                { 
                                                    AnimalPens.Add(subTileLoop.Position);
                                                }
                                                break;
                                            case TerrainBuildingType.PigPen:
                                                ++foodspots;
                                                if (subTile.terrainAmount > TerrainContent.PigReady)
                                                {
                                                    AnimalPens.Add(subTileLoop.Position);
                                                }
                                                break;

                                            case TerrainBuildingType.Tavern:
                                                FoodSpots_workupdate.Add(subTileLoop.Position);
                                                break;
                                            case TerrainBuildingType.Storehouse:
                                                StoragePoints_workupdate.Add(subTileLoop.Position);
                                                break;
                                            case TerrainBuildingType.Carpenter:
                                                city.hasBuilding_carpenter = true;
                                                CraftStation.Add(subTileLoop.Position);
                                                break;
                                            case TerrainBuildingType.Brewery:
                                                city.hasBuilding_brewery = true;
                                                CraftStation.Add(subTileLoop.Position);
                                                break;
                                            
                                            case TerrainBuildingType.Work_CoalPit:
                                                ++coalPitCount;
                                                CraftStation.Add(subTileLoop.Position);
                                                break;

                                            case TerrainBuildingType.Work_Cook:
                                            case TerrainBuildingType.Work_Bench:
                                                CraftStation.Add(subTileLoop.Position);
                                                break;
                                            case TerrainBuildingType.Work_Smith:
                                                city.hasBuilding_smith = true;
                                                CraftStation.Add(subTileLoop.Position);
                                                break;
                                            case TerrainBuildingType.Nobelhouse:
                                                ++nobelHouseCount;
                                                break;
                                            case TerrainBuildingType.Logistics:
                                                logisticsLevel = subTile.terrainAmount;
                                                break;
                                        }
                                        break;
                                    case TerrainMainType.Destroyed:
                                    case TerrainMainType.DefaultLand:
                                        if (emptyLandExpansions > 0)
                                        {
                                            --emptyLandExpansions;

                                            EmptyLand.Add(subTileLoop.Position);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            //Complete
            city.buildingCount_nobelHouse = nobelHouseCount;
            city.buildingCount_coalpit = coalPitCount;
            city.buildingLevel_logistics = logisticsLevel;

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
