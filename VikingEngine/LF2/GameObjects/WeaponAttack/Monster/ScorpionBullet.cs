using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    class ScorpionBullet: Linear3DProjectile
    {
        static readonly DamageData DamageLvl1 = new DamageData(LootfestLib.ScorpionBulletDamage, WeaponUserType.Enemy, ByteVector2.Zero, Gadgets.GoodsType.NONE, Magic.MagicElement.Poision);
        static readonly DamageData DamageLvl2 = new DamageData(LootfestLib.ScorpionBulletDamage * LootfestLib.Level2DamageMultiply, WeaponUserType.Enemy, ByteVector2.Zero, Gadgets.GoodsType.NONE, Magic.MagicElement.Poision);
        const float ProjectileSpeed = 0.016f;
        
        WeaponUtype type;
        const float Scale = 1f;
        const float LifeTime = 1600;

        public ScorpionBullet(Vector3 startPos,  Vector3 target, int level)
            : base(level == 0 ? DamageLvl1 : DamageLvl2, startPos, target, 0.2f, createBound(Scale), ProjectileSpeed)
        {
            basicInit(level == 0 ? WeaponUtype.ScorpionBullet1 : WeaponUtype.ScorpionBullet2);
            NetworkShareObject();
        }
        public ScorpionBullet(System.IO.BinaryReader r, WeaponUtype type)
            : base(r, LF2.ObjSingleBound.QuickBoundingBox(Scale))
        {
            basicInit(type);
        }
        void basicInit(WeaponUtype type)
        {
            this.type = type;
            lifeTime = LifeTime;
            rotateSpeed = new Vector3(0.1f, 0, 0);
            givesDamage = type == WeaponUtype.ScorpionBullet1 ? DamageLvl1 : DamageLvl2;
        }

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
        public override int UnderType
        {
            get
            {
                return (int)type;
            }
        }

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
