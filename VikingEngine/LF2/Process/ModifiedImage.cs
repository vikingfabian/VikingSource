using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest;
using VikingEngine.Voxels;

namespace VikingEngine.LF2.Process
{
    interface ILoadImage
    {
        void SetCustomImage(Graphics.VoxelModel original, int link);
    }

    //måste förladda alla items innan den genereras

    /// <summary>
    /// Combines animated objects with cloth and other
    /// </summary>
    class ModifiedImage : OneTimeTrigger, IQuedObject
    {
        Graphics.VoxelModel originalMesh;
        List<VoxelObjGridData> frames;
        ILoadImage callback;
        VoxelModelName baseImage;
        List<AddImageToCustom> addItems;
        List<ByteVector2> findReplace;
        int link;
        Vector3 posAdj;

        public ModifiedImage(ILoadImage callback, VoxelModelName baseImage,
                    List<ByteVector2> findReplace,
                    List<AddImageToCustom> addItems, Vector3 posAdj)
            : this(callback, baseImage, findReplace, addItems, posAdj, 0)
        { }
        public ModifiedImage(ILoadImage callback,  VoxelModelName baseImage,
            List<ByteVector2> findReplace,
            List<AddImageToCustom> addItems, Vector3 posAdj, int link)
            :base(false)
        {

            if (PlatformSettings.ViewErrorWarnings && addItems != null)
            {
                foreach (AddImageToCustom add in addItems)
                {
                    if (add.obj == VoxelModelName.NUM_Empty)
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
           
            if (updateThread && (addItems == null||  addItems.Count == 0))
                Ref.asynchUpdate.AddThreadQueObj(this);
            else
                Engine.Storage.AddToSaveQue(StartQuedProcess, false);
        }

        public static VoxelObjGridDataAnim npcMale;
        public static VoxelObjGridDataAnim npcFemale;

        public static void Init()
        {
            npcMale = new VoxelObjGridDataAnim( Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.npc_male));
            npcFemale = new VoxelObjGridDataAnim( Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.npc_female));
        }

        public void StartQuedProcess(bool saveThread)
        {

            if (saveThread)
            {
                if (frames == null)
                    frames = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(baseImage);
                if (addItems != null)
                {
                    foreach (AddImageToCustom add in addItems)
                    {
                        add.ReadData();
                    }
                }
                Ref.asynchUpdate.AddThreadQueObj(this);
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
                

                AddToUpdateList(true);
            }
        }

        public static Graphics.VoxelModel Generate(List<VoxelObjGridData> frames, List<AddImageToCustom> addItems, 
            List<ByteVector2> findReplace, Vector3 posAdj)
        {
            Graphics.VoxelModel originalMesh;
            List<VoxelObjGridData> framesResult = new List<VoxelObjGridData>(frames.Count);

            List<VoxelObjGridData> addItemsData = null;
            List<VoxelObjGridDataAnim> addAnimItemsData = null;

            if (addItems != null && addItems.Count > 0)
            {
                foreach (AddImageToCustom item in addItems)
                {
                    if (item.Animated)
                    {
                        if (addAnimItemsData == null)
                            addAnimItemsData = new List<VoxelObjGridDataAnim>();
                        addAnimItemsData.Add(item.GenerateAnimated());
                    }
                    else
                    {
                        if (addItemsData == null)
                            addItemsData = new List<VoxelObjGridData>();
                        addItemsData.Add(item.Generate());
                    }
                }
            }

            for (int frameIx = 0; frameIx < frames.Count; frameIx++)
            {
                List<VoxelObjGridData> combine;
                if (addAnimItemsData == null)
                {
                    combine = addItemsData;
                }
                else
                {
                    if (addItemsData == null)
                    {
                        combine = new List<VoxelObjGridData>(addAnimItemsData.Count);
                    }
                    else
                    {
                        combine = new List<VoxelObjGridData>(addItemsData.Count + addAnimItemsData.Count);
                        combine.AddRange(addItemsData);
                    }
                    foreach (VoxelObjGridDataAnim item in addAnimItemsData)
                    {
                        combine.Add(item.Frames[frameIx]);
                    }
                }
                frames[frameIx].Combine(findReplace, combine);
            }

            posAdj += frames[0].BottomCenterAdj();

            if (frames.Count == 1)
            {
                originalMesh = Editor.VoxelObjBuilder.BuildFromVoxelGrid2(frames[0], posAdj);//BuildFromVoxelGrid_Old(frames[0], posAdj);
            }
            else
            {
                originalMesh = Editor.VoxelObjBuilder.BuildAnimatedFromVoxelGrid2(frames, posAdj);//BuildAnimatedFromVoxelGrid_Old(frames, posAdj);
            }
            return originalMesh;
        }

        public override void Time_Update(float time)
        {
            callback.SetCustomImage(originalMesh, link);
        }
        
    }

    class AddImageToCustom
    {
        bool animated;
        public bool Animated
        { get { return animated; } }
        public VoxelModelName obj { get; private set; }
        List<ByteVector2> findReplace;
        VoxelObjGridData result;

        public AddImageToCustom(VoxelModelName obj, bool animated)
        {
            this.animated = animated;
            this.obj = obj;
            findReplace = new List<ByteVector2>();
        }

        public AddImageToCustom(VoxelModelName obj, List<ByteVector2> findReplace, bool animated)
        {
            this.animated = animated;
            this.obj = obj;
            this.findReplace = findReplace;
        }

        public AddImageToCustom(VoxelModelName obj, byte find, byte replace, bool animated)
        {
            this.animated = animated;
            this.obj = obj;
            this.findReplace = new List<ByteVector2> { new ByteVector2(find, replace) };
        }

        public void ReadData()
        {
            result = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(obj)[0];
        }

        public VoxelObjGridData Generate()
        {
            //VoxelObjListData 
            result.ReplaceMaterial(findReplace);
            return result;
        }

        public VoxelObjGridDataAnim GenerateAnimated()
        {
            VoxelObjGridDataAnim result = new VoxelObjGridDataAnim(Editor.VoxelObjDataLoader.LoadVoxelObjGrid(obj));
            result.ReplaceMaterial(findReplace);
            return result;
        }
    }
}
