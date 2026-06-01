using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Data
{
    struct GiftedAchievement
    {
        public const SpriteName DefaultIcon = SpriteName.WarsUnitLevelLegend;
        public const SpriteName EmptyIcon = SpriteName.WarsUnitLevelMinimal;


        public string name;
        public string description; //one quick sentence

        public AchievementIndex achievement;
        public SteamWrapping.StatsInt stats;
    }

    static class GiftedAchievementCollection
    {
        public static readonly GiftedAchievementType[][] Categories = new GiftedAchievementType[][]
{
            // 0: Combat & Aggression (The Destroyers)
            new GiftedAchievementType[]
            {
                GiftedAchievementType.WarMonger,
                GiftedAchievementType.WarCriminal,
                GiftedAchievementType.SlaughterHouse,
                GiftedAchievementType.AnimalCruelty,
                GiftedAchievementType.OneManArmy,
                GiftedAchievementType.Bully,
                GiftedAchievementType.ScorchedEarth,
            },

            // 1: Strategy & Optimization (The Masterminds & Tryhards)
            new GiftedAchievementType[]
            {
                GiftedAchievementType.MetaPlayer,
                GiftedAchievementType.Tryhard,
                GiftedAchievementType.TheEncyclopedia,
                GiftedAchievementType._4DChessPlayer,
                GiftedAchievementType.SpreadsheetWarrior,
                GiftedAchievementType.DidPracticeInSecret,
                GiftedAchievementType.OverAchiever,
                GiftedAchievementType.AutomationAbuser
            },

            // 2: Diplomacy & Manipulation (The Schemers & Socialites)
            new GiftedAchievementType[]
            {
                GiftedAchievementType.KingMaker,
                GiftedAchievementType.Politian,
                GiftedAchievementType.Socializer,
                GiftedAchievementType.Wormtongue,
                GiftedAchievementType.PuppetMaster,
                GiftedAchievementType.Backstabber,
                GiftedAchievementType.Oathbreaker,
                GiftedAchievementType.BadInfluence
            },

            // 3: Economy & Pacifism (The Builders & Avoiders)
            new GiftedAchievementType[]
            {
                GiftedAchievementType.Turtle,
                GiftedAchievementType.FarmerRush,
                GiftedAchievementType.Hoarder,
                GiftedAchievementType.SwedishNeutrality,
                GiftedAchievementType.LivingInABobble,
                GiftedAchievementType.ShaggyTooDopeAlwaysChilling,
                GiftedAchievementType.Houseplant,
            },

            // 4: Team Roles & Dynamics (The Squad Fillers)
            new GiftedAchievementType[]
            {
                GiftedAchievementType.WhiteKnight,
                GiftedAchievementType.HeroComplexSaviorComplex,
                GiftedAchievementType.SupportSlave,
                GiftedAchievementType.MeatShield,
                GiftedAchievementType.TheCarry,
                GiftedAchievementType.LoneWolf,
                GiftedAchievementType.ArmchairGeneral,
                GiftedAchievementType.HindsightTactician,
                GiftedAchievementType.ControlFreak
            },

            // 5: Chaos & Trolling (The Disruptors)
            new GiftedAchievementType[]
            {
                GiftedAchievementType.TroubleMaker,
                GiftedAchievementType.RandomNothingMakesSense,
                GiftedAchievementType.Troll,
                GiftedAchievementType.MemeLord,
                GiftedAchievementType.DarkSidePlayer
            },

            // 6: Misfortune, Incompetence & Luck (The Strugglers)
            new GiftedAchievementType[]
            {
                GiftedAchievementType.CryBaby,
                GiftedAchievementType.Noob,
                GiftedAchievementType.Scatterbrained,
                GiftedAchievementType.NearSighted,
                GiftedAchievementType.LuckyBastard,
                GiftedAchievementType.Cursed,
                GiftedAchievementType.Salty,
                GiftedAchievementType.SaltMiner,
                GiftedAchievementType.InDebt,
                GiftedAchievementType.OnLifeSupport
            }
        };

        public static GiftedAchievement Get(GiftedAchievementType type)
        {
            switch (type)
            {
                case GiftedAchievementType.WhiteKnight:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_WhiteKnight_Name,
                        description = DssRef.todoLang.GiftAchieve_WhiteKnight_Desc,
                        achievement = AchievementIndex.Gift_WhiteKnight,
                        stats = DssRef.stats.Gifted_WhiteKnight,
                    };

                case GiftedAchievementType.HeroComplexSaviorComplex:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_HeroComplexSaviorComplex_Name,
                        description = DssRef.todoLang.GiftAchieve_HeroComplexSaviorComplex_Desc,
                        achievement = AchievementIndex.Gift_HeroComplexSaviorComplex,
                        stats = DssRef.stats.Gifted_HeroComplexSaviorComplex,
                    };

                case GiftedAchievementType.CryBaby:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_CryBaby_Name,
                        description = DssRef.todoLang.GiftAchieve_CryBaby_Desc,
                        achievement = AchievementIndex.Gift_CryBaby,
                        stats = DssRef.stats.Gifted_CryBaby,
                    };

                case GiftedAchievementType.KingMaker:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_KingMaker_Name,
                        description = DssRef.todoLang.GiftAchieve_KingMaker_Desc,
                        achievement = AchievementIndex.Gift_KingMaker,
                        stats = DssRef.stats.Gifted_KingMaker,
                    };

                case GiftedAchievementType.Turtle:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Turtle_Name,
                        description = DssRef.todoLang.GiftAchieve_Turtle_Desc,
                        achievement = AchievementIndex.Gift_Turtle,
                        stats = DssRef.stats.Gifted_Turtle,
                    };

                case GiftedAchievementType.MetaPlayer:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_MetaPlayer_Name,
                        description = DssRef.todoLang.GiftAchieve_MetaPlayer_Desc,
                        achievement = AchievementIndex.Gift_MetaPlayer,
                        stats = DssRef.stats.Gifted_MetaPlayer,
                    };

                case GiftedAchievementType.Tryhard:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Tryhard_Name,
                        description = DssRef.todoLang.GiftAchieve_Tryhard_Desc,
                        achievement = AchievementIndex.Gift_Tryhard,
                        stats = DssRef.stats.Gifted_Tryhard,
                    };

                case GiftedAchievementType.DidPracticeInSecret:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_DidPracticeInSecret_Name,
                        description = DssRef.todoLang.GiftAchieve_DidPracticeInSecret_Desc,
                        achievement = AchievementIndex.Gift_DidPracticeInSecret,
                        stats = DssRef.stats.Gifted_DidPracticeInSecret,
                    };

                case GiftedAchievementType.TheEncyclopedia:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_TheEncyclopedia_Name,
                        description = DssRef.todoLang.GiftAchieve_TheEncyclopedia_Desc,
                        achievement = AchievementIndex.Gift_TheEncyclopedia,
                        stats = DssRef.stats.Gifted_TheEncyclopedia,
                    };

                case GiftedAchievementType.WarCriminal:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_WarCriminal_Name,
                        description = DssRef.todoLang.GiftAchieve_WarCriminal_Desc,
                        achievement = AchievementIndex.Gift_WarCriminal,
                        stats = DssRef.stats.Gifted_WarCriminal,
                    };

                case GiftedAchievementType.FarmerRush:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_FarmerRush_Name,
                        description = DssRef.todoLang.GiftAchieve_FarmerRush_Desc,
                        achievement = AchievementIndex.Gift_FarmerRush,
                        stats = DssRef.stats.Gifted_FarmerRush,
                    };

                case GiftedAchievementType.Politian:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Politician_Name,
                        description = DssRef.todoLang.GiftAchieve_Politician_Desc,
                        achievement = AchievementIndex.Gift_Politian,
                        stats = DssRef.stats.Gifted_Politian,
                    };

                case GiftedAchievementType.Socializer:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Socializer_Name,
                        description = DssRef.todoLang.GiftAchieve_Socializer_Desc,
                        achievement = AchievementIndex.Gift_Socializer,
                        stats = DssRef.stats.Gifted_Socializer,
                    };

                case GiftedAchievementType.OverAchiever:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_OverAchiever_Name,
                        description = DssRef.todoLang.GiftAchieve_OverAchiever_Desc,
                        achievement = AchievementIndex.Gift_OverAchiever,
                        stats = DssRef.stats.Gifted_OverAchiever,
                    };

                case GiftedAchievementType.Noob:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Noob_Name,
                        description = DssRef.todoLang.GiftAchieve_Noob_Desc,
                        achievement = AchievementIndex.Gift_Noob,
                        stats = DssRef.stats.Gifted_Noob,
                    };

                case GiftedAchievementType.SwedishNeutrality:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_SwedishNeutrality_Name,
                        description = DssRef.todoLang.GiftAchieve_SwedishNeutrality_Desc,
                        achievement = AchievementIndex.Gift_SwedishNeutrality,
                        stats = DssRef.stats.Gifted_SwedishNeutrality,
                    };

                case GiftedAchievementType.TroubleMaker:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_TroubleMaker_Name,
                        description = DssRef.todoLang.GiftAchieve_TroubleMaker_Desc,
                        achievement = AchievementIndex.Gift_TroubleMaker,
                        stats = DssRef.stats.Gifted_TroubleMaker,
                    };
                case GiftedAchievementType.ScorchedEarth:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_ScorchedEarth_Name,
                        description = DssRef.todoLang.GiftAchieve_ScorchedEarth_Desc,
                        achievement = AchievementIndex.Gift_ScorchedEarth,
                        stats = DssRef.stats.Gifted_ScorchedEarth,
                    };

                case GiftedAchievementType.WarMonger:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_WarMonger_Name,
                        description = DssRef.todoLang.GiftAchieve_WarMonger_Desc,
                        achievement = AchievementIndex.Gift_WarMonger,
                        stats = DssRef.stats.Gifted_WarMonger,
                    };

                case GiftedAchievementType.LivingInABobble:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_LivingInABubble_Name,
                        description = DssRef.todoLang.GiftAchieve_LivingInABubble_Desc,
                        achievement = AchievementIndex.Gift_LivingInABobble,
                        stats = DssRef.stats.Gifted_LivingInABobble,
                    };

                case GiftedAchievementType.Bully:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Bully_Name,
                        description = DssRef.todoLang.GiftAchieve_Bully_Desc,
                        achievement = AchievementIndex.Gift_Bully,
                        stats = DssRef.stats.Gifted_Bully,
                    };

                case GiftedAchievementType.ControlFreak:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_ControlFreak_Name,
                        description = DssRef.todoLang.GiftAchieve_ControlFreak_Desc,
                        achievement = AchievementIndex.Gift_ControlFreak,
                        stats = DssRef.stats.Gifted_ControlFreak,
                    };

                case GiftedAchievementType.RandomNothingMakesSense:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_RandomNothingMakesSense_Name,
                        description = DssRef.todoLang.GiftAchieve_RandomNothingMakesSense_Desc,
                        achievement = AchievementIndex.Gift_RandomNothingMakesSense,
                        stats = DssRef.stats.Gifted_RandomNothingMakesSense,
                    };

                case GiftedAchievementType.Hoarder:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Hoarder_Name,
                        description = DssRef.todoLang.GiftAchieve_Hoarder_Desc,
                        achievement = AchievementIndex.Gift_Hoarder,
                        stats = DssRef.stats.Gifted_Hoarder,
                    };

                case GiftedAchievementType.Scatterbrained:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Scatterbrained_Name,
                        description = DssRef.todoLang.GiftAchieve_Scatterbrained_Desc,
                        achievement = AchievementIndex.Gift_Scatterbrained,
                        stats = DssRef.stats.Gifted_Scatterbrained,
                    };

                case GiftedAchievementType.NearSighted:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_NearSighted_Name,
                        description = DssRef.todoLang.GiftAchieve_NearSighted_Desc,
                        achievement = AchievementIndex.Gift_NearSighted,
                        stats = DssRef.stats.Gifted_NearSighted,
                    };

                case GiftedAchievementType.AutomationAbuser:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_AutomationAbuser_Name,
                        description = DssRef.todoLang.GiftAchieve_AutomationAbuser_Desc,
                        achievement = AchievementIndex.Gift_AutomationAbuser,
                        stats = DssRef.stats.Gifted_AutomationAbuser,
                    };

                case GiftedAchievementType.Troll:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Troll_Name,
                        description = DssRef.todoLang.GiftAchieve_Troll_Desc,
                        achievement = AchievementIndex.Gift_Troll,
                        stats = DssRef.stats.Gifted_Troll,
                    };

                case GiftedAchievementType.MemeLord:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_MemeLord_Name,
                        description = DssRef.todoLang.GiftAchieve_MemeLord_Desc,
                        achievement = AchievementIndex.Gift_MemeLord,
                        stats = DssRef.stats.Gifted_MemeLord,
                    };

                case GiftedAchievementType.SupportSlave:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_SupportSlave_Name,
                        description = DssRef.todoLang.GiftAchieve_SupportSlave_Desc,
                        achievement = AchievementIndex.Gift_SupportSlave,
                        stats = DssRef.stats.Gifted_SupportSlave,
                    };

                case GiftedAchievementType.DarkSidePlayer:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_DarkSidePlayer_Name,
                        description = DssRef.todoLang.GiftAchieve_DarkSidePlayer_Desc,
                        achievement = AchievementIndex.Gift_DarkSidePlayer,
                        stats = DssRef.stats.Gifted_DarkSidePlayer,
                    };

                case GiftedAchievementType.SlaughterHouse:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_SlaughterHouse_Name,
                        description = DssRef.todoLang.GiftAchieve_SlaughterHouse_Desc,
                        achievement = AchievementIndex.Gift_SlaughterHouse,
                        stats = DssRef.stats.Gifted_SlaughterHouse,
                    };

                case GiftedAchievementType.AnimalCruelty:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_AnimalCruelty_Name,
                        description = DssRef.todoLang.GiftAchieve_AnimalCruelty_Desc,
                        achievement = AchievementIndex.Gift_AnimalCruelty,
                        stats = DssRef.stats.Gifted_AnimalCruelty,
                    };

                case GiftedAchievementType.LuckyBastard:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_LuckyBastard_Name,
                        description = DssRef.todoLang.GiftAchieve_LuckyBastard_Desc,
                        achievement = AchievementIndex.Gift_LuckyBastard,
                        stats = DssRef.stats.Gifted_LuckyBastard,
                    };

                case GiftedAchievementType.Cursed:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Cursed_Name,
                        description = DssRef.todoLang.GiftAchieve_Cursed_Desc,
                        achievement = AchievementIndex.Gift_Cursed,
                        stats = DssRef.stats.Gifted_Cursed,
                    };

                case GiftedAchievementType.Backstabber:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Backstabber_Name,
                        description = DssRef.todoLang.GiftAchieve_Backstabber_Desc,
                        achievement = AchievementIndex.Gift_Backstabber,
                        stats = DssRef.stats.Gifted_Backstabber,
                    };

                case GiftedAchievementType.Oathbreaker:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Oathbreaker_Name,
                        description = DssRef.todoLang.GiftAchieve_Oathbreaker_Desc,
                        achievement = AchievementIndex.Gift_Oathbreaker,
                        stats = DssRef.stats.Gifted_Oathbreaker,
                    };

                case GiftedAchievementType.Wormtongue:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Wormtongue_Name,
                        description = DssRef.todoLang.GiftAchieve_Wormtongue_Desc,
                        achievement = AchievementIndex.Gift_Wormtongue,
                        stats = DssRef.stats.Gifted_Wormtongue,
                    };

                case GiftedAchievementType.ArmchairGeneral:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_ArmchairGeneral_Name,
                        description = DssRef.todoLang.GiftAchieve_ArmchairGeneral_Desc,
                        achievement = AchievementIndex.Gift_ArmchairGeneral,
                        stats = DssRef.stats.Gifted_ArmchairGeneral,
                    };

                case GiftedAchievementType.Salty:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Salty_Name,
                        description = DssRef.todoLang.GiftAchieve_Salty_Desc,
                        achievement = AchievementIndex.Gift_Salty,
                        stats = DssRef.stats.Gifted_Salty,
                    };

                case GiftedAchievementType.SaltMiner:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_SaltMiner_Name,
                        description = DssRef.todoLang.GiftAchieve_SaltMiner_Desc,
                        achievement = AchievementIndex.Gift_SaltMiner,
                        stats = DssRef.stats.Gifted_SaltMiner,
                    };

                case GiftedAchievementType.PuppetMaster:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_PuppetMaster_Name,
                        description = DssRef.todoLang.GiftAchieve_PuppetMaster_Desc,
                        achievement = AchievementIndex.Gift_PuppetMaster,
                        stats = DssRef.stats.Gifted_PuppetMaster,
                    };

                case GiftedAchievementType.TheCarry:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_TheCarry_Name,
                        description = DssRef.todoLang.GiftAchieve_TheCarry_Desc,
                        achievement = AchievementIndex.Gift_TheCarry,
                        stats = DssRef.stats.Gifted_TheCarry,
                    };

                case GiftedAchievementType.OneManArmy:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_OneManArmy_Name,
                        description = DssRef.todoLang.GiftAchieve_OneManArmy_Desc,
                        achievement = AchievementIndex.Gift_OneManArmy,
                        stats = DssRef.stats.Gifted_OneManArmy,
                    };

                case GiftedAchievementType._4DChessPlayer:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve__4DChessPlayer_Name,
                        description = DssRef.todoLang.GiftAchieve__4DChessPlayer_Desc,
                        achievement = AchievementIndex.Gift__4DChessPlayer,
                        stats = DssRef.stats.Gifted__4DChessPlayer,
                    };

                case GiftedAchievementType.SpreadsheetWarrior:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_SpreadsheetWarrior_Name,
                        description = DssRef.todoLang.GiftAchieve_SpreadsheetWarrior_Desc,
                        achievement = AchievementIndex.Gift_SpreadsheetWarrior,
                        stats = DssRef.stats.Gifted_SpreadsheetWarrior,
                    };

                case GiftedAchievementType.MeatShield:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_MeatShield_Name,
                        description = DssRef.todoLang.GiftAchieve_MeatShield_Desc,
                        achievement = AchievementIndex.Gift_MeatShield,
                        stats = DssRef.stats.Gifted_MeatShield,
                    };

                case GiftedAchievementType.InDebt:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_InDebt_Name,
                        description = DssRef.todoLang.GiftAchieve_InDebt_Desc,
                        achievement = AchievementIndex.Gift_InDebt,
                        stats = DssRef.stats.Gifted_InDebt,
                    };

                case GiftedAchievementType.OnLifeSupport:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_OnLifeSupport_Name,
                        description = DssRef.todoLang.GiftAchieve_OnLifeSupport_Desc,
                        achievement = AchievementIndex.Gift_OnLifeSupport,
                        stats = DssRef.stats.Gifted_OnLifeSupport,
                    };

                case GiftedAchievementType.LoneWolf:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_LoneWolf_Name,
                        description = DssRef.todoLang.GiftAchieve_LoneWolf_Desc,
                        achievement = AchievementIndex.Gift_LoneWolf,
                        stats = DssRef.stats.Gifted_LoneWolf,
                    };

                case GiftedAchievementType.ShaggyTooDopeAlwaysChilling:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_ShaggyTooDopeAlwaysChilling_Name,
                        description = DssRef.todoLang.GiftAchieve_ShaggyTooDopeAlwaysChilling_Desc,
                        achievement = AchievementIndex.Gift_ShaggyTooDopeAlwaysChilling,
                        stats = DssRef.stats.Gifted_ShaggyTooDopeAlwaysChilling,
                    };

                case GiftedAchievementType.BadInfluence:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_BadInfluence_Name,
                        description = DssRef.todoLang.GiftAchieve_BadInfluence_Desc,
                        achievement = AchievementIndex.Gift_BadInfluence,
                        stats = DssRef.stats.Gifted_BadInfluence,
                    };
                case GiftedAchievementType.HindsightTactician:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_HindsightTactician_Name,
                        description = DssRef.todoLang.GiftAchieve_HindsightTactician_Desc,
                        achievement = AchievementIndex.Gift_HindsightTactician,
                        stats = DssRef.stats.Gifted_HindsightTactician,
                    };

                case GiftedAchievementType.Houseplant:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Houseplant_Name,
                        description = DssRef.todoLang.GiftAchieve_Houseplant_Desc,
                        achievement = AchievementIndex.Gift_Houseplant,
                        stats = DssRef.stats.Gifted_Houseplant,
                    };

                case GiftedAchievementType.Sheep:
                    return new GiftedAchievement()
                    {
                        name = DssRef.todoLang.GiftAchieve_Sheep_Name,
                        description = DssRef.todoLang.GiftAchieve_Sheep_Desc,
                        achievement = AchievementIndex.Gift_Sheep,
                        stats = DssRef.stats.Gifted_Sheep,
                    };

                case GiftedAchievementType.NUM:
                default:
#if DEBUG
                    throw new NotImplementedException();
#endif
                    return new GiftedAchievement();
            }
        }
    }


    enum GiftedAchievementType
    {
        WhiteKnight,
        HeroComplexSaviorComplex,
        CryBaby,
        KingMaker,
        Turtle,
        MetaPlayer,
        Tryhard,
        DidPracticeInSecret,
        TheEncyclopedia,
        WarCriminal,
        FarmerRush,
        Politian,
        Socializer,
        OverAchiever,
        Noob,
        SwedishNeutrality,
        TroubleMaker,
        ScorchedEarth,
        WarMonger,
        LivingInABobble,
        Bully,
        ControlFreak,
        RandomNothingMakesSense,
        Hoarder,
        Scatterbrained,
        NearSighted,
        AutomationAbuser,
        Troll,
        MemeLord,
        SupportSlave,
        DarkSidePlayer,
        SlaughterHouse,
        AnimalCruelty,
        LuckyBastard,
        Cursed,
        Backstabber,
        Oathbreaker,
        Wormtongue,
        ArmchairGeneral,
        Salty,
        SaltMiner,
        PuppetMaster,
        TheCarry,
        OneManArmy,
        _4DChessPlayer,
        SpreadsheetWarrior,
        MeatShield,
        InDebt,
        OnLifeSupport,
        LoneWolf,
        ShaggyTooDopeAlwaysChilling,
        BadInfluence,
        HindsightTactician,
        Houseplant,
        Sheep,
        NUM,
    }

    //Glitch abuser

}
