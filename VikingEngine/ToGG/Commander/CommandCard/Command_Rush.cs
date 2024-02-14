using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.CommandCard
{
    class Command_Rush : AbsCommandCard
    {
        //public const int OrderCount = 5;

        override public void init(AbsCmdPlayer player)
        {
            base.init(player);
            //ordersLeft = OrderCount;
            orders = new CheckList<OrderedUnit>(ordersLeft);
        }

        public override void  ModifyMoveLength(AbsUnit unit, ref int moveLength)
        {
 	         moveLength += 1;
        }
        //public override int ModifyMoveLength(Unit unit)
        //{
        //    return base.ModifyMoveLength(unit) + 1;
        //}

        //override public int OrderStartCount { get { return OrderCount; } }

        override public CommandType CommandType { get { return CommandType.Rush; } }

        public override bool HasOrderCount { get { return true; } }
    }
}
