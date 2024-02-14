//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////xna

//namespace VikingEngine.Network
//{
//    abstract class ThreadedSystem.IO.BinaryReader : QueAndSynch
//    {
//        System.IO.BinaryReader r;  
//        Network.PacketType type;
//        AbsNetworkPeer sender;

//        public ThreadedSystem.IO.BinaryReader(Network.PacketType type, System.IO.BinaryReader r, AbsNetworkPeer sender)
//            :base(false)
//        {
//            this.r = new  System.IO.BinaryReader(new System.IO.MemoryStream(r.ReadBytes((int)(r.BaseStream.Length - r.BaseStream.Position))));
//            this.r.BaseStream.Position = 0;
//            this.type = type;
//            this.sender = sender;
//        }

//        protected override bool quedEvent()
//        {
//            return ReadPacket(type, r, sender);
//        }
        
//        /// <returns>If it requires a thread synch</returns>
//        abstract protected bool ReadPacket(Network.PacketType type, System.IO.BinaryReader r, AbsNetworkPeer sender);
//    }
//}
