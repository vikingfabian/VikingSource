using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Network;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Net
{
    interface INetRequestReciever
    {
        void NetRequestCallback(bool successful);
    }

    abstract class AbsNetRequest
    {
        public ushort id;
        public INetRequestReciever callback;
        System.IO.BinaryReader reader;
        protected IntVector2 requestPos = IntVector2.NegativeOne;

        public AbsNetRequest()
        {
        }

        protected void beginRequest()
        {
            bool availableNow = false;

            if (hqRef.netManager.host)
            {
                //do immedietly
                availableNow = true;
                requestCallback(true);
            }
            else
            {
                hqRef.netManager.waitingRequests.members.Add(this);
            }

            id = hqRef.netManager.nextRequestId++;

            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqNetRequest,
                Network.PacketReliability.Reliable, null);
            w.Write((byte)Type);
            w.Write(id);
            w.Write(availableNow);
            toggRef.board.WritePosition(w, requestPos);

            write(w);
            writeSuccessfulAction(w);
        }

        public static void ReadNetRequest(ReceivedPacket packet, RemotePlayer remotePlayer)
        {
            NetRequestType type = (NetRequestType)packet.r.ReadByte();
            ushort id = packet.r.ReadUInt16();
            var request = AbsNetRequest.GetType(type);
            request.id = id;
            bool availableNow = packet.r.ReadBoolean();
            request.requestPos = toggRef.board.ReadPosition(packet.r);

            request.read(packet.r);

            request.storeReader(packet);

            if (availableNow)
            {//Sent by host
                request.requestCallback(true);
            }
            else if (hqRef.netManager.host)
            {
                bool available = request.isAvailable();
                if (available)
                {
                    request.requestCallback(available);
                }

                var w = Ref.netSession.BeginWritingPacket(PacketType.hqNetRequestCallback, PacketReliability.Reliable);
                hqRef.players.netWritePlayer(w, remotePlayer);
                w.Write(id);
                w.Write(available);
            }
            //else
            //{
            //    //Third party reciever
            //    remotePlayer.waitingRequests.debugDelayedAdd(request);
            //}
        }

        public void storeReader(Network.ReceivedPacket packet)
        {
            reader = new DataStream.MemoryStreamHandler().CloneReader(packet.r);
        }

        virtual public void requestCallback(bool successful)
        {
            callback?.NetRequestCallback(successful);

            if (successful && reader != null)
            {
                readSuccessfulAction(reader);
            }
        }

        abstract public bool isAvailable();

        abstract public void write(System.IO.BinaryWriter w);
        abstract public void read(System.IO.BinaryReader r);

        abstract public void writeSuccessfulAction(System.IO.BinaryWriter w);
        abstract public void readSuccessfulAction(System.IO.BinaryReader r);

        abstract protected NetRequestType Type { get; }

        public static AbsNetRequest GetType(NetRequestType type)
        {
            if (type == NetRequestType.SpawnUnit)
            {
                return new SpawnUnitRequest();
            }
            else
            {
                return new SpawnTileObjRequest();
            }
        }
    }

    enum NetRequestType
    {
        SpawnUnit,
        SpawnTileObject,
    }
}
