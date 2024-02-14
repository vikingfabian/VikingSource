using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Input
{
    static class PS4InputLib
    {
        //public const GenericControllerButton Square = GenericControllerButton.B0;
        //public const GenericControllerButton Cross = GenericControllerButton.B1;
        //public const GenericControllerButton Cirkle = GenericControllerButton.B2;
        //public const GenericControllerButton Triangle = GenericControllerButton.B3;

        //public const GenericControllerButton L1 = GenericControllerButton.B4;
        //public const GenericControllerButton R1 = GenericControllerButton.B5;
        //public const GenericControllerButton L2 = GenericControllerButton.B6;
        //public const GenericControllerButton R2 = GenericControllerButton.B7;

        //public const GenericControllerButton Share = GenericControllerButton.B8; 
        //public const GenericControllerButton Options = GenericControllerButton.B9;

        //public const GenericControllerButton LsClick = GenericControllerButton.B10;
        //public const GenericControllerButton RsClick = GenericControllerButton.B11;
        //public const GenericControllerButton GuideButton = GenericControllerButton.B12;
        //public const GenericControllerButton TouchPadClick = GenericControllerButton.B13;


        //public const GenericControllerAxis LeftStickX = GenericControllerAxis.X;
        //public const GenericControllerAxis LeftStickY = GenericControllerAxis.Y;
        //public const GenericControllerAxis RightStickX = GenericControllerAxis.Z;
        //public const GenericControllerAxis RightStickY = GenericControllerAxis.RotationZ;

        //public const GenericControllerAxis Dpad = GenericControllerAxis.PointOfViewControllers0;


        //public static SpriteName GetSpriteName(GenericControllerButton button)
        //{
        //    switch (button)
        //    {
        //        case Square:
        //            return SpriteName.PsButtonSquare;
        //        case Cross:
        //            return SpriteName.PsButtonCross;
        //        case Cirkle:
        //            return SpriteName.PsButtonCirkle;
        //        case Triangle:
        //            return SpriteName.PsButtonTriangle;
        //        case L1:
        //            return SpriteName.ButtonLB;
        //        case R1:
        //            return SpriteName.ButtonRB;
        //        case L2:
        //            return SpriteName.ButtonLT;
        //        case R2:
        //            return SpriteName.ButtonRT;
        //        case Share:
        //            return SpriteName.PsButtonShare;
        //        case Options:
        //            return SpriteName.PsButtonOptions;

        //        case LsClick:
        //            return SpriteName.LSClick;
        //        case RsClick:
        //            return SpriteName.RSClick;
        //        case GuideButton:
        //            return SpriteName.PsGuideButton;
        //        case TouchPadClick:
        //            return SpriteName.PsTouchPad;

        //    }

        //    return SpriteName.MissingImage;
        //}

        public static SpriteName ButtonSprite(Buttons button)
        {
            switch (button)
            {
                default: return SpriteName.NO_IMAGE;
                case Buttons.A: return SpriteName.PsButtonCross;
                case Buttons.B: return SpriteName.PsButtonCirkle;
                case Buttons.X: return SpriteName.PsButtonSquare;
                case Buttons.Y: return SpriteName.PsButtonTriangle;
                case Buttons.Start: return SpriteName.PsButtonOptions;
                case Buttons.Back: return SpriteName.PsButtonShare;

                case Buttons.LeftShoulder: return SpriteName.ButtonLB;
                case Buttons.RightShoulder: return SpriteName.ButtonRB;
                case Buttons.LeftTrigger: return SpriteName.ButtonLT;
                case Buttons.RightTrigger: return SpriteName.ButtonRT;
                case Buttons.LeftStick: return SpriteName.LSClick;
                case Buttons.RightStick: return SpriteName.RSClick;

                case Buttons.DPadDown: return SpriteName.DpadDown;
                case Buttons.DPadLeft: return SpriteName.DpadLeft;
                case Buttons.DPadRight: return SpriteName.DpadRight;
                case Buttons.DPadUp: return SpriteName.DpadUp;

            }
        }

        //public static Buttons ConvertToXInput(GenericControllerButton genericButton)
        //{
        //    switch (genericButton)
        //    {
        //        case Cross: return Buttons.A;
        //        case Cirkle: return Buttons.B;
        //        case Square: return Buttons.X;
        //        case Triangle: return Buttons.Y;
        //        case L1: return Buttons.LeftShoulder;
        //        case L2: return Buttons.LeftTrigger;
        //        case R1: return Buttons.RightShoulder;
        //        case R2: return Buttons.RightTrigger;
        //        case Share: return Buttons.Back;
        //        case Options: return Buttons.Start;

        //        default: return 0;
        //    }
        //}
    }
}
