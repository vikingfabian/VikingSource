using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Voxels;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.LootFest.Process
{
    interface ILoadImage
    {
        void SetCustomImage(Graphics.VoxelModel original, int link);
    }

    //måste förladda alla items innan den genereras

    /// <summary>
    /// Combines animated objects with cloth and other
    /// </summary>
    class ModifiedImage : AbsQuedTasks//OneTimeTrigger, IQuedObject
    {
        protected Graphics.VoxelModel originalMesh;
        protected List<VoxelObjGridDataHD> frames;
        ILoadImage callback;
        protected VoxelModelName baseImage;
        protected List<AddImageToCustom> addItems;
        protected List<TwoAppearanceMaterials> findReplace;
        protected int link;
        protected Vector3 posAdj;

        public ModifiedImage()
            : base()//base(false)
        { }

        public ModifiedImage(ILoadImage callback, VoxelModelName baseImage,
                    List<TwoAppearanceMaterials> findReplace,
                    List<AddImageToCustom> addItems, Vector3 posAdj)
            : this(callback, baseImage, findReplace, addItems, posAdj, 0)
        { }
        public ModifiedImage(ILoadImage callback,  VoxelModelName baseImage,
            List<TwoAppearanceMaterials> findReplace,
            List<AddImageToCustom> addItems, Vector3 posAdj, int link)
            :base()
        {

            if (PlatformSettings.ViewErrorWarnings && addItems != null)
            {
                foreach (AddImageToCustom add in addItems)
                {
                    if (add.obj == VoxelModelName.NUM_NON)
                        throw new Exception();
                }
            }


            this.posAdj = posAdj;
            this.link = link;
            this.callback = callback;
            this.findReplace = findReplace;
            this.baseImage = baseImage;

            this.addItems = addItems;
            bool updateThread = baseImage == VoxelModelName.npc_male || baseImage == VoxelModelName.npc_female;
           
            bool storage = !(updateThread && (addItems == null||  addItems.Count == 0));

            addTaskToQue(storage? MultiThreadType.Storage : MultiThreadType.Asynch);
            //TaskExt.AddTask(this, storage, true);
            //    Ref.asynchUpdate.AddThreadQueObj(this);
            //else
            //    Engine.Storage.AddToSaveQue(StartQuedProcess, false);
        }

        public static VoxelObjGridDataAnimHD npcMale;
        public static VoxelObjGridDataAnimHD npcFemale;

        public static void Init()
        {
            npcMale = new VoxelObjGridDataAnimHD( Editor.VoxelObjDataLoader.LoadVoxelObjGridHD(VoxelModelName.npc_male));
            npcFemale = new VoxelObjGridDataAnimHD(Editor.VoxelObjDataLoader.LoadVoxelObjGridHD(VoxelModelName.npc_female));
        }

        //protected override void SynchedEvent()
        //{
        //    base.SynchedEvent();
        //}

        //protected override void MainThreadTrigger()
        //{
        //    base.MainThreadTrigger();
        //}

        public void runQuedTask(MultiThreadType threadType)
        {

            if (threadType == MultiThreadType.Storage)
            {
                if (frames == null)
                    frames = Editor.VoxelObjDataLoader.LoadVoxelObjGridHD(baseImage);
                if (addItems != null)
                {
                    foreach (AddImageToCustom add in addItems)
                    {
                        add.ReadData();
                    }
                }

                autoRun = true;
                bAsynchTask = true;
                //Ref.asynchUpdate.AddThreadQueObj(this);
            }
            else
            {
                switch (baseImage)
                {
                    case VoxelModelName.npc_male:
                        frames = npcMale.Clone().Frames;
                        break;
                    case VoxelModelName.npc_female:
                        frames = npcFemale.Clone().Frames;
                        break;

                }

                originalMesh = Generate(frames, addItems, findReplace, posAdj);
                

                AddToOrRemoveFromUpdateList(true);
            }
        }

        public static Graphics.VoxelModel Generate(List<VoxelObjGridDataHD> frames, List<AddImageToCustom> addItems, 
            List<TwoAppearanceMaterials> findReplace, Vector3 posAdj)
        {
            Graphics.VoxelModel originalMesh;
            List<VoxelObjGridData> framesResult = new List<VoxelObjGridData>(frames.Count);

            List<VoxelObjGridDataHD> addItemsData = null;
            List<VoxelObjGridDataAnimHD> addAnimItemsData = null;

            if (addItems != null && addItems.Count > 0)
            {
                foreach (AddImageToCustom item in addItems)
                {
                    if (item.Animated)
                    {
                        if (addAnimItemsData == null)
                            addAnimItemsData = new List<VoxelObjGridDataAnimHD>();
                        addAnimItemsData.Add(item.GenerateAnimated());
                    }
                    else
                    {
                        if (addItemsData == null)
                            addItemsData = new List<VoxelObjGridDataHD>();
                        addItemsData.Add(item.Generate());
                    }
                }
            }

            for (int frameIx = 0; frameIx < frames.Count; frameIx++)
            {
                List<VoxelObjGridDataHD> combine;
                if (addAnimItemsData == null)
                {
                    combine = addItemsData;
                }
                else
                {
                    if (addItemsData == null)
                    {
                        combine = new List<VoxelObjGridDataHD>(addAnimItemsData.Count);
                    }
                    else
                    {
                        combine = new List<VoxelObjGridDataHD>(addItemsData.Count + addAnimItemsData.Count);
                        combine.AddRange(addItemsData);
                    }
                    foreach (VoxelObjGridDataAnimHD item in addAnimItemsData)
                    {
                        combine.Add(item.Frames[frameIx]);
                    }
                }
                if (findReplace != null)
                {
                    frames[frameIx].Combine(findReplace, combine);
                }
            }

            posAdj += frames[0].BottomCenterAdj();

            //if (frames.Count == 1)
            //{
            //    originalMesh = Editor.VoxelObjBuilder.BuildAnimatedFromVoxelGrid2(frames, posAdj);//BuildFromVoxelGrid_Old(frames[0], posAdj);
            //}
            //else
            //{
                originalMesh = Editor.VoxelObjBuilder.BuildModelHD(frames, posAdj);//BuildAnimatedFromVoxelGrid_Old(frames, posAdj);
           // }
            return originalMesh;
        }

        public override void Time_Update(float time)
        {
            if (callback != null)
            { callback.SetCustomImage(originalMesh, link); }
        }
        public override string ToString()
        {
            return "Modified Image Loader (" + this.baseImage.ToString() + ")";
        }
    }

    class AddImageToCustom
    {
        bool animated;
        public bool Animated
        { get { return animated; } }
        public VoxelModelName obj { get; private set; }
        List<TwoAppearanceMaterials> findReplace;
        VoxelObjGridDataHD result;

        public AddImageToCustom(VoxelModelName obj, bool animated)
        {
            this.animated = animated;
            this.obj = obj;
            findReplace = new List<TwoAppearanceMaterials>();
        }

        public AddImageToCustom(VoxelModelName obj, List<TwoAppearanceMaterials> findReplace, bool animated)
        {
            this.animated = animated;
            this.obj = obj;
            this.findReplace = findReplace;
        }

        public AddImageToCustom(VoxelModelName obj, AppearanceMaterial find, AppearanceMaterial replace, bool animated)
        {
            this.animated = animated;
            this.obj = obj;
            this.findReplace = new List<TwoAppearanceMaterials> { new TwoAppearanceMaterials(find, replace) };
        }

        public void ReadData()
        {
            result = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(obj)[0];
        }

        public VoxelObjGridDataHD Generate()
        {
            //VoxelObjListData 
            result.ReplaceMaterial(findReplace);
            return result;
        }

        public VoxelObjGridDataAnimHD GenerateAnimated()
        {
            var result = new VoxelObjGridDataAnimHD(Editor.VoxelObjDataLoader.LoadVoxelObjGrid(obj));
            //result.ReplaceMaterial(findReplace);
            return result;
        }
    }
}
