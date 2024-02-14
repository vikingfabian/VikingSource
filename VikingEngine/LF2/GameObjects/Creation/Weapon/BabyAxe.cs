using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.Creation.Weapon
{
    class BabyAxe : ZombieProjectile
    {
        static readonly GameObjects.WeaponAttack.DamageData ProjectileDamage =
            new GameObjects.WeaponAttack.DamageData(1.5f, GameObjects.WeaponAttack.WeaponUserType.Enemy, ByteVector2.Zero);
        
        
        const float AimDiff = 0.1f;
       // byte atRange;
        static readonly IntervalF ProjectileSpeed = new IntervalF(0.01f, 0.014f);
       // static readonly RangeF ProjectileSpeedBoosted = new RangeF(0.016f, 0.017f);

        //VoxelModelName.babyaxe, 4000
        const float LifeTime = 4000;


        public BabyAxe(System.IO.BinaryReader r)
            : base(r)
        {
            givesDamage = ProjectileDamage;
            lifeTime = LifeTime;
        }

        public BabyAxe(Vector3 startPos, Vector3 target) //, bool boosted)
            :base(ProjectileDamage, startPos, target, AimDiff,
                (ProjectileSpeed.GetRandom()),
                LifeTime)
                
        {
            //atRange = randomPercent.ByteVal;
            
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
           // writer.Write(atRange);
        }

        public override int UnderType
        {
            get { return (int)GameObjects.WeaponAttack.WeaponUtype.BabyAxe; }
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.babyaxe; }
        }

        //static readonly Vector3 ProjectileScale = lib.V3(0.08f);
        protected override float ImageScale
        {
            get { return 0.7f; }
        }
    }

   
}
