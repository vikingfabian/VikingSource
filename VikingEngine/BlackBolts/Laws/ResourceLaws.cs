using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.GO.Creature;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.LootFest.GO.Gadgets;

namespace VikingEngine.Core.BlackBolts.Laws
{
    static class ResourceLaws
    {
        static CheckTileLoop checkTileLoop = new CheckTileLoop();

        public static bool CanDispenceResource(MapPlacement placement, CategoryAndType item)
        {
            if (BlackRef.mapData.tileGrid.TryGet(placement.tilePos, out Tile tile))
            {               
                if (tile.pMachine.hasValue)
                {
                    var iO_Unit = tile.pMachine.GetMachine() as IO_unit;
                    if (iO_Unit != null)
                    {
                        return iO_Unit.DispenceResource(placement, item, false);
                    }
                }
                if (tile.canPlaceResource())
                {
                    return true;
                }
            }
            return false;
        }

        public static void DispenceResource(MapPlacement placement, ResourceType resourceType)
        {
            if (BlackRef.mapData.tileGrid.TryGet(placement.tilePos, out Tile tile))
            {
                if (tile.pMachine.hasValue)
                {
                    var iO_Unit = tile.pMachine.GetMachine() as IO_unit;
                    if (iO_Unit != null)
                    {
                        if (iO_Unit.DispenceResource(placement, new CategoryAndType(resourceType), true))
                        {
                            return;
                        }
                    }
                }
            }
            var resource = BlackRef.mapData.SpawnResource(resourceType);
            resource.placeResourceOnFloor(placement.tilePos);
        }

        public static void Dispence(MapPlacement placement, CategoryAndType item)
        {
            switch (item.category)
            {
                case ObjectCategory.Resource:
                    DispenceResource(placement, item.resource);
                    break;
                
                case ObjectCategory.FactoryObject:
                    PlaceObjectData place = new PlaceObjectData()
                    {
                        mapPlacement = placement,
                        component = new Mission.ToolSetupComponent(item.factoryobject)
                    };
                    var creature = ObjectBuilder.Create(place, false, true) as AbsCreature;
                    if (creature != null)
                    {
                        creature.refreshAnimation();
                    }
                    break;

            }
            
        }

        public static void TryCreateResource(MapPlacement placement, ResourceType resourceType)
        {
            if (BlackRef.mapData.tileGrid.TryGet(placement.tilePos, out Tile tile))
            {
                if (tile.pCreature.hasValue)
                {
                    var resource = BlackRef.mapData.SpawnResource(resourceType);
                    tile.pCreature.GetCreature().HandoverItem(resource.pointer);
                }
                else
                {
                    if (CanDispenceResource(placement, new CategoryAndType(resourceType)))
                    {
                        DispenceResource(placement, resourceType);
                    }
                }
            }
        }

        public static void DropResource(IntVector2 tilePos, ResourceType resourceType)
        {
            checkTileLoop.start(tilePos);

            while (checkTileLoop.Next(out IntVector2 tryPos))
            {
                if (BlackRef.mapData.tileGrid.TryGet(tryPos, out Tile tile) &&
                    !tile.pResource.hasValue &&
                    tile.canPlaceResource())
                {
                    DispenceResource(new MapPlacement(tryPos, Dir4.NUM_NON), resourceType);
                    return;
                }
            }
        }

        public static void DropFluid(IntVector2 tilePos, ResourceType resourceType)
        {
            checkTileLoop.start(tilePos);

            while (checkTileLoop.Next(out IntVector2 tryPos))
            {
                if (BlackRef.mapData.tileGrid.TryGet(tryPos, out Tile tile) &&
                    (!tile.fluid.HasValue || tile.fluid.resourceType != resourceType))
                {
                    tile.fluid = new Fluid(resourceType);
                    BlackRef.playScene.mapmodel.decalsNeedsUpdate = true;
                    return;
                }
            }
        }

        public static void ConvertResource(MapPlacement placement, ResourceType convertType)
        {
            var tile = BlackRef.mapData.tileGrid.Get(placement.tilePos);
            if (tile.pResource.hasValue)
            {
                CategoryAndType conversion;
                var res = tile.pResource.GetSolidResource();
                switch (convertType)
                {
                    default:
                        return;

                    case ResourceType.Heat:
                        conversion = ResourceLib.Get(res.placementData.resourceType).fireConvert;
                        break;
                }

                tile.pResource.hasValue = false;
                res.DeleteMe();
                switch (conversion.category)
                {
                    case ObjectCategory.Resource:
                        DispenceResource(placement, conversion.resource);
                        break;
                }
            }
        }
    }

    class CheckTileLoop
    {
        //Dir4 nextDir = Dir4.N;
        HashSet<IntVector2> checkedTiles = new HashSet<IntVector2>(4);
        List<IntVector2> CheckItemOrder = new List<IntVector2>(8);

        int currentCheckItemOrderIx = 0;

        public void start(IntVector2 tile)
        {
            //nextDir = Dir4.N;
            checkedTiles.Clear();
            CheckItemOrder.Clear();

            CheckItemOrder.Add(tile);
            currentCheckItemOrderIx = 0;
        }

        public bool Next(out IntVector2 tile)
        {
            if (currentCheckItemOrderIx >= CheckItemOrder.Count)
            {
                //Refill check list
                int end = CheckItemOrder.Count;
                //foreach (var pos in CheckItemOrder)
                for (int i = 0; i < end; ++i)
                {
                    var pos = CheckItemOrder[i];
                    if (!checkedTiles.Contains(pos))
                    {
                        foreach (var dir in IntVector2.Dir4Array)
                        {
                            var nPos = dir + pos;
                            if (!checkedTiles.Contains(nPos) &&
                                 BlackRef.mapData.tileGrid.InBounds(nPos))
                            {
                                CheckItemOrder.Add(nPos);
                            }
                        }
                        checkedTiles.Add(pos);
                    }
                }
            }

            if (currentCheckItemOrderIx < CheckItemOrder.Count)
            {
                tile = CheckItemOrder[currentCheckItemOrderIx++];
                return true;
            }
            else
            {
                tile = CheckItemOrder[0];
                return false;
            }
        }
    }
}
