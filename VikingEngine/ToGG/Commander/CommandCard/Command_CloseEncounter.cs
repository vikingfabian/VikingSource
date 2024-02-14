using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.CommandCard
{
    class Command_CloseEncounter : AbsCommandCard
    {
        int orderStartCount;

        override public void init(AbsCmdPlayer player)
        {
            base.init(player);
            var available = AvailableUnits(player);
            orders = new CheckList<OrderedUnit>(available.Count);
            foreach (var u in available)
            {
                orders.list.Add(new OrderedUnit(u));
            }

            orderStartCount = orders.list.Count;

            orders.EnableAll(true);
        }

        public static List<AbsUnit> AvailableUnits(AbsCmdPlayer player)
        {
            List<AbsUnit> result = new List<AbsUnit>();
            player.unitsColl.unitsCounter.Reset();
            while (player.unitsColl.unitsCounter.Next())
            {
                if (InCloseEncounter(player.unitsColl.unitsCounter.sel))
                {
                    result.Add(player.unitsColl.unitsCounter.sel);
                }
            }

            return result;
        }

        static bool InCloseEncounter(AbsUnit u)
        {
            return u.bAdjacentOpponents() && !u.Resting;
        }

        public override bool CanBeOrdered(AbsUnit u, AbsCmdPlayer player, bool ignoreContains, out CantOrderReason cantOrderReason)
        {
            if (base.CanBeOrdered(u, player, ignoreContains, out cantOrderReason))
            {
                if (InCloseEncounter(u))
                {
                    return true;
                }
            }
            return false;
        }

        override public int OrderStartCount { get { return orderStartCount; } }

        override public CommandType CommandType { get { return CommandType.Close_encounter; } }

        public override bool HasOrderCount { get { return false; } }
    }
}
