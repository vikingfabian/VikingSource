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

namespace VikingEngine.DSSWars.Display
{
    class TutorialDisplay //: HUD.RichBox.RichboxGui
    {
        public bool refresh = true;
        //TutorialDisplayPart displayPart;
        LocalPlayer player;
        RichMenu display;

        //public Graphics.Image whiteBar;
        

        public TutorialDisplay(LocalPlayer player)
            //: base(HudLib.richboxGui, player.gameControls.input)
        {
            this.player = player;

            VectorRect area = player.playerData.view.safeScreenArea;
            area.Width = HudLib.richboxGui.width;
            area.X = player.playerData.view.safeScreenArea.Right - area.Width;
            area.Y = player.hud.headOptions.MessageStart.Y;
            area.SetBottom(player.playerData.view.safeScreenArea.Bottom, true);

            display = new RichMenu(HudLib.TutorialRbSettings, area, new Vector2(16), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
            display.addBackground(HudLib.HudTutorialBackground, HudLib.GUILayer + 2);
            //displayPart = new TutorialDisplayPart(player, this);
            //parts = new List<HUD.RichBox.RichboxGuiPart>()
            //{
            //    displayPart, 
            //};

            //whiteBar = new Graphics.Image(SpriteName.WhiteArea, new Vector2(Engine.Screen.SafeArea.Right - HudLib.richboxGui.width - Engine.Screen.SmallIconSize, 0),
            //    Engine.Screen.Area.Size, ImageLayers.Background0);
        }

        public void update(ref bool mouseOverHud)
        {
            if (refresh || DssRef.time.oneSecond)
            {
                refresh = false;
                RichBoxContent content = new RichBoxContent();
                player.tutorial.tutorial_ToHud(content);
                display.Refresh(content);
                //displayPart.refresh(player, player.tutorial);
                display.updateMouseInput(ref mouseOverHud);
            }
        }
        public void DeleteMe()
        {
            display.DeleteMe();
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
