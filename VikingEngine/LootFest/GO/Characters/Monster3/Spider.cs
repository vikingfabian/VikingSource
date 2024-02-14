using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters.Monster3
{
    class Spider : AbsMonster3
    {
        static readonly IntervalF ScaleRange = new IntervalF(3f, 3.6f);

        public int webCount = 0;
        Vector3 prevWebPlacement = Vector3.Zero;
        protected IntervalF followTimeRange = new IntervalF(4f, 5f);

        public Spider(GoArgs args)
            : base(args)
        {
            createSpiderImage();
            Health = LfLib.StandardEnemyHealth;
            loadBounds();

            if (args.LocalMember)
            {
                createAiPhys();
                aiPhys.SlideUpLengthPerSec = 4;
                aiPhys.rotateSpeedMoving = AiPhysicsLf3.StandardRotateSpeedMoving * 1.2f;
                aiPhys.rotateSpeedStanding = aiPhys.rotateSpeedMoving * 1.4f;
           
                NetworkShareObject();
            }
        }

        virtual protected void createSpiderImage()
        {
            createImage(VoxelModelName.spider1, ScaleRange.GetRandom(), 0, new Graphics.AnimationsSettings(6, 0.4f, 1));
        }

        public void bullSpiderSpawn(Rotation1D dir)
        {
            alwaysFullAttension();
            aiState = AiState.Waiting;
            aiStateTimer.MilliSeconds = 1;
            updateAiMovement();
            Velocity.Set(dir.Radians, walkingSpeed * 1.2f);
            Jump(0.6f);
        }


        protected override void updateAiMovement()
        {
            if (aiStateTimer.CountDown())
            {
                if (aiState == AiState.Waiting)
                {
                    aiStateTimer.Seconds = Ref.rnd.Float(1f, 2f);
                    rotation = Rotation1D.Random();
                    aiState = AiState.Walking;
                }
                else if (aiState == AiState.Walking && hasTarget())
                {
                    aiStateTimer.Seconds = followTimeRange.GetRandom();//Ref.rnd.Float(4f, 5f);
                    aiState = AiState.Follow;
                }
                else
                {
                    aiStateTimer.Seconds = Ref.rnd.Float(1f, 2f);
                    aiState = AiState.Waiting;

                    if (webCount < 3 && (prevWebPlacement - image.position).Length() > 3f)
                    {
                        new VikingEngine.LootFest.GO.WeaponAttack.Monster.SpiderWeb(this);
                        prevWebPlacement = image.position;
                        webCount++;
                    }
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
        
        protected override bool handleCharacterColl(AbsUpdateObj character, Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            aiStateTimer.MilliSeconds = 0;
            return base.handleCharacterColl(character, collisionData, usesMyDamageBound);
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
           Data.MaterialType.light_blue,
           Data.MaterialType.dark_cyan,
           Data.MaterialType.gray_90);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.Spider; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.UnderConstruction; }
        }
        static readonly Vector3 ExpressionEffectScaleToOffset = new Vector3(0, 0.45f, 0.3f);
        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return ExpressionEffectScaleToOffset * modelScale;
            }
        }

        
    }

    
    
    
}
