using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.WeaponAttack.Monster
{
    class SquigBullet2: AbsGravityProjectile
    {
        public const int BulletVersionOneShot = 0;
        public const int BulletVersionMultiSplit = 1;
        public const int BulletVersionMulti = 2;

        const float ProjectileSpeed = 0.032f;
        const float scaleToBoundSize = 0.3f;


        public SquigBullet2(GoArgs args, Vector3 target)
            : base(args, new DamageData(1f), args.startPos, target,
            new GO.Bounds.ObjectBound(Bounds.BoundShape.BoundingBox, args.startPos, new Vector3(SquigBulletScale(args.characterLevel) * scaleToBoundSize), Vector3.Zero), 
            ProjectileSpeed)
        {
            rotationFollowSpeed = false;

            if (args.LocalMember)
            {
                NetworkShareObject();
            }

            physics.Gravity *= 0.7f;

            if (args.characterLevel == BulletVersionMultiSplit)
            {
                this.ignoreTerrainCollTimer.MilliSeconds = 400;
            }
        }


        override protected void hitShield(Vector3 shieldPos, Rotation1D heroDir)
        {
            for (int i = 0; i < 5; i++)
            {
                new Effects.BouncingBlock2(shieldPos, DamageColors.GetRandom(), 0.16f, heroDir);
            }
        }

        public override void HandleColl3D(Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            base.HandleColl3D(collData, ObjCollision);

            if (characterLevel == BulletVersionMulti)
            { //Create four new bullets
                var splitRot = Velocity.Rotation1D();
                splitRot.Add(MathHelper.PiOver4);

                Vector3 pos = image.position;
                pos.Y += 0.6f;

                for (int i = 0; i < 4; ++i)
                {
                    new SquigBullet2(new GoArgs(pos, BulletVersionMultiSplit), pos + VectorExt.V2toV3XZ(splitRot.Direction(2f), 1f));
                    splitRot.Add(MathHelper.PiOver2);
                }
            }
        }


        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return characterLevel == 0? EnemyProjGreenDamageCols : EnemyProjRedDamageCols;
            }
        }

        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponUserType.Enemy;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SquigBullet; }
        }

        protected override VoxelModelName VoxelObjName
        {
            get { return characterLevel == 0? VoxelModelName.enemy_projectile_green : VoxelModelName.EnemyProjectile; }
        }
        protected override float ImageScale
        {
            get { return SquigBulletScale(characterLevel); }
        }

        static float SquigBulletScale(int level)
        {
            switch (level)
            {
                case BulletVersionOneShot:
                    return 2f;
                case BulletVersionMulti:
                    return 3f;
                case BulletVersionMultiSplit:
                    return 1.5f;
            }

            throw new NotImplementedException();
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }

        

        protected override bool LocalTargetsCheck
        {
            get
            {
                return true;
            }
        }

        protected override RecieveDamageType recieveDamageType
        {
            get
            {
                return RecieveDamageType.ReceiveDamage;
            }
        }
       
    }
}