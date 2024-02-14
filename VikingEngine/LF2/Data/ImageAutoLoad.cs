using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.Data
{
    static class ImageAutoLoad
    {
        static Dictionary<VoxelModelName, Graphics.VoxelModel> autoLoadedImages = new Dictionary<VoxelModelName,
           Graphics.VoxelModel>();
        static Dictionary<VoxelModelName, WaitingToAutoLoad> waitingImages = new Dictionary<VoxelModelName, WaitingToAutoLoad>();


        public static Graphics.VoxelModel GetMasterImage(VoxelModelName model)
        {
            Graphics.VoxelModel result;
            autoLoadedImages.TryGetValue(model, out result);
            return result;
        }
        public static void SetMasterImage(VoxelModelName model, Graphics.VoxelModel master)
        {
            lib.DictionaryAddOrReplace(autoLoadedImages, model, master);
        }


        public static void PreLoadImage(VoxelModelName name, bool animated, float yAdjust)
        {
            if (!autoLoadedImages.ContainsKey(name) && !waitingImages.ContainsKey(name))
            {
                waitingImages.Add(name, new WaitingToAutoLoad(name, null,
                    animated ? Graphics.AnimationsSettings.BasicAnimation : Graphics.AnimationsSettings.OneFrame, 1, yAdjust));
            }
        }


        public static  Graphics.AbsVoxelObj AutoLoadImgReplacement(Graphics.AbsVoxelObj originalImage,
             VoxelModelName name, IReplacementImage replacementImg, float yAdjust)
        {
            return AutoLoadImgReplacement(originalImage, name, replacementImg, yAdjust, Graphics.AnimationsSettings.OneFrame);
        }

        public static  Graphics.AbsVoxelObj AutoLoadImgReplacement(Graphics.AbsVoxelObj originalImage,
            VoxelModelName name, IReplacementImage replacementImg, float yAdjust, Graphics.AnimationsSettings animation)
        {
            Graphics.AbsVoxelObj result = AutoLoadImgInstace(name, replacementImg, 0, yAdjust, animation);
            result.position = originalImage.position;
            result.Rotation = originalImage.Rotation;
            result.scale = originalImage.scale;
            originalImage.DeleteMe();
            return result;
        }

        public static  Graphics.AbsVoxelObj AutoLoadImgInstace(VoxelModelName name,
            IReplacementImage replacementImg, float scale, float yAdjust)
        {
            return AutoLoadImgInstace(name, replacementImg, scale, yAdjust, Graphics.AnimationsSettings.OneFrame);
        }
        public static  Graphics.AbsVoxelObj AutoLoadImgInstace(
            VoxelModelName name,
            IReplacementImage replacementImg,
            float scale,
            float yAdjust,
            Graphics.AnimationsSettings animationsSettings)
        {
            if (autoLoadedImages.ContainsKey(name))
            {
                Graphics.AbsVoxelModelInstance instance;
                if (animationsSettings.Animated)
                {
                    instance = new Graphics.VoxelModelInstance(autoLoadedImages[name], animationsSettings);

                }
                else
                {
                    instance = new Graphics.VoxelModelInstance(autoLoadedImages[name]);
                }
                if (scale > 0)
                {
                    instance.scale = scale * instance.OneScale * Vector3.One;
                }
                return instance;
            }
            else
            {
                const int MaxLoadedImages = 32;
                if (autoLoadedImages.Count >= MaxLoadedImages)
                {
                    List<VoxelModelName> keys = new List<VoxelModelName>(autoLoadedImages.Keys);
                    autoLoadedImages.Remove(keys[Ref.rnd.Int(keys.Count)]);
                }

                //Create a temporary replacement image
                Graphics.AbsVoxelModelInstance instance;


                if (animationsSettings.Animated)
                    instance = new Graphics.VoxelModelInstance(
                        replacementImg.CreateReplacementMasterImg(), 
                        animationsSettings);
                    //instance = LootfestLib.Images.StandardAnimObjInstance(replacementImg, Graphics.AnimationsSettings.OneFrame);
                else
                    instance = new Graphics.VoxelModelInstance(replacementImg.CreateReplacementMasterImg());//LootfestLib.Images.StandardObjInstance(replacementImg);

                //start to load missing image
                if (waitingImages.ContainsKey(name))
                {
                    waitingImages[name].AddWaitingObj(instance, animationsSettings);
                }
                else
                {
                    waitingImages.Add(name, new WaitingToAutoLoad(name, instance, animationsSettings, scale, yAdjust));
                }
                instance.scale = instance.OneScale * scale * Vector3.One;
                return instance;
            }
        }

        public static  void ImageDoneAutoLoading(VoxelModelName name, Graphics.VoxelModel master)
        {
            //autoLoadedImages.Add(name, master);
            SetMasterImage(name, master);
            waitingImages.Remove(name);
        }
    }

    class WaitingToAutoLoad : DataLib.SaveStreamWithQuedProcess
    {
        //readonly float StandardAnimObjHeightAdj = -2;
        public float TimeAlive = 0;
        Graphics.VoxelModel master;
        List<WaitingImgInstace> instances;
        VoxelModelName name;
        bool animated;
        float scale;
        float yAdjust;
        //float heightAdj;

        public WaitingToAutoLoad(VoxelModelName name, Graphics.AbsVoxelModelInstance instance,
            Graphics.AnimationsSettings animationsSettings, float scale, float yAdjust)
            : base(false, Editor.VoxelObjDataLoader.ContentPath(name), true)
        {
            this.yAdjust = yAdjust;
            this.scale = scale;
            this.animated = animationsSettings.Animated;
            this.name = name;
            runSynchTrigger = true;
            instances = new List<WaitingImgInstace>();
            //if (instance != null)
            AddWaitingObj(instance, animationsSettings);
            start();
        }
        public void AddWaitingObj(Graphics.AbsVoxelModelInstance instance, Graphics.AnimationsSettings animationsSettings)
        {
            instances.Add(new WaitingImgInstace(instance, animationsSettings));
        }
        public override void ReadStream(System.IO.BinaryReader r)
        {

            if (animated)
                master = Editor.VoxelObjDataLoader.GetVoxelObjAnim(r, new Vector3(0, yAdjust, 0));
            else
                master = Editor.VoxelObjDataLoader.GetVoxelObj(r, new Vector3(0, yAdjust, 0));
        }
        protected override void MainThreadTrigger()
        {
            //Image is loaded and will raplace the temporary master
           // return;

            Data.ImageAutoLoad.ImageDoneAutoLoading(name, master);
            foreach (WaitingImgInstace i in instances)
            {
                i.SetMaster(master, scale);
            }
        }
    }
    struct WaitingImgInstace
    {
        Graphics.AbsVoxelModelInstance instance;
        Graphics.AnimationsSettings animationsSettings;

        public WaitingImgInstace(Graphics.AbsVoxelModelInstance instance,
            Graphics.AnimationsSettings animationsSettings)
        {
            this.instance = instance;
            this.animationsSettings = animationsSettings;
        }
        public void SetMaster(Graphics.VoxelModel master, float scale)
        {
            if (instance != null)
            {
                instance.SetMaster(master);
                if (scale > 0)
                    instance.scale = master.OneScale * scale * Vector3.One;
                instance.AnimationsSettings = animationsSettings;
            }
        }
    }

    interface IReplacementImage
    {
        Graphics.VoxelModel CreateReplacementMasterImg(); 
    }
    struct TempVoxelReplacementSett : IReplacementImage
    {
        VoxelModelName replacement;
        bool animated;
        public TempVoxelReplacementSett(VoxelModelName replacement, bool animated)
        {  
            this.replacement = replacement;
            this.animated = animated;
        }
        public Graphics.VoxelModel CreateReplacementMasterImg()
        {
            if (animated)
                return  LootfestLib.Images.StandardAnimatedVoxelObjects[replacement];
            else
                return  LootfestLib.Images.StandardVoxelObjects[replacement];
        }
    }
    struct TempBlockReplacementSett : IReplacementImage
    {
        Vector3 scale;
        Color color;
        public TempBlockReplacementSett(Color color, Vector3 scale)
        { 
            this.color = color;
            this.scale = scale;
        }
        public Graphics.VoxelModel CreateReplacementMasterImg()
        {
            return new Graphics.TempVoxelObj(color, scale);
        }
    }
}
