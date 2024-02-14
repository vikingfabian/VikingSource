using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest
{
    /// <summary>
    /// Sends out chunks to clients
    /// </summary>
    class ChunksDataNetwork
    {
        const int MaxSentCheckSum = 2;
        int sentChunkCheckSum = 0;
        List<IntVector2> requestedChunks = new List<IntVector2>();

        public void NetAddChunkRequest(IntVector2 index)
        {
            if (LfRef.WorldHost)
            {
                if (!requestedChunks.Contains(index))
                {
                    requestedChunks.Add(index);
                }
            }
        }

        public void updateSendChunk()
        {
            if (requestedChunks.Count > 0) //&& Ref.netSession.IsHost)
            {
                if (sentChunkCheckSum <= MaxSentCheckSum)
                {
                    SendChunk(requestedChunks[0]);
                    requestedChunks.RemoveAt(0);
                    sentChunkCheckSum += Ref.netSession.RemoteGamersCount;
                    //return true;
                }
                else
                {
                    sentChunkCheckSum--;
                }
            }
            //return false;
        }

       
        public void SendChunk(IntVector2 chunk)
        {
            Map.Chunk screen = LfRef.chunks.GetScreenUnsafe(chunk);
            if (screen != null && screen.DataGridLoadingComplete)
            {
                System.Diagnostics.Debug.WriteLine("sending open chunk");
                new Map.Process.NetSendOpenChunk(screen);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("sending stored chunk");
                new Map.Process.NetLoadAndSendChunk(chunk);
            }

            //TODO fixa

            //Players.AbsPlayer p = LfRef.worldOverView.ChunkHasOwner(chunk);
            //if (p != null && p.Local)
            //{ //private area
            //    DataStream.MemoryStreamHandler data = p.Settings.GetPrivateAreaData(chunk);
            //    if (data != null)
            //    {
            //        System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.SendChunk, Network.PacketReliability.Reliable);
            //        Map.WorldPosition.WriteChunkGrindex_Static(chunk, w);
            //        data.WriteDataArray(w);
            //    }
            //}
            //else
            //{
            //    Map.Chunk screen = LfRef.chunks.GetScreenUnsafe(chunk);
            //    if (screen != null && screen.DataGridLoadingComplete)
            //    {
            //        System.Diagnostics.Debug.WriteLine("sending open chunk");
            //        new Map.Process.NetSendOpenChunk(screen, relyable, false);
            //    }
            //    else
            //    {
            //        System.Diagnostics.Debug.WriteLine("sending stored chunk");
            //        new Map.Process.NetOpenAndSendChunk(chunk);
            //    }
            //}
        }

        public void netRead(Network.ReceivedPacket packet)
        {
            switch (packet.type)
            {
                case Network.PacketType.RequestChunk:
                    NetAddChunkRequest(Map.WorldPosition.ReadChunkGrindex_Static(packet.r));
                    break;
                case Network.PacketType.RequestChunkGroup:
                    IntVector2 chunk3 = Map.WorldPosition.ReadChunkGrindex_Static(packet.r);
                    IntVector2 pos = IntVector2.Zero;
                    for (pos.Y = -1; pos.Y <= 1; pos.Y++)
                    {
                        for (pos.X = -1; pos.X <= 1; pos.X++)
                        {
                            NetAddChunkRequest(chunk3 + pos);
                        }
                    }
                    break;
                case Network.PacketType.GotChunk:
                    sentChunkCheckSum--;
                    break;
                case Network.PacketType.SendChunk:
                    {
                        IntVector2 chunkIndex = Map.WorldPosition.ReadChunkGrindex_Static(packet.r);
                        //System.Diagnostics.Debug.WriteLine("##Recieve chunk: " + chunk2.ToString());

                        var chunk = LfRef.chunks.GetScreenUnsafe(chunkIndex);
                        if (chunk != null)
                        {
                            //System.Diagnostics.Debug.WriteLine("-Update chunk");
                            //update open chunk
                            chunk.netRead(packet.r);
                        }
                        else
                        {
                            //put chunk data on storage
                            Debug.Log("-Save chunk");
                            Map.Chunk.NetworkSaveChunk(packet.r, chunkIndex);
                            //LfRef.worldOverView.ChangedChunk(chunk2);
                        }
                        //send back a check
                        if (Ref.netSession.IsClient)
                        {
                            var sentCheck = Ref.netSession.BeginWritingPacket(Network.PacketType.GotChunk,
                                Network.PacketReliability.Reliable, LfRef.gamestate.LocalHostingPlayer.PlayerIndex);
                        }
                    }
                    break;
            }
        }

        public void NetEvent_PeerLost()
        {
            sentChunkCheckSum = Bound.Min(sentChunkCheckSum - 3, 0);
        }
    }
}
