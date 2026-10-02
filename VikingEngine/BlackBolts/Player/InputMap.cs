using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Input;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.ToGG;
using VikingEngine.ToGG.HeroQuest.Players.Ai;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.Core.BlackBolts.Player
{
    class InputMap : PlayerInputMap
    {
        public IDirectionalMap movement;
        public IDirectionalMap scroll;
        public IButtonMap click;
        public IButtonMap back;
        public IButtonMap rotate;

        public IButtonMap toggleEditMode;
        public InputMap(int playerIx)
            : base(playerIx)
        {
            
        }
        public override IButtonMap MenuClick => throw new NotImplementedException();
        override public void keyboardSetup()
        {
            movement = new AlternativeDirectionalMap(arrowKeys, WASD);
            scroll = new DirectionalMouseScrollMap();
            click = new AlternativeButtonsMap(new MouseButtonMap(MouseButton.Left), new KeyboardButtonMap(Keys.Enter));
            back = new AlternativeButtonsMap(new MouseButtonMap(MouseButton.Right), new KeyboardButtonMap(Keys.Back));
            rotate = new Input.KeyboardButtonMap(Keys.Tab);
            toggleEditMode = new Input.KeyboardButtonMap(Keys.Space);

            menuInput.keyboardSetup();
        }
        public override void genericControllerSetup()
        {
            xboxSetup();
        }
        override public void xboxSetup()
        {
            throw new System.NotImplementedException();
        }
    }
}
