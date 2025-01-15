using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.HUD;
using VikingEngine.PJ.Match3;

namespace VikingEngine.HUD.RichMenu
{
    class RichScrollbar
    {
        RenderTargetDrawContainer scrollerRenderer = null;


        Image slider;
        Graphics.Image background;
        IntervalF valuerange;
        float valueT;
        float slideRange;
        bool mouseDown = false;
        float mouseDownY;
        float mouseDown_SliderY;
        VectorRect area;
        
        public RichScrollbar(VectorRect displayArea, float scrollerWidth, ImageLayers layer)//FloatGetSet property, GuiLayout layout)
        {
            //UpdateSliderPosition();
            //slider.Height = (layout.gui.area.Height / layout.totalHeight) * layout.gui.area.Height - style.memberSpacing * 2;
            area = new VectorRect(new Vector2(displayArea.Right, displayArea.Y), new Vector2(scrollerWidth, displayArea.Height));
            
            scrollerRenderer = new RenderTargetDrawContainer(area.Position, area.Size, layer, new List<AbsDraw>());

            Ref.draw.AddToContainer = scrollerRenderer;
            {
                background = new Image(SpriteName.WhiteArea, Vector2.Zero, area.Size, ImageLayers.Background0);
                slider = new Image(SpriteName.WhiteArea, Vector2.Zero, area.Size, ImageLayers.AbsoluteTopLayer);
                slider.LayerAbove(background);
            }
            Ref.draw.AddToContainer = null;
            scrollerRenderer.Visible = false;

            background.Color = Color.Beige;
            slider.Color = Color.Orange;
        }

        public void Refresh(float contentHeight, float displayHeight)
        {
            //TODO begär id för varje meny
            if (contentHeight > displayHeight)
            {
                float displayHeightPerc = displayHeight / contentHeight;
                float sliderH = Math.Max(scrollerRenderer.Height * displayHeightPerc, Engine.Screen.SmallIconSize); 
                slideRange = scrollerRenderer.Height - sliderH;
                valuerange = new IntervalF(0, contentHeight - displayHeight);
                valueT = 0;
                
                slider.size.Y = sliderH;
                
                scrollerRenderer.Visible = true;
            }
            else
            {
                scrollerRenderer.Visible = false;
            }   
        }

        void updateBarPos()
        { 
            
        }

        public void updateMouseInput()
        {
            if (mouseDown)
            {
                if (Input.Mouse.IsButtonDown(MouseButton.Left))
                {
                    float diff = Input.Mouse.Position.Y - mouseDownY;
                    slider.Ypos = Bound.Set(diff + mouseDown_SliderY, 0, slideRange);
                    valueT = valuerange.GetFromPercent(slider.Ypos / slideRange); 
                }
                else
                {
                    mouseDown = false;
                }
            }
            else
            {
                if (area.IntersectPoint(Input.Mouse.Position) &&
                    Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    mouseDown = true;
                    mouseDownY = Input.Mouse.Position.Y;
                    mouseDown_SliderY = slider.Ypos;
                }
            }
        }
    }
        //public override void OnMouseDrag()
        //{
        //    base.OnMouseDrag();
        //    Vector2 localPos = FindLocalCursorPos();
            
        //        localPos.X -= sliderStartPos.X;

        //        if (Input.Keyboard.Ctrl)
        //        {
        //            valueT = MathHelper.Clamp(valueT + (Input.Mouse.MoveDistance.X * 0.1f / slideLength.X), 0, 1);
        //        }
        //        else
        //        {
        //            valueT = MathHelper.Clamp(((localPos.X - slider.Width / 2) / (slideLength.X)), 0, 1);
        //        }
            
        //}
}
