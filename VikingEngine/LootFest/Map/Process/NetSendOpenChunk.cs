using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Map.Process
{
    class NetSendOpenChunk : IQuedObject
    {
        System.IO.BinaryWriter w;
        Chunk chunk;
        
        public NetSendOpenChunk(Chunk chunk)
        {
            this.chunk = chunk;
            TaskExt.AddTask(this);//Ref.asynchUpdate.AddThreadQueObj(this);
        }

        public void runQuedTask(MultiThreadType threadType)
        {
            SteamWrapping.SteamWriter packet;
            w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.SendChunk, Network.PacketReliability.Reliable, out packet);//.BeginAsynchPacket();
            
            chunk.netWrite(w);

            packet.EndWrite_Asynch();
            //new Network.UnthreadSystem.IO.BinaryWriter(w, Network.PacketType.SendChunk,
            //    Network.SendPacketTo.All, 0, Network.PacketReliability.Reliable, null); 

        }
        
    }
}
