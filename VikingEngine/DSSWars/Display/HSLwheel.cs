using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Display
{
    class HSLwheel
    {
        const float WheelWidth = 120;
        const float Spacing = WheelWidth + 20;
        const float HueCirkleRadius = WheelWidth * PublicConstants.Half;

        const int HUEcirlkeMembers = 18;

        Image selectedColor;
        //List<AbsDraw> controls;
        List<AbsDraw> controlsPalette;
        //Rotation1D hueCirkleRotation = Rotation1D.D0;
        Vector2 hueCirkleCenter;
        Image[] hue;
        const int ColorBlockRadius = 2;
        Image[] saturation;
        Image[] lumination;
        Vector3 currentHSL = VectorExt.V3(0.5f);
        Image HueArrow;
        Image LS, RS;
        Vector2 LSCenter, RSCenter;
        static readonly Vector2 ButtonSize = VectorExt.V2(48);

        public Vector3 HSL
        {
            get { return currentHSL; }
        }
        public Color RGB
        {
            get { return currentCol; }
        }

        public HSLwheel(Color color)
            :this(lib.RGB2HSL(color))
        {  }

        public HSLwheel(Vector3 currentHSL)
        {
            
            //if (currentHSL.X >= 1)
            //{
            //    currentHSL.X -= 1;
            //}
            //if (currentHSL.Y >= 1)
            //{
            //    currentHSL.Y -= 1;
            //}
            this.currentHSL = currentHSL;

            controlsPalette = new List<AbsDraw>();
            
            selectedColor = new Image(SpriteName.WhiteArea, Engine.Screen.CenterScreen - new Vector2(-Spacing, 0), VectorExt.V2(WheelWidth), ImageLayers.Background2, true);
            controlsPalette.Add(selectedColor);
            //select color controls
            Vector2 colorBlockSize = VectorExt.V2(WheelWidth / 5);
            
            Vector2 hueBoxSize = VectorExt.V2(16);
            hueCirkleCenter = new Vector2(selectedColor.Xpos + Spacing, selectedColor.Ypos);
            hue = new Image[HUEcirlkeMembers];
            for (int i = 0; i < HUEcirlkeMembers; i++)
            {
                hue[i] = new Image(SpriteName.WhiteArea, Vector2.Zero, hueBoxSize, ImageLayers.Foreground9, true);
                hue[i].Color = lib.HSL2RGB((float)i / HUEcirlkeMembers, 1, 0.5f);
                controlsPalette.Add(hue[i]);
            }
            LS = new Image(SpriteName.LeftStick_LR, hueCirkleCenter, ButtonSize, ImageLayers.Lay1, true);
            LSCenter = LS.Position;
            controlsPalette.Add(LS);
            HueArrow = new Image(SpriteName.LfMenuMoreMenusArrow, hueCirkleCenter + Vector2.UnitY * -WheelWidth * 0.65f,
                hueBoxSize * 1.4f, ImageLayers.Foreground8, true);
            controlsPalette.Add(HueArrow);
            updateHueCirkle();


            Vector2 saturationStart = new Vector2(selectedColor.Xpos + Spacing, selectedColor.Ypos);
            Vector2 luminationStart = saturationStart;
            luminationStart.X += Spacing;
            Vector2 SLBarCenter = hueCirkleCenter;
            SLBarCenter.X += Spacing;
            saturation = new Image[ColorBlockRadius * PublicConstants.Twice];
            lumination = new Image[ColorBlockRadius * PublicConstants.Twice];
            List<float> barPositions = new List<float> { -colorBlockSize.X * 2, -colorBlockSize.X, colorBlockSize.X, colorBlockSize.X * 2 };

            for (int i = 0; i < ColorBlockRadius * PublicConstants.Twice; i++)
            {
                Vector2 Spos = SLBarCenter; Spos.Y += barPositions[i];
                saturation[i] = new Image(SpriteName.WhiteArea, 
                    Spos,
                    //new Vector2(
                    //saturationStart.X + colorBlockSize.X * ColorBlockRadius,
                    //saturationStart.Y + (i < ColorBlockRadius ? i : i + 1)),
                    colorBlockSize, ImageLayers.Lay3, true);
                Vector2 Lpos = SLBarCenter; Lpos.X += barPositions[i];
                lumination[i] = new Image(SpriteName.WhiteArea, Lpos,
                    //new Vector2(
                    //luminationStart.X + (i < ColorBlockRadius ? i : i + 1) * colorBlockSize.X,
                    //luminationStart.Y),
                    colorBlockSize, ImageLayers.Lay3, true);
                controlsPalette.Add(saturation[i]);
                controlsPalette.Add(lumination[i]);
            }

            RS = new Image(SpriteName.RightStick, SLBarCenter, ButtonSize, ImageLayers.Lay1, true);
            RSCenter = RS.Position;
            controlsPalette.Add(RS);

            //showControls(true);
            updateColor();
        }

        void updateHueCirkle()
        {
            float hueCirkleSteps = MathHelper.TwoPi / HUEcirlkeMembers;

            Rotation1D rot = new Rotation1D((1 - currentHSL.X) * MathHelper.TwoPi);
            for (int i = 0; i < HUEcirlkeMembers; i++)
            {
                hue[i].Position = hueCirkleCenter + rot.Direction(HueCirkleRadius);
                rot.Radians += hueCirkleSteps;
            }
        }

        void showControls(bool show)
        {
            //foreach (AbsDraw img in controls)
            //{
            //    img.Visible = show;
            //}
            foreach (AbsDraw img in controlsPalette)
            {
                img.Visible = show;
            }
        }

        public Color DeleteMe()
        {
            foreach (AbsDraw img in controlsPalette)
            {
                img.DeleteMe();
            }
            return currentCol;
        }

        Color currentCol = Color.Red;
        void updateColor()
        {
            currentCol = lib.HSL2RGB(currentHSL.X, currentHSL.Y, currentHSL.Z);

            const float colStep = 0.15f;
            float colPreview = -colStep * ColorBlockRadius;
            for (int i = 0; i < ColorBlockRadius * PublicConstants.Twice; i++)
            {
                saturation[i].Color = lib.HSL2RGB(currentHSL.X,
                    Bound.Set(currentHSL.Y + colPreview, 0, 1),
                    currentHSL.Z);
                lumination[i].Color = lib.HSL2RGB(currentHSL.X, currentHSL.Y,
                    Bound.Set(currentHSL.Z + colPreview, 0, 1));
                colPreview += colStep;
                if (i == ColorBlockRadius - 1)
                {
                    colPreview += colStep;
                }
            }
            selectedColor.Color = currentCol;
            HueArrow.Color =  lib.HSL2RGB(currentHSL.X, 1, 0.5f);
        }

        public Color UpdateInput(InputMap input)
        {
            //JoyStickValue left = controller.JoyStickValue(Stick.Left);
            //JoyStickValue right = controller.JoyStickValue(Stick.Right);

            Vector2 left = input.move.direction;
            Vector2 right = input.cameraTiltZoom.direction + Input.Mouse.MoveDistance;

            const float MoveLength = 10;
            LS.Xpos = LSCenter.X +  input.move.direction.X * MoveLength;
            RS.Position = RSCenter + right * MoveLength;

            if (left != Vector2.Zero)
                Pad_Event(left, true);
            if (right != Vector2.Zero)
                Pad_Event(right, false);

            //if (controller.KeyDownEvent(Microsoft.Xna.Framework.Input.Buttons.RightStick))
            //{
            //    currentHSL.Y = 1;
            //    currentHSL.Z = 0.5f;
            //    updateColor();
            //    RS.Size = ButtonSize;
            //    new Graphics.Motion2d(MotionType.SCALE, RS, ButtonSize * 0.3f, MotionRepeate.BackNForwardOnce, 66, true);
            //}

            return currentCol;
        }

        public void Pad_Event(Vector2 dir, bool left)
        {
            Vector2 dirTime = dir * Ref.DeltaTimeMs;

            if (left)
            {
                currentHSL.X += -dirTime.X * 0.0004f;
                if (currentHSL.X < 0)
                {
                    currentHSL.X += 1;
                }
                else if (currentHSL.X >= 1)
                {
                    currentHSL.X -= 1;
                }
                updateHueCirkle();
                updateColor();
            }
            else
            {
                const float SLspeed = 0.0008f;
                currentHSL.Y = Bound.Set(currentHSL.Y + dirTime.Y * SLspeed, 0, 1);
                currentHSL.Z = Bound.Set(currentHSL.Z + dirTime.X * SLspeed, 0, 1);
                updateColor();

            }
        }
    }
}
