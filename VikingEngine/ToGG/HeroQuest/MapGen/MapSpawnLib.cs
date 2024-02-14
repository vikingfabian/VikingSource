using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.MapGen
{
    static class MapSpawnLib
    {
        /*
         * Using AREAs as the standard for map size and spawn rate
         * 
         */
        public const int AreaSize = 4;
        public const int AreaTileCount = AreaSize * AreaSize;

        public const int FlagDefaultRadius = AreaSize * 2;

        public const float DefaultEnemyDifficulty = 1f;
        public const float AreaDefaultSpawnDifficulty = 1.7f;
        public const float LootSpawnsPerArea = 0.25f;

        public static MonsterGroupSetup[] GroupSetups;
        public static AreaTypeSetup[] AreaSetups;

        public static void Init()
        {
            GroupSetups = new MonsterGroupSetup[(int)MonsterGroupType.NUM_NONE];
            {
                new MonsterGroupSetup(MonsterGroupType.Beasts,
                    new HqUnitType[] { HqUnitType.Beastman },
                    new HqUnitType[] { HqUnitType.HeavyBeastman });

                new MonsterGroupSetup(MonsterGroupType.Skeletons,
                    new HqUnitType[] { HqUnitType.SkeletonArcher, HqUnitType.SkeletonPeasant, HqUnitType.SkeletonSoldier },
                    new HqUnitType[] { HqUnitType.DarkPriest });

                new MonsterGroupSetup(MonsterGroupType.Goblins,
                    new HqUnitType[] { HqUnitType.GoblinArcher, HqUnitType.GoblinSoldier });

                new MonsterGroupSetup(MonsterGroupType.GoblinRobbers,
                    new HqUnitType[] { HqUnitType.GoblinRunner, HqUnitType.GoblinArcher, HqUnitType.GoblinSoldier });

                new MonsterGroupSetup(MonsterGroupType.OrcsAndGoblins,
                    new HqUnitType[] { HqUnitType.GoblinArcher, HqUnitType.GoblinSoldier, HqUnitType.GoblinWolfRider },
                    new HqUnitType[] { HqUnitType.Ogre, HqUnitType.GoblinWolfRiderCommander });

                new MonsterGroupSetup(MonsterGroupType.OrcGuards,
                    new HqUnitType[] { HqUnitType.GoblinArcher, HqUnitType.OrcGuard },
                    new HqUnitType[] { HqUnitType.Ogre, HqUnitType.GoblinWolfRiderCommander });

                new MonsterGroupSetup(MonsterGroupType.WolfRiders,
                    new HqUnitType[] { HqUnitType.GoblinWolfRider },
                    new HqUnitType[] { HqUnitType.GoblinWolfRiderCommander });

                new MonsterGroupSetup(MonsterGroupType.LizardsAndSnakes,
                    new HqUnitType[] { HqUnitType.Firelizard, HqUnitType.RabidLizard, HqUnitType.Naga },
                    new HqUnitType[] { HqUnitType.NagaCommander });

                new MonsterGroupSetup(MonsterGroupType.Spiders,
                    new HqUnitType[] { HqUnitType.CaveSpider });

                new MonsterGroupSetup(MonsterGroupType.Relaxed,
                   new HqUnitType[] { HqUnitType.Bat });
            }

            AreaSetups = new AreaTypeSetup[(int)AreaType.NUM];
            {
                new AreaTypeSetup(AreaType.Default,
                    new MonsterGroupType[]
                    {
                    MonsterGroupType.Relaxed,
                    MonsterGroupType.Beasts,
                    MonsterGroupType.Goblins,
                    MonsterGroupType.Spiders,
                    },
                new MonsterGroupType[]
                    {
                    MonsterGroupType.Skeletons,
                    MonsterGroupType.Beasts,
                    MonsterGroupType.Goblins,
                    MonsterGroupType.WolfRiders,
                    MonsterGroupType.Spiders,
                    MonsterGroupType.LizardsAndSnakes,
                    MonsterGroupType.OrcsAndGoblins,
                    },
                new MonsterGroupType[]
                    {
                    MonsterGroupType.LizardsAndSnakes,
                    MonsterGroupType.OrcsAndGoblins,
                    }
                );

                new AreaTypeSetup(AreaType.SmallCave,
                    new MonsterGroupType[]
                    {
                    MonsterGroupType.Relaxed,
                    MonsterGroupType.Spiders,
                    },
                new MonsterGroupType[]
                    {
                    MonsterGroupType.Relaxed,
                    MonsterGroupType.Spiders,
                    },
                new MonsterGroupType[]
                    {
                    MonsterGroupType.Spiders,
                    }
                );

                new AreaTypeSetup(AreaType.GoblinForest,
                   new MonsterGroupType[]
                   {
                    MonsterGroupType.GoblinRobbers,
                   },
               new MonsterGroupType[]
                   {
                    MonsterGroupType.Goblins,
                    MonsterGroupType.WolfRiders,
                   },
               new MonsterGroupType[]
                   {
                    MonsterGroupType.OrcsAndGoblins,
                   }
               );
            }
        }

    }

    class AreaTypeSetup
    {
        MonsterGroupType[] lowtier, defaultTier, highTier;

        public AreaTypeSetup(AreaType area, MonsterGroupType[] lowtier, MonsterGroupType[] defaultTier, MonsterGroupType[] highTier)
        {
            this.lowtier = lowtier;
            this.defaultTier = defaultTier;
            this.highTier = highTier;

            MapSpawnLib.AreaSetups[(int)area] = this;
        }

        public MonsterGroupType[] Get(int tier)
        {
            switch (tier)
            {
                case 0: return lowtier;
                default: return defaultTier;
                case 2: return highTier;
            }
        }
    }

    class MonsterGroupSetup
    {
        static readonly Range MinionCountRange = new Range(2, 5);
        //First in array is more common
        HqUnitType[] boss = null;
        HqUnitType[] leader = null;
        HqUnitType[] minion = null;

        public MonsterGroupSetup(MonsterGroupType group, HqUnitType[] minion, HqUnitType[] leader = null, HqUnitType[] boss = null)
        {
            this.boss = boss;
            this.leader = leader;
            this.minion = minion;

            if (group != MonsterGroupType.NUM_NONE)
            {
                MapSpawnLib.GroupSetups[(int)group] = this;
            }
        }

        public void getOneGroup(PcgRandom rnd, List<HqUnitType> result)
        {   
            int count = MinionCountRange.GetRandom(rnd);

            for (int i = 0; i < count; ++i)
            {
                result.Add(arraylib.RandomListMember(minion, rnd));
            }

            if (leader != null && rnd.Chance(0.7))
            {
                result.Add(arraylib.RandomListMember(leader, rnd));
            }
        }
    }

    enum GroupUnitTier
    {
        Boss,
        Leader,
        Minion,
    }
}
