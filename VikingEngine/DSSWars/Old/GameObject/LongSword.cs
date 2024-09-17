//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class LongSword : AbsSoldier
//    {
//        public LongSword()
//            : base()
//        { }
        
//        protected override void initData()
//        {
//            modelScale = StandardModelScale * 1.5f;
//            modelToShadowScale *= 0.7f;
//            boundRadius = DssVar.StandardBoundRadius;

//            walkingSpeed = StandardWalkingSpeed;
//            rotationSpeed = StandardRotatingSpeed;
//            targetSpotRange = StandardTargetSpotRange;
//            attackRange = 0.35f;
//            basehealth = 6;
//            mainAttack = AttackType.Melee;
//            attackDamage = 50;
//            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 4f;

//            attackSoundPitch = 0.7f;
//        }
//        protected override LootFest.VoxelModelName modelName()
//        {
//            //if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_longswordman;
//            //else
//            //    return LootFest.VoxelModelName.little_longswordorc;
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.LongSword; }
//        }

//        public override string description
//        {
//            get { return "Slow and heavy swings"; }
//        }
//    }
//}