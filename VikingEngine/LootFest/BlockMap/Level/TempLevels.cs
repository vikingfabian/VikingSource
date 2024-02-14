//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VikingEngine.LootFest.GO;

//namespace VikingEngine.LootFest.BlockMap.Level
//{
    

//    class Mount : AbsTemplateLevel
//    {
//        public Mount()
//            : base(LevelEnum.Mount)
//        {
//            spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Pitbull, 0, 3),
//                new SpawnPointData( GameObjectType.SpitChick, 0, 6),
//            });

//            spawnMonstersPart2 = spawnMonstersPart1;

//            terrain = new AbsLevelTerrain[]{new NormalTerrain(this)};
//        }

//        protected override void spawnBoss()
//        {
//            new VikingEngine.LootFest.GO.Characters.GoblinWolfRiderBoss(new GoArgs(bossSpawnPos), this);
//        }
//    }

//    class Statue : AbsTemplateLevel
//    {
//        public Statue()
//            : base(LevelEnum.Statue)
//        {
//            spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Hog, 0, 10),
//                new SpawnPointData( GameObjectType.SpitChick, 0, 10),
//                new SpawnPointData( GameObjectType.GoblinScout, 0, 6),
//                new SpawnPointData( GameObjectType.GoblinLineman, 0, 6),

//            });

//            spawnMonstersPart2 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Lizard, 0, 10),
//                new SpawnPointData( GameObjectType.Frog, 0, 10),
//                new SpawnPointData( GameObjectType.GoblinScout, 0, 3),
//                new SpawnPointData( GameObjectType.GoblinLineman, 0, 3),
//                new SpawnPointData( GameObjectType.FatBird, 0, 3),
//            });

//            terrain = new AbsLevelTerrain[] { new CastleTerrain(this) };
//        }

//        protected override void spawnBoss()
//        {
//            new VikingEngine.LootFest.GO.Characters.Boss.StatueBoss(new GoArgs(bossSpawnPos), this);
//        }
//    }

//    class Birds : AbsTemplateLevel
//    {
//        public Birds()
//            : base(LevelEnum.Birds)
//        {
//            spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.GoblinScout, 0, 5),
//                new SpawnPointData( GameObjectType.Hog, 0, 5),
//                new SpawnPointData( GameObjectType.FatBird, 0, 8),
//                new SpawnPointData( GameObjectType.SpitChick, 0, 6),
//            });

//            spawnMonstersPart2 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Hog, 0, 5),
//                new SpawnPointData( GameObjectType.FatBird, 0, 8),
//                new SpawnPointData( GameObjectType.FatBird, 1, 3),
//                new SpawnPointData( GameObjectType.Bee, 0, 2),
//            });

//            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
//        }

//        protected override void spawnBoss()
//        {
//            new VikingEngine.LootFest.GO.Characters.Boss.BirdRiderBoss(new GoArgs(bossSpawnPos), this);
//        }
//    }

//    class Swine : AbsTemplateLevel
//    {
//        public Swine()
//            : base(LevelEnum.Swine)
//        {
//            spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Pitbull, 0, 10),
//                new SpawnPointData( GameObjectType.SpitChick, 0, 5),
//                new SpawnPointData( GameObjectType.Frog, 0, 6),
//            });

//            spawnMonstersPart2 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Frog, 0, 10),
//                new SpawnPointData( GameObjectType.Bee, 0, 2),
//                new SpawnPointData( GameObjectType.Crocodile, 0, 15),
//            });

//            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
//        }

//        protected override void spawnBoss()
//        {
//            new VikingEngine.LootFest.GO.Characters.Monsters.OldSwineBoss(new GoArgs(bossSpawnPos), this);
//        }
//    }

//    class Desert1 : AbsTemplateLevel
//    {
//        public Desert1()
//            : base(LevelEnum.Desert1)
//        {
//            spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Scorpion, 0, 8),
//                new SpawnPointData( GameObjectType.Beetle1, 0, 4),
//                new SpawnPointData( GameObjectType.GoblinScout, 0, 10),
//                new SpawnPointData( GameObjectType.GoblinLineman, 0, 10),

//            });

//            spawnMonstersPart2 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Scorpion, 0, 8),
//                new SpawnPointData( GameObjectType.Beetle1, 0, 8),
//                new SpawnPointData( GameObjectType.Skeleton, 0, 6),

//                new SpawnPointData( GameObjectType.GoblinScout, 0, 3),
//                new SpawnPointData( GameObjectType.GoblinLineman, 0, 3),
//            });

//            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
//        }

//        protected override void spawnBoss()
//        {
//            new VikingEngine.LootFest.GO.Characters.Boss.BeetleRiderBoss(new GoArgs(bossSpawnPos), this);
//        }
//    }

//    class Desert2 : AbsTemplateLevel
//    {
//        public Desert2()
//            : base(LevelEnum.Desert2)
//        {
//            spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Scorpion, 0, 8),
//                new SpawnPointData( GameObjectType.Beetle1, 0, 4),
//                new SpawnPointData( GameObjectType.OrcSoldier, 0, 10),
//                new SpawnPointData( GameObjectType.OrcArcher, 0, 10),

//            });

//            spawnMonstersPart2 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Scorpion, 0, 8),
//                new SpawnPointData( GameObjectType.Beetle1, 0, 8),
//                new SpawnPointData( GameObjectType.Skeleton, 0, 6),

//                new SpawnPointData( GameObjectType.OrcArcher, 0, 3),
//                new SpawnPointData( GameObjectType.OrcSoldier, 0, 3),
//                new SpawnPointData( GameObjectType.OrcKnight, 0, 3),

//            });

//            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
//        }

//        protected override void spawnBoss()
//        {
//            new VikingEngine.LootFest.GO.Characters.Boss.ScorpionBot(new GoArgs(bossSpawnPos), this);
//        }
//    }

//    class Orcs : AbsTemplateLevel
//    {
//        public Orcs()
//            : base(LevelEnum.Orcs)
//        {
//            spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.GoblinScout, 0, 10),
//                new SpawnPointData( GameObjectType.Pitbull, 0, 10),
//                new SpawnPointData( GameObjectType.OrcSoldier, 0, 6),
//            });

//            spawnMonstersPart2 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.OrcArcher, 0, 10),
//                new SpawnPointData( GameObjectType.Pitbull, 0, 10),
//                new SpawnPointData( GameObjectType.OrcSoldier, 0, 15),
//                new SpawnPointData( GameObjectType.OrcKnight, 0, 4),
//            });

//            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
//        }

//        protected override void spawnBoss()
//        {
//            new VikingEngine.LootFest.GO.Characters.OrcWolfRiderBoss(new GoArgs(bossSpawnPos), this);
//        }
//    }
//    class Wolf : AbsTemplateLevel
//    {
//        public Wolf()
//            : base(LevelEnum.Wolf)
//        {
//            spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.GoblinScout, 0, 10),
//                new SpawnPointData( GameObjectType.GoblinLineman, 0, 10),
//                new SpawnPointData( GameObjectType.Hog, 0, 10),
//                new SpawnPointData( GameObjectType.SpitChick, 0, 10),
//                 new SpawnPointData( GameObjectType.Frog, 0, 2),
//            });

//            spawnMonstersPart2 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.GoblinLineman, 0, 10),
//                new SpawnPointData( GameObjectType.GoblinBerserk, 0, 15),
//                 new SpawnPointData( GameObjectType.SpitChick, 0, 10),
//                new SpawnPointData( GameObjectType.Lizard, 0, 15),
//                 new SpawnPointData( GameObjectType.Frog, 0, 5),
//            });

//            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
//        }

//        protected override void spawnBoss()
//        {
//            new VikingEngine.LootFest.GO.Characters.GoblinWolfRiderBoss(new GoArgs(bossSpawnPos), this);
//        }
//    }
//    class Elf1 : AbsTemplateLevel
//    {
//        public Elf1()
//            : base(LevelEnum.Elf1)
//        {
//            spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.ElfArcher, 0, 10),
//                new SpawnPointData( GameObjectType.ElfWardancer, 0, 10),
//                new SpawnPointData( GameObjectType.Bee, 0, 2),
//                new SpawnPointData( GameObjectType.Spider, 0, 2),
//            });

//            spawnMonstersPart2 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.ElfArcher, 0, 10),
//                new SpawnPointData( GameObjectType.ElfWardancer, 0, 10),
//                new SpawnPointData( GameObjectType.ElfKnight, 0, 10),
//            });

//            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
//        }

//        protected override void spawnBoss()
//        {
//            new VikingEngine.LootFest.GO.Characters.Boss.ElfKing(new GoArgs(bossSpawnPos), this);
//        }
//    }
//    class SkeletonDungeon : AbsTemplateLevel
//    {
//        public SkeletonDungeon()
//            : base(LevelEnum.SkeletonDungeon)
//        {
//            spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Skeleton, 0, 15),
//                new SpawnPointData( GameObjectType.Ghost, 0, 10),
//                new SpawnPointData( GameObjectType.Mummy, 0, 10),
//            });

//            spawnMonstersPart2 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Skeleton, 0, 10),
//                new SpawnPointData( GameObjectType.Ghost, 0, 10),
//                new SpawnPointData( GameObjectType.Mummy, 0, 10),
//                new SpawnPointData( GameObjectType.GreenSlime, 0, 14),
//            });

//            terrain = new AbsLevelTerrain[] { new CastleTerrain(this) };
//        }

//        protected override void spawnBoss()
//        {
//            new VikingEngine.LootFest.GO.Characters.Monsters.Skeleton(new GoArgs(bossSpawnPos), this);
//        }
//    }
//    //class Spider1 : AbsTemplateLevel
//    //{
//    //    public Spider1()
//    //        : base(LevelEnum.Spider1)
//    //    {
//    //        spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//    //        {
//    //            new SpawnPointData( GameObjectType.Spider, 0, 15),
//    //            new SpawnPointData( GameObjectType.GreenSlime, 0, 10),
//    //            new SpawnPointData( GameObjectType.MiniSpider, 0, 10),
//    //        });

//    //        spawnMonstersPart2 = new SuggestedSpawns(new List<SpawnPointData>
//    //        {
//    //            new SpawnPointData( GameObjectType.Bat, 0, 10),
//    //            new SpawnPointData( GameObjectType.Skeleton, 0, 10),
//    //            new SpawnPointData( GameObjectType.Spider, 0, 15),
//    //            new SpawnPointData( GameObjectType.PoisionSpider, 0, 10),
//    //            new SpawnPointData( GameObjectType.MiniSpider, 0, 5),
//    //        });

//    //        terrain = new CaveTerrain(this);
//    //    }

//    //    protected override void spawnBoss()
//    //    {
//    //        new GO.Characters.Boss.GoblinSpiderRiderBoss(new GoArgs(bossSpawnPos), this);
//    //    }
//    //}
//    //class Spider2 : AbsTemplateLevel
//    //{
//    //    public Spider2()
//    //        : base(LevelEnum.Spider2)
//    //    {
//    //        spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//    //        {
//    //            new SpawnPointData( GameObjectType.Spider, 0, 15),
//    //            new SpawnPointData( GameObjectType.PoisionSpider, 0, 10),
//    //            new SpawnPointData( GameObjectType.MiniSpider, 0, 10),
//    //        });

//    //        spawnMonstersPart2 = new SuggestedSpawns(new List<SpawnPointData>
//    //        {
//    //            new SpawnPointData( GameObjectType.BullSpider, 0, 10),
//    //            new SpawnPointData( GameObjectType.Spider, 0, 15),
//    //            new SpawnPointData( GameObjectType.PoisionSpider, 0, 10),
//    //            new SpawnPointData( GameObjectType.MiniSpider, 0, 5),
//    //        });

//    //        terrain = new CaveTerrain(this);
//    //    }

//    //    protected override void spawnBoss()
//    //    {
//    //        new GO.Characters.Boss.SpiderBot(new GoArgs(bossSpawnPos), this);
//    //    }
//    //}

//    //class EndBoss : AbsTemplateLevel
//    //{
//    //    public EndBoss()
//    //        : base(LevelEnum.EndBoss)
//    //    {
//    //        spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//    //        {
//    //            new SpawnPointData( GameObjectType.Mummy, 0, 10),
//    //            new SpawnPointData( GameObjectType.Bat, 0, 10),
//    //            new SpawnPointData( GameObjectType.TrapBackNforward, 0, 10),
//    //            new SpawnPointData( GameObjectType.GreenSlime, 0, 10),
//    //        });

//    //        spawnMonstersPart2 = new SuggestedSpawns(new List<SpawnPointData>
//    //        {
//    //           new SpawnPointData( GameObjectType.Mummy, 0, 10),
//    //            new SpawnPointData( GameObjectType.Bat, 0, 10),
//    //            new SpawnPointData( GameObjectType.Bat, 1, 10),
//    //            new SpawnPointData( GameObjectType.GreenSlime, 0, 10),
//    //            new SpawnPointData( GameObjectType.GreenSlime, 1, 10),

//    //        });

//    //        terrain = new AbsLevelTerrain[] { new CastleTerrain(this) };
//    //    }

//    //    protected override void spawnBoss()
//    //    {
//    //        new VikingEngine.LootFest.GO.Characters.Boss.BigOrcBoss(new GoArgs(bossSpawnPos), this);
//    //    }
//    //}

//    class Challenge_Zombies : AbsTemplateLevel
//    {
//        public Challenge_Zombies()
//            : base(LevelEnum.Challenge_Zombies)
//        {
//            spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Zombie, 0, 10),
//                new SpawnPointData( GameObjectType.Skeleton, 0, 10),
//                new SpawnPointData( GameObjectType.ZombieLeader, 0, 20),
//            });

//            spawnMonstersPart2 = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Zombie, 0, 10),
//                new SpawnPointData( GameObjectType.Skeleton, 0, 10),
//                new SpawnPointData( GameObjectType.ZombieLeader, 0, 10),
//                new SpawnPointData( GameObjectType.Harpy, 0, 10),

//            });

//            terrain = new AbsLevelTerrain[] { new CastleTerrain(this) };
//        }

//        protected override void spawnBoss()
//        {
//            new VikingEngine.LootFest.GO.Characters.Boss.StatueBoss(new GoArgs(bossSpawnPos), this);
//        }
//    }
//}
