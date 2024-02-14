using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.PJ
{
    class ExitToLobbyState : AbsPJGameState
    {
        bool host;
        Time delay;

        public ExitToLobbyState(float delay = 0, bool host = true)
           : base(false)
        {
            this.delay = delay;
            this.host = host;

            draw.ClrColor = Color.Black;
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (delay.CountDown())
            {
                new LobbyState(host);
            }
        }
    }
}
