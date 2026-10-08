using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.GO.Creature;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Player;

namespace VikingEngine.Core.BlackBolts.Data
{
    static class ObjectBuilder
    {
        

        public static void Create(PlaceObjectData placementData, bool toggleDestroy)
        {
            if (BlackRef.mapData.tileGrid.TryGet(placementData.mapPlacement.tilePos, out var tile))
            {
                if (tile.IsEmpty())
                {
                    AbsGameObject result = null;
                    switch (placementData.component.objectType)
                    {

                        case FactoryObjectType.IOunit:
                            {
                                var obj = new IO_unit(placementData);
                                BlackRef.mapData.AddObject(obj);
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
                        case FactoryObjectType.GoblinWorker:
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

                    }

                    if (result != null && placementData.includeItem &&
                        FactoryObjectLib.Get( placementData.component.objectType).includeResource != IncludeType.NoInclude)
                    {
                        Laws.ResourceLaws.TryCreateResource(placementData.mapPlacement.tilePos, placementData.resourceType);
                    }
                    //return true;
                }
                else
                { 
                    tile.ClearTile();
                }
            }
            //return false;
        }
    }
}
