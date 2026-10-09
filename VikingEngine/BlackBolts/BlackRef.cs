using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Mission;
using VikingEngine.Core.BlackBolts.Render;

namespace VikingEngine.Core.BlackBolts
{
    static class BlackRef
    {
        public static BlackPlayScene playScene = null;
        public static AbsMissionSetup missionSetup;
        public static MapData mapData = null;
        public static MapStorage storage;

        public static Models models;
    }
}
