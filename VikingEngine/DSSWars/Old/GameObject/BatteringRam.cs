//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class BatteringRam: AbsSoldier
//    {
//        WarmashineWorkerCollection workers;

//        public BatteringRam()
//            : base()
//        { }

//        protected override void initData()
//        {
//            //Spec: eldsprutare
//            modelScale = StandardModelScale * 2f;
//            boundRadius = DssVar.StandardBoundRadius * 2.2f;

//            walkingSpeed = StandardWalkingSpeed * 0.85f;
//            rotationSpeed = StandardRotatingSpeed * 0.06f;
//            walkingWaggleAngle = 0f;

//            attackRange = 0.2f;
//            targetSpotRange = StandardTargetSpotRange;
//            basehealth = 14;
//            mainAttack = AttackType.Melee;
//            attackDamage = 20;
//            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 1.5f;
//            modelToShadowScale = new Vector2(0.4f, 0.85f);

//            //hasShield = true;
//            shieldDamageReduction = 4;
//            canAttackCharacters = false;

//            idleBlinkFrame = idleFrame;
//            attackFrame = 1;
//            walkingAnimation = new WalkingAnimation(idleFrame, idleFrame, float.MaxValue);
//            //countFlagCaptures = true;
//            //countGroupKills = true;
//        }

//        public override void init(Vector3 startPos)
//        {
//            base.init(startPos);

//            const float Xdiff = 0.2f;
//            //const float Zdiff = -0.37f;

//            workers = new WarmashineWorkerCollection();

//            workers.Add(player, modelScale * 0.17f, modelScale * -0.06f); //Mitten
//            workers.Add(player, modelScale * -0.17f, modelScale * -0.02f); //Mitten
//            workers.Add(player, modelScale * -0.10f, modelScale * -0.36f); //Längst bak
//        }

//        public override void updateAnimation()
//        {
//            base.updateAnimation();

//            workers.update(this);
//        }

//        protected override LootFest.VoxelModelName modelName()
//        {
//            //if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_ramman;
//           // else
//            //    return LootFest.VoxelModelName.little_ramorc;
//        }

//        public override void DeleteMe()
//        {
//            base.DeleteMe();
//            workers.DeleteMe();
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.BatteringRam; }
//        }

//        public override string description
//        {
//            get { return "Will only attack structures"; }
//        }
//    }
//}
