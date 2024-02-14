//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////xna

//namespace VikingEngine.Commander
//{
//    class NetActionStartPhase_Host : AbsNetActionHost
//    {
//        public NetActionStartPhase_Host(Players.RemotePlayer p, GamePhaseType phase)
//            :base()
//        {
//            p.pData.writeGlobalIndex(writer);//writePlayerNumber(writer);
//            writer.Write((byte)phase);
//            Send();
//        }

//        protected override NetActionType Type
//        {
//            get { return NetActionType.StartPhase; }
//        }
//    }

//    class NetActionStartPhase_Client : AbsNetActionClient
//    {
//        public NetActionStartPhase_Client(Network.ReceivedPacket packet)
//            : base(packet.r)
//        {
            
//        }

//        public override bool Update()
//        {
//            var p = cmdRef.gamestate.players.readGenericPlayer(reader);
//            cmdRef.gamestate.players.allPlayers.selectedIndex = p.pData.globalPlayerIndex;
//            ((Players.AbsPlayer)p).StartPhase((GamePhaseType)reader.ReadByte());

//            cmdRef.gamestate.playersStatusHUD.PlayerTurn(cmdRef.gamestate.players.allPlayers.selectedIndex);
//            return true;
//        }
//    }
//}
