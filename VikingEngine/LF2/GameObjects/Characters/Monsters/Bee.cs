using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.Characters.Monsters
{
    class Bee : AbsMonster2
    {
        Time noCollisionTimer = new Time(2, TimeUnit.Seconds);
        Rotation1D heightWaving = Rotation1D.Random();
       
        public Bee(Map.WorldPosition startPos, int level)
            : base(startPos, level)
        {
            setHealth(LootfestLib.BeeHealth);
            //basicInit();
            NetworkShareObject();
        }
        public Bee(System.IO.BinaryReader r)
            : base(r)
        {
            //basicInit();
        }

        override protected void basicInit()
        {
            base.basicInit();
            immortalityTime.Seconds = 2;
        }


        override protected void createBound(float imageScale)
        {
            CollisionBound = LF2.ObjSingleBound.QuickRectangleRotated(new Vector3(0.3f * imageScale, 0.3f * imageScale, imageScale * 0.4f));
            TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
        }
        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            return base.willReceiveDamage(damage);
        }
        protected override VoxelModelName imageName
        {
            get { return areaLevel == 0 ? VoxelModelName.bee : VoxelModelName.bee2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet =
            new Graphics.AnimationsSettings(2, 0.8f, 0);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }

        const float StandardHeight = Map.WorldPosition.ChunkStandardHeight + 8;
        bool turning = false;
        Time turningTime = new Time(2000);
        public override void Time_Update(UpdateArgs args)
        {
            image.UpdateAnimation(walkingSpeed, args.time);
            if (localMember)
            {
                if (noCollisionTimer.CountDown())
                    physics.ObsticleCollision();

                oldVelocity = Velocity;
                Velocity.PlaneUpdate(args.time, image);

                heightWaving.Add(0.01f * args.time);
                float goalHeight = StandardHeight + heightWaving.Direction(0.4f).Y;

                switch (aiState)
                {
                    case AiState.Waiting: //only when far off from a target
                        Velocity.SetZeroPlaneSpeed();
                        break;
                    case AiState.Walking:
                        if (turning)
                        {
                            rotateTowardsObject(target, 0.005f, args.time, walkingSpeed);
                            setImageDirFromSpeed();
                        }
                        break;
                    case AiState.Attacking:
                        goalHeight = target.Y + 1f;
                        break;
                }


                float heightDiff = goalHeight - image.position.Y;
                const float MaxHeightSpeed = 0.008f;

                float yspeed = heightDiff * 0.05f;
                if (Math.Abs(yspeed) > MaxHeightSpeed)
                {
                    yspeed = MaxHeightSpeed * lib.FloatToDir(yspeed);
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

        public override void AIupdate(GameObjects.UpdateArgs args)
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
        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Bee; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.yellow, Data.MaterialType.dark_gray, Data.MaterialType.white);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.black, Data.MaterialType.red_brown, Data.MaterialType.red);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return areaLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Bee; }
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
                return areaLevel == 0 ? HighCollDamageLvl1 : HighCollDamageLvl2;
            }
        }

        static readonly Data.Characters.MonsterLootSelection LootSelection = new Data.Characters.MonsterLootSelection(
             Gadgets.GoodsType.Poision_sting, 60, Gadgets.GoodsType.Honny, 100, Gadgets.GoodsType.NONE, 0);
        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { return LootSelection; }
        }

        protected override Vector3 preAttackEffectPos
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
