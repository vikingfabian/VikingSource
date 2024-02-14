using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;

namespace VikingEngine.LF2.GameObjects.Gadgets
{
    class WannaBuyColl
    {
        static readonly IntervalF PriceRange = new IntervalF(0.2f, 1f);
        List<GameObjects.Gadgets.GoodsType> goods;
        float[] goodsPrices;

        public WannaBuyColl(IntVector2 chunkPos)
        {
            Data.RandomSeedInstance seedInstance = new Data.RandomSeedInstance();
            seedInstance.SetSeedPosition(chunkPos);

            goods = new List<GoodsType>
            {
                GoodsType.Glass,
                GoodsType.Broken_glass,
                GoodsType.Grapes,
                GoodsType.Wine,
                GoodsType.Fur,
                GoodsType.Ruby,
                GoodsType.sapphire,
                GoodsType.Crystal,
                GoodsType.Diamond,
                GoodsType.Silver,
                GoodsType.Gold,
                GoodsType.Mithril,

                GoodsType.Ancient_coins, 
                GoodsType.Barbarian_coins, GoodsType.Dwarf_coins, GoodsType.Elvish_coins,
                GoodsType.Orc_coins, GoodsType.South_kingdom_coins, GoodsType.Orc_mead, GoodsType.Beer,
                GoodsType.Cookie,
            };

            int remove = 2 + seedInstance.Next(4);

            for (int i = 0; i < remove; i++)
            {
                goods.RemoveAt( seedInstance.Next(goods.Count));
            }

            goodsPrices = new float[goods.Count];
            for (int i = 0; i < goods.Count; i++)
            {
                float price = seedInstance.Next(PriceRange) * LootfestLib.GoodsValue(goods[i]);
                goodsPrices[i] = price;
            }
        }

        public void ViewLookingFor(HUD.File file)
        {
            file.Text("-I will pay for these", Menu.TextFormatNote);
            for (int i = 0; i < goods.Count; i++)
            {
                GoodsType type = goods[i];
                if (GameObjects.Gadgets.GadgetLib.IsGoodsType(type))
                {

                    for (GameObjects.Gadgets.Quality q = (Quality)0; q < Quality.NUM; q++)
                    {
                        file.Add(new ShopGadgetButtonData(new HUD.Link(),
                            new ShopItem(new Gadgets.Goods(type, q, 1),
                            getGoodsPrice(i, q))));
                    }
                }
                else
                {
                    file.Add(new ShopGadgetButtonData(new HUD.Link(),
                            new ShopItem(new Gadgets.Item(type, 1),
                            getGoodsPrice(i, Quality.NUM))));
                }
            }
        }

        /// <returns>got anything to sell</returns>
        public bool SellList(HUD.File file, GadgetsCollection gadgetColl)
        {
            bool anyThingForSale = false;
            for (int i = 0; i < goods.Count; i++)
            {
                if (GameObjects.Gadgets.GadgetLib.IsGoodsType(goods[i]))
                {
                    for (GameObjects.Gadgets.Quality q = (Quality)0; q < Quality.NUM; q++)
                    {
                        int amount = gadgetColl.Goods.Get(goods[i], q);
                        if (amount > 0)
                        {
                            Goods g = new Goods(goods[i], q, amount);
                            file.Add(new ShopGadgetButtonData(
                                new HUD.Link(HUD.LinkType.Action, (int)NPC.TalkingNPCTalkLink.Special, i, (int)q, 0),
                                new ShopItem(g, getGoodsPrice(i, q))));
                            anyThingForSale = true;
                        }
                    }
                }
                else
                {
                    int amount = gadgetColl.GetItemAmount(goods[i]);
                    if (amount > 0)
                    {
                        Item g = new Item(goods[i], amount);
                        file.Add(new ShopGadgetButtonData(
                            new HUD.Link(HUD.LinkType.Action, (int)NPC.TalkingNPCTalkLink.Special, i, 0, 0),
                            new ShopItem(g, getGoodsPrice(i, Quality.NUM))));
                        anyThingForSale = true;
                    }
                }
            }
            return anyThingForSale;
        }

       
        public void Sell(HUD.IMenuLink link, GadgetsCollection gadgetColl, Players.Player player)
        {
            int goodsIndex = link.Value2;
            GoodsType goodsType = goods[goodsIndex];

            int amount;
            IGadget currentSelling;
            Quality quality = Quality.NUM;

            if (GameObjects.Gadgets.GadgetLib.IsGoodsType(goodsType))
            {
                quality = (Quality)link.Value3;
                amount = gadgetColl.Goods.Get(goodsType, quality);
                currentSelling = new Goods(goodsType, quality, amount);
            }
            else
            {
                amount = gadgetColl.GetItemAmount(goodsType);
                currentSelling = new Item(goodsType, amount);
            }


            if (amount == 1)
            {
                SellGoodsToPlayer(player.Progress.Items, goodsIndex, quality, amount);
            }
            else
            {
                HUD.ValueWheelDialogue valueDialogue = new HUD.ValueWheelDialogue(
                    player.SellItems, player.NPCValueWheelCancelEvent, new Range(1, amount), player.SafeScreenArea, amount, 1, link, null);
                player.ValueDialogue = valueDialogue;
                player.InteractionStoreValue1 = goodsIndex;
                player.InteractionStoreValue2 = (int)quality;
            }
        }

        public void PlayerSellAmount(Players.Player p, int amount)
        {
            SellGoodsToPlayer(p.Progress.Items, p.InteractionStoreValue1, (Quality)p.InteractionStoreValue2, amount);
        }

        void SellGoodsToPlayer(Players.PlayerGadgets playerItems, int index, Quality q, int amount)
        {
            GoodsType type = goods[index];
            IGadget currentSelling;
            if (GameObjects.Gadgets.GadgetLib.IsGoodsType(type))
            {
                currentSelling = new Goods(type, q, amount);
            }
            else
            {
                currentSelling = new Item(type, amount);
            }
            if (playerItems.RemoveItem(currentSelling))
            {
                playerItems.AddMoney(getGoodsPrice(index, q) * amount);
            }
        }

        int getGoodsPrice(int index, Quality q)
        {
            float multi = 1;
            switch (q)
            {
                case Quality.Low:
                    multi = LootfestLib.LowQualPrice;
                    break;
                case Quality.Medium:
                    multi = LootfestLib.MedQualPrice;
                    break;
                case Quality.High:
                    multi = LootfestLib.HighQualPrice;
                    break;
            }
            return Bound.Min(Convert.ToInt32(goodsPrices[index] * multi), 1);
        }
    }
}
