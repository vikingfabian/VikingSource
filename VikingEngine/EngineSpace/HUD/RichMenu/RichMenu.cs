using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.HUD.RichMenu
{
    /// <summary>
    /// Creates a scrollable container of richbox content. Will update input, and create tooltips.
    /// </summary>
    class RichMenu
    {
        public RichBoxGroup richBox;
        protected RichBoxContent content = new RichBoxContent();
        protected Graphics.Image bg;
        public VectorRect edgeArea, displayArea;
        public RbInteraction interaction = null;
        //protected RichboxGui gui;
        Graphics.RectangleLines outLine;

        RenderTargetDrawContainer renderList = null;//Is a target image, rendering the menu content
        
        RichBoxSettings settings;

        float scrollerWidth;
        RichScrollbar scrollBar;
        public RichMenu(RichBoxSettings settings, VectorRect edgeArea, Vector2 edgeThickness, ImageLayers layer)
        { 
            this.settings = settings;
            this.edgeArea = edgeArea;
            displayArea = edgeArea;
            displayArea.AddXRadius(-edgeThickness.X);
            displayArea.AddYRadius(-edgeThickness.Y);
            scrollerWidth = Screen.SmallIconSize;

            renderList = new RenderTargetDrawContainer(displayArea.Position, displayArea.Size, layer, new List<AbsDraw>());
            scrollBar = new RichScrollbar(displayArea, scrollerWidth, layer);
        }

        public void Refresh(RichBoxContent content)
        {
            Ref.draw.AddToContainer = renderList;
            {
                richBox = new RichBoxGroup(Vector2.Zero,
                    displayArea.Width, ImageLayers.Lay0, settings, content, true, true, false);                
                
            } Ref.draw.AddToContainer = null;

            scrollBar.Refresh(richBox.area.Height, displayArea.Height);
        }

        public void updateMouseInput()
        {
            scrollBar.updateMouseInput();
        }
    }
}
