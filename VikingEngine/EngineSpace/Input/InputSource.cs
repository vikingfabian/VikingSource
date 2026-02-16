using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Steamworks;
using VikingEngine.DSSWars;

namespace VikingEngine.Input
{
    struct InputSource
    {
        public static readonly InputSource DefaultPC = new InputSource(InputSourceType.KeyboardMouse);
        public static readonly InputSource Empty = new InputSource(InputSourceType.Num_None);

        public InputSourceType sourceType = InputSourceType.Num_None;
        public ESteamInputType steamInputType;
        public int controllerIndex;
        public bool hasTouch;
        public bool useTouchAsMouseSim;

        public InputSource(IButtonMap button)
        {
            this.sourceType = button.inputSource;
            this.controllerIndex = button.ControllerIndex;
            useTouchAsMouseSim = true;
        }

        public InputSource(InputSourceType source, int controllerIndex = -1, ESteamInputType steamInputType = ESteamInputType.k_ESteamInputType_Unknown)
        {
            this.sourceType = source;
            this.controllerIndex = controllerIndex;
            this.steamInputType = steamInputType;

            hasTouch = steamInputType == ESteamInputType.k_ESteamInputType_MobileTouch ||
                        steamInputType == ESteamInputType.k_ESteamInputType_SteamController ||
                        steamInputType == ESteamInputType.k_ESteamInputType_SteamDeckController ||
                        HasMouse;
            useTouchAsMouseSim = hasTouch;
        }

        public bool HasKeyBoard
        {
            get { return sourceType == InputSourceType.KeyboardMouse || sourceType == InputSourceType.Any; }
        }

        public bool HasMouse
        {
            get { return sourceType == InputSourceType.KeyboardMouse || sourceType == InputSourceType.Any; }
        }
        public bool HasMouseInstance
        {
            get
            {
                return sourceType == InputSourceType.KeyboardMouse ||  useTouchAsMouseSim;
            }
        }

        public bool IsXnaController
        {
            get { return sourceType == InputSourceType.XController; }
        }

        public bool ControllerMode => !useTouchAsMouseSim;
        

        public bool HasControllerInput
        {
            get { return sourceType == InputSourceType.XController || sourceType == InputSourceType.SteamInput; }
        }

        public bool IsSteamInput
        {
            get { return sourceType == InputSourceType.SteamInput; }
        }

        public bool HasIndex
        {
            get { return sourceType == InputSourceType.XController || sourceType == InputSourceType.SteamInput; }
        }

        public override string ToString()
        {
            string result;
            switch (sourceType)
            {
                case InputSourceType.Num_None:
                    result = Ref.langOpt.InputNotSet;
                    break;
                case InputSourceType.XController:
                    result = Ref.langOpt.InputController;
                    break;
                case InputSourceType.KeyboardMouse:
                    result = Ref.langOpt.InputKeyboardMouse;
                    break;
                case InputSourceType.SteamInput:
                    switch (steamInputType)
                    { 
                        case ESteamInputType.k_ESteamInputType_SteamController:
                        case ESteamInputType.k_ESteamInputType_SteamDeckController:
                            result = "Steam";
                            break;
                        case ESteamInputType.k_ESteamInputType_XBox360Controller:
                        case ESteamInputType.k_ESteamInputType_XBoxOneController:
                            result = "Xbox";
                            break;
                        case ESteamInputType.k_ESteamInputType_PS4Controller:
                        case ESteamInputType.k_ESteamInputType_PS3Controller:
                        case ESteamInputType.k_ESteamInputType_PS5Controller:
                            result = "Playstation";
                            break;
                        case ESteamInputType.k_ESteamInputType_SwitchJoyConPair:
                        case ESteamInputType.k_ESteamInputType_SwitchJoyConSingle:
                        case ESteamInputType.k_ESteamInputType_SwitchProController:
                            result = "Switch";
                            break;
                        default:
                            result = DssRef.todoLang.InputSteam;
                            break;
                    }
                    break;
                default:
                    result = sourceType.ToString();
                    break;
            }
            if (HasIndex)
            {
                result += TextLib.Parentheses((controllerIndex + 1).ToString(), true);
            }
            return result;
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            bool result = false;
            var other = (InputSource)obj;

            if (sourceType == other.sourceType)
            {
                if (HasIndex)
                {
                    result = controllerIndex == other.controllerIndex;
                }
                else
                {
                    result = true;
                }

                if (sourceType == InputSourceType.SteamInput)
                { 
                    result &= steamInputType == other.steamInputType;
                }
            }
            return result;
        }


        public bool Connected
        {
            get
            {
                if (sourceType == InputSourceType.XController)
                {
                    return XInput.controllers[controllerIndex].Connected;
                }

                return true;
            }
        }
        public XController Controller
        {
            get
            {
                if (sourceType == InputSourceType.XController)
                {
                    return Input.XInput.Instance(controllerIndex);
                }
                return null;
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((int)sourceType);
            w.Write((int)steamInputType);
            w.Write(controllerIndex);
            w.Write(useTouchAsMouseSim);
        }
        public void read(System.IO.BinaryReader r)
        {
            sourceType = (InputSourceType)r.ReadInt32();
            steamInputType =  (ESteamInputType)r.ReadInt32();
            controllerIndex = r.ReadInt32();
            useTouchAsMouseSim = r.ReadBoolean();
        }
    }

    enum InputSourceType
    {
        KeyboardMouse,
        XController,
        Any,
        
        Keyboard,
        Mouse,
        SteamInput,

        Num_None,
        //GenericController,
    }
}
