using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.CommandCard
{
    class Command_DoubleTime: AbsCommandCard
    {
        override public void init(AbsCmdPlayer player)
        {
            base.init(player);
            orders = new CheckList<OrderedUnit>(ordersLeft);
        }

       override public bool CanBeOrdered(AbsUnit u, AbsCmdPlayer player, bool ignoreContains, out CantOrderReason cantOrderReason)
        {
            if (u != null && u.IsParent(player))
            {
                bool result = u.cmd().CanBeOrdered(true, out cantOrderReason);
                if (result)
                {
                    if (!ignoreContains && containsUnit(u))
                    {
                        cantOrderReason = CantOrderReason.AlreadyOrdered;
                        result = false;
                    }
                }

                return result;
            }
            else
            {
                cantOrderReason = CantOrderReason.NotMyUnit;
                return false;
            }

           // return u != null && u.globalPlayerIndex == player && u.Resting && u.cmd().CanBeOrdered(true) && (ignoreContains || !containsUnit(u));
        }

        override public CommandType CommandType { get { return CommandType.DoubleTime; } }

        public override bool HasOrderCount { get { return true; } }
    }
}
