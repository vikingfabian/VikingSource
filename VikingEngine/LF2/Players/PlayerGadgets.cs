using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LF2.GameObjects.Gadgets;

namespace VikingEngine.LF2.Players
{
#if !CMODE
    class PlayerGadgets : GadgetsCollection
    {
        Players.Player parent;
        public PlayerGadgets(Players.Player parent)
            : base()
        {
            this.parent = parent;
        }

        public void AddStartUpGadget(IGadget gadget)
        {
            base.AddItem(gadget);
        }

        protected override void AddItem(Item item)
        {
            base.AddItem(item);
            item.Amount = items[item.Type];
            parent.UpdateItemAmount(item);
            if (item.Type == GoodsType.Water_bottle)
            {
                parent.hero.SpendMagic(0);
            }
        }
        public override bool RemoveItem(Item item)
        {
            bool result = base.RemoveItem(item);

            if (items.ContainsKey(item.Type))
            {
                item.Amount = items[item.Type];
            }
            else
            {
                item.Amount = 0;
            }
            parent.UpdateItemAmount(item);
            return result;
        }

        public File ItemOptions(GadgetLink menuLink, int backpackLink, 
            GadgetLinkEvent equipDialogue,
            GadgetLinkEvent dropDialogue)
        {
            File file = new File();
            IGadget g = menuLink.Gadget;
            file.AddTitle(g.ToString());
            file.AddDescription(g.GadgetInfo);
            if (PlatformSettings.ViewUnderConstructionStuff) file.AddDescription("Weight: " + g.Weight.ToString());

            if (!parent.hero.FullMagic && g.GadgetType == GadgetType.Item)
            {
                GameObjects.Gadgets.Item item = (GameObjects.Gadgets.Item)g;
                if (item.Type == GoodsType.Water_bottle)
                {
                    file.AddIconTextLink(SpriteName.ItemWaterFull, "Drink", "Refill the magic meter", new HUD.ActionLink(drinkWaterLink));
                }
            }
            if (g.EquipAble && !parent.Progress.GadgetIsEquipped(g))
            {
                menuLink.LinkEvent = equipDialogue;
                file.AddIconTextLink(SpriteName.LFIconEquip, "Equip", menuLink);
            }
            else if (!parent.hero.FullHealth && g.GadgetType == GadgetType.Goods)
            {
                GameObjects.Gadgets.Goods goods = (GameObjects.Gadgets.Goods)g;
                if (goods.Eatable)
                {
                    menuLink.LinkEvent =menuLink.player.QuickEatItem;
                    file.AddIconTextLink(SpriteName.LFHealIcon, "Eat", menuLink);
                }
            }
            menuLink.LinkEvent = dropDialogue;
            file.AddIconTextLink(SpriteName.LFIconDiscardItem, "Drop", "Place the item on the ground, can be picked up again", menuLink);
            file.AddIconTextLink(SpriteName.BoardIconBack, "Back", backpackLink);
            return file;
        }

        void drinkWaterLink()
        {
            parent.hero.DrinkWaterBottle();
            parent.CloseMenu();
        }

        protected override void equipableRemovedEvent(IGadget gadget)
        {
            parent.Progress.EquipableRemovedEvent(gadget);
        }
        protected override void equipableAddedEvent(IGadget gadget)
        {
            parent.Progress.AutoEquipNewGadget(gadget, parent);
        }

        bool overweight = false;
        public bool OverWeight { get { return overweight; } }
        public void CalcWeight()
        {
            overweight = this.Weight() > LootfestLib.MaxWeight;
        }
    }
#endif
}
