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

        //DataLib.ISaveTostorageCallback callBackObj;
        public void Save(bool save, DataStream.FilePath path, bool startThread = false)
        {
        //    this.Save(save, path, false, null);
        //}
        //public void Save(bool save, DataStream.FilePath path, bool threaded, DataLib.ISaveTostorageCallback callBackObj)
        //{

            //this.callBackObj = callBackObj;
            //if (startThread)
            //{
                DataStream.BeginReadWrite.BinaryIO(save, path, WriteBinaryStream, ReadBinaryStream, null, startThread);//ByteArray(save, path, this);
            //}
            //else
            //{
            //    if (save)
            //        DataStream.DataStreamHandler.Write(path, WriteBinaryStream);
            //    else
            //        DataStream.DataStreamHandler.ReadBinaryIO(path, ReadBinaryStream);
            //}
        }
        public void ReplaceMaterial(List<BlockHDPair> findReplace)
        {
            foreach (VoxelObjGridDataHD grid in Frames)
            {
                grid.ReplaceMaterial(findReplace);
            }
        }
        //protected void LoadComplete()
        //{
        //    if (callBackObj != null)
        //        callBackObj.SaveComplete(false, -1, null, false);
        //}
        //abstract public byte[] ByteArraySaveData { get; set; }

        public void WriteBinaryStream(System.IO.BinaryWriter w)
        {
            Voxels.VoxelLib.WriteVoxelObjAnimHD(w, Frames);
        }
        //    List<BlockHD[, ,]> data = new List<BlockHD[, ,]>(Frames.Count);
        //    foreach (VoxelObjGridDataHD f in Frames)
        //    {
        //        data.Add(f.MaterialGrid);
        //    }
        //    Voxels.VoxelLib.WriteVoxelObjAnimHD(w, data);
        //}
        public void ReadBinaryStream(System.IO.BinaryReader r)
        {
            Frames = Voxels.VoxelLib.ReadVoxelObjectAnimHD(r);

        }
        //    if (data == null)
        //    {
        //        byte[] dataArray = r.ReadBytes((int)(r.BaseStream.Length - r.BaseStream.Position));
        //        ByteArraySaveData = dataArray;
        //    }
        //    else
        //    {
        //        Frames = new List<VoxelObjGridDataHD>(data.Count);
        //        for (int i = 0; i < data.Count; i++)
        //        {
        //            Frames.Add(new VoxelObjGridDataHD(data[i]));
        //        }
        //    }
        //}

        public void Merge(VoxelObjGridDataAnimHD other, MergeModelsOption options)
        {
            switch (options.MergeFramesOptions)
            {
                case MergeFramesOptions.NewFirstOnOldFrames:
                    //List<Voxel> add = other.Frames[0].GetVoxelArray();
                    //the original should keeps its size and frames, the other should override its appearance

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

        public VoxelObjGridDataAnimHD(IntVector3 size, int frameCount)
        {
            this.Frames = new List<VoxelObjGridDataHD>(frameCount);
            for (int i = 0; i < frameCount; i++)
            {
                this.Frames.Add(new VoxelObjGridDataHD(size));
            }
        }

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