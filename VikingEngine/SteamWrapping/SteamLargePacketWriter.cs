using NVorbis.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.Network;

namespace VikingEngine.SteamWrapping
{
    class SteamLargePacketWriter: SteamWriter
    {
        const int SendChunkSize = 600;

        DataStream.MemoryStreamHandler file;
        int id;
        int nextPacketIndex = 0;
        int packetCount;
        int writerPos = 0;
        PacketType largePacketType;

        public SteamLargePacketWriter(DataStream.MemoryStreamHandler file, SendPacketTo To, ulong SpecificGamerID, PacketType type)
            :base(PacketReliability.Reliable, false, To, SpecificGamerID)
        {
            this.largePacketType = type;
            this.file = file;
            id = Ref.rnd.Int();
            packetCount = (int)(file.memoryLength / SendChunkSize);

            Ref.netSession.largePackets.Add(id, this);
        }

        public SteamLargePacketWriter(Network.ReceivedPacket packet, int id)
            : base()
        {
            this.id = id;
            Ref.netSession.largePackets.Add(id, this);
            file = new DataStream.MemoryStreamHandler();
            readNext(packet);
        }

        public void begin()
        {
            sendNext();
        }

        public void sendNext()
        {
            Debug.Log($"Send large {nextPacketIndex}/{packetCount}");

            Task.Factory.StartNew(() =>
            {
                var w = writeHead(PacketType.Steam_LargePacket, null);
                w.Write(id);
                w.Write((byte)largePacketType);
                w.Write((ushort)nextPacketIndex++);
                w.Write((ushort)packetCount);

                file.WritePartialDataToWriter(writerPos, SendChunkSize, w);

                EndWrite_Asynch();
            });
        }

        public void readNext(Network.ReceivedPacket packet)
        {
            Task.Factory.StartNew(() =>
            {
                largePacketType = (PacketType)packet.r.ReadByte(); ;
                nextPacketIndex = packet.r.ReadUInt16();
                packetCount = packet.r.ReadUInt16();

                Debug.Log($"Recieve large {nextPacketIndex}/{packetCount}");

                bool fileComplete = file.ReadPartialDataToMemory(packet.r);

                Ref.update.AddSyncAction(new SyncAction(() =>
                {
                    if (fileComplete)
                    {
                        //complete
                        Network.ReceivedPacket largePacket = packet;
                        largePacket.type = largePacketType;
                        largePacket.r = file.GetReader();
                        Ref.NetUpdateReciever().NetEvent_LargePacket(largePacket);
                    }
                    else
                    {
                        var w = Ref.netSession.BeginWritingPacket(Network.PacketType.Steam_LargePacket_Recieved, SendPacketTo.OneSpecific, packet.sender.fullId,
                            Network.PacketReliability.Reliable, null);
                        w.Write(id);
                    }
                }));
            });            
        }

    }
}
