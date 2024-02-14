//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.PJ
//{
//    class Flower : AbsTerrain
//    {
//        static readonly IntervalF FireTimeRange = new IntervalF(1000, 2200);
//        static readonly IntervalF SecondFireTimeRange = new IntervalF(1600, 2000);
//        static readonly IntervalF AngleRange = new IntervalF(0.2f, 1.4f);
//        public static float Scale;
//        public static Vector2 FireBallScale;
//        public static float FireballSpeed;
//        public static void Init()
//        {
//            Scale = Engine.Screen.Height * 0.2f;
//            FireBallScale = new Vector2(Scale * 0.35f);
//            FireballSpeed = Engine.Screen.Height * 0.00014f;
//        }

//        int fireCount;
//        FireBall fireBall1;
//        FireBall fireBall2;
//        Graphics.Image pot;
//        Graphics.Image head;
//        Time fireTime = new Time(FireTimeRange.GetRandom());
//        float fireFace = 0;

//        public Flower(float lvl, float xpos)
//            : base(lvl)
//        {
//            fireCount = 1;
//            if (lvl > 0.01f)
//            {
//                if (Ref.rnd.RandomChance(lvl))
//                {
//                    fireCount = 2;
//                }
//            }

//            BirdLib.SetGameLayer();
//            pot = new Graphics.Image(SpriteName.birdFlowerPot, new Vector2(xpos, Level.GroundY - Scale), new Vector2(Scale * 0.5f, Scale), BirdLib.LayerFlowerPot);
//            head = new Graphics.Image(SpriteName.birdFlowerHead1, new Vector2(pot.Center.X, pot.Bottom - Scale * 0.65f), new Vector2(Scale), BirdLib.LayerFlowerHead, true);
//            head.Rotation = AngleRange.GetRandom();
//        }

//        public override void Update(float time, float translation)
//        {
//            if (fireBall1 == null && fireTime.CountDown())
//            {
//                fireBall1 = fire(translation);
//            }
//            else if (fireCount > 1 && fireBall2 == null && fireTime.CountDown())
//            {
//                fireBall2 = fire(translation);
//            }

//            if (fireFace > 0)
//            {
//                fireFace -= time;
//                if (fireFace <= 0)
//                {
//                    head.SetSpriteName(SpriteName.birdFlowerHead1);
//                }
//            }
//        }

//        FireBall fire(float translation)
//        {
//            SoundManager.PlaySound(LoadedSound.flowerfire, SoundPos(translation, head.Xpos));
//            FireBall result = new FireBall(head.Position, head.Rotation);
//            head.SetSpriteName(SpriteName.birdFlowerHead2);
//            fireFace = 400;
//            fireTime.MilliSeconds = SecondFireTimeRange.GetRandom();
//            return result;
//        }

//        public override bool OnScreen(float translation)
//        {
//            bool result = ScreenX(translation, pot.Xpos) < Engine.Screen.Width;
//            return result;
//        }

//        public override void DeleteMe()
//        {
//            BirdLib.SetGameLayer();
//            pot.DeleteMe();
//            head.DeleteMe();
//            if (fireBall1 != null)
//                fireBall1.DeleteMe();
//            if (fireBall2 != null)
//                fireBall2.DeleteMe();
//        }

//        public override CollisionData CollisionIntersect(Physics.IBound2D gamerBound)
//        {
//            if (fireBall1 != null && fireBall1.WillDamage)
//            {
//                if (fireBall1.Bound.Intersect(gamerBound))
//                    return new CollisionData(this);
//            }
//            if (fireBall2 != null && fireBall2.WillDamage)
//            {
//                if (fireBall2.Bound.Intersect(gamerBound))
//                    return new CollisionData(this);
//            }

//            return null;
//        }
//    }

//    class FireBall : AbsUpdateable
//    {
//        const float BoundScale = 22f / 32f;
//        public Physics.CircleBound Bound;
//        Graphics.Image image;
//        Vector2 velocity;
//        Timer.Basic smokeTimer = new Timer.Basic(500, true);
//        Time noDamageTime = new Time(500);

//        public FireBall(Vector2 pos, float angle)
//            :base(true)
//        {
//            BirdLib.SetGameLayer();
//            Rotation1D dir = new Rotation1D(angle - MathHelper.PiOver2);
            
//            image = new Graphics.Image(SpriteName.birdFireball, pos, Flower.FireBallScale, BirdLib.LayerFireBall, true);
//            Bound = new Physics.CircleBound(image.Position, BoundScale * PublicConstants.Half * image.Width);

//            velocity = dir.Direction(Flower.FireballSpeed);

//            Time_Update(200);
//        }

//        public override void Time_Update(float time)
//        {
//            if (PlayState.IsPaused) return;

//            image.Position += velocity * time;
//            image.Rotation -= 0.01f * time;
//            Bound.Center = image.Position;
//            noDamageTime.CountDown();
//            if (smokeTimer.Update())
//            {
//                new FireBallParticle(image.Position);
//            }
//        }
//        public override void DeleteMe()
//        {
//            BirdLib.SetGameLayer();
//            base.DeleteMe();
//            image.DeleteMe();
//        }
//        public bool WillDamage
//        {
//            get { return noDamageTime.TimeOut; }
//        }
//    }

//    class FireBallParticle : AbsUpdateable
//    {
//        Graphics.Image image;
//        Time fadeTimer = new Time(0.5f, TimeUnit.Seconds);

//        public FireBallParticle(Vector2 pos)
//            :base(true)
//        {
//            const float rndDiff = 0.2f;
//            BirdLib.SetGameLayer();
//            pos.X += Ref.rnd.Plus_MinusF(Flower.FireBallScale.X * rndDiff);
//            pos.Y += Ref.rnd.Plus_MinusF(Flower.FireBallScale.Y * rndDiff);

//            image = new Graphics.Image(SpriteName.birdFireballParticle, pos, Flower.FireBallScale, BirdLib.LayerFireballParticle, true);

//        }
//        public override void Time_Update(float time)
//        {
//            if (fadeTimer.CountDown())
//            {
//                image.Opacity -= 0.002f * time;
//                if (image.Opacity <= 0)
//                {
//                    DeleteMe();
//                }
//            }
//        }
//        public override void DeleteMe()
//        {
//            base.DeleteMe();
//            image.DeleteMe();
//        }

//    }
//}
