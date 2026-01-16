using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Input;
using VikingEngine.PJ.MiniGolf;

namespace VikingEngine.DSSWars.GameState.VoxelEditor
{
    class SearchInput : AbsTextInputUpdate
    {
        VoxelEditorMenu2 editorMenu;
        public SearchInput(VoxelEditorMenu2 editorMenu) 
            :base()
        {
            if (editorMenu.modelSearchFilter == null)
            { 
                editorMenu.modelSearchFilter = string.Empty;
            }

            this.editorMenu = editorMenu;
            init(editorMenu.modelSearchFilter, "voxel search", null);
            InitComplete();
        }

        public override void textInput_refresh(bool textLengthChanged)
        {
            editorMenu.modelSearchFilter = input.result;
        
            editorMenu.menu.needRefresh = true;
        }

        public override void textInput_complete(string result, object tag)
        {
            editorMenu.modelSearchFilter = result;
            base.textInput_complete(result, tag);
            editorMenu.menu.needRefresh = true;
        }
    }
}
