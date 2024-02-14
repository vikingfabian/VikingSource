//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class Fatman: AbsSoldier
//    {
//        const float IdleBeforeHealingSec = 5f;
//        const float addHpPerSec = 5;

//        float addHealth = 0;
//        Time idleTimer = 0;

//        bool inEatAnimation = false;
//        Timer.Basic eatAnimation = new Timer.Basic(200, true);
//        bool firstEatFrame = true;

//        public Fatman()
//            : base()
//        { }

//        protected override void initData()
//        {
//            //Special, explodera
//            modelScale = StandardModelScale * 1.4f;
//            boundRadius = StandardBoundRadius * 1.6f;

//            walkingSpeed = StandardWalkingSpeed * 0.6f;
//            rotationSpeed = StandardRotatingSpeed * 0.5f;
//            targetSpotRange = StandardTargetSpotRange;
//            attackRange = 0.1f;
//            basehealth = 100;
//            mainAttack = AttackType.Melee;
//            attackDamage = 5;
//            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 1.2f;

//            attackFrame = 4;
//            walkingAnimation.startframe = 5;
//            walkingAnimation.endFrame = 9;

//        }
//        protected override LootFest.VoxelModelName modelName()
//        {
//            //if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_fatman;

//            //else if (player.faction == Faction.Neutral)
//            //    return LootFest.VoxelModelName.little_fatneutral;

//            //else
//            //    return LootFest.VoxelModelName.little_fatorc;
//        }

//        public override void update()
//        {
//            inEatAnimation = false;

//            if (state.idle && health < basehealth)
//            {
//                if (idleTimer.AddGameTime_ReachGoalSeconds(IdleBeforeHealingSec))
//                {
//                    inEatAnimation = true;
                    
//                    addHealth += addHpPerSec * Ref.DeltaGameTimeSec;

//                    int add = (int)addHealth;

//                    health = Bound.Max(health + add, basehealth);
//                    addHealth -= add;

//                    refreshHealthbar();
//                }
//            }
//            else
//            {
//                idleTimer.setZero();

//                //idleFrame = 0;
//                addHealth = 0;
//            }
//            base.update();


//            //if (isIdle)
//            //{
//            //    model.Frame = idleFrame;
//            //}
//        }

//        public override void updateAnimation()
//        {
//            if (inEatAnimation)
//            {
//                model.Frame = firstEatFrame ? 2 : 3;

//                if (eatAnimation.UpdateInGame())
//                {   
//                    lib.Invert(ref firstEatFrame);
//                }
//            }
//            else
//            {
//                base.updateAnimation();
//            }
//        }

//        public override void takeDamage(Damage damage)//int damage, bool isLockedDamage, AttackAnimation fromAttack)
//        {
//            base.takeDamage(damage);//, isLockedDamage, fromAttack);

//            idleTimer.setZero();
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.TankDude; }
//        }

//        public override string description
//        {
//            get { return "Tank with huge amount of health"; }
//        }
//    }
//}
