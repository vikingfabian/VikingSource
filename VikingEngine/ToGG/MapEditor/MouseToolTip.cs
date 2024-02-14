using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.ToggEngine.MapEditor
{
    class MouseToolTip : ToggEngine.Display2D.AbsToolTip
    {
        public Graphics.Image toolicon;

        public MouseToolTip(MapControls mapcontrols)
           : base(mapcontrols)
        {
            toolicon = new Graphics.Image(SpriteName.NO_IMAGE, Vector2.Zero, Engine.Screen.SmallIconSizeV2, Layer);
            Add(toolicon);

            completeSetup(toolicon.Size);
        }
    }
}
