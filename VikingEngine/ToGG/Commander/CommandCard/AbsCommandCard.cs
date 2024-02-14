using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.CommandCard
{
    abstract class AbsCommandCard
    {
        public CheckList<OrderedUnit> orders;
        public int ordersLeft;
        protected Commander.Players.AbsCmdPlayer player;

        //public AbsCommandCard(Commander.Players.AbsCmdPlayer player)
        //{
        //    ordersLeft = OrderStartCount;
        //    this.player = player.pData.globalPlayerIndex;
        //}

        virtual public void init(Commander.Players.AbsCmdPlayer player)
        {
            this.player = player;
            ordersLeft = OrderStartCount;
        }

        public void unitsAvailableForOrder(Players.AbsCmdPlayer player,
            out ListWithSelection<AbsUnit> canBeOrdered, out List<IntVector2> positions)
        {
            canBeOrdered = new ListWithSelection<AbsUnit>();
            //canBeOrdered.list.Clear();
            positions = new List<IntVector2>(player.unitsColl.units.Count);
            player.unitsColl.unitsCounter.Reset();
            CantOrderReason cantOrderReason;
            
            while (player.unitsColl.unitsCounter.Next())
            {
                if (player.unitsColl.unitsCounter.sel.cmd().data.underType != UnitUnderType.Special_TacticalBase)
                {
                    if (CanBeOrdered(player.unitsColl.unitsCounter.sel, player, true, out cantOrderReason))
                    {
                        positions.Add(player.unitsColl.unitsCounter.sel.squarePos);
                        canBeOrdered.Add(player.unitsColl.unitsCounter.sel, false);
                    }
                }
            }
            canBeOrdered.selectedIndex = 0;
            //var result = new { canBeOrdered, positions };
            //return canBeOrdered;
        }

        virtual public bool CanBeOrdered(AbsUnit u, Players.AbsCmdPlayer player, 
            bool ignoreContains, out CantOrderReason cantOrderReason)
        {
            if (u != null && u.IsParent(player))
            {
                bool result = u.cmd().CanBeOrdered(false, out cantOrderReason);
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

           // return u != null && u.globalPlayerIndex == player && u.cmd().CanBeOrdered(false) && (ignoreContains || !containsUnit(u));
        }

        //public void createToolTip()
        //{
        //    new ToggEngine.Display2D.OrderToolTip(ordersLeft, OrderStartCount, Commander.cmdRef.players.ActiveLocalPlayer());
        //}

        public void removeDeadUnits()
        {
            for (int i = orders.Count - 1; i >= 0; --i)
            {
                if (orders.list[i].unit.Alive == false)
                {
                    orders.list[i].DeleteMe();
                    orders.list.RemoveAt(i);
                }
            }
        }

        /// <returns>Was able to command</returns>
        virtual public bool OrderUnit(AbsUnit u)
        {
            CantOrderReason cantOrderReason;

            if (containsUnit(u))
            { //toggle back the selection
                removeUnit(u);
            }
            else if (ordersLeft > 0 && CanBeOrdered(u, player, true, out cantOrderReason))
            {
                ordersLeft--;
                if (u.order == null)
                {
                    orders.list.Add(new OrderedUnit(u));
                }
                else
                {
                    orders.list.Add(u.order);
                    u.order.SetState(CheckState.Enabled);
                }

                //refreshSuggestions();

                return true;
            }
            return false;
        }

        

        void removeUnit(AbsUnit u)
        {
            for (int i = 0; i < orders.Count; ++i)
            {
                if (orders.list[i].unit == u)
                {
                    ordersLeft++;
                    //orders.list[i].DeleteMe();
                    orders.list[i].SetState(CheckState.Suggested);
                    orders.list.RemoveAt(i);
                    //refreshSuggestions();
                    return;
                }
            }
        }

        public bool containsUnit(AbsUnit u)
        {
            foreach (OrderedUnit ord in orders.list)
            {
                if (ord.unit == u)
                    return true;
            }
            return false;
        }
        public bool containsEnabledUnit(AbsUnit u)
        {
            foreach (OrderedUnit ord in orders.list)
            {
                if (ord.unit == u)
                    return ord.CheckList_Enabled;
            }
            return false;
        }

        public bool containsOrderedUnit(AbsUnit u)
        {
            foreach (OrderedUnit ord in orders.list)
            {
                if (ord.unit == u)
                {
                    return true;
                }
            }
            return false;
        }

        

        public bool hasEnabledUnits()
        {
            foreach (OrderedUnit ord in orders.list)
            {
                if (ord.CheckList_Enabled)
                    return true;
            }
            return false;
        }

        public void selectUnit(AbsUnit u)
        {
            for (int i = 0; i < orders.list.Count; ++i)
            {
                if (orders.list[i].unit == u)
                {
                    orders.selectedIndex = i;
                    return;
                }
            }
        }

        public OrderedUnit Get(AbsUnit u)
        {
            for (int i = 0; i < orders.list.Count; ++i)
            {
                if (orders.list[i].unit == u)
                {   
                    return orders.list[i];
                }
            }

            return null;
        }

        public void ClearOrders()
        {

            //foreach (var m in orders.list)
            //{
            //    m.DeleteMe();
            //}
            var units = player.unitsColl.units.counter();
            while (units.Next())
            {
                units.sel.order?.DeleteMe();
            }
            orders.list.Clear();
            //while (orders.list.Count > 0)
            //{
            //    removeUnit(arraylib.Last(orders.list).unit);
            //}
        }

        public List<IntVector2> EnabledTiles()
        {
            List<IntVector2> result = new List<IntVector2>(orders.list.Count);
            for (int i = 0; i < orders.list.Count; ++i)
            {
                if (orders.list[i].CheckList_Enabled)
                {
                    result.Add(orders.list[i].unit.squarePos);
                }
            }
            return result;
        }

        public bool IsEmpty()
        {
            return orders.list.Count == 0;
        }
 
        virtual public void ModifyMoveLength(AbsUnit unit, ref int moveLength)
        {
        }

        virtual public int OrderStartCount { get { return OrderCount(
            CommandType, 
            toggRef.gamestate.gameSetup.armyScale, 
            player.settings.armySetup.race); } }

       
        virtual public bool Complete { get { return ordersLeft <= 0; } }


        //public static void Card(CommandType type, ArmyScale scale, Action<CommandType> callback, VikingEngine.HUD.GuiLayout layout)
        //{
        //    new HUD.GuiIconTextButton(SpriteName.cmdOrderCheckFlat, TextLib.EnumName(type.ToString()),
        //        CommandDesc(type, scale), new HUD.GuiAction1Arg<CommandType>(callback, type), false, layout);
        //}

        public static string Name(CommandType type, ArmyScale scale, ArmyRace race)
        {
            if (type == CommandType.Order_3)
            {
                int count = OrderCount(type, scale, race);
                return "Order " + count.ToString();
            }

            return TextLib.EnumName(type.ToString());
        }

        public static string CommandDesc(CommandType type, ArmyScale scale, ArmyRace race)
        {
            int count = OrderCount(type, scale, race);
            string countTxt = count.ToString();

            switch (type)
            {
                case CommandType.Order_3:
                    return "Order " + countTxt + " units, Move and Attack.";
                case CommandType.Close_encounter:
                    return "Order all units in melee range, +1 Attack dice. No movement.";
                case CommandType.Dark_Sky:
                    return "Order " + countTxt + " archers, +1 Attack dice. No movement.";
                case CommandType.Wide_Advance:
                    return "Order all units. Move 1 step. No attacks. No resting afterwards.";
                case CommandType.DoubleTime:
                    return "Order " + countTxt + " resting " + TextLib.PluralEnding("unit", count) + ", Move and Attack";
                case CommandType.Rush:
                    return "Order " + countTxt + " units, +1 Move length. No attacks.";
                default:
                    return "ERR commandDesc " + type.ToString();
            }
        }

        static int OrderCount(CommandType type, ArmyScale scale, ArmyRace race)
        {
            int result;

            switch (type)
            {
                default:
                    result = -1;
                    break;
                case CommandType.Order_3:
                    result = 3;
                    if (race == ArmyRace.Orc)
                    {
                        result += 1;
                    }
                    break;
                case CommandType.Dark_Sky:
                    result = 4;
                    if (race == ArmyRace.Orc)
                    {
                        result += 1;
                    }
                    break;
                case CommandType.Rush:
                    result = 5;
                    if (race == ArmyRace.Orc)
                    {
                        result += 1;
                    }
                    break;
                case CommandType.DoubleTime:
                    result = 1;
                    break;
            }

            if (scale == ArmyScale.Double)
            {
                result *= 2;
            }

            return result;
        }

        public static SpriteName CardImage(CommandType type)
        {
            SpriteName tname = SpriteName.NO_IMAGE;
            switch (type)
            {
                case CommandCard.CommandType.Close_encounter:
                    tname = SpriteName.cmdStrategyCloseEncount;
                    break;
                case CommandCard.CommandType.Dark_Sky:
                    tname = SpriteName.cmdStrategyDarkSky;
                    break;
                case CommandCard.CommandType.DoubleTime:
                    tname = SpriteName.cmdStrategyDoubletime;
                    break;
                case CommandCard.CommandType.Order_3:
                    tname = SpriteName.cmdStrategyOrder3;
                    break;
                case CommandCard.CommandType.Rush:
                    tname = SpriteName.cmdStrategyRush;
                    break;
                case CommandCard.CommandType.Wide_Advance:
                    tname = SpriteName.cmdStrategyWideAdv;
                    break;
            }

            return tname;
        }

        public static void HasPhase(CommandType type, out bool hasMovePhase, out bool hasAttackPhase)
        {
            hasMovePhase = true; hasAttackPhase = true;

            switch (type)
            {
                case CommandType.Dark_Sky:
                case CommandType.Close_encounter:
                    hasMovePhase = false;
                    break;

                case CommandType.Rush:
                case CommandType.Wide_Advance:
                    hasAttackPhase = false;
                    break;
            }
        }

        public static AbsCommandCard Get(CommandType type)
        {
            AbsCommandCard card;

            switch (type)
            {
                case CommandType.Order_3:
                    card = new Command_Order3();
                    break;
                case CommandType.Close_encounter:
                    card = new Command_CloseEncounter();
                    
                    break;
                case CommandType.Dark_Sky:
                    card = new Command_DarkSky();
                    break;
                case CommandType.Wide_Advance:
                    card = new Command_WideAdvance();
                    break;
                case CommandType.DoubleTime:
                    card = new Command_DoubleTime();
                    break;
                case CommandType.Rush:
                    card = new Command_Rush();
                    break;
                default:

                    throw new NotImplementedException();
            }

            return card;
        }

    abstract public CommandType CommandType { get; }
        abstract public bool HasOrderCount { get; }
    }

    enum CommandType
    {
        Order_3,
        Close_encounter, //all in cc may attack, has cooldown
        Dark_Sky, //3 archers, +1 attack
        Wide_Advance, //move 8 units, 1 step
        DoubleTime, //Order one unit that is resting
        Rush,
        NUM_NONE,
    }
}
