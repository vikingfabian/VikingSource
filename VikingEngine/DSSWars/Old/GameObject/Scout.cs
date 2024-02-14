//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class Scout: AbsSoldier
//    {
//        public Scout()
//            : base()
//        { }

//        protected override void initData()
//        {
//            modelScale = StandardModelScale * 0.85f;
//            boundRadius = StandardBoundRadius * 0.85f;

//            walkingSpeed = StandardWalkingSpeed * 1.5f;
//            rotationSpeed = StandardRotatingSpeed;
//            attackRange = 2f;
//            targetSpotRange = StandardTargetSpotRange;
//            basehealth = 4;
//            mainAttack = AttackType.KnifeThrow;
//            attackDamage = 4;
//            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 1.6f;
//            scoutMovement = true;
//        }
//        protected override LootFest.VoxelModelName modelName()
//        {
//           // if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_scoutman;
//           // else
//            //    return LootFest.VoxelModelName.little_scoutorc;
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.Scout; }
//        }

//        public override string description
//        {
//            get { return "Very fast, but useless in combat"; }
//        }
//    }
//}
