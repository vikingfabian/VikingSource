using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.PlayerControls;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.DSSWars.Interface
{
    class TutorialDisplay
    {
        public bool refresh = true;
        LocalPlayer player;
        RichMenu display;

        public TutorialDisplay(LocalPlayer player)
        {
            this.player = player;
            //openMenu();

        }

        private void openMenu()
        {
            if (display == null)
            {
                refresh = true;

                VectorRect area = player.playerData.view.wideScreenSafeScreenArea;
                area.Width = HudLib.richboxGui.width;
                area.X = player.playerData.view.wideScreenSafeScreenArea.Right - area.Width;
                area.Y = player.hud.MessageStart.Y;
                area.SetBottom(player.playerData.view.wideScreenSafeScreenArea.Bottom, true);

                display = new RichMenu(HudLib.TutorialRbSettings, area, new Vector2(16), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
                //display.addBackground(HudLib.HudTutorialBackground, HudLib.GUILayer + 2);
            }
        }

        public void update(ref bool mouseOverHud)
        {
            if (player.hud.maximizedHud)
            {
                openMenu();

                if (refresh || DssRef.time.oneSecond)
                {
                    refresh = false;
                    RichBoxContent content = new RichBoxContent();
                    player.tutorial.tutorial_ToHud(content);
                    display.Refresh(content);
                    display.updateMouseInput(ref mouseOverHud);

                    display.updateHeightFromContent();
                    display.addBackground(HudLib.HudTutorialBackground, HudLib.GUILayer + 2);
                }
            }
            else
            {
                DeleteMe();
            }
        }
        public void DeleteMe()
        {
            if (display != null)
            {
                display.DeleteMe();
                display = null;
            }
        }

    }

    //class TutorialDisplayPart : RichboxGuiPart
    //{
    //    Vector2 pos;
    //    public TutorialDisplayPart(Players.LocalPlayer player, RichboxGui gui)
    //        : base(gui)
    //    {
    //        pos = VectorExt.AddX(player.playerData.view.safeScreenArea.RightTop, -(HudLib.richboxGui.width+ gui.settings.edgeWidth));
    //    }

    //    public void refresh(Players.LocalPlayer player, Tutorial tutorial)
    //    {
    //        beginRefresh();
    //        //RichBoxContent content = new RichBoxContent();
    //        tutorial.tutorial_ToHud(content);
    //        endRefresh(pos, false);
    //    }
    //}
}
