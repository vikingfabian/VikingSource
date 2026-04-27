using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.DSSWars.Interface
{
    class PlayerHud_Pins
    {
        public RichMenu menu;

        public PlayerHud_Pins(LocalPlayer player)
        {
            //createMenu(player);
        }

        public void createMenu(LocalPlayer player)
        {
            if (player.hud.pins.Count > 0 && menu == null)
            {
                var menuArea = player.playerData.view.safeScreenArea;
                menuArea.X = player.hud.head.Right;
                if (player.hud.headOptions != null)
                {
                    menuArea.SetRight(player.hud.headOptions.Left, true);
                }
                else
                {
                    menuArea.SetRight(player.playerData.view.safeScreenArea.Right, true);
                }

                menu = new RichMenu(HudLib.RbSettings, menuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);

            }

        }

        /// <returns>need refresh</returns>
        public bool updateMouseInput(ref bool mouseOver)
        {
            if (menu == null)
                return false;

            bool mouseOverBg = false;
            menu.updateMouseInput(ref mouseOverBg);
            mouseOver |= menu.interaction?.hover != null;
            return menu.needRefresh;
        }

        public void refreshUpdate(LocalPlayer player)
        {
            createMenu(player);

            if (menu != null)
            {
                var content = new RichBoxContent();
                player.hud.pins.toHUD(player, content);
                menu.Refresh(content, player.gameControls.controllerPointer);
            }
        }
    }
}
