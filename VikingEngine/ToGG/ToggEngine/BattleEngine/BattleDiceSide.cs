using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG
{
    struct BattleDiceSide
    {
        public static readonly BattleDiceSide None = new BattleDiceSide(0, BattleDiceResult.Empty);
        public static readonly BattleDiceSide NoResult = new BattleDiceSide(0, BattleDiceResult.NO_RESULT);

        public float chance;
        public BattleDiceResult result;
        public bool enabled;

        public BattleDiceSide(float chance, BattleDiceResult result)
        {
            this.chance = chance;
            this.result = result;
            enabled = true;
        }

        public bool isEnabledAvoid()
        {
            return enabled &&
                (result == BattleDiceResult.Avoid ||
                result == BattleDiceResult.AvoidRanged ||
                result == BattleDiceResult.AvoidLongRange);
        }

        public int blockValue()
        {
            if (result == BattleDiceResult.Block1)
                return 1;
            if (result == BattleDiceResult.Block2)
                return 2;

            return 0;
        }

        public int hitValue(out bool critical)
        {
            critical = false;

            switch (result)
            {
                case BattleDiceResult.Hit1:
                    return 1;

                case BattleDiceResult.CriticalHit:
                    critical = true;
                    return 1;

                case BattleDiceResult.Hit2:
                    return 2;

                case BattleDiceResult.Hit3:
                    return 3;
            }

            return 0;
        }

        public int surgeValue()
        {
            switch (result)
            {
                case BattleDiceResult.Surge1: return 1;

                case BattleDiceResult.Surge2: return 2;

                default: return 0;
            }
        }

        public override string ToString()
        {
            return "Side (" + result.ToString() + " " + TextLib.PercentText(chance) + ")";
        }
    }

    enum BattleDiceResult
    {
        Empty,
        Hit1,
        Hit2,
        Hit3,

        CriticalHit,
        Surge1,
        Surge2,

        Retreat,

        Block1,
        Block2,

        Avoid,
        AvoidRanged,
        AvoidLongRange,

        Suicide,

        NO_RESULT,
    }
}
