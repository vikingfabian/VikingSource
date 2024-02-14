using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;
//xna

namespace VikingEngine.ToGG.Commander.Players
{
    class RemotePlayer : AbsCmdPlayer
    {
        GamePhaseType phaseType = GamePhaseType.Init;
        List<CommandCard.OrderedUnit> orderedUnits = new List<CommandCard.OrderedUnit>();

        public RemotePlayer(Engine.AbsPlayerData pData, int globalIndex, Data.PlayerSetup playerSetup)
            :base(pData, globalIndex, playerSetup)
        {
            relationVisuals.setEnemy();
        }

        public override void Update()
        {
            
        }

        public override GamePhaseType GamePhase
        {
            get { return phaseType; }
        }

        public override void StartPhase(GamePhaseType phase)
        {
            phaseType = phase;
            new NetActionStartPhase_Host(this, phase);
        }

        public override void EndTurn()
        {
            base.EndTurn();

            for (int i = 0; i < orderedUnits.Count; ++i)
            {
               orderedUnits[i].DeleteMe();
            }
            orderedUnits.Clear();
        }

        public void OrderUnit(AbsUnit u, bool add)
        {
            for (int i = 0; i < orderedUnits.Count; ++i)
            {
                if (orderedUnits[i].unit == u)
                {
                    orderedUnits[i].DeleteMe();
                    orderedUnits.RemoveAt(i);
                    break;
                }
            }

            if (add)
            {
                SpectatorTargetPos = u.squarePos;
                orderedUnits.Add(new CommandCard.OrderedUnit(u));
            }
        }

        override public bool IsScenarioOpponent { get { throw new NotImplementedException(); } }

        public override bool LocalHumanPlayer => false;
    }
}
