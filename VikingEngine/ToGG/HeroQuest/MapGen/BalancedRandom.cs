using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.MapGen
{
    class BalancedRandom
    {
        PcgRandom rnd;
        double balance = 0;
        public BalancedRandom(PcgRandom rnd)
        {
            this.rnd = rnd;
        }

        public bool Chance(double percChance, bool higherIsHarder)
        {
            return next(higherIsHarder) <= percChance;
        }

        public float Plus_MinusF(float range, bool higherIsHarder)
        {
            return (float)(-range + 2.0 * range * next(higherIsHarder));
        }

        double next(bool higherIsHarder)
        {
            double value, balanceAdd;
            nextRandom();

            if (balance != 0 && lib.SameDirection(value, balance))
            {
                //ReRoll
                nextRandom();
            }

            balance += balanceAdd;
            return value;

            void nextRandom()
            {
                value = rnd.Double();

                balanceAdd = value - 0.5;
                if (!higherIsHarder)
                {
                    balanceAdd = -balanceAdd;
                }
            }
        }
    }
}
