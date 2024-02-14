using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//

namespace VikingEngine.LF2.GameObjects.PickUp
{
    class DeathLoot: AbsHeroPickUp
    {
        Gadgets.Goods loot;

        public DeathLoot(Vector3 position, Gadgets.GoodsType gtype, int lootLvl)
            : base(position)
        {
            //monster loot level is 0 to 1
            Gadgets.Quality qty = Gadgets.Quality.Medium;
            int amount = 1;
            if (lootLvl <= 0)
            {
                if (Ref.rnd.RandomChance(20))
                    qty = Gadgets.Quality.Low;

            }
            else
            {
                if (Ref.rnd.RandomChance(25 * lootLvl))
                    qty = Gadgets.Quality.High;

            }
            if (Ref.rnd.RandomChance(10 + 15 * lootLvl))
            {
                amount = 2;
            }

            loot = new Gadgets.Goods(gtype, qty, amount);
            NetworkShareObject();

            //basicInit();
            startSetup(position);
        }
        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            loot.WriteStream(writer);
        }
        public DeathLoot(System.IO.BinaryReader r)
            : base(r)
        {
            loot = Gadgets.Goods.FromStream(r, byte.MaxValue);

            pickupInit();
        }

        protected override VoxelModelName imageType
        {
            get 
            {
                if (loot.Type == 0)
                    return VoxelModelName.NUM_Empty;
                else if (loot.Type == Gadgets.GoodsType.Leather ||
                    loot.Type == Gadgets.GoodsType.Skin ||
                    loot.Type == Gadgets.GoodsType.Fur ||
                    loot.Type == Gadgets.GoodsType.Feathers)
                {
                    return VoxelModelName.loot_leather;
                }
                else if (loot.Type == Gadgets.GoodsType.Feathers)
                {
                    return VoxelModelName.loot_feather;
                }
                else return VoxelModelName.Bone; 
            }
        }
        static readonly Data.TempBlockReplacementSett LeatherTempImage = new Data.TempBlockReplacementSett(new Color(147, 81, 28), new Vector3(1.5f, 0.2f, 1.5f));
        static readonly Data.TempBlockReplacementSett FeatherTempImage = new Data.TempBlockReplacementSett(new Color(229, 224, 182), new Vector3(0.5f, 0.1f, 1.5f));
        

        protected override Data.TempBlockReplacementSett tempImage
        {
            get 
            {
                switch (imageType)
                {
                    case VoxelModelName.loot_leather: return LeatherTempImage;
                    case VoxelModelName.loot_feather: return FeatherTempImage;
                    case VoxelModelName.Bone: return BoneTempImage;

                }
                throw new NotImplementedException("Deathloot temp img, " + imageType.ToString());
            }
        }

        public override int UnderType
        {
            get { return (int)PickUpType.DeathLoot; }
        }
        public override ObjectType Type
        {
            get
            {
                return base.Type;
            }
        }
        protected override Gadgets.IGadget item
        {
            get
            {
                return loot;
            }
        }
        protected override float imageScale
        {
            get
            {
                return 2;
            }
        }
    }
}
