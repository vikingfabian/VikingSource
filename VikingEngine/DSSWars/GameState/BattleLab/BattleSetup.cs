using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Engine;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.GameState.BattleLab
{
    class BattleSetup
    {
       
        
        public int selectedPlayer = BattleSetupManager.BothPlayers;
        public ConscriptProfile conscript = new ConscriptProfile() { 
            man = ItemResourceType.Men, 
            weapon = ItemResourceType.Sword,
            specialization = SpecializationType.Traditional,
            training = TrainingLevel.Basic,
        };
        //public ItemResourceType selectedWeapon = ItemResourceType.Sword;
        public int attackingPlayer = 0;
        public int angle = 90;

        public BattleSetup()
        {
        }

        
    }
}
