using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LF2;
using VikingEngine.Physics;

namespace VikingEngine.LF2.GameObjects.Characters.Monsters
{
    class Harpy: AbsMonster2
    {
        Rotation1D heightWaving = Rotation1D.Random();
        static readonly IntervalF ScaleRange = new IntervalF(4f, 4.5f);
        
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
        public Harpy(Map.WorldPosition startPos, int level)
            : base(startPos, level)
        {
            setHealth(LootfestLib.HarpyHealth);
            NetworkShareObject();
            standardStateTime = new Range(1000, 1800) - areaLevel * 500;
        }
         public Harpy(System.IO.BinaryReader r)
            : base(r)
        {
        }
         override protected void createBound(float imageScale)
         {
             Vector3 bodysz = new Vector3(0.17f * imageScale, 0.4f * imageScale, imageScale * 0.25f);
             BoundData2 body = new BoundData2(new Box1axisBound(new VectorVolume(Vector3.Zero, bodysz), Rotation1D.D0), new Vector3(0, bodysz.Y, 0));
             Vector3 wingsz = new Vector3(0.5f * imageScale, 0.12f * imageScale, imageScale * 0.05f);
             BoundData2 wings = new BoundData2(new Box1axisBound(new VectorVolume(Vector3.Zero, wingsz), Rotation1D.D0), new Vector3(0, wingsz.Y * 2f, imageScale * 0.18f));

             CollisionBound = new  ObjMultiBound(new BoundData2[]{ body, wings });
             TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
         }
         protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
         {
             return base.willReceiveDamage(damage);
         }
        protected override VoxelModelName imageName
        {
            get { return areaLevel ==0? VoxelModelName.harpy : VoxelModelName.harpy2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet = 
            new Graphics.AnimationsSettings(6, 0.8f);
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
                physics.ObsticleCollision();
                
                oldVelocity = Velocity;

                Velocity.PlaneUpdate(args.time, image);
                heightWaving.Add(0.01f * args.time);
                float goalHeight = StandardHeight + heightWaving.Direction(0.4f).Y;

                switch (aiState)
                {
                    case AiState.Waiting: //only when far off from a target
                        Velocity.SetZeroPlaneSpeed(); //= Vector2.Zero;
                        break;
                    case AiState.Walking:
                        if (turning && target != null)
                        {
                            rotateTowardsObject(target, 0.005f, args.time, walkingSpeed);
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
                    yspeed = MaxHeightSpeed * lib.FloatToDir(yspeed);
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
        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Harpy; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.zombie_skin, Data.MaterialType.red_brown, Data.MaterialType.orange);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.zombie_skin, Data.MaterialType.dark_blue, Data.MaterialType.orange);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return areaLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Harpy; }
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
                return areaLevel == 0 ? MediumCollDamageLvl1 : MediumCollDamageLvl2;
            }
        }

        static readonly Data.Characters.MonsterLootSelection LootSelection = new Data.Characters.MonsterLootSelection(
             Gadgets.GoodsType.Feathers, 60, Gadgets.GoodsType.Red_eye, 30, Gadgets.GoodsType.Sharp_teeth, 10);
        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { return LootSelection; }
        }


        protected override Vector3 preAttackEffectPos
        {
            get
            {
                return new Vector3(0, 3f, 0);
            }
        }
       
    }
}
