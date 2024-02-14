using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Network;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest
{
    class NetLobby2 : VikingEngine.Network.NetLobby
    {
        public List<VikingEngine.LootFest.GO.NPC.LobbyCharacter> lobbies = new List<GO.NPC.LobbyCharacter>();
        JoinNetwork joinNetwork;

        public NetLobby2()
            : base()
        {
            LfRef.net = this;
        }

        public override void update()
        {
            if (joinNetwork != null)
            {
                if (joinNetwork.failTimer.CountDown())
                {
                    joinNetwork.DeleteMe();
                    joinNetwork = null;
                }
            }
            else
            {
                base.update();
            }

        }

        public bool waitingForId;
        public void joinSessionLink(AbsAvailableSession available)
        {
            waitingForId = true;
            searchLobbies = false;
            
            joinNetwork = new JoinNetwork(available);
            LfRef.gamestate.LocalHostingPlayer.CloseMenu();
        }

        public override void NetworkStatusMessage(NetworkStatusMessage message)
        {
            base.NetworkStatusMessage(message);

            if (message == Network.NetworkStatusMessage.Joining_session && joinNetwork != null)
            {
                joinNetwork.FoundSession();
            }
        }

        public override void NetEvent_PeerJoined(Network.AbsNetworkPeer gamer)
        {
            base.NetEvent_PeerJoined(gamer);

            for (int i = lobbies.Count - 1; i >= 0; --i)
            {
                lobbies[i].remove();
            }

            lobbies.Clear();
            searchLobbies = false;
        }

        public override void NetEvent_PeerLost(Network.AbsNetworkPeer gamer)
        {
            base.NetEvent_PeerLost(gamer);

            if (!Ref.netSession.InMultiplayerSession)//steam.P2PManager.remoteGamers.Count == 0)
            {
                searchLobbies = true;

                if (!LfRef.WorldHost)
                {
                    new GameState.DisconnectScreen();
                }
            }
        }

        public override void NetEvent_SessionsFound(
            List<AbsAvailableSession> availableSessions, 
            List<AbsAvailableSession> prevAvailableSessionsList)
        {
            if (availableSessions != null && 
                LfRef.gamestate.LocalHostingPlayer.inEditor == false)
            {
                base.NetEvent_SessionsFound(availableSessions, prevAvailableSessionsList);

                List<AbsAvailableSession> newLobbies, lostLobbies;

                prevAvailableSessionsList = new List<AbsAvailableSession>(lobbies.Count);
                foreach (var m in lobbies)
                {
                    prevAvailableSessionsList.Add(m.lobby);
                }

                filterNewAndOldLobbies(availableSessions, prevAvailableSessionsList, out newLobbies, out lostLobbies);

                if (lostLobbies.Count > 0)
                {
                    for (int i = lobbies.Count - 1; i >= 0; --i)
                    {
                        if (lostLobbies.Contains(lobbies[i].lobby))
                        {
                            lobbies[i].remove();
                        }
                    }
                }

                for (int i = lobbies.Count - 1; i >= 0; --i)
                {
                    if (lobbies[i].IsDeleted)
                    {
                        lobbies.RemoveAt(i);
                    }
                }


                foreach (var m in newLobbies)
                {
                    Vector3 pos = LfRef.gamestate.LocalHostingPlayer.hero.Position;
                    pos.Y += 5;

                    pos += VectorExt.V2toV3XZ(Rotation1D.Random().Direction(16f));

                    lobbies.Add(new VikingEngine.LootFest.GO.NPC.LobbyCharacter(new GO.GoArgs(pos), m));
                }
            }
            
        }
    }
}
