using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Graphics;
using VikingEngine.Input;
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
            var result = new RichboxButton(
                new List<AbsRichBoxMember>
                {
                    new RichBoxText(caption),
                },
                action, enter, enabled);
            Add(result);
            return result;
        }
        public RichboxButton Button(SpriteName icon, string caption, AbsRbAction action, AbsRbAction enter, bool enabled)
        {
            var result = new RichboxButton(
                new List<AbsRichBoxMember>
                {
                    new RichBoxImage(icon),
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

        public void PlusMinusInt(SpriteName icon, string label, IntGetSetIx property, int propertyIx)
        {
            newLine();
            RichboxIntDisplay intDisplay = new RichboxIntDisplay(property, propertyIx);

            if (icon != SpriteName.NO_IMAGE)
            {
                Add(new RichBoxImage(icon));
                Add(new RichBoxSpace(0.6f));
            }
            Add(new RichBoxText(label));

            Add(new RichBoxTab(0.4f));

            Add(new RichboxButton(
               new List<AbsRichBoxMember>
               {
                    new RichBoxText("-10"),
               },
               new RbAction_ChangeInt(property, propertyIx, -10, intDisplay.refresh)));
            space();
            Add(new RichboxButton(
                new List<AbsRichBoxMember>
                {
                    new RichBoxText("-1"),
                },
                new RbAction_ChangeInt(property, propertyIx, -1, intDisplay.refresh)));
            space();
            Add(intDisplay);
            space();
            Add(new RichboxButton(
                new List<AbsRichBoxMember>
                {
                    new RichBoxText("+1"),
                },
                new RbAction_ChangeInt(property, propertyIx, +1, intDisplay.refresh)));
            space();
            Add(new RichboxButton(
               new List<AbsRichBoxMember>
               {
                    new RichBoxText("+10"),
               },
               new RbAction_ChangeInt(property, propertyIx, +10, intDisplay.refresh)));
            
        }

        public void ButtonDescription(IButtonMap buttonMap, string desc)
        {
            newLine();
            ButtonMap(buttonMap);
            space();
            Add(new RichBoxText(desc));
        }
        public void ButtonMap(IButtonMap buttonMap)
        { 
            List<SpriteName> sprites = new List<SpriteName>(2);
            buttonMap.ListIcons(sprites);

            for (int i = 0; i < sprites.Count; i++)
            {
                Add(new RichBoxImage(sprites[i]));
                if (i < sprites.Count - 1)
                {
                    Add(new RichBoxText("+"));
                }
            }
        }
    }
}
