using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.GO.Creature;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Player;
using VikingEngine.DSSWars;

namespace VikingEngine.Core.BlackBolts.Data
{
    static class ObjectBuilder
    {
        public static AbsGameObject Create(PlaceObjectData placementData, bool toggleDestroy, bool iscreatureSpawn)
        {
            AbsGameObject result = null;
            if (BlackRef.mapData.tileGrid.TryGet(placementData.mapPlacement.tilePos, out var tile))
            {
                if (tile.isLocked && !iscreatureSpawn)
                {
                    return null;
                }

                bool empty = iscreatureSpawn ? !tile.pCreature.hasValue : tile.IsEmpty();

                if (empty)
                {
                   
                    switch (placementData.component.objectType)
                    {

                        case FactoryObjectType.IOunit:
                            {
                                bool canPlace = true;
                                var obj = new IO_unit(placementData);

                                if (obj.tilesize.SideLength() > 1)
                                {
                                    //Check is inside bounds
                                    //Check for locked tiles
                                    //Delete objects in the way
                                   
                                    ForXYLoop loop = new ForXYLoop( MapPlacement.CoverArea(obj.placementData.mapPlacement, obj.tilesize));
                                    while (loop.Next())
                                    {
                                        if (BlackRef.mapData.tileGrid.TryGet(loop.Position, out var checkTile))
                                        {

                                        }
                                        else
                                        {
                                            canPlace = false;
                                            break;
                                        }
                                    }

                                    if (canPlace)
                                    {
                                        loop.Reset();
                                        while (loop.Next())
                                        {
                                            BlackRef.mapData.tileGrid.Get(loop.Position).ClearTile();
                                        }
                                    }
                                    else
                                    {
                                        obj.DeleteMe();
                                    }
                                }

                                if (canPlace)
                                {
                                    BlackRef.mapData.AddObject(obj);
                                    result = obj;
                                }
                            }
                            break;

                        case FactoryObjectType.CreatureSpawner:
                            {
                                var obj = new CreatureSpawner(placementData);
                                result = obj;
                            }
                            break;

                        case FactoryObjectType.Floor_drop:
                            {
                                var obj = new DropToFloor(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;

                        case FactoryObjectType.Belt_dispencer:
                            {
                                var obj = new BeltDispencer(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;

                        case FactoryObjectType.Stone_pillar:
                            {
                                var obj = new Pillar(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case FactoryObjectType.Worker:
                            {
                                var obj = new Worker(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case FactoryObjectType.NightDemon:
                            {
                                var obj = new NightDemon(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case FactoryObjectType.Dragon:
                            {
                                var obj = new Dragon(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case FactoryObjectType.BlackKnight:
                            {
                                var obj = new BlackKnight(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case FactoryObjectType.WhiteKnight:
                            {
                                var obj = new WhiteKnight(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case FactoryObjectType.Belt:
                            {
                                var obj = new Belt(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case FactoryObjectType.Spin_plate:
                            {
                                var obj = new SpinPlate(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case FactoryObjectType.Table:
                            {
                                var obj = new ItemTable(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case FactoryObjectType.Dispencer:
                            {
                                var obj = new Dispencer(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case FactoryObjectType.Delivery_point:
                            {
                                var obj = new DeliveryPoint(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case FactoryObjectType.Garbage_disposal:
                            {
                                var obj = new GarbageDisposal(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case FactoryObjectType.NoBuildZone:
                            tile.tileEffect = TileEffect.NoBuildZone;
                            BlackRef.playScene.mapmodel.floorNeedsUpdate = true;
                            break;
                    }

                    if (result != null && placementData.includeItem &&
                        FactoryObjectLib.Get( placementData.component.objectType).includeResource != IncludeType.NoInclude)
                    {
                        Laws.ResourceLaws.TryCreateResource(placementData.mapPlacement.tilePos, placementData.resourceType);
                    }
                    //return true;
                }
                else if (toggleDestroy)
                {
                    if (tile.tileEffect != TileEffect.None)
                    {
                        BlackRef.playScene.mapmodel.floorNeedsUpdate = true;
                    }
                    tile.ClearTile();

                    var spawnerC = BlackRef.mapData.spawnerList.counter();
                    while (spawnerC.Next())
                    {
                        if (spawnerC.sel.placementData.mapPlacement.tilePos == placementData.mapPlacement.tilePos)
                        {
                            spawnerC.sel.DeleteMe();
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}
