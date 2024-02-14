using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;
using VikingEngine.Input;

namespace VikingEngine.HUD
{
    /// <summary>
    /// Overlays an image of what the user can press to interact in the gui
    /// </summary>
    //class GuiButtonHelpOverlay : GuiOverlay
    //{
    //    BtnMapImage image;

    //    public GuiButtonHelpOverlay(PlayerInputMap inputMap, GuiStyle style, float waitUntilDisplayMS)
    //        : base(inputMap, style, waitUntilDisplayMS)
    //    {
    //        image = new BtnMapImage(inputMap, ButtonActionType.MenuClick, true);
    //        image.Size = new Vector2(style.memberHeight);

    //        drawables.Add(image);
    //        InitDrawableVisibility();
    //    }

    //    protected override void OnUpdate(GuiMember selected)
    //    {
    //        // and the position.
    //        image.Position = selected.GetVisibleRect().TopRight + new Vector2(style.memberSpacing, 0);
    //    }
    //}

    class GuiTooltipOverlay : GuiOverlay
    {
        Image background;
        TextBoxSimple textBox;

        public GuiTooltipOverlay(MenuInputMap inputMap, GuiStyle style, float waitUntilDisplayMS)
             : base(inputMap, style, waitUntilDisplayMS)
        {
            textBox = new TextBoxSimple(
                style.textFormat.Font,
                new Vector2(style.textEdgeSpace),
                style.textFormat.Scale,
                Align.Zero,
                "",
                style.textFormat.Color,
                ImageLayers.AbsoluteTopLayer,
                style.layoutWidth - style.textEdgeSpace * 2f);

            background = new Image(true);
            background.SetSpriteName(style.background);
            background.Color = style.Dark_LabelColor;
            background.Size = new Vector2(style.layoutWidth, textBox.MeasureText().Y);

            // Draw the background first - so add it first. (does that work?)
            drawables.Add(textBox);
            drawables.Add(background);

            InitDrawableVisibility();
        }

        protected override void OnUpdate(GuiMember selected)
        {
            // If there is text
            if (selected.ToolTip != null && selected.ToolTip != "")
            {
                // Set the text to whatever the tooltip of the member is.
                textBox.TextString = selected.ToolTip;
                // Update the background's size depending on the new text.
                background.Size = textBox.MeasureText() + new Vector2(style.textEdgeSpace * 2f);

                // Then set the positions.
                background.Position = selected.GetVisibleRect().Position + new Vector2(selected.size.X + style.memberSpacing + 48, 0);

                if (background.Bottom > Engine.Screen.SafeArea.Bottom)
                {
                    background.Ypos = Engine.Screen.SafeArea.Bottom - background.Height;
                }
                // Remember the text edge space for the text.
                textBox.Position = background.Position + new Vector2(style.textEdgeSpace);
            }
            else
            {
                // If there is no text, set to no text.
                textBox.TextString = "";

                // And set the size of the background to zero.
                background.Size = Vector2.Zero;
            }
        }
    }

    abstract class GuiOverlay
    {
        protected float waitUntilDisplayMS = 1000;

        protected float timeWaitedMS;
        protected List<AbsDraw2D> drawables;
        protected GuiStyle style;
        protected MenuInputMap inputMap;

        public GuiOverlay(MenuInputMap inputMap, GuiStyle style, float waitUntilDisplayMS)
        {
            this.inputMap = inputMap;
            this.style = style;
            this.waitUntilDisplayMS = waitUntilDisplayMS;
            drawables = new List<AbsDraw2D>();
        }

        protected void InitDrawableVisibility()
        {
            float layer = 0.0f;
            foreach (AbsDraw2D drawable in drawables)
            {
                // Make the drawable invisible, and put it on top.
                drawable.Opacity = 0f;
                drawable.PaintLayer = layer;
                layer += 0.0001f;
            }
        }

        /// <summary>
        /// Needs a position and a refresh of what to show
        /// </summary>
        abstract protected void OnUpdate(GuiMember selected);

        public void Update(bool selectionChanged, GuiMember selected)
        {
            if (!selectionChanged && !(inputMap.click.DownEvent || inputMap.back.DownEvent))//.InputReceivedThisFrame())
            {
                // If we're waiting, add to the wait timer.
                timeWaitedMS += Ref.DeltaTimeMs;

                // If we've waited long enough, and the member is in view,
                if (timeWaitedMS >= waitUntilDisplayMS &&
                    selected.GetVisibleRect().Size.Y >= selected.size.Y / 2)
                {
                    foreach (AbsDraw2D drawable in drawables)
                    {
                        // If a drawable is invisible,
                        if (drawable.Opacity == 0)
                        {
                            // then it's time to show it.
                            OnUpdate(selected);
                            new TargetFade(drawable, 1.0f, style.fadeTimeMS);
                        }
                    }
                }
            }
            else
            {
                // If we got new input, reset.
                timeWaitedMS = 0;

                foreach (AbsDraw2D drawable in drawables)
                {
                    // If a drawable is visible,
                    if (drawable.Opacity != 0)
                    {
                        // then it's time to hide it.
                        new TargetFade(drawable, 0.0f, style.fadeTimeMS);
                    }
                }
            }
        }

        public void DeleteMe()
        {
            foreach (AbsDraw2D drawable in drawables)
            {
                drawable.DeleteMe();
            }
        }
    }
}
