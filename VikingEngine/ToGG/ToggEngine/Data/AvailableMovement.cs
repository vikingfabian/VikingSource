using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.CommandCard;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    /// <summary>
    /// Go through all tiles and see if they are passable
    /// </summary>
    class AvailableMovement : AbsQuedTasks//QueAndSynch
    {
        public List<IntVector2> available;
        bool[,] checkedSquare;
        AbsGenericPlayer player;
        AbsUnit unit;
        int moveLength;
        bool hoverDots;

        bool useOrderStartPos;

        public AvailableMovement(AbsGenericPlayer player, AbsUnit unit, bool hoverDots, bool isAsynch)
            : base( QuedTasksType.QueAndSynch)
        {
            this.hoverDots = hoverDots;
            checkedSquare = new bool[toggRef.board.Size.X, toggRef.board.Size.Y];
            this.player = player;
            this.unit = unit;

            useOrderStartPos = !hoverDots && unit.order != null;
            
            moveLength = unit.MoveLengthWithModifiers(useOrderStartPos);
            
            if (isAsynch)
            {
                runQuedAsynchTask();//quedEvent();
                addToSyncedUpdate();
                //AddToUpdateList();
            }
            else
            {
                beginAutoTasksRun();//start();
            }
        }

        //protected override bool quedEvent()
        //{
        protected override void runQuedAsynchTask()
        {
            IntVector2 start = unit.squarePos;

            if (useOrderStartPos)//!hoverDots && unit.order != null)
            {
                start = unit.orderStartPos;
            }

            available = new List<IntVector2>
            {
                start
            };

            availableMovement(0, 1, moveLength);
            clearOutMoveThroughs();

        //    return true;
        }

        void availableMovement(int checkSquaresStart, int checkSquaresLength, int stepsLeft)
        {
            for (int i = checkSquaresStart; i < checkSquaresStart + checkSquaresLength; ++i)
            {
                IntVector2 pos = available[i];
                checkedSquare[pos.X, pos.Y] = true;

                if (stepsLeft > 0 && (i == 0 || toggRef.board.MovementRestriction(pos, unit) != ToggEngine.Map.MovementRestrictionType.MustStop))
                {
                    for (Dir8 dir = 0; dir < Dir8.NUM; ++dir)
                    {
                        IntVector2 toSquare = pos + IntVector2.Dir8Array[(int)dir];
                        if (toggRef.board.tileGrid.InBounds(toSquare) &&
                            !checkedSquare[toSquare.X, toSquare.Y] &&
                            toggRef.board.MovementRestriction(toSquare, unit) != ToggEngine.Map.MovementRestrictionType.Impassable)
                        {
                            if (!available.Contains(toSquare)) 
                                available.Add(toSquare);
                        }
                    }
                }
            }

            if (available.Count > checkSquaresStart + checkSquaresLength)
            {
                checkSquaresStart = checkSquaresStart + checkSquaresLength;
                checkSquaresLength = available.Count - checkSquaresStart;
                availableMovement(checkSquaresStart, checkSquaresLength, stepsLeft - 1);
            }
        }

        void clearOutMoveThroughs()
        {
            for (int i = available.Count - 1; i >= 1; --i)
            {
                if (toggRef.board.CanEndMovementOn(available[i], unit) == false)
                {
                    available.RemoveAt(i);
                }
            }
        }

        public override void runSyncAction()
        {
             player?.mapControls?.SetAvailableTiles(available);
        }
    }
}
