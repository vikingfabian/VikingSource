using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Players
{
    class SouthHaraStartAi : AbsInGameUpdateable
    {
        Faction faction;
        Time nextCheck = new Time(8, TimeUnit.Seconds);
        bool messageDone = false;

        public SouthHaraStartAi(Faction faction) 
            :base(true)
        { 
            this.faction = faction;        
        }

        public override void Time_Update(float time_ms)
        {
            if (nextCheck.CountDown(time_ms))
            {
                nextCheck.Seconds = 2;

                if (faction.HasZeroUnits())
                {
                    DeleteMe();
                }

                var armiesC = faction.armies.counter();
                while (armiesC.Next())
                {
                    if (!messageDone &&
                            armiesC.sel.ai.walkGoal.SideLength(armiesC.sel.tilePos) <= 80)
                    {                       
                        foreach (var p in DssRef.state.localPlayers)
                        {
                            p.hud.messages.Add("Enemy approaching!", "Hara merchenaries has been spotted in the south");
                        }
                        messageDone = true;
                    }

                    if (armiesC.sel.InBattle() ||
                        armiesC.sel.ai.IdleObjetive())
                    {
                        complete();
                    }
                }
            }
        }

        void complete()
        {
            faction.player.GetAiPlayer().nextDecisionTimer.Seconds = 5;
            faction.gold = faction.armyUpkeep * 60 * 20;
            faction.hasDeserters = true;
            DeleteMe();
        }
    }
}
