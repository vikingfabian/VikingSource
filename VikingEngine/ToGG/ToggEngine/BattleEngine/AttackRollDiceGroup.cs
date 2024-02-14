using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
    class AttackRollDiceGroup
    {
        int currentAttack = 0;
        AttackRollDice[] attackWheels;
        BattleDice[] dice;

        public AttackRollDiceGroup(AttackRollDiceDisplay diceDisplay, ref Vector2 pos, BattleDice[] dice, int wheelsWidthCount)
        {
            this.dice = dice;
            attackWheels = new AttackRollDice[arraylib.SafeCount(dice)];
            int rowCount = 0;
            Vector2 wheelPos = pos;

            for (int i = 0; i < attackWheels.Length; ++i)
            {
                if (++rowCount > wheelsWidthCount)
                {
                    rowCount = 1;
                    wheelPos = new Vector2(pos.X, wheelPos.Y + DiceModel.Size.Y + DiceModel.SpacingX);
                }

                attackWheels[i] = new AttackRollDice(diceDisplay, dice[i], wheelPos, i);
                wheelPos.X += DiceModel.Size.X + DiceModel.SpacingX;
            }

            pos.Y = wheelPos.Y + DiceModel.Size.Y;
        }


        public void update()
        {
            foreach (var slot in attackWheels)
            {
                slot.Update();
            }
        }

        public void onRollHover(bool mouseEnter)
        {
            for (int i = 0; i < attackWheels.Length; ++i)
            {
                if (mouseEnter)
                {
                    attackWheels[i].beginRoll(dice[i], i);
                }
                else
                {
                    attackWheels[i].setDieIconTexture(dice[i]);
                }

                attackWheels[i].state = mouseEnter ? AttackRollDiceState.RollButtonHover : AttackRollDiceState.Waiting;
            }
        }

        public void onRollClick()
        {
            for (int i = 0; i < attackWheels.Length; ++i)
            {
                attackWheels[i].beginRoll(dice[i], i);
                attackWheels[i].rotationType_0non_1idle_2full = 2;
            }
        }

        public AttackRollDice setNextResult(BattleDiceSide side)
        {
            AttackRollDice die = null;

            if (arraylib.InBound(attackWheels, currentAttack))
            {
                die = attackWheels[currentAttack];
                die.SetResult(side);
            }
            ++currentAttack;

            return die;
        }

        public BattleDice nextDie()
        {
            return dice[currentAttack];
        }

        public bool bumpNextDie(ref int index)
        {
            if (attackWheels.Length > index)
            {
                attackWheels[index].idleBump();
                ++index;
            }
            return index >= attackWheels.Length;
        }
        
        public bool hasMore()
        {
            return currentAttack < attackWheels.Length;
        }

        public void DeleteMe()
        {
            foreach (var att in attackWheels)
            {
                att.DeleteMe();
            }
        }
    }

}
