using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.MapProcess;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Build
{
    static class BuildTool
    {
        public static void Demolish(City city, IntVector2 subTilePos, bool editor)
        {
            var subTile = DssRef.world.subTileGrid.Get(subTilePos);
            var buildingType = BuildLib.GetType(subTile.mainTerrain, subTile.subTerrain);
            if (buildingType != BuildAndExpandType.NUM_NONE)
            {
                var opt =   BuildLib.BuildOptions[(int)buildingType];
                opt.destroy_async(city, subTilePos);

                bool addRubble = false;
                if (!editor)
                {
                    var bp = opt.blueprint;
                    foreach (var r in bp.resources)
                    {
                        if (r.type == ItemResourceType.ServiceMen)
                        {
                            city.freeServiceMen.amount += r.amount;
                        }
                        else if (r.type != ItemResourceType.Water_G)
                        {
                            int returnAmount = r.amount / 2;
                            if (returnAmount > 4)
                            {
                                addRubble = true;

                                DssRef.state.resources.addItem(
                                    new Resource.ItemResource(
                                      r.type,
                                      //subTile.terrainQuality,
                                      //0,
                                      returnAmount),
                                  ref subTile.collectionPointer);
                            }
                        }
                    }
                }
                if (addRubble)
                {
                    subTile.mainTerrain = TerrainMainType.Resourses;
                    subTile.subTerrain = (byte)TerrainResourcesType.Rubble;
                }
                else
                {
                    subTile.mainTerrain = TerrainMainType.Destroyed;
                    subTile.subTerrain = 0;
                }

                EditSubTile edit = new EditSubTile(city.pfaction, true, subTilePos, subTile, true, true, true);
                edit.Submit();
            }
        }
    }

    enum MapPaintToolShape
    {
        Free,
        Line,
        LShape,
        Area,
        Path,
    }

    enum MapPaintToolCategory
    {
        Default,
        JustOne,
        Road,
        Wall,
    }

    enum BuildCategoryTab
    {
        General,
        Advanced,
        Farming,
        Military,
        Decor,
        Upgrade,
        Automation,
        Filter,
        GodPower,
        NUM
    }

    enum BuildFilterTag
    {

        Workers,

        Gold,
        Resources,
        Water,
        Food,
        Fuel,
        Metals,
        Craft,
        Farm,
        Animals,
        Storage,
        Transport,

        Weapons,
        Military,
        Soldiers,
        Guards,
        Walls,

        Road,
        Garden,
        Statue,
        Flag,

        Optimize,
        Upgrade,
        Research,

        NUM_NONE,
    }

    enum LShapeDir
    {
        NoSet,
        StartX,
        StartY,
    }
}
