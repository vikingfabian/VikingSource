//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.PJ
//{
//    class Mine : AbsTerrain
//    {
//        static readonly IntervalF WaitTimeRange = new IntervalF(1000f, 2000f);
//        const int SplitterCount = 4;
//        public static Vector2 Scale;
//        public static Vector2 BoundHWidth;
//        public static float ScaleToBound;
//        static IntervalF YRange;
//        public static float SpeedLenght;

//        public static void Init()
//        {
//            Scale = new Vector2(Engine.Screen.Height * 0.1f);
//            ScaleToBound = (22f / 32f) * PublicConstants.Half;
//            BoundHWidth = ScaleToBound * Scale;
//            YRange = new IntervalF(Engine.Screen.Height * 0.2f, Engine.Screen.Height * 0.65f);
//            SpeedLenght = Engine.Screen.Height * 0.00008f;
//        }

        
//        MineSplitter[] splitter = null;
//        Physics.RectangleBound Bound;
//        Graphics.Image image;
//        int numFlashes = 4;
//        bool flash = false;
//        Time explodeTimer = new Time(WaitTimeRange.GetRandom());

//        public Mine(float lvl, float xpos)
//            :base(lvl)
//        {
//            BirdLib.SetGameLayer();
//            image = new Graphics.Image(SpriteName.birdSpikeBall, new Vector2(xpos, YRange.GetRandom()), Scale, BirdLib.LayerMine, true);
//            Bound = new Physics.RectangleBound(new RectangleCentered2(image.Position, BoundHWidth));
//        }

//        public override void Update(float time, float translation)
//        {
//            if (splitter == null)
//            {
//                if (explodeTimer.CountDown())
//                {
//                    flash = !flash;

//                    image.SetSpriteName(flash ? SpriteName.birdSpikeBallFlash : SpriteName.birdSpikeBall);
//                    if (--numFlashes <= 0)
//                    {
//                        Rotation1D dir = Rotation1D.D45;
//                        if (level > 0.2f && Ref.rnd.RandomChance(0.3f))
//                        {
//                            dir = Rotation1D.D0;
//                        }
//                        splitter = new MineSplitter[SplitterCount];
//                        for (int i = 0; i < SplitterCount; ++i)
//                        {//EXPLODING
//                            SoundManager.PlaySound(LoadedSound.minefire, SoundPos(translation, image.Xpos));
//                            splitter[i] = new MineSplitter(dir, image.Position, i);
//                            dir.Add(Rotation1D.D90);
//                        }

//                        image.DeleteMe();
//                    }
//                    else
//                    {
//                        explodeTimer.MilliSeconds = 200;
//                    }
//                }
//            }
//        }

//        public override void DeleteMe()
//        {
//            BirdLib.SetGameLayer();
//            if (splitter == null)
//            {
//                image.DeleteMe();
//            }
//            else
//            {
//                for (int i = 0; i < SplitterCount; ++i)
//                {
//                    splitter[i].DeleteMe();
//                }
//            }
//        }

//        public override CollisionData CollisionIntersect(Physics.IBound2D gamerBound)
//        {
//            if (splitter != null)
//            {
//                for (int i = 0; i < SplitterCount; ++i)
//                {
//                    if (splitter[i].Bound.Intersect(gamerBound))
//                    {
//                        return new CollisionData(this);
//                    }
//                }

//            }
//            else
//            {
//                if (Bound.Intersect(gamerBound))
//                {
//                    return new CollisionData(this);
//                }
//            }

//            return null;
//        }

//        public override bool OnScreen(float translation)
//        {
//            bool result = ScreenX(translation, image.Xpos) < Engine.Screen.Width;
//            return result;
//        }

//    }

//    class MineSplitter : AbsUpdateable
//    {
//        const float MineToSplitterScale = 0.8f;

//        public Physics.RectangleBound Bound;
//        Graphics.Image image;
//        Vector2 velocity;

//        public MineSplitter(Rotation1D dir, Vector2 pos, int index)
//            :base(true)
//        {
//            BirdLib.SetGameLayer();
//            image = new Graphics.Image(SpriteName.birdSpikeBall, pos, Mine.Scale * MineToSplitterScale, BirdLib.LayerMineSplitter, true);
//            image.PaintLayer += index * PublicConstants.LayerMinDiff;
//            Bound = new Physics.RectangleBound(new RectangleCentered2(image.Position, Mine.BoundHWidth * MineToSplitterScale));

//            velocity = dir.Direction(Mine.SpeedLenght);
//        }
//        public override void Time_Update(float time)
//        {
//            if (PlayState.IsPaused) return;

//            image.Position += velocity * time;
//            Bound.Center = image.Position;
//        }

//        public override void DeleteMe()
//        {
//            BirdLib.SetGameLayer();
//            image.DeleteMe();
//            base.DeleteMe();
//        }
//    }
//}
