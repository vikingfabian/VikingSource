using System;
using VikingEngine.Core.Wand.Laws;


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

            //cameraControl.update_debugmode(this, mouseOverHud);
            if (WandRef.world.turnOrderManager.turnState == TurnState.WaitingForInput)
            {
                IntVector2 move = inputMap.movement.stepping.Normal_Safe();

                WandRef.world.turnOrderManager.MovePlayer(move);
            }
            cameraControl.update(this,  mouseOverHud);
        }
    }
}
