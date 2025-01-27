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

    /// <summary>
    /// A drag button sourronded by + - buttons
    /// </summary>
    class RbDragButtonGroup
    {

    }
    class RbDragButton : AbsRbButton
    {
        DragButtonSettings settings;
        bool valueTypeInt;
        IntGetSet intValue;
        FloatGetSet floatValue;

        RbText textPointer;
        ThreeSplitSettings textureSett;
        ThreeSplitTexture_Hori texture;

        public RbDragButton(ThreeSplitSettings texture, DragButtonSettings settings, IntGetSet intValue)
        { 
            this.textureSett = texture;
            this.settings = settings;
            this.intValue = intValue;
            valueTypeInt = true;

            textPointer = new RbText(TextLib.LargeNumber((int)settings.max));
            this.content = new List<AbsRichBoxMember> { textPointer };
        }

        public override VectorRect area()
        {
            return texture.area;
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
            texture = new HUD.ThreeSplitTexture_Hori(textureSett, area, layer);

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
        }

        protected override float ButtonEdgeToContentSpace(bool left)
        {
            return textureSett.TotalSideLeght();
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
            moveLengthForValueChange = Screen.MinClickSize * 0.5f; 

        }
        public override bool update(Vector2 mousePosOffSet, RichMenu.RichMenu menu, out bool endInteraction)
        {
            float move = Input.Mouse.Position.X - prevMousePos.X;
            if (Math.Abs(move) > moveLengthForValueChange)
            {
                int change = (int)(move / moveLengthForValueChange);
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
