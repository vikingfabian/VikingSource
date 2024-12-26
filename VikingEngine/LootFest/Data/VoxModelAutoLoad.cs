using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using VikingEngine.Voxels;

namespace VikingEngine.LootFest.Data
{
    class VoxModelAutoLoad
    {
        Dictionary<VoxelModelName, Graphics.VoxelModel> models_loaded = new Dictionary<VoxelModelName, Graphics.VoxelModel>();
        Dictionary<VoxelModelName, WaitingToAutoLoad> models_waitingToLoad = new Dictionary<VoxelModelName, WaitingToAutoLoad>();

        public VoxModelAutoLoad()
        {
            LfRef.modelLoad = this;
        }

        public Graphics.VoxelModel GetMasterImage(VoxelModelName model)
        {
            Graphics.VoxelModel result;
            
            models_loaded.TryGetValue(model, out result);
            
            return result;
        }
        public void SetMasterImage(VoxelModelName model, Graphics.VoxelModel master)
        {
            lock (models_loaded)
            {
                lib.DictionaryAddOrReplace(models_loaded, model, master);
            }
        }


        public void PreLoadImage(VoxelModelName name, bool animated, float yAdjust, bool centerY)
        {
            lock (models_loaded)
            {
                lock (models_waitingToLoad)
                {
                    if (!models_loaded.ContainsKey(name) && !models_waitingToLoad.ContainsKey(name))
                    {
                        models_waitingToLoad.Add(name, new WaitingToAutoLoad(name, null, 1, yAdjust, centerY));
                    }
                }
            }
        }

        public Graphics.AbsVoxelObj AutoLoadModelInstance(
            VoxelModelName name,
            float scale = 1f,
            float yAdjust = 0f, bool centerY = false,
            bool addToRender = true)
        {
            //if (name == VoxelModelName.goblin1)
            //{
            //    lib.DoNothing();
            //}



             Graphics.VoxelModelInstance instance = new Graphics.VoxelModelInstance(null, addToRender);
#if DEBUG
            Debug.CrashIfThreaded();
            instance.DebugName = name.ToString();
#endif


            Graphics.VoxelModel master;
            lock (models_loaded)
            {
                if (models_loaded.TryGetValue(name, out master))
                {
                    instance.SetMaster(master);
                    if (scale > 0)
                    {
                        instance.scale = VectorExt.V3(instance.SizeToScale * scale);
                    }
                }
                else
                {
                    const int MaxLoadedImages = 32;
                    if (models_loaded.Count >= MaxLoadedImages)
                    {
                        List<VoxelModelName> keys = new List<VoxelModelName>(models_loaded.Keys);
                        var removeModel = keys[Ref.rnd.Int(keys.Count)];
                        models_loaded.Remove(removeModel);
                    }

                    lock (models_waitingToLoad)
                    {
                        if (models_waitingToLoad.ContainsKey(name))
                        {
                            models_waitingToLoad[name].AddWaitingObj(instance);
                        }
                        else
                        {
                            models_waitingToLoad.Add(name, new WaitingToAutoLoad(name, instance, scale, yAdjust, centerY));
                        }
                    }
                }
            }

            return instance;
        }

        public void ModelDoneAutoLoading(VoxelModelName name, Graphics.VoxelModel master)
        {
            SetMasterImage(name, master);
            lock (models_waitingToLoad)
            {
                models_waitingToLoad.Remove(name);
            }
        }

        public void debugMenu(VikingEngine.HUD.Gui menu)
        {
            VikingEngine.HUD.GuiLayout layout = new VikingEngine.HUD.GuiLayout("Vox models autoloader", menu);
            {
                new VikingEngine.HUD.GuiLabel("models_loaded", layout);
                foreach (var kv in models_loaded)
                {
                    new VikingEngine.HUD.GuiTextButton(kv.Key.ToString(), null, new VikingEngine.HUD.GuiNoAction(), false, layout);
                }
                
                new VikingEngine.HUD.GuiSectionSeparator(layout);

                new VikingEngine.HUD.GuiLabel("models_waitingToLoad", layout);
                foreach (var kv in models_waitingToLoad)
                {
                    new VikingEngine.HUD.GuiTextButton(kv.Key.ToString(), null, new VikingEngine.HUD.GuiNoAction(), false, layout);
                }
            }
            layout.End();
        }
    }

    class WaitingToAutoLoad : DataLib.StorageTaskWithQuedProcess
    {
        //public float TimeAlive = 0;
        List<Frame> framesData;
        IVerticeData verticeData;
        IntVector3 gridSz;

        List<Graphics.AbsVoxelModelInstance> instances;
        VoxelModelName name;
        float scale;
        float yAdjust;
        bool centerY;

        public WaitingToAutoLoad(VoxelModelName name, Graphics.AbsVoxelModelInstance instance,
            float scale, float yAdjust, bool centerY)
            : base(false, Editor.VoxelObjDataLoader.ContentPath(name), true)
        {
            this.yAdjust = yAdjust;
            this.centerY = centerY;
            this.scale = scale;
            this.name = name;
            runSynchTrigger = true;
            instances = new List<Graphics.AbsVoxelModelInstance>(1);
            AddWaitingObj(instance);
#if DEBUG
            System.Diagnostics.Debug.WriteLine("begin vox load: " + name);
#endif 
            beginAutoTasksRun();
        }
        public void AddWaitingObj(Graphics.AbsVoxelModelInstance instance)
        {
            instances.Add(instance);
        }
        public override void ReadStream(System.IO.BinaryReader r)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("begin vox read: " + name);
#endif 
            Vector3 centerAdjust = new Vector3(0, yAdjust, 0);

            List<VoxelObjGridDataHD> loadedFrames = LootFest.Editor.VoxelObjDataLoader.LoadVoxelObjGridHD(r);
            
            if (centerY)
                centerAdjust += loadedFrames[0].CenterAdj();
            else
                centerAdjust += loadedFrames[0].BottomCenterAdj();

            gridSz = loadedFrames[0].Size;

            verticeData = Editor.VoxelObjBuilder.BuildVerticesHD(loadedFrames, centerAdjust, out framesData);

#if DEBUG
            System.Diagnostics.Debug.WriteLine("end vox read: " + name);
#endif 
            //master = Editor.VoxelObjBuilder.BuildModelHD(
            //   loadedFrames, centerAdjust);

            //master = Editor.VoxelObjDataLoader.ReadVoxelObjAnimHD(r, new Vector3(0, yAdjust, 0), centerY);
        }

        public override void runSyncAction()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("vox main task: " + name);
#endif 
            base.runSyncAction();

            Graphics.VoxelModel master = Editor.VoxelObjBuilder.BuildModelHD(verticeData, gridSz, framesData);


            LfRef.modelLoad.ModelDoneAutoLoading(name, master);
            int i = 0;
            while (i < instances.Count)
            {
                if (instances[i] != null)
                {
                    instances[i].SetMaster(master);
                    if (scale > 0)
                    {
                        instances[i].scale = VectorExt.V3(instances[i].SizeToScale * scale);
                    }
                }

                ++i;
            }
        }
    }
}
