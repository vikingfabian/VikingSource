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
        protected AbsRbAction click, enter;
        protected List<AbsRichBoxMember> content;
        protected Graphics.Image bgPointer;
        public bool enabled;

        public Input.IButtonMap buttonMap = null;
        public Color? overrideBgColor;

        public RichboxButton()
        { }

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
            if (content.Count > 1)
            {
                lib.DoNothing();
            }


            const float HoriSpace = 4;

            
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

            VectorRect area = VectorRect.FromTwoPoints(topLeft, bottomRight);

            if (multiline) area.Width = group.boxWidth;

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

            void createContent(out Vector2 topLeft, out Vector2 bottomRight, out bool multilineContent)
            {
                multilineContent = false;
                float prevY = group.position.Y;
                bool newLine = false;
                topLeft = group.position;

                group.position.X += HoriSpace;

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
                        group.position.X += HoriSpace;
                        newLine = true;
                    }
                }
                group.position.X += HoriSpace;

                bottomRight = group.position;
                if (bottomRight.Y != topLeft.Y)
                {
                    bottomRight.X = group.RightEdgeSpace();
                }
            }
        }

        virtual protected void createPreContent(RichBoxGroup group)
        { }

        public VectorRect area()
        {
            return bgPointer.Area;
        }

        public override void onClick()
        {
            click?.actionTrigger();
        }

        public override void onEnter()
        {
            enter?.actionTrigger();
        }

        public override void getButtons(List<RichboxButton> buttons)
        {
            buttons.Add(this);
        }

        public void setGroupSelectionColor(RichBoxSettings settings, bool selected)
        {
            if (!selected)
            {
                overrideBgColor = settings.buttonSecondary.BgColor;
            }
        }
    }
}
