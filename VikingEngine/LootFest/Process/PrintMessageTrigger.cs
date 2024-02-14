using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Process
{
    class PrintMessageTrigger : OneTimeTrigger
    {
        string message;
        public PrintMessageTrigger(string message)
            :base(true)
        {
            this.message = message;
        }
        public override void Time_Update(float time)
        {
            LfRef.gamestate.LocalHostingPlayerPrint(message);
        }
    }
}
