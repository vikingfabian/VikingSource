using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Commander.Players;

namespace VikingEngine.ToGG.ToggEngine.Data
{
    struct TurnCounter
    {
        public static readonly TurnCounter None = new TurnCounter();

        bool activated;
        public int timeStamp;

        public void SetNow(AbsGenericPlayer player)
        {
            activated = true;
            timeStamp = player.TurnsCount;
        }

        public void SetTurnsAgo(int turnsAgo, AbsGenericPlayer player)
        {
            activated = true;
            timeStamp = player.TurnsCount - turnsAgo;
        }

        public int Count(AbsGenericPlayer player)
        {
            if (activated)
            {
                return player.TurnsCount - timeStamp;
            }
            return int.MaxValue;
        }

        public bool HasPassed(int turns, AbsGenericPlayer player)
        {
            return Count(player) >= turns;
        }

        public override string ToString()
        {
            return "Turn Count (time stamp: " + timeStamp + ", active: " + activated.ToString() + ")";
        }
    }

    struct CooldownCounter
    {
        public static readonly CooldownCounter NoCoolDown = new CooldownCounter(0);

        public int cooldownTurns;
        int currentTurnsLeft;
        //TurnCounter counter;

        public CooldownCounter(int cooldownTurns)
        {
            this.cooldownTurns = cooldownTurns;
            currentTurnsLeft = 0;
        }

        public void SetNow()
        {
            currentTurnsLeft = cooldownTurns + 1;
            //counter.SetTurnsAgo(cooldownTurns, player);
        }

        public void SetReady()
        {
            currentTurnsLeft = 0;
            //counter.SetNow(player);
            //counter = TurnCounter.None;
        }

        public bool IsReady()
        {
            return cooldownTurns == 0 || currentTurnsLeft <= 0;

            //if (cooldownTurns == 0)
            //{
            //    return true;
            //}
            //return counter.HasPassed(cooldownTurns, player);
        }

        int CurrentCount => cooldownTurns - currentTurnsLeft;

        public void CountDown()
        {
            if (currentTurnsLeft > 0)
            {
                --currentTurnsLeft;
            }
        }

        //public string BarText(AbsGenericPlayer player)
        //{
        //    return counter.Count(player).ToString() + "/" + cooldownTurns.ToString();
        //}
        public ValueBar ValueBar()
        {
            return new ValueBar(CurrentCount, cooldownTurns);
        }

        public bool HasCooldown => cooldownTurns > 0;
    }
}
