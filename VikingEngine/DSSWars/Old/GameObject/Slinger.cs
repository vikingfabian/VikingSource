//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class Slinger: AbsSoldier
//    {
//        public Slinger()
//            : base()
//        { }

//        protected override void initData()
//        {
//            modelScale = StandardModelScale * 0.9f;
//            boundRadius = StandardBoundRadius * 0.9f;

//            walkingSpeed = StandardWalkingSpeed * 1.2f;
//            rotationSpeed = StandardRotatingSpeed;
//            attackRange = 9f;
//            targetSpotRange = attackRange + 1;
//            basehealth = 4;
//            mainAttack = AttackType.SlingShot;
//            attackDamage = 1;
//            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 1.5f;
//            scoutMovement = true;
//        }
//        protected override LootFest.VoxelModelName modelName()
//        {
//            //if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_slingman;
//            //else
//            //    return LootFest.VoxelModelName.little_slingorc;
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.Slinger; }
//        }

//        public override string description
//        {
//            get { return "Long range and quick on their feet, perfect harass unit"; }
//        }
//    }
//}
