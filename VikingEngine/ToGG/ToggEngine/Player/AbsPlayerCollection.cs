using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    abstract class AbsPlayerCollection
    {
        public AbsPlayerCollection()
        {
            toggRef.absPlayers = this;
        }

        abstract public AbsGenericPlayer getGenericPlayer(int ix);
        abstract public int PlayerCount { get; }

        public void WriteUnitSetup(System.IO.BinaryWriter w)
        {
            for (int i = 0; i < PlayerCount; ++i)
            {
                if (getGenericPlayer(i).unitsColl.writeHeader(w))
                {
                    getGenericPlayer(i).unitsColl.WriteUnitSetup(w);
                }
            }
            w.Write(false);
        }

        public void ReadUnitSetup(System.IO.BinaryReader r, DataStream.FileVersion version)
        {           
            int playerIx;
            while (UnitCollection.ReadHeader(r, version, out playerIx))
            {
                var p = getGenericPlayer(playerIx);
                p.unitsColl.ReadUnitSetup(r, version);
            }            
        }
        public void writeGenericPlayer(int globalPlayerIndex, System.IO.BinaryWriter w)
        {
            Engine.PlayerData.WriteGlobalIndex(globalPlayerIndex, w);
        }

        public void writeGenericPlayer(AbsGenericPlayer player, System.IO.BinaryWriter w)
        {
            player.pData.writeGlobalIndex(w);
        }

        abstract public AbsGenericPlayer readGenericPlayer(System.IO.BinaryReader r);
        abstract public AbsGenericPlayer GenPlayer(int globalIndex);
        abstract public void clearUnits();

        abstract public void OnOpenCloseMenu(bool open);

        abstract public bool IsOpponent(AbsUnit u1, AbsUnit u2);
        abstract public bool IsOpponent(int p1Index, int p2Index);

        public List<AbsUnit> adjacentOpponents(AbsUnit unit, IntVector2 fromSquare)
        {
            List<AbsUnit> result = new List<AbsUnit>(4);

            foreach (IntVector2 dir in IntVector2.Dir8Array)
            {
                IntVector2 pos = dir + fromSquare;
                if (toggRef.board.tileGrid.InBounds(pos))
                {
                    AbsUnit nUnit = toggRef.board.tileGrid.Get(pos).unit;
                    if (nUnit != null && IsOpponent(unit, nUnit))
                    {
                        result.Add(nUnit);
                    }
                }
            }

            return result;
        }

        public List<AbsUnit> opponentsInLOS(AbsUnit unit, IntVector2 fromSquare, int range, bool includeMeleeRange)
        {
            List<AbsUnit> result = new List<AbsUnit>(8);

            Rectangle2 area = Rectangle2.FromTwoTilePoints(fromSquare - new IntVector2(range), fromSquare + new IntVector2(range));
            area.SetBounds(toggRef.board.tileGrid.Area);

            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                var otherUnit = toggRef.board.tileGrid.Get(loop.Position).unit;
                if (otherUnit != null && IsOpponent(unit, otherUnit))
                {
                    int dist = (loop.Position - fromSquare).SideLength();

                    if (dist > 1 || includeMeleeRange)
                    {
                        IntVector2 block;
                        if (dist == 1 || unit.InLineOfSight(fromSquare, otherUnit.squarePos, 
                            false, out block))
                        {
                            result.Add(otherUnit);
                        }
                    }
                }
            }

            return result;
        }

        public List<AbsUnit> allFriendlyUnits(int player)
        {
            List<AbsGenericPlayer> team = allTeamPlayers(getGenericPlayer(player).pData.teamIndex);

            List<AbsUnit> result = new List<AbsUnit>(8);
            foreach (var m in team)
            {
                var ucounter = m.unitsColl.unitsCounter.IClone();
                while (ucounter.Next())
                {
                    result.Add(ucounter.GetSelection);
                }
            }

            return result;
        }

        abstract public List<AbsGenericPlayer> allTeamPlayers(int team);

        abstract public AbsGenericPlayer LocalHost();

        abstract public void OnUnitsCountChange();

        virtual public void OnMapPan(Vector2 screenPan)
        { }

    }
}
