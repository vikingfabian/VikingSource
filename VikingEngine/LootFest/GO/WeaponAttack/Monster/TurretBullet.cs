using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//xna

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    class TurretBullet :Linear3DProjectile
    {
        static readonly DamageData Damage = new DamageData(LfLib.EnemyAttackDamage);
        const float ProjectileSpeed = 0.014f;
        const float BoundSize = 0.6f;
        const float LifeTime = 2500;

        public TurretBullet(Vector3 startPos, Rotation1D dir)
            : base(GoArgs.Empty, Damage, startPos, dir.Add(Ref.rnd.Plus_MinusF(0.1f)), createBound(Scale), ProjectileSpeed)
        {
            givesDamage = Damage;
            lifeTime = LifeTime;
            this.rotateSpeed = new Vector3(0.1f, 0, 0);
        }
        //public TurretBullet(System.IO.BinaryReader r)
        //    : base(r, createBound(Scale))
        //{
        //    basicInit();
        //}

        //void basicInit()
        //{
        //    givesDamage = Damage;
        //    lifeTime = LifeTime;
        //    this.rotateSpeed = new Vector3(0.1f, 0, 0);
        //}

        public override void Time_Update(UpdateArgs args)
        {
            image.Rotation.RotateWorld(rotateSpeed);
            base.Time_Update(args);
        }

        
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.EnemyProjectile; }
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
                return GameObjectType.TurretBullet;
            }
        }

        const float Scale = 1.6f;
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
    }
}
