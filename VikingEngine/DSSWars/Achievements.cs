using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Event;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars
{
    /*
     * BRAINSTORM
     * -breaking int32 gold
     * 
     */

    class Achievements
    {
        public const int DecorationsTotalCount = 20;
        public const int DecorationsStatueCount = 4;

        public const int FriendshipAllyCount = 8;
        double difficultyPerc;

        const int FactionUniqueUnitTypeCount = 4;
        bool[] factionUniquePurchase = new bool[FriendshipAllyCount];

        public const int LargePopulationCount_Tier1 = 5000;
        public const int LargePopulationCount_Tier2 = 20000;
        public const int LargePopulationCount_Tier3 = 50000;
        

        public Achievements()
        {
            DssRef.achieve = this;
            difficultyPerc = DssRef.difficulty.TotalDifficulty();
        }

        public void asyncUpdate()
        {
            if (Ref.peRnd.ChanceF(0.1f))
            {
                foreach (var p in DssRef.state.localPlayers)
                {
                    var citiesC = p.faction.cities.counter();
                    while (citiesC.Next())
                    {
                        if (citiesC.sel.workForce.amount > LargePopulationCount_Tier1)
                        {
                            DssRef.achieve.UnlockAchievement(AchievementIndex.large_population_tier1);

                            if (citiesC.sel.workForce.amount > LargePopulationCount_Tier2)
                            {
                                DssRef.achieve.UnlockAchievement(AchievementIndex.large_population_tier2);

                                if (citiesC.sel.workForce.amount > LargePopulationCount_Tier3)
                                {
                                    DssRef.achieve.UnlockAchievement(AchievementIndex.large_population_tier3);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UnlockAchievement(AchievementIndex achievement)
        {
#if DEMO
            return;
#endif
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
#if DEMO
            return;
#endif
            if (Ref.steam.isInitialized)
            {
                Ref.steam.Achievements.SetAchievement_async((int)achievement);
            }
            else if (PlatformSettings.DebugLevel < BuildDebugLevel.Release)
            {
                Ref.update.AddSyncAction(new SyncAction(() => { UnlockAchievement(achievement); }));
            }
        }

        public void UnlockAchievement_on25_50_100_150(AchievementIndex achievement25, AchievementIndex achievement50, AchievementIndex achievement100, AchievementIndex achievement150)
        {
            UnlockAchievement(achievement25);

            if (difficultyPerc >= 50)
            {
                UnlockAchievement(achievement50);

                if (difficultyPerc >= 100)
                {
                    UnlockAchievement(achievement100);

                    if (difficultyPerc >= 150)
                    {
                        UnlockAchievement(achievement150);
                    }
                }
            }
        }

        public void onVictory(VictoryType victoryType)
        {
            if (DssRef.difficulty.setting_gameMode == GameModeMainType.Peaceful ||
                DssRef.difficulty.setting_gameMode == GameModeMainType.Spectator)
            {
                //Modes not available 
                return;
            }

            switch (victoryType)
            {
                case VictoryType.DefeatBoss:
                    UnlockAchievement_on25_50_100_150(AchievementIndex.victory_boss_any, AchievementIndex.victory_boss_50, AchievementIndex.victory_boss_100, AchievementIndex.victory_boss_150);
                    break;
            }


            foreach (var p in DssRef.state.localPlayers)
            {
                if (p.statistics.WarsStartedByYou == 0)
                {
                    UnlockAchievement_on25_50_100_150(AchievementIndex.no_war_started_any, AchievementIndex.no_war_started_50, AchievementIndex.no_war_started_100, AchievementIndex.no_war_started_150);
                }
                else if (p.statistics.WarsStartedByYou >= 10)
                {
                    UnlockAchievement(AchievementIndex.warstarter_tier1);
                    if (p.statistics.WarsStartedByYou >= 20)
                    {
                        UnlockAchievement(AchievementIndex.warstarter_tier2);
                        if (p.statistics.WarsStartedByYou >= 40)
                        {
                            UnlockAchievement(AchievementIndex.warstarter_tier3);
                        }
                    }
                }

                findHonorGuard(p);
            }

            if (DssRef.state.events.maxWars > 6)
            {
                UnlockAchievement(AchievementIndex.warjuggler_tier1);
                if (DssRef.state.events.maxWars > 9)
                {
                    UnlockAchievement(AchievementIndex.warjuggler_tier2);
                    if (DssRef.state.events.maxWars > 12)
                    {
                        UnlockAchievement(AchievementIndex.warjuggler_tier3);
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
                        if (groupsC.sel.soldierConscript.conscript.specialization == SpecializationType.HonorGuard)//.type == GameObject.UnitType.HonorGuard)
                        {
                            UnlockAchievement_on25_50_100_150(AchievementIndex.honorguards_any, AchievementIndex.honorguards_50, AchievementIndex.honorguards_100, AchievementIndex.honorguards_150);
                            return;
                        }
                    }
                }
            }


        }

        //public void onAlly(Faction playerFaction, Faction otherFaction)
        //{
        //    if (otherFaction.factiontype == FactionType.GreenWood)
        //    {
        //        UnlockAchievement(AchievementIndex.greenwood_ally);
        //    }

        //    //if (DssRef.state.events.nextEvent >= EventType.DarkLord)
        //    //{
        //    //    //Count allies
        //    //    Task.Factory.StartNew(() =>
        //    //    {
        //    //        int allyCount = 0;

        //    //        for (int i = 0; i < playerFaction.diplomaticRelations.Length; ++i)
        //    //        {
        //    //            var rel = playerFaction.diplomaticRelations[i];
        //    //            if (rel != null &&
        //    //                rel.Relation >= RelationType.RelationType3_Ally &&
        //    //                !DssRef.world.factions[i].HasZeroUnits())
        //    //            {
        //    //                ++allyCount;
        //    //            }
        //    //        }

        //    //        if (allyCount >= FriendshipAllyCount)
        //    //        {
        //    //            Ref.update.AddSyncAction(new SyncAction1Arg<AchievementIndex>(UnlockAchievement, AchievementIndex.friendship));
        //    //        }
        //    //    });
        //    //}
        //    //        catch (Exception ex)
        //    //        {
        //    //            BlueScreen.ThreadException = ex;
        //    //        }
                    
        //    //    });
        //    //}
        //}

        //public void onFactionUniquePurchase(int uniqeTypeIndex)
        //{
        //    if (!factionUniquePurchase[uniqeTypeIndex])
        //    {
        //        factionUniquePurchase[uniqeTypeIndex] = true;

        //        int count = 0;
        //        foreach (var m in factionUniquePurchase)
        //        {
        //            if (m)
        //            {
        //                ++count;
        //            }
        //        }

        //        UnlockAchievement(AchievementIndex.buy_special1);

        //        if (count >= 3)
        //        {
        //            UnlockAchievement(AchievementIndex.buy_special3);
        //        }
        //    }
        //}
    }

    /// <summary>
    /// i = implemented, t = tested
    /// </summary>
    enum AchievementIndex
    {
        /// <summary>
        /// defeat the boss
        /// </summary>
        victory_boss_any,//i
        victory_boss_50,
        victory_boss_100,
        victory_boss_150,

        /// <summary>
        /// have good relations with all nations who speaks to you
        /// </summary>
        victory_worldpeace_any,
        victory_worldpeace_50,
        victory_worldpeace_100,
        victory_worldpeace_150,

        /// <summary>
        /// Grab the whole world to yourself - in sandbox
        /// </summary>
        victory_mini_domination_sandbox_any,
        victory_mini_domination_sandbox_50,
        victory_mini_domination_sandbox_100,
        victory_mini_domination_sandbox_150,

        /// <summary>
        /// Grab the whole world to yourself - in sandbox, medium world size
        /// </summary>
        victory_domination_sandbox_any,
        victory_domination_sandbox_50,
        victory_domination_sandbox_100,
        victory_domination_sandbox_150,

        /// <summary>
        /// Grab the whole world to yourself - in story
        /// </summary>
        victory_mini_domination_story_any,
        victory_mini_domination_story_100,

        /// <summary>
        /// Grab the whole world to yourself - in story, medium world size
        /// </summary>
        victory_domination_story_any,
        victory_domination_story_100,

        /// <summary>
        /// Grab the whole world to yourself - in story, large world size, min 75%
        /// </summary>
        massive_victory_domination,

        /// <summary>
        /// reach victory without starting a single war
        /// </summary>
        no_war_started_any,//i
        no_war_started_50,
        no_war_started_100,
        no_war_started_150,

        /// <summary>
        /// reach world peace victory without starting a single war
        /// </summary>
        peace_and_love_any,
        peace_and_love_100,

        /// <summary>
        /// reach world peace victory without starting a single war, large world size, min 75%
        /// </summary>
        massive_peace_and_love,

        /// <summary>
        /// reach victory, and have started (10, 20, 40) wars, min 75%
        /// </summary>
        warstarter_tier1,//i
        warstarter_tier2,
        warstarter_tier3,

        /// <summary>
        ///  be in open war with 6 nations, then 9, then 12. Achieved on game victory.
        /// </summary>
        warjuggler_tier1,//i
        warjuggler_tier2,
        warjuggler_tier3,

        /// <summary>
        /// have 4 allies, then 8, then 16
        /// </summary>
        friendship_tier1,
        friendship_tier2,
        friendship_tier3,

        /// <summary>
        ///  reach victory, and still have your honor guards
        /// </summary>
        honorguards_any, //i
        honorguards_50,
        honorguards_100,
        honorguards_150,

        /// <summary>
        /// Declare war on an ally.
        /// </summary>
        traitor,

        /// <summary>
        /// destroy the mercenaries on sea
        /// </summary>
        early_hara,


        /// <summary>
        /// Glory to me: contruct the "sword raising player" statue
        /// </summary>
        statue_of_player,

        /// <summary>
        /// Decorations: Constuct 20 decorative buildings, including at least 4 statues
        /// </summary>
        decorations_tier1,
        decorations_tier2,
        decorations_tier3,


        /// <summary>
        /// Knights: Produce cavalry knights
        /// </summary>
        knights,

        /// <summary>
        /// Men of steel: Produce soldiers with steel sword and armor.
        /// </summary>
        men_of_steel,

        /// <summary>
        /// Knights of Lunimari: Produce an army with fully mithril equipped swordsmen and archers
        /// </summary>
        knights_of_lumini,

        /// <summary>
        /// Large population: Reach a workforce of a 5000 men in one city, then 20k, then 50k
        /// </summary>
        large_population_tier1,//i
        large_population_tier2,
        large_population_tier3,

        /// <summary>
        /// Fortress: Own a city with 20 posted guards, then 40, then 60
        /// </summary>
        fortress_tier1,
        fortress_tier2,
        fortress_tier3,

        /// <summary>
        /// Stone Fortress: Own a city with 20 stone wall posted guards, then 40, then 60
        /// </summary>
        stone_fortress_tier1,
        stone_fortress_tier2,
        stone_fortress_tier3,

        /// <summary>
        /// Military might: Have an army power greater than 100, then 200, then 400
        /// </summary>
        military_might_tier1,
        military_might_tier2,
        military_might_tier3,

        /// <summary>
        /// Go 64bit: break the 16 bit limit of gold.
        /// </summary>
        gold_64bit,

        /// <summary>
        /// The Ottoman - defeat a city with bronze siege cannons
        /// </summary>
        ottoman,

        /// <summary>
        /// Purge: Wipe out 1 nation, then 4, then 8
        /// </summary>
        purge_nation_tier1,
        purge_nation_tier2,
        purge_nation_tier3,

        /// <summary>
        /// Max out - casual: gain all tech using casual controls
        /// </summary>
        maxout_casual,

        /// <summary>
        ///  fully research all technologies
        /// </summary>
        techtree,

        /// <summary>
        /// The people rise: 16 group army of only folkmen and slingers
        /// </summary>
        folkmen_rise,

        /// <summary>
        /// Vikings: Have a fleet with 16 ships with sea specialization.
        /// </summary>
        vikings,

        /// <summary>
        /// Slaughtered: Loose 100 soldiers in a battle
        /// </summary>
        slaughtered,

        /// <summary>
        /// Defeating victory: Win after loosing 40 military strength
        /// </summary>
        defeating_victory,

        /// <summary>
        /// Rear flanking: Make a cavalry charge against siege weapons
        /// </summary>
        rear_flanking,

        /// <summary>
        /// Bane of the barbarians: get the Dark Horde reward 
        /// </summary>
        barbarian_bane_any,
        barbarian_bane_100,


        NUM_ACHIEVEMENTS
    }
}
