using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.PJ.Strategy;
using VikingEngine.ToGG;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Data
{
    class SaveGamestate : AbsUpdateable, IStreamIOCallback
    {
        public const int Version = 2;
        MemoryStreamHandler memoryStream = new MemoryStreamHandler();

        DataStream.FilePath path = new DataStream.FilePath(null, "DSS_savestate_v" + Version.ToString(), ".sav");
        bool dataReady = false;
        public bool complete = false;
        ObjectPointerCollection pointers;

        public SaveGamestate()
             : base(false)
        {
            
        }

        public void save()
        {
            AddToUpdateList();

            var w = memoryStream.GetWriter();

            Task.Factory.StartNew(() =>
            {
                writeGameState(w);
                dataReady = true;   
            });
        }

        public void load()
        {
            //complete = true;
            DataStream.BeginReadWrite.BinaryIO(false, path, null, readGameState, this, true);
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            if (save == false)
            {
                pointers.SetPointer();
            }

            complete = true;
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(Version);

            DssRef.storage.write(w);
            Debug.WriteCheck(w);
            DssRef.settings.writeGameState(w);
            Debug.WriteCheck(w);
            DssRef.world.writeGameState(w);
            Debug.WriteCheck(w);
            DssRef.state.writeGameState(w);
        }

        public void readGameState(System.IO.BinaryReader r)
        {
            pointers = new ObjectPointerCollection();

            int version = r.ReadInt32();

            DssRef.storage.read(r);
            Debug.ReadCheck(r);
            DssRef.settings.readGameState(r, version, pointers);
            Debug.ReadCheck(r);
            DssRef.world.readGameState(r, version, pointers);
            Debug.ReadCheck(r);
            DssRef.state.readGameState(r, version, pointers);
        }

        public override void Time_Update(float time_ms)
        {
            if (dataReady)
            { 
                dataReady = false;
                new WriteByteArray(path, memoryStream, this);
            }
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
                    writeFaction(w, gameObject.GetFaction());
                    w.Write((ushort)gameObject.GetArmy().id);
                    break;
                case GameObjectType.City:
                    w.Write((ushort)gameObject.GetCity().parentArrayIndex);
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
                    break;
                case GameObjectType.City:
                    id = r.ReadUInt16();
                    break;
            }
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
            w.Write((ushort)faction.parentArrayIndex);
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
            army.attackTargetFaction = army.attackTarget.faction.parentArrayIndex;
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
            battle.addPart((AbsMapObject)GetObject(), false);
        }
    }
}
