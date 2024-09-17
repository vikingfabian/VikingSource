//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class Farmer: AbsSoldier
//    {
//        static readonly Time SpawnPigTimer = new Time(5, TimeUnit.Seconds);
//        public static readonly float FarmerWalkingSpeed = StandardWalkingSpeed * 0.9f;
//        public const int FarmerHealth = 3;

//        bool pigCountLessThanFarmers = true;

//        FarmerPig pig = null;

//        public Farmer()
//            : base()
//        { }

//        protected override void initData()
//        {
//            //vid idle, spawna grisar, skickas iväg vid move
//            //kort för att sätta eld på grisarna

//            modelScale = StandardModelScale * 0.9f;
//            boundRadius = DssVar.StandardBoundRadius * 0.9f;

//            walkingSpeed = FarmerWalkingSpeed;
//            rotationSpeed = StandardRotatingSpeed;
//            targetSpotRange = StandardTargetSpotRange;
//            attackRange = 0.15f;
//            basehealth = 3;
//            mainAttack = AttackType.Melee;
//            attackDamage = 4;
//            attackTimePlusCoolDown = StandardAttackAndCoolDownTime;

//            //group.groupWaitingTimer = SpawnPigTimer;
//        }
//        protected override LootFest.VoxelModelName modelName()
//        {
//            //if (player.faction == Faction.Human)
//                return LootFest.VoxelModelName.little_farmerman_v2;
            
//           // if (player.faction == Faction.Neutral)
//            //    return LootFest.VoxelModelName.war_farmerneutral;
            
//            //return LootFest.VoxelModelName.little_farmerorc_v2;
//        }

//        public override void update()
//        {
//            if (localMember && isGroupLeader)
//            {
//                if (pigCountLessThanFarmers && group.allSoldiersAreIdle) //&& summoningSickness == null)
//                {
//                    group.groupWaitingTime.MilliSeconds += Ref.DeltaGameTimeMs;

//                    if (group.groupWaitingTime.MilliSeconds >= SpawnPigTimer.MilliSeconds)
//                    {
//                        group.groupWaitingTime.MilliSeconds -= SpawnPigTimer.MilliSeconds;

//                        group.soldiersCounter.Reset();
//                        while (group.soldiersCounter.Next())
//                        {
//                            if (group.soldiersCounter.sel is Farmer)
//                            {
//                                var farmer = (Farmer)group.soldiersCounter.sel;
//                                if (farmer.pig == null || farmer.pig.Dead)
//                                {
//                                    Vector3 pos = farmer.model.position + VectorExt.V2toV3XZ(farmer.rotation.Direction(0.6f));
//                                    farmer.pig = group.createUnit(UnitType.FarmerPig, pos) as FarmerPig;
//                                    break;
//                                }
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    group.groupWaitingTime = 0;
//                }
//            }
//            base.update();
//        }

//        public override void asynchUpdate()
//        {
//            base.asynchUpdate();

//            if (localMember)
//            {
//                int pigs = 0;
//                int farmers = 0;

//                var counter = group.soldiersCounter.Clone();
//                while (counter.Next())
//                {
//                    switch (counter.sel.Type)
//                    {
//                        case UnitType.Farmer:
//                            farmers++;
//                            break;
//                        case UnitType.FarmerPig:
//                            pigs++;
//                            break;

//                    }
//                }

//                pigCountLessThanFarmers = pigs < farmers;
//            }
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.Farmer; }
//        }

//        public override string description
//        {
//            get { return "Cheap cannon fodder"; }
//        }
//    }

//    class FarmerPig : AbsSoldier
//    {
//        public FarmerPig()
//            : base()
//        { }

//        protected override void initData()
//        {
//            modelScale = StandardModelScale * 0.45f;
//            boundRadius = DssVar.StandardBoundRadius * 0.6f;

//            walkingSpeed = Farmer.FarmerWalkingSpeed * 1.4f;
//            rotationSpeed = StandardRotatingSpeed;
//            targetSpotRange = StandardTargetSpotRange;
//            attackRange = 0;
//            basehealth = Farmer.FarmerHealth;
//            mainAttack = AttackType.NUM_NON;
//            attackDamage = 0;
//            attackTimePlusCoolDown = -1;

//            canAttackCharacters = false;
//            canAttackStructure = false;
//            canCaptureArea = false;

//            walkingAnimation.startframe = 2;
//            walkingAnimation.endFrame = 4;

//        }
//        protected override LootFest.VoxelModelName modelName()
//        {
//            // if (player.faction == Faction.Human)
//            return LootFest.VoxelModelName.Apple;//.war_pigman;

//            //if (player.faction == Faction.Neutral)
//            //    return LootFest.VoxelModelName.war_pigneutral;

//            //return LootFest.VoxelModelName.war_pigorc;
//        }

//        public override UnitType Type
//        {
//            get { return UnitType.FarmerPig; }
//        }

//        public override string description
//        {
//            get { return "Cheap cannon fodder"; }
//        }
//    }
//}
