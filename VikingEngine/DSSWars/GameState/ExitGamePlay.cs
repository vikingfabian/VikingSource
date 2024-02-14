using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.DSSWars.GameState
{
    class ExitGamePlay : Engine.GameState
    {
        int waitUpdates = 30;

        public ExitGamePlay()
            :base()
        {
            draw.ClrColor = Color.Black;
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

        //public override Engine.GameStateType Type
        //{
        //    get { return Engine.GameStateType.LoadingContent; }
        //}
    }
}
