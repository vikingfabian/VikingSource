using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars
{
    partial class Faction
    {
        Dictionary<VoxelModelName, Graphics.VoxelModel> models_loaded =
           new Dictionary<VoxelModelName, Graphics.VoxelModel>();

        List<VoxelModelName> processStarted = new List<VoxelModelName>(8);

        public Graphics.AbsVoxelObj AutoLoadModelInstance(VoxelModelName name,
           float scale = 1f,
           //float yAdjust = 0f, bool centerY = false,
           bool addToRender = false)
        {
            //Debug.CrashIfThreaded();
            Graphics.VoxelModelInstance instance = new Graphics.VoxelModelInstance(null, addToRender);
            
            instance.scale.X = scale;
            instance.scale.Y = 0;
#if DEBUG
            instance.DebugName = name.ToString();
#endif
            Graphics.VoxelModel master = null;

            //lock (models_loaded)
            //{
                models_loaded.TryGetValue(name, out master);
            //}

            if (master != null)
            {
                //instance.SetMaster(master);
                //if (instance.scale.X > 0)
                //{
                //    instance.scale = VectorExt.V3(instance.SizeToScale * instance.scale.X);
                //}
                setMaster(instance, master);    
            }
            else
            {
                //instance.scale.X = scale;
                //instance.scale.Y = 0;

                Task.Run(async () =>
                {
                    int numLoops = 0;
#if DEBUG
                    if (!DssRef.models.rawModels.ContainsKey(name))
                    {
                        lib.DoNothing();
                    }
#endif
                    var grid = DssRef.models.rawModels[name];//.Get(name, true);

                    //while (grid == null)
                    //{
                    //    if (++numLoops > 500)
                    //    {
                    //        BlueScreen.ThreadException = new Exception("Model locked in processing: " + name.ToString()); 
                    //    }
                    //    await Task.Delay(100);
                    //    grid = DssRef.modelsRaw.Get(name, false);
                    //}

                    generateFromGrid_asynch(name, grid);

                    while (!models_loaded.TryGetValue(name, out master))
                    {
                        if (++numLoops > 1000)
                        {
                            lib.DoNothing();
                        }
                        await Task.Delay(100);
                    }

                    if (master == null)
                    {
                        lib.DoNothing();
                    }
                    setMaster(instance, master);
                });

                //var grid = DssRef.modelsRaw.Get(name);
                
                //if (grid != null)
                //{
                //    new Timer.Asynch2ArgTrigger<VoxelModelName, VoxelObjGridDataAnimHD>(
                //        generateFromGrid_asynch, name, grid, false);
                //}
            }
            return instance;
        }

        void setMaster(Graphics.VoxelModelInstance instance, Graphics.VoxelModel master)
        {
            instance.SetMaster(master);
            if (instance.scale.X > 0)
            {
                instance.scale = VectorExt.V3(instance.SizeToScale * instance.scale.X);
            }
        }

        public void OnRawModelLoaded_asynch(VoxelModelName name, VoxelObjGridDataAnimHD grid)
        {
            generateFromGrid_asynch(name, grid);
        }

        void generateFromGrid_asynch(VoxelModelName name, VoxelObjGridDataAnimHD grid)
        {
            bool process = false;

            lock (processStarted)
            {
                if (!processStarted.Contains(name))
                {
                    process = true;
                    processStarted.Add(name);
                }
            }

            if (process)
            {
                //new FactionModelBuilder(this, name, grid);
                var model = new FactionModelBuilder().buildModel(this, name, grid);
                lock (models_loaded)
                {
                    if (!models_loaded.ContainsKey(name))
                    {
                        models_loaded.Add(name, model);
                    }
                }
            }
        }

        //public void onNewModel(VoxelModelName name, Graphics.VoxelModel model)
        //{
        //    lock (models_loaded)
        //    {
        //        if (!models_loaded.ContainsKey(name))
        //        {
        //            models_loaded.Add(name, model);
        //        }
        //    }

        //    //var armies = player.faction.armies.counter();
        //    //while (armies.Next())
        //    //{
        //    //    armies.sel.onNewModel(name, model);
        //    //}

        //    //var cities = player.faction.cities.counter();
        //    //while (cities.Next())
        //    //{
        //    //    cities.sel.onNewModel(name, model);
        //    //}
        //}

        public static void SetNewMaster(LootFest.VoxelModelName newModelName, LootFest.VoxelModelName myModelName, 
            Graphics.AbsVoxelObj model, Graphics.VoxelModel master)
        {
            if (model != null && newModelName == myModelName)
            {
                model.SetMaster(master);

                if (model.scale.Y == 0)
                {
                    model.scale = VectorExt.V3(model.SizeToScale * model.scale.X);
                }
            }
        }
    }

    class FactionModelBuilder : Voxels.ModelBuilder
    {
        static readonly IntVector3 TroopBannerStart = new IntVector3(1, 44, 2);
        static readonly IntVector3 HorseBannerStart = new IntVector3(3, 50, 0);
        static readonly IntVector3 CityBannerStart = new IntVector3(6, 44, 0);
        static readonly IntVector3 ArmyBannerStart = new IntVector3(1, 0, 1);
        static readonly IntVector3 ArmyStandStart = new IntVector3(8, 32, 8);
        static readonly IntVector3 ArmyShipStart = new IntVector3(8, 25, 8);
        static readonly IntVector3 CityIconStart = new IntVector3(3, 2, 3);

        //Faction faction;
        //VoxelModelName name;

        //public FactionModelBuilder(Faction faction, VoxelModelName name, VoxelObjGridDataAnimHD grid)
        //   : base()
        //{
        //    this.faction = faction;
        //    this.name = name;

        //    VoxelObjGridDataAnimHD copy = grid.Clone();
        //    copy.ReplaceMaterial(faction.profile.modelColorReplace);

        //    switch (name)
        //    {
        //        case VoxelModelName.banner:
        //            addFlagTexture(copy, TroopBannerStart, true);
        //            break;
        //        case VoxelModelName.horsebanner:
        //            addFlagTexture(copy, HorseBannerStart, true);
        //            break;
        //        case VoxelModelName.citybanner:
        //            addFlagTexture(copy, CityBannerStart, true);
        //            break;
        //        case VoxelModelName.armystand:
        //            addFlagTexture(copy, ArmyStandStart, true);
        //            break;
        //        case VoxelModelName.armybanner:
        //            addFlagTexture(copy, ArmyBannerStart, false);
        //            break;
        //        case VoxelModelName.cityicon:
        //            addFlagTexture(copy, CityIconStart, false);
        //            break;
        //    }

        //    var centerAdjust = grid.Frames[0].BottomCenterAdj();


        //    buildVerticeDataHD(copy.Frames, centerAdjust);


        //    new Timer.Action0ArgTrigger(synchedUpdate);            
        //}

        public Graphics.VoxelModel buildModel(Faction faction, VoxelModelName name, VoxelObjGridDataAnimHD grid)
        {
            //this.faction = faction;
            //this.name = name;

            VoxelObjGridDataAnimHD copy = grid.Clone();
            copy.ReplaceMaterial(faction.profile.modelColorReplace);

            switch (name)
            {
                case VoxelModelName.banner:
                    addFlagTexture(faction, copy, TroopBannerStart, true);
                    break;
                case VoxelModelName.horsebanner:
                    addFlagTexture(faction, copy, HorseBannerStart, true);
                    break;
                case VoxelModelName.citybanner:
                    addFlagTexture(faction, copy, CityBannerStart, true);
                    break;
                case VoxelModelName.armystand:
                    addFlagTexture(faction, copy, ArmyStandStart, true, 0);
                    addFlagTexture(faction, copy, ArmyShipStart, true, 1);
                    break;
                case VoxelModelName.armybanner:
                    addFlagTexture(faction, copy, ArmyBannerStart, false);
                    break;
                case VoxelModelName.cityicon:
                    addFlagTexture(faction, copy, CityIconStart, false);
                    break;
            }

            var centerAdjust = grid.Frames[0].BottomCenterAdj();


            buildVerticeDataHD(copy.Frames, centerAdjust);

            Graphics.VoxelModel model = modelFromVertices();

            return model;
        }
        //void synchedUpdate()
        //{
        //    Graphics.VoxelModel model = modelFromVertices();

        //    faction.onNewModel(name, model);
        //}

        void addFlagTexture(Faction faction, VoxelObjGridDataAnimHD grid, IntVector3 start, bool standing, int frame =0)
        {
            var gridData = grid.Frames[frame];

            var flagLoop = faction.profile.flagDesign.dataGrid.LoopInstance();
            while (flagLoop.Next())
            {
                byte colId = faction.profile.flagDesign.dataGrid.Get(flagLoop.Position);
                var blockCol = faction.profile.blockColors[colId];

                IntVector3 gridPos = start;
                gridPos.X += flagLoop.Position.X;
                if (standing)
                {
                    gridPos.Y -= flagLoop.Position.Y; //inverted
                }
                else
                {
                    gridPos.Z += flagLoop.Position.Y;
                }
                gridData.Set(gridPos, blockCol);
            }
        }
    }
}
