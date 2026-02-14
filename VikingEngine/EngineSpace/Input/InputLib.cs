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
    static class InputLib
    {
        public const float ButtonMaxClickTimeMs = 250;
        public const float ButtonHoldTimeMs = 500;
        public static void Init(MainGame main)
        {
            

            XInput.Init();
        }

        public static void Update()
        {
            XInput.Update();
            Keyboard.Update();
            if (PlatformSettings.RunningWindows)
            {
                Mouse.Update();
            }
        }

        public static bool AnyKeyDownEvent()
        {
            if (Ref.steam.input != null && Ref.steam.input.AnyKeyDownEvent())
            {
                return true;
            }

            foreach (var m in XInput.controllers)
            {
                if (m.Connected)
                {
                    if (m.KeyDownEvent(Buttons.A) ||
                        m.KeyDownEvent(Buttons.B) ||
                        m.KeyDownEvent(Buttons.X) ||
                        m.KeyDownEvent(Buttons.Y) ||
                        m.KeyDownEvent(Buttons.Start) ||
                        m.KeyDownEvent(Buttons.Back))
                    { return true; }
                }
            }

            foreach (var m in Keyboard.AllKeys)
            {
                if (Keyboard.KeyDownEvent(m))
                {
                    return true;
                }
            }

            if (PlatformSettings.RunningWindows)
            {
                if (Mouse.ButtonDownEvent(MouseButton.Left) ||
                    Mouse.ButtonDownEvent(MouseButton.Right))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool inPopupWindow = false;
        public static bool WindowHasFocus
        {
            get { return Ref.main.IsActive && !inPopupWindow; }
        }

        public static void OnGameStateChange()
        {
            Keyboard.ClearInput();
            XInput.OnGameStateChange();
        }

        public static float SetBuffer(float value, float minValue)
        {
            if (Math.Abs(value) < minValue)
                return 0;
            return value;
        }

        public static numBUTTON ButtonConvert(Buttons button)
        {
            switch (button)
            {
                case Buttons.A:
                    return numBUTTON.A;
                case Buttons.B:
                    return numBUTTON.B;
                case Buttons.X:
                    return numBUTTON.X;
                case Buttons.Y:
                    return numBUTTON.Y;
                case Buttons.LeftShoulder:
                    return numBUTTON.LB;
                case Buttons.RightShoulder:
                    return numBUTTON.RB;
                case Buttons.LeftTrigger:
                    return numBUTTON.LT;
                case Buttons.RightTrigger:
                    return numBUTTON.RT;
                case Buttons.Start:
                    return numBUTTON.Start;
                case Buttons.Back:
                    return numBUTTON.Back;
                default:
                    return numBUTTON.UNNOWN;
            }
        }
        public static Buttons ButtonConvert(numBUTTON button)
        {
            switch (button)
            {
                case numBUTTON.A: return Buttons.A;
                case numBUTTON.B: return Buttons.B;
                case numBUTTON.X: return Buttons.X;
                case numBUTTON.Y: return Buttons.Y;
                case numBUTTON.LB: return Buttons.LeftShoulder;
                case numBUTTON.RB: return Buttons.RightShoulder;
                case numBUTTON.LT: return Buttons.LeftTrigger;
                case numBUTTON.RT: return Buttons.RightTrigger;
                case numBUTTON.Start: return Buttons.Start;
                case numBUTTON.Back: return Buttons.Back;
                case numBUTTON.LS_Click: return Buttons.LeftThumbstickDown;
                case numBUTTON.RS_Click: return Buttons.RightThumbstickDown;

                default: throw new NotImplementedException();
            }
        }

        public static bool ChangedEvent(IButtonMap buttons)
        {
            return buttons.DownEvent || buttons.UpEvent;
        }
        public static bool ChangedEvent(IButtonMap buttons, out bool isDown)
        {
            isDown = buttons.IsDown;
            return buttons.DownEvent || buttons.UpEvent;
        }

        public static bool Equals(IButtonMap button1, IButtonMap button2)
        {
            return button1.inputSource == button2.inputSource && 
                button1.buttonIndex == button2.buttonIndex && 
                button1.ControllerIndex == button2.ControllerIndex;
        }

        public static bool Connected(IButtonMap buttons)
        {
            if (buttons.inputSource == InputSourceType.XController)
            {
                return Input.XInput.Instance(buttons.ControllerIndex).Connected;
            }
            return true;
        }

        public static void Vibrate(IButtonMap buttons, float leftMotor, float rightMotor, float time)
        {
            if (buttons.inputSource == InputSourceType.XController)
            {
                Input.XInput.Instance(buttons.ControllerIndex).vibrate(leftMotor, rightMotor, time);
            }
        }

        public static IButtonMap CombineButtons(IButtonMap buttonMap, IButtonMap add)
        {
            if (buttonMap is NoButtonMap)
            {
                return add;
            }
            return new AlternativeButtonsMap(buttonMap, add);
        }

        public static Vector2 OnlyOneDimentionOut(Vector2 directionValue)
        {
            Vector2 result = Vector2.Zero;

            const float Buffer = 0.6f;
            float absX = Math.Abs(directionValue.X);
            float absY = Math.Abs(directionValue.Y);
            if (absX > absY)
            {
                if (absX > Buffer)
                {
                    result.X = directionValue.X;
                }
            }
            else
            {
                if (absY > Buffer)
                {
                    result.Y = directionValue.Y;
                }
            }
            return result;
        }
        //public static bool DownEvent_AnyInstance(IButtonMap buttons)
        //{
        //    if (buttons.inputSource == InputSourceType.XController)
        //    {
        //        Input.XInput.KeyDownEvent(
        //        //return Input.XInput.Instance(buttons.ControllerIndex).Connected;
        //    }
        //    return buttons.DownEvent;
        //}
    }

    struct ButtonGroup
    {
        public int Count;
        public Buttons button1, button2, button3;

        public ButtonGroup(Buttons button)
        {
            Count = 1;
            button1 = button;
            button2 = 0;
            button3 = 0;
        }
        public ButtonGroup(Buttons button1, Buttons button2)
        {
            Count = 2;
            this.button1 = button1;
            this.button2 = button2;
            button3 = 0;
        }
        public ButtonGroup(Buttons button1, Buttons button2, Buttons button3)
        {
            Count = 3;
            this.button1 = button1;
            this.button2 = button2;
            this.button3 = button3;
        }
    }

   

   

    enum PlayerControllerSelection
    {
        KeyboardMouse,
        Keyboard,

        Controller1,
        Controller2,
        Controller3,
        Controller4,
        Num_Non,
    }

    enum ControllerActionSetType
    {
        InGameControls,
        MenuControls,
        EditorControls,
        CardGameControls,
        NUM
    }

    enum ButtonActionType
    {
        // Gameplay
        GameMainAttack,
        GameAlternativeAttack,
        GameJump,
        GameInteract,
        GameUseItem,
        GamePlayCamMode,
        GameEditorMode,
        GamePauseMenu,
        GameHoldPlayerMovement,
        GameAltButton,
        GameChat,

        // Menu
        MenuClick,
        MenuBack,
        MenuTabLeftUp,
        MenuTabRightDown,
        MenuReturnToGame,

        // Editor
        EditorDraw,
        EditorErase,
        EditorSelect,
        EditorSelectAll,
        EditorPick,
        EditorDeselect,
        EditorMirrorX,
        EditorMirrorY,
        EditorUndo,
        EditorHelp,
        EditorPrevious,
        EditorNext,
        EditorMenu,

        // Card Game
        CardSelect,
        CardCancel,
        CardToggleHand,
        CardToggleInfo,
        CardTogglePicks,
        //CardTabLeft,
        //CardTabRight,
        CardEndTurn,
        CardMenu,

        //Bird
        Action0,
        Action1,
        Action2,
        Action3,
        Action4,
        Action5,
        Action6,
        Action7,
        ActionStart,
        ActionMenu,

        // Global
        //GlobalPushToTalk,
        NUM_NON,
    }

    enum DirActionType
    {
        // Gameplay
        GamePlayerMovement,
        GameCamera,
        GamePlayCamZoom,

        // Menu
        MenuMouse,
        MenuMovement,
        MenuScroll,

        // Editor
        EditorMoveXZ,
        EditorCamXMoveY,
        EditorCamXY,
        EditorCamZoom,

        // Card Game
        CardMoveCursor,
        NUM_NON
    }
}
