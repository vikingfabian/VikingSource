using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Mission;
using VikingEngine.Core.BlackBolts.Render;
using VikingEngine.DataStream;


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
            BlackMainScene.bgTex = Ref.main.Content.Load<Texture2D>(Core.BlackBolts.Data.MapStorage.ContentFolder + FilePath.Dir +
                "black bolt_bg");

            VikingEngine.DSSWars.DssRef.stats.start_blackbolt.addOne();
        }
        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            //new BlackPlayScene(new SandboxSetup());
            new BlackMainScene();
        }
    }
}
