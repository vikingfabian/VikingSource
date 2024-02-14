using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    class CatapultAttackAnimation
    {
        VikingEngine.ToGG.Effects.CatapultCombatEffect catapultCombatEffect;
        int state_viewAttacker0_viewDefender1_damage2 = 0;
        Time stateTime = new Time(0.2f, TimeUnit.Seconds);
        Commander.Players.AbsCmdPlayer activePlayer;
        IntVector2 targetSquare;
        AbsUnit attacker;

        public CatapultAttackAnimation(Commander.Players.AbsCmdPlayer activePlayer, AbsUnit attacker, IntVector2 targetSquare,
            Display3D.CatapultTargetGUI catapultTargetGUI)
        {
            this.activePlayer = activePlayer;
            this.attacker = attacker;

            float centerHitChance = 0.2f;

            if (attacker.HasProperty(UnitPropertyType.Catapult_Plus))
            {
                centerHitChance = toggLib.CatapultPlusCenterHitChance;
            }

            if (Ref.rnd.Chance(centerHitChance) == false)
            {
                targetSquare += arraylib.RandomListMember(IntVector2.Dir8Array);
            }

            //Range range = new Range(-1, 1);
            //targetSquare.X += range.GetRandom();
            //targetSquare.Y += range.GetRandom();

            this.targetSquare = targetSquare;
            catapultTargetGUI.viewHitPos(targetSquare);

            activePlayer.SpectatorTargetPos = attacker.squarePos;
        }

        public bool Update()
        {
            bool endAnimation = false;

            if (catapultCombatEffect != null)
            {
                if (catapultCombatEffect.IsDeleted)
                {
                    catapultCombatEffect = null;
                }
                return false;
            }


            if (stateTime.CountDown())
            {
                state_viewAttacker0_viewDefender1_damage2++;

                switch (state_viewAttacker0_viewDefender1_damage2)
                {
                    case 1:
                        catapultCombatEffect = new Effects.CatapultCombatEffect(attacker, targetSquare);
                        activePlayer.SpectatorTargetPos = targetSquare;
                        //stateTime.Seconds = 0.6f;
                        break;
                    case 2:
                        if (toggRef.board.tileGrid.InBounds(targetSquare))
                        {
                            var sq = toggRef.board.tileGrid.Get(targetSquare);
                            bool destruction = sq.destroyTerrain();
                            if (sq.unit != null)
                            {
                                if (destruction && sq.unit.HasProperty(UnitPropertyType.Static_target))
                                {
                                    //Is destroyed with the terrain
                                    sq.unit.health.Value = 1;
                                }
                                
                                sq.unit.TakeHit(attacker);
                            }
                            stateTime.Seconds = 0.6f;

                            Engine.ParticleHandler.AddExpandingParticleArea(Graphics.ParticleSystemType.CommanderDamage,
                                toggRef.board.toWorldPos_Center(targetSquare, 0.3f), 0.4f, 120, 0.1f);
                        }
                        else
                        {
                            endAnimation = true; //Outside map
                        }
                        
                        break;
                        
                    default:
                        endAnimation = true;
                        break;

                }
            }

            if (endAnimation)
            {
                activePlayer.removeAttackerArrow();//.attackerArrow.Visible = false;
            }

            return endAnimation;
        }
    }
}
