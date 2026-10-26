using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.MapProcess;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.LootFest.Map;
using VikingEngine.PJ.Moba.GO;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.GameObject
{

    struct CityTypeSetup
    {
        public int tower;
        public int wall;
        public int lowWall;
        public int servicehouse;
        public bool largeServiceHouse;
        public int road;
        public int centerHall;
        public double percBuilding;
        public double percWallGuard;
        public bool large = false;

        public int cityServiceCount;

        public CityTypeSetup(CityType cityType)
        {
            switch (cityType)
            {
                case CityType.Village:
                    tower = (int)TerrainWallType.DirtTower;
                    wall = (int)TerrainWallType.DirtWall;
                    lowWall = (int)TerrainWallType.DirtWall;
                    servicehouse = (int)TerrainBuildingType.ServiceMenHouse_Small;
                    largeServiceHouse = false;
                    road = (int)TerrainRoadType.CobblestoneRoad;
                    centerHall = (int)TerrainBuildingType.CityHall_Village;
                    percBuilding = 0.15;
                    percWallGuard = 0;
                    cityServiceCount = DssConst.VillageHall_RequiredStaff;
                    break;
                case CityType.Town:
                    tower = (int)TerrainWallType.WoodTower;
                    wall = (int)TerrainWallType.WoodWall;
                    lowWall = (int)TerrainWallType.DirtWall;
                    servicehouse = (int)TerrainBuildingType.ServiceMenHouse_Small;
                    largeServiceHouse = false;
                    road = (int)TerrainRoadType.Square;
                    centerHall = (int)TerrainBuildingType.CityHall_Town;
                    percBuilding = 0.15;
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
                    road = (int)TerrainRoadType.Square;
                    centerHall = (int)TerrainBuildingType.CityHall_Capital;
                    percBuilding = 0.15;
                    percWallGuard = 0.25;
                    cityServiceCount = DssConst.CapitalHall_RequiredStaff;
                    break;

            }
        }
    }

    partial class City
    {        
        IntVector2 barracksReservedSpot;
        public IntVector2 citySquareSubtilePos;

        public void createCampSite(IntVector2 subtilepos)
        {
            maptilePos = subtilepos;
            var localPlayer = pfaction.TryGetPlayer(out var p) && p.IsLocalPlayer();
            EditSubTile edit = new EditSubTile(true, localPlayer, subtilepos, new MapTile1_1(TerrainMainType.Building, (int)TerrainBuildingType.CityHall_Tent), true, true, false) { netShare = true };
           
            edit.SubmitOrExecute();

            Span<Dir4> testDirs = stackalloc Dir4[] { Dir4.S, Dir4.E, Dir4.W, Dir4.N };

            foreach (var dir in testDirs)
            {
                IntVector2 pos = maptilePos + IntVector2.Dir4Array[(int)dir];
                if (DssRef.world.subTileGrid.TryGet(pos, out var tile))
                {
                    if (!tile.IsWater())
                    {
                        citySquareSubtilePos = pos;
                        new EditSubTile(true, localPlayer, pos, Build.BuildLib.BuildOptions[(int)BuildAndExpandType.CitySquare].terrainType, true, true, false) { netShare = true }.SubmitOrExecute();
                        break;
                    }
                }
            }

            foreach (var dir in IntVector2.Dir8Array)
            {
                IntVector2 pos = maptilePos + dir * 2;
                if (DssRef.world.subTileGrid.TryGet(pos, out var tile))
                {
                    if (!tile.IsWater())
                    {
                        new EditSubTile(true, localPlayer, pos, Build.BuildLib.BuildOptions[(int)BuildAndExpandType.WorkerTent].terrainType, true, true, false) { netShare = true }.SubmitOrExecute();
                        break;
                    }
                }
            }
        }

        public void createHallSubtiles(WorldData world)
        {
            CityTypeSetup setup = new CityTypeSetup(this.cityType);
            freeServiceMen.amount -= setup.cityServiceCount;

            {
                EditSubTile hallTile = new EditSubTile(IsNetHosted, false, maptilePos,
                    new MapTile1_1(TerrainMainType.Building, setup.centerHall), true, false, false);
                hallTile.SubmitOrExecute();
            }
            {
                citySquareSubtilePos = VectorExt.AddY(maptilePos, 1);
                EditSubTile squareTile = new EditSubTile(IsNetHosted, false, citySquareSubtilePos,
                    new MapTile1_1(TerrainMainType.Road, (int)TerrainRoadType.Square), true, false, false);
                squareTile.SubmitOrExecute();
            }
        }

        public void createBuildingSubtiles(WorldData world, CityTemplateCollection templateCollection)
        {
            //if (cityType == CityType.UnClaimed)
            //{
            //    //maptilePos = WP.ToSubTilePos_Centered(maptilePos);
            //    var subTile = world.subTileGrid.Get(maptilePos);
            //    subTile.SetType(TerrainMainType.Building, (int)TerrainBuildingType.CityHall_Unclaimed, 1);
            //    world.subTileGrid.Set(maptilePos, subTile);
            //}
            //else 
            if (cityType == CityType.Campsite)
            {
                PcgRandom rnd = new PcgRandom(world.metaData.worldId.seed * myIndex);

                var subtile = /*WP.ToSubTilePos_Centered(*/maptilePos/*)*/;
                subtile.X += rnd.Plus_Minus(3);
                subtile.Y += rnd.Plus_Minus(3);

                createCampSite(subtile);
            }
            else
            {
                PcgRandom rnd = new PcgRandom(world.metaData.worldId.seed * myIndex);

                List<IntVector2> emptyGeneral = new List<IntVector2>();
                Grid2D<CityTemplateCellType> template = templateCollection.getTemplate(this, world, maptilePos, out IntVector2 topleft);

                CityTypeSetup setup = new CityTypeSetup(this.cityType);
              
            List<TerrainBuildingType> craftStations = new List<TerrainBuildingType>
            {
                (setup.large? TerrainBuildingType.GuardHouse_Large: TerrainBuildingType.GuardHouse_Small),
                TerrainBuildingType.Work_Bench,
                //TerrainBuildingType.Work_Cook,
            };

                //IntVector2 templatePos = IntVector2.Zero;
                var templateLoop = template.LoopInstance();
                while (templateLoop.Next())
                {
                    TerrainMainType main = TerrainMainType.Building;
                    int sub = -1;
                    IntVector2 pos = topleft + templateLoop.Position;
                    if (world.subTileGrid.TryGet(pos, out var subTile))
                    {

                        switch (template.Get(templateLoop.Position))
                        {
                            case CityTemplateCellType.General:
                                if (rnd.Chance(setup.percBuilding))
                                {
                                    sub = setup.servicehouse;
                                    onServiceHouseBuild(true, setup.largeServiceHouse);
                                }
                                else
                                {
                                    main = TerrainMainType.Road;
                                    sub = setup.road;
                                    emptyGeneral.Add(pos);
                                }
                                break;
                            case CityTemplateCellType.Wall:
                                {
                                    DefenceStatus defence = new DefenceStatus();
                                    main = TerrainMainType.Wall;
                                    sub = setup.wall;
                                    defence.autoAssign = rnd.Chance(setup.percWallGuard);

                                    defence.init(pos, false);
                                    defenceBuildings.Add(defence);
                                }
                                break;
                            case CityTemplateCellType.OuterWall:
                                {
                                    DefenceStatus defence = new DefenceStatus();
                                    main = TerrainMainType.Wall;
                                    sub = setup.lowWall;
                                    defence.autoAssign = rnd.Chance(setup.percWallGuard);

                                    defence.init(pos, false);
                                    defenceBuildings.Add(defence);
                                }
                                break;
                            case CityTemplateCellType.Tower:
                                {
                                    DefenceStatus defence = new DefenceStatus();
                                    main = TerrainMainType.Wall;
                                    sub = setup.tower;
                                    defence.autoAssign = true;

                                    defence.init(pos, true);
                                    defenceBuildings.Add(defence);
                                }
                                break;
                            case CityTemplateCellType.Gate:
                                {
                                    main = TerrainMainType.Road;
                                    sub = setup.road;
                                }
                                break;
                            case CityTemplateCellType.Road:
                                main = TerrainMainType.Road;
                                sub = setup.road;
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
                                    sub = setup.road;
                                    barracksReservedSpot = pos;
                                }
                                break;
                            case CityTemplateCellType.CityHall:
                                sub = setup.centerHall;
                                maptilePos = pos;
                                break;
                            case CityTemplateCellType.CityCenterSquare:
                                main = TerrainMainType.Road;
                                sub = (int)TerrainRoadType.Square;
                                citySquareSubtilePos = pos;
                                break;
                        }

                        if (sub >= 0)
                        {
                            subTile.SetType(main, sub, 1);
                            world.subTileGrid.Set(pos, subTile);
                        }

                    }
                }

                freeServiceMen.amount -= setup.cityServiceCount;
                //int total = workingAndFreeServiceMen;

                while (freeServiceMen.amount < 1 && emptyGeneral.Count > 0)
                {
                    var pos = arraylib.RandomListMemberPop(emptyGeneral, rnd);
                    var subTile = world.subTileGrid.Get(pos);
                    subTile.SetType(TerrainMainType.Building, setup.servicehouse, 1);
                    world.subTileGrid.Set(pos, subTile);
                    onServiceHouseBuild(true, setup.largeServiceHouse);
                }

            }
        }


        public void cleanOutCityTerrain()
        {
            const int Radius = 20;
            const int CleanRadius = Radius - 8;

            ForXYLoop loop = new ForXYLoop(Rectangle2.FromCenterTileAndRadius(maptilePos, Radius));
            while (loop.Next())
            {
                float r = (maptilePos - loop.Position).Length();
                if (r < CleanRadius || (r < Radius && Ref.rnd.Chance(0.6)))
                {
                    ref var tile = ref DssRef.world.subTileGrid.GetRef(loop.Position);
                    tile.SetType(TerrainMainType.Destroyed, 0, 1);
                }

            }
        }
    }
}
