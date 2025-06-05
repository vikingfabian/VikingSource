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
        int editor_flag_vox;
        int waitUpdates = 2;
        int ProfileIx;
        bool controller;
        public StartEditor(int ProfileIx, bool controller, int editor_flag_vox)
            : base()
        {
            this.ProfileIx = ProfileIx;
            this.controller = controller;
            draw.ClrColor = Color.Black;
            Ref.lobby?.disconnect(null);
            this.editor_flag_vox = editor_flag_vox;
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            if (--waitUpdates <= 0)
            {
                DssRef.world = null;
                Ref.music.stop(false);

                switch (editor_flag_vox)
                {
                    case 0:
                        new PaintFlagState(ProfileIx, controller);
                        break;
                    case 1:
                        XGuide.LocalHost.inputMap = new InputMap(false);
                        //XGuide.LocalHost.inputMap = new LootFest.Players.InputMap(XGuide.LocalHost.localPlayerIndex);
                        //XGuide.LocalHost.inputMap.xboxSetup();
                        //XGuide.LocalHost.inputMap.menuInput.xboxSetup(XGuide.LocalHost.localPlayerIndex);
                        new VoxelEditor.VoxelDesignState(XGuide.LocalHostIndex);
                        break;
                }
            }
        }
    }
}
