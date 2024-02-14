using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG
{
    struct BattleDiceModification
    {
        public BattleDiceResult side;
        public int pointAdd;

        public BattleDiceModification(BattleDiceResult side, int pointAdd)
        {
            this.side = side;
            this.pointAdd = pointAdd;
        }

        public string Description()
        {
            string result = TextLib.ValuePlusMinus(pointAdd) + " points chance to \"" +
                BattleDice.ResultDesc(side) + "\" dice side.";

            return result;
        }

        public void ApplyTo(BattleDice dice)
        {
            float perc = Percent;

            if (pointAdd > 0)
            {
                float max = dice.noneChance();

                if (max <= 0)
                {
                    return;
                }

                Bound.Max(ref perc, max);
            }

            for (int i = 0; i < dice.sides.Count; ++i)
            {
                if (dice.sides[i].result == side)
                {
                    var editSide = dice.sides[i];
                    editSide.chance += perc;// Bound.Min(editSide.chance + perc, 0f);

                    if (editSide.chance <= 0)
                    {
                        editSide.chance = 0;
                        editSide.enabled = false;
                    }

                    dice.sides[i] = editSide;
                    break;
                }
            }
        }

        float Percent => pointAdd * 0.01f;
    }
}
