using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.PickUp
{
    class GruntChest : AbsHeroPickUp
    {
        //byte amount;
        byte lootLvl;

        public GruntChest(Vector3 position, int level)
            : base(position)
        {
            lootLvl = (byte)level;
            NetworkShareObject();
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            writer.Write(lootLvl);
        }
        public GruntChest(System.IO.BinaryReader r)
            : base(r)
        {
            lootLvl = System.IO.BinaryReader.ReadByte();
        }
        

        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.chest_open; }
        }
        protected override Data.TempBlockReplacementSett tempImage
        {
            get { return GameObjects.EnvironmentObj.Chest.ChestTempImage; }
        }

        public override int UnderType
        {
            get { return (int)PickUpType.CoinChest; }
        }

        override protected Gadgets.IGadget item
        { 
            get {
                int amount = (byte)(Ref.rnd.Int(lootLvl * 8) + 6);
                return new Gadgets.GadgetList(new List<Gadgets.IGadget>{
                    new Gadgets.Item(Gadgets.GoodsType.Coins, amount),
                    LootfestLib.GetRandomAnyGadget(lootLvl + 1),
                    LootfestLib.GetRandomAnyGadget(lootLvl + 2),});
            } 
        }
    }
}
