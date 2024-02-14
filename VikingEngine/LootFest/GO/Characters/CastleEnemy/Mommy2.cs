//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Physics;

//namespace VikingEngine.LootFest.GO.Characters.CastleEnemy
//{
//    /// <summary>
//    /// Mommy version2, can move more freely
//    /// </summary>
//    class Mommy2 : AbsMonster
//    {
//        static readonly IntervalF StateTimeRange = new IntervalF(1200, 2000);
//        const float ScaleToBound = 0.35f;
//        Rotation1D goalDir;

//        static readonly IntervalF TargetCheckTimeRange = new IntervalF(lib.SecondsToMS(2), lib.SecondsToMS(10));
//        Time targetCheck = new Time(TargetCheckTimeRange.GetRandom());


//        public Mommy2(Map.WorldPosition startPos, int level)
//            : base(startPos, level)
//        {
//            Health = LfLib.StandardEnemyHealth;
//            NetworkShareObject();
//            aiState = AiState.Walking;
//        }

//        public Mommy2(System.IO.BinaryReader r)
//            : base(r)
//        {
//        }
//        override protected void createBound(float imageScale)
//        {
//            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.24f * imageScale, 0.36f * imageScale, imageScale * 0.31f));
//            TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
//        }

//        static readonly IntervalF FireDistance = new IntervalF(6, 16);
//        public override void AsynchGOUpdate(UpdateArgs args)
//        {
           
//            //base.AIupdate(args);
//            if (aiStateTimer.CountDown(args.time))
//            {
//                switch (aiState)
//                {
//                    case AiState.Walking:
//                        if (targetCheck.CountDown(args.time))
//                        {
//                            targetCheck = new Time(TargetCheckTimeRange.GetRandom());
//                            target = GetClosestHero();
//                            if (FireDistance.IsWithinRange(distanceToObject(target)) &&
//                                Math.Abs(angleDiff(target)) < MathHelper.PiOver2)
//                            {
//                                aiState = AiState.PreAttack;

//                                int numAttacks = Ref.rnd.Int(2, 3);
//                                const float PreAttackTime = 600;
//                                const float AttackTime = 400;

//                                aiStateTimer.MilliSeconds = PreAttackTime + numAttacks * AttackTime;
//                                new Timer.RepeatingActionTrigger(createMoth, PreAttackTime, AttackTime, numAttacks);
//                                return;
//                            }

//                        }
//                        turn();
//                        break;
//                    case AiState.PreAttack:
//                        aiState = AiState.AttackComplete;
//                        aiStateTimer.MilliSeconds = StateTimeRange.GetRandom();
//                        break;
//                }
//            }
//        }

//        void createMoth()
//        {

//        }

//        public override void Time_Update(UpdateArgs args)
//        {

//            switch (aiState)
//            {
//                case AiState.Walking:
//                    rotateTowardsGoalDir(goalDir.Radians, 0.01f, args.time);
//                    setImageDirFromSpeed();
//                    break;
//                case AiState.PreAttack:
//                    Velocity.SetZeroPlaneSpeed();
                    
//                    break;
//                case AiState.AttackComplete:
//                    aiState = AiState.Walking;
//                    Velocity.Set(rotation, walkingSpeed);
//                    break;
//            }
            
//            base.Time_Update(args);
//            if (aiState != AiState.Walking) image.Currentframe = 1;
//        }

//        void turn()
//        {
            
//            goalDir.Radians = rotation.Radians + MathHelper.PiOver2 * Ref.rnd.Dir();
//            Velocity.Set(rotation, walkingSpeed);
//            aiStateTimer.MilliSeconds = StateTimeRange.GetRandom();
//        }
//        protected override VoxelModelName imageName
//        {
//            get { return VoxelModelName.mommy; }
//        }


//        protected override Graphics.AnimationsSettings animSettings
//        {
//            get { return new Graphics.AnimationsSettings(7, 0.5f, 4); }
//        }

//        const float WalkingSpeedLvl1 = 0.0052f;
//        const float LvlSpeedAdd = 0.0004f;
//        protected override float walkingSpeed
//        {
//            get { return WalkingSpeedLvl1 + LvlSpeedAdd * characterLevel; }
//        }
//        protected override float runningSpeed
//        {
//            get { return WalkingSpeedLvl1; }
//        }

//        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
//        public override Effects.BouncingBlockColors DamageColors
//        {
//            get
//            {
//                return DamageColorsLvl1;
//            }
//        }

//        public override GameObjectType Type
//        {
//            get { return GameObjectType.Mummy; }
//        }
//        protected override Monster2Type monsterType
//        {
//            get { return Monster2Type.Mummy; }
//        }

//        static readonly IntervalF ScaleRange = new IntervalF(5, 5.2f);
//        protected override IntervalF scaleRange
//        {
//            get { return ScaleRange; }
//        }

//        protected override WeaponAttack.DamageData contactDamage
//        {
//            get
//            {
//                return CollisionDamage;
//            }
//        }

//        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
//        {
//            base.HandleColl3D(collData, ObjCollision);
//            if (ObjCollision == null)
//            {
//                goalDir = rotation;
//            }
//            targetCheck = new Time(TargetCheckTimeRange.GetRandom());
//        }

//        public override void HandleObsticle(bool wallNotPit, AbsUpdateObj ObjCollision)
//        {
//            base.HandleObsticle(wallNotPit, ObjCollision);
//            rotation.Add(MathHelper.Pi + Ref.rnd.Plus_MinusF(1));
//            goalDir = rotation;
//            targetCheck = new Time(TargetCheckTimeRange.GetRandom());
//        }

     

//        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
//        {//Immune to evil magic
//            return damage.Magic != Magic.MagicElement.Evil && base.willReceiveDamage(damage);
//        }
//        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
//        {
//            targetCheck = new Time(TargetCheckTimeRange.GetRandom());
//            base.handleDamage(damage, local);
//        }
//        protected override int MaxLevel
//        {
//            get
//            {
//                return 8;
//            }
//        }
//        //public override ObjPhysicsType PhysicsType
//        //{
//        //    get
//        //    {
//        //        return  ObjPhysicsType.Character2;
//        //    }
//        //}
//    }
   
//}
