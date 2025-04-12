using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.BattleLab;

namespace VikingEngine.DSSWars.GameState.BattleTrials
{
    class BattleTrialsPlayState : AbsBattlePlayState
    {
        public BattleTrialsPlayState()
            : base()
        { }

        public override PlayStateType PlayType()
        {
            return PlayStateType.BattleTrials;
        }
    }
}
