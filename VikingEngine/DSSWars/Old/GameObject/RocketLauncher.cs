//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class RocketLauncher: AbsSoldier
//    {
//        const int EmptyFrame = 1;
//        const int AddArrowsFrames = 8;
//        WarmashineWorkerCollection workers;

//        //Sprängs när den går sönder, cirkel av pilar (skadar alla)
//        public RocketLauncher()
//            : base()
//        { }

//        protected override void initData()
//        {
//            modelScale = StandardModelScale * 1.4f;
//            boundRadius = StandardBoundRadius * 1.8f;

//            walkingSpeed = StandardWalkingSpeed * 0.8f;
//            rotationSpeed = StandardRotatingSpeed * 0.08f;
//            walkingWaggleAngle = 0f;

//            attackRange = 10f;
//            targetSpotRange = attackRange + 1;
//            maxAttackAngle = 0.07f;
//            basehealth = 10;
//            mainAttack = AttackType.RocketArrow;
//            attackDamage = 4;
//            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 0.25f;
//            attackSetCount = 8;
//            attackSetCoolDown = StandardAttackAndCoolDownTime * 6;
//            modelToShadowScale = new Vector2(0.4f, 0.85f);
//        }

//        public override void init(Vector3 startPos)
//        {
//            base.init(startPos);

//            const float Xdiff = 0.2f;
//            const float Zdiff = -0.37f;

//            workers = new WarmashineWorkerCollection();

//            workers.Add(player, modelScale * Xdiff, modelScale * Zdiff);
//            workers.Add(player, modelScale * -Xdiff, modelScale * Zdiff);
//        }
        
//        public override void updateAnimation()
//        {
//            if (state.walking)
//            {
//                model.Frame = 0;
//            }
//            else
//            {
//                if (attack.attackCooldownTime.HasTime)
//                {
//                    if (attack.attackSetIndex > 0)
//                    {
//                        model.Frame = EmptyFrame + AddArrowsFrames - attack.attackSetIndex;
//                    }
//                    else
//                    {
//                        float percReloadTime = attack.attackCooldownTime.MilliSeconds / attack.prevAttackTime;

//                        const float ReloadPart = 0.12f;

//                        if (percReloadTime < ReloadPart)
//                        {
//                            percReloadTime /= ReloadPart;
//                            model.Frame = (int)(EmptyFrame + (1f - percReloadTime) * AddArrowsFrames);
//                        }
//                        else
//                        {
//                            model.Frame = EmptyFrame;
//                        }
//                    }
//                }
//                else
//                {
//                    model.Frame = EmptyFrame + AddArrowsFrames;
//                }
//            }

//            DssLib.Rotation1DToQuaterion(model, rotation.Radians);
//            workers.update(this);
//        }

//        protected override LootFest.VoxelModelName modelName()
//        {
//            //if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_rocketlauncherman;
//            //else
//            //    return LootFest.VoxelModelName.little_rocketlauncherorc;
//        }

//        public override void DeleteMe()
//        {
//            base.DeleteMe();
//            workers.DeleteMe();
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.RocketLauncher; }
//        }

//        public override string description
//        {
//            get { return "Creates a rain of arrows"; }
//        }
//    }
//}
