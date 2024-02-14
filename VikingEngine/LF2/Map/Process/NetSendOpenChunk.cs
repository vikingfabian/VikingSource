using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Map.Process
{
    class NetSendOpenChunk : QueAndSynch
    {
        System.IO.BinaryWriter chunkWriter;
        Network.PacketReliability relyable;
        Chunk s;
        bool toHost;
        public NetSendOpenChunk(Chunk s, Network.PacketReliability relyable, bool toHost)
            :base(false)
        {
            this.toHost = toHost;
            this.s = s;
            this.relyable = relyable;
            start();
        }

        protected override bool quedEvent()
        {
            chunkWriter =
                Ref.netSession.BeginAsynchPacket();
            //s.Index.WriteStream(chunkWriter);
            Map.WorldPosition.WriteChunkGrindex_Static(s.Index, chunkWriter);
            s.WriteChunk(chunkWriter);

            return true;
        }
        public override void Time_Update(float time)
        {
            Ref.netSession.EndAsynchPacket(chunkWriter, Network.PacketType.LF2_SendChunk, 
                toHost? Network.SendPacketTo.Host : Network.SendPacketTo.All, 0, relyable, null); 
               // toHost? Network.SendPacketToOptions.SendToHost :  Network.SendPacketToOptions.SendToAll);
        }
        
    }
}
