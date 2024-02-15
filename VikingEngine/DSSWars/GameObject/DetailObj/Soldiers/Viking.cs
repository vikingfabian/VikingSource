using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    class Viking : SailorData
    {
        public Viking() 
            :base()
        {
            unitType = UnitType.Viking;
            factionUniqueType = 2;

            ArmySpeedBonusSea = 0.6;
            attackDamage = 50;
            attackDamageStructure = 50;

            goldCost = MathExt.MultiplyInt(1.5, goldCost);
        }
    }
}
