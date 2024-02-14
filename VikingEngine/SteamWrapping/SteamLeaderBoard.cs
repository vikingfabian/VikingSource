#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;

namespace VikingEngine.SteamWrapping
{
    class SteamLeaderBoard
    {
        static readonly DateTime Year2000 = new DateTime(2000, 1, 1, 0, 0, 0);

        public SteamLeaderBoardLocal instance;

        public void Add(SteamLeaderBoardLocal instance)
        {
            this.instance = instance;
        }

        bool hasUploadedLastPlay = false;
        public void uploadlastplayed()
        {
            //WARNING! need at least a second for steam to fully init
            if (!hasUploadedLastPlay)
            {
                hasUploadedLastPlay = true;
                SteamLeaderBoardLocal lastPlayedScore = new SteamLeaderBoardLocal(DownloadAllUserStats.LastPlayedLeaderboard);
                lastPlayedScore.score = NowToScoreValue();
                lastPlayedScore.BeginUpload();
            }
        }

        public static int NowToScoreValue()
        {
            TimeSpan ts = DateTime.Now.Subtract(Year2000);
            int minutesSinceY2000 = (int)ts.TotalMinutes;
            return minutesSinceY2000;
        }

        public static DateTime ScoreToDate(int score)
        {
            DateTime time = Year2000;
            time = time.AddMinutes(score);
            return time;
        }
    }

    abstract class AbsSteamLeaderBoardInstance
    {
        protected const int MaxScoreDetails = 64;
        public int score;
        public StaticList<int> scoreDetails = new StaticList<int>(MaxScoreDetails);
    }

    class SteamLeaderBoardLocal : AbsSteamLeaderBoardInstance
    {        
        string name;
        bool uploadOnFind;

        Action<List<SteamLeaderBoardRemote>> downloadCallback = null;
        //
        SteamCallResult<LeaderboardFindResult_t> findLeaderboardCallback;
        SteamCallResult<LeaderboardScoreUploaded_t> leaderboardScoreUploadedCallback;
        SteamCallResult<LeaderboardScoresDownloaded_t> leaderboardScoreDownloadedCallback;
        public SteamLeaderBoardLocal(string name)
        {
            this.name = name;
        }
        
        public void BeginUpload()
        {
            uploadOnFind = true;
            find();
        }

        public void BeginDownload(Action<List<SteamLeaderBoardRemote>> callback)
        {
            this.downloadCallback = callback;
            uploadOnFind = false;
            find();
        }

        void find()
        {
            if (Ref.steam.isInitialized && Ref.steam.leaderboardsInitialized)
            {
                findLeaderboardCallback = new SteamCallResult<LeaderboardFindResult_t>(onFindLeaderboard);
                var apiCall = SteamAPI.SteamUserStats().FindLeaderboard(name);//"Error");
                findLeaderboardCallback.Set(apiCall);
            }
        }
        
        void onFindLeaderboard(LeaderboardFindResult_t caller, bool ioFailure)
        {
            if (caller.m_bLeaderboardFound != byte.MinValue)
            {
                SteamLeaderboard_t leaderboard = new SteamLeaderboard_t(caller.m_hSteamLeaderboard);

                if (uploadOnFind)
                {
                    leaderboardScoreUploadedCallback = new SteamCallResult<LeaderboardScoreUploaded_t>(onLeaderboardScoreUploaded);
                    ulong apiCall = SteamAPI.SteamUserStats().UploadLeaderboardScore(leaderboard,
                        ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate,
                        score, scoreDetails.Array, scoreDetails.Count);
                    leaderboardScoreUploadedCallback.Set(apiCall);
                }
                else
                {
                    leaderboardScoreDownloadedCallback = new SteamCallResult<LeaderboardScoresDownloaded_t>(onLeaderboardScoreDownloaded);
                    ulong apiCall = SteamAPI.SteamUserStats().DownloadLeaderboardEntries(leaderboard,
                        ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal,
                        0, 100);
                    leaderboardScoreDownloadedCallback.Set(apiCall);
                }
            }
        }

        void onLeaderboardScoreUploaded(LeaderboardScoreUploaded_t caller, bool ioFailure)
        {
            if (caller.m_bSuccess != byte.MinValue)
            {
                Debug.Log("Create leaderboard success");
            }            
        }

        void onLeaderboardScoreDownloaded(LeaderboardScoresDownloaded_t caller, bool ioFailure)
        {
            var downloaded = new List<SteamLeaderBoardRemote>(caller.m_cEntryCount);
            for (int i = 0; i < caller.m_cEntryCount; ++i)
            {
                SteamLeaderBoardRemote remote = new SteamLeaderBoardRemote(caller, i);
                downloaded.Add(remote);
            }
            
            downloadCallback?.Invoke(downloaded);            
        }
    }

    class SteamLeaderBoardRemote : AbsSteamLeaderBoardInstance
    {
        public ulong user;
        public string userName;

        public SteamLeaderBoardRemote(LeaderboardScoresDownloaded_t caller, int index)
        {
            LeaderboardEntry_t entry = new LeaderboardEntry_t();
            
            SteamAPI.SteamUserStats().GetDownloadedLeaderboardEntry(caller.m_hSteamLeaderboardEntries, index, ref entry,
                scoreDetails.Array, MaxScoreDetails);

            scoreDetails.Count = scoreDetails.Array.Length;

            score = entry.m_nScore;
            user = entry.m_steamIDUser;

            userName = SteamAPI.SteamFriends().GetFriendPersonaName(user);
        }

        public override string ToString()
        {
            return userName + " score:" + score.ToString();
        }
    }
}
#endif