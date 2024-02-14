using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.ToggEngine.BattleEngine;

namespace VikingEngine.ToGG
{
    static class LanguageLib
    {
        public const string LootDrop = "Loot drop";
        public const string AttackTerrain = "Attack terrain";
        public const string BattleDie = "Battle Dice";
        public const string Health = "Health";
        public const string BloodRage = "Blood Rage";
        public const string Stamina = "Stamina";

        public const string SpecialAction = "Special action";
        public const string SpecialActionDescStart = SpecialAction + ": ";

        public const string RangedWeaponDescStart = "Ranged weapon: ";
        public const string AtTurnEndDescStart = "At end of turn: ";

        public const string RetaliateDesc = "Will damage any opponent that attacks this unit. Must be in weapon range.";
        public const string MoveEnterPickup = "Will be picked up by a hero who move to the square";

        public const string DefenceDodge = "Dodge dice";
        public const string DefenceArmor = "Armor dice";
        public const string DefenceHeavyArmor = "Heavy armor dice";

        public const string MeleeSpecialist = "Melee combat specialist";
        public const string ProjectileSpecialist = "Ranged combat specialist";


        public static string AccValue(int value)
        {
            return "+" + value.ToString();
        }

        public static string AttackType(AttackType attackType)
        {
            if (attackType.IsMelee)
            {
                return "Melee Attack";
            }
            else
            {
                return "Ranged Attack";
            }
        }

        public static string FormatSentences(params string[] text)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < text.Length; ++i)
            {
                string t = text[i].TrimEnd(' ', '.');
                t.ToUpper(Ref.culture);
                result.Append(t);
                result.Append('.');

                if (arraylib.IsLast(i, text) == false)
                {
                    result.Append(' ');
                }
            }

            return result.ToString();
        }
    }

}
