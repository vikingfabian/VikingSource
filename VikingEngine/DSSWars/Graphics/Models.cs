using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.Graphics;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.Editor;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars
{
   
    class Models
    {
        public Dictionary<VoxelModelName, VoxelObjGridDataAnimHD> rawModels = new Dictionary<VoxelModelName, VoxelObjGridDataAnimHD>();
        Dictionary<VoxelModelName, Graphics.VoxelModel> voxelModels = new Dictionary<VoxelModelName, Graphics.VoxelModel>();
        
        List<VoxelModelData> loadedData = new List<VoxelModelData>();
        bool asycTaskComplete = false;

        public Texture2D[] waterTextures;
        
        public Models()
        {
            DssRef.models = this;
        }

        public void loadContent()
        {
            waterTextures = new Texture2D[4];
            for (int i = 1; i <= 4; ++i)
            {
                waterTextures[i-1] = Ref.main.Content.Load<Texture2D>(DssLib.ContentDir + "watertex_i" + i);
            }
            //RAW
            List<VoxelModelName> loadRawModels = new List<VoxelModelName>
            {
                VoxelModelName.war_worker,
                VoxelModelName.war_recruit,
                VoxelModelName.wars_shipcrew,
                VoxelModelName.wars_captain,

                VoxelModelName.banner,
                VoxelModelName.horsebanner,
                VoxelModelName.armystand,
                VoxelModelName.cityicon,
                VoxelModelName.citybanner,
            };

            new AllUnits().AddModelsToLoad(loadRawModels);

            foreach (var modelName in loadRawModels)
            {
                DataStream.FilePath path = VoxelObjDataLoader.ContentPath(modelName);
                byte[] data = DataStream.DataStreamHandler.Read(path);
                System.IO.MemoryStream s = new System.IO.MemoryStream(data);
                System.IO.BinaryReader r = new System.IO.BinaryReader(s);
                
                var grids = VoxelObjDataLoader.LoadVoxelObjGridHD(r);
                var result = new VoxelObjGridDataAnimHD(grids);

                rawModels.Add(modelName, result);
            }


            //VOXEL
            loadVoxelModel(VoxelModelName.war_town1, false);
            loadVoxelModel(VoxelModelName.war_town2, false);
            loadVoxelModel(VoxelModelName.war_town3, false);
            loadVoxelModel(VoxelModelName.war_town_factory, false);
            loadVoxelModel(VoxelModelName.war_workerhut, false);
            loadVoxelModel(VoxelModelName.city_mine, false);
            loadVoxelModel(VoxelModelName.city_workstation, false);

            loadVoxelModel(VoxelModelName.city_dirtwall, false);
            loadVoxelModel(VoxelModelName.city_dirttower, false);
            loadVoxelModel(VoxelModelName.city_woodwall, false);
            loadVoxelModel(VoxelModelName.city_woodtower, false);
            loadVoxelModel(VoxelModelName.city_stonewall, false);
            loadVoxelModel(VoxelModelName.city_stonetower, false);

            loadVoxelModel(VoxelModelName.city_stonehall, false);
            loadVoxelModel(VoxelModelName.city_workerhut, false);
            loadVoxelModel(VoxelModelName.city_pen, false);

            loadVoxelModel(VoxelModelName.city_cobblestone, false);
            loadVoxelModel(VoxelModelName.city_square, false);
            loadVoxelModel(VoxelModelName.city_smallhouse, false);
            loadVoxelModel(VoxelModelName.city_bighouse, false);
            loadVoxelModel(VoxelModelName.city_tavern, false);
            loadVoxelModel(VoxelModelName.city_barracks, false);

            loadVoxelModel(VoxelModelName.Pig, false);
            loadVoxelModel(VoxelModelName.Hen, false);
            loadVoxelModel(VoxelModelName.Arrow, true);
            loadVoxelModel(VoxelModelName.little_javelin, true);
            loadVoxelModel(VoxelModelName.little_boltarrow, true);
            loadVoxelModel(VoxelModelName.war_cannonball, true);
            loadVoxelModel(VoxelModelName.war_ballista_proj, true);
            loadVoxelModel(VoxelModelName.wars_loading_anim, true);
            loadVoxelModel(VoxelModelName.wars_shipbuild, true);

            loadVoxelModel(VoxelModelName.wars_deserter, false);
            loadVoxelModel(VoxelModelName.horse_brown, false);
            loadVoxelModel(VoxelModelName.horse_white, false);
            loadVoxelModel(VoxelModelName.wars_shipmelee, false);
            loadVoxelModel(VoxelModelName.buildarea, false);

            foreach (var model in DetailMapTile.LoadModel())
            {
                loadVoxelModel(model, false);
            }

            asycTaskComplete = true;

            void loadVoxelModel(VoxelModelName modelName, bool centerY)
            {
                float yAdjust = 0;

                DataStream.FilePath path = VoxelObjDataLoader.ContentPath(modelName);
                byte[] data = DataStream.DataStreamHandler.Read(path);
                System.IO.MemoryStream s = new System.IO.MemoryStream(data);
                System.IO.BinaryReader r = new System.IO.BinaryReader(s);

                Vector3 centerAdjust = new Vector3(0, yAdjust, 0);

                List<VoxelObjGridDataHD> loadedFrames = LootFest.Editor.VoxelObjDataLoader.LoadVoxelObjGridHD(r);

                if (centerY)
                    centerAdjust += loadedFrames[0].CenterAdj();
                else
                    centerAdjust += loadedFrames[0].BottomCenterAdj();

                IntVector3 gridSz = loadedFrames[0].Size;

                List<Frame> framesData;
                IVerticeData verticeData = VoxelObjBuilder.BuildVerticesHD(loadedFrames, centerAdjust, out framesData);

                loadedData.Add(new VoxelModelData(modelName, verticeData, gridSz, framesData)); 
            }
        }

        public void sychLoading()
        {
            if (loadedData != null && asycTaskComplete)
            {
                foreach (VoxelModelData data in loadedData)
                {
                    voxelModels.Add(data.name, data.sychedProcessing());
                }
                loadedData = null;
            }
            
        }

        public Graphics.AbsVoxelObj ModelInstance(
           VoxelModelName name,
           float scale = 1f,
           bool addToRender = true)
        {
            Graphics.VoxelModelInstance instance = new Graphics.VoxelModelInstance(null, addToRender);
#if DEBUG
            Debug.CrashIfThreaded();
            instance.DebugName = name.ToString();

            if (!voxelModels.ContainsKey(name))
            {
                lib.DoNothing();
            }
#endif

            Graphics.VoxelModel master = voxelModels[name];
            instance.SetMaster(master);
            if (scale > 0)
            {
                instance.scale = VectorExt.V3(instance.SizeToScale * scale);
            }

            return instance;
        }

        //public Graphics.AbsVoxelObj AutoLoadModelInstance(
        //    VoxelModelName name,
        //    float scale = 1f,
        //    float yAdjust = 0f, bool centerY = false,
        //    bool addToRender = true)
        //{
        //    Graphics.VoxelModelInstance instance = new Graphics.VoxelModelInstance(null, addToRender);

        //}

        //public VoxelObjGridDataAnimHD Get(VoxelModelName name, bool initiateLoading)
        //{
        //    VoxelObjGridDataAnimHD model = null;
        //    lock (loaded)
        //    {
        //        loaded.TryGetValue(name, out model);
        //    }

        //    if (model == null && initiateLoading)
        //    {
        //        lock (loading)
        //        {
        //            if (!loading.Contains(name))
        //            {
        //                loading.Add(name);
        //                new LoadRawModelTask(name);
        //            }
        //        }

        //    }

        //    return model;
        //}

        //public void onModelLoad_asynch(VoxelModelName name, VoxelObjGridDataAnimHD model)
        //{
        //    lock (loaded)
        //    {
        //        loaded.Add(name, model);
        //    }
        //    //lock (loading)
        //    //{
        //    //    loading.Remove(name);
        //    //}

        //    //var factions = DssRef.world.factions.counter();

        //    //while (factions.Next())//var m in DssRef.state.players)
        //    //{
        //    //    factions.sel.OnRawModelLoaded_asynch(name, model);
        //    //}
        //}
    }
    class VoxelModelData
    {
        public VoxelModelName name;

        IVerticeData verticeData;
        IntVector3 gridSz;
        List<Frame> framesData;

        public VoxelModelData(VoxelModelName name, IVerticeData verticeData, IntVector3 gridSz, List<Frame> framesData)
        {
            this.name = name;
            this.verticeData = verticeData;
            this.gridSz = gridSz;
            this.framesData = framesData;
        }

        public VoxelModel sychedProcessing()
        {
            Graphics.VoxelModel master = VoxelObjBuilder.BuildModelHD(verticeData, gridSz, framesData);

            return master;
        }
    }

//    class LoadRawModelTask : DataLib.StorageTaskWithQuedProcess
//    {
//        VoxelModelName name;
//        VoxelObjGridDataAnimHD result;
//        public LoadRawModelTask(VoxelModelName name)
//            : base(false, VoxelObjDataLoader.ContentPath(name), true)
//        {
//            this.name = name;
//#if DEBUG
//            System.Diagnostics.Debug.WriteLine("begin raw load: " + name);
//#endif            
//            beginAutoTasksRun();
//        }

//        public override void ReadStream(System.IO.BinaryReader r)
//        {
//#if DEBUG
//            System.Diagnostics.Debug.WriteLine("begin raw read: " + name);
//#endif 
//            var grids = VoxelObjDataLoader.LoadVoxelObjGridHD(r);
//            result = new VoxelObjGridDataAnimHD(grids);
//#if DEBUG
//            System.Diagnostics.Debug.WriteLine("end raw read: " + name);
//#endif 
//        }

//        protected override void runQuedAsynchTask()
//        {
//#if DEBUG
//            System.Diagnostics.Debug.WriteLine("begin raw asynch: " + name);
//#endif 
//            base.runQuedAsynchTask();
//            DssRef.modelsRaw.onModelL
//            oad_asynch(name, result);

//#if DEBUG
//            System.Diagnostics.Debug.WriteLine("end raw asynch: " + name);
//#endif 
//        }

//        protected override void runQuedMainTask()
//        {
//#if DEBUG
//            System.Diagnostics.Debug.WriteLine("raw main task: " + name);
//#endif 
//            base.runQuedMainTask();
            
//        }

//        public override void Time_Update(float time_ms)
//        {
//            base.Time_Update(time_ms);
//        }
//    }
}
