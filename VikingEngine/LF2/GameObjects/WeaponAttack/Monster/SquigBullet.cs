using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    
    class SquigBullet : Linear3DProjectile
    {
        
        const float ProjectileSpeed = 0.014f;
        const float BoundSize = 0.6f;
        const float LifeTime = 2000;

        public SquigBullet(Vector3 startPos, Rotation1D dir, WeaponUtype bulletType)
            : base(DamageData.NoN, startPos, dir.Add(Ref.rnd.Plus_MinusF(0.1f)), createBound(Scale(bulletType)), ProjectileSpeed)
        {
            basicInit(bulletType);
        }
        //public SquigBullet(System.IO.BinaryReader r, WeaponUtype bulletType)
        //    : base(r, createBound(Scale(bulletType)))
        //{
        //    basicInit(bulletType);
        //}

        void basicInit(WeaponUtype bulletType)
        {
            this.bulletType = bulletType;
            lifeTime = LifeTime;
            givesDamage = new DamageData((bulletType == WeaponUtype.SquigBullet? 
                LootfestLib.SquigBulletDamage : LootfestLib.SquigSpawnBulletDamage), 
                WeaponUserType.Enemy, this.ObjOwnerAndId);
            rotateSpeed = new Vector3(0.1f, 0, 0);
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

        WeaponUtype bulletType;
        public override int UnderType
        {
            get
            {
                return (int)bulletType;//(int)WeaponUtype.SquigBullet;
            }
        }
        static float Scale(WeaponUtype bulletType) 
        {
            return bulletType == WeaponUtype.SquigBullet ? 0.8f : 0.5f;  
        }
        protected override float ImageScale
        {
            get { return Scale(bulletType); }
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

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GameObjects.NetworkShare.None;
            }
        }

        //public override bool NetShareSett_Creation
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}
        //public override bool NetShareSett_DeleteByClient
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}
        //public override bool NetShareSett_DeleteByHost
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}
    }
}
