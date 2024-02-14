using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.Characters.Monsters
{
    class Zombie : AbsMonster
    {
        SimplePathFinder path;

        public Zombie(GoArgs args)
            : base(args)
        {
            setHealth(LfLib.StandardEnemyHealth);
            if (args.LocalMember)
            {
                path = new SimplePathFinder(this);
            
                NetworkShareObject();
            }
        }

        //public Zombie(System.IO.BinaryReader r)
        //    : base(r)
        //{ }

        public override void Time_Update(UpdateArgs args)
        {
            if (localMember)
            {
                if (checkOutsideUpdateArea_ActiveChunk())
                {
                    DeleteMe();
                    return;
                }

                if (aiState == AiState.Backtracking)
                {
                    if (aiStateTimer.CountDown())
                    {
                        aiState = AiState.Waiting;
                    }
                }
                else
                {
                    if (target == null)
                    {
                        target = GetClosestHero(false);
                    }
                    else
                    {
                        if (path.lengthToGoal > 0.2f)
                        {
                            Velocity.PlaneValue = path.pathDir * WalkingSpeed;
                            setImageDirFromSpeed();
                        }
                        else
                        {
                            Velocity.SetZeroPlaneSpeed();
                            rotateTowardsObject(target);
                            setImageDirFromRotation();
                        }
                    }
                }
            }

            
            base.Time_Update(args);
        }



        public override void AsynchGOUpdate(UpdateArgs args)
        {
            base.AsynchGOUpdate(args);
            if (localMember && target != null)
            {
                path.goalPos = target.Position;
                path.AsynchUpdate(args.time);
            }
        }

        override protected void createBound(float imageScale)
        {
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * 0.25f, imageScale * 0.5f);
            TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
        }

        protected override VoxelModelName imageName
        {
            get { return VoxelModelName.zombie1; }
        }

        static readonly Graphics.AnimationsSettings AnimSet =
            new Graphics.AnimationsSettings(5, 1f, 1);
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return AnimSet; }
        }

        const float WalkingSpeed = 0.006f;

        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return 0; }
        }
        
        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.pastel_yellow_green, 
            Data.MaterialType.darker_yellow, 
            Data.MaterialType.gray_75
            );
        
        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }

        protected override void obsticleAiActions()
        {
            base.obsticleAiActions();
            aiState = AiState.Backtracking;
            aiStateTimer.MilliSeconds = Ref.rnd.Int(400, 600);
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
            get { return GameObjectType.Zombie; }
        }

        override public CardType CardCaptureType
        {
            get { return CardType.Zombie; }
        }
        
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Zombie; }
        }

        static readonly IntervalF ScaleRange = new IntervalF(3.6f, 4f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }
    }
}
