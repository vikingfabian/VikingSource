using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.WeaponAttack.Monster
{
    class SpitChickBullet : Linear3DProjectile
    {
        const float ProjectileSpeed = 0.020f;
        public const float Scale = 1.5f;
        const float LifeTime = 1600;

        public SpitChickBullet(GoArgs args, Vector3 target)
            : base(args, new DamageData(LfLib.EnemyAttackDamage, WeaponUserType.Enemy, NetworkId.Empty),
                args.startPos, target, 0.05f, null, ProjectileSpeed)
        {
            
            lifeTime = LifeTime;
            rotateSpeed = new Vector3(0.1f, 0.05f, 0);

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }
       
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.spitchick_bullet; }
        }
        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.Enemy;
            }
        }

        public override GameObjectType Type
        {
            get
            {
                return GameObjectType.SpitChickBullet;
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
        public override System.IO.FileShare BoundSaveAccess
        {
            get
            {
                return System.IO.FileShare.ReadWrite;
            }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GO.NetworkShare.OnlyCreation;
            }
        }


        
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.CMYK_yellow,
            Data.MaterialType.pure_yellow_green);

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
