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
        MapSelect mapSelect = new MapSelect();
        CameraControl cameraControl;
        public InputMap inputMap;

        RunExecuter runExecuter = new RunExecuter();

        Interface.ToolMenu toolMenu;

        Dir4 toolDir = Dir4.S;

        public bool editMode = true;

        public Player()
        {
            cameraControl = new CameraControl();
            inputMap = new InputMap(0);
            toolMenu = new Interface.ToolMenu();
            mapSelect.Rotation(toolDir);
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
                        onMapClick();
                    }
                    if (inputMap.rotate.DownEvent)
                    {
                        toolDir++;
                        if (toolDir >= Dir4.NUM_NON)
                        {
                            toolDir = Dir4.N;
                        }
                        mapSelect.Rotation(toolDir);
                    }
                }
                else
                {
                    runExecuter.Update();
                }
            }
        }

        void onMapClick()
        {
            if (BlackRef.mapData.tileGrid.TryGet(cameraControl.tilePos, out var tile))
            {
                if (tile.IsEmpty())
                {
                    var obj = new Worker(new MapPlacement() { tilePos = cameraControl.tilePos, direction = toolDir });
                    BlackRef.mapData.AddObject(obj);
                }
            }
        }

        public void onNewTile(IntVector2 tilePos)
        {
            mapSelect.Select(tilePos);
        }
    }
}
