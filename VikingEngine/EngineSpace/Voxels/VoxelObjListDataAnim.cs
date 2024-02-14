using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Voxels
{
    class VoxelObjListDataAnim : AbsVoxelObjDataAnim
    {
        public IntVector3 drawLimits;
        new public List<VoxelObjListData> Frames;

        public VoxelObjListDataAnim(List<VoxelObjGridData> grids)
        {
            Frames = new List<VoxelObjListData>(grids.Capacity);
            foreach (VoxelObjGridData grid in grids)
            {
                Frames.Add(new VoxelObjListData(grid.GetVoxelArray()));
            }
            drawLimits = grids[0].Limits;
        }

        public VoxelObjListDataAnim(List<VoxelObjListData> frames, IntVector3 drawLimits)
        {
            this.Frames = frames;
            this.drawLimits = drawLimits;
        }
        new public void ReplaceMaterial(List<ByteVector2> findReplace)
        {
            foreach (VoxelObjListData f in Frames)
            {
                f.ReplaceMaterial(findReplace);
            }
        }

        override public byte[] ByteArraySaveData
        {
            get
            {
                const byte SaveVersion = 2;
                List<byte> data = new List<byte> { SaveVersion, (byte)Frames.Count, 
                    (byte)drawLimits.X, (byte)drawLimits.Y, (byte)drawLimits.Z};

                for (int frame = 0; frame < Frames.Count; frame++)
                {
                    VoxelObjGridData grid = new VoxelObjGridData(drawLimits, Frames[frame].Voxels);
                    data.AddRange(grid.ToCompressedData());
                }
                return data.ToArray();
            }

            set
            {
                if (Frames == null)
                {
                    Frames = new List<VoxelObjListData>();
                }
                else
                    Frames.Clear();
                byte version = value[0];
                byte numFrames = value[1];
                IntVector3 limits = new IntVector3(value[2], value[3], value[4]);
                VoxelObjGridData grid = new VoxelObjGridData(limits);

                List<byte> data = new List<byte>();
                data.AddRange(value);
                data.RemoveRange(0, 5);

                for (int frame = 0; frame < numFrames; frame++)
                {
                    int pos = grid.FromCompressedData(data);
                    Frames.Add(new VoxelObjListData(grid.GetVoxelArray()));
                    data.RemoveRange(0, pos);
                }
                LoadComplete();
                //currentFrame = new CirkleCounter(numFrames - 1);
                //drawLimits.Max = limits;
                //updateVoxelObj();
                //UpdateDrawLimits();
            }

        }

    }
}
