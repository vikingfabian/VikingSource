using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.PJ.Strategy
{
    class Draw: Engine.Draw
    {
        protected override void drawEvent()
        {
            spriteBatch.GraphicsDevice.Clear(ClrColor);
            
            Draw2d(StrategyLib.MapLayer);
            Draw2d(StrategyLib.HudLayer);
        }
    }
}
