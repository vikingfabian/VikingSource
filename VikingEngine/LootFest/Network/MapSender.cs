//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;
//using VikingEngine.DataLib;

//namespace VikingEngine.LootFest
//{
//    class MapSender : OneTimeQueTrigger2
//    {
//        public const int SendRate = 1000;
        
//        public MapSender()
//            :base(true)
//        {
//        }

//        public override void runQuedTask(MultiThreadType threadType)
//        {
//            IntVector2 pos = IntVector2.Zero;
            
//            LfRef.gamestate.LocalHostingPlayer.Print("Map sending complete");
//            Ref.netSession.BeginWritingPacket(Network.PacketType.SendMapComplete, Network.PacketReliability.ReliableLasy, LfLib.LocalHostIx);
//        }
//    }
//}
