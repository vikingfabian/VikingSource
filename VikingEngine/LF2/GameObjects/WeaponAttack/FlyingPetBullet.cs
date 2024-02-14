using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    class FlyingPetBullet : AbsGravityProjectile
    {
        const float Velocity = 0.015f;
        Characters.FlyingPetType petType = Characters.FlyingPetType.NUM;

        public FlyingPetBullet(Vector3 startPos, Vector3 target, Characters.FlyingPetType petType)
            : base(new WeaponAttack.DamageData(LootfestLib.FlyingPetDamage, WeaponUserType.Friendly, ByteVector2.Zero), startPos, target, LF2.ObjSingleBound.QuickBoundingBox(1), Velocity)
        {
            this.petType = petType;
            
            NetworkShareObject();
            basicInit();
            image.position = startPos;
            
        }
        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            writer.Write((byte)petType);
        }
        public FlyingPetBullet(System.IO.BinaryReader r)
            : base(r)
        {
            petType = (Characters.FlyingPetType)r.ReadByte();
            basicInit();
        }

        override protected void basicInit()
        {
            imageSetup();
            Music.SoundManager.PlaySound(LoadedSound.EnemyProj1, image.position);
            setImageDirFromRotation();

            switch (petType)
            {
                case Characters.FlyingPetType.Angel:
                    particleType = Graphics.ParticleSystemType.GoldenSparkle;
                    break;
                case Characters.FlyingPetType.Dragon:
                    particleType = Graphics.ParticleSystemType.Fire;
                    break;
            }
        }

        public override int UnderType
        {
            get { return (int)WeaponUtype.FlyingPetProjectile; }
        }
        protected override VoxelModelName VoxelObjName
        {
            get
            {
                switch (petType)
                {
                    default:
                        return VoxelModelName.NUM_Empty;
                    case Characters.FlyingPetType.Angel:
                        return VoxelModelName.pet_projectile_angel;
                    case Characters.FlyingPetType.Dragon:
                        return VoxelModelName.FireBall;
                    case Characters.FlyingPetType.Falcon:
                        return VoxelModelName.slingstone;
                    case Characters.FlyingPetType.Pig:
                        return VoxelModelName.pet_projectile_pig;
                    case Characters.FlyingPetType.Bird:
                        return VoxelModelName.pet_bird_projectile;
                }
            }
        }
        protected override float ImageScale
        {
            get 
            {
                switch (petType)
                {
                    default:
                        return 0.6f;
                    case Characters.FlyingPetType.Falcon:
                        return 0.35f;
                    case Characters.FlyingPetType.Angel:
                        return 1;

                }
            }
        }

        override protected bool removeAfterCharColl { get { return true; } }
        const float Gravity = -0.00005f;
        override protected float gravity
        {
            get { return Gravity; }
        }
        protected override WeaponTrophyType weaponTrophyType
        {
            get { return WeaponTrophyType.Other; }
        }
        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return base.NetworkShareSettings;
            }
        }
    }
}
