using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.GameObject
{
    partial class AbsDetailUnit
    {
        //public AbsDetailUnit soldier;

        public float prevAttackTime;
        public Time attackCooldownTime = 0;
        Time attackFrameTime;
        //public int attackSetIndex;

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
            int hitCount = 0;

            if (target != null)
            {
                if (target.IsSingleTarget())
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
                        var groupTarget = target.group.soldiers?.GetRandomUnsafe(Ref.peRnd);
                        if (groupTarget != null)
                        {
                            startAttack(fullUpdate, groupTarget, mainAttack, local);
                            ++hitCount;
                        }
                    }
                }
            }

            return hitCount;
        }

        protected void startAttack(bool fullUpdate, AbsDetailUnit target, bool mainAttack, bool local)
        {
            if (target != null)
            {
                //if (target.GetAbsArmy().debugTagged)
                //{
                //    lib.DoNothing();
                //}
                attackCooldownTime.MilliSeconds = soldierData.attackTimePlusCoolDown;
                prevAttackTime = attackCooldownTime.MilliSeconds;
                attackFrameTime.MilliSeconds = Profile().attackFrameTime;
                               

                int damage;
                float blockReduce = soldierData.blockReducingAttack_Inv;

                //Height advantage
                if (group.position.Y + position.Y - Map.Settings.Height.DefaultGroundYoffset >= target.group.position.Y + target.position.Y &&
                    !IsShipType())
                {
                    blockReduce *= DssConst.HeightAdvantageBlockReduce_multiply;
                    if (fullUpdate)
                    {
                        Vector3 pos = position;
                        pos.Y += DssConst.Men_StandardModelScale * 0.8f;
                        Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.GoldenSparkle, pos, DssConst.Men_StandardModelScale * 0.3f, 6);
                    }
                }

                if (mainAttack)
                {
                    damage = soldierData.attackDamage;

                    if (group != null &&
                        group.soldierConscript.conscript.specialization == SpecializationType.AntiCavalry)
                    {
                        switch (target.DetailUnitType())
                        {
                            case UnitBuildType.ConscriptCavalry:
                            case UnitBuildType.ConscriptBalkong:
                                damage = MathExt.MultiplyInt(DssConst.AntiCavalryBonusMultiply, damage);
                                break;
                        }
                    }
                }
                else
                {
                    damage = soldierData.secondaryAttackDamage;
                }

                damage += damage * group.soldierAttackDamageBonus;

                attackDir = angleToUnit(target);

                if (soldierData.mainAttack == AttackType.Melee && mainAttack)
                {
                    if (fullUpdate)
                    {
                        if (IsShipType())
                        {
                            new ShipMeleeAttack(GetSoldierUnit(), attackDir);
                        }

                        if (Ref.peRnd.ChanceF(DssConst.SoundChanceSword))
                        {
                            switch (group.soldierConscript.conscript.weapon)
                            {
                                case Resource.ItemResourceType.HandSpear:
                                case Resource.ItemResourceType.Pike:
                                case Resource.ItemResourceType.SharpStick:
                                //case Resource.ItemResourceType.KnightsLance:
                                    SoundLib.spear_whoosh.Play(position);

                                    break;

                                case Resource.ItemResourceType.BronzeSword:
                                case Resource.ItemResourceType.ShortSword:
                                    SoundLib.blade_light.Play(position);
                                    break;

                                case Resource.ItemResourceType.Sword:
                                case Resource.ItemResourceType.LongSword:
                                    SoundLib.blade_medium.Play(position);
                                    break;

                                case Resource.ItemResourceType.TwoHandSword:
                                case Resource.ItemResourceType.MithrilSword:
                                    SoundLib.blade_heavy.Play(position);
                                    break;

                                default:
                                    SoundLib.sword.Play(position);
                                    break;
                            }

                        }
                    }

                    target.takeDamage(damage, blockReduce, this, attackDir, GetFaction(), fullUpdate, out _);
                }
                else
                {
                    if (target.soldierData.arrowWeakness)
                    {
                        damage = MathExt.MultiplyInt(DssConst.ArrowWeaknessBonusMultiply, damage);
                    }

                    if (mainAttack)
                    {
                        Projectile.ProjectileAttack(fullUpdate, this, soldierData.mainAttack, target, damage, blockReduce, soldierData.attackSplashCount);
                    }
                    else
                    {
                        Projectile.ProjectileAttack(fullUpdate, this, soldierData.secondaryAttack, target, damage, blockReduce, soldierData.attackSplashCount);
                    }
                }

                var f = this.GetFaction();
                if (f != null && f.player.IsLocalPlayer())
                {
                    if (group.soldierConscript.conscript.isKnight())
                    {
                        DssRef.achieve.UnlockAchievement(AchievementIndex.rear_flanking);
                    }
                    //switch (group.soldierConscript.conscript.weapon)
                    //{
                    //    case Resource.ItemResourceType.KnightsLance:
                    //        if (ItemPropertyColl.Get(target.group.soldierConscript.conscript.weapon).Filter_IsSiegeWeapon)
                    //        { 
                    //            DssRef.achieve.UnlockAchievement(AchievementIndex.rear_flanking);
                    //        }                           
                    //        break;

                        //    //case ItemResourceType.SiegeCannonBronze:
                        //    //    if (target.group.InGuardPost())
                        //    //    {
                        //    //        DssRef.achieve.UnlockAchievement(AchievementIndex.ottoman);
                        //    //    }
                        //    //    break;
                        //}
                }
            }
        }

        //public bool mustCompleteAttackSet()
        //{
        //    return attackSetIndex > 0;
        //}

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
        Catapult,
        Haubitz,
        Cannonball,
        MassiveCannonball,
        FireBomb,
        SlingShot,
        KnifeThrow,
        SecondaryJavelin,
        Javelin,

        GunShot,
        GunBlast,
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
