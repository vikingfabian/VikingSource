using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters.Monster3
{

    class SquigRed : AbsSquig
    {
        static readonly IntervalF ScaleRange = new IntervalF(4.8f, 5.4f);

        public SquigRed(GoArgs args)
            : base(args, VoxelModelName.squig_red, ScaleRange, false, 20)
        {

        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SquigRed; }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.dark_red_orange, 
            Data.MaterialType.darker_red_orange, 
            Data.MaterialType.pure_red_orange);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        const float WalkingSpeed = 0.019f;
        const float CasualWalkSpeed = WalkingSpeed * 0.5f;
        const float JumpSpeed = WalkingSpeed * 1.5f;
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
            get { return JumpSpeed; }
        }

        protected override int squigBulletLevel
        {
            get { return VikingEngine.LootFest.GO.WeaponAttack.Monster.SquigBullet2.BulletVersionOneShot; }
        }
    }

    class SquigRedBaby : AbsSquig
    {
        static readonly IntervalF ScaleRange = new IntervalF(1.4f, 1.8f);

        public SquigRedBaby(GoArgs args)
            : base(args, VoxelModelName.squig_red_baby, ScaleRange, true, 0)
        {

        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SquigRedBaby; }
        }

        protected override int squigBulletLevel
        {
            get { throw new NotImplementedException(); }
        }

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return SquigRed.DamageColorsLvl1;
            }
        }

    }
}
