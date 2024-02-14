using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters
{
    class Crocodile3 : AbsMonster3
    {
        static readonly IntervalF ScaleRangeLvl1 = new IntervalF(5f, 6f);
        static readonly IntervalF ScaleRangeLvl2 = new IntervalF(6f, 7f);

        public Crocodile3(GoArgs args)
            : base(args)
        {
            createImage(VoxelModelName.crockodile1, ScaleRangeLvl1.GetRandom(), 0, new Graphics.AnimationsSettings(6, 0.8f));
            Health = LfLib.LargeEnemyHealth;
            loadBounds();

            if (args.LocalMember)
            {
                createAiPhys();
                NetworkShareObject();
            }
            //aiPhys.yAdj = -1f;
        }
        //public Crocodile3(System.IO.BinaryReader r)
        //    : base(r)
        //{

        //}

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
                    aiStateTimer.Seconds = Ref.rnd.Float(1.4f, 2f);
                    aiState = AiState.Follow;
                }
                else
                {
                    aiStateTimer.Seconds = Ref.rnd.Float(0.8f, 1.2f);

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


        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.CMYK_green,
            Data.MaterialType.darker_green,
            Data.MaterialType.darker_yellow);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return characterLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.Crocodile; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.Crocodile; }
        }

        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return new Vector3(0, 1, 1.6f);
            }
        }
        public override float LightSourceRadius
        {
            get
            {
                return image.scale.X * 11;
            }
        }
        public override float ExspectedHeight
        {
            get
            {
                return 1;
            }
        }

        protected const float WalkingSpeed = 0.01f;
        const float CasualWalkSpeed = WalkingSpeed * 0.2f;

        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
    }
}
