using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class ItemCountDialogue : AbsPopupWindow
    {        
        AbsItem item;
        ItemSlot from;
        ItemSlot to;
        BackPackMenu menu;
        List<ValueButton> valueButtons;

        public ItemCountDialogue(BackPackMenu menu, AbsItem item, ItemSlot from, ItemSlot to)
        {
            this.item = item;
            this.from = from;
            this.to = to;
            this.menu = menu;

            item.itemImage.Visible = false;

            float edge = Engine.Screen.BorderWidth;
            Vector2 buttonSz = Engine.Screen.SmallIconSizeV2;
            float spacing = Engine.Screen.BorderWidth;
            float w = Table.TotalWidth(10,  buttonSz.X, spacing);

            Vector2 pos = contentStartPos();
            Vector2 contentStart = pos;

            valueButtons = new List<ValueButton>(item.count);

            ValueButton v1 = new ValueButton(pos, VectorExt.Add(buttonSz * 2f, spacing), 1);
            valueButtons.Add(v1);

            pos.X += v1.area.Width + spacing;
            pos.Y += buttonSz.Y + spacing;
            
            int x = 2;
            for (int val = 2; val < item.count; ++val)
            {
                ValueButton vb = new ValueButton(pos, buttonSz, val);
                valueButtons.Add(vb);
                pos.X += buttonSz.X + spacing;

                if (++x >= 10)
                {
                    pos.X = contentStart.X;
                    pos.Y += buttonSz.Y + spacing;
                    x = 0;
                }
            }

            pos.X = contentStart.X;
            if (x != 0)
            {
                pos.Y += buttonSz.Y + spacing;
            }

            ValueButton vAll = new ValueButton(pos, new Vector2(w, buttonSz.Y * 1.5f), item.count);
            valueButtons.Add(vAll);

            Vector2 bottomRight = vAll.area.RightBottom;
            VectorRect area = new VectorRect(Vector2.Zero, bottomRight + new Vector2(sideEdge()));
            
            Vector2 mousePos = vAll.area.Center;
            
            area.Position = Input.Mouse.Position - mousePos;
            area = Engine.Screen.SafeArea.KeepSmallerRectInsideBound_Position(area);
            
            images.Move(area.Position);
            foreach (var m in valueButtons)
            {
                m.Move(area.Position);
            }

            Input.Mouse.SetPosition(new IntVector2(vAll.area.Center));

            completeWindow(area, SpriteName.NO_IMAGE, "Select amount", HudLib.PopupLayer, true);
        }

        public bool update()
        {
            foreach (var m in valueButtons)
            {
                if (m.update())
                {
                    hqRef.items.dropItem(menu, item, from, to, m.value);                 
                    return true;
                }
            }

            if (exitButton.update() || Input.Mouse.ButtonDownEvent(MouseButton.Right))
            {
                menu.itemDrag.cancel();
                return true;
            }

            return false;
        }

        public override void DeleteMe()
        {
            foreach (var m in valueButtons)
            {
                m.DeleteMe();
            }
            base.DeleteMe();
        }
        
        class ValueButton : Button
        {
            public int value;

            public ValueButton(Vector2 pos, Vector2 sz, int value)
                : base(new VectorRect(pos, sz), HudLib.PopupLayer, ButtonTextureStyle.Popup)
            {
                this.value = value;
                this.addCenterText(value.ToString(), Color.Black, 0.8f);
            }
        }
    }
}
