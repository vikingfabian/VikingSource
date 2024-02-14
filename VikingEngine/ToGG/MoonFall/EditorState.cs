using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD;

namespace VikingEngine.ToGG.MoonFall
{
    class EditorState : MoonFallState
    {
        Editor editor;

        public EditorState()
            : base()
        {
            editor = new Editor();
        }

        protected override void gameUpdate()
        {
            editor.update();
        }

        protected override void mainMenu(GuiLayout layout)
        {
            new GuiTextButton("Save", null, save, false, layout);
        }

        override public bool InEditor => true;
    }
}
