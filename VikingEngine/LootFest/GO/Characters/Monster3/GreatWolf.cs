using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Physics;

namespace VikingEngine.LootFest.GO.Characters
{
    class GreatWolf : AbsMonster3
    {
        public GreatWolf(GoArgs args)
            : base(args)
        {
            const float StandardScaleToTerrainBound = 0.4f;
            //4-9, 8*14*21
            weaponAttackFrame = 1;
            sleepingFrame1 = 2;

            createImage(VoxelModelName.greatwolf, 5f, new Graphics.AnimationsSettings(10, 0.8f, 5));
            
            
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotatedFromFeet(new Vector3(0.15f * modelScale, 0.22f * modelScale, modelScale * 0.48f), 0f);
            Health = 2;

            if (args.LocalMember)
            {
                TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBoundFromFeetPos(modelScale * StandardScaleToTerrainBound, modelScale * 0.5f, 0f);

                attackRate.Seconds = 2;
                createAiPhys();
                aiPhys.path.maxJumpUp = 4;
                aiPhys.path.maxJumpDown = 16;

                NetworkShareObject();
            }
        }


        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            nextCCAttackTimer.CountDown();
        }

        protected override void updateAiMovement_Idle()
        {
            //base.updateAiMovement_Idle();
            defaultIdleMovement();
        }

        override protected void updateAiMovement_Attacking()
        {
            if (nextCCAttackTimer.TimeOut && 
                !aiPhys.PhysicsStatusFalling && 
                LookingTowardObject(target, 1f) && 
                distanceToObject(target) < 12)
            {
                aiPhys.MovUpdate_MoveForward(rotation, rushSpeed);
                nextCCAttackTimer = attackRate;
                physics.Jump(0.7f, image);
                aiState = AiState.LockedInAttack;
            }
            else if (aiState == AiState.LockedInAttack)
            {
                if (!aiPhys.PhysicsStatusFalling)
                { aiState = AiState.Walking; }
                aiPhys.MovUpdate_MoveForward(rotation, rushSpeed);
            }
            else
            {
                if (nextCCAttackTimer.TimeOut)
                {
                    aiPhys.MovUpdate_MoveTowards(target, 4f, walkingSpeed);
                }
                else
                {
                    aiPhys.MovUpdate_FleeFrom(target, walkingSpeed, 0);
                }
            }
        }

        public override void onAiJumpOverObsticle()
        {
            aiState = AiState.LockedInAttack;
        }

        public override void onFoundHero()
        {
            base.onFoundHero();
            image.Frame = 3;
        }

        //override protected Vector3 headEffectPos()
        //{
        //    Vector3 result = image.Position;
        //    Vector2 forwardDir = rotation.Direction(image.Scale1D * 1.2f);
        //    result.X += forwardDir.X;
        //    result.Y +=  image.Scale1D * 0.2f;
        //    result.Z += forwardDir.Y;
        //    return result;
        //}

        static readonly Vector3 ExpressionEffectScaleToOffset = new Vector3(0, 0.7f, 0.3f);
        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return ExpressionEffectScaleToOffset * modelScale;
            }
        }

        void createCCAttack()
        {
            const float AttackwarningTime = 400;

            aiStateTimer.MilliSeconds = AttackwarningTime;
            Velocity.SetZeroPlaneSpeed();
            aiState = AiState.Attacking;
            Vector3 headPosDiff = new Vector3(0f, 15, 6f) * image.scale;
            new Effects.EnemyAttention(new Time(AttackwarningTime), image, headPosDiff, 0.25f, Effects.EnemyAttentionType.PreAttack);
            image.Frame = 0;

        }

        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            base.HandleColl3D(collData, ObjCollision);
        }
        public override void HandleObsticle(bool wallNotPit, AbsUpdateObj ObjCollision)
        {
            base.HandleObsticle(wallNotPit, ObjCollision);
            if (aiState == AiState.LockedInAttack && ObjCollision == null)
            {
                stopJumpAttack();
            }
        }

        private void stopJumpAttack()
        {
            Velocity.SetZeroPlaneSpeed();
            physics.Gravity = AbsPhysics.StandardGravity;
            aiState = AiState.Waiting;
        }
        

        protected override Vector3 mountSaddlePos()
        {
            //Vector3 result = image.Position;
            //Vector2 forwardDir = rotation.Direction(image.Scale1D * 0.5f);
            //result.X += forwardDir.X;
            //result.Y += image.Scale1D * 8f;
            //result.Z += forwardDir.Y;

            //if (physics.onGround)
            //    riderBounce += Ref.DeltaTimeMs * Velocity.PlaneLength() * 1.2f;
            //result.Y += (float)Math.Cos(riderBounce) * image.Scale1D * 0.6f;

            //return result;
            return calcSaddlePos(0.5f, 8f, 0.5f);
        }

        protected override void updateAnimation()
        {
            base.updateAnimation();
        }

        const float WalkingSpeed = 0.015f;
        const float CasualWalkSpeed = WalkingSpeed * 0.5f;
        const float JumpSpeed = WalkingSpeed * 1.5f;
        override protected float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        protected override float rushSpeed
        {
            get { return JumpSpeed; }
        }

        protected override bool givesContactDamage
        {
            get
            {
                return aiState == AiState.LockedInAttack;
            }
        }

        public override MountType MountType
        {
            get { return MountType.Mount; }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.GreatWolf; }
        }

        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.dark_cyan,
            Data.MaterialType.gray_60);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }
    }
}
