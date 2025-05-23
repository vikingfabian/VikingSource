using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichMenu;
using VikingEngine.PJ.Match3;

namespace VikingEngine.HUD.RichBox
{
    abstract class AbsRbInteraction
    {   
        abstract public bool update(Vector2 mousePosOffSet, RichMenu.RichMenu menu, bool useClickInput, out bool needRefresh, out bool endInteraction);
        abstract public void end(out bool needRefresh);
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

            //foreach (var m in content)
            //{
            //    m.getButtons(buttons);
            //}
            refresh(content);
        }

        public void refresh(List<AbsRichBoxMember> content)
        {
            buttons.Clear();
            foreach (var m in content)
            {
                m.getButtons(buttons);
            }
        }

       
        public bool updateController(RichMenuControllerPointer pointer)
        {
            pointer.pointer.position += pointer.accelerateInput(pointer.inputMap.move.direction);
            pointer.pointer.position = pointer.menu.renderArea.KeepPointInsideBound_Position(pointer.pointer.position);

            refreshControllerHover(pointer);
            return clickUpdate(pointer.menu, true);
        }

        public void refreshControllerHover(RichMenuControllerPointer pointer)
        {
            Vector2 pos = pointer.pointer.position - pointer.menu.renderArea.Position;

            VectorRect area = VectorRect.Zero;
            AbsRbButton prev = hover;
            hover = null;
            float distance = float.MaxValue;
            foreach (var m in buttons)
            {
                area = m.area();
                float arDist = area.distanceTo(pos);
                if (arDist <= 0)
                {
                    hover = m;
                    distance = 0;
                    break;
                }
                else
                {
                    if (arDist < distance && arDist < pointer.maxInteractDistance)
                    {
                        distance = arDist;
                        hover = m;
                    }
                }
            }

            hoverUpdate(pointer.menu, prev);
        }


        /// <returns>Any interaction happened (to avoid multiple)</returns>
        override public bool update(Vector2 mousePosOffSet, RichMenu.RichMenu menu, bool useClickInput, out bool needRefresh, out bool unused1)
        {
            //Debug.Log("Interaction UPDATE");
            //Debug.Log($"Mouse offset: {mousePosOffSet}");
            //Debug.Log($"Menu bg pos: {menu.backgroundArea.Position}");
            //Debug.Log($"Menu content offset: {menu.richBox.GetOffset()}");
            unused1 = false;
            needRefresh = false;
            if (interactionStack != null)
            {
                var result = interactionStack.update(mousePosOffSet, menu, useClickInput, out needRefresh, out bool endInteraction);
                if (endInteraction)
                {
                    interactionStack.end(out needRefresh);
                    interactionStack = null;
                }
                return result;
            }

            AbsRbButton prev = hover;
            int buttonIndex = 0;
            VectorRect area = VectorRect.Zero;
            //VectorRect area2 = VectorRect.Zero;
            //int hoverIx = 0;
            if (clickInput.IsMouse)
            {
                Vector2 pos = Input.Mouse.Position + mousePosOffSet;
                //Debug.Log($"mouse pos: {pos}");
                hover = null;
                

                foreach (var m in buttons)
                {
                    area = m.area();
                    if (area.IntersectPoint(pos))
                    {
                        hover = m;
                        break;
                    }
                    ++buttonIndex;
                }
            }

            hoverUpdate(menu, prev);

            return clickUpdate(menu, useClickInput);
        }

        void hoverUpdate(RichMenu.RichMenu menu, AbsRbButton prev)
        { 
            if (hover != prev)
            {
                if (prev != null)
                {
                    prev.clickAnimation(false);
                    //Debug.Log("deleteTooltip: new hover");
                    menu?.deleteTooltip();
                }
                refreshSelectOutline();
                
                hover?.onEnter(menu);
            }
        }

        bool clickUpdate(RichMenu.RichMenu menu, bool useClickInput)
        {
            if (hover != null)
            {
                if (clickInput.DownEvent && useClickInput)
                {
                    hover.onClick(menu);
                    hover?.clickAnimation(true);
                    return true;
                }
                else if (clickInput.UpEvent)
                {
                    hover.clickAnimation(false);
                }
            }

            return false;
        }

        public override void end(out bool needRefresh)
        {
            needRefresh = false;
            //throw new NotImplementedException();
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
            if (Input.Keyboard.Ctrl)
            {
                lib.DoNothing();
            }

            if (selectionOutline != null)
            {
                Ref.draw.AddToContainer = drawContainer;
                hover = null;
                selectionOutline?.DeleteMe();
                selectionOutline = null;
                Ref.draw.AddToContainer = null;
            }
        }

        public void DeleteMe()
        {
            selectionOutline?.DeleteMe();
        }
    }
}
