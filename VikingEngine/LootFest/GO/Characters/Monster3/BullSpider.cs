using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters.Monster3
{
    class BullSpider : AbsMonster3
    {
        static readonly IntervalF ScaleRange = new IntervalF(10f, 11f);

        public BullSpider(GoArgs args)
            : base(args)
        {
            createImage(VoxelModelName.bull_spider1, ScaleRange.GetRandom(), 0, new Graphics.AnimationsSettings(7, 0.8f, 2));
            Health = LfLib.StandardEnemyHealth;
            loadBounds();

            Health = 2f;

            //Will only recieve damage from butt
            CollisionAndDefaultBound.Bounds[0].ignoresDamage = true;

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
            else if (aiState == AiState.Walking)
            {
                aiPhys.MovUpdate_MoveForward(rotation, casualWalkSpeed);
            }
            else
            {
                if (target == null)
                {
                    aiPhys.MovUpdate_StandStill();
                }
                else
                {
                    aiPhys.MovUpdate_RotateTowards(target, aiPhys.rotateSpeedStanding);
                }
            }
        }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);

            image.Frame = 1;
            image.position += Vector3.Down * image.Scale1D * 0.1f;

            new Effects.ExplodingBullSpider(this);
        }

        public override void BlockSplatter()
        {
            if (Alive)
            {
                base.BlockSplatter();
            }
            else
            {

            }
        }

        protected const float SpiderWalkingSpeed = 0.010f;
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
           Data.MaterialType.darker_red_orange,
           Data.MaterialType.darker_warm_brown,
           Data.MaterialType.darker_cool_brown);
        
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
            get { return GameObjectType.BullSpider; }
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

        protected override bool autoDeleteImage
        {
            get
            {
                return !IsKilled;
            }
        }

        public override void stunForce(float power, float takeDamage, bool headStomp, bool local)
        {
            //base.stunForce(power, takeDamage, headStomp, local);
        }

        protected override bool pushable
        {
            get
            {
                return false;//base.pushable;
            }
        }
    }
}
