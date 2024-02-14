using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD;
using VikingEngine.Network;
using VikingEngine.SteamWrapping;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.HeroQuest.Lobby
{
    class LobbyState : AbsToggState, DataStream.IStreamIOCallback
    {
        HeroSelectScreen selectScreen;
        bool host;

        Graphics.Text2 questStatus;
        FileManager filemanager;
        QuestName? autoRunLevel;
        Timer.Basic statusRefresh = new Timer.Basic(2000, true);

        public LobbyState(bool host, QuestName? autoRunLevel)
            : base()
        {
            this.host = host;
            HudLib.Init();

            hqRef.setup = new Data.QuestSetup(QuestName.NagaBoss);
            if (StartUpSett.QuickRunInSinglePlayer && 
                StartUpSett.QuickRunLevel != null &&
                StartUpSett.QuickRunLevel != QuestName.None)
            {
                autoRunLevel = StartUpSett.QuickRunLevel;
            }

            this.autoRunLevel = autoRunLevel;

            if (autoRunLevel != null)
            {
                hqRef.setup.quest = autoRunLevel.Value;
            }

            new Data.AllUnitsData();
            createNetStatusText();

            {
                VectorRect selectScreenArea = Engine.Screen.SafeArea;
                selectScreenArea.AddToLeftSide(-Engine.Screen.SafeArea.Width * 0.3f);
                selectScreenArea.AddToTopSide(-Engine.Screen.SafeArea.Height * 0.3f);

                selectScreen = new HeroSelectScreen(selectScreenArea, hqLib.MaxPlayers);

                collectLobbyMembers();

                selectScreen.setLocal(Ref.netSession.LocalHost(), host);

                
                selectScreen.setVisuals(hqRef.localPlayers.setups.First);
            }

            //if (host)
            //{                
            //    VectorRect nextButtonArea = new VectorRect(Engine.Screen.SafeArea.RightBottom, HudLib.NextPhaseButtonsSz);
            //    nextButtonArea.Position -= nextButtonArea.Size;
            //}
            
            questStatus = new Graphics.Text2("--", LoadedFont.Regular,
                new Vector2(Engine.Screen.SafeArea.X, Engine.Screen.SafeArea.Height * 0.75f),
                Engine.Screen.TextBreadHeight, Color.LightGreen, ImageLayers.Background0,
                Engine.Screen.SafeArea.Width * 0.3f);
                       
            VectorRect hudArea = Engine.Screen.SafeArea;
            hudArea.AddXRadius(-hudArea.Width * 0.25f);

            if (!host)
            {            
                Ref.netSession.BeginWritingPacket(PacketType.hqEnteredLobby, PacketReliability.Reliable);
                netWriteStatus();
                netStatusText.TextString = "Joined lobby";
            }

            Ref.lobby.startSearchLobbies(false);

            if (host && autoRunLevel != QuestName.TutorialPractice)
            {
                Ref.lobby.startCreateLobby(true);
            }

            new MenuSystem(Input.InputSource.DefaultPC).OpenMenu(false);

            refreshMainMenu();

            filemanager = new FileManager(this);
            onNewMap();

            //Ref.analytics.onStateChange(GameStateType.Lobby);
        }

        public void initEditorPlay(FileManager fileManager)
        {
            this.filemanager = fileManager;
            hqRef.setup.quest = QuestName.Custom;
            hqRef.setup.customName = fileManager.fileName;
            //onNewMap();
            refreshQuestStatus();
        }

        void collectLobbyMembers()
        {
            selectScreen.setNext(Ref.netSession.Host());

            var gamers = Ref.netSession.RemoteGamers();
            if (gamers != null)
            {
                foreach (var m in gamers)
                {
                    selectScreen.setNext(m);
                }
            }

            selectScreen.setNext(Ref.netSession.LocalHost());
        }

        void refreshMainMenu()
        {
            toggRef.menu.menu.PopAllLayouts();

            GuiLayout layout = new GuiLayout("The tavern", toggRef.menu.menu);
            {
                if (host)
                {
                    new GuiTextButton("Select quest", null, selectMission, true, layout);
                }
                new GuiTextButton("Pick hero", null, pickHero, true, layout);
                
                if (!PlatformSettings.Demo)
                {
                    new GuiTextButton("**Debug**", null, debugMenu, false, layout);
                }

                if (Ref.steam.isNetworkInitialized)
                {
                    new GuiTextButton("Invite player", null, Ref.netSession.Invite, false, layout);
                    if (host)
                    {
                        new GuiTextButton("Remove player", null, removePlayer, true, layout);
                    }
                }
                new GuiIconTextButton(SpriteName.birdLobbyReturnButton, "Exit to Main Menu",
                null, exitLobbyLink, false, layout);
            }
            layout.End();
        }

        void removePlayer()
        {
            GuiLayout layout = new GuiLayout("Remove", toggRef.menu.menu);
            {
                var remotes = Ref.netSession.RemoteGamers();
                if (remotes.Count == 0)
                {
                    new GuiLabel("No players", layout);
                }
                else
                {
                    foreach (var p in remotes)
                    {
                        new GuiTextButton(p.Gamertag, null,
                            new GuiAction1Arg<AbsNetworkPeer>(removePlayerQuestion, p),
                            true, layout);
                    }
                }
            }
            layout.End();
        }

        void removePlayerQuestion(AbsNetworkPeer peer)
        {
            GuiLayout layout = new GuiLayout("Remove Options", toggRef.menu.menu);
            {
                new GuiTextButton("Remove " + peer.Gamertag, null,
                    new GuiAction2Arg<AbsNetworkPeer, bool>(removePlayerOk, peer, false), false, layout);
                new GuiTextButton("Remove and Ban", null,
                    new GuiAction2Arg<AbsNetworkPeer, bool>(removePlayerOk, peer, true), false, layout);
            }
            layout.End();
        }

        void removePlayerOk(AbsNetworkPeer peer, bool ban)
        {
            if (ban)
            {
                Ref.gamesett.bannedPeers.add(peer);                
            }

            Ref.netSession.kickFromNetwork(peer);

            toggRef.menu.menu.PopLayouts(2);
        }

        void debugMenu()
        {
            GuiLayout layout = new GuiLayout("Debug", toggRef.menu.menu);
            {
                new GuiTextButton("Refresh", "force client data update", debugRefresh, false, layout);
                new GuiTextButton("Set map seed", null, seedOptions, true, layout);
                new GuiTextButton("Force game start", null, start, false, layout);
            }
            layout.End();
        }

        void seedOptions()
        {
            GuiLayout layout = new GuiLayout("Pick Seed", toggRef.menu.menu);
            {
                for (int i = 1; i < 20; ++i)
                {
                    new GuiTextButton(i.ToString(), null, new GuiActionIndex(setSeed, i), false, layout);
                }
            }
            layout.End();
        }

        void setSeed(int seed)
        {
            toggRef.Seed = seed;
            netWriteQuest();
            refreshQuestStatus();

            refreshMainMenu();
        }

        void debugRefresh()
        {
            collectLobbyMembers();
            refreshQuestStatus();
        }

        void netWriteQuest()
        {
            if (host && !filemanager.lockedInSaving)
            {
                var w = Ref.netSession.BeginWritingPacket(PacketType.hqQuestSetup, PacketReliability.ReliableLasy);
                hqRef.setup.netWrite(w);

                if (hqRef.setup.quest == QuestName.Custom)
                {
                    SaveLib.WriteString(w, hqRef.setup.customName);

                    filemanager.data.memory.WriteSaveFile(w);
                }
            }            
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            refreshQuestStatus();

            netWriteQuest();

            netWriteStatus();
        }

        void selectMission()
        {
            GuiLayout layout = new GuiLayout("Quests", toggRef.menu.menu);
            {
                foreach (var m in hqLib.AvailableMissions)
                {
                    new GuiTextButton(hqLib.QuestTitle(m), null, new GuiAction1Arg<QuestName>(questLink, m), false, layout);
                }
                new GuiSectionSeparator(layout);
                new GuiTextButton("Custom", "Load your maps from the editor", customQuestLink, true, layout);
            }
            layout.End();
        }

        void questLink(QuestName quest)
        {
            hqRef.setup.quest = quest;
            toggRef.menu.menu.PopLayout();
            onNewMap();
        }

        void customQuestLink()
        {
            listSaveFiles(true);
        }

        protected override void loadCustomMap(string fileName, bool fromStorage)
        {
            hqRef.setup.quest = QuestName.Custom;
            hqRef.setup.customName = fileName;

            refreshMainMenu();
            onNewMap();
        }

        void onNewMap()
        {
            if (hqRef.setup.quest != QuestName.None)
            {
                filemanager.loadFile();
                refreshQuestStatus();
            }
        }

        void refreshQuestStatus()
        {
            string loadStatus = filemanager.lockedInSaving ? "Loading" : "Ready";

            questStatus.TextString = "QUEST:" + Environment.NewLine +
                hqRef.setup.ToString() + Environment.NewLine +
                Environment.NewLine +
                "Seed: " + toggRef.Seed.ToString() + Environment.NewLine +
                loadStatus;
        }

        void pickHero()
        {
            HqUnitType[] available = new HqUnitType[]
            {
                HqUnitType.RecruitHeroBow,
                HqUnitType.RecruitHeroSword,
                HqUnitType.ElfHero,
                HqUnitType.KnightHero,
                HqUnitType.KhajaHero,

            };

            GuiLayout layout = new GuiLayout("Pick hero", toggRef.menu.menu);
            {
                foreach (var m in available)
                {
                    var hero = hqRef.unitsdata.Get(m);

                    string[] abilities;
                    HeroDifficulty difficulty;
                    string flavor;

                    flavor = hero.heroSelectDesc(out abilities, out difficulty);

                    string desc = TextLib.Quote(flavor) + Environment.NewLine +
                        "Difficulty: " + difficulty.ToString() + Environment.NewLine;

                    foreach (var ab in abilities)
                    {
                        desc += " *" + ab; 
                    }

                    new GuiTextButton(hero.Name, desc, new GuiAction1Arg<HqUnitType>(selectHeroLink, m), 
                        false, layout);
                }
            }
            layout.End();
        }

        void selectHeroLink(HqUnitType type)
        {
            toggRef.menu.menu.PopLayout();

            selectScreen.localHost.setUnit(type);
            netWriteStatus();
        }
        void exitLobbyLink()
        {
            Ref.lobby.disconnect(null);
            exitLobby();
        }
        void exitLobby()
        {
            new GameState.ExitState();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);            

            toggRef.menu.menu.Update();
            selectScreen.update();

            if (statusRefresh.Update())
            {
                netWriteStatus();
            }

            if (host)
            {
                bool inMultiplayer = selectScreen.count >= 2 ||
                    autoRunLevel == QuestName.TutorialPractice ||
                    hqRef.setup.quest == QuestName.Custom ||
                    PlatformSettings.DevBuild;

                bool readyToPlay = inMultiplayer && selectScreen.allReady();

                //startButton.Enabled = readyToPlay;
                bool autoRun = autoRunLevel != null && 
                    autoRunLevel != QuestName.None;

                if ((autoRun || readyToPlay) && !filemanager.lockedInSaving)
                {
                    if (autoRunLevel == QuestName.TutorialPractice)
                    {
                        selectScreen.localHost.setUnit(HqUnitType.RecruitHeroBow);
                    }
                    start();
                }
            }

            if (Input.Keyboard.KeyDownEvent(Keys.Escape))
            {
                exitLobbyLink();
            }
        }

        public override void NetUpdate()
        {
            base.NetUpdate();
            var w = Ref.netSession.BeginWritingPacket(PacketType.hqLobbyPlayerUpdate, 
                PacketReliability.Unrelyable);
            RemotePlayerPointer.NetWriteLobbyPos(Input.Mouse.Position, w); 
        }

        //bool localPlayerReady()
        //{

        //}

        public void netWriteStatus()
        {
            selectScreen.localHost.readyStatus.mapLoaded = filemanager != null && !filemanager.lockedInSaving;

            var w = Ref.netSession.BeginWritingPacket(PacketType.hqLobbyStatus, PacketReliability.Reliable);
            selectScreen.localHost.readyStatus.write(w);
            selectScreen.localHost.visualSetup.netWriteStatus(w);
        }

        public void netReadStatus(ReceivedPacket packet)
        {
            var member = selectScreen.GetOrCreate(packet.sender);
            if (member != null)
            {
                member.readyStatus.read(packet.r);
                member.visualSetup.netReadStatus(packet.r);
                member.refreshClientStatus();
            }
        }

        void start()
        {
            Ref.netSession.setLobbyJoinable(false);
            new HeroQuestPlay(true, filemanager);
        }        

        public override void NetEvent_PeerJoined(Network.AbsNetworkPeer gamer)
        {
            base.NetEvent_PeerJoined(gamer);

            selectScreen.setNext(gamer).joining(true);
        }

        public override void NetEvent_ConnectionLost(string reason)
        {
            base.NetEvent_ConnectionLost(reason);

            if (host)
            {
                netStatusText.TextString = "Lost connection";
            }
            else
            {
                exitLobby();
            }
        }

        public override void NetEvent_PeerLost(AbsNetworkPeer peer)
        {
            base.NetEvent_PeerLost(peer);

            selectScreen.remove(peer);
        }

        public override void NetworkStatusMessage(NetworkStatusMessage message)
        {
            base.NetworkStatusMessage(message);

            if (message == Network.NetworkStatusMessage.Created_session)
            {
                netStatusText.TextString = "Lobby is online";
            }
        }

        public override void NetworkReadPacket(ReceivedPacket packet)
        {
            base.NetworkReadPacket(packet);

            switch (packet.type)
            {
                case PacketType.hqAssignPlayers:
                    var play = new HeroQuestPlay(false, filemanager);
                    play.init1_netReadAssignHeroIndex(packet.r);
                    break;
                case PacketType.hqQuestSetup:
                    hqRef.setup.netRead(packet.r);

                    if (hqRef.setup.quest == QuestName.Custom)
                    {
                        filemanager.data = new DataStream.ReadToMemory();
                        filemanager.data.memory = new DataStream.MemoryStreamHandler();
                        filemanager.data.memory.ReadSaveFile(packet.r);
                    }
                    else
                    {
                        onNewMap();
                    }

                    refreshQuestStatus();
                    break;

                case PacketType.hqEnteredLobby:
                    {
                        netWriteStatus();
                        netWriteQuest();

                        var member = selectScreen.GetOrCreate(packet.sender);
                        if (member != null)
                        {
                            member.joining(false);
                        }
                    }
                    break;
                case PacketType.hqLobbyStatus:
                    netReadStatus(packet);
                    break;

                case PacketType.hqLobbyPlayerUpdate:
                    {
                        var member = selectScreen.GetOrCreate(packet.sender);
                        if (member != null)
                        {
                            if (member.remotePlayerPointer == null)
                            {
                                member.remotePlayerPointer = new RemotePlayerPointer(member.peer, false);
                            }
                            member.remotePlayerPointer.netRead(packet.r);
                        }
                    }
                    break;
            }
        }

        protected override bool DefaultMouseLock
        {
            get
            {
                return false;
            }
        }
    }
}
