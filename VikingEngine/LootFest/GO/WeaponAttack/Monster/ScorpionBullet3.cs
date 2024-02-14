using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.WeaponAttack.Monster
{
    class ScorpionBullet3 : Linear3DProjectile
    {
        static readonly DamageData DamageLvl1 = new DamageData(LfLib.EnemyAttackDamage, WeaponUserType.Enemy, NetworkId.Empty, Magic.MagicElement.Poision);
        const float ProjectileSpeed = 0.024f;

        const float Scale = 1f;
        const float LifeTime = 1600;

        public ScorpionBullet3(GoArgs args, Vector3 target)
            : base(args, DamageLvl1, args.startPos, target, 0.14f, createBound(Scale), ProjectileSpeed)
        {
            lifeTime = LifeTime;
            rotateSpeed = Vector3.Zero;
            

            setImageDirFromSpeed();
            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.scorpion_bullet; }
        }

        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.dark_red_orange, 
            Data.MaterialType.RGB_red, 
            Data.MaterialType.gray_10);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
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
                return GameObjectType.ScorpionBullet1;
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

    }
}
