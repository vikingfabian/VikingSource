using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest.MapGen;

namespace VikingEngine.ToGG.HeroQuest.QueAction
{
    class MonsterSpawnGroup : ToggEngine.QueAction.AbsQueAction
    {
        Time delayTime = Time.Zero;
        List<HqUnitType> spawn;
        IntVector2 centerPos;
        public bool delayedStart = false;

        public MonsterSpawnGroup(List<HqUnitType> spawn)
            : base()
        {
            this.spawn = spawn;
        }

        public MonsterSpawnGroup(MonsterGroupType groupType, float goalDifficulty)
            : base()
        {
            spawn = new List<HqUnitType>(8);

            float currentSpawnDifficulty = 0;
            goalDifficulty *= hqRef.setup.playerCountDifficulty;

            MonsterGroupSetup groupSetup = MapSpawnLib.GroupSetups[(int)groupType];
            List<HqUnitType> groupMembers = new List<HqUnitType>(8);

            while (true)
            {
                groupMembers.Clear();
                groupSetup.getOneGroup(Ref.rnd, groupMembers);

                foreach (var unitType in groupMembers)
                {
                    var data = hqRef.unitsdata.Get(unitType);
                    spawn.Add(unitType);
                    currentSpawnDifficulty += data.UnitDifficulty;

                    if (currentSpawnDifficulty >= goalDifficulty)
                    {
                        return;
                    }
                }
            }
        }

        public override void onBegin()
        {
            base.onBegin();

            if (delayedStart)
            {
                delayTime = new Time(1.4f, TimeUnit.Seconds);
            }

            refreshAvailableSquares();

            centerPos = findBestSpawnSquare();

            List<IntVector2> positions = getSpawnPositions(centerPos, spawn.Count);

            List<Unit> units = new List<Unit>(spawn.Count);

            for (int i = 0; i < spawn.Count; ++i)
            {
                if (positions.Count > i)
                {
                    IntVector2 pos = positions[i];

                    var u = new Unit(pos, spawn[i], hqRef.players.dungeonMaster);
                    u.Alert();
                    units.Add(u);
                }
            }

            hqRef.players.dungeonMaster.netWriteSpawn(units);
        }

        void refreshAvailableSquares()
        {
            var loop = toggRef.board.tileGrid.LoopInstance();
            while (loop.Next())
            {
                var sq = toggRef.board.tileGrid.Get(loop.Position);
                sq.monsterSpawnAvailable = false;

                if (sq.Revealed && toggRef.board.IsSpawnAvailableSquare(loop.Position))
                {
                    sq.monsterSpawnAvailable = true;

                    var units = hqRef.players.teamUnits(Players.PlayerCollection.HeroTeam);
                    while (units.Next())
                    {
                        if (units.sel.InLineOfSight(loop.Position))
                        {
                            sq.monsterSpawnAvailable = false;
                            break;
                        }
                    }
                }
            }
        }

        IntVector2 findBestSpawnSquare()
        {
            var units = hqRef.players.teamUnits(Players.PlayerCollection.HeroTeam).array.toList();

            IntVector2 best = IntVector2.NegativeOne;
            int bestValue = int.MinValue;

            var loop = toggRef.board.tileGrid.LoopInstance();
            while (loop.Next())
            {
                var sq = toggRef.board.tileGrid.Get(loop.Position);
                if (sq.monsterSpawnAvailable)
                {
                    int adjacentAvailable = 0;
                    int dist = 0;

                    foreach (var dir in IntVector2.Dir8Array)
                    {
                        var adjSq = toggRef.Square(loop.Position + dir);
                        if (adjSq != null && adjSq.monsterSpawnAvailable)
                        {
                            ++adjacentAvailable;
                        }
                    }

                    var closest = Ai.ClosestAndWeakestUnit(loop.Position, units);

                    if (closest != null)
                    {
                        dist = loop.Position.SideLength(closest.squarePos);
                    }

                    int value = adjacentAvailable - dist;

                    if (value > bestValue)
                    {
                        best = loop.Position;
                        bestValue = value;
                    }
                }
            }

            return best;
        }

        List<IntVector2> getSpawnPositions(IntVector2 center, int count)
        {
            List<IntVector2> result = new List<IntVector2>(count);
            result.Add(center);

            int radius = 1;

            for (int i = 0; i < 10; ++i)
            {
                ForXYEdgeLoop edgeLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(center, radius));
                while (edgeLoop.Next())
                {
                    if (toggRef.board.IsSpawnAvailableSquare(edgeLoop.Position))
                    {
                        result.Add(edgeLoop.Position);
                        if (result.Count >= count)
                        {
                            return result;
                        }
                    }
                }
                ++radius;
            }

            return result;
        }

        public override bool CameraTarget(out IntVector2 camTarget, out bool inCamCheck)
        {
            camTarget = centerPos;
            inCamCheck = false;

            return delayTime.TimeOut;
        }

        public override bool update()
        {
            if (delayTime.CountDown())
            {
                return base.update();
            }
            return false;
        }

        public override bool NetShared => false;

        public override ToggEngine.QueAction.QueActionType Type => ToggEngine.QueAction.QueActionType.MonsterSpawnGroup;
    }
}
