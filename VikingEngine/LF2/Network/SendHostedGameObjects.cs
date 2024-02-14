using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2
{
    class SendHostedGameObjects : Timer.AbsTimer
    {
        Network.SendPacketToOptions toGamer;
        Director.GameObjCollection objColl;

        public SendHostedGameObjects(Network.SendPacketToOptions toGamer, Director.GameObjCollection objColl)
            :base(2000, UpdateType.Lasy)
        {
            this.toGamer = toGamer;
            this.objColl = objColl;
        }
        protected override void timeTrigger()
        {
            ISpottedArrayCounter<GameObjects.AbsUpdateObj> active = new SpottedArrayCounter<GameObjects.AbsUpdateObj>(objColl.LocalMembers);
            while (active.Next())
            {
                active.GetMember.NetworkShareObject(toGamer);
            }
        }
    }
}
