using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.Core.BlackBolts.Interface
{
    class PauseMenu
    {
        public bool gameWasPaused;
        Image blackFade;
        RichMenu menu; 
        protected ImageLayers layer = ImageLayers.Foreground7;


        public void openMenu()
        {
            if (menu == null)
            {
                if (Ref.steam.isInitialized)
                {
                    SteamTimeline.SetTimelineGameMode(ETimelineGameMode.k_ETimelineGameMode_Menus);
                }
                gameWasPaused = Ref.isPaused;
                if (!Ref.netSession.InMultiplayerSession)
                {
                    Ref.SetPause(true);
                }
                if (blackFade == null)
                {
                    VectorRect area = Engine.Screen.Area;
                    area.AddRadius(4);
                    blackFade = new Image(SpriteName.WhiteArea, area.Position, area.Size, layer + 5);
                    blackFade.ColorAndAlpha(Color.Black, 0.4f);
                }

                VectorRect menuArea = Engine.Screen.SafeArea;
                
                menuArea.Width = HudLib.HeadDisplayWidth;
                menuArea.X = Engine.Screen.CenterScreen.X - menuArea.Width / 2;

                menu = new RichMenu(HudLib.RbSettings, menuArea, new Vector2(8), RichMenu.DefaultRenderEdge, layer, new PlayerData(PlayerData.AllPlayers));


            }
        }

        public void CloseMenu()
        {
            if (menu != null)
            {
                if (Ref.steam.isInitialized)
                {
                    SteamTimeline.SetTimelineGameMode(ETimelineGameMode.k_ETimelineGameMode_Playing);
                }
                if (Ref.gamesett.settingsHasChanged)
                {
                    Ref.gamesett.settingsHasChanged = false;
                    Ref.gamesett.Save();
                }

                Ref.SetPause(gameWasPaused);
                blackFade?.DeleteMe();
                blackFade = null;
                
                menu.DeleteMe();
                menu = null;

                
            }
        }

    }
}
