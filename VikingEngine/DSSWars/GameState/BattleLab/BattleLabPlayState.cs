using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.XP;
using VikingEngine.Input;
using VikingEngine.ToGG.Commander.LevelSetup;

namespace VikingEngine.DSSWars.GameState.BattleLab
{
    class BattleLabPlayState : AbsBattlePlayState
    {
        public override PlayStateType PlayType()
        {
            return PlayStateType.BattleLab;
        }
    }
}
