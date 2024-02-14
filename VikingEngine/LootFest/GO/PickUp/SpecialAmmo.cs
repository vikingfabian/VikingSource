using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.PickUp
{
    class SpecialAmmoAdd1 : AbsHeroPickUp
    {
        public SpecialAmmoAdd1(GoArgs args)
            : base(args)
        { }

        const float Scale = 1f;
        protected override float imageScale
        {
            get
            {
                return Scale;
            }
        }
        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.specialattup; }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SpecialAmmo1; }
        }

        //protected override Data.TempBlockReplacementSett tempImage
        //{
        //    get { return Effects.HealUp.WaterDropTempImage; }
        //}

        override public bool HelpfulLooterTarget { get { return true; } }
    }

    class SpecialAmmoFullAdd : AbsHeroPickUp
    {
        public SpecialAmmoFullAdd(GoArgs args)
            : base(args)
        { }

        const float Scale = 1.6f;
        protected override float imageScale
        {
            get
            {
                return Scale;
            }
        }
        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.specialammorefill; }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SpecialAmmoFull; }
        }

        //protected override Data.TempBlockReplacementSett tempImage
        //{
        //    get { return Effects.HealUp.WaterDropTempImage; }
        //}

        override public bool HelpfulLooterTarget { get { return true; } }
    }
}
