//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest
//{
//    class CompassPulse : Timer.AbsRepeatingTrigger
//    {
//        const int NumPulses = 25;
//        int pulses = NumPulses;
//        Players.Player p;
//        public CompassPulse(Players.Player p)
//            :base(400, UpdateType.Lasy)
//        {

//            this.p = p;
//        }
//        protected override void timeTrigger()
//        {
//#if !CMODE
//            pulses--;
//            if (pulses <= 0)
//            {
//                DeleteMe();
//            }

//            //create pulse
//            if (p.Mode != Players.PlayerMode.Map)
//            {
//                const float StartTransparentsy = 0.9f;
//                const float ScaleTime = 1800;
//                const float StartFadeOut = ScaleTime * 0.4f;
//                const float FadeOutTime = ScaleTime - StartFadeOut;

//                Graphics.Image ring = new Graphics.Image(SpriteName.InterfaceThinCirkle, p.compass.CompassQuestMarkPos, Vector2.Zero, ImageLayers.AbsoluteTopLayer, true);
//                ring.Transparensy = StartTransparentsy;
//                Graphics.Motion2d scaleUp = new Graphics.Motion2d(Graphics.MotionType.SCALE, ring, Vector2.One * 200,
//                    Graphics.MotionRepeate.NO_REPEAT, ScaleTime, true);
//                Graphics.Motion2d fadeOut = new Graphics.Motion2d(Graphics.MotionType.TRANSPARENSY, ring,
//                    Vector2.One * - StartTransparentsy, Graphics.MotionRepeate.NO_REPEAT, FadeOutTime, false);
//                new Timer.UpdateTrigger(StartFadeOut, fadeOut, true);
//                new Timer.Terminator(ScaleTime, ring);
                    
//            }
//#endif
//        }
//    }
//}
