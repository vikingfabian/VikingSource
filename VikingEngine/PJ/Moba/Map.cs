using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Moba
{
    class Map
    {
        public Graphics.Line line;

        public Map()
        {
            MobaRef.map = this;

            Ref.draw.ClrColor = new Color(63, 99, 0);

            line = new Graphics.Line(MobaLib.UnitScale * 0.5f, ImageLayers.Lay0, Color.SandyBrown,
                Engine.Screen.SafeArea.LeftCenter,
                Engine.Screen.SafeArea.RightCenter);

            new GO.Tower(true);
            new GO.Tower(false);

        }
    }
}
