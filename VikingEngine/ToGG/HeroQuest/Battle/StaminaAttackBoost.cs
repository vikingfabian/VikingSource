using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest
{
    struct StaminaAttackBoost
    {
        public static readonly StaminaAttackBoost Zero = new StaminaAttackBoost();

        public const int MaxBoostCount = 2;
        public int boostCount;
        public int spentStamina;

        public bool AddAttack(Unit attacker)
        {
            int cost = addCost();

            if (boostCount < MaxBoostCount &&
                attacker.data.hero.stamina.spend(cost))
            {
                boostCount++;
                spentStamina += cost;
                return true;
            }

            return false;
        }

        public int addCost()
        {
            if (boostCount == 0)
            {
                return 1;
            }
            else if (boostCount == 1)
            {
                return 2;
            }
            return int.MaxValue;
        }
    }
}
