using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Network
{
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

        public byte Id { get { return id; } }
        public ulong FullId { get { return fullId; } }
        public float SendTime { get { return roundTripTime * 0.5f; } }
        abstract public string Gamertag { get; }
        abstract public bool IsLocal { get; }

        public bool IsRemote => !IsLocal;

        abstract public bool Connected { get; }

        virtual public bool IsInstance => false;

        public bool GotAssignedId => id != byte.MaxValue;
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
            name = SaveLib.ReadString(r);
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
