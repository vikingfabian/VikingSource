using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters.Monsters
{
    class Bee : AbsMonster
    {
        Time noCollisionTimer = new Time(2, TimeUnit.Seconds);
        Rotation1D heightWaving = Rotation1D.Random();

        public Bee(GoArgs args)
            : base(args)
        {
            setHealth(LfLib.StandardEnemyHealth);

            if (args.LocalMember)
            {
                args.startWp.SetAtClosestFreeY(8);
                FlyHeight = args.startWp.Y;
                //basicInit();
                NetworkShareObject();
            }
            immortalityTime.Seconds = 2;
        }
       

        override protected void createBound(float imageScale)
        {
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.3f * imageScale, 0.3f * imageScale, imageScale * 0.4f));
            TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
        }
        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            return base.willReceiveDamage(damage);
        }
        protected override VoxelModelName imageName
        {
            get { return characterLevel == 0 ? VoxelModelName.bee : VoxelModelName.bee2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet =
            new Graphics.AnimationsSettings(2, 0.8f, 0);
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
                if (noCollisionTimer.CountDown())
                    physics.ObsticleCollision();

                oldVelocity = Velocity;
                Velocity.PlaneUpdate(args.time, image);

                heightWaving.Add(0.01f * args.time);
                float goalHeight = FlyHeight + heightWaving.Direction(0.4f).Y;

                switch (aiState)
                {
                    case AiState.Waiting: //only when far off from a target
                        Velocity.SetZeroPlaneSpeed();
                        break;
                    case AiState.Walking:
                        if (turning)
                        {
                            rotateTowardsObject(target, 0.005f, walkingSpeed);
                            setImageDirFromSpeed();
                        }
                        break;
                    case AiState.Attacking:
                        if (hasTarget())
                        {
                            goalHeight = target.Y + 1f;
                        }
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
            }
            else
            {

                updateClientDummieMotion(args.time);
                setImageDirFromRotation();
            }
            characterCritiqalUpdate(true);
        }

        

        static readonly Range AttackTime = new Range(1200, 1800);
        static readonly Range WaitingTime = new Range(500, 700);
        static readonly Range TurningStateTime = new Range(800, 1200);
        static readonly Range standardStateTime = new Range(500, 1200);

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

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            base.handleDamage(damage, local);
            if (aiState == AiState.Attacking)
            {
                rotation += MathHelper.Pi + Ref.rnd.Plus_MinusF(MathHelper.PiOver2);
                Velocity.Set(rotation, walkingSpeed);
                aiState = AiState.Walking;
                aiStateTimer.MilliSeconds = Ref.rnd.Int(400, 600);
            }
        }

        const float WalkingSpeed = 0.012f;
        const float AttackSweepSpeed = 0.016f;

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
            get { return GameObjectType.Bee; }
        }
        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.gray_75,
            Data.MaterialType.pure_yellow,
            Data.MaterialType.gray_35);
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
            get { return Monster2Type.Bee; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.Bee; }
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
                return new Vector3(0, 0.4f, 1f);
            }
        }
        protected override float preAttackScale
        {
            get
            {
                return 0.1f;
            }
        }
        public override float ExspectedHeight
        {
            get
            {
                return 0.5f;
            }
        }
        static readonly IntervalF ScaleRange = new IntervalF(1.6f, 2f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
    }
}
