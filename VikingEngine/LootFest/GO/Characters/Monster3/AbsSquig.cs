using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.GO.Characters.Monster3
{
     /*
    jump: 1
    proj att/fly att: 2
    sleep: 3
    */
    abstract class AbsSquig : AbsMonster3
    {
        bool baby;
        int attackCommoness;

        public AbsSquig(GoArgs args, VoxelModelName model, IntervalF scaleRange, bool baby, int attackCommoness)
            : base(args)
        {
            this.baby = baby;
            this.attackCommoness = attackCommoness;

            jumpFrame = 1;
            preRangeAttackFrame = 2;
            sleepingFrame1 = 3;
            modelScale = scaleRange.GetRandom();

            createImage(model, modelScale, new Graphics.AnimationsSettings(9, 2f, 4));

            loadBounds();

            if (baby)
            {
                Health = LfLib.SuperWeakEnemyHealth;
            }
            else
            {
                if (Type == GameObjectType.SquigHorned)
                    Health = LfLib.LargeEnemyHealth;
                else
                    Health = LfLib.StandardEnemyHealth;
            }

            if (args.LocalMember)
            {
                
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

            if (aiStateTimer.CountDown())
            {
                aiStateTimer.Seconds = Ref.rnd.Float(1f, 2f);

                if (aiState == AiState.Waiting)
                {
                    rotation = Rotation1D.Random();
                    aiState = AiState.Walking;
                }
                else
                {
                    aiState = AiState.Waiting;
                }
            }


            if (aiState == AiState.Walking)
            {
                aiPhys.MovUpdate_MoveForward(rotation, casualWalkSpeed);
            }
            else
            {
                aiPhys.MovUpdate_StandStill();
            }
        }

        override protected void updateAiMovement_Attacking()
        {
            if (aiStateTimer.CountDown())
            {

                if (nextCCAttackTimer.TimeOut &&
                    !aiPhys.PhysicsStatusFalling &&
                    LookingTowardObject(target, 1f) &&
                    distanceToObject(target) < 12)
                { //Jump attack
                    aiPhys.MovUpdate_MoveForward(rotation, rushSpeed);
                    nextCCAttackTimer = attackRate;
                    physics.Jump(0.7f, image);
                    aiState = AiState.LockedInAttack;
                    aiStateTimer.Seconds = 10;
                }
                else if (aiState == AiState.Waiting)
                {
                    if (Ref.rnd.Chance(attackCommoness) && hasTarget())
                    {
                        aiState = AiState.RotatingTowardsGoal;
                        aiStateTimer.Seconds = Ref.rnd.Float(0.6f, 1f);
                    }
                    else if (Ref.rnd.Chance(60) && hasTarget())
                    {
                        aiState = AiState.MoveTowardsTarget;
                        aiStateTimer.Seconds = Ref.rnd.Float(2f, 4f);
                    }
                    else
                    {
                        aiStateTimer.Seconds = Ref.rnd.Float(1f, 2f);
                        rotation = Rotation1D.Random();
                        aiState = AiState.Walking;
                    }
                }
                else if (aiState == AiState.RotatingTowardsGoal)
                {
                    if (this is SquigHorned)
                    {
                        aiState = AiState.PreRush;
                        aiStateTimer.Seconds = 0.2f;
                        preAttackEffect();
                    }
                    else
                    {
                        aiState = AiState.PreRangedAttack;
                        aiStateTimer.Seconds = 0.6f;
                    }
                }
                else if (aiState == AiState.PreRush)
                {
                    
                    //bulletsLeft = Ref.rnd.Int(5, 8);
                    //image.Currentframe = AnimFrame_PreAttack1;
                    aiStateTimer.Seconds = 2f;
                    aiState = AiState.Rush;
                }
                else if (aiState == AiState.PreRangedAttack)
                {
                    preAttackEffect();
                    //bulletsLeft = Ref.rnd.Int(5, 8);
                    //image.Currentframe = AnimFrame_PreAttack1;
                    image.Frame = preRangeAttackFrame;
                    aiState = AiState.RangedAttack;
                    aiStateTimer.Seconds = 0.5f;
                }
                else if (aiState == AiState.RangedAttack)
                {
                    float heightAdd = modelScale * 0.35f;
                    Vector3 firePos = image.position + VectorExt.V2toV3XZ(rotation.Direction(-modelScale * 0.05f), heightAdd);

                    Vector3 targetPos = firePos + VectorExt.V2toV3XZ(rotation.Direction(10), Bound.MaxAbs(storedTarget.HeadPosition.Y - this.image.position.Y, 4) - heightAdd + 0.5f);//.Position.Y

                    //new WeaponAttack.Monster.ScorpionBullet3(new GoArgs(firePos), targetPos);
                    new VikingEngine.LootFest.GO.WeaponAttack.Monster.SquigBullet2(new GoArgs(firePos, squigBulletLevel), targetPos);
                    aiStateTimer.MilliSeconds = 200f;

                    //if (bulletsLeft == 0)
                    //{
                    aiState = AiState.PostRangedAttack;
                    //}
                }
                else if (aiState == AiState.PostRangedAttack || aiState == AiState.Rush || aiState == AiState.EndRush)
                {
                    aiState = AiState.Waiting;
                }
                else
                {
                    if (attentionStatus == HumanoidEnemyAttentionStatus.FoundHero_5)
                        aiStateTimer.Seconds = Ref.rnd.Float(0.5f, 0.8f);
                    else
                        aiStateTimer.Seconds = Ref.rnd.Float(1f, 2f);

                    aiState = AiState.Waiting;
                }
            }//END if (aiStateTimer.CountDown())


            if (aiState == AiState.Walking)
            {
                float speed;
                if (attentionStatus == HumanoidEnemyAttentionStatus.FoundHero_5)
                    speed = walkingSpeed;
                else
                    speed = casualWalkSpeed;

                aiPhys.MovUpdate_MoveForward(rotation, speed);
            }
            else if (aiState == AiState.RotatingTowardsGoal || aiState == AiState.Attacking)
            {
                aiPhys.MovUpdate_RotateTowards(storedTarget, 0.01f);
            }
            else if (aiState == AiState.LockedInAttack)
            {
                if (!aiPhys.PhysicsStatusFalling)
                { 
                    aiState = AiState.Walking;
                    aiStateTimer.MilliSeconds = 200f;
                }
                aiPhys.MovUpdate_MoveForward(rotation, rushSpeed);
            }
            else if (aiState == AiState.Rush || aiState == AiState.EndRush)
            {
                if (aiState == AiState.Rush)
                {
                    aiPhys.rotateSpeedMoving = AiPhysicsLf3.StandardRotateSpeedMoving * 0.5f;
                    if (!aiPhys.MovUpdate_MoveTowards(storedTarget, 0.1f, rushSpeed))
                    {
                        aiState = AiState.EndRush;
                    }
                }
                else
                {
                    aiPhys.MovUpdate_MoveForward(rotation, rushSpeed);
                }

                if (Ref.TimePassed16ms && Ref.rnd.Chance(0.6f))
                {
                        Engine.ParticleHandler.AddParticles(ParticleSystemType.RunningSmoke,
                            new ParticleInitData(Ref.rnd.Vector3_Sq(image.position, 0.3f), Vector3.Zero));
                    
                }
            }
            else if (aiState == AiState.MoveTowardsTarget)
            {
                aiPhys.rotateSpeedMoving = AiPhysicsLf3.StandardRotateSpeedMoving;
                aiPhys.MovUpdate_MoveTowards(storedTarget, 1f, walkingSpeed);
            }
            else
            {
                aiPhys.MovUpdate_StandStill();
            }
        }

        abstract protected int squigBulletLevel { get; }

        public override void onAiJumpOverObsticle()
        {
            aiState = AiState.LockedInAttack;
        }

        public override void onFoundHero()
        {
            base.onFoundHero();
            image.Frame = 3;
        }

        protected override void preAttackEffect()
        {
            if (aiPhys != null)
            {
                aiPhys.Jump(0.6f, image);
            }
            base.preAttackEffect();
        }

        public override Vector3 HeadPosition
        {
            get
            {
                Vector3 result = image.position;
                Vector2 forwardDir = rotation.Direction(image.Scale1D * 1.2f);
                result.X += forwardDir.X;
                result.Y += image.Scale1D * 0.17f;
                result.Z += forwardDir.Y;
                return result;
            }
        }

        

        //override protected Vector3 headEffectPos()
        //{
        //    Vector3 result = image.Position;
        //    Vector2 forwardDir = rotation.Direction(image.Scale1D * 1.2f);
        //    result.X += forwardDir.X;
        //    result.Y += image.Scale1D * 0.17f;
        //    result.Z += forwardDir.Y;
        //    return result;
        //}

        static readonly Vector3 ExpressionEffectScaleToOffset = new Vector3(0, 1f, 1.2f);
        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return new Vector3(0, 0.7f, 0.3f) * modelScale;
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

        protected override void updateAnimation()
        {
            if (sleepingEffect != null)
            {   
                image.Frame = sleepingFrame1;   
            }
            else if (aiState == AiState.LockedInAttack)
            {
                image.Frame = jumpFrame;
            }
            else if (aiState == AiState.PreRangedAttack || aiState == AiState.RangedAttack || aiState == AiState.PostRangedAttack)
            {
                image.Frame = preRangeAttackFrame;
            }
            else
            {
                base.updateAnimation();
            }
        }

       
        public override float LightSourceRadius { get { return image.scale.X * (baby? 8 : 14); } }


        protected override Vector3 mountSaddlePos()
        {
            return calcSaddlePos(0.5f, 8.6f, 1.2f);
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
                return !baby || aiState == AiState.LockedInAttack;
            }
        }

        public override MountType MountType
        {
            get { return baby? MountType.NumNone : MountType.Mount; }
        }


    }
}
