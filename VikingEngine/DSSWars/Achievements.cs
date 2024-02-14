using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars
{
    class Achievements
    {
        double difficultyPerc;
        public Achievements() 
        {
            DssRef.achieve = this;
            difficultyPerc = DssRef.storage.DifficultyLevelPerc();
        }
        public void UnlockAchievement(AchievementIndex achievement)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("[!] Achievement: " + achievement.ToString());
#endif
            if (Ref.steam.isInitialized)
            {
                Ref.steam.Achievements.SetAchievement((int)achievement);
            }
        }

        public void onVictory()
        {
            if (difficultyPerc >= 25)
            {
                UnlockAchievement(AchievementIndex.victory25);

                if (difficultyPerc >= 50)
                {
                    UnlockAchievement(AchievementIndex.victory50);

                    if (difficultyPerc >= 100)
                    {
                        UnlockAchievement(AchievementIndex.victory100);

                        foreach (var p in DssRef.state.localPlayers)
                        {
                            if (p.warsStarted == 0)
                            {
                                UnlockAchievement(AchievementIndex.victory_peace);
                            }
                            else if (p.warsStarted >= 12)
                            {
                                UnlockAchievement(AchievementIndex.warmonger);
                            }

                            findHonorGuard(p);
                        }

                        if (difficultyPerc >= 200)
                        {
                            UnlockAchievement(AchievementIndex.victory200);
                        }
                    }
                }
            }

            void findHonorGuard(Players.LocalPlayer p)
            {
                var armiesC = p.faction.armies.counter();
                while (armiesC.Next())
                {
                    var groupsC = armiesC.sel.groups.counter();
                    while (groupsC.Next())
                    {
                        if (groupsC.sel.type == GameObject.UnitType.HonorGuard)
                        {
                            UnlockAchievement(AchievementIndex.honorguards);
                            return;
                        }
                    }
                }
            }
        }
    }

    enum AchievementIndex
    {
        victory25,
        victory50,
        victory100,
        victory200,
        victory_peace,
        early_hara,
        greenwood_ally,
        viking_naval,
        honorguards,
        buy_special1,
        buy_special3,
        no_darklord,
        warmonger,
        NUM_ACHIEVEMENTS
    }
}
