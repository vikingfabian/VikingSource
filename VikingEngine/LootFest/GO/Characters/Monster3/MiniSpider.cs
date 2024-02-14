using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters.Monster3
{
    class MiniSpider : AbsMonster3
    {
        static readonly IntervalF ScaleRange = new IntervalF(2f, 2f);

        public MiniSpider(GoArgs args)
            : base(args)
        {
            createImage(VoxelModelName.mini_spider1, ScaleRange.GetRandom(), 0, new Graphics.AnimationsSettings(8, 0.8f, 3));
            Health = LfLib.SuperWeakEnemyHealth;
            loadBounds();

            if (args.LocalMember)
            {
                createAiPhys();

                NetworkShareObject();
            }
        }

        protected override void updateAiMovement()
        {
            if (aiStateTimer.CountDown())
            {
                if (aiState == AiState.Waiting)
                {
                    if (Ref.rnd.Chance(70) && hasTarget())
                    {
                        aiState = AiState.RotatingTowardsGoal;
                        aiStateTimer.Seconds = Ref.rnd.Float(0.6f, 1f);
                    }
                    else
                    {
                        aiStateTimer.Seconds = Ref.rnd.Float(2f, 4f);
                        rotation = Rotation1D.Random();
                        aiState = AiState.Walking;
                    }
                }
                else if (aiState == AiState.RotatingTowardsGoal)
                {
                    aiState = AiState.PreAttack;
                    aiStateTimer.Seconds = 0.6f;

                    //netSharedAiState();
                }
                else if (aiState == AiState.PreAttack)
                {
                    aiState = AiState.Attacking;

                    
                    Vector3 firePos = image.position + VectorExt.V2toV3XZ(rotation.Direction(modelScale * 0.4f), 1.0f);

                    Vector3 targetDir = VectorExt.V2toV3XZ(rotation.Direction(10), Bound.MaxAbs(storedTarget.Position.Y - this.image.position.Y, 6) + 10);
                    targetDir.Normalize();

                    //new WeaponAttack.Monster.PoisionSpiderProjectile(new GoArgs(firePos), targetDir);
                    Velocity.Set(rotation.Radians, SpiderWalkingSpeed * 1.5f);
                    aiPhys.Jump(1f, image);

                    aiStateTimer.Seconds = 0.7f;
                }
                else
                {
                    if (attentionStatus == HumanoidEnemyAttentionStatus.FoundHero_5)
                        aiStateTimer.Seconds = Ref.rnd.Float(0.5f, 0.8f);
                    else
                        aiStateTimer.Seconds = Ref.rnd.Float(1f, 2f);

                    aiState = AiState.Waiting;
                }
            }

            if (aiState == AiState.Walking)
            {
                float speed;
                if (attentionStatus == HumanoidEnemyAttentionStatus.FoundHero_5)
                    speed = walkingSpeed;
                else
                    speed = casualWalkSpeed;

                aiPhys.MovUpdate_MoveForward(rotation, speed);
            }
            else if (aiState == AiState.RotatingTowardsGoal)
            {
                aiPhys.MovUpdate_RotateTowards(storedTarget, 0.01f);
            }
            else if (aiState == AiState.Attacking)
            {
                aiPhys.updateMovement();
                if (!aiPhys.PhysicsStatusFalling)
                {
                    aiState = AiState.AttackComplete;
                    //netSharedAiState();
                    Velocity.SetZero();
                }
            }
            else
            {
                aiPhys.MovUpdate_StandStill();
            }
        }

       

        protected override void updateAnimation()
        {
            if (aiState == AiState.PreAttack || aiState == AiState.Attacking)
            {
                image.Frame = 1;
            }
            else
            {
                base.updateAnimation();
            }
        }

        public override bool canBeCardCaptured
        {
            get
            {
                return true;
            }
        }

        protected const float SpiderWalkingSpeed = 0.014f;
        const float CasualWalkSpeed = SpiderWalkingSpeed * 0.5f;

        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        protected override float walkingSpeed
        {
            get { return SpiderWalkingSpeed; }
        }


        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
           Data.MaterialType.darker_green_cyan,
           Data.MaterialType.dark_green_cyan,
           Data.MaterialType.darker_green);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            aiStateTimer.MilliSeconds = 0;
            return base.handleCharacterColl(character, collisionData, usesMyDamageBound);
        }

        protected override bool givesContactDamage
        {
            get
            {
                return aiState == AiState.Attacking;
            }
        }


        public override GameObjectType Type
        {
            get { return GameObjectType.MiniSpider; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.UnderConstruction; }
        }
        static readonly Vector3 ExpressionEffectScaleToOffset = new Vector3(0, 0.45f, 0.2f);
        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return ExpressionEffectScaleToOffset * modelScale;
            }
        }
    }
}
