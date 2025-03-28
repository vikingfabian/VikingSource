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
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using Microsoft.Xna.Framework.Input;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.ToGG.HeroQuest.Display;
using VikingEngine.LootFest.GO.PickUp;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Display
{
    class GameMenuSystem //: MenuSystem
    {
        public const string UnderMenu_Options_Mouse = "options_mouse";
        public const string UnderMenu_Options_Keyboard = "options_keyboard";
        public const string UnderMenu_Options_Keyboard_Key = "options_keyboard_key";
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
                if (DssRef.state.localPlayers[0].hud.head != null)
                {
                    menuArea.Y = DssRef.state.localPlayers[0].hud.head.Bottom + Engine.Screen.IconSize;
                    menuArea.SetBottom(Engine.Screen.SafeArea.Bottom, true);
                }
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
                content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Tutorial_EndTutorial) }, new RbAction(endTutorial))
                {
                    fillWidth = true
                });
            }

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.GameMenu_SaveState) }, new RbAction(saveGameState),
                new RbTooltip_Text(DssRef.lang.GameMenu_SaveStateWarnings))
            {
                fillWidth = true
            });

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.GameMenu_WatchPrologue) }, new RbAction(watchEpilogue))
            {
                fillWidth = true
            });

            if (DssRef.state.IsLocalMultiplayer())
            {
                content.newLine();
                DssRef.storage.multiplayerGameSpeedToMenu(content, menu);
            }

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.GameMenu_NextSong) }, new RbAction(() => { Ref.music.debugNext(); closeMenu(); }))
            {
                fillWidth = true
            });

            content.newLine();
            SettingsToMenu(content, menu, false);
            //Ref.gamesett.volumeOptions(content);
            content.newParagraph();
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

        public static void SettingsToMenu(RichBoxContent content, RichMenu menu, bool lobby)
        {
            Ref.gamesett.volumeOptions(content);

            if (lobby)
            {
                content.newParagraph();
                content.h2(".Monitor options", HudLib.TitleColor_Head);
                Ref.gamesett.monitorOptions(content, menu);
            }

            content.newParagraph();
            content.h2(".Graphics options", HudLib.TitleColor_Head);
            Ref.gamesett.graphicsOptions(content, menu);

            content.newParagraph();
            content.h2(".Input", HudLib.TitleColor_Head);
            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(".Pan on zoom") }, Ref.gamesett.panOnZoomProperty));

            content.newLine();
            content.Add(new RbImage(SpriteName.MouseScroll));
            content.space();
            content.Add(new RbText(".Scroll sensitivity: game"));
            content.space();
            content.Add(new RbDragButton(new DragButtonSettings(0.1f, 10, 0.1f), Ref.gamesett.scrollGameProperty, true));

            content.newLine();
            content.Add(new RbImage(SpriteName.MouseScroll));
            content.space();
            content.Add(new RbText(".Scroll sensitivity: menu"));
            content.space();
            content.Add(new RbDragButton(new DragButtonSettings(0.1f, 10, 0.1f), Ref.gamesett.scrollMenuProperty, true));

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.Mouse, 0.8f), new RbSpace(), new RbText(DssRef.todoLang.MouseSettings_Title) },
                new RbAction2Arg<string, bool>(menu.OpenMenu, UnderMenu_Options_Mouse, true)));
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.Keyboard, 0.8f), new RbSpace(), new RbText(DssRef.todoLang.KeyboardSettings_Title) },
                new RbAction2Arg<string, bool>(menu.OpenMenu, UnderMenu_Options_Keyboard, true)));

            content.newParagraph();
            content.h2(".Gameplay options", HudLib.TitleColor_Head);
            if (lobby)
            {
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.GameMenu_AutoSave) }, autoSaveProperty));
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Tutorial_MenuOption) }, tutorialProperty));
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.lang.GameMenu_UseSpeedX, DssConst.MaxSpeedOption)) }, speed5Property));
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.GameMenu_LongerBuildQueue) }, longerBuildQueueProperty));
            }
            content.newLine();
            content.Add(new RbText(".Blood:", HudLib.TitleColor_Label));
            content.space();
            RbDragButton.RbDragButtonGroup(content, new List<float> { 100 }, new DragButtonSettings(0, GameSettings.MaxBlood, 10), Ref.gamesett.bloodProperty);

            bool autoSaveProperty(int index, bool set, bool value)
            {
                if (set)
                {
                    DssRef.storage.autoSave = value;

                    DssRef.storage.Save(null);
                }
                return DssRef.storage.autoSave;
            }

            bool speed5Property(int index, bool set, bool value)
            {
                if (set)
                {
                    DssRef.storage.speed5x = value;

                    DssRef.storage.Save(null);
                }
                return DssRef.storage.speed5x;
            }

            bool tutorialProperty(int index, bool set, bool value)
            {
                if (set)
                {
                    DssRef.storage.runTutorial = value;

                    DssRef.storage.Save(null);
                }
                return DssRef.storage.runTutorial;
            }

            bool longerBuildQueueProperty(int index, bool set, bool value)
            {
                if (set)
                {
                    DssRef.storage.longerBuildQueue = value;

                    DssRef.storage.Save(null);
                }
                return DssRef.storage.longerBuildQueue;
            }
        }

        static InputActionType CurrentEditInput;

        public static void keyboardOptions(RichMenu menu)
        {
            RichBoxContent content = new RichBoxContent();

            content.h1(DssRef.todoLang.KeyboardSettings_Title, HudLib.TitleColor_Head);

            var map = Ref.gamesett.keyboardMap;
            var list = map.listInputs(true);
            foreach (InputActionType input in list)
            {
                IButtonMap button = null;
                map.getset(input, ref button, false);

                content.newLine();
                content.Add(new RbText(LangLib.InputActionName(input), HudLib.TitleColor_Label));
                content.space();
                //List<AbsRichBoxMember> buttonContent = new List<AbsRichBoxMember>();
                content.Add(new ArtButton(RbButtonStyle.Primary, KeyTypeButtonContent(button.ButtonName, button.Icon), 
                    new RbAction1Arg<InputActionType>(
                    (InputActionType action) => {
                        CurrentEditInput = action;
                        menu.OpenMenu(UnderMenu_Options_Keyboard_Key, true);
                    }, input)));

                //RichBoxContent.ButtonMap(button, buttonContent);
                //new GuiRichButton(HudLib.RbOnGuiSettings, buttonContent, null,
                //    new GuiAction2Arg<bool, InputActionType>(listMapOptions, keyboard, input),
                //    true, layout);
            }

            menu.Refresh(content);
        }

        public static void listMapOptions(RichMenu menu)
        {

            RichBoxContent content = new RichBoxContent();

            content.h1(LangLib.InputActionName(CurrentEditInput), HudLib.TitleColor_Head);

            var map =Ref.gamesett.keyboardMap;

            var availableKeyboardKeys = VikingEngine.Input.Keyboard.AllMappableKeys();

            foreach (Keys key in availableKeyboardKeys)
            {
                content.Add(new ArtButton(RbButtonStyle.Primary, KeyTypeButtonContent(key.ToString(), Input.KeyboardButtonMap.GetKeySprite(key)),
                    new RbAction2Arg<RichMenu, Keys>(onKeyBoardKeySelect, menu, key)));
            }


            //layout.End();

            //inKeyMapsMenu = true;
            //mappingFor = input;
            //layout.OnDelete += closingOptionsMenuEvent;

            menu.Refresh(content);
            new VikingEngine.DSSWars.Players.PlayerControls.KeyMapListener(menu);
        }

        public static void onKeyBoardKeySelect(RichMenu menu, Keys key)
        {
            IButtonMap map = new KeyboardButtonMap(key);
            Ref.gamesett.keyboardMap.getset(CurrentEditInput, ref map, true);
            menu.OpenMenu(UnderMenu_Options_Keyboard, false);
        }

        static List<AbsRichBoxMember> KeyTypeButtonContent(string name, SpriteName icon)
        {
            List<AbsRichBoxMember> buttonContent = new List<AbsRichBoxMember>();
            if (icon != SpriteName.KeyUnknown &&
                icon != SpriteName.NO_IMAGE)
            {
                buttonContent.Add(new RbImage(icon));
            }
            else
            {
                buttonContent.Add(new RbText(name));
            }

            return buttonContent;
        }

        public static void mouseOptions(RichMenu menu)
        {
            RichBoxContent content = new RichBoxContent();

            content.h1(DssRef.todoLang.MouseSettings_Title, HudLib.TitleColor_Head);

            // Map of available actions and their display names
            Dictionary<MouseButtonAction, string> mouseActions = new Dictionary<MouseButtonAction, string>()
            {
                { MouseButtonAction.None, DssRef.todoLang.MouseButtonAction_None },
                { MouseButtonAction.Select, DssRef.todoLang.MouseButtonAction_Select },
                { MouseButtonAction.Cancel, DssRef.todoLang.MouseButtonAction_Cancel },
                { MouseButtonAction.Pan, DssRef.todoLang.MouseButtonAction_Pan },
                { MouseButtonAction.PanAndOrder, DssRef.todoLang.MouseButtonAction_PanAndOrder },
                { MouseButtonAction.Order, DssRef.todoLang.MouseButtonAction_Order },
            };

            // Local helper method for one dropdown
            void AddMouseButtonDropdown(MouseButton button, SpriteName icon, string label)
            {
                DropDownBuilder dropDown = new DropDownBuilder(label);

                MouseButtonAction currentAction = Ref.gamesett.keyboardMap.GetMouseAction(button);

                foreach (var kv in mouseActions)
                {
                    dropDown.AddOption(
                        kv.Value,
                        kv.Key == currentAction,
                        kv.Key == MouseButtonAction.Select,
                        new RbAction1Arg<MouseButtonAction>((MouseButtonAction action) =>
                        {
                            Ref.gamesett.keyboardMap.SetMouseAction(button, action);
                            Ref.gamesett.settingsHasChanged = true;
                            menu.CloseDropDown();
                        }, kv.Key),
                        null
                    );
                }

                dropDown.Build(content, icon, label, menu);
            }

            // Add dropdowns for each mouse button
            AddMouseButtonDropdown(MouseButton.Left, SpriteName.MouseButtonLeft, ".left");
            AddMouseButtonDropdown(MouseButton.Right, SpriteName.MouseButtonRight, ".right");
            AddMouseButtonDropdown(MouseButton.Middle, SpriteName.MouseButtonMiddle, ".middle");
            AddMouseButtonDropdown(MouseButton.X1, SpriteName.MouseButtonX1, ".x1");
            AddMouseButtonDropdown(MouseButton.X2, SpriteName.MouseButtonX2, ".x2");

            menu.Refresh(content);
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
