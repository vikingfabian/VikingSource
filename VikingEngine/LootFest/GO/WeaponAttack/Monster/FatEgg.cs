using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    class FatEgg : Linear3DProjectile
    {
        const float Scale = 2f;
        const float ProjectileSpeed = 0.025f;

        public FatEgg(Vector3 startPos, Vector3 target)
            : base(new DamageData(LfLib.EnemyAttackDamage, WeaponUserType.Enemy, NetworkId.Empty),
            startPos, target, 0.06f, createBound(Scale), ProjectileSpeed)
        {
            //image.Rotation.PointAlongVector(Velocity.Value);
        }

        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.fat_egg; }
        }

        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            base.HandleColl3D(collData, ObjCollision);
            popEgg();
        }

        protected override void onHitCharacter(AbsUpdateObj character)
        {
            base.onHitCharacter(character);
            popEgg();
        }

        void popEgg()
        {
            BlockSplatter();
            var args = new GoArgs();
            args.startWp = new Map.WorldPosition(image.position);
            var bird = new GO.Characters.FatBird(args);
            bird.stunForce(0.6f, 0f, false, true);
        }

        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.pale_warm_brown,
            Data.MaterialType.pale_cool_brown);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        protected override float ImageScale
        {
            get { return Scale; }
        }

        //protected override WeaponTrophyType weaponTrophyType
        //{
        //    get { throw new NotImplementedException(); }
        //}

        public override GameObjectType Type
        {
            get
            {
                return GameObjectType.FatEgg;
            }
        }
    }
}
