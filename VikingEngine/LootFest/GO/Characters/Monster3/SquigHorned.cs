using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.Characters.Monster3
{
    class SquigHorned : AbsSquig
    {
        static readonly IntervalF ScaleRange = new IntervalF(5.8f, 6.4f);

        public SquigHorned(GoArgs args)
            : base(args, VoxelModelName.squig_horned, ScaleRange, false, 40)
        {
            if (DamageBound.MainBound != null)
            {
                DamageBound.MainBound.ignoresDamage = true;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SquigHorned; }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.light_blue,
            Data.MaterialType.dark_blue,
            Data.MaterialType.light_violet);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        const float WalkingSpeed = 0.018f;
        const float CasualWalkSpeed = WalkingSpeed * 0.5f;
        const float RushSpeed = WalkingSpeed * 3f;

        override protected float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        protected override float rushSpeed
        {
            get
            {
                return RushSpeed;
            }
        }

        protected override int squigBulletLevel
        {
            get { throw new NotImplementedException(); }
        }
    }

    class SquigHornedBaby : AbsSquig
    {
        static readonly IntervalF ScaleRange = new IntervalF(2f, 2.2f);

        public SquigHornedBaby(GoArgs args)
            : base(args, VoxelModelName.squig_horned_baby, ScaleRange, true, 60)
        {

        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SquigHornedBaby; }
        }

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return SquigHorned.DamageColorsLvl1;
            }
        }

        protected override int squigBulletLevel
        {
            get { throw new NotImplementedException(); }
        }
    }
}
