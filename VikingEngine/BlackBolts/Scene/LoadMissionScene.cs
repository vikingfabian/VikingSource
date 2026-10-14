using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Mission;

namespace VikingEngine.Core.BlackBolts.Scene
{
    class LoadMissionScene : Engine.GameState
    {
        AbsMissionSetup missionSetup;
        public LoadMissionScene(MissionType mission)
            : base(true)
        {
            switch (mission)
            {
                default:
                    missionSetup = new SandboxSetup();
                    break;
                case MissionType.Tutorial:
                    missionSetup = new TutorialSetup();
                    break;
                case MissionType.WhiteKnight:
                    missionSetup = new WhiteKnightSetup();
                    break;
            }
            
            missionSetup.LoadMissionMap();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            new BlackPlayScene();
        }
    }
}
