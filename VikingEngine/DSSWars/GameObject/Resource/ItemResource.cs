using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    struct ItemResource
    {
        public static readonly ItemResource Empty = new ItemResource();

        public ItemResourceType type;

        /// <summary>
        /// Goes from 0: broken, to 255: masterpiece
        /// </summary>
        public int quality;

        /// <summary>
        /// Tracks time, risk and expences. Based on seconds of work.
        /// </summary>
        public int cost;

        public int amount;

        public ItemResource(ItemResourceType type, int quality, int cost, int amount)
        {
            this.type = type;
            this.quality = quality;
            this.cost = cost;
            this.amount = amount;
        }

        public void merge(ItemResource other)
        {
            quality = (quality * amount + other.quality * other.amount);
            amount += other.amount;
            quality /= amount;
            cost += other.cost;
        }

        public override string ToString()
        {
            return "Item: " + amount.ToString() + type.ToString();
        }
    }    
    
    enum ItemResourceType
    {
        NONE,

        Hen,
        Pig,
        Goat,
        Ox,
        Egg,
        Milk,
        Cheese,
        Meat,

        RawLeather, 
        Leather,

        WoodShoe,
        LeatherShoe,
        
        SoftWood,
        HardWood,
        Planks,
        Barrel,
        Box,
        Wheel,
        Wagon,
        Coal, 
        Tar,

        Linnen,
        Wool,
        Rope,
        Cloth,
        Clothes,
        Bag,

        IronOre,
        TinOre,
        CupperOre,
        SilverOre,
        GoldOre,
        MithrilOre,

        Iron,
        Steel,
        Tin,
        Cupper,
        Bronze,
        Silver,
        Gold,
        Mithril,
        
        Bow,
        Arrow,
        Sword,
        Shield,
        
        Padding,
        Gambeson,
        Brigandine,
        PlateArmor,

        StoneBlock,
        Stone,
        Clay,
        Brick,
        Pot,

        Wheat,
        Bread,
        Beer,

        Water,
        SaltWater,

        Fuel,
        Poop,

        NUM
    }
}
