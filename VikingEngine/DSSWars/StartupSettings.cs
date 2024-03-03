using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars
{
    static class StartupSettings 
    {
        public static bool AutoStartLevel = false;

        public static bool SpawnStartingArmies = PlatformSettings.DevBuild ? false :
            true;//DO NOT CHANGE
        
        public static bool RunAI = PlatformSettings.DevBuild ? false :
           true;//DO NOT CHANGE

        public static bool EndlessResources = PlatformSettings.DevBuild ? false : 
            false;//DO NOT CHANGE

        public static bool EndlessDiplomacy = PlatformSettings.DevBuild ? false :
            false;//DO NOT CHANGE

        public static bool SkipRecruitTime = PlatformSettings.DevBuild ? false :
            false;//DO NOT CHANGE

        public static bool TestOffscreenUpdate = PlatformSettings.DevBuild ? false :
            false;//DO NOT CHANGE

        public static bool RunResoursesUpdate = PlatformSettings.DevBuild ? false :
          false;//DO NOT CHANGE

        public static bool DebugResoursesSuperSpeed = PlatformSettings.DevBuild ? false :
           false;//DO NOT CHANGE

        public static MapSize? SaveLoadSpecificMap = PlatformSettings.DevBuild ? null :
            null;

        public static bool BlockBackgroundLoading = PlatformSettings.DevBuild ? false :
            false;//DO NOT CHANGE
        //public const bool Trailer = true;
    }
}
