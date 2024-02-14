using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
//xna
using VikingEngine.ToGG.Commander.CommandCard;

namespace VikingEngine.ToGG.Commander.LevelSetup
{
    /// <summary>
    /// All the modifiers and settings for a game level
    /// </summary>
    class GameSetup
    {
        public int seed = Ref.rnd.Int(int.MaxValue);

        public List<AbsLobbyMember> lobbyMembers;
        public string loadMap = null;

        public bool useStrategyCards = true;
        public bool useArmyDeployment = false;
        public bool manualGiveOrder = true;
        


        public AbsWinningCondition WinningConditions = null;

        public string missionName = null, missionDescription = null;

        public List<GamePhaseType> activeTurnPhases;

        public GameCategory category = GameCategory.QuickMatch;
        public LevelEnum level = LevelEnum.NONE;
        public bool passiveOpponent = false;

        public List<CommandType> availableStrategyCards = null;

        public bool AiMod_SuperAggressive = false;
        public bool AiMod_TerrainIgnorant = false;
        public bool AiMod_BannerLowPrio = false;

        public List<AiModifier_TargetUnit> aiTargetUnits = new List<AiModifier_TargetUnit>();
        public List<AiModifier_MoveUnit> aiMoveUnits = new List<AiModifier_MoveUnit>();

        public bool useProgressSpawning = false;
        public ArmyScale armyScale = ArmyScale.Standard;

        //public void initRelations()
        //{
        //    if (lobbyMembers == null || lobbyMembers.Count < 2)
        //    {
        //        throw new Exception();
        //    }

        //    int localPlayerCount = 0;
        //    foreach (var m in lobbyMembers)
        //    {
        //        if (m is LocalLobbyMember)
        //        {
        //            localPlayerCount++;
        //        }
        //    }

        //    for (int i = 0; i < lobbyMembers.Count; ++i)//each (var m in lobbyMembers)
        //    {
        //        new PlayerRelationVisuals(lobbyMembers[i], i, localPlayerCount);
        //    }
        //}

        public void GameInit()
        {
            if (WinningConditions == null)
            {
                WinningConditions = new WinningCondition_Score(this);
            }

            activeTurnPhases = new List<GamePhaseType>(4);

            activeTurnPhases.Add(GamePhaseType.SelectCommand);

            if (manualGiveOrder)
            {
                activeTurnPhases.Add(GamePhaseType.GiveOrder);
            }

            activeTurnPhases.Add(GamePhaseType.Move);
            activeTurnPhases.Add(GamePhaseType.Attack);

        }
    }

}
