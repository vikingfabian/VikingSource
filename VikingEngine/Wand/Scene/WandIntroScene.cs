using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Scene;
using VikingEngine.Core.Wand.Render;
using VikingEngine.DSSWars;

namespace VikingEngine.Core.Wand.Scene
{
    class WandIntroScene: Engine.GameState
    {
        public WandIntroScene()
            : base(true)
        {
            HudLib.Init();
            new VikingEngine.Core.Wand.Render.Models();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            new WandPlayScene();
        }
    }
}
