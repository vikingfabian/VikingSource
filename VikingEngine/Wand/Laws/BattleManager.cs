

using VikingEngine.Core.Wand.GO.Creature;

namespace VikingEngine.Core.Wand.Laws
{
    struct AttackData
    {
        public AbsCreature attacker, defender;

        public AttackData(AbsCreature attacker, AbsCreature defender)
        {
            this.attacker = attacker;
            this.defender = defender;
        }
    }

    static class BattleManager
    {
        public static void Attack(AttackData attack)
        {
            if (attack.attacker.health.HasValue)
            {
                int damage = attack.attacker.AttackStrength().GetRandom_Inc();
                attack.defender.TakeDamage(damage);
            }
        }
    }
}
