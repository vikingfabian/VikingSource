using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars;

namespace VikingEngine.Network
{
    /// <summary>
    /// One of potiensially multiple people sharing a computer 
    /// </summary>
    class NetworkInstancePeer
    { 
        public AbsNetworkPeer peer;
        public int SplitScreenIndex = 0;

        /// <summary>
        /// player class object
        /// </summary>
        public object Tag = null;

        public NetworkInstancePeer(AbsNetworkPeer peer, int splitScreenIndex)
        { 
            this.peer = peer;
            this.SplitScreenIndex = splitScreenIndex;
        }

        public void writeNetID(System.IO.BinaryWriter w)
        {
            w.Write(peer.fullId);
            w.Write((byte)SplitScreenIndex);
        }
        public static void ReadNetID(System.IO.BinaryReader r, out AbsNetworkPeer peer, out int SplitScreenIndex)
        {
            ulong peerId = r.ReadUInt64();
            SplitScreenIndex = r.ReadByte();

            peer = Ref.netSession.GetPeer(peerId);

            if (peer == null)
            {
                peer = new PlaceHolderPeer(peerId);
            }
        }        
    }

    abstract class AbsNetworkPeer
    {
        public byte id = byte.MaxValue;
        public ulong fullId;

        public float roundTripTime = 0;
        public int localGamersCount = 1;
        public float lastHeardFrom;// = Ref.TotalTimeSec;

        public bool mapLoadedAndReady = false;
        public bool approved = false;
        public Microsoft.Xna.Framework.Graphics.Texture2D storedGamerIcon = null;
        public NetworkInstancePeer[] instancePeers = null;
        //public bool placeHolder = false;
        /// <summary>
        /// player class object
        /// </summary>
        public object Tag = null;

        //abstract public void kickFromNetwork();

        public override bool Equals(object obj)
        {
            return ((AbsNetworkPeer)obj).Id == Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public override string ToString()
        {
            return Gamertag + (IsLocal ? "(L)" : "(R)") + ": Id(" + id.ToString() + "), Tag(" + TextLib.ToString_Safe(Tag) + ")";
        }

        public void initInstancePeers()
        {
            instancePeers = new NetworkInstancePeer[localGamersCount];
            for (int i = 0; i < localGamersCount; i++)
            {
                instancePeers[i] = new NetworkInstancePeer(this, i);
            }
        }

        public byte Id { get { return id; } }
        public ulong FullId { get { return fullId; } }
        public float SendTime { get { return roundTripTime * 0.5f; } }
        abstract public string Gamertag { get; }
        abstract public bool IsLocal { get; }

        public bool IsRemote => !IsLocal;

        abstract public bool Connected { get; }

        virtual public bool IsInstance => false;

        public bool GotAssignedId => id != byte.MaxValue;

        virtual public bool IsPlaceHolder => false;
    }

    class PlaceHolderPeer : AbsNetworkPeer
    {
        public PlaceHolderPeer(ulong id)
        { 
            this.fullId = id;
        }

        public override string Gamertag => TextLib.Error;

        public override bool IsLocal => false;

        public bool IsRemote => true;

        override public bool Connected => false;

        public override bool IsPlaceHolder => true;

    }

    class OfflinePeer: AbsNetworkPeer
    {
        public override string Gamertag => "Player"; //+ TextLib.IndexToString(id);

        public override bool IsLocal => true;
        public override bool Connected => false;
    }

    class LocalInstancePeer : AbsNetworkPeer
    {
        AbsNetworkPeer localPeer;
        int index;

        public LocalInstancePeer(AbsNetworkPeer localPeer, int index)
        {
            this.localPeer = localPeer;
            this.index = index;
        }

        public override string Gamertag => localPeer.Gamertag + TextLib.Parentheses(TextLib.IndexToString(index));

        public override bool Connected => localPeer.Connected;

        public override bool IsLocal => true;

        public override bool IsInstance => true;
    }

    class StoredPeer : AbsNetworkPeer
    {
        string name;

        public StoredPeer(AbsNetworkPeer original)
        {
            this.fullId = original.fullId;
            name = original.Gamertag;
        }

        public StoredPeer(System.IO.BinaryReader r)
        {
            fullId = r.ReadUInt64();
            name = SaveLib.ReadString_safe(r);
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(fullId);
            SaveLib.WriteString(w, name);
        }

        public override bool Equals(object obj)
        {
            return ((AbsNetworkPeer)obj).fullId == fullId;
        }

        public override string Gamertag => name;

        public override bool IsLocal => false;
        public override bool Connected => false;

    }
}
