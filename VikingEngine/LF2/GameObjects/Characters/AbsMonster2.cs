using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LF2.GameObjects.WeaponAttack;
using VikingEngine.LF2.Data.Characters;

namespace VikingEngine.LF2.GameObjects.Characters
{
    abstract class AbsMonster2 : AbsEnemy
    {
        protected AbsCharacter target;
        protected const float StandardScaleToTerrainBound = 0.4f;
        byte scale;

        protected static readonly DamageData LowCollDamageLvl1 = new DamageData(LootfestLib.MonsterLowCollisionDamage, WeaponUserType.Enemy, ByteVector2.Zero);
        protected static readonly DamageData LowCollDamageLvl2 = new DamageData(LootfestLib.MonsterLowCollisionDamage * LootfestLib.Level2DamageMultiply, WeaponUserType.Enemy, ByteVector2.Zero);
        protected static readonly DamageData MediumCollDamageLvl1 = new DamageData(LootfestLib.MonsterMediumCollisionDamage, WeaponUserType.Enemy, ByteVector2.Zero);
        protected static readonly DamageData MediumCollDamageLvl2 = new DamageData(LootfestLib.MonsterMediumCollisionDamage * LootfestLib.Level2DamageMultiply, WeaponUserType.Enemy, ByteVector2.Zero);
        protected static readonly DamageData HighCollDamageLvl1 = new DamageData(LootfestLib.MonsterHighCollisionDamage, WeaponUserType.Enemy, ByteVector2.Zero);
        protected static readonly DamageData HighCollDamageLvl2 = new DamageData(LootfestLib.MonsterHighCollisionDamage * LootfestLib.Level2DamageMultiply, WeaponUserType.Enemy, ByteVector2.Zero);

        public AbsMonster2(Map.WorldPosition startPos, int level)
            : base(level)
        {
            //areaLevel = level;
            WorldPosition = startPos;
            scale = Ref.rnd.Byte();
            monsterBasicInit(scale);
            image.position = startPos.ToV3();
            image.position.Y = LfRef.chunks.GetScreen(startPos).GetGroundY(startPos);
           
        }

        protected void setHealth(float startHealth)
        {
            Health = startHealth;
            if (areaLevel > 0)
            {
                Health *= LootfestLib.Level2HealthMultiply;
            }
        }
        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return base.NetworkShareSettings;
            }
        }
        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);

            writer.Write(scale);
            writer.Write((byte)areaLevel);
        }

        public AbsMonster2(System.IO.BinaryReader r)
            : base(r)
        {
            scale = r.ReadByte();
            areaLevel = r.ReadByte();
            monsterBasicInit(scale);
            
        }

        void monsterBasicInit(byte scale)
        {
            float imageScale = scaleRange.GetRandom();
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(imageName, TempMonsterImage, imageScale, imageYadj, animSettings);
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

        public override void Time_Update(UpdateArgs args)
        {
            
            base.Time_Update(args);
        }

        public override void AIupdate(GameObjects.UpdateArgs args)
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
                            aiStateTimer.MilliSeconds += 2000 * areaLevel;
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
            if (CollisionBound != null)
            {
                SolidBodyCheck(args.localMembersCounter);
                aiStateTimer.MilliSeconds -= args.time;
            }
        }

        protected void flySweepAIupdate(GameObjects.UpdateArgs args, ref Time turningTime, ref bool turning,
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
                    if (Ref.rnd.RandomChance(60) && distanceToObject(target) < 20)
                    {//start sweeping
                        preAttackEffectUnthreading();
                        rotateTowardsObject(target);
                        aiState = AiState.PreAttack;
                        aiStateTimer.MilliSeconds = preAttackTime;
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


        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
            if (pushable || ObjCollision == null)
            {
                HandleObsticle(true);
            }
            if (ObjCollision != null)
            {
                if (WeaponAttack.WeaponLib.IsFoeTarget(this, ObjCollision, false))
                {
                    //unthread this
                    handleCharacterColl(ObjCollision, new LF2.ObjBoundCollData(collData));//damage 0??
                }
                collData.IntersectionDepth *= 0.5f;
                base.HandleColl3D(collData, ObjCollision);
            }
            
        }

        public override void HandleObsticle(bool wallNotPit)
        {
            base.HandleObsticle(wallNotPit);
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
            if (local)
            {
                if (monsterType < Monster2Type.NUM)
                {
                    MonsterLootSelection loot = lootSelection;
                    if (loot.HasLoot)
                    {
                        new PickUp.DeathLoot(LootDropPos(), loot.GetRandom(), areaLevel);
                        if (Ref.rnd.RandomChance(10))
                        {
                            new PickUp.Coin(LootDropPos(), areaLevel);
                        }
                    }
                }

            
                LfRef.gamestate.MonsterKilled(monsterType);
            }
        }

        

        public override string ToString()
        {
            return monsterType.ToString() + ObjOwnerAndId.ToString();
        }
        abstract protected Monster2Type monsterType { get; }
        abstract protected Graphics.AnimationsSettings animSettings { get; }
       
        abstract protected VoxelModelName imageName { get; }

        abstract protected MonsterLootSelection lootSelection { get; }

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

        
    }
}
