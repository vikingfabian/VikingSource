using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VikingEngine.Input;
using VikingEngine.PJ.Display;
using VikingEngine.Network;
using VikingEngine.PJ.PjEngine;
using VikingEngine.PJ.Lobby;

namespace VikingEngine.PJ
{
    class LobbyState : AbsPJGameState
    {
        const int PublicSessionAutoStartTime = 20;
        const int PublicSessionJoinMinTime = 5;
                
        public ModeAvatarSetup avatarSetup;

        MenuSystem menusystem = null;
        LostControllerDisplay lostControllerDisplay = null;
        public List<GamerData> joinedRemoteGamerData = new List<GamerData>();
        List<JoinSuggestIcon> suggestJoinIcons = new List<JoinSuggestIcon>(2);
        List<Graphics.Image> availableJoinSpots = new List<Graphics.Image>(16);

        List<Graphics.Image> availableRemoteJoinSpots = new List<Graphics.Image>(PjLib.MaxRemotePlayers);
        List<RemoteGamerDisplay> joinedRemotes = new List<RemoteGamerDisplay>(PjLib.MaxRemotePlayers);

        SelectAnimalTutorial selectAnimalTutorial = null;

        Graphics.Image topBar;
        MenuButton startButton, menuButton;
        Graphics.Image startButtonLock;
        MenuButton windowToggleButton;
        OptionsGroup monitorOptions;
        SpriteText autoStartNumber;
        Timer.SecondsCountDown autoStartTimer;

        public bool QuickStart = false;
        bool mouseHasJoined = false;

        public List<DlcButton2> dlcButtons = new List<DlcButton2>(2);

        Timer.Basic adsTimer = new Timer.Basic(10000, true);
        //Time saveSettings = Time.Zero;

        ModeDisplay modeDisplay;

        Display.OptionsGroup lobbyPublicityOptions;
        Graphics.Image lobbyPublicityDesc;
        Graphics.ImageGroup lobbyPublicityImages;

        Timer.Basic searchLobbiesTimer = new Timer.Basic(TimeExt.SecondsToMS(15), true);
        AvailableSessionsDisplay availableSessionsDisplay;

        int sendJoinedGamersStatus = 0;
        Vector2 monitorOptionSz;
        bool isMeatPie = false;

        XInputJoinHandler xInputJoinHandler = new XInputJoinHandler();
        //static bool FirstTimeLoad = false;

        public LobbyState(bool host = true)
            : base(false)
        {
            PjRef.host = host;

            searchLobbiesTimer.TimeLeft = TimeExt.SecondsToMS(3);
            PjRef.LobbySong.PlayStored();

            Input.Mouse.Visible = true;
            this.joinedLocalGamers = new List2<GamerData>();
            refreshControllersInUse();

            Ref.draw.ClrColor = PjLib.ClearColor;
            //Background
            const float SkySzAdd = 20;
            Graphics.Image sky = new Graphics.Image(SpriteName.birdSkyTex, new Vector2(-SkySzAdd), Engine.Screen.ResolutionVec + new Vector2(SkySzAdd * PublicConstants.Twice), ImageLayers.Background9);
            sky.Opacity = 0.2f;

            initTopBar();
            initBottomMenu();

            //refreshAvailableCharacters();
            avatarSetup = new ModeAvatarSetup();
            refreshDlcButtons();
            refreshInputIcons();

            //refreshPlayerCount(true);
            generateStartPositions();

            //Black fade
            PjLib.BlackFade(true);

            if (Ref.netSession.ableToConnect)
            {
                if (host)
                {
                    Ref.netSession.setLobbyJoinable(true);//Ref.steam.LobbyMatchmaker.setJoinable(true);
                    availableSessionsDisplay = new AvailableSessionsDisplay(topBar.Height);

                    if (PjRef.hasAllContentDLC)
                    {
                        Ref.netSession.LobbyPublicity = PjRef.storage.lobbyPublicity;//Ref.steam.LobbyMatchmaker.lobbyPublicity = PjRef.storage.lobbyPublicity;

#if PCGAME
                        Ref.steam.LobbyMatchmaker.CreateLobbyIfNotInOne();
#endif
                    }
                    PjRef.PublicNetwork = PjRef.storage.lobbyPublicity == LobbyPublicity.Public;
                }
                else
                {
                    Ref.netSession.BeginWritingPacketToHost(Network.PacketType.birdClientJoinedLobby, Network.PacketReliability.Reliable, null);
                }
            }
            netWriteJoinedGamers();

            PjLib.checkHostStatus();
            lobbyPublicityDesc = new Graphics.Image(SpriteName.birdLobbyVisibilityHidden,
                new Vector2(Engine.Screen.CenterScreen.X, Engine.Screen.SafeArea.Bottom - Engine.Screen.IconSize * 2f),
                Engine.Screen.IconSize * 0.9f * new Vector2(10, 4), ImageLayers.Foreground4);
            lobbyPublicityDesc.origo = new Vector2(0.5f, 1f);

            checkModeAvailable();

            if (PjRef.storage.previousVictor != null && PjRef.storage.previousVictor.joustAnimal == JoustAnimal.MrW)
            {
                meatpieSetup();
            }
            //onModeChanged();
        }

        void initTopBar()
        {
            topBar = new Graphics.Image(SpriteName.WhiteArea, new Vector2(-20), new Vector2(Engine.Screen.Width + 40, Engine.Screen.Height * 0.25f), ImageLayers.Background8);
            topBar.Color = new Color(13, 28, 44);

            modeDisplay = new ModeDisplay(this, topBar.Bottom);

            float titleH = Engine.Screen.Height * 0.12f;
            Graphics.Image PartyJoustingTitle = new Graphics.Image(SpriteName.pjTitleText,
                Engine.Screen.SafeArea.CenterTop,
                new Vector2(titleH / SpriteSheet.PjTitleTextSz.Y * SpriteSheet.PjTitleTextSz.X, titleH),
                ImageLayers.Background6);
            PartyJoustingTitle.LayerAbove(topBar);
            PartyJoustingTitle.OrigoAtCenterWidth();

            Graphics.Text2 vikingfabianText = new Graphics.Text2("vikingfabian.com",
                LoadedFont.Regular, PartyJoustingTitle.LeftBottom,
                titleH * 0.2f,
                Color.Gray, ImageLayers.AbsoluteBottomLayer);
            vikingfabianText.OrigoAtCenterWidth();
            //Engine.Screen.TextBreadSize, Graphics.Align.CenterWidth, Color.Gray, ImageLayers.AbsoluteBottomLayer);
            vikingfabianText.LayerAbove(topBar);

#if XBOX
            Ref.xbox.presence.Set("lobby");
#endif
        }

        

        void initBottomMenu()
        {
            Vector2 bigButtonsSize = HudLib.BigButtonsSize;//new Vector2(Engine.Screen.IconSize * 2f);
            Vector2 inputButtonSize = new Vector2(Engine.Screen.SmallIconSize);

            IButtonMap menuInput, startInput, modeInput;
            HudLib.HudInputDisplay(out menuInput, out startInput, out modeInput);

            {//MENU
                VectorRect area = new VectorRect(Engine.Screen.SafeArea.X, Engine.Screen.SafeArea.Bottom - bigButtonsSize.Y,
                    bigButtonsSize.X, bigButtonsSize.Y);
                menuButton = new MenuButton(area, true, SpriteName.pjMenuIcon, HudLib.LayButtons, HudLib.LargeButtonSettings);

                //IButtonMap menuInput;
                //if (PjRef.HostingPlayerSource.sourceType == InputSourceType.XController)
                //{
                //    menuInput = new XboxButtonMap(Buttons.Back, 0);
                //}
                //else
                //{
                //    menuInput = new KeyboardButtonMap(Keys.Escape);
                //}
                menuButton.addInputIcon(Dir4.E, menuInput);
            }

            {//START
                VectorRect area = new VectorRect(Engine.Screen.SafeArea.Right - bigButtonsSize.X, Engine.Screen.SafeArea.Bottom - bigButtonsSize.Y,
                    bigButtonsSize.X, bigButtonsSize.Y);
                startButton = new MenuButton(area, true, SpriteName.pjPlayIcon, HudLib.LayButtons, HudLib.LargeButtonSettings);

                //IButtonMap startInput;
                //if (PjRef.HostingPlayerSource.sourceType == InputSourceType.XController)
                //{
                //    startInput = new XboxButtonMap(Buttons.Start, 0);
                //}
                //else
                //{
                //    startInput = new KeyboardButtonMap(Keys.Enter);
                //}
                startButton.addInputIcon(Dir4.W, startInput);

                startButton.Enabled = false;
            }

            if (!PjRef.XboxLayout)
            {//Quick OPTIONS
                Vector2 buttonSz = bigButtonsSize * 0.6f;
                monitorOptionSz = buttonSz * 0.7f;

                VectorRect area = new VectorRect(menuButton.area.Right + bigButtonsSize.X, Engine.Screen.SafeArea.Bottom - buttonSz.Y, buttonSz.X, buttonSz.Y);
                windowToggleButton = new MenuButton(area, true, SpriteName.NO_IMAGE, HudLib.LayButtons, HudLib.ButtonSettings);
                windowToggleButton.iconScaleDown = 0.2f;
                windowToggleButton.refreshIconSz();


                monitorOptions = new OptionsGroup();

                if (Engine.Screen.PcTargetFullScreen == false &&
                    Engine.Screen.MonitorCount > 1)
                {

                    Vector2 pos = new Vector2(area.Right + Engine.Screen.BorderWidth, area.Bottom - monitorOptionSz.Y);

                    monitorOptions.buttons = new MenuOptionButton[Engine.Screen.MonitorCount];

                    int selected = Engine.Screen.OnMonitorIndex;

                    for (int i = 0; i < monitorOptions.buttons.Length; ++i)
                    {
                        MenuOptionButton btn = new MenuOptionButton(new VectorRect(pos, monitorOptionSz), SpriteName.MenuIconMonitorFrame);
                        monitorOptions.buttons[i] = btn;
                        Graphics.TextG number = new Graphics.TextG(LoadedFont.Regular,
                            btn.iconImg.Area.PercentToPosition(new Vector2(0.5f, 0.42f)), Vector2.One,
                            Graphics.Align.CenterAll, TextLib.IndexToString(i),
                            Color.White, ImageLayers.AbsoluteBottomLayer);
                        number.LayerAbove(btn.iconImg);
                        number.SetHeight(btn.iconImg.Height * 0.6f);

                        pos.X += monitorOptionSz.X * 1f;
                    }

                    monitorOptions.select(selected);
                }

                refreshScreenOptionsButtons();
            }
            startButtonLock = new Graphics.Image(SpriteName.birdLock, startButton.area.Center, startButton.area.Size * 0.9f, ImageLayers.AbsoluteBottomLayer, true);
            startButtonLock.LayerAbove(startButton.iconImg);

            autoStartNumber = new SpriteText("", startButton.area.Center, startButton.area.Height * 0.7f, ImageLayers.Lay3, VectorExt.V2Half, Color.White, true);
            autoStartNumber.SetVisible(false);
        }

        void refreshScreenOptionsButtons()
        {
            windowToggleButton.iconImg.SetSpriteName(Engine.Screen.PcTargetFullScreen ? SpriteName.MenuIconMonitorArrowsIn : SpriteName.MenuIconMonitorArrowsOut);
        }


        void refreshNetworkDisplays()
        {
            bool hasNetwork = Ref.netSession.ableToConnect &&
                PjRef.storage.modeSettings.hasNetwork &&
                PjRef.hasAllContentDLC;

            createlobbyPublicityOptions(hasNetwork && PjRef.host);
        }

        void createlobbyPublicityOptions(bool add)
        {
            if (add && lobbyPublicityOptions == null)
            {
                lobbyPublicityOptions = new OptionsGroup();
                lobbyPublicityOptions.buttons = new Display.MenuOptionButton[3];

                lobbyPublicityImages = new Graphics.ImageGroup(lobbyPublicityOptions.buttons.Length);

                Vector2 buttonSz = windowToggleButton.area.Size;
                Vector2 buttonSpacing = new Vector2(Engine.Screen.IconSize * 0.1f);
                Vector2 centerBottom = new Vector2(Engine.Screen.CenterScreen.X, Engine.Screen.SafeArea.Bottom);

                Vector2 topLeft = new Vector2(
                    Table.CenterTableWidth(centerBottom.X, buttonSz.X, buttonSpacing.X, lobbyPublicityOptions.buttons.Length),
                    centerBottom.Y - buttonSz.Y);

                for (int i = 0; i < lobbyPublicityOptions.buttons.Length; ++i)
                {
                    SpriteName icon = SpriteName.NO_IMAGE;
                    switch (i)
                    {
                        case 0:
                            icon = SpriteName.birdNoNetwork;
                            break;
                        case 1:
                            icon = SpriteName.birdFriendsNetwork;
                            break;
                        case 2:
                            icon = SpriteName.birdAnyoneNetwork;
                            break;
                    }

                    var area = Table.CellPlacement(topLeft, false, i, lobbyPublicityOptions.buttons.Length, buttonSz, buttonSpacing);
                    var opt = new Display.MenuOptionButton(area, icon);

                    lobbyPublicityOptions.buttons[i] = opt;
                }

                refreshLobbyPublicity();
            }

            if (!add && lobbyPublicityOptions != null)
            {
                lobbyPublicityOptions.Clear();
                lobbyPublicityOptions = null;
                lobbyPublicityImages.DeleteAll();
                lobbyPublicityImages = null;
            }
        }

        void lobbyPublicityOptionsClick(int index)
        {
            PjRef.storage.lobbyPublicity = (LobbyPublicity)index;
            refreshLobbyPublicity();
            Ref.netSession.LobbyPublicity = PjRef.storage.lobbyPublicity;//Ref.steam.LobbyMatchmaker.SetLobbyPublicity(PjRef.storage.lobbyPublicity);
            PjRef.PublicNetwork = PjRef.storage.lobbyPublicity == LobbyPublicity.Public;

            netwriteLobbyStatus();
        }

        void refreshLobbyPublicity()
        {
            lobbyPublicityOptions.select((int)PjRef.storage.lobbyPublicity);
        }

        void debugFinalScore()
        {
            tryAddLocalGamer(new KeyboardButtonMap(Keys.Left));
            tryAddLocalGamer(new KeyboardButtonMap(Keys.Right));

            foreach (var m in joinedLocalGamers)
            {
                m.coins = Ref.rnd.Int(0, 99);
                m.Victories = Ref.rnd.Int(0, 9);
            }

            PjRef.storage.joinedGamersSetup = joinedLocalGamers;
            new FinalScoreState();
        }


        void onModeChanged()
        {
            modeDisplay.refreshModeVisuals();

            avatarSetup.onModeChanged();
            refreshLocalGamersDisplay();
            refreshRemoteGamersDisplay();
            refreshNetworkDisplays();

            bool canPlay;
            canViewMode(PjRef.storage.Mode, out canPlay);

            if (startButtonLock != null)
            {
                startButtonLock.Visible = !canPlay;
            }
        }

        protected override void createDrawManager()
        {
            draw = new VikingEngine.Engine.Draw2D();
        }

        public void onDlcChanged()
        {
            openMenu(InputSource.DefaultPC);
            menusystem.dlcChangedMenu();
        }


        void generateStartPositions()
        {
            List<Vector2> startPositions = new List<Vector2>(2 * 5 + 2 * 4);
            Joust.Gamer.Init();

            float xShiftPerRow = Joust.Gamer.ImageScale * 0.28f;
            float xSxift = -xShiftPerRow * 2f;
            VectorRect area = Engine.Screen.Area;

            area.AddWidth(-area.Width * 0.34f);
            area.AddHeight(-area.Height * 0.52f);
            area.Y -= area.Height * 0.12f;

            for (int y = 0; y < 4; ++y)
            {

                int cols = lib.IsEven(y) ? 5 : 4;
                float colAdj = cols == 5 ? 0 : 0.5f;

                for (int x = 0; x < cols; ++x)
                {
                    Vector2 pos = new Vector2(
                        area.X + (area.Width / 4f) * (x + colAdj) + xSxift,
                        area.Y + (area.Height / 3f) * y);

                    startPositions.Add(pos);
                }

                xSxift += xShiftPerRow;
            }

            PjRef.StartPositions = new List<Vector2>(startPositions.Count);
            while (startPositions.Count > 0)
            {
                PjRef.StartPositions.Add(arraylib.RandomListMemberPop<Vector2>(startPositions));
            }
        }

        public void meatpieSetup()
        {
            PjRef.storage.joinedGamersSetup.Clear();

            isMeatPie = true;
            PjRef.storage.previousVictor = null;
            avatarSetup.availableJoustAnimals = new List<JoustAnimal>(18);
            for (JoustAnimal ja = JoustAnimal.MeatPie1; ja <= JoustAnimal.MeatPie18; ++ja)
            {
                avatarSetup.availableJoustAnimals.Add(ja);
            }

            PjRef.storage.Mode = PartyGameMode.MeatPie;
            onModeChanged();
        }

        void refreshDlcButtons()
        {
            PjRef.RefreshDlcStatus();

            arraylib.DeleteAndClearArray(dlcButtons);
            Vector2 dlcSz = monitorOptionSz;
            float dlcSpacing = Engine.Screen.BorderWidth;
            Vector2 start = new Vector2(startButton.area.X - Table.TotalWidth(3, dlcSz.X, dlcSpacing) - Engine.Screen.IconSize * 2f,
                Engine.Screen.SafeArea.Bottom - dlcSz.Y);

            int ix = 0;

            if (PjRef.Dlc1Characters)
            {
                VectorRect area = Table.CellPlacement(start, false, ix++, int.MaxValue, dlcSz, new Vector2(dlcSpacing));
                DlcButton2 button = new DlcButton2(SpriteName.DlcBoxPrime, area, "DLC1 - Full Game Unlock");
                dlcButtons.Add(button);
            }
            if (PjRef.Dlc2BETA)
            {
                VectorRect area = Table.CellPlacement(start, false, ix++, int.MaxValue, dlcSz, new Vector2(dlcSpacing));
                DlcButton2 button = new DlcButton2(SpriteName.DlcBoxBling, area, "DLC2 - Bling pack");
                dlcButtons.Add(button);
            }
            if (PjRef.DlcZombie)
            {
                VectorRect area = Table.CellPlacement(start, false, ix++, int.MaxValue, dlcSz, new Vector2(dlcSpacing));
                DlcButton2 button = new DlcButton2(SpriteName.DlcBoxZombie, area, "DLC3 - Zombie");
                dlcButtons.Add(button);
            }
        }

        void refreshLocalGamersDisplay()
        {
            GamerData.SetLeftRightTeams(joinedLocalGamers);

            joinedLocalGamers.loopBegin();
            while (joinedLocalGamers.loopNext())//foreach (var m in joinedLocalGamers)
            {
                joinedLocalGamers.sel.lobbyAvatar.refreshVisuals();
                joinedLocalGamers.sel.lobbyAvatar.refreshIsLast(joinedLocalGamers.IsLast);
            }

            foreach (var ic in suggestJoinIcons)
            {
                ic.DeleteMe();
            }
            suggestJoinIcons.Clear();

            Range limit = localPlayersCountLimits();

            if (joinedLocalGamers.Count < limit.Max)
            {
                int count = joinedLocalGamers.Count == 0 ? limit.Min : 1;

                for (int i = 0; i < count; ++i)
                {
                    JoinSuggestIcon icon = new JoinSuggestIcon(joinedLocalGamers.Count + i);
                    suggestJoinIcons.Add(icon);
                }
            }

            arraylib.DeleteAndClearArray(availableJoinSpots);

            for (int i = joinedLocalGamers.Count + suggestJoinIcons.Count; i < limit.Max; ++i)
            {
                var area = LobbyAvatar.GamerIconPlacement(i);
                area.AddRadius(-0.11f * area.Width);

                Graphics.Image shadowSquare = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.Background4);
                shadowSquare.Color = new Color(125, 125, 125);
                shadowSquare.Opacity = JoinSuggestIcon.FrameOpacity;
                availableJoinSpots.Add(shadowSquare);
            }
        }

        Range localPlayersCountLimits()
        {
            Range count = PjRef.storage.modeSettings.localPlayerRange;//PjLib.ModeSettings(PjRef.storage.mode).localPlayerRange;

            if (joinedRemotes.Count > 0)
            {
                if (PjRef.PublicNetwork)
                {
                    count = new Range(1, 1);
                }
                else
                {
                    count = new Range(1, 4);
                }
            }

            while (count.Max < joinedLocalGamers.Count)
            {
                removeFromLobby(arraylib.Last(joinedLocalGamers));
            }

            return count;
        }

        public void CloseMenu()
        {
            if (menusystem != null)
            {
                menusystem.DeleteMe();
                menusystem = null;
            }
        }

        public MenuSystem openMenu(Input.InputSource menuUser)
        {
            menusystem = new MenuSystem(this, menuUser);

            return menusystem;
        }

        public void Reset()
        {
            PjRef.storage.joinedGamersSetup = joinedLocalGamers;
            new LobbyState();
        }

        public void refreshPlayerCount(bool local)
        {
            refreshLocalGamersDisplay();
            refreshControllersInUse();

            if (PjRef.host)
            {
                startButton.Enabled = CanManuallyStartMode;
            }

            if (local)
            {
                onLocalJoinedGamerChange();
            }
        }

        bool CanStartMode
        {
            get { return
                startButtonLock.Visible == false &&
                joinedLocalGamers.Count + joinedRemotes.Count >= PjRef.storage.modeSettings.localPlayerRange.Min; }
        }

        bool CanManuallyStartMode
        {
            get
            {
                if (publicSessionSetup())
                    return false;
                else
                    return CanStartMode;
            }
        }

        public void onLocalJoinedGamerChange()
        {
            sendJoinedGamersStatus = 10;
        }

        void netWriteJoinedGamers()
        {
            PjLib.NetWriteJoinedGamers(joinedLocalGamers);
        }

        void netReadJoindedGamers(Network.ReceivedPacket packet)
        {
            PjLib.NetReadJoindedGamers(packet);

            refreshRemoteGamersDisplay();
            refreshPlayerCount(false);
        }

        List<Player.RemoteGamerData> listRemoteGamers()
        {
            var remotes = Ref.netSession.RemoteGamers();
            List<Player.RemoteGamerData> result = new List<Player.RemoteGamerData>(remotes.Count);

            foreach (var m in remotes)
            {
                result.Add(PjLib.GetRemote(m));
            }

            return result;
        }

        public override void NetworkReadPacket(Network.ReceivedPacket packet)
        {
            base.NetworkReadPacket(packet);
            switch (packet.type)
            {
                case Network.PacketType.birdJoinedGamers:
                    if (packet.sender.Tag == null)
                    {
                        packet.sender.Tag = new Player.RemoteGamerData(packet.sender);
                        netWriteJoinedGamers();
                    }
                    netReadJoindedGamers(packet);
                    break;
                case Network.PacketType.birdBeginLoadScreen:
                    start(false);
                    break;

                case Network.PacketType.birdClientJoinedLobby:
                    if (PjRef.host)
                    {
                        netwriteLobbyStatus();
                    }
                    break;
                case Network.PacketType.birdLobbyStatus:
                    if (!PjRef.host)
                    {
                        PjRef.PublicNetwork = packet.r.ReadBoolean();
                        int secondsLeft = packet.r.ReadByte();

                        if (publicSessionSetup())
                        {
                            autoStartNumber.Text(secondsLeft.ToString());
                        }
                    }
                    break;
            }
        }

        void netwriteLobbyStatus()
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdLobbyStatus, Network.PacketReliability.Reliable);
            w.Write(PjRef.PublicNetwork);
            w.Write((byte)autoStartTimer.secondsLeft);
        }

        public override void NetEvent_ConnectionLost(string reason)
        {
            if (PjRef.host == false)
            {
                new ExitToLobbyState(0, true);
            }
        }

        public override void NetEvent_PeerJoined(Network.AbsNetworkPeer gamer)
        {
            base.NetEvent_PeerJoined(gamer);
            availableSessionsDisplay.removeSearch();

            PjLib.checkHostStatus();

            if (publicSessionSetup() && PjRef.host)
            {
                //Reset timer to gice extra time to the new player
                if (autoStartNumber.GetVisible())
                {
                    if (autoStartTimer.secondsLeft <= PublicSessionJoinMinTime)
                    {
                        autoStartTimer.set(PublicSessionJoinMinTime);
                    }
                }
            }
        }



        public override void NetEvent_PeerLost(Network.AbsNetworkPeer gamer)
        {
            base.NetEvent_PeerLost(gamer);
            refreshRemoteGamersDisplay();
        }

        void refreshRemoteGamersDisplay()
        {
            joinedRemoteGamerData.Clear();
            arraylib.DeleteAndClearArray(joinedRemotes);

            if (Ref.netSession.ableToConnect)//.steam.isInitialized)
            {
                foreach (var m in Ref.netSession.RemoteGamers())
                {
                    var group = PjLib.GetRemote(m);
                    if (group != null)
                    {
                        for (int localIx = 0; localIx < group.joinedGamers.Count; ++localIx)
                        {
                            GamerData gdata = group.joinedGamers[localIx];
                            var display = new RemoteGamerDisplay(joinedRemotes.Count, m, localIx, gdata);
                            joinedRemotes.Add(display);

                            joinedRemoteGamerData.Add(gdata);
                        }
                    }
                }
            }


            bool viewAvailableRemoteJoin =
                (
                PjRef.hasAllContentDLC &&
                PjRef.storage.lobbyPublicity != LobbyPublicity.Private &&
                PjRef.storage.modeSettings.hasNetwork
                ) ||
                joinedRemotes.Count > 0;

            arraylib.DeleteAndClearArray(availableRemoteJoinSpots);

            if (viewAvailableRemoteJoin)
            {
                for (int i = joinedRemotes.Count; i < PjLib.MaxRemotePlayers; ++i)
                {
                    var area = JoinedRemoteGamerIcon.GamerIconPlacement(i);

                    Graphics.Image shadowSquare = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.Background4);
                    shadowSquare.Opacity = 0.2f;
                    availableRemoteJoinSpots.Add(shadowSquare);
                }
            }
            checkIfModeConflictsWithMultiplayer();
        }

        public void checkIfModeConflictsWithMultiplayer()
        {
            if (Ref.netSession.InMultiplayerSession)
            {
                while (PjRef.storage.modeSettings.hasNetwork == false)
                {
                    nextMode(1);
                }
            }
        }

        public override void Time_Update(float time)
        {
            if (UpdateCount == 1)
            {
                PjLib.MemoryCleanUp();
            }
            else if (UpdateCount == 100)
            {
#if PCGAME
                if (Ref.steam.statsInitialized)
                {
                    Ref.steam.stats.upload();
                }
                if (Ref.steam.leaderBoards != null)
                {
                    Ref.steam.leaderBoards.uploadlastplayed();
                }
#endif
            }

            base.Time_Update(time);

            if (lostControllerDisplay != null)
            {
                updateLostDisplay();
            }
            else if (menusystem != null)
            {
                if (menusystem.menu.Update() ||
                    MenuSystem.CloseMenuInput())
                {
                    CloseMenu();
                }
            }
            else
            {
                updateJoinInput();

                bool startInput;
                bool menuInput;
                InputSource menuUser;

                PjLib.UpdateManagerInput(out startInput, out menuInput, out menuUser);
                updateLostControllers();

                menuInput |= menuButton.update();

                if (windowToggleButton != null)
                {
                    if (windowToggleButton.update())
                    {
                        Ref.gamesett.fullscreenProperty(0, true, !Engine.Screen.PcTargetFullScreen);
                    }
                    if (monitorOptions.update())
                    {
                        //Ref.gamesett.setMonitorIndex(monitorOptions.selected);
                    }
                }

                startInput |= startButton.update();


                if (menuInput)
                {
                    openMenu(menuUser);
                }

                if (PjRef.host && startButton.Enabled && startInput)
                {
                    start(true);
                }

                lobbyPublicityDesc.Visible = false;
                if (PjRef.host)
                {
                    modeDisplay.update();
                    updateLobbyPublicityOptions();
                }

                foreach (var m in dlcButtons)
                {
                    m.update();
                }
            }

            updateNetLobbySearch();

            updatePublicNetSession();

            if (sendJoinedGamersStatus > 0)
            {
                if (--sendJoinedGamersStatus <= 0)
                {
                    netWriteJoinedGamers();
                }
            }
            
            if (PlatformSettings.DevBuild)
            {
                //if (Input.Keyboard.KeyDownEvent(Keys.LeftControl))
                //{
                //    debugFinalScore();
                //}
#if XBOX
                if (Input.XInput.Instance(0).KeyDownEvent(Buttons.A))
                {
                    Ref.xbox.presence.Set("golf");
                }
                if (Input.XInput.Instance(0).KeyDownEvent(Buttons.B))
                {
                    Ref.xbox.presence.Set("joust");
                }
                if (Input.XInput.Instance(0).KeyDownEvent(Buttons.X))
                {
                    Ref.xbox.presence.Set("lobby");
                }
#endif
                if (PjLib.DebugAutoStartMode != null)
                {
                    PjRef.storage.Mode = PjLib.DebugAutoStartMode.Value;
                    avatarSetup.onModeChanged();
                    tryAddLocalGamer(new Input.KeyboardButtonMap(Keys.Down));
                    tryAddLocalGamer(new Input.KeyboardButtonMap(Keys.Right));

                    start(true);
                }
            }
        }

        void updateLobbyPublicityOptions()
        {
            if (lobbyPublicityOptions != null)
            {
                if (lobbyPublicityOptions.update())
                {
                    lobbyPublicityOptionsClick(lobbyPublicityOptions.selected);
                }

                if (lobbyPublicityOptions.mouseOver >= 0)
                {
                    lobbyPublicityDesc.Visible = true;
                    switch (lobbyPublicityOptions.mouseOver)
                    {
                        case 0:
                            lobbyPublicityDesc.SetSpriteName(SpriteName.birdLobbyVisibilityHidden);
                            break;
                        case 1:
                            lobbyPublicityDesc.SetSpriteName(SpriteName.birdLobbyVisibilityFriends);
                            break;
                        case 2:
                            lobbyPublicityDesc.SetSpriteName(SpriteName.birdLobbyVisibilityAnyone);
                            break;
                    }
                }
            }
        }

        void updateNetLobbySearch()
        {
            if (PjRef.host)
            {
                if (searchLobbiesTimer.Update())
                {
                    if (Ref.netSession.ableToConnect)
                    {
#if PCGAME
                        if (Ref.steam.P2PManager.remoteGamers.Count == 0)
                        {
                            if (PjRef.hasAllContentDLC)
                            {
                                Ref.steam.LobbyMatchmaker.CreateLobbyIfNotInOne();
                            }

                            new Timer.TimedAction0ArgTrigger(searchLobbies, TimeExt.SecondsToMS(2f));
                            availableSessionsDisplay.startedSearch();
                        }
                        else
                        {
                            availableSessionsDisplay.removeSearch();
                        }
#endif
                    }
                }

                if (availableSessionsDisplay != null)
                {
                    availableSessionsDisplay.update();
                }
            }
        }


        void updateLostDisplay()
        {
            bool closeInput = false;

            if (lostControllerDisplay.created.msPassed(400))
            {
                closeInput = Input.InputLib.AnyKeyDownEvent();
            }

            if (lostControllerDisplay.update() || closeInput)
            {
                lostControllerDisplay.DelteMe(); lostControllerDisplay = null;
                removeDisconnectedGamers();
            }
        }

        void updatePublicNetSession()
        {
            if (publicSessionSetup())
            {
                if (autoStartNumber.GetVisible() == false)
                {
                    autoStartTimer = new Timer.SecondsCountDown(PublicSessionAutoStartTime);
                    autoStartNumber.Text(autoStartTimer.secondsLeft.ToString());
                }

                if (PjRef.host)
                {
                    if (autoStartTimer.update())
                    {
                        if (autoStartTimer.secondsLeft <= 0)
                        {
                            start(false);
                        }
                        else
                        {
                            autoStartNumber.Text(autoStartTimer.secondsLeft.ToString());
                            netwriteLobbyStatus();
                        }
                    }
                }

                if (joinedLocalGamers.Count != 1)
                {
                    localPlayersCountLimits();

                    setMin1LocalGamer();
                }
            }
            else
            {
                if (autoStartNumber.GetVisible())
                {
                    autoStartNumber.SetVisible(false);
                }
            }
        }

        protected override void onLostController()
        {
            if (lostControllerDisplay == null)
            {
                CloseMenu();
                lostControllerDisplay = new LostControllerDisplay();
            }
        }

        void removeDisconnectedGamers()
        {
            joinedLocalGamers.loopBegin(false);
            while (joinedLocalGamers.loopNext())
            {
                if (InputLib.Connected(joinedLocalGamers.sel.button) == false)
                {
                    //Create lost effect
                    VectorRect area = joinedLocalGamers.sel.lobbyAvatar.area;
                    Graphics.Image disconnectIcon = new Graphics.Image(SpriteName.DisconnectSquare,
                        area.Center, area.Size, ImageLayers.Top0, true);
                    
                    new Timer.Terminator(400, disconnectIcon);
                    removeFromLobby(joinedLocalGamers.sel);
                }
            }
        }

        void setMin1LocalGamer()
        {
            if (joinedLocalGamers.Count < 1)
            {
                IButtonMap button;
                if (PjRef.storage.joinedGamersSetup != null && PjRef.storage.joinedGamersSetup.Count > 0)
                {
                    button = PjRef.storage.joinedGamersSetup[0].button;
                }
                else
                {
                    button = new KeyboardButtonMap(Keys.Space);
                }
                tryAddLocalGamer(button);
            }
        }

        void checkModeAvailable()
        {
            bool canPlay;
            if (canViewMode(PjRef.storage.Mode, out canPlay))
            {
                onModeChanged();
            }
            else
            {
                nextMode(1);
            }
        }

        public void nextMode(int dir)
        {
            if (isMeatPie) return;

            bool canPlay;

            do
            {
                int index = arraylib.IndexFromValue(PjLib.ModeViewOrder, PjRef.storage.Mode);
                index = Bound.SetRollover(index + dir, 0, PjLib.ModeViewOrder.Length - 1);

                PjRef.storage.Mode = PjLib.ModeViewOrder[index];

            } while (canViewMode(PjRef.storage.Mode, out canPlay) == false);

            onModeChanged();
        }

        public bool canViewMode(PartyGameMode mode, out bool canPlay)
        {
            canPlay = true;

            if (mode == PartyGameMode.MeatPie)
            {
                return false;
            }

            var sett = new GameModeSettings(mode);

#if XBOX
            return sett.access >= GameModeAccessibility.Paid_4;
#else

            if (PlatformSettings.DevBuild)
            {
                return true;
            }

           

            if (PlatformSettings.DebugLevel == BuildDebugLevel.ShowDemo)
            {
                return sett.access >= GameModeAccessibility.DevAndDemo_2;
            }

            switch (sett.access)
            {
                case GameModeAccessibility.DevOnly_1:
                    return PlatformSettings.DevBuild;
                    
                case GameModeAccessibility.Beta_3:
                    PjRef.RefreshDlcStatus();
                    return PjRef.Dlc2BETA;

                case GameModeAccessibility.Paid_4:
                    PjRef.RefreshDlcStatus();
                    canPlay = PjRef.Dlc1Characters || PjRef.Dlc2BETA;
                    return true;

                default:
                    return true;
            }
#endif
        }

        void searchLobbies()
        {
#if PCGAME
            if (Ref.steam.P2PManager.remoteGamers.Count == 0)
            {
                Ref.steam.LobbyMatchmaker.FindLobbies();
            }
            else
            {
                availableSessionsDisplay.removeSearch();
            }
#endif
        }

        void removeGamerInput()
        {
            if (joinedLocalGamers.Count > 0)
            {
                removeFromLobby(arraylib.Last(joinedLocalGamers));
            }
        }

        void updateJoinInput()
        {
#if PCGAME
            for (int i = suggestJoinIcons.Count - 1; i >= 0; --i)
            {
                suggestJoinIcons[i].update(mouseHasJoined);
                suggestJoinIcons[i].highlight.Visible = false;

                //MOUSE JOIN
                if (!mouseHasJoined)
                {
                    if (suggestJoinIcons[i].area.IntersectPoint(Input.Mouse.Position))
                    {
                        suggestJoinIcons[i].highlight.Visible = true;

                        if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                        {
                            tryAddLocalGamer(new MouseButtonMap(MouseButton.Left));
                        }
                        else if (Input.Mouse.ButtonDownEvent(MouseButton.Right))
                        {
                            tryAddLocalGamer(new MouseButtonMap(MouseButton.Right));
                        }
                    }
                }
            }

            

            //KEYBOARD JOIN
            foreach (var k in Input.Keyboard.AllKeys)
            {
                if (Input.Keyboard.KeyDownEvent(k))
                {
                    if (k != Keys.Escape && 
                        k != Keys.Enter &&
                        k != Keys.Tab &&
                        k != Keys.Back &&
                        k != Keys.LeftShift &&
                        k != Keys.LeftWindows && 
                        k != Keys.RightWindows &&
                        k != Keys.VolumeUp && 
                        k != Keys.VolumeDown && 
                        k != Keys.VolumeMute &&
                        k != Keys.PrintScreen &&
                        k != Keys.F11 &&
                        k != Keys.F12)
                    {
                        tryAddLocalGamer(new KeyboardButtonMap(k));
                    }
                }
            }
            if (Input.Keyboard.KeyDownEvent(Keys.Back))
            {
                removeGamerInput();
            }
#endif
            

            //XBOX JOIN
            foreach (var xboxController in Input.XInput.controllers)
            {
                if (xboxController.Connected)
                {
                    foreach (var button in PjLib.XinputAvailableJoinButtons)
                    {
                        if (xboxController.KeyDownEvent(button))
                        {
                            tryAddLocalGamer(new XboxButtonMap(button, xboxController.Index));
                        }
                    }

                    if (xboxController.KeyDownEvent(Buttons.DPadLeft))
                    {
                        removeGamerInput();
                    }
                }
            }

            mouseHasJoined = false;
            for (int i = joinedLocalGamers.Count - 1; i >= 0; --i)
            {
                if (joinedLocalGamers[i].button.IsMouse)
                {
                    mouseHasJoined = true;
                }
                joinedLocalGamers[i].lobbyUpdate(this);
            }
        }

        public override void NetEvent_SessionsFound(List<AbsAvailableSession> availableSessions, List<AbsAvailableSession> prevAvailableSessionsList)
        {           
            base.NetEvent_SessionsFound(availableSessions, prevAvailableSessionsList);

            if (PjRef.hasAllContentDLC == false)
            {
                Ref.netSession.sortFriendsOnlyLobbies(availableSessions);
            }

            availableSessionsDisplay.listSessions(availableSessions);
        }

        public override void NetworkStatusMessage(Network.NetworkStatusMessage message)
        {
            base.NetworkStatusMessage(message);

            if (message == Network.NetworkStatusMessage.Found_No_Session)
            {
                availableSessionsDisplay.foundNoSessions();
            }
        }

        void tryAddLocalGamer(IButtonMap button)
        {
            if (publicSessionSetup() && 
                joinedLocalGamers.Count == 1 &&
                InputLib.Equals(joinedLocalGamers[0].button, button) == false)
            {
                //Replace the joined local player instead of adding more
                removeFromLobby(joinedLocalGamers[0]);
            }

            if (joinedLocalGamers.Count < localPlayersCountLimits().Max)
            {
                foreach (var g in joinedLocalGamers)
                {
                    if (InputLib.Equals(g.button, button))
                    {
                        //Already joined
                        return;
                    }
                }

                Debug.Log("Joined " + button.ButtonName);
                var newGamer = new GamerData(button, this);
                joinedLocalGamers.Add(newGamer);
                refreshPlayerCount(true);

                if (!PjRef.HasSetHost)
                {
                    PjRef.HasSetHost = true;
                    PjRef.HostingPlayerSource = new InputSource(button);
                }

                if (selectAnimalTutorial == null && PjRef.storage.modeSettings.avatarType == ModeAvatarType.Joust)
                {
                    selectAnimalTutorial = new SelectAnimalTutorial(newGamer, newGamer.lobbyAvatar, avatarSetup);
                }
                if (joinedLocalGamers.Count > 8 && !selectAnimalTutorial.IsDeleted)
                {
                    selectAnimalTutorial.DeleteMe();
                }
                if (joinedLocalGamers.Count == 1)
                {
                    updateHostingInput();
                }
            }
            else
            {
                //full
            }
        }

        void updateHostingInput()
        {
            PjRef.HostingPlayerSource = new InputSource(joinedLocalGamers[0].button);
            refreshInputIcons();
        }

        void refreshInputIcons()
        {
            IButtonMap menuInput, startInput, modeInput;
            HudLib.HudInputDisplay(out menuInput, out startInput, out modeInput);

            menuButton.refreshInputIcon(menuInput);
            startButton.refreshInputIcon(startInput);
            modeDisplay.nextInputIcon.SetSpriteName(modeInput.Icon);

        }

        public void removeFromLobby(GamerData gamer, bool bRefreshPositions = true)
        {
            gamer.lobbyAvatar.DeleteMe();
            joinedLocalGamers.Remove(gamer);

            if (bRefreshPositions)
            {
                refreshLobbyPositions();
                refreshPlayerCount(true);
            }
        }

        void refreshLobbyPositions()
        {
            for (int i = 0; i < joinedLocalGamers.Count; ++i)
            {
                joinedLocalGamers[i].refreshLobbyPositon(i, joinedLocalGamers.Count, avatarSetup);
            }
        }

        private bool start(bool manualStart)
        {           
            PjRef.storage.joinedGamersSetup = joinedLocalGamers;

            bool canStart = true;
            if (PjRef.host)
            {
                if (manualStart)
                {
                    canStart = CanManuallyStartMode;
                }
                else
                {
                    canStart = CanStartMode;
                }
            }
            else
            {
                setMin1LocalGamer();
            }

            if (Ref.netSession.InMultiplayerSession)
            {
                if (PjRef.storage.modeSettings.hasNetwork == false)
                {
                    canStart = false;
                }
            }

            if (canStart)
            {
                if (Ref.netSession.ableToConnect)
                {
                    PjRef.storage.startingRemoteGamers = new List<GamerData>(joinedRemotes.Count);
                    foreach (var m in Ref.netSession.RemoteGamers())
                    {
                        var group = PjLib.GetRemote(m);
                        if (group != null)
                        {
                            PjRef.storage.startingRemoteGamers.AddRange(group.joinedGamers);
                        }
                    }

#if PCGAME
                    if (joinedRemotes.Count > 0)
                    {
                        PjRef.stats.multiplayerSessions.value++;
                    }
#endif

                    Ref.netSession.setLobbyJoinable(false);
                }

                MediaPlayer.Stop();

                switch (PjRef.storage.Mode)
                {
                    case PartyGameMode.Jousting:
                        new Joust.JoustTutorialScreen(joinedLocalGamers);
                        break;
                    case PartyGameMode.Strategy:
                        new Strategy.StrategyPlayState(joinedLocalGamers);
                        break;
                    case PartyGameMode.CarBall:
                        new CarBall.CarBallTutorialScreen(joinedLocalGamers);
                        break;
                    case PartyGameMode.MiniGolf:
                        new MiniGolf.IntroState(joinedLocalGamers);
                        break;
                    case PartyGameMode.Bagatelle:
                        if (PjRef.host)
                        {
                            Bagatelle.BagLib.seed = Ref.rnd.Uint();
                        }
                        new Bagatelle.BagatelleTutorialScreen(joinedLocalGamers);
                        break;
                    case PartyGameMode.Tank:
                        //new RPG.RpgPlay(joinedLocalGamers);
                        new Tanks.TankPlayState(joinedLocalGamers);
                        break;
                    case PartyGameMode.SuperSmashBirds:
                        new SmashBirds.SmashGameState(joinedLocalGamers);
                        break;
                    case PartyGameMode.SpacePirate:
                        new SpaceWar.SpacePlayState(joinedLocalGamers);
                        break;

                    case PartyGameMode.Match3:
                        new Match3.Match3LoadState(joinedLocalGamers);
                        break;
                    case PartyGameMode.MoneyRoll:
                        new MoneyRoll.MoneyRollState();
                        break;
                    case PartyGameMode.MeatPie:
                        new ExitToLobbyState(1000, true);
                        PjRef.achievements.secretMeatPie.Unlock();
                        break;

                }
                PjRef.storage.saveLoad(true, true);

                return true;
            }
            else if (PjRef.host && startButtonLock.Visible)
            {
                PjLib.TryStartDlcPurchase(0);
            }

            return false;
        }

        public override void OnResolutionChange()
        {
            base.OnResolutionChange();

            if (Ref.gamestate is LobbyState)
            {
                Reset();

                if (menusystem != null)
                {
                    ((LobbyState)Ref.gamestate).openMenu(menusystem.menu.inputSource).options();
                }
            }
        }
        
        public bool publicSessionSetup()
        {
            return joinedRemotes.Count > 0 && PjRef.PublicNetwork;
        }
    }
}
