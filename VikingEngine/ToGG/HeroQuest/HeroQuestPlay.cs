using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Network;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.HeroQuest.Data;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest
{
    class HeroQuestPlay : AbsPlayState
    {
        public ToggEngine.Display2D.PlayerTurnPresentation playerTurnPresentation = null;

        
        public LevelProgress levelProgress;
        public GamePhase gamephase = GamePhase.Init;
        FileManager filemanager;
        int endGameCountDown = 4;
        bool mapLoaded = false;
        /*
        * INIT
        * HeroQuestPlay() > init1_assignHeroIndex()
        * SaveComplete > PacketType.hqClientReady
        * host allGamersReady() > netWriteSetup()
        */

        public HeroQuestPlay(bool hostingGame, FileManager filemanager)
            : base(GameMode.HeroQuest)
        {
            this.filemanager = filemanager;
            levelProgress = new LevelProgress();
            new EventManager();

            new Display.MenuSystem();
            hqRef.setup.RefreshConditions();
            new AllUnitsData();
            new NetManager(hostingGame);
            new Gadgets.ItemManager();
            unitMessages = new ToggEngine.Display3D.UnitMessagesHandler();

            //DiceModel.Init();
            hqRef.gamestate = this;

            new ToggEngine.Map.Board(ToggEngine.Map.BoardType.HeroQuest);


            new PlayerCollection();

            if (Ref.netSession.ableToConnect)
            {
                foreach (var m in Ref.netSession.RemoteGamers())
                {
                    new RemotePlayer(m);
                }
            }

            init1_assignHeroIndex();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (gamephase == GamePhase.EndGame)
            {
                if (--endGameCountDown <= 0)
                {
                    new GameState.ExitState();
                }
            }
            else if (gamephase != GamePhase.Init && mapLoaded)
            {
                bool lockedInActionQue = que.Update();

                if (!lockedInActionQue)
                {
                    if (hqRef.players.needsRefresh)
                    { hqRef.players.Refresh(); }
                    gameHasStarted = true;

                    if (playerTurnPresentation != null)
                    {
                        if (hqRef.players.currentTeam == PlayerCollection.HeroTeam &&
                            !playerTurnPresentation.cameraMoveTime.CountDown())
                        {
                            hqRef.players.localHost.cameraOnHero();
                        }
                        if (playerTurnPresentation.update())
                        {
                            removeTurnPresentation();
                        }
                    }

                    if (hqRef.players.currentTeam == PlayerCollection.HeroTeam)
                    {
                        hqRef.players.localHost.update();

                        var remotes = new SpottedArrayTypeCounter<AbsHQPlayer>(hqRef.players.allPlayers, typeof(RemotePlayer));
                        while (remotes.Next())
                        {
                            remotes.sel.update();
                        }

                        hqRef.players.dungeonMaster.idleUpdate();
                    }
                    else
                    {
                        hqRef.players.dungeonMaster.update();

                        hqRef.players.localHost.UpdateSpectating(hqRef.players.dungeonMaster.SpectatorTargetPos);
                        hqRef.players.localHost.idleUpdate();

                        var remotes = new SpottedArrayTypeCounter<AbsHQPlayer>(hqRef.players.allPlayers, typeof(RemotePlayer));
                        while (remotes.Next())
                        {
                            remotes.sel.idleUpdate();
                        }
                    }
                }

                unitMessages.update();

            }

            toggRef.board.update();
            toggRef.cam.update();
            Ref.music.Update();
        }



        bool update_asynch(int id, float time)
        {
            if (gameHasStarted)
            {
                hqRef.players.update_asynch(time);
            }
            return gamephase == GamePhase.EndGame;
        }

        void removeTurnPresentation()
        {
            if (playerTurnPresentation != null)
            {
                playerTurnPresentation.DeleteMe();
                playerTurnPresentation = null;
            }
        }

        public override void NetUpdate()
        {
            if (gamephase < GamePhase.Gameplay) return;

            base.NetUpdate();
            hqRef.players.localHost.NetUpdate();
        }

        public override void NetEvent_ConnectionLost(string reason)
        {
            base.NetEvent_ConnectionLost(reason);

            disconnect(reason);
            //if (NetLib.AllowDisconnect)
            //{
            //    var exit = new GameState.ExitState();
            //    if (reason != null)
            //    {
            //        exit.LostConnectionMessage(reason);
            //    }
            //}
        }

        public override void NetworkReadPacket(ReceivedPacket packet)
        {
            base.NetworkReadPacket(packet);

            RemotePlayer remotePlayer = packet.sender.Tag as RemotePlayer;

            switch (packet.type)
            {
                case PacketType.hqPlayerStatus:
                    if (remotePlayer != null)
                    {
                        remotePlayer.readNetUpdate(packet.r);
                    }
                    break;
                case PacketType.hqDefenceResult:
                    {
                        bool aiPlayer = packet.r.ReadBoolean();

                        if (aiPlayer)
                        {
                            if (hqRef.players.dungeonMaster.attackDisplay != null &&
                                hqRef.players.dungeonMaster.attackDisplay.attackRoll != null)
                            {
                                hqRef.players.dungeonMaster.attackDisplay.attackRoll.netReadDefenceResult(packet);
                            }
                        }
                    }
                    break;

                case PacketType.hqDiceRoll:
                    {
                        bool isMelee = packet.r.ReadBoolean();
                        bool aiPlayer = packet.r.ReadBoolean();
                        BattleDiceResult dieResult = (BattleDiceResult)packet.r.ReadByte();
                        var totalRollResult = new ToggEngine.BattleEngine.AttackRollResult(packet.r);

                        if (aiPlayer)
                        {
                            if (hqRef.players.dungeonMaster.attackDisplay != null &&
                                hqRef.players.dungeonMaster.attackDisplay.attackRoll != null)
                            {
                                hqRef.players.dungeonMaster.attackDisplay.attackRoll.viewClientAttackResult(dieResult, totalRollResult);
                            }
                        }
                        else if (remotePlayer != null)
                        {
                            remotePlayer.nameDisplay.viewAttackResult(isMelee, dieResult);
                        }
                    }
                    break;
                case PacketType.hqAttackResult:
                    AttackDisplay.NetReadAttackResult(packet);
                    break;

                case PacketType.hqSendDamage:
                    RecordedDamageEvent rec = new RecordedDamageEvent(packet.r);
                    rec.apply();
                    break;

                case PacketType.hqClientReady:
                    if (remotePlayer != null)
                    {
                        remotePlayer.isReady = true;
                        if (hqRef.netManager.host && allGamersReady())
                        {
                            netWriteSetup();
                            startGame();
                        }
                    }
                    else
                    {
                        disconnect("Missing remote player data");
                        //if (NetLib.AllowDisconnect)
                        //{
                        //    new GameState.ExitState().LostConnectionMessage("Missing remote player data");
                        //}
                    }
                    break;
                case PacketType.hqShareSetup:
                    {
                        netReadSetup(packet, remotePlayer);
                        if (!hqRef.netManager.host)
                        {
                            startGame();
                        }
                    }
                    break;
                case PacketType.hqShareEquipment:
                    if (remotePlayer != null)
                    {
                        remotePlayer.Backpack().equipment.read(packet.r);
                    }
                    break;
                case PacketType.hqDodgeEffect:
                    var defender = Unit.NetReadUnitId(packet.r);
                    var attacker = Unit.NetReadUnitId(packet.r);

                    if (defender != null)
                    {
                        defender.dodgeAnimation(attacker, false);
                    }
                    break;
                case PacketType.toggUnitPropertyStatus:
                    ToGG.Data.Property.AbsUnitProperty.NetReadPropertyStatus(packet.r);
                    break;
                case PacketType.hqAiAction:
                    hqRef.players.dungeonMaster.netRead(packet);
                    break;
                case PacketType.hqQueAction:
                    ToggEngine.QueAction.AbsQueAction.NetReadAction(packet);
                    break;
                case PacketType.hqKillMark:
                    new Killmark(packet);
                    break;
                case PacketType.hqTileStomp:
                    toggRef.board.netreadTileStomp(packet.r);
                    break;
                case PacketType.hqAiAlerted:
                    hqRef.players.dungeonMaster.netReadAlert(packet.r);
                    break;
                case PacketType.hqMonsterSpawn:
                    hqRef.players.dungeonMaster.netReadSpawn(packet.r);
                    break;
                case PacketType.hqApplyStatusEffect:
                    Data.Condition.AbsCondition.NetReadAppliedStatusEffect(packet.r);
                    break;
                case PacketType.hqOnObjective:
                    hqRef.setup.conditions.netReadObjective(packet.r);
                    break;
                case PacketType.hqSpectatePos:
                    hqRef.players.dungeonMaster.SpectatorTargetPos = toggRef.board.ReadPosition(packet.r);
                    break;
                case PacketType.hqUseItem:
                    Gadgets.AbsItem.NetReadQuickUse(packet);
                    break;
                case PacketType.hqTileItemColl:
                    Gadgets.TileItemCollection.NetRead(packet.r);
                    break;
                case PacketType.hqTileObjEvent:
                    ToggEngine.TileObjLib.NetReadEvent(packet.r);
                    break;
                case PacketType.hqCommunicate:
                    Players.Phase.CommunicationPalette.NetRead(packet.r);
                    break;
                case PacketType.hqGiftAchievement:
                    ToggEngine.Data.GiftedAchievement.NetRead(packet.r);
                    break;
                case PacketType.hqSendItem:
                    hqRef.items.netReadSendItem(packet);
                    break;
                case PacketType.hqLevelProgress:
                    levelProgress.netRead(packet.r);
                    break;
                case PacketType.hqLevelConditionEvent:
                    byte eventId = packet.r.ReadByte();
                    hqRef.setup.conditions.onConditionEvent(eventId, packet.r);
                    break;
                default:
                    hqRef.netManager.read(packet, remotePlayer);
                    break;
            }
        }

        public override void NetEvent_PeerLost(AbsNetworkPeer peer)
        {
            base.NetEvent_PeerLost(peer);

            disconnect("Lost peer " + peer.Gamertag);
            //var exit = new GameState.ExitState();
            //exit.LostConnectionMessage("Lost peer " + peer.Gamertag);
        }

        void disconnect(string reason)
        {
            if (NetLib.AllowDisconnect)
            {
                var exit = new GameState.ExitState();
                if (reason != null)
                {
                    exit.LostConnectionMessage(reason);
                }
            }
        }

        bool allGamersReady()
        {
            var remotes = new SpottedArrayTypeCounter<AbsHQPlayer>(hqRef.players.allPlayers, typeof(RemotePlayer));
            while (remotes.Next())
            {
                if (!((RemotePlayer)remotes.sel).isReady) return false;
            }
            return true;
        }

        void init1_assignHeroIndex()
        {
            if (hqRef.netManager.host)
            {
                for (int i = 0; i < hqRef.players.allPlayersUnsorted.Count; ++i)
                {
                    hqRef.players.allPlayersUnsorted[i].assignPlayerIx(i);
                }

                if (Ref.netSession.InMultiplayerSession)
                {
                    var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqAssignPlayers, Network.PacketReliability.Reliable);
                    //w.Write(toggRef.board.seed);

                    foreach (var m in hqRef.players.allPlayersUnsorted)
                    {
                        if (m.pData.Type != Engine.PlayerType.Ai)
                        {
                            var peer = m.pData.netPeer();
                            w.Write(true);
                            w.Write(peer.FullId);
                            w.Write((byte)m.pData.globalPlayerIndex);
                        }
                    }
                    w.Write(false);
                }
                onPlayersAssigned();
            }
        }

        public void init1_netReadAssignHeroIndex(System.IO.BinaryReader r)
        {
            while (r.ReadBoolean())
            {
                ulong id = r.ReadUInt64();
                int index = r.ReadByte();

                var p = hqRef.players.PlayerFromNetId(id);
                p.assignPlayerIx(index);
            }
            onPlayersAssigned();
        }

        void onPlayersAssigned()
        {
            hqRef.players.sortPlayers();
            filemanager.loadMapFromMemory();
        }

        void netWriteSetup()
        {
            var w = Ref.netSession.BeginWritingPacket(PacketType.hqShareSetup, PacketReliability.Reliable);
            w.Write(hqRef.netManager.host);

            if (hqRef.netManager.host)
            {
                //hqRef.players.dungeonMaster.hqUnits.writeAllUnits(w);
            }
            hqRef.players.localHost.hqUnits.writeAllUnits(w);
        }

        void netReadSetup(ReceivedPacket packet, RemotePlayer remotePlayer)
        {
            bool host = packet.r.ReadBoolean();

            if (host)
            {
                //hqRef.players.dungeonMaster.hqUnits.readAllUnits(packet.r);
            }

            remotePlayer.hqUnits.readAllUnits(packet.r);
            if (remotePlayer.nameDisplay != null)
            {
                remotePlayer.nameDisplay.refreshImages(remotePlayer);
            }

            if (host)
            {
                netWriteSetup();
            }
        }

        public override void NewMapPrep()
        {
            base.NewMapPrep();
            new Data.LootManager();
        }

        

        public override void MapLoadComplete()
        {
            base.MapLoadComplete();
            hqRef.setup.onStart();

            if (hqRef.netManager.host)
            {
                hqRef.players.localHost.heroIndex = 0;
            }
            else
            {
                Ref.netSession.BeginWritingPacketToHost(PacketType.hqClientReady, 
                    PacketReliability.Reliable, null);
            }

            //questSetup();

            var spawn = new MapGen.SpawnManager();
            hqRef.setup.conditions.environmentSpawn(spawn);
            spawn.spawn();
            hqRef.setup.conditions.monsterSpawn(spawn);

            hqRef.players.localHost.spawnHero();
            //if (hqRef.players.localHost.HeroUnit == null)
            //{
            //    throw new Exception("Hero did not spawn");
            //}

            mapLoaded = true;

            if (hqRef.netManager.host && Ref.netSession.RemoteGamersCount == 0)
            {
                startGame();
            }            
        }

        //void questSetup()
        //{
        //    switch (hqRef.setup.quest)
        //    {
        //        case QuestName.NagaBoss:
        //            var levers = toggRef.board.metaData.tags.list(1);

        //            new ToggEngine.GO.Lever(levers[0], 1);
        //            new ToggEngine.GO.Lever(levers[1], 0);
        //            break;
        //    }
        //}

        
        
        //List<Unit> spawnEnemies(Map.TeamSpawnCollection spawnColl, HqUnitType type, int spawnId)
        //{
        //    var spawnPositions = spawnColl.typeSpawns[spawnId];
        //    List<Unit> units = new List<Unit>(spawnPositions.Count);

        //    foreach (var pos in spawnPositions)
        //    {
        //        units.Add(new Unit(pos, type, hqRef.players.dungeonMaster));
        //    }

        //    return units;
        //}

        //void spawnEnemy(HqUnitType type, IntVector2 pos)
        //{
        //    new Unit(pos, type, hqRef.players.dungeonMaster);
        //}

        void startGame()
        {
            if (gamephase == GamePhase.Init)
            {
                gamephase = GamePhase.StartGameReady;

                var allPlayersCounter = hqRef.players.allPlayers.counter();
                while (allPlayersCounter.Next())
                {
                    allPlayersCounter.sel.startGame();
                }

                if (hqRef.netManager.host)
                {
                    new QueAction.QueActionStartTurn(PlayerCollection.HeroTeam);
                }

                hqRef.events.SendToAll(EventType.GameStart, null);

                new AsynchUpdateable(update_asynch, "Heroquest asynch update", 0);
            }
        }

        public void endGame()
        {
            gamephase = GamePhase.EndGame;
        }
        
        public void endTurn()
        {
            //Debug.Log("End Turn " + currentTeam.ToString() + ": time" + UpdateCount.ToString());

            gamephase = GamePhase.EndTurnAnimations;

            var endingPlayers = hqRef.players.activePlayers();//.allTeamPlayers(currentTeam);

            foreach (var m in endingPlayers)
            {
                AbsHQPlayer p = (AbsHQPlayer)m;
                p.onTurnEnd();
            }

            hqRef.events.SendToAllButInactivePlayers(EventType.TurnEnd, null);

            if (hqRef.netManager.host)
            {
                //int next;

                //if (hqRef.setup.conditions.HasDungeonMaster)
                //{
                //    next = lib.AlternateBetweenTwoValues(currentTeam,
                //        PlayerCollection.HeroTeam, PlayerCollection.DungeonMasterTeam);
                //}
                //else
                //{
                //    next = PlayerCollection.HeroTeam;
                //}

                new QueAction.QueActionStartTurn(hqRef.players.NextTeam());
            }
        }

        public void startNextTurn(int startNextTeam)
        {
            Debug.Log("Start Turn " + startNextTeam.ToString() + ": time" + UpdateCount.ToString());

            if (startNextTeam == PlayerCollection.HeroTeam)
            {
                gamephase = GamePhase.Gameplay;
            }
            else
            {
                gamephase = GamePhase.SpectateAi;
            }
            hqRef.players.currentTeam = startNextTeam;
            
            var p = hqRef.players.activePlayer();

            removeTurnPresentation();
            playerTurnPresentation = new ToggEngine.Display2D.PlayerTurnPresentation(p, 
                hqRef.setup.conditions.HasDungeonMaster == false);
            p.onTurnStart();
            hqRef.events.SendToAllButInactivePlayers(EventType.TurnStart, p);

            hqRef.players.localHost.hud.refreshRemoteEndTurn();
        }
    }

    enum GamePhase
    {
        Init,
        StartGameReady,
        Gameplay,
        EndTurnAnimations,
        SpectateAi,
        EndGame,
    }
}


