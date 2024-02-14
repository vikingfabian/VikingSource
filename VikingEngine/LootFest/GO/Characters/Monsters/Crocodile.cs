//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest.GO.Characters.Monsters
//{
//    //jagar en kort stund, måste sen vila
//    class Crocodile : AbsMonster
//    {
//        static readonly IntervalF ScaleRangeLvl1 = new IntervalF(5f, 6f);
//        static readonly IntervalF ScaleRangeLvl2 = new IntervalF(6f, 7f);

//        protected override IntervalF scaleRange
//        {
//            get { return characterLevel == 0? ScaleRangeLvl1 : ScaleRangeLvl2; }
//        }
//        public Crocodile(GoArgs args)
//            : base(args)
//        {
            
//           setHealth( LfLib.StandardEnemyHealth);
//           NetworkShareObject();
//           Data.VoxModelAutoLoad.PreLoadImage(VoxelModelName.enemyattention, false, 0, false);
//        }

//        public Crocodile(System.IO.BinaryReader r)
//            : base(r)
//        {

//        }

        

//        override protected void createBound(float imageScale)
//        {
//            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotatedFromFeet(new Vector3(0.18f * imageScale, 0.22f * imageScale, imageScale * 0.5f), 0f);
//            TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBoundFromFeetPos(imageScale * 0.22f, imageScale * 0.1f, 0f);
//        }

//        public override void AsynchGOUpdate(GO.UpdateArgs args)
//        {
//            basicAIupdate(args);
//            if (localMember)
//            {
//                if (aiStateTimer.MilliSeconds <= 0)
//                {
//                    if (aiState == AiState.PreAttack)
//                    {
//                        Velocity prev = Velocity;
//                        moveTowardsObject(target, 0, runningSpeed);
//                        aiStateTimer.MilliSeconds = 600 + characterLevel * Ref.rnd.Int(400, 800);
//                        aiState = AiState.Attacking;
//                    }
//                    else
//                    {
//                        if (aiState != AiState.Attacking)
//                        {
//                            target = getClosestCharacter(30, args.allMembersCounter, WeaponAttack.WeaponUserType.Enemy);
//                        }
//                        else
//                        {
//                            target = null;
//                        }

//                        if (target == null)
//                        {
//                            aiState = AiState.Waiting;
//                            Velocity.SetZeroPlaneSpeed();
//                            if (Ref.rnd.RandomChance(60))
//                            {
//                                aiState = AiState.Walking;
//                                rotation = Rotation1D.Random();
//                                Velocity.Set( rotation, walkingSpeed);
//                            }
//                            aiStateTimer.MilliSeconds = Ref.rnd.Int(2000, 4000);
//                            setImageDirFromSpeed();
//                        }
//                        else
//                        {
//                            rotation.Radians = AngleDirToObject(target);
//                            setImageDirFromRotation();
//                            preAttackEffectUnthreading();
//                            aiState = AiState.PreAttack;
//                            aiStateTimer.MilliSeconds = preAttackTime;
//                        }
//                    }

//                }
//            }
//        }
//        public override void Time_Update(UpdateArgs args)
//        {
//            base.Time_Update(args);
//            if (aiState == AiState.Attacking)
//            {
//                //slowly turn toward the victim
//                const float TurningSpeed = 0.003f;
//                rotateTowardsObject(target, TurningSpeed);
//                setImageDirFromSpeed();
//            }
            
//        }
//        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
//        {
//            return base.handleCharacterColl(character, collisionData, usesMyDamageBound);
//        }
//        public override void HandleObsticle(bool wallNotPit, AbsUpdateObj ObjCollision)
//        {
//            base.HandleObsticle(wallNotPit, ObjCollision);
//        }
//        protected override VoxelModelName imageName
//        {
//            get { return characterLevel == 0? VoxelModelName.crockodile1 : VoxelModelName.crockodile2; }
//        }

//        static readonly Graphics.AnimationsSettings AnimSet = 
//            new Graphics.AnimationsSettings(6, 0.8f);
//        protected override Graphics.AnimationsSettings animSettings
//        {
//            get { return AnimSet; }
//        }

//        protected override WeaponAttack.DamageData contactDamage
//        {
//            get
//            {
//                return CollisionDamage;
//            }
//        }

//        const float WalkingSpeed = 0.003f;
//        const float RunningSpeedLvl1 = 0.018f;
//        //const float RunningSpeedLvl2 = 0.006f;
//        protected override float walkingSpeed
//        {
//            get { return WalkingSpeed; }
//        }
//        protected override float runningSpeed
//        {
//            get { return RunningSpeedLvl1; }//areaLevel==0? RunningSpeedLvl1 : RunningSpeedLvl2; }
//        }

//        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.CMYK_green, 
//            Data.MaterialType.darker_green, 
//            Data.MaterialType.darker_yellow);
//        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);

//        public override Effects.BouncingBlockColors DamageColors
//        {
//            get
//            {
//                return characterLevel == 0? DamageColorsLvl1 : DamageColorsLvl2;
//            }
//        }

//        public override GameObjectType Type
//        {
//            get { return GameObjectType.Crocodile; }
//        }
//        protected override Monster2Type monsterType
//        {
//            get { return Monster2Type.Crocodile; }
//        }

       

//        protected override Vector3 expressionEffectPosOffset
//        {
//            get
//            {
//                return new Vector3(0, 1, 1.6f);
//            }
//        }
//        public override float LightSourceRadius
//        {
//            get
//            {
//                return image.Scale.X * 11;
//            }
//        }
//        public override float ExspectedHeight
//        {
//            get
//            {
//                return 1;
//            }
//        }
//    }
//}
