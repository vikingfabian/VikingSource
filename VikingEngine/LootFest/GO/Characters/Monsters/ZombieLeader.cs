using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Characters.Monsters
{
    class ZombieLeader: AbsMonster
    {
        bool fire = false;
        static readonly IntervalF ScaleRange = new IntervalF(3.6f, 4f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
        const float ScaleToBound = 0.40f;
        public ZombieLeader(GoArgs args)
            : base(args)
        {
            setHealth(LfLib.LargeEnemyHealth);
            aiState = AiState.Init;
            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }
        //public ZombieLeader(System.IO.BinaryReader r)
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
            get { return VoxelModelName.zombieleader; }
        }

        static readonly Graphics.AnimationsSettings AnimSet = 
            new Graphics.AnimationsSettings(3, 2f, 1);
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
            if (LfRef.gamestate.GameObjCollection.LocalMembers.Count < 50)
            {

                if (localMember)
                {
                    NetworkWriteObjectState(AiState.PreAttack);
                    physics.Jump(0.1f, image);
                }
                //jump
                //physics.Jump(1, image);
                fire = false;

                const float SpawnAngle = 1f;
                const float SpawnForwardL = 3f;
                Rotation1D spawnDir1 = rotation, spawnDir2 = rotation;
                spawnDir1.Add(-SpawnAngle);
                spawnDir2.Add(SpawnAngle);

                Vector3 firePos1 = image.position + VectorExt.V2toV3XZ(spawnDir1.Direction(SpawnForwardL), -2);
                createProjectile(firePos1);
                Vector3 firePos2 = image.position + VectorExt.V2toV3XZ(spawnDir2.Direction(SpawnForwardL), -2);
                createProjectile(firePos2);
            }
        }

        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            //base.networkReadObjectState(state, r);
            fireAttack();
        }

        virtual protected void createProjectile(Vector3 pos)
        {
            //new WeaponAttack.Monster.SkeletonBone(pos, rotation);
            new Effects.ZombieSpawn(pos);
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
                        aiStateTimer.MilliSeconds = 800;
                    }
                    else if (aiState == AiState.PreAttack)
                    {
                        if (Ref.rnd.Chance(40))
                        {
                            target = GetClosestHero(false);//getClosestCharacter(20, args.allMembersCounter, this.WeaponTargetType);
                            rotation.Radians = AngleDirToObject(target);
                        }
                        else
                        {
                            target = null;
                        }
                        if (target != null && distanceToObject(target) < 20)
                        {//attack
                            aiState = AiState.Attacking;
                        }
                        else //walk around
                        {
                            aiState = AiState.Walking;
                            rotation = Rotation1D.Random();
                        }
                        Velocity.Set(rotation, walkingSpeed);
                        aiStateTimer.MilliSeconds = 1500 + Ref.rnd.Int(1000);
                    }
                    else
                    {
                        aiState = AiState.Waiting;
                        Velocity.SetZeroPlaneSpeed();
                        aiStateTimer.MilliSeconds = 600 + Ref.rnd.Int(600);
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
       
        const float WalkingSpeed = 0.005f;
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return WalkingSpeed; }
        }
        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.pastel_yellow_green,
            Data.MaterialType.darker_yellow,
            Data.MaterialType.gray_75
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
            get { return GameObjectType.ZombieLeader; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.ZombieLeader; }
        }
        
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.ZombieLeader; }
        }
        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return CollisionDamage;
            }
        }
    }
}
