//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.DebugExtensions
//{
//    class MuPullScreen : Engine.GameState
//    {
//        float inputDelay = 500;

//        public MuPullScreen()
//            : base()
//        {
//            Engine.Storage.Reset(true);
//            Ref.draw.ClrColor = Color.DarkBlue;
//            new Graphics.TextBoxSimple(LoadedFont.Regular, Engine.Screen.SafeArea.Position, VectorExt.V2(Engine.Screen.TextSize), Graphics.Align.Zero,
//                "Your storage device is disconnected or full." + System.Environment.NewLine + System.Environment.NewLine + "Press A to restart" +
//                System.Environment.NewLine + System.Environment.NewLine + "Hold B for massage", 
//                Color.White, ImageLayers.Lay1, Engine.Screen.SafeArea.Width);
            
//        }
//        //public override void Button_Event(ButtonValue e)
//        //{
//        //    if (inputDelay <= 0)
//        //    {
//        //        if (e.KeyDown && (e.Button == numBUTTON.Start || e.Button == numBUTTON.A || e.Button == numBUTTON.X))
//        //        {
//        //            Ref.main.GameIntroState(true);
//        //        }
//        //        else if (e.Button == numBUTTON.B)
//        //        {
//        //            Engine.XGuide.Vibrate(e.PlayerIx, lib.BoolToInt01(e.KeyDown));
//        //        }
//        //    }
//        //}
//        public override void Time_Update(float time)
//        {
//            inputDelay -= time;
//            base.Time_Update(time);
//        }

//        public override bool UseInputEvents
//        {
//            get
//            {
//                return true;
//            }
//        }

//        //public override Engine.GameStateType Type
//        //{
//        //    get { return Engine.GameStateType.Other; }
//        //}
//    }
//}
