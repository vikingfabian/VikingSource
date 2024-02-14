//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////xna

//namespace VikingEngine.LootFest.Editor
//{
//    class ThreadedClientAction : Network.ThreadedSystem.IO.BinaryReader
//    {
//        LootFest.Map.WPRange updateRange;
//        Players.ClientPlayer cp;
//        public ThreadedClientAction(System.IO.BinaryReader r, VikingEngine.SteamWrapping.SteamNetworkPeer sender, Players.ClientPlayer cp, Network.PacketType type)
//            : base(type, r, sender)
//        {
//            this.cp = cp;
//            start();
//        }
//        protected override bool ReadPacket(Network.PacketType type, System.IO.BinaryReader r, VikingEngine.SteamWrapping.SteamNetworkPeer sender)
//        {
//            //switch (type)
//            //{
//            //    case Network.PacketType.VoxelEditorDottedLine:
//            //        updateRange = VoxelDesigner.NetworkReadDottedLine(r, cp);
//            //        break;
//            //    case Network.PacketType.VoxelEditorDrawRect:
//            //        updateRange = VoxelDesigner.NetworkReadDrawRect(r, cp);
//            //        break;
//            //    case Network.PacketType.VoxelEditorAddTemplate:
//            //        updateRange = VoxelDesigner.NetworkReadTemplate(r, cp);
//            //        break;
//            //}
            
//            return true;
//        }
//        public override void Time_Update(float time)
//        {
//            VoxelDesigner.UpdateMapArea(updateRange.Min, updateRange.Max);
//        }
//    }
    
//}
