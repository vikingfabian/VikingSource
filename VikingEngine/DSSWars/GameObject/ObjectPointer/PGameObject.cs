using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.PJ.MiniGolf;

namespace VikingEngine.DSSWars.GameObject.ObjectPointer
{
    struct PGameObject
    {
        public static readonly PGameObject Empty = new PGameObject();
        public GameObjectType parentType;
        public GameObjectType objectType;
        public PFaction pfaction;
        public int objectIndex;
        public int groupIndex;
        public int groupMemberIndex;

        public PGameObject()
        {
            objectType = GameObjectType.NONE;
            parentType = GameObjectType.NONE;
            objectIndex = -1;
            groupMemberIndex = -1;
        }

        public PGameObject(
            GameObjectType parentType,
            GameObjectType objectType, 
            PFaction pfaction, 
            int objectIndex, 
            int groupIndex = -1, 
            int groupMemberIndex = -1)
        {
            this.parentType = parentType;
            this.objectType = objectType;
            this.pfaction = pfaction;
            this.objectIndex = objectIndex;
            this.groupIndex = groupIndex;
            this.groupMemberIndex = groupMemberIndex;
        }

        public AbsGameObject Get()
        {
            switch (objectType)
            {
                case GameObjectType.SoldierGroup:
                    if (parentType == GameObjectType.Army)
                    {
                        if (pfaction.TryGetFaction(out var gf))
                        {
                            return gf.armies.GetIndex_Safe(objectIndex)?.groups.GetIndex_Safe(groupIndex);
                        }
                    }
                    else
                    {
                        DssRef.world.cities[objectIndex].groups.GetIndex_Safe(groupIndex);
                    }
                    break;
                case GameObjectType.Soldier:
                    if (pfaction.TryGetFaction(out var sf))
                    {
                        return sf.armies.GetIndex_Safe(objectIndex)?.groups.GetIndex_Safe(groupIndex).soldiers?.GetIndex_Safe(groupMemberIndex);                      
                    }
                    break;

                case GameObjectType.City:
                    return DssRef.world.cities[objectIndex];
            }

            return null;
        }

        public bool TryGetGroup(out AbsGroup group)
        {
            group = Get() as AbsGroup;
            return group != null;
        }

        public bool TryGetSoldier(out SoldierGroup group, out AbsSoldierUnit soldier)
        {
            soldier = null;
            group = null;
            AbsArmy parent = null;
            if (parentType == GameObjectType.Army)
            {
                if (pfaction.TryGetFaction(out var gf))
                {
                    parent = gf.armies.GetIndex_Safe(objectIndex);
                }
            }
            else
            {
                parent = DssRef.world.cities[objectIndex];
            }
            group = parent?.groups.GetIndex_Safe(groupIndex);
            if (group != null)
            {
                soldier = group.soldiers?.GetIndex_Safe(groupMemberIndex);
            }
            return soldier != null;
        }

        public PSoldierGroup GetSoldierGroupPointer()
        {
            return new PSoldierGroup(new PMapObject(objectType, pfaction, objectIndex), groupIndex);
        }
        public PGameObject(System.IO.BinaryReader r)
            : this()
        {
            read(r);
        }
        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)objectType);
            if (objectType != GameObjectType.NONE)
            {
                pfaction.write(w);
                if (objectIndex < 0)
                {
                    w.Write(ushort.MaxValue);
                }
                else
                {
                    w.Write((ushort)objectIndex);

                    switch (objectType)
                    {
                        case GameObjectType.SoldierGroup:
                            w.Write((byte)parentType);
                            if (groupIndex < 0)
                            {
                                w.Write(ushort.MaxValue);
                            }
                            else
                            {
                                w.Write((ushort)groupIndex);
                            }
                            break;
                        case GameObjectType.Soldier:
                            w.Write((byte)parentType);
                            if (groupIndex < 0)
                            {
                                w.Write(ushort.MaxValue);
                            }
                            else
                            {
                                w.Write((ushort)groupIndex);

                                w.Write((ushort)groupMemberIndex);
                            }
                            break;
                    }
                }
            }
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
                else
                {
                    switch (objectType)
                    {
                        case GameObjectType.SoldierGroup:
                            parentType = (GameObjectType)r.ReadByte();
                            groupIndex = r.ReadUInt16();
                            if (groupIndex == ushort.MaxValue)
                            {
                                groupIndex = -1;
                            }
                            break;
                        case GameObjectType.Soldier:
                            parentType = (GameObjectType)r.ReadByte();
                            groupIndex = r.ReadUInt16();
                            if (groupIndex == ushort.MaxValue)
                            {
                                groupIndex = -1;
                            }
                            else
                            {
                                groupMemberIndex = r.ReadUInt16();
                            }
                            break;
                    }
                }
            }
        }

        // 1. Strongly typed Equals (from IEquatable<PGameObject>) to avoid boxing
        public bool Equals(PGameObject other)
        {
            return objectType == other.objectType &&
                    parentType == other.parentType &&
                   pfaction == other.pfaction &&
                   objectIndex == other.objectIndex &&
                   groupIndex == other.groupIndex &&
                   groupMemberIndex == other.groupMemberIndex;
        }

        // 2. Standard object.Equals override
        public override bool Equals(object obj)
        {
            return obj is PGameObject other && Equals(other);
        }

        // 3. GetHashCode override using modern HashCode.Combine
        public override int GetHashCode()
        {
            return HashCode.Combine(objectType, pfaction, objectIndex, groupIndex, groupMemberIndex);
        }

        // 4. Equality Operators
        public static bool operator ==(PGameObject left, PGameObject right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PGameObject left, PGameObject right)
        {
            return !left.Equals(right);
        }

        // 5. ToString override for easy debugging
        public override string ToString()
        {
            return $"PGameObject [Type: {objectType}, Parent: {parentType}, Faction: {pfaction}, ObjIndex: {objectIndex}, GrpIndex: {groupIndex}, MemIndex: {groupMemberIndex}]";
        }

        public bool HasValue()
        {
            return objectType != GameObjectType.NONE;
        }
        public bool IsEmpty()
        {
            return objectType == GameObjectType.NONE;
        }
    }
}
