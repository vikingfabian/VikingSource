using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars.VoxelEditor
{
    class VoxelEditorMenu2 : AbsDesignMenuSystem_Base
    {
        VoxelDesigner designer;
        public VoxelEditorMenu2(VoxelDesigner designer)
        { 
            this.designer = designer;
        }

        override public void closeMenu() { }
        override public bool InMenu { get; }

        /// <returns>Exit</returns>
        override public bool Update()
        { 
        
        }

        override public void openMenu() 
        { throw new NotImplementedException(); }

        override public void selectionMenu()
        { 
        
        }
    }
}
