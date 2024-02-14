using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.GO.Characters.Boss
{
    class GoblinSpiderRiderMiniBoss : GoblinBerserk
    {
        public GoblinSpiderRiderMiniBoss(GoArgs args)
            : base(args)
        {
            if (localMember)
            {
                //this.subLevel = subLevel;
                alwaysFullAttension();

                var mount = new SpiderBoss(args);
                MountAnimal(mount);

            }
        }

        public override GameObjectType Type
        {
            get
            {
                return GameObjectType.GoblinSpiderRiderMiniBoss;
            }
        }
        

    }


    //class GoblinSpiderRiderBoss : GoblinBerserk
    //{
    //    public GoblinSpiderRiderBoss(GoArgs args, BlockMap.AbsLevel level)
    //        : base(args)
    //    {
    //        if (localMember)
    //        {
    //            //this.subLevel = subLevel;
    //            alwaysFullAttension();

    //            var mount = new SpiderBoss(args);
    //            MountAnimal(mount);

    //            if (level != null)
    //            {
    //                var manager = new Director.BossManager(this, level, Players.BabyLocation.);
    //                manager.addBossObject(mount, false);
    //            }
    //        }
    //    }


    //    protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
    //    {
    //        if (isMounted)
    //        {
    //            return false;
    //        }
    //        return base.willReceiveDamage(damage);
    //    }
    //}

    //spider boss, 9 frames = 1: jump attack, 2: pre proj att, 3: proj att 4-8 anim
    class SpiderBoss: AbsMonster3
    {
        const float SpiderBossScale = 10f;
     
        Vector3 prevWebPlacement = Vector3.Zero;

        public SpiderBoss(GoArgs args)
            : base(args)
        {
            createImage(VoxelModelName.spiderboss, SpiderBossScale, 0, new Graphics.AnimationsSettings(9, 0.8f, 4));
            Health = LfLib.StandardEnemyHealth;
            loadBounds();
            Health = LfLib.BossEnemyHealth;

            if (args.LocalMember)
            {
                createAiPhys();
                aiPhys.SlideUpLengthPerSec = 4;
                aiPhys.rotateSpeedMoving = AiPhysicsLf3.StandardRotateSpeedMoving * 1.2f;
                aiPhys.rotateSpeedStanding = aiPhys.rotateSpeedMoving * 1.4f;
           
                NetworkShareObject();
            }
        }

        public void bullSpiderSpawn(Rotation1D dir)
        {
            attentionStatus = HumanoidEnemyAttentionStatus.FoundHero_5;
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
                    aiStateTimer.Seconds = Ref.rnd.Float(4f, 5f);
                    aiState = AiState.Follow;
                }
                else
                {
                    aiStateTimer.Seconds = Ref.rnd.Float(1f, 2f);

                    aiState = AiState.Waiting;

                    if ((prevWebPlacement - image.position).Length() > 6f)
                    {
                        int webCount = 6;
                        Rotation1D webDir = rotation;


                        for (int i = 0; i < webCount; ++i)
                        {
                            new VikingEngine.LootFest.GO.WeaponAttack.Monster.SpiderWeb(new GoArgs(VectorExt.V2toV3XZ(webDir.Direction(8f)) + image.position, 0));
                            webDir.Add(MathHelper.TwoPi / webCount);
                        }
                        prevWebPlacement = image.position;
                        
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

        protected override Vector3 mountSaddlePos()
        {
            return calcSaddlePos(0.4f, 4f, 0.5f);
        }
        
        
        protected override bool handleCharacterColl(AbsUpdateObj character, Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            aiStateTimer.MilliSeconds = 0;
            return base.handleCharacterColl(character, collisionData, usesMyDamageBound);
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.pure_red_orange, 
            Data.MaterialType.darker_red_orange, 
            Data.MaterialType.dark_magenta_red);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SpiderBoss; }
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
