//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input.Touch;

//namespace VikingEngine.Engine
//{
//    struct FingerTouch : IUpdate
//    {
//        float timeHold;
//        Vector2 currentPos;
//        Vector2 speedNow;
//        //Vector2 speedLast;
//        //Vector2 speedOldest;
//        //bool removeMe;

//        public FingerTouch(Vector2 startPos)
//            :this()
//        {
//            currentPos = startPos;
//            //speedNow = Vector2.Zero;
//            //speedLast = Vector2.Zero;
//            //speedOldest = Vector2.Zero;
//           // timeHold = 0;
//            //removeMe = false;
//            Update.AddToUpdate(this, true);


//            Engine.Input.MouseClick_Event(startPos, MouseButton.Left, true);
//            Ref.gamestate.MouseClickAreaCheck(startPos, true);
//        }

//        /// <returns>If it should be removed from update list</returns>
//        public bool Input(TouchLocation data)
//        {
//            if (data.State == TouchLocationState.Moved)
//            {
//                Vector2 delta = data.Position;
//                delta.X -= currentPos.X;
//                delta.Y -= currentPos.Y;
//                Engine.Input.MouseMove_Event(data.Position, delta);
//                speedNow = delta;
//                timeHold = 0;
//            }
//            else if (data.State == TouchLocationState.Released)
//            {
//                Engine.Input.MouseClick_Event(data.Position, MouseButton.Left, false);
//                Ref.gamestate.MouseClickAreaCheck(data.Position, false);
//                //Add flick here
//            }
//            return data.State == TouchLocationState.Released || data.State == TouchLocationState.Invalid;
//        }

//        public UpdateType UpdateType { get { return UpdateType.Full; } }
//        public void Time_Update(float time)
//        {
//            timeHold += time;
//            //return removeMe;
//        }
//        public bool SavingThread { get { return false; } }
//        public void Time_LasyUpdate(float time) { }
//        public int SpottedArrayMemberIndex { get; set; }
//        public bool SpottedArrayUseIndex { get { return true; } }
//    }
//}
