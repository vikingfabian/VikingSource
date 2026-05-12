using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Net
{
    /// <summary>
    /// Helper to find objects over network
    /// </summary>
    static class ObjectId
    {
        public static void WriteFaction(System.IO.BinaryWriter w, Faction faction)
        {
            w.Write((ushort)faction.myIndex);
        }
        public static Faction ReadFaction(System.IO.BinaryReader r)
        { 
            return DssRef.world.faction(r.ReadUInt16());
        }

        public static void WriteSoldier(System.IO.BinaryWriter w, AbsSoldierUnit soldier)
        {
            if (soldier != null)
            {
                w.Write((byte)soldier.myIndex);
                WriteSoldierGroup(w, soldier.group);
            }
            else
            { 
                w.Write(byte.MaxValue);
            }
        }

        public static AbsSoldierUnit ReadSoldier(System.IO.BinaryReader r, out AbsArmy mapObj)
        {
            int soldierIx = r.ReadByte();
            if (soldierIx < byte.MaxValue)
            {
                var group = ReadSoldierGroup(r, out mapObj);
                if (group != null)
                {
                    var soldiers_sp = group.soldiers;
                    if (soldiers_sp != null)
                    {
                        var result = soldiers_sp.GetIndex_Safe(soldierIx);
                        return result;
                    }
                }
            }
            mapObj = null;
            return null;
        }

        public static AbsSoldierUnit ReadSoldier_ForBattle(System.IO.BinaryReader r, out AbsArmy mapObj)
        {
            int soldierIx = r.ReadByte();
            if (soldierIx < byte.MaxValue)
            {
                var group = ReadSoldierGroup(r, out mapObj);
                if (group != null)
                {
                    group.enterBattleState(true, false);

                    var soldiers_sp = group.soldiers;
                    if (soldiers_sp != null)
                    {
                        var result = soldiers_sp.GetIndex_Safe(soldierIx);
                        return result;
                    }
                }
            }
            mapObj = null;
            return null;
        }

        public static void WriteSoldierGroup(System.IO.BinaryWriter w, SoldierGroup soldierGroup)
        {
            if (soldierGroup != null && soldierGroup.army.TryGetTarget(out var tArmy))
            {
                w.Write(tArmy.IsArmy());
                NetWriteMapObjId(w, tArmy);
                w.Write((ushort)soldierGroup.myIndex);
            }
            else
            {
                w.Write(false);
                w.Write(ushort.MaxValue);
            }
        }

        public static SoldierGroup ReadSoldierGroup(System.IO.BinaryReader r, out AbsArmy mapObj)
        { 
            bool isArmy = r.ReadBoolean();
            if (NetReadMapObjId(r, out _, isArmy, out mapObj, out _))
            {
                var result = mapObj.groups.GetIndex_Safe(r.ReadUInt16());
                return result;
            }
            return null;
        }

        public static void NetWriteMapObjId(System.IO.BinaryWriter w, AbsArmy army)
        {
            w.Write((ushort)army.factionIndex);
            w.Write((ushort)army.myIndex);
        }

        public static bool NetReadMapObjId(System.IO.BinaryReader r, out Faction faction, bool bArmy, out AbsArmy mapObj, out bool needInit)
        {
            int factionIx = r.ReadUInt16();

            if (factionIx == ushort.MaxValue)
            {
                mapObj = null;
                needInit = false;
                faction = null;
                return false;
            }

            faction = DssRef.world.faction(factionIx);

            if (faction == null)
            {
                mapObj = null;
                needInit = false;
                return false;
            }

            int unitIx = r.ReadUInt16();

            if (bArmy)
            {
                Army army = faction.armies.GetIndex_Safe(unitIx);
                needInit = false;
                if (army == null)
                {
                    army = new Army();
                    army.factionIndex = factionIx;
                    //faction.armies.HardSet(army, armyIx);
                    army.init(faction, unitIx);
                    needInit = true;
                }
                army.IsNetHosted = faction.player != null && faction.player.IsLocalPlayer();
                mapObj = army;

#if DEBUG
                Debug.Log($"NET read army ({army.myIndex}), faction ({faction.PlayerName}), army count: {faction.armies.Count}");
#endif
            }
            else
            {
                needInit = false;
                //int unitIx = r.ReadUInt16();
                mapObj = DssRef.world.cities[unitIx];
                mapObj.setFaction(faction, false, true);
                faction = mapObj.GetFaction();
            }

            return true;
        }

    }
}
