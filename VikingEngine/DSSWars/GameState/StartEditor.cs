using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.FlagEditor;

namespace VikingEngine.DSSWars.GameState
{
    class StartEditor : AbsDssState
    {
        int waitUpdates = 2;
        int ProfileIx;
        bool controller;
        public StartEditor(int ProfileIx, bool controller)
            : base()
        {
            this.ProfileIx = ProfileIx;
            this.controller = controller;
            draw.ClrColor = Color.Black;
            Ref.lobby?.disconnect(null);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            if (--waitUpdates <= 0)
            {
                DssRef.world = null;
                new PaintFlagState(ProfileIx, controller);
            }
        }
    }
}
