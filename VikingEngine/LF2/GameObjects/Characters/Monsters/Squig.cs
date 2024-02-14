using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters.Monsters
{
    class Squig: AbsMonster2
    {
        bool fire = false;
        static readonly IntervalF ScaleRange = new IntervalF(2f, 2.2f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
        const float ScaleToBound = 0.40f;
        public Squig(Map.WorldPosition startPos, int level)
            : base(startPos, level)
        {
            setHealth(LootfestLib.SquigHealth);
            aiState = AiState.Init;
            NetworkShareObject();
        }
         public Squig(System.IO.BinaryReader r)
            : base(r)
        {
        }
         override protected void createBound(float imageScale)
         {
             CollisionBound = LF2.ObjSingleBound.QuickRectangleRotatedFromFeet(new Vector3(0.34f * imageScale, 0.42f * imageScale, imageScale * 0.35f), new Vector3(0, 0f, -0.14f * imageScale));
             TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBoundFromFeetPos(imageScale * ScaleToBound, imageScale * 0.5f, 0f);
         }
        protected override VoxelModelName imageName
        {
            get { return  areaLevel == 0? VoxelModelName.squig_lvl1 : VoxelModelName.squig_lvl2; }
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
                image.Currentframe = 1;
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
            Vector3 firePos = image.position + Map.WorldPosition.V2toV3(rotation.Direction(image.scale.X * 0.2f), 1);
            createProjectile(firePos);
        }

        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            //base.networkReadObjectState(state, r);
            fireAttack();
        }

        virtual protected void createProjectile(Vector3 pos)
        {
            new WeaponAttack.SquigBullet(pos, rotation, WeaponAttack.WeaponUtype.SquigBullet);
        }

        public override void AIupdate(GameObjects.UpdateArgs args)
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
                        if (Ref.rnd.RandomChance(40))
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
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.light_red, Data.MaterialType.blue_gray, Data.MaterialType.red_brown);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.green, Data.MaterialType.dark_blue, Data.MaterialType.orc_skin);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return areaLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Squig; }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Squig; }
        }
        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return areaLevel == 0 ? LowCollDamageLvl1 : LowCollDamageLvl2;
            }
        }

        static readonly Data.Characters.MonsterLootSelection LootSelection = new Data.Characters.MonsterLootSelection(
             Gadgets.GoodsType.Leather, 60, Gadgets.GoodsType.Horn, 85, Gadgets.GoodsType.Nose_horn, 100);
        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { return LootSelection; }
        }
    }
}
