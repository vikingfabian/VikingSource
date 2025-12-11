using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {        
        IntVector2 barracksReservedSpot;
        public IntVector2 citySquareSubtilePos;


        public void createCampSite(IntVector2 subtilepos)
        {
            cityHallSubtilePos = subtilepos;
            EditSubTile edit = new EditSubTile(subtilepos, new SubTile(TerrainMainType.Building, (int)TerrainBuildingType.CityHall_Tent), true, true, false);
           edit.SubmitOrExecute();

            Span<Dir4> testDirs = stackalloc Dir4[] { Dir4.S, Dir4.E, Dir4.W, Dir4.N };

            foreach (var dir in testDirs)
            {
                IntVector2 pos = cityHallSubtilePos + IntVector2.Dir4Array[(int)dir];
                if (DssRef.world.subTileGrid.TryGet(pos, out var tile))
                {
                    if (!tile.IsWater())
                    {
                        citySquareSubtilePos = pos;
                        new EditSubTile(pos, Build.BuildLib.BuildOptions[(int)BuildAndExpandType.CitySquare].terrainType, true, true, false).SubmitOrExecute();
                        break;
                    }
                }
            }

            foreach (var dir in IntVector2.Dir8Array)
            {
                IntVector2 pos = cityHallSubtilePos + dir * 2;
                if (DssRef.world.subTileGrid.TryGet(pos, out var tile))
                {
                    if (!tile.IsWater())
                    {
                        new EditSubTile(pos, Build.BuildLib.BuildOptions[(int)BuildAndExpandType.WorkerTent].terrainType, true, true, false).SubmitOrExecute();
                        break;
                    }
                }
            }
        }

        public void createBuildingSubtiles(WorldData world, CityTemplateCollection templateCollection)
        {
            if (cityType == CityType.UnClaimed)
            {
                var pos = WP.ToSubTilePos_Centered(tilePos);
                var subTile = world.subTileGrid.Get(pos);
                subTile.SetType(TerrainMainType.Building, (int)TerrainBuildingType.CityHall_Unclaimed, 1);
                world.subTileGrid.Set(pos, subTile);
            }
            else if (cityType == CityType.Campsite)
            {
                PcgRandom rnd = new PcgRandom(world.metaData.seed * myIndex);

                var subtile = WP.ToSubTilePos_Centered(tilePos);
                subtile.X += rnd.Plus_Minus(3);
                subtile.Y += rnd.Plus_Minus(3);

                createCampSite(subtile);
            }
            else
            {
                PcgRandom rnd = new PcgRandom(world.metaData.seed * myIndex);

                List<IntVector2> emptyGeneral = new List<IntVector2>();
                Grid2D<CityTemplateCellType> template = templateCollection.getTemplate(this, world, out IntVector2 startSubTilePos);

                IntVector2 topleft = WP.ToSubTilePos_TopLeft(tilePos) + startSubTilePos;

                int tower;
                //int gate = (int)TerrainWallType.StoneGate;
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

                freeServiceMen.amount -= cityServiceCount;

                //cityServiceCount += 1;

                //string[] template = SquareCity;
                List<TerrainBuildingType> craftStations = new List<TerrainBuildingType>
            {
                (large? TerrainBuildingType.GuardHouse_Large: TerrainBuildingType.GuardHouse_Small),
                TerrainBuildingType.Work_Bench,
                TerrainBuildingType.Work_Cook,
            };

                //IntVector2 templatePos = IntVector2.Zero;
                var templateLoop = template.LoopInstance();
                while (templateLoop.Next())
                {
                    TerrainMainType main = TerrainMainType.Building;
                    int sub = -1;
                    IntVector2 pos = topleft + templateLoop.Position;
                    var subTile = world.subTileGrid.Get(pos);

                    switch (template.Get(templateLoop.Position))
                    {
                        case CityTemplateCellType.General:
                            if (rnd.Chance(percBuilding))
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
                        case CityTemplateCellType.Wall:
                            {
                                DefenceStatus defence = new DefenceStatus();
                                main = TerrainMainType.Wall;
                                sub = wall;
                                defence.autoAssign = rnd.Chance(percWallGuard);

                                defence.init(pos, false);
                                defenceBuildings.Add(defence);
                            }
                            break;
                        case CityTemplateCellType.OuterWall:
                            {
                                DefenceStatus defence = new DefenceStatus();
                                main = TerrainMainType.Wall;
                                sub = lowWall;
                                defence.autoAssign = rnd.Chance(percWallGuard);

                                defence.init(pos, false);
                                defenceBuildings.Add(defence);
                            }
                            break;
                        case CityTemplateCellType.Tower:
                            {
                                DefenceStatus defence = new DefenceStatus();
                                main = TerrainMainType.Wall;
                                sub = tower;
                                defence.autoAssign = true;

                                defence.init(pos, true);
                                defenceBuildings.Add(defence);
                            }
                            break;
                        case CityTemplateCellType.Gate:
                            {
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
                                var building = arraylib.RandomListMemberPop(craftStations, rnd);
                                sub = (int)building;
                                switch (building)
                                {
                                    case TerrainBuildingType.GuardHouse_Small:
                                        onGuardHouseBuild(true, false);
                                        break;
                                    case TerrainBuildingType.GuardHouse_Large:
                                        onGuardHouseBuild(true, true);
                                        break;
                                }
                            }
                            else
                            {
                                main = TerrainMainType.Decor;
                                sub = road;
                                barracksReservedSpot = pos;
                            }
                            break;
                        case CityTemplateCellType.CityHall:
                            sub = centerHall;
                            cityHallSubtilePos = pos;
                            break;
                        case CityTemplateCellType.CityCenterSquare:
                            main = TerrainMainType.Decor;
                            sub = (int)TerrainDecorType.Square;
                            citySquareSubtilePos = pos;
                            break;
                    }

                    if (sub >= 0)
                    {
                        subTile.SetType(main, sub, 1);
                        world.subTileGrid.Set(pos, subTile);
                    }

                }

                int total = workingAndFreeServiceMen;

                while (freeServiceMen.amount < 1 && emptyGeneral.Count > 0)
                {
                    var pos = arraylib.RandomListMemberPop(emptyGeneral, rnd);
                    var subTile = world.subTileGrid.Get(pos);
                    subTile.SetType(TerrainMainType.Building, servicehouse, 1);
                    world.subTileGrid.Set(pos, subTile);
                    onServiceHouseBuild(true, largeServiceHouse);
                }

            }
        }

    }
}
