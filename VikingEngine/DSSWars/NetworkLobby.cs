using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DSSWars
{
    class NetworkLobby
    {
        Dictionary<ulong, Member> members = new Dictionary<ulong, Member>();

        public void NetEvent_PeerJoined(Network.AbsNetworkPeer gamer)
        {
            members.Add(gamer.FullId, new Member(gamer));
        }
        public void NetworkReadPacket(Network.ReceivedPacket packet)
        {
            Member m;
            if (members.TryGetValue(packet.sender.FullId, out m))
            {
                switch (packet.type)
                {
                    case Network.PacketType.rtsWantSeed:
                        {
                            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.rtsSeed, Network.PacketReliability.Reliable);
                            w.Write(WorldData.LoadingWorld.seed);
                        }
                        break;
                    case Network.PacketType.rtsMapLoadedAndReady:
                        m.ready = true;
                        break;
                }
            }
        }

        public void NetEvent_PeerLost(Network.AbsNetworkPeer gamer)
        {
            members.Remove(gamer.FullId);
        }

        public string MembersToString()
        {
            if (members.Count == 0)
            {
                return "Zero clients";
            }
            else
            {
                string result = "Joined: ";
                foreach (var kv in members)
                {
                    result += kv.Value.peer.Gamertag + "(" + (kv.Value.ready? "R" : "*") + "), ";
                }

                return result;
            }
        }

        public bool allReady()
        {
            foreach (var kv in members)
            {
                if (kv.Value.ready == false)
                {
                    return false;
                }
            }
            return true;
        }

        //-----------
        class Member
        {
            public Network.AbsNetworkPeer peer;
            public bool ready = false;

            public Member(Network.AbsNetworkPeer peer)
            {
                this.peer = peer;
            }
        }


    }
}
