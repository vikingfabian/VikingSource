using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//xna
using Microsoft.Xna.Framework;
using VikingEngine.HUD;
using VikingEngine.Network;
using VikingEngine.ToGG.Commander.LevelSetup;

namespace VikingEngine.ToGG.GameState
{
    class MainMenuState : AbsToggState
    {
        ChallengeLevelsData challenges;
        Graphics.ImageGroup loadingStory = null;

        public MainMenuState()
            : base()
        {
            toggRef.ClearMEM();

            challenges = new ChallengeLevelsData();
            new MenuSystem(Input.InputSource.DefaultPC).OpenMenu(false);
             
            refreshMainPage();

            createNetStatusText();

            Vector2 logPos = toggRef.menu.menu.area.RightTop;
            logPos.X += Engine.Screen.IconSize;
            float logW = Engine.Screen.SafeArea.Right - logPos.X;

            new Graphics.TextBoxSimple(LoadedFont.Console, logPos, Vector2.One, Graphics.Align.Zero,
                ChangeLog.Text(),
                Color.White, ImageLayers.Background4, logW);

#if PCGAME
            if (Ref.steam.leaderBoards != null)
            {
                Ref.steam.leaderBoards.uploadlastplayed();
            }
#endif
            if (Ref.lobby == null)
            {
                new HeroQuest.Net.NetLobby();
            }

            Ref.lobby.startSearchLobbies(true);
            //Ref.analytics.onStateChange(GameStateType.Mainmenu);
        }

        void testUpdateThread2()
        {
            new AsynchUpdateable(testUpdateThread2_action, "test", 0);
        }

        int testupdateCount = 0;
        bool testUpdateThread2_action(int id, float time)
        {
            testupdateCount++;

            if (testupdateCount == 300)
            {
                draw.ClrColor = Color.Red;
                throw new Exception();
            }

            return false;
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            
            if (loadingStory != null)
            {
                return;
            }

            toggRef.menu.menu.Update();

            if (PlatformSettings.DevBuild)
            {
                if ((Input.Keyboard.Ctrl || StartUpSett.QuickRunInSinglePlayer) && UpdateCount == 1)
                {
                    hqLobby(true);
                }
                else if (StartUpSett.RunMoonFall)
                {
                    new MoonFall.MoonFallState();
                }
                else if (StartUpSett.RunCommander)
                {
                    quickPlayCommanderVersus();
                }
            }
        }

        void refreshMainPage()
        {
            GuiLayout layout = new GuiLayout("Main Menu", toggRef.menu.menu);
            {
                //if (PlatformSettings.Demo)
                //{
                //    new GuiLabel("Demo Mode", layout);
                //}
                new GuiTextButton("Tutorial", null, runTutorial, false, layout);

                new GuiSectionSeparator(layout);
                new GuiLargeTextButton("Host a Quest", null, 
                    new GuiAction1Arg<bool>(hqLobby, true), false, layout);
                new GuiOptionsList<LobbyPublicity>(SpriteName.NO_IMAGE, "Lobby Publicity ",
                    new List<GuiOption<LobbyPublicity>>{
                        new GuiOption<LobbyPublicity>("Invites only", LobbyPublicity.Private),
                        new GuiOption<LobbyPublicity>("Friends", LobbyPublicity.FriendsOnly),
                        new GuiOption<LobbyPublicity>("Public", LobbyPublicity.Public) 
                    },
                    publicityProperty, layout);
                new GuiSectionSeparator(layout);

                new GuiTextButton("Editor", null, startEditor, false, layout);
                new GuiTextButton("Options", null, toggRef.menu.options, true, layout);
                new GuiTextButton("Credits", null, toggRef.menu.credits, true, layout);
#if PCGAME
                Ref.steam.debugToMenu(layout);
#endif
                new GuiTextButton("Exit", null, exitGame, false, layout);
            }
            layout.End();
        }

        LobbyPublicity publicityProperty(bool set, LobbyPublicity value)
        {
            if (set)
            {
                Ref.netSession.LobbyPublicity = value;
            }
            return Ref.netSession.LobbyPublicity;
        }

        void runTutorial()
        {
            new HeroQuest.Lobby.LobbyState(true, HeroQuest.QuestName.TutorialPractice);
        }

        void hqLobby(bool host)
        {
            new HeroQuest.Lobby.LobbyState(host, null);
        }

        public override void NetworkStatusMessage(NetworkStatusMessage message)
        {
            base.NetworkStatusMessage(message);

            switch (message)
            {
                default:

                    break;
                case Network.NetworkStatusMessage.Searching_Session:
                    netStatusText.TextString = "Searching sessions...";
                    break;
            }
        }

        public override void NetEvent_GotNetworkId()
        {
            base.NetEvent_GotNetworkId();

            new HeroQuest.Lobby.LobbyState(false, null);
        }
        //public override void NetEvent_JoinedLobby(string name, ulong lobbyHost, bool fromInvite)
        //{
        //    base.NetEvent_JoinedLobby(name, lobbyHost, fromInvite);

        //    if (fromInvite)
        //    {
        //        new HeroQuest.Lobby.LobbyState(false, null);
        //    }
        //}

        public override void NetEvent_SessionsFound(List<AbsAvailableSession> availableSessions, List<AbsAvailableSession> prevAvailableSessionsList)
        {
            base.NetEvent_SessionsFound(availableSessions, prevAvailableSessionsList);

            if (arraylib.HasMembers( availableSessions))
            {
                netStatusText.TextString = "Sessions found" + TextLib.ValueInParentheses(availableSessions.Count, true);                
            }
            else
            {
                netStatusText.TextString = "No available sessions";
            }
        }

        void story1Link()
        {
            if (toggRef.storage.activeStoryStorage == null)
            {
                Storage.StoryStorage story = new Storage.StoryStorage(Story1Missions.FileName);
                toggRef.storage.activeStoryStorage = story;

                if (story.hasFile())
                {
                    story.saveLoad(false);
                    VectorRect area = Engine.Screen.Area;
                    area.AddRadius(1);
                    Graphics.Image loadingBg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.Top3);
                    loadingBg.Color = Color.Black;
                    loadingBg.Opacity = 0.6f;
                    Graphics.TextG loadingText = new Graphics.TextG(LoadedFont.Regular, area.Center, Engine.Screen.TextSizeV2 * 2f, Graphics.Align.CenterAll,
                        "Loading...", Color.White, ImageLayers.AbsoluteBottomLayer);
                    loadingText.LayerAbove(loadingBg);

                    loadingStory = new Graphics.ImageGroup(loadingBg, loadingText);
                    return;
                }
            }

            new Story1Missions().ListMissions();
        }

        public void onStoryLoaded()
        {
            if (loadingStory != null)
            {
                loadingStory.DeleteAll();
                loadingStory = null;

                new Story1Missions().ListMissions();
            }
        }

        void highlightButton(string text, Action link, GuiLayout layout, bool showMoreMenusArrow, bool highlight)
        {
            if (highlight)
            {
                var button = new GuiLargeTextButton(text, null, new GuiAction(link), showMoreMenusArrow, layout);
                button.setBackGroundCol(Color.DarkBlue, Color.Blue);
            }
            else
            {
                new GuiTextButton(text, null, link, showMoreMenusArrow, layout);
            }
        }

        void missionTest(int p)
        {
            GameSetup setup = new GameSetup();
            setup.lobbyMembers = new List<AbsLobbyMember>
            {
                new LocalLobbyMember(p),
                new AiLobbyMember(),
            };

            setup.missionDescription = "Debug mission";

            //-----
            setup.loadMap = "map4";
            //----

            new Commander.CmdPlayState(setup);
        }

        void exitGame()
        {
            Ref.update.exitApplication = true;
        }

        void listTutorials()
        {
            challenges.listTutorials(challenges.startMission);
        }

        void startTutorial(LevelEnum lvl)
        {
            missionMenu(challenges.levelSetup(lvl), false);
        }

        void testAiLevel()
        {
            //new Data.Story1Missions().startMission(LevelEnum.testmix);
            GameSetup setup = new GameSetup();
            setup.lobbyMembers = new List<AbsLobbyMember>
            {
                new LocalLobbyMember(0),
                new AiLobbyMember(),
            };

            //setup.level = LevelEnum.testmix;
            //setup.useStrategyCards = true;


            setup.loadMap = "testmix";
            //setup.WinningConditions = new VikingEngine.Commander.Data.WinningCondition_Mission2();//new WinningCondition_Score();
            //setup.missionName = "TEST";
            //setup.missionDescription = "TEST";

            new Commander.CmdPlayState(setup);
        }

        public void MissionResult(GameSetup setup, bool succes)
        {
            if (succes)
            {
                listTutorials();

                GuiLayout layout = new GuiLayout("Mission success!", toggRef.menu.menu);
                {
                    new GuiTextButton("OK", null, toggRef.menu.menu.PopLayout, false, layout);
                }
                layout.End();
            }
            else
            {
                missionMenu(setup, true);
            }
        }

        void missionMenu(GameSetup setup, bool restart)
        {
            GuiLayout layout = new GuiLayout(restart ? "Mission failed!" : setup.missionName, toggRef.menu.menu);
            {
                new GuiLabel(setup.missionDescription, true, layout.gui.style.textFormat, layout);
                new GuiTextButton(restart ? "Retry" : "Begin", null, 
                    new GuiAction1Arg<LevelEnum>(challenges.startMission, setup.level), 
                    false, layout);
                new GuiTextButton("Cancel", null, toggRef.menu.menu.PopLayout, false, layout);
            }
            layout.End();
        }

        public void storyResultMenu(GameSetup setup, EndGameResult result)
        {
            LevelEnum next = LevelEnum.NONE;
            Story1Missions story = new Story1Missions();
            int nextIx = arraylib.IndexFromValue(Story1Missions.Levels, setup.level) + 1;
            
            if (arraylib.InBound( Story1Missions.Levels, nextIx))
            {
                next = Story1Missions.Levels[nextIx];
                if (result == EndGameResult.MissionSuccess)
                {
                    toggRef.storage.activeStoryProgress.onLevelOver(setup.level == LevelEnum.Story1Practice1);
                    toggRef.storage.activeStoryStorage.StorePoint(next);
                }

                new Story1Missions().ListMissions();
            }
        }

        void startEditor()
        {
            new ToggEngine.MapEditor.EditorState(GameMode.HeroQuest);
        }

        void playHotSeat()
        {
            GameSetup setup = new GameSetup();
            setup.lobbyMembers = new List<AbsLobbyMember>
            {
                new LocalLobbyMember(0),
                new LocalLobbyMember(1),
            };
             
            new Commander.CmdPlayState(setup);
        }

        void quickPlayCommanderVersus()
        {
            GameSetup setup = new GameSetup();
            setup.lobbyMembers = new List<AbsLobbyMember>
            {
                new LocalLobbyMember(0),
                new AiLobbyMember(),
            };

            new Commander.CmdPlayState(setup);
        }

        public override void OnResolutionChange()
        {
            base.OnResolutionChange();
            var state = new MainMenuState();
            toggRef.menu.options();
        }

        //void hostLobby()
        //{
        //    new Commander.LobbyState(true, false);
        //}

        protected override bool DefaultMouseLock
        {
            get { return false; }
        }
    }
}
