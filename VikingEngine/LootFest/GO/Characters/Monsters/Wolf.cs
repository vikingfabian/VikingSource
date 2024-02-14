using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Characters.Monsters
{
    //Gör snabba attacker för att sedan vända och springa bort
    class Wolf : AbsMonster
    {
        public Wolf(GoArgs args)
            : base(args)
        {
           setHealth(LfLib.StandardEnemyHealth);
           if (args.LocalMember)
           {
               NetworkShareObject();
           }
           LfRef.modelLoad.PreLoadImage(VoxelModelName.enemyattention, false, 0, false);
        }
        // public Wolf(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //}
         override protected void createBound(float imageScale)
         {
             CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotatedFromFeet(new Vector3(0.15f * imageScale, 0.22f * imageScale, 0.55f * imageScale), 0f);
             TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBoundFromFeetPos(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f, 0f);
         }
        static readonly IntervalF ScaleRange = new IntervalF(2.4f, 2.8f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }

        protected override VoxelModelName imageName
        {
            get { return characterLevel == 0? VoxelModelName.wolf_lvl1 : VoxelModelName.wolf_lvl2; }
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
        public override void AsynchGOUpdate(GO.UpdateArgs args)
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
                        if (Ref.rnd.Chance(60))
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
                            aiStateTimer.MilliSeconds = PreAttackTime;
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

                    rotateTowardsObject(target, TurningSpeed);

                }
                setImageDirFromSpeed();
            }
        }

        protected override void onHitCharacter(AbsUpdateObj character)
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
        const float RunningSpeed = 0.012f;//0.018f; till lvl2
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return RunningSpeed; }
        }
        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.dark_cyan,
            Data.MaterialType.darker_cyan_blue,
            Data.MaterialType.darker_red);
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
            get { return GameObjectType.Wolf; }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Wolf; }
        }
        protected override WeaponAttack.DamageData contactDamage
        {
            get
            {
                return CollisionDamage;//characterLevel == 0 ? MediumCollDamageLvl1 : MediumCollDamageLvl1;
            }
        }
        protected override NetworkClientRotationUpdateType NetRotationType
        {
            get
            {
                return NetworkClientRotationUpdateType.FromSpeed;
            }
        }

    

        protected override Vector3 expressionEffectPosOffset
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
