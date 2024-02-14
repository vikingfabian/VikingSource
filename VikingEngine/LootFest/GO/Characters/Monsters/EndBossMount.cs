//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//namespace VikingEngine.LootFest.GO.Characters.Monsters
//{
//    class EndBossMount : AbsMonster2
//    {
//        /*
//         * Stannar för att skjuta eld, som en eldkastare
//         * Stannar, och tar ett språng framåt
//         * Jagar spelare
//         * Går slumpmässigt
//         */
//        static readonly IntervalF magicianFireRateSec = new IntervalF(6, 10);
//        Time magicianStateTime = new Time(magicianFireRateSec.GetRandom(), TimeUnit.Seconds);
//        Graphics.AbsVoxelObj magicianImg;
//        WeaponAttack.DamageData CollDamage = new WeaponAttack.DamageData(LootfestLib.BossMountCollDamage, WeaponAttack.WeaponUserType.Enemy, ByteVector2.Zero);
//        const float Scale = 10;
//        static readonly IntervalF ScaleRange = new IntervalF(Scale, Scale);
//        const float ScaleToBound = 0.32f;
//        protected override IntervalF scaleRange
//        {
//            get { return ScaleRange; }
//        }
//        public EndBossMount(Map.WorldPosition startPos)
//            : base(startPos, 0)
//        {
//            setHealth(LootfestLib.BossMountHealth);
//            NetworkShareObject();
//            basicinit();
//        }
//        public EndBossMount(System.IO.BinaryReader r)
//            : base(r)
//        {
//            basicinit();
//        }

//        void basicinit()
//        {
//            magicianImg = LootFest.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelObjName.end_magician_riding,
//                TempCharacterImage, 2, 1,
//                new Graphics.AnimationsSettings(2,float.MaxValue, 2));
//        }
//        override protected void createBound(float imageScale)
//        {
//            CollisionBound = LootFest.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.3f * imageScale, 0.5f * imageScale, imageScale * 0.5f));
//            TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * ScaleToBound, imageScale * 0.5f);
//        }
//        Vector3 magicianPosDiff = new Vector3(0, 2, 0.5f);
//        public override void Time_Update(UpdateArgs args)
//        {
//            const float RotateSpeed = 0.002f;

//            base.Time_Update(args);
//            magicianImg.Position = image.Rotation.TranslateAlongAxis(magicianPosDiff, image.Position);
//            magicianImg.Rotation = image.Rotation;

//            if (aiState == AiState.Follow)
//            {
//                rotateTowardsObject(target, RotateSpeed);
//                setImageDirFromRotation();
//            }
//            else if (aiState == AiState.Attacking)
//            {
//                image.Currentframe = 1;
//            }
//            else if (aiState == AiState.RotatingTowardsGoal)
//            {
//                rotateTowardsObject(target, RotateSpeed);
//                setImageDirFromRotation();
//            }

//            if (magicianStateTime.CountDown())
//            {
//                magicianStateTime.Seconds = magicianFireRateSec.GetRandom();

//                if (Ref.rnd.RandomChance(70))
//                {
//                    GO.AbsUpdateObj closeTarget = getClosestCharacter(64, args.allMembersCounter, WeaponAttack.WeaponUserType.Enemy);
//                    if (closeTarget != null)
//                    {
//                        new WeaponAttack.Boss.NecroBall(this, magicianImg.Position, closeTarget);
//                    }
//                }
//                else
//                {
//                    WeaponAttack.Boss.NecroMetroid.MetroidRain(magicianImg.Position, rotation);
//                }
//            }
//        }

//        protected override void updateAnimation()
//        {
//            if (aiState == AiState.Attacking)
//            {
//                image.Currentframe = 1;
//            }
//            else if (aiState == AiState.RotatingTowardsGoal)
//            {
//                image.UpdateAnimation(WalkingSpeed, Ref.DeltaTimeMs);
//            }
//            else
//            {
//                base.updateAnimation();
//            }
//        }

//        IntervalF rotateTowardTargetTime = new IntervalF(600, 900);

//        public override void AIupdate(GO.UpdateArgs args)
//        {
//            basicAIupdate(args);
//            if (localMember)
//            {
//                if (aiStateTimer.MilliSeconds <= 0)
//                {
//                    AI.SelectRandomState rndState = null;
//                    switch (aiState)
//                    {
//                        default:
//                            aiState = AiState.Waiting;
//                            aiStateTimer.MilliSeconds = 600 + Ref.rnd.Int(1200);
//                            randomWalkStateEvent();

//                            break;
//                        case AiState.Waiting:
//                            rndState = new AI.SelectRandomState(
//                                new List<AI.AIstate>{
//                                    new AI.AIstate((int)AiState.Walking, 5, new IntervalF(1000, 4000), randomWalkStateEvent),
//                                    new AI.AIstate((int)AiState.RotatingTowardsGoal, 2, rotateTowardTargetTime, waitStateEvent),
//                                });
//                            target = getRndCharacterWithinView(32, 0.4f, args.allMembersCounter, this.WeaponTargetType);
//                            if (target != null)
//                            {
//                                rndState.AddState(new AI.AIstate((int)AiState.PreAttack, 15, new IntervalF(900, 1100), waitStateEvent));
//                                rndState.AddState(new AI.AIstate((int)AiState.PreRush, 15, new IntervalF(600, 800), waitStateEvent));
//                            }

//                            break;

//                        case AiState.Walking:
//                            rndState = new AI.SelectRandomState(
//                                new List<AI.AIstate>{
//                                    new AI.AIstate((int)AiState.Follow, 2, new IntervalF(600, 2000), null),
//                                    new AI.AIstate((int)AiState.RotatingTowardsGoal, 2, rotateTowardTargetTime, waitStateEvent),
//                                    new AI.AIstate((int)AiState.Waiting, 4, new IntervalF(600, 900), waitStateEvent)
//                                });

//                            //state = Monster2State.Waiting;
//                            //Speed.SetZeroPlaneSpeed();
//                            //stateTime = 600 + Ref.rnd.Int(1600);
//                            break;
//                        case AiState.PreAttack:
//                            aiState = AiState.Attacking;
//                            aiStateTimer.MilliSeconds = 1000 + Ref.rnd.Int(200);
//                            new GameObjectEventTrigger(fireAttackStateEvent, this);
                            
//                            break;
//                        case AiState.PreRush:
//                            aiState = AiState.Rush;
//                            aiStateTimer.MilliSeconds = 800 + Ref.rnd.Int(200);
//                            moveTowardsObject(target, 2, RunningSpeed);
//                            break;
//                    }

//                    if (rndState != null)
//                    {
//                        aiState = (AiState)rndState.GetRandomAndRunEvent(ref aiStateTimer.MilliSeconds);

//                        if (aiState == AiState.RotatingTowardsGoal)
//                        {
//                            target = getClosestCharacter(64, args.allMembersCounter, this.WeaponTargetType);
//                        }
//                    }
                    
//                    setImageDirFromRotation();
//                }
                
//            }

//        }

//        void waitStateEvent()
//        {
//            Velocity.SetZeroPlaneSpeed();
//        }
//        void followStateEvent()
//        {
//            Velocity.Set(rotation, WalkingSpeed);
//        }
//        void fireAttackStateEvent()
//        {
//            Vector3 pos = image.Position + new Vector2toV3(rotation.Direction(4), 1);
//            new WeaponAttack.Monster.FireBreath(pos, rotation, aiStateTimer.MilliSeconds);
//        }
//        void randomWalkStateEvent()
//        {
//            rotation.Add(Ref.rnd.Plus_MinusF(1.5f));
//            Velocity.Set(rotation, WalkingSpeed);
//        }
//        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
//        {
//            if (localMember)
//            {
//                //new EndMagician(magicianImg.Position, rotation);
//            }
//            base.DeathEvent(local, damage);
//        }
//        public override void DeleteMe(bool local)
//        {
//            base.DeleteMe(local);
//            magicianImg.DeleteMe();
//        }
//        protected override bool pushable
//        {
//            get
//            {
//                return false;
//            }
//        }
//        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
//        {
//            base.handleDamage(damage, local);
//        }

//        protected override VoxelObjName imageName
//        {
//            get { return VoxelObjName.endbossmount; }
//        }

//        static readonly Graphics.AnimationsSettings AnimSet =
//            new Graphics.AnimationsSettings(7, 0.8f, 2);
//        protected override Graphics.AnimationsSettings animSettings
//        {
//            get { return AnimSet; }
//        }


//        const float WalkingSpeed = 0.012f;
//        const float RunningSpeed = 0.022f;
//        protected override float walkingSpeed
//        {
//            get { return WalkingSpeed; }
//        }
//        protected override float runningSpeed
//        {
//            get { return RunningSpeed; }
//        }
//        static readonly Effects.BouncingBlockColors DamageCols = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);

//        public override Effects.BouncingBlockColors DamageColors
//        {
//            get
//            {
//                return DamageCols;
//            }
//        }
//        public override GameObjectType Type
//        {
//            get { return GameObjectType.EndBossMount; }
//        }
//        protected override Monster2Type monsterType
//        {
//            get { return Monster2Type.EndBossMount; }
//        }
//        protected override WeaponAttack.DamageData contactDamage
//        {
//            get
//            {
//                return CollDamage;
//            }
//        }

      
//    }
//}
