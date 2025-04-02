using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.CutScene;

namespace VikingEngine.DSSWars
{
    class GameEventsDemo : GameEvents
    {
        Time maxDemoTime = new Time(90f, TimeUnit.Minutes);

        public override void asyncUpdate(float time)
        {
            base.asyncUpdate(time);
            if (maxDemoTime.CountDownGameTime_IfActive())
            {
                Ref.update.AddSyncAction(new SyncAction(onDemoTimeUp));
            }
        }

        void onDemoTimeUp()
        {
            DssRef.state.localPlayers.First().hud.messages.Add(".Times' up!", ".The demo will end in one minute");
            new Timer.TimedAction0ArgTrigger(viewTimesUpScreen, TimeExt.MinutesToMS(1f));
        }

        void viewTimesUpScreen()
        {
            new EndScene(GameEndReason.TimesUp, false);
        }
    }
}
