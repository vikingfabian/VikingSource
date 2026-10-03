using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.Laws;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.ToGG;

namespace VikingEngine.Core.BlackBolts.Player
{
    class Player
    {
       
        public ToolShop toolShop = new ToolShop();
        MapSelect mapSelect = new MapSelect();
        CameraControl cameraControl;
        public InputMap inputMap;

        RunExecuter runExecuter = new RunExecuter();

        Interface.ToolMenu toolMenu;

        

        public bool editMode = true;

        bool drawButtonDown = false;


        public Player()
        {
            cameraControl = new CameraControl();
            inputMap = new InputMap(0);
            toolMenu = new Interface.ToolMenu(this);
            mapSelect.Rotation(toolShop.placementData.mapPlacement.direction);
        }
        public void update()
        {
            bool mouseOverHud = false;
            toolMenu.update(ref mouseOverHud);

            cameraControl.update(this, mouseOverHud);
            
            if (inputMap.toggleEditMode.DownEvent)
            {
                toggleRunSimulation();
            }

            if (editMode)
            {
                if (!mouseOverHud)
                {
                    if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                    {
                        paintOnTile();
                        drawButtonDown = true;
                    }
                }
                if (inputMap.rotate.DownEvent)
                {
                    rotateToolAction();
                }
            }
            else
            {
                runExecuter.Update();
            }
            

            if (inputMap.click.UpEvent)
            {
                drawButtonDown = false;
            }
        }

        public void rotateToolAction()
        {
            toolShop.RotateTool();
            OnToolRefresh();
        }

        public void toggleRunSimulation()
        {
            editMode = !editMode;
            if (editMode)
            {
                BlackRef.mapData.ClearMap();
                BlackRef.mapData.RestoreMap();
            }
            else
            {
                BlackRef.mapData.CreateStorePoint();
                runExecuter.Start();
            }
            toolMenu.NeedRefresh();
        }

        public void OnToolRefresh()
        { 
            mapSelect.Rotation(toolShop.placementData.mapPlacement.direction);
        }

        void paintOnTile()
        {
            var place = toolShop.placementData;
            place.mapPlacement.tilePos = cameraControl.tilePos;
            ObjectBuilder.Create(place, true);
            //if (BlackRef.mapData.tileGrid.TryGet(cameraControl.tilePos, out var tile))
            //{
            //    if (tile.IsEmpty())
            //    {
            //        var placement = new MapPlacement(cameraControl.tilePos, toolShop.toolDir);

                //        switch (toolShop.selectedObjectType)
                //        {
                //            case GameObjectType.Worker:
                //                {
                //                    var obj = new Worker(placement);
                //                    BlackRef.mapData.AddObject(obj);
                //                }
                //                break;
                //            case GameObjectType.Belt:
                //                {
                //                    var obj = new Belt(placement);
                //                    BlackRef.mapData.AddObject(obj);
                //                }
                //                break;
                //            case GameObjectType.Spin_plate:
                //                {
                //                    var obj = new SpinPlate(placement);
                //                    BlackRef.mapData.AddObject(obj);
                //                }
                //                break;
                //            case GameObjectType.Table:
                //                {
                //                    var obj = new ItemTable(placement);
                //                    BlackRef.mapData.AddObject(obj);
                //                }
                //                break;
                //            case GameObjectType.Dispencer:
                //                {
                //                    var obj = new Dispencer(placement, toolShop.selectedResourceType);
                //                    BlackRef.mapData.AddObject(obj);
                //                }
                //                break;
                //            case GameObjectType.Delivery_point:
                //                {
                //                    var obj = new DeliveryPoint(placement);
                //                    BlackRef.mapData.AddObject(obj);
                //                }
                //                break;

                //        }

                //    }
                //}
        }

        public void onNewTile(IntVector2 tilePos)
        {
            mapSelect.Select(tilePos);
            if (drawButtonDown)
            {
                paintOnTile();
            }
        }
    }
}
