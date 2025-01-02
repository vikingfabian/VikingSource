using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.DSSWars.GameState
{
    class ExitGamePlay : AbsDssState
    {
        int waitUpdates = 30;

        public ExitGamePlay()
            :base()
        {
            draw.ClrColor = Color.Black;
            Ref.lobby?.disconnect(null);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            if (--waitUpdates <= 0)
            {
                DssRef.world = null;
                new LobbyState();
            }
        }
    }
}
