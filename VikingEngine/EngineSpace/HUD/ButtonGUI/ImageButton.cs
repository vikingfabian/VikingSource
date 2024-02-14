using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD
{
    class ImageButton : AbsButtonGui
    {
        public ImageButton(SpriteName sprite, VectorRect area, ImageLayers layer, ButtonGuiSettings sett)
            : base(sett)
        {
            this.area = area;
            baseImage = new Graphics.Image(sprite, area.Position, area.Size, layer, false);
            createHighlight();
        }
    }
}
