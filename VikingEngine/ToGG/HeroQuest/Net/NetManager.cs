using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.HeroQuest.Net;
using VikingEngine.Network;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest
{
    class NetManager
    {
        public bool host;
        public ushort nextRequestId = 0;
        public Net.WaitingRequests waitingRequests = new Net.WaitingRequests();

        public NetManager(bool host)
        {
            hqRef.netManager = this;
            this.host = host;
        }

        public void writeMoveUnit(AbsUnit u, bool dataOnly)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqMoveUnit, Network.PacketReliability.Reliable);
            u.writeIndex(w);
            w.Write(dataOnly);
            toggRef.board.WritePosition(w, u.squarePos);
        }

        void readMoveUnit(ReceivedPacket packet)
        {
            var u = hqRef.gamestate.GetUnit(packet.r);
            if (u != null)
            {
                bool dataOnly = packet.r.ReadBoolean();
                IntVector2 toPos = toggRef.board.ReadPosition(packet.r);

                IntVector2 prev = u.squarePos;

                if (dataOnly)
                {
                    u.SetDataPosition(toPos);
                }
                else
                {
                    u.SetPosition(toPos);
                    historyAdd(packet.sender.Gamertag + ": Moved " + u.ToString() +
                        prev.ToString() + "-" + toPos.ToString());
                }

                hqRef.events.SendToAll(ToGG.Data.EventType.OtherUnitMoved, u);
            }
        }

        public void read(ReceivedPacket packet, RemotePlayer remotePlayer)
        {
            switch (packet.type)
            {
                case PacketType.hqMoveUnit:
                    readMoveUnit(packet);
                    break;
                case PacketType.toggUnitVisualPos:
                    {
                        var u = hqRef.gamestate.GetUnit(packet.r);
                        if (u != null)
                        {
                            IntVector2 toPos = toggRef.board.ReadPosition(packet.r);
                            u.SetVisualPosition(toPos);
                        }
                    }
                    break;

                //case PacketType.hqTempAnimationPos:
                //    {
                //        var u = hqRef.gamestate.GetUnit(packet.r);
                //        if (u != null)
                //        {
                //            IntVector2 toPos = toggRef.board.ReadPosition(packet.r);
                //            u.se(toPos);
                //        }
                //    }
                //    break;

                case PacketType.hqNetRequest:
                    AbsNetRequest.ReadNetRequest(packet, remotePlayer);
                    break;

                case PacketType.hqNetRequestCallback:
                    readNetRequestCallback(packet);
                    break;

                case PacketType.hqRequestEndTurn:
                    readEndTurnReq(packet);
                    break;

                case PacketType.hqRestartUnit:
                    {
                        var u = Unit.NetReadUnitId(packet.r);
                        if (u != null)
                        {
                            u.netReadStatus(packet.r);
                            IntVector2 toPos = toggRef.board.ReadPosition(packet.r);
                            u.SetPosition(toPos);
                            u.soldierModel.Visible = true;
                        }
                    }
                    break;
                case PacketType.hqTileObjRemove:
                    {
                        var obj =  ToggEngine.TileObjLib.readTileObj(packet.r);
                        obj?.DeleteMe();
                    }
                    break;
                case PacketType.hqAllyUnitsSetup:
                    new AllyUnitsSetup().read(packet.r);
                    break;
                case PacketType.hqCreateUnit:
                    Unit.ReadCreate(packet.r);
                    break;
            }
        }

        

        void readNetRequestCallback(ReceivedPacket packet)
        {
            var requestingPlayer = hqRef.players.netReadPlayer(packet.r);
            ushort id = packet.r.ReadUInt16();
            bool available = packet.r.ReadBoolean();

            Debug.Log("REQ:: request callback " + id.ToString() + ", available " + available.ToString());

            if (requestingPlayer is LocalPlayer)
            {
                AbsNetRequest request;
                if (waitingRequests.TryPull(id, out request))
                {
                    request.requestCallback(available);
                }
                else
                {
                    throw new NetworkException("Missing local request id" + id.ToString(), id);
                }
            }
            else
            {
                var remote = requestingPlayer as RemotePlayer;

                AbsNetRequest request;
                if (remote.waitingRequests.TryPull(id, out request))
                {
                    Debug.Log("REQ:: callback found waiting request " + request.id.ToString());
                    request.requestCallback(available);
                }
                else
                {
                    //store the callback
                    Debug.Log("REQ:: adding callback " + id.ToString());
                    remote.waitingRequests.callbacks.Add(new NetRequestCallback(id, available));
                }
            }
        }

        void writeEndTurnReq(AbsHQPlayer p, bool end, bool confirmed)
        {
            var w = Ref.netSession.BeginWritingPacket(PacketType.hqRequestEndTurn, PacketReliability.Reliable);
            hqRef.players.netWritePlayer(w, p);
            w.Write(end);
            w.Write(confirmed);
        }
        void readEndTurnReq(ReceivedPacket packet) 
        {
            var p = hqRef.players.netReadPlayer(packet.r);
            bool end = packet.r.ReadBoolean();
            bool confirmed = packet.r.ReadBoolean();

            if (confirmed)
            {
                p.readyToEndTurn_Confirmed = end;
            }
            else if (host)
            {
                p.readyToEndTurn_Confirmed = end;

                writeEndTurnReq(p, end, true);
            }

            hqRef.players.localHost.hud.refreshRemoteEndTurn();

            checkEndTurn();
        }

        public void requestEndTurn()
        {
            var p = hqRef.players.localHost;
            bool confirmed = host || p.readyToEndTurn_Request;
            
            if (confirmed)
            {
                p.readyToEndTurn_Confirmed = p.readyToEndTurn_Request;
            }
            writeEndTurnReq(p, p.readyToEndTurn_Request, confirmed);

            checkEndTurn();
        }

        void checkEndTurn()
        {
            if (host)
            {
                var allP = hqRef.players.allPlayersCounter;
                allP.Reset();
                while (allP.Next())
                {
                    if (allP.sel.IsHero && !allP.sel.readyToEndTurn_Confirmed)
                    {
                        return;
                    }
                }

                //All players are ready
                if (Ref.gamestate.UpdateCount - hqRef.players.localHost.startTurnTime < 10)
                {
                    lib.DoNothing();
                }
                new QueAction.QueActionEndTurn(false);
            }
        }

        public static void WriteHealth(System.IO.BinaryWriter w, int health)
        {
            w.Write((byte)health);
        }

        public static int ReadHealth(System.IO.BinaryReader r)
        {
            return r.ReadByte();
        }

        public void historyAdd(string message)
        {
            if (toggLib.ViewDebugInfo)
            {
                hqRef.players.localHost.hud.historyDisplay.add(message);
            }
        }
    }
}
