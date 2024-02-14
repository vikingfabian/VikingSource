//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.PJ
//{
//    class Coin : AbsTerrain
//    {
//        public static readonly SpriteName[] Animation = new SpriteName[]
//        {
//            SpriteName.birdCoin1,
//            SpriteName.birdCoin2,
//            SpriteName.birdCoin3,
//            SpriteName.birdCoin4,
//            SpriteName.birdCoin5,
//            SpriteName.birdCoin6,
//        };
//        public static Vector2 Scale;
//        static IntervalF YRange;
//        static IntervalF FallingYRange;
//        static float FallSpeed;
//        public static void Init()
//        {
//            Scale = new Vector2(Engine.Screen.Height * 0.06f);
//            YRange = new IntervalF(Engine.Screen.Height * 0.25f, Engine.Screen.Height * 0.6f);
//            FallingYRange = new IntervalF(0, Engine.Screen.Height);
//            CoinParticles.Init();
//            FallSpeed = Engine.Screen.Height * 0.00015f;
//        }

//        Timer.Basic animationTime = new Timer.Basic(132, true);
//        CirkleCounterUp animFrame = new CirkleCounterUp(0, Animation.Length - 1);
//        Graphics.Image image;
//        Physics.CircleBound Bound;

//        bool falling;

//        public Coin(float x, bool falling)
//            :base(0)
//        {
//            this.falling = falling;
//            BirdLib.SetGameLayer();

//            float y;
//            if (falling)
//                y = FallingYRange.GetRandom();
//            else
//                y = YRange.GetRandom();

//            image = new Graphics.Image(SpriteName.birdCoin1, new Vector2(x, y), Scale, BirdLib.LayerCoin, true);
//            Bound = new Physics.CircleBound(image.Position, image.Width * 0.7f);
//        }

//        public override void Update(float time, float translation)
//        {
//            if (animationTime.Update())
//            {
//                image.SetSpriteName(Animation[animFrame.Next()]);
//            }

//            if (falling)
//            {
//                image.Ypos += FallSpeed * time;
//                if (image.Ypos - image.Height > Engine.Screen.Height)
//                {
//                    image.Ypos = -image.Height;
//                }
//                Bound.Center = image.Center;
//            }
//        }

//        public override CollisionData CollisionIntersect(Physics.IBound2D gamerBound)
//        {
//            if (Bound.Intersect(gamerBound))
//                return new CollisionData(this);
//            return null;
//        }
//        public override bool OnScreen(float translation)
//        {
//            bool result = ScreenX(translation, image.Xpos) < Engine.Screen.Width;
//            return result;
//        }

//        public void PickUp()
//        {
//            new CoinParticles(image.Position);
//        }
//        override public void DeleteMe()
//        {
//            BirdLib.SetGameLayer();
//            image.DeleteMe();
//        }
//    }

//    class CoinParticles : AbsUpdateable
//    {
//        const int ParticleCount = 4;
//        static Vector2[] Velocity;
//        public static void Init()
//        {
//            float SpeedLenght = Engine.Screen.Height * 0.0003f;

//            Velocity = new Vector2[ParticleCount]
//            {
//                new Vector2(-SpeedLenght, -SpeedLenght),
//                new Vector2(SpeedLenght, -SpeedLenght),
//                new Vector2(-SpeedLenght, SpeedLenght),
//                new Vector2(SpeedLenght, SpeedLenght),
//            };
//        }

//        float transparentsy = 1f;
//        Graphics.Image[] images;

//        public CoinParticles(Vector2 pos)
//            :base(true)
//        {
//            BirdLib.SetGameLayer();
//            images = new Graphics.Image[ParticleCount];
//            for (int i = 0; i < ParticleCount; ++i)
//            {
//                images[i] = new Graphics.Image(SpriteName.birdCoinParticle, pos, Coin.Scale, BirdLib.LayerCoin, true);
//                images[i].PaintLayer += PublicConstants.LayerMinDiff * i;
//            }
//        }
//        public override void Time_Update(float time)
//        {
//            for (int i = 0; i < ParticleCount; ++i)
//            {
//                images[i].Position += Velocity[i] * time;
//                images[i].Opacity = transparentsy;
//            }

//            transparentsy -= 0.0023f * time;
//            if (transparentsy <= 0)
//            {
//                DeleteMe();
//            }
//        }
//        public override void DeleteMe()
//        {
//            BirdLib.SetGameLayer();
//            base.DeleteMe();
//            for (int i = 0; i < ParticleCount; ++i)
//            {
//                images[i].DeleteMe();
//            }
//        }

//    }
//}
