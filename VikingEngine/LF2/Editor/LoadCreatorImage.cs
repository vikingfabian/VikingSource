using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DataStream;
using VikingEngine.Voxels;

namespace VikingEngine.LF2.Editor
{
    delegate void CreatorImageLoaded(VoxelObjGridDataAnim animationFrames);

    class LoadCreatorImage : IStreamIOCallback
    {
        VoxelObjGridDataAnim animationFrames;
        CreatorImageLoaded callBack;

        public LoadCreatorImage(FilePath path, CreatorImageLoaded callBack)
        {
            this.callBack = callBack;
            animationFrames = new VoxelObjGridDataAnim();
            new DataStream.ReadBinaryIO(path, animationFrames.ReadBinaryStream,  this);
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            callBack(animationFrames);
        }
    }
}
