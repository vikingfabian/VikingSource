//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VikingEngine.Commander.Players;

//namespace VikingEngine.Commander
//{
//    /// <summary>
//    /// Resolve descided movement with backstabs
//    /// </summary>
//    class MoveUnitAction
//    {
//        Battle.AttackAnimation attackAnimation = null;
//        GamePhase_Movement phase;
//        Unit unit;
//        List<MoveLine> backstabbers;
//        VectorRect viewArea;
//        IntVector2 endPos;
//        //NetActionMove_Host netAction;
//        AbsGenericPlayer activePlayer;

//        public MoveUnitAction(GamePhase_Movement phase, Unit unit, MoveLinesGroup moveLines, 
//            VectorRect viewArea, AbsGenericPlayer activePlayer)
//        {
//            this.activePlayer = activePlayer;
//            this.phase = phase;
//            this.unit = unit;
//            this.viewArea = viewArea;

//            moveLines.onMovementAnimationComplete(activePlayer);
//            endPos = moveLines.CurrentSquarePos();

//            backstabbers = moveLines.getBackstabbers();

//            //netAction = new NetActionMove_Host(unit, moveLines);
            
//        }

//        public bool Update()
//        {
//            //end
//            if (attackAnimation == null)
//            {
//                if (!unit.Alive || backstabbers.Count == 0)
//                {
//                    complete();
//                    return true;
//                }
//                else
//                {
//                    MoveLine stabber = backstabbers[0];
//                    backstabbers.RemoveAt(0);

//                    IntVector2 backStabPos = stabber.fromPos;
//                    unit.SetPosition(backStabPos);
//                }
//            }
//            else
//            {
//                if (attackAnimation.Update())
//                {
//                    attackAnimation = null;
//                }
//            }

//            cmdRef.gamestate.absPlayers.LocalHost().SpectatorTargetPos = unit.squarePos;
//            return false;
//        }

//        void complete()
//        {
//            unit.SetPosition(endPos);
//            if (phase != null)
//            {
//                phase.OnMovementActionCompleted();
//            }
//            //netAction.Send();
//        }
//    }
//}
