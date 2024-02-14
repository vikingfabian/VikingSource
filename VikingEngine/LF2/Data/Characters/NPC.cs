using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.Map;
using VikingEngine.LF2.GameObjects.EnvironmentObj;

namespace VikingEngine.LF2.Data.Characters
{
    struct NPCdataArgs
    {
        public static readonly NPCdataArgs GenerateLater = new NPCdataArgs();

        public bool GenerateNow;
        public Map.WorldPosition wp;
        public bool FromLoading;

        public NPCdataArgs(Map.WorldPosition wp)
            :this(wp, false)
        {   
        }

        public NPCdataArgs(Map.WorldPosition wp, bool FromLoading)
        {
            this.wp = wp;
            this.FromLoading = FromLoading;//false;
            GenerateNow = true;
        }
        
    }


    class NPCdata : LF2.Director.IEnvObjGenerator
    {
        //protected static Voxels.VoxelObjGridData goldsmith_station;
        protected static Voxels.VoxelObjGridData bow_station;
        protected static Voxels.VoxelObjGridData armor_station;
        protected static Voxels.VoxelObjGridData cook_station;
        protected static Voxels.VoxelObjGridData vulcan_station;
        //protected static Voxels.VoxelObjGridData sheild_station;
        protected static Voxels.VoxelObjGridData blacksmith_station;
        protected static Voxels.VoxelObjGridData healer_station;
        protected static Voxels.VoxelObjGridData salesman_station;
        protected static Voxels.VoxelObjGridData lumberjack_station;
        protected static Voxels.VoxelObjGridData miner_station;

        protected static Voxels.VoxelObjGridData weaponsmith_station;
        protected static Voxels.VoxelObjGridData tailor_station;
        protected static Voxels.VoxelObjGridData wiselady_station;
        protected static Voxels.VoxelObjGridData priest_station;

        public static void LoadStationImages()
        {
            //goldsmith_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.goldsmith_station)[0];
            bow_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.bow_station)[0];
            armor_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.armour_station)[0];
            cook_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.cook_station)[0];
            vulcan_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.vulcan_station)[0];
            blacksmith_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.blacksmith_station)[0];
            healer_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.healer_station)[0];
            salesman_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.salesman_station)[0];
            lumberjack_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.lumberjack_station)[0];
            miner_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.miner_station)[0];

            weaponsmith_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.weaponsmith_station)[0];
            tailor_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.tailor_station)[0];
            wiselady_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.wiselady_cabin)[0];
            priest_station = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.priest_station)[0];
            
        }

        GameObjects.EnvironmentObj.MapChunkObjectType type;
        public QuestTip QuestTip;
     
        public Map.WorldPosition wp;
       
        //GameObjects.NPC.AbsNPC obj;

        public NPCdata()
        { }

        //public NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType type, Map.WorldPosition wp)
        //    :this(type, wp, GenerateGameObject.Now)
        //{ }
        public NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType type, Map.WorldPosition wp, bool generateGO)
        {
           
            this.type = type;
            this.wp = wp;
            //if (generateType == GenerateGameObject.Now)
            if (generateGO)
            {
               
                generate();

            }
                //else if (generateType == GenerateGameObject.Unthread)
            //    beginGenerate();
        }

        //public NPCdata(NPCdataArgs args, GameObjects.EnvironmentObj.MapChunkObjectType type)
        //    :base(args.wp.ChunkGrindex, args.FromLoading, args.GenerateNow)
        //{
        //    this.type = type;
        //    wp = args.wp;
        //    Start(args.wp.ChunkGrindex, args.GenerateNow);
        //}
        
        //public GameObjects.NPC.AbsNPC NPCObj
        //{
        //    set { obj = value; }
        //}

        //override public void ChunkDeleteEvent()
        //{
        //    if (obj != null && !obj.IsDeleted)
        //        obj.UnthreadedDeleteMe();
        //}
        //override public void RemoveFromChunk()
        //{
        //    LfRef.chunks.GetScreen(obj.SpawnChunk).AddChunkObject(this, false);
        //}

        
        //public override void GenerateGameObjects(Map.Chunk chunk, Map.WorldPosition chunkCenterPos, bool dataGenerated)
        //{
        //    base.GenerateGameObjects(chunkPos, dataGenerated);
        //    generateCharachter();

        //    LfRef.chunks.GetScreen(chunkPos).AddChunkObject(this, true);
        //}

        public bool FirstVisit = true;

        //public void BeginGenerateCharacter(Map.WorldPosition wp, bool chunkLoad)
        //{
        //    System.Diagnostics.Debug.WriteLine("BeginGenerateCharacter:" + this.ToString());
        //    if (!chunkLoad)
        //        LfRef.chunks.GetScreen(wp).AddChunkObject(this, true);
        //    this.wp = wp;
        //}
        void generateCharachter()
        {
            if (type != GameObjects.EnvironmentObj.MapChunkObjectType.Messenger)
            {
               // System.Diagnostics.Debug.WriteLine("generateCharachter:" + this.ToString());
                generate();
            }
        }
        //public void beginGenerate()
        //{
        //    new Timer.Action0ArgTrigger(generate);
        //}

        

        public void generate()
        {
            LfRef.worldOverView.EnvironmentObjectQue.AddGeneratorRequest(new Director.WaitingEnvObjGenerator(this, wp.ChunkGrindex));
        }

        public void GenerateGameObjects(Map.Chunk chunk, Map.WorldPosition chunkCenterPos, bool dataGenerated)
        {
            //Got green light from the env generator que
            GameObjects.AbsUpdateObj obj;

            switch (type)
            {
                case GameObjects.EnvironmentObj.MapChunkObjectType.BasicNPC:
                    obj = new GameObjects.NPC.BasicNPC(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Guard:
                    obj = new GameObjects.NPC.Guard(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Healer:
                    obj = new GameObjects.NPC.Healer(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Horse_Transport:
                    obj = new GameObjects.NPC.HorseTransport(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.ImError:
                    obj = new GameObjects.NPC.ImError(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.ImGlitch:
                    obj = new GameObjects.NPC.ImError(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.ImBug:
                    obj = new GameObjects.NPC.ImError(wp, this);
                    break;


                case GameObjects.EnvironmentObj.MapChunkObjectType.Salesman:
                    obj = new GameObjects.NPC.SalesMan(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Lumberjack:
                    obj = new GameObjects.NPC.Lumberjack(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Miner:
                    obj = new GameObjects.NPC.Miner(wp, this);
                    break;

                case GameObjects.EnvironmentObj.MapChunkObjectType.Father:
                    obj = new GameObjects.NPC.Father(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Mother:
                    obj = new GameObjects.NPC.Mother(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Guard_Captain:
                    obj = new GameObjects.NPC.GuardCaptain(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Granpa:
                    obj = new GameObjects.NPC.GranPa(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.War_veteran:
                    obj = new GameObjects.NPC.WarVeteran(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.DebugNPC:
                    obj = new GameObjects.NPC.DebugNPC(wp, this);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Builder:
                    obj = new GameObjects.NPC.Builder(wp, this);
                    break;



                default:
                    if (
                        type == MapChunkObjectType.Armor_smith ||
                        type == MapChunkObjectType.Blacksmith ||
                        type == MapChunkObjectType.Bow_maker ||
                        type == MapChunkObjectType.Cook ||
                        type == MapChunkObjectType.High_priest ||
                        type == MapChunkObjectType.Tailor ||
                        type == MapChunkObjectType.Weapon_Smith ||
                        type == MapChunkObjectType.Wise_Lady ||
                        type == MapChunkObjectType.Volcan_smith
                        )
                    {
                        obj = new GameObjects.NPC.Worker(wp, this);
                    }
                    else
                    {
                        throw new NotImplementedException("generate npc: " + type.ToString());
                    }

                    break;
            }

            chunk.AddConnectedObject(obj);
        }

        public static NPCdata LoadNPC(GameObjects.EnvironmentObj.MapChunkObjectType type, System.IO.BinaryReader r, byte version, IntVector2 chunkIndex, bool fromNet)
        {
            NPCdata npc = new NPCdata(type, new WorldPosition(chunkIndex), false);
            npc.Load(r, version, chunkIndex, fromNet);
            return npc;
        }
        public void Load(System.IO.BinaryReader r, byte version, IntVector2 chunkIndex, bool fromNet)
        {
            wp = new Map.WorldPosition(chunkIndex);
            ReadStream(r, version);
        }
        public void ReadStream(System.IO.BinaryReader r, byte version)
        {
            wp.ReadChunkObjXZ(r);
        }

//        public static NPCdata GetNPC(GameObjects.EnvironmentObj.MapChunkObjectType type, Map.WorldPosition wp)
//        {
//            switch (type)
//            {
//                default:
//                    return new NPCdata(type, wp, GenerateGameObject.NON);
////                case GameObjects.EnvironmentObj.MapChunkObjectType.BasicNPC:
////                    return new BasicNPCData(args);
////                case GameObjects.EnvironmentObj.MapChunkObjectType.Armour_maker:
////                    return new WorkerArmour(args);
////                case GameObjects.EnvironmentObj.MapChunkObjectType.Bow_maker:
////                    return new WorkerBowmaker(args);
////                case GameObjects.EnvironmentObj.MapChunkObjectType.Cook:
////                    return new WorkerCook(args);
////                case GameObjects.EnvironmentObj.MapChunkObjectType.Gold_smith:
////                    return new WorkerGoldSmith(args);
////                case GameObjects.EnvironmentObj.MapChunkObjectType.Shield_maker:
////                    return new WorkerShieldMaker(args);
////                case GameObjects.EnvironmentObj.MapChunkObjectType.Weapon_Smith:
////                    return new WorkerWeaponSmith(args);
////                case GameObjects.EnvironmentObj.MapChunkObjectType.Volcan_smith:
////                    return new WorkerVulcanSmith(args);

////                case GameObjects.EnvironmentObj.MapChunkObjectType.Healer:
////                    return new HealerData(args);
////                case GameObjects.EnvironmentObj.MapChunkObjectType.Guard:
////                    return new GuardData(args);
                
////#if !CMODE
////                case GameObjects.EnvironmentObj.MapChunkObjectType.Guard_Captain:
////                    return new GuardCaptainData(args);
////                case GameObjects.EnvironmentObj.MapChunkObjectType.Father:
////                    return new FatherData(args);
////#endif
////                case GameObjects.EnvironmentObj.MapChunkObjectType.Horse_Transport:
////                    return new TransportData(args);
////                case GameObjects.EnvironmentObj.MapChunkObjectType.Salesman:
////                    return new SalesManData(args);
////                case GameObjects.EnvironmentObj.MapChunkObjectType.Granpa:
////                    return new GranPaData(args);
//            }
//            throw new NotImplementedException("GetNPC() " + type.ToString());
//            //return null;
//        }

        //override public void WriteStream(System.IO.BinaryWriter w)
        //{
        //    //position.WriteStream(w);
        //    if (obj != null)
        //        wp = obj.SpawnChunk;//obj.SpawnChunk.WriteChunkObjXZ(w);
        //    wp.WriteChunkObjXZ(w);
        //}
        

        //override public void ChunkLoadingComplete()
        //{
        //    //obj.SetStartY();
        //    //image.Position.Y = LfRef.chunks.GetScreen(WorldPosition).GetGroundY(WorldPosition) + 1;
        //}

        //override public bool NeedToBeStored { get { return false; } }
        public Voxels.VoxelObjGridData WorkingStation 
        {
            get
            {
                return WorkingStationFromType(type);
            }
        
        }

        public static void BuildStationOnChunk(GameObjects.EnvironmentObj.MapChunkObjectType npcType, WorldPosition wp)
        {
            Voxels.VoxelObjGridData stationImg = WorkingStationFromType(npcType);
            if (stationImg != null)
            {
                wp.Y = Map.WorldPosition.ChunkStandardHeight;
                stationImg.BuildOnTerrain(wp);
            }
        }

        static Voxels.VoxelObjGridData WorkingStationFromType(GameObjects.EnvironmentObj.MapChunkObjectType npcType)
        {
            switch (npcType)
            {
                case GameObjects.EnvironmentObj.MapChunkObjectType.Armor_smith:
                    return armor_station;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Bow_maker:
                    return bow_station;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Cook:
                    return cook_station;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Healer:
                    return healer_station;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Lumberjack:
                    return lumberjack_station;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Miner:
                    return miner_station;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Salesman:
                    return salesman_station;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Weapon_Smith:
                    return weaponsmith_station;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Volcan_smith:
                    return vulcan_station;

                case GameObjects.EnvironmentObj.MapChunkObjectType.Blacksmith:
                    return blacksmith_station;
                case GameObjects.EnvironmentObj.MapChunkObjectType.High_priest:
                    return priest_station;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Wise_Lady:
                    return wiselady_station;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Tailor:
                    return tailor_station;
                
                default:
                    return null;
            }
        }

        public GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
        {
            get { return type; }
            set { type = value; }
        }

        public override string ToString()
        {
            return TextLib.EnumName(type.ToString());
        }
        
    }

    enum GenerateGameObject
    {
        Now,
        Unthread,
        NON,
    }

//    class TransportData : AbsNPCdata
//    {
//        public TransportData(NPCdataArgs args)
//            : base(args)
//        { }
//        public override GameObjects.NPC.AbsNPC2 generate(Map.WorldPosition wp)
//        {
//            return new LootfestLib.TravelCost(wp, this);
//        }
//        public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
//        {
//            get { return GameObjects.EnvironmentObj.MapChunkObjectType.Horse_Transport; }
//        }

//         override public Voxels.VoxelObjGridData WorkingStation { get { return null; } }
//         public const string Name = "Horse transport";
//         public override string ToString()
//         {
//             return Name;
//         }
//    }

//    class GuardData : AbsNPCdata
//    {
//        public GuardData(NPCdataArgs args)
//            : base(args)
//        { }

//        public override GameObjects.NPC.AbsNPC2 generate(Map.WorldPosition wp)
//        {
//            return new GameObjects.NPC.Guard(wp, this);
//        }

//        public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
//        {
//            get { return GameObjects.EnvironmentObj.MapChunkObjectType.Guard; }
//        }

//        override public Voxels.VoxelObjGridData WorkingStation { get { return null; } }
//    }
//#if !CMODE
//    class FatherData : AbsNPCdata
//    {
//        public FatherData(NPCdataArgs args)
//            : base(args)
//        { }
//        public override GameObjects.NPC.AbsNPC2 generate(Map.WorldPosition wp)
//        {
//            return new GameObjects.NPC.Father(wp, this);
//        }
//        public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
//        {
//            get { return GameObjects.EnvironmentObj.MapChunkObjectType.Father; }
//        }
//        override public Voxels.VoxelObjGridData WorkingStation { get { return null; } }

//        public const string Name = "Father";
//        public override string ToString()
//        {
//            return Name;
//        }
//    }
//class GuardCaptainData : GuardData
//    {
//    public GuardCaptainData(NPCdataArgs args)
//            : base(args)
//        { }
//        public override GameObjects.NPC.AbsNPC2 generate(Map.WorldPosition wp)
//        {
//            return new GameObjects.NPC.GuardCaptain(wp, this);
//        }
//        public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
//        {
//            get { return GameObjects.EnvironmentObj.MapChunkObjectType.Guard_Captain; }
//        }

//         override public Voxels.VoxelObjGridData WorkingStation { get { return null; } }
//         public const string Name = "Guard Captain";
//         public override string ToString()
//         {
//             return Name;
//         }
//    }

//#endif
//    class GranPaData : AbsNPCdata
//    {
//        public GranPaData(NPCdataArgs args)
//            : base(args)
//        { }
//        public override GameObjects.NPC.AbsNPC2 generate(Map.WorldPosition wp)
//        {
//            return new GameObjects.NPC.GranPa(wp, this);
//        }
//        public override GameObjects.EnvironmentObj.MapChunkObjectType MapChunkObjectType
//        {
//            get { return GameObjects.EnvironmentObj.MapChunkObjectType.Granpa; }
//        }

//         override public Voxels.VoxelObjGridData WorkingStation { get { return null; } }
//         public const string Name = "GranPa";
//         public override string ToString()
//         {
//             return Name;
//         }
//    }

    
}
