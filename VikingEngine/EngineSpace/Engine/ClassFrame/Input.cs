using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
////xna
using Microsoft.Xna.Framework.Input.Touch;

namespace VikingEngine
{
    abstract class AbsInput : IDeleteable, IUpdateable
    {
        protected bool inInputList = false;
        public AbsInput()
        {
            if (PlatformSettings.RunningWindows)
            {
               // Engine.Input.AddToMouseInput(this, true);
            }
            inInputList = true;
        }
        virtual public void DeleteMe()
        {
        }
        virtual public bool IsDeleted
        {
            get { return inInputList; }
        }

        virtual public void Time_Update(float time){ }
        
        virtual public void TextInputEvent(int playerIndex, string input, int link){ }
        virtual public void TextInputCancelEvent(int playerIndex) { }
        virtual public void BeginInputDialogueEvent(VikingEngine.Engine.KeyboardInputValues keyInputValues) { }
        
        virtual public UpdateType UpdateType { get { return UpdateType.Full; } }
        
        public int SpottedArrayMemberIndex { get; set; }
        public bool SpottedArrayUseIndex { get { return true; } }
        public bool RunDuringPause { get { return true; } }
    }
}
