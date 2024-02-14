using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG
{
    class UnitDragNDrop
    {
        TimeStamp mouseDownTime;
        AbsGenericPlayer player;
        public bool errorMoveWarning;

        public UnitDragNDrop(AbsGenericPlayer player)
        {
            this.player = player;
        }

        public bool updateUnitSelection()
        {
            if (toggRef.inputmap.click.DownEvent)
            {//Select unit
                mouseDownTime = TimeStamp.Now();

                return true;
            }
            return false;
        }

        public void updateUnitMove(out bool dropInput, out bool undoMove, out bool availableSquare)
        {
            dropInput = false;
            undoMove = false;

            Vector2 toPos = player.mapControls.selectionV2;
            var movelines = player.movingUnit.movelines;

            toPos = toggRef.board.selectionBounds.KeepPointInsideBound_Position(toPos);

            player.movingUnit.SetVisualPosition(toggLib.ToV3(toPos));

            NewSquareResult newSquare = NewSquareResult.Other;

            if (player.mapControls.isOnNewTile)
            {
                //Moved it far enough from the previous pos
                newSquare = movelines.NewSquare(player.mapControls.selectionIntV2);
            }

            IntVector2 endSquare;
            availableSquare = refreshAvailable();

            if (player.mapControls.isOnNewTile)
            {
                if (!availableSquare)
                {
                    //Movelines does not follow mouse tracing anymore, try correct with path finding

                    if (newSquare == NewSquareResult.TooFarOffset)
                    {
                        //1. try trace the last 2-4 squares
                        PathFinding path = new PathFinding();
                        WalkingPath walkingPath = path.FindPath(player.movingUnit, endSquare, player.mapControls.selectionIntV2, false);
                        //todo refresh endsquare

                        if (walkingPath.success)
                        {
                            do
                            {
                                if (walkingPath.squaresEndToStart.Count > 0)
                                {
                                    IntVector2 move = arraylib.PullLastMember(walkingPath.squaresEndToStart);
                                    newSquare = movelines.NewSquare(move);
                                }
                                else
                                {
                                    newSquare = NewSquareResult.MustStop;
                                }
                            } while (newSquare == NewSquareResult.Added);
                        }

                        availableSquare = refreshAvailable();
                    }
                }

                if (availableSquare)
                {
                    errorMoveWarning = false;
                }
                else
                {
                    float l = VectorExt.SideLength(toggRef.board.toWorldPosXZ_Center(endSquare) -
                        player.mapControls.selectionV2);
                    errorMoveWarning = l > 0.9f;
                }
            }

            if (toggRef.inputmap.back.DownEvent)
            {
                undoMove = true;
            }

            if (toggRef.inputmap.click.DownEvent)
            {
                dropInput = true;
            }
            else if (toggRef.inputmap.click.UpEvent)
            {
                if (mouseDownTime.msPassed(200))
                {
                    dropInput = true;
                }
            }

            if (dropInput)
            {
                if (!movelines.HasMoved)
                {
                    undoMove = true;
                }
            }

            bool refreshAvailable()
            {
                endSquare = movelines.CurrentSquarePos();
                return endSquare == player.mapControls.selectionIntV2;
            }
        }

        
    }
}
