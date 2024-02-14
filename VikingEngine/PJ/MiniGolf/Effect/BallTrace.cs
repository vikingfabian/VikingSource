using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.PJ.MiniGolf
{
    class BallTrace : AbsUpdateableImage
    {
        float scaleDownSpeed;

        public BallTrace(Vector2 pos, Vector2 scale, SpriteName tile, Color color, float startOpacity)
            : base(tile, pos, scale, GolfLib.BallTraceLayer, true, false, true)
        {
            this.Color = color;
            Opacity = startOpacity;
            scaleDownSpeed = Size1D * 0.8f;

            AddToRender(PJ.MiniGolf.Draw.BackLayer);
        }

        public override void Time_Update(float time_ms)
        {
            if (Ref.isPaused == false)
            {
                Opacity -= Ref.DeltaGameTimeSec * 0.5f;
                Size1D -= scaleDownSpeed * Ref.DeltaGameTimeSec;

                if (Opacity <= 0 || Size1D <= 2f)
                {
                    DeleteMe();
                }
            }
        }
    }
}
