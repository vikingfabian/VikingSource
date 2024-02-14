using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Process
{
    class NetOpenAndSendChunk
    {
        IntVector2 index;
        public NetOpenAndSendChunk(IntVector2 index)
        {
            this.index = index;
            new DataStream.OpenAndSendFile(
                Chunk.SavePath(index), Network.PacketType.LF2_SendChunk, writePrefix, Network.SendPacketTo.All, 0,
                    Network.PacketReliability.ReliableLasy, LoadAndSendChunkFailure);
        }

        void writePrefix(System.IO.BinaryWriter w)
        {
            Map.WorldPosition.WriteChunkGrindex_Static(index, w);
        }

        void LoadAndSendChunkFailure()
        {
            LfRef.gamestate.ChunkIsMissingFile(index);
        }
    }
}
