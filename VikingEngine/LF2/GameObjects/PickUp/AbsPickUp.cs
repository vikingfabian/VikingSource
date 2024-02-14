using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.GameObjects.PickUp
{
    abstract class AbsPickUp : AbsVoxelObj
    {
        protected static readonly Data.TempBlockReplacementSett BoneTempImage = new Data.TempBlockReplacementSett(new Color(239, 236, 212), new Vector3(0.3f, 0.3f, 1.5f));
        protected static readonly Data.TempBlockReplacementSett AppleTempImage = new Data.TempBlockReplacementSett(new Color(255, 65, 27), new Vector3(1));
        protected float pickUpBlockTime = 400;

        public AbsPickUp(Vector3 position)
            : base()
        {
            if (imageType != VoxelModelName.NUM_Empty)
            {
                startSetup(position);
            }
        }
        public AbsPickUp(System.IO.BinaryReader r)
            : base(r)
        {
            pickupInit();
        }
       
        protected void startSetup(Vector3 position)
        {
            WorldPosition = new Map.WorldPosition(position);
            position.Y = LfRef.chunks.GetScreen(WorldPosition).GetGroundY(WorldPosition) + 2f;
            WorldPosition = new Map.WorldPosition(position);

            //if (imageType != VoxelModelName.NUM_Empty)
            //{
            //    CollisionBound = LootFest.ObjSingleBound.QuickCylinderBound(1f, 0.6f);
            //    image = LootFest.Data.ImageAutoLoad.AutoLoadImgInstace(imageType, tempImage, imageScale, 0);
            //    rotation = Rotation1D.Random();
            //}
            pickupInit();
            
            if (image != null)
            {
                image.position = position;
            }
            Velocity = new VikingEngine.Velocity(Rotation1D.Random, Effects.EffectLib.PickUpStartSideSpeed.GetRandom());
            physics.SpeedY = Effects.EffectLib.StandardStartUpSpeed.GetRandom();
        }

        protected void pickupInit()
        {
            if (imageType != VoxelModelName.NUM_Empty)
            {
                CollisionBound = LF2.ObjSingleBound.QuickCylinderBound(1f, 0.6f);
                image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(imageType, tempImage, imageScale, 0);
                rotation = Rotation1D.Random();
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (localMember)
            {
                if (checkOutsideUpdateArea_ActiveChunk())
                {
                    DeleteMe();
                }
                else
                {
                    physics.Update(args.time);

                }
            }
            else
            {
                base.Time_Update(args);
            }

            pickUpBlockTime -= args.time;

            if (pickUpBlockTime <= 0)
            {
                checkPickup();
                if (timedRemoval && -pickUpBlockTime > pickUpLifeTime)
                {
                    image.scale.Y *= 0.95f;
                    image.scale.X = image.scale.Y;
                    image.scale.Z = image.scale.Y;
                    image.position.Y -= 0.002f * args.time;
                    if (image.scale.Y <= 0.05f)
                    {
                        timedRemovalEvent();
                        DeleteMe();
                    }
                    
                }
            }
        }

        virtual protected void timedRemovalEvent() { } //do nothing 
        virtual protected void checkPickup() { } //do nothing 

        public override void Force(Vector3 center, float force)
        {
            Vector3 diff = image.position - center;
            float l = diff.Length();
            force -= l * 0.04f;
            if (force > 0.05f && physics != null)
            {
                diff = lib.SafeNormalizeV3(diff) * force * 0.01f;
                Velocity.PlaneValue = Map.WorldPosition.V3toV2(diff);
                physics.SpeedY = Bound.Min(Velocity.Y * 2, 0.005f);
                physics.WakeUp();
            }

        }
        const float StandardScale = 1.4f;
        virtual protected float imageScale { get { return StandardScale; } }

        public override ObjectType Type
        {
            get { return ObjectType.PickUp; }
        }
        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
            //
        }
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.BouncingObj2;
            }
        }
        abstract protected VoxelModelName imageType { get; }
        abstract protected Data.TempBlockReplacementSett tempImage { get; }
        /// <summary>
        /// Object will be removed with a timer
        /// </summary>
        virtual protected bool timedRemoval { get { return true; } }

        const float PickUpLifeTime = 30000;
        virtual protected float pickUpLifeTime
        {
            get { return PickUpLifeTime; }
        }

        public override Graphics.LightParticleType LightSourceType
        {
            get
            {
                return Graphics.LightParticleType.Shadow;
            }
        }
        public override Graphics.LightSourcePrio LightSourcePrio
        {
            get
            {
                return Graphics.LightSourcePrio.Low;
            }
        }
        public override float LightSourceRadius
        {
            get
            {
                return imageScale;
            }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return base.NetworkShareSettings;
            }
        }
        protected override RecieveDamageType recieveDamageType
        {
            get { return RecieveDamageType.NoRecieving; }
        }
    }
    
    enum PickUpType
    {
        Spear,
        Coin,
        CoinChest,
        HartContainer,
        Apple,
        Bow,
        DeathLoot,
        HumanoidLoot,
        VoxelObjPresent,
        Herb,
        ReuseArrow,
        Mining,
        FoodScrap,
        HealUp,
        MagicUp,
        NUM
    }
}
