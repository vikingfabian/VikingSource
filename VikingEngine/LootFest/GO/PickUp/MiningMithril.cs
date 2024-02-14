using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.GO.PickUp
{
    class MiningMithril: AbsHeroPickUp
    {
        public MiningMithril(GoArgs args)
            : base(args)
        {
            amount = 1;
            pickUpBlockTime = 800;
            if (args.LocalMember)
            {
                physics.SpeedY = IntervalF.FromCenter(0.014f, 0.006f).GetRandom();
                NetworkShareObject();
            }
        }

        protected override bool giveStartSpeed
        {
            get
            {
                return false;
            }
        }

        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.mithril_ingot; }
        }
        ////static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.LightBlue, new Vector3(1, 0.5f, 0.6f));
        //protected override Data.TempBlockReplacementSett tempImage
        //{
        //    get { return TempImage; }
        //}

        protected override float imageScale
        {
            get
            {
                return 1.8f;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.MiningMithril; }
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }
    }
}
