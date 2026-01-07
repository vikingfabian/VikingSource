//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;

//using VikingEngine.Graphics;
//using VikingEngine.Voxels;

//namespace VikingEngine.LootFest.GameState
//{
//    class VoxelDesignState : Voxels.AbsVoxelDesignerState
//    {
//        VoxelDesigner vDesigner;
//        public VoxelDesignState(int player)
//            : base(true)
//        {
//            vDesigner = new VoxelDesigner(player);
//            init(vDesigner);
//        }

//        void init(VoxelDesigner vDesigner)
//        {
//            Input.Mouse.Visible = false;
//            desinger = vDesigner;
//            Ref.draw.ClrColor = Color.CornflowerBlue;
//        }
//    }
//}
