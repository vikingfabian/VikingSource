using VikingEngine.LootFest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    /// <summary>
    /// Variables that change depending on platform and startup game
    /// </summary>
    static class PlatformSettings
    {
        public static readonly BuildDebugLevel DebugLevel
#if DEBUG
            = BuildDebugLevel.Dev;
#else
            = BuildDebugLevel.Release;
#endif

        public static readonly StartProgram RunProgram =
#if RTS
            StartProgram.PlunderParty;
#elif TOGG
            StartProgram.ToGG; 
#elif PJ
            StartProgram.PartyJousting;
#elif DSS
            StartProgram.DSS;
#elif SPECIAL
            StartProgram.Special;
#else
            StartProgram.LootFest3;
#endif

        public static readonly bool Debug_SteamAPI = false;

        public static readonly bool Debug_AllowDisconnect = false;

        public const bool Debug_HideMouse = false;

        static readonly bool Debug_PlayMusic = true;

        const bool Debug_StartLiveConnection = true;

        static readonly bool Debug_AutoJoinNetSession = true;

        const bool Debug_TravelEverywhere = true;

        const bool Debug_BlueScreen = false;

        const bool Debug_DebugWindow = false;

        const bool Debug_ViewSlowDown = true;

        const bool Debug_SimulateTrial = false;

        const bool Debug_SimulateJoinedControllers = false;

        public const int KeyboardPlayer = 0;

        const bool Debug_HelpAndTutorials = true;

        const bool Debug_ViewCollisionBounds = false;

        public const bool Debug_UseNetworkTimeout = true;

        /// <summary>
        /// Will make a delay when accessing files on the computer
        /// </summary>
        const bool Debug_SimulateXboxLoadingTime = false;

        /// <summary>
        /// View things that are half finished
        /// </summary>
        const bool Debug_ViewUnderConstructionStuff = false; //default=true

        const bool Debug_DebugOptions = true;

        public const int SteamNetworkVersion = 101; //fungerar som nätverks spärr mellan versioner

        public static readonly bool RunningWindows =
#if PCGAME
            true;
#else
            false;
#endif
        public static readonly bool RunningXbox =
#if XBOX
            true;
#else
            false;
#endif

        /// <summary>
        /// What platform the end product will run on, NOT what it is running on now
        /// </summary>
        public static readonly ReleasePlatform TargetPlatform =
#if PCGAME
                    ReleasePlatform.PC;
#elif XBOX
                    ReleasePlatform.Xbox;
#endif

        public static readonly bool PC_platform = TargetPlatform == ReleasePlatform.PC;

        public const bool PCTrial =
#if PCGAME
            false;
#else
            false;//DONT CHANGE
#endif

        public static bool HasUserCreatedContent
        {
            get
            {
                return RunProgram == StartProgram.LootFest3;
            }
        }

        #region DEBUG_SETTINGS
        /// <summary>
        /// Do not change this
        /// </summary>
        const bool LockedToTrue = true;
        /// <summary>
        /// Do not change this
        /// </summary>
        const bool LockedToFalse = false;

        public static readonly bool DevBuild = DebugLevel <= BuildDebugLevel.DebugDemo;

        public static readonly bool ReleaseBuild = DebugLevel > BuildDebugLevel.DebugDemo;

        public static readonly bool SteamAPI =
            DebugLevel >= BuildDebugLevel.Release ? LockedToTrue : Debug_SteamAPI;


        public static bool PlayMusic =
            DebugLevel >= BuildDebugLevel.ShowDemo ? LockedToTrue : Debug_PlayMusic;

        public static readonly bool StartLiveConnection =
            DebugLevel >= BuildDebugLevel.Release ? LockedToTrue : Debug_StartLiveConnection;

        public static readonly bool AutoJoinNetSession =
            DebugLevel != BuildDebugLevel.Dev ? LockedToFalse : Debug_AutoJoinNetSession;

       public static readonly bool BlueScreen =
            DebugLevel >= BuildDebugLevel.ShowDemo ? LockedToTrue : Debug_BlueScreen;

        public static bool DebugPerformanceText =
            DebugLevel > BuildDebugLevel.Release ? LockedToFalse : Debug_DebugWindow;

        public static bool ViewSlowDown =
            DebugLevel > BuildDebugLevel.Release ? LockedToFalse : Debug_ViewSlowDown;

        public static readonly bool SimulateJoinedControllers =
            DebugLevel >= BuildDebugLevel.Release ? LockedToFalse : Debug_SimulateJoinedControllers;

        public static readonly bool ViewCollisionBounds =
          DebugLevel != BuildDebugLevel.Dev ? LockedToFalse : Debug_ViewCollisionBounds;

        public static readonly bool Demo =
            DebugLevel == BuildDebugLevel.DebugDemo ||
            DebugLevel == BuildDebugLevel.PublicDemo || 
            DebugLevel == BuildDebugLevel.ShowDemo;

        /// <summary>
        /// Will make a delay when accessing files on the computer
        /// </summary>
        public static readonly bool SimulateXboxLoadingTime =
         (DebugLevel != BuildDebugLevel.Dev || PlatformSettings.RunningXbox) ? LockedToFalse : Debug_SimulateXboxLoadingTime;

        /// <summary>
        /// View things that are half finished
        /// </summary>
       
        public static readonly bool ViewErrorWarnings = DebugLevel == BuildDebugLevel.Dev;

        public static readonly bool DebugOptions = DebugLevel >= BuildDebugLevel.Release ? LockedToFalse : Debug_DebugOptions;

        public static bool SimulateLostController = false;


        #endregion

        public static string xboxSaveFileName()
        {
            string result = PlatformSettings.RunProgram.ToString() + " save";

            return result;
        }

        public static string SteamVersion
        {
            get
            {
                if (Engine.LoadContent.SteamVersion == null)
                    return "Unknown Version";
                else if (PlatformSettings.Demo)
                    return "Demo v. " + Engine.LoadContent.SteamVersion;
                else
                    return "Version " + Engine.LoadContent.SteamVersion;
            }
        }
        public static string XboxVersion = "UNKNOWN";

        public const string SteamApiDll = "Steam_api64";

        public const string GameTitle =
#if TOGG
         "Towards Gold and Glory";
#elif PJ
         "Party Jousting";
#else
         "Lootfest";
#endif
    }

    enum ReleasePlatform
    {
        PC,
        Xbox,
    }
}
