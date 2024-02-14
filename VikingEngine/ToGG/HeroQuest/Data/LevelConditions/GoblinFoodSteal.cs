using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.HeroQuest.MapGen;
using VikingEngine.ToGG.HeroQuest.Players.Ai;

namespace VikingEngine.ToGG.HeroQuest.Data.LevelConditions
{
    class GoblinFoodSteal : DefaultLevelConditions
    {
        const int Turns = 6;
        const int BossTurn = 2;
        SpawnManager spawnManager;
        AiObjectiveCollect aiObjective;

        public GoblinFoodSteal()
        {
            doom = new DoomData(5);
            doom.silverChest = 3;
        }

        override public List<AbsRichBoxMember> questDescription()
        {
            List<AbsRichBoxMember> rb = new List<AbsRichBoxMember>();
            flavorText(rb, "A band of goblin bandits is trying to steal the village's food supply.");

            missionObjectivesTitle(rb);
            rb.Add(new RichBoxText("Defend the village for " + Turns.ToString() + " turns"));
            rb.Add(new RichBoxNewLine());

            specialConditionsTitle(rb);
            rb.Add(new RichBoxImage(SpriteName.DoomSkull));
            rb.Add(new RichBoxText(": If a goblin leaves the map with food"));
            rb.Add(new RichBoxNewLine());

            //turns
            turnLimitText(rb);

            return rb;
        }
        
        public override void monsterSpawn(SpawnManager spawnManager)
        {
            this.spawnManager = spawnManager;

            aiObjective = new AiObjectiveCollect();
            {
                aiObjective.collect = toggRef.board.metaData.foodstorage.interactPositions;
                aiObjective.bringTo = toggRef.board.metaData.mapEntrace.listAll();
            }

            var attackers = toggRef.board.metaData.tags.list(1);
            var thiefs = toggRef.board.metaData.tags.list(2);

            List<Unit> units = new List<Unit>();
            spawnManager.spawnGroupOnTags(MonsterGroupType.GoblinRobbers,
                attackers, 4f, units);

            foreach (var m in units)
            {
                m.Alert();
            }
            units.Clear();

            spawnManager.spawnGroupOnTags(MonsterGroupType.GoblinRobbers,
                thiefs, 4f, units);

            prepUnits(units, true);
        }

        public override List<Unit> monsterRespawnSession(int turn, bool beforeActions)
        {
            if (!beforeActions)
            {
                List<Unit> units = new List<Unit>();
                var tags = toggRef.board.metaData.mapEntrace.listAll();

                switch (turn)
                {
                    case 1:
                        spawnManager.spawnGroupOnTags(MonsterGroupType.GoblinRobbers,
                            tags, 3f, units);

                        prepUnits(units, false);
                        break;

                    case BossTurn:
                        spawnManager.spawnGroupOnTags(MonsterGroupType.WolfRiders,
                            tags, 4f, units);

                        foreach (var m in units)
                        {
                            m.Alert();
                        }
                        break;

                    case 3:
                        spawnManager.spawnGroupOnTags(MonsterGroupType.GoblinRobbers,
                            tags, 4f, units);

                        prepUnits(units, false);
                        break;
                }

                return units;     
            }

            return null;
        }

        void prepUnits(List<Unit> units, bool allOnObjective)
        {
            var dice = hqRef.players.dungeonMaster.Dice;

            foreach (var m in units)
            {
                m.Alert();

                float rnd = dice.next();

                if (rnd < 0.25f)
                {
                    m.data.properties.Add(new UnitAiObjectiveAlways(aiObjective));
                }
                else if (rnd < 0.8f || allOnObjective)
                {
                    m.data.properties.Add(new UnitAiObjective(aiObjective));
                }
            }
        }

        public override void OnObjective(Unit unit, AttackTargetGroup targetGroup, 
            AiObjectiveType objectiveType, bool local)
        {
            Debug.Log("Condition, OnObjective " + unit.ToString() + unit.squarePos.ToString() + 
                ", " + objectiveType.ToString());

            if (objectiveType == AiObjectiveType.CollectItem)
            {
                unit.addProperty(new Gadgets.CarryProperty(new Gadgets.FoodCarry()));
                new ToggEngine.Display3D.UnitMessageRichbox(unit, "Picked food");

                if (local)
                {
                    netWriteObjective(unit, objectiveType);
                }
            }
            else
            {
                if (!toggRef.board.metaData.mapEntrace.listAll().Contains(unit.squarePos))
                {
                    lib.DoNothing();
                }
                new QueAction.DoomSkullObjective(unit, true);
            }
        }

        public override void OnEvent(EventType eventType, object tag)
        {
            if (eventType == EventType.TurnEnd &&
                hqRef.players.IsDungeonMasterTurn &&
                hqRef.players.dungeonMaster.TurnsCount == Turns)
            {
                new QueAction.GameOver(true);
            }

            base.OnEvent(eventType, tag);
        }

        override public DoomClockType DoomClock => DoomClockType.NoClock;

        override public int? TimeLimit => Turns;
        override public bool EnemyLootDrop => false;
    }
}
