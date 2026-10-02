using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG;

namespace VikingEngine.Core.BlackBolts.Player
{
    class Player
    {
        CameraControl cameraControl;
        InputMap inputMap;

        Interface.ToolMenu toolMenu;

        public Player()
        {
            cameraControl = new CameraControl();
            inputMap = new InputMap(0);
            toolMenu = new Interface.ToolMenu();
        }
        public void update()
        {
            bool mouseOverHud = false;
            toolMenu.update(ref mouseOverHud);

            if (!mouseOverHud)
            {
                cameraControl.update(inputMap);
            }
        }

       
    }
}
