using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;

namespace VikingEngine.DSSWars.Net
{
    /// <summary>
    /// Helper to find objects over network
    /// </summary>
    static class ObjectId
    {
        public static void WriteFaction(System.IO.BinaryWriter w, Faction faction)
        {
            if (faction == null)
            {
                w.Write(ushort.MaxValue);
            }
            else
            {
                w.Write((ushort)faction.myIndex);
            }
        }
        public static Faction ReadFaction(System.IO.BinaryReader r, out int index)
        {
            index = r.ReadUInt16();
            return DssRef.world.faction(index);
        }

        public static void WriteCity(System.IO.BinaryWriter w, City city)
        {
            w.Write((ushort)city.myIndex);
        }
        public static City ReadCity(System.IO.BinaryReader r)
        {
            return DssRef.world.cities[r.ReadUInt16()];
        }

        public static void WriteCityAndOwner(System.IO.BinaryWriter w, City city)
        {
            w.Write((ushort)city.myIndex);
            city.pfaction.write(w);
            //if (city.factionIndex < 0)
            //{
            //    w.Write(ushort.MaxValue);
            //}
            //else
            //{
            //    w.Write((ushort)city.factionIndex);
            //}
        }
        public static City ReadCityAndOwner(System.IO.BinaryReader r)
        {
            var city = DssRef.world.cities[r.ReadUInt16()];
            city.setFaction(DssRef.world.faction(r.ReadUInt16()), false, false, ConvertReason.Assigned, false);
            return city;
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
                var group = ReadSoldierGroup(r, true, out mapObj);
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
                var group = ReadSoldierGroup(r, true, out mapObj);
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

        public static SoldierGroup ReadSoldierGroup(System.IO.BinaryReader r, bool createIfMissing, out AbsArmy mapObj)
        { 
            bool isArmy = r.ReadBoolean();
            if (NetReadMapObjId(r, out _, isArmy, createIfMissing, out mapObj, out _))
            {
                var result = mapObj.groups.GetIndex_Safe(r.ReadUInt16());
                return result;
            }
            return null;
        }

        public static void NetWriteMapObjId(System.IO.BinaryWriter w, AbsArmy army)
        {
            //w.Write((ushort)army.factionIndex);
            army.pfaction.write(w);
            w.Write((ushort)army.myIndex);
        }

        public static bool NetReadMapObjId(System.IO.BinaryReader r, out Faction faction, bool bArmy, bool createIfMissing, out AbsArmy mapObj, out bool needInit)
        {
            //int factionIx = r.ReadUInt16();
            PFaction pfaction = new PFaction(r);
            faction = pfaction.GetFaction();
            if (faction == null)
            {
                mapObj = null;
                needInit = false;
                return false;
            }

            //faction = DssRef.world.faction(factionIx);

            //if (faction == null)
            //{
            //    mapObj = null;
            //    needInit = false;
            //    return false;
            //}

            int unitIx = r.ReadUInt16();

            if (bArmy)
            {
                Army army = faction.armies.GetIndex_Safe(unitIx);
                needInit = false;
                if (army == null)
                {
                    if (createIfMissing)
                    {
                        army = new Army();
                        army.pfaction = pfaction;
                        //faction.armies.HardSet(army, armyIx);
                        army.init(faction, unitIx);
                        needInit = true;
                    }
                    else
                    {
                        mapObj = null;
                        needInit = false;
                        faction = null;
                        return false;
                    }
                }

                if (DssRef.state.host)
                {
                    army.IsNetHosted = faction.player.IsLocal;
                }
                else
                {
                    army.IsNetHosted = faction.player != null && faction.player.IsLocalPlayer();
                }
                mapObj = army;
#if DEBUG
                //Debug.Log($"NET read army ({army.myIndex}), faction ({faction.PlayerName}), army count: {faction.armies.Count}");
#endif
            }
            else
            {
                needInit = false;
                //int unitIx = r.ReadUInt16();
                mapObj = DssRef.world.cities[unitIx];
                mapObj.setFaction(faction, false, true, ConvertReason.Assigned, false);
                faction = mapObj.pfaction.GetFaction();
            }

            return true;
        }

    }
}
