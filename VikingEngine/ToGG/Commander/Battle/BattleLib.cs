using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander
{
    static class BattleLib
    {
        public static BattleDice MeleeDie;
        public static BattleDice RangedDie;
        public static BattleDice BackstabDie;

        public static void Init()
        {
            MeleeDie = new BattleDice( BattleDiceType.Melee, "Melee");
            MeleeDie.sides = new List<BattleDiceSide>
            {
                new BattleDiceSide(toggLib.CloseCombatHitChance, BattleDiceResult.Hit1),
                new BattleDiceSide(toggLib.CloseCombatRetreatChance, BattleDiceResult.Retreat),
            };
            MeleeDie.AddNoneResult();

            RangedDie = new BattleDice(BattleDiceType.Ranged, "Ranged");
            RangedDie.sides = new List<BattleDiceSide>
            {
                new BattleDiceSide(toggLib.RangedCombatHitChance, BattleDiceResult.Hit1),
                new BattleDiceSide(toggLib.RangedCombatRetreatChance, BattleDiceResult.Retreat),
            };
            RangedDie.AddNoneResult();

            BackstabDie = new BattleDice(BattleDiceType.Backstab, "Backstab");
            BackstabDie.sides = new List<BattleDiceSide>
            {
                new BattleDiceSide(toggLib.CloseCombatHitChance, BattleDiceResult.Hit1),
            };
            BackstabDie.AddNoneResult();
        }

        public static bool DefenderCanCounter(bool defenderLeftHisSquare, AttackType attackType, AbsUnit defender)
        {
            return !defenderLeftHisSquare &&
                attackType.IsMelee &&
                defender.cmd().data.canCounterAttack;
        }
    }
}
