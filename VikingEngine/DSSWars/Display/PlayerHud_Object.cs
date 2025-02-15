using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Display
{
    class PlayerHud_Object
    {
        DiplomacyDisplay diplomacy;
        public RichMenu menu;
        public Army otherArmy;
        public PlayerHud_Object(LocalPlayer player)
        {
            //
            diplomacy = new DiplomacyDisplay(player);
        }

        void createMenu(LocalPlayer player)
        {
            if (menu == null)
            {
                var objectMenuArea = player.playerData.view.safeScreenArea;
                objectMenuArea.Width = HudLib.HeadDisplayWidth;

                if (player.hud.head != null)
                {
                    objectMenuArea.Position.Y = player.hud.head.Bottom + Engine.Screen.IconSize * 0.5f;
                }
                objectMenuArea.SetBottom(player.playerData.view.safeScreenArea.Bottom, true);
                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
                var bgTex = menu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);

                bgTex.SetColor(ColorExt.GrayScale(0.9f));
                bgTex.SetOpacity(0.95f);
            }
        }

        void deleteMenu()
        {
            menu?.DeleteMe();
            menu = null;
        }

        public void refresh(Players.LocalPlayer player, RichBoxContent content)
        {
            createMenu(player);
            menu.Refresh(content);
        }

        public void refreshDiplomacy(Players.LocalPlayer player, Faction faction, bool selected)
        {
            if (menu != null && menu.BlockRefresh())
            {
                return;
            }
            if (faction == null)
            {
                deleteMenu();
            }
            else
            {
                createMenu(player);

                var content = new RichBoxContent();
                diplomacy.toHud(content, faction, selected);
                menu.Refresh(content);
            }
        }

        public void refreshObject(Players.LocalPlayer player, GameObject.AbsGameObject obj, bool selected)
        {
            if (menu != null && menu.BlockRefresh())
            {
                return;
            }

            if (obj == null)
            {
                deleteMenu();
            }
            else
            {

                createMenu(player);

                var content = new RichBoxContent();
                obj.toHud(new ObjectHudArgs(content, player, selected));
                menu.Refresh(content);
            }
        }

        /// <returns>need refresh</returns>
        public bool updateMouseInput(ref bool mouseOver)
        {
            if (menu != null)
            {
                menu.updateMouseInput(ref mouseOver);
                return menu.needRefresh;
            }
            return false;
        }
    }
}
