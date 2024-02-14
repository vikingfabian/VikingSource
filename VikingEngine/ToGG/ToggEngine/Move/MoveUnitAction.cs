using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    /// <summary>
    /// Resolve descided movement with backstabs
    /// </summary>
    class MoveUnitAction
    {
        //Commander.Battle.AttackAnimation attackAnimation = null;
        GamePhase_Movement phase;
        AbsUnit unit;
        //List<MoveLine> backstabbers;
        VectorRect viewArea;
        IntVector2 endPos;
        AbsGenericPlayer activePlayer;

        Time introTime = new Time(0.4f, TimeUnit.Seconds);
        Time exitTime = new Time(0.5f, TimeUnit.Seconds);

        int state_0intro_2backstab_3exit = 0;

        public MoveUnitAction(GamePhase_Movement phase, AbsUnit unit, MoveLinesGroup moveLines, 
            VectorRect viewArea, AbsGenericPlayer activePlayer)
        {
            this.activePlayer = activePlayer;
            this.phase = phase;
            this.unit = unit;
            this.viewArea = viewArea;

            if (moveLines != null)
            {
                moveLines.onMovementAnimationComplete(activePlayer);
                endPos = moveLines.CurrentSquarePos();
                //backstabbers = moveLines.getBackstabbers();
                
            }
        }

        public bool Update()
        {
            //end
            switch (state_0intro_2backstab_3exit)
            {
                case 0:
                    toggRef.absPlayers.LocalHost().SpectatorTargetPos = unit.squarePos;
                    if (introTime.CountDown())
                    {
                        state_0intro_2backstab_3exit++;
                    }
                    break;
                case 1:
                    toggRef.absPlayers.LocalHost().SpectatorTargetPos = endPos;

                    unit.SetPosition(endPos);
                    unit.order.CheckList_Enabled = false;

                    state_0intro_2backstab_3exit = 3;

                    if (!unit.Alive || unit.movelines.backStabbersFullCount() == 0)
                    {
                        state_0intro_2backstab_3exit = 3;
                    }
                    else
                    {
                        phase.attackDisplay = new Commander.Battle.AttackDisplay(phase.absPlayer, unit);
                        phase.attackDisplay.beginAttack();

                        state_0intro_2backstab_3exit = 2;
                    }
                    //    MoveLine stabber = backstabbers[0];
                    //    backstabbers.RemoveAt(0);

                    //    IntVector2 backStabPos = stabber.fromPos;
                    //    unit.SetPosition(backStabPos);
                    //}
                    break;
                case 2:
                    if (phase.attackDisplay.Update(ref PhaseUpdateArgs.None))
                    {
                        phase.attackDisplay.DeleteMe();
                        phase.attackDisplay = null;
                        state_0intro_2backstab_3exit++;
                    }
                    break;

                case 3:
                    if (exitTime.CountDown())
                    {
                        return true;
                    }
                    break;

            }


            //if (introTime.CountDown())
            //{
            //    if (attackAnimation == null)
            //    {
            //        if (!unit.Alive || backstabbers == null || backstabbers.Count == 0)
            //        {
            //            complete();
            //            return true;
            //        }
            //        else
            //        {
            //            MoveLine stabber = backstabbers[0];
            //            backstabbers.RemoveAt(0);

            //            IntVector2 backStabPos = stabber.fromPos;
            //            unit.SetPosition(backStabPos);
            //        }
            //    }
            //    else
            //    {
            //        if (attackAnimation.Update())
            //        {
            //            attackAnimation = null;
            //        }
            //    }
            //}

            
            return false;
        }

        void complete()
        {
            unit.SetPosition(endPos);
        }
    }
}
