//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class BombCatapult: AbsBallista
//    {
//        public BombCatapult()
//            : base()
//        { }

//        protected override void initData()
//        {
//            //Spec: död ko som förgiftar område
//            modelScale = StandardModelScale * 1.8f;
//            boundRadius = StandardBoundRadius * 2.2f;

//            walkingSpeed = StandardWalkingSpeed * 0.6f;
//            rotationSpeed = StandardRotatingSpeed * 0.04f;
//            walkingWaggleAngle = 0f;

//            basehealth = 10;
//            mainAttack = AttackType.FireBomb;

//            attackRange = 10f;
//            attackDamage = 6;
//            splashDamage = 4;
//            splashRadius = 4f;

//            targetSpotRange = attackRange + 1;
//            maxAttackAngle = 0.07f;
            
//            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 7;
//            modelToShadowScale = new Vector2(0.4f, 0.85f);
//        }
//        protected override LootFest.VoxelModelName modelName()
//        {
//            //if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_bombcatapultman;
//           // else
//             //   return LootFest.VoxelModelName.little_bombcatapultorc;
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.BombCatapult; }
//        }

//        public override string description
//        {
//            get { return "Fire bombs at long range target"; }
//        }
//    }
//}
