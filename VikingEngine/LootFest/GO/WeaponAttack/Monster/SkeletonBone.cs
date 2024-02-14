using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GO.Bounds;

namespace VikingEngine.LootFest.GO.WeaponAttack.Monster
{
    class LargeSkeletonBone : Linear3DProjectile
    {
        const float ProjectileSpeed = 0.018f;
        const float Scale = 4;
        const float LifeTime = 2000;

        public LargeSkeletonBone(GoArgs args, Vector3 startPos, Rotation1D dir)
            : base(args, DamageData.NoN, startPos, dir.Add(Ref.rnd.Plus_MinusF(0.1f)), createBound(Scale), ProjectileSpeed)
        {
            lifeTime = LifeTime;
            givesDamage = new DamageData(LfLib.EnemyAttackDamage,
                WeaponUserType.Enemy, this.ObjOwnerAndId);
            rotateSpeed = new Vector3(-0.14f, 0, 0);

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        
        public override void Time_Update(UpdateArgs args)
        {
            image.Rotation.RotateWorld(rotateSpeed);
            base.Time_Update(args);
        }

        
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.Bone; }
        }
        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.Enemy;
            }
        }

       // WeaponUtype bulletType;
        public override GameObjectType Type
        {
            get
            {
                return GameObjectType.LargeSkeletonBone;
            }
        }
        protected override float ImageScale
        {
            get { return Scale; }
        }
        //protected override WeaponTrophyType weaponTrophyType
        //{
        //    get { return WeaponTrophyType.Other; }
        //}
        protected override bool LocalTargetsCheck
        {
            get
            {
                return true;
            }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GO.NetworkShare.None;
            }
        }
    
    }

    class SkeletonBone: Linear3DProjectile
    {
        const float ScaleToBoundW = 0.45f;
        const float ScaleToBoundH = 0.2f;
        const float ProjectileSpeed = 0.018f;
        const float Scale = 1.8f;
        const float LifeTime = 2000;

        public SkeletonBone(GoArgs args, Vector3 startPos, Rotation1D dir)
            : base(args, DamageData.NoN, startPos, dir.Add(Ref.rnd.Plus_MinusF(0.06f)),
                new ObjectBound(BoundShape.Cylinder, new Vector3(ScaleToBoundW * Scale, ScaleToBoundH * Scale, ScaleToBoundW * Scale), Vector3.Zero), 
                ProjectileSpeed)
        {
            lifeTime = LifeTime;
            givesDamage = new DamageData(LfLib.EnemyAttackDamage,
                WeaponUserType.Enemy, this.ObjOwnerAndId);
            rotateSpeed = new Vector3(-0.14f, 0, 0);

            Velocity.Y = Ref.rnd.Plus_MinusF(ProjectileSpeed * 0.2f);
            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            image.Rotation.RotateWorld(rotateSpeed);
            base.Time_Update(args);
        }

        
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.Bone; }
        }
        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.Enemy;
            }
        }

       // WeaponUtype bulletType;
        public override GameObjectType Type
        {
            get
            {
                return GameObjectType.SkeletonBone;
            }
        }
        protected override float ImageScale
        {
            get { return Scale; }
        }

        protected override bool LocalTargetsCheck
        {
            get
            {
                return true;
            }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GO.NetworkShare.None;
            }
        }

        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.gray_15);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        protected override RecieveDamageType recieveDamageType
        {
            get
            {
                return RecieveDamageType.ReceiveDamage;
            }
        }
    }
}
