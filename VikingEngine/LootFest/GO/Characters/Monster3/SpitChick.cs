using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters
{
    /// <summary>
    /// Prev squig monster
    /// </summary>
    class SpitChick : AbsMonster3
    {
        static readonly IntervalF ScaleRange = new IntervalF(2f, 2.2f);

        public SpitChick(GoArgs args)
            : base(args)
        {
            createImage(VoxelModelName.spitchick_lvl1, ScaleRange.GetRandom(), 0, new Graphics.AnimationsSettings(7, 0.8f, 2));
            Health = LfLib.StandardEnemyHealth;
            loadBounds();

            if (args.LocalMember)
            {
                createAiPhys();
                aiPhys.path.maxJumpUp = 6;
                aiPhys.path.maxJumpDown = 12;
                aiPhys.Gravity *= 0.86f;
                targetSearchDistanceIdle = 15; targetSearchDistanceTaunted = 24;

                NetworkShareObject();
            }
        }

       
        override protected void updateAiMovement()
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
                    aiStateTimer.Seconds = 0.2f;
                }
                else if (aiState == AiState.PreAttack)
                {
                    aiState = AiState.Attacking;

                    aiPhys.Jump(0.8f, image);
                    Vector3 firePos = image.position + VectorExt.V2toV3XZ(rotation.Direction(modelScale * 0.1f), 1.3f);

                    Vector3 targetPos = firePos + VectorExt.V2toV3XZ(rotation.Direction(10), Bound.MaxAbs(storedTarget.Position.Y - this.image.position.Y, 4));
                    //new WeaponAttack.SquigBullet(firePos, rotation, GameObjectType.SquigBullet);

                    new WeaponAttack.Monster.SpitChickBullet(new GoArgs(firePos), targetPos);
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

        protected override bool givesContactDamage
        {
            get
            {
                return false;
            }
        }

        protected override void updateAnimation()
        {
            if (aiState == AiState.Attacking)
            {
                image.Frame = 1;
            }
            else
            {
                base.updateAnimation();
            }
        }

        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return new Vector3(0, modelScale * 1f, 0);
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SpitChick; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.SpitChick; }
        }
        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.pure_yellow_green,
            Data.MaterialType.pure_yellow_orange,
            Data.MaterialType.light_pea_green);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        protected const float WalkingSpeed = 0.008f;
        protected const float CasualWalkSpeed = StandardWalkingSpeed * 0.5f;
        

        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        override protected float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        
        
    }


}
