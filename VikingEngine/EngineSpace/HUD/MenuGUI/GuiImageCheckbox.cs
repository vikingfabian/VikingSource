using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.HUD
{
    
    class GuiImageCheckbox : GuiMember
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
                icon.Color = checkboxImage.Color;
            }
        }

        /* Fields */
        public bool val;
        Image checkboxImage, icon;

        BoolGetSet property;

        /* Constructors */
        public GuiImageCheckbox(SpriteName iconTile, string toolTip, BoolGetSet property, GuiLayout layout)
            : base(layout, toolTip)
        {
            this.property = property;
            Vector2 sz = new Vector2(style.memberHeight * 0.5f);
            Vector2 pos = new Vector2(GetXOffset(), (style.memberHeight - sz.Y) * 0.5f);
            checkboxImage = new Image(SpriteName.NO_IMAGE, pos, sz, layout.TextLayer, false, true);

            icon = new Image(iconTile, new Vector2(style.memberSpacing), new Vector2(style.memberHeight - style.memberSpacing * 2f),
                layout.TextLayer);
            icon.Xpos += sz.X;
            this.AddAndUpdate(icon);

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
        /* Family Methods */
        protected bool GetValue()
        {
            return property(propertyIndex, false, false);
        }

        protected void SetValue(bool value)
        {
            property(propertyIndex,true, value);
        }

        protected virtual float GetXOffset()
        {
            return 0;
        }
    }
}
