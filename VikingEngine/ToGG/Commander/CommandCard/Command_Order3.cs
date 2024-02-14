using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.Players;

namespace VikingEngine.ToGG.Commander.CommandCard
{
    class Command_Order3 : AbsCommandCard
    {
        //const int OrderCount = 3;

        override public void init(AbsCmdPlayer player)
        {
            base.init(player);

            orders = new CheckList<OrderedUnit>(ordersLeft);
        }

        //override public int OrderStartCount { get { return OrderCount; } }

        override public CommandType CommandType { get { return CommandType.Order_3; } }

        public override bool HasOrderCount { get { return true; } }
    }
}
