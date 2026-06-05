using NVorbis.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using VikingEngine.Network;

namespace VikingEngine.SteamWrapping
{
    /// <summary>
    /// Will auto add to update and keep sending until done
    /// </summary>
    class SteamLargePacketWriter: SteamWriter
    {
        const int SendChunkSize = 1024;

        DataStream.MemoryStreamHandler file;
        int id;
        int nextPacketIndex = 0;
        int packetCount;
        int writerPos = 0;
        PacketType largePacketType;
        bool fileComplete = false;
        TimeStamp sendTime;

        public bool Complete => writerPos >= file.memoryLength;

        public bool TimeOut => sendTime.secPassed(2);

        public SteamLargePacketWriter(DataStream.MemoryStreamHandler file, SendPacketTo To, ulong SpecificGamerID, PacketType type)
        {
            init(PacketReliability.Reliable, false, To, SpecificGamerID);
            this.largePacketType = type;
            this.file = file;
            id = Ref.rnd.Int();
            packetCount = MathExt.Div_Ceiling(file.memoryLength, SendChunkSize);

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
            lockedFromPooling = true;
            sendNext();
        }

        public void sendNext()
        {
            Debug.Log($"Send large {nextPacketIndex}/{packetCount}");

            Task.Factory.StartNew(() =>
            {
                Clear();

                var w = writeHead(PacketType.Steam_LargePacket, null);
                w.Write(id);
                w.Write((byte)largePacketType);
                w.Write((ushort)nextPacketIndex++);
                w.Write((ushort)packetCount);

                file.WritePartialDataToWriter(writerPos, SendChunkSize, w);
                writerPos += SendChunkSize;

                EndWrite_Asynch();
                sendTime = TimeStamp.Now();
            });
        }

        //public override void Time_Update(float time)
        //{
        //    //Not used
        //    base.Time_Update(time);
        //    if (writerPos >= file.memoryLength)
        //    {
        //        Ref.netSession.largePackets.Remove(id);
        //    }
        //}

        public void readNext(Network.ReceivedPacket packet)
        {
            Task.Factory.StartNew(() =>
            {
                largePacketType = (PacketType)packet.r.ReadByte(); ;
                nextPacketIndex = packet.r.ReadUInt16();
                packetCount = packet.r.ReadUInt16();

                Debug.Log($"Recieve large {nextPacketIndex}/{packetCount}");

                fileComplete = file.ReadPartialDataToMemory(packet.r);

                Ref.update.AddSyncAction(new SyncAction(() =>
                {
                    if (fileComplete)
                    {
                        //complete
                        Network.ReceivedPacket largePacket = packet;
                        largePacket.type = largePacketType;
                        largePacket.r = file.GetReader();
                        Ref.NetUpdateReciever().NetEvent_LargePacket(largePacket);

                        Ref.netSession.largePackets.Remove(id);
                    }
                    else
                    {
                        var w = Ref.netSession.BeginWritingPacket(Network.PacketType.Steam_LargePacket_Recieved, Network.PacketReliability.Reliable, SendPacketTo.OneSpecific, packet.sender.fullId,
                             null);
                        w.Write(id);
                    }
                }));
            });            
        }

    }
}
