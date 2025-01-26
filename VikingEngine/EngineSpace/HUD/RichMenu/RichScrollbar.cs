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
        //RenderTargetDrawContainer scrollerRenderer = null;

        NineSplitAreaTexture slider;
        NineSplitAreaTexture background;
        IntervalF valuerange;
        public float scrollResult;
        float slideRange;
        bool mouseDown = false;
        float mouseDownY;
        float mouseDown_SliderY;
        VectorRect area;
        float rowHeight;
        NineSplitSettings sliderTex;
        float sliderHeight = 1;
        ImageLayers layer;
        ImageGroupParent2D sliderGroup;
        Graphics.RectangleLines selectionOutline = null;

        public RichScrollbar(NineSplitSettings sliderTex, NineSplitSettings backgroundTex, 
            VectorRect displayArea, float scrollerWidth, ImageLayers layer)
        {
            this.layer = layer;
            area = new VectorRect(new Vector2(displayArea.Right - 6, displayArea.Y), new Vector2(scrollerWidth, displayArea.Height));
            background = new NineSplitAreaTexture(backgroundTex, area, layer +1);
            slider = null;
            this.sliderTex = sliderTex;
        }

        public VectorRect IncludeScrollArea(VectorRect menuarea)
        { 
            menuarea.SetRight(area.Right, true);
            return menuarea;
        }

        public void Refresh(float contentHeight, float displayHeight, float rowHeight)
        {
            this.rowHeight = rowHeight;

            //TODO begär id för varje meny
            if (contentHeight > displayHeight)
            {
                float displayHeightPerc = displayHeight / contentHeight;
                float sliderH = Math.Max(area.Height * displayHeightPerc, Engine.Screen.MinClickSize); 
                slideRange = area.Height - sliderH;
                valuerange = new IntervalF(0, contentHeight - displayHeight);
                scrollResult = 0;

                sliderHeight = sliderH;

                setVisible(true);
            }
            else
            {
                setVisible(false);
            }   
        }

        void setVisible(bool visible)
        {
            background.SetVisible(visible);

            if (visible)
            {
                //VectorRect size = area;
                //size.Height = sliderHeight;
                slider = new NineSplitAreaTexture(sliderTex, SliderArea(false), layer);
                if (sliderGroup == null)
                {
                    sliderGroup = new ImageGroupParent2D();
                }
                sliderGroup.Add(slider.images);
            }
            else
            { 
                slider.DeleteMe();
                sliderGroup.Clear();
            }

        }

        VectorRect SliderArea(bool includeMoveY)
        {
            VectorRect ar = area;
            if (includeMoveY)
            {
                ar.Y += sliderGroup.ParentY;
            }
            ar.Height = sliderHeight;
            return ar;
        }

        public bool updateMouseInput()
        {
            bool viewOutline = false;
            bool result = false;

            if (mouseDown)
            {
                viewOutline = true;
                if (Input.Mouse.IsButtonDown(MouseButton.Left))
                {                    
                    float diff = Input.Mouse.Position.Y - mouseDownY;
                    sliderGroup.ParentY = Bound.Set(diff + mouseDown_SliderY, 0, slideRange);
                    scrollResult = -valuerange.GetFromPercent(sliderGroup.ParentY / slideRange);

                    result = true;
                }
                else
                {
                    slider.SetColor(Color.White);
                    mouseDown = false;
                }
            }
            else
            {
                if (SliderArea(true).IntersectPoint(Input.Mouse.Position))
                {
                    viewOutline = true;

                    if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                    {
                        mouseDown = true;
                        mouseDownY = Input.Mouse.Position.Y;
                        mouseDown_SliderY = sliderGroup.ParentY;
                        slider.SetColor(RichBox.Artistic.ArtButton.MouseDownCol);
                    }
                }
            }

            if (viewOutline)
            {
                if (selectionOutline == null)
                {
                    selectionOutline = new RectangleLines(SliderArea(true), 2, 1, layer - 1);
                }
                else
                {
                    selectionOutline.Refresh(SliderArea(true));
                }
            }
            else
            {
                selectionOutline?.DeleteMe();
                selectionOutline = null;
            }

            return result;
        }

        public bool updateScrollWheel()
        {
            if (Input.Mouse.Scroll)
            {
                scrollResult = -valuerange.SetBounds(-scrollResult - Input.Mouse.ScrollValue * rowHeight * 2 / 15f);
                sliderGroup.ParentY = slideRange * valuerange.GetValuePercentPos(-scrollResult);
                selectionOutline?.Refresh(SliderArea(true));
                return true;
            }

            return false;
        }

    }
        
}
