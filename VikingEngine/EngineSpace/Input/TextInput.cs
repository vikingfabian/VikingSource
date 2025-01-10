using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.ToGG.GameState;

namespace VikingEngine.Input
{
    //    /// <param name="result">input, null is canceled</param>
    //    public delegate void TextInputEvent(int user, string result, int index);
    class TextInput : GameState
    {
        TextG display;
        string preMarkerText = string.Empty, postMarkerText = string.Empty;
        bool flashMarker = false;
        Timer.Basic flashMarkerTimer = new Timer.Basic(800, true);
        TextInputEvent returnEvent;
        object tag;

        bool bExit = false;
        string result = null;
        int exitDelay = 2;

        public TextInput(string defaultText, TextInputEvent returnEvent, object tag) 
            : base(false)
        {
            this.tag = tag;
            this.returnEvent = returnEvent;
            this.preMarkerText = defaultText;
            RegisterFocusedButtonForTextInput(OnTextInput, true);

            display = new TextG(LoadedFont.Regular, Engine.Screen.Area.PercentToPosition(0.35f, 0.45f), Engine.Screen.TextSizeV2 * 1.2f, Align.Zero, string.Empty, Color.Yellow, ImageLayers.Top1);

            Image icon = new Image(SpriteName.InterfaceTextInput, VectorExt.AddX(display.position, -Engine.Screen.IconSize * 1.0f), 
                Engine.Screen.SmallIconSizeV2, ImageLayers.Top1);

            refresh();
        }

        //private string inputBuffer = "";

        public void RegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method, bool register)
        {
            if (register)
            {
                Ref.main.Window.TextInput += method;
            }
            else
            {
                Ref.main.Window.TextInput -= method;
            }
        }


        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            if (bExit) return;

            // Handle movement keys
            if (e.Character == '\u001B') // Escape key
            {
                bExit = true;
                return;
            }
            else if (e.Character == '\r') // Enter key
            {
                result = preMarkerText + postMarkerText;
                bExit = true;                
                return;
            }
            else if (e.Character == '\b') // Backspace
            {
                if (preMarkerText.Length > 0)
                {
                    preMarkerText = preMarkerText.Substring(0, preMarkerText.Length - 1);
                }
            }
            else if (e.Character == '\u007F') // Delete key
            {
                if (postMarkerText.Length > 0)
                {
                    postMarkerText = postMarkerText.Substring(1);
                }
            }
            else if (e.Character == '\u001C') // Left arrow
            {
                //left();
            }
            else if (e.Character == '\u001D') // Right arrow
            {
                //right();
            }
            else
            {
                // Append valid input character to preMarkerText
                preMarkerText += e.Character;
            }
            
            onInput();
        }

        void onInput()
        { 
            
            flashMarker = true;
            flashMarkerTimer.Reset();

            refresh();
            
        }


        void left()
        { 
            if (preMarkerText.Length > 0)
            {
                postMarkerText = preMarkerText[^1] + postMarkerText;
                preMarkerText = preMarkerText.Substring(0, preMarkerText.Length - 1);
            }
        }
        void right()
        {
            if (postMarkerText.Length > 0)
            {
                preMarkerText += postMarkerText[0];
                postMarkerText = postMarkerText.Substring(1);
            }
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (bExit)
            {
                if (--exitDelay <= 0)
                {
                    Engine.StateHandler.PopGamestate();

                    if (returnEvent == null)
                    {
                        Ref.gamestate.TextInputEvent(result, tag);
                    }
                    else
                    {
                        returnEvent.Invoke(result, 0);
                    }
                }
                return;
            }

            if (Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Left)) // Left arrow
            {
                left();
                onInput();
            }
            else if (Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Right)) // Right arrow
            {
                right();
                onInput();
            }


            if (flashMarkerTimer.Update(time))
            {
                flashMarker = !flashMarker;
                refresh();
            }
        }

        void refresh()
        {
            string marker = flashMarker ? "|" : "";

            display.TextString = LoadContent.CheckCharsSafety(preMarkerText + marker + postMarkerText, LoadedFont.Regular);
        }

        public override void OnDestroy()
        {
            RegisterFocusedButtonForTextInput(OnTextInput, false);
            base.OnDestroy();

        }

    }
}
