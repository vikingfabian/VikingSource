using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.GameObject
{
    partial class AbsDetailUnit
    {
        //public AbsDetailUnit soldier;

        public float prevAttackTime;
        public Time attackCooldownTime = 0;
        Time attackFrameTime;
        public int attackSetIndex;

        public Rotation1D attackDir;

        //public AttackAnimation(AbsDetailUnit soldier)
        //{
        //    this.soldier = soldier;
        //}

        public void updateAttack(float time)
        {
            if (attackCooldownTime.CountDown(time) == false)
            {
                if (IsSoldierUnit())
                {
                    attackFrameTime.CountDown(time);
                }
            }

            //if (attackCooldownTime.MilliSeconds
        }

        public bool inAttackAnimation()
        {
            return attackFrameTime.HasTime;
        }

        protected int startMultiAttack(bool fullUpdate, AbsDetailUnit target, bool mainAttack, int attackCount, bool local)
        {
            int hitCount=0;

            if (attackTarget.IsSingleTarget())
            {
                for (int i = 0; i < attackCount; i++)
                {
                    startAttack(fullUpdate, target, mainAttack, local);
                }

                hitCount = attackCount;
            }
            else
            {
                attackCount += 1;
                for (int i = 0; i < attackCount; i++)
                {
                    var groupTarget = target.group.soldiers.GetRandomUnsafe(Ref.rnd);
                    if (groupTarget != null)
                    {
                        startAttack(fullUpdate, groupTarget, mainAttack, local);
                        ++hitCount;
                    }
                }
            }

            return hitCount;
        }

        protected void startAttack(bool fullUpdate, AbsDetailUnit target, bool mainAttack, bool local)
        {
            if (target != null)
            {
                var data = Data();

                attackCooldownTime.MilliSeconds = data.attackTimePlusCoolDown;
                prevAttackTime = attackCooldownTime.MilliSeconds;
                attackFrameTime.MilliSeconds = data.attackFrameTime;

                attackDir = angleToUnit(target);

                int damage;
                if (mainAttack)
                {
                    if (target.DetailUnitType() == UnitType.City)
                    {
                        damage = data.attackDamageStructure;
                    }
                    else
                    {
                        damage = data.attackDamage;
                    }
                }
                else
                {
                    damage = data.secondaryAttackDamage;
                }

                if (data.mainAttack == AttackType.Melee && mainAttack)
                {
                    target.takeDamage(damage, attackDir, group.army.faction, fullUpdate);
                }
                else
                {
                    if (mainAttack)
                    {
                        Projectile.ProjectileAttack(fullUpdate, this, data.mainAttack, target, damage, data.splashDamageCount, data.percSplashDamage );
                    }
                    else
                    {
                        Projectile.ProjectileAttack(fullUpdate, this, data.secondaryAttack, target, damage, 0, 0);
                    }
                }
            }
        }

        public bool mustCompleteAttackSet()
        {
            return attackSetIndex > 0;
        }

        public bool IsAttacking
        {
            get { return attackCooldownTime.HasTime; }
        }

        public void clearAttack()
        {
            attackFrameTime.setZero();
        }
    }

    enum AttackType
    {
        Melee,
        Arrow,
        Bolt,
        RocketArrow,
        Ballista,
        Cannonball,
        FireBomb,
        SlingShot,
        KnifeThrow,
        SecondaryJavelin,
        Javelin,
        NUM_NON
    }

    enum HasTargetInReach
    {
        InReach,
        MustWalk,
        MustRotate,
        NoTarget,
        UseBlankTarget,
    }
}
