using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Data
{
    static class IOLib
    {
       

        public static void WriteFactionPointer(System.IO.BinaryWriter w, Faction faction)
        {
            w.Write((ushort)faction.index);
        }

        public static Faction ReadFactionPointer(System.IO.BinaryReader r)
        {
            return DssRef.world.factions.Array[r.ReadUInt32()];
        }


    }
}
