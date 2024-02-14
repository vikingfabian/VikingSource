using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.Battle
{
    class AttackAnimation : AbsAttackAnimation
    {
        public IntVector2 attackPosition;
        //public NetActionAttack_Host NetAction;
        BattleDice dice;

        public AttackAnimation(AbsUnit attacker, AbsUnit defender, AttackType AttackType, VectorRect area,
            CommandCard.CommandType CommandType, System.IO.BinaryWriter writerForBackstab, AbsGenericPlayer activePlayer)
                : base(attacker, defender, AttackType, activePlayer)
        {
            attackPosition = defender.squarePos;

            //attacks = new BattleSetup(
            //    attacker, new AttackTarget(defender, AttackType), CommandType);

            //switch (AttackType)
            //{
            //    default:
            //        dice = BattleLib.MeleeDie;
            //        break;
            //    case AttackType.Ranged:
            //        dice = BattleLib.RangedDie;
            //        break;
            //    case AttackType.Backstab:
            //        dice = BattleLib.BackstabDie;
            //        break;
            //}
            if (AttackType.Is(AttackUnderType.BackStab))
            {
                dice = Commander.BattleLib.BackstabDie;
            }
            else if (AttackType.IsMelee)
            {
                dice = Commander.BattleLib.MeleeDie;
            }
            else
            {
                dice = Commander.BattleLib.RangedDie;
            }

            supporterEffects(attacks.supportingUnits);

            beginAttack();
            init(multiplyDie(dice, attacks.AttackerSetup.attackStrength), area, defender);

            //if (writerForBackstab != null)
            //{
            //    //NetAction = new NetActionAttack_Host(writerForBackstab, attacks);
            //}
        }

        

        protected void init(BattleDice[] dice, VectorRect area, AbsUnit defender)
        {
            /*
             * fast bredd, brevid attacken
             * icon för % chans
             * lista attack + support + bonusar (strategi)
             * lista defence
             */

            Vector2 pos;
            placement(area, out pos);
            attackTypeTitle(ref pos);
            attackerAndSupportInfo(ref pos, dice[0]);

            attackwheels = new SlotMashineWheelStrip(ref pos, dice, wheelsWidthCount);
            //diceWheels(ref pos, dice);

            //listAllModifiers(ref pos);
        }

        public override bool Update()
        {
            bool hasMoreAttacks = attackwheels.hasMore() && defender.health.HasValue;

            if (hasMoreAttacks)
            {
                if (introViewTimeAdd.CountDown() && nextResultTimer.Update())
                {
                    bool blockedAttack;
                    BattleDiceResult result = nextAttack(out blockedAttack);
                    activePlayer.SpectatorTargetPos = defender.squarePos;

                    onResult(result, blockedAttack);
                    
                    if (defender.Dead)
                    {
                        //Killed unit
                        attacker.Player.onDestroyedUnit(attacker, defender);
                    }
                }
            }
            else if (finalViewTime.CountDown())
            {
                DeleteMe();

                return true;
            }

            attackwheels.update();
            //foreach (var slot in attackWheels)
            //{
            //    slot.Update();
            //}

            return false;

        }

        protected override BattleDiceResult nextAttack(out bool blockedAttack)
        {
            blockedAttack = false;
            var result = dice.NextRandom(attacker.Player.Dice).result;

            if (result == BattleDiceResult.Hit1)
            {
                if (attacks.hitBlocks > 0)
                {
                    attacks.hitBlocks--;
                    blockedAttack = true;
                }
            }
            else if (result == BattleDiceResult.Retreat)
            {
                if (attacks.retreatIgnores > 0)
                {
                    attacks.retreatIgnores--;
                    blockedAttack = true;
                }
            }

            return result;
        }

        protected void onResult(BattleDiceResult result, bool blockedAttack)
        {
            if (!blockedAttack)
            {
                if (result == BattleDiceResult.Hit1)
                {
                    defender.TakeHit(attacker);
                }
                else if (result == BattleDiceResult.Retreat)
                {
                    if (defender.Retreat(attacker))
                    {
                        retreats++;
                    }
                }
            }

            if (blockedAttack)
            {
                attackwheels.blockEffect();//blockEffect();
            }
        }


        public override void DeleteMe()
        {
            //if (NetAction != null)
            //{
            //    NetAction.AttackComplete();

            //    if (AttackType != AttackType.Backstab)
            //    {
            //        NetAction.Send();
            //    }
            //}
            base.DeleteMe();
        }
    }

    
}
