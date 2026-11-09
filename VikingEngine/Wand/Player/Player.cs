using System;


namespace VikingEngine.Core.Wand.Player
{
    class Player
    {
        CameraControl cameraControl;
        public InputMap inputMap;

        public Player()
        {
            cameraControl = new CameraControl();
            inputMap = new InputMap(0);
        }

        public void update()
        {
            bool mouseOverHud = false;

            cameraControl.update(this, mouseOverHud);
            

        }
    }
}
