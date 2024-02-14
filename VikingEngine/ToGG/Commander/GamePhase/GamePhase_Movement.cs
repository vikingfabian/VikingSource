using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VikingEngine.ToGG.Commander.Battle;
using VikingEngine.ToGG.Commander.CommandCard;
using VikingEngine.ToGG.Commander.Display;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.ToggEngine.Move;

namespace VikingEngine.ToGG
{
    class GamePhase_Movement : Commander.GamePhase.AbsAttackPhase, ToggEngine.Move.IMoveUpdateListerner
    {
        MoveUnitAction moveUnitAction;
        //bool blockPickUp = false;
        MoveUpdate moveUpdate;
        MapSquareAvailableType availableMapSquare = MapSquareAvailableType.None;
        MapSquareAvailableType prevAvailableMapSquare = MapSquareAvailableType.Enabled;
        ToggEngine.Display2D.MoveLengthToolTip moveLengthTooltip;
        //bool hasMoved = false;

        public GamePhase_Movement(Commander.Players.AbsLocalPlayer absplayer)
            : base(absplayer)
        {
            if (absplayer.LocalHumanPlayer)
            {
                moveUpdate = new MoveUpdate(player, this);

                absplayer.commandCard.orders.EnableAll(true);
                if (toggLib.MouseInputMode)
                {
                    absplayer.commandCard.orders.Unselect();
                }
                else
                {
                    absplayer.commandCard.orders.Next_IsEnd(true);
                }
                markAvailable();
            }
            else
            {//Ai
                aiPlayer.commandCard.orders.EnableAll(true);

                aiIsCalculatingProtection = true;
                new Timer.AsynchActionTrigger(calcProtection);
            }

            PhaseMark();
        }

        public void PhaseMark()
        {
            foreach (OrderedUnit ord in CommandCard.orders.list)
            {
                ord.PhaseMark(SpriteName.cmdUnitMoveGui);
            }
        }

        

        void markAvailable()
        {
            List<IntVector2> tiles = new List<IntVector2>(8);

            var units = player.unitsColl.units.counter();
            while (units.Next())
            {
                if (movableUnit(units.sel))
                {
                    tiles.Add(units.sel.squarePos);
                }
            }

            mapControls.SetAvailableTiles(tiles);
            PhaseMarkVisible(true);
        }

        //ToggEngine.Move.IMoveUpdateListerner event
        public void onMoveUpdateEnd(AbsUnit unit, bool canceled)
        {
            int backStab = 0;
            //markAvailable();
            if (!canceled)
            {
                //hasMoved = true;
                backStab = unit.movelines.backStabbersFullCount();
            }

            if (backStab > 0)
            {   
                attackDisplay = new AttackDisplay(player, unit);
                attackDisplay.beginAttack();
                mapControls.removeAvailableTiles();                
            }
            else
            {
                clearMoveDisplay(unit);
            }

            player.commandCard.Get(unit).CheckList_Enabled = false;
        }

        protected override void OnAttackComplete()
        {
            base.OnAttackComplete();
            
            clearMoveDisplay(attackDisplay.attackRoll.defender);

            attackDisplay?.DeleteMe();
            attackDisplay = null;
        }

        void clearMoveDisplay(AbsUnit unit)
        {
            if (unit.movelines != null)
            {
                unit.movelines.clearIcons();
                unit.movelines.setFocus(2);
            }
           
            markAvailable();
        }

        public void onMoveUpdateNewSquare(AbsUnit unit)
        {
        }

        void calcProtection()
        {
            //TODO

            aiIsCalculatingProtection = false;
        }

        bool aiIsCalculatingProtection = false;
        bool aiIsCalculatingNextMove = false;
        bool aiIsCalculatingNextMove_Complete = false;
        Time viewMoveLinesTime;
        public override bool UpdateAi()
        {
            if (aiIsCalculatingProtection)
            {
                return false;
            }
            if (moveUnitAction != null)
            {
                bool complete = moveUnitAction.Update();
                if (complete)
                {
                    moveUnitAction = null;
                    isNewState = true;

                    SelectedOrder.CheckList_Enabled = false;
                    CommandCard.orders.Unselect();
                    CommandCard.removeDeadUnits();
                }
                return false;
            }
            if (viewMoveLinesTime.HasTime)
            {
                if (viewMoveLinesTime.CountDown())
                {
                    if (aiunit.order != null)
                    {
                        aiunit.deleteMoveLines();
                    }
                }
                return false;
            }

            if (!aiIsCalculatingNextMove)
            {
                if (aiIsCalculatingNextMove_Complete)
                {
                    aiIsCalculatingNextMove_Complete = false;

                    CommandCard.selectUnit(aiunit);
                    if (walkingPath == null)
                    {
                        //OnMovementActionCompleted();
                    }
                    else
                    {   
                        viewMoveLinesTime = new Time(900, TimeUnit.Milliseconds);
                        aiunit.movelines = new MoveLinesGroup(aiunit, walkingPath);
                        moveUnitAction = new MoveUnitAction(this, aiunit, aiunit.movelines, Engine.Screen.SafeArea, absPlayer);
                        aiPlayer.SpectatorTargetPos = aiunit.movelines.End;
                    }
                }
                else if (aiPlayer.commandCard.hasEnabledUnits())
                {
                    //Starts here
                    aiIsCalculatingNextMove = true;
                    new Timer.AsynchActionTrigger(asynchCalcNextMove);
                }
                else
                {
                    return true;
                }
            }
            return false;
        }



        WalkingPath walkingPath;
        AbsUnit aiunit;
        void asynchCalcNextMove()
        {
            //Move the one with most value first
            FindMaxValuePointer<AbsUnit> moveFirst = new FindMaxValuePointer<AbsUnit>(false);

            foreach (var u in aiPlayer.commandCard.orders.list)
            {
                if (u.CheckList_Enabled)
                {
                    u.unit.calcOrderValue();
                    moveFirst.Next(u.unit.aiOrderValue, u.unit);
                }
            }

            aiunit = moveFirst.maxMember;

            if (aiunit.aiSuggestedMove != aiunit.squarePos)
            {
                //Path finding to the point
                walkingPath = aiunit.FindPath(aiunit.aiSuggestedMove, false);
            }
            else //Stand still
            {
                aiunit.order.CheckList_Enabled = false;
                walkingPath = null;
            }

            aiIsCalculatingNextMove_Complete = true;
            aiIsCalculatingNextMove = false;
        }




        public override bool OverridingUpdate(ref PhaseUpdateArgs args)
        {
            if (updateAttack(ref args))
            {
                //updateAttackAnimation();
                return true;
            }
            if (moveUnitAction != null)
            {
                moveUnitAction.Update();
                return true;
            }
            return false;
        }

        public override void returnToThisPhaseBeginning()
        {
            foreach (var m in CommandCard.orders.list)
            {
                m.unit.undoMovement(true);
                m.CheckList_Enabled = true;
            }
        }

        OrderedUnit SelectedOrder
        {
            get
            {
                if (CommandCard != null)
                {
                    return CommandCard.orders.Selected();
                }
                return null;
            }
        }


        MoveLinesGroup MoveLines
        {
            get
            {
                if (player.movingUnit != null)
                {
                    return player.movingUnit.movelines;
                }
                return null;
            }
            set
            {
                player.movingUnit.movelines = value;
            }
        }

        bool movableUnit(AbsUnit unit)
        {
            bool movable = CommandCard.containsOrderedUnit(unit) && !unit.MovedThisTurn;

            return movable;
        }


        public override void Update(ref PhaseUpdateArgs args)
        {
            availableMapSquare = MapSquareAvailableType.None;
            bool newTooltip = false;

            if (!args.mouseOverHud)
            {
                if (player.movingUnit == null)
                {
                    mapControls.updateMapMovement(true);


                    if (mapControls.SelectedUnit != null)
                    {
                        if (movableUnit(mapControls.SelectedUnit))
                        {
                            availableMapSquare = MapSquareAvailableType.Enabled;
                            if (moveUpdate.updateHover())
                            {
                                onSelect();
                            }
                        }
                        else
                        {
                            availableMapSquare = MapSquareAvailableType.Disabled;
                        }

                        if (mapControls.isOnNewTile && CommandCard.containsOrderedUnit(mapControls.SelectedUnit))
                        {
                            new BeginMoveTooltip(availableMapSquare == MapSquareAvailableType.Enabled, player);
                            newTooltip = true;
                        }
                    }
                    
                }
                else
                {
                    mapControls.updateMapMovement(true);
                    MapSquareAvailableType availableMapSquare = MapSquareAvailableType.None;
                    moveUpdate.updateMove(ref availableMapSquare);

                    if (player.movingUnit == null)
                    {
                        onDrop();
                        args.refreshPhaseStatus = true;
                    }
                    else
                    {
                        if (mapControls.isOnNewTile)
                        {
                            moveLengthTooltip.refresh(player.movingUnit);
                        }
                    }
                }
            }

            if (availableMapSquare != prevAvailableMapSquare)
            {
                prevAvailableMapSquare = availableMapSquare;
                mapControls.setAvailable(availableMapSquare);
            }

            if (mapControls.isOnNewTile && !newTooltip && player.movingUnit == null)
            {
                player.mapControls.removeToolTip();
            }
        }

        //void unselectUnit()
        //{
        //    CommandCard.orders.Unselect();
        //    player.markAvailableCommandUnits();

        //    player.onNewTile();
        //}

        

        //void undoMove(OrderedUnit order) 
        //{
        //    if (order != null) order.unit.undoMovement();
        //}

        //void beginUpdateAvailableMovement()
        //{
        //    mapControls.removeAvailableTiles();
        //    OrderedUnit ord = CommandCard.orders.Selected();
        //    if (ord != null)
        //    {
        //        new AvailableMovement(player, ord.unit, false, false);
        //    }
        //}
        //public void OnMovementActionCompleted()
        //{
        //    moveUnitAction = null;
        //    isNewState = true;

        //    SelectedOrder.CheckList_Enabled = false;
        //    if (player != null)
        //    {
        //        player.selectUnit(null);
        //    }
          
        //    CommandCard.orders.Unselect();
        //    CommandCard.removeDeadUnits();
        //    PhaseMarkVisible(true);

        //    if (player != null)
        //    {
        //        player.markAvailableCommandUnits();
        //    }
        //}

        void onSelect()
        {
            moveLengthTooltip = new MoveLengthToolTip(mapControls);
            moveLengthTooltip.refresh(player.movingUnit);

            PhaseMarkVisible(false);
        }

        void onDrop()
        {
            player.mapControls.removeToolTip();
            moveLengthTooltip = null;      
            
        }

        public override void OnSelectUnit(bool select)
        {
            //mapControls.pointer.Visible = !select;
            if (select)
            {
                if (player.movingUnit.movelines == null)
                {
                    MoveLines = new MoveLinesGroup(player.movingUnit);
                }

                MoveLines.onPlayerPickUp(player.mapControls, player.movingUnit);
                
                
                mapControls.setSelectionPos(player.movingUnit.soldierModel.Position);
                mapControls.setAvailable(true);
            }
            else
            {

                if (MoveLines != null)
                {
                    MoveLines.setFocus(2);
                }
            }
        }

        public override bool ViewHoverUnitDisplay
        {
            get
            {
                return SelectedOrder == null;
            }
        }

        public override EnableType canExitPhase()
        {
            if (SelectedOrder != null)
            {
                return EnableType.Locked;
            }

            if (CommandCard.orders.HasEnabledMember())
            {
                return EnableType.Able_NotRecommended;
            }
            else
            {
                return EnableType.Enabled;
            }
        }
        public override void EndTurnNotRecommendedText(out string title, out string message,out string okText)
        {
            title = "Start attack phase?";
            message = "You can move more units";
            okText = "Start attacking";
        }

        protected override string name
        {
            get { return "Movement"; }
        }

        public override void OnExitPhase(bool forwardToNext)
        {
            base.OnExitPhase(forwardToNext);
            foreach (var m in CommandCard.orders.list)
            {
                if (m.unit.movelines != null)
                {
                    m.unit.movelines.setFocus(2);
                }
            }
        }

        public override bool canGoToPreviousPhase()
        {
            bool hasMoved = false;

            foreach (var m in CommandCard.orders.list)
            {
                if (m.unit.movelines != null && m.unit.movelines.HadBackstabbers())
                {
                    hasMoved = true;
                }
            }

            if (hasMoved)
            {
                return false;
            }
            return true;
        }

        public override GamePhaseType Type
        {
            get { return GamePhaseType.Move; }
        }
    }
}
