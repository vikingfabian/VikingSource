using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest
{
    struct NetworkId
    {
        public static NetworkId Empty = new NetworkId();

        static int nextId = 0;
        public int id;
        public byte hostingPlayer;

        public NetworkId(byte hostingPlayer)
        {
            this.hostingPlayer = hostingPlayer;
            id = nextId++;
        }

        public NetworkId(System.IO.BinaryReader r)
            :this()
        {
            read(r);
        }

        public void write(System.IO.BinaryWriter w)
        {
            //if (PlatformSettings.SteamAPI)
            //{
                w.Write(hostingPlayer);
            //}
            //else
            //{
            //    w.Write((byte)hostingPlayer);
            //}
            w.Write(id);
        }
        public void read(System.IO.BinaryReader r)
        {
            //if (PlatformSettings.SteamAPI)
            //{
            //    hostingPlayer = r.ReadByte();
            //}
            //else
            //{
                hostingPlayer = r.ReadByte();
            //}

            id = r.ReadInt32();
        }

        public override int GetHashCode()
        {
            return id + hostingPlayer * 100000;
        }
        public override bool Equals(object obj)
        {
            return obj is NetworkId && (NetworkId)obj == this;
        }
        public static bool operator ==(NetworkId value1, NetworkId value2)
        {
            return value1.hostingPlayer == value2.hostingPlayer && value1.id == value2.id;
        }
        public static bool operator !=(NetworkId value1, NetworkId value2)
        {
            return value1.hostingPlayer != value2.hostingPlayer || value1.id != value2.id;
        }

        public override string ToString()
        {
            return "Net Id(P" + hostingPlayer.ToString() + ", ID" + id.ToString() + ")";
        }
    }
}
