using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LootFest.GO.PickUp
{
    class Coin : AbsHeroPickUp
    {
        //int amount;

        public Coin(GoArgs args)
            : base(args)
        {
            if (args.LocalMember)
            {
                amount = Ref.rnd.Int(1, 2);
                NetworkShareObject();
            }
            else
            {
                amount = args.reader.ReadByte();
            }
        }
        public override void netWriteGameObject(System.IO.BinaryWriter w)
        {
            base.netWriteGameObject(w);
            w.Write(amount);
        }
        //public Coin(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //    amount = r.ReadByte();
        //}

        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.Coin; }
        }
        ////static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(255, 228, 0), new Vector3(1, 1, 0.2f));
        //protected override Data.TempBlockReplacementSett tempImage
        //{
        //    get { return TempImage; }
        //}

        public override GameObjectType Type
        {
            get { return GameObjectType.Coin; }
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }
    }
}
