using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.LevelSetup;

namespace VikingEngine.ToGG
{
    class StartLevelState : Engine.GameState
    {
        GameSetup setup;

        public StartLevelState(GameSetup setup)
            : base()
        {
            this.setup = setup;
        }

        public override void Time_Update(float time)
        {
            //base.Time_Update(time);
            new Commander.CmdPlayState(setup);
        }
    }
}
