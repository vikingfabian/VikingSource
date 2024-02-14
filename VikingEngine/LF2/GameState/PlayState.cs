using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Input;
using VikingEngine.LootFest;
using VikingEngine.HUD;
//

namespace VikingEngine.LF2
{

    class PlayState : Engine.GameState 
    {
        //public static PlayState Singleton; 

        //bool hostSettingsTerrainDamage;
        public static List<Map.Terrain.Area.AbsArea> DebugWarpLocations;
        bool gotGuideUpEvent = false;
        public bool HasRecievedWorldOverview = false;
        public int NumEnemies = 0;
        
        float lastUpdateTime = 0;
        //public static int RunAItest = int.NUM_NON;
        public static bool GamePaused = false;
        const float TileAnimFrame = 80;
        float tileAnimationTime = 0;
        Players.Player[] controllers = new Players.Player[4];
        List<Players.Player> localPlayers = new List<Players.Player>();
        SpottedArray<Players.ClientPlayer> clientPlayers;
        SpottedArrayCounter<Players.ClientPlayer> clientPlayersCounter;


        public Director.GameObjCollection GameObjCollection;

        const int MaxSentCheckSum = 2;
        int sentChunkCheckSum = 0;
        List<IntVector2> requestedChunks = new List<IntVector2>();
        //public static List<GameObjects.Characters.Hero> Heroes;
        public static bool SplitScreen = false;
        public Music.MusicDirector MusicDirector;

        public Players.Player LocalHostingPlayer
        {
            get {
                if (localPlayers.Count == 0) return null;
                return localPlayers[0]; }
        }
        public bool TerrainDestruction
        {
            get 
            {

                return true;
            }
        }
        //Map.World worldMap;
        

        public Data.Progress Progress = new Data.Progress();


        //bool Qkey = false;
        //bool Wkey = false;

        public void RequestedChunksToDoc(File file)
        {
            foreach (IntVector2 c in requestedChunks)
            {
                file.TextBoxBread(c.ToString());
            }
        }

        public PlayState()
        {
            LfRef.gamestate = this;
            Director.LightsAndShadows.Instance = new Director.LightsAndShadows();
            GameObjCollection = new Director.GameObjCollection(this);
#if PCGAME
            //Engine.Input.SimulateController = false;
            Input.Mouse.Visible = false;
#endif
            clientPlayers = new SpottedArray<Players.ClientPlayer>();
            clientPlayersCounter = new SpottedArrayCounter<Players.ClientPlayer>(clientPlayers);

            System.Diagnostics.Debug.WriteLine("--Playstate start--, stream open:" + DataLib.SaveLoad.GetStreamIsOpen.ToString());
            System.Diagnostics.Debug.WriteLine("Map.World.RunningAsHost: " + Map.World.RunningAsHost.ToString());
            System.Diagnostics.Debug.WriteLine("Session is host or offline: " + Ref.netSession.IsHostOrOffline.ToString());
            NumEnemies = 0;

            //Ref.netSession.InviteWaiting = false;

            //Ref.main.TargetElapsedTime = Engine.Update.FrameTime30FPS;
            Engine.Update.SetFrameRate(30);
             
        }

       
        public void LoadGame(Map.World map)
        {
            LfRef.worldMap = map;
            //if (PlatformSettings.DebugWindow)
            //{
            //    Debug.DebugLib.LaunchWindow();
            //}

            bool autoCreateSessions = 
                Map.World.RunningAsHost
//#if !WINDOWS
//                && Engine.XGuide.LocalHost.OnlineSessionsPrivilege
//#endif    
                ;

            Ref.netSession.maxGamers = LootfestLib.MaxGamers;

            Ref.netSession.settings = new Network.Settings(     
                autoCreateSessions?
                    Network.SearchAndCreateSessionsStatus.Create : Network.SearchAndCreateSessionsStatus.NON,
                    0);
            readyToJoinMessage();

            //draw.SetDrawType(Ref.draw.RenderLootfest);
            
           // worldMap = map;
            
            
            draw.ClrColor = new Color(148, 170, 243);
            //updateWatchKeysList();
            LfRef.LocalHeroes = new StaticList<GameObjects.Characters.Hero>(LootfestLib.MaxLocalGamers);//new List<GameObjects.Characters.Hero>();
            LfRef.AllHeroes = new StaticList<GameObjects.Characters.Hero>(LootfestLib.MaxGamers);

            int numPlayers = 0;
            int screenIx = 0;
            for (int setDraw = 0; setDraw < 2; setDraw++)
            {
                for (int ix = int.Player1; ix <= int.Player4; ix++)
                {
                    Engine.PlayerData p = Engine.XGuide.GetPlayer(ix);
                    //hostingPlayer = p;
                    if (p.IsActive)
                    {
                        if (setDraw == 0)
                            numPlayers++;
                        else
                        {
                            p.view.SetDrawArea(numPlayers, screenIx,  new Graphics.CameraSettings(Graphics.CameraSettings.StandardSpeedSetting), true);
                            draw.AddPlayerScreen(p);

                            Players.Player newP = new Players.Player(p, numPlayers, 0,localPlayers.Count == 0, this);
                            localPlayers.Add(newP);
                            LocalHostingPlayer.updateSettings();
                            screenIx++;
                        }

                    }
                }
            }
            
            SplitScreen = numPlayers != 1;
            Effects.EffectLib.Use3dEffects = numPlayers == 1;
            
            foreach (Players.Player player in localPlayers)
            {
                GameObjects.Characters.Hero hero = player.CreateHero();
                hero.Restart(false);
                LfRef.LocalHeroes.Add(hero);
                controllers[(int)player.Index] = player;
                //if (RunAItest != int.NUM_NON)
                //{
                //    if (player.Index != RunAItest)
                //        new Players.AI(hero);
                //}

            }
            LfRef.worldMap.GameStart();
            
            LootfestLib.Images.HideTargetImage();
            LootfestLib.Images.InitAnimation();
            CreateSkydome();
           
           GameObjCollection.Start();


           Progress.Save(false);

           if (Map.World.RunningAsHost)
             Data.WorldsSummaryColl.BeginSave(localPlayers, Progress);
           MusicDirector = new Music.MusicDirector();
          
        }


        float timeSinceReadyMessage = 0;
        private void readyToJoinMessage()
        {
            if (!Map.World.RunningAsHost && Network.Session.Connected)
            {
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_NewPlayerDoneLoadingMap,
                    Network.PacketReliability.Reliable, Ref.netSession.LocalHostIndex);
            }
            timeSinceReadyMessage = 0;
        }

        void createNetDebug()
        {
            Ref.netSession.DebugText = new TextG(LoadedFont.Bold, new Vector2(100, Engine.Screen.Height * 0.7f),
                new Vector2(1), Align.Zero, "--", Color.White, ImageLayers.AbsoluteTopLayer);
        }

        DesignTutorial tutorial = null;
        public void StartTutorial()
        {
            tutorial = new DesignTutorial();
        }
        public bool SaveMap
        {
            get
            {
                return Map.World.RunningAsHost;

            }
        }

        //public override void ErrorMessage(string message)
        //{
        //    if (PlatformSettings.DebugOptions)
        //    {
        //        LocalHostingPlayerPrint(message);
        //        base.ErrorMessage(message);
        //    }
        //}

        public void CreateSkydome()
        {
            //Skydome
            
            draw.CurrentRenderLayer = (int)RenderLayer.Layer2;
            Graphics.Mesh skydome = new Graphics.Mesh(LoadedMesh.skydome, new Vector3(Map.WorldPosition.WorldSizeX * PublicConstants.Half,
                 10, Map.WorldPosition.WorldSizeZ * PublicConstants.Half),
                 new Graphics.TextureEffect(TextureEffectType.Flat, SpriteName.TextureSkydome), 1000);
            draw.CurrentRenderLayer = 0;
            skydome.Y = -120;
            skydome.RotateWorld(new Vector3(0, -MathHelper.PiOver2, 0));
            skydome.AlwaysInCameraCulling = true;
            draw.SkyDome = skydome;
        }

        protected override void createDrawManager()
        {
            draw = new Draw();
        }

//        void updateWatchKeysList()
//        {
//            WatchKeys = new List<Microsoft.Xna.Framework.Input.Keys> { Keys.T, Keys.Enter};//, Keys.F2 };
//#if PCGAME
//            WatchKeys.AddRange(new List<Microsoft.Xna.Framework.Input.Keys> { Keys.Tab, Keys.Q, Keys.A, Keys.W, Keys.S, Keys.D, Keys.F2, Keys.Enter, 
//                Keys.Escape, Keys.Back, Keys.B, Keys.Z, Keys.E, Keys.C, Keys.M, Keys.Y});
//#endif
//            foreach (Players.Player p in localPlayers)
//            {
//                if (p.UsingInputDialogue)
//                {
//                    WatchKeys = KeyboardInputKeys;
//                    break;
//                }
//            }
//            if (PlatformSettings.DebugOptions)
//                WatchKeys.Add(Keys.F2);
//        }

        public void BarrelKillTrophy(Vector3 position)
        {
            const float MinDistance = Map.WorldPosition.ChunkWidth * 4;
            foreach (Players.Player p in localPlayers)
            {
                if ((p.hero.Position - position).Length() < MinDistance)
                {
                    p.UnlockThrophy(Trophies.Kill3InABarrelBlast);
                }
            }
        }

        public void MonsterKilled(GameObjects.Characters.Monster2Type type)
        {
            if (type < GameObjects.Characters.Monster2Type.NUM)
            {

                foreach (Players.Player p in localPlayers)
                {
                    p.Settings.KilledMonsterTypes[(int)type]++;
                    if (!p.Settings.UnlockedThrophy[(int)Trophies.Kill10OfEachEnemyType])
                    {
                        for (int i = 0; i < (int)GameObjects.Characters.Monster2Type.NUM; i++)
                        {
                            if (p.Settings.KilledMonsterTypes[i] < LootfestLib.Trophy_KillOfEachEnemyType)
                            {
                                return;
                            }
                        }
                    }
                    p.UnlockThrophy(Trophies.Kill10OfEachEnemyType);
                }
            }
        }
        public GameObjects.EnvironmentObj.Chest DropGadget(//GameObjects.Gadgets.IGadget gadget, 
            GameObjects.Characters.Hero hero)
        {
            //Check if there is any discard piles close by, otherwise create a new one
            //List<GameObjects.AbsUpdateObj> activeAndClientObjectList =  GameObjCollection.ActiveAndClients;
            //foreach (GameObjects.AbsUpdateObj obj in activeAndClientObjectList)
            ISpottedArrayCounter<GameObjects.AbsUpdateObj> counter = GameObjCollection.AllMembersUpdateCounter;
            while(counter.Next())
            {
                if (counter.GetMember is GameObjects.EnvironmentObj.DiscardPile)
                {
                    if (hero.distanceToObject(counter.GetMember) <= 5)
                    {
                        GameObjects.EnvironmentObj.Chest chest = (GameObjects.EnvironmentObj.Chest)counter.GetMember;
                        new MoveItemsBetweenCollections(hero.Player.Progress.Items, new GadgetLink(null, null, null, hero.Player), 
                            chest.Data.GadgetColl, hero.Player, true);
                        return chest;
                    }
                }
            }
            //found no existing piles
            GameObjects.EnvironmentObj.DiscardPile pile = new GameObjects.EnvironmentObj.DiscardPile(hero.WorldPosition, null);
            //hero.Player.Progress.Items.RemoveItem(gadget);
            return pile;
        }

        void netSharePvp()
        {
            System.IO.BinaryWriter w =  Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_PVPminigame, Network.PacketReliability.Reliable, LocalHostingPlayer.Index);
            Map.WorldPosition.WriteChunkGrindex_Static(LocalHostingPlayer.hero.ScreenPos, w); //LocalHostingPlayer.hero.ScreenPos.WriteChunkGrindex(w);
        }

        //List<GameObjects.Characters.Hero> AllHeroes()
        //{
        //    List<GameObjects.Characters.Hero> heroes = new List<GameObjects.Characters.Hero>(localPlayers.Count + clientPlayers.Count);
        //    LfRef.LocalHeroes.AddRangeToList(heroes);

        //    clientPlayersCounter.Reset();
        //    while (clientPlayersCounter.Next())
        //    {
        //        heroes.Add(clientPlayersCounter.Member.hero);
        //    }
        //    return heroes;
        //}

        public void RefreshHeroesList()
        {
            LfRef.AllHeroes.QuickClear();

            LfRef.LocalHeroes.QuickClear();
            LfRef.AllHeroes.QuickClear();
            foreach (var local in localPlayers)
            {
                LfRef.LocalHeroes.Add(local.hero);
                LfRef.AllHeroes.Add(local.hero);
            }
            clientPlayersCounter.Reset();
            while (clientPlayersCounter.Next())
            {
                LfRef.AllHeroes.Add(clientPlayersCounter.Member.hero);
            }
        }

        public List<Players.AbsPlayer> AllPlayers()
        {
            List<Players.AbsPlayer> result = new List<Players.AbsPlayer>(localPlayers.Count + clientPlayers.Count);
            foreach (Players.Player p in localPlayers)
            {
                result.Add(p);
            }
            clientPlayersCounter.Reset();
            while (clientPlayersCounter.Next())
            {
                result.Add(clientPlayersCounter.Member);
            }
            return result;
        }


        public void ListLocalGamers(HUD.File file, int dialogue, Players.Player me)
        {
            for (int i = 0; i < localPlayers.Count; i++)
            {
                if (localPlayers[i] != me)
                    file.AddIconTextLink(SpriteName.IconSplitScreen, localPlayers[i].Name, new HUD.Link(dialogue, i) );
            }
            if (localPlayers.Count == 1)
                file.AddDescription("Press START on a second controller for split screen");
        }
        public IntVector2 GetLocalGamerPosition(int index)
        {
            if (localPlayers.Count > index)
            {
                return localPlayers[index].hero.ScreenPos;
            }
            return LocalHostingPlayer.hero.ScreenPos;
        }

        List<string> defileList()
        {
            List<string> result = DataLib.SaveLoad.ListStorageFolders("*", true);
            for (int i = 0; i < result.Count; i++)
            {
                result[i] += ":" + DataLib.SaveLoad.FilesInStorageDir(result[i]).Count.ToString();
            }
            result.Add("NumFiles:" + DataLib.SaveLoad.FilesInStorageDir(TextLib.EmptyString).Count.ToString());
            return result;
        }
        //public void SendDeFile(int player)
        //{
        //    string text = "";
        //    List<string> files = defileList();
        //    foreach (string f in files)
        //    {
        //        text += f + " ";
        //    }
        //    text = TextLib.FirstLetters(text, byte.MaxValue - 1);
        //    Engine.XGuide.SendFeedBack(player, text);
        //}

        //public override void TextInputEvent(PlayerIndex playerIndex, string input, int link)
        //{
        //    controllers[(int)playerIndex].TextInputEvent(playerIndex, input, link);
            
        //    if (input[0] == '.')
        //    {
        //        bool acceptedCheat = true;
        //        switch (input.Remove(0, 1))
        //        {
        //            default: acceptedCheat = false; break;
        //            case "debug":
        //                LocalHostingPlayer.Settings.DebugUnlocked = true;
        //                LocalHostingPlayer.SettingsChanged();
        //                break;
        //            case "denet":
        //                createNetDebug();
        //                break;
        //            case "crash13":
        //                //throw new Exception("Crash Testing");
        //                new Debug.CrashTimer();
        //                break;
        //            case "defile":
        //                HUD.File file = new HUD.File();
        //                List<string> files = defileList();
        //                foreach (string f in files)
        //                {
        //                    file.AddTextLink(f);
        //                }
        //                file.AddIconTextLink(SpriteName.LFIconLetter, "Send", (int)Players.Link.SendDefile);
        //                LocalHostingPlayer.OpenMenuFile(file);
        //                break;
        //        }
        //        if (acceptedCheat)
        //        {
        //            LocalHostingPlayer.Print(input + " activated");
        //        }
        //    }
        //    //updateWatchKeysList();
        //}
        //public override void TextInputCancelEvent(PlayerIndex playerIndex)
        //{
        //    controllers[(int)playerIndex].TextInputCancelEvent(playerIndex);
        //    //updateWatchKeysList();
        //}

        const int MaxMessages = 40;
        List<ChatMessageData> messageHistory = new List<ChatMessageData>();
        public void AddChat(ChatMessageData message, bool local)
        {
            if (!local)
                LocalHostingPlayer.PrintChat(message, LoadedSound.Dialogue_Neutral);
            if (messageHistory.Count >= MaxMessages)
            {
                messageHistory.RemoveAt(messageHistory.Count - 1);
            }
            messageHistory.Insert(0, message);
        }

        public void WriteMessageHistory(System.IO.BinaryWriter w)
        {
            const byte Version = 0;
            w.Write(Version);
            w.Write((byte)messageHistory.Count);
            foreach (ChatMessageData m in messageHistory)
            {
                m.WriteStream(w);
            }
        }
        public void ReadMessageHistory(System.IO.BinaryReader r)
        {
            messageHistory.Clear();
            byte version = r.ReadByte();
            int amount = r.ReadByte();
            for (int i = 0; i < amount; ++i)
            {
                messageHistory.Add(new ChatMessageData(r));
            }
        }


        //public void DeadEnemy(GameObjects.Characters.CharacterUtype type)
        //{
        //    foreach (Players.Player p in localPlayers)
        //    {
        //        p.Settings.DeadEnemy(type);
        //    }
        //}
        //public File ChatHistoryFile(HUD.ActionIndexEvent openPage)
        //{
        //    File file = new File((int)Players.MenuPageName.MainMenu);
        //    if (messageHistory.Count <= 0)
        //    {
        //        file.TextBoxBread("Empty");
        //    }
        //    foreach (ChatMessageData m in messageHistory)
        //    {
        //        file.TextBoxChatSender(m.Sender);
        //        file.TextBoxBread(m.Text);
        //    }
        //    file.AddIconTextLink(SpriteName.LFIconGoBack, "Back", new HUD.ActionIndexLink(openPage,  (int)Players.MenuPageName.MainMenu));
        //    return file;
        //}
        
       
        public void SaveProgress(bool exit)
        {   
            saveChunks();
            if (Map.World.RunningAsHost)
            {
                Progress.Save(true);
            }
            SavePlayer(exit);
        }

        void saveChunks()
        {
            LfRef.chunks.OpenChunksCounter.Reset();
            while (LfRef.chunks.OpenChunksCounter.Next())
            {
                if (LfRef.chunks.OpenChunksCounter.sel.UnSavedChanges)
                {
                    LfRef.chunks.OpenChunksCounter.sel.SaveData(true, null, true);
                }
            }

            Data.WorldsSummaryColl.BeginSave(localPlayers, Progress);
        }

        public void SavePlayer(bool exit)
        {
            foreach (Players.Player p in localPlayers)
            {
                if (exit)
                    p.SaveAndExit();
                else
                {
                    p.SaveProgress();
                }
            }
        }
        //public override void BlockMenuOpenPage(int page, int playerIx)
        //{
        //    controllers[(int)playerIx].BlockMenuOpenPage(page, playerIx);
        //}
        //public override void BlockMenuClose(int playerIx)
        //{
        //    controllers[(int)playerIx].BlockMenuClose(playerIx);
        //}
        //public override void BlockMenuLinkEvent(HUD.IMenuLink link, int playerIx, numBUTTON button)
        //{
        //    controllers[(int)playerIx].BlockMenuLinkEvent(link, playerIx, button);
        //}
        //public override void BlockMenuValueOptionEvent(int link, double newValue, int playerIx)
        //{
        //    controllers[(int)playerIx].BlockMenuValueOptionEvent(link, newValue, playerIx);
        //}
        //public override void BlockMenuListOptionEvent(int link, int option, int playerIx)
        //{
        //    controllers[(int)playerIx].BlockMenuListOptionEvent(link, option, playerIx);
        //}
        //public override void BlockMenuCheckboxEvent(int link, bool value, int playerIx)
        //{
        //    controllers[(int)playerIx].BlockMenuCheckboxEvent(link, value, playerIx);
        //}
        //public override bool BlockMenuBoolValue(int playerIx, bool value, bool get, int valueIx)
        //{
        //    return controllers[(int)playerIx].BlockMenuBoolValue(playerIx, value, get, valueIx);
        //}
        //public override float BlockMenuFloatValue(int playerIx, float value, bool get, int valueLink)
        //{
        //    return controllers[(int)playerIx].BlockMenuFloatValue(playerIx, value, get, valueLink);
        //}
        //public override void BeginInputDialogueEvent(KeyboardInputValues keyInputValues)
        //{
        //    controllers[(int)keyInputValues.PlayerIndex].BeginInputDialogueEvent(keyInputValues);
        //    //updateWatchKeysList();
        //}

        
        public void NetAddChunkRequest(IntVector2 index)
        {
            if (!requestedChunks.Contains(index))
            {
                bool hasStoredChunk = false;
                Players.AbsPlayer owner = LfRef.worldOverView.ChunkHasOwner(index);
                if (owner != null)
                {
                    //hasStoredChunk = owner.Local;
                    if (owner.Local)
                    {
                        Map.Terrain.Area.PrivateHome home = LfRef.worldOverView.GetArea(index) as Map.Terrain.Area.PrivateHome;
                        if (home != null)
                        {
                            IntVector2 localPos = home.ToLocalPos(index);
                            if (owner.Settings.PrivateAreaData[localPos.X, localPos.Y] != null)
                                hasStoredChunk = true;
                        }
                    }
                }
                else
                {
                    hasStoredChunk = LfRef.worldOverView.GetChunkData(index).changed;
                }

                if (hasStoredChunk)
                {
                    requestedChunks.Add(index);
                }
            }
        }


        bool updateSendChunk()
        {
            if (requestedChunks.Count > 0) //&& Ref.netSession.IsHost)
            {
                if (sentChunkCheckSum <= MaxSentCheckSum)
                {
                    SendChunk(requestedChunks[0], Network.PacketReliability.Reliable);
                    requestedChunks.RemoveAt(0);
                    sentChunkCheckSum += Ref.netSession.RemoteGamersCount;
                    return true;
                }
                else
                {
                    sentChunkCheckSum--;
                }
            }
            return false;
        }


        
        public void QuitToMenu()
        {
            Ref.netSession.Disconnect(null);
            SaveProgress(true);
            exitGame = true;
        }
       
        //public override void EndStateEvent()
        //{
        //    Ref.netSession.Disconnect();
           
        //    Engine.Storage.Reset(false);
        //}
        public override void OnDestroy()
        {
            base.OnDestroy();

            Ref.netSession.Disconnect(null);

            //Engine.Storage.Reset(false);
        }


        //public override void  Pad_Event(JoyStickValue e)
        //{

        //    if (

        //        tutorial == null && 

        //        controllers[e.ContolIx] != null)
        //    {
        //        controllers[e.ContolIx].Pad_Event(e);
        //    }
        //}
        //public override void PadUp_Event(Stick padIx, int contolIx)
        //{
        //    if (controllers[contolIx] != null)
        //    {
        //        controllers[contolIx].PadUp_Event(padIx, contolIx);
        //    }
        //}
//        public override void KeyPressEvent(Microsoft.Xna.Framework.Input.Keys key, bool keydown)
//        {

//#if PCGAME
//            //if (key == Keys.Enter && keydown && !Engine.XGuide.IsVisible)
//            //    localHostingPlayer.beginSendMessage();
//            //else
//            //{
//            //    if (controllers[0] != null)
//            //    {
//            //        controllers[0].KeyPressEvent(key, keydown);
//            //    }
//            //}
//#endif
//            if (keydown && !Engine.XGuide.IsVisible && (key == Keys.T || key == Keys.Enter) && !LocalHostingPlayer.UsingInputDialogue)
//                LocalHostingPlayer.beginSendMessage();
//            else if (PlatformSettings.DebugOptions && keydown && key == Keys.F2)
//            {
//                Map.WorldPosition wp = LfRef.LocalHeroes[0].WorldPosition;
//                wp.WorldGrindex.X+=4;
//                const int TestBoss = 0; Progress.BossKey(TestBoss, null, true); new GameObjects.EnvironmentObj.BossLock(wp.ChunkGrindex, TestBoss);
                
//            }
//            else
//            {
//                controllers[(int)localPlayers[0].Index].KeyPressEvent(key, keydown);
//            }
//        }
//        public override void MouseScroll_Event(int scroll, Vector2 position)
//        {
//            if (controllers[0] != null)
//            {
//                controllers[0].MouseScroll_Event(scroll, position);
//            }
//        }
//        public override void MouseClick_Event(Vector2 position, MouseButton button, bool keydown)
//        {
//            if (controllers[0] != null)
//            {
//                controllers[0].MouseClick_Event(position, button, keydown);
//            }
//        }
//        public override void MouseMove_Event(Vector2 position, Vector2 deltaPos)
//        {
//            if (controllers[0] != null)
//            {
//                controllers[0].MouseMove_Event(position, deltaPos);
//            }
//        }
//        public override void Button_Event(ButtonValue e)
//        {
//            if (!exitGame)
//            {
//                if (tutorial != null)
//                {
//                    if (tutorial.Button_Event(e))
//                    {
//                        tutorial.DeleteMe();
//                        tutorial = null;
//                    }
//                }
//                else
//                {
//                    //#endif
//                    if (e.KeyDown)
//                    {
//                        switch (e.Button)
//                        {
//                            case numBUTTON.B:
//                                LootfestLib.Images.HideTargetImage();
//                                break;
//                        }
//                    }
//                    if (controllers[e.ContolIx] != null)
//                    {
//                        controllers[e.ContolIx].Button_Event(e);
//                    }
//                    else if (e.KeyDown && e.Button == numBUTTON.Start)
//                    {

//                        //join options
//                        PlayerData p = Engine.XGuide.GetPlayer(e.PlayerIx);
//                        if (!PlatformSettings.UseGamerServices || (p.SignedIn && (p.OnlineSessionsPrivilege || !Network.Session.ConnectedOverLive)))
//                        {

//                            joinPlayer(e.PlayerIx);
//                        }
//                        else
//                        {
//                            if (Ref.netSession.IsFull)
//                            {
//                                LocalHostingPlayerPrint("Network session is full");
//                            }
//                            else
//                                Engine.XGuide.ShowSignIn(true);
//                        }
//                    }
//                }
               
//            }
//        }

//        public override void  GamerSignedInEvent(PlayerData playerController, Microsoft.Xna.Framework.GamerServices.SignedInGamer signedOutGamer)
//        {
////     base.GamerSignedOutEvent(playerController, signedOutGamer);
////}
////        //public override void GamerSignedInEvent(PlayerData gamer)
////        //{

//            if (playerController.OnlineSessionsPrivilege)
//            {
//                joinPlayer(playerController.Index);

//                Ref.netSession.AddLocalGamer(playerController.Index);
//            }
//        }
//        public override void GamerSignedOutEvent(PlayerData playerController, Microsoft.Xna.Framework.GamerServices.SignedInGamer signedOutGamer)
//        {
//        //    base.GamerSignedOutEvent(playerController, signedOutGamer);
//        //}
//        //public override void GamerSignedOutEvent(PlayerData gamer)
//        //{
//            int index = signedOutGamer.PlayerIndex;
//            if (index == LocalHostingPlayer.Index)
//            { //will never happen more or less
//                new MainMenuState(true);
//            }
//            else
//            {
//                for (int i = 1; i < localPlayers.Count; i++)
//                {
//                    if (localPlayers[i].Index == index)
//                    {
//                        localPlayers[i].DeleteMe();
//                        break;
//                    }
//                }
//            }
//        }
        public void RemoveScreenPlayer(Players.Player p)
        {
            controllers[(int)p.Index] = null;
            localPlayers.Remove(p);

            LfRef.LocalHeroes.Clear();
            foreach (Players.Player ap in localPlayers)
            {
                LfRef.LocalHeroes.Add(ap.hero);
            }
            UpdateSplitScreen();
            LfRef.worldOverView.PlayerLeftEvent();
            LF2.Map.World.UpdateChunkLoadRadius();

            //playersChangedEvent();
        }
        void joinPlayer(int playerIx)
        {
            //if (Ref.netSession.IsFull)
            //{
            //    LocalHostingPlayerPrint("Session is full");
            //}
            //else
            //{
                //Ref.netSession.add(playerIx);
                Engine.PlayerData pdata = Engine.XGuide.GetPlayer(playerIx);
                //pdata.view.SetDrawArea(localPlayers.Count + 1, localPlayers.Count, new Graphics.CameraSettings(Graphics.CameraSettings.StandardSpeedSetting), true);
            pdata.view.SetDrawArea(localPlayers.Count + 1, localPlayers.Count, true, null);    
            Players.Player newPlayer = new Players.Player(pdata, localPlayers.Count, localPlayers.Count - 1, localPlayers.Count == 0, this);//null;
                localPlayers.Add(newPlayer);

                UpdateSplitScreen();

                GameObjects.Characters.Hero hero = newPlayer.CreateHero();
                hero.Restart(false);
                //LfRef.LocalHeroes.Add(hero);
                
                controllers[(int)newPlayer.Index] = newPlayer;
                LF2.Map.World.UpdateChunkLoadRadius();

                //playersChangedEvent();
                //Ref.draw.ClearSSAOtarget();
            //}
        }

        public void UpdateSplitScreen()
        {
            draw.ActivePlayerScreens.Clear();

            for (int i = 0; i < localPlayers.Count; i++)
            {
                Engine.PlayerData p = Engine.XGuide.GetPlayer(localPlayers[i].Index);
               
                localPlayers[i].UpdateSplitScreen(p, localPlayers.Count, i);
                draw.AddPlayerScreen(p);
            }
        }


        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            float halfUpdateTime = lastUpdateTime + time;
            lastUpdateTime = time;
            if (!GamePaused)
            {
                GameObjCollection.Update(time);
            }
            
            tileAnimationTime += time;
            if (tileAnimationTime >= TileAnimFrame)
            {
                LootfestLib.Images.UpdateAnimation();
                tileAnimationTime = 0;
            }

            if (Ref.update.LasyUpdatePart == LasyUpdatePart.Part5)
            {
                Data.WorldsSummaryColl.SlowWorldDeleteCheck();

                //if (!Map.World.RunningAsHost && Ref.netSession.InMultiplayerSession && clientPlayers.Count == 0)
                //{ //If the player wont get the startup packets
                //    timeSinceReadyMessage += Ref.update.LazyUpdateTime;
                //    if (timeSinceReadyMessage > TimeExt.SecondsToMS(10))
                //    {
                //        //if (PlatformSettings.Debug > BuildDebugLevel.TesterDebug_2)
                //        //{
                //            Ref.netSession.Disconnect("");
                //        //}
                //        //else
                //        //{
                //        //    readyToJoinMessage();
                //        //}
                //    }
                //}
            }
            else if (Ref.update.LasyUpdatePart == LasyUpdatePart.Part7_GameState)
            {

                updatePause();


                if (!GamePaused)
                {
                    GameObjCollection.LasyUpdate(Ref.update.LazyUpdateTime);

                    if (autoSaveTimer.Update(Ref.update.LazyUpdateTime))
                    {
                        if (LocalHostingPlayer.Settings.AutoSave)
                        {
                            SaveProgress(false);
                            LocalHostingPlayer.Print("Auto save", SpriteName.InterfaceTextInput);
                        }
                    }
                }

                if (Engine.XGuide.InOverlay)
                {
                    if (!gotGuideUpEvent)
                    {
                        foreach (Players.Player p in localPlayers)
                        {
                            p.GuideUpEvent();
                        }

                    }
                    gotGuideUpEvent = true;
                }
                else
                {
                    gotGuideUpEvent = false;
                }
                Music.SoundManager.EnemySoundTimer--;

                if (exitGame)// && Engine.Storage.Empty)
                {
                    new GameState.ExitState();//new MainMenuState();
                }
            }

            //if (PlatformSettings.DevBuild)
            //{
            //    for (Keys k = Keys.D0; k <= Keys.D9; k++)
            //    {
            //        if (Input.Keyboard.KeyDownEvent(k))
            //        {
            //            debugAction((int)(k - Keys.D0));
            //        }
            //    }

            //    if (Input.Controller.Instance(0).IsButtonDown(Buttons.LeftTrigger) &&
            //        Input.Controller.Instance(0).IsButtonDown(Buttons.RightTrigger))
            //    {
            //        debugAction(Buttons.A);
            //        debugAction(Buttons.B);
            //        debugAction(Buttons.X);
            //        debugAction(Buttons.Y);
            //    }
            //}

            foreach (Players.Player p in localPlayers)
            {
                p.Update(time, this);
            }

            if (Ref.update.LasyUpdatePart == LasyUpdatePart.Part4 || Director.LightsAndShadows.Instance.NewLightSource)
            {
                Director.LightsAndShadows.Instance.NewLightSource = false;
                LfRef.worldMap.UpdateHeightMapLights();
            }
        }



        private void updatePause()
        {
            bool wasPaused = GamePaused;

            GamePaused = true;

            clientPlayersCounter.Reset();
            while (clientPlayersCounter.Next())
            {
                GamePaused = GamePaused && clientPlayersCounter.sel.IsPausing;
            }
            //foreach (Players.ClientPlayer p in clientPlayers)
            //{
            //    GamePaused = GamePaused && p.IsPausing;
            //}
            foreach (Players.Player p in localPlayers)
            {
                GamePaused = GamePaused && p.IsPausing;
            }
            
            if (wasPaused != GamePaused)
            {
                foreach (Players.Player p in localPlayers)
                {
                    p.Pause(GamePaused);
                }
            }
        }

        //int testtt = 0;
        //void debugAction(Buttons button)
        //{
        //    if (Input.Controller.Instance(0).KeyDownEvent(button))
        //    {
        //        //testtt++;
        //        //if (lib.EvenValue(testtt))
        //        //    lib.DoNothing();

        //        Map.WorldPosition wp = LfRef.LocalHeroes[0].WorldPosition;
        //        wp.WorldGrindex.X+=8;
        //        Tweaking.DebugAction(button, wp);
        //    }
        //}

        //void debugAction(int number)
        //{
        //    Map.WorldPosition wp = LfRef.LocalHeroes[0].WorldPosition;
        //    wp.WorldGrindex.X += 8;
        //    Tweaking.DebugAction(number, wp);
       
        //}

        public void UpdateMissioni()
        {

            ISpottedArrayCounter<GameObjects.AbsUpdateObj> objects = GameObjCollection.AllMembersUpdateCounter;//activeAndClientObjects();
            //foreach (GameObjects.AbsUpdateObj obj in objects)
            while (objects.Next())
            {
                if (objects.GetSelection.Type == GameObjects.ObjectType.Character)
                {
                    ((GameObjects.Characters.AbsCharacter)objects.GetSelection).UpdateMissioni();
                }
            }
            foreach (Players.Player p in localPlayers)
            {
                //Make the compass '!' pulse once
                const float LifeTime = 800;
                Graphics.Image expressPulse = new Graphics.Image(SpriteName.IconMapQuest, p.compass.CompassQuestMarkPos, Vector2.One * 16, ImageLayers.Lay9, true);
                new Graphics.Motion2d(Graphics.MotionType.SCALE, expressPulse, Vector2.One * 100, Graphics.MotionRepeate.NO_REPEAT, LifeTime, true);
                new Graphics.Motion2d(Graphics.MotionType.OPACITY, expressPulse, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT, LifeTime, true);
                new Timer.Terminator(LifeTime, expressPulse);
            }

        }

        bool exitGame = false;
        Timer.Basic autoSaveTimer = new Timer.Basic(TimeExt.MinutesToMS(1), true);

       
       static readonly GameObjects.Characters.CharacterUtype[] ZombieLevel = new GameObjects.Characters.CharacterUtype[]
        {
            GameObjects.Characters.CharacterUtype.Zombie,
            GameObjects.Characters.CharacterUtype.Skeleton,
            GameObjects.Characters.CharacterUtype.LeaderZombie,
            GameObjects.Characters.CharacterUtype.BabyZombie,
            GameObjects.Characters.CharacterUtype.DogZombie,
            GameObjects.Characters.CharacterUtype.FatZombie,
        };
        int numZombiesWaiting = 0;
        Timer.Basic spawnTimer = new Timer.Basic();

        public const int MaxNumEnemies = 40;
        static readonly Range MinNumZombies = new Range(4, 6);
        static readonly Range SharedNumZombies = new Range(12, 20);


        public void LocalHostingPlayerPrint(string text, SpriteName icon)
        {
            LocalHostingPlayer.Print(text, icon);
        }
        public void LocalHostingPlayerPrint(string text)
        {
            LocalHostingPlayer.Print(text);
        }

        Players.PlayerSettings playerSettings;
        public void ChangedSettings(int player, Players.PlayerSettings settings)
        {
            if (LocalHostingPlayer.Index == player)
            {
                playerSettings = settings;
            }
        }


        public override void NetEvent_PeerJoined(Network.AbsNetworkPeer gamer)
        {
            base.NetEvent_PeerJoined(gamer);

            GamePaused = false;
            if (!gamer.IsLocal)
            {
            //    int pIx = Engine.XGuide.Network.AbsNetworkPeerIndex(gamer);
            //    foreach (Players.Player p in localPlayers)
            //    {
            //        if (p.Index == pIx)
            //            return;
            //    }
            //   // joinPlayer(pIx);
            //}
            //else
            //{
                foreach (Players.Player p in localPlayers)
                {
                    p.NetworkPlayerJoined(gamer);
                }
                if (Ref.netSession.IsHost)
                {
                    Data.RandomSeed.ShareWorldIx();
                    foreach (Players.Player p in localPlayers)
                    {
                        p.Settings.NumNetworkGuests++;
                    }
                }
                Music.SoundManager.PlayFlatSound(LoadedSound.player_enters);
            }
            
            if (!gamer.IsLocal || PlatformSettings.DebugOptions)
            { LocalHostingPlayerPrint(gamer.Gamertag + " joined"); }

           NetworkLib.GamerJoinedEvent(gamer);
            //playersChangedEvent();
        }
//#if PCGAME
//        public override void NetworkPlayerJoined(Network.Client gamer)
//        {
//            //localHostingPlayerPrint(gamer.ToString() + " joined");
//        }
//#endif
        public bool LoadingTerrain
        {
            get
            {
                foreach (Players.Player p in localPlayers)
                {
                    if (p.WarpLoading)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        //public override void NetworkJoined(Network.AbsNetworkPeer me)
        //{
        //    LocalHostingPlayerPrint("You joined a game");
        //}
        // public override void Network.AbsNetworkPeerLost(Network.AbsNetworkPeer gamer)
        public override void NetEvent_PeerLost(Network.AbsNetworkPeer gamer)
        {
            base.NetEvent_PeerLost(gamer);
            LocalHostingPlayerPrint(gamer.Gamertag + " left");

            GameObjCollection.NetworkPlayerLost(gamer);
            clientPlayer = GetClientPlayer(gamer);
            if (clientPlayer != null)
            {
                //LfRef.worldOverView.EnvironmentObjectQue.LostNetwork.AbsNetworkPeer(clientPlayer);
                clientPlayer.DeleteMe();//index är -1, borde köra gamertag
                clientPlayers.Remove(clientPlayer);

                LfRef.worldOverView.PlayerLeftEvent();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Gamer ID" + gamer.Id.ToString() + " could not be removed");
            }

            sentChunkCheckSum = Bound.Min(sentChunkCheckSum - 3, 0);
        }
        //public override void InviteAcceptedEvent()
        //{
        //    QuitToMenu();
        //}

        //public override void NetworkConnectionLost(Network.AbsNetworkPeer host, NetworkSessionEndReason endReason, bool onPurpose)
        public override void NetEvent_ConnectionLost(string reason)
        {
            if (!Ref.netSession.IsHost) 
            {
                new GameState.ExitState().LostConnectionMessage(reason);
            }
        }
        public override void NetworkRunUnrelyableUpdate()
        {
            ISpottedArrayCounter<GameObjects.AbsUpdateObj> active = GameObjCollection.AllMembersUpdateCounter;
            while(active.Next())
            {
                if (active.GetMember.NetworkShareSettings.Update && active.GetMember.HasNetworkClient)
                {
                    active.GetMember.NetworkUpdatePacket(Network.PacketReliability.Unrelyable);
                }
            }
        }

        public override void NetworkStatusMessage(Network.NetworkStatusMessage message)
        {
            string text = null;

            switch (message)
            {
                default:
                    text = TextLib.EnumName(message.ToString());
                    break;
                case Network.NetworkStatusMessage.Need_to_log_in:

                    break;
                case Network.NetworkStatusMessage.Session_ended:
                    if (Map.World.RunningAsHost)
                    {
                        goto default;
                    }
                    else
                    {
                        text = null;
                    }
                    break;
                case Network.NetworkStatusMessage.Created_session:
                    text = "Started hosting network";
                    //update ownership id
                    GameObjCollection.UpdateOwnerId(LocalHostingPlayer.StaticNetworkId);
                    break;
            }

            if (text != null)
                LocalHostingPlayerPrint(text);
        }
        void sendAllHostedObjects(Network.SendPacketToOptions toGamer)
        {
            if (Ref.netSession.IsHost)
            {
                LfRef.worldOverView.NetworkSend(toGamer);
                Progress.NetworkWriteProgress();

                //börja skicka alla chunks i center av spelaren

                NetAddChunkRequest(Map.WorldPosition.CenterChunk);
                const int Radius = Map.World.StanadardOpenRadius;
                for (int X = Map.WorldPosition.CenterChunk.X - Radius; X <= Map.WorldPosition.CenterChunk.X + Radius; X++)
                {
                    for (int Y = Map.WorldPosition.CenterChunk.Y - Radius; Y <= Map.WorldPosition.CenterChunk.Y + Radius; Y++)
                    {
                        if (X != Map.WorldPosition.CenterChunk.X || Y != Map.WorldPosition.CenterChunk.Y)
                            NetAddChunkRequest(new IntVector2(X, Y));
                    }
                }
            }
            //Send all your hosted game objects
            foreach (Players.Player p in localPlayers)
            {
                p.NetworkSharePlayer(toGamer);
            }
            //delay på detta
            new SendHostedGameObjects(toGamer, GameObjCollection);
        }

        List<Map.Terrain.ScreenTopographicData> topographs = new List<Map.Terrain.ScreenTopographicData>();
        List<IntVector2> topgraphStartPos = new List<IntVector2>();
        Players.ClientPlayer clientPlayer = null;
        GameObjects.AbsUpdateObj clientObj;
        Map.Chunk screen;
        protected void readPacket(Network.ReceivedPacket packet, Players.Player toSpecificGamer)
        {
            //Network.NetLib.PacketType = (Network.PacketType)r.ReadByte();

            //if (Network.NetLib.PacketType != Network.PacketType.GameObjUpdate)
            //{
            //    System.Diagnostics.Debug.WriteLine("READ packet type: " + Network.NetLib.PacketType.ToString());
            //}
            System.IO.BinaryReader r = packet.r;
            var sender = packet.sender;

            switch (packet.type)//Network.NetLib.PacketType)
                {
                    default:
                        LocalHostingPlayer.NetworkReadPacket(Network.NetLib.PacketType, sender, r);
                        break;
                    case Network.PacketType.LF2_GameObjUpdate:
                        clientObj = GameObjCollection.GetActiveOrClientObjFromIndex(r);
                        if (clientObj != null)
                        {
                            if (clientObj.NetworkShareSettings.Update)
                                clientObj.NetReadUpdate(r);
                            else
                                Debug.LogError( "Sent update to non updateable object: " + clientObj.ToString());
                        }
                        break;
                    case Network.PacketType.LF2_ToSpecificPlayer:
                        byte to = r.ReadByte();

                        foreach (Players.Player pl in localPlayers)
                        {
                            if (pl.StaticNetworkId == to)
                            {
                                readPacket(packet, pl); 
                                break;
                            }
                        }
                        break;
                    case Network.PacketType.LF2_PlayerVisualMode:
                        clientPlayer = GetClientPlayer(sender);
                        if (clientPlayer != null)
                        {
                            clientPlayer.NetworkReadVisualMode(r);
                        }
                        break;
                    case Network.PacketType.LF2_StartAttack:
                        clientPlayer = GetClientPlayer(sender);
                        if (clientPlayer != null)
                        {
                            clientPlayer.StartAttack(r);
                        }
                        break;
                    case Network.PacketType.LF2_TakeDamage:
                        GameObjects.AbsUpdateObj damageTo = GameObjCollection.GetActiveOrClientObjFromIndex(r);
                        if (damageTo != null)
                        {
                            damageTo.NetworkReadDamage(r);
                        }
                        break;
                  case Network.PacketType.LF2_HostShareDamageVisuals:
                        clientObj = GameObjCollection.GetActiveOrClientObjFromIndex(r);
                        if (clientObj != null)
                            clientObj.NetworkReadVisualDamage(r);
                        break;
                    case Network.PacketType.LF2_AddGameObject:
                        ObjBuilder.Build(r, sender, GetClientPlayer(sender.Id), GameObjCollection);
                        break;
                    case Network.PacketType.LF2_LostClientObj:
                        byte lostClientId = r.ReadByte();
                        if (PlatformSettings.ViewErrorWarnings)
                        {
                            Debug.DebugLib.Print(Debug.PrintCathegoryType.Warning, "Gamer requested lost game obj");
                        }

                        GameObjects.AbsUpdateObj reSendObj = GameObjCollection.GetLocalMember(lostClientId);
                        if (reSendObj != null)
                        {
                            if (reSendObj is GameObjects.PickUp.AbsHeroPickUp)
                            {
                                return;
                            }
                            else if (reSendObj is GameObjects.Characters.Hero)
                            {//send the who plyar instead
                                ((GameObjects.Characters.Hero)reSendObj).Player.NetworkSharePlayer(new Network.SendPacketToOptions(sender.Id));
                            }
                            else
                            {
                                if (PlatformSettings.Debug == BuildDebugLevel.DeveloperDebug_1)
                                    Debug.DebugLib.Print(Debug.PrintCathegoryType.Output, "Resending obj: " + reSendObj.ToString());

                                reSendObj.NetworkShareObject(new Network.SendPacketToOptions(sender.Id));
                            }
                        }
                        else if (PlatformSettings.Debug == BuildDebugLevel.DeveloperDebug_1)
                                Debug.DebugLib.Print(Debug.PrintCathegoryType.Output,"Gameobject not found");


                        break;
                    case Network.PacketType.LF2_CreateEffect:
                        EffectBuilder.Build(r, GameObjCollection);
                        break;
                    case Network.PacketType.LF2_MapCreation:
                        Players.Player.NetworkMapCreation(r);
                        break;
                    case Network.PacketType.LF2_NewPlayer://för kort
               
                        if (r.Position < r.Length)
                        {
                            Players.ClientPlayer sameIdClient = GetClientPlayer(sender);
                            if (sameIdClient != null)
                            {
                                //must remove the item to not get error
                                sameIdClient.DeleteMe();
                                clientPlayers.Remove(sameIdClient);
                            }

                            Players.ClientPlayer newClient = new Players.ClientPlayer(r, sender);
                            clientPlayers.Add(newClient);
                        }
                        else if (PlatformSettings.ViewErrorWarnings)
                        {
                            throw new Exception();
                        }
                        break;
                    //case Network.PacketType.LF2_GivePrivateAreaIx:
                    //    Players.AbsPlayer player = GetPlayerFromId(r.ReadByte());
                    //    if (player != null)
                    //    {
                    //        player.NetReadPrivateHomeLocation(r);
                    //    }
                    //    break;
                    case Network.PacketType.LF2_Chat:
                        string text = TextLib.EmptyString;

                        try
                        {
                            while (r.BaseStream.Position < r.BaseStream.Length)
                            {
                                text += r.ReadString();
                            }
                        }
                        catch (Exception e)
                        {
                            if (PlatformSettings.DebugOptions)
                            {
                                AddChat(new ChatMessageData(e.Message, "Error"), false);
                            }
                        }
                        AddChat(new ChatMessageData(text, sender.Gamertag), false);
                        Music.SoundManager.PlayFlatSound(LoadedSound.PickUp);
                        // localHostingPlayer.PrintChat(text, sender);
                        break;
                    case Network.PacketType.LF2_QuickMessage:
                        AddChat(new ChatMessageData(NetworkLib.QuickMessages[r.ReadByte()], sender.Gamertag), false);
                        Music.SoundManager.PlayFlatSound(LoadedSound.PickUp);
                        break;
                    case Network.PacketType.LF2_Express:
                        clientPlayer = GetClientPlayer(sender);
                        if (clientPlayer != null && clientPlayer.hero != null)
                        {
                            clientPlayer.setVisualMode(Players.VisualMode.Non, false);
                            VoxelModelName express = (VoxelModelName)r.ReadByte();
                            clientPlayer.hero.Express(express, false);

                            const float MessageDist = 20;
                            if (clientPlayer.hero.distanceToObject(LocalHostingPlayer.hero) > MessageDist)
                            { //Add to chat if the player is to far away
                                string say = TextLib.EmptyString;
                                switch (express)
                                {
                                    case VoxelModelName.express_anger:
                                        say = "Im angry!";
                                        break;
                                    case VoxelModelName.express_hi:
                                        say = "Hi!";
                                        break;
                                    case VoxelModelName.express_laugh:
                                        say = "Hahaha!";
                                        break;
                                    case VoxelModelName.express_teasing:
                                        say = "Just teasing!";
                                        break;
                                    case VoxelModelName.express_thumbup:
                                        say = "Thumbs up!";
                                        break;
                                }

                                AddChat(new ChatMessageData("Expression: " + say, sender.Gamertag), false);
                            }
                        }
                        break;
                    case Network.PacketType.LF2_GameObjectState:
                        GameObjects.AbsUpdateObj gameobj = GameObjCollection.GetActiveOrClientObjFromIndex(r);
                        if (gameobj != null)
                        {
                            gameobj.NetworkReadObjectState(r);
                        }
                        break;
                    case Network.PacketType.LF2_NewPlayerDoneLoadingMap:
                        sendAllHostedObjects(new Network.SendPacketToOptions(sender.Id));
                        break;
                    case Network.PacketType.LF2_WorldOverview: //skickas aldrig
                        System.Diagnostics.Debug.WriteLine("##Recieved World overview");
                        LfRef.worldOverView.NetworkReceive(r);
                        HasRecievedWorldOverview = true;
                        break;
                    case Network.PacketType.LF2_RequestChunk:
                        NetAddChunkRequest(Map.WorldPosition.ReadChunkGrindex_Static(r));
                        break;
                    case Network.PacketType.LF2_RequestChunkGroup:
                            IntVector2 chunk3 = Map.WorldPosition.ReadChunkGrindex_Static(r);
                            IntVector2 pos = IntVector2.Zero;
                            for (pos.Y = -1; pos.Y <= 1; pos.Y++)
                            {
                                for (pos.X = -1; pos.X <= 1; pos.X++)
                                {
                                    NetAddChunkRequest(chunk3 + pos);
                                }
                            }
                        break;
                    case Network.PacketType.LF2_SendChunk:

                        {
                            IntVector2 chunk2 = Map.WorldPosition.ReadChunkGrindex_Static(r);
                            //System.Diagnostics.Debug.WriteLine("##Recieve chunk: " + chunk2.ToString());
                            
                            screen = LfRef.chunks.GetScreenUnsafe(chunk2);
                            if (screen != null)
                            {
                                //System.Diagnostics.Debug.WriteLine("-Update chunk");
                                //update open chunk
                                screen.NetworkReciveChunk(r);
                            }
                            else

                            {
                                //put chunk data on storage
                                System.Diagnostics.Debug.WriteLine("-Save chunk");
                                Map.Chunk.NetworkSaveChunk(r, chunk2);
                                LfRef.worldOverView.ChangedChunk(chunk2);
                            }
                            //send back a check
                            if (Ref.netSession.IsClient)
                            {
                                System.IO.BinaryWriter sentCheck = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_GotChunk,
                                    Network.PacketReliability.Reliable, LocalHostingPlayer.Index);
                            }
                        }
                        break;
                    case Network.PacketType.LF2_GotChunk:
                        sentChunkCheckSum--;
                        break;
                //case Network.PacketType.LF2_TextBlocks:
                //    clientPlayer = GetClientPlayer(sender);
                //    if (clientPlayer != null)
                //    {
                //        Editor.VoxelDesigner.NetworkReadTextBlocks(r, clientPlayer);
                //    }
                //    break;
                //case Network.PacketType.LF2_VoxelEditorDrawRect:
                //    clientPlayer = GetClientPlayer(sender);
                //    if (clientPlayer != null)
                //    {
                //        new Editor.ThreadedClientAction(r, sender, clientPlayer, Network.NetLib.PacketType);
                //    }
                //    break;
                //case Network.PacketType.LF2_VoxelEditorDottedLine:
                //    clientPlayer = GetClientPlayer(sender);
                //    if (clientPlayer != null)
                //    {
                //        new Editor.ThreadedClientAction(r, sender, clientPlayer, Network.NetLib.PacketType);
                //    }
                //    break;
                //case Network.PacketType.LF2_VoxelEditorAddTemplate:
                //    clientPlayer = GetClientPlayer(sender);
                //    if (clientPlayer != null)
                //    {
                //        new Editor.ThreadedClientAction(r, sender, clientPlayer, Network.NetLib.PacketType);
                //    }
                //    break;
                //case Network.PacketType.LF2_VoxelEditorUndo:
                //    clientPlayer = GetClientPlayer(sender);
                //    if (clientPlayer != null)
                //    {

                //        if (!clientPlayer.EditorUndo())
                //        {
                //            dont have the undo registred
                //            read position and request the update
                //            IntVector2 pos2 = Map.WorldPosition.ReadChunkGrindex_Static(r);
                //            LfRef.chunks.GetScreen(pos2).RequestChunkDataUpdate();
                //        }
                //    }
                //    break;
                //case Network.PacketType.LF2_VoxelEditorDrawSphere:
                //    Editor.VoxelDesigner.NetworkReadSphere(r);
                //    break;

                case Network.PacketType.LF2_RemoveGameObject:
                        clientObj = GameObjCollection.GetActiveOrClientObjFromIndex(r);
                        if (clientObj != null)
                        {
                            clientObj.NetworkReadDeleteMe(r);//.DeleteMe();
                        }
                        break;

                    
                    case Network.PacketType.LF2_ChangedApperance:
                        
                            //if (r.ReadBoolean())//isHero
                            //{
                                clientPlayer= GetClientPlayer(sender);
                                //Players.ClientPlayer cp5 
                                if (clientPlayer != null)
                                {
                                    clientPlayer.UpdateAppearance(r);
                                }
                            //}
                            //else
                            //{
                            //    GameObjects.AbsUpdateObj toy = GameObjCollection.GetActiveOrClientObjFromIndex(r);
                            //    if (toy != null)
                            //    {
                            //        ((GameObjects.Toys.AbsRC)toy).UpdateImage(r);
                            //    }
                            //}
                        
                        break;
                    case Network.PacketType.LF2_OpenCloseDoor:
                        IntVector2 doorScreen = Map.WorldPosition.ReadChunkGrindex_Static(r);
                        //screen = LfRef.chunks.GetScreenUnsafe(doorScreen);
                        if (LfRef.chunks.ChunksDataLoaded(doorScreen))//screen != null && screen.Openstatus >= Map.ScreenOpenStatus.DotMapDone)
                        {
                            LfRef.chunks.GetScreen(doorScreen).ReadOpenCloseDoor(r); 
                        }
                        else //door is changed in a non open chunk
                        {
                            if (Map.World.RunningAsHost)
                            {
                                //store change in a list
                                LfRef.worldMap.AddEnvObjectChange(new Map.EnvironmentObjectChanged(doorScreen, r, Map.EnvironmentChangedType.DoorOpenClose));
                            }
                            else 
                            { //note as changed, remove save file
                                //screen = LfRef.chunks.GetScreen(doorScreen);
                                //screen.RemoveFile();
                                chunkIsOutOfDate(doorScreen);
                            }
                        }
                           

                        break;
                case Network.PacketType.LF2_RemoveChunkObject:
                        IntVector2 chunkObjScreen = Map.WorldPosition.ReadChunkGrindex_Static(r);
                        screen = LfRef.chunks.GetScreenUnsafe(chunkObjScreen);
                        if (screen != null && screen.DataGridLoadingComplete)
                        {
                            screen.NetReadRemoveChunkObject(r);
                        }
                        else //door is changed in a non open chunk
                        {

                            if (Map.World.RunningAsHost)
                            {
                                LfRef.worldMap.AddEnvObjectChange(new Map.EnvironmentObjectChanged(chunkObjScreen, r, Map.EnvironmentChangedType.ObjectRemoved));
                            }
                            else
                            {
                                //note as changed, remove save file
                                screen = LfRef.chunks.GetScreen(chunkObjScreen);
                                screen.CompleteRemoval();
                                LfRef.chunks.RemoveChunk(chunkObjScreen);
                            }
                        }
                        break;
    //                case Network.PacketType.LF2_createDoor:
    //                    IntVector2 doorChunk = Map.WorldPosition.ReadChunkGrindex_Static(r);
    //                    screen = LfRef.chunks.GetScreenUnsafe(doorChunk);
    //                    if (screen != null)
    //                    {
    //                        screen.AddChunkObject(new GameObjects.EnvironmentObj.Door(r, byte.MaxValue, screen.Index, true), true);
    //                    }
    //                    break;
    //                case Network.PacketType.LF2_ClientStartingEditing:
    //                    clientPlayer = GetClientPlayer(sender);
    //                    if (clientPlayer != null)
    //                    {
    //                        clientPlayer.InBuildMode = true;
    //                        clientPlayer.BuildingPos = Map.WorldPosition.ReadChunkGrindex_Static(r); //.ReadStreamNisse(r);

    //                        if (Ref.netSession.IsHost)
    //                        {

    //                            Map.WorldPosition minWp = Editor.VoxelDesigner.HeroPosToCreationStartPos(clientPlayer.BuildingPos);
    //                            //min//wp.UpdateWorldGridPos();
    //                            Map.WorldPosition maxWp = minWp;
    //                            maxWp.WorldGrindex += Editor.VoxelDesigner.CreationSizeLimit.Max;

    //                            //min//wp.UpdateChunkPos(); max//wp.UpdateChunkPos();

    //                            IntVector2 bpos = IntVector2.Zero;
    //                            //const int Radius = 1;
    //                            for (bpos.Y = minWp.ChunkGrindex.Y; bpos.Y <= maxWp.ChunkGrindex.Y; bpos.Y++)
    //                            {
    //                                for (bpos.X = minWp.ChunkGrindex.X; bpos.X <= maxWp.ChunkGrindex.X; bpos.X++)
    //                                {
    //                                    screen = LfRef.chunks.GetScreenUnsafe(bpos);
    //                                    //if (Map.World.RunningAsHost)
    //                                    //{
    //                                    //    new Map.SafeCopyCheck(bpos);
    //                                    //}

    //                                    if (screen == null || !screen.DataGridLoadingComplete)
    //                                    {
    //                                        if (Map.World.RunningAsHost)
    //                                        {
    //                                            screen = LfRef.chunks.GetScreen(bpos);
    //                                            screen.generate1_Topographic();
    //                                            screen.generate2_HeightMap();
    //                                            screen.generate3_Detail();
    //#if WINDOWS
    //                                            screen.ClientEditingFlag = true;
    //#endif

    //                                        }
    //                                        else
    //                                        {
    //                                            //map is changed and the files should be disqualified
    //                                            screen = LfRef.chunks.GetScreen(bpos);
    //                                            screen.CompleteRemoval();
    //                                        }
    //                                    }

    //                                }
    //                            }
    //                        }
    //                    }
    //                    break;
    //                case Network.PacketType.LF2_ClientEndingEditing:
    //                    Players.ClientPlayer cp6 = GetClientPlayer(sender);
    //                    if (cp6 != null)
    //                    {
    //                        cp6.InBuildMode = false;
    //                    }
    //                    break;
                    //case Network.PacketType.SendMapComplete:
                    //    LocalHostingPlayerPrint("Receving map completed");
                    //    new GameState.SaveChunks(Data.WorldsSummaryColl.CurrentWorld);
                    //    break;
                  
                    //case Network.PacketType.FatzombieBeginAttack:
                    //    Creation.Zombies.FatZombie fatZomb = (Creation.Zombies.FatZombie)GameObjCollection.GetActiveOrClientObjFromIndex(r);
                    //    if (fatZomb != null) //är alltid null
                    //    {
                    //        fatZomb.Fire();
                    //    }
                    //    break;
                    case Network.PacketType.LF2_Explosion:
                        new GameObjects.WeaponAttack.Explosion(r);
                        break;
                    case Network.PacketType.LF2_PlayerDied:
                        LocalHostingPlayerPrint(sender.Gamertag + " died");
                        Progress.HeroDied();
                        break;
                    
                    case Network.PacketType.LF2_ChangeClientPermissions:
                        LocalHostingPlayer.ClientPermissions = (Players.ClientPermissions)r.ReadByte();
                        break;
                    case Network.PacketType.LF2_InviteReady:
                        if (Ref.netSession.IsHost)
                            Data.RandomSeed.ShareWorldIx();
                        break;
                    case Network.PacketType.LF2_RequestGeneratingEnvObj:
                        LfRef.worldOverView.EnvironmentObjectQue.ClientRequest(r, GetClientPlayer(sender));    
                        break;
                    case Network.PacketType.LF2_PermitGeneratingEnvObj:
                        LfRef.worldOverView.EnvironmentObjectQue.HostPertmit(r);
                        break;
                    case Network.PacketType.LF2_ClosingChunk:
                        LfRef.worldOverView.EnvironmentObjectQue.ReadClosingChunk(r, GetClientPlayer(sender)); 
                        break;
                    case Network.PacketType.LF2_RequestPickChestItem:
                        if (r.ReadByte() != GameObjects.EnvironmentObj.ChestData.RequestSecureCode)
                        {
                            if (PlatformSettings.DevBuild)
                                throw new Exception("Chest secure code error");
                            else
                                return;

                        }
                        GameObjects.EnvironmentObj.Chest chest = (GameObjects.EnvironmentObj.Chest)GameObjCollection.GetActiveMember(r);

                        if (chest != null)
                        {
                            
                            chest.Data.ClientRequestingPickItem(r, sender);
                        }
                        break;
                    case Network.PacketType.LF2_PickChestItemPermit:
                        GameObjects.EnvironmentObj.Chest chest2 = GameObjCollection.GetActiveOrClientObjFromIndex(r) as GameObjects.EnvironmentObj.Chest;
//#if RELEASE
                        if (chest2 != null)
//#endif
                        {
                            chest2.Data.HostPickItemPermit(r);
                        }
                        break;
                    case Network.PacketType.LF2_DropItemToChest:
                        GameObjects.EnvironmentObj.Chest chest3 = (GameObjects.EnvironmentObj.Chest)GameObjCollection.GetActiveOrClientObjFromIndex(r);
//#if RELEASE
                        if (chest3 != null)
                        //#endif
                        {
                            chest3.Data.GamerAddedItem(r);
                        }
                        else
                        {
                            Debug.LogError( "PacketType.DropItemToChest chest is null");
                        }
                        break;
                    case Network.PacketType.LF2_BombExplosion:
                        GameObjects.WeaponAttack.ItemThrow.AbsBomb bomb = GameObjCollection.GetActiveOrClientObjFromIndex(r) as GameObjects.WeaponAttack.ItemThrow.AbsBomb;
                        if (bomb != null)
                        {
                            bomb.NetworkReadExplosion(r);
                        }
                        break;
                    case Network.PacketType.LF2_OutdatedChunk:
                        LfRef.worldMap.AddOutDatedChunks(new List<IntVector2> { Map.WorldPosition.ReadChunkGrindex_Static(r) });
                        break;

                    case Network.PacketType.LF2_NewEquipSetup:
                        clientPlayer = GetClientPlayer(sender);
                        if (clientPlayer != null)
                            clientPlayer.NewEquipSetup(r);
                        break;
                    case Network.PacketType.LF2_QuestDialogue:
                        Progress.ReadQuestDialogue(r);
                        break;
                    case Network.PacketType.LF2_GameCompleted:
                        IntVector2 magicianDeathPos = Map.WorldPosition.ReadChunkGrindex_Static(r);
                        bool noDeaths = r.ReadBoolean();
                        GameCompleted(false, magicianDeathPos, noDeaths);
                        break;
                    case Network.PacketType.LF2_GameProgress:
                        Progress.NetworkReadProgress(r);        
                        break;
                    case Network.PacketType.LF2_EggnestDestroyedEvent:
                        GameObjects.Characters.EggNest.NetworkReadNestDestroyed(r);
                        //toSpecificGamer.EggnestDestroyed();
                        break;
                    case Network.PacketType.LF2_BossKey:
                        Progress.BossKey(r.ReadByte(), LfRef.LocalHeroes[0], false);
                        break;
                    case Network.PacketType.LF2_MapFlag:
                        clientPlayer = GetClientPlayer(sender);
                        if (clientPlayer != null)
                        {
                            clientPlayer.Progress.NetworkReadMapFlag(r);
                        }
                        break;

                }
        }

        void chunkIsOutOfDate(IntVector2 pos)
        {
            ForXYLoop loop = new ForXYLoop(IntVector2.NegativeOne, IntVector2.One);
            while (!loop.Done)
            {
                IntVector2 chunkIx = pos + loop.Next_Old();
                screen = LfRef.chunks.GetScreen(chunkIx);
                screen.CompleteRemoval();
                LfRef.chunks.RemoveChunk(chunkIx);
            }
            
            
        }
        public void GameCompleted(bool local, IntVector2 chunk, bool noDeaths)
        {
            Progress.GeneralProgress = Data.GeneralProgress.GameComplete;
            for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
            {
                GameObjects.Characters.Hero h = LfRef.LocalHeroes[i];
                //check distance
                if ((h.ScreenPos - chunk).SideLength() <= 4)
                {
                    h.Player.UnlockThrophy(Trophies.DefeatFinalBoss);
                    MusicDirector.GameCompletedSong();
                }
                //check no death trophy
                if (noDeaths)
                {
                    h.Player.UnlockThrophy(Trophies.CompleteGameWihtOutSingleDeath);
                }
            }

            if (local)
            {
                //net share
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_GameCompleted, Network.PacketReliability.ReliableLasy);
                Map.WorldPosition.WriteChunkGrindex_Static(chunk, w); //chunk.WriteChunkGrindex(w);
                w.Write(noDeaths);
            }
        }

                

        public void RemoveMessage(IMessage message)
        {
            foreach (Players.Player p in localPlayers)
            {
                p.AddMessage(message, false);
            }
        }
        public override void NetworkReadPacket(Network.ReceivedPacket packet)
        {
            readPacket(packet, null);
        }

        //public override bool NetworkLowDataOutBuffer()
        //{
        //    return updateSendChunk();
        //}


        public Players.ClientPlayer GetClientPlayer(Network.AbsNetworkPeer gamer)
        {
            return GetClientPlayer(gamer.Id);
        }
        //public Players.ClientPlayer GetClientPlayer(Network.AbsNetworkPeer sender)
        //{
        //    return GetClientPlayer(sender.Id);
        //}
        Players.AbsPlayer GetPlayerFromId(byte id)
        {
            foreach (Players.Player p in localPlayers)
            {
                if (p.StaticNetworkId == id)
                    return p;
            }
            return GetClientPlayer(id);
        }

        public Players.ClientPlayer GetClientPlayer(byte networkID)
        {
            clientPlayersCounter.Reset();
            while (clientPlayersCounter.Next())
            {
                if (clientPlayersCounter.sel.StaticNetworkId == networkID)
                    return clientPlayersCounter.sel;
            }
            //foreach (Players.ClientPlayer cp in clientPlayers)
            //{
            //    if (cp.StaticNetworkIndex == networkID)
            //        return cp;
            //}
            Debug.LogError("Get sender ID, " + Network.NetLib.PacketType.ToString());
            return null;
            
        }
        public void SendChunk(IntVector2 chunk)
        {
            this.SendChunk(chunk, Network.PacketReliability.ReliableLasy);
        }
        public void SendChunk(IntVector2 chunk, Network.PacketReliability relyable)
        {
            Players.AbsPlayer p = LfRef.worldOverView.ChunkHasOwner(chunk);
            if (p != null && p.Local)
            { //private area
                DataStream.MemoryStreamHandler data = p.Settings.GetPrivateAreaData(chunk);
                if (data != null)
                {
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_SendChunk, Network.PacketReliability.Reliable);
                    Map.WorldPosition.WriteChunkGrindex_Static(chunk, w);
                    data.WriteDataArray(w);
                }
            }
            else
            {
                Map.Chunk screen = LfRef.chunks.GetScreenUnsafe(chunk);
                if (screen != null && screen.DataGridLoadingComplete)
                {
                    System.Diagnostics.Debug.WriteLine("sending open chunk");
                    new Map.Process.NetSendOpenChunk(screen, relyable, false);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("sending stored chunk");
                    new Map.Process.NetOpenAndSendChunk(chunk);
                }
            }
        }
        public Vector3 GetPlayerPosition(byte networkID)
        {
            Players.ClientPlayer cp = GetClientPlayer(networkID);
            if (cp == null)
            {
                return LocalHostingPlayer.HeroPos;
            }
            return cp.HeroPos;
        }

        public bool ClientIsCloseToChunk(IntVector2 pos)
        {
           
            //not thread safe!
            //for (int i = clientPlayers.FirstIndex(); i < clientPlayers.SpottedLength; clientPlayers.NextIndex(ref i))
            //{
            for (int i = 0; i < clientPlayers.SpottedLength; ++i)
            {
                var saObject = clientPlayers[i];
                if (saObject != null)
                {
                    if ((saObject.hero.ScreenPos - pos).SideLength() <= Map.World.OpenScreenRadius_Mesh)
                    {
                        return true;
                    }
                }
            }


            //clientPlayersWorldGenCounter.Reset();
            //while (clientPlayersCounter.Next())
            //{
            //    if ((clientPlayersWorldGenCounter.Member.hero.ScreenPos - pos).SideLength() <= Map.World.StanadardOpenRadius)
            //    {
            //        return true;
            //    }
            //}

            //foreach (Players.ClientPlayer cp in clientPlayers)
            //{
            //    if ((cp.hero.ScreenPos - pos).SideLength() <= Map.World.StanadardOpenRadius)
            //    {
            //        return true;
            //    }
            //}
            return false;
        }
        public bool ClientIsUsingChunk(IntVector2 pos)
        {
            if (Ref.netSession.InMultiplayerSession && Ref.netSession.IsHost)
            {
                //clientPlayersWorldGenCounter.Reset();
                //while (clientPlayersCounter.Next())
                //{
                //for (int i = clientPlayers.FirstIndex(); i < clientPlayers.SpottedLength; clientPlayers.NextIndex(ref i))
                for (int i = 0; i < clientPlayers.SpottedLength; ++i)
                {
                    Players.ClientPlayer cp = clientPlayers[i];
                    if (cp != null)
                    {
                        if (cp.InBuildMode)
                        {
                            if (Math.Abs(pos.X - cp.BuildingPos.X) <= 1 && Math.Abs(pos.Y - cp.BuildingPos.Y) <= 1)
                            {
                                return true;
                            }
                        }
                    }
                }
                //foreach (Players.ClientPlayer cp in clientPlayers)
                //{
                //    if (cp.InBuildMode)
                //    {
                //        if (Math.Abs(pos.X - cp.BuildingPos.X) <= 1 && Math.Abs(pos.Y - cp.BuildingPos.Y) <= 1)
                //        {
                //            return true;
                //        }
                //    }
                //}
            }
            return false;
        }
        //public override Engine.GameStateType Type
        //{
        //    get { return Engine.GameStateType.InGame; }
        //}

        public void ChunkIsMissingFile(IntVector2 chunk)
        {
            LfRef.worldOverView.UnChangedChunk(chunk);
            LfRef.worldMap.AddOutDatedChunks(new List<IntVector2> { chunk });

            //should tell clients
            if (Map.World.RunningAsHost)
            {
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_OutdatedChunk, Network.PacketReliability.Reliable);
                Map.WorldPosition.WriteChunkGrindex_Static(chunk, w);//chunk.ShortVec.WriteStream(w);
            }
        }
        public override bool UseInputEvents
        {
            get
            {
                return true;
            }
        }
    }
    //class ChatMessageData
    //{
    //    public string Text;
    //    public string Sender;
    //    public ChatMessageData(string text, string sender)
    //    {
    //        Text = text;
    //        Sender = sender;
    //    }

    //    public ChatMessageData(System.IO.BinaryReader r)
    //    {
    //        ReadStream(r);
    //    }

    //    public void WriteStream(System.IO.BinaryWriter w)
    //    {
    //        SaveLib.WriteString(w, Text);
    //        SaveLib.WriteString(w, Sender);

    //    }
    //    public void ReadStream(System.IO.BinaryReader r)
    //    {
    //        Text = SaveLib.ReadString(r);
    //        Sender = SaveLib.ReadString(r);
    //    }
    //}
}
