using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Input;

namespace VikingEngine.LootFest.Players
{
    class InputMap : PlayerInputMap
    {
        public IDirectionalMap movement;
        public IButtonMap holdMovement;
        public IDirectionalMap camera;
        public IDirectionalMap cameraZoom;
        public IButtonMap mainAttack;
        public IButtonMap altAttack;
        public IButtonMap jump;
        public IButtonMap interact;
        public IButtonMap useItem;
        public IButtonMap altButton;
        public IButtonMap firstPersonToggle;
        
        public Voxels.EditorInputMap editorInput;

        public IButtonMap chat = new KeyboardButtonMap(Keys.Enter);

        public InputMap(int playerIx)
            :base(playerIx)
        { }

        protected override void init()
        {
            base.init();
            editorInput = new Voxels.EditorInputMap();
        }

        override public void keyboardSetup()
        {
            movement = new AlternativeDirectionalMap(arrowKeys, WASD);
            holdMovement = new KeyboardButtonMap(Keys.LeftAlt);

            camera = new AlternativeDirectionalMap(
                new KeyPlusDirectionalMap(new AlternativeButtonsMap(new KeyboardButtonMap(Keys.LeftShift), new KeyboardButtonMap(Keys.RightShift)), arrowKeys),
                new DirectionalMouseMap());

            cameraZoom = new AlternativeDirectionalMap(
                new DirectionalMouseScrollMap(),

                new DirectionalButtonsMap(
                new KeyboardButtonMap(Keys.D1),
                new KeyboardButtonMap(Keys.D2),
                new NoButtonMap(),
                new NoButtonMap()
                ));

            if (inputSource.controllerIndex == 0)
            {
                mainAttack = new AlternativeButtonsMap(new MouseButtonMap(MouseButton.Left), new KeyboardButtonMap(Keys.V));
                altAttack = new AlternativeButtonsMap(new MouseButtonMap(MouseButton.Right), new KeyboardButtonMap(Keys.C));
            }
            else
            {
                mainAttack = new AlternativeButtonsMap(new KeyboardButtonMap(Keys.V), new MouseButtonMap(MouseButton.Left));
                altAttack = new AlternativeButtonsMap(new KeyboardButtonMap(Keys.C), new MouseButtonMap(MouseButton.Right));
            }

            jump = new KeyboardButtonMap(Keys.Space);
            interact = new KeyboardButtonMap(Keys.Q);
            useItem = new AlternativeButtonsMap(new KeyboardButtonMap(Keys.E), new MouseButtonMap(MouseButton.Middle));
            altButton = new KeyboardButtonMap(Keys.LeftAlt);
            firstPersonToggle = new KeyboardButtonMap(Keys.F2);
            

            menuInput.keyboardSetup();
            editorInput.keyboardSetup();
        }

        override public void xboxSetup()
        {
            movement = new DirectionalXboxMap(ThumbStickType.Left, false, inputSource.controllerIndex);
            holdMovement = new XboxButtonMap(Buttons.RightTrigger, inputSource.controllerIndex);
            camera = new DirectionalXboxMap(ThumbStickType.Right, false, inputSource.controllerIndex);
            cameraZoom = new DirectionalXboxMap(ThumbStickType.D, false, inputSource.controllerIndex);

            mainAttack = new XboxButtonMap(Buttons.X, inputSource.controllerIndex);
            altAttack = new XboxButtonMap(Buttons.B, inputSource.controllerIndex);
            jump = new XboxButtonMap(Buttons.A, inputSource.controllerIndex);
            interact = new XboxButtonMap(Buttons.Y, inputSource.controllerIndex);
            useItem = new XboxButtonMap(Buttons.RightShoulder, inputSource.controllerIndex);
            altButton = new XboxButtonMap(Buttons.LeftTrigger, inputSource.controllerIndex);
            firstPersonToggle = new XboxButtonMap(Buttons.RightStick, inputSource.controllerIndex);
            

            menuInput.xboxSetup(inputSource.controllerIndex);
            editorInput.xboxSetup(inputSource.controllerIndex, movement, camera);
        }

        //override public void ps4Setup()
        //{
        //    movement = new DirectionalGenericDualAxesMap(PS4InputLib.LeftStickX, PS4InputLib.LeftStickY, false, controllerIndex);
        //    holdMovement = new GenericControllerButtonMap(PS4InputLib.R2, controllerIndex);
        //    camera = new DirectionalGenericDualAxesMap(PS4InputLib.RightStickX, PS4InputLib.RightStickY, false, controllerIndex);
        //    cameraZoom = new DirectionalGenericDpadMap(PS4InputLib.Dpad, false, controllerIndex);

        //    mainAttack = new GenericControllerButtonMap(PS4InputLib.Square, controllerIndex);
        //    altAttack = new GenericControllerButtonMap(PS4InputLib.Cirkle, controllerIndex);
        //    jump = new GenericControllerButtonMap(PS4InputLib.Cross, controllerIndex);
        //    interact = new GenericControllerButtonMap(PS4InputLib.Triangle, controllerIndex);
        //    useItem = new GenericControllerButtonMap(PS4InputLib.R1, controllerIndex);
        //    firstPersonToggle = new GenericControllerButtonMap(PS4InputLib.RsClick, controllerIndex);
            
            
        //    altButton = new GenericControllerButtonMap(PS4InputLib.L2, controllerIndex);

        //    menuInput.ps4Setup(controllerIndex);
        //    editorInput.ps4Setup(controllerIndex, movement, camera);
        //}

        public override void genericControllerSetup()
        {
            xboxSetup();
        }
    }
}
