using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameState.FlagEditor;
using VikingEngine.DSSWars.GameState.MapEditor2.IconEditor;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars.GameState.MapEditor2
{
    class StartIconEditor : AbsDssState
    {
        //EditorType editor;
        int waitUpdates = 2;
        WorldData revertWorld;
        //int ProfileIx;
        //bool controller;
        public StartIconEditor(WorldData revertWorld/*int ProfileIx, bool controller, EditorType editor*/)
            : base()

        {
            this.revertWorld = revertWorld;
            //this.ProfileIx = ProfileIx;
            //this.controller = controller;
            draw.ClrColor = Color.Black;
            Ref.lobby?.disconnect(null);

            //this.editor = editor;
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            if (--waitUpdates <= 0)
            {
                DssRef.world = null;
                Ref.music.stop(false);

                
                new MapEditor2_Scene(revertWorld);
                       
            }
        }
    }
}
