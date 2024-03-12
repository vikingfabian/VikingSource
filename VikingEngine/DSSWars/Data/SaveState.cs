using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.PJ.Strategy;

namespace VikingEngine.DSSWars.Data
{
    class SaveGamestate : AbsUpdateable
    {
        MemoryStreamHandler memoryStream = new MemoryStreamHandler();

        public SaveGamestate() :
            base(false)
        { }

        public void save()
        {
            AddToUpdateList();

            var w = memoryStream.GetWriter();

            Task.Factory.StartNew(() =>
            {
                DssRef.storage.write(w);
            });

            //TODO spara alla pointers sist, eller någon pointer replacement list
            DssRef.world.writeGameState(w);

            DssRef.state.events
        }

        public override void Time_Update(float time_ms)
        {
            
        }

    }

    class ObjectPointerCollection
    { 
        public List<AbsObjectPointer> pointers = new List<AbsObjectPointer>();

        public void SetPointer()
        {
            foreach (var m in pointers)
            { 
                m.SetPointer();
            }
        }
    }

    abstract class AbsObjectPointer
    {
        public AbsGameObject pointer;
        GameObjectType gameObjectType;
        int factionIndex;
        int id;

        public void WriteObjectPointer(System.IO.BinaryWriter w, AbsGameObject gameObject)
        {
            var type = gameObject.gameobjectType();
            w.Write((byte)type);
            switch (type)
            {
                case GameObjectType.Army:
                    writeFaction(w, gameObject.Faction());
                    w.Write((ushort)gameObject.GetArmy().id);
                    break;
                case GameObjectType.City:
                    w.Write((ushort)gameObject.GetCity().index);
                    break;
            }
        }

        public void ReadObjectPointer(System.IO.BinaryReader r)
        {
            gameObjectType = (GameObjectType)r.ReadByte();
            switch (gameObjectType)
            {
                case GameObjectType.Army:
                    factionIndex = readFaction(r);
                    id = r.ReadUInt16();
                    //var armiesC = faction.armies.counter();
                    //while (armiesC.Next())
                    //{
                    //    if (armiesC.sel.id == id)
                    //    {
                    //        return armiesC.sel;
                    //    }
                    //}
                    break;
                case GameObjectType.City:
                    //return DssRef.world.cities[r.ReadUInt16()];
                    id = r.ReadUInt16();
                    break;
            }

            //throw new Exception("ReadObjectPointer");

        }

        protected Faction GetFaction()
        {
           return DssRef.world.factions.Array[factionIndex];
        }

        protected AbsGameObject GetObject()
        {
            switch (gameObjectType)
            {
                case GameObjectType.Army:
                    var armiesC = GetFaction().armies.counter();
                    while (armiesC.Next())
                    {
                        if (armiesC.sel.id == id)
                        {
                            return armiesC.sel;
                        }
                    }
                    break;
                case GameObjectType.City:
                    return DssRef.world.cities[id];
            }

            throw new Exception("GetObject from Pointer");

        }

        public void writeFaction(System.IO.BinaryWriter w, Faction faction)
        {
            w.Write((ushort)faction.index);
        }

        public int readFaction(System.IO.BinaryReader r)
        {
            return r.ReadUInt16();
        }

        abstract public void SetPointer();
    }

    class ArmyAttackObjectPointer: AbsObjectPointer
    {
        Army army;

        public ArmyAttackObjectPointer(System.IO.BinaryWriter w, AbsGameObject target)
        {
            WriteObjectPointer(w, target);        
        }

        public ArmyAttackObjectPointer(BinaryReader r, Army army)
        {
            this.army = army;
            ReadObjectPointer(r);
        }

        public override void SetPointer()
        {
            army.attackTarget = (AbsMapObject)GetObject();
        }
    }

class BattleMemberObjectPointer: AbsObjectPointer
    {
        BattleGroup battle;

        public BattleMemberObjectPointer(System.IO.BinaryWriter w, AbsGameObject target)
        {
            WriteObjectPointer(w, target);        
        }

        public BattleMemberObjectPointer(BinaryReader r, BattleGroup battle)
        {
            this.battle = battle;
            ReadObjectPointer(r);
        }

        public override void SetPointer()
        {
            battle  = (AbsMapObject)GetObject();
        }
    }
}
