using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.HUD.RichBox
{
    struct DragButtonSettings
    {
        public float min, max;
        public float step;

        public DragButtonSettings(float min, float max, float step)
        { 
            this.min = min;
            this.max = max;
            this.step = step;
        }
    }

    class RbDragButton : AbsRbButton
    {
        /// <summary>
        /// A drag button sourronded by + - buttons
        /// </summary>
        /// <param name="options">Positive values, low to high</param>
        public static void RbDragButtonGroup(RichBoxContent content, List<int> options, DragButtonSettings settings, IntGetSet intValue)
        {
            var dragButton = new RbDragButton(settings, intValue);

            for (int i = options.Count - 1; i >= 0; --i)
            {
                content.Add(new RbDragOptionButton(dragButton, -options[i]));
            }

            content.Add(dragButton);

            for (int i = 0; i < options.Count; ++i)
            {
                content.Add(new RbDragOptionButton(dragButton, options[i]));
            }
        }

        DragButtonSettings settings;
        bool valueTypeInt;
        IntGetSet intValue;
        FloatGetSet floatValue;

        RbText textPointer;
        ThreeSplitTexture_Hori texture;
        
        public RbDragButton(DragButtonSettings settings, IntGetSet intValue, AbsRbAction enter = null)
        {
            this.enter = enter;
            this.settings = settings;
            this.intValue = intValue;
            valueTypeInt = true;

            textPointer = new RbText(TextLib.LargeNumber((int)settings.max));
            this.content = new List<AbsRichBoxMember> { textPointer };
            enabled = true;
        }

        public RbDragButton(DragButtonSettings settings, FloatGetSet floatValue, AbsRbAction enter = null)
        {
            this.enter = enter;
            this.settings = settings;
            this.floatValue = floatValue;
            valueTypeInt = false;

            textPointer = new RbText(TextLib.OneDecimal(settings.max));
            this.content = new List<AbsRichBoxMember> { textPointer };
            enabled = true;
        }

        public override VectorRect area()
        {
            return texture.Area();
        }

        public override void setGroupSelectionColor(RichBoxSettings settings, bool selected)
        {
            throw new NotImplementedException();
        }

        protected override void createPreContent(RichBoxGroup group)
        {
            base.createPreContent(group);
        }

        public override void Create(RichBoxGroup group)
        {
            base.Create(group);
            valueChangeInput(0);
        }

        protected override void createBackground(RichBoxGroup group, VectorRect area, ImageLayers layer)
        {
            texture = new HUD.ThreeSplitTexture_Hori(group.settings.dragButtonTex, area, layer + 2);

            group.images.AddRange(texture.images);
        }

        public override void onClick(RichMenu.RichMenu menu)
        {
            new DragButtonInteraction(menu, this);
        }

        public void valueChangeInput(float change)
        {
            if (valueTypeInt)
            {
                int value = intValue.Invoke(false, 0);
                if (change != 0)
                {
                    value = Convert.ToInt32(Bound.Set(value + change * settings.step, settings.min, settings.max));
                    intValue.Invoke(true, value);
                }
                textPointer.pointer.TextString = TextLib.LargeNumber(value);
            }
            else
            {
                float value = floatValue.Invoke(false, 0);
                if (change != 0)
                {
                    value = Bound.Set(value + change * settings.step, settings.min, settings.max);
                    floatValue.Invoke(true, value);
                }
                textPointer.pointer.TextString = TextLib.OneDecimal(value);
            }
        }

        protected override float ButtonEdgeToContentSpace(RichBoxGroup group, bool left)
        {
            return group.settings.dragButtonTex.TotalSideLeght() + 12;
        }

        public override bool UseButtonContentSettings()
        {
            return false;
        }
    }

    class RbDragOptionButton : Artistic.ArtButton
    {
        RbDragButton parent;
        int add;

        public RbDragOptionButton(RbDragButton parent, int add)
        {
            this.parent = parent;
            this.buttonStyle = Artistic.RbButtonStyle.Primary;
            this.add = add;

            content = new List<AbsRichBoxMember> { new RbText(TextLib.PlusMinus(add)) };            
        }

        public override void onClick(RichMenu.RichMenu menu)
        {
            parent.valueChangeInput(add);
        }
    }

    class DragButtonInteraction : AbsRbInteraction
    {
        RbDragButton dragButton;
        Vector2 prevMousePos;
        IntervalF mouseXRange;
        float moveLengthForValueChange;
        public DragButtonInteraction(RichMenu.RichMenu menu, RbDragButton dragButton) 
        {
            prevMousePos = Input.Mouse.Position;
            this.dragButton = dragButton;
            menu.interaction.interactionStack = this;

            mouseXRange = new IntervalF(menu.edgeArea.X, menu.edgeArea.Right);
            moveLengthForValueChange = Screen.MinClickSize * 0.8f; 

        }
        public override bool update(Vector2 mousePosOffSet, RichMenu.RichMenu menu, out bool endInteraction)
        {
            float move = Input.Mouse.Position.X - prevMousePos.X;
            if (Math.Abs(move) > moveLengthForValueChange)
            {
                float change = (int)(move / moveLengthForValueChange);
                prevMousePos.X += change * moveLengthForValueChange;

                if (Input.Mouse.Position.X < mouseXRange.Min)
                {
                    Input.Mouse.SetPosition(new IntVector2(mouseXRange.Max + (mouseXRange.Min - Input.Mouse.Position.X), Input.Mouse.Position.Y));
                    prevMousePos.X = mouseXRange.Max;
                }
                else if (Input.Mouse.Position.X > mouseXRange.Max)
                {
                    Input.Mouse.SetPosition(new IntVector2( mouseXRange.Min + (Input.Mouse.Position.X- mouseXRange.Max), Input.Mouse.Position.Y));
                    prevMousePos.X = mouseXRange.Min;
                }
                dragButton.valueChangeInput(change);
            }

            endInteraction = Input.Mouse.ButtonUpEvent(MouseButton.Left);
            return true;
        }
    }
}
