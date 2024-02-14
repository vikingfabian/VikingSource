using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest;
using VikingEngine.Physics;

namespace VikingEngine.LootFest.GO.Characters.Monsters
{
    class Scorpion: AbsMonster
    {
        //jobbar mot goal dir
        //skjuter om man står för nära framför den
        bool fire = false;
        static readonly IntervalF ScaleRange = new IntervalF(9f, 10f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
        const float ScaleToBound = 0.40f;
        public Scorpion(GoArgs args)
            : base(args)
        {
            setHealth(LfLib.LargeEnemyHealth);
            NetworkShareObject();
        }
        // public Scorpion(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //}
         override protected void createBound(float imageScale)
         {
             Vector3 bodyHalfSz = new Vector3(0.36f * imageScale, 0.14f * imageScale, imageScale * 0.26f);
             BoundData2 body = new BoundData2(new Box1axisBound(new VectorVolume(Vector3.Zero, bodyHalfSz), Rotation1D.D0), new Vector3(0, bodyHalfSz.Y, 0.08f * imageScale));

             Vector3 clawSz = new Vector3(0.10f * imageScale, 0.06f * imageScale, 0.15f * imageScale);
             Vector3 clawOffset = new Vector3(-0.22f * imageScale, 0.08f * imageScale, -0.32f * imageScale);
             BoundData2 leftClaw = new BoundData2(new Box1axisBound(new VectorVolume(Vector3.Zero, clawSz), Rotation1D.D0), clawOffset);
             clawOffset.X = -clawOffset.X;
             BoundData2 rightClaw = new BoundData2(new Box1axisBound(new VectorVolume(Vector3.Zero, clawSz), Rotation1D.D0), clawOffset);

             Vector3 tailSz = new Vector3(0.068f * imageScale, 0.26f * imageScale, imageScale * 0.17f);
             BoundData2 tail = new BoundData2(new Box1axisBound(new VectorVolume(Vector3.Zero, tailSz), Rotation1D.D0), new Vector3(0, 1.4f * tailSz.Y, 0.29f * imageScale));

             CollisionAndDefaultBound = new GO.Bounds.ObjectBound(new BoundData2[] { body, leftClaw, rightClaw, tail });
             TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * ScaleToBound, imageScale * 0.5f);
         }
        protected override VoxelModelName imageName
        {
            get { return characterLevel == 0? VoxelModelName.scorpion1 : VoxelModelName.scorpion2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet = 
            new Graphics.AnimationsSettings(6, 0.8f);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }

        Rotation1D goalDir = Rotation1D.Random();
        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            basicAIupdate(args);
            if (localMember)
            {
                if (aiStateTimer.MilliSeconds <= 0)
                {
                    aiStateTimer.MilliSeconds = 1200 + Ref.rnd.Int(600);
                    goalDir.Add(Ref.rnd.Plus_MinusF(2));
                    target = getClosestCharacter(50, args.allMembersCounter, this.WeaponTargetType);
                    if (target != null)
                    {
                        if (distanceToObject(target) <= 20 && LookingTowardObject(target, 1.6f))
                        {
                            //fire bullet
                            fire = true;
                        }

                        if (Ref.rnd.Chance(60))
                        {
                            goalDir.Radians = AngleDirToObject(target);
                        }

                    }
                }
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (fire)
            {
                fire = false;
                Vector3 firePos = image.position;
                firePos.Y += image.scale.Y * 4f;
                firePos += VectorExt.V2toV3XZ(rotation.Direction(-image.scale.Z *6));
                new WeaponAttack.ScorpionBullet(new GoArgs(firePos, characterLevel), firePos, target.Position);
            }

            rotateTowardsGoalDir(goalDir.Radians, 0.002f, walkingSpeed);
            setImageDirFromSpeed();
            base.Time_Update(args);
        }

        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            //base.HandleColl3D(collData, ObjCollision);

            if (ObjCollision != null)
            {
                goalDir = Rotation1D.Random();
                if (WeaponAttack.WeaponLib.IsFoeTarget(this, ObjCollision, false))
                {
                    //unthread this
                    handleCharacterColl(ObjCollision, collData, false);
                }
            }
            else
            {
                removeForce();
                obsticlePushBack(collData);
                goalDir.Add(Ref.rnd.Plus_MinusF(0.4f));
            }
            
            //Vector3 depth = CollisionBound.IntersectionDepthAndDir(collData) * 0.2f;
            //depth.Y = 0;
            //Position += depth;
           
        }

        const float WalkingSpeedLvl1 = 0.007f;
        const float WalkingSpeedLvl2 = 0.01f;
        protected override float walkingSpeed
        {
            get { return characterLevel == 0 ? WalkingSpeedLvl1 : WalkingSpeedLvl2; }
        }
        protected override float runningSpeed
        {
            get { return walkingSpeed; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.darker_red_orange,
            Data.MaterialType.gray_85,
            Data.MaterialType.darker_warm_brown);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return characterLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.Scorpion; }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Scorpion; }
        }
        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return CollisionDamage;//characterLevel == 0 ? MediumCollDamageLvl1 : MediumCollDamageLvl1;
            }
        }


        override protected float imageYadj { get { return -0.5f; } }
    }
}
