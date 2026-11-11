using System;
using VikingEngine.Core.Wand.Display.HeadDisplay;
using VikingEngine.Core.Wand.Laws;
using VikingEngine.ToGG;


namespace VikingEngine.Core.Wand.Player
{
    class Player
    {
        CameraControl cameraControl;
        public InputMap inputMap;
        public EquipmentSetup equipment = new EquipmentSetup();
        EquipmentDisplay equipmentDisplay;
        public Player()
        {
            cameraControl = new CameraControl();
            inputMap = new InputMap(0);

            equipmentDisplay = new EquipmentDisplay(equipment);
        }

        public void update()
        {
            bool mouseOverHud = false;

            equipmentDisplay.update(ref mouseOverHud);

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
