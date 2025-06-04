using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.VoxelEditor
{
    class VoxelDesignState : Voxels.AbsVoxelDesignerState
    {
        VoxelDesigner vDesigner;
        public VoxelDesignState(int player)
            : base(true)
        {
            vDesigner = new VoxelDesigner(player);
            init(vDesigner);
        }

        void init(VoxelDesigner vDesigner)
        {
            Input.Mouse.Visible = false;
            desinger = vDesigner;
            Ref.draw.ClrColor = Color.CornflowerBlue;
        }
    }
}
