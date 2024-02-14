//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;

//namespace Game1.LootFest.GameObjects.Toys
//{
//    class RaceDeathRestarter : Update
//    {
//        const float MoveLength = 6;
//        float goalHeight;
//        AbsRC rc;

//        public RaceDeathRestarter(AbsRC rc)
//            :base(true)
//        {
//            this.rc = rc;
//            goalHeight = rc.Position.Y;
//            rc.Position +=new Vector3(0, MoveLength, 0);

//        }
//        public override void Time_Update(float time)
//        {
//            Vector3 pos = rc.Position;
//            pos.Y += -0.008f * time;
//            if (pos.Y <= goalHeight)
//            {
//                pos.Y = goalHeight;
//                rc.LockControls = false;
//                DeleteMe();
//            }
//            rc.Position = pos;
//        }
//    }
//}
