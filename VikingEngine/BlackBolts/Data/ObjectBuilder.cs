using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.GO;
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
                    switch (placementData.gameObjectType)
                    {
                        case GameObjectType.Belt_dispencer:
                            {
                                var obj = new BeltDispencer(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;

                        case GameObjectType.Stone_pillar:
                            {
                                var obj = new Pillar(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case GameObjectType.Worker:
                            {
                                var obj = new Worker(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case GameObjectType.Belt:
                            {
                                var obj = new Belt(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case GameObjectType.Spin_plate:
                            {
                                var obj = new SpinPlate(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case GameObjectType.Table:
                            {
                                var obj = new ItemTable(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case GameObjectType.Dispencer:
                            {
                                var obj = new Dispencer(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;
                        case GameObjectType.Delivery_point:
                            {
                                var obj = new DeliveryPoint(placementData);
                                BlackRef.mapData.AddObject(obj);
                                result = obj;
                            }
                            break;

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
