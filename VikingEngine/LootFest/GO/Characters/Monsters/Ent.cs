using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Characters.Monsters
{
    //går långsamt mot spelaren, stannar ofta, skickar iväg rot attack
    class Ent : AbsMonster
    {
        bool fire = false;
        Map.WalkingPath walkingPath;
        static readonly IntervalF ScaleRange = new IntervalF(3.6f, 4f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
        public Ent(GoArgs args)
            : base(args)
        {
            LfRef.modelLoad.PreLoadImage(VoxelModelName.ent_root, false, 0, false);
            LfRef.modelLoad.PreLoadImage(VoxelModelName.target_warning, false, 1, false);
            setHealth(LfLib.LargeEnemyHealth);
            NetworkShareObject();
        }

        //public Ent(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //    Data.VoxModelAutoLoad.PreLoadImage(VoxelModelName.ent_root, false, 0, false);
        //}

        override protected void createBound(float imageScale)
        {
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * 0.25f, imageScale * 0.5f);
            TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
        }


        protected override VoxelModelName imageName
        {
            get { return characterLevel == 0 ? VoxelModelName.ent : VoxelModelName.ent2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet =
            new Graphics.AnimationsSettings(7, 0.6f);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }
        protected override bool pushable
        {
            get
            {
                return false;
            }
        }
        public override void Time_Update(UpdateArgs args)
        {
            if (fire)
            {
                fire = false;
                //new WeaponAttack.RootAttack(args, image.Position, new Rotation1D(AngleDirToObject(target) + Ref.rnd.Plus_MinusF(0.2f)), distanceToObject(target));
                //root attack
                //lägg in en serie av phys push
            }

            base.Time_Update(args);
            if (walkingPath == null)
            {
                Velocity.SetZeroPlaneSpeed();
            }
            else
            {
                Velocity.PlaneValue = walkingPath.WalkTowardsNode(image.position, rotation, 0.0005f * args.time, walkingSpeed);
                setImageDirFromSpeed();
                oldVelocity = Velocity;
                moveImage(Velocity, args.time);
            }
        }
        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            basicAIupdate(args);
            if (localMember)
            {
                aiStateTimer.MilliSeconds -= args.time;
                if (aiStateTimer.MilliSeconds <= 0)
                {
                    if (aiState == AiState.Walking)
                    {
                        //taking a break
                        waitingState();
                    }
                    else
                    {
                        aiState = AiState.Walking;
                        target = getClosestCharacter(characterLevel == 0 ? 64 : 80, args.allMembersCounter, this.WeaponTargetType);
                        if (target == null)
                        {
                            walkingPath = LfRef.chunks.PathFindRandomDir(image.position);
                            aiStateTimer.MilliSeconds = 1400 + Ref.rnd.Int(800);
                        }
                        else
                        {
                            float l = distanceToObject(target);

                            if (6 < l && l < 16 && Ref.rnd.Chance(characterLevel == 0 ? 50 : 70))
                            {
                                //fire root attack
                                aiState = AiState.Attacking;
                                fire = true;
                                walkingPath = null;
                                aiStateTimer.MilliSeconds = 2600 + Ref.rnd.Int(800) - 800 * characterLevel;
                            }
                            else if (l > 2)
                            {
                                walkingPath = walkingPathTowardsObject(target);
                                aiStateTimer.MilliSeconds = 2000 + Ref.rnd.Int(800);
                            }
                            else
                            {
                                waitingState();
                            }
                        }

                    }
                }
            }
        }

        void waitingState()
        {
            walkingPath = null;
            aiState = AiState.Waiting;
            aiStateTimer.MilliSeconds = 800 + Ref.rnd.Int(800);
        }

        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            base.HandleColl3D(collData, ObjCollision);
            if (ObjCollision != null && aiState == AiState.Walking)
            {
                waitingState();
            }
        }

        const float WalkingSpeed = 0.0034f;

        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return 0; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return characterLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return CollisionDamage;//MediumCollDamageLvl1;
            }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.Ent; }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Ent; }
        }

       
    }
}