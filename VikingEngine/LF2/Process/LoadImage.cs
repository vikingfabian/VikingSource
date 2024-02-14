using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Voxels;

namespace VikingEngine.LF2.Process
{
    
    class LoadImage : DataLib.SaveStreamWithQuedProcess
    {
        Vector3 centerAdj;
        bool deleteOrgMesh;
        ILoadImage callback;
        VoxelModelName baseImage;
        Graphics.VoxelModel originalMesh;

        public LoadImage(ILoadImage callback, VoxelModelName baseImage)
            : this(callback, baseImage, Vector3.Zero)
        {
            
        }
        public LoadImage(ILoadImage callback, VoxelModelName baseImage, Vector3 centerAdj)
            :base(false, Editor.VoxelObjDataLoader.ContentPath(baseImage), true)
        {
            this.centerAdj = centerAdj;
            //this.deleteOrgMesh = deleteOrgMesh;
            this.baseImage = baseImage;
            this.callback = callback;

            originalMesh = LF2.Data.ImageAutoLoad.GetMasterImage(baseImage);
            if (originalMesh == null)
                this.start();
            else
            {
                callback.SetCustomImage(originalMesh, (int)baseImage);
            }
        }

        public override void ReadStream(System.IO.BinaryReader r)
        {
            this.runSynchTrigger = true;
            originalMesh = Editor.VoxelObjDataLoader.GetVoxelObjMaster(r, centerAdj);
            //if (deleteOrgMesh)
            //    originalMesh.DeleteMe();
        }
        protected override void MainThreadTrigger()
        {
            Data.ImageAutoLoad.SetMasterImage(baseImage, originalMesh);
            callback.SetCustomImage(originalMesh, (int)baseImage);
        }
    }
}
