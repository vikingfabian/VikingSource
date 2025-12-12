using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Resource
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

        public ItemResource(ItemResourceType type, int amount)
        {
            this.type = type;
            this.quality = 1;
            this.cost = 1;
            this.amount = amount;
        }

        public void merge(ItemResource other)
        {
            quality = quality * amount + other.quality * other.amount;
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
                
        Egg,
        Meat,

        Wheat,
        Beer,
        Food_G,
        ConservedFood,

        Water_G,
        Fuel_G,

        Leather,
        Stone_G,
        Clay,
        Brick,

        SoftWood,
        HardWood,
        DryWood,
        StorageBox,
        Wagon2Wheel,
        Wagon4Wheel,
        WagonClosed,
        WagonIron,
        WagonSteel,
        Toolkit,
        CoolingFluid,

        Wood_Group,
        RawFood_Group,
        SkinLinen_Group,
        
        Rapeseed,
        Hemp,
        Linen,

        Salt,
        Coal,
        BogIron,
        IronOre_G,
        TinOre,
        CopperOre,
        SilverOre,
        GoldOre,
        RawMithril,

        Iron_G,
        Steel,
        Tin,
        Copper,
        Bronze,
        Silver,
        Gold,
        Mithril,

        Bow,
        LongBow,

        SharpStick,
        Sword,
        Pike,

        BucklerShield, RoundShield, HeaterShield, TowerShield,


        BronzeArmor,
        PaddedArmor,
        HeavyPaddedArmor,
        IronArmor,
        HeavyIronArmor,
        LightPlateArmor,
        FullPlateArmor,
        MithrilArmor,

        MountBronzeArmor,
        MountPaddedArmor,
        MountHeavyPaddedArmor,
        MountIronArmor,
        MountHeavyIronArmor,
        MountLightPlateArmor,
        MountFullPlateArmor,
        MountMithrilArmor,

        Ballista,
        //KnightsLance,
        TwoHandSword,

        BronzeSword,
        ShortSword,
        HandSpear,
        LongSword,
        Warhammer,
        MithrilSword,
        SlingShot,
        ThrowingSpear,
        Crossbow,
        MithrilBow,

        Sulfur,
        LeadOre,
        Lead,
        BloomeryIron,
        CastIron,

        BlackPowder,
        GunPowder,
        LedBullet,
        HandCulverin,
        HandCannon,
        Rifle,
        Blunderbuss,

        Manuballista,
        Catapult,
        UN_Trebuchet,
        UN_BatteringRam,
        SiegeCannonBronze,
        ManCannonBronze,
        SiegeCannonIron,
        ManCannonIron,
        
        CopperCoin,
        BronzeCoin,
        SilverCoin,
        ElfCoin,

        AutomatedItem,

        RoseWarrior_soldier,
        RoseWarrior_tank,
        RoseWarrior_dog,

        Men,
        NobelMen,
        ServiceMen,

        Hen,
        Pig,
        Oxen,
        KineOxen,

        Dog,
        Hound,

        Pony,
        Horse,
        WarHorse,
        DraftHorse,

        WildPig,
        WildHog,
        WarHog,
        StagHog,

        Wolf,
        Warg,
        AlphaWarg,

        WildCat,
        Lion,
        WarLion,

        Elephant,
        WarElephant,
        Oliphant,

        Palisade,
        UNUSED,
        //WorkerTent,
        Settler,

        NUM,

        RESOURCES,
    }

}
