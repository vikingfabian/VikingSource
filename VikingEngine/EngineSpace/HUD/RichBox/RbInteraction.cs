using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.PJ.Match3;

namespace VikingEngine.HUD.RichBox
{
    class RbInteraction
    {
        //public Vector2 outlineOffset = Vector2.Zero;
        public AbsRbButton hover = null;
        public List<AbsRbButton> buttons = new List<AbsRbButton>(4);
        public ImageLayers layer;

        Graphics.RectangleLines selectionOutline = null;
        Input.IButtonMap clickInput;
        public RenderTargetDrawContainer drawContainer = null;

        public RbInteraction(List<AbsRichBoxMember> content, ImageLayers layer,  Input.IButtonMap clickInput)
        {
            this.layer = layer;
            this.clickInput = clickInput;
           
            foreach (var m in content)
            {
                m.getButtons(buttons);
            }
        }

        public bool update(Vector2 mousePosOffSet)
        {
            if (clickInput.IsMouse)
            {
                Vector2 pos = Input.Mouse.Position + mousePosOffSet;
                //if (drawContainer != null)
                //{
                //    pos -= drawContainer.Position;
                //}
                AbsRbButton prev = hover;
                hover = null;
                VectorRect area = VectorRect.Zero;

                foreach (var m in buttons)
                {
                    if (m.buttonMap != null && m.buttonMap.DownEvent)
                    {
                        m.onClick();
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
                    prev?.clickAnimation(false);
                    refreshSelectOutline();
                }
            }
            if (hover != null)
            {
                if (clickInput.DownEvent)
                {
                    hover.onClick();
                    hover.clickAnimation(true);
                    return true;
                }
                else if (clickInput.UpEvent)
                { 
                    hover.clickAnimation(false);
                }
            }

            return false;
        }

        public void refreshSelectOutline()
        {
            Ref.draw.AddToContainer = drawContainer;

            selectionOutline?.DeleteMe();
            if (hover != null)
            {
                var ar = hover.area();
                //ar.Position += outlineOffset;
                selectionOutline = new RectangleLines(ar, 2, 1, layer);
                hover.onEnter();
            }

            Ref.draw.AddToContainer = null;
        }

        public void clearSelection()
        {
            Ref.draw.AddToContainer = drawContainer;
            hover = null;
            selectionOutline?.DeleteMe();
            selectionOutline = null;
            Ref.draw.AddToContainer = null;
        }

        public void DeleteMe()
        {
            selectionOutline?.DeleteMe();
        }
    }
}
