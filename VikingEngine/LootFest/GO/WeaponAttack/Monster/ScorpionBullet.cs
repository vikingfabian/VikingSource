using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//xna

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    class ScorpionBullet: Linear3DProjectile
    {
        static readonly DamageData DamageLvl1 = new DamageData(LfLib.EnemyAttackDamage, WeaponUserType.Enemy, NetworkId.Empty, Magic.MagicElement.Poision);
        //static readonly DamageData DamageLvl2 = new DamageData(LootfestLib.ScorpionBulletDamage * LootfestLib.Level2DamageMultiply, WeaponUserType.Enemy, ByteVector2.Zero, Magic.MagicElement.Poision);
        const float ProjectileSpeed = 0.016f;
        
        GameObjectType type;
        const float Scale = 1f;
        const float LifeTime = 1600;

        public ScorpionBullet(GoArgs args, Vector3 startPos, Vector3 target)
            : base(args, DamageLvl1, startPos, target, 0.2f, createBound(Scale), ProjectileSpeed)
        {
            this.type = args.characterLevel == 0 ? GameObjectType.ScorpionBullet1 : GameObjectType.ScorpionBullet2;
            lifeTime = LifeTime;
            rotateSpeed = new Vector3(0.1f, 0, 0);
            givesDamage = DamageLvl1;
            // basicInit(args.characterLevel == 0 ? GameObjectType.ScorpionBullet1 : GameObjectType.ScorpionBullet2);
            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }
        //public ScorpionBullet(System.IO.BinaryReader r, GameObjectType type)
        //    : base(r, LootFest.ObjSingleBound.QuickBoundingBox(Scale))
        //{
        //    basicInit(type);
        //}
        //void basicInit(GameObjectType type)
        //{
            
        //}

        public override void Time_Update(UpdateArgs args)
        {
            image.Rotation.RotateWorld(rotateSpeed);
            base.Time_Update(args);
        }

        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.enemy_projectile_green; }
        }
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return EnemyProjGreenDamageCols;
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
                return type;
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
    }
}
