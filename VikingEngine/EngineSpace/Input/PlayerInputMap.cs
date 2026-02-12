using Microsoft.Xna.Framework.Input;

namespace VikingEngine.Input
{
    abstract class PlayerInputMap
    {
        /* Static readonlies */
        public static readonly DirectionalButtonsMap arrowKeys = new DirectionalButtonsMap(
            new KeyboardButtonMap(Keys.Up),
            new KeyboardButtonMap(Keys.Down),
            new KeyboardButtonMap(Keys.Left),
            new KeyboardButtonMap(Keys.Right)
            );
        public static readonly DirectionalButtonsMap WASD = new DirectionalButtonsMap(
            new KeyboardButtonMap(Keys.W),
            new KeyboardButtonMap(Keys.S),
            new KeyboardButtonMap(Keys.A),
            new KeyboardButtonMap(Keys.D)
            );

        /* Fields */
        public int playerIndex;
        public Input.InputSource inputSource;

        public HUD.MenuInputMap menuInput;

        public PlayerInputMap()
        { }

        public PlayerInputMap(int player)
        {
            this.playerIndex = player;
            init();
            setInputSource(InputSourceType.KeyboardMouse, 0);
            
        }

        virtual protected void init()
        {
            menuInput = new HUD.MenuInputMap();
        }

        abstract public IButtonMap MenuClick { get; }

        public void setInputSource(InputSourceType inputSource, int index)
        {
            this.inputSource = new InputSource(inputSource, index);
            //this.controllerIndex = index;

            switch (inputSource)
            {
                case InputSourceType.Mouse:
                case InputSourceType.Keyboard:
                case InputSourceType.KeyboardMouse:
                    keyboardSetup();
                    break;
                case InputSourceType.SteamInput:
                    steamSetup();
                    break;
                case InputSourceType.XController:
                    xboxSetup();
                    break;
            }
        }

        public static IntVector2 GenericMoveStepping()
        {
            return arrowKeys.stepping + WASD.stepping;
        }
        public static bool GenericClick()
        {
            return Keyboard.KeyDownEvent(Keys.Enter) || Keyboard.KeyDownEvent(Keys.Space);
        }
        
        abstract public void keyboardSetup();
        abstract public void xboxSetup();

        virtual public void steamSetup(){ }
        //abstract public void ps4Setup();
        abstract public void genericControllerSetup();

        /// <summary>
        /// Run the controller vibration 
        /// </summary>
        /// <param name="time">in milllisec</param>
        /// <param name="left">low freq engine</param>
        /// <param name="right">high freq engine</param>
        public void Vibrate(float time, float left, float right)
        {
            if (inputSource.sourceType == InputSourceType.XController)
            {
                XInput.Instance(this.playerIndex).vibrate(left, right, time);
            }
        }

        public bool Connected
        {
            get {
                switch (inputSource.sourceType)
                {
                    default: return true;

                    case InputSourceType.XController:
                        return Input.XInput.Instance(inputSource.controllerIndex).Connected;
                }
            }
        }

        virtual public Voxels.EditorInputMap VoxelEditorInput()
        {
            return null;
        }
    }
}
