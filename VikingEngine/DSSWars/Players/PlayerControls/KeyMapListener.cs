using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.GO.PickUp;

namespace VikingEngine.DSSWars.Players.PlayerControls
{
    class KeyMapListener : AbsUpdateable
    {
        RichMenu menu;
        List<Keys> availableKeyboardKeys;
        public KeyMapListener(RichMenu menu)
            :base(true)
        { 
            this.menu = menu;
            menu.OnPageDelete += DeleteMe;
            availableKeyboardKeys = Input.Keyboard.AllMappableKeys();
        }

        public override void Time_Update(float time_ms)
        {
            foreach (var key in availableKeyboardKeys)
            {
                if (Input.Keyboard.KeyDownEvent(key))
                {
                    GameMenuSystem.onKeyBoardKeySelect(menu, key);
                }
            }
            if (Input.Keyboard.KeyDownEvent(Keys.Escape))
            {
                menu.OpenMenu(GameMenuSystem.UnderMenu_Options_Keyboard, StackOption.ClearStack);
            }
        }
        public override void DeleteMe()
        {
            base.DeleteMe();
            
        }
    }
}
