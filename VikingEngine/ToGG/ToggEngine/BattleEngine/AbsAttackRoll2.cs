using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
    class AbsAttackRoll2
    {
        public AttackRollDiceDisplay attackDice;
        protected int wheelsWidthCount;
        protected float width;
        protected Vector2 topLeft;
        protected Time introViewTimeAdd;
        protected AttackRollDice waitingOnDie = null;
        protected Timer.Basic nextResultTimer;
        public AttackRollResult attackRollResult = new AttackRollResult();

        AttackRollDiceDisplay bumpMotionGroup;
        Time nextBumpMotionTimer = new Time(Ref.rnd.Float(3f, 8f), TimeUnit.Seconds);
        int bumpMotionDiceIndex = 0;

        public static void AttackWheelsSize(out int wheelsWidthCount, out float width)
        {
            wheelsWidthCount = (int)((Engine.Screen.Height * 0.52f) / (DiceModel.Size.X + DiceModel.SpacingX));
            width = wheelsWidthCount * (DiceModel.Size.X + DiceModel.SpacingX) - DiceModel.SpacingX;
        }

        protected void viewDieResult(BattleDiceSide side)
        {
            waitingOnDie = attackDice.dice.setNextResult(side);
        }

        public void addHits(int hits, bool critical)
        {
            int crits = lib.BoolToInt01(critical) * hits;
            damageBobble().Add(hits, crits);
            attackRollResult.hits += hits;
            attackRollResult.critiqualHits += crits;
        }

        protected DiceResultLabel damageBobble()
        {
            if (attackDice.resultLabel1 == null)
            {
                attackDice.resultLabel1 = new DiceResultLabel(attackDice, true, 0);
            }

            return attackDice.resultLabel1;
        }

        virtual protected void updateDice()
        {
            attackDice.dice.update();
        }

        public void idleUpdate()
        {
            updateDice();

            if (nextBumpMotionTimer.CountDown())
            {
                if (bumpMotionDiceIndex == 0)
                {
                    //if (bumpMotionGroup == null)
                    //{
                    //    bumpMotionGroup = attackDice;
                    //}
                    //else
                    //{
                        bumpMotionGroup = nextDiceGroup(bumpMotionGroup);
                    //}
                }

                if (bumpMotionGroup.dice.bumpNextDie(ref bumpMotionDiceIndex))
                {
                    //End of row
                    bumpMotionDiceIndex = 0;
                    nextBumpMotionTimer = new Time(Ref.rnd.Float(4f, 6f), TimeUnit.Seconds);
                }
                else
                {
                    nextBumpMotionTimer.MilliSeconds = 400;
                }
            }
        }

        public void postUpdate()
        {
            attackDice.dice.update();
        }

        virtual protected AttackRollDiceDisplay nextDiceGroup(AttackRollDiceDisplay current)
        {
            return attackDice;
        }
    }
}
