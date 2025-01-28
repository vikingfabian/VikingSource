using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.HUD.RichBox
{

    abstract class AbsRbButton : AbsRichBoxMember
    {
        protected AbsRbAction click, enter;
        protected List<AbsRichBoxMember> content;
        public bool fillWidth = false;
        
        public bool enabled = true;

        virtual protected float ButtonEdgeToContentSpace(RichBoxGroup group, bool left)
        {
            const float HoriSpace = 6;
            return HoriSpace;
        }

        public override void Create(RichBoxGroup group)
        {
            if (content.Count > 1)
            {
                lib.DoNothing();
            }
            
            float heigh = group.lineSpacingHalf;

            group.parentMember.Push(this);

            group.TryCreate_Start();
            createContent(out Vector2 topLeft, out Vector2 bottomRight, out bool multiline);

            if (bottomRight.X + 4 > group.boxWidth)
            {
                group.TryCreate_Undo();
                group.newLine();
                createContent(out topLeft, out bottomRight, out multiline);
            }
            else
            {
                group.TryCreate_Complete();
            }

            group.parentMember.Pop();

            topLeft.Y -= heigh;
            bottomRight.Y += group.lineSpacingHalf;

            if (fillWidth)
            {
                bottomRight.X = group.boxWidth;
                group.newLine();
            }
            else
            {
                group.position.X += 8;
            }

            VectorRect area = VectorRect.FromTwoPoints(topLeft, bottomRight);

            if (multiline) area.Width = group.boxWidth;

            area.AddXRadius(2);
            area.AddYRadius(-2);
            //bgPointer = new Image(SpriteName.WhiteArea_LFtiles, area.Position, area.Size, group.layer + 1);
            createBackground(group, area, group.layer + 1);

            
            group.buttonGrid_Y_X.Last().Add(this);
           


            void createContent(out Vector2 topLeft, out Vector2 bottomRight, out bool multilineContent)
            {
                multilineContent = false;
                float prevY = group.position.Y;
                bool newLine = false;
                topLeft = group.position;

                group.position.X += ButtonEdgeToContentSpace(group, true);

                createPreContent(group);

                foreach (var m in content)
                {
                    m.Create(group);

                    if (newLine)
                    {
                       
                        multilineContent = true;
                    }
                    if (prevY < group.position.Y)
                    {
                        //multiline button
                        //area.Width = group.boxWidth;
                        group.position.X += ButtonEdgeToContentSpace(group, false);
                        newLine = true;
                    }
                }
                group.position.X += ButtonEdgeToContentSpace(group, false);

                bottomRight = group.position;
                if (bottomRight.Y != topLeft.Y)
                {
                    bottomRight.X = group.RightEdgeSpace();
                }
            }
        }

        abstract protected void createBackground(RichBoxGroup group, VectorRect area, ImageLayers layer);

        virtual protected void createPreContent(RichBoxGroup group)
        { }

        abstract public VectorRect area();

        public override void onClick(RichMenu.RichMenu menu)
        {
            click?.actionTrigger();
        }

        virtual public void clickAnimation(bool keyDown)
        { }

        public override void onEnter(RichMenu.RichMenu menu)
        {
            if (enter != null)
            {
                if (menu != null)
                {
                    var tooltip = enter.tooltip();
                    if (tooltip != null)
                    {
                        menu.addToolTip(tooltip, this.area());
                        return;
                    }
                }
                enter.actionTrigger();
            }
        }

        public override void getButtons(List<AbsRbButton> buttons)
        {
            buttons.Add(this);
        }

        abstract public void setGroupSelectionColor(RichBoxSettings settings, bool selected);

        virtual public bool UseButtonContentSettings()
        {
            return true;
        }
    }

    class RbButton : AbsRbButton
    {
        protected Graphics.Image bgPointer;
        public Color? overrideBgColor;
        public RbButton()
        { }

        public RbButton(List<AbsRichBoxMember> content, AbsRbAction click, AbsRbAction enter = null, bool enabled = true, Color? overrideBgColor = null)
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

        override protected void createBackground(RichBoxGroup group, VectorRect area, ImageLayers layer)
        {
            bgPointer = new Image(SpriteName.WhiteArea_LFtiles, area.Position, area.Size, layer);

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
        }

        override public void setGroupSelectionColor(RichBoxSettings settings, bool selected)
        {
            if (!selected)
            {
                overrideBgColor = settings.buttonSecondary.BgColor;
            }
        }
        override public VectorRect area()
        {
            return bgPointer.Area;
        }
    }
}
