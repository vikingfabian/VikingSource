using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.Creation.Weapon
{
    class SkeletonBone : ZombieProjectile
    {
        static readonly GameObjects.WeaponAttack.DamageData ProjectileDamage =
            new GameObjects.WeaponAttack.DamageData(0.5f, GameObjects.WeaponAttack.WeaponUserType.Enemy, ByteVector2.Zero);
        static readonly GameObjects.WeaponAttack.DamageData ProjectileDamageBoosted =
            new GameObjects.WeaponAttack.DamageData(1f, GameObjects.WeaponAttack.WeaponUserType.Enemy, ByteVector2.Zero);
        
        const float AimDiff = 0.1f;
        const float AimDiffBoosted = 0.06f;
        static readonly IntervalF ProjectileSpeed = new IntervalF(0.01f, 0.014f);
        static readonly IntervalF ProjectileSpeedBoosted = new IntervalF(0.016f, 0.017f);

        const float LifeTime = 4000;
        const float LifeTimeBoosted = 6000;
        bool boosted;

        public SkeletonBone(System.IO.BinaryReader r)
            : base(r)
        {
            boosted = r.ReadBoolean();
            givesDamage = boosted? ProjectileDamageBoosted : ProjectileDamage;
            lifeTime = LifeTime;
        }

        public SkeletonBone(Vector3 startPos, Vector3 target, bool boosted)
            :base(ProjectileDamage, startPos, target, (boosted? AimDiffBoosted : AimDiff),
                (boosted? ProjectileSpeedBoosted.GetRandom() : ProjectileSpeed.GetRandom()),
                (boosted? LifeTimeBoosted : LifeTime))
                
        {
            this.boosted = boosted;
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            writer.Write(boosted);
        }

        public override int UnderType
        {
            get { return (int)GameObjects.WeaponAttack.WeaponUtype.SkeletonBone; }
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.Bone; }
        }

        protected override float ImageScale
        {
            get { return 0.7f; }
        }

    }
}
