using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;
using VikingEngine.EngineSpace.Maths;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.HUD
{
    
    //enum GuiMemberSizeType
    //{
    //    FullWidth,
    //    NormalMemberWidth,
    //    HalfHeight,
    //    Square,
    //    SquareHalfSize,
    //    SquareDoubleSize,
    //    Scrollbar,
    //}

    class GuiMember : ImageGroupParent2D
    {
        protected GuiLayout layoutParent;
        protected GuiStyle style;
        public int propertyIndex = int.MinValue;

        public Vector2 size;
        //protected virtual GuiMemberSizeType SizeType { get { return GuiMemberSizeType.NormalMemberWidth; } }
        protected virtual GuiMemberSizeType SizeType { get { return GuiMemberSizeType.StandardButtonSize; } }
        public virtual GuiMemberSelectionType SelectionType { get { return GuiMemberSelectionType.Selectable; } }

        protected Graphics.Image background;

        protected virtual bool AutoAddToLayout { get { return true; } }

        public string ToolTip;

        bool isHovered;
        public virtual bool IsHovered
        {
            get
            {
                return isHovered;
            }
            set
            {
                if (value) // cursor is over
                {
                    if (isHovered)
                        OnHover();
                    else
                    {
                        isHovered = true;
                        OnEnter();
                    }
                }
                else
                {
                    if (isHovered)
                    {
                        isHovered = false;
                        OnLeave();
                    }
                }
            }
        }

        protected bool isPressed;
        virtual public bool IsPressed
        {
            get
            {
                return isPressed;
            }
            set
            {
                if (value) // pressing
                {
                    if (isPressed)
                        OnHold();
                    else
                    {
                        isPressed = true;
                        OnPress();
                    }
                }
                else
                {
                    if (isPressed)
                    {
                        isPressed = false;
                        OnRelease();
                    }
                }
            }
        }

        //public GuiMember(GuiLayout layout, string toolTip)
        protected static Vector2 GetSize(GuiLayout layout, GuiMemberSizeType type)
        {
            //ayoutParent = layout;
            //style = layout.gui.style;
            var style = layout.gui.style;

            //switch (SizeType)
            float buttonW = layout.renderList.Width - style.memberSpacing * 2;
            Vector2 size;

            switch (type)
            {
                case GuiMemberSizeType.FullWidth:
                    size = new Vector2(layout.renderList.Width, style.memberHeight + style.memberSpacing);
                    break;
                //case GuiMemberSizeType.NormalMemberWidth:
                //    size = new Vector2(layout.renderList.Width - style.memberSpacing * 2, style.memberHeight);
                case GuiMemberSizeType.StandardButtonSize:
                    size = new Vector2(buttonW, style.memberHeight);
                    break;
                case GuiMemberSizeType.LargeButtonSize:
                    size = new Vector2(buttonW, style.memberHeight * GuiStyle.LargeButtonScaleUp);
                    break;
                case GuiMemberSizeType.HalfHeight:
                //    size = new Vector2(layout.renderList.Width - style.memberSpacing * 2, style.memberHeight / 2);
                    size = new Vector2(buttonW, style.memberHeight / 2);
                    break;
                case GuiMemberSizeType.Square:
                    size = new Vector2(style.memberHeight, style.memberHeight);
                    break;
                case GuiMemberSizeType.SquareDoubleSize:
                    // const int RowCount = 4;
                    // size = new Vector2( (layout.renderList.Width - (RowCount + 1) * style.memberSpacing) / RowCount);//new Vector2(style.memberHeight * 2, style.memberHeight * 2);
                    {
                        const int RowCount = 6;

                        size = new Vector2((layout.renderList.Width - (RowCount + 1) * style.memberSpacing) / RowCount);//new Vector2(style.memberHeight * 2, style.memberHeight * 2);
                    }
                    break;
                case GuiMemberSizeType.SquareHalfSize:
                    {
                        const int RowCount = 24;
                        size = new Vector2((layout.renderList.Width - (RowCount + 1) * style.memberSpacing) / RowCount);
                    }
                    break;
                case GuiMemberSizeType.Scrollbar:
                    size = new Vector2(layout.scrollerRenderer.Width, layout.gui.area.Height);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return size;
        }

        public GuiMember(GuiLayout layout, string toolTip)
        {
            layoutParent = layout;
            style = layout.gui.style;

            float buttonW = layout.renderList.Width - style.memberSpacing * 2;

            size = GetSize(layout, SizeType);
          
            if (AutoAddToLayout)
                layout.Add(this);

            background = new Image(style.background, Vector2.Zero, size, layout.Layer);
            background.Color = style.Mid_ButtonCol;

            this.AddAndUpdate(background);

            this.ToolTip = toolTip;
        }

        public void setBackGroundCol(Color color, Color highlight)
        {
            background.Color = color;
            style.Mid_ButtonCol = color;
            style.ShineColor = highlight;
        }

        public void AddToLayout()
        {
            layoutParent.Add(this);
        }

        protected void updateHeight(float h)
        { this.updateHeight(h, true); }
        protected void updateHeight(float h, bool tellLayout)
        {
            float changeAmount = h - size.Y;
            background.Height = h;
            size.Y = h;
            if (tellLayout)
                layoutParent.MemberHeightUpdate(this, changeAmount);
        }

        virtual public bool HorizontalStacking { get { return false; } }

        public Vector2 FindScreenPosition()
        {
            if (this is GuiScrollbar)
            {
                return layoutParent.scrollerRenderer.Position + ParentPosition;
            }
            return layoutParent.renderList.Transform2D + 
                layoutParent.renderList.position + ParentPosition;
        }

        public VectorRect GetVisibleRect()
        {
            VectorRect rect = new VectorRect(FindScreenPosition(), size);
            return rect.AreaIntersection(layoutParent.gui.area);
        }

        protected Vector2 FindLocalCursorPos()
        {
            return Input.Mouse.Position - FindScreenPosition();
        }

        virtual protected void OnEnter()
        {
            background.Color = style.ShineColor;
            new ChangeColor(background, style.ShineColor, style.fadeTimeMS, true); // makes sure we override any previous fade out too
        }

        virtual protected void OnHover()
        {

        }

        virtual protected void OnLeave()
        {
            new ChangeColor(background, style.Mid_ButtonCol, style.fadeTimeMS, true);
        }

        virtual protected void OnPress()
        {
            Color color = ColorExt.Multiply(style.ShineColor, style.Tint);
            background.Color = color;
        }

        virtual protected void OnHold()
        {
        }

        virtual protected void OnRelease()
        {
            new ChangeColor(background, style.ShineColor, style.fadeTimeMS, true);
        }

        virtual public void OnMouseDrag()
        {
        }

        virtual public void MoveInputWhenSelected(Vector2 move)
        {
        }

        virtual public void MoveInputWhenHover(Vector2 move)
        {
        }

        virtual public void Refresh(GuiMember sender)
        {

        }

        virtual public void update() { }
    }

    abstract class AbsGuiButton : GuiMember
    {
        public IGuiAction clickAction, hoverAction = null;
        public GuiHoldSettings holdSettings = null;
        //public IGuiAction holdAction;

        protected Image moreMenusArrow = null;

        public AbsGuiButton(IGuiAction action, string toolTip, bool showMoreMenusArrow, GuiLayout layout)
            : base(layout, toolTip)
        {
            this.clickAction = action;
            if (showMoreMenusArrow)
            {
                Vector2 moreMenusArrowSize = new Vector2(style.memberHeight * 0.4f);
                moreMenusArrow = new Image(SpriteName.LfMenuMoreMenusArrow, 
                    new Vector2(size.X - style.memberSpacing - moreMenusArrowSize.X, size.Y * 0.5f), 
                    moreMenusArrowSize, layout.TextLayer, false, true);
                moreMenusArrow.origo = new Vector2(0, 0.5f);
                this.AddAndUpdate(moreMenusArrow);
            }
        }

        protected override void OnPress()
        {
            base.OnPress();

            clickAction?.guiActionTrigger(layoutParent.gui, this);

            if (holdSettings != null)
            {
                holdSettings.reset();
                //OnRelease();
            }
            else
            {
IsPressed = false;
            }

            layoutParent.gui.RefreshAllMembers(this);
        }

        protected override void OnHover()
        {
            base.OnHover();
            hoverAction?.guiActionTrigger(layoutParent.gui, this);
        }

        //protected override void OnHold()
        //{
        //    base.OnHold();

        //    holdAction?.guiActionTrigger(layoutParent.gui, this);
        //}

        public override void update()
        {
            base.update();

            if (isPressed && holdSettings != null && clickAction != null)
            {
                if (holdSettings.update())
                {
                    clickAction.guiActionTrigger(layoutParent.gui, this);
                }
            }
        }
    }

    class GuiImageButton : AbsGuiButton
    {
        protected Image image;

        public GuiImageButton(SpriteName SpriteName, string toolTip, Action action, bool showMoreMenusArrow, GuiLayout layout)
            : this(SpriteName, toolTip, new GuiAction(action), showMoreMenusArrow, layout)
        { }

        public GuiImageButton(SpriteName SpriteName, string toolTip, IGuiAction action, bool showMoreMenusArrow, GuiLayout layout)
            : base(action, toolTip, showMoreMenusArrow, layout)
        {
            float height = style.memberHeight - style.textEdgeSpace * 2;
            var rect = DataLib.SpriteCollection.Get(SpriteName).Source;
            float width = height / rect.Height * rect.Width;

            image = new Image(SpriteName,
                new Vector2(style.textEdgeSpace),
                new Vector2(width, height),
                layoutParent.UnderTextLayer);

            this.AddAndUpdate(image);
        }
        
        protected virtual Vector2 GetTextOffset()
        {
            return Vector2.Zero;
        }

        protected virtual float GetTextWidthModifier()
        {
            return 0;
        }
    }

    class GuiDirectionalMapButton : GuiTextButton
    {
        VikingEngine.Input.IDirectionalMap map;
        string title;

        public GuiDirectionalMapButton(string title, VikingEngine.Input.IDirectionalMap map, IGuiAction action, bool showMoreMenusArrow, GuiLayout layout)
            : base("", null, action, showMoreMenusArrow, layout)
        {
            this.title = title;
            this.map = map;
        }

        public override void update()
        {
            text.TextString = title + map.direction.ToString();
        }
    }

    class GuiLargeTextButton : GuiTextButton
    {
        public GuiLargeTextButton(string textString, string toolTip, IGuiAction action, bool showMoreMenusArrow, GuiLayout layout)
            : base(textString, toolTip, action, showMoreMenusArrow, layout)
        {
            text.Size *= GuiStyle.LargeButtonScaleUp;
        }

        protected override GuiMemberSizeType SizeType
        {
            get
            {
                return GuiMemberSizeType.LargeButtonSize;
            }
        }
    }

    class GuiRichButton : AbsGuiButton
    {
        RichBoxGroup boxGroup;
        public GuiRichButton(RichBoxSettings settings, List<AbsRichBoxMember> members, string toolTip, IGuiAction action, bool showMoreMenusArrow, GuiLayout layout)
             : base(action, toolTip, showMoreMenusArrow, layout)
        {
            boxGroup = new RichBoxGroup(new Vector2(style.textEdgeSpace), size.X - style.textEdgeSpace * 2,
                layout.TextLayer, settings, members);

            foreach (var m in boxGroup.images)
            {
                this.AddAndUpdate(m as AbsDraw2D);
            }
        }
    }

    class GuiTextButton : AbsGuiButton
    {
        //protected TextG text;
        public TextG text;

        DelayedGetCall1Arg<string> textStringGetter;

        public GuiTextButton(string textString, string toolTip, Action action, bool showMoreMenusArrow, GuiLayout layout)
            : this(textString, toolTip, new GuiAction(action), showMoreMenusArrow, layout)
        { }

        public GuiTextButton(DelayedGetCall1Arg<string> textStringGetter, string toolTip, IGuiAction action, bool moreMenusArrow, GuiLayout layout)
            : this(textStringGetter.InvokeGet(), toolTip, action, moreMenusArrow, layout)
        {
            this.textStringGetter = textStringGetter;
        }

        public GuiTextButton(DelayedGetCall1Arg<string> textStringGetter, string toolTip, Action action, bool moreMenusArrow, GuiLayout layout)
            : this(textStringGetter, toolTip, new GuiAction(action), moreMenusArrow, layout)
        { }

        public GuiTextButton(string textString, string toolTip, IGuiAction action, bool showMoreMenusArrow, GuiLayout layout, bool bInitText = true)
            : base(action, toolTip, showMoreMenusArrow, layout)
        {
            //text = new TextG(layout.gui.style.textFormat.Font, new Vector2(style.textEdgeSpace, style.memberHeight * 0.5f) + GetTextOffset(),
            if (bInitText)
            {
                initText(textString, showMoreMenusArrow, layout);
            }
        }

        protected void initText(string textString, bool showMoreMenusArrow, GuiLayout layout)
        {
            var size = GetSize(layout, this.SizeType);

            text = new TextG(layout.gui.style.textFormat.Font, new Vector2(style.textEdgeSpace, size.Y * 0.5f) + GetTextOffset(),
                style.textFormat.Scale, Align.CenterHeight, textString, style.textFormat.Color, layoutParent.TextLayer);
            if (showMoreMenusArrow)
            {
                text.SetMaxWidth(size.X - moreMenusArrow.Width - style.textEdgeSpace * 2 + GetTextWidthModifier());
            }
            this.AddAndUpdate(text);
        }

        

        protected virtual Vector2 GetTextOffset()
        {
            return Vector2.Zero;
        }

        protected virtual float GetTextWidthModifier()
        {
            return 0;
        }

        public override void Refresh(GuiMember sender)
        {
            base.Refresh(sender);
            if (textStringGetter != null)
            {
                text.TextString = textStringGetter.InvokeGet();
            }
        }

        protected override GuiMemberSizeType SizeType
        {
            get
            {
                return base.SizeType;
            }
        }
    }

    class GuiIconTextButton : GuiTextButton
    {
        public ImageAdvanced icon;
        DelayedGetCall1Arg<SpriteName> SpriteNameGetter;

        public GuiIconTextButton(SpriteName iconTile, string textString, string toolTip, IGuiAction action, bool showMoreMenusArrow, GuiLayout layout)
            : base(textString, toolTip, action, showMoreMenusArrow, layout, false)
        {
            SetImg(iconTile);
            this.AddAndUpdate(icon);

            initText(textString, showMoreMenusArrow, layout);
            //this.AddAndUpdate(text);
        }

        public GuiIconTextButton(SpriteName iconTile, string textString, string toolTip, Action action, bool showMoreMenusArrow, GuiLayout layout)
            : this(iconTile, textString, toolTip, new GuiAction(action), showMoreMenusArrow, layout)
        { }

        public GuiIconTextButton(DelayedGetCall1Arg<SpriteName> SpriteNameGetter, string textString, string toolTip, IGuiAction action, bool showMoreMenusArrow, GuiLayout layout)
            : this(SpriteNameGetter.InvokeGet(), textString, toolTip, action, showMoreMenusArrow, layout)
        {
            this.SpriteNameGetter = SpriteNameGetter;
        }

        public GuiIconTextButton(DelayedGetCall1Arg<SpriteName> SpriteNameGetter, string textString, string toolTip, Action action, bool showMoreMenusArrow, GuiLayout layout)
            : this(SpriteNameGetter, textString, toolTip, new GuiAction(action), showMoreMenusArrow, layout)
        { }

        public GuiIconTextButton(DelayedGetCall1Arg<SpriteName> SpriteNameGetter, DelayedGetCall1Arg<string> textStringGetter, string toolTip, IGuiAction action, bool showMoreMenusArrow, GuiLayout layout)
            : base(textStringGetter, toolTip, action, showMoreMenusArrow, layout)
        {
            this.SpriteNameGetter = SpriteNameGetter;
            SetImg(SpriteNameGetter.InvokeGet());
            this.AddAndUpdate(icon);
            //this.AddAndUpdate(text);
        }

        public GuiIconTextButton(DelayedGetCall1Arg<SpriteName> SpriteNameGetter, DelayedGetCall1Arg<string> textStringGetter, string toolTip, Action action, bool showMoreMenusArrow, GuiLayout layout)
            : this(SpriteNameGetter, textStringGetter, toolTip, new GuiAction(action), showMoreMenusArrow, layout)
        { }

        void SetImg(SpriteName SpriteName)
        {
           
            icon = new ImageAdvanced(SpriteName,
                new Vector2(style.textEdgeSpace),
                new Vector2(this.size.Y - style.textEdgeSpace * 2),
                layoutParent.UnderTextLayer, false);
            if (SpriteName == SpriteName.NO_IMAGE)
            {
                icon.Visible = false;
            }
            else
            {
                icon.Visible = true;
            }
        }

        protected override Vector2 GetTextOffset()
        {
            if (icon != null && icon.Visible)
            {
                return new Vector2(this.size.Y - style.textEdgeSpace, 0);
            }
            return Vector2.Zero;
        }

        protected override float GetTextWidthModifier()
        {
            return - style.memberHeight + style.textEdgeSpace;
        }

        public override void Refresh(GuiMember sender)
        {
            base.Refresh(sender);
            if (SpriteNameGetter != null)
            {
                SpriteName SpriteName = SpriteNameGetter.InvokeGet();
                icon.SetSpriteName(SpriteName);
                if (SpriteName == SpriteName.NO_IMAGE)
                {
                    icon.Visible = false;
                }
                else
                {
                    icon.Visible = true;
                }
            }
        }
    }

    class GuiLargeIconTextButton : GuiIconTextButton
    {
        public GuiLargeIconTextButton(SpriteName iconTile, string textString, string toolTip, IGuiAction action, bool showMoreMenusArrow, GuiLayout layout)
            : base(iconTile,textString, toolTip, action, showMoreMenusArrow, layout)
        {
            text.Size *= GuiStyle.LargeButtonScaleUp;
        }

        protected override GuiMemberSizeType SizeType
        {
            get
            {
                return GuiMemberSizeType.LargeButtonSize;
            }
        }
    }

    class GuiDialogButton : GuiTextButton
    {
        string label;

        public GuiDialogButton(string textString, string toolTip, IGuiAction action, bool showMoreMenusArrow, GuiLayout layout)
            : base(textString, toolTip, action, showMoreMenusArrow, layout)
        {
            label = textString;
        }

        protected override void OnPress()
        {
            confirmationDialog(clickAction);
            IsPressed = false;
            OnRelease();
        }

        public void confirmationDialog(IGuiAction action)
        {
            GuiAction1Arg<IGuiAction> doThis = new GuiAction1Arg<IGuiAction>(DoAction, action);
            GuiLayout layout = new GuiLayout(label + "?", layoutParent.gui);
            {
                new GuiTextButton(Ref.langOpt.Hud_Yes, null, doThis, moreMenusArrow != null, layout);
                new GuiTextButton(Ref.langOpt.Hud_No, null, new GuiAction(layoutParent.gui.PopLayout), moreMenusArrow != null, layout);
            }
            layout.End();
        }

        void DoAction(IGuiAction action)
        {
            action.guiActionTrigger(layoutParent.gui, this);
            layoutParent.gui.PopLayout();
        }
    }

    class GuiIcon : AbsGuiButton
    {
        public ImageAdvanced iconImage;
 
        public GuiIcon(SpriteName image, string toolTip, IGuiAction action, bool showMoreMenusArrow, GuiLayout layout)
            : base(action, toolTip, showMoreMenusArrow, layout)
        {
            background.Size = size;

            //float borderSize = size.X / 8;

            //Vector2 imagePos = new Vector2(borderSize);
            //Vector2 imageSz = size - new Vector2(2 * borderSize);

            //if (showMoreMenusArrow)
            //{
            //    imageSz *= 0.8f;
            //    imagePos.Y += imageSz.Y * 0.1f;
            //}

            iconImage = new ImageAdvanced(image, Vector2.Zero, Vector2.One, layout.Layer - 1, false);

            iconScale(showMoreMenusArrow ? 0.8f : 1f);

            this.AddAndUpdate(iconImage);
        }

        public void iconScale(float scalePerc)
        {
            float borderSize = size.X / 8;

            //Vector2 imagePos = new Vector2(borderSize);
            //Vector2 imageSz = size - new Vector2(2 * borderSize);

            VectorRect area = new VectorRect(
                new Vector2(borderSize),
                size - new Vector2(2 * borderSize));

            if (scalePerc != 1f)
            {
                float goalW = scalePerc * area.Width;
                float diff = goalW - area.Width;

                area.AddRadius(diff * 0.5f);
            }

            iconImage.Position = area.Position;
            iconImage.Size = area.Size;
        }

        protected override GuiMemberSizeType SizeType { get { return GuiMemberSizeType.Square; } }
    }

    class GuiBigIcon : GuiIcon
    {
        public GuiBigIcon(SpriteName image, string toolTip, IGuiAction action, bool showMoreMenusArrow, GuiLayout layout)
            : base(image, toolTip, action, showMoreMenusArrow, layout)
        { }

        protected override GuiMemberSizeType SizeType { get { return GuiMemberSizeType.SquareDoubleSize; } }
    }

    class GuiSmallIcon : GuiIcon
    {
        public GuiSmallIcon(SpriteName image, string toolTip, IGuiAction action, bool showMoreMenusArrow, GuiLayout layout)
            : base(image, toolTip, action, showMoreMenusArrow, layout)
        { }

        protected override GuiMemberSizeType SizeType { get { return GuiMemberSizeType.SquareHalfSize; } }
    }

    class GuiDelegateLabel : AbsGuiLabel
    {
        /* Fields */
        StringGetSet property;

        /* Constructors */
        public GuiDelegateLabel(StringGetSet property, bool multiline, TextFormat textFormat, GuiLayout layout)
            : base(property(false, null), multiline, textFormat, layout)
        {
            this.property = property;
        }

        public GuiDelegateLabel (StringGetSet property, GuiLayout layout)
            : base(property(false, null), layout)
        {
            this.property = property;
        }

        /* Family Methods */
        protected override void SetValue(string value)
        {
            property(true, value);
        }

        protected override string GetValue()
        {
            return property(false, null);
        }
    }

    class GuiInstanceLabel : AbsGuiLabel
    {
        /* Fields */
        IFieldReflector<string> property;

        /* Constructors */
        public GuiInstanceLabel(IFieldReflector<string> property, bool multiline, TextFormat textFormat, GuiLayout layout)
            : base("", multiline, textFormat, layout)
        {
            this.property = property;
        }

        public GuiInstanceLabel(IFieldReflector<string> property, GuiLayout layout)
            : base("", layout)
        {
            this.property = property;
        }

        /* Family Methods */
        protected override void SetValue(string value)
        {
            property.Set(value);
        }

        protected override string GetValue()
        {
            return property.Get();
        }
    }

    

    class GuiCheckbox : AbsGuiCheckbox
    {
        /* Fields */
        BoolGetSet property;

        /* Constructors */
        public GuiCheckbox(string label, string toolTip, BoolGetSet property, GuiLayout layout)
            : base(label, toolTip, layout)
        {
            this.property = property;
        }

        /* Family Methods */
        protected override bool GetValue()
        {
            return property(propertyIndex, false, false);
        }

        protected override void SetValue(bool value)
        {
            property(propertyIndex, true, value);
        }

    }



    class GuiInstanceCheckbox : AbsGuiCheckbox
    {
        /* Fields */
        IFieldReflector<bool> property;

        /* Constructors */
        public GuiInstanceCheckbox(string label, string toolTip, IFieldReflector<bool> property, GuiLayout layout)
            : base(label, toolTip, layout)
        {
            this.property = property;
        }

        /* Family Methods */
        protected override bool GetValue()
        {
            return property.Get();
        }

        protected override void SetValue(bool value)
        {
            property.Set(value);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            property.DeleteMe();
        }
    }

    abstract class AbsGuiCheckbox : GuiMember
    {
        

        /* Properties */
        public bool Value
        {
            get { return val; }
            set
            {
                val = value;
                if (val)
                {
                    checkboxImage.SetSpriteName(SpriteName.LfCheckYes);
                    checkboxImage.Color = Color.White;
                }
                else
                {
                    checkboxImage.SetSpriteName(SpriteName.LfCheckNo);
                    checkboxImage.Color = Color.LightGray;
                }
            }
        }

        /* Fields */
        public bool val;
        Image checkboxImage;

        /* Constructors */
        public AbsGuiCheckbox(string label, string toolTip, GuiLayout layout)
            : base(layout, toolTip)
        {
            Vector2 pos = new Vector2(GetXOffset(), 0);
            checkboxImage = new Image(SpriteName.NO_IMAGE, pos, new Vector2(style.memberHeight), layout.TextLayer, false, true);

            TextG text = new TextG(style.textFormat.Font, new Vector2(style.textEdgeSpace),
                style.textFormat.Scale, Align.Zero, label, style.textFormat.Color, layout.TextLayer);
            text.Xpos += style.memberHeight + GetXOffset();
            text.SetMaxWidth(size.X - style.textEdgeSpace * 2 - checkboxImage.Width);

            this.AddAndUpdate(text);
            this.AddAndUpdate(checkboxImage);
        }

        /* Family Methods */
        protected override void OnPress()
        {
            base.OnPress();

            Value = !Value;

            SetValue(Value);

            IsPressed = false;
            OnRelease();
        }

        public override void Refresh(GuiMember sender)
        {
            base.Refresh(sender);
            Value = GetValue();
        }

        /* Novelty Methods */
        protected abstract bool GetValue();
        protected abstract void SetValue(bool value);

        protected virtual float GetXOffset()
        {
            return 0;
        }
    }

    class GuiRadioButtonGroup : GuiLabel
    {
        class GuiRadioButton : GuiCheckbox
        {
            GuiRadioButtonGroup group;
            int index;

            protected override float GetXOffset()
            {
                return layoutParent.gui.style.memberHeight / 2;
            }

            public GuiRadioButton(GuiRadioButtonGroup group, string label, string toolTip, int index, GuiLayout layout)
                : base(label, toolTip, null, layout)
            {
                this.group = group;
                this.index = index;
            }

            protected override void OnPress()
            {
                base.OnPress();
                group.TellSet(index);
            }
        }

        List<GuiRadioButton> radioButtons;

        IntGetSet intProperty;

        public GuiRadioButtonGroup(string title, string[] labels, string[] toolTips, IntGetSet property, GuiLayout layout)
            : base(title, layout)
        {
            radioButtons = new List<GuiRadioButton>(labels.Length);

            for (int i = 0; i < labels.Length; ++i)
            {
                radioButtons.Add(new GuiRadioButton(this, labels[i], toolTips[i], i, layout));
            }

            this.intProperty = property;
            TellSet(property(false, int.MinValue));
        }

        void TellSet(int index)
        {
            for (int i = 0; i < radioButtons.Count; ++i)
            {
                radioButtons[i].Value = index == i;
            }

            if (intProperty != null)
                intProperty(true, index);
        }
    }

    

    class GuiInstanceFloatSlider : AbsGuiSlider
    {
        IFieldReflector<float> property;

        /* Constructors */
        public GuiInstanceFloatSlider(string label, IFieldReflector<float> property, IntervalF range, bool vertical, GuiLayout layout)
            : base(SpriteName.NO_IMAGE, label, null, range, vertical, layout)
        {
            this.property = property;
            valueFormat = "0.##";
        }

        /* Family Methods */
        protected override void SetValue(float value)
        {
            property.Set(value);
        }

        protected override float GetValue()
        {
            return property.Get();
        }

        protected override bool ValuesMatch(float value)
        {
            float actual = GetValue();
            return Math.Abs(actual - value) < 0.001f;
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            property.DeleteMe();
        }
    }

    class GuiInstanceIntSlider : AbsGuiSlider 
    {
        IFieldReflector<int> property;

        /* Constructors */
        public GuiInstanceIntSlider(string label, IFieldReflector<int> property, IntervalF range, bool vertical, GuiLayout layout)
            : base( SpriteName.NO_IMAGE, label, null, range, vertical, layout)
        {
            this.property = property;
            valueFormat = "0";
        }

        /* Family Methods */
        protected override void SetValue(float value)
        {
            property.Set(Convert.ToInt32(value));
        }

        protected override float GetValue()
        {
            return property.Get();
        }

        protected override bool ValuesMatch(float value)
        {
            int actual = Convert.ToInt32(GetValue());
            int compare = Convert.ToInt32(value);

            return actual == compare;
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            property.DeleteMe();
        }
    }

    

    class GuiScrollbar : GuiFloatSlider
    {
        public GuiScrollbar(FloatGetSet property, GuiLayout layout)
            : base(SpriteName.NO_IMAGE, null, property, new IntervalF(0, layout.totalHeight - layout.gui.area.Height), true, layout)
        {
            background.Color = style.Dark_LabelColor;
            slider.Color = style.SliderColor;
            UpdateSliderPosition();
            slider.Height = (layout.gui.area.Height / layout.totalHeight) * layout.gui.area.Height - style.memberSpacing * 2;
        }

        protected override bool AutoAddToLayout { get { return false; } }

        protected override GuiMemberSizeType SizeType { get { return GuiMemberSizeType.Scrollbar; } }
    }

    //class GuiColorPicker : AbsGuiSlider
    //{
    //    ColorGetSet property;
    //    MultiLerpVec3 gradient;

    //    public GuiColorPicker(string label, string toolTip, ColorGetSet property, GuiLayout layout)
    //        : base(label, null, new IntervalF(0, 1), false, layout)
    //    {
    //        this.property = property;

    //        gradient = new MultiLerpVec3();
    //        gradient.Add(new Vector3(1, 0, 0));
    //        gradient.Add(new Vector3(1, 0, 1));
    //        gradient.Add(new Vector3(0, 0, 1));
    //        gradient.Add(new Vector3(0, 1, 1));
    //        gradient.Add(new Vector3(0, 1, 0));
    //        gradient.Add(new Vector3(1, 1, 0));
    //        gradient.Add(new Vector3(1, 0, 0));

    //        Color initialValue = property(false, new Color(0, 0, 0));
    //        valueT = gradient.FindT(initialValue.ToVector3());
    //        UpdateSliderPosition();

    //        slider.Color = lib.MixColors(style.ShineColor, initialValue);
    //    }

    //    /* Family Methods */
    //    protected override void SetValue(float value)
    //    {
            
    //    }

    //    protected override float GetValue()
    //    {
    //        return valueT;
    //    }

    //    protected override bool ValuesMatch(float value)
    //    {
    //        float actual = GetValue();
    //        return actual - value < 0.0001f;
    //    }

    //    protected override void OnSliderMoved()
    //    {
    //        base.OnSliderMoved();

    //        Color color = new Color(gradient.GetAt(valueT));

    //        if (property != null)
    //            property(true, color);

    //        style.Tint = color;
    //        background.Color = lib.MixColors(style.Tint, style.ShineColor);
    //    }

    //    protected override void OnPress()
    //    {
    //        new ChangeColor(slider, style.MiddleColor, style.fadeTimeMS, true);
    //        new ChangeColor(background, lib.MixColors(style.ShineColor, style.Tint), style.fadeTimeMS, true);

    //        Engine.Sound.PlaySound(style.openSound, 1.0f);
    //    }

    //    protected override void OnRelease()
    //    {
    //        new ChangeColor(slider, lib.MixColors(style.ShineColor, style.Tint), style.fadeTimeMS, true);
    //        new ChangeColor(background, style.BrightestColor, style.fadeTimeMS, true);

    //        Engine.Sound.PlaySound(style.closeSound, 1.0f);
    //    }

    //    protected override void OnEnter()
    //    {
    //        new ChangeColor(background, style.BrightestColor, style.fadeTimeMS, true);
    //    }

    //    protected override void OnLeave()
    //    {
    //        new ChangeColor(background, style.DarkestColor, style.fadeTimeMS, true);

    //        //Engine.Sound.PlaySound(style.closeSound, 1.0f, new Pan(0), 1.5f);
    //    }

    //    protected override void UpdateText()
    //    { }
    //}

    class GuiSectionSeparator : GuiMember
    {
        /* Properties */
        public override GuiMemberSelectionType SelectionType { get { return GuiMemberSelectionType.None; } }
        protected override GuiMemberSizeType SizeType { get { return GuiMemberSizeType.HalfHeight; } }

        /* Constructors */
        public GuiSectionSeparator(GuiLayout layout)
            : base(layout, null)
        {
            background.Color = layout.gui.style.BackgroundColor;
        }
    }

    
}
