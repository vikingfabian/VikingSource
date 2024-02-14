using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.SteamWrapping;

namespace VikingEngine.Input
{
    class ControllerChoice
    {
        //public string name;
        public InputSourceType inputSource;
        public int controllerIx;

        public ControllerChoice(InputSourceType type, int controllerIx)
        {
            this.inputSource = type;
            this.controllerIx = controllerIx;
        }

        public bool connected()
        {
            switch (inputSource)
            {
                //case PlayerInputSource.GenericController:
                //    return SharpDXInput.controllers[controllerIx] != null;
                case InputSourceType.XController:
                    return Input.XInput.Instance(controllerIx).Connected;
                //case PlayerInputSource.SteamController:
                //    return SteamGamepad.GetController(controllerIx) != null;

                default: throw new NotImplementedException();
            }
        }

        public bool joinClick()
        {
            switch (inputSource)
            {
                //case PlayerInputSource.GenericController:
                //    for (GenericControllerButton btn = 0; btn < GenericControllerButton.NUM; btn++)
                //    {
                //        if (SharpDXInput.controllers[controllerIx].DownEvent(btn))
                //        {
                //            return true;
                //        }
                //    }
                //    break;
                case InputSourceType.XController:
                    if (Input.XInput.Instance(controllerIx).KeyDownEvent(Microsoft.Xna.Framework.Input.Buttons.A,
                        Microsoft.Xna.Framework.Input.Buttons.X,
                        Microsoft.Xna.Framework.Input.Buttons.Start))
                    {
                        return true;
                    }
                    break;
                //case PlayerInputSource.SteamController:
                //    var pad = SteamGamepad.GetController(controllerIx);
                //    if (pad != null)
                //    {
                //        if (pad.DownEvent(ButtonActionType.MenuClick) ||
                //            pad.DownEvent(ButtonActionType.MenuReturnToGame))
                //        {
                //            return true;
                //        }
                //    }
                //    break;
            }

            return false;
        }

        public string name()
        {
            switch (inputSource)
            {
                //case PlayerInputSource.GenericController:
                //    {
                //        GenericController c =  SharpDXInput.controllers[controllerIx];
                //        if (c.type == ControllerType.PS4)
                //        {
                //            return "PS4-" + TextLib.IndexToString(controllerIx);
                //        }
                //        else
                //        {
                //            return c.instance.ProductName;
                //        }
                //    }
                case InputSourceType.XController:
                    return "Xbox" + TextLib.IndexToString(controllerIx);
                //case InputSourceType.SteamController:
                //    return "Steam" + TextLib.IndexToString(controllerIx);

                default: throw new NotImplementedException();
            }
        }

        public SpriteName joinButtonIcon()
        {
            switch (inputSource)
            {
                //case PlayerInputSource.GenericController:
                //    GenericController c =  SharpDXInput.controllers[controllerIx];
                //    if (c.type == ControllerType.PS4)
                //    {
                //        return SpriteName.PsButtonCross;
                //    }
                //    else
                //    {
                //        return SpriteName.GenericButton0;
                //    }
                case InputSourceType.XController:
                    return SpriteName.ButtonA;
                //case PlayerInputSource.SteamController:
                //    return SteamGamepad.GetController(controllerIx).GetIcon(ButtonActionType.MenuClick);

                default: throw new NotImplementedException();
            }
        }
    }
}
