using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars
{
    class Achievements
    {
        public const int DecorationsTotalCount = 20;
        public const int DecorationsStatueCount = 4;

        public const int FriendshipAllyCount = 8;
        double difficultyPerc;

        const int FactionUniqueUnitTypeCount = 4;
        bool[] factionUniquePurchase = new bool[FriendshipAllyCount];

        public const int LargePopulationCount = 5000;
        public bool largePopulation = false;

        public const int AllUnitTypesCount = 6;

        public Achievements() 
        {
            DssRef.achieve = this;
            difficultyPerc = DssRef.difficulty.TotalDifficulty();
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
            else if (PlatformSettings.DebugLevel < BuildDebugLevel.Release)
            {
                DssRef.state.localPlayers[0].hud.messages.Add("Achivement", achievement.ToString());
            }
        }

        public void UnlockAchievement_async(AchievementIndex achievement)
        {
            if (Ref.steam.isInitialized)
            {
                Ref.steam.Achievements.SetAchievement_async((int)achievement);
            }
            else if (PlatformSettings.DebugLevel < BuildDebugLevel.Release)
            {
                Ref.update.AddSyncAction(new SyncAction(() => { UnlockAchievement(achievement); }));
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
                            if (p.statistics.WarsStartedByYou == 0)
                            {
                                UnlockAchievement(AchievementIndex.victory_peace);
                            }
                            else if (p.statistics.WarsStartedByYou >= 12)
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
                        if (groupsC.sel.soldierConscript.conscript.specialization == GameObject.Conscript.SpecializationType.HonorGuard)//.type == GameObject.UnitType.HonorGuard)
                        {
                            UnlockAchievement(AchievementIndex.honorguards);
                            return;
                        }
                    }
                }
            }
        }

        public void onAlly(Faction playerFaction, Faction otherFaction)
        {
            if (otherFaction.factiontype == FactionType.GreenWood)
            {
                UnlockAchievement(AchievementIndex.greenwood_ally);
            }

            if (DssRef.state.events.nextEvent >= EventType.DarkLord)
            {
                //Count allies
                Task.Factory.StartNew(() =>
                {
                    int allyCount = 0;

                    for (int i = 0; i < playerFaction.diplomaticRelations.Length; ++i)
                    {
                        var rel = playerFaction.diplomaticRelations[i];
                        if (rel != null &&
                            rel.Relation >= RelationType.RelationType3_Ally &&
                            !DssRef.world.factions[i].HasZeroUnits())
                        {
                            ++allyCount;
                        }
                    }

                    if (allyCount >= FriendshipAllyCount)
                    {
                        Ref.update.AddSyncAction(new SyncAction1Arg<AchievementIndex>(UnlockAchievement, AchievementIndex.friendship));
                    }
                });
            }
        }

        public void onFactionUniquePurchase(int uniqeTypeIndex)
        {
            if (!factionUniquePurchase[uniqeTypeIndex])
            {
                factionUniquePurchase[uniqeTypeIndex] = true;

                int count = 0;
                foreach (var m in factionUniquePurchase)
                {
                    if (m)
                    {
                        ++count;
                    }
                }

                UnlockAchievement(AchievementIndex.buy_special1);

                if (count >= 3)
                {
                    UnlockAchievement(AchievementIndex.buy_special3);
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
        friendship,

        //Resource update
//-1 statue
//-many statues
//-produce knights
//-army with all unit types
//-deliver food to starving city
//-create an archer with archer culture and a soldier with warrior culture
//-city with a population of 1000
        
        /// <summary>
        /// Statue: Build one statue
        /// </summary>
        statue,//t

        /// <summary>
        /// Decorations: Constuct 20 decorative buildings, including at least 4 statues
        /// </summary>
        decorations,//t

        /// <summary>
        /// Knights of Elite: Produce cavalry knights with the best armor and training
        /// </summary>
        elite_knights,//t

        /// <summary>
        /// All unit types: Own an army with all unit types
        /// </summary>
        all_unit_types,//t

        /// <summary>
        /// Take-out: Deliver food to a starving city
        /// </summary>
        deliver_food,//t

        /// <summary>
        ///  Men of culture: Create a bowman with archer culture, and a soldier with warrior culture
        /// </summary>
        soldier_culture,//t

        /// <summary>
        /// Large population: Reach a workforce of a 5000 men in one city
        /// </summary>
        large_population,//t

        NUM_ACHIEVEMENTS
    }
}
