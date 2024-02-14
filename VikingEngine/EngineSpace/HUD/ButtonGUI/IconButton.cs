using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.HUD
{
    class IconButton : ImageGroupButton
    {
        public IconButton(SpriteName iconTile, VectorRect area, ImageLayers layer, ButtonGuiSettings sett)
            : base(area, layer, sett)
        {
            addCoverImage(iconTile);
        }
    }
}
