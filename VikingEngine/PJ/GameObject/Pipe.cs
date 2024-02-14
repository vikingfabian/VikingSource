//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.PJ
//{
//    class Pipe : AbsTerrain
//    {
//        static IntervalF LevelToGap;
//        static float currentY;
//        static IntervalF YRange;
//        static float MaxYChange;
//        static Vector2 Scale;
//        static Vector2 BoundHalfScale;
//        public static void Init()
//        {
//            Scale.X = Engine.Screen.Height * 0.24f;
//            const int TileWCount = 2;
//            const int TileHCount = 7;

//            Scale.Y = Scale.X / TileWCount * TileHCount;
//            int pixH = TileHCount * 32;
//            BoundHalfScale = Scale * new Vector2(42f / 64f, (float)(pixH - 2)/pixH) * PublicConstants.Half;

//            float maxGap = Engine.Screen.Height * 0.325f;//0.32f;
//            float minGap = Engine.Screen.Height * 0.28f;
//            LevelToGap = new IntervalF(maxGap, minGap);

//            YRange = new IntervalF(Engine.Screen.Height * 0.12f, Engine.Screen.Height * 0.5f);
//            MaxYChange = YRange.Length * 0.8f;
//            currentY = YRange.GetFromPercent(0.5f);
//        }

//        float scorePoint;
//        Graphics.Image over, under;
//        Physics.RectangleBound bound1, bound2;
//        Action scorePointEvent = null;

//        public Pipe(float lvl, float xpos, Action scorePointEvent)
//            : base(lvl)
//        {
//            this.scorePointEvent = scorePointEvent;
//            BirdLib.SetGameLayer();

//            float y = currentY;
//            float gap = LevelToGap.GetFromPercent(lvl);
//            over = new Graphics.Image(SpriteName.birdPillar, new Vector2(xpos, y - Scale.Y), Scale, BirdLib.LayerPillar);
//            over.SpriteEffects = SpriteEffects.FlipVertically;
//            //over.Color = Color.Green;
//            under = new Graphics.Image(SpriteName.birdPillar, new Vector2(over.Xpos, y + gap), Scale, BirdLib.LayerPillar);
//            //under.Color = over.Color;

//            bound1 = new Physics.RectangleBound(new RectangleCentered2(over.Center, BoundHalfScale));
//            bound2 = new Physics.RectangleBound(new RectangleCentered2(under.Center, BoundHalfScale));

//            //next pipe
//            currentY = Bound.SetBounds(currentY + Ref.rnd.Plus_MinusF(MaxYChange), YRange);
//            scorePoint = over.Center.X;
//        }

//        public override void DeleteMe()
//        {
//            BirdLib.SetGameLayer();
//            over.DeleteMe();
//            under.DeleteMe();
//        }

//        public override CollisionData CollisionIntersect(Physics.IBound2D gamerBound)
//        {
//            if (scorePointEvent != null && gamerBound.Center.X >= scorePoint)
//            {
//                scorePointEvent();
//                scorePointEvent = null;
//            }

//            if (bound1.Intersect(gamerBound) || bound2.Intersect(gamerBound))
//                return new CollisionData(this);
//            return null;
//        }

//        public override bool OnScreen(float translation)
//        {
//            bool result = over.Xpos + translation < Engine.Screen.Width;
//            return result; 
//        }
//        public override void Update(float time, float translation)
//        {
//            //do nothing
//        }
//    }
//}
