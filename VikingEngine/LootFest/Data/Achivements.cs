using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Data
{
    static class Achievements
    {
        public static void UnlockAchievement(AchievementIndex achievement, Players.Player player)
        {
#if PCGAME
            if (PlatformSettings.PC_platform && Ref.steam.isInitialized)
            {
                Ref.steam.Achievements.SetAchievement((int)achievement);
            }
#endif
            if (!player.Storage.progress.achievements[(int)achievement])
            {
                player.Storage.progress.achievements[(int)achievement] = true;
                player.Print("Trophy: " + NameAndDesc(achievement).String1, icon(achievement, true));
            }
        }

        public static TwoStrings NameAndDesc(AchievementIndex achievement)
        {
            switch (achievement)
            {
                case AchievementIndex.DefeatFinalBoss:
                    return new TwoStrings("Final Boss", "Complete the game");
                case AchievementIndex.CompletAllLevels:
                    return new TwoStrings("All levels", "Complete all levels");
                case AchievementIndex.CaptureAllCardTypes:
                    return new TwoStrings("All cards", "Capture all card types");
            }
            throw new NotImplementedException("Achievement name and desc: " + achievement.ToString());
        }

        public static SpriteName icon(AchievementIndex achievement, bool unlocked)
        {
            switch (achievement)
            {
                case AchievementIndex.DefeatFinalBoss:
                    return unlocked ? SpriteName.LfAchievementFinalBoss : SpriteName.LfAchievementFinalBoss_lock;
                case AchievementIndex.CompletAllLevels:
                    return unlocked ? SpriteName.LfAchievementAllLevels : SpriteName.LfAchievementAllLevels_lock;
                case AchievementIndex.CaptureAllCardTypes:
                    return unlocked ? SpriteName.LfAchievementAllCards : SpriteName.LfAchievementAllCards_lock;
            }
            throw new NotImplementedException("Achievement icon: " + achievement.ToString());
        }

    }

    enum AchievementIndex
    {
        DefeatFinalBoss,
        CompletAllLevels,
        CaptureAllCardTypes,
        NUM_ACHIEVEMENTS
    }
}
