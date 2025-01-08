using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.HUD.RichBox
{
    class RbInteraction
    {
        public RbButton hover = null;
        public List<RbButton> buttons = new List<RbButton>(4);
        public ImageLayers layer;

        Graphics.RectangleLines selectionOutline = null;
        Input.IButtonMap clickInput;        

        public RbInteraction(List<AbsRichBoxMember> content, ImageLayers layer,  Input.IButtonMap clickInput)
        {
            this.layer = layer;
            this.clickInput = clickInput;
           
            foreach (var m in content)
            {
                m.getButtons(buttons);
                //RichboxButton button = m as RichboxButton;
                //if (button != null)
                //{
                //    buttons.Add(button);
                //}
            }
        }

        public bool update()
        {
            if (clickInput.IsMouse)
            {
                Vector2 pos = Input.Mouse.Position;
                RbButton prev = hover;
                hover = null;
                VectorRect area = VectorRect.Zero;

                foreach (var m in buttons)
                {
                    if (m.buttonMap != null && m.buttonMap.DownEvent)
                    {
                        m.onClick();//.click?.actionTrigger();
                    }

                    area = m.area();
                    if (area.IntersectPoint(pos))
                    {
                        hover = m;
                        break;
                    }
                }

                if (hover != prev)
                {
                    //selectionOutline?.DeleteMe();
                    //if (hover != null)
                    //{
                    //    selectionOutline = new RectangleLines(area, 2, 1, layer);
                    //    hover.enter?.actionTrigger();
                    //    //Debug.Log("on enter");
                    //}
                    refreshSelectOutline();
                }
            }
            if (clickInput.DownEvent && hover != null)
            {
                hover.onClick();//.click?.actionTrigger();
                return true;
            }

            return false;
        }

        public void refreshSelectOutline()
        {
            selectionOutline?.DeleteMe();
            if (hover != null)
            {
                selectionOutline = new RectangleLines(hover.area(), 2, 1, layer);
                hover.onEnter();//.enter?.actionTrigger();
                //Debug.Log("on enter");
            }
        }

        public void clearSelection()
        {
            hover=null;
            selectionOutline?.DeleteMe();
            selectionOutline = null;
        }

        public void DeleteMe()
        {
            selectionOutline?.DeleteMe();
        }
    }
}
