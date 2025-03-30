using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        //static readonly string[] SquareCity = new string[]
        //    {
        //        "TWWWWWWT",
        //        "W______W",
        //        "W______W",
        //        "WrrrHrrW",
        //        "W__cXc_W",
        //        "W__crc_W",
        //        "W___r__W",
        //        "TWWWWWWT",

        //    };

        //static readonly string[] RoundSquareCity = new string[]
        //    {
        //        "  WWWW",
        //        " TW__WT ",
        //        "WW____WW",
        //        "WrrrHrrW",
        //        "W__cXc_W",
        //        "WW_crcWW",
        //        " TW_rWT ",
        //        "  WWWW  ",

        //    };

        //static readonly string[] SquareCity_Segmented = new string[]
        //   {
        //        "TWWWWWWT",
        //        "W___H__W",
        //        "W___X__W",
        //        "TWWWrWWT",
        //        "w__crc_w",
        //        "w__crc_w",
        //        "w___r__w",
        //        "wwwwrwww",

        //   };

        IntVector2 barracksReservedSpot;
        public IntVector2 cityStorageCenter;
        public void createBuildingSubtiles(WorldData world, CityTemplateCollection templateCollection)
        {
            List<IntVector2> emptyGeneral = new List<IntVector2>();
            Grid2D<CityTemplateCellType> template = templateCollection.getTemplate(this, world, out IntVector2 startSubTilePos);

            IntVector2 topleft = WP.ToSubTilePos_TopLeft(tilePos) + startSubTilePos;

            int tower;
            int gate = (int)TerrainWallType.StoneGate;
            int wall;
            int lowWall;
            int servicehouse;
            bool largeServiceHouse;
            int road;
            int centerHall;
            double percBuilding;
            double percWallGuard;
            bool large = false;

            int cityServiceCount;

            switch (this.cityType)
            {
                case CityType.Village:
                    tower = (int)TerrainWallType.DirtTower;
                    wall = (int)TerrainWallType.DirtWall;
                    lowWall = (int)TerrainWallType.DirtWall;
                    servicehouse = (int)TerrainBuildingType.ServiceMenHouse_small;
                    largeServiceHouse = false;
                    road = (int)TerrainDecorType.CobbleStones;
                    centerHall = (int)TerrainBuildingType.CityHall_Village;
                    percBuilding = 0.3;
                    percWallGuard = 0;
                    cityServiceCount = DssConst.VillageHall_RequiredStaff;
                    break;
                case CityType.Town:
                    tower = (int)TerrainWallType.WoodTower;
                    wall = (int)TerrainWallType.WoodWall;
                    lowWall = (int)TerrainWallType.DirtWall;
                    servicehouse = (int)TerrainBuildingType.ServiceMenHouse_small;
                    largeServiceHouse = false;
                    road = (int)TerrainDecorType.Square;
                    centerHall = (int)TerrainBuildingType.CityHall_Town;
                    percBuilding = 0.5;
                    percWallGuard = 0.1;
                    cityServiceCount = DssConst.TownHall_RequiredStaff;
                    break;
                default:
                    large = true;
                    tower = (int)TerrainWallType.StoneTower;
                    wall = (int)TerrainWallType.StoneWall;
                    lowWall = (int)TerrainWallType.WoodWall;
                    servicehouse = (int)TerrainBuildingType.ServiceMenHouse_Large;
                    largeServiceHouse = true;
                    road = (int)TerrainDecorType.Square;
                    centerHall = (int)TerrainBuildingType.CityHall_Capital;
                    percBuilding = 0.6;
                    percWallGuard = 0.25;
                    cityServiceCount = DssConst.CapitalHall_RequiredStaff;
                    break;

            }
            cityServiceCount += 1;

            //string[] template = SquareCity;
            List<TerrainBuildingType> craftStations = new List<TerrainBuildingType>
            {
                (large? TerrainBuildingType.GuardHouse_Large: TerrainBuildingType.GuardHouse_Small),
                TerrainBuildingType.Work_Bench,
                TerrainBuildingType.Work_Cook,
            };

            //IntVector2 templatePos = IntVector2.Zero;
            template.LoopBegin();
            while (template.LoopNext())
            {
            //for (templatePos.Y = 0; templatePos.Y < template.Length; templatePos.Y++)
            //{ 
            //    string row = template[templatePos.Y];
            //    for (templatePos.X = 0; templatePos.X < row.Length; templatePos.X++)
            //    {
                    TerrainMainType main = TerrainMainType.Building;
                    int sub = -1;
                    IntVector2 pos = topleft + template.LoopPosition;
                    var subTile = world.subTileGrid.Get(pos);

                    switch (template.LoopValueGet())
                    {
                        case CityTemplateCellType.General:
                            if (world.rnd.Chance(percBuilding))
                            {
                                sub = servicehouse;
                                onServiceHouseBuild(true, largeServiceHouse);
                            }
                            else
                            {
                                main = TerrainMainType.Decor;
                                sub = road;
                                emptyGeneral.Add(pos);
                            }
                            break;
                        case  CityTemplateCellType.Wall:
                            {
                                DefenceStatus defence = new DefenceStatus();
                                main = TerrainMainType.Wall;
                                sub = wall;
                                defence.autoAssign = world.rnd.Chance(percWallGuard);

                                defence.init(pos);
                                defenceBuildings.Add(defence);
                            }
                            break;
                        case  CityTemplateCellType.OuterWall:
                            {
                                DefenceStatus defence = new DefenceStatus();
                                main = TerrainMainType.Wall;
                                sub = lowWall;
                                defence.autoAssign = world.rnd.Chance(percWallGuard);

                                defence.init(pos);
                                defenceBuildings.Add(defence);
                            }
                            break;
                        case  CityTemplateCellType.Tower:
                            {
                                DefenceStatus defence = new DefenceStatus();
                                main = TerrainMainType.Wall;
                                sub = tower;
                                defence.autoAssign = true;

                                defence.init(pos);
                                defenceBuildings.Add(defence);
                            }
                            break;
                        case CityTemplateCellType.Gate:
                            {
                                //DefenceStatus defence = new DefenceStatus();
                                //main = TerrainMainType.Wall;
                                //sub = gate;
                                //defence.autoAssign = false;

                                //defence.init(pos);
                                //defenceBuildings.Add(defence);
                                main = TerrainMainType.Decor;
                                sub = road;
                            }
                            break;
                    case CityTemplateCellType.Road:
                            main = TerrainMainType.Decor;
                            sub = road;
                            break;

                        case CityTemplateCellType.CraftArea:
                            if (craftStations.Count > 0)
                            {
                                sub = (int)arraylib.RandomListMemberPop(craftStations, DssRef.world.rnd);
                            }
                            else
                            {
                                main = TerrainMainType.Decor;
                                sub = road;
                                barracksReservedSpot = pos;
                            }                            
                            break;
                        case  CityTemplateCellType.CityHall:
                            sub = centerHall;
                            cityHallSubtilePos = pos;
                            break;
                        case  CityTemplateCellType.CityCenterSquare:
                            main = TerrainMainType.Decor;
                            sub = (int)TerrainDecorType.Square;
                            cityStorageCenter = pos;
                            break;
                    }

                    if (sub >= 0)
                    {
                        subTile.SetType(main, sub, 1);
                        world.subTileGrid.Set(pos, subTile);
                    }
                
            }

            while (totalServiceMen < cityServiceCount && emptyGeneral.Count > 0)
            {
                var pos = arraylib.RandomListMemberPop(emptyGeneral, world.rnd);
                var subTile = world.subTileGrid.Get(pos);
                subTile.SetType(TerrainMainType.Building, servicehouse, 1);
                world.subTileGrid.Set(pos, subTile);
                onServiceHouseBuild(true, largeServiceHouse);
            }
            //for (int y = 0; y < WorldData.TileSubDivitions; ++y)
            //{
            //    for (int x = 0; x < WorldData.TileSubDivitions; ++x)
            //    {
            //        IntVector2 pos = topleft;
            //        pos.X += x;
            //        pos.Y += y;
            //        var subTile = world.subTileGrid.Get(pos);

            //        TerrainMainType main = TerrainMainType.Building;
            //        int sub;

            //        bool edgeX = x == 0 || x == WorldData.TileSubDivitions_MaxIndex;
            //        bool edgeY = y == 0 || y == WorldData.TileSubDivitions_MaxIndex;

            //        if (edgeX || edgeY)
            //        {
            //            DefenceStatus defence = new DefenceStatus();
            //            main = TerrainMainType.Wall;
            //            if (edgeX && edgeY)
            //            {
            //                sub = tower;
            //                defence.autoAssign = true;
            //            }
            //            else
            //            {
            //                sub = wall;
            //                defence.autoAssign = world.rnd.Chance(percWallGuard);
            //            }
            //            defence.init(pos);
            //            defenceBuildings.Add(defence);

            //        }
            //        else if (x == 4 && y == 3)
            //        {
            //            sub = centerHall;
            //            cityHallSubtilePos = pos;
            //        }
            //        else if (x == 4 && y == 4)
            //        {
            //            main = TerrainMainType.Decor;
            //            sub = (int)TerrainDecorType.Square;
            //        }
            //        else if (x == 3 && y == 4)
            //        {
            //            sub = (int)TerrainBuildingType.Work_Cook;
            //        }
            //        else if (x == 5 && y == 4)
            //        {
            //            sub = (int)TerrainBuildingType.Work_Bench;
            //        }
            //        else if (x == 1 && y ==1)
            //        {
                       
            //            sub = (int)(large? TerrainBuildingType.GuardHouse_Large: TerrainBuildingType.GuardHouse_Small);
            //            onGuardHouseBuild(true, large); 
            //        }
            //        else
            //        {
            //            if (world.rnd.Chance(percBuilding))
            //            {
            //                sub = servicehouse;
            //                onServiceHouseBuild(true, largeServiceHouse);
            //            }
            //            else
            //            {
            //                main = TerrainMainType.Decor;
            //                sub = road;
            //            }
            //        }

            //        subTile.SetType(main, sub, 1);
            //        world.subTileGrid.Set(pos, subTile);
            //    }
            //}
        }

    }
}
