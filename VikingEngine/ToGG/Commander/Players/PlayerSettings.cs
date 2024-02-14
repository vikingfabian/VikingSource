using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.CommandCard;
using VikingEngine.ToGG.Commander.GO;

namespace VikingEngine.ToGG.Commander.Players
{
    class PlayerSettings
    {
        static int ArmyIndex = 0;
        public CmdStrategyCardDeck commandCardDeck;
        public ArmySetup armySetup = null;

        public PlayerSettings()
        { 
            //commandCardDeck = new CmdStrategyCardDeck();
            ArmyIndex++;
            //armySetup = new ArmySetup(ArmyRace.Human);//lib.IsOdd(ArmyIndex)? ArmyRace.Human : ArmyRace.Undead);
        }

        public void Set(ArmySetup armySetup)
        {
            this.armySetup = armySetup;
            commandCardDeck = new CmdStrategyCardDeck(armySetup.race);
        }
    }
}
