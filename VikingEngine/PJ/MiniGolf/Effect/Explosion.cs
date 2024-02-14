using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf.Effect
{
    class Explosion : AbsInGameUpdateable
    {
        float radius;
        Graphics.Image image;
        Time viewTime = new Time(180);
        Ball causingPlayer;
        public Vector2 center;

        public Explosion(Vector2 center, Ball causingPlayer)
            : base(true)
        {
            this.causingPlayer = causingPlayer;

            Ref.draw.CurrentRenderLayer = Draw.BackLayer;

            this.center = center;
            radius = GolfRef.gamestate.BallScale * 1f;
            image = new Graphics.Image(SpriteName.WhiteCirkle,
                center, new Vector2(radius * 2f), GolfLib.BallEffectLayer, true);
            
            Ref.draw.CurrentRenderLayer = Draw.HudLayer;

            DamageOrigin origin = new DamageOrigin();
            origin.center = center;
            origin.fromTerrain = false;

            //Find targets
            Physics.CircleBound bound = new Physics.CircleBound(center, radius);
            
            for (int i = GolfRef.objects.balls.Count - 1; i >= 0; --i)
            {
                var ball = GolfRef.objects.balls[i];
                if (bound.Intersect2(ball.bound).IsCollision)
                {
                    if (ball != causingPlayer)
                    {
                        PjRef.achievements.golfBombHit.Unlock();
                    }
                    ball.takeDamage(origin);
                }
            }
            for (int i = GolfRef.objects.items.Count - 1; i >= 0; --i)
            {
                if (bound.Intersect2(GolfRef.objects.items[i].bound).IsCollision)
                {
                    GolfRef.objects.items[i].takeDamage(origin);
                }
            }
        }

        public override void Time_Update(float time_ms)
        {
            if (viewTime.CountDown())
            {
                DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            image.DeleteMe();
        }
    }
}
