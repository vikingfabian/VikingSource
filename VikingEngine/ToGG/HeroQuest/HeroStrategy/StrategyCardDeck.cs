using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.HeroQuest.HeroStrategy;

namespace VikingEngine.ToGG.HeroQuest
{
    class StrategyCardDeck : AbsStrategyCardDeck
    {
        public HeroStrategy.AbsHeroStrategy active = null;

        public List<AbsHeroStrategy> cards;

        public StrategyCardDeck(List<HeroStrategyType> types)
        {
            cards = new List<AbsHeroStrategy>(types.Count);

            foreach (var m in types)
            {
                cards.Add(HeroStrategy.AbsHeroStrategy.GetStrategy(m));
            }
        }

        public void onTurnStart()
        {
            foreach (var m in cards)
            {
                m.onTurnStart();
            }

            active = null;
        }

        public void selectId(int id)
        {
            foreach (var m in cards)
            {
                if (m.Id == id)
                {
                    active = m as HeroStrategy.AbsHeroStrategy;
                    break;
                }
            }
        }

        public override List<AbsStrategyCard> Cards()
        {
            return arraylib.CastObject<AbsHeroStrategy, AbsStrategyCard>(cards);
        }
    }

    enum HeroStrategyType
    {
        NONE,
        Advance,
        FromTheShadows,
        PoisionBomb,
        StunBomb,
        Trapper,
        Fight,
        AimedShot,
        Run,
        Rest,

        LineOfFire,
        LeapAttack,
        ArrowRain,
        UltimatePiercingShot,
        Swing3,
        UltimateShieldWall,
        RunAndHide,
        UltimateHeroicCry,
        UltimateLootrun,
        KnifeDance,
        UltimateEarthShake,

        RabbitShape,
        LynxShape,
        BearShape,
        UltimateWerewolf,
    }
}
