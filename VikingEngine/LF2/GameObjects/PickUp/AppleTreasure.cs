using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.PickUp
{
    class AppleTreasure : AbsHeroPickUp
    {
        static readonly IntervalF Scale = new IntervalF(0.1f, 0.14f);
        public AppleTreasure(Vector3 position)
            :base(position)
        {
            image.scale = Vector3.One * Scale.GetRandom();
            NetworkShareObject();
        }
        public AppleTreasure(System.IO.BinaryReader r)
            : base(r)
        {
        }


        protected override VoxelModelName imageType
        {
            get {  return VoxelModelName.Apple; }
        }
        protected override Data.TempBlockReplacementSett tempImage
        {
            get { return AppleTempImage; }
        }
       
        protected override bool rotating
        {
            get
            {
                return false;
            }
        }
        public override int UnderType
        {
            get { return (int)PickUpType.Apple; }
        }
        static readonly Gadgets.Goods OneApple = new Gadgets.Goods(Gadgets.GoodsType.Apple);
        override protected Gadgets.IGadget item
        { get { return OneApple; } }
        
    }
}
