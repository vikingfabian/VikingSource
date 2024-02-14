using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Characters.Monsters
{
    class Skeleton : AbsMonster
    {
        bool fire = false;
        static readonly IntervalF ScaleRange = new IntervalF(3.6f, 4f);
        protected override IntervalF scaleRange
        {
            get { return characterLevel == 0? ScaleRange : IntervalF.FromCenter(9, 0); }
        }
        const float ScaleToBound = 0.40f;
        public Skeleton(GoArgs args, BlockMap.AbsLevel level)
            : base(args)
        {
            
            aiState = AiState.Init;

            if (characterLevel == 0)
            {
                setHealth(LfLib.StandardEnemyHealth);
            }
            else
            {
                Health = LfLib.BossEnemyHealth;
            }

            if (args.LocalMember)
            {
                NetworkShareObject();
                if (characterLevel >= 1)
                {
                    managedGameObject = true;

                    //var lvl = LfRef.levels.GetSubLevelUnsafe(args.startWp).WorldLevel;
                    var manager = new Director.BossManager(this, level, Players.BabyLocation.NUM);

                    int MinionCount = 4;
                    //if (level.LevelEnum == BlockMap.LevelEnum.SkeletonDungeon)
                    //{
                    //    MinionCount = 2;
                    //}
                    //else
                    //{
                    //    MinionCount = 4;
                    //}


                    Rotation1D dir = Rotation1D.Random();
                    for (int i = 0; i < MinionCount; ++i)
                    {
                        Vector3 pos = image.position + VectorExt.V2toV3XZ(dir.Direction(6));
                        var minion = new Skeleton(new GoArgs(pos, 0), level);
                        dir.Add(MathHelper.TwoPi / MinionCount);

                        manager.addBossObject(minion, true);
                    }
                }
            }
        }
        //public Skeleton(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //}
        override protected void createBound(float imageScale)
        {
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * 0.25f, imageScale * 0.5f);
            TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
        }
        protected override VoxelModelName imageName
        {
            get { return VoxelModelName.Skeleton; }
        }

        static readonly Graphics.AnimationsSettings AnimSet = 
            new Graphics.AnimationsSettings(7, 1f, 3);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (fire)
            {
                fireAttack();
            }
            base.Time_Update(args);
            if (aiState == AiState.PreAttack)
            {
                image.Frame = 1;
            }
        }

        void fireAttack()
        {
            if (localMember)
            {
                NetworkWriteObjectState(AiState.PreAttack);
                physics.Jump(1, image);
            }
            //jump
            //physics.Jump(1, image);
            fire = false;
            Vector3 firePos = image.position + VectorExt.V2toV3XZ(rotation.Direction(image.scale.X * 0.2f), 1);
            createProjectile(firePos);
        }

        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            //base.networkReadObjectState(state, r);
            fireAttack();
        }

        virtual protected void createProjectile(Vector3 pos)
        {
            if (characterLevel == 0)
            {
                new WeaponAttack.Monster.SkeletonBone(GoArgs.Empty,pos, rotation);
            }
            else
            {
                new WeaponAttack.Monster.LargeSkeletonBone(GoArgs.Empty, pos, rotation);

                const float SideAngle = 0.6f;
                new WeaponAttack.Monster.LargeSkeletonBone(GoArgs.Empty, pos, rotation - SideAngle);
                new WeaponAttack.Monster.LargeSkeletonBone(GoArgs.Empty, pos, rotation + SideAngle);
            }
        }

        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            basicAIupdate(args);
            if (localMember)
            {
                if (aiStateTimer.MilliSeconds <= 0)
                {
                    //change state
                    if (aiState == AiState.Init)
                    {
                        rotation.Radians = AngleDirToObject(closestGoodGuy(args.allMembersCounter)) + Ref.rnd.Plus_MinusF(1);
                        Velocity.Set( rotation, walkingSpeed);
                        aiStateTimer.MilliSeconds = 1000 + Ref.rnd.Int(1000);
                       
                        aiState = AiState.Walking;
                    }
                    else if (aiState == AiState.Waiting)
                    {
                        //fire bullet
                        fire = true;
                        aiState = AiState.PreAttack;
                        aiStateTimer.MilliSeconds = 600;
                    }
                    else if (aiState == AiState.PreAttack)
                    {
                        if (Ref.rnd.Chance(40))
                        {
                            target = getClosestCharacter(20, args.allMembersCounter, this.WeaponTargetType);
                            rotation.Radians = AngleDirToObject(target);
                        }
                        else
                        {
                            target = null;
                        }
                        if (target != null)
                        {//attack
                            aiState = AiState.Attacking;
                        }
                        else //walk around
                        {
                            aiState = AiState.Walking;
                            rotation = Rotation1D.Random();
                        }
                        Velocity.Set(rotation, walkingSpeed);
                        aiStateTimer.MilliSeconds = 2000 + Ref.rnd.Int(1000);
                    }
                    else
                    {
                        aiState = AiState.Waiting;
                        Velocity.SetZeroPlaneSpeed();
                        aiStateTimer.MilliSeconds = 600 + Ref.rnd.Int(600);
                    }
                    if (characterLevel >= 1)
                    {
                        aiStateTimer.MilliSeconds *= 0.5f;
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

                if (boundary != null)
                {
                    boundary.update(this);
                }
            }

        }
       
        const float WalkingSpeed = 0.006f;
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return WalkingSpeed; }
        }
        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.gray_15, 
            Data.MaterialType.light_red_orange, 
            Data.MaterialType.light_red
            );
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return characterLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.Skeleton; }
        }
        override public CardType CardCaptureType
        {
            get
            {
                if (characterLevel == 0)
                    return CardType.Skeleton;
                else
                    return CardType.SkeletonBoss;
            }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Skeleteon; }
        }
        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return CollisionDamage;
            }
        }
        protected override bool givesContactDamage
        {
            get
            {
                return false;
            }
        }
    }
}
