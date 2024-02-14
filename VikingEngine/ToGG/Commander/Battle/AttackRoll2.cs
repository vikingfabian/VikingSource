using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.Battle
{
    class AttackRoll2: AbsAttackRoll2
    {
        public BattleSetup setup;
        //public BattleDice dice;
        public List<AbsUnit> attacker;
        public AbsUnit defender;
        public int completedRetreats = 0;
        public IntVector2 attackPosition;
        Graphics.ImageGroup images = new Graphics.ImageGroup();

        public AttackRoll2(List<AbsUnit> attacker, AbsUnit defender, AttackType AttackType,
             CommandCard.CommandType CommandType, VectorRect area)
        {
            this.attacker = attacker;
            this.defender = defender;

            attackPosition = defender.squarePos;
            

            //if (AttackType.Is(AttackUnderType.BackStab))
            //{
            //    dice = Commander.BattleLib.BackstabDie;
            //}
            //else if (AttackType.IsMelee)
            //{
            //    dice = Commander.BattleLib.MeleeDie;
            //}
            //else
            //{
            //    dice = Commander.BattleLib.RangedDie;
            //}

            setup = new BattleSetup(
                    attacker, new AttackTarget(defender, AttackType), CommandType);

            //setup = new BattleSetup(attacker, new AttackTarget(defender, AttackType), CommandType);

            initVisuals(area);
        }

        public void initVisuals(VectorRect area)
        {
            AttackWheelsSize(out wheelsWidthCount, out width);
            Vector2 pos = area.Position;
            topLeft = pos;

            var attackDiceArray = multiplyDie(setup.dice, setup.AttackerSetup.attackStrength);
            attackDice = new AttackRollDiceDisplay(true, setup, attacker, ref pos,
                attackDiceArray, wheelsWidthCount, width);

            float introtime, rolltime;
            bool simultaniousDefence;
            toggRef.gamestate.attackRollSpeed.rollTimer(setup.AttackerSetup.attackStrength,
                out simultaniousDefence, out introtime, out rolltime);

            nextResultTimer = new Timer.Basic(rolltime, true);
            introViewTimeAdd = new Time(introtime);

            //support icons
            foreach (var m in setup.supportingUnits.units)
            {
                Vector3 wp = toggRef.board.toGuiPos_Center(m.unit.squarePos, ModelLayers.UnitMoveAndAttackGUI);
                
                Graphics.Mesh tDot = new Graphics.Mesh(LoadedMesh.plane, wp, new Vector3(0.25f),
                    Graphics.TextureEffectType.Flat, 
                    m.support == 1? SpriteName.cmdSupporterIcon1 : SpriteName.cmdSupporterIcon2, 
                    Color.White);

                tDot.Rotation = toggLib.PlaneTowardsCam;
                images.Add(tDot);
            }
        }

        public void beginAttack()
        {
            toggRef.gamestate.attackRollSpeed.onAttackRoll();
            attackDice.dice.onRollClick();
        }

        public bool Update()
        {
            if (waitingOnDie != null)
            {
                if (waitingOnDie.state >= AttackRollDiceState.EndBounce)
                {
                    resolveDieResult(waitingOnDie);
                    waitingOnDie = null;
                }
            }
            else if (introViewTimeAdd.CountDown())
            {
                bool hasMoreAttacks = attackDice.dice.hasMore();

                if (hasMoreAttacks)
                {
                    if (nextResultTimer.Update())
                    {
                        BattleDiceSide rndDieSide = attackDice.dice.nextDie().NextRandom(MainAttacker.Player.Dice);
                        viewDieResult(rndDieSide);
                    }
                }
                else
                {
                    return true;
                }
            }

            //attackDice.dice.update();
            updateDice();

            return false;
        }

        void resolveDieResult(AttackRollDice die)
        {
            BattleDiceSide side = die.dieSide;
            //bool blockedAttack = false;

            switch (side.result)
            {
                case BattleDiceResult.Hit1:
                    attackRollResult.hits += 1;
                    damageBobble().set(attackRollResult.hits);

                    if (setup.hitBlocks > 0)
                    {
                        setup.hitBlocks--;
                        defender.blockAnimation();
                    }
                    else
                    {
                        defender.TakeHit(MainAttacker);
                    }
                    break;
                case BattleDiceResult.Retreat:
                    attackRollResult.retreats += 1;
                    retreatBobble().set(attackRollResult.retreats);

                    if (setup.retreatIgnores > 0)
                    {
                        setup.retreatIgnores--;
                        defender.blockRetreatAnimation();
                    }
                    else
                    {
                        if (defender.Retreat(MainAttacker))
                        {
                            completedRetreats++;
                        }
                    }
                    break;
            }

            //if (blockedAttack)
            //{
            //    defender.blockAnimation();
            //}
        }

        DiceResultLabel retreatBobble()
        {
            if (attackDice.resultLabel2 == null)
            {
                attackDice.resultLabel2 = new DiceResultLabel(attackDice, false, 3);
            }

            return attackDice.resultLabel2;
        }

        protected BattleDice[] multiplyDie(BattleDice die, int count)
        {
            BattleDice[] result = new BattleDice[count];
            for (int i = 0; i < count; ++i)
            {
                result[i] = die;
            }

            return result;
        }

        public void DeleteMe()
        {
            images.DeleteAll();
            attackDice.DeleteMe();
        }

        public AbsUnit MainAttacker => attacker[0];
    }
}
