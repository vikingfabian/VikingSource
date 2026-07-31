using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace VikingEngine.DSSWars
{
    static class StartupSettings 
    {
        public static string LeaderboardVersion = "apr2026";
        public static bool LeaderboardInBeta = PlatformSettings.DevBuild ? false :
           false; //RETAIL

        public static bool AutoStartLevel = false;

        //## DEFAULT TRUE ##
        public static bool Saves = PlatformSettings.DevBuild ? true :
            true;//TRUE

        public static bool SpawnStartingArmies = PlatformSettings.DevBuild ? true :
            true;//DO NOT CHANGE
        
        public static bool RunAI = PlatformSettings.DevBuild ? true :
           true;//DO NOT CHANGE


        //## DEFAULT FALSE ##
        public static bool EndlessResources = PlatformSettings.DevBuild ? true : 
            false;//DO NOT CHANGE    

        public static bool EndlessDiplomacy = PlatformSettings.DevBuild ? false :
            false;//DO NOT CHANGE

        public static bool UnlockAllProgress = PlatformSettings.DevBuild ? true :
            false;//DO NOT CHANGE

        public static bool PauseCheat = PlatformSettings.DevBuild ? false :
            false;//DO NOT CHANGE    

        public static bool CasualInstaBuild = PlatformSettings.DevBuild ? false :
            false;//DO NOT CHANGE

        public static bool TestOffscreenUpdate = PlatformSettings.DevBuild ? false :
            false;//DO NOT CHANGE

        public static bool RunResoursesUpdate = PlatformSettings.DevBuild ? false :
          false;//DO NOT CHANGE

        public static bool DebugResoursesSuperSpeed = PlatformSettings.DevBuild ? false :
           false;//DO NOT CHANGE
        
        public static bool BlockBackgroundLoading = PlatformSettings.DevBuild ? false :
            false;//DO NOT CHANGE

        public static bool BlockMessages = PlatformSettings.DevBuild ? false :
           false;//DO NOT CHANGE

        public static bool BlockTooltip = PlatformSettings.DevBuild ? false :
          false;//DO NOT CHANGE

        public static MapSize? SaveLoadSpecificMap = PlatformSettings.DevBuild ? null :
            null;//DO NOT CHANGE


        public static bool CheatActive =>
            !SpawnStartingArmies ||
            !RunAI ||
            PauseCheat ||
            UnlockAllProgress ||
            EndlessResources ||
            EndlessDiplomacy ||
            BlockMessages ||
            BlockTooltip ||
            CasualInstaBuild;

    }
}
