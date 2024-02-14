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

        //override public byte[] ByteArraySaveData
        //{
        //    get
        //    {
        //        IntVector3 limits = Frames[0].Limits;
        //        const byte SaveVersion = 2;
        //        List<byte> data = new List<byte> { SaveVersion, (byte)Frames.Count, 
        //            (byte)limits.X, (byte)limits.Y, (byte)limits.Z};

        //        for (int frame = 0; frame < Frames.Count; frame++)
        //        {
        //            data.AddRange(Frames[frame].ToCompressedData());
        //        }
        //        return data.ToArray();
        //    }

        //    set
        //    {
        //        if (value.Length <= 4)
        //        {
        //            //Err corrupt file
        //            Frames = null;
        //            return;
        //        }

        //        if (Frames == null)
        //        {
        //            Frames = new List<VoxelObjGridDataHD>();
        //        }
        //        else
        //            Frames.Clear();
        //        byte version = value[0];
        //        byte numFrames = value[1];
        //        IntVector3 limits = new IntVector3(value[2], value[3], value[4]);
                

        //        List<byte> data = new List<byte>();
        //        data.AddRange(value);
        //        data.RemoveRange(0, 5);

        //        for (int frame = 0; frame < numFrames; frame++)
        //        {
        //            VoxelObjGridDataHD grid = new VoxelObjGridDataHD(limits);
        //            int pos = byteArrayToGrid(data, grid);//grid.FromCompressedData(data);
        //            Frames.Add(grid);
        //            data.RemoveRange(0, pos);
        //        }

        //        LoadComplete();
        //    }

        //}
        //virtual protected int byteArrayToGrid(List<byte> data, VoxelObjGridDataHD grid)
        //{
        //    return grid.FromCompressedData(data);
        //}
    }
}