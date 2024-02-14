using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.Characters
{
    class Horse : AbsCharacter
    {
        static readonly IntervalF WalkingModeTime = new IntervalF(400, 1000);
        static readonly IntervalF WaitingModeTime = new IntervalF(400, 3000);
        const float WalkingSpeed = 0.007f;

        Effects.BouncingBlockColors damCols;

        public Horse(GoArgs args)
            : base(args)
        {
            WorldPos = args.startWp;
            Health = LfLib.CritterHealth;
            modelScale = 4.8f;

            VoxelModelName modelName;
            switch (Ref.rnd.Int(3))
            {
                default:
                    modelName = VoxelModelName.horse_white;
                    damCols = new Effects.BouncingBlockColors(Data.MaterialType.gray_25,
                        Data.MaterialType.gray_15);
                    break;
                case 1:
                    modelName = VoxelModelName.horse_brown;
                    damCols = new Effects.BouncingBlockColors(Data.MaterialType.dark_warm_brown,
                        Data.MaterialType.gray_85);
                    break;
                case 2:
                    modelName = VoxelModelName.horse_red;
                    damCols = new Effects.BouncingBlockColors(Data.MaterialType.dark_red_orange,
                        Data.MaterialType.dark_yellow_orange);
                    break;
            }

            animSettings = new Graphics.AnimationsSettings(7, 1f, 2);
            image = LfRef.modelLoad.AutoLoadModelInstance(modelName,
               modelScale, 1, false);
            image.position = args.startPos;

            loadBounds();

            if (args.LocalMember)
            {
                aiState = AiState.Walking;
                NextMode();
                NetworkShareObject();
            }
        }

        void NextMode()
        {
            if (aiState == AiState.Waiting)
            {
                aiState = AiState.Walking;
                Rotation = Rotation1D.Random();
                Velocity.Set(rotation, WalkingSpeed);
                aiStateTimer = WalkingModeTime.GetRandom();
                setImageDirFromRotation();
            }
            else
            {
                aiState = AiState.Waiting;
                Velocity.SetZeroPlaneSpeed();
                aiStateTimer = WaitingModeTime.GetRandom();
            }
        }

        public override void Time_LasyUpdate(ref float time)
        {
            base.Time_LasyUpdate(ref time);
            if (aiStateTimer.CountDown(time))
            {
                NextMode();
            }

            activeCheckUpdate();
        }

        public override void AsynchGOUpdate(UpdateArgs args)
        {
            //if (localMember)
            //{

                var hero = GetClosestHero(true);

                if (distanceToObject(hero) <= 3 && !hero.isMounted)
                {
                    new Timer.Action1ArgTrigger<AbsUpdateObj>(hero.InteractPrompt_ver2, this);
                }
            //}
        }

        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            if (start)
            {
                hero.player.setNewHero(new PlayerCharacter.HorseRidingHero(hero.player), false);
                DeleteMe(true);
            }
        }

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return damCols;
            }
        }

        public override SpriteName InteractVersion2_interactIcon
        {
            get
            {
                return SpriteName.LfMountIcon;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.Horse; }
        }
    }
}
