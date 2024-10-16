using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameObject.Worker;

namespace VikingEngine.DSSWars.Map
{
    class CityStructure
    {
        public static readonly CityStructure Singleton = new CityStructure();

        public List<IntVector2> FoodSpots_workupdate = new List<IntVector2>(4);
        public List<IntVector2> Trees = new List<IntVector2>(20);
        public List<IntVector2> Stones = new List<IntVector2>(20);
        public List<IntVector2> FarmPlant = new List<IntVector2>(20);
        public List<IntVector2> FarmGather = new List<IntVector2>(20);
        public List<IntVector2> AnimalPens = new List<IntVector2>(20);
        public List<IntVector2> Mines = new List<IntVector2>(20);
        public List<IntVector2> CraftStation = new List<IntVector2>(20);
        public List<IntVector2> EmptyLand = new List<IntVector2>(2);
        public List<IntVector2> ResourceOnGround = new List<IntVector2>(20);
        int nobelHouseCount = 0;

        public bool newCity = true;

        public void setupTutorialMap(City city)
        {
            IntVector2 topleft;
            ForXYLoop subTileLoop;

            int wood = 2;
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

        public void update(City city, int workerCount)
        {
            int emptyLandExpansions = 2;

            IntVector2 topleft;
            ForXYLoop subTileLoop;
            FoodSpots_workupdate.Clear();
            Trees.Clear();
            Stones.Clear();
            FarmPlant.Clear();
            FarmGather.Clear();
            AnimalPens.Clear();
            Mines.Clear();
            CraftStation.Clear();
            EmptyLand.Clear();
            ResourceOnGround.Clear();
            nobelHouseCount = 0;

            FoodSpots_workupdate.Add(WP.ToSubTilePos_Centered(city.tilePos));

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
                                var subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);

                                if (subTile.collectionPointer >= 0)
                                {
                                    if (ResourceOnGround.Count < workerCount)
                                    { 
                                        ResourceOnGround.Add(subTileLoop.Position);
                                    }
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
                                                    Trees.Add(subTileLoop.Position);
                                                }
                                                break;

                                            case Map.TerrainSubFoilType.StoneBlock:
                                            case Map.TerrainSubFoilType.Stones:
                                                if (Trees.Count < workerCount)
                                                {
                                                    Stones.Add(subTileLoop.Position);
                                                }
                                                break;

                                            case TerrainSubFoilType.WheatFarm:                                                
                                            case TerrainSubFoilType.LinenFarm:
                                                if (subTile.terrainAmount == TerrainContent.FarmCulture_Empty)
                                                {
                                                    FarmPlant.Add(subTileLoop.Position);
                                                }
                                                else if (subTile.terrainAmount >= TerrainContent.FarmCulture_ReadySize)
                                                {
                                                    FarmGather.Add(subTileLoop.Position);
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
                                                if (subTile.terrainAmount > TerrainContent.HenReady)
                                                { 
                                                    AnimalPens.Add(subTileLoop.Position);
                                                }
                                                break;
                                            case TerrainBuildingType.PigPen:
                                                if (subTile.terrainAmount > TerrainContent.PigReady)
                                                {
                                                    AnimalPens.Add(subTileLoop.Position);
                                                }
                                                break;

                                            case TerrainBuildingType.Tavern:
                                                FoodSpots_workupdate.Add(subTileLoop.Position);
                                                break;
                                            case TerrainBuildingType.Carpenter:
                                                city.hasBuilding_carpenter = true;
                                                CraftStation.Add(subTileLoop.Position);
                                                break;
                                            case TerrainBuildingType.Brewery:
                                                city.hasBuilding_brewery = true;
                                                CraftStation.Add(subTileLoop.Position);
                                                break;
                                            case TerrainBuildingType.Work_Cook:
                                            case TerrainBuildingType.Work_Bench:
                                            case TerrainBuildingType.Work_CoalPit:
                                                CraftStation.Add(subTileLoop.Position);
                                                break;
                                            case TerrainBuildingType.Work_Smith:
                                                city.hasBuilding_smith = true;
                                                CraftStation.Add(subTileLoop.Position);
                                                break;
                                            case TerrainBuildingType.Nobelhouse:
                                                ++nobelHouseCount;
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
            city.nobelHouse_buildingCount = nobelHouseCount;
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
    }
}
