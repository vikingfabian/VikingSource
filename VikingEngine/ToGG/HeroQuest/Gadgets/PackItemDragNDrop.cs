using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.HeroQuest.Display;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class PackItemDragNDrop : AbsItemDragNDrop
    {
        ItemSlot fromSlot;
        BackPackMenu menu;

        public PackItemDragNDrop(BackPackMenu menu, ItemSlot slot)
            : base()
        {
            this.fromSlot = slot;
            this.menu = menu;
        }

        public void update(ItemSlot mouseOver)
        {
            bool dragDropEvent = mouseDrag.update();

            if (dragDropEvent)
            {
                if (mouseDrag.state == Input.MouseDragState.Drag)
                {
                    item = hqRef.items.beginDragItem(fromSlot);

                    mousePosDiff = item.itemImage.Position - mouseDrag.mouseDownPos;
                    item.itemImage.Visible = true;
                    item.itemImage.Layer = HudLib.BackpackLayer - 8;
                    menu.onDrag(item);
                }
                else if (mouseDrag.state == Input.MouseDragState.Drop)
                {
                    if (mouseOver != null)
                    {
                        hqRef.items.beginDropItem(menu, item, fromSlot, mouseOver);                        
                    }
                    else
                    {
                        cancel();
                    }
                }
            }

            if (mouseDrag.state == Input.MouseDragState.Drag)
            {
                item.itemImage.Position = Input.Mouse.Position + mousePosDiff;                
            }
        }

        //public void update(SendToPlayerButton mouseOver)
        //{
        //    bool dragDropEvent = mouseDrag.update();

        //    if (dragDropEvent && mouseDrag.state == Input.MouseDragState.Drop)
        //    {

        //    }
        //}

        public void onItemSwap(AbsItem prevItem, ItemSlot slot)
        {
            item = prevItem;
            mouseDrag.replaceItemOnDrop();
            fromSlot = slot;
        }

        public void cancel()
        {
            hqRef.items.returnItem(item, fromSlot, false);
            menu.EndDragItem();
        }
    }
}
