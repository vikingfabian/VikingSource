using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.PickUp
{
#if CMODE
    class ThrowingSpear: AbsPickUp
    {
        public ThrowingSpear(Vector3 position)
            :base(position)
        { }
        public ThrowingSpear(System.IO.BinaryReader r)
            : base(r)
        {
        }

        //protected override Graphics.VoxelObj orgImage()
        //{
        //    return LootfestLib.Images.StandardVoxelObjects[VoxelModelName.ThrowingSpear];
        //}

        public override int UnderType
        {
            get { return (int)PickUpType.Spear; }
        }
        static readonly Gadgets.Goods OneSpear = new Gadgets.Goods(Gadgets.GoodsType.Spear);
        override protected Gadgets.IGadget item
        { get { return OneSpear; } }

        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.ThrowingSpear; }
        }
    }
#endif
}
