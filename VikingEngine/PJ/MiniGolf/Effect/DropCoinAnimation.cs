using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    class DropCoinAnimation : AbsUpdateable
    {
        Graphics.Image image;
        Vector2 goalPos;

        public DropCoinAnimation(Ball fromBall, Rotation1D dir, bool longDistance)
            :base(true)
        {
            Ref.draw.CurrentRenderLayer = Draw.ShadowObjLayer;

            image = new Graphics.Image(SpriteName.birdCoin1, fromBall.image.Position, 
                new Vector2(GolfRef.gamestate.CoinScale * 0.05f), GolfLib.BallEffectLayer, true);

            float length = longDistance ? 1.5f : 1f;
            goalPos = image.Position + dir.Direction(GolfRef.gamestate.CoinScale * Ref.rnd.Float(1.0f, 1.2f) * length);

            Ref.draw.CurrentRenderLayer = Draw.HudLayer;
        }

        public override void Time_Update(float time_ms)
        {
            for (int i = 0; i < Ref.GameTimePassed16ms; ++i)
            {
                Vector2 diff = goalPos - image.Position;
                if (diff.Length() < 1f)
                {
                    DeleteMe();
                    return;
                }
                image.Position += diff * 0.26f;

                float szDiff = GolfRef.gamestate.CoinScale - image.Size1D;
                image.Size1D += szDiff * 0.3f;
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            image.DeleteMe();
            new Coin(goalPos, CoinValue.Value1, true);
        }
    }
}
