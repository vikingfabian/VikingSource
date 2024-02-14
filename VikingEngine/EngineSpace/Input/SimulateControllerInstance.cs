//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework;

//namespace VikingEngine.Input
//{
//    class SimulateControllerInstance : XboxPadInstance
//    {
//        const Keys SimulateDpadKey = Keys.LeftControl;

//        Keys up = Keys.Up;
//        Keys down = Keys.Down;
//        Keys left = Keys.Left;
//        Keys right = Keys.Right;

//        Dictionary<Buttons, Keys> buttonToKey;

//        public SimulateControllerInstance(PlayerIndex playerIx)
//            : base(playerIx)
//        {
//            buttonToKey = new Dictionary<Buttons, Keys>
//            {
//                { Buttons.A, Keys.Space },
//                { Buttons.B, Keys.B },
//                { Buttons.X, Keys.X },
//                { Buttons.Y, Keys.Y },
//                { Buttons.Start, Keys.Escape },
//                { Buttons.Back, Keys.Back },
//                { Buttons.LeftShoulder, Keys.Q },
//                { Buttons.RightShoulder, Keys.Z },
//                { Buttons.LeftTrigger, Keys.D1 },
//                { Buttons.RightTrigger, Keys.D2 },
//            };
//        }

//        public override bool KeyDownEvent(Buttons button)
//        {
//            if (button == Buttons.DPadLeft)
//            {
//                return Keyboard.IsKeyDown(SimulateDpadKey) && Keyboard.KeyDownEvent(left);
//            }
//            if (button == Buttons.DPadRight)
//            {
//                return Keyboard.IsKeyDown(SimulateDpadKey) && Keyboard.KeyDownEvent(right);
//            }
//            if (button == Buttons.DPadUp)
//            {
//                return Keyboard.IsKeyDown(SimulateDpadKey) && Keyboard.KeyDownEvent(up);
//            }
//            if (button == Buttons.DPadDown)
//            {
//                return Keyboard.IsKeyDown(SimulateDpadKey) && Keyboard.KeyDownEvent(down);
//            }

//            if (buttonToKey.ContainsKey(button))
//            {
//                return Keyboard.KeyDownEvent(buttonToKey[button]);
//            }
//            return false;
//        }

//        override public bool KeyDownEvent(Buttons button1, Buttons button2)
//        {
//            return KeyDownEvent(button1) || KeyDownEvent(button2);
//        }
//        override public bool KeyDownEvent(Buttons button1, Buttons button2, Buttons button3)
//        {
//            return KeyDownEvent(button1) || KeyDownEvent(button2) || KeyDownEvent(button3);
//        }

//        public override bool IsButtonDown(Buttons button)
//        {
//            if (buttonToKey.ContainsKey(button))
//            {
//                return Keyboard.IsKeyDown(buttonToKey[button]);
//            }
//            return false;
//        }

//        bool checkButton(Buttons button)
//        {
//            if (buttonToKey.ContainsKey(button))
//            {
//                if (Keyboard.KeyDownEvent(buttonToKey[button]))
//                {
//                    return true;
//                }
//            }
//            return false;
//        }
        
//        override protected Vector2 RawJoyStickValue(Stick stickType)
//        {
//            switch (stickType)
//            {
//                case Stick.Left:
//                    if (Keyboard.IsKeyDown(Keys.LeftControl) || Keyboard.IsKeyDown(Keys.LeftShift))
//                        return Vector2.Zero;
//                    break;
//                case VikingEngine.Stick.D:
//                    if (!Keyboard.IsKeyDown(SimulateDpadKey))
//                        return Vector2.Zero;
//                    break;
//                case VikingEngine.Stick.Right:
//                    if (!Keyboard.IsKeyDown(Keys.LeftShift))
//                        return Vector2.Zero;
//                    break;
//            }

//            Vector2 value = Vector2.Zero;
//            if (Keyboard.IsKeyDown(left)) { value.X = -1; }
//            else if (Keyboard.IsKeyDown(right)) { value.X = 1; }
//            if (Keyboard.IsKeyDown(up)) { value.Y = 1; }
//            else if (Keyboard.IsKeyDown(down)) { value.Y = -1; }

//            return value;
//        }

//        //public override bool IsConnected
//        //{
//        //    get
//        //    {
//        //        return true;
//        //    }
//        //}
//    }
//}
