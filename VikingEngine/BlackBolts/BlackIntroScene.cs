using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Render;

namespace VikingEngine.Core.BlackBolts
{
    class BlackIntroScene : Engine.GameState
    {
        public BlackIntroScene() 
            :base(true)
        {
            new Models();
        }
        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            new BlackPlayScene();
        }
    }
}
