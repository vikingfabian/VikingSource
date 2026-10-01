using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.BlackBolts
{
    class BlackIntroScene : Engine.GameState
    {
        public BlackIntroScene() 
            :base(true)
        { }
        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            new BlackPlayScene();
        }
    }
}
