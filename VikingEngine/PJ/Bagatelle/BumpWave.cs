using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Bagatelle
{
    class BigBumpWavePickupEffect : IChildObject
    {
        static readonly Time ViewTime = new Time(120, TimeUnit.Milliseconds);
        static readonly Time HideTime = new Time(90, TimeUnit.Milliseconds);
        int viewCount = 2;
        Graphics.Image image;
        Time time = ViewTime;
        Ball ball;

        public BigBumpWavePickupEffect(Ball ball, BagatellePlayState state)
        {
            this.ball = ball;
            ball.childObjects.Add(this);

            image = new Graphics.Image(SpriteName.birdBumpWave, ball.image.Position,
                new Vector2(state.BumpWaveGoalScale_Big), ImageLayers.AbsoluteBottomLayer, true);
            image.Opacity = BumpWave.Opacity;
            image.LayerBelow(ball.image);
        }

        public bool update()
        {
            if (time.CountDown())
            {                
                image.Position = ball.position;
                image.Visible ^= true;

                if (image.Visible)
                {
                    time = ViewTime;
                }
                else
                {
                    time = HideTime;

                    if (--viewCount <= 0)
                    {                        
                        return true;
                    }
                }
            }

            return false;
        }

        public void onRemoval()
        {
            image.DeleteMe();
        }
    }

    class BumpWave : AbsGameObject, IChildObject
    {
        public const int CollisionDamage = 2;
        public const float Opacity = 0.5f;

        public Ball ball;
        List<AbsGameObject> hitObjects = new List<AbsGameObject>(4);

        Physics.CircleBound circleBound;
        float goalScale;
        //int otherballsHitCount = 0;
        //float sizeUpSpeed;

        public BumpWave(Ball ball, LocalGamer gamer, bool big, BagatellePlayState state)
            :base()
        {
            setGamer(gamer);
            this.ball = ball;
            this.state = state;

            ball.childObjects.Add(this);

            image = new Graphics.Image(SpriteName.ClickCirkleEffect, ball.image.Position, 
                new Vector2(state.BallScale * 1.0f), ImageLayers.AbsoluteBottomLayer, true);
            image.Opacity = Opacity;
            image.LayerBelow(ball.image);

            goalScale = big ? state.BumpWaveGoalScale_Big : state.BumpWaveGoalScale;

            //sizeUpSpeed = state.BallScale * (big ? 18f : 12f);

            circleBound = new Physics.CircleBound(image.Position, 1);
            bound = circleBound;
        }

        public bool update()
        {
            image.Position = ball.image.Position;
            bound.Center = image.Position;
            image.Size1D += state.BallScale * 12f * Ref.DeltaTimeSec;

            if (localGamer != null)
            {
                circleBound.radius = image.Size1D * 0.42f;
                bound = circleBound;

                foreach (var m in state.gameobjects.list)
                {
                    if (m != ball)
                    {
                        Physics.Collision2D coll = bound.Intersect2(m.bound);
                        if (coll.IsCollision)
                        {
                            if (hitObjects.Contains(m) == false)
                            {
                                if (m is Ball && ((Ball)m).alive)
                                {
                                    localGamer.onBallKill();
                                }

                                hitObjects.Add(m);
                                m.OnHitWaveCollision(ball, localGamer);
                            }
                        }
                    }
                }
            }

            if (image.Size1D >= goalScale)
            {
                return true;
            }

            return false;
        }

        public void onRemoval()
        {
            image.DeleteMe();
        }

        public override Ball GetBall()
        {
            return ball;
        }
    }
}
