using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.ObjectPointer
{
    struct PMapObject
    {
        public static readonly PMapObject Empty = new PMapObject();

        public GameObjectType objectType;
        public PFaction pfaction;
        public int objectIndex;

        public PMapObject()
        {
            objectIndex = -1;
        }

        public PMapObject(GameObjectType objectType,
            PFaction pfaction,
            int objectIndex)
        {
            this.objectType = objectType;
            this.pfaction = pfaction;
            this.objectIndex = objectIndex;
        }

        public AbsMapObject Get()
        {
            if (objectIndex < 0)
            { 
                return null;
            }

            switch (objectType)
            {
                case GameObjectType.Army:
                    if (pfaction.TryGetFaction(out var f))
                    {
                        return f.armies.GetIndex_Safe(objectIndex);
                    }
                    break;
                case GameObjectType.City:
                    return DssRef.world.cities[objectIndex];
            }
            
            return null;
        }

        public bool TryGetAbsArmy(out AbsArmy army)
        {
            army = Get() as AbsArmy;
            return army != null;
        }

        public bool TryGetGroup(out AbsGroup group)
        {
            group = Get() as AbsGroup;
            return group != null;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)objectType);
            if (objectType != GameObjectType.NONE)
            {
                pfaction.write(w);
                w.Write((ushort)objectIndex);                
            }
            
        }
        public PMapObject(System.IO.BinaryReader r)
        {
            read(r);
        }
        public void read(System.IO.BinaryReader r)
        {
            objectType = (GameObjectType)r.ReadByte();
            if (objectType != GameObjectType.NONE)
            {
                pfaction.read(r);
                objectIndex = r.ReadUInt16();
                if (objectIndex == ushort.MaxValue)
                {
                    objectIndex = -1;
                }                
            }
        }

        // 1. Strongly typed Equals (from IEquatable<PMapObject>) to avoid boxing
        public bool Equals(PMapObject other)
        {
            return objectType == other.objectType &&

                   objectIndex == other.objectIndex &&
                   pfaction == other.pfaction;
        }

        // 2. Standard object.Equals override
        public override bool Equals(object obj)
        {
            return obj is PMapObject other && Equals(other);
        }

        // 3. GetHashCode override using modern HashCode.Combine
        public override int GetHashCode()
        {
            return HashCode.Combine(objectType, pfaction, objectIndex);
        }

        // 4. Equality Operators
        public static bool operator ==(PMapObject left, PMapObject right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PMapObject left, PMapObject right)
        {
            return !left.Equals(right);
        }

        // 5. ToString override for easy debugging
        public override string ToString()
        {
            return $"PMapObject [Type: {objectType}, Faction: {pfaction}, ObjIndex: {objectIndex}]";
        }
    }
}
