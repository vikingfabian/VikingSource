using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Input;
//xna
using VikingEngine.EngineSpace.Graphics.DeferredRendering;
using VikingEngine.SteamWrapping;

namespace VikingEngine.LootFest
{

    class PlayState : Engine.GameState 
    {
        public int NumEnemies = 0;
        
        float lastUpdateTime = 0;
        
        public List<Players.Player> localPlayers = new List<Players.Player>();
        public SpottedArray<Players.ClientPlayer> clientPlayers;
        public SpottedArrayCounter<Players.ClientPlayer> clientPlayersCounter;

        public Director.GameObjCollection GameObjCollection;
        ChunksDataNetwork chunksDataNetwork = new ChunksDataNetwork();
        

        public Display.SaveFilesMenu saveFilesMenu = null;
        public Display.SelectControllerMenu selectControllerMenu = null;

        public PlayState()
        {
            LfRef.gamestate = this;
            Director.LightsAndShadows.Instance = new Director.LightsAndShadows();
            GameObjCollection = new Director.GameObjCollection();
            new SpawnDirector();
            new Data.VoxModelAutoLoad();

            clientPlayers = new SpottedArray<Players.ClientPlayer>();
            clientPlayersCounter = new SpottedArrayCounter<Players.ClientPlayer>(clientPlayers);

            Debug.Log("--Playstate start--, stream open:" + DataLib.SaveLoad.GetUsingStorage.ToString());
            Debug.Log("Map.World.RunningAsHost: " + LfRef.WorldHost.ToString());
            Debug.Log("Session is host or offline: " + Ref.netSession.IsHostOrOffline.ToString());
           
            new Data.Progress();

            if (PlatformSettings.PC_platform)
            {
                Input.Mouse.Visible = false;
            }
        }
        
        public void LoadGame(Map.World map, Data.WorldData worldData)
        {
            Debug.Log("Load world, seed:" + worldData.seed.ToString() + ", Hosting world:" + worldData.hostingWorld.ToString());
            LfRef.LocalHeroes = new StaticList<GO.PlayerCharacter.AbsHero>(LfLib.MaxLocalGamers);
            LfRef.AllHeroes = new StaticList<GO.PlayerCharacter.AbsHero>(LfLib.MaxGamers);
            LfRef.world = map;
            draw.ClrColor = new Color(148, 170, 243);

            LfRef.WorldHost = worldData.hostingWorld;

            if (worldData.hostingWorld)
            {
                bool autoCreateSessions = LfRef.WorldHost;

                Ref.netSession.maxGamers = LfLib.MaxGamers;

                Ref.netSession.settings = new Network.Settings(
                    autoCreateSessions ?
                        Network.SearchAndCreateSessionsStatus.Create : Network.SearchAndCreateSessionsStatus.NON,
                        0);

                addLocalStartingPlayer(1, XGuide.LocalHostIndex, worldData);
            }
            else
            {
                int numPlayers = 0;

                foreach (var store in LfRef.storage.storages)
                {
                    if (store != null && store.player != null)
                    {
                        numPlayers++;
                    }
                }

                foreach (var store in LfRef.storage.storages)
                {
                    if (store != null && store.player != null)
                    {
                        var player = addLocalStartingPlayer(numPlayers, store.player.PlayerIndex, worldData);
                        store.AssignToPlayer(player);
                    }
                }
            }

            LfRef.world.GameStart();

            LfRef.Images.HideTargetImage();
           
            GameObjCollection.Start();

            if (worldData.hostingWorld)
            {
                Ref.lobby.startSearchLobbies(true);
            }
            else
            {
                LfRef.gamestate.readyToJoinMessage();
            }

#if PCGAME
            if (Ref.steam.leaderboardsInitialized)
            { Ref.steam.leaderBoards.uploadlastplayed(); }
#endif

            if (PlatformSettings.DevBuild && DebugSett.DebugChunkLoading)
            { new DebugChunkLoading(); }
        }

        Players.Player addLocalStartingPlayer(int numPlayers, int playerIx, Data.WorldData worldData)
        {
            Engine.PlayerData p = Engine.XGuide.GetPlayer(playerIx);
            p.IsActive = true;
            int screenIx = draw.AddPlayerScreen(p);

            Players.Player player = new Players.Player(p, numPlayers, 0, localPlayers.Count == 0, this);
            localPlayers.Add(player);
            p.view.SetDrawArea(numPlayers, screenIx, true, player);

            GO.PlayerCharacter.AbsHero hero = player.CreateHero(worldData.isClient);
            //hero.RestartTeleport();

            return player;
        }

        float timeSinceReadyMessage = 0;
        public void readyToJoinMessage()
        {
            if (Ref.netSession.IsClient)
            {
                LocalHostingPlayerPrint("Ready to join message sent");
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.NewPlayerDoneLoadingMap,
                    Network.PacketReliability.Reliable);

                netWriteAllHostedObjects(Network.SendPacketToOptions.SendToAll); 
            }
            timeSinceReadyMessage = 0;
        }

        void createNetDebug()
        {
            Ref.netSession.DebugText = new TextG(LoadedFont.Regular, new Vector2(100, Engine.Screen.Height * 0.7f),
                VectorExt.V2(1), Align.Zero, "--", Color.White, ImageLayers.AbsoluteTopLayer);
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
                return LfRef.WorldHost;

            }
        }

        protected override void createDrawManager()
        {
            if (DebugSett.UseDeferredRenderer)
                draw = new DeferredRenderer();
            else
                draw = new LootfestDraw();
        }

        public void MonsterKilled(GO.Characters.Monster2Type type)
        {
        }
        
        void netSharePvp()
        {
            System.IO.BinaryWriter w =  Ref.netSession.BeginWritingPacket(Network.PacketType.PVPminigame, Network.PacketReliability.Reliable, LocalHostingPlayer.PlayerIndex);
            Map.WorldPosition.WriteChunkGrindex_Static(LocalHostingPlayer.hero.ScreenPos, w); //LocalHostingPlayer.AbsHero.ScreenPos.WriteChunkGrindex(w);
        }

        public void RefreshHeroesList()
        {
            StaticList<GO.PlayerCharacter.AbsHero> local = new StaticList<GO.PlayerCharacter.AbsHero>(localPlayers.Count);
            StaticList<GO.PlayerCharacter.AbsHero> all = new StaticList<GO.PlayerCharacter.AbsHero>(localPlayers.Count + clientPlayers.Count);

            foreach (var l in localPlayers)
            {
                local.Add(l.hero);
                all.Add(l.hero);
            }
            clientPlayersCounter.Reset();
            while (clientPlayersCounter.Next())
            {
                all.Add(clientPlayersCounter.sel.hero);
            }

            LfRef.LocalHeroes = local;
            LfRef.AllHeroes = all;
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
                result.Add(clientPlayersCounter.sel);
            }
            return result;
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
        public void SendDeFile(int player)
        {
            string text = "";
            List<string> files = defileList();
            foreach (string f in files)
            {
                text += f + " ";
            }
            text = TextLib.FirstLetters(text, byte.MaxValue - 1);
        }

        public override void TextInputEvent(int playerIndex, string input, int link)
        {
            localPlayers[playerIndex].TextInputEvent(playerIndex, input, link);
            
            if (input[0] == '.')
            {
                bool acceptedCheat = true;
                switch (input.Remove(0, 1))
                {
                    default: acceptedCheat = false; break;
                    case "denet":
                        createNetDebug();
                        break;
                    case "crash13":
                        new DebugExtensions.CrashTimer();
                        break;
                }
                if (acceptedCheat)
                {
                    LocalHostingPlayer.Print(input + " activated");
                }
            }
        }
        public override void TextInputCancelEvent(int playerIndex)
        {
            localPlayers[playerIndex].TextInputCancelEvent(playerIndex);
        }

        const int MaxMessages = 40;
        List<HUD.ChatMessageData> messageHistory = new List<HUD.ChatMessageData>();
        public void AddChat(HUD.ChatMessageData message, bool local)
        {
            if (!local)
                LocalHostingPlayer.PrintChat(message, local, LoadedSound.Dialogue_Neutral);
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
            foreach (HUD.ChatMessageData m in messageHistory)
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
                messageHistory.Add(new HUD.ChatMessageData(r));
            }
        }
        
        public void SaveProgress(bool exit)
        {
            saveChunks();
            SavePlayer(exit);
            LfRef.world.saveUpdate();
#if PCGAME
            if (Ref.steam.statsInitialized)
            {
                Ref.steam.stats.upload();
            }
#endif
        }

        void saveChunks()
        {
            LfRef.chunks.OpenChunksCounter.Reset();
            while (LfRef.chunks.OpenChunksCounter.Next())
            {
                if (LfRef.chunks.OpenChunksCounter.sel.unsavedChanges)
                {
                    LfRef.chunks.OpenChunksCounter.sel.saveChanges();
                }
            }
        }

        public void SavePlayer(bool exit)
        {
            foreach (Players.Player p in localPlayers)
            {
                if (exit)
                    p.SaveAndExit();
                else
                {
                    p.Save();
                }
            }
        }

        public override void BeginInputDialogueEvent(KeyboardInputValues keyInputValues)
        {
            localPlayers[keyInputValues.PlayerIndex].BeginInputDialogueEvent(keyInputValues);
        }
        
        
        public void QuitToMenu()
        {
            Ref.netSession.Disconnect(null);
            SaveProgress(true);
        }

        public void returningFromEditor()
        {
            
            Players.Player p = localPlayers[0];
            p.CloseMenu();
            p.localPData.view.Camera.LookTarget = p.hero.Position;

            LfRef.world.ReloadAllChunks();
        }
       
        public override void OnDestroy()
        {
            base.OnDestroy();
            TaskExt.ClearStorageQue();//Engine.Storage.Reset(false);
        }

        public void RemoveScreenPlayer(Players.Player p)
        {
            localPlayers.Remove(p);

            LfRef.LocalHeroes.Clear();
            foreach (Players.Player ap in localPlayers)
            {
                LfRef.LocalHeroes.Add(ap.hero);
            }
            UpdateSplitScreen();
            LfRef.world.RefreshChunkLoadRadius();
        }

        public bool lockedInMenu
        {
            get
            {
                return selectControllerMenu != null || saveFilesMenu != null;
            }
        }

        public void addLocalPlayer()
        {
            if (!lockedInMenu)
            {
                for (int nextAvailablePlayerIx = 0; nextAvailablePlayerIx <= Engine.Draw.MaxScreenSplit; nextAvailablePlayerIx++)
                {
                    if (!Engine.XGuide.GetPlayer(nextAvailablePlayerIx).IsActive)
                    {
                        Debug.Log("Join player: " + nextAvailablePlayerIx.ToString());
                        joinPlayer(nextAvailablePlayerIx);

                        return;
                    }
                }
            }
        }

        void joinPlayer(int playerIx)
        {
            //First check if player is already joined
            foreach (var p in localPlayers)
            {
                if (p.PlayerIndex == playerIx)
                {
                    //Already joined
                    p.FreeStorageLock();
                    p.selectSaveFile();
                    return;
                }
            }

           
            Engine.PlayerData pdata = Engine.XGuide.GetPlayer(playerIx);
            pdata.view.SetDrawArea(localPlayers.Count + 1, localPlayers.Count, true, null);
            Players.Player newPlayer = new Players.Player(pdata, localPlayers.Count, localPlayers.Count - 1, localPlayers.Count == 0, this);//null;
            localPlayers.Add(newPlayer);

            UpdateSplitScreen();

            GO.PlayerCharacter.AbsHero hero = newPlayer.CreateHero(false);
            hero.RestartTeleport();
                
            LfRef.world.RefreshChunkLoadRadius();
            
        }

        public override void OnResolutionChange()
        {
            base.OnResolutionChange();
            UpdateSplitScreen();
        }

        public void UpdateSplitScreen()
        {
            draw.ActivePlayerScreens.Clear();

            for (int i = 0; i < localPlayers.Count; i++)
            {
                Engine.PlayerData p = Engine.XGuide.GetPlayer(localPlayers[i].PlayerIndex);
               
                localPlayers[i].UpdateSplitScreen(p, localPlayers.Count, i);
                draw.AddPlayerScreen(p);
            }
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            float halfUpdateTime = lastUpdateTime + time;
            lastUpdateTime = time;
            if (!Ref.isPaused)
            {
                GameObjCollection.Update(time);
                
            }

            if (Ref.update.LasyUpdatePart == LasyUpdatePart.Part7_GameState)
            {
                updatePause();

                if (!Ref.isPaused)
                {
                    GameObjCollection.LasyUpdate(Ref.update.LazyUpdateTime);

                    if (autoSaveTimer.Update(Ref.update.LazyUpdateTime))
                    {
                       SaveProgress(false);
                    }
                }

                Music.SoundManager.weaponClinkTimer.CountDown(Ref.update.LazyUpdateTime);
            }

            if (PlatformSettings.DevBuild)
            {
                if (Input.Keyboard.Alt)
                {
                    for (Keys k = Keys.D0; k <= Keys.D9; k++)
                    {
                        if (Input.Keyboard.KeyDownEvent(k))
                        {
                            debugAction((int)(k - Keys.D0));
                        }
                    }
                }

                if (Input.XInput.Instance(0).IsButtonDown(Buttons.LeftTrigger) &&
                    Input.XInput.Instance(0).IsButtonDown(Buttons.RightTrigger))
                {
                    debugAction(Buttons.A);
                    debugAction(Buttons.B);
                    debugAction(Buttons.X);
                    debugAction(Buttons.Y);
                }

                if (Input.Keyboard.KeyDownEvent(Keys.D9))
                {
                    LfRef.chunks.checkOpenChunksPointers_debug();
                }
            }

            if (selectControllerMenu != null)
            {
                LocalHostingPlayer.updateCameraTargetChasing();
                LocalHostingPlayer.localPData.view.Camera.Time_Update(Ref.DeltaTimeMs);
                if (selectControllerMenu.Update())
                {
                    selectControllerMenu = null;
                }
            }
            else if (saveFilesMenu != null)
            {
                LocalHostingPlayer.updateCameraTargetChasing();
                LocalHostingPlayer.localPData.view.Camera.Time_Update(Ref.DeltaTimeMs);
                if (saveFilesMenu.Update())
                {
                    saveFilesMenu = null;
                }
            }
            else
            {
                //if (Engine.XGuide.OverridePlayerInput == false)
                {
                    for (int i = 0; i < localPlayers.Count; ++i)
                    {
                        localPlayers[i].Update(time, this);
                    }
                }
            }

            if (Ref.update.LasyUpdatePart == LasyUpdatePart.Part4 || Director.LightsAndShadows.Instance.NewLightSource)
            {
                Director.LightsAndShadows.Instance.NewLightSource = false;
                LfRef.world.UpdateHeightMapLights();
            }

            if (PlatformSettings.PlayMusic)
            { Ref.music.Update(); }

            //LfRef.worldMap.mesh.update();
            LfRef.world.GenerateGameObjectsUpdate();

            LfRef.net.update();
        }


        private void updatePause()
        {
            bool isPaused = true;
            bool inLoading = true;

            clientPlayersCounter.Reset();
            while (clientPlayersCounter.Next())
            {
                isPaused = isPaused && clientPlayersCounter.sel.IsPausing;
            }

            foreach (Players.Player p in localPlayers)
            {
                isPaused = isPaused && p.IsPausing;
                inLoading = inLoading && p.InLoadingScene;
            }

            //if (PlatformSettings.SteamAPI)
            {
                if (Engine.XGuide.InOverlay)//.steam.inOverlay)
                { isPaused = true; }
            }
            //else
            //{
            //    if (Engine.XGuide.IsVisible)
            //    { isPaused = true; }
            //}

            if (Ref.isPaused != isPaused)
            {
                Ref.isPaused = isPaused;
                foreach (Players.Player p in localPlayers)
                {
                    p.Pause(isPaused);
                }
            }
            Ref.inLoading = inLoading;
        }

        //int testtt = 0;
        void debugAction(Buttons button)
        {
            if (Input.XInput.Instance(0).KeyDownEvent(button))
            {
                Map.WorldPosition wp = LfRef.LocalHeroes[0].WorldPos;
                wp.WorldGrindex.X+=8;
                DebugAction.ButtonAction(button, wp);
            }
        }

        void debugAction(int number)
        {
            Map.WorldPosition wp = LfRef.LocalHeroes[0].WorldPos;
            wp.WorldGrindex.X += 8;
            DebugAction.KeyAction(number, wp);
       
        }

        Timer.Basic autoSaveTimer = new Timer.Basic(TimeExt.MinutesToMS(1), true);

        public void LocalHostingPlayerPrint(string text, SpriteName icon)
        {
            LocalHostingPlayer.Print(text, icon);
        }
        public void LocalHostingPlayerPrint(string text)
        {
            if (LocalHostingPlayer != null)
                LocalHostingPlayer.Print(text);
        }

        public override void NetEvent_PeerJoined(Network.AbsNetworkPeer gamer)
        {
            Ref.isPaused = false;
            
            if (!gamer.IsLocal)
            {
                Music.SoundManager.PlayFlatSound(LoadedSound.player_enters);
#if PCGAME
                LfRef.stats.multiplayerSessions.value++;
#endif
            }
            
            if (!gamer.IsLocal || PlatformSettings.DebugOptions)
            { LocalHostingPlayerPrint(Engine.LoadContent.CheckCharsSafety(gamer.Gamertag, VikingEngine.HUD.MessageHandler.StandardFont) + 
                " joined"); }
        }

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
        
        public override void NetEvent_PeerLost(Network.AbsNetworkPeer gamer)
        {
            LocalHostingPlayerPrint(Engine.LoadContent.CheckCharsSafety(gamer.Gamertag, VikingEngine.HUD.MessageHandler.StandardFont) + " left");

            GameObjCollection.NetworkPlayerLost(gamer);
            clientPlayer = GetClientPlayer(gamer);
            if (clientPlayer != null)
            {
                LfRef.levels2.chunkHostDirector.LostPeer(clientPlayer);
                addClientPlayer(clientPlayer, false);
            }
            else
            {
                Debug.Log("Gamer ID" + gamer.Id.ToString() + " could not be removed");
            }

            chunksDataNetwork.NetEvent_PeerLost();
        }
        
        void addClientPlayer(Players.ClientPlayer player, bool add)
        {
            if (add)
            {
                clientPlayers.Add(player);
            }
            else
            {
                player.DeleteMe();
                clientPlayers.Remove(player);
            }

            int index = 0;
            clientPlayersCounter.Reset();
            while (clientPlayersCounter.Next())
            {
                clientPlayersCounter.sel.statusDisplay.refreshPosition(index);
                index++;
            }

            LfRef.gamestate.RefreshHeroesList();
        }
        
        public override void NetUpdate()
        {
            chunksDataNetwork.updateSendChunk();

            ISpottedArrayCounter<GO.AbsUpdateObj> active = GameObjCollection.AllMembersUpdateCounter;
            while(active.Next())
            {
                if (active.GetSelection.NetworkShareSettings.Update && active.GetSelection.HasNetworkClient)
                {
                    active.GetSelection.NetworkUpdatePacket(Network.PacketReliability.Unrelyable);
                }
            }
        }

        public override void NetworkStatusMessage(Network.NetworkStatusMessage message)
        {
            string text = null;

            switch (message)
            {
                //default:
                //    //text = TextLib.EnumName(message.ToString());
                //    break;
                //case Network.NetworkStatusMessage.Need_to_sign_in:

                //    break;
                //case Network.NetworkStatusMessage.Session_ended:
                //    if (Map.World.RunningAsHost)
                //    {
                //        goto default;
                //    }
                //    else
                //    {
                //        text = null;
                //    }
                //    break;
                case Network.NetworkStatusMessage.Created_session:
                    text = "Started hosting network";
                    //update ownership id
                    GameObjCollection.UpdateOwnerId(LocalHostingPlayer.pData.netId());
                    break;
            }

            if (text != null)
                LocalHostingPlayerPrint(text);

            //LfRef.net.NetworkStatusMessage(message);
        }
        void netWriteAllHostedObjects(Network.SendPacketToOptions toGamer)
        {
            if (Ref.netSession.IsHost)
            {


                //börja skicka alla chunks i center av spelaren
                //NetAddChunkRequest(Map.WorldPosition.CenterChunk);
                //const int Radius = Map.World.StandardOpenRadius;
                //for (int X = Map.WorldPosition.CenterChunk.X - Radius; X <= Map.WorldPosition.CenterChunk.X + Radius; X++)
                //{
                //    for (int Y = Map.WorldPosition.CenterChunk.Y - Radius; Y <= Map.WorldPosition.CenterChunk.Y + Radius; Y++)
                //    {
                //        if (X != Map.WorldPosition.CenterChunk.X || Y != Map.WorldPosition.CenterChunk.Y)
                //            NetAddChunkRequest(new IntVector2(X, Y));
                //    }
                //}
            }
            //Send all your hosted game objects
            foreach (Players.Player p in localPlayers)
            {
                p.NetworkSharePlayer(toGamer);
            }
            //delay på detta
            new SendHostedGameObjects(toGamer, GameObjCollection);

            LfRef.levels2.netWriteStatus();

            
        }

        Players.ClientPlayer clientPlayer = null;
        GO.AbsUpdateObj clientObj;
        Map.Chunk screen;
        protected void readPacket(Network.ReceivedPacket packet, Players.Player toSpecificGamer)
        {
            if (Network.NetLib.PacketType != Network.PacketType.GameObjUpdate)
            {
                System.Diagnostics.Debug.WriteLine("READ packet type: " + Network.NetLib.PacketType.ToString());
            }
            System.IO.BinaryReader r = packet.r;
            var sender = packet.sender;

            switch (packet.type)
            {
                default:
                    LocalHostingPlayer.NetworkReadPacket(Network.NetLib.PacketType, sender, r);
                    break;
                
                case Network.PacketType.GameObjUpdate:
                    clientObj = GameObjCollection.GetFromId(r);
                    if (clientObj != null)
                    {
                        if (!clientObj.NetworkLocalMember && clientObj.NetworkShareSettings.Update)
                            clientObj.NetReadUpdate(r);
                        else
                            Debug.LogError("Sent update to non updateable object: " + clientObj.ToString());
                    }
                    break;
                
                case Network.PacketType.ToSpecificPlayer:
                    byte to = r.ReadByte();

                    foreach (Players.Player pl in localPlayers)
                    {
                        if (pl.pData.netId() == to)
                        {
                            readPacket(packet, pl);
                            break;
                        }
                    }
                    break;

#if PCGAME
                case Network.PacketType.VoiceChat:
                    Ref.steam.VOIP.readVoice(packet);
                    break;
#endif
                case Network.PacketType.RequestMapSeed:
                    if (Ref.netSession.IsHost)
                    {
                        LfRef.levels2.WorldSeed.ShareWorldSeed();
                    }
                    break;

                case Network.PacketType.WorldSeed:
                    //Recieve start info about the world to join it
                    if (LfRef.WorldHost)
                    {
                        Debug.Log("recieve world seed");
                        VikingEngine.LootFest.Data.WorldData worldData = new Data.WorldData(false);
                        worldData.seed = packet.r.ReadInt32();
                        new GameState.LoadingMap(worldData);

                    }
                    break;
                case Network.PacketType.DesignAreaStorageHeader:
                    if (LocalHostingPlayer.hero.Level != null)
                    {
                        LocalHostingPlayer.hero.Level.designAreas.netRead(packet);
                        LfRef.world.areaStorageHeaderNeedsUpdate = true;
                    }
                    break;
                case Network.PacketType.SuitMainAttack:
                    clientPlayer = GetClientPlayer(sender);
                    if (clientPlayer != null)
                    {
                        clientPlayer.gear.suit.netReadMainAttack(packet);
                    }
                    break;

                case Network.PacketType.SuitSpecialAttack:
                    clientPlayer = GetClientPlayer(sender);
                    if (clientPlayer != null)
                    {
                        VikingEngine.LootFest.GO.SuitType type = (GO.SuitType)r.ReadByte();
                        if (clientPlayer.gear.suit.Type == type)
                        {
                            clientPlayer.gear.suit.netReadSpecAttack(packet);
                        }
                    }
                    break;

                case Network.PacketType.CardCaptureEffect:
                    clientPlayer = GetClientPlayer(sender);
                    if (clientPlayer != null && clientPlayer.statusDisplay != null)
                    {
                        new Effects.CardCaptureHUDEffect(clientPlayer, r);
                    }
                    break;

                case Network.PacketType.RequestAreaUnlock:
                    if (Ref.netSession.IsHost)
                    {
                        var lvl = LfRef.levels2.readLevel(r);
                        if (lvl != null)
                        {
                            byte area = r.ReadByte();
                            lvl.unlockedAreas.Add(area);
                            VikingEngine.LootFest.GO.EnvironmentObj.AreaLock.RefreshAllLevelLocks(lvl);
                        }
                    }
                    break;

                case Network.PacketType.RequestLevelCollectAdd:
                    if (Ref.netSession.IsHost)
                    {
                        var lvl = LfRef.levels2.readLevel(r);
                        if (lvl != null)
                        {
                            int add = r.ReadInt32();
                            lvl.collectAdd(add);
                            lvl.netWriteLevelState();
                            
                        }
                    }
                    break;

                case Network.PacketType.DestroyLevel:
                    LfRef.levels2.destroyLevel(r.ReadByte(), false);
                    break;

                case Network.PacketType.AddGameObject:
                    Director.GameObjectSpawn.ReadSpawn(packet);
                    break;

                case Network.PacketType.StunForce:
                    {
                        var gameobj = GameObjCollection.GetFromId(r) as VikingEngine.LootFest.GO.Characters.AbsMonster3;
                        if (gameobj != null)
                        {
                            gameobj.netReadStunForce(r);
                        }
                    }
                    break;

                case Network.PacketType.LevelStatus:
                    if (Ref.netSession.IsClient)
                    {
                        var lvl = LfRef.levels2.readLevel(r);
                        if (lvl != null)
                        {
                            lvl.netReadLevelState(r);
                        }
                    }
                    break;

                case Network.PacketType.FoundHeroEffect:
                    {
                        var gameobj = GameObjCollection.GetFromId(r) as VikingEngine.LootFest.GO.Characters.AbsMonster3;
                        if (gameobj != null)
                        {
                            gameobj.onFoundHero();
                        }
                    }
                    break;
                case Network.PacketType.MapCreation:
                    Players.Player.NetworkMapCreation(r);
                    break;
                case Network.PacketType.SharePlayer:
                    if (r.BaseStream.Position < r.BaseStream.Length)
                    {
                        if (GetClientPlayer(sender) == null)
                        {
                            Players.ClientPlayer newClient = new Players.ClientPlayer(r, sender);
                            addClientPlayer(newClient, true);
                        }
                    }
                    else if (PlatformSettings.ViewErrorWarnings)
                    {
                        throw new Exception();
                    }
                    break;

                

                case Network.PacketType.EnteredLevel:
                    {
                        var hero = GameObjCollection.GetFromId(r) as VikingEngine.LootFest.GO.PlayerCharacter.AbsHero;
                        var level = (BlockMap.LevelEnum)r.ReadByte();

                        if (hero != null && hero.clientPlayer != null)
                        {
                            hero.clientPlayer.inLevel = level;
                        }

                        if (level == BlockMap.LevelEnum.Creative && 
                            LfRef.WorldHost && 
                            LocalHostingPlayer.hero.Level != null)
                        {
                            LocalHostingPlayer.hero.Level.designAreas.delayedNetWrite();//netWrite();
                        }
                    }
                    break;

                //case Network.PacketType.GivePrivateAreaIx:
                //    Players.AbsPlayer player = GetPlayerFromId(r.ReadByte());
                //    if (player != null)
                //    {
                //        player.NetReadPrivateHomeLocation(r);
                //    }
                //    break;

                case Network.PacketType.Chat:
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
                            AddChat(new HUD.ChatMessageData(e.Message, "Error"), false);
                        }
                    }

                    AddChat(new HUD.ChatMessageData(text, sender.Gamertag), false);

                    Music.SoundManager.PlayFlatSound(LoadedSound.PickUp);
                    break;

                case Network.PacketType.ShieldHit:
                    {
                        var gameobj = GameObjCollection.GetFromId(r) as VikingEngine.LootFest.GO.Characters.AbsHumanoidEnemy;
                        if (gameobj != null)
                        {
                            gameobj.netReadShieldHit(r);
                        }
                    }
                    break;
                case Network.PacketType.BossDefeatedAnimation:
                    new VikingEngine.LootFest.Effects.BossDefeatedAnimation(packet);
                    break;
                case Network.PacketType.Express:
                    clientPlayer = GetClientPlayer(sender);
                    if (clientPlayer != null && clientPlayer.hero != null)
                    {
                        clientPlayer.setVisualMode(Players.VisualMode.Non, false);
                        VoxelModelName express = (VoxelModelName)r.ReadUInt16();
                        clientPlayer.hero.Express(express);
                    }
                    break;
                case Network.PacketType.GameObjectState:
                    {
                        GO.AbsUpdateObj gameobj = GameObjCollection.GetFromId(r);
                        if (gameobj != null)
                        {
                            gameobj.NetworkReadObjectState(r);
                        }
                    }
                    break;
                case Network.PacketType.NewPlayerDoneLoadingMap:
                    netWriteAllHostedObjects(new Network.SendPacketToOptions(sender.FullId));
                    break;
                //case Network.PacketType.WorldOverview: //skickas aldrig
                //    Debug.Log("##Recieved World overview");
                //    //LfRef.worldOverView.NetworkReceive(r);
                //    HasRecievedWorldOverview = true;
                //    break;

                case Network.PacketType.RequestChunk:
                case Network.PacketType.RequestChunkGroup:
                case Network.PacketType.GotChunk:
                case Network.PacketType.SendChunk:
                    chunksDataNetwork.netRead(packet);
                    break;

                case Network.PacketType.VoxelEdit:
                    //Voxels.EditorDrawTools.NetReadVoxelEdit(packet);
                    new Editor.EditorPacket(packet);
                    break;
                //case Network.PacketType.TextBlocks:
                //    clientPlayer = GetClientPlayer(sender);
                //    if (clientPlayer != null)
                //    {
                //        Editor.VoxelDesigner.NetworkReadTextBlocks(r, clientPlayer);
                //    }
                //    break;
                //case Network.PacketType.VoxelEditorDrawRect:
                //    clientPlayer = GetClientPlayer(sender);
                //    if (clientPlayer != null)
                //    {
                //        new Editor.ThreadedClientAction(r, sender, clientPlayer, Network.NetLib.PacketType);
                //    }
                //    break;
                //case Network.PacketType.VoxelEditorDottedLine:
                //    clientPlayer = GetClientPlayer(sender);
                //    if (clientPlayer != null)
                //    {
                //        new Editor.ThreadedClientAction(r, sender, clientPlayer, Network.NetLib.PacketType);
                //    }
                //    break;
                //case Network.PacketType.VoxelEditorAddTemplate:
                //    clientPlayer = GetClientPlayer(sender);
                //    if (clientPlayer != null)
                //    {
                //        new Editor.ThreadedClientAction(r, sender, clientPlayer, Network.NetLib.PacketType);
                //    }
                //    break;
                //case Network.PacketType.VoxelEditorUndo:
                //    //clientPlayer = GetClientPlayer(sender);
                //    //if (clientPlayer != null)
                //    //{

                //    //    if (!clientPlayer.EditorUndo())
                //    //    {
                //    //        //dont have the undo registred
                //    //        //read position and request the update
                //    //        IntVector2 pos2 = Map.WorldPosition.ReadChunkGrindex_Static(r);
                //    //        LfRef.chunks.GetScreen(pos2).RequestChunkDataUpdate();
                //    //    }
                //    //}
                //    break;
                //case Network.PacketType.VoxelEditorDrawSphere:
                //    Editor.VoxelDesigner.NetworkReadSphere(r);
                //    break;

                case Network.PacketType.GameObjDamageAndRemoval:
                    clientObj = GameObjCollection.GetFromId(r);
                    if (clientObj != null)
                    {
                        clientObj.netReadDamageAndRemoval(r);//.NetworkReadDeleteMe(r);//.DeleteMe();
                    }
                    break;


                case Network.PacketType.ChangedApperance:

                    //if (r.ReadBoolean())//isHero
                    //{
                    clientPlayer = GetClientPlayer(sender);
                    //Players.ClientPlayer cp5 
                    if (clientPlayer != null)
                    {
                        clientPlayer.UpdateAppearance(r);
                    }
                    //}
                    //else
                    //{
                    //    GO.AbsUpdateObj toy = GameObjCollection.GetActiveOrClientObjFromIndex(r);
                    //    if (toy != null)
                    //    {
                    //        ((GO.Toys.AbsRC)toy).UpdateImage(r);
                    //    }
                    //}

                    break;
                case Network.PacketType.OpenCloseDoor:
                    IntVector2 doorScreen = Map.WorldPosition.ReadChunkGrindex_Static(r);
                    //screen = LfRef.chunks.GetScreenUnsafe(doorScreen);
                    if (LfRef.chunks.ChunksDataLoaded(doorScreen))//screen != null && screen.Openstatus >= Map.ScreenOpenStatus.DotMapDone)
                    {
                        //LfRef.chunks.GetScreen(doorScreen).ReadOpenCloseDoor(r); 
                    }
                    else //door is changed in a non open chunk
                    {
                        if (LfRef.WorldHost)
                        {
                            //store change in a list
                            LfRef.world.AddEnvObjectChange(new Map.EnvironmentObjectChanged(doorScreen, r, Map.EnvironmentChangedType.DoorOpenClose));
                        }
                        else
                        { //note as changed, remove save file
                            //screen = LfRef.chunks.GetScreen(doorScreen);
                            //screen.RemoveFile();
                            chunkIsOutOfDate(doorScreen);
                        }
                    }


                    break;
                //case Network.PacketType.RemoveChunkObject:
                //        IntVector2 chunkObjScreen = Map.WorldPosition.ReadChunkGrindex_Static(r);
                //        screen = LfRef.chunks.GetScreenUnsafe(chunkObjScreen);
                //        if (screen != null && screen.DataGridLoadingComplete)
                //        {
                //            screen.NetReadRemoveChunkObject(r);
                //        }
                //        else //door is changed in a non open chunk
                //        {

                //            if (Map.World.RunningAsHost)
                //            {
                //                LfRef.worldMap.AddEnvObjectChange(new Map.EnvironmentObjectChanged(chunkObjScreen, r, Map.EnvironmentChangedType.ObjectRemoved));
                //            }
                //            else
                //            {
                //                //note as changed, remove save file
                //                screen = LfRef.chunks.GetScreen(chunkObjScreen);
                //                screen.CompleteRemoval();
                //                LfRef.chunks.RemoveChunk(chunkObjScreen);
                //            }
                //        }
                //        break;
                case Network.PacketType.createDoor:
                    IntVector2 doorChunk = Map.WorldPosition.ReadChunkGrindex_Static(r);
                    screen = LfRef.chunks.GetScreenUnsafe(doorChunk);
                    if (screen != null)
                    {
                        //screen.AddChunkObject(new GO.EnvironmentObj.Door(r, byte.MaxValue, screen.Index, true), true);
                    }
                    break;
                case Network.PacketType.ClientStartingEditing:
                    clientPlayer = GetClientPlayer(sender);
                    if (clientPlayer != null)
                    {
                        clientPlayer.InBuildMode = true;
                        clientPlayer.BuildingPos = Map.WorldPosition.ReadChunkGrindex_Static(r); //.ReadStreamNisse(r);

                        if (Ref.netSession.IsHost)
                        {

                            Map.WorldPosition minWp = Editor.VoxelDesigner.HeroPosToCreationStartPos(clientPlayer.BuildingPos);
                            //min//wp.UpdateWorldGridPos();
                            Map.WorldPosition maxWp = minWp;
                            maxWp.WorldGrindex += Editor.VoxelDesigner.CreationSizeLimit.Max;

                            //min//wp.UpdateChunkPos(); max//wp.UpdateChunkPos();

                            IntVector2 bpos = IntVector2.Zero;
                            //const int Radius = 1;
                            for (bpos.Y = minWp.ChunkGrindex.Y; bpos.Y <= maxWp.ChunkGrindex.Y; bpos.Y++)
                            {
                                for (bpos.X = minWp.ChunkGrindex.X; bpos.X <= maxWp.ChunkGrindex.X; bpos.X++)
                                {
                                    screen = LfRef.chunks.GetScreenUnsafe(bpos);
                                    //if (Map.World.RunningAsHost)
                                    //{
                                    //    new Map.SafeCopyCheck(bpos);
                                    //}

                                    if (screen == null || !screen.DataGridLoadingComplete)
                                    {
                                        if (LfRef.WorldHost)
                                        {
                                            screen = LfRef.chunks.GetScreen(bpos);
                                            //screen.generate1_Topographic();
                                            screen.generate2_HeightMap();
                                            screen.generate3_Detail();
#if PCGAME
                                            screen.ClientEditingFlag = true;
#endif

                                        }
                                        else
                                        {
                                            //map is changed and the files should be disqualified
                                            screen = LfRef.chunks.GetScreen(bpos);
                                            screen.CompleteRemoval();
                                        }
                                    }

                                }
                            }
                        }
                    }
                    break;
                case Network.PacketType.ClientEndingEditing:
                    Players.ClientPlayer cp6 = GetClientPlayer(sender);
                    if (cp6 != null)
                    {
                        cp6.InBuildMode = false;
                    }
                    break;
                case Network.PacketType.PlayerDied:
                    LocalHostingPlayerPrint(Engine.LoadContent.CheckCharsSafety(sender.Gamertag, VikingEngine.HUD.MessageHandler.StandardFont) + " died");
                    //Progress.AbsHeroDied();
                    break;

                case Network.PacketType.ChangeClientPermissions:
                    LocalHostingPlayer.ClientPermissions = (Players.ClientPermissions)r.ReadByte();
                    break;
                case Network.PacketType.InviteReady:
                    //if (Ref.netSession.IsHost)
                    //    LfRef.seed.ShareWorldIx();//Data.WorldSeed.ShareWorldIx();
                    break;
                case Network.PacketType.RequestGeneratingEnvObj:
                    LfRef.levels2.chunkHostDirector.ClientRequest(r, GetClientPlayer(sender));
                    break;
                case Network.PacketType.PermitGeneratingEnvObj:
                    LfRef.levels2.chunkHostDirector.netReadChunkHost(r);
                    break;
                case Network.PacketType.ReturnChunkHosting:
                    LfRef.levels2.chunkHostDirector.netReadReturnChunkHosting(r, GetClientPlayer(sender));
                    break;

                case Network.PacketType.RequestWorldLevel:
                    LfRef.levels2.netReadRequestLevel(r);
                    break;
                case Network.PacketType.CreateWorldLevel:
                    LfRef.levels2.netReadCreateLevel(r);
                    break;
                case Network.PacketType.BombExplosion:
                    GO.WeaponAttack.ItemThrow.AbsBomb bomb = GameObjCollection.GetFromId(r) as GO.WeaponAttack.ItemThrow.AbsBomb;
                    if (bomb != null)
                    {
                        bomb.NetworkReadExplosion(r);
                    }
                    break;
                case Network.PacketType.OutdatedChunk:
                    LfRef.world.AddOutDatedChunks(new List<IntVector2> { Map.WorldPosition.ReadChunkGrindex_Static(r) });
                    break;


                case Network.PacketType.GameCompleted:
                    IntVector2 magicianDeathPos = Map.WorldPosition.ReadChunkGrindex_Static(r);
                    bool noDeaths = r.ReadBoolean();
                    GameCompleted(false, magicianDeathPos, noDeaths);
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
           // Progress.GeneralProgress = Data.GeneralProgress.GameComplete;
            for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
            {
                GO.PlayerCharacter.AbsHero h = LfRef.LocalHeroes[i];
                //check distance
                if ((h.ScreenPos - chunk).SideLength() <= 4)
                {
                    //h.Player.UnlockThrophy(Trophies.DefeatFinalBoss);
                    //MusicDirector.GameCompletedSong();
                }
                //check no death trophy
                //if (noDeaths)
                //{
                //    h.Player.UnlockThrophy(Trophies.CompleteGameWithoutSingleDeath);
                //}
            }

            if (local)
            {
                //net share
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.GameCompleted, Network.PacketReliability.ReliableLasy);
                Map.WorldPosition.WriteChunkGrindex_Static(chunk, w); //chunk.WriteChunkGrindex(w);
                w.Write(noDeaths);
            }
        }

                

        //public void RemoveMessage(IMessage message)
        //{
        //    foreach (Players.Player p in localPlayers)
        //    {
        //        p.AddMessage(message, false);
        //    }
        //}
        public override void NetworkReadPacket(Network.ReceivedPacket packet)
        {
            readPacket(packet, null);
        }

        //public override bool NetworkLowDataOutBuffer()
        //{
        //    return updateSendChunk();
        //}


        //public Players.ClientPlayer GetClientPlayer(Network.AbsNetworkPeer gamer)
        //{
        //    return GetClientPlayer(gamer.Id);
        //}
        public Players.ClientPlayer GetClientPlayer(Network.AbsNetworkPeer sender)
        {
            return GetClientPlayer(sender.Id);
        }
        Players.AbsPlayer GetPlayerFromId(byte id)
        {
            foreach (Players.Player p in localPlayers)
            {
                if (p.pData.netId() == id)
                    return p;
            }
            return GetClientPlayer(id);
        }

        public Players.ClientPlayer GetClientPlayer(byte networkID)
        {
            clientPlayersCounter.Reset();
            while (clientPlayersCounter.Next())
            {
                if (clientPlayersCounter.sel.pData.netId() == networkID)
                    return clientPlayersCounter.sel;
            }
            Debug.LogWarning("Get sender ID, " + Network.NetLib.PacketType.ToString());
            return null;
            
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

        public bool HeroIsCloseToChunk(IntVector2 pos, int sideLength)
        {
            for (int i = 0; i < LfRef.AllHeroes.Count; ++i)
            {
                var saObject = LfRef.AllHeroes[i];
                if (saObject != null)
                {
                    if ((saObject.ScreenPos - pos).SideLength() <= sideLength)//Map.World.OpenScreenRadius_Mesh)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool ClientIsUsingChunk(IntVector2 pos)
        {
            if (Ref.netSession.HasInternet && Ref.netSession.IsHost)
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
            //LfRef.worldOverView.UnChangedChunk(chunk);
            LfRef.world.AddOutDatedChunks(new List<IntVector2> { chunk });

            //should tell clients
            if (LfRef.WorldHost)
            {
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.OutdatedChunk, Network.PacketReliability.Reliable);
                Map.WorldPosition.WriteChunkGrindex_Static(chunk, w);//chunk.ShortVec.WriteStream(w);
            }
        }

        public Players.Player LocalHostingPlayer
        {
            get
            {
                if (localPlayers.Count == 0) return null;
                return localPlayers[0];
            }
        }
        public bool TerrainDestruction
        {
            get
            {
                return true;
            }
        }
    }
    
}
