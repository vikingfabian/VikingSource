using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GO.WeaponAttack;

namespace VikingEngine.LootFest.GO
{
    class BarbarianDaneSuit : AbsBarbarian, IHandWeaponAttack2CallbackObj
    {
        const float AttackTime = 400;
        const float ReloadTime = 250;
        const float WeaponScale = 0.24f;
        const float BoundScaleH = HandWeaponAttackSettings.SwordBoundScaleH;  
        const float BoundScaleW = 5f;        
        const float BoundScaleL = 6.5f;
        static readonly Vector3 ScaleToPosDiff = new Vector3(3f, 0.5f, 3.8f);

        WeaponAttack.HandWeaponAttackSettings downSwing;

        public BarbarianDaneSuit(Players.AbsPlayer user)
            : base(user, VoxelModelName.barbarian2haxe_base)
        {
            primaryWeaponAttack = new WeaponAttack.HandWeaponAttackSettings(
                GameObjectType.DaneAttack, HandWeaponAttackSettings.SwordStartScalePerc,
                WeaponScale,
                new Vector3(BoundScaleW, BoundScaleH, BoundScaleL),
                ScaleToPosDiff, 
                AttackTime,
                -2f, HandWeaponAttackSettings.SwordSwingEndAngle, 0.5f,
                    new WeaponAttack.DamageData(LfLib.HeroNormalAttack, WeaponAttack.WeaponUserType.Player,
                        user.hero.ObjOwnerAndId)
                );

            downSwing = new WeaponAttack.HandWeaponAttackSettings(
               GameObjectType.DaneAttack, HandWeaponAttackSettings.SwordStartScalePerc,
               WeaponScale,
               new Vector3(BoundScaleH, BoundScaleW, BoundScaleL),
               ScaleToPosDiff,
               600,
               -3f, 0.04f, 0.3f,
                   new WeaponAttack.DamageData(LfLib.HeroStrongAttack, WeaponAttack.WeaponUserType.Player,
                       user.hero.ObjOwnerAndId)
               );

            downSwing.HorizontalSwing = false;
            downSwing.fireParticles = true;
        }

        public override void Update(Players.InputMap controller)
        {
            base.Update(controller);

            if (superSwingWaitingForGroundHit && player.hero.heroPhysics.jumpableGround)
            { superSwingExplosion(); }
        }

        protected override void UseSpecial()
        {
            if (player.hero.isMounted)
            {
                new WeaponAttack.ItemThrow.ThrowAxe(player.hero);
            }
            else
            {
                var attack = new WeaponAttack.HandWeaponAttack2(downSwing, originalWeaponMesh1, player.hero, true);
                attack.Wep2CallbackObj = this;
                attackAnimationTime = downSwing.attackTime + 120;
                Time actionTime = new Time( downSwing.attackTime + ReloadTime);
                player.hero.setTimedMainAction(actionTime, true);
            }
            
        }


        public void OnWeaponAttackSwingStop(Vector3 particleCenter)
        {
            superSwingExplosionLocalPos = particleCenter - player.hero.Position;

            if (player.hero.heroPhysics.jumpableGround)
            {
                superSwingExplosion();
            }
            else
            {
                superSwingWaitingForGroundHit = true;
            }
        }

        bool superSwingWaitingForGroundHit = false;
        Vector3 superSwingExplosionLocalPos;
        void superSwingExplosion()
        {
            superSwingWaitingForGroundHit = false;
            Vector3 particleCenter = player.hero.Position + superSwingExplosionLocalPos;
            {
                const int ParticleCount = 16;
                Rotation1D rot = Rotation1D.Random();
                float radStep = MathHelper.TwoPi / ParticleCount;

                Graphics.ParticleInitData p = new Graphics.ParticleInitData(particleCenter);
                var particles = new List<Graphics.ParticleInitData>(ParticleCount);
                for (int i = 0; i < ParticleCount; ++i)
                {
                    p.StartSpeed = VectorExt.V2toV3XZ(rot.Direction(4));
                    particles.Add(p);
                    rot.Add(radStep);
                }
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, particles);
            }
            {
                const int ParticleCount = 32;
                Rotation1D rot = Rotation1D.Random();
                float radStep = MathHelper.TwoPi / ParticleCount;

                
                Graphics.ParticleInitData p = new Graphics.ParticleInitData();
                var particles = new List<Graphics.ParticleInitData>(ParticleCount * 4);
                for (int i = 0; i < ParticleCount; ++i)
                {
                    Vector3 dir = VectorExt.V2toV3XZ(rot.Direction(1));
                    p.StartSpeed = dir * 24f;//new Vector2toV3(rot.Direction(16));

                    p.Position = particleCenter + dir * 2f;
                    particles.Add(p);

                    p.Position = particleCenter + dir * 2.25f;
                    particles.Add(p);

                    p.Position = particleCenter + dir * 2.5f;
                    particles.Add(p);

                    p.Position = particleCenter + dir * 2.75f;
                    particles.Add(p);

                    rot.Add(radStep);
                }
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.BulletTrace, particles);

                //Music.SoundManager.PlaySound(LoadedSound.barrel_explo, particleCenter);
                const float StunRadius = 7f;
                const float StunForce = 2f;
                Effects.EffectLib.Force(LfRef.gamestate.GameObjCollection.localMembersUpdateCounter, 
                    WeaponUserType.Enemy, true, particleCenter, 1.5f, StunRadius, StunForce);
                Effects.EffectLib.VibrationCenter(particleCenter, 0.5f, 600, 10f);
                Effects.EffectLib.CameraShakeCenter(particleCenter, 0.5f);
            }

        }


        
        public override SuitType Type
        {
            get { return SuitType.BarbarianDane; }
        }
        public override SpriteName PrimaryIcon
        {
            get { return SpriteName.LFDaneIcon1; }
        }
        public override SpriteName SpecialAttackIcon
        {
            get { return SpriteName.LFDaneIcon2; }
        }
        override protected float primaryAttackTime { get { return AttackTime; } }
        override protected float primaryReloadTime { get { return ReloadTime; } }
        override protected float primaryAttackMovementPerc { get { return 0f; } }
    }
}

