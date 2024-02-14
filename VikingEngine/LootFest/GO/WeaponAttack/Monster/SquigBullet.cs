using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//xna

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    
    class SquigBullet : Linear3DProjectile
    {
        const float ProjectileSpeed = 0.014f;
        const float LifeTime = 2000;

        public SquigBullet(Vector3 startPos, Rotation1D dir, GameObjectType bulletType)
            : base( GoArgs.Empty, DamageData.NoN, startPos, dir.Add(Ref.rnd.Plus_MinusF(0.1f)), 
            createBound(Scale(bulletType)), ProjectileSpeed)
        {
            basicInit(bulletType);
        }

        void basicInit(GameObjectType bulletType)
        {
            this.bulletType = bulletType;
            lifeTime = LifeTime;
            givesDamage = new DamageData(LfLib.EnemyAttackDamage, 
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

        GameObjectType bulletType;
        public override GameObjectType Type
        {
            get
            {
                return bulletType;
            }
        }
        static float Scale(GameObjectType bulletType) 
        {
            return bulletType == GameObjectType.SquigBullet ? 0.8f : 0.5f;  
        }
        protected override float ImageScale
        {
            get { return Scale(bulletType); }
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
    }
}
