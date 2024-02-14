using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest
{
    class SendHostedGameObjects : Timer.AbsTimer
    {
        Network.SendPacketToOptions toGamer;
        Director.GameObjCollection objColl;

        public SendHostedGameObjects(Network.SendPacketToOptions toGamer, Director.GameObjCollection objColl)
            :base(2000, UpdateType.Lazy)
        {
            this.toGamer = toGamer;
            this.objColl = objColl;
        }
        protected override void timeTrigger()
        {
            ISpottedArrayCounter<GO.AbsUpdateObj> active = new SpottedArrayCounter<GO.AbsUpdateObj>(objColl.LocalMembers);
            while (active.Next())
            {
                active.GetSelection.NetworkShareObject(toGamer);
            }
        }
    }
}
