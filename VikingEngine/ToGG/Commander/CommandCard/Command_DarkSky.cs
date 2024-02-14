using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.CommandCard
{
    class Command_DarkSky : AbsCommandCard
    {
        //public const int OrderCount = 4;

        override public void init(AbsCmdPlayer player)
        {
            base.init(player);
            //ordersLeft = OrderCount;
            orders = new CheckList<OrderedUnit>(ordersLeft);
        }

        public override bool CanBeOrdered(AbsUnit u, AbsCmdPlayer player, bool ignoreContains, out CantOrderReason cantOrderReason)
        {
            return base.CanBeOrdered(u, player, ignoreContains, out cantOrderReason) && u.Data.wep.projectileRange > 0;
        }

        //override public int OrderStartCount { get { return OrderCount; } }

        override public CommandType CommandType { get { return CommandType.Dark_Sky; } }

        public override bool HasOrderCount { get { return true; } }
    }
}
