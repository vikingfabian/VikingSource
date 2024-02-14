using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2
{
    class MoveItemsBetweenCollections : HUD.IValueWheelDialogueCallback
    {
        GameObjects.Gadgets.IGadgetsCollection from; 
        //int fromIndex;
        GameObjects.Gadgets.IGadgetsCollection toColl;
        Players.Player player;
        GameObjects.Gadgets.IGadget item;

        public MoveItemsBetweenCollections(GameObjects.Gadgets.IGadgetsCollection from, GadgetLink link, 
            GameObjects.Gadgets.IGadgetsCollection toColl, Players.Player player, bool autoMoveAll)
        {
            this.from = from;
            //this.fromIndex = fromIndex;
            this.toColl = toColl;
            this.player = player;

            item = link.Gadget; //from.PickItemFromIndex(fromIndex, int.MaxValue, false, toColl);
            
            if (autoMoveAll || item.StackAmount == 1)
            {
                ValueWheelDialogueOKEvent(item.StackAmount, link, null);
            }
            else
            {//select amount
                player.ValueDialogue = new HUD.ValueWheelDialogue(
                    this, new Range(1, item.StackAmount), player.SafeScreenArea,
                    item.StackAmount, 1, link);
            }
        }
        //Player.ValueDialogue = null;
        public void ValueWheelDialogueCancelEvent()
        {
            player.ItemMoveCompleted();
        }
        public void ValueWheelDialogueOKEvent(int value, HUD.IMenuLink link, Object non)
        {
            item.StackAmount = value;
            from.TransferreItem(item, toColl);
            player.ItemMoveCompleted(); 
        }
    }
}
