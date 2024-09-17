//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class Javeliner : AbsSoldier
//    {
//        public Javeliner()
//            : base()
//        { }

//        protected override void initData()
//        {
//            //bygger skyttetorn som extra kort
//            modelScale = StandardModelScale * 0.8f;
//            boundRadius = DssVar.StandardBoundRadius * 0.8f;

//            walkingSpeed = StandardWalkingSpeed * 1.05f;
//            rotationSpeed = StandardRotatingSpeed;

//            attackRange = 3f;
//            targetSpotRange = StandardTargetSpotRange;
//            basehealth = 5;
//            mainAttack = AttackType.Javelin;
//            attackDamage = 3;
//            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 1.5f;
//            //hasShield = true;
//            shieldDamageReduction = 3;
//            turnTowardsDamage = true;
//        }
//        protected override LootFest.VoxelModelName modelName()
//        {
//            //if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_javelinman;
//            //else
//            //    return LootFest.VoxelModelName.little_javelinorc;
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.Javeliner; }
//        }

//        public override string description
//        {
//            get { return "Ranged unit with a shield"; }
//        }

//    }

    
//}
