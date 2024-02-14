using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//xna

//namespace VikingEngine.Commander
//{
//    class NetActionFollowUp_Host: AbsNetActionHost
//    {
//        public NetActionFollowUp_Host(Unit unit, IntVector2 toPos)
//            :base()
//        {
//            unit.writeIndex(writer);
//            toPos.WriteByteStream(writer);
//            Send();
//        }

//        protected override NetActionType Type
//        {
//            get { return NetActionType.FollowUp; }
//        }
//    }

//    class NetActionFollowUp_Client : AbsNetActionClient
//    {
//        public NetActionFollowUp_Client(Network.ReceivedPacket packet)
//            : base(packet.r)
//        {
            
//        }

//        public override bool Update()
//        {
//            Unit unit = cmdRef.gamestate.GetUnit(reader);
//            IntVector2 toPos = IntVector2.FromByteStream(reader);
//            unit.SlideToSquare(toPos, true);
//            return true;
//        }
//    }
//}

