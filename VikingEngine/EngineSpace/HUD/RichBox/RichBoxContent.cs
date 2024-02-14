using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Players;

namespace VikingEngine.HUD.RichBox
{
    class RichBoxContent : List<AbsRichBoxMember>
    {
        public RichBoxContent()
            : base(16)
        { }

        public RichBoxText text(string textline)
        {
            newLine();
            var textCont = new RichBoxText(textline);
            Add(textCont);

            return textCont;
        }

        public RichBoxText icontext(SpriteName icon, string textline)
        {
            newLine();
            Add(new RichBoxImage(icon));
            space();
            var textCont = new RichBoxText(textline);
            Add(textCont);

            return textCont;
        }

        public void add(SpriteName icon, string textline)
        {
            newLine();
            Add(new RichBoxImage(icon));
            Add(new RichBoxText(textline));
        }

        public void space(int spaces = 1)
        { 
            Add(new RichBoxSpace(spaces));
        }

        public RichBoxText h1(string textline)
        {
            newLine();
            Add(new RichBoxBeginTitle(1));
            var text = new RichBoxText(textline);
            Add(text);

            return text;
        }

        public RichBoxText h2(string textline)
        {
            newLine();
            Add(new RichBoxBeginTitle(2));
            var text = new RichBoxText(textline);
            Add(text);

            return text;
        }

        public void newLine()
        {
            if (this.Count > 0)
            {
                Add(new RichBoxNewLine());
            }
        }

        public void newParagraph()
        {
            Add(new RichBoxNewLine(true, 2f));
        }

        public RichboxButton Button(string caption, AbsRbAction action, AbsRbAction enter, bool enabled)
        {
            var result =new RichboxButton(
                new List<AbsRichBoxMember>
                {
                    new RichBoxText(caption),
                },
                action, enter, enabled);
            Add(result); 
            return result;
        }

        public RichBoxImage ListDot()
        {
            var dot =new  RichBoxImage(SpriteName.WhiteArea, 0.4f, 1f, 2f);
            dot.color = Color.DarkGray;
            Add(dot);
            return dot;
        }

    }
}
