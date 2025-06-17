using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.Voxels
{
    abstract class AbsVoxelObjDataAnimHD
    {
        public List<VoxelObjGridDataHD> Frames;

        public IntVector3 Size { get { return Frames[0].Size; } }

        public void Save(bool save, DataStream.FilePath path, bool startThread = false)
        {
            DataStream.BeginReadWrite.BinaryIO(save, path, WriteBinaryStream, ReadBinaryStream, null, startThread);           
        }
        public void BucketFill(IntVector3 pos, int frame, ushort replace, bool continious, bool allFrames)
        {
            ushort find = Frames[frame].Get(pos);
            if (allFrames)
            {
                foreach (VoxelObjGridDataHD grid in Frames)
                {
                    grid.BucketFill(pos, find, replace, continious);
                }
            }
            else
            {
                Frames[frame].BucketFill(pos, find, replace, continious);
            }
        }

        public void ReplaceMaterial(List<BlockHDPair> findReplace)
        {
            foreach (VoxelObjGridDataHD grid in Frames)
            {
                grid.ReplaceMaterial(findReplace);
            }
        }

        public void WriteBinaryStream(System.IO.BinaryWriter w)
        {
            Voxels.VoxelLib.WriteVoxelObjAnimHD(w, Frames);
        }
        public void ReadBinaryStream(System.IO.BinaryReader r)
        {
            Frames = Voxels.VoxelLib.ReadVoxelObjectAnimHD(r);

        }

        public void Merge(VoxelObjGridDataAnimHD other, MergeModelsOption options)
        {
            switch (options.MergeFramesOptions)
            {
                case MergeFramesOptions.NewFirstOnOldFrames:

                    foreach (VoxelObjGridDataHD frame in Frames)
                    {
                        frame.Merge(other.Frames[0], options.KeepOldGridSize, options.NewBlocksReplaceOld, IntVector3.Zero);
                    }
                    break;
                case MergeFramesOptions.OldFirstOnNewFrames:
                    bool resize = options.KeepOldGridSize && this.Size != other.Size;

                    foreach (VoxelObjGridDataHD frame in other.Frames)
                    {
                        if (resize)
                        {
                            frame.Resize(this.Size);
                        }
                        frame.Merge(Frames[0], options.KeepOldGridSize, !options.NewBlocksReplaceOld, IntVector3.Zero);
                    }
                    break;
                case MergeFramesOptions.FrameByFrame:
                    for (int frameIx = 0; frameIx < other.Frames.Count; ++frameIx)
                    {
                        if (frameIx < Frames.Count)
                        {
                            Frames[frameIx].Merge(other.Frames[frameIx], options.KeepOldGridSize, options.NewBlocksReplaceOld, IntVector3.Zero);
                        }
                        else
                        {
                            VoxelObjGridDataHD newFrame = other.Frames[frameIx];
                            newFrame.Resize(Frames[0].Size);
                            Frames.Add(newFrame);
                        }
                    }
                    break;
            }
        }
    }

    class VoxelObjGridDataAnimHD : AbsVoxelObjDataAnimHD
    {
        public VoxelObjGridDataAnimHD()
        { }
        public VoxelObjGridDataAnimHD(List<VoxelObjGridDataHD> grids)
        {
            this.Frames = grids;
        }

        public VoxelObjGridDataAnimHD Clone()
        {
            List<VoxelObjGridDataHD> frames = new List<VoxelObjGridDataHD>(Frames.Count);
            for (int i = 0; i < Frames.Count; i++)
            {
                frames.Add(Frames[i].Clone());
            }
            return new VoxelObjGridDataAnimHD(frames);
        }

    }
}