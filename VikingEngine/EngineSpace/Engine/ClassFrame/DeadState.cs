using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.SteamWrapping;

namespace VikingEngine.Engine
{
    class DeadState : GameState
    {
        public DeadState() 
            :base()
        { 
             
        }

        protected override void createDrawManager()
        {
            base.createDrawManager();
            draw.ClrColor = Color.Purple;
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                new SteamManager();
                new DSSWars.IntroState(false);
            }
        }
    }
}
