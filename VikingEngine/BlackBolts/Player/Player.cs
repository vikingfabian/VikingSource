using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.Interface;
using VikingEngine.Core.BlackBolts.Laws;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.ToGG;

namespace VikingEngine.Core.BlackBolts.Player
{
    class Player
    {

        IOdisplay iodisplay = new IOdisplay();
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
            iodisplay.update(cameraControl.camera);

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

                BlackRef.playScene.mapmodel.decalsNeedsUpdate = true;
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
            iodisplay.refresh(cameraControl.tilePos);
        }

        public void onNewTile(IntVector2 tilePos)
        {
            mapSelect.Select(tilePos);
            if (drawButtonDown)
            {
                paintOnTile();
            }
            else
            {
                iodisplay.refresh(tilePos);
            }
        }
    }
}
