using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG
{
    class DiceArray
    {
        public BattleDice dice;
        public int count;

        public DiceArray(BattleDice type, int count = 1)
        {
            this.dice = type;
            this.count = count;
        }
    }
}
