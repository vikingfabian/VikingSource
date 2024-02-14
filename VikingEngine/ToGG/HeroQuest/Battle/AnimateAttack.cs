using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.BattleEngine;

namespace VikingEngine.ToGG.HeroQuest
{
    class AnimateAttack : AbsUpdateable
    {
        AttackRoll2 attackRoll;
        DefenderRoll defenderRoll;

        StateTimer timer = new StateTimer();
        //int state = 0;
        float introTime;// = 1000;//200;
        

        public AnimateAttack(float introTime, AttackRoll2 attackRoll, DefenderRoll defenderRoll)
            :base(true)
        {
            this.introTime = introTime;
            this.attackRoll = attackRoll;
            this.defenderRoll = defenderRoll;
        }

        public override void Time_Update(float time_ms)
        {
            timer.update();

            if (timer.passedTime(introTime))
            {
                animateWeapon();
            }
            else if(timer.passedTime(introTime + 300))
            {
                animateDamage();

                DeleteMe();
            }
        }

        void animateWeapon()
        {
            effectsLib.WeaponSwing(
                attackRoll.attacker, defenderRoll.unit.squarePos,
                attackRoll.AttackType);
        }

        void animateDamage()
        {
            var defender = defenderRoll.unit;

            if (defenderRoll.result.dodge)
            {
                defender.hq().dodgeAnimation(attackRoll.attacker, true);
            }
            else
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqAttackResult, Network.PacketReliability.Reliable);

                Damage dealDamage = Damage.Zero;
                dealDamage.applyType = DamageApplyType.Attack;

                dealDamage.Value += attackRoll.attackRollResult.critiqualHits;
                int blockableDamage = attackRoll.attackRollResult.hits - attackRoll.attackRollResult.critiqualHits;

                int blocks = Bound.Min(defenderRoll.result.blocks - attackRoll.attackRollResult.pierce, 0);

                dealDamage.Value += Bound.Min(blockableDamage - blocks, 0);

                if (dealDamage.Value > 0)
                {
                    w.Write(true);

                    HeroQuest.RecordedDamageEvent rec;
                    defender.hq().TakeDamage(dealDamage, DamageAnimationType.AttackSlash,
                        null, out rec, true);
                    defenderRoll.target.recordedDamage = rec;
                    rec.write(w);

                    if (defender.Dead)
                    {
                        attackRoll.attacker.Player.onDestroyedUnit(attackRoll.attacker, defender);
                    }
                    else
                    {
                        attackRoll.attacker.Player.onDamagedUnit(attackRoll.attacker, defender);
                    }

                    toggRef.hud.addInfoCardDisplay(new ToggEngine.Display2D.UnitCardDisplay(
                        defender, ToggEngine.Display2D.UnitDisplaySettings.Defender, hqRef.playerHud));
                }
                else
                {
                    w.Write(false);
                    defender.hq().netWriteUnitId(w);
                    defender.hq().blockAnimation();
                }
            }
        }
    }

    
}
