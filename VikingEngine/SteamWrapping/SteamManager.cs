
using VikingEngine.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars;


#if PCGAME

using Steamworks;//
#endif
namespace VikingEngine.SteamWrapping
{
#if PCGAME
    struct SteamApplicationSettings
    {
        /* Fields */
        public AppId_t appId;

        /* Constructors */
        public SteamApplicationSettings(
            AppId_t appId)
        {
            this.appId = appId;
        }
    }

    class SteamManager
    {
        public bool IsGameOverlayActive { get; private set; }
        public SteamAchievements Achievements = null;
        public SteamLeaderBoard leaderBoards;
        public SteamStats stats;
        public SteamLobbyMatchmaker LobbyMatchmaker = null;
        public SteamP2PManager P2PManager = null;
        //public SteamVOIP VOIP = null;
        public SteamDLC DLC = null;
        public SInput input = null;

        /* Fields */
        public bool isInitialized = false;
        public bool isNetworkInitialized = false;
        public bool statsInitialized = false;
        public bool leaderboardsInitialized = false;

        public bool inOverlay = false;
        public SteamApplicationSettings applicationSettings;

        public string UserCloudPath = "unknown_user";

        Callback<GameOverlayActivated_t> gameOverlayActivatedCB;
        Callback<UserStatsReceived_t> UserStatsRecievedCallback;
        Callback<UserStatsStored_t> UserStatsStoredCallback;
        //SteamWarningMessageHookDelegate warningHook;

        static void SteamAPIDebugTextHook(int severity, StringBuilder builder)
        {
            string msg = builder.ToString();
            if (severity == 0)
                Debug.Log(msg);
            else if (severity == 1)
                Debug.LogWarning(msg);
            else
                Debug.LogError(msg);
        }

        public SteamManager()
        {
            Ref.steam = this;

            if (PlatformSettings.SteamAPI && SteamAPI.Init())
            {
                isInitialized = true;

                applicationSettings = SetupSteamApplicationSettings(PlatformSettings.RunProgram);

                SetupSubsystems(applicationSettings);
                UserCloudPath = SteamUser.GetSteamID().ToString();

                if (PlatformSettings.RunProgram == StartProgram.LootFest3)
                {
                    new LootFest.Data.GameStats();
                }
                else if (PlatformSettings.RunProgram == StartProgram.PartyJousting)
                {
#if PJ
                new PJ.PjEngine.GameStats();
#endif
                }
//                else if (PlatformSettings.RunProgram == StartProgram.DSS)
//                {
//#if DSS
//                    new DSSWars.Data.GameStats();
//#endif
//                }
            }
            else
            {
                alwaysInit();
                Debug.LogError("SteamAPI_Init() failed.");
                Debug.LogError("Next to the EXE, there must be steam_api.dll, steam_api64.dll & steam_appid.txt");
            }
        }

        public void OnShutdown()
        {
            if (Ref.steam.isInitialized)
            {
                SteamInput.Shutdown();
            }
        }

        public void GoOffline()
        {
            isInitialized = false;
            isNetworkInitialized = false;
            statsInitialized = false;
            leaderboardsInitialized = false;
        }

        /// <summary>
        /// Returns false if an error occured
        /// </summary>
        //public bool Initialize()
        //{
        //    applicationSettings = SetupSteamApplicationSettings(PlatformSettings.RunProgram);

        //    //isInitialized = SteamAPI.Init(applicationSettings.appId);
           
        //    if (!isInitialized)
        //    {
        //        Debug.LogError("SteamAPI_Init() failed.");
        //        Debug.LogError("Next to the EXE, there must be steam_api.dll, steam_api64.dll & steam_appid.txt");
        //        return false;
        //    }

        //    SetupSubsystems(applicationSettings);
        //    //UserCloudPath = SteamAPI.SteamUser().GetSteamID().ToString();
        //    UserCloudPath = SteamUser.GetSteamID().ToString();

        //    return true;
        //}


        SteamApplicationSettings SetupSteamApplicationSettings(StartProgram program)
        {
            SteamApplicationSettings result;
            
            switch (program)
            {
                case StartProgram.LootFest3:
                    result = new SteamApplicationSettings(new AppId_t(367030));
                    break;
                case StartProgram.DSS:
                    if (PlatformSettings.STEAM_DEMO)
                    {
                        result = new SteamApplicationSettings(new AppId_t(3585100));
                    }
                    else
                    {
                        result = new SteamApplicationSettings(new AppId_t(1223150));
                    }
                    break;
                case StartProgram.PartyJousting:
                    result = new SteamApplicationSettings(new AppId_t(437900));
                    break;                    
                case StartProgram.ToGG:
                    if (PlatformSettings.STEAM_DEMO)
                    {
                        result = new SteamApplicationSettings(new AppId_t(878070));
                    }
                    else
                    {
                        result = new SteamApplicationSettings(new AppId_t(629450));
                    }                   
                    break;

                default:
                    throw new Exception("Steam Settings not setup by the programmers.");
            }

            return result;
        }

        void alwaysInit()
        {
            if (PlatformSettings.RunProgram == StartProgram.LootFest3 ||
                PlatformSettings.RunProgram == StartProgram.DSS ||
                PlatformSettings.RunProgram == StartProgram.ToGG ||
                PlatformSettings.RunProgram == StartProgram.PartyJousting)
            {
                Achievements = new SteamAchievements();
            }
            if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
#if DSS
                new DSSWars.Data.GameStats();
#endif
            }
        }

        void SetupSubsystems(SteamApplicationSettings settings)
        {
            alwaysInit();
            //warningHook = SteamAPIDebugTextHook;
            //SteamAPI.SteamClient().SetWarningMessageHook(warningHook);

            gameOverlayActivatedCB = new Callback<GameOverlayActivated_t>(OnGameOverlayActivated, false);
            UserStatsRecievedCallback = new Callback<UserStatsReceived_t>(OnUserStatsRecieved, false);
            UserStatsStoredCallback = new Callback<UserStatsStored_t>(OnUserStatsStored, false);
            input = new SInput();

            
            leaderBoards = new SteamLeaderBoard();

            AbsGameStats gamestats = null;
            if (PlatformSettings.RunProgram == StartProgram.LootFest3)
            {
                gamestats = LootFest.LfRef.stats;
            }
            else if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
#if DSS
                gamestats = DssRef.stats;
#endif
            }
            else if (PlatformSettings.RunProgram == StartProgram.PartyJousting)
            {
#if PJ
                gamestats = PJ.PjRef.stats;
#endif
            }
            else
            { gamestats = new TestGameStats(); }

            stats = new SteamStats(gamestats);

            if (PlatformSettings.RunProgram == StartProgram.LootFest3 ||
                PlatformSettings.RunProgram == StartProgram.PartyJousting ||
                PlatformSettings.RunProgram == StartProgram.DSS ||
                PlatformSettings.RunProgram == StartProgram.ToGG)
            {
                if (PlatformSettings.OnlineMultiplayer)
                {
                    P2PManager = new SteamP2PManager();
                    LobbyMatchmaker = new SteamLobbyMatchmaker();
                    //VOIP = new SteamVOIP();

                    isNetworkInitialized = true;
                }
            }

            DLC = new SteamDLC();

            //RequestStats();
        }

        public void Update()
        {
            if (isInitialized)
            {
                SteamAPI.RunCallbacks();
                
                if (P2PManager != null)
                {
                    //VOIP.Update();

                    P2PManager.update();
                }
            }
        }

        //bool RequestStats()
        //{
        //    if (!Ref.steam.isInitialized || !SteamUser.BLoggedOn())
        //    {
        //        return false;
        //    }

        //    return SteamUserStats.RequestCurrentStats();//SteamAPI.SteamUserStats().RequestCurrentStats();
        //}

        //public void Shutdown()
        //{
        //    if (isInitialized)
        //    {
        //        //SteamGamepad.Shutdown();
        //        isInitialized = false;
        //    }
        //}

        bool initUserStats = false;
        void OnUserStatsRecieved(UserStatsReceived_t caller)
        {
            if (Ref.steam.isInitialized && caller.m_nGameID == SteamUtils.GetAppID().m_AppId) // Other games may be requesting...
            {
                if (caller.m_eResult == EResult.k_EResultOK)
                {
                    if (!initUserStats)
                    {
                        initUserStats = true;

                        Debug.Log("Received stats and achievements from Steam");

                        if (Achievements != null)
                        {
                            Achievements.OnUserStatsRecieved(caller);
                        }
                        if (leaderBoards != null)
                        {
                            leaderboardsInitialized = true;
                            //leaderBoards.OnUserStatsRecieved(caller);
                        }
                        if (stats != null)
                        {
                            stats.OnUserStatsRecieved(caller);
                            statsInitialized = true;
                        }
                    }
                }
                else
                {
                    Debug.LogError("RequestStats failed, code " + caller.m_eResult.ToString());
                }
            }
        }

        /// <summary>
        /// Called any time we attempt to store information on Steam
        /// </summary>
        void OnUserStatsStored(UserStatsStored_t caller)
        {
            if (caller.m_nGameID == SteamUtils.GetAppID().m_AppId)
            {
                if (caller.m_eResult == EResult.k_EResultOK)
                {
                    Debug.Log("Stored stats for Steam");
                }
                else
                {
                    Debug.LogError("StatsStored failed, code " + caller.m_eResult);
                }
            }
        }

        void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
        {
            if (pCallback.m_bActive != 0)
            {
                Debug.Log("Steam Overlay has been activated");
                inOverlay = true;
            }
            else
            {
                Debug.Log("Steam Overlay has been closed");
                Ref.draw.settingsChanged2dImagesRefresh();
                inOverlay = false;
            }
        }

        public void debugToMenu(HUD.GuiLayout layout)
        {
            if (PlatformSettings.DevBuild)
            {
                new HUD.GuiTextButton(">>download user stats", null, downloadUserStats, false, layout);
                new HUD.GuiTextButton(">>download crash reports", null, downloadCrashReports, false, layout);
            }
        }

        public void debugInfoToMenu(HUD.GuiLayout layout)
        {
            new HUD.GuiLabel("Leaderboards Init: " + Ref.steam.leaderboardsInitialized.ToString(), layout);
            new HUD.GuiLabel("Stats Init: " + Ref.steam.statsInitialized.ToString(), layout);
        }

        void downloadUserStats()
        {
            new DownloadAllUserStats();
        }
        public void downloadCrashReports()
        {
            new DebugExtensions.DownloadSteamCrashReports(true, null);
        }
    }

#else
        class SteamManager
    { }
#endif
}