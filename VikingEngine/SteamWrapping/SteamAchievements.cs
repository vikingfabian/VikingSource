
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
#if PCGAME
using Valve.Steamworks;

namespace VikingEngine.SteamWrapping
{
    struct SteamAchievementData
    {
        public int enumValue;

        public string idString;
        public string name;
        public string description;
        public bool achieved;
        public int iconImage;

        public SteamAchievementData(int enumValue, string idString)
        {
            this.enumValue = enumValue;
            this.idString = idString;
            name = "";
            description = "";
            achieved = false;
            iconImage = 0;
        }
    }

    class SteamAchievements
    {
        SteamCallback<UserAchievementStored_t> UserAchievementStoredCallback;

        SteamAchievementData[] achievements;
        bool isInitialized;
        
        /* Constructors */
        public SteamAchievements()
        {
            isInitialized = false;

            if (PlatformSettings.RunProgram == StartProgram.LootFest3)
            {
                achievements = new SteamAchievementData[(int)LootFest.Data.AchievementIndex.NUM_ACHIEVEMENTS];

                for (LootFest.Data.AchievementIndex ix = 0; ix < LootFest.Data.AchievementIndex.NUM_ACHIEVEMENTS; ++ix)
                {
                    var achivement = new SteamAchievementData((int)ix, ix.ToString());
                    achievements[achivement.enumValue] = achivement;
                }
            }
            else if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
                achievements = new SteamAchievementData[(int)DSSWars.AchievementIndex.NUM_ACHIEVEMENTS];

                for (DSSWars.AchievementIndex ix = 0; ix < DSSWars.AchievementIndex.NUM_ACHIEVEMENTS; ++ix)
                {
                    var achivement = new SteamAchievementData((int)ix, ix.ToString());
                    achievements[achivement.enumValue] = achivement;
                }
            }

            //            if (PlatformSettings.RunProgram == StartProgram.Wars)
            //            {
            //#if DSS
            //                achievements = new SteamAchievementData[(int)Wars.AchievementIndex.NUM_ACHIEVEMENTS];

            //                for (Wars.AchievementIndex ix = 0; ix < Wars.AchievementIndex.NUM_ACHIEVEMENTS; ++ix)
            //                {
            //                    var achivement = new SteamAchievementData((int)ix, ix.ToString());
            //                    achievements[achivement.enumValue] = achivement;
            //                }
            //#endif
            //            }

            UserAchievementStoredCallback = new SteamCallback<UserAchievementStored_t>(OnUserAchievementStored, false);

            //achievements = new SteamAchievementData[names.Count];
            

            //for (int i = 0; i < achievements.Length; ++i)
            //{
            //    achievements[i] = new SteamAchievementData(names[i]);
            //}

            //RequestStats();
        }

        /* Novelty methods */
        public bool SetAchievement(int enumValue)
        {
            if (isInitialized)
            {
                var achievement = achievements[enumValue];
                if (!achievement.achieved)
                {
                    SteamAPI.SteamUserStats().SetAchievement(achievement.idString);
                    return SteamAPI.SteamUserStats().StoreStats();
                }
            }

            return false;
        }

        public bool SetAchievement(string id)
        {
            if (isInitialized)
            {
                SteamAPI.SteamUserStats().SetAchievement(id);
                return SteamAPI.SteamUserStats().StoreStats();
            }

            return false;
        }

        public bool ClearAchievement(int enumValue)
        {
            if (isInitialized)
            {
                SteamAPI.SteamUserStats().ClearAchievement(achievements[enumValue].idString);
                return SteamAPI.SteamUserStats().StoreStats();
            }

            return false;
        }

        public void ResetAllAchievements()
        {
            for (int i = 0; i < achievements.Length; ++i)
            {
                ClearAchievement(i);
            }
        }

        

        /// <summary>
        /// Called any time we attempt to request stats
        /// </summary>
        public void OnUserStatsRecieved(UserStatsReceived_t caller)
        {
            isInitialized = true;

            if (achievements != null)
            {
                for (int i = 0; i < achievements.Length; ++i)
                {
                    SteamAchievementData a = achievements[i];

                    SteamAPI.SteamUserStats().GetAchievement(a.idString, out a.achieved);
                    a.name = SteamAPI.SteamUserStats().GetAchievementDisplayAttribute(a.idString, "name");
                    a.description = SteamAPI.SteamUserStats().GetAchievementDisplayAttribute(a.idString, "desc");

                    achievements[i] = a;
                }
            }
        }

       

        /// <summary>
        /// Called any time achievements are succesfully stored on Steam
        /// </summary>
        /// <param name="caller"></param>
        void OnUserAchievementStored(UserAchievementStored_t caller)
        {
            if (caller.m_nGameID == SteamAPI.SteamUtils().GetAppID())
            {
                Debug.Log("Stored achievements for Steam");
            }
        }
    }

   
}
#endif