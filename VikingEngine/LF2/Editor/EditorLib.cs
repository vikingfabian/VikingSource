using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Voxels;

namespace VikingEngine.LF2.Editor
{
    class LoadTemplateFile : DataLib.SaveStreamWithQuedProcess
    {
        VoxelDesigner vd;
        IntVector3 drawCoord;
        RangeIntV3 drawLimits;
        VoxelObjListData selectedVoxels;
        IntVector3 gridSize;

        public LoadTemplateFile(DataStream.FilePath path, VoxelDesigner vd, IntVector3 drawCoord, RangeIntV3 drawLimits)
            : base(false, path, false)
        {
            this.drawCoord = drawCoord;
            this.drawLimits = drawLimits;
            this.vd = vd;
            runSynchTrigger = true;
            start();
        }

        public override byte[] ByteArraySaveData
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                List<byte> uncompressed = new List<byte>(value.Length);
                uncompressed.AddRange(value);
                gridSize = new IntVector3(uncompressed[0], uncompressed[1], uncompressed[2]);
                uncompressed.RemoveRange(0, 3);
                VoxelObjGridData grid2 = new VoxelObjGridData(gridSize);
                grid2.FromCompressedData(uncompressed);
                selectedVoxels = new VoxelObjListData(grid2.GetVoxelArray());

                //IntVector3 move = drawCoord;
                for (Dimensions d = Dimensions.X; d <= Dimensions.Z; d++)
                {
                    if (drawCoord.GetDimension(d) + gridSize.GetDimension(d) > drawLimits.Max.GetDimension(d))
                    {
                        drawCoord.SetDimension(d, drawLimits.Max.GetDimension(d) - gridSize.GetDimension(d));
                    }
                }

                selectedVoxels.Move(drawCoord, drawLimits);
                //selectionArea = new RangeIntV3(move, move + gridSize2);
                //startUpdateVoxelObj(true);
                //openHeadMenu(false);
            }
        }

        protected override void MainThreadTrigger()
        {
            vd.InsertLoadedTemplate(selectedVoxels, gridSize, drawCoord);
        } 
    }
    

    class NetworkDraw : IVoxelDesigner
    {
        Map.WorldPosition worldPos;
        public NetworkDraw(Map.WorldPosition worldPos)
        {
            this.worldPos = worldPos;
        }

        public void SetVoxel(IntVector3 drawPoint, byte material)
        {
            Map.WorldPosition pos = worldPos;
            pos.WorldGrindex.Add(drawPoint);
            //pos.UpdateChunkPos();
            LfRef.chunks.Set(pos, material);

        }
        public byte GetVoxel(IntVector3 drawPoint)
        {
            Map.WorldPosition pos = worldPos;
            pos.WorldGrindex.Add(drawPoint);
            //pos.UpdateChunkPos();
            return LfRef.chunks.Get(pos);

        }
    }

    struct LetterRows
    {
        public RangeIntV3 selectionArea;
        public List<int> lengths;
    }
    struct VoxelDesignerQueLoader : IQuedObject
    {
        ThreadedLoad type;
        int part;
        VoxelDesigner vd;
        int menuId;

        public VoxelDesignerQueLoader(ThreadedLoad type, VoxelDesigner vd)
            : this(type, 0, 0, vd)
        {

        }

        public VoxelDesignerQueLoader(ThreadedLoad type, int part, int menuId, VoxelDesigner vd)
        {
            this.menuId = menuId;
            this.vd = vd;
            this.type = type;
            this.part = part;
            Engine.Storage.AddToSaveQue(StartQuedProcess, false);
        }

        public void StartQuedProcess(bool saveThread)
        {
            vd.StartQuedProcess(type, part, menuId);
        }

    }

    enum GeomitricType
    {
        Sphere,
        Cylinder,
        Pyramid,
    }
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
