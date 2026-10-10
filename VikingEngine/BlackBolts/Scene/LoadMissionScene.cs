using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Mission;

namespace VikingEngine.Core.BlackBolts.Scene
{
    class LoadMissionScene : Engine.GameState
    {
        AbsMissionSetup missionSetup;
        public LoadMissionScene()
            : base(true)
        {
            missionSetup = new TutorialSetup();
            missionSetup.LoadMissionMap();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            new BlackPlayScene();
        }
    }
}
