using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.SteamWrapping;

namespace VikingEngine.DSSWars.Data
{
    class GameStats : AbsGameStats
    {
        public int guardsRecruited = 0;

        #region DEMO_ONLY
        public StatsInt startNewDemo = new StatsInt("startnew_demo");
        public StatsInt completeDemo = new StatsInt("complete_demo");
        //public StatsInt startNewBattleDemo = new StatsInt("startnew_battledemo");//new
        //public StatsInt completeBattleDemo = new StatsInt("complete_battledemo");//new
        #endregion

        public StatsInt startUp = new StatsInt("startup");

        //English, Simplified Chinese, Japanese, Russian, Spanish, German, French, Turkish, Brazialian Portuguese, Italian
        public StatsInt pickLanguageStart = new StatsInt("pick_language_start");
        public StatsInt language_english = new StatsInt("language_english");
        public StatsInt language_simplified_chinese = new StatsInt("language_simplified_chinese");
        public StatsInt language_japanese = new StatsInt("language_japanese");
        public StatsInt language_russian = new StatsInt("language_russian");
        public StatsInt language_spanish = new StatsInt("language_spanish");
        public StatsInt language_german = new StatsInt("language_german");
        public StatsInt language_french = new StatsInt("language_french");
        public StatsInt language_turkish = new StatsInt("language_turkish");
        public StatsInt language_brazilian_portuguese = new StatsInt("language_brazilian_portuguese");
        public StatsInt language_italian = new StatsInt("language_italian");
        public StatsInt language_korean = new StatsInt("language_korean");

        public StatsInt blueScreen = new StatsInt("bluescreen");
        public StatsInt startTutorial = new StatsInt("start_tutorial");
        public StatsInt completeTutorial = new StatsInt("complete_tutorial");
        public StatsInt skipTutorial = new StatsInt("skip_tutorial");
        public StatsInt skip_advisor = new StatsInt("skip_advisor");

        public StatsInt startNewStory = new StatsInt("startnew_story");
        public StatsInt startNewQuickBoss = new StatsInt("startnew_quickboss");
        public StatsInt startQuickMatch = new StatsInt("startnew_quickmatch");
        public StatsInt startnew_casual = new StatsInt("startnew_casual");
        public StatsInt startNewSandbox = new StatsInt("startNewSandbox");
        public StatsInt startNewPeaceful = new StatsInt("startNewPeaceful");
        public StatsInt startNewSpectator = new StatsInt("startNewSpectator");

        public StatsInt startnewsize_full = new StatsInt("startnewsize_full");
        public StatsInt startnewsize_onecity = new StatsInt("startnewsize_onecity");
        public StatsInt startnewsize_settler = new StatsInt("startnewsize_settler");

        public StatsInt keyboard_user = new StatsInt("keyboard_user");
        public StatsInt controller_user = new StatsInt("controller_user");

        public StatsInt startNew25perc = new StatsInt("startnew_25perc");
        public StatsInt startNew50perc = new StatsInt("startnew_50perc");
        public StatsInt startNew75perc = new StatsInt("startnew_75perc");
        public StatsInt startNew100perc = new StatsInt("startnew_100perc");
        public StatsInt startNew125perc = new StatsInt("startnew_125perc");
        public StatsInt startNew150perc = new StatsInt("startnew_150perc");
        public StatsInt startNew175perc = new StatsInt("startnew_175perc");
        public StatsInt startNew200perc = new StatsInt("startnew_200perc");
        public StatsInt startNew300perc = new StatsInt("startnew_300perc");

        public StatsInt startNew_MapSmall = new StatsInt("startnew_map_small");
        public StatsInt startNew_MapLarge = new StatsInt("startnew_map_large");
        public StatsInt startNew_MapHuge = new StatsInt("startnew_map_huge");

        public StatsInt startNewLocalMultiplayer = new StatsInt("startnew_localmp");

        public StatsInt saveCustomFlag = new StatsInt("save_flag");
        public StatsInt start_voxeleditor = new StatsInt("start_voxeleditor");
        public StatsInt start_mapgenerator = new StatsInt("start_mapgenerator");
        public StatsInt start_character_creator = new StatsInt("start_character_creator");

        public StatsInt start_battle_lab = new StatsInt("start_battle_lab");
        public StatsInt battle_lab_newbattle = new StatsInt("battle_lab_newbattle");
        public StatsInt start_commander = new StatsInt("start_commander");
        public StatsInt commander_won = new StatsInt("commander_won");
        public StatsInt commander_lost = new StatsInt("commander_lost");

        public StatsInt won25perc = new StatsInt("won_25perc");
        public StatsInt won50perc = new StatsInt("won_50perc");
        public StatsInt won75perc = new StatsInt("won_75perc");
        public StatsInt won100perc = new StatsInt("won_100perc");
        public StatsInt won125perc = new StatsInt("won_125perc");
        public StatsInt won150perc = new StatsInt("won_150perc");
        public StatsInt won175perc = new StatsInt("won_175perc");
        public StatsInt won200perc = new StatsInt("won_200perc");
        public StatsInt won300perc = new StatsInt("won_300perc");

        public StatsInt lost25perc = new StatsInt("lost_25perc");
        public StatsInt lost50perc = new StatsInt("lost_50perc");
        public StatsInt lost75perc = new StatsInt("lost_75perc");
        public StatsInt lost100perc = new StatsInt("lost_100perc");
        public StatsInt lost125perc = new StatsInt("lost_125perc");
        public StatsInt lost150perc = new StatsInt("lost_150perc");
        public StatsInt lost175perc = new StatsInt("lost_175perc");
        public StatsInt lost200perc = new StatsInt("lost_200perc");
        public StatsInt lost300perc = new StatsInt("lost_300perc");


        StatsInt gameLength_passed5min = new StatsInt("lenght_5min");
        StatsInt gameLength_passed15min = new StatsInt("lenght_15min");
        StatsInt gameLength_passed30min = new StatsInt("lenght_30min");
        StatsInt gameLength_passed1hour = new StatsInt("lenght_1h");
        StatsInt gameLength_passed2hour = new StatsInt("lenght_2h");
        StatsInt gameLength_passed5hour = new StatsInt("lenght_5h");
        StatsInt gameLength_passed10hour = new StatsInt("lenght_10h");
        StatsInt gameLength_passed20hour = new StatsInt("lenght_20h");
        StatsInt gameLength_passed30hour = new StatsInt("lenght_30h");
        StatsInt gameLength_passed100hour = new StatsInt("lenght_100h");

        public StatsInt Gifted_WhiteKnight = new StatsInt("Gifted_WhiteKnight");
        public StatsInt Gifted_HeroComplexSaviorComplex = new StatsInt("Gifted_HeroComplexSaviorComplex");
        public StatsInt Gifted_CryBaby = new StatsInt("Gifted_CryBaby");
        public StatsInt Gifted_KingMaker = new StatsInt("Gifted_KingMaker");
        public StatsInt Gifted_Turtle = new StatsInt("Gifted_Turtle");
        public StatsInt Gifted_MetaPlayer = new StatsInt("Gifted_MetaPlayer");
        public StatsInt Gifted_Tryhard = new StatsInt("Gifted_Tryhard");
        public StatsInt Gifted_DidPracticeInSecret = new StatsInt("Gifted_DidPracticeInSecret");
        public StatsInt Gifted_TheEncyclopedia = new StatsInt("Gifted_TheEncyclopedia");
        public StatsInt Gifted_WarCriminal = new StatsInt("Gifted_WarCriminal");
        public StatsInt Gifted_FarmerRush = new StatsInt("Gifted_FarmerRush");
        public StatsInt Gifted_Politian = new StatsInt("Gifted_Politian");
        public StatsInt Gifted_Socializer = new StatsInt("Gifted_Socializer");
        public StatsInt Gifted_OverAchiever = new StatsInt("Gifted_OverAchiever");
        public StatsInt Gifted_Noob = new StatsInt("Gifted_Noob");
        public StatsInt Gifted_SwedishNeutrality = new StatsInt("Gifted_SwedishNeutrality");
        public StatsInt Gifted_TroubleMaker = new StatsInt("Gifted_TroubleMaker");
        public StatsInt Gifted_ScorchedEarth = new StatsInt("Gifted_ScorchedEarth");
        public StatsInt Gifted_WarMonger = new StatsInt("Gifted_WarMonger");
        public StatsInt Gifted_LivingInABobble = new StatsInt("Gifted_LivingInABobble");
        public StatsInt Gifted_Bullie = new StatsInt("Gifted_Bullie");
        public StatsInt Gifted_ControlFreak = new StatsInt("Gifted_ControlFreak");
        public StatsInt Gifted_RandomNothingMakesSense = new StatsInt("Gifted_RandomNothingMakesSense");
        public StatsInt Gifted_Hoarder = new StatsInt("Gifted_Hoarder");
        public StatsInt Gifted_Scatterbrained = new StatsInt("Gifted_Scatterbrained");
        public StatsInt Gifted_NearSighted = new StatsInt("Gifted_NearSighted");//
        public StatsInt Gifted_AutomationAbuser = new StatsInt("Gifted_AutomationAbuser");
        public StatsInt Gifted_Troll = new StatsInt("Gifted_Troll");
        public StatsInt Gifted_MemeLord = new StatsInt("Gifted_MemeLord");
        public StatsInt Gifted_SupportSlave = new StatsInt("Gifted_SupportSlave");
        public StatsInt Gifted_DarkSidePlayer = new StatsInt("Gifted_DarkSidePlayer");
        public StatsInt Gifted_SlaughterHouse = new StatsInt("Gifted_SlaughterHouse");
        public StatsInt Gifted_AnimalCruelty = new StatsInt("Gifted_AnimalCruelty");
        public StatsInt Gifted_LuckyBastard = new StatsInt("Gifted_LuckyBastard");
        public StatsInt Gifted_Cursed = new StatsInt("Gifted_Cursed");
        public StatsInt Gifted_Backstabber = new StatsInt("Gifted_Backstabber");
        public StatsInt Gifted_Oathbreaker = new StatsInt("Gifted_Oathbreaker");
        public StatsInt Gifted_Wormtongue = new StatsInt("Gifted_Wormtongue");
        public StatsInt Gifted_ArmchairGeneral = new StatsInt("Gifted_ArmchairGeneral");
        public StatsInt Gifted_Salty = new StatsInt("Gifted_Salty");
        public StatsInt Gifted_SaltMiner = new StatsInt("Gifted_SaltMiner");
        public StatsInt Gifted_PuppetMaster = new StatsInt("Gifted_PuppetMaster");
        public StatsInt Gifted_TheCarry = new StatsInt("Gifted_TheCarry");
        public StatsInt Gifted_OneManArmy = new StatsInt("Gifted_OneManArmy");
        public StatsInt Gifted__4DChessPlayer = new StatsInt("Gifted__4DChessPlayer");
        public StatsInt Gifted_SpreadsheetWarrior = new StatsInt("Gifted_SpreadsheetWarrior");
        public StatsInt Gifted_MeatShield = new StatsInt("Gifted_MeatShield");
        public StatsInt Gifted_InDebt = new StatsInt("Gifted_InDebt");
        public StatsInt Gifted_OnLifeSupport = new StatsInt("Gifted_OnLifeSupport");
        public StatsInt Gifted_LoneWolf = new StatsInt("Gifted_LoneWolf");
        public StatsInt Gifted_ShaggyTooDopeAlwaysChilling = new StatsInt("Gifted_ShaggyTooDopeAlwaysChilling");
        public StatsInt Gifted_BadInfluence = new StatsInt("Gifted_BadInfluence");
        public StatsInt Gifted_HindsightTactician = new StatsInt("Gifted_HindsightTactician");
        public StatsInt Gifted_Houseplant = new StatsInt("Gifted_Houseplant");
        public StatsInt Gifted_Sheep = new StatsInt("Gifted_Sheep");

        public GameStats()
        {
            DssRef.stats = this;
        }

        public override List<IStatsValue> collectTimedValues()
        {
            return new List<IStatsValue>
            {
            };
        }

        public override List<IStatsValue> listGlobalStats()
        {
            return new List<IStatsValue>
                {
                    startUp,
                    blueScreen,
                    startTutorial,
                    completeTutorial,
                    skipTutorial,
                    skip_advisor,

                    keyboard_user,
                    controller_user,

                    saveCustomFlag,
            #if DEMO
                    startNewDemo,
                    completeDemo,
            #else
                    startNewStory,
                    startNewQuickBoss,
                    startQuickMatch,
                    startnew_casual,
                    startNewSandbox,
                    startNewPeaceful,
                    startNewSpectator,

                    startNew25perc,
                    startNew50perc,
                    startNew75perc,
                    startNew100perc,
                    startNew125perc,
                    startNew150perc,
                    startNew175perc,
                    startNew200perc,
                    startNew300perc,

                    startNew_MapSmall,
                    startNew_MapLarge,
                    startNew_MapHuge,

                    startNewLocalMultiplayer,

                    start_voxeleditor,
                    start_mapgenerator,
                    start_character_creator,

                    start_battle_lab,
                    battle_lab_newbattle,
                    start_commander,
                    commander_won,
                    commander_lost,

                    won25perc,
                    won50perc,
                    won75perc,
                    won100perc,
                    won125perc,
                    won150perc,
                    won175perc,
                    won200perc,
                    won300perc,

                    lost25perc,
                    lost50perc,
                    lost75perc,
                    lost100perc,
                    lost125perc,
                    lost150perc,
                    lost175perc,
                    lost200perc,
                    lost300perc,
            #endif
                    gameLength_passed5min,
                    gameLength_passed15min,
                    gameLength_passed30min,
                    gameLength_passed1hour,
            #if !DEMO
                    gameLength_passed2hour,
                    gameLength_passed5hour,
                    gameLength_passed10hour,
                    gameLength_passed20hour,
                    gameLength_passed30hour,
                    gameLength_passed100hour,

                    pickLanguageStart,
                    language_english,
                    language_simplified_chinese,
                    language_japanese,
                    language_russian,
                    language_spanish,
                    language_german,
                    language_french,
                    language_turkish,
                    language_brazilian_portuguese,
                    language_italian,
                    language_korean,

                    Gifted_WhiteKnight,
                    Gifted_HeroComplexSaviorComplex,
                    Gifted_CryBaby,
                    Gifted_KingMaker,
                    Gifted_Turtle,
                    Gifted_MetaPlayer,
                    Gifted_Tryhard,
                    Gifted_DidPracticeInSecret,
                    Gifted_TheEncyclopedia,
                    Gifted_WarCriminal,
                    Gifted_FarmerRush,
                    Gifted_Politian,
                    Gifted_Socializer,
                    Gifted_OverAchiever,
                    Gifted_Noob,
                    Gifted_SwedishNeutrality,
                    Gifted_TroubleMaker,
                    Gifted_ScorchedEarth,
                    Gifted_WarMonger,
                    Gifted_LivingInABobble,
                    Gifted_Bullie,
                    Gifted_ControlFreak,
                    Gifted_RandomNothingMakesSense,
                    Gifted_Hoarder,
                    Gifted_Scatterbrained,
                    Gifted_NearSighted,
                    Gifted_AutomationAbuser,
                    Gifted_Troll,
                    Gifted_MemeLord,
                    Gifted_SupportSlave,
                    Gifted_DarkSidePlayer,
                    Gifted_SlaughterHouse,
                    Gifted_AnimalCruelty,
                    Gifted_LuckyBastard,
                    Gifted_Cursed,
                    Gifted_Backstabber,
                    Gifted_Oathbreaker,
                    Gifted_Wormtongue,
                    Gifted_ArmchairGeneral,
                    Gifted_Salty,
                    Gifted_SaltMiner,
                    Gifted_PuppetMaster,
                    Gifted_TheCarry,
                    Gifted_OneManArmy,
                    Gifted__4DChessPlayer,
                    Gifted_SpreadsheetWarrior,
                    Gifted_MeatShield,
                    Gifted_InDebt,
                    Gifted_OnLifeSupport,
                    Gifted_LoneWolf,
                    Gifted_ShaggyTooDopeAlwaysChilling,
                    Gifted_BadInfluence,
                    Gifted_HindsightTactician,
                    Gifted_Houseplant,
            #endif
                };
        }

        public override void getStats()
        {
            foreach (var stat in listGlobalStats())
            {
                stat.getStat();
            }
        }

        public override void initAndSetStats()
        {
            foreach (var stat in listGlobalStats())
            {
                stat.initAndSet();
            }
        }

        public override void collectValues(float prevTotalTimeSec)
        {
            if (DssRef.time != null)
            {
                var gametime = DssRef.time.TotalIngameTime();
                if (gametime.TotalMinutes >= 5)
                {
                    gameLength_passed5min.addOne_ifUnset();
                }
                if (gametime.TotalMinutes >= 15)
                {
                    gameLength_passed15min.addOne_ifUnset();
                }
                if (gametime.TotalMinutes >= 30)
                {
                    gameLength_passed30min.addOne_ifUnset();
                }

                if (gametime.TotalHours >= 1)
                {
                    gameLength_passed1hour.addOne_ifUnset();
                }
                if (gametime.TotalHours >= 2)
                {
                    gameLength_passed2hour.addOne_ifUnset();
                }
                if (gametime.TotalHours >= 5)
                {
                    gameLength_passed5hour.addOne_ifUnset();
                }
                if (gametime.TotalHours >= 10)
                {
                    gameLength_passed10hour.addOne_ifUnset();
                }
                if (gametime.TotalHours >= 20)
                {
                    gameLength_passed20hour.addOne_ifUnset();
                }
                if (gametime.TotalHours >= 30)
                {
                    gameLength_passed30hour.addOne_ifUnset();
                }
                if (gametime.TotalHours >= 100)
                {
                    gameLength_passed100hour.addOne_ifUnset();
                }
            }
        }
    }
}
