using Microsoft.Xna.Framework.Input;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.Input
{
    abstract class PlayerInputMap : IRichMenuInputMap
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

        public MouseInstance mouse = null;
        IDirectionalMap touchMap;

        public PlayerInputMap()
        { }

        public PlayerInputMap(int player)
        {
            this.playerIndex = player;
            init();
            setInputSource(new InputSource(InputSourceType.KeyboardMouse));            
        }

        virtual public void SetMouse(MouseInstance mouse)
        { 
            this.mouse = mouse;
        }

        virtual public MouseInstance RbMouseInstance() { return mouse != null? mouse: Input.Mouse.Instances[0]; }
        virtual public IButtonMap RbClick() { return new MouseButtonMap(MouseButton.Left); }
        virtual public IDirectionalMap RbScroll() { return new DirectionalMouseScrollMap(); }

        virtual public IntVector2 RbMoveSteps() { return IntVector2.Zero; }
        virtual public bool RbControllerMode => false;

        virtual public bool RbHasController => false;

        virtual protected void init()
        {
            menuInput = new HUD.MenuInputMap();
        }

        abstract public IButtonMap MenuClick { get; }

        public void setInputSource(InputSource source)
        {
            this.inputSource = source;
            //this.controllerIndex = index;

            switch (inputSource.sourceType)
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

        public bool Ctrl()
        {
            if (inputSource.HasKeyBoard)
            {
                return Input.Keyboard.Ctrl;
            }
            return false;
        }
        public bool Alt()
        {
            if (inputSource.HasKeyBoard)
            {
                return Input.Keyboard.Alt;
            }
            return false;
        }
        public bool Shift()
        {
            if (inputSource.HasKeyBoard)
            {
                return Input.Keyboard.Shift;
            }
            return false;
        }
    }
}
