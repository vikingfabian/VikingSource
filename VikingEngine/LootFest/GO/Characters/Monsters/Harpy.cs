using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest;
using VikingEngine.Physics;

namespace VikingEngine.LootFest.GO.Characters.Monsters
{
    class Harpy: AbsMonster
    {
        Rotation1D heightWaving = Rotation1D.Random();
        static readonly IntervalF ScaleRange = new IntervalF(4f, 4.5f);
        
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
        public Harpy(GoArgs args)
            : base(args)
        {
            setHealth(LfLib.StandardEnemyHealth);

            if (args.LocalMember)
            {
                this.WorldPos.SetAtClosestFreeY(8);
                FlyHeight = WorldPos.Y;

                NetworkShareObject();
                standardStateTime = new Range(1000, 1800) - characterLevel * 500;
            }
        }
        // public Harpy(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //}
         override protected void createBound(float imageScale)
         {
             Vector3 bodysz = new Vector3(0.17f * imageScale, 0.4f * imageScale, imageScale * 0.25f);
             BoundData2 body = new BoundData2(new Box1axisBound(new VectorVolumeC(Vector3.Zero, bodysz), Rotation1D.D0), new Vector3(0, bodysz.Y, 0));
             Vector3 wingsz = new Vector3(0.5f * imageScale, 0.12f * imageScale, imageScale * 0.05f);
             BoundData2 wings = new BoundData2(new Box1axisBound(new VectorVolumeC(Vector3.Zero, wingsz), Rotation1D.D0), new Vector3(0, wingsz.Y * 2f, imageScale * 0.18f));

             CollisionAndDefaultBound = new GO.Bounds.ObjectBound(new BoundData2[] { body, wings });
             TerrainInteractBound = new GO.Bounds.ObjectBound(VikingEngine.LootFest.GO.Bounds.BoundShape.Cylinder, Vector3.Zero,
                 new Vector3(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f, 0), Vector3.Zero);//LootFest.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f, this);
         }
         protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
         {
             return base.willReceiveDamage(damage);
         }
        protected override VoxelModelName imageName
        {
            get { return characterLevel ==0? VoxelModelName.harpy : VoxelModelName.harpy2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet = 
            new Graphics.AnimationsSettings(6, 0.8f);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }

        float FlyHeight;// = Map.WorldPosition.ChunkStandardHeight + 8;
        bool turning = false;
        Time turningTime = new Time(2000);
        public override void Time_Update(UpdateArgs args)
        {
            animSettings.UpdateAnimation(image, walkingSpeed, args.time);
            if (localMember)
            {
                physics.ObsticleCollision();
                
                oldVelocity = Velocity;

                Velocity.PlaneUpdate(args.time, image);
                heightWaving.Add(0.01f * args.time);
                float goalHeight = FlyHeight + heightWaving.Direction(0.4f).Y;

                switch (aiState)
                {
                    case AiState.Waiting: //only when far off from a target
                        Velocity.SetZeroPlaneSpeed(); //= Vector2.Zero;
                        break;
                    case AiState.Walking:
                        if (turning && target != null)
                        {
                            rotateTowardsObject(target, 0.005f, walkingSpeed);
                            setImageDirFromSpeed();
                        }
                        break;
                    case AiState.Attacking:
                        if (target != null)
                            goalHeight = target.Y + 1f;
                        break;
                }


                float heightDiff = goalHeight - image.position.Y;
                const float MaxHeightSpeed = 0.008f;

                float yspeed = heightDiff * 0.05f;
                if (Math.Abs(yspeed) > MaxHeightSpeed)
                {
                    yspeed = MaxHeightSpeed * lib.ToLeftRight(yspeed);
                }
                image.position.Y += yspeed * args.time;

                //if (checkOutsideUpdateArea_ActiveChunk())
                //{
                //    DeleteMe();
                //}
            }
            else
            {

                updateClientDummieMotion(args.time);
                setImageDirFromRotation();
            }
            characterCritiqalUpdate(true);
        }

        static readonly Range AttackTime = new Range(1200, 1600);
        static readonly Range WaitingTime = new Range(500, 700);
        static readonly Range TurningStateTime = new Range(800, 1600);
        Range standardStateTime;

        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            //float above the hero and make attacks where it flies down in a sweep
            //can use target heigh as goal height when attacking
            basicAIupdate(args);
            if (localMember)
            {
                flySweepAIupdate(args, ref turningTime, ref turning,
                    standardStateTime, AttackTime, WaitingTime, TurningStateTime);
               
            }
        }

        const float WalkingSpeed = 0.011f;
        const float AttackSweepSpeed = 0.014f;

        const float WalkingSpeedLvl2 = WalkingSpeed * 1.4f;
        const float AttackSweepSpeedLvl2 = AttackSweepSpeed * 1.2f;
        
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return AttackSweepSpeed; }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.Harpy; }
        }
        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.zombie_skin,
            Data.MaterialType.dark_pea_green,
            Data.MaterialType.pure_red_orange);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return characterLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Harpy; }
        }
        public override CardType CardCaptureType
        {
            get
            {
                return CardType.Harpy;
            }
        }
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.FlyingObj;
            }
        }
        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return CollisionDamage;
            }
        }

        


        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return new Vector3(0, 3f, 0);
            }
        }
       
    }
}
