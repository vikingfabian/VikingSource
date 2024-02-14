//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class Dog: AbsSoldier
//    {
//        public Dog()
//            : base()
//        { }

//        protected override void initData()
//        {
//            //Stannar upp efter kill, tuggar på ben, växer sen
//            //Motskill, kasta köttben som dom springer efter
//            modelScale = StandardModelScale * 0.6f;
//            boundRadius = StandardBoundRadius * 0.6f;

//            walkingSpeed = StandardWalkingSpeed * 2f;
//            rotationSpeed = StandardRotatingSpeed;
//            walkingWaggleAngle = 0.1f;
//            targetSpotRange = StandardTargetSpotRange;
//            attackRange = 0.2f;
//            basehealth = 3;
//            mainAttack = AttackType.Melee;
//            attackDamage = 10;
//            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 0.8f;
//            canCaptureArea = false;
//            canAttackStructure = false;
//            willFlee = false;
//            modelToShadowScale = new Vector2(0.4f, 0.7f);
//            scoutMovement = true;
//            attackFrameTime = 250;
//        }
//        protected override LootFest.VoxelModelName modelName()
//        {
//            //if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_dogman;
            
//           // if (player.faction == Faction.Neutral)
//           //     return LootFest.VoxelModelName.war_dogneutral;
            
//           // return LootFest.VoxelModelName.little_dogorc;
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.Dog; }
//        }

//        public override string description
//        {
//            get { return "Super fast, high damage units. Can't pick flags."; }
//        }
//    }
//}
