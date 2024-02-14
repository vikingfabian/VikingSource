using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    abstract class AbsGravityProjectile : AbsWeapon
    {
        protected ParticleSystemType particleType = ParticleSystemType.BulletTrace;
         //static Debug.RealTimeFloatTweak DistToAngleAdj = new Debug.RealTimeFloatTweak("DistToAngleAdj", 0.0008f);
         //static Debug.WatchValue watchFireAngle = new Debug.WatchValue("FireAngle"); 
         //float Yspeed;    
        
        public AbsGravityProjectile(DamageData givesDamage, Vector3 startPos, Vector3 target, LF2.AbsObjBound bound, float startSpeed)
            : this(givesDamage, startPos, 
                PhysicsLib.FireAngle(startPos, target, startSpeed),
                bound)
        {
            
        }
        public AbsGravityProjectile(DamageData givesDamage, Vector3 startPos, Vector3 speed, LF2.AbsObjBound bound)
            : base(givesDamage)
        {
            Velocity = new VikingEngine.Velocity(speed); //Map.WorldPosition.V3toV2(speed);
            imageSetup();
            
            //Yspeed = speed.Y;
            givesDamage.Push = WeaponPush.Small;
            givesDamage.PushDir.Radians = Velocity.Radians();//Rotation1D.FromDirection(this.Speed);
            CollisionBound = bound;
            image.position = startPos;
            WorldPosition = new Map.WorldPosition(startPos);
#if !CMODE
            particleSetup();
#endif
        }

        
        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            w.Write(image.position);
            w.Write(Velocity.Value);
            Velocity.Write(w);
            //writer.Write(Yspeed);
        }
        public AbsGravityProjectile(System.IO.BinaryReader r)
            : base(r)
        {
            Vector3 startPos = r.ReadVector3();//måste skapa image direkt
            Velocity.Read(r);

            imageSetup();
            image.position = startPos;
            WorldPosition = new Map.WorldPosition(image.position);
            //Speed = r.ReadVector2();
            //Yspeed = r.ReadSingle();
            
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

        protected static float bowTypeToFireSpeed(Data.Gadgets.BluePrint bowType)
        {
            switch (bowType)
            {
                case Data.Gadgets.BluePrint.ShortBow:
                    return 0.034f;
                default:
                    return 0.04f;
                case Data.Gadgets.BluePrint.MetalBow:
                    return 0.044f;
            }
        }
        public override void Time_Update(UpdateArgs args)
        {
            if (localMember || LocalDamageCheck)
                characterCollCheck(LocalDamageCheck ? args.localMembersCounter : args.allMembersCounter);
            moveProjectile(args.time);
            UpdateBound();
        }

        public static Vector3 TargetRandomness(Vector3 target, float radius)
        {
            return target + Map.WorldPosition.V2toV3(Rotation1D.Random.Direction((float)lib.Rnd.NextDouble() * radius));
        }

        protected void imageSetup()
        {
            if (VoxelObjName != VoxelModelName.NUM_Empty)
                image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelObjName, ArrowTempImage, ImageScale, 0);
            setImageDirFromSpeed();
        }
        abstract protected VoxelModelName VoxelObjName
        {
            get;
        }
        abstract protected float ImageScale
        {
            get;
        }
    
        void  moveProjectile(float time)
        {

            Velocity.Update(time, image, gravity);

            if (image.position.Y < 0)
            {
                this.DeleteMe();
            }

            //smoke
            if (Ref.rnd.RandomChance(40))
            {
                Engine.ParticleHandler.AddParticles(particleType,
                    new ParticleInitData(image.position, Vector3.Zero));

#if WINDOWS
                if (localMember && (Position.X < 3 || Position.Z < 3))
                {
                    throw new Exception();
                }
#endif

                physics.Update(time);
            }

        }

        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
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
                return GameObjects.ObjPhysicsType.Projectile;
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
