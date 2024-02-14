using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DataStream;
using VikingEngine.Voxels;

namespace VikingEngine.LootFest.Editor
{
    delegate void CreatorImageLoaded(VoxelObjGridDataAnimHD animationFrames);

    class LoadCreatorImage : IStreamIOCallback
    {
        VoxelObjGridDataAnimHD animationFrames;
        CreatorImageLoaded callBack;

        public LoadCreatorImage(FilePath path, CreatorImageLoaded callBack)
        {
            this.callBack = callBack;
            animationFrames = new VoxelObjGridDataAnimHD();
            new DataStream.ReadBinaryIO(path, animationFrames.ReadBinaryStream,  this);
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            callBack(animationFrames);
        }
    }
}
