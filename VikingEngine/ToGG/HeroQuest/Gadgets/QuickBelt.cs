using Microsoft.Xna.Framework;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class QuickBelt
    {
        //int slotCount = 4;

        public SlotArray slots;
        bool visible = true;
        public VectorRect hudArea;

        public QuickBelt()
        {
            //slotCount = player.

            
        }

        public void setSlotCount(int slotCount, AbsHQPlayer player)
        {
            //this.slotCount = slotCount;

            slots = new SlotArray(slotCount);
            for (int i = 0; i < slotCount; ++i)
            {
                slots.Add(new ItemSlot(SlotType.Quickbelt));
            }

            if (player is LocalPlayer)
            {
                createButtons();
                reset();
            }
        }

        public void createButtons()
        {
            Vector2 slotSize = SquareSize();
            float spacing = Engine.Screen.BorderWidth;

            float w = Table.TotalWidth(slots.Count, slotSize.X, spacing);

            Vector2 pos = Engine.Screen.SafeArea.CenterBottom;
            pos.X -= w * 0.5f;
            pos.Y -= slotSize.Y;

            hudArea = new VectorRect(pos.X, pos.Y, w, slotSize.Y);

            for (int i = 0; i < slots.Count; ++i)
            {                
                slots[i].createButton(new VectorRect(pos, slotSize), HudLib.BgLayer);
                slots[i].button.slotView = SlotView.Hud;
                
                pos.X += slotSize.X + spacing;
            }
        }

        public static Vector2 SquareSize()
        {
            return Engine.Screen.IconSizeV2;
        }

        public void update(LocalPlayer player, ref PlayerDisplay display)
        {
            if (visible)
            {
                display.mouseOverHud |= hudArea.IntersectPoint(Input.Mouse.Position);

                foreach (var m in slots)
                {
                    if (m.button.update())
                    {
                        if (player.itemDrag == null)
                        {
                            player.itemDrag = new QuickItemDragNDrop(m);
                        }
                    }
                }
            }
        }

        public void clear()
        {
            foreach (var m in slots)
            {
                m.clear();
            }
        }

        public void reset()
        {
            clear();
        }

        public void setVisible(bool value)
        {
            if (value != visible)
            {
                visible = value;

                foreach (var m in slots)
                {
                    m.Visible = visible;
                }
            }
        }

        public bool addItem(AbsItem item)
        {
            foreach (var m in slots)
            {
                if (m.item == null)
                {
                    m.addItem(item);
                    return true;
                }
            }

            return false;
        }

        public int hasItem(ItemFilter itemFilter)
        {
            int count = 0;

            foreach (var m in slots)
            {
                if (m.Contains(itemFilter))
                {
                    count += m.item.count;
                }
            }

            return count;
        }

        public bool RemoveItem(ItemFilter itemFilter, int count)
        {
            //Tar bara bort 1
            if (hasItem(itemFilter) >= count)
            {
                for (int i = slots.Count - 1; i >= 0; --i)
                {
                    if (slots[i].Contains(itemFilter))
                    {
                        slots[i].clear();
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsFull()
        {
            foreach (var m in slots)
            {
                if (m.item == null)
                {
                    return false;
                }
            }
            return true;
        }
    }

    
}
