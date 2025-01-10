using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
            Engine.XGuide.BeginKeyBoardInput(new Engine.KeyboardInputValues("error", textString,
                0, 0, TextInputEvent));//, layoutParent.gui, null);
        }

        void TextInputEvent(int user, string result, int index)
        {
            textString = result;
            text.TextString = ">" + textString;
            if (callBack != null)
            { callBack(user, result, index); }
        }
    }
}
