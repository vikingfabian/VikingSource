using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.FlagEditor;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars.GameState
{
    class StartEditor : AbsDssState
    {
        EditorType editor;
        int waitUpdates = 2;
        int ProfileIx;
        bool controller;
        public StartEditor(int ProfileIx, bool controller, EditorType editor)
            : base()
        {
            this.ProfileIx = ProfileIx;
            this.controller = controller;
            draw.ClrColor = Color.Black;
            Ref.lobby?.disconnect(null);
            
            this.editor = editor;
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            if (--waitUpdates <= 0)
            {
                DssRef.world = null;
                Ref.music.stop(false);

                switch (editor)
                {
                    case  EditorType.Flag:
                        new PaintFlagState(ProfileIx, controller);
                        break;
                    case  EditorType.Voxel:
                        DssRef.stats.start_voxeleditor.addOne_ifUnset();
                        XGuide.LocalHost.inputMap = new InputMap(false);
                        new VoxelEditor.VoxelDesignState(false, XGuide.LocalHostIndex);
                        break;
                    case   EditorType.Character:
                        DssRef.stats.start_character_creator.addOne_ifUnset();
                        new CharacterCreator.CharacterCreatorScene();
                        break;

                    case EditorType.Shader:
                        new ShaderLab.ShaderLabScene();
                        break;
                    case EditorType.Files:
                        new FileLab.FileLabScene();
                        break;
                }
            }
        }
    }

    enum EditorType
    { 
        Flag,
        Voxel,
        Character,
        Shader,
        Files,
    }
}
