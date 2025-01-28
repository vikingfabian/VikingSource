using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.DSSWars.Display
{
    class GameHudMenu
    {
        RichMenu headDisplay;
        RichMenu factionDisplay;

        RichMenu objectDisplay;

        public GameHudMenu(LocalPlayer player)
        {
            float headWidth = HudLib.HeadDisplayWidth * 1.6f;
            var headMenuArea = player.playerData.view.safeScreenArea;
            headMenuArea.Width = headWidth;
            headDisplay = new RichMenu(HudLib.RbSettings, headMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
            refreshFaction(player);
            headMenuArea = headDisplay.richBox.area;
            headDisplay.addBackground(new NineSplitSettings(SpriteName.WarsHudHeadBarBg, 1, 8, 1f, true, true), HudLib.GUILayer + 2);

            var objectMenuArea = player.playerData.view.safeScreenArea;
            objectMenuArea.Width = HudLib.HeadDisplayWidth;
            objectDisplay = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
            objectDisplay.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);
        }

        public void refreshFaction(Players.LocalPlayer player)
        {
            var content = new RichBoxContent();
            player.faction.toHud(new ObjectHudArgs(content, player, false));

        }

        public void refreshObject(Players.LocalPlayer player, GameObject.AbsGameObject obj, bool selected)
        {
            //interaction?.DeleteMe();
            //interaction = null;

            //setVisible(obj != null);

            if (obj != null)
            {
                //beginRefresh();
                //if (obj.CanMenuFocus() && player.input.inputSource.IsController)
                //{
                //    content.Add(new HUD.RichBox.RbImage(player.input.ControllerFocus.Icon));
                //    content.Add(new HUD.RichBox.RbText(":"));
                //    content.newLine();
                //}
                var content = new RichBoxContent();
                obj.toHud(new ObjectHudArgs(content, player, selected));
                objectDisplay.Refresh(content);
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
    }
}
