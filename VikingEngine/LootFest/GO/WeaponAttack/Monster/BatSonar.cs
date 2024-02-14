using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.WeaponAttack.Monster
{
    class BatSonar : Linear3DProjectile
    {
        const float ProjectileSpeed = 0.018f;
        float scaleUpTime = 0;

        public BatSonar(GoArgs args, Vector3 startPos, Vector3 target)
            : base(args, new DamageData(0f, WeaponUserType.Enemy, NetworkId.Empty, Magic.MagicElement.Stunn), 
                startPos, target, 0f, 
                ObjSingleBound.QuickRectangleRotated2(Vector3.Zero), ProjectileSpeed)
        {
            image.Rotation.PointAlongVector(Velocity.Value);
            image.Scale1D = 0;
            lifeTime = 2000;

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            scaleUpTime += args.time;
            float scale = scaleUpTime * 0.00018f + 0.1f;
            image.Scale1D = scale;

            CollisionAndDefaultBound.MainBound.halfSize = new Vector3(7.8f * scale, 0.3f, 3f * scale);
            CollisionAndDefaultBound.MainBound.refreshBound();

            //VectorVolume boundVol = CollisionAndDefaultBound.MainBound.outerBound;
            //boundVol.HalfSize = new Vector3(7.8f * scale, 0.3f, 3f * scale);
            //CollisionAndDefaultBound.MainBound.Bound.CenterScale = boundVol;

            base.Time_Update(args);
        }

        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.bat_sonar; }
        }

        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.Enemy;
            }
        }

        protected override float ImageScale
        {
            get { return 0f; }
        }

        public override GameObjectType Type
        {
            get
            {
                return GameObjectType.BatSonar;
            }
        }

        public override bool SolidBody
        {
            get
            {
                return false;
            }
        }
        //protected override WeaponTrophyType weaponTrophyType
        //{
        //    get { return WeaponTrophyType.Other; }
        //}

        protected override bool removeAfterCharColl
        {
            get
            {
                return false;
            }
        }
    }
}
