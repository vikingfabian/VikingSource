using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    abstract class Linear3DProjectile: AbsWeapon
    {
        protected Vector3 rotateSpeed = Vector3.Zero;
        protected float lifeTime = 1500;
        //protected float heightSpeed;

        public Linear3DProjectile()
            : base(DamageData.NoN)
        {

        }

        protected void linear3DProjectileSetup(DamageData givesDamage, Vector3 startPos, Vector3 target, float aimDiffAngle, float velocity, LF2.ObjSingleBound bound)
        {
            Vector3 diff = target - startPos;
            if (aimDiffAngle > 0)
            {
                float length = diff.Length();
                length = (float)(Math.Sin(aimDiffAngle) * length);
                diff.X += Ref.rnd.Plus_MinusF(length);
                diff.Y += Ref.rnd.Plus_MinusF(length);
                diff.Z += Ref.rnd.Plus_MinusF(length);

            }
            diff = lib.SafeNormalizeV3(diff);
            diff *= velocity;

            linear3DProjectileSetup(givesDamage, startPos, Map.WorldPosition.V3toV2(diff), diff.Y, bound);
        }
        protected void linear3DProjectileSetup(DamageData damage, Vector3 startPos, Vector2 sideSpeed, float speedY, LF2.ObjSingleBound bound)
        {
            if (autoImageSetup)
                imageSetup(startPos);
            Velocity.PlaneValue = sideSpeed;
            Velocity.Y = speedY;
            CollisionBound = bound;
            rotation.Radians = Velocity.Radians(); //Rotation1D.FromDirection(Speed);
            setImageDirFromRotation();
            damage.PushDir = rotation;
            this.givesDamage = damage;
        }

        

        public Linear3DProjectile(DamageData givesDamage,
            Vector3 startPos, Vector3 target, float aimDiffAngle, LF2.ObjSingleBound bound, float startSpeed)
            : base( givesDamage, null, Vector3.Zero)
        {
            linear3DProjectileSetup(givesDamage, startPos, target, aimDiffAngle, startSpeed, bound);

        }

        public Linear3DProjectile(DamageData givesDamage,
            Vector3 startPos, Rotation1D dir, LF2.ObjSingleBound bound, float startSpeed)
            : base(givesDamage, null, Vector3.Zero)
        {
            linear3DProjectileSetup(givesDamage, startPos, dir.Direction(startSpeed), 0, bound);

        }

        public Linear3DProjectile(System.IO.BinaryReader r, LF2.ObjSingleBound bound)
            : base(r)
        {
            CollisionBound = bound;
            Vector3 pos = r.ReadVector3();
            if (autoImageSetup)
                imageSetup(pos);
            //image.Position = 
            Velocity.Read(r);
            //Speed = r.ReadVector2();
            //heightSpeed = r.ReadSingle();
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            w.Write(image.position);
            Velocity.Write(w);
            //writer.Write(Speed);
            //writer.Write(heightSpeed);
        }


        virtual protected bool autoImageSetup { get { return true; } }

        protected void imageSetup(Vector3 startPos)
        {
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelObjName, ArrowTempImage, ImageScale, 0);
            image.position = startPos;
        }
        public override void Time_Update(UpdateArgs args)
        {
            if (rotateSpeed != Vector3.Zero)
                image.Rotation.RotateWorld(rotateSpeed);
            basicUpdate(args);
        }
        public override void Time_LasyUpdate(ref float time)
        {
        }
        void basicUpdate(UpdateArgs args)
        {
            //image.Position.Y += heightSpeed * args.time;
            //image.Position.X += Speed.X * args.time;
            //image.Position.Z += Speed.Y * args.time;

            Velocity.Update(args.time, image);

            lifeTime -= args.time;
            if (lifeTime <= 0)
                DeleteMe();

            if (Ref.rnd.RandomChance(40))
            {
                Engine.ParticleHandler.AddParticles(ParticleSystemType.BulletTrace,
                        new ParticleInitData(image.position, Vector3.Zero));
            }
            UpdateBound();
            physics.Update(args.time);

            if (localMember || LocalDamageCheck)
            {
                characterCollCheck(LocalDamageCheck ? args.localMembersCounter : args.allMembersCounter);
            }
        }
        
        protected override bool removeAfterCharColl
        {
            get
            {
                return true;
            }
        }
        static readonly NetworkShare NetShare = new NetworkShare(true, false, false, false);
        override public NetworkShare NetworkShareSettings { get { return NetShare; } }
        
        protected override bool ClientPhysics
        {
            get { return true; }
        }

        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
            DeleteMe();
        }
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return GameObjects.ObjPhysicsType.Projectile;
            }
        }

        protected override NetworkClientRotationUpdateType NetRotationType
        {
            get
            {
                return NetworkClientRotationUpdateType.NoRotation;
            }
        }
        
        protected static LF2.ObjSingleBound createBound(float imagescale)
        {
            const float ScaleToBound = 0.45f;
            return LF2.ObjSingleBound.QuickCylinderBound(ScaleToBound * imagescale, ScaleToBound * imagescale);
        }

        abstract protected VoxelModelName VoxelObjName
        {
            get;
        }
        abstract protected float ImageScale
        {
            get;
        }
    }
}
