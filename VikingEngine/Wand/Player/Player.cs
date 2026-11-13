using Steamworks;
using System;
using System.Collections.Generic;
using VikingEngine.Core.Wand.Display.HeadDisplay;
using VikingEngine.Core.Wand.GO.Item;
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
                    selectWand(i);
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

        public void dropWand(int index)
        {
            equipment.selectedWand = null;
            var wand = equipment.wands.PullIndex(index);

            if (WandRef.world.map.tileGrid.Get(WandRef.world.hero.currentPos.tilePos).item.itemType == GO.ObjectPointer.TileItemType.Altar)
            {
                //Sacrifice
                rechargeWands();
            }
            else
            {
                var item = (MagicWandItem)ObjectLifeTime.SpawnItem(WandRef.world.hero.currentPos.tilePos, GO.ObjectPointer.TileItemType.MagicWand, true);
                item.wand = wand;
            }
        }

        public void rechargeWands()
        { 
            for (int i = 0; i < equipment.wands.Count; i++)
            {
                equipment.wands[i].recharge();
            }
        }

        public void FoundMagic(int tier)
        {
            if (equipment.wands.Count > 0)
            {
                List<int> unfound = new List<int>();

                for (int tierIx = 0; tierIx < tier; tierIx++)
                {
                    for (int trial = 0; trial < 100; trial++)
                    {
                        int rndIx = Ref.rnd.Int(equipment.wands.Count);
                        var wand = equipment.wands[rndIx];
                        unfound.Clear();
                        for (int i = 0; i < wand.magicEffects.Count; ++i)
                        {
                            if (wand.magicEffects[i].isFound == false)
                            {
                                unfound.Add(i);
                            }
                        }

                        if (unfound.Count > 0)
                        {
                            int magicIx = arraylib.RandomListMember(unfound);
                            wand.magicEffects[magicIx].isFound = true;
                            break;
                        }
                    }
                }
            }
        }

        public void selectWand(int index)
        {
            if (equipment.selectedWand.HasValue && equipment.selectedWand.Value == index)
            {
                equipment.selectedWand = null;
            }
            else
            {
                equipment.selectedWand = index;
            }
            onSelectWand();
        }

        void onSelectWand()
        { 
            MenuNeedRefresh();
        }
    }
}
