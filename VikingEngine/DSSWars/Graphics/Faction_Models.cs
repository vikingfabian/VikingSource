using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VikingEngine.DebugExtensions;
using VikingEngine.Graphics;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars
{
    partial class Faction
    {
        Dictionary<int, Graphics.VoxelModel> models_loaded =
           new Dictionary<int, Graphics.VoxelModel>();

        List<int> processStarted = new List<int>(8);

        public Graphics.VoxelModelInstance AutoLoadModelInstance(VoxelModelName name,
           float scale = 1f, bool addToRender = false)
        {

            Graphics.VoxelModelInstance instance = new Graphics.VoxelModelInstance(null, addToRender);

            instance.scale.X = scale;
            instance.scale.Y = 0;
#if DEBUG
            instance.DebugName = name.ToString();
#endif
            getOrCreateMaster(name, instance);
            return instance;
        }

        public VoxelModelInstance_Pooled AutoLoadModelInstance_batched(VoxelModelName name,
           float scale = 1f)
        {

            VoxelModelInstance_Pooled instance = DssRef.models.NextInstance_Pooled();
#if DEBUG
            instance.DebugName = name.ToString() + ", fac" + parentArrayIndex.ToString();
#endif
            instance.scale.X = scale;
            instance.scale.Y = 0;

            getOrCreateMaster(name, instance);

            Ref.draw.drawBatch.Add(instance);

            return instance;
        }

        public VoxelModelInstance_Pooled AutoLoadModelInstance_character(SoldierModelData modelData,
          float scale = 1f)
        {

            VoxelModelInstance_Pooled instance = DssRef.models.NextInstance_Pooled();
#if DEBUG
            instance.DebugName = modelData.ToString() + ", fac" + parentArrayIndex.ToString();
#endif
            instance.scale.X = scale;
            instance.scale.Y = 0;

            Graphics.VoxelModel master = null;

            int id = modelData.GetHashCode();
            models_loaded.TryGetValue(id, out master);

            if (master != null)
            {
                setMaster(instance, master);
            }
            else
            {
                Task.Run(async () =>
                {
                    try
                    {
                        int numLoops = 0;

                        {
                            bool process = false;

                            lock (processStarted)
                            {
                                if (!processStarted.Contains(id))
                                {
                                    process = true;
                                    processStarted.Add(id);
                                }
                            }

                            if (process)
                            {
                                var model = new CharacterModelBuilder().buildModel(this, modelData);
                                lock (models_loaded)
                                {
                                    if (!models_loaded.ContainsKey(id))
                                    {
                                        models_loaded.Add(id, model);
                                    }
                                }
                            }
                        }

                        while (!models_loaded.TryGetValue(id, out master))
                        {
                            if (++numLoops > 1000)
                            {
                                //lib.DoNothing();
                                BlueScreen.ThreadException = new EndlessLoopException("Load faction master " + modelData.ToString());
                            }
                            await Task.Delay(100);
                        }

                        setMaster(instance, master);
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }

                });

            }

            Ref.draw.drawBatch.Add(instance);

            return instance;
        }

        private void getOrCreateMaster(VoxelModelName name, VoxelModelInstance instance)
        {
            Graphics.VoxelModel master = null;

            models_loaded.TryGetValue((int)name, out master);

            if (master != null)
            {
                setMaster(instance, master);
            }
            else
            {
                Task.Run(async () =>
                {
                    try
                    {
                        int numLoops = 0;
//#if DEBUG
//                        if (!DssRef.models.rawModels.ContainsKey(name))
//                        {
//                            lib.DoNothing();
//                        }
//#endif
                        var grid = DssRef.models.rawModels[name];


                        generateFromGrid_asynch(name, grid);

                        while (!models_loaded.TryGetValue((int)name, out master))
                        {
                            if (++numLoops > 1000)
                            {
                                //lib.DoNothing();
                                BlueScreen.ThreadException = new EndlessLoopException("Load faction master " + name);
                            }
                            await Task.Delay(100);
                        }

                        setMaster(instance, master);
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }

                });

            }
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
                if (!processStarted.Contains((int)name))
                {
                    process = true;
                    processStarted.Add((int)name);
                }
            }

            if (process)
            {
                var model = new FactionModelBuilder().buildModel(this, name, grid);
                lock (models_loaded)
                {
                    if (!models_loaded.ContainsKey((int)name))
                    {
                        models_loaded.Add((int)name, model);
                    }
                }
            }
        }

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

    
}
