//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.PJ
//{
//    class Cannon : AbsTerrain
//    {
//        Bullet bullet = null;
//        float xpos;

//        public Cannon(float lvl, float xpos)
//            : base(lvl)
//        {
//            this.xpos = xpos;
//        }

//        public override void Update(float time, float translation)
//        {
//            if (bullet == null)
//            {
//                if (OnScreen(translation))
//                    bullet = new Bullet(xpos, level);
//            }
//            else
//            {
//                bullet.update(time);
//            }
//        }

//        public override bool OnScreen(float translation)
//        {
//            bool result = ScreenX(translation, xpos) < Engine.Screen.Width;
//            return result;
//        }

//        public override void DeleteMe()
//        {
//            if (bullet != null)
//                bullet.DeleteMe();
//        }

//        public override CollisionData CollisionIntersect(Physics.IBound2D gamerBound)
//        {
//            if (bullet != null)
//            {
//                if (bullet.Bound.Intersect(gamerBound))
//                    return new CollisionData(this);
//            }

//            return null;
//        }

       
//    }

//    class Bullet
//    {
//        public static Vector2 Scale;
//        static Vector2 BoundHWidth;
//        static IntervalF YRange;
//        static IntervalF LevelToSpeedX;// = -0.16f;
//        static IntervalF LevelToRndSpeedAdd;

//        public static void Init()
//        {
//            Scale.Y = Engine.Screen.Height * 0.1f;
//            Scale.X = Scale.Y * 2;

//            BoundHWidth = Scale * new Vector2(19f / 32f, 22f / 32f) * PublicConstants.Half;

//            YRange = new IntervalF(Engine.Screen.Height * 0.15f, Engine.Screen.Height * 0.8f);

//            float minSpeedX = -Engine.Screen.Height * 0.00022f;
//            float maxSpeedX = -Engine.Screen.Height * 0.00020f;
//            LevelToSpeedX = new IntervalF(minSpeedX, maxSpeedX);

//            LevelToRndSpeedAdd = new IntervalF(1, 1.5f);
//        }

//        Timer.Basic smokeTimer = new Timer.Basic(300, true);
//        public Physics.RectangleBound Bound;
//        Graphics.Image image;
//        float speedX;

//        public Bullet(float cannonX, float lvl)
//        {
//            BirdLib.SetGameLayer();
//            image = new Graphics.Image(SpriteName.birdCannonBall, new Vector2(cannonX + Scale.X, YRange.GetRandom()), Scale, BirdLib.LayerCannonball, true);
//            Bound = new Physics.RectangleBound(new RectangleCentered2(image.Position, BoundHWidth));
//            speedX = LevelToSpeedX.GetFromPercent(lvl) * Ref.rnd.Float(1f, LevelToRndSpeedAdd.GetFromPercent(lvl));
//        }

//        public void update(float time)
//        {
//            image.Xpos += speedX * time;
//            Bound.Center = image.Position;
//            if (smokeTimer.Update())
//            {
//                new BulletSmoke(image.Position);
//            }
//        }

//        public void DeleteMe()
//        {
//            image.DeleteMe();
//        }
//    }

//    class BulletSmoke : AbsUpdateable
//    {
//        Graphics.Image img;
//        int state_Grow_Hold_Fade = 0;
//        Time stateTime;
//        float goalWidth = Bullet.Scale.Y * 0.9f;

//        static int layerAdd = 0;

//        public BulletSmoke(Vector2 pos)
//            :base(true)
//        {
//            BirdLib.SetGameLayer();
//            pos.X += Ref.rnd.Plus_MinusF(Bullet.Scale.Y * 0.3f);
//            pos.Y += Ref.rnd.Float(Bullet.Scale.Y * 0.2f);

//            img = new Graphics.Image(lib.RandomBool() ? SpriteName.birdCannonBallSmoke1 : SpriteName.birdCannonBallSmoke2, pos, Vector2.Zero, BirdLib.LayerCannonballSmoke, true);
//            img.PaintLayer += layerAdd * PublicConstants.LayerMinDiff;
//            img.Rotation += Ref.rnd.Int(4) * MathHelper.PiOver2;
//            if (--layerAdd < -10) layerAdd = 0;

//        }

//        public override void Time_Update(float time)
//        {
//            switch (state_Grow_Hold_Fade)
//            {
//                case 0:
//                    img.Size += new Vector2(0.0024f * goalWidth * time);
//                    if (img.Width >= goalWidth)
//                    {
//                        state_Grow_Hold_Fade++;
//                        stateTime.Seconds = 0.5f;
//                    }
//                    break;
//                case 1:
//                    if (stateTime.CountDown())
//                    {
//                        state_Grow_Hold_Fade++;
//                    }
//                    break;
//                case 2:
//                    img.Opacity -= 0.002f * time;
//                    if (img.Opacity <= 0)
//                    {
//                        this.DeleteMe();
//                    }
//                    break;
//            }
//        }

//        public override void DeleteMe()
//        {
//            BirdLib.SetGameLayer();
//            base.DeleteMe();
//            img.DeleteMe();
//        }
//    }
//}
