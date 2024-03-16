using System.Collections.Generic;
using System.Linq;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Data
{
    static class IOLib
    {
        public static void WriteGameObject(System.IO.BinaryWriter w, AbsGameObject gameObject)
        {
            var type = gameObject.gameobjectType();
            w.Write((byte)type);
            switch (type)
            {
                case GameObjectType.Army:
                    WriteFactionPointer(w, gameObject.GetFaction());
                    w.Write((ushort)gameObject.parentArrayIndex);
                    break;

                case GameObjectType.City:
                    w.Write((ushort)gameObject.parentArrayIndex);
                    break;

                case GameObjectType.Faction:
                    WriteFactionPointer(w, gameObject.GetFaction());
                    break;
            }
        }

        public static AbsGameObject ReadGameObject(System.IO.BinaryReader r)
        {
            var gameObjectType = (GameObjectType)r.ReadByte();
            switch (gameObjectType)
            {
                case GameObjectType.Army:
                    {
                        var faction = ReadFactionPointer(r);
                        var index = r.ReadUInt16();
                        return faction.armies.Array[index];
                    }
                case GameObjectType.City:
                    {
                        var index = r.ReadUInt16();
                        return DssRef.world.cities[index];
                    }
                case GameObjectType.Faction:
                    {
                        var index = r.ReadUInt16();
                        return DssRef.world.factions.Array[index];
                    }
            }

            throw new NotImplementedExceptionExt("ReadGameObject " + gameObjectType.ToString());
        }

        public static void WriteObjectList<T>(System.IO.BinaryWriter w, T[] objects) where T : AbsGameObject
        {
            if (objects != null)
            {
                WriteObjectList(w, objects.ToList());
            }
            else
            {
                w.Write(false);
            }
        }

        public static void WriteObjectList<T>(System.IO.BinaryWriter w, List<T> objects) where T : AbsGameObject
        {
            if (objects != null)
            {
                w.Write(true);
                w.Write((ushort)objects.Count);

                foreach (var m in objects)
                {
                    WriteGameObject(w, m);
                }
            }
            else
            {
                w.Write(false);
            }
        }

        public static List<T> ReadObjectList<T>(System.IO.BinaryReader r) where T : AbsGameObject
        {
            if (r.ReadBoolean())
            { 
                int count = r.ReadUInt16();
                var objects = new List<T>();
                for (int i = 0; i < count; i++)
                {
                    T obj = (T)ReadGameObject(r);
                    objects.Add(obj);
                }

                return objects;
            }
            else
            { return null; }
        }


        public static void WriteBinaryList<T>(System.IO.BinaryWriter w, T[] objects) where T : IBinaryIOobj
        {
            if (objects != null)
            {
                WriteBinaryList(w, objects.ToList());
            }
            else
            {
                w.Write(false);
            }
        }

        public static void WriteBinaryList<T>(System.IO.BinaryWriter w, List<T> objects) where T : IBinaryIOobj
        {
            if (objects != null)
            {
                w.Write(true);
                w.Write((ushort)objects.Count);

                foreach (var m in objects)
                {
                    m.write(w);
                }
            }
            else
            {
                w.Write(false);
            }
        }

        public static List<T> ReadBinaryList<T>(System.IO.BinaryReader r) where T : IBinaryIOobj
        {
            if (r.ReadBoolean())
            {
                int count = r.ReadUInt16();
                var objects = new List<T>();
                for (int i = 0; i < count; i++)
                {
                    T obj = default(T);
                    obj.read(r);
                    objects.Add(obj);
                }

                return objects;
            }
            else
            { return null; }
        }


        public static void WriteFactionPointer(System.IO.BinaryWriter w, Faction faction)
        {
            w.Write((ushort)faction.parentArrayIndex);
        }

        public static Faction ReadFactionPointer(System.IO.BinaryReader r)
        {
            return DssRef.world.factions.Array[r.ReadUInt32()];
        }


    }
}
