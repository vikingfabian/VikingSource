using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;
using VikingEngine.EngineSpace.Maths;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.HUD
{
    class GuiRichLabel : GuiMember
    {
        RichBoxGroup boxGroup;

        public GuiRichLabel(List<AbsRichBoxMember> members, GuiLayout layout)
             : base(layout, null)
        {
            init(new RichBoxSettings(style), members, layout);
        }

        public GuiRichLabel(RichBoxSettings settings, List<AbsRichBoxMember> members, GuiLayout layout)
             : base(layout, null)
        {
            init(settings, members, layout);
        }

        void init(RichBoxSettings settings, List<AbsRichBoxMember> members, GuiLayout layout)
        {
            boxGroup = new RichBoxGroup(new Vector2(style.textEdgeSpace), size.X - style.textEdgeSpace * 2,
                layout.TextLayer, settings, members);

            foreach (var m in boxGroup.images)
            {
                this.AddAndUpdate(m as AbsDraw2D);
            }

            updateHeight(boxGroup.area.Height + style.textEdgeSpace * 2f, false);
            background.Color = style.Dark_LabelColor;
        }

        public override GuiMemberSelectionType SelectionType { get { return GuiMemberSelectionType.None; } }
    }

    abstract class AbsGuiLabel : GuiMember
    {
        /* Properties */
        public override GuiMemberSelectionType SelectionType { get { return GuiMemberSelectionType.None; } }

        /* Fields */
        public AbsTextLine text;

        /* Constructors */
        public AbsGuiLabel(string textString, bool multiline, TextFormat textFormat, GuiLayout layout)
            : base(layout, null)
        {
            if (multiline)
            {
                text = new TextBoxSimple(textFormat.Font, new Vector2(style.textEdgeSpace), textFormat.Scale, Align.Zero, textString, textFormat.Color, layout.TextLayer, size.X - style.textEdgeSpace * 2);
            }
            else
            {
                text = new TextG(textFormat.Font, new Vector2(style.textEdgeSpace), textFormat.Scale, Align.Zero, textString, textFormat.Color, layout.TextLayer);
            }
            text.SetMaxWidth(size.X - style.textEdgeSpace * 2);
            this.AddAndUpdate(text);

            updateHeight(text.MeasureText().Y + style.textEdgeSpace * 2f, false);
            background.Color = style.Dark_LabelColor;
        }

        public AbsGuiLabel(string textString, GuiLayout layout)
            : this(textString, false, layout.gui.style.textFormat, layout)
        { }

        /* Family Methods */
        public override void Refresh(GuiMember sender)
        {
            base.Refresh(sender);

            text.TextString = GetValue();
            updateHeight(text.MeasureText().Y + style.textEdgeSpace * 2f);
        }

        /* Novelty Methods */
        protected abstract void SetValue(string value);
        protected abstract string GetValue();
    }

    class GuiLabel : AbsGuiLabel
    {
        /* Constructors */
        public GuiLabel(string textString, bool multiline, TextFormat textFormat, GuiLayout layout)
            : base(textString, multiline, textFormat, layout)
        { }

        public GuiLabel(string textString, GuiLayout layout)
            : base(textString, layout)
        { }

        /* Family Methods */
        protected override void SetValue(string value)
        {
            return;
        }

        protected override string GetValue()
        {
            return text.TextString;
        }
    }

    class GuiImageLabel : GuiMember
    {
        /* Properties */
        public override GuiMemberSelectionType SelectionType { get { return GuiMemberSelectionType.None; } }

        /* Fields */
        protected Graphics.Image image;

        /* Constructors */
        public GuiImageLabel(SpriteName sprite, GuiLayout layout)
            : base(layout, null)
        {

            float height = size.Y - style.memberSpacing;
            var rect = DataLib.SpriteCollection.Get(sprite).Source;
            float width = height / rect.Height * rect.Width;

            image = new Image(sprite, new Vector2(style.memberSpacing), new Vector2(width, height), layout.TextLayer);
            this.AddAndUpdate(image);

            background.Color = style.Dark_LabelColor;
            //background.Color = style.BackgroundColor;
        }

    }

    class GuiLargeImageLabel : GuiImageLabel
    {
        public GuiLargeImageLabel(SpriteName sprite, GuiLayout layout)
            : base(sprite, layout)
        {

        }

        protected override GuiMemberSizeType SizeType => GuiMemberSizeType.LargeButtonSize;
    }

    class GuiTitle : GuiLabel
    {
        /* Properties */
        protected override GuiMemberSizeType SizeType { get { return GuiMemberSizeType.FullWidth; } }

        /* Constructors */
        public GuiTitle(string str, bool multiline, TextFormat format, GuiLayout layout)
            : base(str, multiline, format, layout)
        {
            SetTextColor();
        }

        public GuiTitle(string str, GuiLayout layout)
            : this(str, false, layout.gui.style.textFormat, layout)
        { }

        /* Family Methods */
        public override void Refresh(GuiMember sender)
        {
            base.Refresh(sender);
            SetTextColor();
        }

        /* Novelty Methods */
        void SetTextColor()
        {
            text.Color = style.Tint;
        }
    }

    class GuiImageTitle : GuiMember
    {
        /* Properties */
        public override GuiMemberSelectionType SelectionType { get { return GuiMemberSelectionType.None; } }

        /* Fields */
        protected Graphics.Image image;

        /* Constructors */
        public GuiImageTitle(SpriteName sprite, GuiLayout layout)
            : base(layout, null)
        {

            float height = size.Y - style.memberSpacing;
            var rect = DataLib.SpriteCollection.Get(sprite).Source;
            float width = height / rect.Height * rect.Width;

            image = new Image(sprite, new Vector2(style.memberSpacing), new Vector2(width, height), layout.TextLayer);
            this.AddAndUpdate(image);

            background.Color = style.BackgroundColor;
        }

        protected override GuiMemberSizeType SizeType
        {
            get
            {
                return GuiMemberSizeType.HalfHeight;
            }
        }

    }
}
