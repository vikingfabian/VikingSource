using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Commander.Battle;

namespace VikingEngine.ToGG.Commander.GamePhase
{
    abstract class AbsAttackPhase : AbsGamePhase
    {
        public AttackDisplay attackDisplay = null;
        protected FollowUp followUp = null;

        public AbsAttackPhase(Commander.Players.AbsLocalPlayer player)
            : base(player)
        { }

        protected void viewFollowUpPrompt()
        {
            if (toggRef.board.MovementRestriction(attackDisplay.attackRoll.attackPosition, attackDisplay.attackRoll.MainAttacker) ==
                            ToggEngine.Map.MovementRestrictionType.Impassable)
            {
                //endAttackAnimation();
            }
            else
            {
                if (player == null)
                { //Run ai
                    int stayValue = attackDisplay.attackRoll.MainAttacker.positionValue(attackDisplay.attackRoll.MainAttacker.squarePos);
                    int followUpValue = attackDisplay.attackRoll.MainAttacker.positionValue(attackDisplay.attackRoll.attackPosition);

                    if (followUpValue > stayValue)
                    {
                        FollowUp.Commit(attackDisplay.attackRoll);
                    }
                    else
                    {
                        //endAttackAnimation();
                    }
                }
                else
                {
                    followUp = new FollowUp(attackDisplay.attackRoll, player);
                }
            }
        }

        protected bool updateAttack(ref PhaseUpdateArgs args)
        {
            if (attackDisplay != null)
            {
                if (attackDisplay.Update(ref args))
                {
                    OnAttackComplete();
                    return true;
                }

                return attackDisplay.IsAttacking;
            }

            return false;
        }

        protected void removeFollowUp()
        {
            followUp.DeleteMe();
            followUp = null;

            absPlayer.updateAttackAbility(true, true);
        }

        virtual protected void OnAttackComplete()
        {
            PhaseMarkVisible(true);
        }
    }
}
