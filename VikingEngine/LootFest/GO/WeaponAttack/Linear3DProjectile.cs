using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//xna

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    abstract class Linear3DProjectile: AbsWeapon
    {
        protected Vector3 rotateSpeed = Vector3.Zero;
        protected float lifeTime = 1500;
        //protected float heightSpeed;

        public Linear3DProjectile(GoArgs args)
            : base(args)
        {
           
        }

        protected void linear3DProjectileSetup(GoArgs args, DamageData givesDamage, Vector3 startPos, Vector3 target,
            float aimDiffAngle, float velocity, GO.Bounds.ObjectBound bound)
        {
            Vector3 diff = Vector3.Zero;
            if (args.LocalMember)
            {
                diff = target - startPos;
                if (aimDiffAngle > 0)
                {
                    float length = diff.Length();
                    length = (float)(Math.Sin(aimDiffAngle) * length);
                    diff.X += Ref.rnd.Plus_MinusF(length);
                    diff.Y += Ref.rnd.Plus_MinusF(length);
                    diff.Z += Ref.rnd.Plus_MinusF(length);

                }
                diff = VectorExt.SafeNormalizeV3(diff);

                //fel
                image.Rotation.QuadRotation = Quaternion.CreateFromAxisAngle(diff, 0f);
                diff *= velocity;
            }
            linear3DProjectileSetup(args, givesDamage, startPos, VectorExt.V3XZtoV2(diff), diff.Y, bound);
        }
        protected void linear3DProjectileSetup(GoArgs args, DamageData damage, Vector3 startPos, Vector2 sideSpeed, 
            float speedY, GO.Bounds.ObjectBound bound)
        {
            if (autoImageSetup)
                imageSetup(startPos);

            if (args.LocalMember)
            {
                Velocity.PlaneValue = sideSpeed;
                Velocity.Y = speedY;
            }

            if (BoundSaveAccess == System.IO.FileShare.ReadWrite)
            {
                modelScale = ImageScale;
                loadBounds();
            }
            else
            {
                CollisionAndDefaultBound = bound;
            }
            rotation.Radians = Velocity.Radians();
            damage.PushDir = rotation;
            rotation.Add(MathHelper.Pi);

            this.givesDamage = damage;
        }

        public Linear3DProjectile(DamageData givesDamage,
            Vector3 startPos, Vector3 target, float aimDiffAngle, GO.Bounds.ObjectBound bound, float startSpeed)
            : this(GoArgs.Empty, givesDamage, startPos, target, aimDiffAngle, bound, startSpeed)
        { }

        public Linear3DProjectile(GoArgs args, DamageData givesDamage,
            Vector3 startPos, Vector3 target, float aimDiffAngle, GO.Bounds.ObjectBound bound, float startSpeed)
            : base(args, givesDamage, null, Vector3.Zero)
        {

            if (!args.LocalMember)
            {
                startPos = SaveLib.ReadVector3(args.reader);
                Velocity.Read(args.reader);
            }
            linear3DProjectileSetup(args, givesDamage, startPos, target, aimDiffAngle, startSpeed, bound);

        }

        public Linear3DProjectile(GoArgs args, DamageData givesDamage,
            Vector3 startPos, Rotation1D dir, GO.Bounds.ObjectBound bound, float startSpeed)
            : base(args, givesDamage, null, Vector3.Zero)
        {
            if (args.LocalMember)
            {
                givesDamage.Push = WeaponPush.Small;
                //givesDamage.PushDir = dir;
                givesDamage.PushDir.Add(MathHelper.Pi);
                linear3DProjectileSetup(args, givesDamage, startPos, dir.Direction(startSpeed), 0, bound);
                setImageDirFromRotation();
            }
            else
            {
                CollisionAndDefaultBound = bound;
                Vector3 pos = SaveLib.ReadVector3(args.reader);//r.ReadVector3();
                if (autoImageSetup)
                    imageSetup(pos);
                //image.Position = 
                Velocity.Read(args.reader);
            }
        }

        //public Linear3DProjectile(System.IO.BinaryReader r, GO.Bounds.ObjectBound bound)
        //    : base(r)
        //{
            
        //    //Speed = r.ReadVector2();
        //    //heightSpeed = r.ReadSingle();
        //}

        public override void netWriteGameObject(System.IO.BinaryWriter w)
        {
            base.netWriteGameObject(w);
            SaveLib.WriteVector(w, image.position);
            Velocity.Write(w);
            //writer.Write(Speed);
            //writer.Write(heightSpeed);
        }


        virtual protected bool autoImageSetup { get { return true; } }

        protected void imageSetup(Vector3 startPos)
        {
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelObjName, ImageScale, 0, true);
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
            Velocity.Update(args.time, image);

            lifeTime -= args.time;
            if (lifeTime <= 0)
                DeleteMe();

            if (Ref.rnd.Chance(40))
            {
                Engine.ParticleHandler.AddParticles(ParticleSystemType.BulletTrace,
                        new ParticleInitData(image.position, Vector3.Zero));
            }
            UpdateBound();
            physics.Update(args.time);

            if (localMember || LocalTargetsCheck)
            {
                characterCollCheck(LocalTargetsCheck ? args.localMembersCounter : args.allMembersCounter);
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

        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            DeleteMe();
        }
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return GO.ObjPhysicsType.Projectile;
            }
        }

        protected override NetworkClientRotationUpdateType NetRotationType
        {
            get
            {
                return NetworkClientRotationUpdateType.NoRotation;
            }
        }

        protected static GO.Bounds.ObjectBound createBound(float imagescale)
        {
            const float ScaleToBound = 0.45f;
            return LootFest.ObjSingleBound.QuickCylinderBound(ScaleToBound * imagescale, ScaleToBound * imagescale);
        }

        abstract protected VoxelModelName VoxelObjName
        {
            get;
        }
        abstract protected float ImageScale
        {
            get;
        }

        public override float Scale1D
        {
            get
            {
                return base.Scale1D;
            }
        }

        
    }
}
