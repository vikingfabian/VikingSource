//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;
//using VikingEngine.Graphics;

//namespace VikingEngine.LootFest.GO.Characters
//{
//    /// <summary>
//    /// OLD version
//    /// </summary>
//    class Orc : AbsEnemy
//    {
//        //springer efter spelaren, 
//        //stannar upp i defens när han kommer nära, 
//        //på vapen avstånd gör han utfall
//        //cirkulerar åt sidorna
//        const float WalkingSpeed = 0.01f;
//        static readonly IntervalF BasicScale = new IntervalF(2.6f, 2.9f);
//        const int SwordAttackTime = 500;
//        const int PreFireBowTime = 500;
//        static readonly Range BowFireTime = new Range(800, 1200);
        
//        HumanoidType htype;
//        Rotation1D preFireRotation;
//        float attackingDist;
//        float swordScale;
//        Effects.VisualBow visualBow;
//        bool leaderBoost = false;
//        public bool LeaderBoost
//        {
//            set
//            {
//                //if (leaderBoost != value)
//                //{
//                //    leaderBoost = value;
//                //    if (leaderBoost)
//                //    {
//                //        armor += 1;
//                //    }
//                //    else
//                //    {
//                //        armor -= 1;
//                //    }
//                //    updateDamage();
//                //}
//            }
//        }

//        WeaponAttack.DamageData wepDamage;
//        Vector3 targetPos;
//        List<Orc> group = null;

//        public Orc(GoArgs args, HumanoidType htype, List<Orc> group)
//            : base(args)
//        {
//            //this.areaLevel = level;
//            WorldPos = args.startWp;
//            this.group = group;
//            this.htype = htype;
//            target = null;
//            float scale;
//            humanoidBasicInit(out scale);
//            Health = 1;
//            image.Position = args.startWp.PositionV3;
//            image.Position.Y = Map.WorldPosition.ChunkStandardHeight;

//            NetworkShareObject();
//        }
//        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
//        {
//            base.ObjToNetPacket(writer);
//            writer.Write((byte)htype);
//            writer.Write((byte)characterLevel);
//        }
//        public Orc(System.IO.BinaryReader r)
//            : base(r)
//        {
//            htype = (HumanoidType)r.ReadByte();
//            characterLevel = r.ReadByte();
//            float scale;
//            humanoidBasicInit(out scale);

//            //Health *= 2;
//        }
        

//        void humanoidBasicInit(out float scale)
//        {
//            VoxelModelName imgType = characterLevel == 0? VoxelModelName.orch : VoxelModelName.orc2;

//            scale = BasicScale.GetRandom();
            
//            //armor = LootfestLib.HumanoidStandardArmor;
//            attackingDist = 10;
//            updateDamage();

//            switch (htype)
//            {
//                default:
//                    swordScale = 0.14f;
//                    break;
//                case HumanoidType.Leader:
//                    swordScale = 0.16f;
//                    //armor = LootfestLib.HumanoidLeaderArmor;
//                    imgType = characterLevel == 0? VoxelModelName.orch_leader : VoxelModelName.orch_leader;
//                    scale *= 1.2f;

//                    if (group != null)
//                    {
//                        foreach (Orc h in group)
//                        {
//                            h.LeaderBoost = true;
//                        }
//                    }
//                    this.bosslevel = 2;
//                    break;
//                case HumanoidType.Brute:
//                    swordScale = 0.2f;
//                    //armor = LootfestLib.HumanoidBruteArmor;
//                    //Health += LootfestLib.HumanoidBruteHealthBonus;
//                    scale *= 1.8f;
//                    attackingDist = 14;
//                    this.bosslevel = 1;
//                    break;
//                case HumanoidType.Archer:
//                    //armor = LootfestLib.HumanoidArcherArmor;
//                    attackingDist = 30;
//                    Data.VoxModelAutoLoad.PreLoadImage(VoxelModelName.orcbow, false, 0, false);
//                    break;
//            }
//            if (characterLevel > 0)
//            {
//                swordScale *= 1.2f;
//            }
            

//            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBoundFromFeetPos(scale * 0.22f, scale * 0.45f, 0f);
//            image = LfRef.modelLoad.AutoLoadModelInstance(imgType, TempCharacterImage, scale, 1, false, new Graphics.AnimationsSettings(8, 1.1f, 2));
//        }

//        void updateDamage()
//        {
//            wepDamage = WeaponDamage(leaderBoost, htype == HumanoidType.Archer, this.ObjOwnerAndId, characterLevel > 0);
//        }
//        public static WeaponAttack.DamageData WeaponDamage(bool leaderBoost, bool arrow, NetworkId userIx, bool level2)
//        {
//            float dam = 1f;//level2 ? LootfestLib.HumanoidDamageLvl2 : LootfestLib.HumanoidDamageLvl1;
            
//            WeaponAttack.DamageData result = new WeaponAttack.DamageData(dam, WeaponAttack.WeaponUserType.Enemy, userIx);
            
//            if (arrow)
//            {
//                result.Damage *= 0.6f;
//            }
//            return result;
//        }

//        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
//        {
//            base.handleDamage(damage, local);
//            if (Alive && htype == HumanoidType.Archer && Ref.rnd.RandomChance(40))
//            {
//                fleeState();
//            }
//        }


//        static readonly IntervalF StrafeAngle = new IntervalF(MathHelper.PiOver4, MathHelper.PiOver2);
//        void startCirkleMove()
//        {
//            aiState = AiState.CircleMove;
//            rotation.Add(StrafeAngle.GetRandom() * Ref.rnd.Dir());
//            Velocity.Set(rotation, WalkingSpeed);
//            setImageDirFromRotation();
//            aiStateTimer.MilliSeconds = 600;
//        }

//        static readonly Vector3 VisualBowPosDiff = new Vector3(0.8f, -0.1f, 0.65f);
//        void viewBow()
//        {
//            if (localMember)
//                NetworkWriteObjectState(AiState.PreAttack);
//            lib.SafeDelete(visualBow);
//            visualBow = new Effects.VisualBow(image, VoxelModelName.orcbow, new Time(PreFireBowTime + BowFireTime.Min * 0.5f), VisualBowPosDiff);
           
//        }
//        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
//        {
//            switch (state)
//            {
//                case AiState.PreAttack:
//                    if (htype == HumanoidType.Archer)
//                        viewBow();
//                    break;
//                case AiState.Attacking:
//                    StartAttack(r.ReadBoolean());
//                    break;
//            }
//        }

//        public override void AsynchGOUpdate(GO.UpdateArgs args)
//        {
//            if (localMember)
//            {
//                const float BullRushSpeed = WalkingSpeed * 1.6f;
//                const float ShieldWalkSpeed = 0.005f;

//                //UPDATE
//                if (aiState == AiState.RotatingTowardsGoal || aiState == AiState.ShieldMotion)
//                {
//                    //pick random object and check if it is in the way
//                    AbsUpdateObj obj = getRndCharacter(24, args.allMembersCounter, WeaponAttack.WeaponUserType.Enemy);
//                    if (LookingAtObject(obj, MathHelper.PiOver4))
//                    {
//                        startCirkleMove();
//                    }

//                    //check for obsticals in front
//                    Vector3 check = image.Position + Velocity.Value * 500;
//                    //check += new Vector2toV3(Speed * 500);
//                    check.Y += 1;

//                    if (LfRef.chunks.Get(check) != 0)
//                    {
//                        startCirkleMove();
//                    }
//                }
//                SolidBodyCheck(args.allMembersCounter);


//                //NEW STATE CHECK
//                aiStateTimer.MilliSeconds -= args.time;
//                if (aiStateTimer.MilliSeconds <= 0)
//                {
                    
//                    switch  (aiState)
//                    {
//                        default:
//                            target = getClosestCharacter(60, args.allMembersCounter, WeaponAttack.WeaponUserType.Enemy);

//                            if (target == null)
//                            {
//                                waitState();
//                            }
//                            else
//                            {

//                                Vector2 targetPosDiff = this.PositionDiff(target);
//                                float dist = targetPosDiff.Length();
//                                if (dist > attackingDist)
//                                {
//                                    aiState = AiState.RotatingTowardsGoal;
//                                    targetPosDiff.Normalize();
//                                    Velocity.PlaneValue = targetPosDiff * WalkingSpeed;
//                                }
//                                else if (htype == HumanoidType.Archer || dist < 6)
//                                {
//                                    //view bow and fire direction

//                                    if (htype == HumanoidType.Archer)
//                                    {
//                                        aiState = AiState.PreAttack;
//                                        aiStateTimer.MilliSeconds = PreFireBowTime;
//                                        rotateTowardsObject(target);
//                                        preFireRotation = rotation;
//                                        new GameObjectEventTrigger(viewBow, this);
//                                    }
//                                    else
//                                    {
//                                        AttackState();
//                                    }
//                                }
//                                else
//                                {
//                                    if (htype == HumanoidType.Brute && Ref.rnd.RandomChance(10))
//                                    {
//                                        aiState = AiState.PreRush;
//                                        Velocity.SetZeroPlaneSpeed();
//                                        AngleDirToObject(target);
//                                        aiStateTimer.MilliSeconds = preAttackTime;
//                                        preAttackEffectUnthreading();
//                                    }
//                                    else
//                                    {
//                                        aiState = AiState.ShieldMotion;
//                                        moveTowardsObject(target, 1, ShieldWalkSpeed);
//                                    }
//                                }

//                                const float ArcherFleeInRange = 6;
//                                if (htype == HumanoidType.Archer &&
//                                    (aiState == AiState.Attacking || aiState == AiState.Waiting) &&
//                                    dist <= ArcherFleeInRange)
//                                {
//                                    fleeState();
//                                }

//                            }
//                            break;
//                        case AiState.PreRush:
//                            moveTowardsObject(target, 0, BullRushSpeed);
//                            aiStateTimer.MilliSeconds = 1000;
//                            aiState = AiState.Rush;
//                            attack = true;
                            
//                            return;
//                        case AiState.Rush:
//                            aiState = AiState.Waiting;
//                            aiStateTimer.MilliSeconds = 1000;
//                            Velocity.SetZeroPlaneSpeed();
//                            return;
//                        case AiState.PreAttack:
//                            AttackState();
//                            return;
//                        case AiState.Attacking:
//                            if (htype != HumanoidType.Archer && Ref.rnd.RandomChance(40))
//                            {
//                                moveTowardsObject(target, 1, ShieldWalkSpeed);
//                                aiStateTimer.MilliSeconds = Ref.rnd.Int(200, 400);
//                                aiState = AiState.SmallAdvance;
//                            }
//                            else
//                            {
//                                waitState();
//                            }
//                            target = null;
//                            return;
//                    }
                    
//                }
//            }
//        }

//        private void waitState()
//        {
//            aiState = AiState.Waiting;
//            Velocity.SetZeroPlaneSpeed();
//        }

//        private void AttackState()
//        {
//            Velocity.SetZeroPlaneSpeed();
//            if (this.htype == HumanoidType.Archer)
//            {
//                aiStateTimer.MilliSeconds = BowFireTime.GetRandom();
//            }
//            else
//            {
//                aiStateTimer.MilliSeconds = SwordAttackTime + 1000;
//                rotateTowardsObject(target);
//                rotation.Add(Ref.rnd.Plus_MinusF(0.14f));
//                setImageDirFromRotation();
//                image.Currentframe = 4;
//            }
//            aiState = AiState.Attacking;
//            attack = true;
            
//        }
//        void fleeState()
//        {
//            aiState = AiState.Flee;
//            aiStateTimer.MilliSeconds = new Range(600, 1400).GetRandom();
//            if (target != null)
//                moveTowardsObject(target, 0, -WalkingSpeed, MathHelper.PiOver4);
//        }
        

//        protected override void preAttackEffect()
//        {
//            //animation for bullrush attack
//            physics.Jump(1, image);
//        }
//        protected override Vector3 expressionEffectPosOffset
//        {
//            get
//            {
//                return new Vector3(0, image.Scale.Y * 20, 0);
//            }
//        }
        

//        bool attack = false;
        
//        public override void Time_Update(UpdateArgs args)
//        {
//            base.Time_Update(args);
//            if (localMember)
//            {
//                if (htype == HumanoidType.Archer && (aiState == AiState.PreAttack || aiState == AiState.Attacking))
//                {
//                    float targetDir = AngleDirToObject(target);
//                    if (Math.Abs(rotation.AngleDifference(targetDir)) <= 0.08f)
//                    {
//                        rotation.Radians = targetDir;
//                        targetPos = target.Position;
//                    }
//                }
//                if (attack)
//                {
//                    StartAttack(aiState != AiState.Rush);
//                    //stateRefreshTime += 400;
//                    attack = false;
//                    if (htype != HumanoidType.Archer)
//                    {
//                        System.IO.BinaryWriter w = NetworkWriteObjectState(AiState.Attacking);
//                       //System.IO.BinaryWriter w = NetworkSendAttack();
//                       w.Write(aiState != AiState.Rush);
//                    }
//                }
//                setImageDirFromSpeed();
//            }

//            if (visualBow != null)
//            {
//                if (visualBow.Time_Update(args.time))
//                {
//                    visualBow = null;
//                }
//            }
//        }

//        public void StartAttack(bool preAttack)
//        {
//            switch (htype)
//            {
//                default:
                    
//                    new WeaponAttack.EnemyHandWeaponAttack(aiStateTimer.MilliSeconds, this,
//                        LfRef.modelLoad.AutoLoadModelInstance(characterLevel == 0? VoxelModelName.orc_sword1 : VoxelModelName.orc_sword2,
//                        TempSwordImage, 0, 0, false),
//                        swordScale, wepDamage, localMember, swordAttackEvent, preAttack);
//                    break;
//                case HumanoidType.Archer:
//                    new WeaponAttack.OrcArrow(image.Position + Vector3.Up * 1.4f, targetPos, leaderBoost, characterLevel > 0, this.ObjOwnerAndId);
                    
//                    break;
//            }
//        }

//        void swordAttackEvent()
//        {
//            image.Currentframe = 1;
//        }

//        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
//        {

//            if (group != null)
//            {
//                foreach (Orc h in group)
//                {
//                    h.LeaderBoost = false;
//                }
//            }
//            base.DeathEvent(local, damage);

//        }
//        public override void DeleteMe(bool local)
//        {
//            base.DeleteMe(local);
//            lib.SafeDelete(visualBow);
//        }

//        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
//        {
//            if (ObjCollision != null)
//            {
//                if (ObjCollision.WeaponTargetType == WeaponAttack.WeaponUserType.Enemy)
//                {
//                    if (LookingAtObject(ObjCollision, MathHelper.PiOver4))
//                    {
//                        startCirkleMove();
//                    }
//                }
//            }
//            base.HandleColl3D(collData, ObjCollision);
//        }

//        protected override void updateAnimation()
//        {
//            if (aiState != AiState.Attacking)
//                base.updateAnimation();
//        }

//        public override GameObjectType Type
//        {
//            get { return GameObjectType.Humanioid; }
//        }
//        public override float ExspectedHeight
//        {
//            get
//            {
//                return image.Scale.Y * 20;
//            }
//        }
////orc
////l red, d gray, brown
////orc leader
////l red, d gray, d blue

//        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
//        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
//        public override Effects.BouncingBlockColors DamageColors
//        {
//            get
//            {
//                return DamageColorsLvl1;
//            }
//        }
        
//    }
//    enum HumanoidType
//    {
//        Leader,
//        SwordsMan,
//        Brute,
//        Archer,
//        NUM
//    }
    
//}
