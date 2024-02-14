using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ
{
    class JoinSuggestIcon
    {
        public const float FrameOpacity = 0.4f;
        public VectorRect area;

        public Graphics.Image border, highlight, keyboard, controller, hand, mouse, mousePointer;
        Timer.Basic stateTimer = new Timer.Basic(TimeExt.SecondsToMS(4f), true);
        int state_0keyboard_1controller_2mouse = 0;

        public JoinSuggestIcon(int index)
        {
            area = LobbyAvatar.GamerIconPlacement(index);

            border = new Graphics.Image(SpriteName.birdPlayerFrame, area.Position, area.Size, ImageLayers.Background4);
            border.Opacity = FrameOpacity;

            highlight = new Graphics.Image(SpriteName.WhiteArea, border.Center, border.Size, ImageLayers.Background6, true);
            highlight.Visible = false;

            keyboard = new Graphics.Image(SpriteName.Keyboard, area.Center, area.Size * 0.2f * new Vector2(4, 3), ImageLayers.Lay5, true);
            keyboard.Ypos += area.Height * 0.1f;

            controller = new Graphics.Image(SpriteName.birdControllerIcon, area.Center, area.Size * 0.35f * new Vector2(3, 2), 
                ImageLayers.Lay6, true);
            controller.Ypos += area.Height * 0.12f;
            controller.Opacity = 0f;

            hand = new Graphics.Image( SpriteName.birdUserHand, area.Center, area.Size * 0.65f, ImageLayers.Lay4, true);
            hand.Xpos += area.Width * 0.2f;
            hand.Ypos -= hand.Height * 0.4f;

            new Graphics.Motion2d(Graphics.MotionType.MOVE, hand, Vector2.UnitY * hand.Height * 0.35f, 
                Graphics.MotionRepeate.BackNForwardLoop,
                400, true);

            mouse = new Graphics.Image(SpriteName.Mouse, area.Center, area.Size * 0.6f, ImageLayers.Lay6, true);
            mouse.Ypos -= area.Height * 0.05f;
            mouse.Opacity = 0f;

            mousePointer = new Graphics.Image(SpriteName.MousePointer, area.LeftBottom, area.Size * 0.4f, ImageLayers.Lay4);
            mousePointer.Opacity = 0f;

            if (PjRef.XboxLayout)
            {
                controller.Opacity = 1;
                hand.Opacity = 1;

                keyboard.Visible = false;
                mouse.Visible = false;
            }
        }

        const float FadeTime = 300;
        public void update(bool mouseHasJoined)
        {
            if (PjRef.XboxLayout)
            {
                return;
            }

            if (stateTimer.Update())
            {
                state_0keyboard_1controller_2mouse++;
                int max = mouseHasJoined ? 1 : 2;
                if (state_0keyboard_1controller_2mouse > max)
                {
                    state_0keyboard_1controller_2mouse = 0;
                }

                switch (state_0keyboard_1controller_2mouse)
                {
                    case 1:
                        new Graphics.Motion2d(Graphics.MotionType.OPACITY, keyboard, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT,
                            FadeTime, true);
                        new Graphics.Motion2d(Graphics.MotionType.OPACITY, controller, Vector2.One, Graphics.MotionRepeate.NO_REPEAT,
                            FadeTime, true);
                        break;
                    case 2://Mouse
                        new Graphics.Motion2d(Graphics.MotionType.OPACITY, hand, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT,
                            FadeTime, true);
                        new Graphics.Motion2d(Graphics.MotionType.OPACITY, controller, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT,
                            FadeTime, true);

                        new Graphics.Motion2d(Graphics.MotionType.OPACITY, mouse, Vector2.One, Graphics.MotionRepeate.NO_REPEAT,
                            FadeTime, true);
                        new Graphics.Motion2d(Graphics.MotionType.OPACITY, mousePointer, Vector2.One, Graphics.MotionRepeate.NO_REPEAT,
                            FadeTime, true);

                        mousePointer.Position = area.LeftBottom;
                        const float MoveTime = 1000f;
                        var movePointer = new Graphics.Motion2d(Graphics.MotionType.MOVE, mousePointer, new Vector2(area.Width * 0.5f, -area.Height * 0.4f),
                             Graphics.MotionRepeate.NO_REPEAT, MoveTime, false);
                        new Timer.UpdateTrigger(FadeTime, movePointer, true);
                        new Timer.TimedAction0ArgTrigger(mouseClickEffect, FadeTime + MoveTime);

                        break;

                    case 0://Keyboard
                        new Graphics.Motion2d(Graphics.MotionType.OPACITY, mouse, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT,
                            FadeTime, true);
                        new Graphics.Motion2d(Graphics.MotionType.OPACITY, mousePointer, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT,
                            FadeTime, true);

                        if (controller.Opacity > 0f)
                        {
                            new Graphics.Motion2d(Graphics.MotionType.OPACITY, controller, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT,
                                FadeTime, true);
                        }

                        new Graphics.Motion2d(Graphics.MotionType.OPACITY, hand, Vector2.One, Graphics.MotionRepeate.NO_REPEAT,
                            FadeTime, true);
                        new Graphics.Motion2d(Graphics.MotionType.OPACITY, keyboard, Vector2.One, Graphics.MotionRepeate.NO_REPEAT,
                            FadeTime, true);
                        break;
                }
            }
        }

        void mouseClickEffect()
        {
            const float ScaleUpTime = 600;

            Graphics.Image cirkle = new Graphics.Image(SpriteName.ClickCirkleEffect, mousePointer.Position, Vector2.One, ImageLayers.AbsoluteBottomLayer, true);
            cirkle.LayerBelow(mousePointer);

            new Graphics.Motion2d(Graphics.MotionType.SCALE, cirkle, area.Size * 0.8f, Graphics.MotionRepeate.NO_REPEAT, ScaleUpTime, true);
            new Graphics.Motion2d(Graphics.MotionType.OPACITY, cirkle, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT, ScaleUpTime, true);
            new Timer.Terminator(ScaleUpTime, cirkle);

        }

        public void DeleteMe()
        {
            highlight.DeleteMe();
            border.DeleteMe(); keyboard.DeleteMe(); hand.DeleteMe();
            controller.DeleteMe(); mouse.DeleteMe();
            mousePointer.DeleteMe();
        }
    }
}
