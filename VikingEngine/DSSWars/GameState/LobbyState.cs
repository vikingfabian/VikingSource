using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//
using VikingEngine.HUD;
using VikingEngine.Engine;
using VikingEngine.Network;
using System.Threading;
using VikingEngine.PJ;
using VikingEngine.PJ.CarBall;
using VikingEngine.PJ.Strategy;
using Microsoft.Xna.Framework.Content;
using VikingEngine.Graphics;
using VikingEngine.Timer;
using VikingEngine.LootFest;
using VikingEngine.Input;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DebugExtensions;
using System.ComponentModel.Design;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameState;
using VikingEngine.HUD.RichBox;
using System.Linq;
using VikingEngine.DSSWars.Players;
using System.IO;
using VikingEngine.DataStream;
using VikingEngine.HUD.RichMenu;
using VikingEngine.HUD.RichBox.Artistic;
using System.Reflection.Metadata;
using VikingEngine.DSSWars.GameState.MapEditor;

namespace VikingEngine.DSSWars
{
    class LobbyState : AbsDssState
    {
        Display.MenuSystem menuSystem;
        MapBackgroundLoading mapBackgroundLoading;
        NetworkLobby netLobby = new NetworkLobby();
        GameTimer emitTimer = new GameTimer(0.1f);

        Texture2D bgTex;
        Graphics.ImageAdvanced bgImage = null;
        Display.SplitScreenDisplay splitScreenDisplay = new Display.SplitScreenDisplay();
        XInputJoinHandler joinHandler = new XInputJoinHandler();
        bool controllerStartGameUpdate = false;
        Graphics.TextG maploading;
        GuiLabel difficultyLevelText = null;

        InputButtonType mappingFor;
        bool inKeyMapsMenu = false;
        List<Keys> availableKeyboardKeys;

        VectorRect underMenuArea;
        RichMenu richmenu;
        const float MenuBgOpacity = 0.9f;
        RichMenu topMenu, underMenu;
        public LobbyState()
            : base()
        {
            HudLib.Init();
            Ref.isPaused = false;
            Engine.Screen.SetupSplitScreen(1, true);
            if (!StartupSettings.BlockBackgroundLoading)
            {
                mapBackgroundLoading = new MapBackgroundLoading(null);
            }

            Ref.draw.ClrColor = new Color(11, 30, 34);

            menuSystem = new Display.MenuSystem(new InputMap(Engine.XGuide.LocalHostIndex), Display.MenuType.Lobby);
            DssRef.storage.checkConnected();
            //mainMenu();

            Graphics.TextG version = new Graphics.TextG(LoadedFont.Console, Screen.SafeArea.RightBottom,
                Engine.Screen.TextSizeV2, new Align(Vector2.One), string.Format(DssRef.lang.Lobby_GameVersion, Engine.LoadContent.SteamVersion),
                Color.LightYellow, ImageLayers.Background2);

            maploading = new Graphics.TextG(LoadedFont.Console, Screen.SafeArea.LeftBottom,
                Engine.Screen.TextSizeV2, new Align(new Vector2(0, 1f)), "...",
                Color.DarkGray, ImageLayers.Background2);

            new Timer.AsynchActionTrigger(load_asynch, true);
            new Timer.TimedAction0ArgTrigger(playMusic, 1000);

            if (Ref.gamesett.language == LanguageType.NONE)
            {
                selectLanguageMenu();
            }

            availableKeyboardKeys = VikingEngine.Input.Keyboard.AllKeys.ToList();

            //reserved keys
            availableKeyboardKeys.Remove(Keys.Escape);
            availableKeyboardKeys.Remove(Keys.Enter);
            availableKeyboardKeys.Remove(Keys.Up);
            availableKeyboardKeys.Remove(Keys.Down);
            availableKeyboardKeys.Remove(Keys.Left);
            availableKeyboardKeys.Remove(Keys.Right);

            if (Ref.lobby == null)
            {
                new NetLobby();
            }

            Ref.lobby.startSearchLobbies(true);

            //testMenu2();
            createMenuLayout();

            //newGameSettings2();
        }

        void createMenuLayout()
        {
            VectorRect leftArea = Screen.SafeArea;
            leftArea.Width = Screen.IconSize * 5;
            leftArea.Round();

            VectorRect titleArea = leftArea;
            titleArea.Height = leftArea.Width * 0.25f;
            titleArea.Round();

            VectorRect menuArea = leftArea;
            menuArea.AddToTopSide(-titleArea.Height);

            const int BgOffScreenLength = 10;

            VectorRect titleBgArea = titleArea;
            {
                titleBgArea.X = -BgOffScreenLength;
                titleBgArea.SetRight(titleArea.Right, true);
                titleBgArea.Y = -BgOffScreenLength;
                titleBgArea.SetBottom(titleArea.Bottom, true);

                NineSplitAreaTexture titleBg = new NineSplitAreaTexture(new NineSplitSettings(SpriteName.WarsHudScrollerBg, 1, 6, 1f, true, true), titleBgArea, ImageLayers.Lay8);
            }

            new TextG(LoadedFont.Bold, titleArea.Center, Screen.TextTitleScale * 2f, Align.CenterAll, "DSS 2", HudLib.TitleColor_Head, ImageLayers.Lay4);

            VectorRect menuBgArea = menuArea;
            {
                menuBgArea.X = titleBgArea.X;
                menuBgArea.SetRight(titleBgArea.Right, true);
                menuBgArea.SetBottom(Engine.Screen.Area.Bottom + BgOffScreenLength, true);
                NineSplitAreaTexture menuBg = new NineSplitAreaTexture(new NineSplitSettings(SpriteName.WarsHudScrollerBg, 1, 6, 1f, true, true), menuBgArea, ImageLayers.Lay9);
                //menuBg.SetOpacity(MenuBgOpacity);
            }

            VectorRect menuContentArea = menuArea;
            //menuContentArea.AddRadius(-8);

            topMenu = new RichMenu(HudLib.RbSettings, menuContentArea, new Vector2(8), RichMenu.DefaultRenderEdge, ImageLayers.Lay4, new PlayerData(PlayerData.AllPlayers));

            topMenu.Refresh(new RichBoxContent() { new RbNewLine() });
            refreshMainMenu2();

            underMenuArea = new VectorRect(menuBgArea.Right + Screen.BorderWidth, menuContentArea.Y, Screen.IconSize * 6, menuContentArea.Height);
            
        }

        void openUnderMenu()
        {
            if (underMenu == null)
            {
                underMenu = new RichMenu(HudLib.RbSettings, underMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, ImageLayers.Lay4, new PlayerData(PlayerData.AllPlayers));
                underMenu.addBackground(new NineSplitSettings(SpriteName.WarsHudScrollerBg, 1, 6, 1f, true, true), ImageLayers.Lay9).SetOpacity(MenuBgOpacity);
            }
        }

        

        void testMenu2()
        {
            var area = Screen.SafeArea;
            area.Width = Screen.IconSize * 8;
            area.X = Screen.CenterScreen.X;

            
            richmenu = new RichMenu(HudLib.RbSettings, area, new Vector2(10), RichMenu.DefaultRenderEdge, ImageLayers.Top1, new PlayerData(PlayerData.AllPlayers));
            richmenu.addBackground(HudLib.HudMenuBackground, ImageLayers.Top1_Back);

            RichBoxContent content = new RichBoxContent();
            content.h1("New menu", HudLib.TitleColor_Head);
            content.text("Text text text");
            content.newLine();
            content.Add(new RbDragButton(new DragButtonSettings(1, 100, 1), IntGetSet));
            content.newLine();
            RbDragButton.RbDragButtonGroup(content, new List<int> { 1,10 }, new DragButtonSettings(1, 100, 1), IntGetSet);

            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("check") }, BoolGetSet));

            content.newLine();

            content.Add(new ArtOption(true, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsResource_Food), new RbText("opt1") }, null));
            content.Add(new ArtOption(false, new List<AbsRichBoxMember> { new RbText("opt2") }, null));
            content.Add(new ArtOption(false, new List<AbsRichBoxMember> { new RbText("opt3") }, null));

            content.newLine();
            content.Add(new ArtTabgroup(new List<ArtTabMember>
            {
                new ArtTabMember(new List<AbsRichBoxMember> { new RbText("tab1") }),
                new ArtTabMember(new List<AbsRichBoxMember> { new RbText("tab2") }),
                new ArtTabMember(new List<AbsRichBoxMember> { new RbText("tab3") }),
            }, 0, null));

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Start new game") }, null, null));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Load") }, null, null));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Options") }, null, null));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Map editor") }, null, null));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Voxel editor") }, null, null));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Flag painter") }, null, null));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Exit") }, null, null));

            for (int i = 0; i < 100; i++)
            {
                content.newLine();
                var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("test" + i.ToString()) }, null, new RbTooltip(TooltipTest, i));
                btn.fillWidth = true;
                content.Add(btn);
                content.Button("test" + i.ToString(), null, null, true);
            }

            richmenu.Refresh(content);
        }

        void TooltipTest(RichBoxContent content, object tag)
        {
            content.h2("Tooltip");
            content.text("Button no: " + tag.ToString());
        }

        bool testBool = false;
        bool BoolGetSet(int index, bool set, bool value)
        {
            if (set)
            {
                testBool = value;
            }
            return testBool;
        }

        int testInt = 1;
        int IntGetSet(bool set, int value)
        {
            if (set)
            {
                testInt = value;
            }
            return testInt;
        }

        void load_asynch()
        {
            bgTex = Ref.main.Content.Load<Texture2D>(DssLib.ContentDir + "dss_bg");
            new Timer.Action0ArgTrigger(loadingComplete);
        }

        void loadingComplete()
        {
            float w = Engine.Screen.SafeArea.Width;
            float h = w / bgTex.Width * bgTex.Height;
            float x = Engine.Screen.SafeArea.X;
            float y = Screen.CenterScreen.Y - h * 0.5f;

            bgImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                new Vector2(x, y), new Vector2(w, h), ImageLayers.Background5, false);
            bgImage.Texture = bgTex;
            bgImage.SetFullTextureSource();
            bgImage.Opacity = 0.5f;

            Vector2 promoworkerSz = new Vector2(9, 6) * new Vector2(h * 0.02f);

            var worker1 = new Graphics.Image(SpriteName.warsWorkerPromoCannon, VectorExt.AddY(Engine.Screen.Area.PercentToPosition(0.7f, 1f), -promoworkerSz.Y * 0.9f), promoworkerSz, ImageLayers.Background5);
            worker1.LayerAbove(bgImage);

            //var worker2 = new Graphics.Image(SpriteName.warsWorkerPromoBox, VectorExt.AddY(Engine.Screen.Area.PercentToPosition(0.6f, 1f), -promoworkerSz.Y * 0.9f), promoworkerSz, ImageLayers.Background5);
            //worker2.LayerAbove(bgImage);

            //var worker3 = new Graphics.Image(SpriteName.warsWorkerPromoBox, VectorExt.AddY(Engine.Screen.Area.PercentToPosition(0.5f, 1f), -promoworkerSz.Y * 0.8f), promoworkerSz, ImageLayers.Background5);
            //worker3.LayerAbove(bgImage);

            //var worker4 = new Graphics.Image(SpriteName.warsWorkerPromoBox, VectorExt.AddY(Engine.Screen.Area.PercentToPosition(0.2f, 1f), -promoworkerSz.Y * 0.9f), promoworkerSz, ImageLayers.Background5);
            //worker4.LayerAbove(bgImage);

        }

        void playMusic()
        {
            if (Ref.music != null)
            {
                Ref.music.PlaySong(Data.Music.Intro, false);
            }
        }

        void refreshMainMenu2()
        {
            const float ButtonTextTabbing = 0.15f;
            const float ArrowTabbing = 0.9f;
            const float ArrowScale = 0.6f;
            SpriteName moreOptArrow = SpriteName.LfMenuMoreMenusArrow;
            RichBoxContent content = new RichBoxContent();

            {
                content.newLine();

                var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { 
                    new RbBeginTitle(), 
                    new RbImage(SpriteName.MissingImage), 
                    new RbTab(ButtonTextTabbing), 
                    new RbText(DssRef.lang.Settings_NewGame),
                    new RbTab(ArrowTabbing),
                    new RbImage(moreOptArrow, ArrowScale),
                }, 
                    new RbAction(newGameSettings2), null);
                btn.fillWidth = true;
                content.Add(btn);
            }
            {
                content.newLine();
                var btn = new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { 
                    new RbImage(SpriteName.MissingImage), new RbTab(ButtonTextTabbing), new RbText(DssRef.lang.GameMenu_LoadState),
                    new RbTab(ArrowTabbing),
                    new RbImage(moreOptArrow, ArrowScale),
                }, null, null);
                btn.fillWidth = true;
                content.Add(btn);
            }
           
            content.newParagraph();
            {
                var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.MenuIconSettings) }, null);
                content.Add(btn);
            }
            {
                var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.EditorToolPencil) }, new RbAction(listEditors), new RbTooltip_Text("Editor"));
                content.Add(btn);
            }

            content.Add(new RbNewLine_AtHeight(topMenu.richboxArea.Height - topMenu.richBox.lineSpacing * 2f));
            {
                content.newParagraph();
                var btn = new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbImage(SpriteName.MissingImage), new RbTab(ButtonTextTabbing), new RbText(DssRef.lang.Lobby_ExitGame) }, new RbAction(exitGame), null);
                //btn.fillWidth = true;
                content.Add(btn);
            }

            topMenu.Refresh(content);
        }

        void listEditors()
        {
            openUnderMenu();

            RichBoxContent content = new RichBoxContent();

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>() { new RbText("Map editor") },
                new RbAction(openMapEditor)));

            underMenu.Refresh(content);
        }

        void openMapEditor()
        {
            new MapEditor_Generator();
        }

        void mainMenu()
        {
            controllerStartGameUpdate = false;
            menuSystem.openMenu();
            menuSystem.menu.PopAllLayouts();

            

            var saves = DssRef.storage.meta.listSaves();

            GuiLayout layout = new GuiLayout(string.Empty, menuSystem.menu);
            {
                if (StartupSettings.CheatActive)
                {
                    new GuiLabel("! debug cheats !", layout);
                }
#if DEBUG
                new GuiLargeTextButton(DssRef.lang.Lobby_Start, null, new GuiAction(startGame), false, layout);
#endif

                if (arraylib.HasMembers(saves))
                {
                    new GuiTextButton(DssRef.lang.GameMenu_ContinueFromSave, saves[0].InfoString(), new GuiAction1Arg<SaveStateMeta>(continueFromSave, saves[0]), false, layout);
                }

                new GuiLargeTextButton(DssRef.lang.Settings_NewGame, null, new GuiAction(newGameSettings) /*new GuiAction(startGame)*/, true, layout);

                if (arraylib.HasMembers(saves))
                {
                    new GuiTextButton(DssRef.lang.GameMenu_LoadState, null, listSaves, true, layout);
                }

                new GuiTextButton(string.Format(DssRef.lang.Lobby_LocalMultiplayerEdit, DssRef.storage.playerCount),
                    null, localMultiplayerMenu, true, layout);

                for (int playerNum = 1; playerNum <= DssRef.storage.playerCount; ++playerNum)
                {
                    var playerData = DssRef.storage.localPlayers[playerNum - 1];
                    if (DssRef.storage.playerCount > 1)
                    {
                        new GuiLabel(string.Format(DssRef.lang.Player_DefaultName, playerNum), layout);
                        new GuiTextButton(DssRef.lang.Lobby_NextScreen, null, new GuiAction1Arg<int>(nextScreenIndex, playerNum), false, layout);
                    }
                    DssRef.storage.flagStorage.flagDesigns[playerData.profile].Button(layout, new GuiAction1Arg<int>(listProfiles, playerNum), true);
                    new GuiTextButton(DssRef.lang.Lobby_FlagEdit, null, new GuiAction1Arg<int>(openProfileEditor, playerData.profile), false, layout);

                    if (DssRef.storage.playerCount > 1)
                    {
                        new GuiTextButton(string.Format(Ref.langOpt.InputSelect, playerData.inputSource.ToString()), null, new GuiAction3Arg<int, bool, SaveStateMeta>(selectInputMenu, playerNum, false, null), true, layout);
                    }

                    new GuiSectionSeparator(layout);
                }
                if (DssRef.storage.playerCount > 1)
                {
                    new GuiCheckbox(Ref.langOpt.VerticalSplitScreen, null, verticalSplitProperty, layout);
                    menuSystem.multiplayerGameSpeedToMenu(layout);
                }


                new GuiSectionSeparator(layout);
                new GuiIconTextButton(SpriteName.AutomationGearIcon, Ref.langOpt.Options_title, null, new GuiAction(optionsMenu), true, layout);
                //new GuiTextButton("*Crash game*", null, crashTest, false, layout); 

                new GuiTextButton("Play Commander", "A small tactical board game", new GuiAction(extra_PlayCommanderVersus), false, layout);
                
                if (PlatformSettings.DevBuild)
                {
                    new GuiTextButton("Map file generator", "Creates maps to play on. Takes about 10 minutes.", mapFileGenerator, false, layout);
                    

                    new GuiLargeTextButton("Test sound", null, new GuiAction(testsound), false, layout);
                    new GuiTextButton("Load mod", null, loadMod, false, layout);
                    if (Ref.steam.statsInitialized)
                    {
                        new GuiTextButton("Initialize steam stats", null, Ref.steam.stats.initializeAllStatsOnSteam, false, layout);
                        new GuiTextButton("Load global steam stats", null, Ref.steam.stats.beginRequestGlobalStats, false, layout);
                    }
                    new GuiTextButton("Text Input", null, new Action(() =>
                    {
                        new TextInput("test", null, null);
                    }), true, layout);
                }
                new GuiTextButton("Credits", null, credits, true, layout);

                //new GuiTextButton("Voxel Editor", "Tool to create the voxel models. Xbox controller required!", voxeleditor, false, layout);
                new GuiSectionSeparator(layout);
                new GuiTextButton(DssRef.lang.Lobby_ExitGame, null, exitGame, false, layout);
            } layout.End();

            
        }

        void loadMod()
        {
            string dir = "Modding" + DataStream.FilePath.Dir + "ModConst.txt";

            Data.Constants.ModLoader loader = new Data.Constants.ModLoader(dir);
        }

        void testsound()
        {
            Ref.music.stop(true);
            Ref.music.PlaySong(Data.Music.IAmYourDoom, false);
        }
        public void credits()
        {
            GuiLayout layout = new GuiLayout("Credits", menuSystem.menu);
            layout.scrollOnly = true;
            {
                //var oldFormat = menu.style.textFormat;
                //menu.style.textFormat.Font = LoadedFont.Console;
                //menu.style.textFormat.size *= 1.6f;

                new GuiLabel("DSS war party", layout);

                new GuiLabel("Art, Design & Programming:" + Environment.NewLine +
                    "Fabian \"Viking\" Jakobsson", layout);

                new GuiLabel("Music:" + Environment.NewLine +
                    "Diva Production Music / Melody Loops" + Environment.NewLine +
                    "EdRecords / Melody Loops" + Environment.NewLine +
                    "Jon Wright / Melody Loops" + Environment.NewLine +
                    "Erick McNereney / Melody Loops" + Environment.NewLine +
                    "Soundroll / Melody Loops", layout);

                new GuiLabel("Main playtesters:" + Environment.NewLine +
                    "Pontus Bengtsson" + Environment.NewLine +
                    "Craig \"Total Miner\" Martin" + Environment.NewLine +
                    "Rocky Johnsson" + Environment.NewLine +
                    "blumpo" + Environment.NewLine +
                    "Staticwombat"
                    , layout);

                //new GuiLabel("Winner of the Creative Coast \"Game Concept Challenge\" 2018 Award", layout);

                new GuiSectionSeparator(layout);

                new GuiLabel("vikingfabian games", layout);
            }
            layout.End();
        }
        //void settingsGui(GuiLayout layout)
        //{

        void keyMappingMenu()
        {
            GuiLayout layout = new GuiLayout(DssRef.lang.Settings_ButtonMapping, menuSystem.menu);
            {
                new GuiTextButton(HudLib.InputName(InputSourceType.Keyboard), null, new GuiAction1Arg<bool>(keyMappingMenu_InputSource, true), true, layout);
                //new GuiTextButton(HudLib.InputName(InputSourceType.XController), null, new GuiAction1Arg<bool>(keyMappingMenu_InputSource, false), true, layout);
            }
            layout.End();
        }

        void keyMappingMenu_InputSource(bool keyboard)
        {
            GuiLayout layout = new GuiLayout(HudLib.InputName(keyboard ? InputSourceType.Keyboard : InputSourceType.XController), menuSystem.menu);
            {
                var map = keyboard ? Ref.gamesett.keyboardMap : Ref.gamesett.controllerMap;
                var list = map.listInputs(keyboard);
                foreach (var input in list)
                {
                    IButtonMap button = null;
                    map.getset(input, ref button, false);
                    List<AbsRichBoxMember> buttonContent = new List<AbsRichBoxMember>(6)
                    {
                        new RbText(map.Name(input) + ": "),
                    };
                    RichBoxContent.ButtonMap(button, buttonContent);
                    new GuiRichButton(HudLib.RbOnGuiSettings, buttonContent, null,
                        new GuiAction2Arg<bool, InputButtonType>(listMapOptions, keyboard, input),
                        true, layout);
                }
            }
            layout.End();
        }

        void listMapOptions(bool keyboard, InputButtonType input)
        {
            var map = keyboard ? Ref.gamesett.keyboardMap : Ref.gamesett.controllerMap;
            GuiLayout layout = new GuiLayout(map.Name(input), menuSystem.menu);
            {
                if (keyboard)
                {
                    foreach (var key in availableKeyboardKeys)
                    {
                        var icon = Input.KeyboardButtonMap.GetKeyTile(key);
                        if (icon != SpriteName.KeyUnknown)
                        {
                            new GuiImageButton(icon, null,
                                new GuiAction1Arg<Keys>(listMapOptions_keyboardlink, key),
                                false, layout);
                        }
                    }
                }
            }
            layout.End();

            inKeyMapsMenu = true;
            mappingFor = input;
            layout.OnDelete += closingOptionsMenuEvent;

        }

        void closedKeymapsMenu()
        {
            inKeyMapsMenu = false;
        }

        void listMapOptions_keyboardlink(Keys key)
        {
            IButtonMap buttonMap = new KeyboardButtonMap(key);
            Ref.gamesett.keyboardMap.getset(mappingFor, ref buttonMap, true);

            menuSystem.menu.PopLayout();
            menuSystem.menu.PopLayout();
            keyMappingMenu_InputSource(true);

        }
        void listMapOptions_controllerlink(InputButtonType input, IButtonMap buttonmap)
        {

        }

        //}
        void selectLanguageMenu()
        {
            Translation translate = new Translation();
            var options = translate.available();
            GuiLayout layout = new GuiLayout(string.Empty, menuSystem.menu);
            {
                foreach (var option in options)
                {
                    new GuiImageButton(translate.sprite(option), null, new GuiAction1Arg<LanguageType>(selectLanguegeLink, option), false, layout);
                }
            }
            layout.End();
        }

        void selectLanguegeLink(LanguageType language)
        {
            if (language != Ref.gamesett.language)
            {
                Ref.gamesett.language = language;
                new ChangeLanguageState();
            }
        }

        
        void gameModeClick(GameMode mode)
        {
            DssRef.difficulty.setting_gameMode = mode;
            DssRef.storage.Save(null);
            refreshDifficultyLevel();
            mainMenu();
            newGameSettings();
        }

        void newGameSettings2()
        {
            openUnderMenu();
            RichBoxContent content = new RichBoxContent();

            var start = new ArtButton(RbButtonStyle.Primary, 
                new List<AbsRichBoxMember> { new RbBeginTitle(), new RbText(DssRef.lang.Lobby_Start) },
                new RbAction(startGame));
            //start.fillWidth = false;
            content.Add(start);

            content.newParagraph();
            content.text(DssRef.lang.Lobby_MapSizeTitle).overrideColor = HudLib.TitleColor_Label;
            for (MapSize sz = 0; sz < MapSize.NUM; ++sz)
            {
                content.newLine();
                content.Add( new ArtOption(DssRef.storage.mapSize == sz, new List<AbsRichBoxMember> { new RbText(WorldData.SizeString(sz)) },
                    null));
            }

            content.newParagraph();
            content.text(string.Format(DssRef.lang.Settings_DifficultyLevel, DssRef.difficulty.PercDifficulty)).overrideColor = HudLib.TitleColor_Label;
            Difficulty.OptionsRb(content);

            content.newParagraph();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Settings_AllowPause) }, allowPauseProperty));
            content.newLine();
            //new GuiCheckbox(DssRef.lang.Settings_AllowPause, null, allowPauseProperty, layout);

            content.newParagraph();
            content.text(DssRef.lang.Settings_GameMode).overrideColor = HudLib.TitleColor_Label;
            for (GameMode mode = 0; mode < GameMode.NUM; ++mode)
            {
                content.newLine();
                gameModeText(mode, out string caption, out string desc);
                content.Add(new ArtOption(mode == DssRef.difficulty.setting_gameMode, new List<AbsRichBoxMember> { new RbText(caption) }, null)); 
            }

            underMenu.Refresh(content);

        }
        void newGameSettings()
        {
            var mapSizes = new List<GuiOption<MapSize>>((int)MapSize.NUM);
            for (MapSize sz = 0; sz < MapSize.NUM; ++sz)
            {
                mapSizes.Add(new GuiOption<MapSize>(WorldData.SizeString(sz), sz));
            }


            GuiLayout layout = new GuiLayout(string.Empty, menuSystem.menu);
            {
                new GuiLargeTextButton(DssRef.lang.Lobby_Start, null, new GuiAction(startGame), false, layout);
                new GuiOptionsList<MapSize>(SpriteName.NO_IMAGE, DssRef.lang.Lobby_MapSizeTitle, mapSizes, mapSizeProperty, layout);
                new GuiCheckbox(DssRef.lang.Settings_GenerateMaps, DssRef.lang.Settings_GenerateMaps_SlowDescription, generateNewMapsProperty, layout);

                difficultyLevelText = new GuiLabel("XXX", layout);

                new GuiTextButton(string.Format(DssRef.lang.Settings_DifficultyLevel, DssRef.difficulty.PercDifficulty), null, selectDifficultyMenu, true, layout);
                new GuiSectionSeparator(layout);

                new GuiLabel(DssRef.lang.Hud_Advanced, layout);
                

                gameModeText(DssRef.difficulty.setting_gameMode, out string modecaption, out string modedesc);

                new GuiTextButton(DssRef.lang.Settings_GameMode + " (" + modecaption + ")", modedesc, selectGameModeMenu, true, layout);
                new GuiCheckbox(DssRef.lang.Settings_AllowPause, null, allowPauseProperty, layout);
                //new GuiLabel(, layout);
                var foodSlider = new GuiFloatSlider(SpriteName.WarsResource_Food, DssRef.lang.Settings_FoodMultiplier, foodMultiProperty, new IntervalF(0.5f, 10f), false, layout);
                foodSlider.onLeaveCallback = new Action(foodSliderLeave);
                foodSlider.ToolTip = DssRef.lang.Settings_FoodMultiplier_Description;

                new GuiCheckbox(DssRef.todoLang.Settings_CentralGold, DssRef.todoLang.Settings_CentralGold_Description, centralGoldProperty, layout);

                new GuiTextButton(DssRef.lang.Settings_ResetToDefault, null, resetToDefault, false, layout);
            }
            layout.End();

            refreshDifficultyLevel();
        }

        public float foodMultiProperty(bool set, float value)
        {
            return GetSet.Do<float>(set, ref DssRef.difficulty.setting_foodMulti, value);
        }

        void gameModeText(GameMode mode, out string caption, out string desc)
        {
            caption = null;
            desc = null;
            switch (mode)
            {
                case GameMode.FullStory:
                    caption = DssRef.lang.Settings_Mode_Story;
                    desc = DssRef.lang.Settings_Mode_IncludeBoss + " " + DssRef.lang.Settings_Mode_IncludeAttacks;
                    break;
                case GameMode.Sandbox:
                    caption = DssRef.lang.Settings_Mode_Sandbox;
                    desc = DssRef.lang.Settings_Mode_IncludeAttacks;
                    break;
                case GameMode.Peaceful:
                    caption = DssRef.lang.Settings_Mode_Peaceful;
                    desc = DssRef.lang.Settings_Mode_Peaceful_Description;
                    break;
            }
        }
        //void gameSettingsMenu()
        //{
        //    GuiLayout layout = new GuiLayout(string.Empty, menuSystem.menu);
        //    {
        //        new GuiCheckbox(DssRef.lang.Settings_GenerateMaps, DssRef.lang.Settings_GenerateMaps_SlowDescription, generateNewMapsProperty, layout);
        //        new GuiTextButton(DssRef.lang.Settings_GameMode, null, selectGameModeMenu, true, layout);
        //        new GuiCheckbox(DssRef.lang.Settings_AllowPause, null, allowPauseProperty, layout);
        //        //new GuiCheckbox(DssRef.lang.Settings_BossEvents, DssRef.lang.Settings_BossEvents_SandboxDescription, bossProperty, layout);

        //    }
        //    layout.End();
        //}
        void selectGameModeMenu()
        {
            GuiLayout layout = new GuiLayout(string.Empty, menuSystem.menu);
            {
                for (GameMode mode = 0; mode < GameMode.NUM; ++mode)
                {
                    gameModeText(mode, out string caption, out string desc);
                    //new GuiTextButton(DssRef.lang.Settings_Mode_Story, DssRef.lang.Settings_Mode_InclueBoss + " " + DssRef.lang.Settings_Mode_InclueAttacks,
                    //    new GuiAction1Arg<GameMode>(gameModeClick, Data.GameMode.FullStory), false, layout);
                    //new GuiTextButton(DssRef.lang.Settings_Mode_Sandbox, DssRef.lang.Settings_Mode_InclueAttacks,
                    //        new GuiAction1Arg<GameMode>(gameModeClick, Data.GameMode.Sandbox), false, layout);
                    new GuiTextButton(caption, desc,
                        new GuiAction1Arg<GameMode>(gameModeClick, mode), false, layout);
                }
            }
            layout.End();
        }

        void selectDifficultyMenu()
        {
            GuiLayout layout = new GuiLayout(string.Empty, menuSystem.menu);
            {
                Difficulty.OptionsGui(layout, difficultyOptionsLink);
            }
            layout.End();
        }


        void difficultyOptionsLink(int difficulty)
        {
            DssRef.difficulty.set(difficulty);
            DssRef.storage.Save(null);
            refreshDifficultyLevel();
            mainMenu();
            newGameSettings();
            //menuSystem.menu.PopLayout();
        }

        void resetToDefault()
        {
            DssRef.difficulty = new Difficulty();
            DssRef.storage.Save(null);
            mainMenu();
            newGameSettings();
        }

        void foodSliderLeave()
        { 
            DssRef.storage.Save(null);
        }

        void extra_PlayCommanderVersus()
        {
            VikingEngine.ToGG.toggLib.Init();
            VikingEngine.ToGG.Commander.BattleLib.Init();
            new ToGG.ToggEngine.Map.SquareDic();
            ToGG.ToggEngine.Map.MainTerrainProperties.Init();
            new VikingEngine.ToGG.InputMap(0);
            //new Network.Session();

            ToGG.Commander.LevelSetup.GameSetup setup = new ToGG.Commander.LevelSetup.GameSetup();
            setup.lobbyMembers = new List<ToGG.AbsLobbyMember>
            {
                new ToGG.LocalLobbyMember(0),
                new ToGG.AiLobbyMember(),
            };

            new ToGG.Commander.CmdPlayState(setup);
        }

        void refreshDifficultyLevel()
        {
            //double levelPerc = DssLib.AiEconomyLevel[DssRef.storage.aiEconomyLevel];
            //int aggdiff = (int)DssRef.storage.aiAggressivity - (int)AiAggressivity.Medium;
            //levelPerc *= 1.0 + aggdiff * 0.5;

            //double bossTimeDiff = DssRef.storage.bossTimeSettings - BossTimeSettings.Normal;
            //levelPerc *= 1.0 - bossTimeDiff * 0.25;

            //double diplomacyDiff = DssRef.storage.diplomacyDifficulty - 1;
            //levelPerc *= 1.0 + diplomacyDiff * 0.5;

            //if (!DssRef.storage.honorGuard)
            //{
            //    levelPerc *= 1.25;
            //}

            //string Settings_TotalDifficulty = "Total Difficulty {0}%";
            if (difficultyLevelText != null)
            {
                difficultyLevelText.text.TextString = string.Format(DssRef.lang.Settings_TotalDifficulty, DssRef.difficulty.TotalDifficulty());
            }
        }

        public bool allowPauseProperty(int index, bool set, bool value)
        {
            if (set)
            {
                DssRef.difficulty.setting_allowPauseCommand = value;
                DssRef.storage.Save(null);
                refreshDifficultyLevel();
            }
            return DssRef.difficulty.setting_allowPauseCommand;
        }

        public bool centralGoldProperty(int index, bool set, bool value)
        {
            if (set)
            {
                DssRef.storage.centralGold = value;
                DssRef.storage.Save(null);
                refreshDifficultyLevel();
            }
            return DssRef.storage.centralGold;
        }

        public bool bossProperty(int index, bool set, bool value)
        {
            if (set)
            {
                DssRef.difficulty.runEvents = value;
                DssRef.storage.Save(null);
                refreshDifficultyLevel();
            }
            return DssRef.difficulty.runEvents;
        }

        public MapSize mapSizeProperty(bool set, MapSize value)
        {
            if (set && DssRef.storage.mapSize != value)
            {
                DssRef.storage.mapSize = value;
                DssRef.storage.Save(null);

                restartBackgroundLoading();
            }
            return DssRef.storage.mapSize;
        }

        void crashTest()
        {
            BlueScreen.ThreadException = new Exception("crash test");
        }

        void restartBackgroundLoading()
        {
            if (mapBackgroundLoading != null)
            {
                mapBackgroundLoading.Abort();
                mapBackgroundLoading = new MapBackgroundLoading(null);
            }
        }

        void selectInputMenu(int playerNumber, bool startGame, SaveStateMeta saveMeta)
        {
            var available = availableInput();
            GuiLayout layout = new GuiLayout(Ref.langOpt.InputSelect, menuSystem.menu);
            {
                foreach (var m in available)
                {
                    if (startGame)
                    {
                        if (m.IsController)
                        {
                            new GuiIconTextButton(SpriteName.ButtonSTART, HudLib.InputName(m.sourceType), null, new GuiAction2Arg<InputSource, SaveStateMeta>(selectController_startGame, m, saveMeta), false, layout);
                        }
                        else
                        {
                            new GuiTextButton(HudLib.InputName(m.sourceType), null, new GuiAction2Arg<InputSource, SaveStateMeta>(selectController_startGame, m, saveMeta), false, layout);
                        }
                    }
                    else
                    {
                        new GuiTextButton(HudLib.InputName(m.sourceType), null, new GuiAction2Arg<int, InputSource>(selectInputClick, playerNumber, m), false, layout);
                    }
                }
            }
            layout.End();
        }

        void inputWarningMenu()
        {
            GuiLayout layout = new GuiLayout(DssRef.lang.Lobby_WarningTitle, menuSystem.menu);
            {
                new GuiLabel(DssRef.lang.Lobby_PlayerWithoutInputWarning, layout);
                new GuiIconTextButton(SpriteName.MenuIconResume, Ref.langOpt.Hud_Back, null, mainMenu, false, layout);
                new GuiIconTextButton(SpriteName.MenuPixelIconPlay, DssRef.lang.Lobby_IgnoreWarning, null, startGame_nochecks, false, layout);
            }
            layout.End();
        }

        void selectInputClick(int playerNumber, InputSource source)
        {
            var playerData = DssRef.storage.localPlayers[playerNumber - 1];
            playerData.inputSource = source;
            DssRef.storage.checkPlayerDoublettes(playerNumber - 1);

            DssRef.storage.Save(null);
            refreshSplitScreen();
            mainMenu();
        }

        List<InputSource> availableInput()
        {
            var result = joinHandler.ListConneted();
            result.Insert(0, InputSource.DefaultPC);
            return result;
        }

        void nextScreenIndex(int playerNumber)
        {
            var ix = playerNumber - 1;
            var playerData = DssRef.storage.localPlayers[ix];
            var prevScreen = playerData.screenIndex;
            playerData.screenIndex++;
            if (playerData.screenIndex >= DssRef.storage.playerCount)
            {
                playerData.screenIndex = 0;
            }

            //Find player to swap with
            for (var i = 0; i < DssRef.storage.playerCount; i++)
            {
                if (i != ix &&
                    playerData.screenIndex == DssRef.storage.localPlayers[i].screenIndex)
                {
                    DssRef.storage.localPlayers[i].screenIndex = prevScreen;
                    break;
                }
            }

            refreshSplitScreen();
        }

        public bool verticalSplitProperty(int index, bool set, bool value)
        {
            if (set)
            {
                DssRef.storage.verticalScreenSplit = value;
                refreshSplitScreen();
                DssRef.storage.Save(null);
            }
            return DssRef.storage.verticalScreenSplit;
        }

        public bool autoSaveProperty(int index, bool set, bool value)
        {
            if (set)
            {
                DssRef.storage.autoSave = value;

                DssRef.storage.Save(null);
            }
            return DssRef.storage.autoSave;
        }

        public bool speed5Property(int index, bool set, bool value)
        {
            if (set)
            {
                DssRef.storage.speed5x = value;

                DssRef.storage.Save(null);
            }
            return DssRef.storage.speed5x;
        }
        public bool longerBuildQueueProperty(int index, bool set, bool value)
        {
            if (set)
            {
                DssRef.storage.longerBuildQueue = value;

                DssRef.storage.Save(null);
            }
            return DssRef.storage.longerBuildQueue;
        }

        public bool tutorialProperty(int index, bool set, bool value)
        {
            if (set)
            {
                DssRef.storage.runTutorial = value;

                DssRef.storage.Save(null);
            }
            return DssRef.storage.runTutorial;
        }

        public bool generateNewMapsProperty(int index, bool set, bool value)
        {
            if (set && DssRef.storage.generateNewMaps != value)
            {
                DssRef.storage.generateNewMaps = value;
                DssRef.storage.Save(null);
                restartBackgroundLoading();
            }
            return DssRef.storage.generateNewMaps;
        }

        void refreshSplitScreen()
        {
            findUnusedInput();

            checkScreenIndexes();

            splitScreenDisplay.Refresh(menuSystem.menu.area.Right);
        }

        void checkScreenIndexes()
        {
            List<int> expectedIndexes = new List<int>();
            for (int i = 0; i < DssRef.storage.playerCount; ++i)
            {
                expectedIndexes.Add(i);
            }

            for (int i = 0; i < DssRef.storage.playerCount; ++i)
            {
                expectedIndexes.Remove(DssRef.storage.localPlayers[i].screenIndex);
            }

            if (expectedIndexes.Count > 0)
            {
                //error, reset indexes
                for (int i = 0; i < GameStorage.MaxLocalPlayerCount; ++i)
                {
                    DssRef.storage.localPlayers[i].screenIndex = i;
                }
            }
        }
        void findUnusedInput()
        {
            //find unused input
            //Remove used
            List<InputSource> available = availableInput();

            for (int i = 0; i < DssRef.storage.playerCount; ++i)
            {
                for (int j = 0; j < available.Count; j++)
                {
                    if (DssRef.storage.localPlayers[i].inputSource.Equals(available[j]))
                    {
                        available.RemoveAt(j);
                        break;
                    }
                }
            }

            for (int i = 0; i < DssRef.storage.playerCount; ++i)
            {
                if (DssRef.storage.localPlayers[i].inputSource.sourceType == InputSourceType.Num_Non)
                {
                    if (available.Count > 0)
                    {
                        DssRef.storage.localPlayers[i].inputSource = arraylib.PullFirstMember(available);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        void localMultiplayerMenu()
        {
            GuiLayout layout = new GuiLayout(DssRef.lang.Lobby_LocalMultiplayerTitle, menuSystem.menu);
            {
                new GuiLabel(DssRef.lang.Lobby_LocalMultiplayerControllerRequired, layout);
                for (int i = 1; i <= GameStorage.MaxLocalPlayerCount; ++i)
                {
                    new GuiTextButton(i.ToString(), null, new GuiAction2Arg<int, bool>(setPlayerCount, i, true), false, layout);
                }
            }
            layout.End();
        }

        void setPlayerCount(int count, bool menuReturn)
        {
            DssRef.storage.playerCount = count;
            refreshSplitScreen();

            if (menuReturn)
            {
                mainMenu();
            }
        }

        void exitGame()
        {
            Ref.update.exitApplication = true;
        }

        void listProfiles(int playerNumber)
        {
            GuiLayout layout = new GuiLayout(DssRef.lang.Lobby_FlagSelectTitle, menuSystem.menu);
            {
                for (int i = 0; i < DssRef.storage.flagStorage.flagDesigns.Count; ++i)
                {
                    DssRef.storage.flagStorage.flagDesigns[i].Button(layout, new GuiAction2Arg<int, int>(selectProfileLink, playerNumber, i), false);
                }
            }
            layout.End();
        }

        void optionsMenu()
        {
            
            GuiLayout layout = new GuiLayout(Ref.langOpt.Options_title, menuSystem.menu);
            {
                new GuiImageButton(new Translation().sprite(Ref.gamesett.language), null, new GuiAction(selectLanguageMenu), true, layout);

                new GuiIconTextButton(SpriteName.Keyboard, DssRef.lang.Settings_ButtonMapping, null, new GuiAction(keyMappingMenu), true, layout);
                Ref.gamesett.optionsMenu(layout);
                new GuiCheckbox(DssRef.lang.GameMenu_AutoSave, null, autoSaveProperty, layout);
                new GuiCheckbox(DssRef.lang.Tutorial_MenuOption, null, tutorialProperty, layout);
                new GuiCheckbox(string.Format(DssRef.lang.GameMenu_UseSpeedX, LocalPlayer.MaxSpeedOption), null, speed5Property, layout);
                new GuiCheckbox(DssRef.lang.GameMenu_LongerBuildQueue, null, longerBuildQueueProperty, layout);
            }
            layout.End();

            layout.OnDelete += closingOptionsMenuEvent;
        }

        void closingOptionsMenuEvent()
        {
            Ref.gamesett.Save();
            if (Ref.gamesett.graphicsHasChanged)
            {
                Ref.gamesett.graphicsHasChanged = false;
                new LobbyState();
            }
        }

        void selectProfileLink(int playerNumber, int profile)
        {

            int ix = playerNumber - 1;
            LocalPlayerStorage playerData = DssRef.storage.localPlayers[ix];
            playerData.inputSource = InputSource.DefaultPC;
            DssRef.storage.checkPlayerDoublettes(playerNumber - 1);

            playerData.profile = profile;

            DssRef.storage.checkPlayerDoublettes(ix);

            DssRef.storage.Save(null);
            refreshSplitScreen();
            mainMenu();
        }

        void voxeleditor()
        {
            Ref.music.stop(false);
            XGuide.LocalHost.inputMap = new LootFest.Players.InputMap(XGuide.LocalHost.localPlayerIndex);
            XGuide.LocalHost.inputMap.xboxSetup();
            XGuide.LocalHost.inputMap.menuInput.xboxSetup(XGuide.LocalHost.localPlayerIndex);
            new LootFest.GameState.VoxelDesignState(XGuide.LocalHostIndex);
        }

        void mapFileGenerator()
        {
            new MapFileGeneratorState();
        }


        void openProfileEditor(int ProfileIx)
        {

            int p = -1;
            bool bController = Input.XInput.KeyIsDown(Buttons.A, ref p) || Input.XInput.KeyIsDown(Buttons.X, ref p);
            new PaintFlagState(ProfileIx, bController);
        }

        protected override void createDrawManager()
        {
            draw = new DSSWars.DrawMenu();
        }


        public override void Time_Update(float time)
        {
            //emitGlow();
            base.Time_Update(time);

            menuSystem.menu?.Update();

            topMenu.updateMouseInput();
            underMenu?.updateMouseInput();

            splitScreenDisplay.update();
            if (mapBackgroundLoading != null)
            {
                mapBackgroundLoading.Update();
                maploading.TextString = mapBackgroundLoading.ProgressString();
            }
            if (StartupSettings.AutoStartLevel && PlatformSettings.DevBuild)
            {
                startGame();
            }

            if (joinHandler.ConnectEvent())
            {
                DssRef.storage.checkConnected();
                refreshSplitScreen();
            }

            if (controllerStartGameUpdate)
            {
                int index;
                if (Input.XInput.KeyDownEvent_index(Buttons.Start, out index))
                {
                    selectController_startGame(new InputSource(InputSourceType.XController, index), null);
                }
            }

            if (VikingEngine.Input.Keyboard.Ctrl && VikingEngine.Input.Keyboard.KeyDownEvent(Keys.V))
            {
                voxeleditor();
            }

            if (VikingEngine.Input.Keyboard.Ctrl && VikingEngine.Input.Keyboard.KeyDownEvent(Keys.M))
            {
                openMapEditor();
            }

            if (Ref.music != null)
            {
                Ref.music.Update();
            }

            if (inKeyMapsMenu)
            {
                foreach (var key in availableKeyboardKeys)
                {
                    if (Input.Keyboard.KeyDownEvent(key))
                    {
                        listMapOptions_keyboardlink(key);
                    }
                }
            }

            //richmenu.updateMouseInput();
        }

        void emitGlow()
        {
            if (emitTimer.TimeOut_Event)
            {
                emitTimer.goalTimeSec = Ref.rnd.Float(0.01f, 0.2f);
                emitTimer.Reset();

                if (bgImage != null && DssRef.storage.playerCount == 1)
                {
                    Ref.draw.CurrentRenderLayer = 1;
                    float maxSpeed = bgImage.Ypos * 0.0001f;
                    Vector2 speed = Ref.rnd.vector2_cirkle(maxSpeed);
                    speed.Y -= maxSpeed * 0.5f;
                    var particle = new ParticleImage(SpriteName.WhiteArea, bgImage.Area.PercentToPosition(0.51f, 0.43f), VectorExt.V2(bgImage.Height * 0.01f), ImageLayers.Background7, speed);
                    particle.Color = Color.LightYellow;
                    particle.Opacity = 0.2f;
                    particle.particleData.setFadeout(400, 200);
                    Ref.draw.CurrentRenderLayer = 0;
                }
            }
        }


        void startGame()
        {
            if (DssRef.storage.playerCount == 1)
            {
                var availableList = availableInput();
                if (availableList.Count > 1)
                {
                    controllerStartGameUpdate = true;
                    selectInputMenu(1, true, null);
                }
                else
                {
                    selectController_startGame(availableList[0], null);
                }
                return;
            }
            else
            {
                //Check if a player is without input
                for (int i = 0; i < DssRef.storage.playerCount; ++i)
                {
                    if (DssRef.storage.localPlayers[i].inputSource.sourceType == InputSourceType.Num_Non)
                    {
                        inputWarningMenu();
                        return;
                    }
                }

            }
            startGame_nochecks();
        }

        void startGame_nochecks()
        {
            new StartGame(true, netLobby, null, mapBackgroundLoading);
        }

        void listSaves()
        {
            var saves = DssRef.storage.meta.listSaves();

            GuiLayout layout = new GuiLayout(DssRef.lang.GameMenu_LoadState, menuSystem.menu);
            {
                for (int i = 0; i < saves.Count; ++i)
                {
                    var save = saves[i];
                    new GuiTextButton(save.TitleString(), save.InfoString(), new GuiAction1Arg<SaveStateMeta>(continueFromSave, save), false, layout); 
                }

                new GuiSectionSeparator(layout);
                new GuiTextButton(DssRef.lang.Lobby_ExportSave, string.Format( DssRef.lang.Lobby_ExportSave_Description, SaveMeta.ImportSaveFolder), exportSave_listsaves, true, layout);
                new GuiTextButton(DssRef.lang.Lobby_ImportSave, null, importSaves, true, layout); 
            }
            layout.End();
        }

        void exportSave_listsaves()
        {
            var saves = DssRef.storage.meta.listSaves();

            GuiLayout layout = new GuiLayout(DssRef.lang.Lobby_ExportSave, menuSystem.menu);
            {
                for (int i = 0; i < saves.Count; ++i)
                {
                    var save = saves[i];
                    new GuiTextButton(save.TitleString(), save.InfoString(), new GuiAction1Arg<SaveStateMeta>(exportSaveSelected, save), false, layout);
                }
            }
            layout.End();
        }

        void exportSaveSelected(SaveStateMeta saveMeta)
        {
            SaveStateMeta exportPath = new SaveStateMeta();
            exportPath.storageSetup();
            exportPath.import = saveMeta.ExportString();

            var fileName = DataStreamHandler.SearchFilesInStorageDir(saveMeta.Path, false)[0];
            File.Copy(fileName, exportPath.Path.CompletePath(true), overwrite: true);

            mainMenu();
        }

        bool importSavesMenu = false;
        void importSaves()
        {
            var saves = DssRef.storage.meta.listSaves();
            importSavesMenu = true;

            GuiLayout layout = new GuiLayout(DssRef.lang.Lobby_ImportSave, menuSystem.menu);
            {
                new GuiLabel(DssRef.lang.Hud_Loading, layout);
            }
            layout.OnDelete += new Action(() => { importSavesMenu = false; });
            layout.End();

            new Timer.AsynchActionTrigger(loadSaveImportsList_async, true);
        }
        void loadSaveImportsList_async()
        {
            var list = DssRef.storage.meta.ListSaveImports();


            for (int i =0; i < list.Count; ++i)//each (var f in list)
            {
                list[i] = list[i].Split(Path.DirectorySeparatorChar).Last();
            }

            new Timer.Action1ArgTrigger<List<string>>(listImports, list);
        }

        void listImports(List<string> names)
        {
            if (importSavesMenu)
            {
                menuSystem.menu.PopLayout();

                GuiLayout layout = new GuiLayout(DssRef.lang.GameMenu_LoadState, menuSystem.menu);
                {
                    for (int i = 0; i < names.Count; ++i)
                    {
                        var save = names[i];
                        new GuiTextButton(LoadContent.CheckCharsSafety( save, LoadedFont.Regular), null, new GuiAction1Arg<string>(importSave, save), false, layout);
                    }

                    if (names.Count == 0)
                    {
                        new GuiLabel(DssRef.lang.Hud_EmptyList, layout);
                    }
                }
                layout.End();
            }
        }

        void importSave(string name)
        {
            SaveStateMeta meta = new SaveStateMeta();            
            meta.import = name;

            meta.loadImportMeta();
        }

        public void continueFromSave(SaveStateMeta saveMeta)//int listIndex)
        {
            //var save =DssRef.storage.meta.listSaves()[listIndex];

            if (saveMeta == null) 
            {
                return;
            }

            if (saveMeta.localPlayerCount == DssRef.storage.playerCount)
            {
                if (mapBackgroundLoading != null)
                {
                    mapBackgroundLoading.Abort();
                }
                //mapBackgroundLoading = new MapBackgroundLoading(save);

                var availableList = availableInput();
                if (availableList.Count > 1)
                {
                    controllerStartGameUpdate = true;
                    selectInputMenu(1, true, saveMeta);
                }
                else
                {
                    selectController_startGame(availableList[0], saveMeta);
                }
                //new StartGame(netLobby, save, mapBackgroundLoading);
            }
            else
            {
                setPlayerCount(saveMeta.localPlayerCount, false);
                GuiLayout layout = new GuiLayout(DssRef.lang.Lobby_WarningTitle, menuSystem.menu);
                {
                    new GuiLabel(string.Format( DssRef.lang.GameMenu_Load_PlayerCountError, saveMeta.localPlayerCount), layout);
                    new GuiIconTextButton(SpriteName.MenuIconResume, Ref.langOpt.Hud_OK, null, mainMenu, false, layout);
                }
                layout.End();                
            }
            
        }

        void selectController_startGame(InputSource inputSource, SaveStateMeta saveMeta)
        {
            var playerData = DssRef.storage.localPlayers[0];
            playerData.inputSource = inputSource;
            DssRef.storage.checkPlayerDoublettes(0);

            //SaveStateMeta save = null;
            //if (saveIndex >= 0)
            //{
            //    save =DssRef.storage.meta.listSaves()[saveIndex];
            //}
            new StartGame(true, netLobby, saveMeta, mapBackgroundLoading);
        }
        
    }

    class GamerStatus
    {
        public Graphics.TextS text;
        public bool joined = false;
        public Graphics.ImageAdvanced flagTexure;
    }

}
