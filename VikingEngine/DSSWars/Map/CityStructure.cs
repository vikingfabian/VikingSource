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

        List<IntVector2> FoodSpots_workupdate = new List<IntVector2>(4);

        public bool newCity = true;

        public void updateIfNew(City city)
        {
            if (newCity)
            {
                newCity = false;

                update(city);
            }
        }

        public void update(City city)
        {
            IntVector2 topleft;
            ForXYLoop subTileLoop;
            FoodSpots_workupdate.Clear();

            FoodSpots_workupdate.Add(WP.ToSubTilePos_Centered(city.tilePos));

            //Cirkle outward from city to find resources
            for (int radius = 1; radius <= city.cityTileRadius; ++radius)
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
                                    case TerrainMainType.Building:
                                        var building = (TerrainBuildingType)subTile.subTerrain;

                                        switch (building)
                                        {
                                            case TerrainBuildingType.Tavern:
                                                FoodSpots_workupdate.Add(subTileLoop.Position);
                                                break;
                                            case TerrainBuildingType.Carpenter:
                                                city.hasBuilding_carpenter = true;
                                                break;
                                        }
                                        break;
                                }
                            }
                        }
                    }
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
    }
}
