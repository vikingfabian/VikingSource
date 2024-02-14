using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters.Monsters
{
    //jagar en kort stund, måste sen vila
    class Crocodile : AbsMonster2
    {
        static readonly IntervalF ScaleRangeLvl1 = new IntervalF(5f, 6f);
        static readonly IntervalF ScaleRangeLvl2 = new IntervalF(6f, 7f);

        protected override IntervalF scaleRange
        {
            get { return areaLevel == 0? ScaleRangeLvl1 : ScaleRangeLvl2; }
        }
        public Crocodile(Map.WorldPosition startPos, int level)
            : base(startPos, level)
        {
            
           setHealth( LootfestLib.CrockodileHealth);
           NetworkShareObject();
           Data.ImageAutoLoad.PreLoadImage(VoxelModelName.enemyattention, false, 0);
        }

        public Crocodile(System.IO.BinaryReader r)
            : base(r)
        {

        }

        

        override protected void createBound(float imageScale)
        {
            CollisionBound = LF2.ObjSingleBound.QuickRectangleRotatedFromFeet(new Vector3(0.18f * imageScale, 0.22f * imageScale, imageScale * 0.5f), 0f);
            TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBoundFromFeetPos(imageScale * 0.22f, imageScale * 0.1f, 0f);
        }

        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            basicAIupdate(args);
            if (localMember)
            {
                if (aiStateTimer.MilliSeconds <= 0)
                {
                    if (aiState == AiState.PreAttack)
                    {
                        moveTowardsObject(target, 0, runningSpeed);
                        aiStateTimer.MilliSeconds = 600 + areaLevel * Ref.rnd.Int(400, 800);
                        aiState = AiState.Attacking;
                    }
                    else
                    {
                        if (aiState != AiState.Attacking)
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
                                Velocity.Set( rotation, walkingSpeed);
                            }
                            aiStateTimer.MilliSeconds = Ref.rnd.Int(2000, 4000);
                            setImageDirFromSpeed();
                        }
                        else
                        {
                            rotation.Radians = AngleDirToObject(target);
                            setImageDirFromRotation();
                            preAttackEffectUnthreading();
                            aiState = AiState.PreAttack;
                            aiStateTimer.MilliSeconds = preAttackTime;
                        }
                    }

                }
            }
        }
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            if (aiState == AiState.Attacking)
            {
                //slowly turn toward the victim
                const float TurningSpeed = 0.003f;
                rotateTowardsObject(target, TurningSpeed, args.time);
                setImageDirFromSpeed();
            }
            
        }
        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            return base.handleCharacterColl(character, collisionData);
        }
        protected override VoxelModelName imageName
        {
            get { return areaLevel == 0? VoxelModelName.crockodile1 : VoxelModelName.crockodile2; }
        }

        static readonly Graphics.AnimationsSettings AnimSet = 
            new Graphics.AnimationsSettings(6, 0.8f);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }

        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return areaLevel == 0 ? HighCollDamageLvl1 : HighCollDamageLvl2;
            }
        }

        const float WalkingSpeed = 0.003f;
        const float RunningSpeedLvl1 = 0.018f;
        const float RunningSpeedLvl2 = 0.022f;
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return areaLevel==0? RunningSpeedLvl1 : RunningSpeedLvl2; }
        }

        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.orc_skin,Data.MaterialType.mossy_green,Data.MaterialType.zombie_skin);
        static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(Data.MaterialType.brown, Data.MaterialType.dark_gray, Data.MaterialType.light_brown);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return areaLevel == 0? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }

        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Crocodile; }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Crocodile; }
        }

        static readonly Data.Characters.MonsterLootSelection LootSelection = new Data.Characters.MonsterLootSelection(
              Gadgets.GoodsType.Scaley_skin, 60, Gadgets.GoodsType.Sharp_teeth, 85, Gadgets.GoodsType.Rib, 100);
        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { return LootSelection; }
        }

        protected override Vector3 preAttackEffectPos
        {
            get
            {
                return new Vector3(0, 1, 1.6f);
            }
        }
        public override float LightSourceRadius
        {
            get
            {
                return image.scale.X * 11;
            }
        }
        public override float ExspectedHeight
        {
            get
            {
                return 1;
            }
        }
    }
}
