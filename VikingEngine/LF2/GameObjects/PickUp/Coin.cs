using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.GameObjects.PickUp
{
    class Coin : AbsHeroPickUp
    {
        byte amount;

        public Coin(Vector3 position, int level)
            : base(position)
        {
            amount = (byte)(Ref.rnd.Int(level * 5) + Ref.rnd.Int(6) + 1);
            NetworkShareObject();
        }
        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            w.Write(amount);
        }
        public Coin(System.IO.BinaryReader r)
            : base(r)
        {
            amount = r.ReadByte();
        }

        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.Coin; }
        }
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(255, 228, 0), new Vector3(1, 1, 0.2f));
        protected override Data.TempBlockReplacementSett tempImage
        {
            get { return TempImage; }
        }

        public override int UnderType
        {
            get { return (int)PickUpType.Coin; }
        }

        override protected Gadgets.IGadget item
        { get { return new Gadgets.Item(Gadgets.GoodsType.Coins, amount); } }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }
    }
}
