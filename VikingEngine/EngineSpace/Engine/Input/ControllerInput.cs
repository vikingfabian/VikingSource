//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

//namespace VikingEngine.Engine
//{
//    class ControllerInput
//    {
//        const float TriggerBuffer = 0.01f;
//        static Vector2 DpadState = new Vector2();

//        static readonly int NumSticks = (int)Stick.NUM;
//        static readonly int NumButtons = (int)numBUTTON.NUM;

//        static Vector2[] padRawValues = new Vector2[NumSticks];
//        static bool[] bCurrentButtonStates = new bool[NumButtons];
//        public bool IsConnected = false;

//        PlayerIndex playeIndex;
//        VikingEngine.Input.AbsControllerInstance controllerInstance;

//        JoyStickValue joyStickVal = new JoyStickValue();
//        ButtonValue buttonVal = new ButtonValue();

//        StepBuffert stepX = new StepBuffert();
//        StepBuffert stepY = new StepBuffert();

//        //bool[] GamerInput = new bool[NumButtons];
//        bool[] pad = new bool[NumSticks];

//        public ControllerInput(int ix)
//        {
//            playeIndex = (PlayerIndex)ix;
//            joyStickVal.ContolIx = ix;
//            controllerInstance = VikingEngine.Input.Controller.Instance(ix);
//        }

//        public void SimulateController(Vector2 arrows, bool shift, bool ctrl)
//        {
//            //Input.KeyCheck(Keys.Q, numBUTTON.LT, buttonVal.PlayerIx);
//            //Input.KeyCheck(Keys.W, numBUTTON.RT, buttonVal.PlayerIx);
//            //Input.KeyCheck(Keys.E, numBUTTON.LB, buttonVal.PlayerIx);
//            //Input.KeyCheck(Keys.R, numBUTTON.RB, buttonVal.PlayerIx);
//            //Input.KeyCheck(Keys.Space, numBUTTON.A, buttonVal.PlayerIx);
//            //Input.KeyCheck(Keys.B, numBUTTON.B, buttonVal.PlayerIx);
//            //Input.KeyCheck(Keys.X, numBUTTON.X, buttonVal.PlayerIx);
//            //Input.KeyCheck(Keys.Y, numBUTTON.Y, buttonVal.PlayerIx);

//            //Input.KeyCheck(Keys.Back, numBUTTON.Back, buttonVal.PlayerIx);
//            //Input.KeyCheck(Keys.Escape, numBUTTON.Start, buttonVal.PlayerIx);


//            //ARROWS+shift simulate right pad
//            //ARROWS simulate left pad
//            joyStickVal.Direction = arrows;
//            joyStickVal.Time();
//            if (joyStickVal.Direction != Vector2.Zero)
//            {
//                joyStickVal.Stick = Stick.Left;
//                if (shift) { joyStickVal.Stick = Stick.Right; }
//                else if (ctrl) { joyStickVal.Stick = Stick.D; }
//                joyStickVal.KeyDownEvent = !pad[(int)joyStickVal.Stick];
//                joyStickVal.Stepping.X = stepX.Update(joyStickVal.Direction.X);
//                joyStickVal.Stepping.Y = stepY.Update(joyStickVal.Direction.Y);
//                //Ref.gamestate.Pad_Event(joyStickVal);
//                pad[(int)joyStickVal.Stick] = true;
//            }
//            else
//            {
//                pad[(int)joyStickVal.Stick] = false;
//                stepX.PadUp();
//                stepY.PadUp();
//            }
//        }

//        public void UpdateInput()
//        {
//            if (GamePad.GetState(playeIndex).IsConnected)
//            {
//                //new
//                for (numBUTTON button = 0; button < numBUTTON.NUM; ++button)
//                {
//                    if (controllerInstance.KeyChangedEvent(VikingEngine.Input.InputLib.ButtonConvert(button)))
//                    {
//                        //button state changed, throw event
//                        buttonVal.Button = button;
//                        buttonVal.KeyDown = controllerInstance.IsButtonDown(VikingEngine.Input.InputLib.ButtonConvert(button));
//                        //Ref.gamestate.Button_Event(buttonVal);
//                    }
//                }

     
//                //Put all direction pads in a list

//                padRawValues[(int)Stick.Left] = GamePad.GetState(playeIndex).ThumbSticks.Left;
//                padRawValues[(int)Stick.Right] = GamePad.GetState(playeIndex).ThumbSticks.Right;
//                DpadState = Vector2.Zero;
//                if (GamePad.GetState(playeIndex).DPad.Left == ButtonState.Pressed) { DpadState.X = -1; }
//                else if (GamePad.GetState(playeIndex).DPad.Right == ButtonState.Pressed) { DpadState.X = 1; }
//                if (GamePad.GetState(playeIndex).DPad.Up == ButtonState.Pressed) { DpadState.Y = 1; }
//                else if (GamePad.GetState(playeIndex).DPad.Down == ButtonState.Pressed) { DpadState.Y = -1; }
//                padRawValues[(int)Stick.D] = DpadState;

//                for (int padIx = 0; padIx < NumSticks; padIx++)
//                {
//                    Vector2 value = padRawValues[padIx];
//                    if (Math.Abs(value.X) > ControlSense.AnalogBuffert ||
//                        Math.Abs(value.Y) > ControlSense.AnalogBuffert)
//                    {
//                        joyStickVal.InverseDir(value);
//                        joyStickVal.Time();
//                        joyStickVal.Stick = (Stick)padIx;
//                        joyStickVal.KeyDownEvent = !pad[padIx];
//                        //lägg till steppping
//                        joyStickVal.Stepping.X = stepX.Update(joyStickVal.Direction.X);
//                        joyStickVal.Stepping.Y = stepY.Update(joyStickVal.Direction.Y);

//                       // Ref.gamestate.Pad_Event(joyStickVal);
//                        pad[padIx] = true;
//                    }
//                    //current release will give padup event
//                    else if (pad[padIx])
//                    {
//                       // Ref.gamestate.PadUp_Event((Stick)padIx, (int)playeIndex);
//                        pad[padIx] = false;
//                        stepX.PadUp();
//                        stepY.PadUp();
//                    }
//                }



//            }//End IF Isconnected
//        }
//    }
//}
