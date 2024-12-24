using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Engine;

namespace VikingEngine
{
    /// <summary>
    /// References to the most common used parts of the engine
    /// </summary>
    static class Ref
    {
        public static Sound.MusicPlayer music;
        public static GameState gamestate;
        public static Engine.Draw draw;
        public static Engine.Update update;
        //public static Engine.AsynchUpdate asynchUpdate;
        public static Network.Session netSession;
        public static Network.NetLobby lobby;
        public static MainGame main;
        public static Sound.SoundManager sound;
        public static GameSettings gamesett;
        public static VikingEngine.SteamWrapping.SteamManager steam;
        //public static DataLib.Language language;
        public static HUD.AbsOptionsLanguage langOpt;                                                                                                                                                               
        public static System.Globalization.CultureInfo culture;

        
#if XBOX
        public static VikingEngine.XboxWrapping.XboxManager xbox;
#endif
        public static PcgRandom rnd = new PcgRandom();
        
        public static Network.INetworkUpdateReviever NetUpdateReciever()
        {
            if (lobby == null) return gamestate;

            return lobby;
        }

#if PCGAME
        public static SteamWrapping.SteamLobbyMatchmaker steamlobby
        {
            get { return steam.LobbyMatchmaker; }
        }
        public static SteamWrapping.SteamP2PManager p2p
        {
            get { return steam.P2PManager; }
        }
#endif

        public static float DeltaTimeMs;
        public static float TargetDeltaTimeMs;
        public static float DeltaTimeSec;
        public static float TotalTimeSec, PrevTotalTimeSec;
        public static float TotalGameTimeSec, PrevTotalGameTimeSec;
        public static int TotalFrameCount = 0;

        
        public static float TargetGameTimeSpeed = 1f;
        public static float GameTimeSpeed = TargetGameTimeSpeed;

        public static float DeltaGameTimeMs
        {
            get { return isPaused? 0 : DeltaTimeMs * GameTimeSpeed; }
        }
        public static float DeltaGameTimeSec
        {
            get { return isPaused ? 0 : DeltaTimeSec * GameTimeSpeed; }
        }

        /// <summary>
        /// Gameplay is paused
        /// </summary>
        public static bool isPaused = false;

        public static bool IsRunning { get { return !isPaused; } }

        /// <summary>
        /// All local players are in ingame loading scenes
        /// </summary>
        public static bool inLoading = false;


        /// <summary>How many times faster the update is than 30fps</summary>
        public static int UpdateTimes30FPS;

        /// <summary>For 60fps, acceleration and other processes sensitive to change in FPS</summary>
        public static bool TimePassed16ms = false;

        /// <summary>For 60fps or more, how many times * 60fps game runs</summary>
        public static int GameTimePassed16ms = 0;

        public static void ClearGarbage()
        {
            System.GC.Collect();
        }

        public static void SetPause(bool pause)
        {
            isPaused = pause;

            if (isPaused)
            {
                GameTimeSpeed = 0f;
            }
            else
            {
                GameTimeSpeed = TargetGameTimeSpeed;
            }
        }

        public static void SetGameSpeed(float multiply)
        { 
            TargetGameTimeSpeed = multiply;

            if (!isPaused)
            {
                GameTimeSpeed = TargetGameTimeSpeed;
            }
        }

        public static void ResetGameTime()
        {       
            TotalTimeSec = 0;
            PrevTotalTimeSec = 0;
            
            TotalFrameCount = 0;

            TotalGameTimeSec = 0;
            PrevTotalGameTimeSec = 0;
        }
    }
}
