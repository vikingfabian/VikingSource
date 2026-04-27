using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Engine;
using VikingEngine.Input;
using VikingEngine.SteamWrapping;

namespace VikingEngine.HUD
{
    class GuiTextInputbox : GuiIconTextButton
    {
        string textString;
        TextInputEvent callBack;

        public GuiTextInputbox(string text, TextInputEvent callBack, GuiLayout layout)
            : base(SpriteName.InterfaceTextInput, ">" + text, null,
                new GuiNoAction(), false, layout)
        {
            this.callBack = callBack;
            clickAction = new GuiAction(clickEvent);
            this.textString = text;
        }

        void clickEvent()
        {
            var reciever = new TextInputState(textString, TextInputEvent, null);
            SteamInputManager.tryOpenSteamKeyboard(reciever);
        }

        void TextInputEvent(string result, object tag)
        {
            if (result != null)
            {
                textString = result;
                text.TextString = ">" + textString;
            }
            callBack?.Invoke(result, tag);
        }
    }
}
