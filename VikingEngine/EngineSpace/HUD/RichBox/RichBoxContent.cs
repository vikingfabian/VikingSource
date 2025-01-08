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

        public RbText text(string textline)
        {
            newLine();
            var textCont = new RbText(textline);
            Add(textCont);

            return textCont;
        }

        public RbText icontext(SpriteName icon, string textline)
        {
            newLine();
            Add(new RbImage(icon));
            space();
            var textCont = new RbText(textline);
            Add(textCont);

            return textCont;
        }

        public AbsRichBoxMember AddReturn(AbsRichBoxMember member)
        { 
            Add(member);
            return member;
        }

        //public void iconText(SpriteName icon, string textline)
        //{
        //    newLine();
        //    Add(new RichBoxImage(icon));
        //    Add(new RichBoxText(textline));
        //}

        public void space(float spaces = 1f)
        { 
            Add(new RichBoxSpace(spaces));
        }

        public RbText h1(string textline)
        {
            newLine();
            Add(new RbBeginTitle(1));
            var text = new RbText(textline);
            Add(text);

            return text;
        }

        public RbText h2(string textline)
        {
            newLine();
            Add(new RbBeginTitle(2));
            var text = new RbText(textline);
            Add(text);

            return text;
        }

        public void newLine()
        {
            if (this.Count > 0)
            {
                Add(new RbNewLine());
            }
        }

        public void newParagraph()
        {
            Add(new RbNewLine(true, 2f));
        }

        public RbButton Button(string caption, AbsRbAction action, AbsRbAction enter, bool enabled)
        {
            var result = new RbButton(
                new List<AbsRichBoxMember>
                {
                    new RbText(caption),
                },
                action, enter, enabled);
            Add(result);
            return result;
        }
        public RbButton Button(SpriteName icon, string caption, AbsRbAction action, AbsRbAction enter, bool enabled)
        {
            var buttonContent = new List<AbsRichBoxMember>(3);
            
            if (icon != SpriteName.NO_IMAGE)
            {
                buttonContent.Add(new RbImage(icon));
                buttonContent.Add(new RichBoxSpace());
            }
            buttonContent.Add(new RbText(caption));
            //{
            //    new RichBoxImage(icon),
            //    new RichBoxText(caption),
            //},

            var result = new RbButton(
                buttonContent,
                action, enter, enabled);
            Add(result);
            return result;
        }

        public RbImage BulletPoint()
        {
            var dot =new  RbImage(SpriteName.WhiteArea, 0.4f, 1f, 2f);
            dot.color = Color.DarkGray;
            Add(dot);
            return dot;
        }

        public void PlusMinusInt(SpriteName icon, string label, IntGetSetIx property, int propertyIx)
        {
            newLine();
            RbDisplay intDisplay = new RbDisplay(property, propertyIx);

            if (icon != SpriteName.NO_IMAGE)
            {
                Add(new RbImage(icon));
                Add(new RichBoxSpace(0.6f));
            }
            Add(new RbText(label));

            Add(new RbTab(0.4f));

            Add(new RbButton(
               new List<AbsRichBoxMember>
               {
                    new RbText("-10"),
               },
               new RbAction_ChangeInt(property, propertyIx, -10, intDisplay.refresh)));
            space();
            Add(new RbButton(
                new List<AbsRichBoxMember>
                {
                    new RbText("-1"),
                },
                new RbAction_ChangeInt(property, propertyIx, -1, intDisplay.refresh)));
            space();
            Add(intDisplay);
            space();
            Add(new RbButton(
                new List<AbsRichBoxMember>
                {
                    new RbText("+1"),
                },
                new RbAction_ChangeInt(property, propertyIx, +1, intDisplay.refresh)));
            space();
            Add(new RbButton(
               new List<AbsRichBoxMember>
               {
                    new RbText("+10"),
               },
               new RbAction_ChangeInt(property, propertyIx, +10, intDisplay.refresh)));
            
        }

        public void ButtonDescription(IButtonMap buttonMap, string desc)
        {
            newLine();
            this.buttonMap(buttonMap);
            space();
            Add(new RbText(desc));
        }
        public void buttonMap(IButtonMap buttonMap)
        {
            ButtonMap(buttonMap, this);
            //List<SpriteName> sprites = new List<SpriteName>(2);
            //buttonMap.ListIcons(sprites);

            //for (int i = 0; i < sprites.Count; i++)
            //{
            //    Add(new RichBoxImage(sprites[i]));
            //    if (i < sprites.Count - 1)
            //    {
            //        Add(new RichBoxText("+"));
            //    }
            //}
        }
        public static void ButtonMap(IButtonMap buttonMap, List<AbsRichBoxMember> content )
        { 
            List<SpriteName> sprites = new List<SpriteName>(2);
            buttonMap.ListIcons(sprites);

            for (int i = 0; i < sprites.Count; i++)
            {
                content.Add(new RbImage(sprites[i]));
                if (i < sprites.Count - 1)
                {
                    content.Add(new RbText("+"));
                }
            }
        }
    }
}
