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
    class GoblinFoodBoss : DefaultLevelConditions
    {
        Unit boss;
        AiObjectiveMoveToUnit feedObjective;

        const int Heal = 8;
        const int DamageFeedTrigger = Heal - 3;
        BellValue foodHeal = new BellValue(Heal);
        const int DefaultFeederCount = 6;

        public GoblinFoodBoss()
        {
            doom = new DoomData(10);
            doom.goldChest = doom.TotalSkullCount - 3;
            doom.silverChest = doom.goldChest - 3;
        }

        override public List<AbsRichBoxMember> questDescription()
        {
            List<AbsRichBoxMember> rb = new List<AbsRichBoxMember>();
            flavorText(rb, "After tracking the goblins into the forest, we found their nest.");

            missionObjectivesTitle(rb);
            rb.Add(new RbText("Defeat the Goblin boss"));
            rb.Add(new RbNewLine());

            specialConditionsTitle(rb);
            rb.Add(new RbText("Goblins may feed the boss to heal him"));

            return rb;
        }

        public override void monsterSpawn(SpawnManager spawnManager)
        {
            base.monsterSpawn(spawnManager);

            boss = hqRef.players.dungeonMaster.hqUnits.Get(HqUnitType.GoblinBoss);
            feedObjective = new AiObjectiveMoveToUnit(boss);

            var feederTags = toggRef.board.metaData.tags.list(2);
            int feederCount = MathExt.MultiplyInt(spawnManager.playerCountDifficulty, DefaultFeederCount);

            Bound.Max(ref feederCount, feederTags.Count);

            for (int i = 0; i < feederCount; ++i)
            {
                var pos = arraylib.RandomListMemberPop(feederTags, spawnManager.rnd);

                var unit = new Unit(pos, HqUnitType.GoblinRunner, hqRef.players.dungeonMaster);
                unit.Alert();
                unit.addProperty(new UnitAiIdle());
                unit.addProperty(new Gadgets.CarryProperty(new Gadgets.FoodCarry()));
            }
        }

        public override void OnEvent(EventType eventType, object tag)
        {
            //TODO NET
            if (hqRef.netManager.host)
            {
                if (eventType == EventType.TurnStart && ((Players.AbsHQPlayer)tag).IsDungeonMaster)
                {
                    if (boss.health.ValueRemoved >= DamageFeedTrigger)
                    {
                        //Boss has taken damage
                        var runningToFeed = hqRef.players.dungeonMaster.hqUnits.GetList_AiObjective(UnitAiState.ObjectiveGoal);

                        if (runningToFeed.Count == 0)
                        {
                            //Activate one feeder
                            var feeders = hqRef.players.dungeonMaster.hqUnits.GetList(UnitPropertyType.CarryItem);
                            if (feeders.Count > 0)
                            {
                                var rndUnit = arraylib.RandomListMember(feeders).hq();
                                activateFeeder(rndUnit);

                                var w = netWriteConditionEvent(ActivateFeederEvent);
                                rndUnit.netWriteUnitId(w);
                            }
                        }
                    }
                }
            }

            base.OnEvent(eventType, tag);
        }

        const byte ActivateFeederEvent = 0;
        const byte BoosFoodHealEvent = 1;

        public override void onConditionEvent(byte eventId, BinaryReader r)
        {
            switch (eventId)
            {
                case ActivateFeederEvent:
                    {
                        var unit = Unit.NetReadUnitId(r);
                        if (unit != null)
                        {
                            activateFeeder(unit);
                        }
                    }
                    break;
                case BoosFoodHealEvent:
                    {
                        var unit = Unit.NetReadUnitId(r);
                        HealUnit heal = new HealUnit(r);

                        if (unit != null)
                        {
                            healBoss(unit, heal);
                        }
                    }
                    break;
            }
        }

        void activateFeeder(Unit unit)
        {
            unit.data.properties.remove(UnitPropertyType.UnitAiIdle);
            unit.data.properties.Add(new UnitAiObjectiveAlways(feedObjective));
        }

        public override void OnObjective(Unit unit, AttackTargetGroup targetGroup, 
            AiObjectiveType objectiveType, bool local)
        {
            if (objectiveType == AiObjectiveType.MoveTo)
            {
                if (local)
                {
                    HealUnit heal = new HealUnit(
                        boss,
                        foodHeal.Next(hqRef.players.dungeonMaster.Dice),
                         HealType.Nature,
                         false, false);
                    var w = netWriteConditionEvent(BoosFoodHealEvent);
                    
                    unit.netWriteUnitId(w);
                    heal.write(w);

                    healBoss(unit, heal);
                }
            }
        }

        void healBoss(Unit unit, HealUnit heal)
        {
            unit.data.properties.removeAiState();
            unit.data.properties.remove(UnitPropertyType.CarryItem);

            heal.apply();

            new ToggEngine.Display3D.UnitMessageRichbox(boss, "Yum!");
        }

        override public bool EnemyLootDrop => false;
    }
}
