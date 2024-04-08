﻿using System;
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
using VikingEngine.DSSWars.Data;

namespace VikingEngine.DSSWars
{
    class LobbyState : Engine.GameState
    {
        Display.MenuSystem menuSystem;
        MapBackgroundLoading mapBackgroundLoading;
        NetworkLobby netLobby = new NetworkLobby();
        GameTimer emitTimer = new GameTimer(0.1f);

        Texture2D bgTex;
        Graphics.ImageAdvanced bgImage = null;
        Display.SplitScreenDisplay splitScreenDisplay= new Display.SplitScreenDisplay();
        XInputJoinHandler joinHandler = new XInputJoinHandler();
        bool controllerStartGameUpdate = false;
        Graphics.TextG maploading;
        GuiLabel difficultyLevelText = null;
        public LobbyState()
            :base()
        {
            Ref.isPaused = false;
            Engine.Screen.SetupSplitScreen(1, true);
            if (!StartupSettings.BlockBackgroundLoading)
            {
                mapBackgroundLoading = new MapBackgroundLoading();
            }
            float txtSz = Engine.Screen.Height * 0.0012f;

            Ref.draw.ClrColor = new Color(11, 30, 34);

            menuSystem = new Display.MenuSystem(new InputMap(Engine.XGuide.LocalHostIndex),  Display.MenuType.Lobby);
            DssRef.storage.checkConnected();
            mainMenu();

            Graphics.TextG version = new Graphics.TextG(LoadedFont.Console, Screen.SafeArea.RightBottom, 
                Engine.Screen.TextSizeV2, new Align(Vector2.One), string.Format(DssRef.lang.Lobby_GameVersion,Engine.LoadContent.SteamVersion), 
                Color.LightYellow, ImageLayers.Background2);

            maploading = new Graphics.TextG(LoadedFont.Console, Screen.SafeArea.LeftBottom,
                Engine.Screen.TextSizeV2, new Align(new Vector2(0, 1f)), "...",
                Color.DarkGray, ImageLayers.Background2);

            new Timer.AsynchActionTrigger(load_asynch, true);
            new Timer.TimedAction0ArgTrigger(playMusic, 1000);
        }       
        
        void load_asynch()
        {
            bgTex = Ref.main.Content.Load<Texture2D>(DssLib.ContentDir + "dss_bg");
            new Timer.Action0ArgTrigger(loadingComplete);
        }

        void loadingComplete()
        {
            float w = Engine.Screen.SafeArea.Width;
            float h = w/bgTex.Width*bgTex.Height;
            float x = Engine.Screen.SafeArea.X;            
            float y = Screen.CenterScreen.Y - h * 0.5f;

            bgImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                new Vector2(x, y), new Vector2(w, h), ImageLayers.Background5, false);
            bgImage.Texture = bgTex;
            bgImage.SetFullTextureSource();
            bgImage.Opacity = 0.5f;

            //Graphics.TextG title = new Graphics.TextG(LoadedFont.Bold, bgImage.CenterTop, Engine.Screen.TextSizeV2 * 2.6f, new Align(new Vector2(PublicConstants.Half, 1f)),
            //    "DSS War Party", new Color(36,80,89), ImageLayers.Background8);
        }

        void playMusic()
        {
            if (Ref.music != null)
            {
                Ref.music.PlaySong(Data.Music.Intro, false);
            }
        }
       
        void mainMenu()
        {
            controllerStartGameUpdate = false;
            menuSystem.openMenu();
            menuSystem.menu.PopAllLayouts();

            var mapSizes = new List<GuiOption<MapSize>>((int)MapSize.NUM);
            for (MapSize sz = 0; sz < MapSize.NUM; ++sz)
            {
                string name=null;
                switch (sz)
                {
                    case MapSize.Tiny: name = DssRef.lang.Lobby_MapSizeOptTiny; break;
                    case MapSize.Small: name = DssRef.lang.Lobby_MapSizeOptSmall; break;
                    case MapSize.Medium: name = DssRef.lang.Lobby_MapSizeOptMedium; break;
                    case MapSize.Large: name = DssRef.lang.Lobby_MapSizeOptLarge; break;
                    case MapSize.Huge: name = DssRef.lang.Lobby_MapSizeOptHuge; break;
                    case MapSize.Epic: name = DssRef.lang.Lobby_MapSizeOptEpic; break;
                }
                var dim= WorldData.SizeDimentions(sz);
                name += " " + 
                    string.Format(DssRef.lang.Lobby_MapSizeDesc, 
                        Math.Round(dim.X * WorldData.TileWidthInKm),
                        Math.Round(dim.Y * WorldData.TileWidthInKm));
                mapSizes.Add(new GuiOption<MapSize>(name, sz));
            }
            
            GuiLayout layout = new GuiLayout(string.Empty, menuSystem.menu);
            {
                if (StartupSettings.CheatActive)
                {
                    new GuiLabel("! debug cheats !", layout);
                }
                new GuiLargeTextButton(DssRef.lang.Lobby_Start, null, new GuiAction(startGame), false, layout);
                
                new GuiTextButton(string.Format(DssRef.lang.Lobby_LocalMultiplayerEdit, DssRef.storage.playerCount),
                    null, localMultiplayerMenu, true, layout);
                
                for (int playerNum = 1; playerNum <= DssRef.storage.playerCount; ++playerNum)
                {
                    var playerData= DssRef.storage.localPlayers[playerNum - 1];
                    if (DssRef.storage.playerCount > 1)
                    {
                        new GuiLabel(string.Format( DssRef.lang.Player_DefaultName, playerNum), layout);
                        new GuiTextButton(DssRef.lang.Lobby_NextScreen, null, new GuiAction1Arg<int>(nextScreenIndex, playerNum), false, layout);
                    }
                    DssRef.storage.flagStorage.flagDesigns[playerData.profile].Button(layout, new GuiAction1Arg<int>(listProfiles, playerNum), true);
                    new GuiTextButton(DssRef.lang.Lobby_ProfileEdit, null, new GuiAction1Arg<int>( openProfileEditor, playerData.profile), false, layout);
                    
                    if (DssRef.storage.playerCount > 1)
                    {
                        new GuiTextButton(string.Format(Ref.langOpt.InputSelect, playerData.inputSource.ToString()), null, new GuiAction2Arg<int, bool>(selectInputMenu, playerNum, false), true, layout);
                    }
                    
                    new GuiSectionSeparator(layout);
                }
                if (DssRef.storage.playerCount > 1)
                {
                    new GuiCheckbox(Ref.langOpt.VerticalSplitScreen, null, verticalSplitProperty, layout);
                }

                new GuiOptionsList<MapSize>(SpriteName.NO_IMAGE, DssRef.lang.Lobby_MapSizeTitle, mapSizes, mapSizeProperty, layout);
                new GuiCheckbox("Generate new maps", "Generating is slower than loading the pre-built maps", generateNewMapsProperty, layout);


                difficultyLevelText = new GuiLabel("XXX", layout);
                
                new GuiTextButton(string.Format(DssRef.lang.Settings_DifficultyLevel, DssRef.difficulty.PercDifficulty), null, selectDifficultyMenu, true, layout);

                new GuiCheckbox("Allow pause and command", null, allowPauseProperty, layout);
                new GuiCheckbox("Boss events", "Turning off the boss will put the game in a sandbox mode with no ending.", bossProperty, layout);

                new GuiSectionSeparator(layout);
                new GuiTextButton(Ref.langOpt.Options_title, null, new GuiAction(optionsMenu), true, layout);
                //new GuiTextButton("*Crash game*", null, crashTest, false, layout); 
                if (PlatformSettings.DevBuild)
                {
                    new GuiTextButton("Map file generator", "Creates maps to play on. Takes about 10 minutes.", mapFileGenerator, false, layout);
                }
                //new GuiTextButton("Voxel Editor", "Tool to create the voxel models. Xbox controller required!", voxeleditor, false, layout);
                new GuiSectionSeparator(layout);
                new GuiTextButton(DssRef.lang.Lobby_ExitGame, null, exitGame, false, layout);
            } layout.End();

            refreshDifficultyLevel();
        }



        //void settingsGui(GuiLayout layout)
        //{
            

        //}

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
            difficultyLevelText.text.TextString = string.Format( DssRef.lang.Settings_TotalDifficulty, DssRef.difficulty.TotalDifficulty());
        }

        //public BossTimeSettings bossTimeProperty(bool set, BossTimeSettings value)
        //{
        //    if (set)
        //    {
        //        DssRef.storage.bossTimeSettings = value;
        //        DssRef.storage.Save(null);
        //        refreshDifficultyLevel();
        //    }
        //    return DssRef.storage.bossTimeSettings;
        //}

        //public BossSize bossSizeProperty(bool set, BossSize value)
        //{
        //    if (set)
        //    {
        //        DssRef.storage.bossSize = value;
        //        DssRef.storage.Save(null);
        //        refreshDifficultyLevel();
        //    }
        //    return DssRef.storage.bossSize;
        //}

        //public AiAggressivity aggresiveProperty(bool set, AiAggressivity value)
        //{
        //    if (set)
        //    {
        //        DssRef.storage.aiAggressivity = value;
        //        DssRef.storage.Save(null);
        //        refreshDifficultyLevel();
        //    }
        //    return DssRef.storage.aiAggressivity;
        //}

        //public int aiEconomyProperty(bool set, int value)
        //{
        //    if (set)
        //    {
        //        DssRef.storage.aiEconomyLevel = value;
        //        DssRef.storage.Save(null);
        //        refreshDifficultyLevel();
        //    }
        //    return DssRef.storage.aiEconomyLevel;
        //}

        //public int diplomacyDifficultyProperty(bool set, int value)
        //{
        //    if (set)
        //    {
        //        DssRef.storage.diplomacyDifficulty = value;
        //        DssRef.storage.Save(null);
        //        refreshDifficultyLevel();
        //    }
        //    return DssRef.storage.diplomacyDifficulty;
        //}
        public bool allowPauseProperty(int index, bool set, bool value)
        {
            if (set)
            {
                DssRef.difficulty.allowPauseCommand = value;
                DssRef.storage.Save(null);
                refreshDifficultyLevel();
            }
            return DssRef.difficulty.allowPauseCommand;
        }

        public bool bossProperty(int index, bool set, bool value)
        {
            if (set)
            {
                DssRef.difficulty.boss = value;
                DssRef.storage.Save(null);
                refreshDifficultyLevel();
            }
            return DssRef.difficulty.boss;
        }

        //public bool honorGuardProperty(int index, bool set, bool value)
        //{
        //    if (set)
        //    {
        //        DssRef.storage.honorGuard = value;
        //        DssRef.storage.Save(null);
        //        refreshDifficultyLevel();
        //    }
        //    return DssRef.storage.honorGuard;
        //}

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
                    mapBackgroundLoading = new MapBackgroundLoading();
                }
        }

        void selectInputMenu(int playerNumber, bool startGame)
        {
            var available = availableInput();
            GuiLayout layout = new GuiLayout(Ref.langOpt.InputSelect, menuSystem.menu);
            {
                foreach(var m in available)
                {
                    if (startGame)
                    {
                        if (m.IsController)
                        {
                            new GuiIconTextButton(SpriteName.ButtonSTART, m.ToString(), null, new GuiAction1Arg<InputSource>(selectController_startGame, m), false, layout);
                        }
                        else 
                        {
                            new GuiTextButton(m.ToString(), null, new GuiAction1Arg<InputSource>(selectController_startGame, m), false, layout);
                        }
                    }
                    else 
                    {
                        new GuiTextButton(m.ToString(), null, new GuiAction2Arg<int, InputSource>(selectInputClick, playerNumber, m), false, layout);
                    }
                }
            }
            layout.End();
        }

        void inputWarningMenu()
        {
            GuiLayout layout = new GuiLayout("Warning", menuSystem.menu);
            {
                new GuiLabel("One player has no input", layout);
                new GuiIconTextButton(SpriteName.MenuIconResume, "Back", null, mainMenu, false, layout);
                new GuiIconTextButton(SpriteName.MenuPixelIconPlay, "Ignore warning", null, startGame_nochecks, false, layout);
            }
            layout.End();
        }

        void selectInputClick(int playerNumber, InputSource source)
        {
            var playerData = DssRef.storage.localPlayers[playerNumber - 1];
            playerData.inputSource = source;
            DssRef.storage.checkPlayerDoublettes(playerNumber-1);

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
            for (var i = 0;i < DssRef.storage.playerCount;i++) 
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
                    new GuiTextButton(i.ToString(), null, new GuiAction1Arg<int>(setPlayerCount, i), false, layout);
                }
            }
            layout.End();
        }

        void setPlayerCount(int count)
        {
            DssRef.storage.playerCount = count;
            refreshSplitScreen();
            mainMenu();
        }

        void exitGame()
        {
            Ref.update.exitApplication = true;
        }

        void listProfiles(int playerNumber)
        {
            GuiLayout layout = new GuiLayout(DssRef.lang.Lobby_ProfilesSelectTitle, menuSystem.menu);
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
                Ref.gamesett.optionsMenu(layout);
            }
            layout.End();

            layout.OnDelete += closingOptionsMenu;
        }

        void closingOptionsMenu()
        {
            Ref.gamesett.Save();
        }

        void selectProfileLink(int playerNumber, int profile)
        {
            
            int ix = playerNumber - 1;
            var playerData = DssRef.storage.localPlayers[ix];
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
            
            new PaintFlagState(ProfileIx);
        }

        protected override void createDrawManager()
        {
            draw = new DSSWars.DrawMenu();
        }

        
        public override void Time_Update(float time)
        {
            //emitGlow();
            base.Time_Update(time);

            menuSystem.menu.Update();
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
                    selectController_startGame(new InputSource(InputSourceType.XController, index));
                }
            }

            if (VikingEngine.Input.Keyboard.Ctrl && VikingEngine.Input.Keyboard.KeyDownEvent(Keys.V))
            {
                voxeleditor();
            }

            if (Ref.music != null)
            {
                Ref.music.Update();
            }
        }

        void emitGlow()
        {
            if (emitTimer.TimeOut_Event)
            {
                emitTimer.goalTimeSec = Ref.rnd.Float(0.01f, 0.2f);
                emitTimer.Reset();

                if (bgImage != null && DssRef.storage.playerCount==1)
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

        void selectController_startGame(InputSource inputSource)
        {
            var playerData = DssRef.storage.localPlayers[0];
            playerData.inputSource = inputSource;
            DssRef.storage.checkPlayerDoublettes(0);

            new StartGame(netLobby, mapBackgroundLoading);
        }

       

        void startGame()
        {
            if (DssRef.storage.playerCount == 1)
            {
                var availableList = availableInput();
                if (availableList.Count > 1)
                {
                    controllerStartGameUpdate = true;
                    selectInputMenu(1, true);
                }
                else
                {
                    selectController_startGame(availableList[0]);
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
            new StartGame(netLobby, mapBackgroundLoading);
        }


        //public override void NetEvent_PeerJoined(Network.AbsNetworkPeer gamer)
        //{
        //    base.NetEvent_PeerJoined(gamer);
        //    netLobby.NetEvent_PeerJoined(gamer);
        //}
        //public override void NetworkReadPacket(Network.ReceivedPacket packet)
        //{
        //    base.NetworkReadPacket(packet);
        //    netLobby.NetworkReadPacket(packet);
        //}
        //public override void NetEvent_PeerLost(Network.AbsNetworkPeer gamer)
        //{
        //    base.NetEvent_PeerLost(gamer);
        //    netLobby.NetEvent_PeerLost(gamer);
        //}
    }

    class GamerStatus
    {
        public Graphics.TextS text;
        public bool joined = false;
        public Graphics.ImageAdvanced flagTexure;
    }

}
