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

            for (int i = 0; i < equipment.wands.Count; i++)
            {
                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.D1 + i) && equipment.wands.Array[i].charges > 0)
                {
                    if (equipment.selectedWand.HasValue && equipment.selectedWand.Value == i)
                    {
                        equipment.selectedWand = null;
                    }
                    else
                    {
                        equipment.selectedWand = i;
                    }
                    onSelectWand();
                }
            }

            //cameraControl.update_debugmode(this, mouseOverHud);
            if (WandRef.world.turnOrderManager.turnState == TurnState.WaitingForInput)
            {
                IntVector2 move = inputMap.movement.stepping.Normal_Safe();

                if (equipment.selectedWand.HasValue)
                {
                    if (WandRef.world.turnOrderManager.UseMagicWeapon(equipment.wands.Array[equipment.selectedWand.Value], move))
                    {
                        equipment.selectedWand = null;
                    }
                }
                else
                {
                    WandRef.world.turnOrderManager.MovePlayer(move);
                }
            }
            cameraControl.update(this, mouseOverHud);
        }

        public void MenuNeedRefresh()
        {
            equipmentDisplay.NeedRefresh();
        }

        public void selectWand(int index)
        {
            equipment.selectedWand = index;
            onSelectWand();
        }

        void onSelectWand()
        { 
            MenuNeedRefresh();
        }
    }
}
