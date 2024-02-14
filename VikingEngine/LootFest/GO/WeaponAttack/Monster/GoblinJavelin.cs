using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.WeaponAttack.Monster
{
    class GoblinJavelin: Linear3DProjectile
    {
        const float ProjectileSpeed = 0.024f;
        public const float Scale = 4;
        const float BoundSize = Scale * 0.1f;
        const float LifeTime = 2000;

        public GoblinJavelin(GoArgs args, Vector3 startPos, Vector3 target)
            : base(args, new DamageData(LfLib.EnemyAttackDamage, WeaponUserType.Enemy, NetworkId.Empty),
            startPos, target, 0.06f,
            ObjSingleBound.QuickBoundingBox(new Vector3(BoundSize), new Vector3(0, BoundSize * 0.5f, Scale * 0.45f)), ProjectileSpeed)
        {
            lifeTime = LifeTime;
            CollisionAndDefaultBound.use3DRotationOffset = true;
            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }
        
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            image.Rotation.PointAlongVector(Velocity.Value);
        }

       
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.goblin_spear; }
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
                return GameObjectType.GoblinJavelin;
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
                return GO.NetworkShare.OnlyCreation;
            }
        }
    }
}