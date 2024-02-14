using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.PickUp
{
    abstract class AbsSmallBoost : AbsHeroPickUp
    {
        public AbsSmallBoost(Vector3 position)
            : base(position)
        {

        }

        protected override void heroPickUp(Characters.Hero hero)
        {
            hero.PickUpSmallBoost(Health_NotMagicBoost);
        }
        const float Scale = 1f;
        protected override float imageScale
        {
            get
            {
                return Scale;
            }
        }
        protected abstract bool Health_NotMagicBoost { get; }
    }

    class SmallHealthBoost : AbsSmallBoost
    {
        public SmallHealthBoost(Vector3 position)
            : base(position)
        {

        }
        protected override bool Health_NotMagicBoost { get { return true; } }
        public override int UnderType
        {
            get { return (int)PickUpType.HealUp; }
        }
        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.healup_effect; }
        }
        protected override Data.TempBlockReplacementSett tempImage
        {
            get { return Effects.HealUp.HeartTempImage; }
        }
    }
    class SmallMagicBoost : AbsSmallBoost
    {
        public SmallMagicBoost(Vector3 position)
            : base(position)
        {

        }
        protected override bool Health_NotMagicBoost { get { return false; } }
        public override int UnderType
        {
            get { return (int)PickUpType.MagicUp; }
        }
        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.magicup_effect; }
        }
        protected override Data.TempBlockReplacementSett tempImage
        {
            get { return Effects.HealUp.WaterDropTempImage; }
        }
    }
}
