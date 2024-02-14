using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LF2.GameObjects.Gadgets;

namespace VikingEngine.LF2.GameObjects.PickUp
{
    class MiningLoot : AbsHeroPickUp
    {
        GoodsType baseStone;
        byte lootLvl;
        static readonly IntervalF Scale = new IntervalF(0.25f, 0.3f);
        
        public MiningLoot(Vector3 position, GoodsType baseStone, byte lootLvl)
            : base(position)
        {
            this.baseStone = baseStone;
            this.lootLvl = lootLvl;    
            image.scale = Vector3.One * Scale.GetRandom();
            NetworkShareObject();
        }
        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            w.Write(lootLvl);
            w.Write((byte)baseStone);
        }
        public MiningLoot(System.IO.BinaryReader r)
            : base(r)
        {
            this.lootLvl = r.ReadByte(); 
            baseStone = (GoodsType)r.ReadByte();
        }


        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.mining_loot; }
        }
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.DarkGray, new Vector3(1.0f));
        protected override Data.TempBlockReplacementSett tempImage
        {
            get { return TempImage; }
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
            get { return (int)PickUpType.Mining; }
        }
        
        static readonly PlayMechanics.SelectRandomItem<GoodsType> RandomItem =  new PlayMechanics.SelectRandomItem<GoodsType>( 
            new List<PlayMechanics.ItemCommoness<GoodsType>>
            {
                new PlayMechanics.ItemCommoness<GoodsType>( GoodsType.Coal, 80),
                new PlayMechanics.ItemCommoness<GoodsType>( GoodsType.Bronze, 40),
                new PlayMechanics.ItemCommoness<GoodsType>( GoodsType.Iron, 30),
                new PlayMechanics.ItemCommoness<GoodsType>( GoodsType.Silver, 20),
                new PlayMechanics.ItemCommoness<GoodsType>( GoodsType.Gold, 10),
                new PlayMechanics.ItemCommoness<GoodsType>( GoodsType.Mithril, 1),
                new PlayMechanics.ItemCommoness<GoodsType>( GoodsType.Diamond, 1),
                new PlayMechanics.ItemCommoness<GoodsType>( GoodsType.Ruby, 1),
                new PlayMechanics.ItemCommoness<GoodsType>( GoodsType.Crystal, 1),
                new PlayMechanics.ItemCommoness<GoodsType>( GoodsType.sapphire, 1),
            });
        static readonly PlayMechanics.SelectRandomItem<Quality> RandomQuality = new PlayMechanics.SelectRandomItem<Quality>(
            new List<PlayMechanics.ItemCommoness<Quality>>
            {
                new PlayMechanics.ItemCommoness<Quality>(Quality.Low, 10),
                new PlayMechanics.ItemCommoness<Quality>(Quality.Medium, 15),
                new PlayMechanics.ItemCommoness<Quality>(Quality.High, 5),

            });

        override protected Gadgets.IGadget item
        { 
            get 
            {
                Percent stoneChance = new Percent(lootLvl > 1? 0.5f : 0.99f);
                Goods result = new Goods(baseStone, 1);
                ChanceAndLuck lootRoll = new ChanceAndLuck(lootLvl / LootfestLib.PickAxeAndSickleDamageRange.Max);
                //int secondRoll = 0;
                if (!stoneChance.DiceRoll())
                {
                    result.Type = RandomItem.GetRandom(lootRoll.SecondRollChance);
                }
                result.Quality = RandomQuality.GetRandom(lootRoll.SecondRollChance);
                return result; 
            } 
        }

        
    }
}
