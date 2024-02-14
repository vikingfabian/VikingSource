using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
namespace VikingEngine.LF2.GameObjects.PickUp
{
    class CoinChest : AbsHeroPickUp
    {
        const float Scale = 0.3f;
        static readonly LF2.ObjSingleBound Bound = LF2.ObjSingleBound.QuickBoundingBox(Scale * 8);

        public CoinChest(Vector3 position)
            : base(position)
        {
            NetworkShareObject();
        }
        public CoinChest(System.IO.BinaryReader r)
            : base(r)
        {
        }

        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.chest_open; }
        }
        
        protected override Data.TempBlockReplacementSett tempImage
        {
            get { return GameObjects.EnvironmentObj.Chest.ChestTempImage; }
        }

        protected override void basicInit()
        {
            base.basicInit();
            CollisionBound = Bound;
            image.scale = lib.V3(Scale);
        }

        public override int UnderType
        {
            get { return (int)PickUpType.CoinChest; }
        }

        static readonly Gadgets.Goods Coins = new Gadgets.Goods(Gadgets.GoodsType.Gold, Gadgets.Quality.Medium, 50);
        override protected Gadgets.IGadget item
        { get { return Coins; } }
    }
}
