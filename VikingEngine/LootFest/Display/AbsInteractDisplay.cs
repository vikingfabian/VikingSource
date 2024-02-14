using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.Display
{
    abstract class AbsInteractDisplay
    {
        protected bool inputToRemove_notTimed = false;

        protected Time time;
        protected const float MinDisplayTimeSec = 0.6f;
        Time minViewTime_ForInput = new Time(100);
        public Players.Player player;
        Vector3 startPos;

        public AbsInteractDisplay()
        { }
        public AbsInteractDisplay(Players.Player p)
        {
            this.player = p;
            this.startPos = p.hero.Position;
            p.deleteInteractDisplay();
            p.interactDisplay = this;
        }

        virtual public void refresh(Players.Player player, object sender)
        { 
            time.Seconds = MinDisplayTimeSec;
        }

        /// <returns>Remove me</returns>
        virtual public bool Update()
        {
            if (inputToRemove_notTimed)
            {
                if (minViewTime_ForInput.CountDown())
                {
                    if ((startPos - player.hero.Position).Length() > 6 ||
                        player.inputMap.interact.DownEvent ||//.DownEvent(VikingEngine.Input.ButtonActionType.GameInteract) ||
                        player.inMenu)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (time.CountDown())
                {
                    DeleteMe();
                    return true;
                }
            }
            return false;
        }

        virtual public bool overrideInteractInput { get { return inputToRemove_notTimed; } }

        abstract public void DeleteMe();
    }
}
