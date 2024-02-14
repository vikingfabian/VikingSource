using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Characters.Monsters
{
    class Squig: AbsMonster
    {
        bool fire = false;
        static readonly IntervalF ScaleRange = new IntervalF(2f, 2.2f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
        const float ScaleToBound = 0.40f;
        public Squig(GoArgs args)
            : base(args)
        {
            setHealth(LfLib.StandardEnemyHealth);
            aiState = AiState.Init;
            NetworkShareObject();
        }
        //public Squig(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //}
         override protected void createBound(float imageScale)
         {
             CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotatedFromFeet(new Vector3(0.34f * imageScale, 0.42f * imageScale, imageScale * 0.35f), new Vector3(0, 0f, -0.14f * imageScale));
             TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBoundFromFeetPos(imageScale * ScaleToBound, imageScale * 0.5f, 0f);
         }
        protected override VoxelModelName imageName
        {
            get { return  characterLevel == 0? VoxelModelName.squig_lvl1 : VoxelModelName.squig_lvl2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet = 
            new Graphics.AnimationsSettings(7, 0.8f, 2);
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
            new WeaponAttack.SquigBullet(pos, rotation, GameObjectType.SquigBullet);
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
       
        const float WalkingSpeed = 0.006f;
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return WalkingSpeed; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.dark_red_orange,
            Data.MaterialType.dark_yellow_orange,
            Data.MaterialType.dark_red);
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
            get { return GameObjectType.Squig; }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Squig; }
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
