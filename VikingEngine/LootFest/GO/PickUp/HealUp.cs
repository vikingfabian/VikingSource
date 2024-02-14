using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.PickUp
{
    //abstract class AbsSmallBoost : AbsHeroPickUp
    //{
    //    public AbsSmallBoost(Vector3 position)
    //        : base(position)
    //    {

    //    }

    //    //protected override void heroPickUp(PlayerCharacter.AbsHero hero)
    //    //{
    //    //    hero.PickUpSmallBoost(Health_NotMagicBoost);
    //    //}
        
    //    protected abstract bool Health_NotMagicBoost { get; }
    //}

    class HealUp : AbsHeroPickUp
    {
        public HealUp(GoArgs args)
            : base(args)
        {

        }
        //protected override bool Health_NotMagicBoost { get { return true; } }
        
        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.healup_effect; }
        }
        //protected override Data.TempBlockReplacementSett tempImage
        //{
        //    get { return Effects.HealUp.HeartTempImage; }
        //}

        const float Scale = 1f;
        protected override float imageScale
        {
            get
            {
                return Scale;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.HealUp; }
        }
        override public bool HelpfulLooterTarget { get { return true; } }
    }
    //class SmallMagicBoost : AbsSmallBoost
    //{
    //    public SmallMagicBoost(Vector3 position)
    //        : base(position)
    //    {

    //    }
    //    protected override bool Health_NotMagicBoost { get { return false; } }
    //    //public override GameObjectType Type
    //    //{
    //    //    get { return (int)PickUpType.MagicUp; }
    //    //}
    //    protected override VoxelObjName imageType
    //    {
    //        get { return VoxelObjName.magicup_effect; }
    //    }
    //    protected override Data.TempBlockReplacementSett tempImage
    //    {
    //        get { return Effects.HealUp.WaterDropTempImage; }
    //    }
    //}
}
