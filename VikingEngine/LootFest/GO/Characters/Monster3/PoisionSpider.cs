using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters.Monster3
{
    class PoisionSpider : AbsMonster3
    {
        static readonly IntervalF ScaleRange = new IntervalF(3.5f, 3.5f);

        public PoisionSpider(GoArgs args)
            : base(args)
        {
            createImage(VoxelModelName.poison_spider1, ScaleRange.GetRandom(), 0, new Graphics.AnimationsSettings(7, 0.8f, 2));
            Health = LfLib.StandardEnemyHealth;
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
                }
                else if (aiState == AiState.PreAttack)
                {
                    aiState = AiState.Attacking;

                    aiPhys.Jump(0.4f, image);
                    Vector3 firePos = image.position + VectorExt.V2toV3XZ(rotation.Direction(modelScale * 0.4f), 1.0f);

                    Vector3 targetDir = VectorExt.V2toV3XZ(rotation.Direction(10), Bound.MaxAbs(storedTarget.Position.Y - this.image.position.Y, 6) + 10);
                    targetDir.Normalize();

                    new WeaponAttack.Monster.PoisionSpiderProjectile(new GoArgs(firePos), targetDir);
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
            else
            {
                aiPhys.MovUpdate_StandStill();
            }
        }

        protected override void updateAnimation()
        {
            if (aiState == AiState.PreAttack)
            {
                image.Frame = 1;
            }
            else
            {
                base.updateAnimation();
            }
        }

        protected const float HogWalkingSpeed = 0.008f;
        const float CasualWalkSpeed = HogWalkingSpeed * 0.5f;
        protected const float HogRunningSpeed = 0.014f;
        protected const float RunningSpeedLvl2 = HogRunningSpeed * 1.4f;

        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        protected override float walkingSpeed
        {
            get { return HogWalkingSpeed; }
        }
        protected override float rushSpeed
        {
            get { return characterLevel == 0 ? HogRunningSpeed : RunningSpeedLvl2; }
        }


        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
           Data.MaterialType.gray_40, 
            Data.MaterialType.light_pea_green, 
            Data.MaterialType.darker_red_orange);
        
        protected override bool handleCharacterColl(AbsUpdateObj character, Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            aiStateTimer.MilliSeconds = 0;
            return base.handleCharacterColl(character, collisionData, usesMyDamageBound);
        }

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.PoisionSpider; }
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

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return base.NetworkShareSettings;
            }
        }

        protected override bool givesContactDamage
        {
            get
            {
                return false;
            }
        }
    }
}
