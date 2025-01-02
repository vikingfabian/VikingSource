using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DataStream;
//using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.LootFest.Data;
using VikingEngine.PJ.Strategy;
using VikingEngine.ToGG;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Data
{
    class SaveGamestate : AbsUpdateable, IStreamIOCallback
    {
        public const int Version = 11;
        public const int SubVersion = 47; 
        //public const int MergeVersion = 26;
        MemoryStreamHandler memoryStream = new MemoryStreamHandler();

        bool dataReady = false;
        public bool complete = false;
        ObjectPointerCollection pointers;
        SaveStateMeta meta;
        public WorldData worldData;

        public SaveGamestate(SaveStateMeta meta)
             : base(false)
        {
            this.meta = meta;    
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
            DataStream.BeginReadWrite.BinaryIO(false, meta.Path, null, readGameState, this, true);
        }

        

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            //TODO error handling
            if (save == false)
            {
                pointers.SetPointer();
            }

            complete = true;
        }

        public void writeNet(System.IO.BinaryWriter w)
        {
            writeGameState(w);
        }
        public void readNet(System.IO.BinaryReader r)
        {
            readGameState(r);
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            new SaveVersion(Version, SubVersion).write(w);

            //META
            meta.write(w);
            Debug.WriteCheck(w);

            //WORLD
            DssRef.world.writeMapFile(w);

            //STATE
            DssRef.storage.write(w, true);
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

            SaveVersion version = new SaveVersion();
            version.read(r);

            //META            
            meta.read(r);
            Debug.ReadCheck(r);
            
            //WORLD
            worldData = new WorldData();
            worldData.metaData = meta.worldmeta;
            worldData.readMapFile(r);
            DssRef.world = worldData;
            

            DssRef.state.initGameState(false, pointers);

            //STATE
            DssRef.storage.read(r, true);
            Debug.ReadCheck(r);
            DssRef.settings.readGameState(r, version.sub, pointers);
            Debug.ReadCheck(r);
            DssRef.world.readGameState(r, version.sub, pointers);
            Debug.ReadCheck(r);
            DssRef.state.readGameState(r, version.sub, pointers);
            DssRef.time.setTotalTime(meta.playTime);
        }

        public override void Time_Update(float time_ms)
        {
            if (dataReady)
            { 
                dataReady = false;
                var path = meta.Path;
                System.IO.Directory.CreateDirectory(path.CompleteDirectory);
                new WriteByteArray(path, memoryStream, this);
            }
        }

    }

    class ObjectPointerCollection
    { 
        //public List<LocalPlayer> localPlayers = new List<LocalPlayer>();
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
            if (gameObject == null)
            {
                w.Write((byte)GameObjectType.NONE);
            }
            else
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
        }

        public void ReadObjectPointer(System.IO.BinaryReader r)
        {
            gameObjectType = (GameObjectType)r.ReadByte();
            switch (gameObjectType)
            {
                case GameObjectType.NONE:
                    pointer = null;
                    return;
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
                    var faction = GetFaction();
                    if (faction != null)
                    {
                        var armiesC = faction.armies.counter();
                        while (armiesC.Next())
                        {
                            if (armiesC.sel.id == id)
                            {
                                return armiesC.sel;
                            }
                        }
                    }
                    break;
                case GameObjectType.City:
                    return DssRef.world.cities[id];
            }

            //throw new Exception("GetObject from Pointer");
            return null;
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
        bool teleport;

        public ArmyAttackObjectPointer(System.IO.BinaryWriter w, AbsGameObject target)
        {
            //if (target != null)
            //{
                WriteObjectPointer(w, target);
            //}
        }

        public ArmyAttackObjectPointer(BinaryReader r, Army army, bool teleport)
        {
            this.army = army;
            this.teleport = teleport;
            ReadObjectPointer(r);
        }

        public override void SetPointer()
        {
            var target = (AbsMapObject)GetObject();

            if (target != null)
            {
                if (teleport)
                {
                    army.Order_Attack_Setup(target);
                }
                else
                {
                    army.Order_Attack(target);
                }
            }
        }
    }

    //class BattleMemberObjectPointer: AbsObjectPointer
    //{
    //    //BattleGroup battle;

    //    public BattleMemberObjectPointer(System.IO.BinaryWriter w, AbsGameObject target)
    //    {
    //        WriteObjectPointer(w, target);        
    //    }

    //    //public BattleMemberObjectPointer(BinaryReader r, BattleGroup battle)
    //    //{
    //    //    this.battle = battle;
    //    //    ReadObjectPointer(r);
    //    //}

    //    //public override void SetPointer()
    //    //{
    //    //    battle.addPart((AbsMapObject)GetObject(), false);
    //    //}
    //}

    struct SaveVersion
    {
        public int major;
        public int sub;

        public SaveVersion(int main, int sub)
        { 
            this.major = main;
            this.sub = sub;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(major);
            w.Write(sub);
        }

        public void read(System.IO.BinaryReader r)
        {
            major = r.ReadInt32();
            sub = r.ReadInt32();
        }
    }
}
