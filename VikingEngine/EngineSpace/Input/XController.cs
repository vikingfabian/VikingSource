using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace VikingEngine.Input
{
    class XController
    {
        const float TriggerBuffer = 0.01f;

        ThumbStick[] thumbSticks;
        int index;

        public int Index { get { return index; } }
        public GamePadState currentPadState;
        public GamePadState previousPadState;
        public bool hasUser = false;
        public bool var_waitingForStickRelease = false;

        Time vibrationTime = Time.Zero;
        float vibrationLeftMotor = 0, vibrationRightMotor = 0;

        public XController(int index)
        {
            this.index = index;
            thumbSticks = new ThumbStick[(int)ThumbStickType.NUM_NON];
            for (int i = 0; i < thumbSticks.Length; i++)
            {
                thumbSticks[i] = new ThumbStick((ThumbStickType)i, (int)index);
            }

            var cap = GamePad.GetCapabilities(index);
           
        }

        public static int ButtonB_KeyUpTime = 0;
        public void Update()
        {
            previousPadState = currentPadState;
            currentPadState = GamePad.GetState(index);
#if XBOX
            if (currentPadState.IsButtonUp(Buttons.B) && previousPadState.IsButtonDown(Buttons.B))
            {
                ButtonB_KeyUpTime = 4;
            }            
#endif
            if (vibrationTime.HasTime)
            {
                GamePad.SetVibration(index, vibrationLeftMotor, vibrationRightMotor);

                if (vibrationTime.CountDown())
                {
                    endVibration();
                }
            }
        }

        public bool BackButtonDownEvent()
        { //Locks the Back button three frames after B keyUp
#if XBOX
            if (ButtonB_KeyUpTime > 0)
            {
                return false;
            }
#endif
            return currentPadState.IsButtonDown(Buttons.Back) && previousPadState.IsButtonUp(Buttons.Back);
        }

        

         public bool Connected
        {
            get { return currentPadState.IsConnected || PlatformSettings.SimulateJoinedControllers; }
        }

        /// <summary>
        /// is the button down/up right now
        /// </summary>
        /// <returns>is keydown</returns>
        public bool IsButtonDown(Buttons button)
        {
            return currentPadState.IsButtonDown(button);
        }

        public List<Buttons> listAllButtonDown()
        {
            List<Buttons> result = new List<Buttons>();
            foreach (var m in XInput.AllButtons)
            {
                if (currentPadState.IsButtonDown(m))
                {
                    result.Add(m);
                }
            }

            return result;
        }

        /// <summary>
        /// is the button down/up right now
        /// </summary>
        /// <returns>is keydown</returns>
        public bool IsButtonDown(ButtonGroup buttons)
        {
            bool result = currentPadState.IsButtonDown(buttons.button1);

            if (buttons.Count > 1 && !result)
            {
                result = currentPadState.IsButtonDown(buttons.button2);
                if (buttons.Count > 2 && !result)
                {
                    result = currentPadState.IsButtonDown(buttons.button3);
                }
            }

            return result;
        }

        public bool KeyDownEvent(Buttons button1, Buttons button2, Buttons button3)
        {
            return (previousPadState.IsButtonUp(button1) && currentPadState.IsButtonDown(button1)) ||
                (previousPadState.IsButtonUp(button2) && currentPadState.IsButtonDown(button2)) ||
                (previousPadState.IsButtonUp(button3) && currentPadState.IsButtonDown(button3));
        }

        public bool KeyDownEvent(Buttons button1, Buttons button2)
        {
            return (previousPadState.IsButtonUp(button1) && currentPadState.IsButtonDown(button1)) ||
                (previousPadState.IsButtonUp(button2) && currentPadState.IsButtonDown(button2));
        }

        /// <summary>
        /// If button was pressed this frame
        /// </summary>
        public bool KeyDownEvent(Buttons button)
        {
            if (button == Buttons.Back)
            {
                return BackButtonDownEvent();
            }

            bool result = previousPadState.IsButtonUp(button) && currentPadState.IsButtonDown(button);
            return result;
        }

        /// <summary>
        /// If button was pressed this frame
        /// </summary>
        public bool KeyDownEvent(ButtonGroup buttons)
        {
            bool result = previousPadState.IsButtonUp(buttons.button1) && currentPadState.IsButtonDown(buttons.button1);

            if (buttons.Count > 1 && !result)
            {
                result = previousPadState.IsButtonUp(buttons.button2) && currentPadState.IsButtonDown(buttons.button2);
                if (buttons.Count > 2 && !result)
                {
                    result = previousPadState.IsButtonUp(buttons.button3) && currentPadState.IsButtonDown(buttons.button3);
                }
            }

            return result;
        }
        

        /// <summary>
        /// If button was released this frame
        /// </summary>
        public bool KeyUpEvent(Buttons button)
        {
            return previousPadState.IsButtonDown(button) && currentPadState.IsButtonUp(button);
        }

        /// <summary>
        /// if the key status was different previous frame
        /// </summary>
        public bool KeyChangedEvent(Buttons button)
        {
            return previousPadState.IsButtonDown(button) != currentPadState.IsButtonDown(button);
        }

        /// <summary>
        /// if the key status was different previous frame
        /// </summary>
         public bool KeyChangedEvent(Buttons button, out bool keyDown)
        {
            keyDown = currentPadState.IsButtonDown(button);
            return previousPadState.IsButtonDown(button) != keyDown;
        }


         protected bool checkKeyDownEvent(Buttons button)
        {
            return previousPadState.IsButtonUp(button) && currentPadState.IsButtonDown(button);
        }
         protected bool checkKeyUpEvent(Buttons button)
        {
            return previousPadState.IsButtonDown(button) && currentPadState.IsButtonUp(button);
        }
         protected bool checkButtonPressed(Buttons button)
        {
            return currentPadState.IsButtonDown(button);
        }
        
        public float LeftTrigger
        {
            get
            {
                return InputLib.SetBuffer(currentPadState.Triggers.Left, TriggerBuffer);
            }
        }

        public float RightTrigger
        {
            get
            {
                return InputLib.SetBuffer(currentPadState.Triggers.Right, TriggerBuffer);
            }
        }

        /// <returns>The stick has an input value</returns>
        public bool bJoyStick(ThumbStickType stickType)
        {
            return RawJoyStickValue(stickType) != Vector2.Zero;
        }

         public bool bLeftStick { get { return bJoyStick(ThumbStickType.Left); } }
         public bool bRightStick { get { return bJoyStick(ThumbStickType.Right); } }
         public bool bDpad { get { return bJoyStick(ThumbStickType.D); } }

         protected Vector2 RawJoyStickValue(ThumbStickType stickType)
        {
            Vector2 value;
            switch (stickType)
            {
                case VikingEngine.ThumbStickType.Left:
                    value = currentPadState.ThumbSticks.Left;
                    break;
                case VikingEngine.ThumbStickType.Right:
                    value = currentPadState.ThumbSticks.Right;
                    break;
                default: //Dpad
                    value = Vector2.Zero;
                    if (currentPadState.DPad.Left == ButtonState.Pressed) { value.X = -1; }
                    else if (currentPadState.DPad.Right == ButtonState.Pressed) { value.X = 1; }
                    if (currentPadState.DPad.Up == ButtonState.Pressed) { value.Y = 1; }
                    else if (currentPadState.DPad.Down == ButtonState.Pressed) { value.Y = -1; }
                    break;
            }

            return value;
        }

        public JoyStickValue JoyStickValue(ThumbStickType stickType)
        {
            Vector2 value = RawJoyStickValue(stickType);

            value.X = InputLib.SetBuffer(value.X, ControlSense.AnalogBuffert);
            value.Y = -InputLib.SetBuffer(value.Y, ControlSense.AnalogBuffert); //inverting Y

            //If the check is made more than once per frame, the step buffer wont work
            ThumbStick ts = thumbSticks[(int)stickType];
            JoyStickValue result = ts.UpdateStepBuffer(value);
            result.Time();
            return result;
        }

        public void vibrate(float leftMotor, float rightMotor, float time, bool forceLevel = false)
        {
            if (Ref.gamesett.VibrationLevel > 0)
            {
                float perc = (float)Ref.gamesett.VibrationLevel / 100;
                leftMotor *= perc;
                rightMotor *= perc;

                if (forceLevel)
                {
                    vibrationLeftMotor = leftMotor;
                    vibrationRightMotor = rightMotor;
                    vibrationTime.MilliSeconds = time;
                }
                else
                {
                    vibrationLeftMotor = Math.Max(leftMotor, vibrationLeftMotor);
                    vibrationRightMotor = Math.Max(rightMotor, vibrationRightMotor);
                    vibrationTime.MilliSeconds = Math.Max(time, vibrationTime.MilliSeconds);
                }
                GamePad.SetVibration(index, vibrationLeftMotor, vibrationRightMotor);
            }
        }

        public void onGameStateChange()
        {
            previousPadState = currentPadState;  //ClearInput();
            endVibration();
        }

        void endVibration()
        {
            GamePad.SetVibration(index, 0, 0);
            vibrationLeftMotor = 0;
            vibrationRightMotor = 0;
            vibrationTime.setZero();
        }


        public override string ToString()
        {
            return "Xinput controller(" + TextLib.IndexToString(index) + ")";
        }
    }
}
