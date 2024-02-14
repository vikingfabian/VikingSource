using VikingEngine.ToGG.Commander;

namespace VikingEngine.ToGG
{
    /// <summary>
    /// Settings for debug quick startup
    /// </summary>
    static class StartUpSett
    {
        public static readonly bool QuickRunInSinglePlayer =
#if DEBUG
            false;
#else
            false;
#endif
        public static readonly HeroQuest.HqUnitType QuickRunHeroClass = HeroQuest.HqUnitType.KnightHero;
        public static readonly HeroQuest.QuestName? QuickRunLevel = HeroQuest.QuestName.testchamber;

        public static readonly bool AlertAllMonsters = false;

        public static readonly ArmyRace cmdAutoPickMyArmy = ArmyRace.Elf;
        public static readonly ArmyRace cmdAutoPickAiArmy = ArmyRace.Orc;

        public static readonly bool RunMoonFall =
#if DEBUG
            false;
#else
false;
#endif
        public static readonly bool RunCommander =
#if DEBUG
            false;
#else
false;
#endif
    }
}
