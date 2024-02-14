using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Voxels;

namespace VikingEngine.LootFest.Process
{
    
    class LoadImage : DataLib.StorageTaskWithQuedProcess
    {
        Vector3 centerAdj;
        //bool deleteOrgMesh;
        ILoadImage callback;
        VoxelModelName baseImage;
        Graphics.VoxelModel originalMesh;

        //public LoadImage(ILoadImage callback, VoxelModelName baseImage)
        //    : this(callback, baseImage, Vector3.Zero)
        //{
            
        //}
        public LoadImage(ILoadImage callback, VoxelModelName baseImage, Vector3 centerAdj)
            :base(false, Editor.VoxelObjDataLoader.ContentPath(baseImage), true)
        {
            this.centerAdj = centerAdj;
            //this.deleteOrgMesh = deleteOrgMesh;
            this.baseImage = baseImage;
            this.callback = callback;

            originalMesh = LfRef.modelLoad.GetMasterImage(baseImage);
            if (originalMesh == null)
                this.beginAutoTasksRun();//this.beginStorageTask();
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
        protected override void runQuedMainTask()
        {
            base.runQuedMainTask();
            LfRef.modelLoad.SetMasterImage(baseImage, originalMesh);
            callback.SetCustomImage(originalMesh, (int)baseImage);
        }
    }
}
