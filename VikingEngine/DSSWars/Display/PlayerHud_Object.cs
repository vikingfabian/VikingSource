using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.DSSWars.Display
{
    class PlayerHud_Object
    {
        RichMenu menu;
        public PlayerHud_Object(LocalPlayer player)
        {
            //



        }

        void createMenu(LocalPlayer player)
        {
            if (menu == null)
            {
                var objectMenuArea = player.playerData.view.safeScreenArea;
                objectMenuArea.Width = HudLib.HeadDisplayWidth;
                objectMenuArea.Position.Y = player.hud.head.Bottom + Engine.Screen.IconSize * 0.5f;
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

        public void refreshObject(Players.LocalPlayer player, GameObject.AbsGameObject obj, bool selected)
        {
            //interaction?.DeleteMe();
            //interaction = null;

            //setVisible(obj != null);

            if (obj == null)
            {
                deleteMenu();
            }
            else
            {

                createMenu(player);

                //beginRefresh();
                //if (obj.CanMenuFocus() && player.input.inputSource.IsController)
                //{
                //    content.Add(new HUD.RichBox.RbImage(player.input.ControllerFocus.Icon));
                //    content.Add(new HUD.RichBox.RbText(":"));
                //    content.newLine();
                //}
                var content = new RichBoxContent();
                obj.toHud(new ObjectHudArgs(content, player, selected));
                menu.Refresh(content);
                //if (gui.menuState.Count > 0)
                //{
                //    content.newLine();
                //    content.Button(Ref.langOpt.Hud_Back, new RbAction(gui.menuBack, SoundLib.menuBack),
                //        null, true);
                //}
                //endRefresh(pos, selected);
                //viewOutLine(player.mapControls.focusedObjectMenuState());
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
