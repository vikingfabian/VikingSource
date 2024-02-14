//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class Viking: AbsSoldier
//    {
//        const float SpeedUpTimeSec = 5;
//        const float WalkingSpeedUp = 2f;
//        const float AttackSpeedUp = 2f;

//        int rageState_0Non_1Up_2Down = 0;

//        const float SpeedDownTimeSec = SpeedUpTimeSec;
//        const float WalkingSpeedDown = 0.75f;
//        const float AttackSpeedDown = 0.75f;

//        Time rageStateTimer;

//        public Viking()
//            : base()
//        { }

//        protected override void initData()
//        {
//            modelScale = StandardModelScale;
//            boundRadius = StandardBoundRadius;

//            walkingSpeed = StandardWalkingSpeed * 1.3f;
//            rotationSpeed = StandardRotatingSpeed;
//            targetSpotRange = StandardTargetSpotRange;
//            attackRange = 0.1f;
//            basehealth = 10;
//            mainAttack = AttackType.Melee;
//            attackDamage = 10;
//            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 0.8f;
//            willFlee = false;
//        }
//        protected override LootFest.VoxelModelName modelName()
//        {
//            //if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_vikingman;
//            //else
//            //    return LootFest.VoxelModelName.little_vikingorc;
//        }

//        public override void update()
//        {
//            //if (summoningSickness == null)
//            //{
//            //    if (rageState_0Non_1Up_2Down == 1)
//            //    {
//            //        if (Ref.TimePassed16ms)
//            //        {
//            //            Vector3 firePos = model.position;
//            //            firePos.Y += modelScale * 0.7f;
//            //            Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.Fire, firePos, modelScale * 0.1f, 2);
//            //        }

//            //        if (rageStateTimer.CountDownGameTime())
//            //        {
//            //            rageState_0Non_1Up_2Down = 2;
                        
//            //            rageStateTimer.Seconds = SpeedDownTimeSec;
//            //            walkingSpeedMultiplier = WalkingSpeedDown;

//            //            var tired = addCondition(Battle.UnitConditionType.Tired);
//            //            tired.coolDown.Seconds = SpeedDownTimeSec;
//            //        }
//            //    }
//            //    else if (rageState_0Non_1Up_2Down == 2)
//            //    {
//            //        if (rageStateTimer.CountDownGameTime())
//            //        {
//            //            rageState_0Non_1Up_2Down = 0;
                        
//            //            walkingSpeedMultiplier = 1f;
//            //            removeCondition(Battle.UnitConditionType.Tired);
//            //        }
//            //    }
//            //}
//            base.update();
//        }
        
//        public override void onEvent(UnitEventType type)
//        {
//            if (type == UnitEventType.MoveOrder)
//            {
//                if (rageState_0Non_1Up_2Down == 0)
//                {
//                    rageState_0Non_1Up_2Down = 1;
//                    rageStateTimer.Seconds = SpeedUpTimeSec;
//                    walkingSpeedMultiplier = WalkingSpeedUp;
//                }
//            }
//            base.onEvent(type);
//        }

//        //public override float getAttackTimeModifiers()
//        //{
//        //    float result = base.getAttackTimeModifiers();

//        //    if (rageState_0Non_1Up_2Down == 1)
//        //    {
//        //        result *= AttackSpeedUp;
//        //    }
//        //    else if (rageState_0Non_1Up_2Down == 2)
//        //    {
//        //        result *= AttackSpeedDown;
//        //    }

//        //    return result;
//        //}

//        public override UnitType Type
//        {
//            get { return UnitType.Viking; }
//        }

//        public override string description
//        {
//            get { return "Fast and hard hitting, low health."; }
//        }
//    }
//}
