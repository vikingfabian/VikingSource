using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;
using Microsoft.Xna.Framework;
using VikingEngine.Engine;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Input;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.PJ
{
    class MenuSystem
    {
        public Gui menu;
        LobbyState lobby;

        public MenuSystem(InputSource input)
        {
            menu = new Gui(GuiStyle(), Screen.SafeArea, 0f, HudLib.LayMenu, input);
            menu.useAnyControllerInput = true;
        }

        public MenuSystem(LobbyState lobby, InputSource input)
            :this(input)
        {
            this.lobby = lobby;
            MainMenu();
        }

        public static GuiStyle GuiStyle()
        {
            return new GuiStyle(
                (Screen.PortraitOrientation ? Screen.Width : Screen.Height) * 0.61f, 5, SpriteName.LFMenuRectangleSelection);
        }
        
        public void dlcChangedMenu()
        {
            GuiLayout layout = new GuiLayout("New DLC detected", menu);
            {
                new GuiTextButton("OK", null, lobby.Reset, false, layout);
            }
            layout.End();
        }

        public void MainMenu()
        {
            GuiLayout layout = new GuiLayout(SpriteName.birdLobbyMenuButton, null, menu, GuiLayoutMode.MultipleColumns);
            {
                if (PlatformSettings.DevBuild)
                {
#if PCGAME
                    Ref.steam.debugToMenu(layout);
#endif
                    new GuiTextButton("**Crash Game", null, crashGame, false, layout);
                    new GuiTextButton("**Achievements", null, debugAchievements, false, layout);
#if PCGAME
                    Ref.steam.debugInfoToMenu(layout);
#endif
                }
                new GuiBigIcon(SpriteName.MenuIconResume, null, new GuiAction(lobby.CloseMenu), false, layout);
                new GuiBigIcon(SpriteName.birdLobbyExitButton, null, new GuiAction(lobby.Reset), false, layout);

                if (!PjRef.XboxLayout)
                {
                    new GuiBigIcon(SpriteName.MenuIconSettings, null, new GuiAction(options), true, layout);
                }

                new GuiBigIcon(SpriteName.MenuIconCredits, null, new GuiAction(credits), true, layout);

                if (PjRef.XboxLayout)
                {
                    quickOptions(layout);
                }

                //new GuiTextButton("**Debug Achievement", "For optional cert only", optionalCertAchievement, false, layout);
                if (!PjRef.XboxLayout)
                {
                    new GuiImageButton(SpriteName.MenuIconExit, null, new GuiActionWithAreYouSureDialogue(exitGame_OK),
                    false, layout);
                }
            }
            layout.End();            
        }

        void optionalCertAchievement()
        {
            PjRef.achievements.m3Timeout.Unlock();
        }

        public void options()
        {
           
            GuiLayout layout = new GuiLayout(SpriteName.MenuIconSettings, menu);
            {
                quickOptions(layout);

                new GuiSectionSeparator(layout);
                resolutionOptions(layout);
                new GuiImageCheckbox(SpriteName.MenuIconMonitorArrowsOut, null, Ref.gamesett.fullscreenProperty, layout);

                new GuiSectionSeparator(layout);
                new GuiTextButton("*Debug Controls*", null, controlDebugScreen, true, layout);
            }
            layout.End();
        }

        void quickOptions(GuiLayout layout)
        {
            new GuiFloatSlider(SpriteName.MenuIconMusicVol, null, Ref.gamesett.musicVolProperty, new IntervalF(0, 1), false, layout);
            new GuiFloatSlider(SpriteName.MenuIconSoundVol, null, Ref.gamesett.soundVolProperty, new IntervalF(0, 1), false, layout);
            new GuiIntSlider(SpriteName.ControllerVibration, null, vibrationProperty, new IntervalF(0, 200), false, layout);
        }

        int vibrationProperty(bool set, int value)
        {
            if (set)
            {
                var c = menu.inputSource.Controller;

                if (c == null)
                {
                    c = Input.XInput.Default();
                }

                if (c != null)
                {
                    c.vibrate(0.7f, 0.7f, 200, true);
                }
            }
            return GetSet.Do<int>(set, ref Ref.gamesett.VibrationLevel, value);
        }

        void controlDebugScreen()
        {
            new GameState.DebugControlsState();
        }


        void resolutionOptions(GuiLayout layout)
        {
            Ref.gamesett.listMonitors(layout);

            var resoutionPercOptions = Engine.Screen.ResoutionPercOptions();

            List<GuiOption<int>> optionsList = new List<GuiOption<int>>();
            foreach (var m in resoutionPercOptions)
            {
                optionsList.Add(new GuiOption<int>(m.ToString() + "%", m));
            }

            new GuiIconOptionsList<int>(SpriteName.MenuIconScreenResolution, "R", optionsList, Ref.gamesett.resolutionPercProperty, layout);
            new GuiIconTextButton(SpriteName.MenuIconScreenResolution, "Youtube", null, recordingResolutionOptions, true, layout);
        }

        void recordingResolutionOptions()
        {
            GuiLayout layout = new GuiLayout("Recording setup", menu);
            {
                var monitor = Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter;
                for (RecordingPresets rp = 0; rp < RecordingPresets.NumNon; ++rp)
                {
                    IntVector2 sz = Engine.Screen.RecordingPresetsResolution(rp);
                    if (sz.Y > monitor.CurrentDisplayMode.Height)
                    {
                        break;
                    }
                    else
                    {
                        if (rp == Engine.Screen.UseRecordingPreset)
                        {
                            new GuiIconTextButton(SpriteName.LfCheckYes, rp.ToString(), null,
                                new GuiAction1Arg<RecordingPresets>(Ref.gamesett.setRecordingPreset, rp), false, layout);
                        }
                        else
                        {
                            new GuiTextButton(rp.ToString(), null,
                                new GuiAction1Arg<RecordingPresets>(Ref.gamesett.setRecordingPreset, rp), false, layout);
                        }
                    }
                }
            }
            layout.End();
        }

        GraphicsAdapter monitorProperty(bool set, GraphicsAdapter val)
        {
            if (set)
            {
                Screen.Monitor = val;
                Screen.ApplyScreenSettings();
            }

            return Screen.Monitor;
        }
        
        private void credits()
        {
            GuiLayout layout = new GuiLayout(SpriteName.MenuIconCredits, null, menu, GuiLayoutMode.SingleColumn);
            layout.scrollOnly = true;
            {
                string version;
                if (PjRef.XboxLayout)
                {
                    version = PlatformSettings.XboxVersion;
                }
                else
                {
                    version = PlatformSettings.SteamVersion;
                }

                new GuiLabel("Party Jousting - " + version, layout);
                new GuiRichLabel(
                    new List<HUD.RichBox.AbsRichBoxMember>
                    {
                        new HUD.RichBox.RbImage(SpriteName.MenuIconScreenResolution),
                        new HUD.RichBox.RbText(Engine.Screen.RenderingResolution.ToString("x"))
                    }, 
                    layout);

#if XBOX
                new GuiRichLabel(
                    new List<HUD.RichBox.AbsRichBoxMember>
                    {
                        new HUD.RichBox.RichBoxImage(SpriteName.defaultGamerIcon),
                        new HUD.RichBox.RichBoxText(Ref.xbox.gamer.Name())
                    }, 
                    layout);
#endif
                new GuiSectionSeparator(layout);

                new GuiLargeImageLabel(SpriteName.CreditsProgrammerArt, layout);
                new GuiLabel("Fabian \"Viking\" Jakobsson", layout);

                new GuiLargeImageLabel(SpriteName.CreditsMusic, layout);
                new GuiLabel("Martin \"Akri\" Grönlund", layout);

                new GuiLargeImageLabel(SpriteName.CreditsTranslation, layout);
                new GuiLabel(
                    "Aurelie Perrin (French)" + Environment.NewLine +
                    "Mykola \"NickDp\" Kosarev (Russian, Ukranian)" + Environment.NewLine +
                    "Thomas Enge Yoshi Lund (Norweigian)" + Environment.NewLine +
                    "Theodoros Kakiouzis (Greek)" + Environment.NewLine +
                    "Attila \"ab1311\" (German)" + Environment.NewLine +
                    "Pedro J Durán (Spanish)" + Environment.NewLine +
                    "David Stenström & Jung-min Kim (Korean)", layout);

                new GuiLargeImageLabel(SpriteName.CreditsPlaytest, layout);
                new GuiLabel(
                    "Pontus Bengtsson" + Environment.NewLine +
                    "Samuel Reneskog" + Environment.NewLine +
                    "Thomas Enge Yoshi Lund" + Environment.NewLine +
                    "Krischan Olsson" + Environment.NewLine +
                    "Tobias Reneskog" + Environment.NewLine +
                    "EeuorT", layout);

                new GuiSectionSeparator(layout);

                new GuiLabel("vikingfabian.com", layout);
            }
            layout.End();
        }

        void debugAchievements()
        {
            PlatformService.Achievements.ToMenu(PjRef.achievements.All(), menu);
        }

        void crashGame()
        {
            throw new TestException();
        }

        void exitGame_OK()
        {
            Ref.update.ExitToDash();
        }

        public void DeleteMe()
        {
            PjRef.storage.saveLoad(true, true);
            menu.DeleteMe();
        }

        public static bool CloseMenuInput()
        {
            return Input.Keyboard.KeyDownEvent(Keys.Escape) ||
                Input.XInput.KeyDownEvent(Buttons.Start) ||
                Input.XInput.KeyDownEvent(Buttons.Back);
        }
    }
}
