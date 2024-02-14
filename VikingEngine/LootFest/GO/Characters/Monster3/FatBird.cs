using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters
{
    
    class FatBird : AbsMonster3
    {
        const int AnimFallFrame = 0;
        const int AnimDizzyFrame = 1;
        float targetAbovePos;

        protected int eggCount = 0;


        public FatBird(GoArgs args)
            : base(args)
        {
            VoxelModelName modelName;
            if (characterLevel == 0)
            {
                modelScale = Ref.rnd.Float(3, 4);
                Health = LfLib.StandardEnemyHealth;
                targetAbovePos = 10;
                modelName = VoxelModelName.fat_bird;
            }
            else
            {
                modelScale = Ref.rnd.Float(6, 7);
                Health = LfLib.LargeEnemyHealth;
                targetAbovePos = 13;
                eggCount = 2;
                modelName = VoxelModelName.fat_bird2;
                projectileRate.Seconds = 3f;
            }
            //1. fall
            //2. dizzy
            //3-5. fly
            createImage(modelName, modelScale, new Graphics.AnimationsSettings(5, 100f, 2));

            LfRef.bounds.LoadBound(this);

            if (args.LocalMember)
            {
                createAiPhys();
                aiPhys.yAdj = -0.11f * modelScale;

                alwaysFullAttension();
                NetworkShareObject();
                //aiState = AiState.MoveTowardsTarget;
            }
        }

       

        float fallToY;

        

        protected override void updateAiMovement_Attacking()
        {
            
            if (hasTarget())
            {
                if (aiState == AiState.PreAttack)
                {
                    if (aiStateTimer.CountDown())
                    {
                        aiState = AiState.Attacking;
                        Velocity.Value = Vector3.Zero;
                        fallToY = WorldPos.Screen.GetClosestFreeY(WorldPos);
                    }
                }
                else if (aiState == AiState.Attacking)
                {
                    if (aiPhys.MovUpdate_FallToGround(physics.Gravity * 2.2f))
                    {
                        onGroundPounce(1f);

                        float vibrationForce, shakeForce, stunTime;
                        if (characterLevel == 0)
                        {
                            vibrationForce = 0.2f;
                            shakeForce = 0.3f;
                            stunTime = 0.6f;
                        }
                        else
                        {
                            vibrationForce = 0.4f;
                            shakeForce = 0.6f;
                            stunTime = 1f;
                        }

                        Effects.EffectLib.VibrationCenter(image.position, vibrationForce, 600, 10f);
                        Effects.EffectLib.CameraShakeCenter(image.position, shakeForce);

                        checkCharacterCollision(LfRef.gamestate.GameObjCollection.localMembersUpdateCounter, true);
                        stunForce(stunTime, 0, false, true);
                    }
                }
                else if (aiState == AiState.MoveTowardsTarget)
                {
                    Vector3 goalPos = target.Position;
                    goalPos.Y += targetAbovePos;
                    aiPhys.MovUpdate_MoveTowards(goalPos, 0, walkingSpeed);

                    Vector3 diff = goalPos - image.position;
                    float ydiff = diff.Y;
                    diff.Y = 0;

                    if (eggCount > 0)
                    {
                        if (nextRangedAttackTimer.CountDown())
                        {
                            if (diff.Length() < 15f)
                            {
                                //fire
                                nextRangedAttackTimer = projectileRate;
                                new WeaponAttack.FatEgg(image.position, target.Position + Vector3.Up * 2f);
                                eggCount--;
                            }
                        }
                    }
                    else
                    {
                        if (diff.Length() < 0.5f && Math.Abs(ydiff) < 2f)
                        {
                            aiState = AiState.PreAttack;
                            Velocity.Value = Vector3.Zero;
                            aiStateTimer.MilliSeconds = 400;
                            preAttackEffect();
                        }
                    }

                    if (aiStateTimer.CountDown())
                    {
                        aiStateTimer.MilliSeconds = Ref.rnd.Int(200, 1600);
                        aiState = AiState.Walking;
                    }
                }
                else
                {
                    aiPhys.MovUpdate_MoveForward(rotation, walkingSpeed);
                    if (aiStateTimer.CountDown())
                    {
                        aiStateTimer.MilliSeconds = Ref.rnd.Int(2000, 6000);
                        aiState = AiState.MoveTowardsTarget;
                    }
                }
            }
        }

        protected override void updateAiMovement_Idle()
        {
            //base.updateAiMovement_Idle();
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

        protected override void updateAnimation()
        {
            if (aiState == AiState.PreAttack || aiState == AiState.Attacking)
            {
                image.Frame = AnimFallFrame;
            }
            else if (aiState == AiState.IsStunned)
            {
                image.Frame = AnimDizzyFrame;
            }
            else
            {
                base.updateAnimation();
            }
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
            get { return GameObjectType.FatBird; }
        }

        override public CardType CardCaptureType
        {
            get
            {
                if (characterLevel == 0)
                    return CardType.FatBird1;
                else
                    return CardType.FatBird2;
            }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.pure_red,
            Data.MaterialType.pastel_magenta_red,
            Data.MaterialType.pure_red_orange);
        public static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(
            Data.MaterialType.dark_magenta_red,
            Data.MaterialType.dark_red,
            Data.MaterialType.pure_yellow_orange);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return characterLevel == 0? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return new Vector3(0, 0.9f, 0.10f) * modelScale;
            }
        }

        const float FlySpeedLvl1 = 0.018f;
        const float FlySpeedLvl2 = 0.014f;

        protected override float walkingSpeed
        {
            get
            {
                return characterLevel == 0? FlySpeedLvl1 : FlySpeedLvl2;
            }
        }
    }
}
