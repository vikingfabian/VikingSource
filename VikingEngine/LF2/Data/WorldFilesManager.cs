using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Data
{

    ///// <summary>
    ///// Keeps track of all worlds you got, when you last visited them, name, status, deleted
    ///// </summary>
    //class WorldFilesManager : IBinaryIOobj
    //{
    //    List<WorldSummary2> worlds;
    //    List<WorldSummary2> deletedWorlds;

    //    public void WriteStream(System.IO.BinaryWriter w)
    //    {
    //        const byte Version = 0;
    //        w.Write(Version);
    //        w.Write((ushort)worlds.Count);
    //        foreach (WorldSummary2 world in worlds)
    //        {
    //            world.WriteStream(w);
    //        }
    //        //deleted
    //        w.Write((ushort)deletedWorlds.Count);
    //        foreach (WorldSummary2 world in deletedWorlds)
    //        {
    //            world.WriteStream(w);
    //        }
    //    }
    //    public void ReadStream(System.IO.BinaryReader r)
    //    {
    //        worlds = new List<WorldSummary2>();
    //        deletedWorlds = new List<WorldSummary2>();

    //        byte version = r.ReadByte();
    //        int numWorlds = r.ReadUInt16();
    //        for (int i = 0; i < numWorlds; i++)
    //        {
    //            worlds.Add(WorldSummary2.FromStream(r, version));
    //        }
    //        //deleted
    //        numWorlds = r.ReadUInt16();
    //        for (int i = 0; i < numWorlds; i++)
    //        {
    //            deletedWorlds.Add(WorldSummary2.FromStream(r, version));
    //        }
    //    }
    //}

    //struct WorldSummary2
    //{
    //    //%completed
    //    //gold
    //    //num enemies killed
    //    public ushort SaveIndex;
    //    string name;
    //    DateTime created;
    //    DateTime lastVisit;

    //    public void WriteStream(System.IO.BinaryWriter w)
    //    {
    //        w.Write(SaveIndex);
    //        SaveLib.WriteString(w, name);
    //        w.Write(created.Ticks);
    //        w.Write(lastVisit.Ticks);
    //    }
    //    public void ReadStream(System.IO.BinaryReader r, byte version)
    //    {
    //        SaveIndex = r.ReadUInt16();
    //        name = SaveLib.ReadString(r);
    //        created = new DateTime(r.ReadInt64());
    //        lastVisit = new DateTime(r.ReadInt64());
    //    }
    //    public static WorldSummary2 FromStream(System.IO.BinaryReader r, byte version)
    //    {
    //        WorldSummary2 result = new WorldSummary2();
    //        result.ReadStream(r, version);
    //        return result;
    //    }
    //}
}
