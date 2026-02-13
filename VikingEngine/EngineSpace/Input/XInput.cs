using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace VikingEngine.Input
{
    static class XInput
    {
        public static readonly Buttons[] AllButtons = new Buttons[]
        {
            Buttons.DPadUp,
            Buttons.DPadDown,
            Buttons.DPadLeft,
            Buttons.DPadRight,
            Buttons.Start,
            Buttons.Back,
            Buttons.LeftStick,
            Buttons.RightStick,
            Buttons.LeftShoulder,
            Buttons.RightShoulder,
            Buttons.BigButton,
            Buttons.A,
            Buttons.B,
            Buttons.X,
            Buttons.Y,
            Buttons.LeftThumbstickLeft,
            Buttons.RightTrigger,
            Buttons.LeftTrigger,
            Buttons.RightThumbstickUp,
            Buttons.RightThumbstickDown,
            Buttons.RightThumbstickRight,
            Buttons.RightThumbstickLeft,
            Buttons.LeftThumbstickUp,
            Buttons.LeftThumbstickDown,
            Buttons.LeftThumbstickRight,
        };

        public static List2<XController> controllers;

        public static void Init()
        {
            controllers = new List2<XController>(GamePad.MaximumGamePadCount);

            Update();                
        }
        
        public static void Update()
        {
            if (Ref.steam.isInitialized)
            {
                Ref.steam.input.update();
            }

            if (controllers == null)
            {
                return;
            }

            XController.ButtonB_KeyUpTime--;

            while (GamePad.MaximumGamePadCount > controllers.Count)
            {
                var c = new XController(controllers.Count);
                controllers.Add(c);
            }

            for (int i = 0; i < GamePad.MaximumGamePadCount; i++)
            {   
                controllers[i].Update();
            }
        }

        public static void OnGameStateChange()
        {
            if (controllers == null)
            {
                return;
            }

            for (int i = 0; i < controllers.Count; i++)
            {
                controllers[i].onGameStateChange();
            }
            
        }

        static public bool KeyDownEvent(Buttons button, out int player)
        {
            if (controllers != null)
            {
                for (int i = 0; i < controllers.Count; i++)
                {
                    if (controllers[i].KeyDownEvent(button))
                    {
                        player = Instance(i).Index;
                        return true;
                    }
                }
            }
            player = -1;
            return false;
        }

        static public bool AnyActivationKey_DownEvent(out int player)
        {
            if (controllers != null)
            {
                for (int i = 0; i < controllers.Count; i++)
                {
                    if (controllers[i].KeyDownEvent(Buttons.A, Buttons.X, Buttons.Start))
                    {
                        player = Instance(i).Index;
                        return true;
                    }
                }
            }
            player = -1;
            return false;
        }

        static public bool KeyIsDown(Buttons button, ref int player)
        {
            if (controllers != null)
            {
                for (int i = 0; i < controllers.Count; i++)
                {
                    if (controllers[i].IsButtonDown(button))
                    {
                        player = Instance(i).Index;
                        return true;
                    }
                }
            }
            return false;
        }

        static public bool KeyDownEvent_index(Buttons button, out int index)
        {
            if (controllers != null)
            {
                for (int i = 0; i < controllers.Count; i++)
                {
                    if (controllers[i].KeyDownEvent(button))
                    {
                        index = i;
                        return true;
                    }
                }
            }
            index= -1;
            return false;
        }

        static public bool KeyDownEvent(Buttons button)
        {
            if (controllers != null)
            {
                for (int i = 0; i < controllers.Count; i++)
                {
                    if (controllers[i].KeyDownEvent(button))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static public bool KeyDownEvent(Buttons button1, Buttons button2)
        {
            if (controllers != null)
            {
                for (int i = 0; i < controllers.Count; i++)
                {
                    if (controllers[i].KeyDownEvent(button1, button2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static XController Instance(int index)
        {
            arraylib.TryGet(controllers, index, out var instance);
            return instance;
        }

        public static int MaxIndex()
        {
            if (controllers != null)
            {
                for (int i = controllers.Count - 1; i >= 0; --i)
                {
                    if (controllers[i].Connected)
                    {
                        return i;
                    }
                }
            }
            return 0;
        }

        public static bool HasConnectedController()
        {
            if (controllers != null)
            {
                for (int i = controllers.Count - 1; i >= 0; --i)
                {
                    if (controllers[i].Connected)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static XController Default()
        {
            if (controllers != null)
            {
                var c = controllers[(int)Engine.XGuide.LocalHostIndex];
                if (c.Connected)
                {
                    return c;
                }

                controllers.loopBegin();
                while (controllers.loopNext())
                {
                    if (controllers.sel.Connected)
                    {
                        return controllers.sel;
                    }
                }
            }

            return null; //Found no controller
        }

        public static XController Main
        {
            get
            {
                if (PlatformSettings.SteamAPI)
                    throw new Exception("Only for xbox use");

                return controllers[(int)Engine.XGuide.LocalHostIndex];
            }
        }
    }
    

    class ThumbStick
    {
        JoyStickValue value = new JoyStickValue();
        DirXYstepping stepping = new DirXYstepping();

        public ThumbStick(ThumbStickType type, int controllerIx)
        {
            value.Stick = type;
            value.ContolIx = controllerIx;
        }

        public JoyStickValue UpdateStepBuffer(Vector2 input)
        {
            this.value.Direction = input;
            this.value.Stepping = stepping.update(input, true);
            return this.value;
        }

        public Vector2 Value => value.Direction;
        public IntVector2 Stepping => value.Stepping;
    }

    struct DirXYstepping
    {
        public DirectionalStepping x;
        public DirectionalStepping y;

        public IntVector2 update(Vector2 input, bool useTime)
        {
            IntVector2 result = new IntVector2(
                x.Update(input.X, useTime),
                y.Update(input.Y, useTime));

            return result;
        }
    }

    struct DirectionalStepping
    {
        float accumulation;
        bool keyDown;
        const float FalloverVal = 360;
        const float HspeedFalloverVal = 170;
        float fallOver;

        public int Update(float dirInput, bool useTime)
        {
            const float InputBuffer = 0.3f;
            if (Math.Abs(dirInput) <= InputBuffer)
            {
                keyDown = false;
                accumulation = 0;
                fallOver = FalloverVal;
                return 0;
            }
            else
            {
                if (useTime)
                {
                    dirInput *= Ref.DeltaTimeMs;
                }
                accumulation += dirInput;
                if (!keyDown)
                {
                    keyDown = true;
                    return lib.ToLeftRight(accumulation);
                }
                else if (Math.Abs(accumulation) >= fallOver)
                {
                    fallOver = HspeedFalloverVal;
                    int result = lib.ToLeftRight(accumulation);
                    accumulation = 0;
                    return result;
                }

            }
            return 0;
        }
    }
}
