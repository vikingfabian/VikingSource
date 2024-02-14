using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//

namespace VikingEngine.LF2.GameObjects.PickUp
{
    class HumanoidLoot : AbsHeroPickUp
    {
        byte level;
        
        public HumanoidLoot(Vector3 position, int level)
            : base(position)
        {
            level = (byte)(level);
            NetworkShareObject();
        }
        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            writer.Write(level);
        }
        public HumanoidLoot(System.IO.BinaryReader r)
            : base(r)
        {
            level = r.ReadByte();
        }
       
        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.loot_sack; }
        }
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(196,122,27), new Vector3(1.2f));
        protected override Data.TempBlockReplacementSett tempImage
        {
            get { return TempImage; }
        }

        public override int UnderType
        {
            get { return (int)PickUpType.HumanoidLoot; }
        }
        override protected Gadgets.IGadget item
        {
            get
            {
                return LootfestLib.GetRandomAnyGadget(level);
            }
        }
       
    }
}
