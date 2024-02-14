using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.Input
{
    static class Keyboard
    {
        static KeyboardState previousState;
        static KeyboardState currentState;

        static float holdEscTime = 0;

        public static void Update()
        {
            previousState = currentState;
            currentState =  Microsoft.Xna.Framework.Input.Keyboard.GetState();

            if (currentState.IsKeyDown(Keys.Escape))
            {
                holdEscTime += Ref.DeltaTimeSec;
                if (holdEscTime > 3f)
                { Ref.update.exitApplication = true; }
            }
            else
            {
                holdEscTime = 0;
            }
        }

        public static void ClearInput()
        {
            previousState = currentState;
        }

        public static bool IsKeyDown(Keys key)
        {
            if (InputLib.WindowHasFocus)
            {
                return currentState.IsKeyDown(key);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// If key was released this frame
        /// </summary>
        public static bool KeyUpEvent(Keys key)
        {
            if (InputLib.WindowHasFocus)
            {
                return previousState.IsKeyDown(key) && currentState.IsKeyUp(key);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// If key was pressed this frame
        /// </summary>
        public static bool KeyDownEvent(Keys key)
        {
            if (InputLib.WindowHasFocus)
            {
                return previousState.IsKeyUp(key) && currentState.IsKeyDown(key);
            }
            else
            {
                return false;
            }
        }

        public static Keys? CheckKeyDowns(List<Keys> keyList)
        {
            if (currentState.GetHashCode() != 0)
            {
                foreach (Keys k in keyList)
                {
                    if (KeyDownEvent(k))
                        return k;
                }
            }
            return null;
        }
        public static Keys? CheckKeyUps(List<Keys> keyList)
        {
            if (previousState.GetHashCode() != 0)
            {
                foreach (Keys k in keyList)
                {
                    if (KeyUpEvent(k))
                        return k;
                }
            }
            
            return null;
        }

        public static bool Shift
        {
            get { return currentState.IsKeyDown(Keys.LeftShift) || currentState.IsKeyDown(Keys.RightShift); }
        }
        public static bool Ctrl
        {
            get { return currentState.IsKeyDown(Keys.LeftControl) || currentState.IsKeyDown(Keys.RightControl); }
        }
        public static bool Alt
        {
            get { return currentState.IsKeyDown(Keys.LeftAlt) || currentState.IsKeyDown(Keys.RightAlt); }
        }

        static void checkInputLetter(Keys letter, ref string text)
        {
            if (KeyDownEvent(letter))
            {
                if (Shift)
                {
                    text += TextLib.LastLetters( letter.ToString().ToUpper());
                }
                else
                {
                    text += TextLib.LastLetters(letter.ToString().ToLower());
                }
            }
        }

        public static readonly Keys[] AllKeys = new Keys[]
        {
            Keys.Back,
            Keys.Tab,
            Keys.Enter,
            Keys.Pause,
            Keys.CapsLock,
            Keys.Kana,
            Keys.Kanji,
            Keys.Escape,
            Keys.ImeConvert,
            Keys.ImeNoConvert,
            Keys.Space,
            Keys.PageUp,
            Keys.PageDown,
            Keys.End,
            Keys.Home,
            Keys.Left,
            Keys.Up,
            Keys.Right,
            Keys.Down,
            Keys.Select,
            Keys.Print,
            Keys.Execute,
            Keys.PrintScreen,
            Keys.Insert,
            Keys.Delete,
            Keys.Help,
            Keys.D0,
            Keys.D1,
            Keys.D2,
            Keys.D3,
            Keys.D4,
            Keys.D5,
            Keys.D6,
            Keys.D7,
            Keys.D8,
            Keys.D9,
            Keys.A,
            Keys.B,
            Keys.C,
            Keys.D,
            Keys.E,
            Keys.F,
            Keys.G,
            Keys.H,
            Keys.I,
            Keys.J,
            Keys.K,
            Keys.L,
            Keys.M,
            Keys.N,
            Keys.O,
            Keys.P,
            Keys.Q,
            Keys.R,
            Keys.S,
            Keys.T,
            Keys.U,
            Keys.V,
            Keys.W,
            Keys.X,
            Keys.Y,
            Keys.Z,
            Keys.LeftWindows,
            Keys.RightWindows,
            Keys.Apps,
            Keys.Sleep,
            Keys.NumPad0,
            Keys.NumPad1,
            Keys.NumPad2,
            Keys.NumPad3,
            Keys.NumPad4,
            Keys.NumPad5,
            Keys.NumPad6,
            Keys.NumPad7,
            Keys.NumPad8,
            Keys.NumPad9,
            Keys.Multiply,
            Keys.Add,
            Keys.Separator,
            Keys.Subtract,
            Keys.Decimal,
            Keys.Divide,
            Keys.F1,
            Keys.F2,
            Keys.F3,
            Keys.F4,
            Keys.F5,
            Keys.F6,
            Keys.F7,
            Keys.F8,
            Keys.F9,
            Keys.F10,
            Keys.F11,
            Keys.F12,
            Keys.F13,
            Keys.F14,
            Keys.F15,
            Keys.F16,
            Keys.F17,
            Keys.F18,
            Keys.F19,
            Keys.F20,
            Keys.F21,
            Keys.F22,
            Keys.F23,
            Keys.F24,
            Keys.NumLock,
            Keys.Scroll,
            Keys.LeftShift,
            Keys.RightShift,
            Keys.LeftControl,
            Keys.RightControl,
            Keys.LeftAlt,
            Keys.RightAlt,
            Keys.BrowserBack,
            Keys.BrowserForward,
            Keys.BrowserRefresh,
            Keys.BrowserStop,
            Keys.BrowserSearch,
            Keys.BrowserFavorites,
            Keys.BrowserHome,
            Keys.VolumeMute,
            Keys.VolumeDown,
            Keys.VolumeUp,
            Keys.MediaNextTrack,
            Keys.MediaPreviousTrack,
            Keys.MediaStop,
            Keys.MediaPlayPause,
            Keys.LaunchMail,
            Keys.SelectMedia,
            Keys.LaunchApplication1,
            Keys.LaunchApplication2,
            Keys.OemSemicolon,
            Keys.OemPlus,
            Keys.OemComma,
            Keys.OemMinus,
            Keys.OemPeriod,
            Keys.OemQuestion,
            Keys.OemTilde,
            Keys.ChatPadGreen,
            Keys.ChatPadOrange,
            Keys.OemOpenBrackets,
            Keys.OemPipe,
            Keys.OemCloseBrackets,
            Keys.OemQuotes,
            Keys.Oem8,
            Keys.OemBackslash,
            Keys.ProcessKey,
            Keys.OemCopy,
            Keys.OemAuto,
            Keys.OemEnlW,
            Keys.Attn,
            Keys.Crsel,
            Keys.Exsel,
            Keys.EraseEof,
            Keys.Play,
            Keys.Zoom,
            Keys.Pa1,
            Keys.OemClear,
        };
    }
}
