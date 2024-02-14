using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.HeroQuest.MapGen;
using VikingEngine.ToGG.HeroQuest.Players.Ai;

namespace VikingEngine.ToGG.HeroQuest.Data.LevelConditions
{
    class GoblinCounter : DefaultLevelConditions
    {
        //TAGS
        //1. Treasure
        //2. Villagers
        //3. Enemy wave1
        //4. Enemy wave2
        //const int Skulls = 10;
        const int Turns = 8;

        const float StartDifficulty = 7;
        const float Turn1Difficulty = 5;
        const float Turn3Difficulty = 7;
        const float Turn4Difficulty = 7;

        SpawnManager spawnManager;
        //List<IntVector2> barrackTargets;
        MonsterGroupSetup wave1Monsters, wave2Monsters;
        public GoblinCounter()
        {            
            doom = new DoomData(10);
            doom.goldChest = doom.TotalSkullCount - 2;
            doom.silverChest = doom.goldChest - 3;

            wave1Monsters = new MonsterGroupSetup(MonsterGroupType.NUM_NONE,
                new HqUnitType[]
                {
                    HqUnitType.GoblinArcher,
                    HqUnitType.GoblinSoldier,
                }
                ,
                new HqUnitType[]
                {
                    HqUnitType.GoblinBloated,
                }
                );

            wave2Monsters = new MonsterGroupSetup(MonsterGroupType.NUM_NONE,
                new HqUnitType[]
                {
                    HqUnitType.GoblinArcher,
                    HqUnitType.GoblinSoldier,
                    HqUnitType.GoblinWolfRider,
                    HqUnitType.CannonTroll,
                },
                new HqUnitType[]
                {
                    HqUnitType.GoblinWolfRiderCommander,
                });
        }

        override public List<AbsRichBoxMember> questDescription()
        {
            List<AbsRichBoxMember> rb = new List<AbsRichBoxMember>();
            flavorText(rb, "A goblin army came for revenge, we quickly escorted the villagers into nearby barracks.");
            flavorText(rb, "The king has lended you soldiers from his personal guard - use them well.");

            missionObjectivesTitle(rb);
            rb.Add(new RichBoxText("Defend the barracks for " + Turns.ToString() + " turns"));
            rb.Add(new RichBoxNewLine());

            specialConditionsTitle(rb);
            rb.Add(new RichBoxImage(SpriteName.DoomSkull));
            rb.Add(new RichBoxText(": A monster may attack one of the barracks"));
            rb.Add(new RichBoxNewLine());

            //turns
            turnLimitText(rb);

            return rb;
        }

        public override void monsterSpawn(SpawnManager spawnManager)
        {
            this.spawnManager = spawnManager;

            //1. Treasure
            //2. Villagers
            //3. Enemy start
            objectiveAttackTargets = toggRef.board.metaData.tags.list(2);
            spawnTreasures();

            var startWave = toggRef.board.metaData.tags.list(3);

            spawnUnits(StartDifficulty, true, startWave);
        }

        public override List<Unit> monsterRespawnSession(int turn, bool beforeActions)
        {
            List<Unit> spawns = null;

            if (!beforeActions)
            {
                switch (turn)
                {
                    case 1:
                        spawns = spawnUnits(Turn1Difficulty, true, respawnPositions());
                        break;
                    case 3:
                        spawns = spawnUnits(Turn3Difficulty, false, respawnPositions());
                        break;
                    case 4:
                        spawns = spawnUnits(Turn4Difficulty, false, respawnPositions());
                        break;
                }
            }

            return spawns;
        }

        List<IntVector2> respawnPositions()
        {
            int enter1, enter2;

            enter1 = Ref.rnd.Int(1, 6);

            do
            {
                enter2 = Ref.rnd.Int(1, 6);
            } while (enter1 != enter2);

            List<IntVector2> result = new List<IntVector2>(16);
            result.AddRange(toggRef.board.metaData.mapEntrace.list(enter1));
            result.AddRange(toggRef.board.metaData.mapEntrace.list(enter2));

            return result;
        }

        List<Unit> spawnUnits(float difficulty, bool wave1, List<IntVector2> positions)
        {
            List<Unit> units = new List<Unit>();

            spawnManager.spawnGropsAfterGoal(wave1? wave1Monsters : wave2Monsters,
                difficulty * spawnManager.playerCountDifficulty,
                positions, units);

            foreach (var m in units)
            {
                m.Alert();
            }

            return units;
        }

        void spawnTreasures()
        {
            var treasureTags = toggRef.board.metaData.tags.list(1);

            for (int i = 0; i < 2; ++i)
            {
                var pos = arraylib.RandomListMemberPop(treasureTags, spawnManager.rnd);
                ToggEngine.TileObjLib.CreateObject(ToggEngine.TileObjectType.ItemCollection,
                    pos, new Gadgets.TileItemCollData(LootLevel.Level2), false);
            }

            for (int i = 0; i < 2; ++i)
            {
                var pos = arraylib.RandomListMemberPop(treasureTags, spawnManager.rnd);
                ToggEngine.TileObjLib.CreateObject(ToggEngine.TileObjectType.ItemCollection,
                    pos, new Gadgets.TileItemCollData(LootLevel.Level3), false);
            }
        }

        void spawnAllyGuards()
        {
            var setup = new Net.AllyUnitsSetup();
            setup.begin();
            {
                int guardsPerPlayer = hqRef.players.HeroPlayersCount <= 2 ? 2 : 1;
                var players = hqRef.players.allTeamPlayers(Players.PlayerCollection.HeroTeam);
                bool meleeSoldier = true;

                foreach (var p in players)
                {
                    for (int i = 0; i < guardsPerPlayer; ++i)
                    {
                        setup.nextUnit(meleeSoldier ? 
                            HqUnitType.KingsGuardSpearman : HqUnitType.KingsGuardArcher,
                            (Players.AbsHQPlayer)p);

                        lib.Invert(ref meleeSoldier);
                    }
                }
            }setup.end();
        }

        public override void OnEvent(EventType eventType, object tag)
        {
            if (eventType == EventType.GameStart && hqRef.netManager.host)
            {
                spawnAllyGuards();
            }

            if (eventType == EventType.TurnEnd &&
                hqRef.players.currentTeam == Players.PlayerCollection.DungeonMasterTeam)
            {
                if (hqRef.players.dungeonMaster.TurnsCount == Turns ||
                    hqRef.players.dungeonMaster.hqUnits.units.Count == 0)
                {
                    new QueAction.GameOver(true);
                }
            }

            base.OnEvent(eventType, tag);
        }

        public override void OnObjective(Unit unit, AttackTargetGroup targetGroup, AiObjectiveType objectiveType, bool local)
        {
            if (local)
            {
                if (objectiveType == AiObjectiveType.AttackObject &&
                    targetGroup != null)
                {
                    new QueAction.DoomSkullObjective(targetGroup[0].position);
                }
            }
            base.OnObjective(unit, targetGroup, objectiveType, local);
        }

        override public DoomClockType DoomClock => DoomClockType.NoClock;
        override public int? TimeLimit => Turns;
        override public bool EnemyLootDrop => false;
    }
}
