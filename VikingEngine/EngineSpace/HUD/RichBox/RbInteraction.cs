using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.PJ.Match3;

namespace VikingEngine.HUD.RichBox
{
    abstract class AbsRbInteraction
    {
        abstract public bool update(Vector2 mousePosOffSet, RichMenu.RichMenu menu, out bool endInteraction);
    }
    class RbInteraction: AbsRbInteraction
    {
        public AbsRbButton hover = null;
        public List<AbsRbButton> buttons = new List<AbsRbButton>(4);
        public ImageLayers layer;

        Graphics.RectangleLines selectionOutline = null;
        Input.IButtonMap clickInput;
        public RenderTargetDrawContainer drawContainer = null;
        public AbsRbInteraction interactionStack = null;

        public RbInteraction(List<AbsRichBoxMember> content, ImageLayers layer,  Input.IButtonMap clickInput)
        {
            this.layer = layer;
            this.clickInput = clickInput;
           
            foreach (var m in content)
            {
                m.getButtons(buttons);
            }
        }

        /// <returns>Any interaction happened (to avoid multiple)</returns>
        override public bool update(Vector2 mousePosOffSet, RichMenu.RichMenu menu, out bool unused1)
        {
            unused1 = false;
            if (interactionStack != null)
            {
                var result = interactionStack.update(mousePosOffSet, menu, out bool endInteraction);
                if (endInteraction)
                {
                    interactionStack = null;
                }
                return result;
            }

            AbsRbButton prev = hover;

            if (clickInput.IsMouse)
            {
                Vector2 pos = Input.Mouse.Position + mousePosOffSet;
               
                hover = null;
                VectorRect area = VectorRect.Zero;

                foreach (var m in buttons)
                {
                    area = m.area();
                    if (area.IntersectPoint(pos))
                    {
                        hover = m;
                        break;
                    }
                }
            }

            if (hover != prev)
            {
                prev?.clickAnimation(false);
                refreshSelectOutline();
                
                hover?.onEnter();
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

            end
            return false;
        }

        public void refreshSelectOutline()
        {
            Ref.draw.AddToContainer = drawContainer;

            selectionOutline?.DeleteMe();
            if (hover != null)
            {
                var ar = hover.area();
                selectionOutline = new RectangleLines(ar, 2, 1, layer);                
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
