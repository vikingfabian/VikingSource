//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////xna
//using VikingEngine.Commander.CommandCard;

//namespace VikingEngine.Commander
//{
//    class NetActionEndTurn_Host : AbsNetActionHost
//    {
//        public NetActionEndTurn_Host(Players.AbsLocalPlayer player)
//            :base()
//        {
//            player.pData.writeGlobalIndex(writer);//.writePlayerNumber(writer);

//            if (player.commandCard != null)
//            {
//                writer.Write(true);
//                if (player.settings.commandCardDeck.SelectedCommand != CommandType.Wide_Advance)
//                {
//                    foreach (var order in player.commandCard.orders.list)
//                    {
//                        if (order.unit.AttackedThisTurn || order.unit.MovedThisTurn)
//                        {
//                            writer.Write(true);
//                            order.unit.Resting = true;
//                            writer.Write((byte)order.unit.unitIndex);
//                        }
//                    }
//                    writer.Write(false);
//                }
//                else
//                {
//                    writer.Write(byte.MinValue);
//                }
//                player.commandCard.ClearOrders();
//                player.commandCard = null;
//            }
//            else
//            {
//                writer.Write(false);
//            }
//            Send();
//        }

//        protected override NetActionType Type
//        {
//            get { return NetActionType.EndTurn; }
//        }
//    }

//    class NetActionEndTurn_Client : AbsNetActionClient
//    {
//        public NetActionEndTurn_Client(Network.ReceivedPacket packet)
//            : base(packet.r)
//        {
            
//        }

//        public override bool Update()
//        {
//            var player = cmdRef.gamestate.players.readGenericPlayer(reader);
//            if (reader.ReadBoolean())
//            {
//                player.unitsColl.unitsCounter.Reset();
//                while (player.unitsColl.unitsCounter.Next())
//                {
//                    player.unitsColl.unitsCounter.Member.Resting = false;
//                }

//                while (reader.ReadBoolean())
//                {
//                    int unitIx = reader.ReadByte();
//                    if (player.unitsColl.units.Array[unitIx] != null)
//                        player.unitsColl.units.Array[unitIx].Resting = true;
//                }
//            }

//            ((Players.AbsPlayer)player).collectBannerVP();

//            if (Ref.netSession.IsHostOrOffline)
//            {
//                cmdRef.gamestate.BeginNextPlayer();
//            }

//            return true;
//        }
//    }
//}
