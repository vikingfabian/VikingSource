using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.Characters.Monsters
{
    class HogBaby : Hog3
    {
        public HogBaby(GoArgs args)
            : base(args, VoxelModelName.hog_baby, ScaleRange)
        {
            //setHealth(1);
            Health = 1;

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        //public HogBaby(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //}

        //protected override VoxelModelName imageName
        //{
        //    get
        //    {
        //        return VoxelModelName.hog_baby;
        //    }
        //}

        public static readonly Effects.BouncingBlockColors BabyHogDamageColors = new Effects.BouncingBlockColors(
            Data.MaterialType.dark_yellow_orange,
            Data.MaterialType.pure_red_orange,
            Data.MaterialType.light_red);
        public override Effects.BouncingBlockColors DamageColors
        { get { return BabyHogDamageColors; } }


        static readonly IntervalF ScaleRange = new IntervalF(3f, 3.4f);
        //protected override IntervalF scaleRange
        //{
        //    get { return ScaleRange; }
        //}

        public override CardType CardCaptureType
        {
            get
            {
                return CardType.HogBaby;
            }
        }
        public override GameObjectType Type
        {
            get
            {
                return  GameObjectType.HogBaby;
            }
        }

        const float WalkingSpeed = HogWalkingSpeed * 1.3f;
        const float RunningSpeed = HogRunningSpeed * 1.3f;
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float  rushSpeed
        {
            get { return RunningSpeed; }
        }
    }
}
