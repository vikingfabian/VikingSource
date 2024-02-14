using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace VikingEngine.PJ
{
    class DeathFlash : AbsUpdateable
    {
        Graphics.Image whiteArea = null;

        public DeathFlash()
            :base(true)
        {
        }
        public override void Time_Update(float time)
        {
            //Ref.draw.CurrentRenderLayer = BirdLib.RenderLayerHUD;
            if (whiteArea == null)
            {
                whiteArea = new Graphics.Image(SpriteName.WhiteArea, new Vector2(-20), Engine.Screen.ResolutionVec + new Vector2(40), ImageLayers.Background1);
                whiteArea.Opacity = 0.5f;
            }
            else
            {
                whiteArea.DeleteMe();
                this.DeleteMe();
            }
        }
    }
}
