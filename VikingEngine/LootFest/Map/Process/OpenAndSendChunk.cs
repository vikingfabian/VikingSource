using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Map.Process
{
    class NetLoadAndSendChunk
    {
        IntVector2 chunk;
        public NetLoadAndSendChunk(IntVector2 chunk)
        {
            this.chunk = chunk;

            IntVector2 localPos;
            var level = LfRef.levels2.GetLevelUnsafe(chunk);

            if (level != null)
            {

                BlockMap.DesignAreaStorage area = level.designAreas.getArea(chunk, out localPos);
                var path = area.ChunkPath(chunk);

                new DataStream.OpenAndSendFile(
                    path, Network.PacketType.SendChunk, writePrefix, Network.SendPacketTo.All, 0,
                        Network.PacketReliability.ReliableLasy, LoadAndSendChunkFailure);
            }
        }

        void writePrefix(System.IO.BinaryWriter w)
        {
            Map.WorldPosition.WriteChunkGrindex_Static(chunk, w);
        }

        void LoadAndSendChunkFailure()
        {
            LfRef.gamestate.ChunkIsMissingFile(chunk);
        }
    }
}
