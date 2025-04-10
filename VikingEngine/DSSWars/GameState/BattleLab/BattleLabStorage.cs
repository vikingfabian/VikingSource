using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameState.BattleLab
{
    class BattleLabStorage
    {
        public static BattleLabStorage Singleton;

        public List<BattleSetup> previousSetups = new List<BattleSetup>();
        public BattleSetup setup = new BattleSetup();
    }
}
