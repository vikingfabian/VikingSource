using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD;
using VikingEngine.ToGG.HeroQuest;
using VikingEngine.ToGG;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Display.CutScene;
using VikingEngine.Engine;
using VikingEngine.HUD.RichMenu;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.Input;

namespace VikingEngine.DSSWars.Display
{
    class GameMenuSystem //: MenuSystem
    {
        bool gameWasPaused;
        Graphics.Image blackFade;
        protected ImageLayers layer = ImageLayers.Foreground7;
        RichMenu menu;
        InputMap input;
        public GameMenuSystem()
            //: base(new InputMap(Engine.XGuide.LocalHostIndex), MenuType.InGame)
        {
            input = new InputMap(true);
            
        }

        public void openMenu()
        {
            if (menu == null)
            {
                gameWasPaused = Ref.isPaused;
                Ref.SetPause(true);

                if (blackFade == null)
                {
                    VectorRect area = Engine.Screen.Area;
                    area.AddRadius(4);
                    blackFade = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, layer + 5);
                    blackFade.ColorAndAlpha(Color.Black, 0.4f);
                }

                VectorRect menuArea = Engine.Screen.SafeArea;
                menuArea.Y = DssRef.state.localPlayers[0].hud.head.Bottom + Engine.Screen.IconSize;
                menuArea.SetBottom(Engine.Screen.SafeArea.Bottom, true);

                menuArea.Width = HudLib.HeadDisplayWidth;
                menuArea.X = Engine.Screen.CenterScreen.X - menuArea.Width / 2;

                menu = new RichMenu(HudLib.RbSettings, menuArea, new Vector2(8), RichMenu.DefaultRenderEdge, layer, new PlayerData(PlayerData.AllPlayers));
                
                //base.openMenu();
            }
        }

        public bool menuUpdate()
        {
            bool mouseOver = false;
            if (menu != null)
            {
                menu.updateMouseInput(ref mouseOver);
                if (input.Menu.DownEvent)
                {
                    closeMenu();
                }

                return true;
            }

            return false;
        }

        void completeMenu(RichBoxContent content)
        {
            menu.Refresh(content);
            menu.updateHeightFromContent();
            menu.addBackground(new NineSplitSettings(SpriteName.WarsHudScrollerBg, 1, 6, 1f, true, true), layer + 2);
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

                Ref.SetPause(gameWasPaused);
                blackFade?.DeleteMe();
                blackFade = null;
                menu.DeleteMe();
                menu = null;
            }
        }

        void watchEpilogue()
        {
            closeMenu();
            new CutScene.NightmarePrologue();
        }

        void saveGameState()
        {
            closeMenu();

            if (DssRef.state.cutScene == null)
            {
                new SaveScene(false);
            }
        }

        void saveAndExit()
        {
            closeMenu();

            if (DssRef.state.cutScene == null)
            {
                new SaveScene(true).ExitGame = true;
            }
            else
            {
                DssRef.state.exit();
            }
        }

        void exit()
        {
            if (Ref.steam.statsInitialized)
            {
                Ref.steam.stats.upload();
            }
            closeMenu();
            DssRef.state.exit();
        }

        public void pauseMenu()
        {
            openMenu();
            RichBoxContent content = new RichBoxContent();
           
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.GameMenu_Resume) }, new RbAction(closeMenu))
                {
                    fillWidth = true
                });

            if (DssRef.storage.runTutorial)
            { //TODO yes no dialogue
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Tutorial_EndTutorial) }, new RbAction(endTutorial))
                {
                    fillWidth = true
                });
            }

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.GameMenu_SaveState) }, new RbAction(saveGameState),
                new RbTooltip_Text(DssRef.lang.GameMenu_SaveStateWarnings))
            {
                fillWidth = true
            });

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.GameMenu_WatchPrologue) }, new RbAction(watchEpilogue))
            {
                fillWidth = true
            });

            if (DssRef.state.IsLocalMultiplayer())
            {
                content.newLine();
                DssRef.storage.multiplayerGameSpeedToMenu(content, menu);
            }

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.GameMenu_NextSong) }, new RbAction(() => { Ref.music.debugNext(); closeMenu(); }))
            {
                fillWidth = true
            });

            content.newLine();
            Ref.gamesett.volumeOptions(content);

            content.Add(new RbSeperationLine());

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.GameMenu_ExitGame) }, new RbAction(exit))
            {
                fillWidth = true
            });
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_SaveAndExit) }, new RbAction(saveAndExit))
            {
                fillWidth = true
            });

            completeMenu(content);
            //openMenu();
            //GuiLayout layout = new GuiLayout(DssRef.lang.GameMenu_Title, menu);
            //{

            //    new GuiTextButton(DssRef.lang.GameMenu_Resume, null, closeMenu, false, layout);

            //    if (DssRef.storage.runTutorial)
            //    {
            //        new GuiDialogButton(DssRef.lang.Tutorial_EndTutorial, null, new GuiAction(endTutorial),
            //            false, layout);
            //    }
            //    new GuiTextButton(DssRef.lang.GameMenu_SaveState, DssRef.lang.GameMenu_SaveStateWarnings, saveGameState, false, layout);
            //    new GuiTextButton(DssRef.lang.GameMenu_WatchPrologue, null, watchEpilogue, false, layout);

            //    if (DssRef.state.IsLocalMultiplayer())
            //    {
            //        multiplayerGameSpeedToMenu(layout);
            //    }
            //    new GuiTextButton(DssRef.lang.GameMenu_NextSong, null, new GuiAction(() => { Ref.music.debugNext(); closeMenu(); }), false, layout);
            //    new GuiCheckbox("Model light effect", null, Ref.gamesett.modelLightProperty, layout);

            //    Ref.gamesett.soundOptions(layout);
            //    new GuiSectionSeparator(layout);
            //    new GuiDialogButton(DssRef.lang.GameMenu_ExitGame, null, new GuiAction(exit), false, layout);

            //    new GuiDialogButton(DssRef.lang.Hud_SaveAndExit, null, new GuiAction(saveAndExit), false, layout);

            //}
            //layout.End();
        }

        void endTutorial()
        {
            DssRef.stats.skipTutorial.addOne();

            foreach (var p in DssRef.state.localPlayers)
            {
                p.tutorial?.EndTutorial();
            }
            closeMenu();
        }

        public void debugMenu()
        {
            openMenu();
            //GuiLayout layout = new GuiLayout("DEBUG", menu);
            //{
            //    DssRef.state.localPlayers[0].debugMenu(layout);
            //}
            //layout.End();
        }

        public void controllerLost()
        {
            pauseMenu();

        }

        public bool IsOpen()
        { 
            return menu != null;
        }
    }
}
