using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.HUD.RichBox
{
    class RichboxButton : AbsRichBoxMember
    {
        public AbsRbAction click, enter;
        List<AbsRichBoxMember> content;
        Graphics.Image bgPointer;
        public bool enabled;

        public Input.IButtonMap buttonMap = null;
        public Color? overrideBgColor;

        public RichboxButton(List<AbsRichBoxMember> content, AbsRbAction click, AbsRbAction enter = null, bool enabled = true, Color? overrideBgColor = null)
        {
            this.content = content;
            this.click = click;
            this.enter = enter;
            if (this.click != null)
            {
                this.click.enabled = enabled;
            }           
            this.enabled = enabled;
            this.overrideBgColor = overrideBgColor;
        }

        public void addShortCutButton(Input.IButtonMap buttonMap, bool enableInput= true)
        {
            if (enableInput)
            {
                this.buttonMap = buttonMap;
            }
            content.Insert(0, new RichBoxImage(buttonMap.Icon, 1, 0, 1f));
        }

        public override void Create(RichBoxGroup group)
        {
            Vector2 topLeft = group.position;
            float heigh = group.lineSpacingHalf;

            group.parentMember.Push(this);

            group.position.X += 4;
            foreach (var m in content)
            {
                m.Create(group);
            }
            group.position.X += 4;

            group.parentMember.Pop();

            Vector2 bottomRight = group.position;
            if (bottomRight.Y != topLeft.Y)
            {
                bottomRight.X = group.RightEdgeSpace();
            }

            topLeft.Y -= heigh;
            bottomRight.Y += group.lineSpacingHalf;

            VectorRect area = VectorRect.FromTwoPoints(topLeft, bottomRight);
            area.AddXRadius(2);
            area.AddYRadius(-2);
            bgPointer = new Image(SpriteName.WhiteArea_LFtiles, area.Position, area.Size, group.layer + 1);
            
            if (overrideBgColor != null)
            {
                bgPointer.Color = overrideBgColor.Value;
            }
            else if (enabled)
            {
                bgPointer.Color = group.settings.button.BgColor;
            }
            else 
            {
                bgPointer.Color = group.settings.buttonDisabled.BgColor;
            }
            
            group.images.Add(bgPointer);
            group.buttonGrid_Y_X.Last().Add(this);
        }

        public VectorRect area()
        {
            return bgPointer.Area;
        }

    }
}
