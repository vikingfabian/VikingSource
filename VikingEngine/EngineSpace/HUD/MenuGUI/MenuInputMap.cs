using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Input;

namespace VikingEngine.HUD
{
    class MenuInputMap
    {
        public IButtonMap OpenCloseKeyBoard;
        public IButtonMap OpenCloseController;

        public IDirectionalMap mouse;
        public IDirectionalMap movement;
        public IDirectionalMap scroll;
        public IButtonMap click;
        public IButtonMap back;
        public IButtonMap tabLeftUp;
        public IButtonMap tabRightDown;

        public MenuInputMap()
        {
            mouse = new DirectionalMouseMap();
            OpenCloseKeyBoard = new KeyboardButtonMap(Keys.Escape);
        }

        public bool openCloseInputEvent()
        {
            return OpenCloseKeyBoard.DownEvent || OpenCloseController.DownEvent;
        }

        public IButtonMap MainOpenCloseKey(InputSource inputSource)
        {
            if (inputSource.sourceType == InputSourceType.XController)
            {
                return OpenCloseController;
            }
            else
            {
                return OpenCloseKeyBoard;
            }
        }

        public void keyboardSetup()
        {
            OpenCloseController = new NoButtonMap();
            movement = new AlternativeDirectionalMap(Input.PlayerInputMap.arrowKeys, Input.PlayerInputMap.WASD);
            scroll = new DirectionalMouseScrollMap();

            if (PlatformSettings.RunProgram == StartProgram.ToGG)
            {
                click = new AlternativeButtonsMap(new MouseButtonMap(MouseButton.Left), new KeyboardButtonMap(Keys.Enter));
            }
            else
            {
                click = new AlternativeButtonsMap(new MouseButtonMap(MouseButton.Left), new KeyboardButtonMap(Keys.Enter));
            }
            back = new AlternativeButtonsMap(new MouseButtonMap(MouseButton.Right), new KeyboardButtonMap(Keys.Back));
            tabLeftUp = new KeyboardButtonMap(Keys.PageUp);
            tabRightDown = new KeyboardButtonMap(Keys.PageDown);
        }

        public void xboxSetup(int controllerIndex)
        {
            OpenCloseController = new XboxButtonMap(Buttons.Start, controllerIndex);

            movement = new AlternativeDirectionalMap(
                new DirectionalXboxMap(ThumbStickType.Left, false, controllerIndex),
                new DirectionalXboxMap(ThumbStickType.D, false, controllerIndex));

            scroll = new AlternativeDirectionalMap(
                new DirectionalMouseScrollMap(), 
                new DirectionalXboxTriggerMap(controllerIndex));

            click = new AlternativeButtonsMap(new XboxButtonMap(Buttons.A, controllerIndex), new XboxButtonMap(Buttons.X, controllerIndex));
            
            back = new AlternativeButtonsMap(new XboxButtonMap(Buttons.B, controllerIndex), new MouseButtonMap(MouseButton.Right));
            tabLeftUp = new XboxButtonMap(Buttons.LeftShoulder, controllerIndex);
            tabRightDown = new XboxButtonMap(Buttons.RightShoulder, controllerIndex);
        }

        //public void ps4Setup(int controllerIndex)
        //{
        //    OpenCloseController = new GenericControllerButtonMap(PS4InputLib.Options, controllerIndex);

        //    movement = new DirectionalGenericDualAxesMap(PS4InputLib.LeftStickX, PS4InputLib.LeftStickY, false, controllerIndex);
        //    scroll = new AlternativeDirectionalMap(new DirectionalMouseScrollMap(), 
        //        new DirectionalButtonsMap(
        //            new GenericControllerButtonMap(PS4InputLib.R2, controllerIndex), 
        //            new GenericControllerButtonMap(PS4InputLib.L2, controllerIndex), 
        //            new NoButtonMap(), new NoButtonMap()));

        //    click = new AlternativeButtonsMap(
        //        new GenericControllerButtonMap(PS4InputLib.Cross, controllerIndex),
        //        new GenericControllerButtonMap(PS4InputLib.Square, controllerIndex)
        //        );

        //    back = new AlternativeButtonsMap(new GenericControllerButtonMap(PS4InputLib.Cirkle, controllerIndex), new MouseButtonMap(MouseButton.Right));
        //    tabLeftUp = new GenericControllerButtonMap(PS4InputLib.L1, controllerIndex);
        //    tabRightDown = new GenericControllerButtonMap(PS4InputLib.R1, controllerIndex);
        //}
    }
}
