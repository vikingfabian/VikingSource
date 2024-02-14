using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Input;
//
using VikingEngine.HUD;

namespace VikingEngine.LF2.Players
{
    partial class Player : AbsPlayer, IDialogueCallback, IBinaryIOobj,
        VikingEngine.DataStream.IStreamIOCallback, HUD.IMessageHandlerParent, ICompassIconLocation
    {

        #region CONSTANTS
        const float HeroCamChase = 0.0023f;
        const int HealthCost = 2;
        const int SpawnPointCost = 10;
        const int Sword2Cost = 100;
        const int Sword3Cost = 2000;
        const int Spear2Cost = 500;
        const int LightTankCost = 50;
        const int PiratePackCost = 100;
        const int GirlyPackCost = 60;
        const int TotalMinerPackCost = 100;
        const int LightPlaneCost = 50;
        const string NetworkSettingsTitle = "Multiplayer";
        public const int CharacterWidth = 2;
        public const int CharacterHeight = 4;

        const float ChangeZoomSpeed = 0.0017f;



        
        #endregion

        #region VARIABLES
        //bool requestedMap = false;
        //bool requestedPermission = false;
        //int storeInt1 = 0;

        ButtonTutorial buttonTutorial = null;
        
        bool appearanceChanged = false;

        public const int ChatMessageMaxChars = 250;
        File mFile;
        List<GamerName> gamerNames = new List<GamerName>();
        HUD.ButtonLayout buttonLayOut;

        //Editor.VoxelDesigner voxelDesigner = null;
        //Map.WorldPosition voxelDesignerStartPos;
        //GameObjects.Toys.Football football = null;
        //GameObjects.Toys.GolfBall golfBall = null;
        float controlLock = 0;
        PlayerMode currentMode = PlayerMode.NON;
        Link listMaterialsDialogue;
        protected bool localHost = false;
        public override bool LocalHost
        {
            get { return localHost; }
        }
        IntVector2 travelGoal;
       
        public void SetQuestLogTutorial() { questLogTutorial = true; }

        bool DpadUp = true;
        #endregion

        #region MENU
        //Menu menu = null;
        public MenuSystem menuystem;
        MenuPageName currentMenu = MenuPageName.EMPTY;
        HUD.MessageHandler messageHandler;
        //HUD.InputDialogue inputDialogue;
        //public bool UsingInputDialogue { get { return inputDialogue != null; } }

        //void listMaterials(Link d, int parentPage)
        //{
        //    listMaterialsDialogue = d;
        //    mFile = new File(parentPage);
        //    Editor.VoxelDesigner.listMaterials(mFile, (int)d, false, Settings, (int)Link.ShowHideMaterialNames, 0);
        //    OpenMenuFile(mFile);
        //}
#endregion

        #region MESSAGES
        public List<IMessage> messages = null;
        IMessage currentMessage = null;
        public void AddMessage(IMessage message, bool add)
        {
            if (add)
            {
                Music.SoundManager.PlayFlatSound(LoadedSound.chat_message);
                messages.Add(message);
                Print(message.Subject, SpriteName.LFIconLetter);
            }
            else
            {
                messages.Remove(message);
            }
        }
        #endregion
        
        #region INTERACTIONS

        public int InteractionMenuTab = 0;
        public int InteractionStoreValue1 = 0;
        public int InteractionStoreValue2 = 0;
        public void SellItems(int amount, HUD.IMenuLink link, Object non)
        {
            if (hero.InteractingWith != null)
            {
                GameObjects.NPC.SalesMan salesman = hero.InteractingWith as GameObjects.NPC.SalesMan;
                salesman.PlayerSellAmount(this, amount);
            }
            NPCValueWheelCancelEvent();
        }
        public void NPCValueWheelCancelEvent()
        {
            ValueDialogue = null;
            if (hero.InteractingWith != null)
            {
                hero.Player.OpenMenuFile(hero.InteractingWith.Interact_TalkMenu(hero));
            }
        }
        void interact()
        {
            InteractionMenuTab = 0;

            if (hero.InteractingWith != null)
            {
                if (hero.InteractingWith.InteractType == GameObjects.InteractType.Door && !LfRef.chunks.ChunksDataLoaded(hero.ScreenPos))
                {
                    Print(StillLoadingMessage);
                    return;
                }
                if (hero.TriggerInteraction())
                {
                    if (hero.InteractingWith != null)
                    {

                        if (hero.InteractingWith.InteractType == GameObjects.InteractType.SpeakDialogue)
                        {
                            mode = PlayerMode.InDialogue;
                            openPage(MenuPageName.TalkingToNPC);
                            if (hero.InteractingWith is GameObjects.NPC.SalesMan && ((GameObjects.NPC.SalesMan)hero.InteractingWith).WillBuyItems)
                            {
                                menuTabs = new HUD.MenuTabs(
                                   new List<HUD.Tab>{ 
                                    new HUD.Tab(SpriteName.LFIconBuy),
                                    new HUD.Tab(SpriteName.LFIconCoins), 
                                    new HUD.Tab(SpriteName.IconQuestExpression), 
                                }, menu);
                            }
                        }

                        else if (hero.InteractingWith.InteractType == GameObjects.InteractType.Chest ||
                            hero.InteractingWith.InteractType == GameObjects.InteractType.DiscardPile)
                        {
                            SpriteName chestIcon;
                            if (((GameObjects.EnvironmentObj.Chest)hero.InteractingWith).MapChunkObjectType == GameObjects.EnvironmentObj.MapChunkObjectType.Chest)
                            {
                                chestIcon = SpriteName.IconChest;
                            }
                            else
                            {
                                chestIcon = SpriteName.LFIconDiscardItem;
                            }

                            openPage(MenuPageName.ChestDialogue);
                            menuTabs = new HUD.MenuTabs(
                                new List<HUD.Tab>{ 
                                new HUD.Tab(chestIcon),
                                new HUD.Tab(SpriteName.IconBackpack), 
                            },
                                menu);
                            currentMenu = MenuPageName.ChestDialogue;
                            updateChestDialogue();
                        }
                    }

                }
            }
        }
        #endregion


        Director.MonsterSpawn monsterSpawn = new Director.MonsterSpawn();
        Director.TreasureSpawn treasureSpawn = new Director.TreasureSpawn();
        PlayerProgress progress;
        public Compass compass;
        public PlayerProgress Progress
        {
            get { return progress; }
        }
        public override AbsPlayerProgress AbsPlayerProgress
        {
            get { return progress; }
        }


        PlayerMode mode
        {
            get { return currentMode; }
            set 
            {
                if (currentMode != value)
                {
                    currentMode = value;

                    VisualMode visualMode;
                    switch (mode)
                    {
                        default:
                            visualMode = VisualMode.Menu;
                            break;
                        case PlayerMode.Creation:
                            visualMode = VisualMode.Build;
                            break;
                        case PlayerMode.CreationCamera:
                            visualMode = VisualMode.Build;
                            break;
                        case PlayerMode.CreationSelection:
                            visualMode = VisualMode.Build;
                            break;
                        case PlayerMode.Play:
                            visualMode = VisualMode.Non;
                            //reset hero modifiers
                            if (hero != null)
                                hero.ResetModifiers();
                            break;
                        //case PlayerMode.RCtoy:
                        //    visualMode = VisualMode.RC;
                        //    break;
                    }
                    if (mode != PlayerMode.Play)
                    { //prevent the hero from swing sword by himself
                        hero.Attack(null, false, EquipSlot.NUM, GameObjects.Gadgets.GadgetAlternativeUseType.Standard);
                    }
                    setVisualMode(visualMode, true);
                }
            }
        }
        public PlayerMode Mode
        {
            get { return currentMode; }
        }
        
        public override string Name
        {
            //kommer bli fel i splitscreen
            get { return localPData.PublicName(LoadedFont.Regular); } 
        }

        


        public void HideControls()
        {
            if (buttonLayOut != null)
            {
                buttonLayOut.DeleteMe();
                buttonLayOut = null;
            }
        }

        

       
        //public HUD.ValueWheelDialogue ValueDialogue = null;
        
        public bool LockMessageHandler
        {
            get
            {
                return
                    mode == PlayerMode.ChestDialogue ||
                    mode == PlayerMode.FriendLostController ||
                    mode == PlayerMode.InDialogue ||
                    mode == PlayerMode.InMenu ||
                    mode == PlayerMode.Map;
            }
        }
        Dialogue dialogue;
       // GameObjects.Toys.AbsRC rcToy;
        
        VectorRect screenArea;
        VectorRect safeScreenArea;
        public VectorRect SafeScreenArea
        {
            get { return safeScreenArea; }
        }
        public VectorRect ScreenArea
        {
            get { return screenArea; }
        }

        override public int Index
        { get { 
            
            return localPData.localPlayerIndex; } }
        
        public void UpdateSplitScreen(Engine.PlayerData controllerLink, int numPlayers, int screenIndex)
        {
            Graphics.AbsCamera creationCam = null;
            if (mode == PlayerMode.Creation)
            {
                creationCam = localPData.view.Camera;
                localPData.view.Camera = savedCamera;
            }

                controllerLink.view.camType = (Settings.FPSview) ?
                    Graphics.CameraType.FirstPerson : Graphics.CameraType.TopView;

                controllerLink.view.SetDrawArea(numPlayers, screenIndex, Settings.CameraSettings, true);

            if (controllerLink.view.camType == Graphics.CameraType.FirstPerson)
            {
                ((Graphics.FirstPersonCamera)localPData.view.Camera).Person = hero;

            }

            this.localPData = controllerLink;
            ZoomRange = new IntervalF(20, localPData.view.Camera.CurrentZoom);

            screenArea = new VectorRect(controllerLink.view.DrawArea);

            const float SafePercent = 0.06f;
            Vector2 removeSafeBorder = SafePercent * Engine.Screen.ResolutionVec;
            safeScreenArea = screenArea;

            if (screenArea.Right == Engine.Screen.Width)
            {
                safeScreenArea.Width -= removeSafeBorder.X;
            }
            if (screenArea.Bottom == Engine.Screen.Height)
            {
                safeScreenArea.Height -= removeSafeBorder.Y;
            }

            if (screenArea.X == 0)
            {
                safeScreenArea.AddToLeftSide(-removeSafeBorder.X);
            }
            if (screenArea.Y == 0)
            {
                safeScreenArea.AddToTopSide(-removeSafeBorder.Y);
            }

            float screenRatio = lib.SmallestValue(screenArea.Width, screenArea.Height) /
                lib.LargestValue(screenArea.Width, screenArea.Height);
            localPData.view.Camera.FieldOfView = Bound.Max(88 * screenRatio, 60);


            localPData.view.Camera.positionChaseLengthPercentage = HeroCamChase;
            updateCamZoom();

            if (hero != null)
            {
                localPData.view.Camera.goalLookTarget = hero.Position;
            }

            HideControls();

            if (mode == PlayerMode.Creation)
            {
                
                savedCamera = localPData.view.Camera;
                if (creationCam.aspectRatio != localPData.view.Camera.aspectRatio)
                {
                    EndCreationMode();
                }
                else
                {
                    localPData.view.Camera = creationCam;
                }
            }

            updateHUD();

            compass.UpdateCompassPosition();

            updateExitFPSHUD();
        }

        void updateExitFPSHUD()
        {
            removeExitModeButton();
            if (Settings.FPSview)
                exitModeButton = new ExitModeButton(SpriteName.DpadUp, safeScreenArea);
        }

        void removeExitModeButton()
        {
            if (exitModeButton != null)
            {
                exitModeButton.DeleteMe();
                exitModeButton = null;
            }
        }


        static readonly IntervalF CameraAngleRangeX = new IntervalF(1.5f, 1.7f); 
        static readonly IntervalF[] CameraAngleY = new IntervalF[] 
        {
            new IntervalF(0.4f, 0.5f),
            new IntervalF(0.66f, 0.78f),
            new IntervalF(0.78f, 0.88f)
        };


        Vector2 camGoalAngle;
        const int NumAngleSamples = 32;
        CirkleCounterUp currentAngleSample = new CirkleCounterUp(NumAngleSamples - 1);
        Vector2[] characterGoalAngleSamples = new Vector2[NumAngleSamples];
        IntervalF ZoomRange;
        float currentZoomPerc;
        ZoomMode zoomMode = ZoomMode.TopView;
        const float MenuZoom1p = 20;

        
        public Player(Engine.PlayerData pData, int numPlayers, int screenIndex, bool localHost, PlayState gamestate)
            :base(pData)
        {
            menuystem = new MenuSystem(this);
            this.localPData = pData;
            pData.Tag = this;
            if (pData.inputMap == null)
            {
                inputMap = new InputMap(pData.localPlayerIndex);
                pData.inputMap = inputMap;
            }
            else
            {
                inputMap = pData.inputMap as InputMap;
            }
            //inputMap = Input.Controller.Instance(pData.Index);

            progress = new PlayerProgress(this);
            compass = new Compass(this, gamestate);

            
            this.localHost = localHost;
            Settings = new PlayerSettings(this, true, pData.PublicName(LoadedFont.Regular));
            
            Settings.Save(false, this);
            //SaveUndo(false);
            UpdateSplitScreen(pData, numPlayers, screenIndex);
           
            camGoalAngle = new Vector2(CameraAngleRangeX.Center, CameraAngleY[1].Center);

            mapSettings = new MiniMapSettings();
            mapSettings.Scale = 4;

            
            messageHandler = new HUD.MessageHandler(2, SpriteName.LFMessageBackground, 4000, Vector2.Zero, this);
            updateMessageHandlerPos();
            
            mode = PlayerMode.Play;
            localPData.IsActive = true;

            if (!Map.World.RunningAsHost)
            {
                new DataStream.CreateFolderToQue(visitorFolderPath);
            }
            Director.LightsAndShadows.Instance.LocalGamerJoinedEvent();

            //checkPrivateHomeLocation();
           
        }

        const int EventTriggerStartupHelp = 0;
        public void EventTriggerCallBack(int type)
        {

            switch (type)
            {

                case EventTriggerStartupHelp:
                    if (LfRef.gamestate.Progress.GeneralProgress == Data.GeneralProgress.TalkToFather2_HogKilled)
                        beginButtonTutorial(new ButtonTutorialArgs(inputMap.viewHelp, null, 
                            LootfestLib.ViewBackText + " to view control overview", screenArea));
                    break;

            }
        }

        public IntVector2 ChunkUpdateCenter
        {
            get
            {
                return hero.ScreenPos;
            }
        }
        public Vector3 CamTargetPosition()
        {
            return hero.Position;
        }
        public Rotation1D CamTargetMoveDir()
        {
            return hero.Rotation;
        }

        public void AutoEquipMessage(SpriteName button, GameObjects.Gadgets.IGadget item)
        {
            messageHandler.addMessage(new AutoAssignButtonMessage(messageHandler.NextPosition(), button, item));
        }
        public void MapLocationMessage()
        {
            LF2.Music.SoundManager.PlayFlatSound(LoadedSound.PickUp);
            Print("New map location", SpriteName.LFIconMap);
        }
        public void Print(string message)
        { 
            if (messageHandler != null)
                messageHandler.Print(message); 
        }
        public void Print(string message, SpriteName icon)
        { 
            if (messageHandler != null)
                messageHandler.Print(message, icon); 
        }


        public void PrintChat(ChatMessageData message, LoadedSound sound)
        {
            if (messageHandler != null)
                messageHandler.PrintChat(message, safeScreenArea.Width);
            Music.SoundManager.PlayFlatSound(sound);
        }

        void ScreenCheck()
        {
            System.Diagnostics.Debug.WriteLine("Empty Screen:" + LfRef.chunks.GetScreen(hero.ScreenPos).EmptyScreenCheck().ToString());
        }

        bool warpLock = false;
        public bool WarpLoading
        {
            get { return warpLock && controlLock > 0; }
        }

        public void LockControls(bool warpLoading)
        {
            warpLock = warpLoading;
            if (warpLoading)
            {
                const float WarpLoadTime = 1500;
                const float WarpImmortalTime = WarpLoadTime + 1000;
                hero.ImmortalityTime = WarpImmortalTime;
                controlLock = WarpLoadTime;
                Graphics.TextS loading = new TextS(LoadedFont.Bold,
                    safeScreenArea.Center, new Vector2(1f), Align.CenterAll, LanguageManager.Wrapper.LoadingTerrain(),
                    Color.CornflowerBlue, ImageLayers.AbsoluteTopLayer);
                new Timer.TextFeed(500, loading);
                new Timer.Terminator(controlLock, loading);

            }
            else
                controlLock = 800;
        }
        public GameObjects.Characters.Hero CreateHero()
        {
            hero = new GameObjects.Characters.Hero(this);
            LfRef.gamestate.RefreshHeroesList();

            save(false);
            createHUD();
            this.LockControls(true);
            
            NetworkSharePlayer(Network.SendPacketToOptions.SendToAll);
            return hero;
        }

        //public void ViewSongName(string song, string creator)
        //{
        //    messageHandler.addMessage(new MusicMessage(messageHandler.NextPosition(), 
        //        safeScreenArea.Width, song, creator));
        //}
        

        

        public void GuideUpEvent()
        {
            if (mode == PlayerMode.Play && Ref.netSession.InMultiplayerSession)
            {
                openPage(MenuPageName.MainMenu);
            }
        }

        //override public bool EditorUndo()
        //{
        //    if (undoActions.Count > 0)
        //    {
        //        IntVector2 pos = base.NextUndoPos();
        //        if (base.EditorUndo())
        //        {
        //            return true;
        //        }
        //        Print("Undo " + undoActions.Count.ToString());
        //        //share
        //        System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_VoxelEditorUndo, 
        //            Network.PacketReliability.ReliableLasy, Index);
        //        Map.WorldPosition.WriteChunkGrindex_Static(pos, w); 
        //    }
        //    return false;
        //}
       
        

        void changeCamera()
        {
            Settings.FPSview = !Settings.FPSview;
            LfRef.gamestate.UpdateSplitScreen();
        }

        
        /// <summary>
        /// Calculate the relative movement compared to the camera angle
        /// </summary>
        /// <returns>Direction to move the hero</returns>
        public Vector2 HeroMoveDir()
        {
            if (mode == PlayerMode.Play && !hero.LockControls)
            {
                Vector2 moveDir = inputMap.movement.direction;//.JoyStickValue(Stick.Left).Direction;
                if (localPData.view.Camera.CamType == Graphics.CameraType.FirstPerson)
                {
                    return ((Graphics.FirstPersonCamera)localPData.view.Camera).MoveCamera(moveDir);
                }
                else
                {
                    return VectorExt.RotateVector(moveDir, localPData.view.Camera.TiltX - MathHelper.PiOver2);
                }
            }
            else return Vector2.Zero;
        }

        void moveCamera(Vector2 dirAndTime)
        {
            
            if (localPData.view.Camera.CamType == Graphics.CameraType.FirstPerson)
            {//FPS
                localPData.view.Camera.RotateCamera(dirAndTime);
            }
            else
            {
                if (Math.Abs(dirAndTime.X) > Math.Abs(dirAndTime.Y))
                {
                    localPData.view.Camera.TiltX += dirAndTime.X * 0.0033f;
                }
                else
                {
                    currentZoomPerc += dirAndTime.Y * ChangeZoomSpeed;
                    updateCamZoom();
                }                
            }
        }

        
        void updateCurrentMenu()
        {
            openPage(currentMenu);
        }

        
        void removePlayer(bool bann, Network.AbsNetworkPeer gamer)
        {
            
            //Network.AbsNetworkPeer gamer = Ref.netSession.GetGamerFromID((byte)id);
            if (bann)
            {
                NetworkLib.BlockGamer(gamer);
            }
            Print(gamer.Gamertag + " is removed");
            Ref.netSession.kickFromNetwork(gamer);
            //Ref.netSession.RemovePlayer((byte)id);

            //if (!bann && !Settings.GotPrivateGameSuggestion)
            //{
            //    Settings.GotPrivateGameSuggestion = true;
            //    mFile = new File();
            //    mFile.AddTitle("Don't like visitors?");
            //    mFile.AddDescription("You can set your world to private in the " + NetworkSettingsTitle);
            //    mFile.AddTextLink("OK", (int)Link.NetworkSettings);
            //}
            //else
            //    openPage(MenuPageName.MainMenu);
        }
        
        byte currentClientID;

        static readonly List<ClientPermissions> PermissionList = new List<ClientPermissions>
        {
            ClientPermissions.Spectator, ClientPermissions.Build, ClientPermissions.Full
        };

        public override void BlockMenuClose(int playerIx)
        {
            CloseMenu();
        }

        const string TravelToGamer = "Travel to gamer";
        public override void BlockMenuLinkEvent(HUD.IMenuLink link, int playerIx, numBUTTON button)
        {
            //if (mode == PlayerMode.Creation)
            //{
            //    if (voxelDesigner != null)
            //        voxelDesigner.BlockMenuLinkEvent(link, playerIx, button);
            //}
            //else if (inputDialogue != null)
            //{
            //    inputDialogue.BlockMenuLinkEvent(link, playerIx, button);
            //}
            //else
            //{
                if (link is HUD.Link && link.Value1 == Network.Session.SelectRemoteGamerDialogue)
                {
                    currentClientID = (byte)link.Value2;
                    Network.AbsNetworkPeer gamer = Ref.netSession.GetGamerFromID((byte)link.Value2);
                    mFile = new File((int)MenuPageName.NetworkSettings);
                    mFile.AddTitle(gamer.Gamertag);

                    if (!Ref.netSession.IsSystemLink)
                        mFile.AddTextLink("Gamer card", new HUD.Link((int)Link.GamerViewCard_dialogue, link.Value2));//link.Value2, (int)Link.GamerViewCard_dialogue);


                    if (Ref.netSession.IsHost)
                    {
                        // Unused var
                        //int currentPermission = 0;
                        ClientPlayer cp = LfRef.gamestate.GetClientPlayer((byte)link.Value2);
                        if (cp != null)
                        {
                            mFile.AddTextLink("Remove player", new HUD.Link((int)Link.GamerKickOutOptions_dialogue, link.Value2));//link.Value2, (int)Link.GamerKickOutOptions_dialogue);
                        }
                    }
                    OpenMenuFile(mFile);
                }
                else if (hero.InteractingWith != null)
                {
                    if (hero.InteractingWith.InteractType == GameObjects.InteractType.Chest ||
                        hero.InteractingWith.InteractType == GameObjects.InteractType.DiscardPile)
                    {
                        switch ((Link)link.Value1)
                        {
                            case Link.TakeEverything:

                                if (hero.InteractingWith == null)
                                {
                                    CloseMenu();
                                }
                                else
                                {
                                    GadgetLink gLink = new GadgetLink(null, progress.Items, null, this);
                                    List<GameObjects.Gadgets.IGadget> gadgets = ((GameObjects.EnvironmentObj.Chest)hero.InteractingWith).Data.GadgetColl.ToList();
                                    foreach (GameObjects.Gadgets.IGadget g in gadgets)
                                    {
                                        gLink.Gadget = g;
                                        new MoveItemsBetweenCollections(((GameObjects.EnvironmentObj.Chest)hero.InteractingWith).Data,
                                            gLink,
                                            progress.Items, this, true);
                                    }
                                    updateChestDialogue();
                                }

                                break;
                        }

                    }
                    else
                    {
                        if (hero.InteractingWith.Interact_LinkClick((HUD.Link)link, hero, menu))
                        {
                            CloseMenu();
                        }
                    }
                }
                else
                {
                    LinkClick((Link)link.Value1, link, playerIx);
                }
            //}
        }

        public void ItemMoveCompleted()
        {
            //1. remove value dialogue
            //2. update the menu
            if (menuTabs == null)
            {
                openPage(MenuPageName.Backpack);
            }
            else
            {
                ValueDialogue = null;

                updateChestDialogue();

            }
        }

        

        //public override void BlockMenuListOptionEvent(int link, int option, int playerIx)
        //{
        //    if (mode == PlayerMode.Creation)
        //    {
        //        voxelDesigner.BlockMenuListOptionEvent(link, option, playerIx);
        //    }
        //    else
        //    {
        //        switch ((Link)link) 
        //        {

        //            case Link.LinkSoundLevel:
        //                Music.MusicLib.ChangeLevel(option, false);
        //                openPage(MenuPageName.MainMenu);
        //                break;
        //            case Link.LinkMusikLevel:
        //                Music.MusicLib.ChangeLevel(option, true);
        //                openPage(MenuPageName.MainMenu);
        //                break;
        //            case Link.ChangeClientPermissions:
        //                UpdateClientPermission(currentClientID, PermissionList[option], true);
        //                break;
        //            case Link.NetworkSessionType:
        //                Network.NetworkCanJoinType sessionOpenType = (Network.NetworkCanJoinType)option;
        //                if (sessionOpenType != Ref.netSession.SessionOpenType)
        //                {
        //                    Settings.SessionOpenType = sessionOpenType;
        //                    Ref.netSession.SessionOpenType = sessionOpenType;
        //                    SettingsChanged();
        //                }
        //                break;
                    
        //        }
        //        SettingsChanged();
        //    }
        //}

       

        
        void removeCurrentMessage()
        {
            if (currentMessage != null)
            {
                messages.Remove(currentMessage);
                currentMessage = null;
                if (messages.Count > 0)
                {
                    openPage(MenuPageName.Messages);
                }
                else
                {
                    openPage(MenuPageName.MainMenu);
                }
            }
        }

        
        void setSpawnPointMember(HUD.File file)
        {
            file.AddTextLink("Set Spawn (" + SpawnPointCost.ToString() + ")", "Set this location as your respawn position", (int)Link.WarpToSetHere);
        }
       

        void travelTo(IntVector2 pos)
        {
            if ((hero.ScreenPos - pos).SideLength() > 3)
            {
                LockControls(true);
            }
            hero.JumpTo(pos);
            
        }

        void beginThreadedMenu(Link link)
        {
            mFile = new File();
            mFile.AddDescription("loading...");
            OpenMenuFile();
            new Process.ThreadedMenu(this, link, menu.MenuId);
        }

        public HUD.File GenerateThreadedMenu(Link link)
        {
            HUD.File file = null;
            if (menu != null && menu.Visible)
            {
                file = new File();
                switch (link)
                {
                    
                    default:
                    //Link.CreatureImage and Select obj to send
                        int dialogue = link == Link.CreatureImage ? (int)Link.CritterImage_dialogue : (int)Link.SelectAnimObj_dialogue;

                        currentMenu = MenuPageName.Creature;
                        file = Editor.VoxelDesigner.listUseMadeModels(false, dialogue);
                        if (mFile.Empty)
                        {
                            file.AddTitle("Empty");
                            file.AddDescription("You need to create an image in AnimDesign first");
                            file.AddIconTextLink(SpriteName.LFIconGoBack, "OK", 
                                link == Link.CreatureImage?
                                (int)Link.CritterMenu : (int)Link.NetworkSettings);
                        }
                        break;
                }
            }
            return file;
        }

        public void MyMoneyToMenu(HUD.File file)
        {

            file.Insert(0, new YourResourcesDescData(new  IconAndText[] { progress.Items.MenuResourceDesc(GameObjects.Gadgets.GoodsType.Coins) }));
        }

        void appearancesAdded()
        {
            Print("Appearance options added");
        }
        void blocksAdded()
        {
            Print("More blocks added");
        }
        void RCsAdded()
        {
            Print("New RC toy added");
        }


        public void ManualAlternativeEquiping()
        {
            mFile = new File((int)MenuPageName.Manual);
            mFile.TextBoxTitle("Alternative equiping");
            mFile.TextBoxBread("Sometimes a weapon has several ways to attack, example: bows can fire both ordinary arrows and golden arrows.");
            mFile.TextBoxBread("Go into the equip menu, click on a button to equip it, when selecting a weapon with several attack types, you will get a list to select the attack type from.");
            mFile.TextBoxBread("Example: You can equip a bow with ordinary arrows on button X, and the same bow but with golden arrows on button A.");
            OpenMenuFile();
        }

        void LinkClick(Link name, HUD.IMenuLink link, int playerIx)
        {
            switch (name)
            {
#region MANUAL
                //lägg till food
                case Link.ManualControls:
                    mFile = new File((int)MenuPageName.Manual);
                    mFile.TextBoxTitle("Controls");//mControls.addText("Controls");
                    mFile.TextBoxBread("You can, at any moment in the game, " + LootfestLib.ViewBackText + " to get a controls overview.");//mControls.addText("You can, at any moment in the game, " + LootfestLib.ViewBackText + " to get a controls overview.", TextStyle.LF_Bread);
                    OpenMenuFile();
                    break;
                case Link.ManualCrafting:
                    mFile = new File((int)MenuPageName.Manual);
                    mFile.TextBoxTitle("Crafting");
                    mFile.TextBoxBread("Crafting allows you to turn raw materials into weapons, armor, food and other useful items. In each village or city you will find workers that can craft the items for you. You can view what materials they require by talking to them. Some items have alternative materials, so here you can be strategic and try to get the most out of what you have.");
                    mFile.TextBoxSubTitle("Weapons and armor");
                    mFile.TextBoxBread("The higher quality materials you use, the stronger and better your result will be. The overall quality will be set by the raw material with the lowest quality, a chain isn't stronger than its weakest link, so avoid mixing in low quality materials if the rest are good.");
                    mFile.TextBoxSubTitle("Magic enchanting");
                    mFile.TextBoxBread("The Volcan smith will craft magic weapons. The type of the gem will determine what element the magic will have.");
                    mFile.TextBoxSubTitle("Food");
                    mFile.TextBoxBread("Cooking food will raise its healing power. Higher quality materials will give better healing.");
                    mFile.TextBoxSubTitle("Projectiles");
                    mFile.TextBoxBread("When crafting projectiles you will have a chance to get a bonus item, the chance is larger if the material quality is high.");
                    OpenMenuFile();
                    break;
                case Link.ManualEquipItems:
                    mFile = new File((int)MenuPageName.Manual);
                    mFile.TextBoxTitle("Equip items");
                    mFile.TextBoxBread("You can equip weapons and armor. The weapons can be mapped to any of the buttons X, A or B - just select 'Equip' in the pause menu When you unlock the weapon belt (talk to the War Veteran), you can equip three groups of weapons and swap between them with the shoulder buttons.");
                    mFile.TextBoxBread("Items auto equip to empty slots when you pick them up, but you can turn that off in the game settings.");
                    OpenMenuFile();
                    break;
                case Link.ManualMultiplayer:
                    mFile = new File((int)MenuPageName.Manual);
                    mFile.TextBoxTitle("Multiplayer");
                    mFile.TextBoxBread("The game fully supports drop-in multiplayer and you can play a combination of split screen and online network. To join in split screen, just pick up a another controller and press START. If you need some extra help you can change your network to 'open' in '" + NetworkSettingsTitle + "'. Gold membership is required for online network.");
                    OpenMenuFile();
                    break;
                case Link.ManualQuests:
                    mFile = new File((int)MenuPageName.Manual);
                    mFile.TextBoxTitle("Quests");
                    mFile.TextBoxBread("The story driven quests will help you through the game. You can actually complete the game without following a single quest, but that's not recommended. You will have an '!' on the map, this is the place you will do your next mission. The '!' is also on your compass in the lower right corner of the screen. If you forgot what you where supposed to do, " + 
                        LootfestLib.ViewBackText + " to view tips on the current quest goal.");
                    OpenMenuFile();
                    break;
                case Link.ManualTraveling:
                    mFile = new File((int)MenuPageName.Manual);
                    mFile.TextBoxTitle("Traveling");
                    mFile.TextBoxBread("There is a fast transport between each friendly population, but if you want to save coins you can walk instead - that's much more dangerous but you might find something interesting that was hidden.");
                    mFile.TextBoxBread("Map");
                    mFile.TextBoxBread("You can place a waypoint on the map to easier navigate there using the compass. Notice that the 'N' on the compass is upward on the map. You have a green arrow on the compass that shows the direction your character is walking in.");
                    OpenMenuFile();
                    break;

#endregion

                case Link.DebugGetAllMagicRings:
                    for (GameObjects.Magic.MagicRingSkill skill = GameObjects.Magic.MagicRingSkill.NO_SKILL + 1; skill < GameObjects.Magic.MagicRingSkill.NUM; skill++)
                    {
                        progress.Items.AddItem(new GameObjects.Gadgets.Jevelery(skill));
                    }
                    openPage(MenuPageName.Backpack);
                    break;
                case Link.DebugGetAllGoods:
                    progress.Items.Goods.DebugGet10OfEach();
                    openPage(MenuPageName.Backpack);
                    break;

                //case Link.QuestLog:
                //    OpenMenuFile(LfRef.gamestate.Progress.QuestLog());
                //    break;
                case Link.DebugSetMission:
                    mFile = new File();
                    mFile.AddDescription("Current:" + LfRef.gamestate.Progress.GeneralProgress.ToString());
                    for (int i = 0; i < (int)Data.GeneralProgress.NUM; i++)
                    {
                        mFile.AddTextLink(((Data.GeneralProgress)i).ToString(), new HUD.Link((int)Link.DebugSetMissionSelect, i));
                    }
                    OpenMenuFile();
                    break;
                case Link.DebugSetMissionSelect:
                    {
                        Data.GeneralProgress value = (Data.GeneralProgress)link.Value2;
                        if (value == Data.GeneralProgress.DestroyMonsterSpawn) 
                            LfRef.gamestate.Progress.DestroyMonsterSpawnIndex = 1;
                        
                        LfRef.gamestate.Progress.GeneralProgress = value;
                        LfRef.gamestate.Progress.SetQuestGoal(null);
                        CloseMenu();
                    }
                    break;
                case Link.DebugSetQuest:
                    mFile = new File();
                    for (int i = 0; i < (int)Data.GeneralProgress.NUM; i++)
                    {
                        mFile.AddTextLink(((Data.GeneralProgress)i).ToString(), new HUD.Link( (int)Link.DebugSetProgress_dialogue, i));//i, (int)Link.DebugSetProgress_dialogue);
                    }
                    break;


                
                case Link.DebugUnlockLocations:
                    LfRef.worldOverView.DebugUnlockAll();
                    CloseMenu();
                    break;
                case Link.DebugAddOneNPC:
                    new GameObjects.NPC.BasicNPC(hero.WorldPosition, null);
                    break;
                case Link.UseHorseTravelOK:
                    if (progress.Items.Pay(LootfestLib.TravelCost))
                    {
                        Music.SoundManager.PlayFlatSound(LoadedSound.buy);
                        hero.JumpTo(travelGoal);
                        CloseMenu();
                    }
                    break;
                case Link.DebugGetGoods:
                    progress.Items.Goods.DebugGet10OfEach();
                    break;
                case Link.Backpack:
                    openPage(MenuPageName.Backpack);
                    break;
                case Link.DiscardItems:
                    //create a discard pile and interact with it
                    GameObjects.EnvironmentObj.DiscardPile pile = new GameObjects.EnvironmentObj.DiscardPile(hero.WorldPosition, null);
                    hero.InteractingWith = pile;
                    interact();
                    nextPage(true);
                    break;

                case Link.Express:
                    hero.Express((VoxelModelName)link.Value2, true);
                    CloseMenu();
                    break;
                case Link.DebugCorruptAllFiles:
                    DataLib.SaveLoad.CorruptFiles = !DataLib.SaveLoad.CorruptFiles;

                    break;
                case Link.DebugTakeDamage:
                    hero.TakeDamage(new GameObjects.WeaponAttack.DamageData(1, GameObjects.WeaponAttack.WeaponUserType.Enemy, ByteVector2.Zero), true);
                    break;
                case Link.SendDefile:
                    LfRef.gamestate.SendDeFile(playerIx);
                    break;
                case Link.BeginRestoreChunkOptions:
                    beginThreadedMenu(name);
                    break;
                case Link.RestoreChunkToGenerated:

                    break;
                case Link.AboutRestore:
                    DataLib.DocFile info = new DataLib.DocFile();
                    info.addText("About chunk restore", TextStyle.LF_UTitle);
                    info.addText("Will turn a 32 by 32 blocks area back in time. A new safe copy is made each hour. Safe copies older than 48versions ago will be removed.", TextStyle.LF_Bread);
                    info.addText("When restoring a chunk, you will have to wait a few seconds before it updates.", TextStyle.LF_Bread);
                    info.addText("Restore to generated", TextStyle.LF_UTitle);
                    info.addText("This will completely wipe out any changes to the chunk and it will return to the appearance it had when the world was new", TextStyle.LF_Bread);
                    info.addText("Network", TextStyle.LF_UTitle);
                    info.addText("The restoration update wont appeat to the clients until they reenter your world", TextStyle.LF_Bread);
                    menu.OpenDocument(info);
                    break;
                case Link.Invite:
                    Engine.XGuide.Invite(playerIx);
                    break;
                case Link.DebugBlockTexture:
                    Image texture = new Image(SpriteName.BlockTexture, new Vector2(100), new Vector2(LoadTiles.BlockTextureWidth * 2), ImageLayers.AbsoluteTopLayer);
                    new Timer.Terminator(10000, texture);
                    break;
                case Link.DebugChunkStatus:
                    //sätt kameran i -x-,+x pos
                    mFile = new File();
                    localPData.view.Camera.TiltX = MathHelper.PiOver2;
                    IntVector2 pos = IntVector2.Zero;
                    const int Radius = 1;

                    Map.ChunkData changed = LfRef.worldOverView.GetChunkData(hero.ScreenPos);
                    bool correct = LfRef.chunks.GetScreen(hero.ScreenPos).CorrectLoaded;
                    for (pos.Y = (short)(hero.ScreenPos.Y - Radius); pos.Y <= hero.ScreenPos.Y + Radius; pos.Y++)
                    {
                        for (pos.X = (short)(hero.ScreenPos.X - Radius); pos.X <= hero.ScreenPos.X + Radius; pos.X++)
                        {
                            Map.Chunk c =  LfRef.chunks.GetScreen(pos);
                            string text = c.ToString();
                            if (pos == hero.ScreenPos)
                            { 
                                text = "Center: " + text;
                            }
                            mFile.TextBoxBread(text);
                            if (pos == hero.ScreenPos)
                            {
                                mFile.TextBoxBread("Data: " + c.chunkData.ToString());
                            }
                        }
                    }
                    if (Ref.netSession.IsHostOrOffline)
                    {
                        mFile.TextBoxTitle("Chunk Que");
                    }
                    else
                    {
                        mFile.TextBoxTitle("30 last recieved chunks");
                    }
                    LfRef.gamestate.RequestedChunksToDoc(mFile);
                    mFile.TextBoxTitle("Chunk Update status");
                    OpenMenuFile();
                    break;
                
                case Link.Statistics:
                    DataLib.DocFile statistics = new DataLib.DocFile();
                    statistics.addText("Game statistics", TextStyle.LF_HeadTitle);
                    statistics.addText("Time spent: " + TextLib.TimeToText(Settings.TimeSpent, false), TextStyle.LF_Bread);
                    menu.OpenDocument(statistics);
                    break;
                //case Link.ToyTerrainEffect:
                //    DataLib.DocFile terrainEffectFile = new DataLib.DocFile();
                //    if (rcToy.RcCategory == GameObjects.Toys.RcCategory.Car || rcToy.RcCategory == GameObjects.Toys.RcCategory.Tank )
                //    {
                //        terrainEffectFile.addText("Gold:", TextStyle.LF_UTitle);
                //        terrainEffectFile.addText("Speed boost", TextStyle.LF_Bread);
                //        terrainEffectFile.addText("Dirt and Sand:", TextStyle.LF_UTitle);
                //        terrainEffectFile.addText("Speed decrease", TextStyle.LF_Bread);
                //        terrainEffectFile.addText("ForrestGround and BurnedGround:", TextStyle.LF_UTitle);
                //        terrainEffectFile.addText("Large speed decrease", TextStyle.LF_Bread);
                //        terrainEffectFile.addText("Lava:", TextStyle.LF_UTitle);
                //        terrainEffectFile.addText("Large damage", TextStyle.LF_Bread);
                //        terrainEffectFile.addText("Water:", TextStyle.LF_UTitle);
                //        terrainEffectFile.addText("Small damage", TextStyle.LF_Bread);
                //        terrainEffectFile.addText("Iron:", TextStyle.LF_UTitle);
                //        terrainEffectFile.addText("Quick fire reload", TextStyle.LF_Bread);
                //        terrainEffectFile.addText("Leather:", TextStyle.LF_UTitle);
                //        terrainEffectFile.addText("Small bounce", TextStyle.LF_Bread);
                //        terrainEffectFile.addText("Purple skin:", TextStyle.LF_UTitle);
                //        terrainEffectFile.addText("Large bounce", TextStyle.LF_Bread);
                //    }
                //    else if (rcToy.RcCategory == GameObjects.Toys.RcCategory.Ship)
                //    {
                //        terrainEffectFile.addText("Will only work properly on water or lava", TextStyle.LF_Bread);
                //        terrainEffectFile.addText("Gold:", TextStyle.LF_UTitle);
                //        terrainEffectFile.addText("Speed boost and quick reload", TextStyle.LF_Bread);
                //        terrainEffectFile.addText("Iron:", TextStyle.LF_UTitle);
                //        terrainEffectFile.addText("Large damage", TextStyle.LF_Bread);
                //        terrainEffectFile.addText("Bronze:", TextStyle.LF_UTitle);
                //        terrainEffectFile.addText("Small damage", TextStyle.LF_Bread);
                //    }
                //    else //flying
                //    {
                //        terrainEffectFile.addText("Fly close above the block to get the material effect", TextStyle.LF_Bread);
                //        terrainEffectFile.addText("Gold:", TextStyle.LF_UTitle);
                //        terrainEffectFile.addText("Speed boost", TextStyle.LF_Bread);
                //        terrainEffectFile.addText("Iron:", TextStyle.LF_UTitle);
                //        terrainEffectFile.addText("Quick fire reload", TextStyle.LF_Bread);
                       
                //    }
                //    terrainEffectFile.addText("White:", TextStyle.LF_UTitle);
                //    terrainEffectFile.addText("Heal up", TextStyle.LF_Bread);
                //    menu.OpenDocument(terrainEffectFile);
                //    break;
                //case Link.SwordApperance:
                //    mFile = new File((int)MenuPageName.ChangeApperance);
                //    List<VoxelModelName> swords = availableSwords();
                //    foreach (VoxelModelName vn in swords)
                //    {
                //        mFile.AddTextLink(vn.ToString(), new HUD.Link((int)Link.SwordType_dialogue, (int)vn));
                //    }
                //    OpenMenuFile(mFile);
                //    break;
                case Link.DebugListHostedObj:
                    OpenMenuFile(LfRef.gamestate.GameObjCollection.DebugListGameObjects(true));
                    break;
                case Link.DebugListClientObj:
                    OpenMenuFile(LfRef.gamestate.GameObjCollection.DebugListGameObjects(false));
                    break;

                case Link.DebugListMeshes:
                    menu.OpenDocument(Ref.draw.ListMeshes());
                    break;
                case Link.DebugListUpdate:
                   // menu.OpenDocument(Ref.update.UpdateListToFile());
                    mFile = new File();
                    Ref.update.UpdateListToFile(mFile);
                    OpenMenuFile();
                    break;
                case Link.ReadAboutPermissions:
                    DataLib.DocFile aboutPerm = new DataLib.DocFile();
                    aboutPerm.addText("Spectator", TextStyle.LF_UTitle);
                    aboutPerm.addText("Will be able to walk around. Fight zombies. Can send a request once.", TextStyle.LF_Bread);
                    aboutPerm.addText("Build", TextStyle.LF_UTitle);
                    aboutPerm.addText("Will be able to add and remove blocks.", TextStyle.LF_Bread);
                    aboutPerm.addText("Full", TextStyle.LF_UTitle);
                    aboutPerm.addText("No limits to building or sending requests", TextStyle.LF_Bread);
                    menu.OpenDocument(aboutPerm);
                    break;
                case Link.RebootNetwork:
                    Ref.netSession.Disconnect();
                    openPage(MenuPageName.NetworkSettings);
                    break;
                
                case Link.SendMapToClients:
                    removeCurrentMessage();
                    new MapSender();
                    new Process.SendMapTime(this);
                    break;
                case Link.RequestBuildPermissionAccept:
                    //spara
                    //MainMenuState.SaveVisitedWorld();
                    UpdateClientPermission(currentMessage.SenderID, Players.ClientPermissions.Build, true);
                    removeCurrentMessage();
                    break;
                case Link.RequestMapDeny:
                    Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_RequestMapDenied, 
                        Network.PacketReliability.ReliableLasy, Index);
                    removeCurrentMessage();
                    break;
                case Link.MessageDeny:
                    removeCurrentMessage();
                    break;
                case Link.RequestMap:
                    Ref.netSession.BeginWritingPacketToHost(Network.PacketType.LF2_RequestMap, 
                        Network.PacketReliability.ReliableLasy, Index);
                    Print("Map request sent");
                    CloseMenu();
                    break;
                //case Link.ListMessages:
                //    openPage(MenuPageName.Messages);
                //    break;
                case Link.RequestBuildPermission:
                    Print("Permission request sent");
                    Ref.netSession.BeginWritingPacketToHost(Network.PacketType.LF2_RequestBuildPermission, 
                        Network.PacketReliability.ReliableLasy, Index);
                    CloseMenu();
                    break;
                
                case Link.NetSendMessage:
                    beginSendMessage();
                    break;
                
                //case Link.NameThisLocation:
                //    if (Data.WorldsSummaryColl.CurrentWorld.NamedAreas.Count < Data.NamedArea.MaxAreas)
                //    {
                //        Engine.XGuide.BeginKeyBoardInput(new KeyboardInputValues("Name of the location", "Max " + 
                //        Data.NamedArea.NameMaxLenght.ToString() + " chars", "Location", playerIx));
                //        waitingTextInputType = TextInputType.NameLocation;
                //        CloseMenu();
                //    }
                //    else
                //    {
                //        mFile = new File();
                //        mFile.AddTitle("To many");
                //        mFile.AddDescription("You can have max " + Data.NamedArea.MaxAreas.ToString() + " named areas");
                //        mFile.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)Link.BackToMain);
                //        OpenMenuFile(mFile);
                //    }
                    
                //    break;
                case Link.WarpTo:
                    openPage(MenuPageName.Travel);
                    break;
                case Link.WarpToCenter:
                    travelTo(Map.WorldPosition.CenterChunk);
                    CloseMenu();
                    break;
                
                case Link.NetworkSettings:
                    openPage(MenuPageName.NetworkSettings);
                    break;
                //case Link.ResumeRCmode:
                //    CloseMenu();
                //    mode = PlayerMode.RCtoy;
                //    break;
                //case Link.ExitRCmode:
                //    exitRCmode();
                //    break;
                
                case Link.DesignMode:
                    BeginCreationMode();
                    break;
                
                case Link.SaveAndExit:
                    if (LfRef.gamestate.LocalHostingPlayer == this)
                    {
                        LfRef.gamestate.QuitToMenu();
                        CloseMenu();
                    }
                    else
                    {
                        save(true);
                        DeleteMe();
                    }
                    break;
                case Link.ExitWithoutSave:
                    //Ref.netSession.Disconnect(null);
                    new GameState.ExitState(); //new MainMenuState();
                    break;
                case Link.YouDiedOk:
                    //remove money and place the hero on his start spot
                    progress.Items.Pay(LootfestLib.DeathCost);
                    hero.Restart(false);
                    CloseMenu();
                    this.LockControls(true);
                    break;
                case Link.CloseMenu:
                    CloseMenu();
                    break;
                case Link.QuitGameQuest:
                    mFile = new File();
                    if (LfRef.gamestate.LocalHostingPlayer == this)
                    {
                        mFile.AddTitle("Exit game");
                        mFile.AddDescription("Save and quit back to main menu");
                    }
                    else
                    {
                        mFile.AddTitle("Drop out");
                        mFile.AddDescription("Remove yourself from the splitscreen?");
                    }

                    if (Ref.netSession.IsHostOrOffline)
                    {
                        mFile.AddTextLink("Save and Exit", (int)Link.SaveAndExit);
                        if (!Settings.AutoSave)
                            mFile.AddTextLink("Exit without save", (int)Link.ExitWithoutSave);

                    }
                    else
                    {
                        mFile.AddTextLink("Leave world", (int)Link.SaveAndExit);
                    }
                    mFile.AddTextLink("Cancel", (int)Link.CloseMenu);
                    

                    OpenMenuFile(mFile);//File = quitFile;

                    break;
                
                case Link.Inventory:
                    mFile = new File();

                    mFile.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)Link.BackToMain);
                    OpenMenuFile(mFile);//File = inv;
                    break;
                case Link.BackToMain:
                    openPage(MenuPageName.MainMenu);
                    break;
                case Link.DEBUG:
                    openPage(MenuPageName.Debug);
                    break;
                case Link.DebugJump:
                    hero.JumpTo(new IntVector2(10)); 
                    CloseMenu();
                    break;
                case Link.DeugListChangedChunks:
                    mFile = new File();
                    LfRef.worldOverView.ListChangedChunks(mFile);
                    OpenMenuFile(mFile);
                    break;
                
                case Link.WatchValue:
                    OpenMenuFile(Debug.WatchValue.ListTweaks(100));
                    break;
                case Link.DebugNextSong:
#if CMODE
                    Music.MusicManager.NextSong();
#endif
                    break;
                case Link.UnlockMap:
                    CloseMenu();
                    break;
                case Link.DebugCrash:
                    Map.Chunk testScreen = LfRef.chunks.GetScreenUnsafe(new IntVector2(256));

                    break;
                case Link.DebugCorruptChunk:
                    LfRef.chunks.GetScreen(hero.ScreenPos).DebugCorruptChunk();
                    break;
                case Link.ScreenCheckLink:
                    ScreenCheck();
                    CloseMenu();
                    break;
                
                case Link.StartDebugMode:
                    hero.DebugMode = !hero.DebugMode;
                    if (hero.DebugMode)
                        localPData.view.Camera.CurrentZoom = 150;
                    CloseMenu();
                    break;
                case Link.Get100g:

                    progress.Items.AddMoney(100);

                    break;
                case Link.DebugFacts:
                    //collect all the facts in the world
                    ShowDebugFacts();
                    break;
                
                //case Link.GameSettings:
                //    openPage(MenuPageName.Settings);
                //    break;
#region APPERANCE
                case Link.Appearance:
                    openPage(MenuPageName.ChangeApperance);
                    break;

                case Link.ChangeHat:
                    mFile = new File();
                    mFile.Properties.ParentPage = (int)MenuPageName.ChangeApperance;
                    List<HatType> hattypes = new List<HatType>();
                    for (HatType i = (HatType)0; i < HatType.Pirate1; i++)
                    {
                        hattypes.Add(i);
                    }
#if CMODE
                    if (settings.UnlockedPiratePack)
                    {
#endif
                        hattypes.Add(HatType.Pirate1); hattypes.Add(HatType.Pirate2); hattypes.Add(HatType.Pirate3); 
#if CMODE
                    }
                    if (settings.UnlockedGirlyPack)
                    {
#endif
                        hattypes.Add(HatType.Girly1); hattypes.Add(HatType.Girly2);
#if CMODE
                    }
#endif
                    foreach (HatType hat in hattypes)
                    {
                        mFile.AddTextLink(hat.ToString(), new HUD.Link((int)Link.HatType_dialogue, (int)hat));//(int)hat, (int)Link.HatType_dialogue);
                    }
                    OpenMenuFile(mFile);
                    break;
                case Link.ChangeMouth:
                    mFile = new File();
                    mFile.Properties.ParentPage = (int)MenuPageName.ChangeApperance;
                    List<MouthType> mouthtypes = new List<MouthType>();
                    for (MouthType i = (MouthType)0; i < MouthType.NUM; i++)
                    {
                        mouthtypes.Add(i);
                    }
#if CMODE
                    if (settings.UnlockedPiratePack)
                    {
#endif
                        mouthtypes.Add(MouthType.Pirate);
#if CMODE
                    }
                    if (settings.UnlockedGirlyPack)
                    {
#endif
                        //mouthtypes.Add(MouthType.Girly1); mouthtypes.Add(MouthType.Girly2);
#if CMODE
                    }
#endif
                    foreach (MouthType mouth in mouthtypes)
                    {
                        mFile.AddTextLink(mouth.ToString(), new HUD.Link((int)Link.MouthType_dialogue, (int)mouth));
                    }
                    OpenMenuFile(mFile);
                    break;
                case Link.ChangeEyes:
                    mFile = new File((int)MenuPageName.ChangeApperance);
                    mFile.Properties.ParentPage = (int)MenuPageName.ChangeApperance;
                    List<EyeType> eyetypes = new List<EyeType>();
                    for (EyeType i = (EyeType)0; i < EyeType.NUM; i++)
                    {
                        eyetypes.Add(i);
                    }
#if CMODE
                    if (settings.UnlockedPiratePack)
                    {
                        eyetypes.Add(EyeType.Pirate);
                    }
                    if (settings.UnlockedGirlyPack)
                    {
                        eyetypes.Add(EyeType.Girly1); eyetypes.Add(EyeType.Girly2); eyetypes.Add(EyeType.Girly3);
                    }
#else
                   // eyetypes.Add(EyeType.Pirate); eyetypes.Add(EyeType.Girly1); eyetypes.Add(EyeType.Girly2); eyetypes.Add(EyeType.Girly3);
#endif
                    foreach (EyeType eye in eyetypes)
                    {
                        mFile.AddTextLink(eye.ToString(), new HUD.Link((int)Link.EyesType_dialogue, (int)eye));//(int)eye, (int)Link.EyesType_dialogue);
                    }
                    OpenMenuFile(mFile);
                    break;
                case Link.ClothColor:
                    listMaterials(Link.ClothColor_dialogue, (int)MenuPageName.ChangeApperance);
                    break;
                case Link.PantsColor:
                    listMaterials(Link.PantsColor_dialogue, (int)MenuPageName.ChangeApperance);
                    break;
                case Link.ShoeColor:
                    listMaterials(Link.ShoeColor_dialogue, (int)MenuPageName.ChangeApperance);
                    break;
                case Link.HairColor:
                    listMaterials(Link.HairColor_dialogue, (int)MenuPageName.ChangeApperance);
                    break;
                case Link.BeltColor:
                    listMaterials(Link.BeltColor_dialogue, (int)MenuPageName.ChangeApperance);
                    break;
                case Link.BeltBuckleColor:
                    listMaterials(Link.BeltBuckleColor_dialogue, (int)MenuPageName.ChangeApperance);
                    break;
                case Link.BeltType:
                    mFile = new File((int)MenuPageName.ChangeApperance);
                    mFile.Properties.ParentPage = (int)MenuPageName.ChangeApperance;
                    List<string> beltTypes = new List<string>();
                    for (BeltType t = (BeltType)0; t < BeltType.NUM; t++)
                    {
                        mFile.AddTextLink(TextLib.EnumName(t.ToString()), new HUD.Link((int)Link.BeltType_dialogue, (int)t));//(int)t, (int)Link.BeltType);
                    }
                    OpenMenuFile();
                    break;
                
                case Link.Beard: //välj skägg
                    mFile = File.OpenOptionList(LanguageManager.Wrapper.AppearanceBeardTitle(),
                    new List<DataLib.OptionLink>
                    {
                        new DataLib.OptionLink(LanguageManager.Wrapper.AppearanceFacialNon(), (int)Players.BeardType.Shaved),
                        new DataLib.OptionLink(LanguageManager.Wrapper.AppearanceFacialSmallBeard(), (int)Players.BeardType.BeardSmall),
                        new DataLib.OptionLink(LanguageManager.Wrapper.AppearanceFacialLargeBeard(), (int)Players.BeardType.BeardLarge),
                        new DataLib.OptionLink(LanguageManager.Wrapper.AppearanceFacialPlumbersMustache(), (int)Players.BeardType.MustachePlummer),
                        new DataLib.OptionLink(LanguageManager.Wrapper.AppearanceFacialBikersMustache(), (int)Players.BeardType.MustacheBiker)

                    }, (int)Link.BeardType_dialogue);
                    mFile.Properties.ParentPage = (int)MenuPageName.ChangeApperance;
                    OpenMenuFile();
                    break;
                case Link.SkinnColor:
                    listMaterials(Link.SkinnColor_dialogue, (int)MenuPageName.ChangeApperance);
                    break;

                case Link.HatMainColor:
                    listMaterials(Link.HatMainColor_dialogue, (int)MenuPageName.ChangeApperance);
                    break;
                case Link.CapeColor:
                    listMaterials(Link.CapeColor_dialogue, (int)MenuPageName.ChangeApperance);
                    break;
                case Link.HatDetailColor:
                    listMaterials(Link.HatDetailColor_dialogue, (int)MenuPageName.ChangeApperance);
                    break;
                case Link.BeardType_dialogue:
                    Settings.BeardType = (BeardType)link.Value2;
                    if (Settings.BeardType == BeardType.Shaved)
                    {
                        openPage(MenuPageName.ChangeApperance);
                        hero.UpdateAppearance(false);
                    }
                    else
                    {
                        listMaterials(Link.BeardColor_dialogue, (int)MenuPageName.ChangeApperance);
                    }
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                case Link.HairColor_dialogue:
                    Settings.hairColor = (byte)link.Value2;
                    openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                case Link.SkinnColor_dialogue:
                    Settings.SkinColor = (byte)link.Value2;
                    openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                case Link.BeardColor_dialogue:
                    Settings.BeardColor = (byte)link.Value2;
                    openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                case Link.ClothColor_dialogue:
                    Settings.ClothColor = (byte)link.Value2;
                    openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;

                case Link.PantsColor_dialogue:
                    Settings.PantsColor = (byte)link.Value2;
                    openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                case Link.ShoeColor_dialogue:
                    Settings.ShoeColor = (byte)link.Value2;
                    openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                case Link.HatMainColor_dialogue:
                    Settings.HatMainColor = (byte)link.Value2;
                    openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                case Link.HatDetailColor_dialogue:
                    Settings.HatDetailColor = (byte)link.Value2;
                    openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                case Link.CapeColor_dialogue:
                    Settings.CapeColor = (byte)link.Value2;
                    openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;

                case Link.BeltColor_dialogue:
                    Settings.BeltColor = (byte)link.Value2;
                    openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                case Link.BeltBuckleColor_dialogue:
                    Settings.BeltBuckleColor = (byte)link.Value2;
                    openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                case Link.BeltType_dialogue:
                    Settings.BeltType = (BeltType)link.Value2;
                    openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                case Link.HatType_dialogue:
                    Settings.HatType = (HatType)link.Value2;
                    //openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                case Link.EyesType_dialogue:
                    Settings.EyesType = (EyeType)link.Value2;
                    //openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                case Link.MouthType_dialogue:
                    Settings.MouthType = (MouthType)link.Value2;
                    //openPage(MenuPageName.ChangeApperance);
                    hero.UpdateAppearance(false);
                    SettingsChanged();
                    appearanceChanged = true;
                    break;
                #endregion

                //OLD DIALOGUE

                        
                case Link.OpenMenuType_dialogue:
                    openPage((MenuPageName)link.Value2);
                    break;
                case Link.OpenMessage_dialogue:
                    currentMessage = messages[link.Value2];
                    OpenMenuFile(currentMessage.OpenMessage());
                    break;
                        
                //case Link.ListRCcolors_dialogue:
                //    changeRCcolorIx = link.Value2;
                //    listMaterials(Link.RCcolor_dialogue, (int)MenuPageName.RCPauseMenu);
                //    break;
                case Link.GamerWarpTo_dialogue:
                    Map.WorldPosition wp = new Map.WorldPosition(
                        LfRef.gamestate.GetPlayerPosition((byte)link.Value2));
                   
                    if (wp.ChunkGrindex.X > 2)
                    {
                        travelTo(wp.ChunkGrindex);
                    }
                    CloseMenu();
                    break;
                case Link.GamerViewCard_dialogue:
                    Network.AbsNetworkPeer gamer = Ref.netSession.GetGamerFromID((byte)link.Value2);
                    Engine.XGuide.ViewGamerCard(gamer, playerIx);
                    break;
                case Link.GamerKickOutOptions_dialogue:
                    mFile = new File((int)MenuPageName.NetworkSettings);
                    Network.AbsNetworkPeer gamer2 = Ref.netSession.GetGamerFromID((byte)link.Value2);
                    mFile.AddTitle("You are about to exclude " + gamer2.ToString() + " from the game.");
                    mFile.AddTextLink("Cancel", (int)Link.NetworkSettings);
                    mFile.AddTextLink("Remove only", new HUD.Link((int)Link.GamerKickOutOK_dialogue, link.Value2));
                    mFile.AddTextLink("Remove and block", new HUD.Link((int)Link.GamerKickOutBannOK_dialogue, link.Value2));
                    mFile.AddTextLink("Block and report", new HUD.Link((int)Link.GamerKickOutBannReportOK_dialogue, link.Value2));
                    OpenMenuFile(mFile);//File = mFile;
                    break;
                case Link.GamerKickOutBannReportOK_dialogue:
                    Engine.XGuide.ViewGamerCard(Ref.netSession.GetGamerFromID((byte)link.Value2), Index);
                    removePlayer(true, link.Value2);
                    break;
                case Link.GamerKickOutBannOK_dialogue:
                    removePlayer(true, link.Value2);
                    break;
                case Link.GamerKickOutOK_dialogue:
                    removePlayer(false, link.Value2);
                    break;
                //case Link.SelectRCtoy_dialogue:
                //    GameObjects.Toys.ToyType toyType = (GameObjects.Toys.ToyType)link.Value2;
                  
                //    beginRCmode(toyType);
                //    break;

                case Link.Warp_dialogue:
                    hero.JumpTo(PlayState.DebugWarpLocations[link.Value2].AreaChunkCenter);
                    CloseMenu();
                    break;
                    
            }
        }


        List<GameObjects.PickUp.VoxelObjPresent> voxelObjPresents = new List<GameObjects.PickUp.VoxelObjPresent>();

        void DialogueClick(Link dialogue, HUD.Link link, int playerIx)
        {
           
        }

        
       

        public void updateSettings()
        {
            LfRef.gamestate.ChangedSettings(Index, Settings);
        }
        void ShowDebugFacts()
        {
            DataLib.DocFile file = new DataLib.DocFile();
            GameObjects.Magic.MagicLib.DebugFacts(file);
            menu.OpenDocument(file);
        }

        int numHealthPrompts = 3;
        bool prompedUseHealth = false;
        bool firstTimeUsingHealth = true;
        public void HandleDamage()
        {
           
            exitCurrentMode();
            
            if (!hero.Alive)
            {
                openPage(MenuPageName.GameOver);
                LockControls(false);
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_PlayerDied, 
                    Network.PacketReliability.ReliableLasy, Index);

                LfRef.gamestate.Progress.HeroDied();

                Settings.NumTimesDied++;
                hero.DeathAnimation(true);
                if (localHost)
                    LfRef.gamestate.MusicDirector.DeathEvent();

            }
            if (numHealthPrompts > 0 && !prompedUseHealth && hero.PercentHealth < 0.4f && progress.Items.GotHealing())
            {
                numHealthPrompts--;
                prompedUseHealth = true;

                if (firstTimeUsingHealth && PlatformSettings.HelpAndTutorials)
                {
                    firstTimeUsingHealth = false;
                    beginButtonTutorial(new ButtonTutorialArgs(LootfestLib.InputHealUpImg, LootfestLib.InputHealUp, null, "to eat healthy food", safeScreenArea));
                }
                else
                {
                    Print("to heal", LootfestLib.InputHealUpImg);
                }
            }

            
        }

        public bool ReadyForButtonTutorial { get { return mode == PlayerMode.Play && QuestDialogue == null; } }

        public void beginButtonTutorial(ButtonTutorialArgs tut)
        {
            if (tut.begin)
            {
                if (ReadyForButtonTutorial)
                {
                    Music.SoundManager.PlayFlatSound(LoadedSound.Dialogue_DidYouKnow);
                    exitCurrentMode();
                    mode = PlayerMode.ButtonTutorial;
                    buttonTutorial = new ButtonTutorial(tut);
                    hero.Immortal = true;
                }
                else
                {
                    new PostPhonedTutorial(this, tut);
                }
            }
            else
            {
                mode = PlayerMode.Play;
                buttonTutorial.DeleteMe();
                buttonTutorial = null;
                hero.Immortal = false; 
            }
        }

        public override void DeleteMe()
        {
            Ref.update.AddToUpdate(this, false);
            clearGamerNames();
            Settings.Save(true, this);
            HideControls();
            messageHandler.DeleteMe();
            compass.DeleteMe();
            if (menu != null)
                menu.DeleteMe();
            LfRef.gamestate.RemoveScreenPlayer(this);
            base.DeleteMe();

            foreach (GamerName gn in gamerNames)
            {
                gn.DeleteMe();
            }
            foreach (AbsHUD h in HUDgroup)
            {
                h.DeleteMe();
            }
            localPData.IsActive = false;

            if (pauseBorder != null)
                pauseBorder.DeleteMe();
            if (pauseText != null)
                pauseText.DeleteMe();

            Director.LightsAndShadows.Instance.LocalGamerJoinedEvent();
        }
        public void SaveAndExit()
        {
            messageHandler = null;
            Settings.Save(true, this);
            save(true);
        }

        public void AddItem(GameObjects.Gadgets.GoodsType g, int count)
        {
            if (GameObjects.Gadgets.GadgetLib.IsGoodsType(g))
            {
                AddItem(new GameObjects.Gadgets.Goods(g, count), false);
            }
            else
            {
                AddItem(new GameObjects.Gadgets.Item(g, count), false);
            }
        }

        public void AddItem(GameObjects.Gadgets.IGadget gadget, bool message)
        {

            progress.Items.AddItem(gadget);
            if (message)
            {
                if (gadget.GadgetType == GameObjects.Gadgets.GadgetType.GadgetList)
                {
                    GameObjects.Gadgets.GadgetList list = gadget as GameObjects.Gadgets.GadgetList;
                    foreach (GameObjects.Gadgets.IGadget g in list.Gadgets)
                    {
                        messageHandler.addMessage(new PickupMessage(messageHandler.NextPosition(), g));
                    }
                }
                else
                {
                    messageHandler.addMessage(new PickupMessage(messageHandler.NextPosition(), gadget));
                }
            }

        }



        public bool BuyItem(GameObjects.Gadgets.ShopItem item)
        {
            if (progress.Items.Pay(item.Price))
            {

                if (item.Item.GadgetType == GameObjects.Gadgets.GadgetType.Goods)
                {
                    GameObjects.Gadgets.Goods g = (GameObjects.Gadgets.Goods)item.Item;
                    g.Amount = 1;

                    item.Item = g;
                }
                AddItem(item.Item, true);
                Music.SoundManager.PlayFlatSound(LoadedSound.PickUp);
                return true;
            }
            Music.SoundManager.PlayFlatSound(LoadedSound.shieldcrash);
            return false;
        }


        public void CloseMenu()
        {
#if WINDOWS
            Debug.DebugLib.CrashIfThreaded();
#endif
#if PCGAME
            Engine.Input.CenterMouse = true;
#endif
            if (hero.Alive)
            {
                if (hero.InteractingWith != null)
                    hero.InteractingWith.InteractEvent(hero, false);
                if (ValueDialogue != null)
                {
                    ValueDialogue.DeleteMe();
                    ValueDialogue = null;
                }
                mode = PlayerMode.Play;

                if (menu != null)
                    menu.DeleteMe();
                menu = null;
                updateCamZoom();
                currentMenu = MenuPageName.EMPTY;
                if (menuTabs != null)
                {
                    menuTabs.DeleteMe();
                    menuTabs = null;
                }

                if (settingsChanged)
                {
                    Settings.Save(true, this);

                    save(true);
                    if (appearanceChanged)
                    {
                        NetShareAppearance();
                    }
                    settingsChanged = false;
                }
            }

            hero.UpdateShield();

        }

        

        static readonly Range ZoomModeRange = new Range(
            (PlatformSettings.Debug == BuildDebugLevel.DeveloperDebug_1 ? (int)ZoomMode.Debug : (int)ZoomMode.TopView), 
            (int)ZoomMode.NUM - 1);
        static readonly IntervalF CurrentZoomRange = new IntervalF(0, 1);

        const float ZoomMultiplier = 8;

        const float DebugMaxZoom = 80f * ZoomMultiplier;
        const float DebugMinZoom = 8.4f * ZoomMultiplier;
        const float TopViewMaxZoom = DebugMinZoom;
        const float TopViewMinZoom = 3 * ZoomMultiplier;
        const float IsoViewMaxZoom = TopViewMinZoom;
        const float IsoViewMinZoom = 2 * ZoomMultiplier;
        const float ShoulderViewMaxZoom = IsoViewMinZoom;
        const float ShoulderViewMinZoom = 1 * ZoomMultiplier;

        static readonly IntervalF ZoomRangeDebug = new IntervalF(DebugMinZoom, DebugMaxZoom);
        static readonly IntervalF ZoomRangeTop = new IntervalF(TopViewMinZoom, TopViewMaxZoom);
        static readonly IntervalF ZoomRangeIso = new IntervalF(IsoViewMinZoom, IsoViewMaxZoom);
        static readonly IntervalF ZoomRangeShoulder = new IntervalF(ShoulderViewMinZoom, ShoulderViewMaxZoom);

        const float DebugMaxAngle = 0.3f;
        const float DebugMinAngle = 0.6f;
        const float TopViewMaxAngle = DebugMinAngle;
        const float TopViewMinAngle = 0.9f;
        const float IsoViewMaxAngle = TopViewMinAngle;
        const float IsoViewMinAngle = 1.14f;
        const float ShoulderViewMaxAngle = IsoViewMinAngle;
        const float ShoulderViewMinAngle = 1.24f;

        
        void nextPage(bool rightDir)
        {
            if (menuTabs != null)
            {
                Music.SoundManager.PlayFlatSound(rightDir? LoadedSound.MenuSelect : LoadedSound.MenuBack);
                int page = menuTabs.TabEvent(rightDir);
                if (currentMenu == MenuPageName.ChestDialogue)
                {
                    updateChestDialogue();

                }
                else if (hero.InteractingWith != null)
                {
                    OpenMenuFile(hero.InteractingWith.Interact_MenuTab(page, hero));
                }
            }
        }

        void updateChestDialogue()
        {
            if (hero.InteractingWith == null)
            {
                CloseMenu();
            }
            else
            {
                mFile = new File();
                switch (menuTabs.Page)
                {
                    case ChestToBackpackPage:

                        mFile.AddTitle(hero.InteractingWith.ToString());

                        GameObjects.Gadgets.GadgetsCollection items = ((GameObjects.EnvironmentObj.Chest)hero.InteractingWith).Data.GadgetColl;
                        if (items.Empty)
                        {
                            mFile.AddDescription("--The " + hero.InteractingWith + " is empty--");
                        }
                        else
                        {
                            mFile.AddTextLink("Take everything", (int)Link.TakeEverything);
                            items.ToMenu(mFile, new GadgetLink(PickFromChest_dialogue, progress.Items, null, this), GameObjects.Gadgets.MenuFilter.All);
                        }

                        break;
                    case BackpackToChestPage:
                        mFile.AddTitle("Backpack");
                        progress.Items.ToMenu(mFile, new GadgetLink(DropToChest_dialogue, progress.Items, null, this), GameObjects.Gadgets.MenuFilter.All);
                        break;
                }
                OpenMenuFile(mFile);//File = file;
            }
        }

        static readonly IntervalF AngleRangeDebug = new IntervalF(DebugMinAngle, DebugMaxAngle);
        static readonly IntervalF AngleRangeTop = new IntervalF(TopViewMinAngle, TopViewMaxAngle);
        static readonly IntervalF AngleRangeIso = new IntervalF(IsoViewMinAngle, IsoViewMaxAngle);
        static readonly IntervalF AngleRangeShoulder = new IntervalF(ShoulderViewMinAngle, ShoulderViewMaxAngle);
        
        //lägg till cam target adj

        void updateCamZoom()
        {
            if (!CurrentZoomRange.IsWithinRange(currentZoomPerc))  
            {
                ZoomMode oldZoom = zoomMode;
                zoomMode = (ZoomMode)Bound.Set((int)zoomMode - lib.ToLeftRight(currentZoomPerc), ZoomModeRange);
                if (oldZoom == zoomMode)
                {
                    currentZoomPerc = Bound.Set(currentZoomPerc, CurrentZoomRange);
                }
                else
                {
                    currentZoomPerc -= lib.ToLeftRight(currentZoomPerc);
                    
                }
                
            }

            IntervalF zoomRange;
            IntervalF angleRange;
            
            switch (zoomMode)
            {
                case ZoomMode.Debug:
                    zoomRange = ZoomRangeDebug;
                    angleRange = AngleRangeDebug;
                    break;
                case ZoomMode.TopView:
                    zoomRange =ZoomRangeTop;
                    angleRange = AngleRangeTop;
                    break;
                default:
                    zoomRange = ZoomRangeIso;
                    angleRange = AngleRangeIso;
                    break;
                case ZoomMode.CloseUp:
                    zoomRange =ZoomRangeShoulder;
                    angleRange = AngleRangeShoulder;
                    break;
            }

            localPData.view.Camera.CurrentZoom = zoomRange.GetFromPercent(currentZoomPerc);
            localPData.view.Camera.TiltY = angleRange.GetFromPercent(currentZoomPerc);
            
        }
        const int ChestToBackpackPage = 0;
        const int BackpackToChestPage = 1;
        //HUD.MenuTabs menuTabs = null;
        

#if PCGAME 
        //Vector2 WASDdir = Vector2.Zero;
        //public override void MouseScroll_Event(int scroll, Vector2 position)
        //{
            
        //    if (mode == PlayerMode.Creation)
        //    {
        //         voxelDesigner.MouseScroll_Event(scroll, position);
        //    }
        //    else if (mode == PlayerMode.InMenu)
        //    {
        //        MouseScroll(scroll);
        //    }
        //}
        //public override void KeyPressEvent(Microsoft.Xna.Framework.Input.Keys key, bool keydown)
        //{
           
            
        //}

        public override void MouseClick_Event(Vector2 position, MouseButton button, bool keydown)
        {
            switch (mode)
            {
                case PlayerMode.Play:

                    if (button == MouseButton.Left)
                        progress.Equipped.Use(EquipSlot.Primary, keydown, hero, progress.Items);
                    //hero.Attack(equipped.Weapons[numBUTTON.A], keydown, (byte)0);
                    else if (button == MouseButton.Right && keydown)
                        throwSpear();
                    break;
                //case PlayerMode.Creation:
                //    voxelDesigner.MouseClick_Event(position, button, keydown);
                //    break;
                //case PlayerMode.InMenu:
                //    MouseClick_Event(position, button, keydown);
                //    break;
            }
        }
        public override void MouseMove_Event(Vector2 position, Vector2 deltaPos)
        {
            switch (mode)
            {
                case PlayerMode.Play:
                    moveCamera(deltaPos * 1);
                    break;
                //case PlayerMode.Creation:
                //    voxelDesigner.MouseMove_Event(position, deltaPos);
                //    break;
            }
        }
#endif


        

        public void UseHealing(GameObjects.Gadgets.FoodHealingPower healing)
        {
            if (healing.BasicHealing > 0)
            {
                firstTimeUsingHealth = false;
                hero.Health += healing.BasicHealing * LootfestLib.HeroHealth;
                Print("Yum!", GameObjects.Gadgets.GadgetLib.GadgetIcon(healing.Goods.Type));
                prompedUseHealth = false;
                new Effects.HealUp(hero, true, false);
                Music.SoundManager.PlayFlatSound(LoadedSound.HealthUp);
                new GameObjects.PickUp.FoodScrap(hero, healing.Goods.Type);
            }
        }


        public void DialogueClosedEvent()
        {
            //done with opening phrase
            dialogue = null;
            if (mode == PlayerMode.InDialogue)
            {
                openPage(MenuPageName.TalkingToNPC);
            }
        }


      
        
        public void exitCurrentMode()
        {
            if (buttonTutorial != null)
            {
                buttonTutorial.DeleteMe();
                buttonTutorial = null;
            }
            deleteInputDialogue();
            if (exitModeButton != null)
                exitModeButton.DeleteMe();
            if (menu != null)
                CloseMenu();
            if (voxelDesigner != null)
                EndCreationMode();

            else if (miniMap != null)
                openMiniMap(false);

        }

       

        

        VectorRect menuArea()
        {
            float w = Menu.Scale * LoadTiles.MenuRectangleWidth;
            if (w > safeScreenArea.Width)
            {
                w = safeScreenArea.Width;
            }
            return new VectorRect(
                safeScreenArea.Position + new Vector2(0, 10),
                new Vector2(w, safeScreenArea.Height));
        }

        

        void messageHistory()
        {
            OpenMenuFile(LfRef.gamestate.ChatHistoryFile(openPage));
        }

        void aboutPermissions(HUD.File file)
        {
            file.AddIconTextLink(SpriteName.IconInfo, "Permissions", (int)Link.ReadAboutPermissions);
        }

        GameObjects.EnvironmentObj.IInteractionObj lastInteraction = null;

        void beginOpenMenu()
        {
#if WINDOWS
            Debug.DebugLib.CrashIfThreaded();
#endif
            if (menu == null)
            {
                menu = new Menu(menuArea(), safeScreenArea,
                    ImageLayers.Foreground7, Index);
                mFile = null;
                Music.SoundManager.PlayFlatSound(LoadedSound.MenuSelect);
            }
            mode = PlayerMode.InMenu;
        }

        public override void BlockMenuOpenPage(int page, int playerIx)
        {
            openPage((MenuPageName)page);
        }

        public void UnlockThrophy(Trophies type)
        {
            if (!Settings.UnlockedThrophy[(int)type])
            {
                Music.SoundManager.PlayFlatSound(LoadedSound.Trophy);

                SpriteName iconTile = LootfestLib.TrophyIcon(type);
                Print("Tropy unlocked", iconTile);
                Settings.UnlockedThrophy[(int)type] = true;
                SettingsChanged();

                if (Settings.UnlockedAllTrophies)
                {
                    //view a gratulation message
                    exitCurrentMode();
                    mFile = new File();
                    mFile.AddTitle("Congratulations!");
                    mFile.AddDescription("All trophies are unlocked.");
                    mFile.AddDescription("You got a new friend, waiting for you in the appearance settings.");
                    mFile.AddTextLink("Take me there", new HUD.ActionLink(pageAppearance));
                    mFile.AddIconTextLink(SpriteName.LFIconGoBack, "Back", new HUD.ActionLink(CloseMenu));
                    LockControls(false);
                    OpenMenuFile();
                }
                else
                {
                    // View a throphy icon in the center of the screen for a moment
                    Image backg = new Image(SpriteName.TrophyUnlocked, new Vector2(Engine.Screen.CenterScreen.X, Engine.Screen.Height * 0.3f),
                        new Vector2(Engine.Screen.Height * 0.12f), ImageLayers.Lay2, true);
                    Graphics.Parent p = new Graphics.Parent();
                    p.InitParent(backg);
                    p.StandardRelation = new ChildRelation(true, true, false);

                    Image icon = new Image(iconTile, Vector2.Zero, Vector2.One, ImageLayers.Lay1, true);
                    p.AddChild(icon, Vector2.Zero, Vector2.One, 0, -1);

                    //ingoing
                    const float InTime = 160;
                    new Graphics.Motion2d(MotionType.SCALE, p, new Vector2(backg.Width * 0.4f), MotionRepeate.BackNForwardOnce, InTime, true);

                    //outgoing
                    const float ViewTime = 1400;
                    const float OutTime = InTime * 2;
                    new Timer.UpdateTrigger(ViewTime,
                        new Graphics.Motion2d(MotionType.TRANSPARENSY, backg, new Vector2NegativeOne, MotionRepeate.NO_REPEATE, OutTime, false),
                        true);
                    new Timer.UpdateTrigger(ViewTime,
                       new Graphics.Motion2d(MotionType.TRANSPARENSY, icon, new Vector2NegativeOne, MotionRepeate.NO_REPEATE, OutTime, false),
                       true);
                    new Timer.Terminator(ViewTime + OutTime, p);


                }
            }
        }

       

        float lastOutOfAmmoMessage = 0;
        public void OutOfAmmo(string ammoType)
        {
            Music.SoundManager.PlayFlatSound(LoadedSound.out_of_ammo);
            if (Ref.update.TotalGameTime - lastOutOfAmmoMessage > 1000)
            {
                lastOutOfAmmoMessage = Ref.update.TotalGameTime;
                Print("Out of " + ammoType);
            }
        }

        void appearanceSelectPetTypeMenu()
        {
            mFile = new File();
            mFile.Properties.ParentPage = (int)MenuPageName.ChangeApperance;
            for (int i = 0; i < (int)GameObjects.Characters.FlyingPetType.NUM; i++)
            {
                mFile.AddTextLink(((GameObjects.Characters.FlyingPetType)i).ToString(), new HUD.Link(appearanceSelectPetType, i));
            }
            OpenMenuFile();
        }
        
        void appearanceSelectPetType(int playerIndex, HUD.Link value)
        {
            Settings.FlyingPetType = (GameObjects.Characters.FlyingPetType)value.Value1;
            Settings.UseFlyingPet = true;
            appearanceChanged = true;
            mFile = new File();
            if (hero.HasFlyingPet)
            {
                mFile.AddTitle("Reset pet?");
                mFile.AddDescription("You must do this to change its appearance.");
            }
            mFile.AddDescription("The pet will appear in " + GameObjects.Characters.FlyingPet.ResetPetTimeSec.ToString() + "seconds.");
            if (hero.HasFlyingPet)
            {
                mFile.AddTextLink("Reset", new HUD.Link(resetPet));
                mFile.AddTextLink("Cancel", new HUD.Link(openPage, (int)MenuPageName.ChangeApperance));
            }
            else
            {
                mFile.AddTextLink("OK", new HUD.Link(openPage, (int)MenuPageName.ChangeApperance));
            }
            OpenMenuFile();
        }

        void resetPet()
        {
            hero.RemovePet();
            openPage(MenuPageName.ChangeApperance);
        }
        void removePet()
        {
            hero.RemovePet();
            Settings.UseFlyingPet = false;
        }

        public void OpenMenuFile()
        {
            this.OpenMenuFile(mFile);
        }
        public void OpenMenuFileFromThread(HUD.File file, int menuId)
        {
            if (menu != null && menu.Visible && menuId == menu.MenuId)
            {
                this.OpenMenuFile(file);
            }
        }
        
        public void OpenMenuFile(HUD.File file)
        {
            if (hero.Alive)
            {
                beginOpenMenu();
                
                menu.File = file;
                Music.SoundManager.PlayFlatSound(LoadedSound.MenuSelect);
            }
        }

        public void FriendLostController(bool lost)
        {
            if (hero.Alive)
            {
                if (lost)
                {
                    if (mode != PlayerMode.LostController)
                    {
                        LockControls(false);
                        openPage(MenuPageName.FriendLostController);
                        mode = PlayerMode.FriendLostController;
                    }
                }
                else if (mode == PlayerMode.FriendLostController)
                {
                    openPage(MenuPageName.MainMenu);
                }
            }
        }


        Graphics.AbsCamera savedCamera;

        public void SetRandomMaterial()
        {
            if (voxelDesigner != null)
            {
                voxelDesigner.SetRandomMaterial();
            }
        }

        const string StillLoadingMessage = "Hold on, still loading";



        void updateLocalVisualMode(float time, PlayerMode mode)
        {
            if (Engine.XGuide.IsVisible)
            {
                setVisualMode(VisualMode.Guide, true);
            }
            else if (visualMode == VisualMode.Guide)
            {
                if (mode == PlayerMode.InMenu)
                {
                    setVisualMode(VisualMode.Menu, true);
                }
                else
                {
                    setVisualMode(VisualMode.Non, true);
                }
            }

        }

        public void Update(float time, PlayState gamestate)
        {
            if (menu == null && dialogue == null)
            {

                const float TargetInfront = 3f / NumAngleSamples;
                if (hero != null)
                    characterGoalAngleSamples[currentAngleSample.Value] = hero.Rotation.Direction(TargetInfront);
                currentAngleSample.Next();

                camGoalAngle = Vector2.Zero;
                for (int i = 0; i < NumAngleSamples; i++)
                {
                    camGoalAngle.X += characterGoalAngleSamples[i].X;
                    camGoalAngle.Y += characterGoalAngleSamples[i].Y;
                }


            }

            localPData.view.Camera.UsePositionFromRotation = true;
            localPData.Time_Update();
            if (mode == PlayerMode.Creation)
            {
                if (voxelDesigner != null)
                    voxelDesigner.Time_Update(time);
            }
            else
            {
#if PCGAME
                if (WASDdir != Vector2.Zero)
                {
                    moveCharacter(WASDdir);
                }
#endif
                if (menu == null)
                {
                    Vector3 target = hero.Position;
                    localPData.view.Camera.ChaseTargetSpeed = HeroCamChase;
                    {
                        target.X += camGoalAngle.X;
                        target.Z += camGoalAngle.Y;
                        target.Y += 2;
                    }

                    localPData.view.Camera.GoalTarget(target);
                }

            }
            localPData.view.Camera.Time_Update(time);
            foreach (GamerName gn in gamerNames)
            {
                gn.Update(localPData.view.Camera, localPData.view.Viewport, safeScreenArea);
            }

            compass.Update(gamestate);
            monsterSpawn.Update(time, hero);
            treasureSpawn.Update(time, hero);

            if (Ref.update.LasyUpdatePart == LasyUpdatePart.Part6_Player)
            {
                //calculate the wanted camera angle
                //the camera should turn to show the direction of walking
                //höja vyn lite när man går emot kameran

                updateLocalVisualMode(time, mode);
                if (!Engine.Update.IsRunningSlow)
                {
                    UpdateGamerNames();
                }
                if (mode == PlayerMode.LostController)
                {

                    if (!localPData.LostController)
                    {
                        for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
                        {
                            GameObjects.Characters.Hero h = LfRef.LocalHeroes[i];
                            h.Player.FriendLostController(false);
                        }
                    }
                }
                else if (!hero.IsBot)
                {
                    if (localPData.LostController)
                    {
                        openPage(MenuPageName.LostController);
                        mode = PlayerMode.LostController;
                        for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
                        {
                            GameObjects.Characters.Hero h = LfRef.LocalHeroes[i];
                            h.Player.FriendLostController(true);
                        }

                    }
                }
                if (controlLock > 0)
                {
                    controlLock -= Ref.update.LazyUpdateTime;
                    if (controlLock <= 0)
                    {
                        hero.CheckIfUnderGround();
                    }
                }
                Settings.TimeSpent += Ref.update.LazyUpdateTime;
                progress.Update(Ref.update.LazyUpdateTime);
                viewNewMapLocationTime.CountDown(Ref.update.LazyUpdateTime);

                progress.Items.CalcWeight();
            }

            {//Debug camera at 0, 0, 0
               // ControllerLink.view.Camera.Target = Vector3.Zero;
                
            }
        }

        public void UpdateGamerNames()
        {
            if (Settings.ViewPlayerNames)
            {

                foreach (GamerName gn in gamerNames)
                {
                    gn.Keep = false;
                }

               //List<GameObjects.Characters.Hero> heroes = LfRef.gamestate.AllHeroes();
                for (int i = 0; i < LfRef.AllHeroes.Count; ++i)//foreach (GameObjects.Characters.Hero h in LfRef.AllHeroes)
                {
                    GameObjects.Characters.Hero h = LfRef.AllHeroes[i];
                    if (!h.NetworkLocalMember)
                    {
                        if (h.PositionDiff(hero).Length() <= 60)
                        {
                            bool found = false;
                            foreach (GamerName gn in gamerNames)
                            {
                                if (gn.Hero == h)
                                {
                                    gn.Keep = true;
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                                gamerNames.Add(new GamerName(h, h.clientPlayer));
                        }
                    }
                }

                for (int i = gamerNames.Count - 1; i >= 0; i--)
                {
                    if (!gamerNames[i].Keep)
                    {
                        gamerNames[i].DeleteMe();
                        gamerNames.RemoveAt(i);
                    }
                }
            }
            else
            {
                clearGamerNames();
            }
        }

        public void UnlockPrivateHome()
        {
            if (Progress.Items.Pay(LootfestLib.PrivateHomeCost))
            {
                Settings.UnlockedPrivateHome = true;
                SettingsChanged();
                CloseMenu();
                //lägg till map flash
                openMiniMap(true);
            }
        }

        public override bool IsPausing
        {
            get { return mode != Players.PlayerMode.Play && mode != Players.PlayerMode.Creation; ; }
        }
        override public bool Local { get { return true; } }
    }

    enum TextInputType
    {
        NON,
        NameLocation,
    }
    
}
