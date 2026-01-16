#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;

namespace VikingEngine.SteamWrapping
{
    class DownloadAllUserStats : AbsUpdateable
    {
        public const string LastPlayedLeaderboard = "lastplay";
        SteamWrapping.SteamLeaderBoardLocal leaderboard;

        public DownloadAllUserStats()
            : base(true)
        {
            Debug.Log("ALL USER STATS");
            leaderboard = new SteamWrapping.SteamLeaderBoardLocal(LastPlayedLeaderboard);
            leaderboard.BeginDownload(onDownload);
        }

        void onDownload(List<SteamWrapping.SteamLeaderBoardRemote> values)
        {
            Debug.CrashIfThreaded();
            foreach (var m in values)
            {
                new DownloadUserStats(m);
            }

            DeleteMe();
        }

        public override void Time_Update(float time_ms)
        {
        }
    }

    class DownloadUserStats : AbsUpdateable
    {
        CallResult<UserStatsReceived_t> userStatsReceivedCallback;
        SteamLeaderBoardRemote leaderboard;

        public DownloadUserStats(SteamLeaderBoardRemote leaderboard)
            : base(true)
        {
            this.leaderboard = leaderboard;
            userStatsReceivedCallback = new CallResult<UserStatsReceived_t>(onUserStatsReceived);
            var apiCall = SteamUserStats.RequestUserStats(leaderboard.user);
            userStatsReceivedCallback.Set(apiCall);
        }

        void onUserStatsReceived(UserStatsReceived_t caller, bool ioFailure)
        {
            if (!ioFailure)
            {
                Debug.Log("---" + leaderboard.userName + "---");
                Debug.Log(SteamLeaderBoard.ScoreToDate(leaderboard.score).ToString());

                var values = Ref.steam.stats.gamestats.collectTimedValues();
                foreach (var m in values)
                {
                    m.getUserStats(leaderboard.user);
                    Debug.Log(m.ToString());
                }
            }

            DeleteMe();
        }

        public override void Time_Update(float time_ms)
        {
        }
    }
}
#endif