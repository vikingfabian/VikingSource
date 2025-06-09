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
        public VoxelDesignState(bool controller, int player)
            : base(true)
        {
            vDesigner = new VoxelDesigner(controller, player);
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
