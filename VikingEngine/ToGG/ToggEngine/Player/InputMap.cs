using VikingEngine.Input;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.ToGG
{
    class InputMap : PlayerInputMap
    {
        public IDirectionalMap movement;
        public IDirectionalMap scroll;
        public IButtonMap click;
        public IButtonMap back;
        public IButtonMap quickMoveItem;
        public IButtonMap nextPhase;
        public IButtonMap prevPhase;
        public IButtonMap moreInfo;
        public IButtonMap lineOfSight;
        public IButtonMap communications;

        public InputMap(int playerIx)
            : base(playerIx)
        {
            toggRef.inputmap = this;

            foreach (var m in Engine.XGuide.players)
            {
                m.inputMap = this;
            }
        }

        override public void keyboardSetup()
        {
            movement = new AlternativeDirectionalMap(arrowKeys, WASD);
            scroll = new DirectionalMouseScrollMap();
            click = new AlternativeButtonsMap(new MouseButtonMap(MouseButton.Left), new KeyboardButtonMap(Keys.Enter));
            back = new AlternativeButtonsMap(new MouseButtonMap(MouseButton.Right), new KeyboardButtonMap(Keys.Back));
            quickMoveItem = new AlternativeButtonsMap(new MouseButtonMap(MouseButton.Right), new MouseButtonMap(MouseButton.DoubleClick));

            nextPhase = new Input.AlternativeButtonsMap(
                new Input.KeyboardButtonMap(Keys.Space),
                new Input.KeyboardButtonMap(Keys.End));

            prevPhase = new Input.KeyboardButtonMap(Keys.Home);

            moreInfo = new AlternativeButtonsMap(new Input.KeyboardButtonMap(Keys.LeftControl), new Input.KeyboardButtonMap(Keys.RightControl));           
            lineOfSight = new AlternativeButtonsMap(new Input.KeyboardButtonMap(Keys.LeftShift), new Input.KeyboardButtonMap(Keys.RightShift));
            communications = new Input.KeyboardButtonMap(Keys.Tab);

            menuInput.keyboardSetup();
        }

        override public void xboxSetup()
        {
            throw new System.NotImplementedException();
        }

        public bool anyExitKey()
        {
            return click.DownEvent || back.DownEvent || nextPhase.DownEvent || menuInput.openCloseInputEvent();
        }

        //override public void ps4Setup()
        //{
        //    throw new System.NotImplementedException();
        //}

        public override void genericControllerSetup()
        {
            xboxSetup();
        }
    }
}
