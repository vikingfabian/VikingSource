using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters.Monsters
{
    //Gör snabba attacker för att sedan vända och springa bort
    class Wolf : AbsMonster2
    {
        public Wolf(Map.WorldPosition startPos, int level)
            : base(startPos, level)
        {
           setHealth(LootfestLib.WolfHealth);
           NetworkShareObject();
           Data.ImageAutoLoad.PreLoadImage(VoxelModelName.enemyattention, false, 0);
        }
         public Wolf(System.IO.BinaryReader r)
            : base(r)
        {
        }
         override protected void createBound(float imageScale)
         {
             CollisionBound = LF2.ObjSingleBound.QuickRectangleRotatedFromFeet(new Vector3(0.15f * imageScale, 0.22f * imageScale, imageScale * 0.48f), 0f);
             TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBoundFromFeetPos(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f, 0f);
         }
        static readonly IntervalF ScaleRange = new IntervalF(2.4f, 2.8f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }

        protected override VoxelModelName imageName
        {
            get { return areaLevel == 0? VoxelModelName.wolf_lvl1 : VoxelModelName.wolf_lvl2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet = 
            new Graphics.AnimationsSettings(5, 1f);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }
        //const int PreAttackTime = 500;
        //void preAttackEffect()
        //{
        //    new Effects.EnemyAttention(new Time(PreAttackTime), image, new Vector3(0, 1, 1f), 0.2f);
        //}
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            basicAIupdate(args);
            if (localMember)
            {
                if (aiStateTimer.MilliSeconds <= 0)
                {
                    if (aiState == AiState.PreAttack)
                    {
                        attack();
                        aiStateTimer.MilliSeconds = Ref.rnd.Int(5000, 10000);
                        return;
                    }
                    else if (aiState == AiState.Waiting || aiState == AiState.Walking)//state != Monster2State.Flee)
                    {
                        target = getClosestCharacter(30, args.allMembersCounter, WeaponAttack.WeaponUserType.Enemy);
                    }
                    else
                    {
                        target = null;
                    }

                    if (target == null)
                    {
                        aiState = AiState.Waiting;
                        Velocity.SetZeroPlaneSpeed();
                        if (Ref.rnd.RandomChance(60))
                        {
                            aiState = AiState.Walking;
                            rotation = Rotation1D.Random();
                            Velocity.Set(rotation, walkingSpeed);
                        }
                        aiStateTimer.MilliSeconds = Ref.rnd.Int(2000, 4000);
                    }
                    else
                    {
                        if (aiState == AiState.Attacking)
                        {
                            attack();
                        }
                        else
                        {
                            aiState = AiState.PreAttack;
                            rotation.Radians = AngleDirToObject(target);
                            setImageDirFromRotation();
                            aiStateTimer.MilliSeconds = preAttackTime;
                            //new Timer.ActionEventTrigger(preAttackEffect);
                            preAttackEffectUnthreading();
                        }
                        //
                       
                    }


                }

            }
        }

        void attack()
        {
            aiState = AiState.Attacking;
            moveTowardsObject(target, 0, runningSpeed);
            aiStateTimer.MilliSeconds = Ref.rnd.Int(3000, 5000);
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            if (localMember)
            {
                if (aiState == AiState.Attacking)
                {
                    //slowly turn toward the victim
                    const float TurningSpeed = 0.0025f;

                    rotateTowardsObject(target, TurningSpeed, args.time);

                }
                setImageDirFromSpeed();
            }
        }

        protected override void HitCharacter(AbsUpdateObj character)
        {
            flee(character);
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            base.handleDamage(damage, local);
            flee(target);
            aiStateTimer.MilliSeconds *= 0.6f;
        }
        void flee(AbsUpdateObj from)
        {
            aiStateTimer.MilliSeconds = 1200 + Ref.rnd.Int(1200);
            aiState = AiState.Flee;
            moveTowardsObject(from, 0, -runningSpeed);
        }


        const float WalkingSpeed = 0.005f;
        const float RunningSpeed = 0.018f;
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return RunningSpeed; }
        }
        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.dark_gray, Data.MaterialType.blue_gray, Data.MaterialType.red_brown);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.dark_blue, Data.MaterialType.blue, Data.MaterialType.red_brown);
        
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return areaLevel == 0 ? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }
        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Wolf; }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Wolf; }
        }
        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return areaLevel == 0 ? HighCollDamageLvl1 : HighCollDamageLvl2;
            }
        }
        protected override NetworkClientRotationUpdateType NetRotationType
        {
            get
            {
                return NetworkClientRotationUpdateType.FromSpeed;
            }
        }

        static readonly Data.Characters.MonsterLootSelection LootSelection = new Data.Characters.MonsterLootSelection(
             Gadgets.GoodsType.Fur, 60, Gadgets.GoodsType.Sharp_teeth, 85, Gadgets.GoodsType.Animal_paw, 100);
        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { return LootSelection; }
        }

        protected override Vector3 preAttackEffectPos
        {
            get
            {
                return new Vector3(0, 1, 1f);
            }
        }
        protected override float preAttackScale
        {
            get
            {
                return 0.2f;
            }
        }
    }
}
