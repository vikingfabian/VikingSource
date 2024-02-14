using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;
//xna

namespace VikingEngine.ToGG
{
    class NetActionFollowUp_Host: AbsNetActionHost
    {
        public NetActionFollowUp_Host(AbsUnit unit, IntVector2 toPos)
            :base()
        {
            unit.writeIndex(writer);
            toPos.writeByte(writer);
            Send();
        }

        protected override NetActionType Type
        {
            get { return NetActionType.FollowUp; }
        }
    }

    class NetActionFollowUp_Client : AbsNetActionClient
    {
        public NetActionFollowUp_Client(Network.ReceivedPacket packet)
            : base(packet.r)
        {
            
        }

        public override bool Update()
        {
            AbsUnit unit = toggRef.gamestate.GetUnit(reader);
            IntVector2 toPos = IntVector2.FromReadByte(reader);
            unit.SlideToSquare(toPos, true);
            return true;
        }
    }
}

