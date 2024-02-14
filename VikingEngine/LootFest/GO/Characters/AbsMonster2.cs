using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GO.WeaponAttack;

namespace VikingEngine.LootFest.GO.Characters
{
    abstract class AbsMonster : AbsEnemy
    {
        
        protected const float StandardScaleToTerrainBound = 0.4f;
        byte scale;

        protected static readonly DamageData CollisionDamage = new DamageData(LfLib.EnemyAttackDamage, WeaponUserType.Enemy, NetworkId.Empty);
        //protected static readonly DamageData LowCollDamageLvl2 = new DamageData(LootfestLib.MonsterLowCollisionDamage * LootfestLib.Level2DamageMultiply, WeaponUserType.Enemy, ByteVector2.Zero);
        //protected static readonly DamageData MediumCollDamageLvl1 = new DamageData(LootfestLib.MonsterMediumCollisionDamage, WeaponUserType.Enemy, ByteVector2.Zero);
        //protected static readonly DamageData MediumCollDamageLvl2 = new DamageData(LootfestLib.MonsterMediumCollisionDamage * LootfestLib.Level2DamageMultiply, WeaponUserType.Enemy, ByteVector2.Zero);
        //protected static readonly DamageData HighCollDamageLvl1 = new DamageData(LootfestLib.MonsterHighCollisionDamage, WeaponUserType.Enemy, ByteVector2.Zero);
        //protected static readonly DamageData HighCollDamageLvl2 = new DamageData(LootfestLib.MonsterHighCollisionDamage * LootfestLib.Level2DamageMultiply, WeaponUserType.Enemy, ByteVector2.Zero);

        public AbsMonster(GoArgs args)
            : base(args)
        {
            if (args.LocalMember)
            {
                scale = Ref.rnd.Byte();
            }
            else
            {
                scale = args.reader.ReadByte();
            }
            monsterBasicInit(scale);

            image.position = args.startPos;
        }
        public override void netWriteGameObject(System.IO.BinaryWriter writer)
        {
            base.netWriteGameObject(writer);

            writer.Write(scale);
            //writer.Write((byte)characterLevel);
        }

        protected void setHealth(float startHealth)
        {
            Health = startHealth;
            
        }
        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return base.NetworkShareSettings;
            }
        }
       

        //public AbsMonster(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //    scale = System.IO.BinaryReader.ReadByte();
        //    characterLevel = System.IO.BinaryReader.ReadByte();
        //    monsterBasicInit(scale);
            
        //}

        void monsterBasicInit(byte scale)
        {
            float imageScale = scaleRange.GetRandom();

            image = LfRef.modelLoad.AutoLoadModelInstance(imageName, imageScale, imageYadj, false);
            createBound(imageScale);
            if (PlatformSettings.ViewCollisionBounds)
                TerrainInteractBound.DebugBoundColor(Color.DarkGray);

        }

        virtual protected float imageYadj { get { return 1; } }

        protected override void moveImage(Velocity speed, float time)
        {
            base.moveImage(speed, time);
        }

        abstract protected void createBound(float imageScale);

        abstract protected float walkingSpeed { get; }
        abstract protected float runningSpeed { get; }
        abstract protected IntervalF scaleRange { get; }

        //public override void Time_Update(UpdateArgs args)
        //{
        //    base.Time_Update(args);

        //    //crashIfOutsideMap();
        //}

        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            basicAIupdate(args);
            if (localMember)
            {
                if (aiStateTimer.MilliSeconds <= 0)
                {
                    //change state
                    if (aiState == AiState.Waiting)
                    {

                        float moveSpeed = walkingSpeed;
                        target = getClosestCharacter(20, args.allMembersCounter, WeaponAttack.WeaponUserType.Enemy);
                        rotation.Radians = AngleDirToObject(target);
                        aiStateTimer.MilliSeconds = 3000;
                        if (target != null)
                        {//attack
                            aiState = AiState.Attacking;
                            aiStateTimer.MilliSeconds += 2000 * characterLevel;
                        }
                        else //walk around
                        {
                            aiState = AiState.Walking;
                            rotation = Rotation1D.Random();
                        }
                        Velocity.Set(rotation, moveSpeed);
                        
                    }
                    else
                    {
                        aiState = AiState.Waiting;
                        Velocity.SetZeroPlaneSpeed();
                        aiStateTimer.MilliSeconds = 600 + Ref.rnd.Int(1600);
                    }

                    setImageDirFromRotation();
                }
                else
                {
                    if (aiState == AiState.Attacking)
                    {
                        rotation.Radians = AngleDirToObject(target);
                        Velocity.Set(rotation, walkingSpeed);
                        setImageDirFromRotation();
                    }
                }
            }
            
        }

        protected void basicAIupdate(UpdateArgs args)
        {
            UpdateWorldPos();
            if (CollisionAndDefaultBound != null)
            {
                SolidBodyCheck(args.localMembersCounter);
                aiStateTimer.MilliSeconds -= args.time;
            }
        }

        protected void flySweepAIupdate(GO.UpdateArgs args, ref Time turningTime, ref bool turning,
            Range standardStateTime, Range attackTime, Range waitingTime, Range turningStateTime)
        {
            if (aiStateTimer.MilliSeconds <= 0)
            {
                aiStateTimer.MilliSeconds = standardStateTime.GetRandom();
                target = getClosestCharacter(92, args.allMembersCounter, this.WeaponTargetType);
                if (aiState == AiState.PreAttack)
                {
                    aiState = AiState.Attacking;
                    moveTowardsObject(target, 2, runningSpeed);
                    aiStateTimer.MilliSeconds = attackTime.GetRandom();
                    setImageDirFromSpeed();
                }
                else if (target == null)
                {
                    aiState = AiState.Waiting;
                }
                else if (aiState == AiState.Waiting || aiState == AiState.Attacking)
                {
                    aiState = AiState.Walking;
                }
                else if (aiState == AiState.Walking)
                {
                    if (Ref.rnd.Chance(60) && distanceToObject(target) < 20)
                    {//start sweeping
                        preAttackEffectUnthreading();
                        rotateTowardsObject(target);
                        aiState = AiState.PreAttack;
                        aiStateTimer.MilliSeconds = PreAttackTime;
                    }
                    else
                    {

                        aiState = AiState.Waiting;
                        aiStateTimer.MilliSeconds = waitingTime.GetRandom();
                    }
                    Velocity.SetZeroPlaneSpeed();
                }

                if (turningTime.CountDown(args.time))
                {
                    turningTime.MilliSeconds = turningStateTime.GetRandom();
                    turning = !turning;
                }
            }
        }


        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            if (pushable || ObjCollision == null)
            {
                HandleObsticle(true, ObjCollision);
            }
            if (ObjCollision != null)
            {
                if (WeaponAttack.WeaponLib.IsFoeTarget(this, ObjCollision, false))
                {
                    //unthread this
                    handleCharacterColl(ObjCollision, collData, false);//damage 0??
                }
                collData.IntersectionDepth *= 0.5f;
                base.HandleColl3D(collData, ObjCollision);
            }

           // crashIfOutsideMap();
        }

        public override void HandleObsticle(bool wallNotPit, AbsUpdateObj ObjCollision)
        {
            base.HandleObsticle(wallNotPit, ObjCollision);

            obsticleAiActions();
            //crashIfOutsideMap();
        }

        /// <summary>
        /// React from hitting an obsticle by moving away from it somehow
        /// </summary>
        virtual protected void obsticleAiActions()
        {
            if (!Velocity.ZeroPlaneSpeed)
            {
                rotation.Radians += MathHelper.PiOver2 + Ref.rnd.Plus_MinusF(MathHelper.PiOver4);
                aiState = AiState.Walking;
                aiStateTimer.MilliSeconds = Ref.rnd.Int(200, 500);
                Velocity.Set(rotation, walkingSpeed);
                setImageDirFromRotation();
            }
        }



        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);
        }

        protected override void handleDamage(DamageData damage, bool local)
        {
            base.handleDamage(damage, local);
        }

        //void crashIfOutsideMap()
        //{
        //    //WorldPosition = new Map.WorldPosition(image.Position);
        //    if (!WorldPosition.CorrectPos)
        //    {
        //        throw new Exception("Outside map error");
        //    }
        //}

        

        public override string ToString()
        {
            return monsterType.ToString() + ObjOwnerAndId.ToString();
        }
        abstract protected Monster2Type monsterType { get; }
        abstract protected Graphics.AnimationsSettings animSettings { get; }
       
        abstract protected VoxelModelName imageName { get; }

        //abstract protected MonsterLootSelection lootSelection { get; }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return base.PhysicsType;
            }
        }
    }

    
    enum Monster2Type
    {
        Crocodile,
        Ent,
        FireGoblin,
        Frog,
        Harpy,
        Hog,
        Lizard,
        Scorpion,
        Spider,
        Wolf,
        Squig,
        SquigSpawn,

        Mummy,
        Ghost,

        Bee,
        NUM,
        EvilSpider,
        EndBossMount,

        Zombie, ZombieLeader, Skeleteon,
    }
}
