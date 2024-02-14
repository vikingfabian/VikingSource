using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Voxels
{
    class DottedDrawStroke
    {
        public IntervalIntV3 volume;
        public Voxels.VoxelObjGridDataHD undoData;

        public void add(IntervalIntV3 dotVolume, AbsVoxelDesigner designer)
        {
            if (undoData == null)
            {
                this.volume = dotVolume;
                undoData = new VoxelObjGridDataHD(volume.Size);

                ForXYZLoop loop = new ForXYZLoop(undoData.Size);
                while (loop.Next())
                {
                    undoData.Set(loop.Position, designer.GetVoxel(loop.Position + volume.Min));
                }
            }
            else
            {
                if (dotVolume.Min.X < volume.Min.X ||
                    dotVolume.Min.Y < volume.Min.Y ||
                    dotVolume.Min.Z < volume.Min.Z ||

                    dotVolume.Max.X > volume.Max.X ||
                    dotVolume.Max.Y > volume.Max.Y ||
                    dotVolume.Max.Z > volume.Max.Z)
                {
                    var oldVolume = volume;
                    var oldUndoData = undoData;

                    //The new dot is outside previous stored area
                    volume.Min.X = Math.Min(dotVolume.Min.X, volume.Min.X);
                    volume.Min.Y = Math.Min(dotVolume.Min.Y, volume.Min.Y);
                    volume.Min.Z = Math.Min(dotVolume.Min.Z, volume.Min.Z);

                    volume.Max.X = Math.Max(dotVolume.Max.X, volume.Max.X);
                    volume.Max.Y = Math.Max(dotVolume.Max.Y, volume.Max.Y);
                    volume.Max.Z = Math.Max(dotVolume.Max.Z, volume.Max.Z);

                    undoData = new VoxelObjGridDataHD(volume.Size);

                    ForXYZLoop loop = new ForXYZLoop(undoData.Size);
                    while (loop.Next())
                    {
                        ushort block;
                        IntVector3 wp = loop.Position + volume.Min;
                        if (oldVolume.pointInBounds(wp))
                        {
                            block = oldUndoData.Get(wp - oldVolume.Min);
                        }
                        else
                        {
                            block = designer.GetVoxel(wp);
                        }
                        undoData.Set(loop.Position, block);
                    }
                }

            }
        }
    }
}
