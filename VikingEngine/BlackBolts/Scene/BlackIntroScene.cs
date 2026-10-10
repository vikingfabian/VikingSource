using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Mission;
using VikingEngine.Core.BlackBolts.Render;

namespace VikingEngine.Core.BlackBolts.Scene
{
    class BlackIntroScene : Engine.GameState
    {
        public BlackIntroScene() 
            :base(true)
        {
            DSSWars.HudLib.Init();
            new Models();
            FactoryObjectLib.init();
            ResourceLib.Init();
            new MapStorage();
        }
        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            //new BlackPlayScene(new SandboxSetup());
            new BlackMainScene();
        }
    }
}
