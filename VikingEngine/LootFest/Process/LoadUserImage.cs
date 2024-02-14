//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest.Process
//{

//    class loadUserVoxelModel : DataLib.StorageTaskWithQuedProcess //ska ha trigger
//    {
//        Graphics.VoxelModel result;
//        ILoadImage callback;
//        int player;
//        public LoadUserVoxelModel(string name, ILoadImage callback, int player)
//            : base(false, Editor.DesignerStorage.CustomVoxelObjPath(name), true)
//        {
//            checkPath = true;
//            runSynchTrigger = true;
//            this.player = player;
//            this.callback = callback;
//            beginStorageTask();
//        }

//        public override void ReadStream(System.IO.BinaryReader r)
//        {
//            result = Editor.VoxelObjDataLoader.GetVoxelObjAnimHD(r, Vector3.Zero, false);
//        }
//        protected override void MainThreadTrigger()
//        {
//            if (result == null)
//            {
//                Engine.XGuide.Message("Corrupt file", path + " can't be opened, please remove it", player);
//            }
//            else
//            {
//                callback.SetCustomImage(result, 0);
//            }

//        }
//    }
//}
