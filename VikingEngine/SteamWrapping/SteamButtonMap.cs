using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;
using VikingEngine.Input;

namespace VikingEngine.SteamWrapping
{
    struct SteamAnalogMap : IDirectionalMap
    {
        int controllerIx;
        SteamActionSet actionSet;
        bool mouseMode;
        SteamAnalogAction actionType;
        DirXYstepping steppingData = new DirXYstepping();

        public SteamAnalogMap(SteamActionSet actionSet, bool mouseMode, SteamAnalogAction action, int controllerIx)
        {
            this.actionSet = actionSet;
            this.actionType = action;
            this.controllerIx = controllerIx;
            this.mouseMode = mouseMode;
        }
        public Vector2 direction
        {
            get
            {
                var data = Ref.steam.input.controllers[controllerIx].analog_current[(int)actionType];

                if (data.bActive == 1)
                {
                    return new Vector2(data.x, data.y);
                }
                return Vector2.Zero;
            }
        }
        public Vector2 directionAndTime { get { return mouseMode ? direction : direction * Ref.DeltaTimeMs; } }
        public IntVector2 stepping { get { return steppingData.update(direction, false); } }
        public bool plusKeyIsDown { get { return false; } }

        public string directionsName { get { return actionType.ToString(); } }
        public InputSourceType inputSource { get { return InputSourceType.XController; } }
        public int ControllerIndex { get { return controllerIx; } set { controllerIx = value; } }

        public SpriteName Icon
        {
            get
            {
                return Ref.steam.input.actionIcon(controllerIx, actionSet, actionType);
            }
        }
        public void ListIcons(List<SpriteName> list)
        {
            list.Add(Icon);
        }
        public void ListIcons(List<SpriteName> list, out SpriteName plusKeyIcon, bool includeAlternative)
        {
            list.Add(Icon);
            plusKeyIcon = SpriteName.NO_IMAGE;
        }

        public void write(System.IO.BinaryWriter w)
        {
            throw new NotImplementedException();
        }
        public void read(System.IO.BinaryReader r)
        {
            throw new NotImplementedException();
        }
    }

    struct SteamButtonMap : IButtonMap
    {
        int controllerIx;
        SteamActionSet actionSet;
        SteamDigitalAction actionType;

        public override string ToString()
        {
            return $"steam action \"{actionType}\", ix{controllerIx}";
        }

        public SteamButtonMap(SteamActionSet actionSet, SteamDigitalAction action, int controllerIx)
        {
            this.actionType = action;
            this.controllerIx = controllerIx;
        }
        public bool IsActive
        {
            get
            {
                return Ref.steam.input.IsActive(controllerIx, actionType);
            }
        }
        public bool IsDown { get { return Ref.steam.input.controllers[controllerIx].digital_isDown_current[(int)actionType]; } }
        public bool DownEvent
        {
            get
            {
                var controller = Ref.steam.input.controllers[controllerIx];

                return controller.digital_isDown_current[(int)actionType] &&
                      !controller.digital_isDown_previous[(int)actionType];
            }
        }

        public bool DownEvent_AnyInstance
        {
            get
            {
                foreach (var controller in Ref.steam.input.controllers)
                {
                    if (controller.digital_isDown_current[(int)actionType] &&
                          !controller.digital_isDown_previous[(int)actionType])
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool UpEvent
        {
            get
            {
                var controller = Ref.steam.input.controllers[controllerIx];

                return !controller.digital_isDown_current[(int)actionType] &&
                      controller.digital_isDown_previous[(int)actionType];
            }
        }

        public float Value
        {
            get
            {
                return IsDown ? 1f : 0f;
            }
        }

        public bool IsMouse { get { return false; } }
        public string ButtonName { get { return actionType.ToString(); } }
        public InputSourceType inputSource { get { return InputSourceType.XController; } }
        public int buttonIndex { get { return (int)actionType; } }
        public int ControllerIndex { get { return controllerIx; } set { controllerIx = value; } }



        public SpriteName Icon
        {
            get
            {
                return Ref.steam.input.actionIcon(controllerIx, actionSet, actionType);//XboxInputLib.ButtonSprite(button);
            }
        }
        public void ListIcons(List<SpriteName> list)
        {
            list.Add(Icon);
        }

        public void ToRichContent(RichBoxContent content)
        {
            var icon = Ref.steam.input.actionIcon(controllerIx, actionSet, actionType);
            if (icon != SpriteName.NO_IMAGE)
            {
                content.Add(new RbImage(icon));
            }
            else
            {
                SInput.UnusedLayerToRichContent(content);
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            throw new NotImplementedException();
        }
        public void read(System.IO.BinaryReader r)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(SteamButtonMap))
            {
                SteamButtonMap other = (SteamButtonMap)obj;
                return other.actionType == this.actionType;
            }
            return false;
        }
    }
}
