using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Process
{
    class PickFromChest
    {
        GameObjects.Gadgets.IGadget item;
        GameObjects.Gadgets.IGadgetsCollection playerItems;

        public PickFromChest(GameObjects.EnvironmentObj.Chest chest, GameObjects.Gadgets.IGadget item, GameObjects.Gadgets.IGadgetsCollection playerItems)
        {
            this.item = item;
            if (item == null)
                throw new NullReferenceException("PickFromChest");
            this.playerItems = playerItems;
            chest.Data.GadgetColl.RemoveItem(item);//chest.Data.GadgetColl.PickItemFromIndex(index, amount, true);
            if (chest.Data.RequestPickItem(item, this))
            {
                GotSendPermit();
            }
        }
        public void GotSendPermit()
        {
            playerItems.AddItem(item);
        }

        /// <returns>Complete and should be removed</returns>
        public bool HostPermit(bool permit, ushort hashtag, GameObjects.Gadgets.GadgetsCollection chestCollection)
        {
            if (item.ItemHashTag == hashtag)
            {
                if (permit)
                {
                    GotSendPermit();
                }
                else
                { //put it back (?)

                }
                return true;
            }
            return false;
        }
    }
}
