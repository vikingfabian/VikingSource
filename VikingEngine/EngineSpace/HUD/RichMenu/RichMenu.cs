using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public VectorRect area, contentArea;
        public RbInteraction interaction = null;
        //protected RichboxGui gui;
        Graphics.RectangleLines outLine;

        RenderTargetDrawContainer renderList = null;//Is a target image, rendering the menu content
        RichBoxSettings settings;
        public RichMenu(RichBoxSettings settings, VectorRect area, Vector2 edgeThickness, ImageLayers layer)
        { 
            this.settings = settings;
            this.area = area;
            contentArea = area;
            contentArea.AddXRadius(-edgeThickness.X);
            contentArea.AddYRadius(-edgeThickness.Y);

            renderList = new RenderTargetDrawContainer(contentArea.Position, contentArea.Size, layer, new List<AbsDraw>());
        }

        public void Refresh(RichBoxContent content)
        {
            Ref.draw.AddToContainer = renderList;
            {
                richBox = new RichBoxGroup(Vector2.Zero,
                    contentArea.Width, ImageLayers.Lay0, settings, content, true, true, false);
            }
            Ref.draw.AddToContainer = null;

        }
    }
}
