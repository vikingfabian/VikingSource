using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.ToggEngine.Data;

namespace VikingEngine.ToGG.HeroQuest.HeroStrategy
{
    class UltimateShieldWall : AbsHeroStrategy
    {
        const int RetaliateStrength = 3;
        const int HeavyArmorCount = 2;

        public UltimateShieldWall()
        {
            cardSprite = SpriteName.hqStrategyShieldWall;
            name = UnltimateTitle;//"Shield wall";

            var retaliate = new Data.Retaliate(RetaliateStrength);
            description = "Move once. You and any adjacent friendly unit gain " + 
                TextLib.ValuePlusMinus(HeavyArmorCount) + " " + LanguageLib.DefenceHeavyArmor + 
                " and '" + retaliate.Name + "' for 1 turn.";
            useBloodRage = true;
        }

        public override void onTurnEnd(Unit heroUnit)
        {
            base.onTurnEnd(heroUnit);

            applyProperties(heroUnit);
            var adjacent = heroUnit.adjacentFriendlies(IntVector2.NegativeOne);
            if (adjacent != null)
            {
                foreach (var m in adjacent)
                {
                    applyProperties(m.hq());
                }
            }

            void applyProperties(Unit unit)
            {
                var retaliate = new Data.Retaliate(RetaliateStrength);
                retaliate.useCount = new ToGG.Data.UntilNextTurn();
                unit.data.properties.Add(retaliate);

                var shield = new Data.HeavyShield(HeavyArmorCount);
                shield.useCount = new ToGG.Data.UntilNextTurn();
                unit.data.properties.Add(shield);
            }
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            setMoveAttackCount(heroUnit, 1, 0);
        }

        //public override List<ActionType> actionList()
        //{
        //    return new List<ActionType>() { ActionType.Move };
        //}

        override public HeroStrategyType Type { get { return HeroStrategyType.UltimateShieldWall; } }
    }
}
