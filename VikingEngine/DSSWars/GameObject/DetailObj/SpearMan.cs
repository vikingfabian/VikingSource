//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.DSSWars.GameObject
//{
//    class SpearMan: AbsSoldier
//    {
//        const int WalkingShield = 4;
//        const int FullShield = 8;

//        const float FullShieldDelaySec = 4f;
//        float timeIdle = 0;

//        public SpearMan()
//            : base()
//        { }

//        protected override void initData()
//        {
//            modelScale = StandardModelScale;
//            boundRadius = DssVar.StandardBoundRadius;

//            walkingSpeed = StandardWalkingSpeed * 0.8f;
//            rotationSpeed = StandardRotatingSpeed * 0.5f;
//            targetSpotRange = StandardTargetSpotRange;
//            attackRange = 0.2f;
//            basehealth = 20;
//            mainAttack = AttackType.Melee;
//            attackDamage = 3;
//            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 1.5f;

//            //hasShield = true;
//            idleBlinkFrame = 0;
//            shieldDamageReduction = WalkingShield;
//            turnTowardsDamage = true;
//        }
//        protected override LootFest.VoxelModelName modelName()
//        {
//            //if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_spearman;
//            //else
//            //    return LootFest.VoxelModelName.little_spearorc;
//        }

//        public override void update()
//        {
//            base.update();

//            if (state.idle)
//            {
//                timeIdle += Ref.DeltaGameTimeSec;
//                if (timeIdle >= FullShieldDelaySec)
//                {
//                    shieldDamageReduction = FullShield;
//                }
//            }
//            else
//            {
//                timeIdle = 0;
//                shieldDamageReduction = WalkingShield;
//            }
//        }

        

//        public override void orderHalt()
//        {
//            base.orderHalt();

//            timeIdle = FullShieldDelaySec;
//        }

//        public override void updateAnimation()
//        {
//            base.updateAnimation();

//            if (shieldDamageReduction == FullShield)
//            {
//                model.Frame = 1;
//            }
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.SpearMan; }
//        }

//        public override string description
//        {
//            get { return "Blocks damage in front of them, great against projectiles."; }
//        }
//    }
//}
