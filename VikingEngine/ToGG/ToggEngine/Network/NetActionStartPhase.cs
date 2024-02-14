using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//xna

namespace VikingEngine.ToGG
{
    class NetActionStartPhase_Host : AbsNetActionHost
    {
        public NetActionStartPhase_Host(Commander.Players.RemotePlayer p, GamePhaseType phase)
            :base()
        {
            p.pData.writeGlobalIndex(writer);//writePlayerNumber(writer);
            writer.Write((byte)phase);
            Send();
        }

        protected override NetActionType Type
        {
            get { return NetActionType.StartPhase; }
        }
    }

    class NetActionStartPhase_Client : AbsNetActionClient
    {
        public NetActionStartPhase_Client(Network.ReceivedPacket packet)
            : base(packet.r)
        {
            
        }

        public override bool Update()
        {
            var p = Commander.cmdRef.players.readGenericPlayer(reader);
            Commander.cmdRef.players.allPlayers.selectedIndex = p.pData.globalPlayerIndex;
            ((Commander.Players.AbsCmdPlayer)p).StartPhase((GamePhaseType)reader.ReadByte());

            Commander.cmdRef.gamestate.playersStatusHUD.PlayerTurn(Commander.cmdRef.players.allPlayers.selectedIndex);
            return true;
        }
    }
}
