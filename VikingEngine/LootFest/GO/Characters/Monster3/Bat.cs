using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace VikingEngine.LootFest.GO.Characters
{
    class Bat : AbsMonster3
    {
        const int DungeonRoofY = LfLib.DungeonGroundHeight + 8;
        static readonly IntervalF RoofHangoutTime = new IntervalF(2000, 3000);
        float flySpeed;
        int sonarCount;
        Vector3 sonarTarget;

        public Bat(GoArgs args)
            : base(args)
        {
            VoxelModelName modelName;
            if (characterLevel == 0)
            {
                modelScale = Ref.rnd.Float(2f, 2.5f);
                modelName = VoxelModelName.bat1;
                flySpeed = 0.016f;
            }
            else
            {
                modelScale = Ref.rnd.Float(4f, 4.5f);
                modelName = VoxelModelName.bat2;
                flySpeed = 0.01f;
                projectileRate.Seconds = 4f;
                Health = LfLib.LargeEnemyHealth;
            }

            WorldPos.Y = DungeonRoofY;
            createImage(modelName, modelScale, new Graphics.AnimationsSettings(3, 100f, 1));
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.2f * modelScale, 0.2f * modelScale, modelScale * 0.38f), modelScale * 0.4f);
            


            if (args.LocalMember)
            {
                TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBound(modelScale * 0.2f, modelScale * 0.2f);

                createAiPhys();

                alwaysFullAttension();
                aiState = AiState.Waiting;
                aiStateTimer.MilliSeconds = RoofHangoutTime.GetRandom();

                NetworkShareObject();
            }
        }

        //public Bat(System.IO.BinaryReader r)
        //    : base(r)
        //{

        //}

        protected override void updateAiMovement_Attacking()
        {
            if (target == null || aiState == AiState.Waiting)
            {
                if (aiStateTimer.CountDown())
                {
                    if (hasTarget() && distanceToObject(target) < 18)
                    {
                        //fly sweep attack
                        aiState = AiState.MoveTowardsTarget;
                        if (characterLevel == 0)
                        {
                            aiStateTimer.Seconds = Ref.rnd.Float(1.5f, 2f);
                        }
                        else
                        {
                            aiStateTimer.Seconds = Ref.rnd.Float(4f, 5f);
                        }
                    }
                }
            }
            else if (aiState == AiState.MoveTowardsTarget)
            {

                aiPhys.MovUpdate_MoveTowards(target.HeadPosition, 0, flySpeed);

                if (characterLevel == 1 && nextRangedAttackTimer.TimeOut)
                {
                    float dist = distanceToObject(target);
                    if (8 < dist && dist < 18 && 
                        LookingTowardObject(target, 1.2f))
                    {
                        aiState = AiState.PreAttack;
                        aiStateTimer.MilliSeconds = 1000;
                        //var anim = image.AnimationsSettings;
                        animSettings.NumFramesPlusIdle = 5;
                        animSettings.NumIdleFrames = 3;
                        //image.AnimationsSettings = anim;
                    }
                }
                
                if (aiStateTimer.CountDown())
                {
                    retreat();
                }
            }
            else if (aiState == AiState.PreAttack)
            {
                aiPhys.MovUpdate_RotateTowards(target, 0.06f);
                if (aiStateTimer.CountDown())
                {
                    //Begin fire stunn sonar
                    aiState = AiState.Attacking;
                    aiStateTimer.MilliSeconds = 0;//600;

                    sonarCount = 3;
                    sonarTarget = target.HeadPosition;
                }
            }
            else if (aiState == AiState.Attacking)
            {
                if (aiStateTimer.CountDown())
                {
                    if (sonarCount > 0)
                    {
                        aiStateTimer.MilliSeconds = 100;
                        sonarCount--;

                        Vector3 sonarStart = image.position + 
                            VectorExt.V2toV3XZ(rotation.Direction(modelScale * 0.5f), modelScale * 0.2f);

                        new WeaponAttack.Monster.BatSonar(GoArgs.Empty, sonarStart, sonarTarget);
                    }
                    else
                    {
                        nextRangedAttackTimer = projectileRate;
                        aiState = AiState.AttackComplete;
                        aiStateTimer.Seconds = 1f;
                    }
                }
            }
            else if (aiState == AiState.AttackComplete)
            {
                //var anim = image.AnimationsSettings;
                animSettings.NumFramesPlusIdle = 3;
                animSettings.NumIdleFrames = 1;
                //image.AnimationsSettings = anim;

                if (aiStateTimer.CountDown())
                {
                    aiState = AiState.MoveTowardsTarget;
                    aiStateTimer.Seconds = Ref.rnd.Float(3f, 4f);
                }
            }
            else
            {
                //Retreat to roof pos
                aiPhys.MovUpdate_MoveTowards(retreatPos, 0, flySpeed);
                if (image.position.Y >= DungeonRoofY - 0.4f)
                {
                    aiState = AiState.Waiting;
                    aiStateTimer.MilliSeconds = RoofHangoutTime.GetRandom();
                    Velocity.SetZero();
                }
            }

            nextRangedAttackTimer.CountDown();
        }

        protected override void moveImage(Velocity speed, float time)
        {
            base.moveImage(speed, time);
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            retreat();
            return base.handleCharacterColl(character, collisionData, usesMyDamageBound);
        }

        Vector3 retreatPos;
        void retreat()
        {
            aiPhys.flyPath.lengthToGoal = 100;
            aiState = AiState.Walking;
            retreatPos = image.position + VectorExt.V2toV3XZ(Rotation1D.Random().Direction(Ref.rnd.Float(10f, 14f)));
            retreatPos.Y = DungeonRoofY;
        }

        protected override void updateAnimation()
        {
            if (aiState == AiState.Waiting)
            {
                image.Frame = 0;
            }
            else
            {
                animSettings.UpdateAnimation(image, 1f, Ref.DeltaTimeMs);
            }
        }

        protected override void updateAiMovement_Idle()
        {
            updateAiMovement_Attacking();
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.Bat; }
        }

        override public CardType CardCaptureType
        {
            get {
                if (characterLevel == 0)
                    return CardType.Bat1;
                else
                    return CardType.Bat2; }
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.FlyingAi;
            }
        }

        protected override bool animationUseMoveVelocity
        {
            get
            {
                return false;
            }
        }

        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return new Vector3(0, 0.9f, 0.10f) * modelScale;
            }
        }

        public override Vector3 HeadPosition
        {
            get
            {
                return base.HeadPosition;
            }
        }
        
        override protected float walkingSpeed
        {
            get { return flySpeed; }
        }

        protected override bool givesContactDamage
        {
            get
            {
                return aiState == AiState.MoveTowardsTarget || aiState == AiState.Walking;
            }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.pure_violet, 
            Data.MaterialType.pure_violet_magenta);
        public static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(
            Data.MaterialType.darker_yellow_orange, 
            Data.MaterialType.pure_yellow_orange);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return characterLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }


    }
}
