using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters.Monster3
{
    class Scorpion3: AbsMonster3
    {
        static readonly IntervalF ScaleRange = new IntervalF(8f, 9f);
        const int AnimFrame_PreAttack1 = 1;
        const int AnimFrame_PreAttack2 = 2;
        const int AnimFrame_Attack = 3;

        int bulletsLeft;

        public Scorpion3(GoArgs args)
            : base(args)
        {
            createImage(VoxelModelName.scorpion1v2, ScaleRange.GetRandom(), 0, new Graphics.AnimationsSettings(9, 0.8f, 4));
            Health = LfLib.LargeEnemyHealth;
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
                    bulletsLeft = Ref.rnd.Int(5, 8);
                    image.Frame = AnimFrame_PreAttack1;
                    aiState = AiState.Attacking;
                    aiStateTimer.Seconds = 0.5f;
                }
                else if (aiState == AiState.Attacking)
                {
                    bulletsLeft--;

                    float heightAdd = modelScale * 0.35f;
                    Vector3 firePos = image.position + VectorExt.V2toV3XZ(rotation.Direction(-modelScale * 0.05f), heightAdd);

                    Vector3 targetPos = firePos + VectorExt.V2toV3XZ(rotation.Direction(10), Bound.MaxAbs(storedTarget.HeadPosition.Y - this.image.position.Y, 4) - heightAdd + 0.5f);//.Position.Y
                    
                    new WeaponAttack.Monster.ScorpionBullet3(new GoArgs(firePos), targetPos);
                    aiStateTimer.MilliSeconds = 200f;

                    if (bulletsLeft == 0)
                    {
                        aiState = AiState.Waiting;
                    }
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
            else if (aiState == AiState.RotatingTowardsGoal || aiState == AiState.Attacking)
            {
                aiPhys.MovUpdate_RotateTowards(storedTarget, 0.01f);
            }
            else
            {
                aiPhys.MovUpdate_StandStill();
            }
        }


        Timer.Basic preAttackTimer = new Timer.Basic(66, true);

        protected override void updateAnimation()
        {
            if (aiState == AiState.PreAttack)
            {
                if (preAttackTimer.Update())
                {
                    image.Frame = image.Frame == AnimFrame_PreAttack1? AnimFrame_PreAttack2 : AnimFrame_PreAttack1;
                }
            }
            else if (aiState == AiState.Attacking)
            {
                image.Frame = AnimFrame_Attack;
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
                return new Vector3(0, modelScale * 0.3f, 0);
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.Scorpion; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.UnderConstruction; }
        }
        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.darker_red_orange, 
            Data.MaterialType.gray_85);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        protected const float WalkingSpeed = 0.009f;
        protected const float CasualWalkSpeed = StandardWalkingSpeed * 0.7f;
        

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
