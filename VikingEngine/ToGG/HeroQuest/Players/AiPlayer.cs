using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.Data;
using VikingEngine.ToGG.HeroQuest.Players.Ai;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Players
{
    class AiPlayer : AbsHQPlayer
    {
        const bool DebugAsynchActions = true;

        List2<Unit> activationOrder;
        UnitAiActions unitActions = new UnitAiActions(null);
        
        MoveLinesGroup movelines;

        AiState state = AiState.None;
        Time stateTime = Time.Zero;

        public AiPlayer(Engine.AiPlayerData pData)
            : base(pData)
        {
            pData.name = "Dungeon Master";
            activationOrder = new List2<Unit>(32);
            
        }

        public override void onTurnStart()
        {
            turnStartGenericSetup();
            base.onTurnStart();

            hqUnits.unitsCounter.Reset();
            while (unitsColl.unitsCounter.Next())
            {
                hqUnits.unitsCounter.sel.ai_onTurnStart();
            }

            SpectatorTargetPos = IntVector2.NegativeOne;
            stateTime.Seconds = 2f;

            if (hqRef.netManager.host)
            {
                setState(AiState.Start);
            }
            else
            {
                setState(AiState.None);
            }

            hqRef.playerHud.createDoomBar();

            if (Display.DoomTrack.ViewTime)
            {
                new QueAction.DoomClock();
            }
        }

        public override void onTurnEnd()
        {
            base.onTurnEnd();
            hqRef.playerHud.removeDoomBar();
            //hqUnits.OnEvent(ToGG.Data.EventType.TurnEnd, null);
            clearClientVisuals();
        }

        public override void startGame()
        {
            base.startGame();
            new Timer.AsynchActionTrigger(startGame_Asynch);
        }

        void startGame_Asynch()
        {
            var enemies = hqRef.players.CollectEnemyUnits(this);

            while (enemies.Next())
            {
                List<Unit> alertedUnits = collectAlertedUnits(enemies.sel, true);
                foreach (var m in alertedUnits)
                {
                    m.Alert();
                }
            }
        }

        public override void update()
        {
            //Find the most valuable move, move n attack, repeat until all moved
            //TODO special moves, attack n move order
            if (que.Update() || otherPlayerLockQue.Update())
            {
                return;
            }

            switch (state)
            {
                case AiState.Start:
                    if (stateTime.CountDown())
                    {
                        activationOrder.loopBegin();
                        setState(AiState.BeginFindNextActivationUnit);
                    }
                    break;

                case AiState.BeginFindNextActivationUnit:
                    findNextActivationUnit();
                    break;

                case AiState.NextUnitAction:
                    if (checkAlive())
                    {
                        setState(AiState.AsynchCalculation);
                        if (DebugAsynchActions)
                        {
                            nextUnitAction_Asynch();
                        }
                        else
                        {
                            new Timer.AsynchActionTrigger(nextUnitAction_Asynch);
                        }
                    }
                    break;

                case AiState.Move:
                    if (unitActions.unit != null)
                    {
                        Debug.Log("Move unit " + unitActions.unit.ToString());

                        SpectatorTargetPos = unitActions.unit.squarePos;

                        toggRef.hud.addInfoCardDisplay(new ToggEngine.Display2D.UnitCardDisplay(unitActions.unit,
                            ToggEngine.Display2D.UnitDisplaySettings.AiAction, hqRef.playerHud));
                        toggRef.hud.unitsGui.refresh(unitActions.unit, Display3D.UnitStatusGuiSettings.MouseHover);

                        if (hqRef.netManager.host)
                        {
                            movelines = new MoveLinesGroup(unitActions.unit, unitActions.walkingPath);
                        }

                        spectateMove(movelines);

                        if (movelines == null && !hqRef.netManager.host)
                        {
                            setState(AiState.None);
                        }
                        else
                        {
                            new ToggEngine.QueAction.AiMoveAction(this, movelines, unitActions.unit);

                            setState(AiState.MoveAnimation);

                            if (movelines != null && movelines.MoveLength > 0)
                            {
                                toggRef.board.soundFromSquare(movelines.start);
                                toggRef.board.soundFromSquare(movelines.End);
                            }

                            if (hqRef.netManager.host)
                            {
                                var w = netWriteAiState(AiState.Move);
                                unitActions.unit.writeIndex(w);
                                movelines.Write(w);
                            }
                        }
                    }
                    break;

                case AiState.EndMovement:
                    if (stateTime.CountDown())
                    {
                        unitActions.unit.deleteMoveLines();

                        if (IsHost)
                        {
                            setState(AiState.NextUnitAction);
                        }
                        else
                        {
                            setState(AiState.None);
                        }
                    }
                    break;

                case AiState.Attack:
                    updateAttack();
                    break;

                case AiState.AttackAnimation:
                    if (IsHost)
                    {
                        if (checkAlive())
                        {
                            if (attackDisplay.updateAttack())
                            {
                                removeAttackDisplay();
                                setState(AiState.NextUnitAction);
                            }
                        }
                    }
                    else
                    {
                        if (attackDisplay != null && attackDisplay.attackRoll != null)
                        {
                            attackDisplay.attackRoll.clientUpdate();
                        }
                    }
                    
                    break;

                case AiState.AttackObjective:
                    stateTime.Seconds = 1f;
                    setState(AiState.AttackObjectiveEvent);

                    var target = unitActions.targetGroup[0];
                    effectsLib.SwingAndSlash(unitActions.unit, target.position, target.attackType);
                    break;

                case AiState.AttackObjectiveEvent:
                    hqRef.setup.conditions.OnObjective(unitActions.unit,
                        unitActions.targetGroup,
                        AiObjectiveType.AttackObject, IsHost);

                    //stateTime.Seconds = 2f;

                    setState(AiState.NextUnitAction);
                    break;

                case AiState.Objective:
                    hqRef.setup.conditions.OnObjective(unitActions.unit, null, 
                        unitActions.objectiveType, true);

                    stateTime.Seconds = 0.6f;
                    setState(AiState.NextUnitAction);
                    break;

                case AiState.UnitActivationComplete:
                    if (stateTime.CountDown())
                    {
                        if (unitActions.unit != null)
                        {
                            unitActions.unit.aiHasBeenActivated = true;
                        }
                        setState(AiState.BeginFindNextActivationUnit);
                    }
                    else
                    {
                        if (unitActions.unit != null)
                        {
                            SpectatorTargetPos = unitActions.unit.squarePos;
                        }
                    }
                    break;

                case AiState.BeginEndTurn:
                    setState(AiState.EndTurn);
                    stateTime.Seconds = 0.6f;
                    break;

                case AiState.EndTurn:
                    if (IsHost)
                    {
                        if (stateTime.CountDown())
                        {
                            new MonsterRespawn(false);
                            new QueAction.QueActionEndTurn(true);
                            setState(AiState.None);
                        }
                    }
                    break;
            }
        }

        void updateAttack()
        {
            if (checkAlive())
            {
                if (stateTime.CountDown())
                {
                    if (unitActions.targetGroup == null)
                    {
                        setState(AiState.NextUnitAction);
                    }
                    else
                    {
                        var targetType = unitActions.targetGroup.attackTargetType();

                        switch (targetType)
                        {
                            case AttackTargetType.Unit:
                                setState(AiState.AttackAnimation);

                                toggRef.hud.addInfoCardDisplay(new ToggEngine.Display2D.UnitCardDisplay(unitActions.unit,
                               ToggEngine.Display2D.UnitDisplaySettings.AiAction, hqRef.playerHud));

                                var w = netWriteAiState(state);
                                attackDisplay = new AttackDisplay(unitActions.unit, unitActions.targetGroup, this, w);
                                break;

                            case AttackTargetType.Objective:
                                setState(AiState.AttackObjective);
                                netWriteAttackObjective();
                                break;
                        }
                    }
                }
            }
        }

        bool checkAlive()
        {
            if (unitActions != null &&
                unitActions.unit != null &&
                unitActions.unit.Alive)
            {
                return true;
            }
            else
            {
                Debug.Log("CHECK ALIVE FALSE: " + state.ToString());
                setState(AiState.UnitActivationComplete);
                return false;
            }
        }

        public void holdCamera()
        {
            stateTime.Seconds += 1f;
        }

        void spectateMove(MoveLinesGroup moveLines)
        {
            if (moveLines != null)
            {
                if (moveLines.MoveLength < 4)
                {
                    SpectatorTargetPos = moveLines.End;
                }
                else
                {
                    SpectatorTargetPos = moveLines.lines[
                        MathExt.MultiplyInt(0.7, moveLines.lines.Count)].toPos;
                }
            }
        }

        void removeAttackDisplay()
        {
            attackDisplay?.DeleteMe();
            attackDisplay = null;
        }

        System.IO.BinaryWriter netWriteAiState(AiState state)
        {
            if (hqRef.netManager.host)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqAiAction, Network.PacketReliability.Reliable);
                w.Write((byte)state);
                return w;
            }

            return null;
        }

        public override void onActionComplete(ToggEngine.QueAction.AbsQueAction action, bool emptyQue)
        {
            base.onActionComplete(action, emptyQue);

            if (action.Type == ToggEngine.QueAction.QueActionType.MoveAction)
            {
                toggRef.board.onUnitMovement(unitActions.unit, movelines);
                stateTime.Seconds = 0.8f;

                setState(AiState.EndMovement);
            }

            if (action is AbsPerformUnitAction)
            {
                if (unitActions.unit != null)
                {
                    unitActions.unit.hasMadeSpecialAction = true;
                }
            }
        }

    
        public void netRead(Network.ReceivedPacket packet)
        {
            clearClientVisuals();

            setState((AiState)packet.r.ReadByte());

            switch (state)
            {
                case AiState.Move:
                    unitActions = new UnitAiActions((Unit)hqRef.gamestate.GetUnit(packet.r));
                    movelines = new MoveLinesGroup(unitActions.unit, packet.r);
                    movelines.isLocal = true;
                    movelines.setFocus(0);

                    if (unitActions.unit != null)
                    {
                        hqRef.netManager.historyAdd("Ai: Moved " + unitActions.unit.ToString() +
                            unitActions.unit.squarePos.ToString() + "-" + movelines.End.ToString());
                    } 
                    break;

                case AiState.AttackAnimation:
                    attackDisplay = new AttackDisplay(this, packet.r);
                    break;

                case AiState.AttackObjective:
                    unitActions = new UnitAiActions((Unit)hqRef.gamestate.GetUnit(packet.r));
                    unitActions.targetGroup = new AttackTargetGroup(packet.r);
                    break;
            }
        }

        void netWriteAttackObjective()
        {
            var w = netWriteAiState(AiState.AttackObjective);
            if (w != null)
            {
                unitActions.unit.netWriteUnitId(w);
                unitActions.targetGroup.write(w);
            }
        }



        void clearClientVisuals()
        {
            if (movelines != null)
            {
                movelines.DeleteMe();
                movelines = null;
            }

            removeAttackDisplay();
        }

        void setState(AiState newState)
        {
            state = newState;

            if (!IsHost)
            {
                if (state != AiState.Attack && state != AiState.AttackAnimation)
                {
                    removeAttackDisplay();
                }
            }
        }

        void findNextActivationUnit()
        {
            var opponents = hqRef.players.CollectEnemyUnits(this);
            bool hasAliveOpponents = false;

            while (opponents.Next())
            {
                if (opponents.sel.Alive && opponents.sel.hasHealth())
                { 
                    hasAliveOpponents = true;
                    break;
                }
            }

            
            if (hasAliveOpponents)
            {
                if (arraylib.HasDuplicatePointer(activationOrder))
                {
                    throw new Exception();
                }

                while (activationOrder.loopNext())
                {
                    if (activationOrder.sel.Alive)
                    {
                        if (activationOrder.sel.condition.GetBase(
                            Data.Condition.BaseCondition.CantActivate) == null)
                        {
                            unitActions = new UnitAiActions(activationOrder.sel);

                            Debug.Log("Next ai unit: " + unitActions.unit.ToString());

                            setState(AiState.NextUnitAction);
                            return;
                        }
                    }
                    else
                    {
                        activationOrder.loopRemove();
                    }
                }
            }
            setState(AiState.BeginEndTurn);
        }

        void nextUnitAction_Asynch()
        {
            AiState aiState = unitActions.NextAction_Asynch();
            setState(aiState);
        }

        public static void ClosestWalkToOpponent(UnitAiActions unitActions, out WalkingPath walkingPath, out AbsUnit towardsUnit)
        {
            towardsUnit = null;
            int distance = int.MaxValue;
            walkingPath = null;

            var opponents = hqRef.players.CollectEnemyUnits(unitActions.unit.Player);
                    
            
            while (opponents.Next())
            {
                if (unitActions.unit.canTargetUnit(opponents.sel))
                {
                    var path = unitActions.unit.FindPath(opponents.sel.squarePos, true);

                    if (path.Length < distance ||
                        (path.Length == distance && opponents.sel.lastMoveTime > towardsUnit.lastMoveTime))
                    {
                        towardsUnit = opponents.sel;
                        distance = path.squaresEndToStart.Count;
                        walkingPath = path;
                    }
                }
            }
        }

        int distanceToClosestHero(AbsUnit fromUnit)
        {
            FindMinValuePointer<AbsUnit> closest = new FindMinValuePointer<AbsUnit>();
            var opponents = hqRef.players.CollectEnemyUnits(this);

            while (opponents.Next())
            {
                closest.Next((opponents.sel.squarePos - fromUnit.squarePos).SideLength(), opponents.sel);
            }

            if (closest.minMember == null)
            {
                return int.MaxValue;
            }
            else
            {
                return (int)closest.minValue;
            }
        }

        public void checkAlertFromMovingOpponent_Asynch(AbsUnit enemyUnit)
        {
            List<Unit> alertedUnits = collectAlertedUnits(enemyUnit, false);

            netWriteAlertedGroup(alertedUnits);

            AlertGroup(alertedUnits);
        }

        public void checkAlertFromDoor_Asynch(Door door)
        {
            List<Unit> alertedUnits = collectAlertedUnits(door);

            netWriteAlertedGroup(alertedUnits);

            AlertGroup(alertedUnits);
        }

        void netWriteAlertedGroup(List<Unit> alertedUnits)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqAiAlerted, Network.PacketReliability.Reliable);
            w.Write((byte)alertedUnits.Count);
            foreach (var m in alertedUnits)
            {
                m.netWriteUnitId(w);
            }
        }

        public void alertedOnAttack(Unit damagedUnit)
        {
            if (!damagedUnit.aiAlerted && damagedUnit.Alive)
            {
                List<Unit> alertedUnits = new List<Unit> { damagedUnit };
                alertedUnitCallOthers(damagedUnit, alertedUnits);

                netWriteAlertedGroup(alertedUnits);

                AlertGroup(alertedUnits);
            }
        }

        List<Unit> collectAlertedUnits(AbsUnit enemyUnit, bool gameStart)
        {       
            List<Unit> alertedUnits = new List<Unit>();

            collectAlertedUnits(enemyUnit.squarePos, gameStart, alertedUnits);

            return alertedUnits;
            //var counter = hqUnits.unitsCounter.Clone();
            //while (counter.Next())
            //{
            //    var m = counter.sel;

            //    int distance;
            //    if (gameStart)
            //    {
            //        distance = AbsUnit.GameStartAlertDistance;
            //    }
            //    else
            //    {
            //        distance = m.alertDistance();
            //    }

            //    if (!m.aiAlerted &&
            //        m.canBeAlerted() &&
            //        m.InRangeAndSight(enemyUnit.squarePos, distance, true, false))
            //    {
            //        Unit alerted = (Unit)m;
            //        alertedUnits.Add(alerted);

            //        alertedUnitCallOthers(alerted, alertedUnits);
            //    }
            //}

            //return alertedUnits;
        }

        List<Unit> collectAlertedUnits(Door door)
        {
            List<Unit> alertedUnits = new List<Unit>();

            if (door.leftToRightModel)
            {
                addPos(VectorExt.AddX(door.position, -1));
                addPos(VectorExt.AddX(door.position, 1));
            }
            else
            {
                addPos(VectorExt.AddY(door.position, -1));
                addPos(VectorExt.AddY(door.position, 1));
            }

            return alertedUnits;

            void addPos(IntVector2 pos)
            {
                if (toggRef.board.tileGrid.InBounds(pos))
                {
                    collectAlertedUnits(pos, false, alertedUnits);
                }
            }
        }

        void collectAlertedUnits(IntVector2 alertPos, bool gameStart, List<Unit> alertedUnits)
        {
            //List<Unit> alertedUnits = new List<Unit>();

            var counter = hqUnits.unitsCounter.Clone();
            while (counter.Next())
            {
                var m = counter.sel;

                int distance;
                if (gameStart)
                {
                    distance = AbsUnit.GameStartAlertDistance;
                }
                else
                {
                    distance = m.alertDistance();
                }

                if (!m.aiAlerted &&
                    m.canBeAlerted() &&
                    m.InRangeAndSight(alertPos, distance, true, false))
                {
                    Unit alerted = (Unit)m;
                    alertedUnits.Add(alerted);

                    alertedUnitCallOthers(alerted, alertedUnits);
                }
            }

            //return alertedUnits;
        }

        void alertedUnitCallOthers(Unit alerted, List<Unit> alertedUnits)
        {
            var counter2 = hqUnits.unitsCounter.Clone();
            while (counter2.Next())
            {
                var friendly = counter2.sel;
                if (!friendly.aiAlerted &&
                    friendly.canBeAlerted() &&
                    friendly.sideDistanceTo(alerted) <= friendly.alertDistance() &&
                    toggRef.board.InLineOfSight_Simplified(alerted.squarePos, friendly.squarePos, false))
                {
                    alertedUnits.Add((Unit)friendly);
                }
            }
        }

        public void netReadAlert(System.IO.BinaryReader r)
        {
            int count = r.ReadByte();
            List<Unit> alertedUnits = new List<Unit>(count);

            for (int i = 0; i < count; ++i)
            {                
                var u = Unit.NetReadUnitId(r);
                if (u != null)
                {
                    alertedUnits.Add(u);
                }
            }

            AlertGroup(alertedUnits);
        }

        public void netWriteSpawn(List<Unit> units)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqMonsterSpawn, 
                Network.PacketReliability.Reliable);
            w.Write((byte)units.Count);
            foreach (var m in units)
            {
                m.writePlayerCollection(w);
            }
        }

        public void netReadSpawn(System.IO.BinaryReader r)
        {
            int count = r.ReadByte();

            for (int i = 0; i < count; ++i)
            {
                new Unit(r, DataStream.FileVersion.Max, hqUnits);
            }
        }

        public static void AlertGroup(List<Unit> alertedUnits)
        {
            float timeDelay = 40;
            foreach (var m in alertedUnits)
            {
                if (!m.aiAlerted)
                {
                    m.Alert();
                    if (m.canBeAlerted())
                    {
                        new Timer.TimedAction0ArgTrigger(m.AlertEffect, timeDelay);
                        timeDelay += 100;
                    }
                }
            }
        }

        public override void AlertedUnit(Unit unit)
        {
            if (unit.canBeAlerted())
            {
                if (activationOrder.Contains(unit))
                {
                    throw new Exception();
                }

                activationOrder.Add(unit);
            }
        }

        public override void onNewUnit(AbsUnit unit)
        {
            base.onNewUnit(unit);
            if (!unit.aiAlerted && unit.canBeAlerted())
            {
                unit.hq().condition.Set(Data.Condition.ConditionType.Idle, 1,
                    false, false, false);
            }
        }

        public override void OnEvent(EventType eventType, object tag)
        {
            switch (eventType)
            {
                case EventType.DoorOpened:
                    new Timer.Asynch1ArgTrigger<Door>(
                        hqRef.players.dungeonMaster.checkAlertFromDoor_Asynch, tag as Door);
                        break;
            }

            base.OnEvent(eventType, tag);


        }

        bool IsHost { get { return hqRef.netManager.host; } }

        override public int AiActiveUnitsCount => activationOrder.Count;

        public override bool IsScenarioOpponent
        {
            get
            {
                return true;
            }
        }
        override public bool IsHero { get { return false; } }

        public override bool LocalHumanPlayer => false;
        public override bool IsDungeonMaster => true;

        public override bool IsLocal => hqRef.netManager.host;
        public override Gadgets.Backpack Backpack()
        {
            throw new NotImplementedException();
        }
        public override AbsHeroInstance HeroInstance => throw new NotImplementedException();

        public override Unit HeroUnit => throw new NotImplementedException();
    }

    enum AiState
    {
        Start,

        BeginFindNextActivationUnit,
        NextUnitAction,
        Move,
        MoveAnimation,
        EndMovement,
        Attack,
        AttackAnimation,
        AttackObjective,
        AttackObjectiveEvent,
        Objective,
        UnitActivationComplete,

        BeginEndTurn,
        EndTurn,

        AsynchCalculation,
        None,

    }
}
