using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.Network;

namespace VikingEngine.HUD.RichBox
{
    struct DragButtonSettings
    {
        public float min, max;
        public float step;

        public DragButtonSettings(Range minMax, float step)
        {
            this.min = minMax.Min;
            this.max = minMax.Max;
            this.step = step;
        }

        public DragButtonSettings(IntervalF minMax, float step)
        {
            this.min = minMax.Min;
            this.max = minMax.Max;
            this.step = step;
        }

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
        public static void RbDragButtonGroup(RichBoxContent content, List<float> options, DragButtonSettings settings, IntGetSetTag intValue, bool useSymbols, object tag = null)
        {
            var dragButton = new RbDragButton(settings, intValue, null, tag);

            for (int i = options.Count - 1; i >= 0; --i)
            {
                content.Add(new RbDragOptionButton(dragButton, -options[i], useSymbols));
            }

            content.Add(dragButton);

            for (int i = 0; i < options.Count; ++i)
            {
                content.Add(new RbDragOptionButton(dragButton, options[i], useSymbols));
            }
        }

        public static void RbDragButtonGroup(RichBoxContent content, List<float> options, DragButtonSettings settings, FloatGetSetTag floatValue, bool oneDecimal = true, object tag = null)
        {
            var dragButton = new RbDragButton(settings, floatValue, oneDecimal, null, tag);

            for (int i = options.Count - 1; i >= 0; --i)
            {
                content.Add(new RbDragOptionButton(dragButton, -options[i], false));
            }

            content.Add(dragButton);

            for (int i = 0; i < options.Count; ++i)
            {
                content.Add(new RbDragOptionButton(dragButton, options[i], false));
            }
        }

        DragButtonSettings settings;
        DragValueType valueType;
        protected object tag;
        protected IntGetSetTag intValue;
        FloatGetSetTag floatValue;

        protected RbText textPointer;
        ThreeSplitTexture_Hori texture;
        
        public RbDragButton(DragButtonSettings settings, IntGetSetTag intValue, AbsRbAction enter = null, object tag = null)
        {
            this.enter = enter;
            this.settings = settings;
            this.tag = tag;
            this.intValue = intValue;
            valueType = DragValueType.Int;

            textPointer = new RbText(TextLib.LargeNumber((int)settings.max));
            this.content = new List<AbsRichBoxMember> { textPointer };
            enabled = true;

            //refreshValueDisplay();
        }

        public RbDragButton(DragButtonSettings settings, FloatGetSetTag floatValue, bool oneDecimal, AbsRbAction enter = null, object tag = null)
        {
            this.enter = enter;
            this.settings = settings;
            this.tag = tag;
            this.floatValue = floatValue;
            valueType = oneDecimal? DragValueType.Float_1Dec : DragValueType.Float_2Dec;

            textPointer = new RbText(oneDecimal? TextLib.OneDecimal(settings.max) : TextLib.TwoDecimal(settings.max));
            this.content = new List<AbsRichBoxMember> { textPointer };
            enabled = true;

            //refreshValueDisplay();
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
            //valueChangeInput(0, false);
            refreshValueDisplay();
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

        virtual public void valueChangeInput(float change, bool dragStep)
        {
            if (change != 0)
            {

                if (dragStep)
                {
                    change *= settings.step;
                }

                bool hasChanged = false;

                if (valueType == DragValueType.Int)
                {
                    int value = intValue.Invoke(tag, false, 0);

                    int prev = value;
                    value = Convert.ToInt32(Bound.Set(value + change, settings.min, settings.max));
                    if (prev != value)
                    {
                        intValue.Invoke(tag, true, value);

                        textPointer.pointer.TextString = TextLib.LargeNumber(value);

                        hasChanged = true;
                    }
                }
                else
                {
                    float value = floatValue.Invoke(tag, false, 0);

                    float prev = value;
                    value = Bound.Set(value + change, settings.min, settings.max);
                    if (prev != value)
                    {
                        floatValue.Invoke(tag, true, value);

                        textPointer.pointer.TextString = valueType == DragValueType.Float_1Dec ? TextLib.OneDecimal(value) : TextLib.TwoDecimal(value);

                        hasChanged = true;
                    }
                }

                if (hasChanged)
                {
                    (change > 0 ? DSSWars.SoundLib.scroll_forward : DSSWars.SoundLib.scroll_back).Play();
                }
                else
                { 
                    DSSWars.SoundLib.soft_buzz_error.Play();
                }
            }
        }

        virtual public void refreshValueDisplay()
        {
            if (textPointer != null && textPointer.pointer != null)
            {
                if (valueType == DragValueType.Int)
                {
                    if (intValue != null)
                    {
                        int value = intValue.Invoke(tag, false, 0);

                        textPointer.pointer.TextString = TextLib.LargeNumber(value);
                    }
                }
                else
                {
                    if (floatValue != null)
                    {
                        float value = floatValue.Invoke(tag, false, 0);

                        textPointer.pointer.TextString = valueType == DragValueType.Float_1Dec ? TextLib.OneDecimal(value) : TextLib.TwoDecimal(value);
                    }
                }
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

    enum DragValueType
    { 
        Int,
        Float_1Dec,
        Float_2Dec,

    }

    class RbDragOptionButton : Artistic.ArtButton
    {
        RbDragButton parent;
        float add;

        public RbDragOptionButton(RbDragButton parent, float add, bool useSymbols)
        {
            this.parent = parent;
            this.buttonStyle = Artistic.RbButtonStyle.Primary;

            
            this.add = add;


            if (useSymbols)
            {
                content = new List<AbsRichBoxMember> { new RbText(LangLib.ValueSymbol((int)add)) };
                enter = new RbTooltip_Text(TextLib.PlusMinus(add));
            }
            else
            {
                content = new List<AbsRichBoxMember> { new RbText(TextLib.PlusMinus(add)) };
            }
        }

        public override void onClick(RichMenu.RichMenu menu)
        {
            parent.valueChangeInput(add, false);
        }
    }

    class DragButtonInteraction : AbsRbInteraction
    {
        RbDragButton dragButton;
        Vector2 prevMousePos;
        IntervalF mouseXRange;
        float moveLengthForValueChange;
        TimeStamp timeStamp;
        float buttonCenter;
        //float controllerMove = 0;
        DirXYstepping controllerStepping = new DirXYstepping();
        public DragButtonInteraction(RichMenu.RichMenu menu, RbDragButton dragButton) 
        {
            prevMousePos = Input.Mouse.Position;
            this.dragButton = dragButton;
            menu.interaction.interactionStack = this;
            timeStamp = TimeStamp.Now();
            
            moveLengthForValueChange = Screen.MinClickSize * 0.8f;
            mouseXRange = new IntervalF(menu.backgroundArea.X + Engine.Screen.IconSize, menu.backgroundArea.Right - Engine.Screen.IconSize);
            buttonCenter = dragButton.area().Center.X + menu.backgroundArea.X;
        }
        public override bool update(Vector2 mousePosOffSet, RichMenu.RichMenu menu, bool useClick, out bool needRefresh, out bool endInteraction)
        {
            
            float move = Input.Mouse.Position.X - prevMousePos.X;
            if (Math.Abs(move) > moveLengthForValueChange)
            {
                float change = (int)(move / moveLengthForValueChange);
                prevMousePos.X += change * moveLengthForValueChange;
                
                if (Input.Mouse.Position.X < mouseXRange.Min)
                {
                    Input.Mouse.SetPosition(new IntVector2(mouseXRange.Max, Input.Mouse.Position.Y));
                    prevMousePos.X = mouseXRange.Max;
                }
                else if (Input.Mouse.Position.X > mouseXRange.Max)
                {
                    Input.Mouse.SetPosition(new IntVector2(mouseXRange.Min, Input.Mouse.Position.Y));
                    prevMousePos.X = mouseXRange.Min;
                }
                needRefresh = true;
                dragButton.valueChangeInput(change, true);
            }
            else
            {
                needRefresh = false;
            }

            endInteraction = Input.Mouse.ButtonUpEvent(MouseButton.Left);

            return false;
        }

        public override bool updateController(RichMenuControllerPointer pointer, RichMenu.RichMenu menu, bool useClickInput, out bool needRefresh, out bool endInteraction, out float pushScroll)
        {
            pushScroll = 0;
            pointer.pointer.Visible = false;

            int steps = controllerStepping.update(pointer.inputMap.move.direction).X;
            //pointer.inputMap.move.
            //controllerMove += pointer.accelerateInput(pointer.inputMap.move.direction).X * 0.2f;

            //if (Math.Abs(controllerMove) > moveLengthForValueChange)
            //{
            //    float change = (int)(controllerMove / moveLengthForValueChange);
            //    controllerMove -= change * moveLengthForValueChange;

            if (steps == 0)
            {
                needRefresh = false;
            }
            else
            {
                needRefresh = true;
                dragButton.valueChangeInput(steps, true);
            }
            //}
            //else
            //{
            //    needRefresh = false;
            //}

            if (pointer.inputMap.MenuClick.IsDown == false)
            {
                endInteraction = true;
                pointer.pointer.Visible = true;
            }
            else
            {
                endInteraction = false;
            }

            return false;
        }

        public override void end(float pointerX, out bool needRefresh)
        {
            if (timeStamp.belowTime_ms(InputLib.ButtonMaxClickTimeMs))
            {
                dragButton.valueChangeInput(lib.BoolToLeftRight(pointerX/*prevMousePos.X*/ > buttonCenter), true);
                needRefresh = true;
            }

            needRefresh = true;
        }
    }
}
