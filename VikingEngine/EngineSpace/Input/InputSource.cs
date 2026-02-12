using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VikingEngine.DSSWars;

namespace VikingEngine.Input
{
    struct InputSource
    {
        public static readonly InputSource DefaultPC = new InputSource(InputSourceType.KeyboardMouse);
        public static readonly InputSource Empty = new InputSource(InputSourceType.Num_Non);

        public InputSourceType sourceType;
        public int controllerIndex;
        public bool hasTouch;
        public bool useTouch;

        public InputSource(IButtonMap button)
        {
            this.sourceType = button.inputSource;
            this.controllerIndex = button.ControllerIndex;
            useTouch = true;
        }

        public InputSource(InputSourceType source, int controllerIndex = -1)
        {
            this.sourceType = source;
            this.controllerIndex = controllerIndex;
        }

        public bool HasKeyBoard
        {
            get { return sourceType == InputSourceType.KeyboardMouse || sourceType == InputSourceType.Any; }
        }

        public bool HasMouse
        {
            get { return sourceType == InputSourceType.KeyboardMouse || sourceType == InputSourceType.Any || 
                    (sourceType == InputSourceType.SteamInput && hasTouch && useTouch); }
        }

        public bool IsControllerOnly
        {
            get { return sourceType == InputSourceType.XController; }
        }

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
                case InputSourceType.Num_Non:
                    result = Ref.langOpt.InputNotSet;
                    break;
                case InputSourceType.XController:
                    result = Ref.langOpt.InputController;
                    break;
                case InputSourceType.KeyboardMouse:
                    result = Ref.langOpt.InputKeyboardMouse;
                    break;
                case InputSourceType.SteamInput:
                    result = DssRef.todoLang.InputSteam;
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
            var other = (InputSource)obj;

            if (sourceType == other.sourceType)
            {
                if (HasIndex)
                {
                    return controllerIndex == other.controllerIndex;
                }
                else
                {
                    return true;
                }
            }
            return false;
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
            w.Write(controllerIndex);
            w.Write(useTouch);
        }
        public void read(System.IO.BinaryReader r)
        {
            sourceType = (InputSourceType)r.ReadInt32();
            controllerIndex = r.ReadInt32();
            useTouch = r.ReadBoolean();
        }
    }

    enum InputSourceType
    {
        KeyboardMouse,
        XController,
        Any,
        Num_Non,

        Keyboard,
        Mouse,
        SteamInput,
        //GenericController,
    }
}
