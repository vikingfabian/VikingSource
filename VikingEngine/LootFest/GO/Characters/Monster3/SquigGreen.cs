using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.Characters.Monster3
{
    class SquigGreen : AbsSquig
    {
        static readonly IntervalF ScaleRange = new IntervalF(3.8f, 4.4f);

        public SquigGreen(GoArgs args)
            : base(args, VoxelModelName.squig_green, ScaleRange, false, 80)
        {

        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SquigGreen; }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.pure_pea_green,
            Data.MaterialType.darker_yellow,
            Data.MaterialType.darker_warm_brown);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        protected override int squigBulletLevel
        {
            get { return VikingEngine.LootFest.GO.WeaponAttack.Monster.SquigBullet2.BulletVersionMulti; }
        }
    }

    class SquigGreenBaby : AbsSquig
    {
        static readonly IntervalF ScaleRange = new IntervalF(1f, 1f);

        public SquigGreenBaby(GoArgs args)
            : base(args, VoxelModelName.squig_green_baby, ScaleRange, true, 0)
        {

        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SquigGreenBaby; }
        }

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return SquigGreen.DamageColorsLvl1;
            }
        }

        protected override int squigBulletLevel
        {
            get { throw new NotImplementedException(); }
        }
    }
}
