using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Scene;
using VikingEngine.Core.Wand.Render;

namespace VikingEngine.Core.Wand.Scene
{
    class WandIntroScene: Engine.GameState
    {
        public WandIntroScene()
            : base(true)
        {
            DSSWars.HudLib.Init();
            new Models();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            new WandPlayScene();
        }
    }
}
