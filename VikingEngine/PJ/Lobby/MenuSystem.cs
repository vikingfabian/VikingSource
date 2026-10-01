using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars;
using VikingEngine.Engine;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;
using VikingEngine.SteamWrapping;

namespace VikingEngine.PJ
{
    class MenuSystem
    {
        protected ImageLayers layer = ImageLayers.Foreground0;
        RichMenu menu;
        LobbyState lobby;

        Graphics.Image blackFade;

        public MenuSystem()
        {
        }

        public MenuSystem(LobbyState lobby)
           
        {
            this.lobby = lobby;
            MainMenu();
        }

        public static GuiStyle GuiStyle()
        {
            return new GuiStyle(
                (Screen.PortraitOrientation ? Screen.Width : Screen.Height) * 0.61f, 5, SpriteName.LFMenuRectangleSelection);
        }

        public void openMenu()
        {
            if (menu == null)
            {
                if (Ref.steam.isInitialized)
                {
                    SteamTimeline.SetTimelineGameMode(ETimelineGameMode.k_ETimelineGameMode_Menus);
                }
                
                if (blackFade == null)
                {
                    VectorRect area = Engine.Screen.Area;
                    area.AddRadius(4);
                    blackFade = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, layer + 5);
                    blackFade.ColorAndAlpha(Color.Black, 0.8f);
                }

                VectorRect menuArea = Engine.Screen.SafeArea;
                
                menuArea.Width = Engine.Screen.IconSize * 8;
                menuArea.X = Engine.Screen.CenterScreen.X - menuArea.Width / 2;

                menu = new RichMenu(HudLib.RbSettings, menuArea, new Vector2(8), RichMenu.DefaultRenderEdge, layer, new PlayerData(PlayerData.AllPlayers));

            }
        }
        public void closeMenu()
        {
            if (menu != null)
            {
               
                if (Ref.gamesett.settingsHasChanged)
                {
                   
                    Ref.gamesett.settingsHasChanged = false;
                    Ref.gamesett.Save();
                }

                blackFade?.DeleteMe();
                blackFade = null;
                menu.DeleteMe();
                menu = null;

                Ref.steam.input.SetActionSet(SteamActionSet.DefaultSet);
                //GC.Collect();
            }
        }

        public bool menuUpdate()
        {
            bool mouseOver = false;
            if (menu != null)
            {
                if (menu.needRefresh)
                {
                    MainMenu();
                }
                menu.updateMouseInput(ref mouseOver);


                return true;
            }

            return false;
        }

        public bool Open => menu != null;

        public void MainMenu()
        {
            openMenu();
            RichBoxContent content = new RichBoxContent();

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.MenuIconResume) },
                new RbAction(closeMenu))
            {  fillWidth = true });

            Ref.gamesett.pjVolumeOptions(content);

            content.newParagraph();

            Ref.gamesett.pjMonitorOptions(content, menu);

            if (Ref.steam.isInitialized && Ref.steam.input.connectMaxCount > 0)
            {
                content.newParagraph();
                for (int i = 0; i < Ref.steam.input.connectMaxCount; i++)
                {
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                        new RbImage(SpriteName.DoConnectDevice) ,
                        new RbSpace(0.25f),
                        new RbImage((SpriteName)((int)SpriteName.PixController1 + i)),
                    },
                    Ref.steam.input.KeepConnectedProperty) { propertyTag = i });
                }
            }

            content.newParagraph();
            content.Add(new RbSeperationLine(Color.LightGray, 0.6f) { thick = true });
            content.Add(new RbNewLine(true, 1.2f));

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.MenuIconExit) },
               new RbAction(exitGame_OK))
            { fillWidth = true });

            completeMenu(content);

        }

        void completeMenu(RichBoxContent content)
        {
            menu.Refresh(content, null);
            //menu.updateHeightFromContent(Engine.Screen.SafeArea.Bottom);
            menu.addBackground(HudLib.HudMenuBackground, layer + 2);
        }

        void optionalCertAchievement()
        {
            PjRef.achievements.m3Timeout.Unlock();
        }

        void crashGame()
        {
            throw new TestException();
        }

        void exitGame_OK()
        {
            Ref.update.ExitToDash();
        }

        public static bool CloseMenuInput()
        {
            return Input.Keyboard.KeyDownEvent(Keys.Escape) ||
                Input.XInput.KeyDownEvent(Buttons.Start) ||
                Input.XInput.KeyDownEvent(Buttons.Back) ||
                Ref.steam.input.AnyKeyDownEvent(SteamDigitalAction.close_menu);
        }
    }
}
