using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters
{
    class Pitbull: AbsMonster3
    {
        static readonly IntervalF ScaleRange = new IntervalF(3.8f, 4f);

        public Pitbull(GoArgs args)
            : base(args)
        {
            createImage(VoxelModelName.pitbull_lvl1, ScaleRange.GetRandom(), 0, new Graphics.AnimationsSettings(5, 0.8f, 1));
            Health = LfLib.StandardEnemyHealth;
            loadBounds();

            if (args.LocalMember)
            {
                createAiPhys();
                NetworkShareObject();
                //aiPhys.yAdj = -1f;
            }
        }

       
        protected override void updateAiMovement()
        {
            
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
                    aiStateTimer.Seconds = Ref.rnd.Float(2f, 2.2f);

                    aiState = AiState.Waiting;
                }
            }


            if (aiState == AiState.Follow)
            {
                aiPhys.MovUpdate_MoveTowards(storedTarget, 2f, walkingSpeed);
            }
            else if (aiState == AiState.Walking)
            {
                aiPhys.MovUpdate_MoveForward(rotation, casualWalkSpeed);                
            }
            else
            {
                aiPhys.MovUpdate_StandStill();
            }
        }

        protected const float WalkingSpeed = 0.009f;
        const float CasualWalkSpeed = WalkingSpeed * 0.5f;
        
        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }


        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
           Data.MaterialType.light_warm_brown,
            Data.MaterialType.gray_70);
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
            get { return GameObjectType.Pitbull; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.Pitbull; }
        }

        static readonly Vector3 ExpressionEffectScaleToOffset = new Vector3(0, 0.55f, 0.26f);
        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return ExpressionEffectScaleToOffset * modelScale;
            }
        }
    }
}
