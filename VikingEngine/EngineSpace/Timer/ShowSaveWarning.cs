//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;
//using VikingEngine.Graphics;


//namespace VikingEngine.Timer
//{
//    class ShowSaveWarning : AbsTimer
//    {
//        const float ShowTime = 1000;
//        const float FadeTime = 800;
//        Graphics.TextS saveText;

//        public ShowSaveWarning(bool save)
//            : base(ShowTime + FadeTime, UpdateType.Lazy)
//        {
//            const string SaveTxt = "Saving...";
//            const string LoadTxt = "Loading...";
//            saveText = new Graphics.TextS(LoadedFont.CartoonLarge,
//                new Vector2(Engine.Screen.Width * 0.35f, Engine.Screen.Height * 0.8f), VectorExt.V2(1.2f),
//                Graphics.Align.Zero, TextLib.EmptyString, Color.Blue, ImageLayers.AbsoluteTopLayer);
//            Timer.TextFeed feed = new Timer.TextFeed(0, saveText, save ? SaveTxt : LoadTxt);
            
//            new Timer.UpdateTrigger(ShowTime,
//                new Graphics.Motion2d(Graphics.MotionType.OPACITY,
//                    saveText, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT, FadeTime, false), true);
//            //Lumberjack.GameState.Play.Storage.ShowingSaveWarning = true;
//            //new Timer.Terminator(ShowTime + FadeTime, saveText);
//        }
//        protected override void timeTrigger()
//        {
//            saveText.DeleteMe();
//            //Lumberjack.GameState.Play.Storage.ShowingSaveWarning = false;
//        }
//    }
//}
