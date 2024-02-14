using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.CommandCard;

namespace VikingEngine.ToGG.Commander.Players
{
    class AiPlayer : AbsLocalPlayer
    {
        public AiPlayer(Engine.AbsPlayerData pData, int globalIndex, Data.PlayerSetup playerSetup)
            : base(pData, globalIndex, playerSetup)
        {
            relationVisuals.setEnemy();
        }

        public override void StartPhase(GamePhaseType phase)
        {
            gamePhase = AbsGamePhase.GetPhase(phase, this);
        }

        public override void Update()
        {
            //if (playerTurnPresentation == null || playerTurnPresentation.IsDeleted)
            {
                if (gamePhase.UpdateAi())
                {
                    nextPhase(true);
                }
            }
        }

        override public bool IsScenarioOpponent { get { return true; } }

        public override bool LocalHumanPlayer => false;
        //public override string Name()
        //{
        //    return "CPU";
        //}
    }

    class PassiveAiPlayer : AiPlayer
    {
        public PassiveAiPlayer(Engine.AbsPlayerData pData, int globalIndex, Data.PlayerSetup playerSetup)
            : base(pData, globalIndex, playerSetup)
        {

        }

        public override void Update()
        {
            //base.Update();
            //beginEndTurn();
            toggRef.gamestate.BeginNextPlayer();
        }

        public override bool IsPassive
        {
            get
            {
                return true;
            }
        }
    }
}
