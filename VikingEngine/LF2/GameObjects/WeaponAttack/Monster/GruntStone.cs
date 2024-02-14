using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    class GruntStone : EnemyGravityProjectile
    {
        static readonly DamageData Damage = new DamageData(LootfestLib.GruntStoneDamage, WeaponUserType.Enemy, ByteVector2.Zero);
        const float BoundScale = 1;

        public GruntStone(Characters.AbsCharacter target,  Vector3 startPos)
            : base(Damage, startPos, target.Position, LF2.ObjSingleBound.QuickBoundingBox(BoundScale))
        {
            NetworkShareObject();
        }

        public GruntStone(System.IO.BinaryReader r)
            : base(r)
        {
            CollisionBound = LF2.ObjSingleBound.QuickBoundingBox(BoundScale);
            givesDamage = Damage;
        }

        const float Scale =  0.6f;
        protected override float ImageScale
        {
            get
            {
                return Scale;
            }
        }
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
        }
        protected override VoxelModelName VoxelObjName
        {
            get
            {
                return VoxelModelName.slingstone;
            }
        }
        public override int UnderType
        {
            get { return (int)WeaponUtype.GruntStone; }
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
