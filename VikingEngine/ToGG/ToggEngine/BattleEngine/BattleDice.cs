using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG
{
    class BattleDice
    {
        public List<BattleDiceSide> sides;
        public BattleDiceType type;
        public string name;
        public float value = 1f;
        public SpriteName icon = SpriteName.MissingImage;

        public BattleDice(BattleDiceType type, string name)
        {
            this.type = type;
            this.name = name;
        }

        public void AddNoneResult()
        {
            sides.Add(new BattleDiceSide(0f, BattleDiceResult.Empty));
        }
        
        public float chanceSum()
        {
            float sum = 0;
            foreach (var m in sides)
            {
                sum += m.chance;
            }
            return sum;
        }

        public float noneChance()
        {
            return 1f - chanceSum();
        }

        public BattleDiceSide NextRandom(DiceRoll rnd)
        {
            float diceRoll = rnd.next();

            float totalChance = 0;
            foreach (var m in sides)
            {
                if (m.chance > 0f)
                {
                    totalChance += m.chance;
                    if (diceRoll < totalChance)
                    {
                        return m;
                    }
                }
            }

            return BattleDiceSide.None;
        }
        
        public BattleDice Clone()
        {
            BattleDice clone = new BattleDice(type, name);
            clone.icon = icon;
            clone.sides = new List<BattleDiceSide>(sides);
          
            return clone;
        }

        public void removeSide(BattleDiceResult side)
        {
            for (int i = 0; i < sides.Count; ++i)
            {
                if (sides[i].result == side)
                {
                    sides.RemoveAt(i);
                    return;
                }
            }
        }

        public void textures(out SpriteName icon, out SpriteName sideTex)
        {
            DiceTypeTexture(this.type, out icon, out sideTex);
        }

        public BattleDiceSide? getSide(BattleDiceResult type)
        {
            foreach (var side in sides)
            {
                if (side.result == type)
                {
                    return side;
                }
            }

            return null;
        }

        public void setChance(BattleDiceResult type, float chance)
        {
            for (int i = 0; i < sides.Count; ++i)
            {
                if (sides[i].result == type)
                {
                    BattleDiceSide side = sides[i];
                    side.chance = chance;
                    sides[i] = side;
                }
            }
        }

        public static void DiceTypeTexture(BattleDiceType type, out SpriteName icon, out SpriteName sideTex)
        {
            switch (type)
            {
                case BattleDiceType.Attack:
                    icon = SpriteName.toggDieTexAttackFront;
                    sideTex = SpriteName.toggDieTexAttackSide;
                    break;
                case BattleDiceType.Dodge:
                    icon = SpriteName.hqDieTexDefenceDodgeFront;
                    sideTex = SpriteName.hqDieTexDefenceSide;
                    break;
                case BattleDiceType.Armor:
                    icon = SpriteName.hqDieTexDefenceArmorFront;
                    sideTex = SpriteName.hqDieTexDefenceSide;
                    break;
                case BattleDiceType.HeavyArmor:
                    icon = SpriteName.hqDieTexDefenceHeavyArmorFront;
                    sideTex = SpriteName.hqDieTexDefenceSide;
                    break;

                case BattleDiceType.Melee:
                    icon = SpriteName.toggDieTexMeleeAttackFront;
                    sideTex = SpriteName.toggDieTexAttackSide;
                    break;
                case BattleDiceType.Ranged:
                    icon = SpriteName.toggDieTexRangeAttackFront;
                    sideTex = SpriteName.toggDieTexAttackSide;
                    break;
                case BattleDiceType.Backstab:
                    icon = SpriteName.toggDieTexAttackFront;
                    sideTex = SpriteName.toggDieTexAttackSide;
                    break;

                default:
                    icon = SpriteName.MissingImage;
                    sideTex = SpriteName.MissingImage;
                    break;
            }
        }

        public static SpriteName ResultIcon(BattleDiceResult r)
        {
            if (toggRef.mode == GameMode.HeroQuest)
            {
                switch (r)
                {
                    case BattleDiceResult.Empty:
                        return SpriteName.hqDieTexMiss;

                    case BattleDiceResult.Hit1:
                        return SpriteName.hqDieTexHit1;

                    case BattleDiceResult.Hit2:
                        return SpriteName.hqDieTexHit2;

                    case BattleDiceResult.Hit3:
                        return SpriteName.hqDieTexHit3;

                    case BattleDiceResult.CriticalHit:
                        return SpriteName.hqDieTexCritiqualHit;

                    case BattleDiceResult.Surge1:
                        return SpriteName.hqDieTexSurge1;

                    case BattleDiceResult.Surge2:
                        return SpriteName.hqDieTexSurge2;

                    //case BattleDiceResult.Retreat:
                    //    return SpriteName.cmdSlotMashineRetreat;

                    case BattleDiceResult.Block1:
                        return SpriteName.hqDieTexBlock1;

                    case BattleDiceResult.Block2:
                        return SpriteName.hqDieTexBlock2;

                    case BattleDiceResult.Avoid:
                        return SpriteName.hqDieTexDodge;

                    case BattleDiceResult.AvoidRanged:
                        return SpriteName.hqDieTexDodgeRanged;

                    case BattleDiceResult.AvoidLongRange:
                        return SpriteName.hqDieTexDodgeLongRange;

                }
            }
            else
            {
                switch (r)
                {
                    case BattleDiceResult.Empty:
                        return SpriteName.cmdDieTexMiss;

                    case BattleDiceResult.Hit1:
                        return SpriteName.cmdDieTexHit;

                    case BattleDiceResult.Retreat:
                        return SpriteName.cmdDieTexRetreat;

                }
            }

            return SpriteName.MissingImage;
        }

        public static string ResultDesc(BattleDiceResult r)
        {
            switch (r)
            {
                case BattleDiceResult.Hit1:
                    return "Hit";

                case BattleDiceResult.Hit2:
                    return "Two hits";

                case BattleDiceResult.Hit3:
                    return "Three hits";

                case BattleDiceResult.CriticalHit:
                    return "Critical hit";

                case BattleDiceResult.Retreat:
                    return "Retreat";

                case BattleDiceResult.Surge1:
                    return "Surge";

                case BattleDiceResult.Surge2:
                    return "Two surges";

                case BattleDiceResult.Block1:
                    return "Block";

                case BattleDiceResult.Block2:
                    return "Two blocks";

                case BattleDiceResult.Avoid:
                    return "Avoid melee and ranged attacks";

                case BattleDiceResult.AvoidRanged:
                    return "Avoid ranged attacks";

                case BattleDiceResult.AvoidLongRange:
                    return "Avoid long ranged attack";

                case BattleDiceResult.Empty:
                    return "Blank";

                default: return "ERR";

            }
        }

        public static string DiceTypeName(BattleDiceType type)
        {
            return Get(type).name;
            //switch (type)
            //{
            //    case BattleDiceType.Attack:
            //        return "Attack";

            //    case BattleDiceType.Dodge:
            //        return "Dodge";

            //    case BattleDiceType.Armor:
            //        return "Armor";

            //    case BattleDiceType.HeavyArmor:
            //        return "Heavy Armor";

            //    default: return "ERR";

            //}
        }

        public static BattleDice Get(BattleDiceType type)
        {
            switch (type)
            {
                case BattleDiceType.Attack:
                    return HeroQuest.hqLib.BattleDie;
                case BattleDiceType.Dodge:
                    return HeroQuest.hqLib.BattleDie;
                case BattleDiceType.Armor:
                    return HeroQuest.hqLib.ArmorDie;
                case BattleDiceType.HeavyArmor:
                    return HeroQuest.hqLib.HeavyArmorDie;

                case BattleDiceType.Melee:
                    return ToGG.Commander.BattleLib.MeleeDie;
                case BattleDiceType.Ranged:
                    return ToGG.Commander.BattleLib.RangedDie;
                case BattleDiceType.Backstab:
                    return ToGG.Commander.BattleLib.BackstabDie;

                default:
                    throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return type.ToString() + " Dice (" + sides.Count + ")";
        }
    }
    

    enum BattleDiceType
    {
        None,
        Attack,
        Dodge,
        Armor,
        HeavyArmor,

        Melee,
        Ranged,
        Backstab,
    }    
}
