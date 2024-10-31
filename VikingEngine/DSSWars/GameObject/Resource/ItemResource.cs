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

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)type);
            w.Write((ushort)amount);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            type = (ItemResourceType)r.ReadByte();
            amount = r.ReadUInt16();
        }

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
        DryWood,
        Planks,
        Barrel,
        Box,
        Wheel,
        Wagon,
        Coal,
        Tar,

        Linen,
        Wool,
        Rope,
        Cloth,
        Clothes,
        Bag,

        IronOre_G,
        TinOre,
        CupperOre,
        SilverOre,
        GoldOre,
        MithrilOre,

        Iron_G,
        Steel,
        Tin,
        Cupper,
        Bronze,
        Silver,
        Gold,
        Mithril,

        Bow,
        LongBow,

        SharpStick,
        Sword,
        Shield,

        LightArmor,
        MediumArmor,
        HeavyArmor,

        StoneBlock,
        Stone_G,
        Clay,
        Brick,
        Pot,

        Wheat,
        Bread,
        Beer,
        Food_G,

        Water_G,
        SaltWater,

        Fuel_G,
        Poop,

        Ballista,
        KnightsLance,
        TwoHandSword,

        Wood_Group,
        RawFood_Group,
        SkinLinen_Group,
        Men,
        Rapeseed,
        Hemp,

        NUM,
    }

    //enum ItemResourceType
    //{
    //    NONE,

    //    Hen,
    //    Pig,
    //    Goat,
    //    Ox,
    //    Egg,
    //    Milk,
    //    Cheese,
    //    Meat,

    //    RawLeather, 
    //    Leather,

    //    WoodShoe,
    //    LeatherShoe,

    //    SoftWood,
    //    HardWood,
    //    DryWood,
    //    Planks,
    //    Barrel,
    //    Box,
    //    Wheel,
    //    Wagon,
    //    Coal, 
    //    Tar,

    //    Linen,
    //    Wool,
    //    Rope,
    //    Cloth,
    //    Clothes,
    //    Bag,

    //    IronOre_G,
    //    TinOre,
    //    CupperOre,
    //    SilverOre,
    //    GoldOre,
    //    MithrilOre,

    //    Iron_G,
    //    Steel,
    //    Tin,
    //    Cupper,
    //    LongBow,
    //    Silver,
    //    Gold,
    //    Mithril,

    //    Bow,


    //    SharpStick,
    //    Sword,
    //    Shield,

    //    LightArmor,
    //    MediumArmor,
    //    HeavyArmor,
    //    //Padding,
    //    //Gambeson,
    //    //Brigandine,
    //    //PlateArmor,

    //    StoneBlock,
    //    Stone_G,
    //    Clay,
    //    Brick,

    //    Hemp,

    //    Wheat,
    //    Rapeseed,
    //    RES_,
    //    Beer,
    //    Food_G,

    //    Water_G,
    //    SaltWater,

    //    Fuel_G,
    //    Poop,

    //    Ballista,
    //    KnightsLance,
    //    TwoHandSword,

    //    Wood_Group,
    //    RawFood_Group,
    //    SkinLinen_Group,
    //    Men,



    //    NUM,
    //}
}
