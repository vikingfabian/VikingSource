using Microsoft.Xna.Framework.Input;
using VikingEngine.Input;

namespace VikingEngine.Input
{
    static class XboxInputLib
    {
        //public static Buttons ConvertToXInput(GenericControllerButton genericButton)
        //{
        //    switch (genericButton)
        //    {
        //        case GenericControllerButton.B0: return Buttons.A;
        //        case GenericControllerButton.B1: return Buttons.B;
        //        case GenericControllerButton.B2: return Buttons.X;
        //        case GenericControllerButton.B3: return Buttons.Y;
        //        case GenericControllerButton.B4: return Buttons.LeftShoulder;
        //        //case GenericControllerButton.B0: return Buttons.LeftTrigger;
        //        case GenericControllerButton.B5: return Buttons.RightShoulder;
        //        //case GenericControllerButton.B0: return Buttons.RightTrigger;
        //        case GenericControllerButton.B6: return Buttons.Back;
        //        case GenericControllerButton.B7: return Buttons.Start;

        //        default: return 0;
        //    }
        //}

        public static SpriteName ButtonSprite(Buttons button)
        {
            switch (button)
            {
                default: return SpriteName.NO_IMAGE;
                case Buttons.A: return SpriteName.ButtonA;
                case Buttons.B: return SpriteName.ButtonB;
                case Buttons.X: return SpriteName.ButtonX;
                case Buttons.Y: return SpriteName.ButtonY;
                case Buttons.Start: return SpriteName.ButtonMENU;
                case Buttons.Back: return SpriteName.ButtonVIEW;
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
    }
}
