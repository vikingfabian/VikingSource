using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Net
{
    class AllyUnitsSetup
    {
        System.IO.BinaryWriter w;
        int nextIndex;

        public void begin()
        {
            w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqAllyUnitsSetup, 
                Network.PacketReliability.Reliable);

            nextIndex = toggRef.board.metaData.AllyUnitsStartIndex();
        }

        public Unit nextUnit(HqUnitType unitType, Players.AbsHQPlayer player)
        {
            IntVector2 pos = toggRef.board.metaData.getEntrance(nextIndex++);

            w.Write(true);

            w.Write((byte)unitType);
            hqRef.players.netWritePlayer(w, player);
            toggRef.board.WritePosition(w, pos);

            return createUnit(unitType, player, pos);
        }

        public void read(System.IO.BinaryReader r)
        {
            while (r.ReadBoolean())
            {
                HqUnitType unitType = (HqUnitType)r.ReadByte();
                var player = hqRef.players.netReadPlayer(r);
                IntVector2 pos = toggRef.board.ReadPosition(r);

                createUnit(unitType, player, pos);
            }
        }

        Unit createUnit(HqUnitType unitType, Players.AbsHQPlayer player, IntVector2 pos)
        {
            if (player.LocalHumanPlayer)
            {
                var unit = new Unit(pos, unitType, player);
                unit.netSendCreate();
                return unit;
            }

            return null;
        }

        public void end()
        {
            w.Write(false);
        }        
    }
}
