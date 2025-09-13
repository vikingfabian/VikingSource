using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.Map;
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

        /// <summary>
        /// Slaughtered: Loose 100 soldiers in a battle
        /// </summary>
        public const int SlaughteredCount = 100;

        /// <summary>
        /// Defeating victory: Win after loosing 40 military strength
        /// </summary>
        public const float Defeating_victory_strengthLost = 40;

        public const int FriendshipAllyCount = 8;
        double difficultyPerc;

        public const int LargePopulationCount_Tier1 = 4000;
        public const int LargePopulationCount_Tier2 = 10000;
        public const int LargePopulationCount_Tier3 = 16000;
        

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
                    if (p.faction.militaryStrength > 100)
                    {
                        UnlockAchievement_async(AchievementIndex.military_might_tier1);

                        if (p.faction.militaryStrength > 200)
                        {
                            UnlockAchievement_async(AchievementIndex.military_might_tier2);

                            if (p.faction.militaryStrength > 400)
                            {
                                UnlockAchievement_async(AchievementIndex.military_might_tier3);
                            }
                        }
                    }

                    if (p.faction.money.copper > int.MaxValue)
                    {
                        UnlockAchievement_async(AchievementIndex.gold_64bit);
                    }

                    var citiesC = p.faction.cities.counter();
                    while (citiesC.Next())
                    {
                        if (citiesC.sel.workForce.amount > LargePopulationCount_Tier1)
                        {
                            UnlockAchievement_async(AchievementIndex.large_population_tier1);

                            if (citiesC.sel.workForce.amount > LargePopulationCount_Tier2)
                            {
                                UnlockAchievement_async(AchievementIndex.large_population_tier2);

                                if (citiesC.sel.workForce.amount > LargePopulationCount_Tier3)
                                {
                                    UnlockAchievement_async(AchievementIndex.large_population_tier3);
                                }
                            }
                        }

                        int posted = 0;
                        int posted_stone = 0;


                        var groupsC = citiesC.sel.groups.counter();
                        while (groupsC.Next())
                        {
                            int post = groupsC.sel.GetGuardGroup().assignedToPost_IdAndPosition;
                            if (post > 0)
                            {
                                posted++;

                                switch ((TerrainWallType)DssRef.world.subTileGrid.Get(conv.IntToIntVector2(post)).subTerrain)
                                {
                                    case TerrainWallType.StoneGate:
                                    case TerrainWallType.StoneWallGreen:
                                    case TerrainWallType.StoneWall:
                                    case TerrainWallType.StoneTower:
                                    case TerrainWallType.StoneWallWoodHouse:
                                        posted_stone++;
                                        break;
                                }
                            }
                        }
                        //20 posted guards, then 40, then 80
                        if (posted >= 20)
                        {
                            UnlockAchievement_async(AchievementIndex.fortress_tier1);

                            if (posted >= 40)
                            {
                                UnlockAchievement_async(AchievementIndex.fortress_tier2);

                                if (posted >= 80)
                                {
                                    UnlockAchievement_async(AchievementIndex.fortress_tier3);

                                }
                            }

                            if (posted_stone >= 20)
                            {
                                UnlockAchievement_async(AchievementIndex.stone_fortress_tier1);

                                if (posted_stone >= 40)
                                {
                                    UnlockAchievement_async(AchievementIndex.stone_fortress_tier2);

                                    if (posted_stone >= 80)
                                    {
                                        UnlockAchievement_async(AchievementIndex.stone_fortress_tier3);

                                    }
                                }
                            }
                        }

                        
                    }

                    var armiesC = p.faction.armies.counter();
                    while (armiesC.Next())
                    {
                        int vikings = 0;
                        int farmers = 0;

                        var groupsC = armiesC.sel.groups.counter();
                        while (groupsC.Next())
                        {
                            switch (groupsC.sel.soldierConscript.conscript.weapon)
                            {
                                case Resource.ItemResourceType.SharpStick:
                                case Resource.ItemResourceType.SlingShot:
                                    farmers++; 
                                    break;
                            }

                            if (groupsC.sel.isShip && groupsC.sel.soldierConscript.conscript.specialization == SpecializationType.Sea)
                            { 
                                vikings++;
                            }
                        }

                        if (farmers >= 16)
                        {
                            UnlockAchievement_async(AchievementIndex.folkmen_rise);
                        }

                        if (vikings >= 16)
                        {
                            UnlockAchievement_async(AchievementIndex.vikings);
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
                DssRef.state.localPlayers?[0].hud.messages.Add("Achivement", achievement.ToString());
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

        public void UnlockAchievement_onAny_50_100_150(AchievementIndex achievementAny, AchievementIndex achievement50, AchievementIndex achievement100, AchievementIndex achievement150)
        {
            UnlockAchievement(achievementAny);

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

        public void UnlockAchievement_onAny_100(AchievementIndex achievementAny, AchievementIndex achievement100)
        {
            UnlockAchievement(achievementAny);

            if (difficultyPerc >= 100)
            {
                UnlockAchievement(achievement100);
            }
        }

        public void UnlockAchievement_on75(AchievementIndex achievement)
        {
            if (difficultyPerc >= 75)
            {
                UnlockAchievement(achievement);
            }
        }

        public void onAllyCount(int allies)
        {
            if (allies >= 4)
            {
                UnlockAchievement_async(AchievementIndex.friendship_tier1);
                if (allies >= 8)
                {
                    UnlockAchievement_async(AchievementIndex.friendship_tier2);
                    if (allies >= 16)
                    {
                        UnlockAchievement_async(AchievementIndex.friendship_tier3);
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
                    UnlockAchievement_onAny_50_100_150(AchievementIndex.victory_boss_any, AchievementIndex.victory_boss_50, AchievementIndex.victory_boss_100, AchievementIndex.victory_boss_150);
                    break;
                case VictoryType.WorldPeace:
                    UnlockAchievement(AchievementIndex.victory_worldpeace_any);


                    break;
                case VictoryType.Domination:
                    UnlockAchievement_onAny_50_100_150(AchievementIndex.victory_mini_domination_sandbox_any, AchievementIndex.victory_mini_domination_sandbox_50, AchievementIndex.victory_mini_domination_sandbox_100, AchievementIndex.victory_mini_domination_sandbox_150);

                    if (DssRef.world.metaData.mapSize >= MapSize.Medium)
                    {
                        UnlockAchievement_onAny_50_100_150(AchievementIndex.victory_domination_sandbox_any, AchievementIndex.victory_domination_sandbox_50, AchievementIndex.victory_domination_sandbox_100, AchievementIndex.victory_domination_sandbox_150);  
                    }

                    if (DssRef.difficulty.setting_gameMode == GameModeMainType.FullStory)
                    {
                        if (DssRef.world.metaData.mapSize < MapSize.Medium)
                        {
                            UnlockAchievement_onAny_100(AchievementIndex.victory_mini_domination_story_any, AchievementIndex.victory_mini_domination_story_100);
                        }
                        else
                        {
                            UnlockAchievement_onAny_100(AchievementIndex.victory_domination_story_any, AchievementIndex.victory_domination_story_100);

                            if (DssRef.world.metaData.mapSize >= MapSize.Large)
                            {
                                UnlockAchievement_on75(AchievementIndex.massive_victory_domination);
                            }
                        }
                    }

                    break;
            }

            if (!DssRef.difficulty.setting_allowPauseCommand)
            { 
                UnlockAchievement_onAny_50_100_150(AchievementIndex.no_pause_any, AchievementIndex.no_pause_50, AchievementIndex.no_pause_100, AchievementIndex.no_pause_150);
            }

            foreach (var p in DssRef.state.localPlayers)
            {
                if (p.statistics.WarsStartedByYou == 0)
                {
                    UnlockAchievement_onAny_50_100_150(AchievementIndex.no_war_started_any, AchievementIndex.no_war_started_50, AchievementIndex.no_war_started_100, AchievementIndex.no_war_started_150);

                    //if (victoryType == VictoryType.WorldPeace)
                    //{
                    //    UnlockAchievement_onAny_100(AchievementIndex.peace_and_love_any, AchievementIndex.peace_and_love_100);

                    //    if (DssRef.world.metaData.mapSize >= MapSize.Large)
                    //    {
                    //        UnlockAchievement_on75(AchievementIndex.massive_peace_and_love);
                    //    }
                    //}
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

            int hill_Factions = 0;
            var factionsC = DssRef.world.factions.counter();
            while (factionsC.Next())
            {
                if (factionsC.sel.factiontype == FactionType.BramblebrookHill ||
                    factionsC.sel.factiontype == FactionType.Tumblehill)
                {
                    ++hill_Factions;
                }
            }

            if (hill_Factions >= 2)
            {
                UnlockAchievement_onAny_100(AchievementIndex.worth_saving_any, AchievementIndex.worth_saving_100);
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
                            UnlockAchievement_onAny_50_100_150(AchievementIndex.honorguards_any, AchievementIndex.honorguards_50, AchievementIndex.honorguards_100, AchievementIndex.honorguards_150);
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
        victory_worldpeace_any,//i

        /// <summary>
        /// Grab the whole world to yourself - in sandbox
        /// </summary>
        victory_mini_domination_sandbox_any,//i
        victory_mini_domination_sandbox_50,
        victory_mini_domination_sandbox_100,
        victory_mini_domination_sandbox_150,

        /// <summary>
        /// Grab the whole world to yourself - in sandbox, medium world size
        /// </summary>
        victory_domination_sandbox_any,//i
        victory_domination_sandbox_50,
        victory_domination_sandbox_100,
        victory_domination_sandbox_150,

        /// <summary>
        /// Grab the whole world to yourself - in story
        /// </summary>
        victory_mini_domination_story_any,//i
        victory_mini_domination_story_100,

        /// <summary>
        /// Grab the whole world to yourself - in story, medium world size
        /// </summary>
        victory_domination_story_any,//i
        victory_domination_story_100,

        /// <summary>
        /// Grab the whole world to yourself - in story, large world size, min 75%
        /// </summary>
        massive_victory_domination,//i

        /// <summary>
        /// reach victory without starting a single war
        /// </summary>
        no_war_started_any,//i
        no_war_started_50,
        no_war_started_100,
        no_war_started_150,


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
        friendship_tier1,//i, t
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
        traitor,//i, t

        /// <summary>
        /// destroy the mercenaries on sea
        /// </summary>
        early_hara,//i, t


        /// <summary>
        /// Glory to me: contruct the "sword raising player" statue
        /// </summary>
        statue_of_player,//i, t

        /// <summary>
        /// Decorations: Constuct 20 decorative buildings, including at least 4 statues, then 40/8, then 80/16
        /// </summary>
        decorations_tier1,//i, t
        decorations_tier2,
        decorations_tier3,


        /// <summary>
        /// Knights: Produce cavalry knights
        /// </summary>
        knights,//i, t

        /// <summary>
        /// Men of steel: Produce soldiers with steel sword and armor.
        /// </summary>
        men_of_steel,//i, t

        /// <summary>
        /// Knights of Lunimari: Produce an army with fully mithril equipped swordsmen and archers
        /// </summary>
        knights_of_lumini,//i

        /// <summary>
        /// Large population: Reach a workforce of a 4000 men in one city, then 10k, then 16k
        /// </summary>
        large_population_tier1,//i
        large_population_tier2,
        large_population_tier3,

        /// <summary>
        /// Fortress: Own a city with 20 posted guards, then 40, then 80
        /// </summary>
        fortress_tier1,//i
        fortress_tier2,
        fortress_tier3,

        /// <summary>
        /// Stone Fortress: Own a city with 20 stone wall posted guards, then 40, then 80
        /// </summary>
        stone_fortress_tier1,//i
        stone_fortress_tier2,
        stone_fortress_tier3,

        /// <summary>
        /// Military might: Have an army power greater than 100, then 200, then 400
        /// </summary>
        military_might_tier1,//i
        military_might_tier2,
        military_might_tier3,

        /// <summary>
        /// Go 64bit: break the 16 bit limit of gold.
        /// </summary>
        gold_64bit,//i, t

        /// <summary>
        /// The Ottoman - defeat a city with bronze siege cannons
        /// </summary>
        ottoman,//i

        /// <summary>
        /// Purge: Wipe out 1 nation, then 4, then 12. story, 75% difficulty
        /// </summary>
        purge_nation_tier1,//i, t
        purge_nation_tier2,
        purge_nation_tier3,

        /// <summary>
        /// Max out - casual: gain all tech using casual controls
        /// </summary>
        maxout_casual,//i

        /// <summary>
        ///  fully research all technologies
        /// </summary>
        techtree,//i, t

        /// <summary>
        /// The people rise: 16 group army of only folkmen and slingers
        /// </summary>
        folkmen_rise,//i

        /// <summary>
        /// Vikings: Have a fleet with 16 ships with sea specialization.
        /// </summary>
        vikings,//i, t

        /// <summary>
        /// Slaughtered: Loose 100 soldiers in a battle
        /// </summary>
        slaughtered,//i, t

        /// <summary>
        /// Defeating victory: Win after loosing 40 military strength
        /// </summary>
        defeating_victory,//i

        /// <summary>
        /// Rear flanking: Make a cavalry charge against siege weapons
        /// </summary>
        rear_flanking,//i, t (can be cheesed)

        /// <summary>
        /// Bane of the barbarians: get the Dark Horde reward 
        /// </summary>
        barbarian_bane_any,//i, t
        barbarian_bane_100,

        /// <summary>
        /// Deliver gold
        /// </summary>
        gold_deliver,//i

        /// <summary>
        /// Reach victory with locked pause command 
        /// </summary>
        no_pause_any,//i
        no_pause_50,
        no_pause_100,
        no_pause_150,

        /// <summary>
        /// Destroy servants of dread, before the end boss
        /// </summary>
        early_dread_any,//i
        early_dread_100,

        /// <summary>
        /// Destroy the united kingdom, before the end boss
        /// </summary>
        early_uk_any,//i, t
        early_uk_100,

        /// <summary>
        /// just to test that achivements run
        /// </summary>
        first_game,//i, t

        /// <summary>
        /// Terminate the first faction to attack you
        /// </summary>
        destroy_first_attacker_any,//i
        destroy_first_attacker_100,

        /// <summary>
        /// Reach victory with both the "hill" factions still alive
        /// </summary>
        worth_saving_any,//i
        worth_saving_100,

        /// <summary>
        /// Ally with both the "hill" factions
        /// </summary>
        worthy_friends,//i, t


        NUM_ACHIEVEMENTS
    }
}
