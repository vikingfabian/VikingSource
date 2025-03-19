using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        public void createBuildingSubtiles(WorldData world)
        {
            IntVector2 topleft = WP.ToSubTilePos_TopLeft(tilePos);

            int tower;
            int wall;
            int servicehouse;
            bool largeServiceHouse;
            int road;
            int centerHall;
            double percBuilding;
            double percWallGuard;

            switch (this.cityType)
            {
                case CityType.Village:
                    tower = (int)TerrainWallType.DirtTower;
                    wall = (int)TerrainWallType.DirtWall;
                    servicehouse = (int)TerrainBuildingType.ServiceMenHouse_small;
                    largeServiceHouse = false;
                    road = (int)TerrainDecorType.CobbleStones;
                    centerHall = (int)TerrainBuildingType.CityHall_Village;
                    percBuilding = 0.3;
                    percWallGuard = 0;
                    break;
                case CityType.Town:
                    tower = (int)TerrainWallType.WoodTower;
                    wall = (int)TerrainWallType.WoodWall;
                    servicehouse = (int)TerrainBuildingType.ServiceMenHouse_small;
                    largeServiceHouse = false;
                    road = (int)TerrainDecorType.Square;
                    centerHall = (int)TerrainBuildingType.CityHall_Town;
                    percBuilding = 0.5;
                    percWallGuard = 0.1;
                    break;
                default:
                    tower = (int)TerrainWallType.StoneTower;
                    wall = (int)TerrainWallType.StoneWall;
                    servicehouse = (int)TerrainBuildingType.ServiceMenHouse_Large;
                    largeServiceHouse = true;
                    road = (int)TerrainDecorType.Square;
                    centerHall = (int)TerrainBuildingType.CityHall_Capital;
                    percBuilding = 0.6;
                    percWallGuard = 0.25;
                    break;

            }

            for (int y = 0; y < WorldData.TileSubDivitions; ++y)
            {
                for (int x = 0; x < WorldData.TileSubDivitions; ++x)
                {
                    IntVector2 pos = topleft;
                    pos.X += x;
                    pos.Y += y;
                    var subTile = world.subTileGrid.Get(pos);

                    TerrainMainType main = TerrainMainType.Building;
                    int sub;

                    bool edgeX = x == 0 || x == WorldData.TileSubDivitions_MaxIndex;
                    bool edgeY = y == 0 || y == WorldData.TileSubDivitions_MaxIndex;

                    if (edgeX || edgeY)
                    {
                        DefenceStatus defence = new DefenceStatus();
                        main = TerrainMainType.Wall;
                        if (edgeX && edgeY)
                        {
                            sub = tower;
                            defence.autoAssign = true;
                        }
                        else
                        {
                            sub = wall;
                            defence.autoAssign = world.rnd.Chance(percWallGuard);
                        }
                        defence.init(pos);
                        defenceBuildings.Add(defence);

                    }
                    else if (x == 4 && y == 3)
                    {
                        sub = centerHall;
                        cityHallSubtilePos = pos;
                    }
                    else if (x == 4 && y == 4)
                    {
                        main = TerrainMainType.Decor;
                        sub = (int)TerrainDecorType.Square;
                    }
                    else if (x == 3 && y == 4)
                    {
                        sub = (int)TerrainBuildingType.Work_Cook;
                    }
                    else if (x == 5 && y == 4)
                    {
                        sub = (int)TerrainBuildingType.Work_Bench;
                    }
                    else if (x == 1 && y ==1)
                    {
                        bool large = cityType == CityType.Capital;
                        sub = (int)(large? TerrainBuildingType.GuardHouse_Large: TerrainBuildingType.GuardHouse_Small);
                        onGuardHouseBuild(true, large); 
                    }
                    else
                    {
                        if (world.rnd.Chance(percBuilding))
                        {
                            sub = servicehouse;
                            onServiceHouseBuild(true, largeServiceHouse);
                        }
                        else
                        {
                            main = TerrainMainType.Decor;
                            sub = road;
                        }
                    }

                    subTile.SetType(main, sub, 1);
                    world.subTileGrid.Set(pos, subTile);
                }
            }
        }

    }
}
