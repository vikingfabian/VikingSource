using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using VikingEngine.SteamWrapping;

namespace VikingEngine.Input
{
    abstract class PlayerInputMap
    {
        /* Static readonlies */
        public static readonly DirectionalButtonsMap arrowKeys = new DirectionalButtonsMap(
            new KeyboardButtonMap(Keys.Up),
            new KeyboardButtonMap(Keys.Down),
            new KeyboardButtonMap(Keys.Left),
            new KeyboardButtonMap(Keys.Right)
            );
        public static readonly DirectionalButtonsMap WASD = new DirectionalButtonsMap(
            new KeyboardButtonMap(Keys.W),
            new KeyboardButtonMap(Keys.S),
            new KeyboardButtonMap(Keys.A),
            new KeyboardButtonMap(Keys.D)
            );

        /* Fields */
        public int playerIndex;
        public Input.InputSource inputSource;//int controllerIndex;
        //public InputSourceType inputSource;

        public HUD.MenuInputMap menuInput;

        /* Constructors */
        public PlayerInputMap(System.IO.BinaryReader r, int version)
        {
            init();
            keyboardSetup();
            read(r, version);
        }

        public PlayerInputMap(int player)
        {
            this.playerIndex = player;
            init();
            setInputSource(InputSourceType.KeyboardMouse, 0);
            
        }

        virtual protected void init()
        {
            menuInput = new HUD.MenuInputMap();
        }

        

        public void setInputSource(InputSourceType inputSource, int index)
        {
            this.inputSource = new InputSource(inputSource, index);
            //this.controllerIndex = index;

            switch (inputSource)
            {
                case InputSourceType.KeyboardMouse:
                    keyboardSetup();
                    break;
                case InputSourceType.XController:
                    xboxSetup();
                    break;
                //case PlayerInputSource.GenericController:
                //    if (SharpDXInput.controllers[controllerIndex].type == ControllerType.PS4)
                //    {
                //        ps4Setup();
                //    }
                //    else
                //    {
                //        genericControllerSetup();
                //    }
                //    break;
            }
        }

        public static IntVector2 GenericMoveStepping()
        {
            return arrowKeys.stepping + WASD.stepping;
        }
        public static bool GenericClick()
        {
            return Keyboard.KeyDownEvent(Keys.Enter) || Keyboard.KeyDownEvent(Keys.Space);
        }
        
        abstract public void keyboardSetup();
        abstract public void xboxSetup();
        //abstract public void ps4Setup();
        abstract public void genericControllerSetup();

        public void write(System.IO.BinaryWriter w)
        {
        }
        public void read(System.IO.BinaryReader r, int version)
        {
        }

        //Timer.Basic steamVibratePulse = new Timer.Basic(50, true);
        //float vibrationTime = 0;
        //float currentVibrationStrength = 0;
        //float vibrateLeft, vibrateRight;

        /// <summary>
        /// Run the controller vibration 
        /// </summary>
        /// <param name="time">in milllisec</param>
        /// <param name="left">low freq engine</param>
        /// <param name="right">high freq engine</param>
        public void Vibrate(float time, float left, float right)
        {
            if (inputSource.sourceType == InputSourceType.XController)
            {
                XInput.Instance(this.playerIndex).vibrate(left, right, time);
            }
        }

        //public static void StopAllVibration()
        //{
        //    for (PlayerIndex ix = PlayerIndex.One; ix <= PlayerIndex.Four; ix++)
        //    {
        //        GamePad.SetVibration(ix, 0, 0);
        //    }
        //}

        //public void update()
        //{
        //    if (vibrationTime > 0)
        //    {
        //        vibrationTime -= Ref.DeltaTimeMs;
        //        if (vibrationTime <= 0)
        //        {
        //            GamePad.SetVibration((PlayerIndex)inputSource.controllerIndex, 0, 0);
        //        }
        //    }
        //}

        //public bool HasMouse()
        //{ 
        //    return inputSource.HasKeyBoard == 
        //}

        public bool Connected
        {
            get {
                switch (inputSource.sourceType)
                {
                    default: return true;
                    case InputSourceType.XController:
                        return Input.XInput.Instance(inputSource.controllerIndex).Connected;
                }
            }
        }

        
    }
}
