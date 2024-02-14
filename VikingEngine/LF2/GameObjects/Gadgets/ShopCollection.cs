using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.GameObjects.EnvironmentObj;

namespace VikingEngine.LF2.GameObjects.Gadgets
{
    class ShopCollection 
    {
        public List<ShopItem> items;
        static readonly List<GoodsType> Stones = new List<GoodsType> { 
            GoodsType.Granite, GoodsType.Marble, GoodsType.Sandstone, GoodsType.Flint, };
        static readonly List<GoodsType> GoodsForSale = new List<GoodsType>
        {
            GoodsType.Apple,
            GoodsType.Apple_pie,
            GoodsType.Grilled_apple,
            GoodsType.Grilled_meat,
            GoodsType.Seed,
            GoodsType.Grapes,
            //
            GoodsType.Glass,
            //
            GoodsType.Diamond, GoodsType.Ruby, GoodsType.Crystal, GoodsType.sapphire,
            //
            GoodsType.Iron, GoodsType.Bronze, GoodsType.Silver, GoodsType.Gold, GoodsType.Mithril,
            GoodsType.Iron, GoodsType.Bronze, GoodsType.Silver,
            //
            GoodsType.Stick,
            GoodsType.Wood,
        };
        
        const int SalesManAdd = 5;

        public ShopCollection(MapChunkObjectType type, IntVector2 chunkPos)
        {
            Data.RandomSeedInstance seedInstance = new Data.RandomSeedInstance();
            seedInstance.SetSeedPosition(chunkPos);

            items = new List<ShopItem>();
            int numPackets;
            IntervalF cheapPriceRange = new IntervalF(0.5f, 1.4f);

            switch (type)
            {
                case MapChunkObjectType.Lumberjack:
                    List<GoodsType> lumberGoods = new List<GoodsType>
                    {
                        GoodsType.Stick,
                        GoodsType.Wood,
                        GoodsType.Stick,
                        GoodsType.Wood,
                        GoodsType.Stick,
                        GoodsType.Wood,
                        GoodsType.Skin,
                        GoodsType.Leather,
                        GoodsType.Fur,
                        GoodsType.Scaley_skin,
                        GoodsType.Feathers,
                        GoodsType.Feathers,
                        GoodsType.Meat,
                        //
                        GoodsType.Nose_horn,
                        GoodsType.Horn,
                        GoodsType.Tusks,
                        GoodsType.Sharp_teeth,
                    };
                    
                    for (int i = 0; i < 2; i++)
                    {
                        numPackets = 1 + seedInstance.Next(8);
                        for (int packet = 0; packet < numPackets; packet++)
                        {
                            pickFromList(seedInstance, lumberGoods, numPackets, cheapPriceRange, false, false);
                        }
                    }
                    break;
                case MapChunkObjectType.Miner:
                    
                    List<GoodsType> minerGoods = new List<GoodsType>
                    {
                        GoodsType.Granite, GoodsType.Marble, GoodsType.Sandstone, GoodsType.Flint,
                        GoodsType.Iron, GoodsType.Bronze, GoodsType.Silver, GoodsType.Gold,
                    };
                    
                    for (int i = 0; i < 2; i++)
                    {
                        numPackets = 1 + seedInstance.Next(8);
                        for (int packet = 0; packet < numPackets; packet++)
                        {
                            ////int packetSz = 1;
                            pickFromList(seedInstance, minerGoods, numPackets, cheapPriceRange, false, false);
                        }
                    }
                     
                    break;
                case MapChunkObjectType.Salesman:
                    IntervalF priceRange = new IntervalF(0.8f, 2f);
                    pickFromList(seedInstance, GoodsForSale, 6 + seedInstance.Next(8), priceRange, false, true);
                    addItems(seedInstance, GoodsType.Arrow, 20, new Range(2, 10), priceRange);
                    addItems(seedInstance, GoodsType.SlingStone, 30, new Range(2, 10), priceRange);
                    addItems(seedInstance, GoodsType.Javelin, 16, new Range(1, 6), priceRange);
                    GoodsType[] bombs = new GoodsType[] { GoodsType.Evil_bomb, GoodsType.Fire_bomb, GoodsType.Fluffy_bomb, GoodsType.Lightning_bomb, GoodsType.Poision_bomb, };
                    const byte BombChance = 8;
                    foreach (GoodsType bombType in bombs)
                    {
                        if (seedInstance.BytePercent(BombChance))
                        {
                            addItems(seedInstance, bombType, 8, new Range(1, 4), priceRange);
                        }
                    }

                    const int NumApplePieMaterial = 4;
                    ShopItem apples = new ShopItem(new Goods(GoodsType.Apple, 4), 10);
                    ShopItem seed = new ShopItem(new Goods(GoodsType.Seed, 2), 20);
                    for (int i = 0; i < NumApplePieMaterial; i++)
                    {
                        items.Add(apples); items.Add(seed);
                    }

                    //basic weapons

                    GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType[] StandardHandWeaponsForSale = new WeaponGadget.StandardHandWeaponType[]
                    {
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeAxe,
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeSword,
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronSword,
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronAxe,
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeLongSword,
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronLongSword,
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeLongAxe,
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronLongAxe,
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BronzeDagger,
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.IronDagger,
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.PickAxe,
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.Sickle,
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.Spear,
                    };
                    
                    int numStandardWeapons = seedInstance.Next(2);
                   
                    for (int i = 0; i < numStandardWeapons; i++)
                    {
                        GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType weptype = arraylib.RandomListMemeber(StandardHandWeaponsForSale);
                            //(WeaponGadget.StandardHandWeaponType)seedInstance.Next(
                            //(int)GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.NUM);
                        items.Add(new ShopItem(new GameObjects.Gadgets.WeaponGadget.HandWeapon(weptype), 
                            LootfestLib.StandardHandWeaponValue(weptype) * seedInstance.Next(priceRange)));
                    }
                    
                    //bows
                    byte bowRnd = seedInstance.NextByte();
                    if (bowRnd < 20)
                    {
                        Data.Gadgets.BluePrint bowType = bowRnd < 5? Data.Gadgets.BluePrint.LongBow : Data.Gadgets.BluePrint.ShortBow;
                        items.Add(new ShopItem(new GameObjects.Gadgets.WeaponGadget.Bow(bowType), 
                            LootfestLib.StandardBowValue(bowType)));
                    }
                    break;
            }
        }

        void addItems(Data.RandomSeedInstance seedInstance, GoodsType type, int amountInPacket, Range numPacketsRange, IntervalF priceRange)
        {
            int price = (int)(amountInPacket * LootfestLib.ItemsValue(type) * seedInstance.Next(priceRange));
            int numPackets = seedInstance.Next(numPacketsRange);
            ShopItem packet = new ShopItem(new Item(type, amountInPacket), price);
            for (int i = 0; i < numPackets; i++)
            {
                items.Add(packet);
            }
        }
        
        void pickFromList(Data.RandomSeedInstance seedInstance, List<GoodsType> Goods, int pickNum, IntervalF priceRange, bool allQualities, bool lowQual)
        {
           
            List<GoodsType> goods = new List<GoodsType>(Goods.Count);
            goods.AddRange(Goods);

            for (int i = 0; i < pickNum; i++)
            {
                int pickIndex = seedInstance.Next(goods.Count);
                GoodsType goodsType = goods[pickIndex];
                goods.RemoveAt(pickIndex);

                int amount = 1;
                float baseCost = LootfestLib.GoodsValue(goodsType) * seedInstance.Next(priceRange);

                if (baseCost < 1)
                {
                    amount = 5;
                }
                if (baseCost < 20)
                {
                    amount += seedInstance.Next(5);
                }
                baseCost *= amount;

                if (allQualities)
                {
                    items.Add(new ShopItem(new Goods(goodsType, Quality.High, 10), (int)(baseCost * LootfestLib.LowQualPrice)));
                    items.Add(new ShopItem(new Goods(goodsType, Quality.Medium, 20), (int)(baseCost * LootfestLib.MedQualPrice)));
                    if (lowQual)
                        items.Add(new ShopItem(new Goods(goodsType, Quality.Low, 20), (int)(baseCost * LootfestLib.HighQualPrice)));
                }
                else
                {
                    byte rndQual = seedInstance.NextByte();
                    Quality q;
                    int price;

                    if (lowQual && rndQual < 130)
                    {
                        q = Quality.Low;
                        price = (int)(baseCost * LootfestLib.LowQualPrice);
                    }
                    else if (rndQual < 200)
                    {
                        q = Quality.Medium;
                        price = (int)(baseCost * LootfestLib.MedQualPrice);
                    }
                    else
                    {
                        q = Quality.High;
                        price = (int)(baseCost * LootfestLib.HighQualPrice);
                    }

                    items.Add(new ShopItem(new Goods(goodsType, q, amount), price));
                }
                
            }
        }
        static readonly int Dialogue = (int)Players.Link.Interact_dialogue;
        public void ToMenu(HUD.File file)
        {
            for (int i = 0; i < items.Count; i++)
            {
                file.Add(new ShopGadgetButtonData(new HUD.Link(Dialogue, i), items[i]));
            }
        }
    }
    struct ShopItem
    {
        public IGadget Item;
        public int Price;
        public ShopItem(IGadget Item, float price)
            :this(Item, Convert.ToInt32(price))
        { }
        public ShopItem(IGadget Item, int price)
        {
            this.Item = Item;
            this.Price =Bound.Min(price, 1);
        }
        public override string ToString()
        {
            return Item.ToString() + ", " + Price.ToString() + LootfestLib.MoneyText;
        }
    }
}
