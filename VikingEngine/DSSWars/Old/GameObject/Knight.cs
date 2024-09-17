//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class Knight: AbsSoldier
//    {
//        public Knight()
//            : base()
//        { }

//        protected override void initData()
//        {
//            modelScale = StandardModelScale * 1.4f;
//            boundRadius = DssVar.StandardBoundRadius * 1.9f;

//            walkingSpeed = StandardWalkingSpeed * 0.9f;
//            rotationSpeed = StandardRotatingSpeed * 0.8f;
//            targetSpotRange = StandardTargetSpotRange;
//            attackRange = 0.2f;
//            basehealth = 50;
//            shieldDamageReduction = 3;
//            mainAttack = AttackType.Melee;
//            attackDamage = 20;
//            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 1.2f;

//            attackSoundPitch = 0.5f;
//        }
//        protected override LootFest.VoxelModelName modelName()
//        {
//           // if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_knightman;
//            //else
//            //    return LootFest.VoxelModelName.little_knightorc;
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.Knight; }
//        }

//        public override string description
//        {
//            get { return "Powerful unit with a swing that kills most things in one hit"; }
//        }
//    }
//}
