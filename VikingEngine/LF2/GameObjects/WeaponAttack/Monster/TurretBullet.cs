using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    class TurretBullet :Linear3DProjectile
    {
        static readonly DamageData Damage = new DamageData(LootfestLib.TurretBulletDamage);
        const float ProjectileSpeed = 0.014f;
        const float BoundSize = 0.6f;
        const float LifeTime = 2500;

        public TurretBullet(Vector3 startPos, Rotation1D dir)
            : base(Damage, startPos, dir.Add(Ref.rnd.Plus_MinusF(0.1f)), createBound(Scale), ProjectileSpeed)
        {
            basicInit();
        }
        public TurretBullet(System.IO.BinaryReader r)
            : base(r, createBound(Scale))
        {
            basicInit();
        }

        override protected void basicInit()
        {
            givesDamage = Damage;
            lifeTime = LifeTime;
            this.rotateSpeed = new Vector3(0.1f, 0, 0);
        }

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
        public override int UnderType
        {
            get
            {
                return (int)WeaponUtype.TurretBullet;
            }
        }

        const float Scale = 1.6f;
        protected override float ImageScale
        {
            get { return Scale; }
        }
        protected override WeaponTrophyType weaponTrophyType
        {
            get { return WeaponTrophyType.Other; }
        }
        protected override bool LocalDamageCheck
        {
            get
            {
                return true;
            }
        }
    }
}
