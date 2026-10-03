using System;
using System.Collections.Generic;
using System.Text;
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
            mapSelect.Rotation(toolShop.toolDir);
        }
        public void update()
        {
            bool mouseOverHud = false;
            toolMenu.update(ref mouseOverHud);

            if (!mouseOverHud)
            {
                cameraControl.update(this);

                if (inputMap.toggleEditMode.DownEvent)
                {
                    editMode = !editMode;
                    if (!editMode)
                    { 
                        runExecuter.Start();
                    }
                }

                if (editMode)
                {
                    if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                    {
                        paintOnTile();
                        drawButtonDown = true;
                    }
                    if (inputMap.rotate.DownEvent)
                    {
                        //toolDir++;
                        //if (toolDir >= Dir4.NUM_NON)
                        //{
                        //    toolDir = Dir4.N;
                        //}
                        toolShop.RotateTool();
                        OnToolRefresh();
                    }
                }
                else
                {
                    runExecuter.Update();
                }
            }

            if (inputMap.click.UpEvent)
            {
                drawButtonDown = false;
            }
        }

        public void OnToolRefresh()
        { 
            mapSelect.Rotation(toolShop.toolDir);
        }

        void paintOnTile()
        {
            if (BlackRef.mapData.tileGrid.TryGet(cameraControl.tilePos, out var tile))
            {
                if (tile.IsEmpty())
                {
                    var placement = new MapPlacement(cameraControl.tilePos, toolShop.toolDir);

                    switch (toolShop.selectedObjectType)
                    {
                        case GameObjectType.Worker:
                            {
                                var obj = new Worker(placement);
                                BlackRef.mapData.AddObject(obj);
                            }
                            break;
                        case GameObjectType.Belt:
                            {
                                var obj = new Belt(placement);
                                BlackRef.mapData.AddObject(obj);
                            }
                            break;
                        case GameObjectType.Spin_plate:
                            {
                                var obj = new SpinPlate(placement);
                                BlackRef.mapData.AddObject(obj);
                            }
                            break;
                        case GameObjectType.Table:
                            {
                                var obj = new ItemTable(placement);
                                BlackRef.mapData.AddObject(obj);
                            }
                            break;
                        case GameObjectType.Dispencer:
                            {
                                var obj = new Dispencer(placement, toolShop.selectedResourceType);
                                BlackRef.mapData.AddObject(obj);
                            }
                            break;
                        case GameObjectType.Delivery_point:
                            {
                                var obj = new DeliveryPoint(placement);
                                BlackRef.mapData.AddObject(obj);
                            }
                            break;

                    }

                }
            }
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
