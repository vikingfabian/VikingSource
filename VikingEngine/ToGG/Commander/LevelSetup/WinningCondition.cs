using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.LevelSetup
{
    abstract class AbsWinningCondition
    {
        protected const string HoldTheBanner = "Win by holding the strategic banner. ";
        protected const string NoDeadLeader = "Your leader must survive.";
        protected const string NoDeadValuable = "All valuable units must survive.";

        protected static string MaxOpponentVPDesc(int vp)
        {
            return "Don't let the opponent collect " + vp.ToString() + " Victory Points.";
        }

        public int timeLimit = int.MinValue;

        public EndGameResult CheckWinningConditions(Commander.Players.AbsCmdPlayer player)
        {
            if (player.TurnsCount > 0)
            {
                if (allOpponentsAreDestroyed(player))
                {
                    if (toggRef.gamestate.gameSetup.level == LevelEnum.NONE)
                    {
                        return EndGameResult.PlayerWon;
                    }
                    else
                    {
                        return EndGameResult.MissionSuccess;
                    }
                }
                
                return this.CheckWinningConditions_Specific(player);
            }
            return EndGameResult.non;
        }

        bool allOpponentsAreDestroyed(Commander.Players.AbsCmdPlayer player)
        {
            var opponents = Commander.cmdRef.players.opponentPlayers(player);
            int totalEnemies = 0;
            foreach (var m in opponents)
            {
                if (m.unitSetupComplete)
                {
                    totalEnemies += m.unitsColl.units.Count;
                }
                else
                {
                    return false;
                }
            }

            return totalEnemies == 0;
        }

        abstract public EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player);

        protected bool playerHasAllFlags(Commander.Players.AbsCmdPlayer player)
        {
            toggRef.board.tileGrid.LoopBegin();

            while (toggRef.board.tileGrid.LoopNext())
            {
                var sq = toggRef.board.tileGrid.LoopValueGet();
                if (sq.tileObjects.HasObject(TileObjectType.TacticalBanner))
                {
                    if (sq.unit == null || sq.unit.globalPlayerIndex != player.pData.globalPlayerIndex)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        virtual public void onEndTurn(Commander.Players.AbsCmdPlayer player)
        { }

        virtual public void gameStartSetup() { }

        public bool HasTurnLimit { get { return timeLimit >= 0; } }

        virtual public void onUnitDestroyed(AbsUnit u) { }

        virtual public string Description() { throw new NotImplementedException(); }
    }

    class WinningCondition_Score : AbsWinningCondition
    {
        public WinningCondition_Score(GameSetup setup)
        {
            foreach (var m in setup.lobbyMembers)
            {
                m.playersetup.collectingVP = toggLib.WinnerScore;
            }
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (player.VictoryPoints >= toggLib.WinnerScore)
            {//Game ends
                return EndGameResult.PlayerWon;
            }
            return EndGameResult.non;
        }
    }

    class WinningCondition_DestroyAllMission : AbsWinningCondition
    {
        
        public WinningCondition_DestroyAllMission(int timeLimit)
        {
            this.timeLimit = timeLimit;
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (player is Commander.Players.LocalPlayer)
            {
                var units = Commander.cmdRef.players.CollectEnemyUnits(player.pData.globalPlayerIndex);
                if (units.array.Count == 0)
                {
                    return EndGameResult.MissionSuccess;
                }

                if (player.TurnsCount >= timeLimit)
                {
                    return EndGameResult.MissionFailed;
                }
            }

            return EndGameResult.non;
        }
    }

    class WinningCondition_DestroyAllEvil : AbsWinningCondition
    {
        public WinningCondition_DestroyAllEvil(int timeLimit)
        {
            this.timeLimit = timeLimit;
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (player is Commander.Players.LocalPlayer)//.BottomPlayer)
            {
                var units = Commander.cmdRef.players.CollectEnemyUnits(player.pData.globalPlayerIndex);

                int goodCount = 0, evilCount = 0;

                while (units.Next())
                {
                    if (units.sel.HasProperty(ChallengeLevelsData.GoodDummyProperty))
                    {
                        goodCount++;
                    }
                    else
                    {
                        evilCount++;
                    }
                }

                if (goodCount > 0 && evilCount == 0)
                {
                    return EndGameResult.MissionSuccess;
                }

                if (player.TurnsCount >= timeLimit)
                {
                    return EndGameResult.MissionFailed;
                }
            }

            return EndGameResult.non;
        }
    }

    class WinningCondition_CaptureFlags : AbsWinningCondition
    {
        public WinningCondition_CaptureFlags(int timeLimit)
        {
            this.timeLimit = timeLimit;
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (player is Commander.Players.LocalPlayer)
            {
                if (playerHasAllFlags(player))
                {
                    return EndGameResult.MissionSuccess;
                }

                if (player.TurnsCount >= timeLimit)
                {
                    return EndGameResult.MissionFailed;
                }
            }

            return EndGameResult.non;
        }

    }

    class WinningCondition_CaptureFlagsAndTakeNoDamage : AbsWinningCondition
    {
        public WinningCondition_CaptureFlagsAndTakeNoDamage(int timeLimit)
        {
            this.timeLimit = timeLimit;
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (player is Commander.Players.LocalPlayer)
            {
                if (playerHasAllFlags(player) && player.totalDamageRecieved == 0)
                {
                    return EndGameResult.MissionSuccess;
                }

                if (player.TurnsCount >= timeLimit)
                {
                    return EndGameResult.MissionFailed;
                }
            }

            return EndGameResult.non;
        }

    }


    class WinningCondition_Flee : AbsWinningCondition
    {
        int MaxOpponentVP;

        public WinningCondition_Flee(int MaxOpponentVP)
        {
            this.MaxOpponentVP = MaxOpponentVP;
        }

        public override EndGameResult CheckWinningConditions_Specific(Commander.Players.AbsCmdPlayer player)
        {
            if (player is Commander.Players.LocalPlayer)
            {
                if (player.unitsColl.units.Count == 0)
                {
                    return EndGameResult.MissionSuccess;
                }
            }
            else
            {
                if (player.VictoryPoints >= MaxOpponentVP)
                {//Game ends
                    return EndGameResult.MissionFailed;
                }
            }
            return EndGameResult.non;
        }

        public override string Description()
        {
            return "Flee with all units before the opponent collects " + MaxOpponentVP.ToString() + " Victory Points";
        }
    }


    struct EndGame
    {
        public static readonly EndGame DontEnd = new EndGame();

        public EndGameResult result;
        public string description;
    }

    enum EndGameResult
    {
        non,
        PlayerWon,
        MissionSuccess,
        MissionFailed,
    }
}
