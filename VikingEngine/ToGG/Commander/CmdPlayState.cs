using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.GameState;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.Commander.GO;
using VikingEngine.ToGG.Commander.LevelSetup;
//xna

namespace VikingEngine.ToGG.Commander
{    
    class CmdPlayState : AbsPlayState
    {
        List<AbsNetActionClient> netActions = new List<AbsNetActionClient>();
        ToggEngine.Display2D.PlayerTurnPresentation playerTurnPresentation = null;
        public Display.PlayersStatusHUD playersStatusHUD;

        public CmdPlayState(GameSetup gameSetup)
            : base(GameMode.Commander)
        {
            const string MusicFolder = "StrategyMusic";

            new UnitsData.AllUnits(); 
            this.gameSetup = gameSetup;
            gameSetup.GameInit();

            cmdRef.gamestate = this;
            //toggRef.menu = null;
            new MenuSystem(Input.InputSource.DefaultPC);
            cmdRef.hud = new Display.PlayerHUD();
            unitMessages = new ToggEngine.Display3D.UnitMessagesHandler();

            //draw.ClrColor = Color.White;
            //if (Ref.music == null)
            //{
            //    Ref.music = new Sound.MusicPlayer();
                    
            //    Ref.music.SetPlaylist(new List<Sound.SongData>
            //    {
            //        //new Sound.SongData(MusicFolder + FilePath.Dir + "Aftermath_loop", true, 1f),
            //        new Sound.SongData(MusicFolder + DataStream.FilePath.Dir + "BBaaB_loop", true, 1f),
            //        new Sound.SongData(MusicFolder + DataStream.FilePath.Dir + "Gargoyle_loop", true, 1f),
            //        new Sound.SongData(MusicFolder + DataStream.FilePath.Dir + "RM 1 - Introversion", false, 1f),
            //        new Sound.SongData(MusicFolder + DataStream.FilePath.Dir + "RM 10 - Incubation", false, 1f),
            //        new Sound.SongData(MusicFolder + DataStream.FilePath.Dir + "RM 2 - Arcane Benevolence", false, 1f),
            //        new Sound.SongData(MusicFolder + DataStream.FilePath.Dir + "RM 3 - Left in Autumn", false, 1f),
            //        new Sound.SongData(MusicFolder + DataStream.FilePath.Dir + "RM 4 - Warhogs", false, 1f),
            //        new Sound.SongData(MusicFolder + DataStream.FilePath.Dir + "RM 5 - Suddenly Empty", false, 1f),
            //        new Sound.SongData(MusicFolder + DataStream.FilePath.Dir + "RM 6 - Auderesne", false, 1f),
            //        new Sound.SongData(MusicFolder + DataStream.FilePath.Dir + "RM 7 - For Eternity", false, 1f),
            //        new Sound.SongData(MusicFolder + DataStream.FilePath.Dir + "RM 8 - Asynchronous Flanking", false, 1f),
            //        new Sound.SongData(MusicFolder + DataStream.FilePath.Dir + "RM 9 - Weeping Bedlam", false, 1f),
            //        new Sound.SongData(MusicFolder + DataStream.FilePath.Dir + "YesGod_loop", true, 1f),
            //    }, true);
            //}
            
            
            new ToggEngine.Map.Board(gameSetup, ToggEngine.Map.BoardType.Match);
            new Commander.Players.PlayerCollection().createPlayers(gameSetup);

            playersStatusHUD = new Display.PlayersStatusHUD(Commander.cmdRef.players.PlayerCount);

            if (gameSetup.loadMap == null)
            {
                toggRef.board.model.beginGenerateModel();
                if (Ref.netSession == null || Ref.netSession.IsHostOrOffline)
                {
                    BeginNextPlayer(true);
                }
            }
            else
            {
                loadingMap = new Data.LoadMap(gameSetup);
            }
        }

        public override void Time_Update(float time)
        {
            if (loadingMap != null) return;

            base.Time_Update(time);

            if (netActions.Count > 0)
            {
                if (netActions[0].Update())
                {
                    netActions.RemoveAt(0);
                }
            }
            else if (playerTurnPresentation != null)
            {
                Commander.cmdRef.players.ActiveLocalPlayer().mapControls.updateMapMovement(false);

                if (playerTurnPresentation.update())
                {
                    playerTurnPresentation.DeleteMe();
                    playerTurnPresentation = null;
                    EndNextPlayerAnimation();
                }
            }
            else
            {
                bool lockedInActionQue = que.Update();

                if (!lockedInActionQue)
                {
                    Commander.cmdRef.players.allPlayers.Selected().Update();
                    if (!(Commander.cmdRef.players.allPlayers.Selected() is Players.LocalPlayer) &&
                        !Commander.cmdRef.players.allPlayers.Selected().IsPassive)
                    {
                        foreach (Players.AbsCmdPlayer p in Commander.cmdRef.players.allPlayers.list)
                        {
                            p.UpdateSpectating();
                        }
                    }
                }
            }

            unitMessages.update();
            toggRef.board.update();
            toggRef.cam.update();
            //camera.Time_Update(time);
            Ref.music?.Update();
        }

        public override void NetworkReadPacket(Network.ReceivedPacket packet)
        {
        
            switch (packet.type)
            {
                case Network.PacketType.cmdShareUnitSetup:
                    {
                       // var p = cmdRef.gamestate.players.readGenericPlayer(packet.r);
                       // p.unitsColl.ReadUnitSetup(packet.r);
                    }
                    break;
                case Network.PacketType.cmdNetAction:
                    {
                        AbsNetActionClient action = null;

                        switch ((NetActionType)packet.r.ReadByte())
                        { 
                            //case NetActionType.Attack:
                            //    action = new NetActionAttack_Client(packet.r, Engine.Screen.SafeArea);
                            //    break;
                            //case NetActionType.Movement:
                            //    action = new NetActionMove_Client(packet);
                            //    break;
                            case NetActionType.EndTurn:
                                action = new NetActionEndTurn_Client(packet);
                                break;
                            case NetActionType.StartPhase:
                                action = new NetActionStartPhase_Client(packet);
                                break;
                            case NetActionType.FollowUp:
                                action = new NetActionFollowUp_Client(packet);
                                break;
                        }

                        netActions.Add(action);
                    }
                    break;
                case Network.PacketType.cmdSelectedCommand:
                    CommandCard.CommandType commandType = (CommandCard.CommandType)packet.r.ReadByte();
                    new Effects.ViewSelectedCommand(commandType);
                    break;
                case Network.PacketType.cmdOrderUnit:
                    {
                        AbsGenericPlayer p;
                        AbsUnit u = GetUnit(packet.r, out p);
                        if (p is Players.RemotePlayer)
                        {
                            bool addOrder = packet.r.ReadBoolean();
                            Players.RemotePlayer remoteP = (Players.RemotePlayer)p;
                            remoteP.OrderUnit(u, addOrder);
                        }
                    }
                    break;
        
            }
        }

        public override void NetEvent_ConnectionLost(string reason)
        {
            new GameState.MainMenuState();
        }

        bool autowin = false;
        override public void debugAutoWin()
        {
            autowin = true;
            toggRef.menu.CloseMenu();
            BeginNextPlayer();
        }

        override public void BeginNextPlayer()
        {
            BeginNextPlayer(false);
        }

        bool nextPlayer_firstTurn;
        GamePhaseType nextPlayer_startPhase;
        bool nextPlayer_deployPhase = false;

        private void BeginNextPlayer(bool firstTurn)
        {//turn completed, next player
            nextPlayer_firstTurn = firstTurn;

            Players.AbsCmdPlayer nextPlayer;
            Players.AbsCmdPlayer currentPlayer = Commander.cmdRef.players.allPlayers.Selected();

            if (firstTurn)
            {
                gameSetup.WinningConditions.gameStartSetup();
                nextPlayer = currentPlayer;
            }
            else
            {
                int non1; bool non2;
                nextPlayer = Commander.cmdRef.players.allPlayers.PeekNext(true, out non1, out non2);

                currentPlayer.EndTurn();
            }

            if (checkEndGame()) return;

            playersStatusHUD.PlayerTurn(nextPlayer.pData.globalPlayerIndex);

            if (nextPlayer.settings.armySetup == null && gameSetup.loadMap == null)
            {
                nextPlayer_startPhase = GamePhaseType.SelectArmy;
                nextPlayer_deployPhase = true;
            }
            else
            {
                nextPlayer_startPhase = GamePhaseType.SelectCommand;
                nextPlayer_deployPhase = false;
            }

            if (nextPlayer_deployPhase == false &&
                !firstTurn &&
                !nextPlayer.IsPassive)
            {
                bool turnCountDisplay = gameSetup.WinningConditions.HasTurnLimit;
                playerTurnPresentation = new ToggEngine.Display2D.PlayerTurnPresentation(nextPlayer, turnCountDisplay);
            }
            else
            {
                EndNextPlayerAnimation();
            }
        }

        protected void EndNextPlayerAnimation()
        {
            if (Ref.netSession.IsHostOrOffline)
            {
                if (!nextPlayer_firstTurn)
                {
                    Commander.cmdRef.players.allPlayers.Next_IsEnd(true);
                }

                playersStatusHUD.Refresh();
                var currentPlayer = Commander.cmdRef.players.allPlayers.Selected();
                currentPlayer.onTurnStart(nextPlayer_deployPhase);
                currentPlayer.StartPhase(nextPlayer_startPhase);
            }
            else
            {
                Commander.cmdRef.players.allPlayers.Next_IsEnd(true);
            }

            Commander.cmdRef.players.allPlayers.Selected().OnTurnStart();
        }

        public bool checkEndGame()
        {
            foreach (var m in Commander.cmdRef.players.allPlayers.list)
            {
                var endGame = gameSetup.WinningConditions.CheckWinningConditions(m);
                
                if (autowin)
                { endGame = EndGameResult.MissionSuccess; }

                if (endGame != EndGameResult.non)
                {
                    if (endGame == EndGameResult.PlayerWon)
                    {
                        new WinnerState(m);
                    }
                    else if (endGame == EndGameResult.MissionFailed || endGame == EndGameResult.MissionSuccess)
                    {
                        if (gameSetup.level != LevelEnum.NONE && endGame == EndGameResult.MissionSuccess)
                        {
                            toggRef.storage.onChallengeComplete(gameSetup.level);
                        }
                        var main = new MainMenuState();

                        if (Story1Missions.Levels.Contains(gameSetup.level))
                        {
                            main.storyResultMenu(gameSetup, endGame);
                        }
                        else
                        {
                            main.MissionResult(gameSetup, endGame == EndGameResult.MissionSuccess);
                        }
                    }
                    return true;
                }
            }

            return false;
        }

        public override void MapLoadComplete()
        {
            base.MapLoadComplete();

            if (gameSetup.useProgressSpawning)
            {
                toggRef.storage.activeStoryProgress.SpawnOnMap();
            }

            BeginNextPlayer(true);
        }

        //public override void SaveComplete(bool save, int player, bool completed, byte[] value)
        //{
        //    base.SaveComplete(save, player, completed, value);

        //    if (!save && gameSetup.useProgressSpawning)
        //    {
        //        toggRef.storage.activeStoryProgress.SpawnOnMap();
        //    }

        //    BeginNextPlayer(true);
        //}
    }
}
