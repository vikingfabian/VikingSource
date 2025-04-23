using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Engine;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.GameState.BattleLab
{
    class BattleSetup
    {
       
        
        public int selectedPlayer = BattleSetupManager.BothPlayers;
        public ItemResourceType selectedWeapon = ItemResourceType.Sword;
        public int attackingPlayer = 0;
        public int angle = 90;

        public BattleSetup()
        {
        }

        
    }
}
