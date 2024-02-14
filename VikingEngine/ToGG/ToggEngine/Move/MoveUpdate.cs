using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Move
{
    interface IMoveUpdateListerner
    {
        void onMoveUpdateEnd(AbsUnit unit, bool canceled);
        void onMoveUpdateNewSquare(AbsUnit unit);
    }

    class MoveUpdate
    {
        static readonly Color DisabledMoveModelGray = ColorExt.GrayScale(0.4f);

        UnitDragNDrop unitDragNDrop;
        //public AbsUnit movingUnit = null;
        AbsGenericPlayer player;
        IMoveUpdateListerner listerner;
        bool firstUpdate = true;

        public MoveUpdate(AbsGenericPlayer player, IMoveUpdateListerner listerner)
        {
            this.listerner = listerner;
            this.player = player;
            unitDragNDrop = new UnitDragNDrop(player);
        }

        public bool updateHover()
        {
            if (unitDragNDrop.updateUnitSelection())
            { //On click
                player.movingUnit = player.mapControls.SelectedUnit;
                new AvailableMovement(player, player.movingUnit, false, false);
                if (player.movingUnit.movelines == null)
                {
                    new MoveLinesGroup(player.movingUnit);
                }
                else
                {
                    player.movingUnit.movelines.setFocus(0);
                }
                return true;
            }

            return false;
        }

        public void updateMove(ref MapSquareAvailableType availableMapSquare)
        {
            bool dropInput, undoMove, availableSquare;
            unitDragNDrop.updateUnitMove(out dropInput, out undoMove, out availableSquare);

            if (availableSquare)
            {
                availableMapSquare = MapSquareAvailableType.Enabled;
                player.movingUnit.soldierModel.model.Color = Color.White;
            }
            else
            {
                player.movingUnit.soldierModel.model.Color = DisabledMoveModelGray;
            }

            player.mapControls.viewErrorCross(unitDragNDrop.errorMoveWarning);

            if (dropInput)
            {//complete movement
                if (player.movingUnit.movelines.HasNewMoves())
                {
                    moveUnit();
                }
                else
                {
                    undoMove = true;
                }
            }

            if (undoMove)
            {
                player.movingUnit.undoMovement(false);
                if (player.movingUnit.movelines != null)
                {
                    player.movingUnit.movelines.setFocus(2);
                }

                listerner.onMoveUpdateEnd(player.movingUnit, true);
            }

            if (dropInput || undoMove)
            {
                player.movingUnit.soldierModel.model.Color = Color.White;
                player.movingUnit = null;                
            }

            if (player.movingUnit != null)
            {
                if (player.mapControls.isOnNewTile || firstUpdate)
                {
                    firstUpdate = false;
                    
                    listerner.onMoveUpdateNewSquare(player.movingUnit);
                    toggRef.board.soundFromSquare(player.mapControls.selectionIntV2);
                    //updateMoveToolTip();
                    //hud.unitsGui.refresh(player.mapControls.selectionIntV2, movingUnit);

                    //var actions = new QueAction.SquareActionList(movingUnit.movelines,
                    //    (Unit)movingUnit, true);
                    //actions.createActionIcons(movingUnit.movelines);
                }
            }
        }

        void moveUnit()
        {
            if (player.movingUnit.movelines.HasNewMoves())
            {
                //onStrategyAction();
                player.movingUnit.movelines.moveActionIcons.DeleteAll();
                player.movingUnit.movelines.checkOnMoveCompleted();

                toggRef.board.onUnitMovement(player.movingUnit, player.movingUnit.movelines);

                if (toggRef.mode == GameMode.HeroQuest)
                {
                    int staminaUse = player.movingUnit.movelines.staminaCost();

                    HeroQuest.HeroData hero = player.movingUnit.hq().data.hero;
                    if (hero != null)
                    {
                        hero.stamina.spend(staminaUse);
                    }
                }
                //request om att flytta här
                new QueAction.MoveAction(player, player.movingUnit.movelines, player.movingUnit, listerner);
                
            }
            else
            {
                player.movingUnit.SetVisualPosition(player.movingUnit.squarePos);
                listerner.onMoveUpdateEnd(player.movingUnit, true);
            }
        }
    }
}
