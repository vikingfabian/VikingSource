using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters
{
    class Hog3 : AbsMonster3
    {
        static readonly IntervalF ScaleRange = new IntervalF(4f, 5.5f);

        public Hog3(GoArgs args)
            : this(args, VoxelModelName.hog_v2_lvl1, ScaleRange)
        {
            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        public Hog3(GoArgs args, VoxelModelName modelName, IntervalF scaleRange)
            : base(args)
        {
            createImage(modelName, scaleRange.GetRandom(), 0, new Graphics.AnimationsSettings(6, 0.8f, 2));
            Health = LfLib.LargeEnemyHealth;
            loadBounds();

            if (args.LocalMember)
            {
                createAiPhys();
            }
        }
     
        protected override void updateAiMovement()
        {
            //if (aiPhys != null)
            //{
            if (aiStateTimer.CountDown())
            {
                if (aiState == AiState.Waiting)
                {
                    aiStateTimer.Seconds = Ref.rnd.Float(2f, 4f);
                    rotation = Rotation1D.Random();
                    aiState = AiState.Walking;
                }
                else if (aiState == AiState.Walking && Ref.rnd.Chance(70) && hasTarget())
                {
                    aiStateTimer.Seconds = Ref.rnd.Float(1.8f, 2.4f);
                    aiState = AiState.Follow;
                }
                else
                {
                    aiStateTimer.Seconds = Ref.rnd.Float(1f, 2f);

                    aiState = AiState.Waiting;
                }
            }


            if (aiState == AiState.Follow)
            {
                aiPhys.MovUpdate_MoveTowards(storedTarget, 2f, walkingSpeed);
            }
            else if (aiState == AiState.Attacking)
            {
                aiPhys.MovUpdate_MoveTowards(storedTarget, 2f, rushSpeed);
            }
            else if (aiState == AiState.Walking)
            {
                aiPhys.MovUpdate_MoveForward(rotation, casualWalkSpeed);
            }
            else
            {
                aiPhys.MovUpdate_StandStill();
            }
            //}
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
           Data.MaterialType.darker_yellow_orange,
           Data.MaterialType.darker_red,
           Data.MaterialType.dark_cool_brown);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);

        protected override bool handleCharacterColl(AbsUpdateObj character, Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            aiStateTimer.MilliSeconds = 0;
            return base.handleCharacterColl(character, collisionData, usesMyDamageBound);
        }

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return characterLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.Hog; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.Hog; }
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
