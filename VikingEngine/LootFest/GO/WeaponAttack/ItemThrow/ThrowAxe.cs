using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.WeaponAttack.ItemThrow
{
    class ThrowAxe : AbsGravityProjectile
    {
        const float Scale = 2f;
        Vector3 rotate;

        public ThrowAxe(VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero)
            : base(GoArgs.Empty, new DamageData(LfLib.BasicDamage, WeaponUserType.Friendly, NetworkId.Empty), hero.FireProjectilePosition, StartSpeed(hero),
            new Bounds.ObjectBound(Bounds.BoundShape.BoundingBox, new Vector3(Scale * 0.5f), Vector3.Zero))
        {
            rotationFollowSpeed = false;
            rotate = new Vector3(0, 10f * (float)Ref.main.TargetElapsedTime.TotalSeconds, 0f);
        }

        static Vector3 StartSpeed(VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero)
        {
            Vector3 speed = hero.Velocity.Value * 0.6f;
            speed += VectorExt.V2toV3XZ(hero.FireDir(GameObjectType.ThrowAxe).Direction(0.012f), 0.024f);
            return speed;
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            image.Rotation.RotateAxis(rotate);
        }

        protected override float ImageScale
        {
            get { return Scale; }
        }
        
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.throw_axe; }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.ThrowAxe; }
        }
    }
}
