using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    interface IHandWeaponAttack2CallbackObj
    {
        void OnWeaponAttackSwingStop(Vector3 particleCenter);
    }

    class HandWeaponAttack2 : AbsHandWeaponAttack
    {
        public IHandWeaponAttack2CallbackObj Wep2CallbackObj = null;
        HandWeaponAttackSettings sett;
        float swingSpeed;
        public Effects.BouncingBlockColors damageColors = new Effects.BouncingBlockColors(Data.MaterialType.gray_40, Data.MaterialType.gray_50);
        public bool preAttackState = false;

        public HandWeaponAttack2(HandWeaponAttackSettings sett, Graphics.VoxelModel masterImage, Characters.AbsCharacter parent, bool localUse)
            : base(GoArgs.Empty, sett.attackTime, parent, new Graphics.VoxelModelInstance(masterImage), sett.startScale, sett.damage, localUse)
        {
            weaponPosDiff = sett.weaponPosDiff;

            this.horizontalSwing = sett.HorizontalSwing;
            
            this.sett = sett;

            CollisionAndDefaultBound = new Bounds.ObjectBound(new LootFest.BoundData2(new VikingEngine.Physics.Box1axisBound(
                new VectorVolume(Vector3.Zero, sett.scale * sett.scaleToBoundSize), 
                parent.FireDir(this.Type)),
                Vector3.Zero));

            swingAngle = sett.swingStartAngle;
            swingSpeed =  sett.swingEndAngle - sett.swingStartAngle;
            swingSpeed /= sett.swingTime;
        }

        public void BeginPreAttack(Vector3 adjustOffset)
        {
            preAttackState = true;
            swingAngle = 0;
            weaponPosDiff += adjustOffset;
        }
        public void EndPreAttack()
        {
            preAttackState = false;
            swingAngle = sett.swingStartAngle;
            weaponPosDiff = sett.weaponPosDiff;

            //Music.SoundManager.PlaySound(LoadedSound.Sword1, image.Position);
            //Ref.sound.Play(LoadedSound.FastSwing, Position);
        }
        public override void Time_Update(UpdateArgs args)
        {
            if (!callBackObj.Alive)
            {
                DeleteMe();
                return;
            }

            if (preAttackState)
            {

            }
            else
            {
                if (sett.swingTime > 0)
                {
                    swingAngle += swingSpeed * Ref.DeltaTimeMs;
                    sett.swingTime -= Ref.DeltaTimeMs;
                    if (sett.swingTime <= 0)
                    {
                        if (Wep2CallbackObj != null)
                        {
                            Wep2CallbackObj.OnWeaponAttackSwingStop(particleCenter(attackAngle));
                        }
                    }
                }
                else
                {
                    swingAngle = sett.swingEndAngle;
                }

                if (image.Scale1D < sett.scale)
                {
                    image.Scale1D += (sett.scale - sett.startScale) * 10f * Ref.DeltaTimeSec;
                }

                base.Time_Update(args);

            }
        }

        protected override void updateAttackBound(Vector3 attackPos, RotationQuarterion attackAngle)
        {
            if (!preAttackState)
            {
                Vector3 moveBoundOutward = Vector3.Zero;
                moveBoundOutward.Z = CollisionAndDefaultBound.MainBound.halfSize.Z;
                Rotation1D angle;
                if (horizontalSwing)
                {
                    angle = callBackObj.FireDir(this.Type) + swingAngle;
                }
                else
                {
                    angle = callBackObj.FireDir(this.Type);
                }
                CollisionAndDefaultBound.UpdatePosition2(angle, attackAngle.TranslateAlongAxis(moveBoundOutward, attackPos));

                if (Ref.TimePassed16ms && sett.swingTime > 0)
                {
                    Vector3 particleCenter = this.particleCenter(attackAngle);
                    Engine.ParticleHandler.AddParticleArea(ParticleSystemType.BulletTrace, particleCenter, 0.25f, 4);

                    if (sett.fireParticles)
                    {
                        Engine.ParticleHandler.AddParticleArea(ParticleSystemType.Fire, particleCenter, 0.5f, 8);
                    }
                }

            }
        }

        Vector3 particleCenter(RotationQuarterion attackAngle)
        {
            Vector3 moveBoundOutward = Vector3.Zero;
            moveBoundOutward.Z = CollisionAndDefaultBound.MainBound.halfSize.Z * 0.6f;
            return attackAngle.TranslateAlongAxis(moveBoundOutward, CollisionAndDefaultBound.MainBound.center);
        }

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return damageColors;
            }
        }

        override public NetworkShare NetworkShareSettings { get { return GO.NetworkShare.None; } }
        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return base.WeaponTargetType;
            }
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            return base.handleCharacterColl(character, collisionData, usesMyDamageBound);
        }
        //protected override Data.Gadgets.BluePrint HandWeaponType
        //{
        //    get { return sett.handWeaponType; }
        //}
        //protected override WeaponTrophyType weaponTrophyType
        //{
        //    get
        //    {
        //        return WeaponTrophyType.Other;
        //    }
        //}

        public override GameObjectType Type
        {
            get
            {
                return sett.handWeaponType;
            }
        }
    }

    struct HandWeaponAttackSettings
    {
        public const float SwordBoundScaleW = 2.6f;
        public const float SwordBoundScaleH = 4f;
        public const float SwordBoundScaleL = 8.5f;

        public const float SwordStartScalePerc = 0.6f;
        public const float SwordSwingStartAngle = -1.5f;
        public const float SwordSwingEndAngle = 0.8f;
        public const float SwordSwingPercTime = 0.6f;

        public static readonly Vector3 StandardScaleToPosDiff = new Vector3(4f, 1.4f, 4f);

        public GO.GameObjectType handWeaponType;
        public float startScale;
        public float scale;
        public Vector3 scaleToBoundSize;
        public float attackTime;
        public DamageData damage;

        public float swingStartAngle, swingEndAngle;
        public float swingTime;
        public bool HorizontalSwing;

        public bool fireParticles;

        public Vector3 weaponPosDiff;

        public HandWeaponAttackSettings(GO.GameObjectType handWeaponType, float startScalePerc, float scale, 
            Vector3 scaleToBoundSize, Vector3 scaleToPosDiff, 
            float attackTime, float swingStartAngle, float swingEndAngle, float swingPercTime, DamageData damage)
        {
            this.handWeaponType = handWeaponType;
            this.startScale = startScalePerc * scale;
            this.scale = scale;
            this.scaleToBoundSize = scaleToBoundSize;
            this.attackTime = attackTime;
            this.damage = damage;

            this.swingStartAngle = swingStartAngle;
            this.swingEndAngle = swingEndAngle;
            this.swingTime = swingPercTime * attackTime;

            HorizontalSwing = true;
            fireParticles = false;

            weaponPosDiff = scaleToPosDiff * scale;
        }
    }
}
