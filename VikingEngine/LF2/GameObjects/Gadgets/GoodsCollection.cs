using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.Gadgets
{
    class GoodsCollection
    {
        int[,] goodsQty_Type_Quality = new int[Goods.NumTypes, (int)Quality.NUM];

        //public int Get(GoodsType type)
        //{ return Get((int)type); }

        public int Get(GoodsType type)
        {
            int result = 0;
            for (Quality q = 0; q < Quality.NUM; ++q)
            {
                result += Get(type, q); //goodsQty_Type_Quality[type, i];
            }
            return result;
        }

        //public int Get(GoodsType type, Quality quality)
        //{
        //    return goodsQty_Type_Quality[(int)type, (int)quality];
        //}

        //public int Get(int type, int quality)
        //{ return goodsQty_Type_Quality[type, quality]; }

        public void WriteStream(System.IO.BinaryWriter w)
        {
            Goods g = new Goods();
            for (GoodsType type = Goods.FirstType; type < GoodsType.END_GOODS; type++)
            {
                g.Type = type;
                for (Quality quality = 0; quality < Quality.NUM; ++quality)
                {
                    if (Get(type, quality) > 0)//goodsQty_Type_Quality[type, quality] > 0)
                    {
                        g.Quality = quality;
                        g.Amount = Get(type, quality);//goodsQty_Type_Quality[type, quality];
                        g.WriteStream(w);
                    }
                }
            }
            w.Write(byte.MaxValue);
        }
        public void ReadStream(System.IO.BinaryReader r, byte version)
        {
            Goods g = new Goods();
            goodsQty_Type_Quality = new int[Goods.NumTypes, (int)Quality.NUM];
            while (true)
            {
                //byte type = r.ReadByte();
                //if (type == byte.MaxValue) return;
                //if (version == GadgetLib.FistReleaseVersion)
                //{
                //    type += Goods.FirstEnumIndex;
                //}
                //byte quality = r.ReadByte();
               // goodsQty_Type_Quality[type, quality] = DataStream.DataStreamLib.ReadGrowingAddValue(r);
               //Set((GoodsType)type, (Quality)quality, DataStream.DataStreamLib.ReadGrowingAddValue(r));

                g.ReadStream(r, version, WeaponGadget.GadgetSaveCategory.NUM_NONE);
                if ((byte)g.Type == byte.MaxValue) return; //found end byte

                Set(g.Type, g.Quality, g.Amount);
            }
        }

        public void Set(GoodsType type, Quality quality, int amount)
        {
            goodsQty_Type_Quality[(int)type - Goods.FirstEnumIndex, (int)quality] = amount;
        }
        public int Get(GoodsType type, Quality quality)
        {
            return goodsQty_Type_Quality[(int)type - Goods.FirstEnumIndex, (int)quality];
        }

        public GoodsCollection()
        {
            //add some random items
            //for (int i = 0; i < 10; i++)
            //{
            //    AddItem(RandomItem());
            //}
        }
        public bool Empty
        {
            get
            {
                for (int type = 0; type < Goods.NumTypes; type++)
                {
                    for (int quality = 0; quality < (int)Quality.NUM; quality++)
                    {
                        if (goodsQty_Type_Quality[type, quality] > 0)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        public GoodsCollection(int[,] goods)
        {
            this.goodsQty_Type_Quality = goods;
        }
        public GoodsCollection CloneMe()
        {
            int[,] clone = (int[,])goodsQty_Type_Quality.Clone();
            return new GoodsCollection(clone);
        }

        public void AddCollection(GoodsCollection collection)
        {
            for (int type = 0; type < Goods.NumTypes; type++)
            {
                for (int quality = 0; quality < (int)Quality.NUM; quality++)
                {
                    goodsQty_Type_Quality[type, quality] += collection.goodsQty_Type_Quality[type, quality];//.Get(type, quality);
                }
            }
        }


        public void Add(List<Goods> goods)
        {
            foreach (Goods g in goods)
            { this.Add(g); }
        }
        public void Add(Goods goods)
        {
            goodsQty_Type_Quality[(int)goods.Type - Goods.FirstEnumIndex, (int)goods.Quality] += goods.Amount;
        }
        public bool TryRemove(Goods goods)
        {
            if (GotItem(goods))
            {
                //goodsQty_Type_Quality[(int)goods.Type, (int)goods.Quality] -= goods.Amount;
                Remove(goods);
                return true;
            }
            return false;
        }
        public bool Remove(Goods goods)
        {
            int x = (int)goods.Type - Goods.FirstEnumIndex;
            int y = (int)goods.Quality;
            goodsQty_Type_Quality[x, y] -= goods.Amount;
            if (goodsQty_Type_Quality[x, y] < 0)
            {
                goodsQty_Type_Quality[x, y] = 0;
                return false;
            }
            return true;
        }

        public void AddItem(Gadgets.IGadget gadget)
        {
            Add((Goods)gadget);
            //goodsQty_Type_Quality[(int)goods.Type, (int)goods.Quality] += goods.Amount;
        }

        const int ByteShift = 4;
        const int FirstShiftValue = 1 + 2 + 4 + 8;

        public void ToMenu(HUD.File file, int currentIndex, GadgetLink linkEvent)
        {
            for (GoodsType type = Goods.FirstType; type < GoodsType.END_GOODS; ++type)
            {
                for (Quality quality = 0; quality < Quality.NUM; ++quality)
                {
                    if (Get(type, quality) > 0)//goodsQty_Type_Quality[type, quality] > 0)
                    {
                        file.Add(new GadgetButtonData(linkEvent, new Goods(type, quality, Get(type, quality)), SpriteName.LFMenuItemBackground, true));
                    }
                }
            }
        }

        public List<IGadget> ToGadgetList()
        {
            List<IGadget> result = new List<IGadget>();

            for (GoodsType type = Goods.FirstType; type < GoodsType.END_GOODS; ++type)
            {
                for (Quality quality = 0; quality < Quality.NUM; ++quality)
                {
                    if (Get(type, quality) > 0)
                    {
                        result.Add(new Goods(type, quality, Get(type, quality)));
                    }
                }
            }
            return result;
        }

        public List<Goods> ToGoodsList()
        {
            List<Goods> result = new List<Goods>();

            for (GoodsType type = Goods.FirstType; type < GoodsType.END_GOODS; ++type)
            {
                for (Quality quality = 0; quality < Quality.NUM; ++quality)
                {
                    if (Get(type, quality) > 0)
                    {
                        result.Add(new Goods(type, quality, Get(type, quality)));
                    }
                }
            }
            return result;
        }

        public IGadget PickItemFromHashTag(ushort hashtag, int amount, bool remove)
        {

            for (GoodsType type = Goods.FirstType; type < GoodsType.END_GOODS; ++type)
            {
                for (Quality quality = 0; quality < Quality.NUM; ++quality)
                {
                    int gotAmount = Get(type, quality);
                    if (gotAmount > 0)
                    {
                        Goods g = new Goods(type, quality, gotAmount);
                        if (g.ItemHashTag == hashtag )
                        {
                            if (gotAmount < amount)
                                return null;
                            if (remove)
                            {
                                Set(type, quality, gotAmount - amount);
                            }
                            g.Amount = amount;
                            return g;
                        }
                    }
                }
            }
            return null;
        }

        public IGadget PickItemFromMenu(HUD.Link link, int amount, bool remove)
        {
            //link: 1: Item type, 2: Index, 3: UnderIndex, 4:Detail
            Quality quality = (Quality)link.Value2;
            GoodsType type = (GoodsType)link.Value3;

            Goods result = new Goods(type, quality,
                lib.SmallestOfTwoValues(Get(type, quality), amount));

            if (remove)
            {
                this.Remove(result);
                //goodsQty_Type_Quality[type, quality] -= result.Amount;
            }
            return result;
        }


        public Goods PickItemFromIndex(int index, int amount, bool remove)
        {

            Quality quality = (Quality)(index & FirstShiftValue);
            GoodsType type = (GoodsType)(index >> ByteShift);

            Goods result = new Goods(type, quality,
                lib.SmallestOfTwoValues( Get(type, quality), amount));

            if (remove)
            {
                this.Remove(result);
                //goodsQty_Type_Quality[type, quality]-= result.Amount;
            }
            return result;
        }

        //Goods RandomItem()
        //{
        //    Goods randomGoods = new Goods();
        //    randomGoods.Amount = 1;
        //    randomGoods.Type = (GoodsType)Data.RandomSeed.Instance.Next((int)Goods.NumTypes);
        //    byte quality = Data.RandomSeed.Instance.NextByte();
        //    if (quality < 50)
        //    {
        //        randomGoods.Quality = Quality.Low;
        //    }
        //    else if (quality < 210)
        //    {
        //        randomGoods.Quality = Quality.Medium;
        //    }
        //    else
        //    {
        //        randomGoods.Quality = Quality.High;
        //    }

        //    return randomGoods;
        //}
        public bool GotItem(Goods goods)
        {
            return Get(goods.Type, goods.Quality) >= goods.Amount;//goodsQty_Type_Quality[(int)goods.Type, (int)goods.Quality] >= goods.Amount;
        }
        int AmountOfType(GoodsType type)
        {
            int result = 0;
            for (Quality q = 0; q < Quality.NUM; ++q)
            {
                result += Get(type, q);//goodsQty_Type_Quality[(int)type, q];
            }
            return result;
        }

        public int Weight()
        {
            int result = 0;
            for (int i = 0; i < goodsQty_Type_Quality.GetLength(0); ++i)
            {
                for (int j = 0; j < goodsQty_Type_Quality.GetLength(1); ++j)
                {
                    result += goodsQty_Type_Quality[i, j];
                }
            }
            return result * LootfestLib.GoodsWeight;
        }

        public bool Pay(GoodsCollection goods)
        {
            return this.Pay(goods.ToGoodsList());
        }

        public bool Pay(List<Goods> goods)
        {
            GoodsCollection testPay = this.CloneMe();
            foreach (Goods g in goods)
            {
                if (!testPay.TryRemove(g))
                    return false;
            }
            this.goodsQty_Type_Quality = testPay.goodsQty_Type_Quality;
            return true;
        }

        //public bool PayCraftingTemplate(Data.Gadgets.CraftingTemplate template)
        //{
        //    return Pay(template.useItems);
        //}
        //public int GetAmountOfItems(GoodsType type)
        //{
        //    return goodsQty_Type_Quality[(int)type, (int)Goods.NonQualityGoods];
        //}
        //public void SetAmountOfItems(GoodsType type, int amount)
        //{
        //    goodsQty_Type_Quality[(int)type, (int)Goods.NonQualityGoods] = amount;
        //}
        public bool GotItemAnyQuality(GoodsType type, int amount)
        {
            return AmountOfType(type) >= amount;
        }

        /// <returns>Goods to pay with, returns null if cant pay</returns>
        public List<Goods> CanPayThisQualityOrHigher(Goods item)
        {
            List<Goods> result = null;
            int leftToPay = item.Amount;
            for (Quality checkQ = item.Quality; checkQ < Quality.NUM; ++checkQ)
            {
                int hasAmount = lib.SmallestOfTwoValues(Get(item.Type, checkQ), leftToPay);
                if (hasAmount > 0)
                {
                    if (result == null) result = new List<Goods>();
                    result.Add(new Goods(item.Type,checkQ, hasAmount));
                    leftToPay -= hasAmount;
                    if (leftToPay <= 0)
                    {
                        return result;
                    }
                }

            }
            return null;
        }

        //public QualityAccess GotItemThisQualityOrHigher(Goods goods)
        //{
        //    QualityAccess result = new QualityAccess();
        //    if (goodsQty_Type_Quality[(int)goods.Type, (int)goods.Quality] >= 1)
        //    {
        //        result.Use = new int[(int)Quality.NUM];
        //        int gotleft = goods.Amount;
        //        for (Quality q = goods.Quality; q < Quality.NUM; q++)
        //        {
        //            int amountOfQuality = goodsQty_Type_Quality[(int)goods.Type, (int)q];
        //            if (amountOfQuality >= gotleft)
        //            {
        //                result.Use[(int)q] = gotleft;
        //                result.GotQuality = true;
        //                return result;
        //            }
        //            else
        //            {
        //                result.Use[(int)q] = amountOfQuality;
        //                gotleft -= amountOfQuality;
        //            }

        //        }


        //    }
        //    return result;
        //}

        
        public void DebugGet10OfEach()
        {
            for (int type = Goods.FirstEnumIndex; type < Goods.NumTypes; type++)
            {
                Add(new Goods((GoodsType)type, Quality.Medium, 10));
            }
        }
    }
   
}
