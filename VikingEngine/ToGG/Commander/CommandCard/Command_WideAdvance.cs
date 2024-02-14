using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.CommandCard
{
    class Command_WideAdvance : AbsCommandCard
    {
        override public void init(AbsCmdPlayer player)
        {
            base.init(player);
            ordersLeft = 0;
            orders = new CheckList<OrderedUnit>(ordersLeft);
            CantOrderReason cantOrderReason;
            player.unitsColl.unitsCounter.Reset();
            while (player.unitsColl.unitsCounter.Next())
            {
                if (player.unitsColl.unitsCounter.sel.cmd().CanBeOrdered(false, out cantOrderReason))
                {
                    orders.list.Add(new OrderedUnit(player.unitsColl.unitsCounter.sel));
                }
            }
        }

        //public override int ModifyMoveLength(Unit unit)
        //{
        //    return 1;
        //}
        public override void ModifyMoveLength(AbsUnit unit, ref int moveLength)
        {
            moveLength = 1;
            //base.ModifyMoveLength(unit, ref moveLength);
        }

        override public int OrderStartCount { get { return orders.list.Count; } }

        override public CommandType CommandType { get { return CommandType.Wide_Advance; } }

        public override bool HasOrderCount { get { return false; } }
    }
}
