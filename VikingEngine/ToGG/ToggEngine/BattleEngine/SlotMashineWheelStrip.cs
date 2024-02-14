using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
    class SlotMashineWheelStrip
    {
        int currentAttack = 0;
        SlotMashineWheel[] attackWheels;
        List<BlockHitEffect> blocks = new List<BlockHitEffect>();
        BattleDice[] dice;

        public SlotMashineWheelStrip(ref Vector2 pos, BattleDice[] dice, int wheelsWidthCount)
        {
            this.dice = dice;
            attackWheels = new SlotMashineWheel[dice.Length];
            int rowCount = 0;
            Vector2 wheelPos = pos;

            for (int i = 0; i < dice.Length; ++i)
            {
                if (++rowCount > wheelsWidthCount)
                {
                    rowCount = 1;
                    wheelPos = new Vector2(pos.X, wheelPos.Y + SlotMashineWheel.Size.Y + SlotMashineWheel.SpacingX);
                }

                attackWheels[i] = new SlotMashineWheel(dice[i], wheelPos, i);
                wheelPos.X += SlotMashineWheel.Size.X + SlotMashineWheel.SpacingX;
            }

            pos.Y = wheelPos.Y + SlotMashineWheel.Size.Y;
        }

        public void update()
        {
            foreach (var slot in attackWheels)
            {
                slot.Update();
            }
        }

        public void setNextResult(BattleDiceResult result)
        {
            attackWheels[currentAttack].SetResult(result);
            ++currentAttack;
        }

        public BattleDice nextDie()
        {
            return dice[currentAttack];
        }

        public void blockEffect()
        {
            blocks.Add(new BlockHitEffect(attackWheels[currentAttack], true));
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

            foreach (var block in blocks)
            {
                block.DeleteMe();
            }
        }
    }
}
