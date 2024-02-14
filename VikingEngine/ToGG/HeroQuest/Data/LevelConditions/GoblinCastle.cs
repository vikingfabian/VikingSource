using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.HeroQuest.MapGen;
using VikingEngine.ToGG.HeroQuest.Players.Ai;

namespace VikingEngine.ToGG.HeroQuest.Data.LevelConditions
{
    class GoblinCastle : DefaultLevelConditions
    {
        AiObjectiveInteract aiObjective;

        public GoblinCastle()
        {
            doom = new DoomData(17);
            doom.goldChest = doom.TotalSkullCount - 5;
            doom.silverChest = doom.goldChest - 3;
        }

        override public List<AbsRichBoxMember> questDescription()
        {
            List<AbsRichBoxMember> rb = new List<AbsRichBoxMember>();
            flavorText(rb, "Scouts has found the leader behind the goblin attacks. It is time to pay him a visit.");

            missionObjectivesTitle(rb);
            rb.Add(new RichBoxText("Defeat the Goblin army leader"));
            rb.Add(new RichBoxNewLine());

            specialConditionsTitle(rb);
            rb.Add(new RichBoxText("Some guards has the Objective to ring the alarm bell, and spawn a group of soldiers"));

            return rb;
        }

        public override void monsterSpawn(SpawnManager spawnManager)
        {
            aiObjective = new AiObjectiveInteract();
            aiObjective.pos = toggRef.board.metaData.alarmbell.interactPositions;

            var guardTags = toggRef.board.metaData.tags.list(1);

            foreach (var pos in guardTags)
            {
                var guard = new Unit(pos, HqUnitType.GoblinSoldier, hqRef.players.dungeonMaster);
                int dogCount = hqRef.setup.setupForPlayerCount - 2;

                IntVector2 dogPos = pos;

                for (int i = 0; i < dogCount; ++i)
                {
                    dogPos = spawnManager.randomAdjacentSpawnAvailable(dogPos).Value;
                    new Unit(dogPos,
                        HqUnitType.GuardDog, hqRef.players.dungeonMaster);
                }
            }

            //Tag 2, 3 är vakter med nyckel
            keyGuard(2, Gadgets.RuneKeyType.Hera);
            keyGuard(3, Gadgets.RuneKeyType.Froe);

            var doorGuards = toggRef.board.metaData.tags.list(5);
            foreach (var pos in doorGuards)
            {
                new Unit(pos, HqUnitType.OrcGuard, hqRef.players.dungeonMaster);
            }

            var bellRingers = toggRef.board.metaData.tags.list(4);
            foreach (var pos in bellRingers)
            {
                var guard = new Unit(pos, HqUnitType.GoblinGuard, hqRef.players.dungeonMaster);
                guard.addProperty(new UnitAiObjectiveAlways(aiObjective));
            }

            var bossRoom = toggRef.board.metaData.tags.list(10);
            new Unit(arraylib.RandomListMemberPop(bossRoom, spawnManager.rnd), 
                HqUnitType.GoblinKnightBoss, hqRef.players.dungeonMaster);

            int bossGuardsCount = hqRef.setup.setupForPlayerCount - 1;
            for (int i = 0; i < bossGuardsCount; ++i)
            {
                new Unit(arraylib.RandomListMemberPop(bossRoom, spawnManager.rnd),
                HqUnitType.OrcGuard, hqRef.players.dungeonMaster);
            }
        }

        void keyGuard(int tag, Gadgets.RuneKeyType key)
        {
            IntVector2 pos = toggRef.board.metaData.tags.first(tag);
            var guard = new Unit(pos, HqUnitType.GoblinGuard, hqRef.players.dungeonMaster);

            guard.addProperty(new Gadgets.CarryProperty(new Gadgets.RuneKey(key)));
            
            guard.addProperty(new UnitAiObjectiveAlways(aiObjective));
        }

        public override void OnObjective(Unit unit, AttackTargetGroup targetGroup,
           AiObjectiveType objectiveType, bool local)
        {
            Debug.Log("Condition, OnObjective " + unit.ToString() + unit.squarePos.ToString() +
                ", " + objectiveType.ToString());

            if (objectiveType == AiObjectiveType.MoveTo)
            {
                if (unit.MovedThisTurn)
                {
                    //new ToggEngine.Display3D.UnitMessageRichbox(unit, "Bell ready");
                    bellReady(unit);

                    var w = netWriteConditionEvent(0);
                    unit.netWriteUnitId(w);
                }
                else
                {
                    unit.data.properties.removeAiState();
                    new ToggEngine.Display3D.UnitMessageRichbox(unit, "ALARM!");

                    if (local)
                    {
                        var spawn = new QueAction.MonsterSpawnGroup(MonsterGroupType.OrcGuards, 6);
                        spawn.delayedStart = true;
                        netWriteObjective(unit, objectiveType);
                    }
                }
            }
        }

        public override void onConditionEvent(byte eventId, BinaryReader r)
        {
            base.onConditionEvent(eventId, r);

            if (eventId == 0)
            {
                var u = Unit.NetReadUnitId(r);
                bellReady(u);
            }
        }

        void bellReady(Unit unit)
        {
            if (unit != null)
            {
                new ToggEngine.Display3D.UnitMessageRichbox(unit, "Bell ready");
            }
        }
    }
}
