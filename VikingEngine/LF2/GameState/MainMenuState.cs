using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using Microsoft.Xna.Framework.Input;
using VikingEngine.LootFest;
using VikingEngine.HUD;
using VikingEngine.Network;
//

namespace VikingEngine.LF2
{

    class MainMenuState : GameState.AbsJoiningState//, IQuedObject, DataLib.IRemoveFolderCallback
    {
#if PCGAME
        string serverIp;
        //int port = 2056;
#endif
        const int WorldNameMaxChars = 16;
        protected bool startSearchingSessions = true;
        int numListedSessions = 0;
        protected Network.AbsNetworkPeer host;
        bool loadMainPage = true;
        protected bool inMainMenu = false;
        // protected HUD.AbsMenu menu;
        protected MenuSystem2 menuSystem = null;

        //HUD.MessageHandler messageHandler;
        Timer.Basic updateNetworkLobby = new Timer.Basic(8000);

        public Gui menu;

        public MainMenuState()
            : this(true)
        { }

        public MainMenuState(bool startSearchingSessions)
            : base()
        {
            //draw.SetDrawType(Ref.draw.RenderBasicOneLayer);
#if PCGAME
            //Engine.Input.SimulateLeftPad = true;
#endif
            this.startSearchingSessions = startSearchingSessions;
            DataLib.SaveLoad.ResetStreamIsOpen();
            Map.World.RunningAsHost = true;


            Vector2 descTextSize = new Vector2(1, 1.2f);

            draw.ClrColor = Color.Black;
            //Vector2 menuSize = new Vector2(LoadTiles.MenuBackgWidth * Menu.Scale, Engine.Screen.Height * 0.8f);

            //menu = new Menu(
            //    new VectorRect(Engine.Screen.SafeArea.Position,
            //    menuSize),VectorRect.ZeroOne, ImageLayers.Background5, int.NUM_NON);

            var style = new GuiStyle(
                (Screen.PortraitOrientation ? Screen.Width : Screen.Height) * 0.61f, 5, SpriteName.LFMenuRectangleSelection);
            menu = new Gui(style, Engine.Screen.SafeArea, 0f, LfLib.Layer_GuiMenu, Input.InputSource.DefaultPC);

            checkerBackg();

            //Game1.HUD.Message.BlockMessages = false;
            //Background
            draw.Camera = new Graphics.TopViewCamera(CamZoom, new Vector2(1.3f, CamTiltY));
            draw.Camera.FieldOfView = 40;
            draw.Camera.goalLookTarget = CamTargetCenter;
            new Timer.AsynchActionTrigger(StartQuedProcess, true);
            //Engine.Storage.AddToSaveQue(StartQuedProcess, false);

            //Network
            //messageHandler = new HUD.MessageHandler(3, SpriteName.LFMessageBackground, 2000, 
            //    Engine.Screen.SafeArea.Position + new Vector2(menuSize.X, 0), null);

            Ref.netSession.settings = new Network.Settings(Network.SearchAndCreateSessionsStatus.NON, 8000);
            //if (!Ref.netSession.InviteWaiting)
            //{
            //    Ref.netSession.Disconnect();

            //    if (startSearchingSessions)
            //        Ref.netSession.BeginUpdateAvailableSessions();
            //}

            if (loadMainPage)
            {
                GuiLayout layout = new GuiLayout("Main menu", menu);
                {
                    new GuiLabel("loading...", layout);
                }
                layout.End();
                //HUD.File file = new HUD.File();
                //file.AddDescription("loading...");
                //menu.File = file;

            }

            //Graphics.VoxelObjEffectBasicNormal preloadEffect = Graphics.VoxelObjEffectBasicNormal.Singleton;
            //if (preloadEffect == null)
            //    throw new Exception();

            loadBackground();
        }

        bool updateMenu = false;
        bool lockControls = true;

        void loadBackground()
        {
            draw.CurrentRenderLayer = 1;
            createMenuStateBackg(false);

            draw.CurrentRenderLayer = 0;
            Editor.MainMenuScene loadScene = (Editor.MainMenuScene)Ref.rnd.Int((int)Editor.MainMenuScene.NUM);
            Editor.Scene.SceneCollection scene = new Editor.Scene.SceneCollection(null, null);
            scene.load(loadScene.ToString(), false);
        }


        public static int CheckerSize;
        public static IntVector2 NumCheckerSquares;

        void checkerBackg()
        {
            const float SkyPercentHeight = 0.6f;

            draw.CurrentRenderLayer = 1;



            CheckerSize = (int)(Engine.Screen.Width * 0.02f);
            Vector2 sqSz = new Vector2(CheckerSize);
            NumCheckerSquares = new IntVector2(
                Engine.Screen.Width / CheckerSize,
                Engine.Screen.Height / CheckerSize) + 1;

            int groundLevel = (int)(SkyPercentHeight * NumCheckerSquares.Y);

            ForXYLoop loop = new ForXYLoop(NumCheckerSquares);
            //100, 149, 237
            ColorRange skyCol = new ColorRange(new Color(50, 76, 117), new Color(25, 35, 60));//Color.CornflowerBlue, Color.DarkBlue);//new ColorRange(Color.CornflowerBlue, Color.DarkBlue);
            ColorRange groundCol = new ColorRange(new Color(0, 0.2f, 0), Color.Black);


            while (loop.Next())
            {
                ColorRange colRange;
                float percentPos;
                if (loop.Position.Y < groundLevel || (loop.Position.Y == groundLevel && Ref.rnd.RandomChance(60)))
                {//Sky
                    colRange = skyCol;
                    percentPos = (float)loop.Position.Y / groundLevel;
                }
                else
                {//Ground
                    colRange = groundCol;
                    percentPos = (float)(loop.Position.Y - groundLevel) / (NumCheckerSquares.Y - groundLevel);
                }
                percentPos = Bound.Set(percentPos + Ref.rnd.Plus_MinusF(0.1f), 0, 1);
                Color rndCol = colRange.PercentPos(percentPos);

                Graphics.Image tile = new Graphics.Image(
                    SpriteName.WhiteArea, (loop.Position * CheckerSize).Vec, sqSz, ImageLayers.Background8);
                tile.Color = rndCol;

            }

            //Create a bunch of clouds
            for (int i = 0; i < 20; ++i)
            {
                new Effects.CheckerCloud(i);
            }

            draw.CurrentRenderLayer = 0;
        }

        const float CamTiltY = 1.47f;
        const float CamZoom = 38f;

        const float TiltRange = 0.3f;
        const float ZoomRange = 12f;
        static readonly Vector3 CamTargetCenter = new Vector3(0, 4, 0);
        float cameraY = 200;
        static readonly IntervalF ZoomBounds = IntervalF.FromCenter(CamZoom, ZoomRange);
        static readonly IntervalF TiltBounds = IntervalF.FromCenter(CamTiltY, TiltRange);
        Rotation1D camSinusMotion = Rotation1D.D0;



        void updateCamInput(float time)
        {
            const float TiltYSpeed = 0.001f;
            const float ZoomSpeed = 0.05f;

            //if (inputDialogue == null)
            {
                foreach (var ins in Input.XInput.controllers)
                //for (PlayerIndex i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
                {
                    //Input.AbsControllerInstance instance = Input.Controller.Instance(i);
                    if (ins.Connected)
                    {
                        draw.Camera.TiltY += ins.JoyStickValue(ThumbStickType.Right).DirAndTime.Y * TiltYSpeed;
                        if (ins.IsButtonDown(Buttons.LeftTrigger))
                        {
                            draw.Camera.CurrentZoom += time * ZoomSpeed;
                        }
                        else if (ins.IsButtonDown(Buttons.RightTrigger))
                        {
                            draw.Camera.CurrentZoom -= time * ZoomSpeed;
                        }
                    }
                }
            }
            draw.Camera.TiltY = Bound.Set(draw.Camera.TiltY, TiltBounds);
            draw.Camera.CurrentZoom = Bound.Set(draw.Camera.CurrentZoom, ZoomBounds);


            draw.Camera.TiltX -= 0.00016f * time;
            Rotation1D camrot = new Rotation1D(draw.Camera.TiltX);
            Vector3 target = CamTargetCenter;
            cameraY += 0.02f * (CamTargetCenter.Y - cameraY);
            target.Y = cameraY;
            Vector2 adj = camrot.Direction(-12);
            target.X = adj.X;
            target.Z = adj.Y;
            draw.Camera.LookTarget = target;

            draw.Camera.Time_Update(time);
        }

        public void StartQuedProcess()//bool saveThread)
        {
            Data.WorldsSummaryColl.Load();
            updateMenu = true;
            lockControls = false;

        }

        public override void OnDestroy()
        {
            //base.OnDestroy();
        }

        //public override void InviteAcceptedEvent()
        //{
        //    new GameState.Joining(Ref.netSession.invitedPlayer);
        //}
        //public override void NetworkStatusMessage(Network.NetworkStatusMessage message)
        //{
        //    if (messageHandler != null &&
        //        message != Network.NetworkStatusMessage.Need_to_log_in &&
        //        message != Network.NetworkStatusMessage.Searching_Session &&
        //        message != Network.NetworkStatusMessage.Found_No_Session &&
        //            message != Network.NetworkStatusMessage.Found_Session)
        //    {
        //        messageHandler.Print(TextLib.EnumName(message.ToString()));
        //    }
        //}
        void openMainPage()
        {
        //    OpenMainPage(false);
        //}
        //void OpenMainPage(bool listSessions)
        //{


            //const string ExitGame = "Exit to dashboard";
           // startSearchingSessions = true;
           //// int optionsBeforeSession;
           // int oldnumListedSessions = numListedSessions;
           // Vector2 selPos = Vector2.One * 5;
            //int selectedMemberY = 0;
            inMainMenu = true;
            //HUD.File mainPage = new HUD.File();


            //if (inMainMenu)
            //{
            //    selPos = menu.GetSelectionPos;
            //    selectedMemberY = menu.SelectedMember.Y;
            //}


            //if (Engine.XGuide.IsTrial)
            //{
            //    Engine.XGuide.UpdateTrialCheck();
            //    mainPage.AddTextLink("Run trial", Data.WorldsSummaryColl.savedWorlds.Count == 0? (int)MainMenuLink.NewWorld : (int)MainMenuLink.Continue);
            //    //mainPage.AddTextLink("Mini game", "Try the bonus game: \"Fabians little platformer\"", new HUD.Link(platformMiniGameTrial));
            //    mainPage.AddTextLink("Buy game", (int)MainMenuLink.BuyGame);

            //    mainPage.AddTextLink(ExitGame, (int)MainMenuLink.ExitGame);
            //    menu.File = mainPage;
            //}
            //else
            //{

            menu.PopAllLayouts();
            GuiLayout layout = new GuiLayout("Main menu", menu);
            {
                if (Data.WorldsSummaryColl.ContinueWorld == null)
                {
                    //optionsBeforeSession = 1;
                    newWorldButton(layout);
                }
                else
                {
                    //optionsBeforeSession = 2;

                    var info = TextLib.ListToString(Data.WorldsSummaryColl.ContinueWorld.LiveButtonInfo());

                    new GuiLargeTextButton("Continue " + Data.WorldsSummaryColl.ContinueWorld.WorldName, info, 
                        new GuiAction(linkContinue), false, layout);
                    new GuiTextButton("Select World", null, listWorlds, true, layout);
                    new GuiTextButton("Options", null, options, true, layout);
                    new GuiTextButton("Credits", null, credits, true, layout);                    
                    new GuiTextButton("Exit", null, Ref.update.Exit, false, layout);

                }
            }
            layout.End();

          

            //if (listSessions)
            //{
            //    numListedSessions = NetworkHandler.ListSessions(mainPage, true);
            //}
            //else
            //    numListedSessions = 0;

            //if (PlatformSettings.TargetPlatform == ReleasePlatform.Xbox &&
            //    numListedSessions == 0 && Ref.netSession.AbleToSearchSessions)
            //{
            //    mainPage.AddDescription("Looking for online games");
            //}

            //if (PlatformSettings.DebugOptions || PlatformSettings.PC_platform)
            //{
            //    mainPage.AddTextLink("AnimDesign", "The animation designer", (int)MainMenuLink.AnimationDesign);

            //    if (PlatformSettings.ViewUnderConstructionStuff)
            //        mainPage.AddTextLink("Scene maker beta", "Arrange models to a scene", (int)MainMenuLink.SceneMaker);
            //}

            //mainPage.AddTextLink("Cinematic trailer", new HUD.ActionLink(openTrailer));

            // mainPage.AddTextLink("Mini game", "Play \"Fabians little platformer\"", new HUD.Link(selectPlatformMiniGameLevel));

            //mainPage.AddTextLink("Credits", new HUD.ActionLink(credits));

            //if (PlatformSettings.TargetPlatform == ReleasePlatform.Xbox)
            //    mainPage.Checkbox2("System Link", "Needs to be turned off if you want to play over the internet", 0);
            //if (PlatformSettings.DebugOptions)
            //    mainPage.AddTextLink("*Browse files*", "debug only", new HUD.ActionLink(browseFiles));

            //mainPage.AddTextLink(ExitGame, (int)MainMenuLink.ExitGame);


            //menu.File = mainPage;

            //if (selectedMemberY > optionsBeforeSession)
            //{
            //    selPos.Y += (numListedSessions - oldnumListedSessions) * Menu.MenuBlockHeight;
            //}
            //menu.SetSelectionPos(selPos);
            //}
        }

        //ConnectionLost(string endReason)

        void listWorlds()
        {
            GuiLayout layout = new GuiLayout("Worlds", menu);
            {
                newWorldButton(layout);

                for (int i = Data.WorldsSummaryColl.savedWorlds.Count - 1; i >= 0; i--)
                {
                    var world = Data.WorldsSummaryColl.savedWorlds[i];

                    if (!world.Removed)
                    {
                        var info = TextLib.ListToString(world.LiveButtonInfo());

                        new GuiTextButton(world.WorldName, info,
                            new GuiActionIndex(editWorld, i), true, layout);
                    }
                }
            }
            //layout.End();

            //HUD.File wFile = new HUD.File();
            //wFile.AddTextLink("New World", (int)MainMenuLink.NewWorld);
            //for (int i = Data.WorldsSummaryColl.savedWorlds.Count - 1; i >= 0; i--)
            //{
            //    if (!Data.WorldsSummaryColl.savedWorlds[i].Removed)
            //        LiveButton(wFile, new HUD.Link((int)MainMenuLink.EditWord, i),
            //            new List<string> { Data.WorldsSummaryColl.savedWorlds[i].WorldName },
            //            Data.WorldsSummaryColl.savedWorlds[i].LiveButtonInfo());
            //}

            //menu.File = wFile;
        }

        void newWorldButton(GuiLayout layout)
        {
            new GuiLargeTextButton("New World", null, new GuiAction(newWorld), false, layout);
        }

#if PCGAME
        //public override void TextInputEvent(ref string input)
        //{
        //    HUD.TextEditor.LastUsedTextEditor.UpdateText(input);
        //    serverIp = input;
        //}
#endif
#if PCGAME
        //mainPage.AddDescription("Network port");
        //mainPage.AddTextEditor(Ref.netSession.port.ToString(), (int)MainMenuLink.NetworkPort);
        //mainPage.AddTitle("Connect to server");
        //mainPage.AddDescription("Host IP");
        //mainPage.AddTextEditor("", (int)MainMenuLink.ConnectToIp);
        //mainPage.AddTextLink("Connect", (int)MainMenuLink.ConnectToServer);
#endif

        public void options()
        {
            GuiLayout layout = new GuiLayout("Options", menu);
            {
                //if (PlatformSettings.DevBuild)
                {
                    //new GuiTextButton("*Crash game*", "Debug blue screen", crash, false, layout);
                    
                }
                Ref.gamesett.optionsMenu(layout);
                //if (Ref.gamesett.bannedPeers.HasMembers)
                //{
                //    new GuiTextButton("Banned players", null, listBlockedPlayers, true, layout);
                //}
                
            }
            layout.End();

            layout.OnDelete += closingOptionsMenu;
        }

        void closingOptionsMenu()
        {
            Ref.gamesett.Save();
        }

        void openTrailer()
        {
            new GameState.TrailerState();
        }
        //void browseFiles()
        //{
        //    inMainMenu = false;
        //    DataStream.DataStreamLib.BrowseFiles(TextLib.EmptyString, menu);
        //}
        //public void LevelLockedInTrial()
        //{
        //    loadMainPage = false;
        //    inMainMenu = false;
        //    mFile = new HUD.File();
        //    mFile.AddTitle("Trial mode");
        //    mFile.AddDescription("You can't play this level in trial");
        //    mFile.AddIconTextLink(SpriteName.LFIconGoBack, "OK", new HUD.ActionLink(openMainPage));

        //    menu.File = mFile;
        //}
        public void RemoveFolderCallBack()
        {
            if (!IsDeleted)
                listWorlds();
        }
        //void selectPlatformMiniGameLevel(int playerIx)
        //{
        //    inMainMenu = false;
        //    HUD.File file = new HUD.File();
        //    for (int i = 1; i <= Platformer.World.NumLevels; ++i)
        //    {
        //        file.AddTextLink("Level " + i.ToString(), new HUD.Link(startPlatformerMiniGame, i));
        //    }
        //    menu.File = file;
        //}
        //void startPlatformerMiniGame(int playerIx, HUD.Link link)
        //{
        //    Platformer.World.LevelIndex = link.Value1;
        //    new Platformer.Play((PlayerIndex)playerIx);

        //}
        //void platformMiniGameTrial(int playerIx)
        //{
        //    Platformer.World.LevelIndex = 1;
        //    new Platformer.Play((PlayerIndex)playerIx);
        //}

        //public override void PadUp_Event(Stick padIx, int contolIx)
        //{
        //    if (inputDialogue != null)
        //    {
        //        inputDialogue.PadUp_Event(padIx, contolIx);
        //    }
        //    else
        //    {
        //        menu.PadUpEvent(padIx);
        //    }
        //}
        //public override void Pad_Event(JoyStickValue e)
        //{
        //    if (e.Stick != Stick.Right)
        //    {
        //        menu.MoveSelection(e);

        //    }

        //}

        int pointingToScreen(int fromScreen, List<Vector2> screenDirs, Vector2 padDir)
        {
            Rotation1D padAngle = Rotation1D.FromDirection(padDir);
            float lowestAngleDiff = float.MaxValue;
            int lowestScreenIx = -1;
            for (int i = 0; i < screenDirs.Count; i++)
            {
                if (i != fromScreen)
                {
                    Vector2 diff = screenDirs[i];
                    diff -= screenDirs[fromScreen];
                    float angleDiff = padAngle.AngleDifference(Rotation1D.FromDirection(diff));
                    if (angleDiff < lowestAngleDiff)
                    {
                        lowestAngleDiff = angleDiff;
                        lowestScreenIx = i;
                    }
                }
            }

            return lowestScreenIx;

        }

        //public static int numJoinedPlayers()
        //{
        //    int numPlayersJoined = 0;
        //    for (int i = int.Player1; i <= int.Player4; i++)
        //    {
        //        if (Engine.XGuide.GetPlayer(i).IsActive)
        //        {
        //            numPlayersJoined++;
        //        }
        //    }
        //    return numPlayersJoined;

        //}


        //public override void Button_Event(ButtonValue e)
        //{
        //    if (inputDialogue != null)
        //    {
        //        inputDialogue.Button_Event(e);
        //    }
        //    else
        //    {
        //        if (!lockControls)
        //        {
        //            if (e.KeyDown && (e.Button == numBUTTON.A || e.Button == numBUTTON.Start))
        //            {
        //                menu.Click(e.PlayerIx, e.Button);
        //            }
        //            else if (e.KeyDown && (e.Button == numBUTTON.B || e.Button == numBUTTON.Back))
        //            {
        //                if (menu.InDocumentView)
        //                    menu.Back(e.PlayerIx);
        //                else if (inMainMenu)
        //                {
        //                    new GameState.IntroScene();
        //                }
        //                else
        //                    OpenMainPage(true);
        //                Music.SoundManager.PlayFlatSound(LoadedSound.MenuBack);
        //            }
        //        }
        //    }
        //}

        void simulatePlayerJoin(int num)
        {
            joinPlayer((int)num);
        }
        public override void Time_Update(float time)
        {
            updateCamInput(time);
            base.Time_Update(time);

            //if (updateNetworkLobby.Update(time))
            //{
            //    updateNetworkLobby.Reset();
            //    if (inMainMenu)
            //    {
            //        Ref.netSession.BeginUpdateAvailableSessions();
            //    }
            //}

            if (updateMenu)
            {
                worldsLoaded();
                updateMenu = false;
            }



            if (Ref.update.LasyUpdatePart == LasyUpdatePart.Part5)
            {
                Data.WorldsSummaryColl.SlowWorldDeleteCheck();
            }
            //else if (Ref.update.LasyUpdatePart == LasyUpdatePart.Part7_GameState)
            //{
            //    if (Ref.netSession.InviteWaiting)
            //    {
            //        new GameState.Joining(Ref.netSession.invitedPlayer);
            //    }
            //}
        }

        virtual protected void worldsLoaded()
        {
            if (loadMainPage)
                openMainPage();
        }

#if PCGAME
        public override void NetworkReadPacket(ReceivedPacket packet)
        {  
            //Network.NetLib.PacketType = (Network.PacketType)System.IO.BinaryReader.ReadByte();
            //if (Network.NetLib.PacketType == Network.PacketType.PCSendClientId)
            //{
            //    Ref.netSession.GetClientId(System.IO.BinaryReader.ReadByte());
            //}
        }
#endif
        //public override void NetworkAvailableSessionsUpdated(bool isAvailable, AvailableNetworkSessionCollection availableSessions)
        //{
        //    if (inMainMenu)
        //    {
        //        OpenMainPage(isAvailable);
        //    }
        //}

        void joinPlayer(int ix)
        {
#if WINDOWS
            if (ix == int.Mouse)
                ix = int.Player1;
#endif
            if (!Engine.XGuide.GetPlayer(ix).IsActive)
            {
                Engine.XGuide.GetPlayer(ix).IsActive = true;
            }
        }


        bool stopSaveQue = true;
        void loadWorld(Data.WorldSummary sum)
        {
            Data.WorldsSummaryColl.PlayInWorld(sum);

            //if (checkIfSignedIn(playerIx))
            {
                //if (stopSaveQue)
                //    Engine.Storage.Reset(false);

                //Ref.netSession.settings.status = 
                //    (Engine.XGuide.GetPlayer(playerIx).OnlineSessionsPrivilege || Ref.netSession.IsSystemLink)? Network.SearchAndCreateSessionsStatus.Create : Network.SearchAndCreateSessionsStatus.NON;
                joinPlayer(Engine.XGuide.LocalHostIndex);
                Data.RandomSeed.LoadWorld();
                new GameState.LoadingMap(sum);
            }
        }

        virtual protected void seeGamerTag(int pIx)
        { }
        

        //public static void LiveButton(HUD.File file, HUD.Link link, List<string> firstRow, List<string> secondRow)
        //{
        //    TextFormat format1 = Menu.Text;
        //    TextFormat format2 = format1;
        //    format2.Scale *= 0.6f;
        //    format2.Color = Color.LightGray;
        //    file.Add(
        //        new HUD.LiveTwoRowButtonData(SpriteName.NO_IMAGE, firstRow, secondRow, null, format1, format2, link));
        //}

        //Data.WorldSummary currentlyEditingWorld;
        //void editWorld(int index)
        //{
        //    editWorld(index);

        //}

        void editWorld(int index)
        {
            Data.WorldSummary world = Data.WorldsSummaryColl.savedWorlds[index];

            GuiLayout layout = new GuiLayout("World " + world.WorldName, menu);
            {
                new GuiLargeTextButton("Play", null,
                    new GuiAction1Arg<Data.WorldSummary>(loadWorld, world), false, layout);

                new GuiDialogButton("Delete", null,
                   new GuiAction1Arg<int>(removeWorldOK, index), false, layout);
            }
            layout.End();

            //HUD.File file = new HUD.File();
            //file.AddTitle(name);

            //file.Add(new WorldSizeDescData(folder));
            //file.AddTextLink("Play", new HUD.Link(loaddialogue, index));
            
            //    file.AddTextLink("Rename", new HUD.Link((int)MainMenuLink.RenameWorld, index));
            //    file.AddTextLink("Delete", new HUD.Link(deletedialogue, index));
            
            //menu.File = file;
        }

        //HUD.File mFile;

        void joinSession(bool local, int hostindex, int playerIx)
        {
            joinPlayer(playerIx);

            new GameState.Joining(hostindex, local);
        }
        //public override bool BlockMenuBoolValue(int playerIx, bool value, bool get, int valueIx)
        //{
        //    if (!get)
        //    {
        //        Ref.netSession.IsSystemLink = value;
        //    }
        //    return Ref.netSession.IsSystemLink;
        //}

        void removeWorldOK(int index)
        {
            Data.WorldsSummaryColl.savedWorlds[index].Removed = true;
            menu.PopLayout();
            listWorlds();
            Data.WorldsSummaryColl.BeginSave();

        }

        //void removeWorld(int worldIx)
        //{
        //    // removepath = path;
        //    HUD.File file = new HUD.File();
        //    file.AddTitle("Delete world?");
        //    file.AddTextLink("Delete", new HUD.Link((int)MainMenuLink.RemoveWorldOK, worldIx));
        //    file.AddTextLink("Cancel", (int)MainMenuLink.Back);
        //    menu.File = file;
        //}



        //bool checkIfSignedIn(int playerIx)
        //{
        //    if (!PlatformSettings.UseGamerServices)
        //        return true;
        //    if (Engine.XGuide.GetPlayer(playerIx).SignedIn)
        //    {
        //        return true;
        //    }
        //    Engine.XGuide.ShowSignIn(false);
        //    return false;
        //}
        //void linkJoinSessionDialogue()
        //{
        //    joinSession(true, link.Value2, playerIx);
        //}
        //void 

//        public override void BlockMenuLinkEvent(HUD.IMenuLink link, int playerIx, numBUTTON button)
//        {
//            //if (inputDialogue != null)
//            //{
//            //    inputDialogue.BlockMenuLinkEvent(link, playerIx, button);
//            //    return;
//            //}
//            Engine.XGuide.LocalHostIndex = playerIx;
//            inMainMenu = false;
//            //const int LinkGoBack = 10;
//            //const int LinkSendMessage = 11;
//            //if (link.Value1 == NetworkHandler.JoinLocalSessionDialogue)
//            //{
//            //    //join game, show loading screen
//            //    //if ( checkIfSignedIn(playerIx))
//            //        joinSession(true, link.Value2, playerIx);
//            //}
//            //else if (link.Value1 == NetworkHandler.JoinLiveSessionDialogue)
//            //{
//            //    //join game, show loading screen
//            //   if (Engine.XGuide.GetPlayer(playerIx).OnlineSessionsPrivilege)
//            //       joinSession(false, link.Value2, playerIx);
//            //   else
//            //       messageHandler.Print("Gold membership needed");
//            //}
//            //else if (link.Value1 == NetworkHandler.ListMoreSessionsLink)
//            //{
//            //    mFile = new HUD.File();
//            //    mFile.AddTitle("Available sessions");
//            //    NetworkHandler.ListSessions(mFile, false);
//            //    menu.File = mFile;
//            //}
//            //else
//            //{
//            switch ((MainMenuLink)link.Value1)
//            {

//                case MainMenuLink.LoadWord:
//                    loadWorld(Data.WorldsSummaryColl.savedWorlds[link.Value2]);
//                    break;
//                //case MainMenuLink.EditWord:
//                //    editWorld(link.Value2);
//                //    break;

//                case MainMenuLink.RemoveWorld:
//                    removeWorld(link.Value2);
//                    break;



//#if PCGAME

//                case MainMenuLink.ConnectToServer:
//                    Ref.netSession.JoinServer(serverIp);
//                    break;
//#endif
//                case MainMenuLink.RemoveWorldOK:
//                    removeWorldOK(link.Value2);
//                    break;
//                case MainMenuLink.AnimationDesign:
//                    new GameState.VoxelDesignState(playerIx);
//                    break;
//                case MainMenuLink.SceneMaker:
//                    new Editor.SceneMaker(playerIx);
//                    break;
//                case MainMenuLink.BuyGame:
//                    Engine.PlayerData p = Engine.XGuide.GetPlayer(playerIx);
//                    if (p.SignedIn)
//                    {
//                        if (p.CanPurchaseGame())
//                        {
//                            Engine.XGuide.ShowBuyDialoge(playerIx);
//                        }
//                        else
//                        {
//                            cantPurchase("Your profile don't allow purchases");
//                        }
//                    }
//                    else
//                    {
//                        cantPurchase("Need a signed in profile");
//                    }
//                    break;
//                //case MainMenuLink.Credits:
//                //    credits();
//                //    break;
//                case MainMenuLink.ExitGame:
//                    Ref.update.exitApplication = true;
//                    break;
//                case MainMenuLink.Back:
//                    OpenMainPage(true);
//                    break;
//                case MainMenuLink.ReConnect:

//                    break;
//                case MainMenuLink.SeeGamerTag:
//                    seeGamerTag(playerIx);
//                    break;

//                //case MainMenuLink.Continue:
//                //    loadWorld(Data.WorldsSummaryColl.savedWorlds[Data.WorldsSummaryColl.savedWorlds.Count - 1], playerIx);

//                //    break;
//                case MainMenuLink.SelectWorld:
//                    listWorlds();
//                    break;
//                //case MainMenuLink.RenameWorld:
//                //    Engine.XGuide.BeginKeyBoardInput(new KeyboardInputValues(
//                //        "Rename " + Data.WorldsSummaryColl.savedWorlds[link.Value2].WorldName,
//                //        "Max " + WorldNameMaxChars.ToString() + "characters", Data.WorldsSummaryColl.savedWorlds[link.Value2].WorldName, (PlayerIndex)playerIx,
//                //        link.Value2, renameWorld));
//                //    break;
//                case MainMenuLink.DebugRemoveWorld:
//                    mFile = new HUD.File();
//                    mFile.AddDescription("Click for instant delete");
//                    for (int i = 0; i < Data.WorldsSummaryColl.savedWorlds.Count; i++)
//                    {
//                        mFile.AddTextLink(Data.WorldsSummaryColl.savedWorlds[i].WorldName, new HUD.Link((int)MainMenuLink.RemoveWorld, i));//i, (int)MainMenuLink.RemoveWorld);
//                    }
//                    menu.File = mFile;
//                    break;
//                case MainMenuLink.NewWorld:

//                    break;
//            }
//        }

        void newWorld()
        {
            joinPlayer(Engine.XGuide.LocalHostIndex);
            int worldIx = Data.WorldsSummaryColl.savedWorlds.Count + 1;
            bool uniqueIx = false;

            do
            {
                uniqueIx = true;
                foreach (Data.WorldSummary w in Data.WorldsSummaryColl.savedWorlds)
                {
                    if (w.SaveIndex == worldIx)
                    {
                        uniqueIx = false;
                        worldIx++;
                        break;
                    }
                }
            } while (!uniqueIx);


            new GameState.LoadingMap(Data.WorldsSummaryColl.CreateNewWorld(worldIx));
        }

        void linkContinue()
        {
            loadWorld(Data.WorldsSummaryColl.savedWorlds[Data.WorldsSummaryColl.savedWorlds.Count - 1]);
        }

        void credits()
        {
            GuiLayout layout = new GuiLayout("Credits", menu);
            layout.scrollOnly = true;
            {
                var oldFormat = menu.style.textFormat;
                menu.style.textFormat.Font = LoadedFont.Console;
                menu.style.textFormat.size *= 1.6f;

                new GuiLabel(PlatformSettings.GameTitle + " - " + PlatformSettings.SteamVersion, layout);

                new GuiLabel("Art, Design & Programming:" + Environment.NewLine +
                    "Fabian \"Viking\" Jakobsson", layout);

                new GuiLabel("Music & Tech Programming:" + Environment.NewLine +
                    "Martin \"Akri\" Grönlund", layout);

                new GuiLabel("Main playtesters:" + Environment.NewLine +
                    
                    "MechaWho" + Environment.NewLine +
                    "Jonas 'Hårfager' Andersson" + Environment.NewLine +
                    "Kristian Nyman" + Environment.NewLine +
                    "Pontus Bengtsson" + Environment.NewLine +
                    "Pontus Bengtsson" + Environment.NewLine +
                    "Samuel Reneskog" + Environment.NewLine +
                    "Gustav Bok" + Environment.NewLine +
                    "Brent Marvich"
                    , layout);
                    
                new GuiSectionSeparator(layout);

                new GuiLabel("vikingfabian.com", layout);

                menu.style.textFormat = oldFormat;
            }
            layout.End();
        }



        //HUD.InputDialogue inputDialogue;
        //public override void BeginInputDialogueEvent(KeyboardInputValues keyInputValues)
        //{
        //    inputDialogue = new HUD.InputDialogue(menu, keyInputValues);
        //}
        
        //void renameWorld(PlayerIndex user, string result, int index)
        //{
        //    Data.WorldsSummaryColl.savedWorlds[index].OverridingName = result;
        //    editWorld(index);
        //}

        //public override void TextInputEvent(PlayerIndex playerIndex, string input, int link)
        //{
        //    if (input != null && input != TextLib.EmptyString)
        //    {
        //        currentlyEditingWorld.OverridingName = input;
        //        Data.WorldsSummaryColl.BeginSave();
        //    }
        //    TextInputCancelEvent(playerIndex);
        //}
        //public override void TextInputCancelEvent(PlayerIndex playerIndex)
        //{
        //    if (inputDialogue != null)
        //    {
        //        inputDialogue.DeleteMe();
        //        inputDialogue = null;
        //    }
        //    listWorlds();

        //}

        //void cantPurchase(string reason)
        //{
        //    mFile = new HUD.File();
        //    mFile.AddTitle("Can't purchase");
        //    mFile.AddDescription(reason);
        //    mFile.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)MainMenuLink.Back);
        //    menu.File = mFile;
           
        //}
        //public override Engine.GameStateType Type
        //{
        //    get { return Engine.GameStateType.MainMenu; }
        //}

        public override bool UseInputEvents
        {
            get
            {
                return true;
            }
        }
        
    }

//    enum MainMenuLink
//    {
//        NewWorld,
//        Continue,
//        SelectWorld,
//        //Credits,
//        ExitGame,
//        SeeGamerTag,
//        ReConnect,
//        DebugRemoveWorld,
//        Back,
//        BuyGame,
//        AnimationDesign,
//        SceneMaker,

//        RemoveWorldOK,

//        LoadWord,
//        EditWord,
//        RenameWorld,
//        RemoveWorld,
//        SelectLastVisited,
//        SelectDownloadedWorld,
//        PlayLastVisited,
//        PlayDownloadedWorld,
//        DeleteDownloadedWorld,
//        SaveLastVisited,

//#if PCGAME
//        ConnectToIp,
//        NetworkPort,
//        ConnectToServer,
//#endif
//    }
   
}
