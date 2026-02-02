using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.ToGG;

namespace VikingEngine.CardDesign
{
    class NumberDragButton : RbDragButton
    {
        public static void RbDragButtonGroup(RichBoxContent content, DragButtonSettings settings, IntGetSetTag intValue, bool useSymbols, object tag = null)
        {
            var dragButton = new NumberDragButton(settings, intValue, null, tag);

            //for (int i = options.Count - 1; i >= 0; --i)
            //{
                content.Add(new NumberOptionButton(dragButton, -Number.Endless.value, useSymbols));
            //}

            content.Add(dragButton);

            //for (int i = 0; i < options.Count; ++i)
            //{
                content.Add(new NumberOptionButton(dragButton, Number.Endless.value, useSymbols));
            //}
        }


        public NumberDragButton(DragButtonSettings settings, IntGetSetTag intValue, AbsRbAction enter = null, object tag = null)
            :base(settings, intValue, enter, tag)
        { }

        public override void valueChangeInput(float change, bool dragStep)
        {
            base.valueChangeInput(change, dragStep);
            if (change != 0)
            {
                int value = intValue.Invoke(tag, false, 0);
                if (Math.Abs(value) > Number.MaxValue)
                {
                    textPointer.pointer.TextString = new Number(value).ToString();
                }
            }
        }

        public override void refreshValueDisplay()
        {
            if (textPointer != null && textPointer.pointer != null)
            {
                int value = intValue.Invoke(tag, false, 0);
                if (Math.Abs(value) > Number.MaxValue)
                {
                    textPointer.pointer.TextString = new Number(value).ToString();
                }
                else
                {
                    textPointer.pointer.TextString = TextLib.LargeNumber(value);
                }
            }
        }
    }

    class NumberOptionButton : ArtButton
    {
        RbDragButton parent;
        float add;

        public NumberOptionButton(RbDragButton parent, int add, bool useSymbols)
        {
            this.parent = parent;
            this.buttonStyle = RbButtonStyle.Primary;


            this.add = add;

            if (Math.Abs(add) > Number.MaxValue)
            {
                content = new List<AbsRichBoxMember> { new RbText(new Number(add).ToString()) };
            }
            else if (useSymbols)
            {
                content = new List<AbsRichBoxMember> { new RbText(LangLib.ValueSymbol((int)add)) };
                enter = new RbTooltip_Text(TextLib.PlusMinus(add));
            }
            else
            {
                content = new List<AbsRichBoxMember> { new RbText(TextLib.PlusMinus(add)) };
            }
        }

        public override void onClick(RichMenu menu)
        {
            parent.valueChangeInput(add, false);
        }
    }

}
