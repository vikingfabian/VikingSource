using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.HeroQuest.Players;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.QueAction
{
    class SquareActionList : List<AbsSquareAction>
    {
        public SquareActionList(MoveLinesGroup movelines, ToggEngine.GO.AbsUnit unit, bool local)
            :base()
        {
            foreach (var m in movelines.lines)
            {
                if (!m.partialLock)
                {
                    {
                        var sq = toggRef.board.tileGrid.Get(m.fromPos);

                        sq.tileObjects.collectSquareAction(m.fromPos, false, local, unit, this);
                    }
                    {
                        var sq = toggRef.board.tileGrid.Get(m.toPos);

                        sq.tileObjects.collectSquareAction(m.toPos, true, local, unit, this);
                    }
                }
            }
        }

        public void createActionIcons(MoveLinesGroup movelines)
        {
            movelines.moveActionIcons.DeleteAll();
            foreach (var m in this)
            {
                m.createIcon(movelines.moveActionIcons);
            }
        }
    }

    class MoveAction : AbsQueAction
    {
        SquareActionList squareActions;
        ToggEngine.GO.AbsUnit unit;
        IntervalTimer nextSquareDelay = new IntervalTimer(800, false, true);
        public IntVector2 SpectatorPos;
        MoveLinesGroup movelines;
        IntVector2 endPos;
        Move.IMoveUpdateListerner listerner;

        public MoveAction(AbsGenericPlayer player, MoveLinesGroup movelines, ToggEngine.GO.AbsUnit unit, 
            Move.IMoveUpdateListerner listerner)
            :base(player)
        {
            this.unit = unit;
            this.movelines = movelines;
            this.listerner = listerner;
        }

        public override void onBegin()
        {
            if (unit != null)
            {
                SpectatorPos = unit.squarePos;

                squareActions = new SquareActionList(movelines, unit, isLocalAction);

                endPos = movelines.CurrentSquarePos();
                unit.SetDataPosition(endPos);
            }
        }

        public void begin(out bool completed)
        {
            completed = update();
        }

        override public bool update()
        {
            if (unit.Dead)
            {
                return true;
            }

            bool moveComplete = false;
            
            if (nextSquareDelay.Active)
            {
                nextSquareDelay.CountDown();
            }
            else
            {
                if (squareActions.Count > 0)
                {
                    var action = arraylib.PullFirstMember(squareActions);

                    unit.SetVisualPosition(action.position, true);
                    action.onCommitMove(unit);

                    if (action.DelayTime > 0)
                    {   
                        //unit.squarePos = action.position;
                        SpectatorPos = action.position;

                        nextSquareDelay.Set(action.DelayTime);
                    }

                    if (toggRef.mode != GameMode.HeroQuest || 
                        unit.hq().condition.GetBase(HeroQuest.Data.Condition.BaseCondition.Immobile) != null)
                    {
                        unit.SetPosition(action.position);
                        //hqRef.netManager.writeMoveUnit(unit, false);
                        endMovement();
                    }
                }
                else
                {
                    unit.SetPosition(endPos);
                    endMovement();

                    listerner?.onMoveUpdateEnd(unit, false);
                    //player.onMoveComplete(this);
                }
            }

            return moveComplete;

            void endMovement()
            {
                unit.movelines.setPartialLock();
                if (toggRef.mode == GameMode.HeroQuest)
                {
                    unit.movelines.clearIcons();
                    unit.movelines.setFocus(2);
                }
                unit.OnEvent(ToGG.Data.EventType.ParentUnitMoved, true, movelines);
                if (toggRef.mode == GameMode.HeroQuest)
                {
                    HeroQuest.hqRef.events.SendToAll(ToGG.Data.EventType.OtherUnitMoved, unit);

                    HeroQuest.hqRef.netManager.writeMoveUnit(unit, false);
                }
                moveComplete = true;
            }
        }

        public override QueActionType Type => QueActionType.MoveAction;

        public override bool IsPlayerQue => true;

        public override bool NetShared => false;
    }

    class AiMoveAction : MoveAction
    {
        Time delay;

        public AiMoveAction(AbsGenericPlayer player, MoveLinesGroup movelines, AbsUnit unit)
            :base(player, movelines, unit, null)
        {
            int unitcount = player.AiActiveUnitsCount;

            if (unitcount <= 6)
            {
                delay.MilliSeconds = 800;
            }
            else if (unitcount <= 10)
            {
                delay.MilliSeconds = 500;
            }
            else
            {
                delay.MilliSeconds = 300;
            }

            isLocalAction = toggRef.NetHost;//hqRef.netManager.host;
        }

        public override bool update()
        {
            if (delay.CountDown())
            {
                return base.update();
            }
            else
            {
                return false;
            }
        }
    }

    abstract class AbsSquareAction
    {
        public float DelayTime = 800;
        public IntVector2 position;
        public bool enterSquare;
        protected bool localAction;

        public AbsSquareAction(IntVector2 pos, bool enterSquare, bool localAction)
        {
            this.position = pos;
            this.enterSquare = enterSquare;
            this.localAction = localAction;
        }

        abstract public void onCommitMove(ToggEngine.GO.AbsUnit unit);

        virtual public void createIcon(Graphics.ImageGroup icons)
        {
            //Add non
        }
    }

    class TileObjectActivation : AbsSquareAction
    {
        ToggEngine.GO.AbsTileObject tileobj;

        public TileObjectActivation(IntVector2 pos, bool enterSquare, bool localAction, 
            ToggEngine.GO.AbsTileObject tileobj)
            :base(pos, enterSquare, localAction)
        {
            this.tileobj = tileobj;
        }

        public override void onCommitMove(ToggEngine.GO.AbsUnit unit)
        {
            if (enterSquare)
            {
                
                tileobj.onMoveEnter(unit, localAction);
            }
            else
            {
                tileobj.onMoveLeave(unit, localAction);
            }
        }

        public override void createIcon(ImageGroup icons)
        {
            tileobj.createMoveStepIcon(icons);
        }
    }
}
