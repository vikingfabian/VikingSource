using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.GameObjects.PickUp
{
    class ArrowPickUp : AbsHeroPickUp
    {
        public ArrowPickUp(Vector3 position)
            : base(position)
        {
            NetworkShareObject();
        }
        public ArrowPickUp(System.IO.BinaryReader r)
            : base(r)
        {
        }

        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.Arrow; }
        }

        protected override Data.TempBlockReplacementSett tempImage
        {
            get { return WeaponAttack.AbsWeapon.ArrowTempImage; }
        }

        public override int UnderType
        {
            get { return (int)PickUpType.ReuseArrow; }
        }
        protected override Gadgets.IGadget item
        {
            get
            {
                return new Gadgets.Item(Gadgets.GoodsType.Arrow);
            }
        }
    }
    
}
