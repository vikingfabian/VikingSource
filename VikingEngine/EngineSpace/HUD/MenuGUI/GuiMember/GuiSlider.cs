using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;
using VikingEngine.EngineSpace.Maths;

namespace VikingEngine.HUD
{
    abstract class AbsGuiSlider : GuiMember
    {
        /* Constants */
        const float STEPPING = 0.0005f;

        /* Fields */
        protected Image slider;
        Image icon;
        protected IntervalF range;
        protected float valueT;
        protected string valueFormat;

        TextG text;
        string label;
        bool vertical;
        Vector2 slideLength;
        Vector2 sliderStartPos;

        bool newValueOnLeave = false;
        public Action onLeaveCallback = null;

        /* Constructors */
        public AbsGuiSlider(SpriteName iconTile, string label, string toolTip, IntervalF range, bool vertical, GuiLayout layout)
            : base(layout, toolTip)
        {
            Vector2 textStartPos = new Vector2(style.textEdgeSpace);
            slideLength = size;
            sliderStartPos = new Vector2(style.memberSpacing);

            if (iconTile != SpriteName.NO_IMAGE)
            {
                icon = new Image(iconTile,
                    new Vector2(style.textEdgeSpace),
                    new Vector2(style.memberHeight - style.textEdgeSpace * 2),
                    layoutParent.UnderTextLayer);

                //slideLength.X -= icon.Width;
                sliderStartPos.X += icon.Width + style.memberSpacing;
                textStartPos.X += icon.Width * 2f;
                this.AddAndUpdate(icon);
            }

            this.vertical = vertical;
            background.Color = style.Dark_LabelColor;

            if (!vertical)
            {
                text = new TextG(style.textFormat.Font, textStartPos,
                    style.textFormat.Scale, Align.Zero, label, style.textFormat.Color, layout.TextLayer);
                UpdateTextWidth();
                this.AddAndUpdate(text);
            }

            this.label = label;
            this.range = range;

            Vector2 sliderSize;
            float sliderWidth = style.memberHeight / 2;
            if (vertical)
            {
                sliderSize = new Vector2(background.Width - 2 * style.memberSpacing, sliderWidth);
            }
            else
            {
                sliderSize = new Vector2(sliderWidth, background.Height - 2 * style.memberSpacing);
            }

            slideLength -= sliderSize + sliderStartPos;

            slider = new Image(style.background, sliderStartPos, sliderSize, layout.UnderTextLayer, false, true);
            slider.Color = layout.gui.style.SliderColor;
            this.AddAndUpdate(slider);
        }

        /* Family Methods */
        public override void OnMouseDrag()
        {
            base.OnMouseDrag();
            Vector2 localPos = FindLocalCursorPos();
            if (vertical)
            {
                valueT = MathHelper.Clamp(((localPos.Y - slider.Height / 2) / (size.Y - slider.Height)), 0, 1);
            }
            else
            {
                localPos.X -= sliderStartPos.X;

                if (Input.Keyboard.Ctrl)
                {
                    valueT = MathHelper.Clamp(valueT + (Input.Mouse.MoveDistance.X * 0.1f / slideLength.X), 0, 1);
                }
                else
                {
                    valueT = MathHelper.Clamp(((localPos.X - slider.Width / 2) / (slideLength.X)), 0, 1);
                }
            }
            OnSliderMoved();
        }

        public override void MoveInputWhenHover(Vector2 move)
        {
            base.MoveInputWhenHover(move);

            if (Math.Abs(move.X) > 0.8f && Math.Abs(move.Y) < 0.2f)
            {
                MoveInputWhenSelected(move);
            }
        }

        public override void MoveInputWhenSelected(Vector2 move)
        {
            float vt = valueT;
            if (vertical)
            {
                valueT = MathHelper.Clamp(valueT + ((float)move.Y) * STEPPING * Ref.DeltaTimeMs, 0, 1);
            }
            else
            {
                valueT = MathHelper.Clamp(valueT + ((float)move.X) * STEPPING * Ref.DeltaTimeMs, 0, 1);
            }
            OnSliderMoved();
        }

        protected override void OnEnter()
        {
            new ChangeColor(slider, style.ShineColor, style.fadeTimeMS, true);
        }

        protected override void OnLeave()
        {
            new ChangeColor(slider, style.SliderColor, style.fadeTimeMS, true);
            if (newValueOnLeave)
            {
                onLeaveCallback?.Invoke();
            }
            newValueOnLeave = false;
        }

        protected override void OnPress()
        {
            Color color = ColorExt.Multiply(style.ShineColor, style.Tint);
            slider.Color = color;

            style.openSound?.Play();//.PlayFlat(layoutParent.gui.soundVolume);//Engine.Sound.PlaySound(style.openSound, 1.0f);
        }

        protected override void OnRelease()
        {
            new ChangeColor(slider, style.ShineColor, style.fadeTimeMS, true);

            style.closeSound?.Play();//?.Play();// Engine.Sound.PlaySound(style.closeSound, 1.0f);
        }

        public override void Refresh(GuiMember sender)
        {
            base.Refresh(sender);
            if (!ValuesMatch(range.GetFromPercent(valueT)))
            {
                valueT = range.GetValuePercentPosClamped(GetValue());
            }
            UpdateSliderPosition();
            UpdateText();
        }

        /* Novelty Methods */
        protected abstract void SetValue(float value);
        protected abstract float GetValue();
        protected abstract bool ValuesMatch(float value);

        protected virtual void OnSliderMoved()
        {
            UpdateSliderPosition();
            UpdateText();
            SetValue(range.GetFromPercent(valueT));
            layoutParent.gui.RefreshAllMembers(this);
            newValueOnLeave = true;
        }

        protected void UpdateSliderPosition()
        {
            if (vertical)
            {
                slider.Ypos = ParentY + MathExt.Lerp(style.memberSpacing, size.Y - slider.Height - style.memberSpacing, valueT);
            }
            else
            {
                slider.Xpos = ParentX + sliderStartPos.X + slideLength.X * valueT;//Mgth.Lerp(sliderStartPos.X, slideLength.X - slider.Width - style.memberSpacing, valueT);
            }
        }

        protected virtual void UpdateText()
        {
            if (!vertical)
            {
                string valueText = range.GetFromPercent(valueT).ToString(valueFormat);

                if (label == null)
                {
                    text.TextString = valueText;
                }
                else
                {
                    text.TextString = label + ": " + valueText;
                }

                //updateHeight(text.MeasureText().Y);
                UpdateTextWidth();
            }
        }

        void UpdateTextWidth()
        {
            text.SetMaxWidth(size.X - style.textEdgeSpace * 2);
        }
    }

    class GuiFloatSlider : AbsGuiSlider
    {
        FloatGetSet property;

        /* Constructors */
        public GuiFloatSlider(SpriteName iconTile, string label, FloatGetSet property, IntervalF range, bool vertical, GuiLayout layout)
            : base(iconTile, label, null, range, vertical, layout)
        {
            this.property = property;
            valueFormat = "0.##";
        }

        /* Family Methods */
        protected override void SetValue(float value)
        {
            if (property != null)
                property(true, value);
        }

        protected override float GetValue()
        {
            if (property != null)
                return property(false, 0);
            return 0;
        }

        protected override bool ValuesMatch(float value)
        {
            float actual = GetValue();
            return Math.Abs(actual - value) < 0.001f;
        }
    }

    class GuiIntSlider : AbsGuiSlider
    {
        IntGetSet property;

        /* Constructors */
        public GuiIntSlider(SpriteName iconTile, string label, IntGetSet property, IntervalF range, bool vertical, GuiLayout layout)
            : base(iconTile, label, null, range, vertical, layout)
        {
            this.property = property;
            valueFormat = "0";
        }

        /* Family Methods */
        protected override void SetValue(float value)
        {
            property(true, Convert.ToInt32(value));
        }

        protected override float GetValue()
        {
            return property(false, 0);
        }

        protected override bool ValuesMatch(float value)
        {
            int actual = Convert.ToInt32(GetValue());
            int compare = Convert.ToInt32(value);

            return actual == compare;
        }
    }
}
