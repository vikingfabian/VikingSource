using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    abstract class AbsGravityProjectile : AbsWeapon
    {
        protected ParticleSystemType particleType = ParticleSystemType.BulletTrace;
        static DebugExtensions.RealTimeFloatTweak DistToAngleAdj = new DebugExtensions.RealTimeFloatTweak("DistToAngleAdj", 0.0008f);
        static DebugExtensions.WatchValue watchFireAngle = new DebugExtensions.WatchValue("FireAngle"); 
         //float Yspeed;    
        protected bool rotationFollowSpeed = true;
        protected Time ignoreTerrainCollTimer = Time.Zero;

        public AbsGravityProjectile(DamageData givesDamage, Vector3 startPos, Vector3 target, GO.Bounds.ObjectBound bound, float startSpeed)
            : this(GoArgs.Empty, givesDamage, startPos, target, bound, startSpeed)
        {

        }

        public AbsGravityProjectile(GoArgs args, DamageData givesDamage, Vector3 startPos, Vector3 target, GO.Bounds.ObjectBound bound, float startSpeed)
            : this(args, givesDamage, startPos, 
                PhysicsLib.FireAngle(startPos, target, startSpeed),
                bound)
        {
            
        }
        public AbsGravityProjectile(GoArgs args, DamageData givesDamage, Vector3 startPos, Vector3 speed, GO.Bounds.ObjectBound bound)
            : base(args, givesDamage, null, Vector3.Zero)
        {
            characterLevel = args.characterLevel;

            if (VoxelObjName != VoxelModelName.NUM_NON)
            {
                bool center = false;
                if (VoxelObjName == VoxelModelName.throw_axe || 
                    VoxelObjName == VoxelModelName.enemy_projectile_green || 
                    VoxelObjName == VoxelModelName.EnemyProjectile)
                {
                    center = true;
                }
                image = LfRef.modelLoad.AutoLoadModelInstance(VoxelObjName, ImageScale, 0, center);
            }

            CollisionAndDefaultBound = bound;

            if (args.LocalMember)
            {
                Velocity = new VikingEngine.Velocity(speed); //VectorExt.V3XZtoV2(speed);
               // imageSetup();

                //Yspeed = speed.Y;
                givesDamage.Push = WeaponPush.Small;
                givesDamage.PushDir.Radians = Velocity.Radians();//Rotation1D.FromDirection(this.Speed);
                
                image.position = startPos;
                WorldPos = new Map.WorldPosition(startPos);

                particleSetup();
            }
            else
            {
                startPos = SaveLib.ReadVector3(args.reader);//r.ReadVector3();//måste skapa image direkt
                Velocity.Read(args.reader);

                //imageSetup();
                image.position = startPos;
                WorldPos = new Map.WorldPosition(image.position);
            }


            setImageDirFromSpeed();
        }

        
        public override void netWriteGameObject(System.IO.BinaryWriter w)
        {
            base.netWriteGameObject(w);
            SaveLib.WriteVector( w, image.position);
           SaveLib.WriteVector( w,Velocity.Value);
            Velocity.Write(w);
            //writer.Write(Yspeed);
        }
        

        protected void particleSetup()
        {
            switch (givesDamage.Magic)
            {
                case Magic.MagicElement.Evil:
                    particleType = ParticleSystemType.Smoke;
                    break;
                case Magic.MagicElement.Fire:
                    particleType = ParticleSystemType.Fire;
                    break;
                case Magic.MagicElement.Lightning:
                    particleType = ParticleSystemType.LightSparks;
                    break;
                case Magic.MagicElement.Poision:
                    particleType = ParticleSystemType.Poision;
                    break;

            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (bCollisionCheck)
            {
                if (localMember || LocalTargetsCheck)
                {
                    characterCollCheck(LocalTargetsCheck ? args.localMembersCounter : args.allMembersCounter);
                }
            }
            moveProjectile(args.time);
            UpdateBound();

            if (rotationFollowSpeed)
            {
                image.Rotation.PointAlongVector(Velocity.Value);
            }
        }

        public static Vector3 TargetRandomness(Vector3 target, float radius)
        {
            return target + VectorExt.V2toV3XZ(Rotation1D.Random().Direction((float)Ref.rnd.Double() * radius));
        }

        abstract protected VoxelModelName VoxelObjName
        {
            get;
        }
        abstract protected float ImageScale
        {
            get;
        }
    
        void moveProjectile(float time)
        {

            Velocity.Update(time, image, gravity);

            if (image.position.Y < 0)
            {
                this.DeleteMe();
            }

            if (Ref.TimePassed16ms)
            {
                //smoke
                if (Ref.rnd.Chance(40))
                {
                    Engine.ParticleHandler.AddParticles(particleType,
                        new ParticleInitData(image.position, Vector3.Zero));
                }

                if (ignoreTerrainCollTimer.CountDown(Ref.UpdateTimes30FPS))
                {
                    physics.Update(time);
                }
            }
        }

        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            DeleteMe();
        }

        const float StandardProjectileGravity = -0.00006f;
        virtual protected float gravity
        {
            get { return StandardProjectileGravity; }
        }
        public override void Time_LasyUpdate(ref float time)
        {
            if (image.position.Y < 2)
            {
                DeleteMe();
            }
        }
           
        
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return GO.ObjPhysicsType.Projectile;
            }
        }
        static readonly NetworkShare NetShare = new NetworkShare(true, false, false, false);
        override public NetworkShare NetworkShareSettings { get { return NetShare; } }
        
        protected override bool ClientPhysics
        {
            get { return true; }
        }
    }
    
}
