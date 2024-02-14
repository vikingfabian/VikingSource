using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Process
{
    
    class LoadUserVoxelModel : DataLib.SaveStreamWithQuedProcess //ska ha trigger
    {
        Graphics.VoxelObjAnimated result;
        ILoadImage callback;
        int player;
        public LoadUserVoxelModel(string name, ILoadImage callback, int player)
            : base(false, Editor.VoxelDesigner.CustomVoxelObjPath(name), true)
        {
            //checkPath = true;
            runSynchTrigger = true;
            this.player = player;
            this.callback = callback;
            start();
        }

        public override void ReadStream(System.IO.BinaryReader r)
        {
            result = Editor.VoxelObjDataLoader.GetVoxelObjAnim(r, Vector3.Zero);
        }
        protected override void MainThreadTrigger()
        {
            if (result == null)
            {
                Engine.XGuide.Message("Corrupt file", path + " can't be opened, please remove it", player);
            }
            else
            {
                callback.SetCustomImage(result, 0);
            }
            
        }
    }
}
