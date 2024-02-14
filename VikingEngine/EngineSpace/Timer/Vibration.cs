//using System;
//using System.IO;

//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
////using Microsoft.Xna.Framework.Storage;
//using Microsoft.Xna.Framework.Media;
//namespace VikingEngine.Timer
//{
//    class Vibration : Update_Old
//    {
//        PlayerIndex player;
//        float timeLeft;

//        public Vibration(int playerIx, float time, float strength, Pan side)
//        {

//            if (//Engine.PlayerData.PlayerIxIsController(playerIx))
//            {
//                //player = (PlayerIndex)playerIx;
//                GamePad.SetVibration(, side.GetSide(strength, true), side.GetSide(strength, false));
//                timeLeft = time;
//                this.AddToUpdateList(true);
//            }
//        }
//        public override void Time_Update(float time)
//        {
//            timeLeft -= time;
//            if (timeLeft < 1)
//            {
//                GamePad.SetVibration(player, 0, 0);
//                DeleteMe();
//                //return true;
//            }
//            //return false;
//        }
//    }
//}
