using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.GameState;

namespace VikingEngine.Input
{

    interface ITextInputReciever
    {
        void textInput_refresh(bool textLengthChanged);
        
        void textInput_complete(string result, object tag);
    }

    class TextInput
    {
        string preMarkerText = string.Empty, postMarkerText = string.Empty;
        bool flashMarker = false;
        Timer.Basic flashMarkerTimer = new Timer.Basic(800, true);
        
        ITextInputReciever inputReciever;
        public string recieverId;
        object tag;

        bool bExit = false;
        string result = null;
        float exitDelay = 250;

        KeyboardState previousState;
        KeyboardState currentState;

        public TextInput(string defaultText, ITextInputReciever reciever, string recieverId, object tag)
        {
            this.recieverId = recieverId;
            this.tag = tag;
            this.inputReciever = reciever;
            this.preMarkerText = defaultText;
            RegisterFocusedButtonForTextInput(OnTextInput, true);
            Ref.update.textInput = this;
            refresh(true);
        }

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

            onInput(true);
        }

        void onInput(bool textLengthChanged)
        {

            flashMarker = true;
            flashMarkerTimer.Reset();

            refresh(textLengthChanged);

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

        public void Update()
        {
            if (bExit)
            {
                exitDelay -= Ref.DeltaTimeMs;
                if (exitDelay <= 0)
                {                   
                    inputReciever.textInput_complete(result, tag);
                    DeleteMe();
                }
                return;
            }

            previousState = currentState;
            currentState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            if (KeyDownEvent(Keys.Left)) // Left arrow
            {
                left();
                onInput(false);
            }
            else if (KeyDownEvent(Keys.Right)) // Right arrow
            {
                right();
                onInput(false);
            }


            if (flashMarkerTimer.Update())
            {
                flashMarker = !flashMarker;
                refresh(false);
            }
        }

        bool KeyDownEvent(Keys key)
        {
            return previousState.IsKeyUp(key) && currentState.IsKeyDown(key);
        }

        void refresh(bool textLengthChanged)
        {
            

            inputReciever.textInput_refresh(textLengthChanged);
        }

        public string DisplayText()
        {
            string marker = flashMarker ? "|" : "";
            return LoadContent.CheckCharsSafety(preMarkerText + marker + postMarkerText, LoadedFont.Regular);
        }

        public void DeleteMe()
        {
            RegisterFocusedButtonForTextInput(OnTextInput, false);
            Ref.update.textInput = null;
        }

        public bool Exiting => bExit;
    }

    abstract class AbsTextInputUpdate : AbsUpdateable, ITextInputReciever
    {
        TextInput input;
        public AbsTextInputUpdate()
            :base(true)
        {
            
        }

        protected void init(string defaultText, string recieverId, object tag)
        { 
            input = new TextInput(defaultText, this, recieverId, tag);
        }
        
        public override void Time_Update(float time)
        {
            input.Update();
        }

        abstract public void textInput_refresh(bool textLengthChanged);

        virtual public void textInput_complete(string result, object tag)
        {
            DeleteMe();
        }
    }

    class TextInputState : GameState, ITextInputReciever
    {
        TextInputEvent returnEvent;
        TextInput input;
        TextG display;
        public TextInputState(string defaultText, TextInputEvent returnEvent, object tag)
             : base(false)
        {
            this.returnEvent = returnEvent;
            
            display = new TextG(LoadedFont.Regular, Engine.Screen.Area.PercentToPosition(0.35f, 0.45f), Engine.Screen.TextSizeV2 * 1.2f, Align.Zero, string.Empty, Color.Yellow, ImageLayers.Top1);

            Image icon = new Image(SpriteName.InterfaceTextInput, VectorExt.AddX(display.position, -Engine.Screen.IconSize * 1.0f),
                Engine.Screen.SmallIconSizeV2, ImageLayers.Top1);

            input = new TextInput(defaultText, this, "TextInputState", tag);
            textInput_refresh(true);
        }
        public override void OnDestroy()
        {
            display.DeleteMe();
        }

        public void textInput_refresh(bool textLengthChanged)
        {
            if (input != null)
            {
                display.TextString = input.DisplayText();
            }
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            input.Update();
        }

        public void textInput_complete(string result, object tag)
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
    }
}
