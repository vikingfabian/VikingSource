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
        public VectorRect area;
        public RbInteraction interaction = null;
        protected RichboxGui gui;
        Graphics.RectangleLines outLine;

        RenderTargetDrawContainer renderList = null;//Is a target image, rendering the menu content


    }
}
