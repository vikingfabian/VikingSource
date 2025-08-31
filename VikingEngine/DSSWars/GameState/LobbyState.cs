using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Xml.Linq;
using Valve.Steamworks;
using VikingEngine.DataStream;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.GameState.BattleLab;
using VikingEngine.DSSWars.GameState.MapEditor;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.Graphics;
//
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.LootFest;
using VikingEngine.LootFest.GO.WeaponAttack;
using VikingEngine.Network;
using VikingEngine.PJ;
using VikingEngine.PJ.CarBall;
using VikingEngine.PJ.Strategy;
using VikingEngine.Sound;
using VikingEngine.Timer;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars
{
    class LobbyState : AbsDssState
    {    
        Interface.MenuSystem menuSystem;
        MapBackgroundLoading mapBackgroundLoading;
        NetworkLobby netLobby = new NetworkLobby();
        GameTimer emitTimer = new GameTimer(0.1f);

        Texture2D bgTex;
        Graphics.ImageAdvanced bgImage = null;
        Interface.SplitScreenDisplay splitScreenDisplay = new Interface.SplitScreenDisplay();
        XInputJoinHandler joinHandler = new XInputJoinHandler();
        bool controllerStartGameUpdate = false;
        Graphics.TextG maploading;
        GuiLabel difficultyLevelText = null;

        StartGameMode startGameMode = StartGameMode.Play;
        InputActionType mappingFor;
        //bool inKeyMapsMenu = false;
        //List<Keys> availableKeyboardKeys;

        VectorRect underMenuArea;
        //RichMenu richmenu;
        const float MenuBgOpacity = 0.9f;
        const float ButtonTextTabbing = 0.15f;

        RichMenu topMenu, underMenu, reportsMenu;

        static readonly string LobbyAmbienceDir = Ambience.AmbienceDir + "lobby" + DataStream.FilePath.Dir;

        static readonly LoopingSoundData[] AmbienceSounds = new LoopingSoundData[]
           {
                new LoopingSoundData(LobbyAmbienceDir + "mystery_amb_v1_fear1_loop", 0.04f),
                new LoopingSoundData(LobbyAmbienceDir + "mystery_amb_v1_theme1_loop", 0.04f),
           };
        LoopingSound lobbyAmbienceLoop;

        const string UnderMenu_NewGame = "newgame";
        const string UnderMenu_ListEditors = "editors";
        const string UnderMenu_ListExtra = "extra";
        const string UnderMenu_ListMusic= "music list";
        const string UnderMenu_PlayerSetup = "playersett";
        const string UnderMenu_PlayerProfile = "playerprofile";
        const string UnderMenu_ListSaves = "saves";
        const string UnderMenu_ListSavesForExport = "exportsaves";
        const string UnderMenu_Options = "options";
        const string UnderMenu_DemoModes = "demo_modes";
        const string UnderMenu_Options_Language = "lang";


        const float MoreArrowTabbing = 0.9f;
        const float MoreArrowScale = 0.4f;
        SpriteName moreOptArrow = SpriteName.LfMenuMoreMenusArrow;
        SaveStateMeta loadGame = null;
        MessageGroup_Editor messages;
        public LobbyState(Texture2D bgTex, bool startLoadingMap = true)
            : base()
        {
            DssRef.storage.profileStorage.refreshProfiles();
            HudLib.Init();
            Ref.isPaused = false;
            Engine.Screen.SetupSplitScreen(1, true);
            if (startLoadingMap && !StartupSettings.BlockBackgroundLoading)
            {
                //if (PlatformSettings.STEAM_DEMO)
                //{
                //    DssRef.storage.mapSize = MapSize.Medium;
                //}
                mapBackgroundLoading = new MapBackgroundLoading(null as SaveStateMeta);
            }

            Ref.draw.ClrColor = new Color(11, 30, 34);

            menuSystem = new Interface.MenuSystem(new InputMap(Engine.XGuide.LocalHostIndex), Interface.MenuType.Lobby);
            DssRef.storage.checkConnected();
           
            Graphics.TextG version = new Graphics.TextG(LoadedFont.Console, Screen.SafeArea.RightBottom,
                Engine.Screen.TextSizeV2, new Align(Vector2.One), string.Format(HudLib.EngineVersionString, Engine.LoadContent.SteamVersion),
                Color.LightYellow, ImageLayers.Background2);

            maploading = new Graphics.TextG(LoadedFont.Console, Screen.SafeArea.LeftBottom,
                Engine.Screen.TextSizeV2, new Align(new Vector2(0, 1f)), "...",
                Color.DarkGray, ImageLayers.Background2);

            new Timer.AsynchActionTrigger(load_asynch, true);
            //new Timer.TimedAction0ArgTrigger(playMusic, 1000);


            if (Ref.lobby == null)
            {
                new NetLobby();
            }
            else
            {
                Ref.lobby.startSearchLobbies(true);
            }
            createMenuLayout();
            RestoreMenuStack();

            this.bgTex = bgTex;
            createBackground();
            messages = new MessageGroup_Editor();
#if DEBUG
            //new TimedAction0ArgTrigger(collectReports, 600);

#endif
        }
        void refreshUnderMenu()
        {
            switch (underMenu.menuStack.LastOrDefault())
            {
                case UnderMenu_NewGame:
                    loadGame = null;
                    newGameSettings2();
                    break;

                case UnderMenu_Options:
                    optionsMenu2();
                    break;

                case GameMenuSystem.UnderMenu_Options_Mouse:
                case GameMenuSystem.UnderMenu_Options_Keyboard:
                case GameMenuSystem.UnderMenu_Options_Keyboard_Key:
                    GameMenuSystem.refreshPage(underMenu, true);
                    //    GameMenuSystem.mouseOptions(underMenu);
                    //    break;

                    //case GameMenuSystem.UnderMenu_Options_Keyboard:
                    //    GameMenuSystem.keyboardOptions(underMenu);
                    //    break;

                    //case GameMenuSystem.UnderMenu_Options_Keyboard_Key:
                    //    GameMenuSystem.listMapOptions(underMenu);
                    break;

                case UnderMenu_Options_Language:
                    selectLanguageMenu();
                    break;

                case UnderMenu_ListSaves:
                    listSaves2();
                    break;
                case UnderMenu_ListSavesForExport:
                    exportSave_listsaves2();
                    break;

                case UnderMenu_DemoModes:
                    demoModesPage();
                    break;


                case UnderMenu_ListEditors:
                    {
                        var playerData = DssRef.storage.localPlayers.First();
                        DssRef.storage.profileStorage.selectedIx = playerData.profileIndex;

                        //var profile = DssRef.storage.profileStorage.Selected();
                        DssRef.storage.flagStorage.selectedIx = playerData.Profile().flag.StorageIndex;//profile.flag.StorageIndex;

                        RichBoxContent content = new RichBoxContent();

                        content.h1(DssRef.lang.Lobby_Category_Editor, HudLib.TitleColor_Head);

                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary, HudLib.AddLockOnDemo(new List<AbsRichBoxMember>() {
                            new RbImage(SpriteName.WarsMapIcon, 0.9f),
                            new RbSpace(),
                            new RbText(DssRef.lang.Lobby_Editor_MapEditor) }),
                            new RbAction(openMapEditor), null, !PlatformSettings.STEAM_DEMO));

                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary, HudLib.AddLockOnDemo(new List<AbsRichBoxMember>() {
                            new RbImage(SpriteName.VoxelEditorColorCube, 0.9f),
                            new RbSpace(),
                            new RbText(DssRef.lang.Lobby_Editor_VoxelEditor) }),
                            new RbAction(voxeleditor), new RbTooltip_Text(DssRef.lang.VoxelEditor_Description), !PlatformSettings.STEAM_DEMO));

                        content.newParagraph();

                        
                        //listAndEditProfile(content, 1, playerData, true);
                                                
                        //content.newLine();
                        listAndEditFlag(content, playerData, true);

                        content.newLine();
                        listAndEditCharacter(content, 0, true);

                        underMenu.Refresh(content);
                    }
                    break;

                case UnderMenu_ListExtra:
                    {

                        RichBoxContent content = new RichBoxContent();

                        content.h1(DssRef.lang.Lobby_Category_ExtraModes, HudLib.TitleColor_Head);

                        content.newLine();

                        content.Add(new ArtButton(RbButtonStyle.Primary, HudLib.AddLockOnDemo(new List<AbsRichBoxMember>() { new RbText(DssRef.lang.Lobby_Mode_BattleLab) }),
                            new RbAction(startBattleLab), new RbTooltip_Text(DssRef.lang.Lobby_Mode_BattleLab_Description), !PlatformSettings.STEAM_DEMO));

                        content.newLine();

                        content.Add(new ArtButton(RbButtonStyle.Primary, HudLib.AddLockOnDemo(new List<AbsRichBoxMember>() { new RbText(DssRef.lang.Lobby_Mode_Commander) }),
                            new RbAction(extra_PlayCommanderVersus), new RbTooltip_Text(DssRef.lang.Lobby_Mode_Commander_Description), !PlatformSettings.STEAM_DEMO));

                        content.newLine();

                        content.Add(new ArtButton(RbButtonStyle.Primary, HudLib.AddLockOnDemo(new List<AbsRichBoxMember>() { new RbText(DssRef.lang.Lobby_MusicPlayList) }),
                            new RbAction2Arg<string, StackOption>(openUnderMenu, UnderMenu_ListMusic, StackOption.ClearStack), null, !PlatformSettings.STEAM_DEMO));

                        content.newParagraph();
                        content.Add(new RbButton(HudLib.AddLockOnDemo(new List<AbsRichBoxMember>() { new RbText("File debug lab") }),
                            new RbAction(fileLab), null));

#if DEBUG
                        if (Ref.steam.isInitialized)
                        {
                            content.newLine();
                            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("Initialize steam stats") }, new RbAction(Ref.steam.stats.initializeAllStatsOnSteam)));
                            content.newLine();
                            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("Load global steam stats") }, new RbAction(Ref.steam.stats.beginRequestGlobalStats)));
                        }
#endif

                        //                    if (Ref.steam.statsInitialized)
                        //                    {
                        //                        new GuiTextButton("Initialize steam stats", null, Ref.steam.stats.initializeAllStatsOnSteam, false, layout);
                        //                        new GuiTextButton("Load global steam stats", null, Ref.steam.stats.beginRequestGlobalStats, false, layout);
                        //                    }

                        underMenu.Refresh(content);
                    }
                    break;

                case UnderMenu_PlayerSetup:
                    {
                        RichBoxContent content = new RichBoxContent();
                        if (underMenu.menuStack.Count > 1)
                        {
                            HudLib.returnButton(content, underMenu, true, null);
                        }
                        //bool startTutorialDisplay = false;

                        var title = content.h1(string.Empty, HudLib.TitleColor_Head);
                        content.newLine();

                        bool startAvailable = checkAllPlayersHasControls();

                        switch (startGameMode)
                        {
                            case StartGameMode.Play:
                                {
                                    title.text = PlatformSettings.STEAM_DEMO ? DssRef.lang.LobbyDemoMode_Demo : DssRef.lang.Settings_NewGame;

                                    var start = new ArtButton(RbButtonStyle.Primary,
                                      new List<AbsRichBoxMember> { new RbBeginTitle(), new RbImage(SpriteName.WarsHudIconStart), new RbSpace(), new RbText(DssRef.lang.Lobby_Start) },
                                      new RbAction(startGame), null, startAvailable);
                                    content.Add(start);
                                }
                                break;

                            case StartGameMode.Tutorial:
                                {
                                    title.text = DssRef.lang.Lobby_Tutorial;

                                    var startlong = new ArtButton(RbButtonStyle.Primary,
                                      new List<AbsRichBoxMember> { new RbBeginTitle(), new RbImage(SpriteName.WarsHudIconStart), new RbSpace(), new RbText(DssRef.lang.Lobby_Tutorial) },
                                      new RbAction1Arg<bool>(startTutorial, false), null, startAvailable);
                                    content.Add(startlong);
                                    //content.newLine();
                                    //var startshort = new ArtButton(RbButtonStyle.Secondary,
                                    //  new List<AbsRichBoxMember> { new RbBeginTitle(), new RbImage(SpriteName.WarsHudIconStart), new RbSpace(), new RbText(DssRef.lang.LobbyDemoMode_ShortTutorial) },
                                    //  new RbAction1Arg<bool>(startTutorial, true), null, startAvailable);
                                    ////start.fillWidth = false;
                                    //content.Add(startshort);
                                }
                                break;
                            case StartGameMode.BattleTrials:
                                {
                                    title.text = DssRef.lang.BattleTrials_Title;

                                    content.newLine();
                                    var start = new ArtButton(RbButtonStyle.Primary,
                                          new List<AbsRichBoxMember> { new RbBeginTitle(), new RbImage(SpriteName.WarsHudIconStart), new RbSpace(), new RbText(DssRef.lang.Lobby_Start) },
                                          new RbAction(startTrial), null, startAvailable);
                                    content.Add(start);
                                }
                                break;
                        }

                        if (!startAvailable)
                        {
                            content.newLine();
                            content.Add(new RbText(DssRef.lang.Lobby_PlayerWithoutInputWarning, HudLib.NotAvailableColor));
                        }

                        content.newParagraph();

                        playerSetupToMenu(content);

                        const bool ViewMultiplayer = true;
                        if (ViewMultiplayer)
                        {
                            content.newParagraph();

                            DropDownBuilder mpOptions = new DropDownBuilder("local_mp");
                            {
                                for (int i = 1; i <= GameStorage.MaxLocalPlayerCount; ++i)
                                {
                                    mpOptions.AddOption(i.ToString(), i == DssRef.storage.playerCount, i == 1, new RbAction2Arg<int, bool>(setPlayerCount, i, true), null);
                                }
                                mpOptions.injectAfter = new List<AbsRichBoxMember>(2) { new RbSpace() };
                                HudLib.InfoButton(mpOptions.injectAfter, new RbTooltip_Text(DssRef.lang.Lobby_LocalMultiplayerControllerRequired));
                                mpOptions.Build(content, SpriteName.NO_IMAGE, DssRef.lang.Lobby_LocalMultiplayerEdit, underMenu);
                            }
                        }

                        underMenu.Refresh(content);

                    }
                    break;

                case UnderMenu_PlayerProfile:
                    {
                        var profile = DssRef.storage.profileStorage.Selected();
                        DssRef.storage.flagStorage.selectedIx = profile.flag.StorageIndex;
                        DssRef.storage.characterStorage.selectedIx = profile.character.StorageIndex;

                        RichBoxContent content = new RichBoxContent();
                        HudLib.returnButton(content, underMenu, true, null);

                        content.h1(DssRef.lang.Lobby_PlayerProfileEdit, HudLib.TitleColor_Head);

                        content.newLine();
                        HudLib.Label(content, DssRef.lang.HUD_DisplayName);
                        content.newLine();
                        var editButton = new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbImage(SpriteName.InterfaceTextInput) },
                                new RbAction(beginEditPlayerName), null);
                            content.Add(editButton);
                            content.space();
                        
                        content.Add(new RbText(profile.DisplayName(), Color.LightYellow));
                        
                        content.newParagraph();

                        listAndEditFlag(content, DssRef.storage.localPlayers[DssRef.storage.selectedPlayer], false);

                        listAndEditCharacter(content, DssRef.storage.profileStorage.selectedIx, false);

                        underMenu.Refresh(content);
                    }
                    break;

                case UnderMenu_ListMusic:
                    {

                        RichBoxContent content = new RichBoxContent();

                        List<Sound.SongData> list = Music.PlayList();
                        foreach (var m in list)
                        {
                            content.newLine();
                            content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> {
                                new RbImage(SpriteName.MenuPixelIconMusicVol),
                                new RbSpace(),
                                new RbText(m.name) },
                                new RbAction1Arg<Sound.SongData>(Ref.music.PlaySong, m)));
                        }

                        content.newParagraph();

                        List<Sound.SongData> other = Music.OtherSongs();
                        foreach (var m in other)
                        {
                            content.newLine();
                            content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> {
                                new RbImage(SpriteName.MenuPixelIconMusicVol),
                                new RbSpace(),
                                new RbText(m.name) },
                                new RbAction1Arg<Sound.SongData>(Ref.music.PlaySong, m)));
                        }

                        underMenu.Refresh(content);
                    }
                    break;
            }
        }

        void beginEditPlayerName()
        {
            var profile = DssRef.storage.profileStorage.Selected();
            new TextInput(profile.DisplayName(), PlayerNameEditEvent, null);
        }
        void PlayerNameEditEvent(string result, object tag)
        {
            var profile = DssRef.storage.profileStorage.Selected();
            { 
                profile.name = result;
            }
            DssRef.storage.profileStorage.SetSelected(profile);
            DssRef.storage.profileStorage.SaveSelected();
            underMenu.needRefresh = true;
        }

        public void playOnCustomMap(MapBackgroundLoading map)
        {
            mapBackgroundLoading = map;
            openUnderMenu(UnderMenu_NewGame, StackOption.ClearStack);
        }

        public void cancelCustomMap()
        {
            restartBackgroundLoading();
            openUnderMenu(UnderMenu_NewGame, StackOption.ReplaceLast);
        }

        void createMenuLayout()
        {
            VectorRect leftArea = Screen.SafeArea;
            leftArea.Width = Screen.IconSize * 5;
            leftArea.Round();

            VectorRect titleArea = leftArea;
            titleArea.Height = leftArea.Width * 0.6f;
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

                VectorRect recolorArea = titleBgArea;
                recolorArea.AddRadius(-2);
                Image recolor = new Image(SpriteName.WhiteArea, recolorArea.Position, recolorArea.Size, ImageLayers.Lay7);
                recolor.Color = new Color(100, 125, 134, 150);
            }

            //new TextG(LoadedFont.Bold, titleArea.Center, Screen.TextTitleScale * 2f, Align.CenterAll, "DSS 2", HudLib.TitleColor_Head, ImageLayers.Lay4);
            var logo = new Image(SpriteName.DSS2MainMenu, titleArea.PercentToPosition(new Vector2(0.5f, 0.4f)), VectorExt.Normalize( SpriteSheet.DSS2Logo.Vec, out _) * titleArea.Height * 2f, ImageLayers.Lay4, true);
            logo.Opacity = 0.7f;
            //logo.Color = HudLib.TitleColor_Head;

            VectorRect menuBgArea = menuArea;
            {
                menuBgArea.X = titleBgArea.X;
                menuBgArea.SetRight(titleBgArea.Right, true);
                menuBgArea.SetBottom(Engine.Screen.Area.Bottom + BgOffScreenLength, true);
                NineSplitAreaTexture menuBg = new NineSplitAreaTexture(new NineSplitSettings(SpriteName.WarsHudScrollerBg, 1, 6, 1f, true, true), menuBgArea, ImageLayers.Lay9);

                VectorRect recolorArea = menuBgArea;
                recolorArea.AddRadius(-3);
                Image recolor = new Image(SpriteName.WhiteArea, recolorArea.Position, recolorArea.Size, ImageLayers.Lay7);
                recolor.Color = new Color(100, 125, 134, 50);
                //menuBg.SetOpacity(MenuBgOpacity);
            }

            VectorRect menuContentArea = menuArea;
            menuContentArea.AddToTopSide(-Engine.Screen.IconSize *0.5f);
            //menuContentArea.AddRadius(-8);

            topMenu = new RichMenu(HudLib.RbSettings, menuContentArea, new Vector2(8), RichMenu.DefaultRenderEdge, ImageLayers.Lay4, new PlayerData(PlayerData.AllPlayers));

            topMenu.Refresh(new RichBoxContent() { new RbNewLine() });
            mainMenu2();

            underMenuArea = new VectorRect(menuBgArea.Right + Screen.BorderWidth, menuContentArea.Y, Screen.IconSize * 6, menuContentArea.Height);            
        }

        public void openUnderMenu(string menuName, StackOption stack)
        {
            if (underMenu == null)
            {
                underMenu = new RichMenu(HudLib.RbSettings, underMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, ImageLayers.Lay4, new PlayerData(PlayerData.AllPlayers));
                underMenu.addBackground(new NineSplitSettings(SpriteName.WarsHudScrollerBg, 1, 6, 1f, true, true), ImageLayers.Lay9).SetOpacity(MenuBgOpacity);
            }
            else
            {
                closingOptionsMenuEvent();
            }

            underMenu.OpenMenu(menuName, stack);
        }


        void collectReports()
        {
            createReportMenu();
            if (Ref.steam.isInitialized)
            {
                var report = new DebugExtensions.DownloadSteamCrashReports(false, reportContent);
            }
            else
            {
                RichBoxContent content = new RichBoxContent();
                content.text("Steam not initialized");
                reportsMenu.OpenMenu(content, string.Empty);
            }

            void createReportMenu()
            {
                var area = Engine.Screen.SafeArea;
                area.X = underMenuArea.X;
                area.SetRight(Engine.Screen.SafeArea.Right, true);
                reportsMenu = new RichMenu(HudLib.RbSettings, area, new Vector2(8), RichMenu.DefaultRenderEdge, ImageLayers.Background0, new PlayerData(PlayerData.AllPlayers));
                reportsMenu.addBackground(new NineSplitSettings(SpriteName.WarsHudScrollerBg, 1, 6, 1f, true, true), ImageLayers.Background3);
            }

            void reportContent(DownloadSteamCrashReports reports)
            {
                RichBoxContent content = new RichBoxContent();
                foreach (var report in reports.reports)
                {
                    bool first = true;
                    foreach (var line in report)
                    {
                        if (first)
                        {
                            first = false;
                            content.h2(line, HudLib.TitleColor_Head);
                        }
                        else
                        {
                            content.text(line);
                        }
                    }

                    content.Add(new RbSeperationLine());
                }

                reportsMenu.OpenMenu(content, string.Empty);
            }
        }
              

        

        void closingOptionsMenuEvent()
        {
            if (Ref.gamesett.settingsHasChanged)
            {
                Ref.gamesett.settingsHasChanged = false;
                Ref.gamesett.Save();
            }
            if (Ref.gamesett.graphicsHasChanged)
            {
                Ref.gamesett.graphicsHasChanged = false;
                new LobbyState(bgTex);
            }
        }

        public static Texture2D LoadBg()
        {
            var bgTex = Ref.main.Content.Load<Texture2D>(DssLib.ContentDir + "darkforest_bg");
            return bgTex;
        }

        void load_asynch()
        {
            //bgTex = Ref.main.Content.Load<Texture2D>(DssLib.ContentDir + "darkforest_bg");
            //new Timer.Action0ArgTrigger(loadingComplete);

            LoopingSound sound = new LoopingSound();
            sound.setVolume(0);
            sound.Load(arraylib.RandomListMember(AmbienceSounds));

            lobbyAmbienceLoop = sound;
        }

        void createBackground()
        {
            float w = Engine.Screen.SafeArea.Width;
            float h = w / bgTex.Width * bgTex.Height;
            float x = Engine.Screen.Area.Right - w;
            float y = Screen.CenterScreen.Y - h * 0.5f;

            bgImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                new Vector2(x, y), new Vector2(w, h), ImageLayers.Background5, false);
            bgImage.Texture = bgTex;
            bgImage.SetFullTextureSource();
            bgImage.Color = ColorExt.GrayScale(0.8f);
            bgImage.Opacity = 0.8f;

            //Vector2 promoworkerSz = new Vector2(9, 6) * new Vector2(h * 0.02f);

            //var worker1 = new Graphics.Image(SpriteName.warsWorkerPromoCannon, VectorExt.AddY(Engine.Screen.Area.PercentToPosition(0.7f, 1f), -promoworkerSz.Y * 0.9f), promoworkerSz, ImageLayers.Background5);
            //worker1.LayerAbove(bgImage);
        }

        void playMusic()
        {
            if (Ref.music != null)
            {
                Ref.music.PlaySong(Data.Music.Intro, false);
            }
        }

        void testCrash()
        {
            throw new Exception("Test crash");
            //Ref.sentry.debugMessage();
        }

        // new HUD.GuiTextButton(">>download crash reports", null, downloadCrashReports, false, layout);
        void mainMenu2()
        {
           
            
            RichBoxContent content = new RichBoxContent();
#if DEBUG

            if (StartupSettings.CheatActive)
            {
                content.text("! debug cheats !");
            }
            content.Button("start", new RbAction(startGame), null, true);
            content.Button("map editor", new RbAction(openMapEditor), null, true);
            content.Button("map2", new RbAction(map2), null, true);

            content.Button("battle lab", new RbAction(startBattleLab), null, true);
            content.Button("trial", new RbAction(startTrial), null, true);
            content.Button("cresh reports", new RbAction(Ref.steam.downloadCrashReports), null, true);
            if (Ref.steam.isInitialized)
            {
                content.Button("wish", new RbAction(() =>
                    {
                        SteamAPI.SteamFriends().ActivateGameOverlayToStore(
                        3585100,
                        EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
                    }
                ), null, true);
            }

            content.Button("crash", new RbAction(testCrash), null, true);

            content.Button("Character creator", new RbAction(characterCreator), null, true);
            content.Button("Shader lab", new RbAction(shaderLab), null, true);

#endif

#if DEMO
            //{
            //    content.newLine();

            //    var moreArrow = new RbImage(moreOptArrow, MoreArrowScale);
            //    moreArrow.color = HudLib.MenuMoreOptionsArrowCol;

            //    var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
            //        new RbBeginTitle(),
            //        new RbImage(SpriteName.WarsHudIconTutorial),
            //        new RbTab(ButtonTextTabbing),
            //        new RbText(DssRef.lang.Lobby_Tutorial),
            //        new RbTab(MoreArrowTabbing),
            //        moreArrow,
            //    },
            //    new RbAction1Arg<bool>(beginDemoTutorial,  true), null);
            //    btn.fillWidth = true;
            //    content.Add(btn);
            //}


            {
                content.newLine();

                var moreArrow = new RbImage(moreOptArrow, MoreArrowScale);
                moreArrow.color = HudLib.MenuMoreOptionsArrowCol;

                var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbBeginTitle(),
                    new RbImage(SpriteName.WarsHudIconStart),
                    new RbTab(ButtonTextTabbing),
                    new RbText(DssRef.lang.LobbyDemoMode_Demo),
                    new RbTab(MoreArrowTabbing),
                    moreArrow,
                },
                new RbAction2Arg<string, StackOption>(openUnderMenu, UnderMenu_DemoModes, StackOption.ClearStack),
                //new RbAction(beginDemo), 
                new RbTooltip_Text(string.Format(DssRef.lang.Demo_Description, 90)));
                btn.fillWidth = true;
                content.Add(btn);
            }
            //content.newLine();
           
            //content.Add(new RbButton(new List<AbsRichBoxMember>() { new RbText(DssRef.lang.Lobby_Mode_BattleLab) },
            //                new RbAction(startBattleLab), new RbTooltip_Text(DssRef.lang.Lobby_Mode_BattleLab_Description)));

            HudLib.WishListButton(content);

            if (false)
            {
                content.newParagraph();

                var moreArrow = new RbImage(moreOptArrow, MoreArrowScale);
                moreArrow.color = HudLib.MenuMoreOptionsArrowCol;

                var btn = new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                    new RbBeginTitle(),
                    new RbImage(SpriteName.WarsHudIconStart),
                    new RbTab(ButtonTextTabbing),
                    new RbText(DssRef.lang.BattleTrials_Title),
                    new RbTab(MoreArrowTabbing),
                    moreArrow,
                },
                new RbAction(beginTrialDemo), new RbTooltip_Text(DssRef.lang.BattleTrials_Description));
                btn.fillWidth = true;
                content.Add(btn);
            }
            //{
            //    content.newParagraph();

            //    var moreArrow = new RbImage(moreOptArrow, MoreArrowScale);
            //    moreArrow.color = HudLib.MenuMoreOptionsArrowCol;

            //    var btn = new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
            //        new RbBeginTitle(),
            //        new RbImage(SpriteName.WarsHudIconTutorial),
            //        new RbTab(ButtonTextTabbing),
            //        new RbText(DssRef.lang.LobbyDemoMode_LongTutorial),
            //        new RbTab(MoreArrowTabbing),
            //        moreArrow,
            //    },
            //    new RbAction1Arg<bool>(beginDemoTutorial, false), null);
            //    btn.fillWidth = true;
            //    content.Add(btn);
            //}
#else
            var saves = DssRef.storage.meta.listSaves();
            if (arraylib.HasMembers(saves))
            {
                content.newLine();

                var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbBeginTitle(),
                    new RbImage(SpriteName.WarsHudIconOpen),
                    new RbTab(ButtonTextTabbing),
                    new RbText(DssRef.lang.GameMenu_ContinueFromSave),
                    new RbTab(MoreArrowTabbing),
                },
                new RbAction1Arg<SaveStateMeta>(continueFromSave, saves[0]), new RbTooltip_Text(saves[0].InfoString()));
                btn.fillWidth = true;
                content.Add(btn);
            }
            {
                content.newLine();

                var moreArrow = new RbImage(moreOptArrow, MoreArrowScale);
                moreArrow.color = HudLib.MenuMoreOptionsArrowCol;

                var btn = new ArtButton(arraylib.HasMembers(saves)  ? RbButtonStyle.Secondary: RbButtonStyle.Primary, new List<AbsRichBoxMember> { 
                    new RbBeginTitle(), 
                    new RbImage(SpriteName.WarsHudIconAdd), 
                    new RbTab(ButtonTextTabbing), 
                    new RbText(DssRef.lang.Settings_NewGame),
                    new RbTab(MoreArrowTabbing),
                    moreArrow,
                },
                new RbAction2Arg<string, StackOption>(openUnderMenu, UnderMenu_NewGame, StackOption.ClearStack), null);
                btn.fillWidth = true;
                content.Add(btn);
            }
            {
                content.newLine();

                var moreArrow = new RbImage(moreOptArrow, MoreArrowScale);
                moreArrow.color = HudLib.MenuMoreOptionsArrowCol;

                var btn = new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconOpen), new RbTab(ButtonTextTabbing), new RbText(DssRef.lang.GameMenu_LoadState),
                    new RbTab(MoreArrowTabbing),
                    moreArrow,
                }, new RbAction2Arg<string, StackOption>(openUnderMenu, UnderMenu_ListSaves, StackOption.ClearStack), null);
                btn.fillWidth = true;
                content.Add(btn);
            }
           
#endif
            content.newParagraph();
            {
                var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconSettings) },
                     new RbAction2Arg<string, StackOption>(openUnderMenu, UnderMenu_Options, StackOption.ClearStack), new RbTooltip_Text(DssRef.lang.Lobby_Category_Options));
                content.Add(btn);
            }
            {
                var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconEditor) }, 
                    new RbAction2Arg<string, StackOption>(openUnderMenu, UnderMenu_ListEditors, StackOption.ClearStack), new RbTooltip_Text(DssRef.lang.Lobby_Category_Editor));
                content.Add(btn);
            }
            {
                var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconExtraModes) },
                    new RbAction2Arg<string, StackOption>(openUnderMenu, UnderMenu_ListExtra , StackOption.ClearStack), new RbTooltip_Text(DssRef.lang.Lobby_Category_ExtraModes));
                content.Add(btn);
            }

            //EXIT
            content.Add(new RbNewLine_AtHeight(topMenu.richboxArea.Height - topMenu.richBox.lineSpacing * 2f));
            {
                content.newParagraph();
                var btn = new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconExit, 0.7f), new RbTab(ButtonTextTabbing), new RbText(DssRef.lang.Lobby_ExitGame) }, new RbAction(exitGame, RbSoundType.Back), null);
                //btn.fillWidth = true;
                content.Add(btn);
            }

            topMenu.Refresh(content);
        }

        void demoModesPage()
        {
            RichBoxContent content = new RichBoxContent();

            content.h1(DssRef.lang.Settings_AdvancedControls, HudLib.TitleColor_Head);
            content.space();
            HudLib.InfoButton(content, new RbTooltip(tooltip, false));
            //content.text(DssRef.lang.Settings_AdvancedControls_Description, HudLib.InfoYellow_Light);
            modeButtons(false);

            content.Add(new RbSeperationLine());

            content.h1(DssRef.lang.Settings_CasualControls, HudLib.TitleColor_Head);
            content.space();
            HudLib.InfoButton(content, new RbTooltip(tooltip, true));
            //content.text(DssRef.lang.Settings_CasualControls_Description, HudLib.InfoYellow_Light);
            modeButtons(true);

            void modeButtons(bool casual)
            {
                if (!casual)
                {
                    content.newLine();

                    var moreArrow = new RbImage(moreOptArrow, MoreArrowScale);
                    moreArrow.color = HudLib.MenuMoreOptionsArrowCol;

                    var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            new RbBeginTitle(),
                            new RbImage(SpriteName.WarsHudIconTutorial),
                            new RbTab(ButtonTextTabbing),
                            new RbText(DssRef.lang.Lobby_Tutorial),
                            new RbTab(MoreArrowTabbing),
                            moreArrow,
                        },
                    new RbAction2Arg<bool, bool>(beginDemoLink, casual, true), null);
                    btn.fillWidth = true;
                    content.Add(btn);
                }


                {
                    content.newLine();

                    var moreArrow = new RbImage(moreOptArrow, MoreArrowScale);
                    moreArrow.color = HudLib.MenuMoreOptionsArrowCol;

                    var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                            new RbBeginTitle(),
                            new RbImage(SpriteName.WarsHudIconStart),
                            new RbTab(ButtonTextTabbing),
                            new RbText(DssRef.lang.LobbyDemoMode_Demo),
                            new RbTab(MoreArrowTabbing),
                            moreArrow,
                        },
                    new RbAction2Arg<bool, bool>(beginDemoLink, casual, false),
                    //new RbAction(beginDemo), 
                    new RbTooltip_Text(string.Format(DssRef.lang.Demo_Description, 90)));
                    btn.fillWidth = true;
                    content.Add(btn);
                }
                
            }

            underMenu.Refresh(content);
        
        
            void tooltip(RichBoxContent content, object tag)
            {
                bool casual = (bool)tag;

                content.text(casual? DssRef.lang.Settings_CasualControls_Description: DssRef.lang.Settings_AdvancedControls_Description, HudLib.InfoYellow_Light);
            }
        }


        void openPlayerSetupForMode(StartGameMode mode)
        {
            this.startGameMode = mode;
            openUnderMenu(UnderMenu_PlayerSetup, StackOption.Stack);
        }

        void beginDemoLink(bool casual, bool tutorial)
        {
            //DssRef.storage.runTutorial_1short_2normal = 2;//bShort ? 1 : 2;
            //openUnderMenu(UnderMenu_PlayerSetup, StackOption.Stack);

            if (!casual)
            {
                if (tutorial)
                {
                    DssRef.storage.runTutorial_1short_2normal = 2;
                }
                else
                {
                    DssRef.storage.runTutorial_1short_2normal = 0;
                    SaveStateMeta meta = new SaveStateMeta();
                    meta.playmap = "demomap5";

                    loadGame = meta;
                }
            }
            else
            {
                //Casual
                DssRef.storage.runTutorial_1short_2normal = 2;
            }

            //DssRef.storage.runTutorial_1short_2normal = (casual || tutorial)? 2 : 0;
            DssRef.storage.profileStorage.SetCasualToAll(casual);

            openPlayerSetupForMode( tutorial? StartGameMode.Tutorial : StartGameMode.Play);
        }

        void beginDemo()
        {
            DssRef.storage.runTutorial_1short_2normal = 0;

            SaveStateMeta meta = new SaveStateMeta();
            meta.playmap = "demomap5";

            loadGame = meta;
            //openUnderMenu(UnderMenu_PlayerSetup, StackOption.Stack);
            openPlayerSetupForMode(StartGameMode.Play);
        }
        void beginTrialDemo()
        {
            //DssRef.storage.runTutorial_1short_2normal = 0;

            //SaveStateMeta meta = new SaveStateMeta();
            //meta.playmap = "demomap2";

            //loadGame = meta;
            //openUnderMenu(UnderMenu_PlayerSetup, StackOption.Stack);
            openPlayerSetupForMode(StartGameMode.BattleTrials);
        }
        void openMapEditor()
        {
            mapBackgroundLoading?.Abort();
            new MapEditor_GeneratorScene();
        }

        void startBattleLab()
        {
            //mapBackgroundLoading?.Abort();
            new StartBattleLab(mapBackgroundLoading);
        }
        void startTrial()
        {
            mapBackgroundLoading?.Abort();
            new StartBattleLab();
        }
        //        void mainMenu()
        //        {
        //            controllerStartGameUpdate = false;
        //            menuSystem.openMenu();
        //            menuSystem.menu.PopAllLayouts();

        //            var saves = DssRef.storage.meta.listSaves();

        //            GuiLayout layout = new GuiLayout(string.Empty, menuSystem.menu);
        //            {
        //                if (StartupSettings.CheatActive)
        //                {
        //                    new GuiLabel("! debug cheats !", layout);
        //                }
        //#if DEBUG
        //                new GuiLargeTextButton(DssRef.lang.Lobby_Start, null, new GuiAction(startGame), false, layout);
        //#endif

        //                if (arraylib.HasMembers(saves))
        //                {
        //                    new GuiTextButton(DssRef.lang.GameMenu_ContinueFromSave, saves[0].InfoString(), new GuiAction1Arg<SaveStateMeta>(continueFromSave, saves[0]), false, layout);
        //                }

        //                new GuiLargeTextButton(DssRef.lang.Settings_NewGame, null, new GuiAction(newGameSettings) /*new GuiAction(startGame)*/, true, layout);

        //                if (arraylib.HasMembers(saves))
        //                {
        //                    new GuiTextButton(DssRef.lang.GameMenu_LoadState, null, listSaves, true, layout);
        //                }

        //                new GuiTextButton(string.Format(DssRef.lang.Lobby_LocalMultiplayerEdit, DssRef.storage.playerCount),
        //                    null, localMultiplayerMenu, true, layout);

        //                for (int playerNum = 1; playerNum <= DssRef.storage.playerCount; ++playerNum)
        //                {
        //                    var playerData = DssRef.storage.localPlayers[playerNum - 1];
        //                    if (DssRef.storage.playerCount > 1)
        //                    {
        //                        new GuiLabel(string.Format(DssRef.lang.Player_DefaultName, playerNum), layout);
        //                        new GuiTextButton(DssRef.lang.Lobby_NextScreen, null, new GuiAction1Arg<int>(nextScreenIndex, playerNum), false, layout);
        //                    }
        //                    DssRef.storage.flagStorage.flagDesigns[playerData.flagDesignIndex].Button(layout, new GuiAction1Arg<int>(listProfiles, playerNum), true);
        //                    new GuiTextButton(DssRef.lang.Lobby_FlagEdit, null, new GuiAction1Arg<int>(openProfileEditor, playerData.flagDesignIndex), false, layout);

        //                    if (DssRef.storage.playerCount > 1)
        //                    {
        //                        new GuiTextButton(string.Format(Ref.langOpt.InputSelect, playerData.inputSource.ToString()), null, new GuiAction3Arg<int, bool, SaveStateMeta>(selectInputMenu, playerNum, false, null), true, layout);
        //                    }

        //                    new GuiSectionSeparator(layout);
        //                }
        //                if (DssRef.storage.playerCount > 1)
        //                {
        //                    new GuiCheckbox(Ref.langOpt.VerticalSplitScreen, null, verticalSplitProperty, layout);
        //                    //menuSystem.multiplayerGameSpeedToMenu(layout);
        //                }


        //                new GuiSectionSeparator(layout);
        //                //new GuiIconTextButton(SpriteName.AutomationGearIcon, Ref.langOpt.Options_title, null, new GuiAction(optionsMenu), true, layout);
        //                //new GuiTextButton("*Crash game*", null, crashTest, false, layout); 

        //                new GuiTextButton("Play Commander", "A small tactical board game", new GuiAction(extra_PlayCommanderVersus), false, layout);

        //                if (PlatformSettings.DevBuild)
        //                {
        //                    new GuiTextButton("Map file generator", "Creates maps to play on. Takes about 10 minutes.", mapFileGenerator, false, layout);

        //                    //new GuiLargeTextButton("Test sound", null, new GuiAction(testsound), false, layout);
        //                    new GuiTextButton("Load mod", null, loadMod, false, layout);

        //                    if (Ref.steam.statsInitialized)
        //                    {
        //                        new GuiTextButton("Initialize steam stats", null, Ref.steam.stats.initializeAllStatsOnSteam, false, layout);
        //                        new GuiTextButton("Load global steam stats", null, Ref.steam.stats.beginRequestGlobalStats, false, layout);
        //                    }

        //                    new GuiTextButton("Text Input", null, new Action(() =>
        //                    {
        //                        new TextInput("test", null, null);
        //                    }), true, layout);
        //                }
        //                new GuiTextButton("Credits", null, credits, true, layout);

        //                //new GuiTextButton("Voxel Editor", "Tool to create the voxel models. Xbox controller required!", voxeleditor, false, layout);
        //                new GuiSectionSeparator(layout);
        //                new GuiTextButton(DssRef.lang.Lobby_ExitGame, null, exitGame, false, layout);
        //            } layout.End();


        //        }

        void loadMod()
        {
            string dir = "Modding" + DataStream.FilePath.Dir + "ModConst.txt";

            Data.Constants.ModLoader loader = new Data.Constants.ModLoader(dir);
        }

        //void testsound()
        //{
        //    Ref.music.stop(true);
        //    Ref.music.PlaySong(Data.Music.IAmYourDoom, false);
        //}
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
            //GuiLayout layout = new GuiLayout(HudLib.InputName(keyboard ? InputSourceType.Keyboard : InputSourceType.XController), menuSystem.menu);
            //{
            //    var map = keyboard ? Ref.gamesett.keyboardMap : Ref.gamesett.controllerMap;
            //    var list = map.listInputs(keyboard);
            //    foreach (var input in list)
            //    {
            //        IButtonMap button = null;
            //        map.getset(input, ref button, false);
            //        List<AbsRichBoxMember> buttonContent = new List<AbsRichBoxMember>(6)
            //        {
            //            new RbText(map.Name(input) + ": "),
            //        };
            //        RichBoxContent.ButtonMap(button, buttonContent);
            //        new GuiRichButton(HudLib.RbOnGuiSettings, buttonContent, null,
            //            new GuiAction2Arg<bool, InputActionType>(listMapOptions, keyboard, input),
            //            true, layout);
            //    }
            //}
            //layout.End();
        }

        //void listMapOptions(bool keyboard, InputActionType input)
        //{
        //    var map = keyboard ? Ref.gamesett.keyboardMap : Ref.gamesett.controllerMap;
        //    GuiLayout layout = new GuiLayout(map.Name(input), menuSystem.menu);
        //    {
        //        if (keyboard)
        //        {
        //            foreach (var key in availableKeyboardKeys)
        //            {
        //                var icon = Input.KeyboardButtonMap.GetKeyTile(key);
        //                if (icon != SpriteName.KeyUnknown)
        //                {
        //                    new GuiImageButton(icon, null,
        //                        new GuiAction1Arg<Keys>(listMapOptions_keyboardlink, key),
        //                        false, layout);
        //                }
        //            }
        //        }
        //    }
        //    layout.End();

        //    inKeyMapsMenu = true;
        //    mappingFor = input;
        //    layout.OnDelete += closingOptionsMenuEvent;

        //}

        //void closedKeymapsMenu()
        //{
        //    inKeyMapsMenu = false;
        //}

        void listMapOptions_keyboardlink(Keys key)
        {
            IButtonMap buttonMap = new KeyboardButtonMap(key);
            Ref.gamesett.keyboardMap.getset(mappingFor, ref buttonMap, true);

            menuSystem.menu.PopLayout();
            menuSystem.menu.PopLayout();
            keyMappingMenu_InputSource(true);

        }
        void listMapOptions_controllerlink(InputActionType input, IButtonMap buttonmap)
        {

        }

        void selectLanguageMenu()
        {
            //menuSystem.menu.blockMenuReturn = Ref.gamesett.language == LanguageType.NONE;

            RichBoxContent content = new RichBoxContent();

            Presentation.Translation translate = new Presentation.Translation();
            var options = translate.available();
            //GuiLayout layout = new GuiLayout(string.Empty, menuSystem.menu);
            //{
                foreach (var option in options)
                {
                    content.newLine();
                    var btn =new RbButton(new List<AbsRichBoxMember> { new RbImage(translate.sprite(option)) },
                        new RbAction1Arg<LanguageType>(selectLanguegeLink, option));
                btn.overrideBgColor = ColorExt.VeryDarkGray;
                content.Add(btn);
                //new GuiImageButton(translate.sprite(option), null, new GuiAction1Arg<LanguageType>(selectLanguegeLink, option), false, layout);
            }
            //}
            //layout.End();

            underMenu.Refresh(content);
        }

        void selectLanguegeLink(LanguageType language)
        {
            //menuSystem.menu.blockMenuReturn = false;

            if (language != Ref.gamesett.language)
            {
                Ref.gamesett.language = language;
                new ChangeLanguageRefresh();
            }
            else
            {
                underMenu.menuBack();
                //menuSystem.menu.PopLayout();
            }
        }

        
        void gameModeClick(GameModeMainType mode)
        {
            DssRef.difficulty.setting_gameMode = mode;
            DssRef.storage.Save(null);
            refreshDifficultyLevel();
            underMenu.CloseDropDown();
            //mainMenu();
            //newGameSettings();
        }

        void newGameSettings2()
        {

            RichBoxContent content = new RichBoxContent();
            {
                content.newLine();

                var moreArrow = new RbImage(moreOptArrow, MoreArrowScale);
                moreArrow.color = HudLib.MenuMoreOptionsArrowCol;

                var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                                    new RbBeginTitle(),
                                    new RbImage(SpriteName.WarsHudIconAdd),
                                    new RbSpace(),
                                    new RbText(DssRef.lang.Hud_Next),
                                    new RbTab(MoreArrowTabbing),
                                    moreArrow,
                                },
                    new RbAction1Arg<StartGameMode>(openPlayerSetupForMode, StartGameMode.Play), null);

                btn.fillWidth = true;
                content.Add(btn);
            }

            content.newParagraph();
            content.h1(DssRef.lang.Lobby_GameSetup, HudLib.TitleColor_Head);

            GameStorage defaultOptions = new GameStorage();

            var loadingMeta = mapBackgroundLoading.WorldData()?.metaData;

            bool continueCustomMap = loadingMeta != null && loadingMeta.customEditorMap;
            if (continueCustomMap)
            {
                content.newLine();
                content.Add(new RbText(DssRef.lang.MapType_CustomMap));
                content.space();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_Cancel) },
                    new RbAction(cancelCustomMap), new RbTooltip_Text(DssRef.lang.MapType_CustomMap)));
            }
            else
            {
                DropDownBuilder mapSzOptions = new DropDownBuilder("mapSz");
                {
                    for (MapSize sz = 0; sz < MapSize.Epic; ++sz)
                    {
                        mapSzOptions.AddOption(WorldData.SizeString(sz), DssRef.storage.mapSize == sz, defaultOptions.mapSize == sz,
                            new RbAction1Arg<MapSize>(setMapSize, sz), null);
                    }
                    mapSzOptions.Build(content, SpriteName.NO_IMAGE, DssRef.lang.Lobby_MapSizeTitle, underMenu);
                }
            }

            Difficulty.OptionsRb(content, underMenu, difficultyOptionsLink);
             

            DropDownBuilder modeOptions = new DropDownBuilder("mode");
            {
                foreach (var mode in Difficulty.AvailableModes)
                {
                   LangLib.GameModeText(mode, out string caption, out string desc);
                    modeOptions.AddOption(caption, mode == DssRef.difficulty.setting_gameMode, mode == Difficulty.DefaultMode,
                        new RbAction1Arg<GameModeMainType>(gameModeClick, mode), new RbTooltip_Text(desc));
                }
                modeOptions.Build(content, SpriteName.NO_IMAGE, DssRef.lang.Settings_GameMode, underMenu);
            }

            content.h2(DssRef.lang.Settings_AdvancedGameSettings, HudLib.TitleColor_Head);

            if (DssRef.difficulty.setting_gameMode != GameModeMainType.Spectator)
            {
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Tutorial_MenuOption) }, tutorialProperty));
            }

            if (!continueCustomMap)
            {
                //content.newLine();
                //content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.MapType_GenerateNewMap) }, generateNewMapsProperty, null));

                if (DssRef.storage.generateNewMaps)
                {
                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Map_CustomSeed) }, bCustomSeedProperty, null));
                    if (DssRef.storage.mapSettings.customSeed)
                    {
                        RbDragButton.RbDragButtonGroup(content, new List<float> { 1f }, new DragButtonSettings(ushort.MinValue, ushort.MaxValue, 1),
                            SeedProperty);
                    }
                }
            }
            
            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Settings_AllowPause) }, allowPauseProperty));
            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Settings_CentralGold) }, centralGoldProperty,
                new RbTooltip_Text(DssRef.lang.Settings_CentralGold_Description)));
            content.newLine();
            content.Add(new RbImage(SpriteName.WarsResource_Food));
            content.space();
            content.Add(new RbText(DssRef.lang.Settings_FoodMultiplier, HudLib.TitleColor_Label));
            content.space();
            content.Add(new RbDragButton(new DragButtonSettings(0.5f, 10f, 0.1f), FoodMultiProperty, true, new RbTooltip_Text(DssRef.lang.Settings_FoodMultiplier_Description)));

            content.newLine();
            content.Add(new RbImage(SpriteName.WarsResource_WaterAdd));
            content.space();
            content.Add(new RbText(DssRef.lang.Settings_WaterMultiplier, HudLib.TitleColor_Label));
            content.space();
            content.Add(new RbDragButton(new DragButtonSettings(0.2f, 10f, 0.1f), WaterMultiProperty, true, new RbTooltip_Text(DssRef.lang.Settings_WaterMultiplier_Description)));


            content.newLine();
            content.Add(new RbImage(SpriteName.WarsWorkerAdd));
            content.space();
            content.Add(new RbText(DssRef.lang.Settings_ChildMultiplier, HudLib.TitleColor_Label));
            content.space();
            content.Add(new RbDragButton(new DragButtonSettings(0.2f, 10f, 0.1f), ChildMultiProperty, true, new RbTooltip_Text(DssRef.lang.Settings_ChildMultiplier_Description)));

            content.newLine();
            content.Add(new RbImage(SpriteName.WarsHammer));
            content.space();
            content.Add(new RbText(DssRef.lang.Settings_CraftMultiplier, HudLib.TitleColor_Label));
            content.space();
            content.Add(new RbDragButton(new DragButtonSettings(0.1f, 4f, 0.1f), CraftMultiProperty, true, new RbTooltip_Text(DssRef.lang.Settings_CraftMultiplier_Description)));

            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Settings_ResetToDefault) }, new RbAction(resetToDefault)));

            underMenu.Refresh(content);

            bool tutorialProperty(object tag, bool set, bool value)
            {
                if (set)
                {
                    DssRef.storage.runTutorial_1short_2normal = value ? 2 : 0;

                    DssRef.storage.Save(null);
                }
                return DssRef.storage.runTutorial_1short_2normal != 0;
            }

            //bool generateNewMapsProperty(int index, bool set, bool value)
            //{
            //    if (set && DssRef.storage.generateNewMaps != value)
            //    {
            //        DssRef.storage.generateNewMaps = value;
            //        DssRef.storage.Save(null);
            //        restartBackgroundLoading();
            //    }
            //    return DssRef.storage.generateNewMaps;
            //}

            bool bCustomSeedProperty(object tag, bool set, bool value)
            {
                if (set)
                {
                    DssRef.storage.mapSettings.customSeed = value;

                    restartBackgroundLoading();
                    DssRef.storage.Save(null);
                }
                return DssRef.storage.mapSettings.customSeed;
            }

            int SeedProperty(bool set, int value)
            {

                if (set)
                {
                    DssRef.storage.mapSettings.seed = (ushort)value;

                    restartBackgroundLoading();
                    DssRef.storage.Save(null);
                }
                return DssRef.storage.mapSettings.seed;
            }
        }
               
      

        private void playerSetupToMenu(RichBoxContent content)
        {
            var available = availableInput();

            content.h1(DssRef.lang.Lobby_PlayerSetup, HudLib.TitleColor_Head);

            for (int playerNum = 1; playerNum <= DssRef.storage.playerCount; ++playerNum)
            {                
                var playerData = DssRef.storage.localPlayers[playerNum - 1];
                if (DssRef.storage.playerCount > 1)
                {
                    content.h2(string.Format(DssRef.lang.Player_DefaultName, playerNum), HudLib.TitleColor_Name);
                }
                content.newLine();
                if (!PlatformSettings.STEAM_DEMO)
                {
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Settings_CasualControls) }, DssRef.storage.profileStorage.casualProperty, new RbTooltip_Text(DssRef.lang.Settings_CasualControls_Description))
                    { propertyTag = playerData.profileIndex, });
                }

                if (available.Count > 1)
                {
                    DropDownBuilder inputOptions = new DropDownBuilder($"inputOptions{playerNum}");
                    foreach (var m in available)
                    {
                        inputOptions.AddOption(m.IsController ? SpriteName.birdControllerIcon : SpriteName.Keyboard, m.ToString(),
                            playerData.inputSource.Equals(m), m.HasMouse,
                            new RbAction1Arg<InputSource>((InputSource inputSource) =>
                            {
                                playerData.inputSource = inputSource;
                                DssRef.storage.checkPlayerDoublettes(0);
                                refreshSplitScreen();
                                underMenu.CloseDropDown();
                            }, m), null);
                    }
                    inputOptions.Build(content, SpriteName.NO_IMAGE, DssRef.lang.Settings_Title_Input, underMenu);

                    
                }
                listAndEditProfile(content, playerNum, playerData, false);
                if (DssRef.storage.playerCount > 1)
                {
                    content.newLine();
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Lobby_NextScreen) },
                        new RbAction1Arg<int>(nextScreenIndex, playerNum)));
                }
            }

            if (DssRef.storage.playerCount > 1 && available.Count == 1)
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.cmdWarningTriangle));
                content.space();
                content.Add(new RbText(DssRef.lang.MustTurnOffSteamInput, HudLib.InfoYellow_Light));
            }
        }

        void listAndEditProfile(RichBoxContent content, int playerNum, LocalPlayerStorage playerData, bool editor)
        {
            DssRef.storage.profileStorage.refreshProfiles();

            DropDownBuilder options = new DropDownBuilder("listprofiles" + playerNum.ToString());
            {
                for (int i = 0; i < DssRef.storage.profileStorage.profiles.Count; ++i)
                {
                    options.AddSubOption(DssRef.storage.profileStorage.profiles[i].RbButton(), i == playerData.profileIndex, false, new RbAction2Arg<int, int>(selectProfileLink, playerNum, i), null);
                }
                options.menuCaption = playerData.Profile().RbButton();
                options.injectAfter = new List<AbsRichBoxMember>() {
                                    new ArtButton(editor? RbButtonStyle.Primary : RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                                        new RbImage(SpriteName.EditorToolPencil) }, new RbAction2Arg<int, int>(openProfileEditor, playerNum -1, playerData.profileIndex), new RbTooltip_Text(DssRef.lang.Lobby_PlayerProfileEdit))
                                };
                options.Build(content, SpriteName.NO_IMAGE, null, underMenu);
            }
        }
        void selectProfileLink(int playerNumber, int profile)
        {
            int ix = playerNumber - 1;
            LocalPlayerStorage playerData = DssRef.storage.localPlayers[ix];

            //TODO
            playerData.profileIndex = profile;

            DssRef.storage.checkPlayerDoublettes(ix);

            DssRef.storage.Save(null);
            refreshSplitScreen();

            underMenu.CloseDropDown();
        }
        void openProfileEditor(int playerIndex, int ProfileIx)
        {
            DssRef.storage.selectedPlayer = playerIndex;
            DssRef.storage.profileStorage.selectedIx = ProfileIx;
            openUnderMenu(UnderMenu_PlayerProfile, StackOption.Stack);
        }

        void listAndEditCharacter(RichBoxContent content, int profileIx, bool editor)
        {
            var profile = DssRef.storage.profileStorage.Selected();

            DropDownBuilder flagOptions = new DropDownBuilder("listcharacters");
            {
                for (int i = 0; i < DssRef.storage.characterStorage.profiles.Count; ++i)
                {
                    flagOptions.AddSubOption(DssRef.storage.characterStorage.profiles[i].RbButton(DssRef.storage.flagStorage.selectedIx, true), i == profile.character.StorageIndex, false, new RbAction1Arg<int>(selectCharacterLink, i), null);
                }
                flagOptions.menuCaption = DssRef.storage.profileStorage.profiles[profileIx].character.RbButton(DssRef.storage.flagStorage.selectedIx, true);
                flagOptions.injectAfter = new List<AbsRichBoxMember>() {
                                    new ArtButton(editor? RbButtonStyle.Primary : RbButtonStyle.Secondary,
                                     HudLib.AddLockOnDemo(new List<AbsRichBoxMember> {
                                        new RbImage(SpriteName.EditorToolPencil) }), new RbAction(characterCreator), new RbTooltip_Text(DssRef.lang.Editor_CharacterCreator + ": " + DssRef.lang.Editor_CharacterCreator_Description), !PlatformSettings.STEAM_DEMO)
                                };
                flagOptions.Build(content, SpriteName.NO_IMAGE, null, underMenu);
            }
        }

        void selectCharacterLink(int charIx)
        {
            //LocalPlayerStorage playerData = DssRef.storage.localPlayers[ix];

            //TODO
            //playerData.flagDesignIndex = profile;

            //DssRef.storage.checkPlayerDoublettes(ix);

            var profile = DssRef.storage.profileStorage.Selected();
            {
                profile.character = DssRef.storage.characterStorage.profiles[charIx];
                DssRef.storage.characterStorage.selectedIx = charIx;
            }
            DssRef.storage.profileStorage.SetSelected(profile);
            DssRef.storage.profileStorage.SaveSelected();
            //DssRef.storage.Save(null);
            refreshSplitScreen();

            underMenu.CloseDropDown();
        }
        void characterCreator()
        {
            storeMenuStack();

            DssRef.storage.flagStorage.selectedIx = DssRef.storage.profileStorage.Selected().flag.StorageIndex;
            new StartEditor(-1, false, EditorType.Character);
        }

        void shaderLab()
        {
            new StartEditor(-1, false, EditorType.Shader);
        }

        void listAndEditFlag(RichBoxContent content, LocalPlayerStorage playerData, bool editor)
        {
            DropDownBuilder flagOptions = new DropDownBuilder("listflags");
            {
                for (int i = 0; i < DssRef.storage.flagStorage.flagDesigns.Count; ++i)
                {
                    flagOptions.AddSubOption(DssRef.storage.flagStorage.flagDesigns[i].RbButton(), i == DssRef.storage.flagStorage.selectedIx/*playerData.Flag().StorageIndex*/, false, new RbAction1Arg<int>(selectFlagLink, i), null);
                }
                flagOptions.menuCaption = playerData.Flag().RbButton();
                flagOptions.injectAfter = new List<AbsRichBoxMember>() {
                                    new ArtButton(editor? RbButtonStyle.Primary : RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                                        new RbImage(SpriteName.EditorToolPencil) }, new RbAction1Arg<int>(openFlagEditor, DssRef.storage.flagStorage.selectedIx/*playerData.Flag().StorageIndex*/), new RbTooltip_Text(DssRef.lang.Lobby_FlagEdit))
                                };
                flagOptions.Build(content, SpriteName.NO_IMAGE, null, underMenu);
            }
        }
        void selectFlagLink(int flagIx)
        {
            //LocalPlayerStorage playerData = DssRef.storage.localPlayers[ix];

            //TODO
            //playerData.flagDesignIndex = profile;

            //DssRef.storage.checkPlayerDoublettes(ix);

            var profile = DssRef.storage.profileStorage.profiles[DssRef.storage.profileStorage.selectedIx];
            {
                DssRef.storage.flagStorage.selectedIx = flagIx;
                profile.flag = DssRef.storage.flagStorage.flagDesigns[flagIx];
            }
            DssRef.storage.profileStorage.profiles[DssRef.storage.profileStorage.selectedIx] = profile;
            DssRef.storage.profileStorage.SaveSelected();
            //DssRef.storage.Save(null);
            refreshSplitScreen();

            underMenu.CloseDropDown();
        }
        void openFlagEditor(int flagIx)
        {
            storeMenuStack();

            int p = -1;
            bool bController = Input.XInput.KeyIsDown(Buttons.A, ref p) || Input.XInput.KeyIsDown(Buttons.X, ref p);
            new StartEditor(flagIx, bController, 0);
        }

        void storeMenuStack()
        {
            if (DssRef.settings == null)
            {
                new PlaySettings();
            }

            if (underMenu != null)
            {
                DssRef.settings.returnFromEditorMenuStack = underMenu.menuStack;
            }
        }
        public void RestoreMenuStack()
        {
            if (DssRef.settings != null)
            {
                if (DssRef.settings.returnFromEditorMenuStack != null)
                {
                    openUnderMenu(string.Empty, StackOption.Stack);
                    underMenu.menuStack = DssRef.settings.returnFromEditorMenuStack;
                    refreshUnderMenu();
                }
                DssRef.settings.returnFromEditorMenuStack = null;
            }
        }
        //void newGameSettings()
        //{
        //    var mapSizes = new List<GuiOption<MapSize>>((int)MapSize.NUM);
        //    for (MapSize sz = 0; sz < MapSize.NUM; ++sz)
        //    {
        //        mapSizes.Add(new GuiOption<MapSize>(WorldData.SizeString(sz), sz));
        //    }


        //    GuiLayout layout = new GuiLayout(string.Empty, menuSystem.menu);
        //    {
        //        new GuiLargeTextButton(DssRef.lang.Lobby_Start, null, new GuiAction(startGame), false, layout);
        //        new GuiOptionsList<MapSize>(SpriteName.NO_IMAGE, DssRef.lang.Lobby_MapSizeTitle, mapSizes, mapSizeProperty, layout);
        //        new GuiCheckbox(DssRef.lang.Settings_GenerateMaps, DssRef.lang.Settings_GenerateMaps_SlowDescription, generateNewMapsProperty, layout);

        //        difficultyLevelText = new GuiLabel("XXX", layout);

        //        new GuiTextButton(string.Format(DssRef.lang.Settings_DifficultyLevel, DssRef.difficulty.PercDifficulty), null, selectDifficultyMenu, true, layout);
        //        new GuiSectionSeparator(layout);

        //        new GuiLabel(DssRef.lang.Hud_Advanced, layout);


        //        gameModeText(DssRef.difficulty.setting_gameMode, out string modecaption, out string modedesc);

        //        new GuiTextButton(DssRef.lang.Settings_GameMode + " (" + modecaption + ")", modedesc, selectGameModeMenu, true, layout);
        //        new GuiCheckbox(DssRef.lang.Settings_AllowPause, null, allowPauseProperty, layout);
        //        new GuiLabel(, layout);
        //        var foodSlider = new GuiFloatSlider(SpriteName.WarsResource_Food, DssRef.lang.Settings_FoodMultiplier, foodMultiProperty, new IntervalF(0.5f, 10f), false, layout);
        //        foodSlider.onLeaveCallback = new Action(foodSliderLeave);
        //        foodSlider.ToolTip = DssRef.lang.Settings_FoodMultiplier_Description;

        //        new GuiCheckbox(DssRef.lang.Settings_CentralGold, DssRef.lang.Settings_CentralGold_Description, centralGoldProperty, layout);

        //        new GuiTextButton(DssRef.lang.Settings_ResetToDefault, null, resetToDefault, false, layout);
        //    }
        //    layout.End();

        //    refreshDifficultyLevel();
        //}

        public float FoodMultiProperty(bool set, float value)
        {
            return GetSet.Do<float>(set, ref DssRef.difficulty.setting_foodMulti, value);
        }
        public float WaterMultiProperty(bool set, float value)
        {
            return GetSet.Do<float>(set, ref DssRef.difficulty.setting_waterMulti, value);
        }

        public float ChildMultiProperty(bool set, float value)
        {
            return GetSet.Do<float>(set, ref DssRef.difficulty.setting_childMulti, value);
        }
        public float CraftMultiProperty(bool set, float value)
        {
            return GetSet.Do<float>(set, ref DssRef.difficulty.setting_craftMulti, value);
        }

       
       

        void selectGameModeMenu()
        {
            GuiLayout layout = new GuiLayout(string.Empty, menuSystem.menu);
            {
                for (GameModeMainType mode = 0; mode < GameModeMainType.NUM; ++mode)
                {
                    LangLib.GameModeText(mode, out string caption, out string desc);

                    new GuiTextButton(caption, desc,
                        new GuiAction1Arg<GameModeMainType>(gameModeClick, mode), false, layout);
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
            //mainMenu();
            //newGameSettings();
            underMenu.CloseDropDown();

            //menuSystem.menu.PopLayout();
        }

        void resetToDefault()
        {

            DssRef.difficulty = new Difficulty();
            DssRef.storage.defaultGameSettings();
            DssRef.storage.Save(null);
            //mainMenu();
            //newGameSettings();
            underMenu.Refresh();
        }

        void foodSliderLeave()
        { 
            DssRef.storage.Save(null);
        }

        void extra_PlayCommanderVersus()
        {
            //VikingEngine.ToGG.toggLib.Init();
            //VikingEngine.ToGG.Commander.BattleLib.Init();
            //new ToGG.ToggEngine.Map.SquareDic();
            //ToGG.ToggEngine.Map.MainTerrainProperties.Init();
            //new VikingEngine.ToGG.InputMap(0);
            ////new Network.Session();

            //ToGG.Commander.LevelSetup.GameSetup setup = new ToGG.Commander.LevelSetup.GameSetup();
            //setup.lobbyMembers = new List<ToGG.AbsLobbyMember>
            //{
            //    new ToGG.LocalLobbyMember(0),
            //    new ToGG.AiLobbyMember(),
            //};

            //new ToGG.Commander.CmdPlayState(setup);
            new StartExtra();
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

        public bool allowPauseProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                DssRef.difficulty.setting_allowPauseCommand = value;
                DssRef.storage.Save(null);
                refreshDifficultyLevel();
            }
            return DssRef.difficulty.setting_allowPauseCommand;
        }

        public bool centralGoldProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                DssRef.storage.centralGold = value;
                DssRef.storage.Save(null);
                refreshDifficultyLevel();
            }
            return DssRef.storage.centralGold;
        }

        public bool bossProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                DssRef.difficulty.runStory = value;
                DssRef.storage.Save(null);
                refreshDifficultyLevel();
            }
            return DssRef.difficulty.runStory;
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

        public void setMapSize(MapSize value)
        {
            DssRef.storage.mapSize = value;
            DssRef.storage.Save(null);
            underMenu.CloseDropDown();

            restartBackgroundLoading();
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
                mapBackgroundLoading = new MapBackgroundLoading(null as SaveStateMeta);
            }
        }


        //void inputWarningMenu()
        //{
        //    GuiLayout layout = new GuiLayout(DssRef.lang.Lobby_WarningTitle, menuSystem.menu);
        //    {
        //        new GuiLabel(DssRef.lang.Lobby_PlayerWithoutInputWarning, layout);
        //        new GuiIconTextButton(SpriteName.MenuIconResume, Ref.langOpt.Hud_Back, null, mainMenu, false, layout);
        //        new GuiIconTextButton(SpriteName.MenuPixelIconPlay, DssRef.lang.Lobby_IgnoreWarning, null, startGame_nochecks, false, layout);
        //    }
        //    layout.End();
        //}

        //void selectInputClick(int playerNumber, InputSource source)
        //{
        //    var playerData = DssRef.storage.localPlayers[playerNumber - 1];
        //    playerData.inputSource = source;
        //    DssRef.storage.checkPlayerDoublettes(playerNumber - 1);

        //    DssRef.storage.Save(null);
        //    refreshSplitScreen();
        //    mainMenu();
        //}

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

        public bool verticalSplitProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                DssRef.storage.verticalScreenSplit = value;
                refreshSplitScreen();
                DssRef.storage.Save(null);
            }
            return DssRef.storage.verticalScreenSplit;
        }

       
        //public bool longerBuildQueueProperty(int index, bool set, bool value)
        //{
        //    if (set)
        //    {
        //        DssRef.storage.longerBuildQueue = value;

        //        DssRef.storage.Save(null);
        //    }
        //    return DssRef.storage.longerBuildQueue;
        //}

        //public bool tutorialProperty(int index, bool set, bool value)
        //{
        //    if (set)
        //    {
        //        DssRef.storage.runTutorial = value;

        //        DssRef.storage.Save(null);
        //    }
        //    return DssRef.storage.runTutorial;
        //}

        

        void refreshSplitScreen()
        {
            findUnusedInput();

            checkScreenIndexes();

            splitScreenDisplay.Refresh(underMenuArea.Right);
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

            underMenu.CloseDropDown();
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
        void optionsMenu2()
        { 
            RichBoxContent content = new RichBoxContent();

            var btn = new RbButton(new List<AbsRichBoxMember> { new RbImage(new Presentation.Translation().sprite(Ref.gamesett.language)) },
                new RbAction2Arg<string, StackOption>(openUnderMenu, UnderMenu_Options_Language, StackOption.Stack));
            btn.overrideBgColor = ColorExt.VeryDarkGray;
            content.Add(btn);

            Ref.gamesett.volumeOptions(content);
            GameMenuSystem.SettingsToMenu(content, underMenu, true);

            content.newParagraph();
            IOLib.FileCheckToHud(content);

            underMenu.Refresh(content);
        }

       
        

        public override void OnResolutionChange()
        {
            base.OnResolutionChange();
            Ref.gamesett.Save();
            new LobbyState(bgTex).openUnderMenu(UnderMenu_Options, StackOption.ClearStack);
        }
        public override void LostFocus()
        {
            base.LostFocus();
            
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            lobbyAmbienceLoop?.StopAndUnload();
        }
        

        //void optionsMenu()
        //{            
        //    GuiLayout layout = new GuiLayout(Ref.langOpt.Options_title, menuSystem.menu);
        //    {
        //        new GuiImageButton(new Translation().sprite(Ref.gamesett.language), null, new GuiAction(selectLanguageMenu), true, layout);

        //        new GuiIconTextButton(SpriteName.Keyboard, DssRef.lang.Settings_ButtonMapping, null, new GuiAction(keyMappingMenu), true, layout);
        //        Ref.gamesett.optionsMenu(layout);
        //        new GuiCheckbox(DssRef.lang.GameMenu_AutoSave, null, autoSaveProperty, layout);
        //        new GuiCheckbox(DssRef.lang.Tutorial_MenuOption, null, tutorialProperty, layout);
        //        new GuiCheckbox(string.Format(DssRef.lang.GameMenu_UseSpeedX, LocalPlayer.MaxSpeedOption), null, speed5Property, layout);
        //        new GuiCheckbox(DssRef.lang.GameMenu_LongerBuildQueue, null, longerBuildQueueProperty, layout);
        //    }
        //    layout.End();

        //    layout.OnDelete += closingOptionsMenuEvent;
        //}



   

        void voxeleditor()
        {
            storeMenuStack();

            new StartEditor(0, true,  EditorType.Voxel);
        }

        void fileLab()
        {
            storeMenuStack();

            new StartEditor(0, true, EditorType.Files);
        }

        void map2()
        {
            new StartEditor(0, true, EditorType.Map2);
        }

        void mapFileGenerator()
        {
            new MapFileGeneratorState();
        }


        

        protected override void createDrawManager()
        {
            draw = new DSSWars.DrawMenu();
        }


        public override void Time_Update(float time)
        {
            bool mouseOver = false;
            base.Time_Update(time);



            menuSystem.menu?.Update();

            topMenu.updateMouseInput(ref mouseOver);
            if (underMenu != null)
            {
                underMenu.updateMouseInput(ref mouseOver);
                if (underMenu.needRefresh)
                {
                    refreshUnderMenu();
                }
            }

            splitScreenDisplay.update();

            if (mapBackgroundLoading != null)
            {
                mapBackgroundLoading.Update();
                maploading.TextString = mapBackgroundLoading.ProgressString();
            }

            messages.Update(ref mouseOver);
            //if (StartupSettings.AutoStartLevel && PlatformSettings.DevBuild)
            //{
            //    startGame();
            //}

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
            lobbyAmbienceLoop?.fadeInSound(0.5f, Ref.gamesett.AmbientVol());

            //if (inKeyMapsMenu)
            //{
            //    foreach (var key in availableKeyboardKeys)
            //    {
            //        if (Input.Keyboard.KeyDownEvent(key))
            //        {
            //            listMapOptions_keyboardlink(key);
            //        }
            //    }
            //}

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
        public void loadFileClick(SaveStateMeta saveMeta)
        {
            loadGame = saveMeta;

            openPlayerSetupForMode(StartGameMode.Play);
        }

        public void continueFromSave(SaveStateMeta saveMeta)//int listIndex)
        {
            //var save =DssRef.storage.meta.listSaves()[listIndex];

            if (saveMeta == null)
            {
                return;
            }

            
            mapBackgroundLoading?.Abort();
            

            var availableList = availableInput();
                
            new StartGame(true, netLobby, saveMeta, mapBackgroundLoading);
          
        }

        void startTutorial(bool shorter)
        {
            DssRef.storage.runTutorial_1short_2normal = shorter ? 1 : 2;
            startGame();
        }

        void startGame()
        {
            if (checkAllPlayersHasControls())
            {
                if (loadGame != null)
                {
                    continueFromSave(loadGame);
                }
                else
                {
                    //if (DssRef.storage.playerCount == 1)
                    //{
                    //    var availableList = availableInput();
                    //    //if (availableList.Count > 1)
                    //    //{
                    //    //    controllerStartGameUpdate = true;
                    //    //    selectInputMenu(1, true, null);
                    //    //}
                    //    //else
                    //    {
                    //        selectController_startGame(availableList[0], null);
                    //    }
                    //    return;
                    //}
                    //else
                    //{
                    //Check if a player is without input
                    //for (int i = 0; i < DssRef.storage.playerCount; ++i)
                    //{
                    //    if (DssRef.storage.localPlayers[i].inputSource.sourceType == InputSourceType.Num_Non)
                    //    {
                    //        //inputWarningMenu();
                    //        return;
                    //    }
                    //}

                    //}

                    startGame_nochecks();
                }
            }
        }

        bool checkAllPlayersHasControls()
        {
            for (int i = 0; i < DssRef.storage.playerCount; ++i)
            {
                if (DssRef.storage.localPlayers[i].inputSource.sourceType == InputSourceType.Num_Non)
                {
                    if (DssRef.storage.playerCount == 1)
                    {
                        var playerData = DssRef.storage.localPlayers[i];
                        playerData.inputSource = availableInput().First();
                        return true;
                    }
                    else
                    {
                        //inputWarningMenu();
                        return false;
                    }
                }
            }
            return true;
        }

        void startGame_nochecks()
        {
            new StartGame(true, netLobby, null, mapBackgroundLoading);
        }

        void listSaves2()
        {
            var saves = DssRef.storage.meta.listSaves();

            RichBoxContent content = new RichBoxContent();
            //HudLib.returnButton(content, underMenu, true, null);

            for (int i = 0; i < saves.Count; ++i)
            {
                var save = saves[i];

                content.newLine();

                var moreArrow = new RbImage(moreOptArrow, MoreArrowScale);
                moreArrow.color = HudLib.MenuMoreOptionsArrowCol;

                var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                                    new RbImage(SpriteName.WarsHudIconOpen),
                                    new RbSpace(),
                                    new RbText(save.TitleString()),
                                    new RbTab(MoreArrowTabbing),
                                    moreArrow,
                                },
                    new RbAction1Arg<SaveStateMeta>(loadFileClick, save), 
                    new RbTooltip_Text(save.InfoString()));

                btn.fillWidth = true;
                content.Add(btn);
            }

            content.newParagraph();

            SaveStateMeta exportPath = new SaveStateMeta();
            exportPath.import = " ";
            var tooltip = new RbTooltip_Text(string.Format(DssRef.lang.ExportImportDescription, exportPath.Path.CompleteDirectory));

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconExport), new RbSpace() ,new RbText(DssRef.lang.Lobby_ExportSave) },
                new RbAction2Arg<string, StackOption>(openUnderMenu, UnderMenu_ListSavesForExport, StackOption.Stack), tooltip));
            
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconImport), new RbSpace(), new RbText(DssRef.lang.Lobby_ImportSave) },
                new RbAction(importSaves2), tooltip));

            //new GuiSectionSeparator(layout);
            //new GuiTextButton(DssRef.lang.Lobby_ExportSave, string.Format(DssRef.lang.Lobby_ExportSave_Description, SaveMeta.ImportSaveFolder), exportSave_listsaves, true, layout);
            //new GuiTextButton(DssRef.lang.Lobby_ImportSave, null, importSaves, true, layout);

            underMenu.Refresh(content);
        }


        //void listSaves()
        //{
        //    var saves = DssRef.storage.meta.listSaves();

        //    GuiLayout layout = new GuiLayout(DssRef.lang.GameMenu_LoadState, menuSystem.menu);
        //    {
        //        for (int i = 0; i < saves.Count; ++i)
        //        {
        //            var save = saves[i];
        //            new GuiTextButton(save.TitleString(), save.InfoString(), new GuiAction1Arg<SaveStateMeta>(continueFromSave, save), false, layout); 
        //        }

        //        new GuiSectionSeparator(layout);
        //        new GuiTextButton(DssRef.lang.Lobby_ExportSave, string.Format( DssRef.lang.Lobby_ExportSave_Description, SaveMeta.ImportSaveFolder), exportSave_listsaves, true, layout);
        //        new GuiTextButton(DssRef.lang.Lobby_ImportSave, null, importSaves, true, layout); 
        //    }
        //    layout.End();
        //}

        //void exportSave_listsaves()
        //{

        //    var saves = DssRef.storage.meta.listSaves();

        //    GuiLayout layout = new GuiLayout(DssRef.lang.Lobby_ExportSave, menuSystem.menu);
        //    {
        //        for (int i = 0; i < saves.Count; ++i)
        //        {
        //            var save = saves[i];
        //            new GuiTextButton(save.TitleString(), save.InfoString(), new GuiAction1Arg<SaveStateMeta>(exportSaveSelected, save), false, layout);
        //        }
        //    }
        //    layout.End();
        //}
        void exportSave_listsaves2()
        {
            RichBoxContent content = new RichBoxContent();
            HudLib.returnButton(content, underMenu, true, null);
            var saves = DssRef.storage.meta.listSaves();

            for (int i = 0; i < saves.Count; ++i)
            {
                var save = saves[i];
                var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconExport),
                    new RbSpace(),
                    new RbText(save.TitleString()),
                    },
                    new RbAction1Arg<SaveStateMeta>(exportSaveSelected, save),
                    new RbTooltip_Text(save.InfoString()));

                btn.fillWidth = true;
                content.Add(btn);
            }
            
            underMenu.Refresh(content);
        }

        void exportSaveSelected(SaveStateMeta saveMeta)
        {
            SaveStateMeta exportPath = new SaveStateMeta();
            //exportPath.storageSetup();
            exportPath.import = saveMeta.ExportString();

            var fileName = FileToDiskManager.SearchFilesInStorageDir(saveMeta.Path, false)[0];
            File.Copy(fileName, exportPath.Path.CompletePath(true), overwrite: true);

            RichBoxContent content = new RichBoxContent();
            content.h1(DssRef.lang.Hud_SaveCompleted, HudLib.TitleColor_Head);
            content.text(DssRef.lang.Lobby_ExportSave);
            content.newLine();
            content.Add(new RbText(exportPath.Path.CompletePath(true), HudLib.InfoYellow_Light));
            messages.Add(content);
            //mainMenu2();
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
        }

        void importSaves2()
        {
            RichBoxContent content = new RichBoxContent();
            HudLib.returnButton(content, underMenu, true, null);


            var saves = DssRef.storage.meta.listSaves();
            importSavesMenu = true;

            content.Add(new RbText(DssRef.lang.Hud_Loading, HudLib.InfoYellow_Light));

            underMenu.menuStack.Add("import");
            underMenu.Refresh(content);
            new Timer.AsynchActionTrigger(loadSaveImportsList_async2, true);

            
        }

        void loadSaveImportsList_async2()
        {
            var list = DssRef.storage.meta.ListSaveImports();


            for (int i =0; i < list.Count; ++i)//each (var f in list)
            {
                list[i] = list[i].Split(Path.DirectorySeparatorChar).Last();
            }

            new Timer.Action1ArgTrigger<List<string>>(listImports2, list);
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

        void listImports2(List<string> names)
        {
            RichBoxContent content = new RichBoxContent();
            HudLib.returnButton(content, underMenu, true, null);

            if (importSavesMenu)
            {
                //menuSystem.menu.PopLayout();

                //GuiLayout layout = new GuiLayout(DssRef.lang.GameMenu_LoadState, menuSystem.menu);
                //{
                    for (int i = 0; i < names.Count; ++i)
                    {
                        var save = names[i];
                            var btn = new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                                    new RbImage(SpriteName.WarsHudIconImport),
                                    new RbSpace(),
                                    new RbText(LoadContent.CheckCharsSafety(save, LoadedFont.Regular)),

                                },
                        new RbAction1Arg<string>(importSave, save));

                        btn.fillWidth = true;
                        content.Add(btn);
                    //new GuiTextButton(LoadContent.CheckCharsSafety(save, LoadedFont.Regular), null, new GuiAction1Arg<string>(importSave, save), false, layout);
                    }

                    if (names.Count == 0)
                    {
                        content.Add(new RbText(DssRef.lang.Hud_EmptyList, HudLib.InfoYellow_Light));
                    }
                //}
                //layout.End();
            }
            underMenu.Refresh(content);
        }

        void importSave(string name)
        {
            SaveStateMeta meta = new SaveStateMeta();            
            meta.import = name;
            loadGame = meta;
            openPlayerSetupForMode(StartGameMode.Play);
            //meta.loadImportMeta();
        }
        //public void loadFileClick(SaveStateMeta saveMeta)
        //{
        //    loadGame = saveMeta;

        //    openPlayerSetupForMode(StartGameMode.Play);
        //}



        void selectController_startGame(InputSource inputSource, SaveStateMeta saveMeta)
        {
            var playerData = DssRef.storage.localPlayers[0];
            playerData.inputSource = inputSource;
            DssRef.storage.checkPlayerDoublettes(0);

            new StartGame(true, netLobby, saveMeta, mapBackgroundLoading);
        }

        //void startGame(SaveStateMeta saveMeta)
        //{
        //    //var playerData = DssRef.storage.localPlayers[0];
        //    //playerData.inputSource = inputSource;
        //    //DssRef.storage.checkPlayerDoublettes(0);

        //    new StartGame(true, netLobby, saveMeta, mapBackgroundLoading);
        //}

    }

    class GamerStatus
    {
        public Graphics.TextS text;
        public bool joined = false;
        public Graphics.ImageAdvanced flagTexure;
    }

    enum StartGameMode
    { 
        Play,
        Tutorial,
        BattleLab,
        BattleTrials,
    }

}
