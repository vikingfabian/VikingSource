using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    static class effectsLib
    {
        public static void WeaponSwing(AbsUnit attacker, IntVector2 defender,
           AttackType attacktype)
        {
            if (attacker != null)
            {
                if (attacktype.IsMelee)
                {
                    new CloseCombatEffect(attacker.squarePos, defender);
                }
                else
                {
                    new RangedCombatEffect(attacker.squarePos, defender);
                }
            }
        }

        public static void SwingAndSlash(AbsUnit attacker, IntVector2 defender,
           AttackType attacktype)
        {
            if (attacker != null)
            {
                WeaponSwing(attacker, defender, attacktype);

                new Effects.DamageSlashEffect(defender, attacker.squarePos).delay = 400;
            }
        }
    }
}
