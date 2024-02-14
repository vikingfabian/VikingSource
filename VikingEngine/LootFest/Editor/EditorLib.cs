using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Voxels;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.LootFest.Editor
{
    //class LoadTemplateFile : DataLib.StorageTaskWithQuedProcess
    //{
    //    VoxelDesigner designer;
    //    IntVector3 drawCoord;
    //    IntervalIntV3 drawLimits;
    //    VoxelObjListDataHD selectedVoxels;
    //    IntVector3 gridSize;

    //    IntervalIntV3 volume;

    //    public LoadTemplateFile(DataStream.FilePath path, VoxelDesigner designer, IntVector3 drawCoord, IntervalIntV3 drawLimits)
    //        : base(false, path, false)
    //    {
    //        this.drawCoord = drawCoord;
    //        this.drawLimits = drawLimits;
    //        this.designer = designer;
    //        runSynchTrigger = true;
    //        beginStorageTask();
    //    }

    //    public override byte[] ByteArraySaveData
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //        set
    //        {
    //            DataStream.MemoryStreamHandler stream = new DataStream.MemoryStreamHandler();
    //            stream.SetByteArray(value);

    //            VoxelObjGridDataHD grid2 = VoxelLib.ReadVoxelObjectAnimHD(stream.GetReader())[0];
    //            selectedVoxels = new VoxelObjListDataHD(grid2.GetVoxelArray());
                
    //            for (Dimensions d = Dimensions.X; d <= Dimensions.Z; d++)
    //            {
    //                if (drawCoord.GetDimension(d) + gridSize.GetDimension(d) > drawLimits.Max.GetDimension(d))
    //                {
    //                    drawCoord.SetDimension(d, drawLimits.Max.GetDimension(d) - gridSize.GetDimension(d));
    //                }
    //            }

    //            selectedVoxels.Move(drawCoord, drawLimits);

    //            volume = selectedVoxels.getMinMax();
    //        }
    //    }

    //    protected override void MainThreadTrigger()
    //    {
    //        designer.InsertLoadedTemplate(selectedVoxels, volume);
    //    } 
    //}
    

    class NetworkDraw : IVoxelDesigner
    {
        Map.WorldPosition worldPos;

        public NetworkDraw(Map.WorldPosition worldPos)
        {
            this.worldPos = worldPos;
        }

        public void SetVoxel(IntVector3 drawPoint, ushort material)
        {
            Map.WorldPosition pos = worldPos;
            pos.WorldGrindex.Add(drawPoint);
            //pos.UpdateChunkPos();
            pos.SetBlock(material);
            //LfRef.chunks.Set(pos, material);

        }
        public ushort GetVoxel(IntVector3 drawPoint)
        {
            Map.WorldPosition pos = worldPos;
            pos.WorldGrindex.Add(drawPoint);
            //pos.UpdateChunkPos();
            return pos.GetBlock();
            //return LfRef.chunks.Get(pos);
        }
    }

    struct LetterRows
    {
        public IntervalIntV3 selectionArea;
        public List<int> lengths;
    }
    //struct VoxelDesignerQueLoader : IQuedObject
    //{
    //    ThreadedLoad type;
    //    int part;
    //    VoxelDesigner vd;
    //    int menuId;

    //    public VoxelDesignerQueLoader(ThreadedLoad type, VoxelDesigner vd)
    //        : this(type, 0, 0, vd)
    //    {

    //    }

    //    public VoxelDesignerQueLoader(ThreadedLoad type, int part, int menuId, VoxelDesigner vd)
    //    {
    //        this.menuId = menuId;
    //        this.vd = vd;
    //        this.type = type;
    //        this.part = part;
    //        //Engine.Storage.AddToSaveQue(StartQuedProcess, false);
    //    }

    //    //public void runQuedTask(MultiThreadType threadType)
    //    //{
    //    //    vd.StartQuedProcess(type, part, menuId);
    //    //}

    //}

    //enum GeomitricType
    //{
    //    Sphere,
    //    Cylinder,
    //    Pyramid,
    //}
    enum ThreadedLoad
    {
        StartUp,
        ListTemplates,
        ListTemplatesCategory,
    }
    enum MainMenuScene
    {
        bosslock,
        coverlike_stand,
        NUM
    }
}
