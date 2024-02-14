//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////xna

//namespace VikingEngine.Commander
//{
//    abstract class AbsNetActionHost
//    {
//        public System.IO.BinaryWriter writer;

//        public AbsNetActionHost()
//        {
//            init();
//        }
//        public AbsNetActionHost(System.IO.BinaryWriter writer) 
//        {
//            if (writer == null)
//            {
//                init();
//            }
//            else
//            {
//                this.writer = writer;
//            }
//        }

//        void init()
//        {
//            //writer = Ref.netSession.BeginAsynchPacket();
//            //writer.Write((byte)Type);
//        }

//        virtual public void Send()
//        {
//            //Ref.netSession.WritePacket(writer, Network.PacketType.cmdNetAction, Network.PacketReliability.Reliable);
//        }

//        abstract protected NetActionType Type { get; }
//    }

//    enum NetActionType
//    {
//        Attack,
//        Movement,
//        EndTurn,
//        StartPhase,
//        FollowUp,
//    }
//}
