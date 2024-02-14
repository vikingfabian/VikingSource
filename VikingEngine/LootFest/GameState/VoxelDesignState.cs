using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.GameState
{
    class VoxelDesignState : Voxels.AbsVoxelDesignerState
    {
        Editor.VoxelDesigner vDesigner;
        public VoxelDesignState(int player)
            : base(true)
        {
            vDesigner = new Editor.VoxelDesigner(player);
            init(vDesigner);
        }

        //public VoxelDesignState(int player, LootFest.Map.Terrain.HandMadeTerrainMaster editTerrain)
        //    : base(false)
        //{
        //    vDesigner = new Editor.VoxelDesigner(player, editTerrain);
        //    init(vDesigner);
        //}

        void init(Editor.VoxelDesigner vDesigner)
        {
            Input.Mouse.Visible = false;
            desinger = vDesigner;
            Ref.draw.ClrColor = Color.CornflowerBlue;
        }


        
    }
}
