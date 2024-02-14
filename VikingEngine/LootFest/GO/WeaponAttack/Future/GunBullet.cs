using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.WeaponAttack.Future
{
    abstract class GunBullet : GO.WeaponAttack.Linear3DProjectile
    {
        //public const float LifeTime = 800;
        
        public GunBullet(GO.Characters.AbsCharacter character, Vector3 offset, float aimDiffAngle, float speed, float lifeTime, Bounds.ObjectBound bound)
            : base(GoArgs.Empty ,new GO.WeaponAttack.DamageData(LfLib.HeroNormalAttack, GO.WeaponAttack.WeaponUserType.Player, character.ObjOwnerAndId),
                lib.RotatePosDiffAroundCenter(character.Position, offset, character.FireDir(GameObjectType.MashineGunBullet).Radians), character.FireDir(GameObjectType.MashineGunBullet) + Ref.rnd.Plus_MinusF(aimDiffAngle), bound, speed)
        {
            this.lifeTime = lifeTime;

            const float PercPlayerSpeedAdd = 0.6f;
            Velocity.Value.X += character.Velocity.Value.X * PercPlayerSpeedAdd;
            Velocity.Value.Z += character.Velocity.Value.Z * PercPlayerSpeedAdd;

        }
        protected override float adjustRotation
        {
            get
            {
                return 0;
            }
        }

        public override System.IO.FileShare BoundSaveAccess
        {
            get
            {
                return System.IO.FileShare.None;
            }
        }

        //protected override WeaponTrophyType weaponTrophyType
        //{
        //    get { return WeaponTrophyType.Arrow_Slingstone; }
        //}
    
    }

    class SideArmBullet : GunBullet
    {
        const float Scale = 0.6f;

        public SideArmBullet(GO.Characters.AbsCharacter character)
            : base(character, new Vector3(0.8f, 0.4f, 1.8f), 0f, 0.016f, 500,
                new Bounds.ObjectBound(Bounds.BoundShape.Cylinder, Vector3.Zero, new Vector3(Scale * 0.5f, Scale, 0), Vector3.Zero))
        {

        }

        protected override float ImageScale
        {
            get { return Scale; }
        }

        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.gunbullet; }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SideArmBullet; }
        }
    }

    class MashineGunBullet : GunBullet
    {
        const float Scale = 0.8f;

        public MashineGunBullet(GO.Characters.AbsCharacter character)
            : base(character, new Vector3(0.8f, 0.4f, 2f), 0.12f, 0.016f, 2000, 
                new Bounds.ObjectBound(Bounds.BoundShape.Cylinder, Vector3.Zero, new Vector3(Scale * 0.5f, Scale, 0), Vector3.Zero))
        {

        }

        protected override float ImageScale
        {
            get { return Scale; }
        }

        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.gunbullet; }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.MashineGunBullet; }
        }
    }

    
}


