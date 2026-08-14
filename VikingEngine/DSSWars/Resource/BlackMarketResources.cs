using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.Players;
using VikingEngine.DSSWars.Data;

namespace VikingEngine.DSSWars.Resource
{

    struct MarketResource
    {
        public ItemResourceType item;
        public Money cost;

        public MarketResource(ItemResourceType item, int goldCost)
        { 
            this.item = item;
            cost = Money.FromGold(goldCost);
        }
    }
    static class BlackMarketResources
    {
        
        static int Cost_RawFood = DssConst.FoodGoldValue_BlackMarket - 5;
        static int Cost_Food = DssConst.FoodGoldValue_BlackMarket;
        static int Cost_Wood = 50;
        static int Cost_Stone = 60;
        static int Cost_Brick = 100;
        static int Cost_SkinAndLinnen = 50;
        static int Cost_Iron = 500;

        static readonly MarketResource[] Resources =
        {
            new MarketResource(ItemResourceType.Wood_Group, Cost_Wood),
            new MarketResource(ItemResourceType.Stone_G, Cost_Stone),
            new MarketResource(ItemResourceType.Brick, Cost_Brick),
            new MarketResource(ItemResourceType.Iron_G, Cost_Iron),
            new MarketResource(ItemResourceType.RawFood_Group, Cost_RawFood),
            new MarketResource(ItemResourceType.SkinLinen_Group, Cost_SkinAndLinnen),
        };

        static readonly int[] PurchaseCount = { 20, 100, 500 };

        

        public static void AiPurchaseUpdate(City city, Faction faction)
        {
            if (city != null && city.pfaction.GetFaction().GetGold(city) > 1000000) 
            {
                for (int i = 0; i < 3; i++)
                {
                    MarketResource res = arraylib.RandomListMember(Resources);
                    var gres = city.GetGroupedResource(res.item);
                    if (gres.amount < 300)
                    {
                        city.blackMarketPurchase(res.item, 100, res.cost);
                    }
                }
            }
        }

        public static void AiPurchaseWood(City city, Faction faction)
        {
            int count = 5;
            if (faction.payGold(count * Cost_Wood, false, city))
            {
                city.AddGroupedResource(EntityComponent.CityResourceIndex.wood, count);
                //city.res_wood.amount += count;
            }
        }
        public static bool AiPurchaseIron(City city, Faction faction)
        {
            int count = CraftBuildingLib.CraftSmith_IronUse;
            if (faction.payGold(count * Cost_Iron, false, city))
            {
                city.AddGroupedResource(EntityComponent.CityResourceIndex.iron, count, false);
                //city.res_iron.amount += count;
                return true;
            }
            return false;
        }

        static Money CostMultiply(City city, Money cost)
        {
            if (city.cityCulture == CityCulture.Backtrader)
            {
                return cost / 2;
            }
            return cost;
        }

        public static void ToHud(LocalPlayer player, RichBoxContent content, City city)
        {
            if (city.cityCulture == CityCulture.Lawbiding && player.tutorial == null)
            {
                city.cultureToHud(player, content, false);
                return;
            }

            content.h2(DssRef.lang.Hud_PurchaseTitle_Resources, HudLib.TitleColor_Head);

            content.Add(new RbSeperationLine());
            int lineCount = 0;
            foreach (var r in Resources)
            {
                ResourceToHud(r, player, content, city);
                if (++lineCount >= 2)
                {
                    lineCount = 0;
                    content.Add(new RbSeperationLine());
                }
            }
            content.newLine();
            HudLib.LabelAndText(content, SpriteName.WarsStockpileLimit, DssRef.lang.Hud_Available, player.BlackMarketCount.ToString());

            content.newParagraph();
            content.Add(new RbSeperationLine());
            content.Add(new RbImage(SpriteName.rtsUpkeep));
            content.Add(new RbText(Cost_Food.ToString()));
            content.Add(new RbTab(0.3f));
            content.Add(new RbImage(SpriteName.WarsResource_Food));
            content.space();
            content.Add(new RbText(DssRef.lang.Resource_TypeName_Food));
            content.space();
            HudLib.InfoButton(content, new RbTooltip(tooltip));

        }

        static void tooltip(RichBoxContent content, object tag)
        {
            content.h2(SpriteName.WarsResource_Food, DssRef.lang.Resource_TypeName_Food, HudLib.TitleColor_Name);
            content.text(DssRef.lang.Info_WhenFoodRunsOut);
            content.text(DssRef.lang.Hud_NoLimit, HudLib.InfoYellow_Light);
        }

        public static void ResourceToHud(MarketResource res, LocalPlayer player, RichBoxContent content, City city)
        {
           
            Resource(CostMultiply(city, res.cost), res.item);

            void Resource(Money cost, ItemResourceType resourceType)
            {
                int count = 1;
                //int non = 0;
                IconName.Item(resourceType, out SpriteName itemIcon, out string itemName);

                content.newLine();

                content.Add(new RbImage(SpriteName.rtsUpkeep));
                content.Add(new RbText(cost.ToString()));
                content.Add(new RbTab(0.3f));

                ArtButton button = new ArtButton( RbButtonStyle.Primary,new List<AbsRichBoxMember>
                    {
                        new RbImage(itemIcon),
                        new RbSpace(0.5f),
                        new RbText(TextLib.LargeFirstLetter( itemName)),
                    },
                new RbAction3Arg<ItemResourceType, int, Money>(city.blackMarketPurchase, resourceType, count, cost, RbSoundType.Buy),
                tooltip(count), player.pfaction.GetFaction().hasMoney(cost, city));

                content.Add(button);
                content.Add(new RbTab(0.5f));
                
                int prevCount = int.MaxValue;
                foreach (var c in PurchaseCount)
                {
                    count = c;

                    if (player.BlackMarketCount < count && player.BlackMarketCount > prevCount)
                    {
                        count = player.BlackMarketCount;
                    }
                    prevCount = c;

                    bool canPay = player.pfaction.GetFaction().hasMoney(cost * count, city);
                    bool available = count <= player.BlackMarketCount;

                    ArtButton xbutton = new ArtButton( RbButtonStyle.Secondary, new List<AbsRichBoxMember>
                        {
                            new RbText(string.Format(DssRef.lang.Hud_XTimes, count)),
                        },
                    new RbAction3Arg<ItemResourceType, int, Money>(city.blackMarketPurchase, resourceType, count, cost, RbSoundType.Buy),
                    tooltip(count), canPay && available);
                    content.Add(xbutton);
                }


                AbsRbAction tooltip(int count)
                {
                    return new RbTooltip((RichBoxContent content, object tag) =>
                    {
                        content.h2(DssRef.lang.Hud_PurchaseTitle_Cost, HudLib.TitleColor_Label);
                        content.newLine();
                        HudLib.ResourceCost(content, ResourceType.Gold, cost.GetGold32() * count, (int)player.pfaction.GetFaction().GetGold(city));


                        bool outOfStock = count > player.BlackMarketCount;
                        if (outOfStock)
                        {
                            content.newLine();
                            content.iconicontext(SpriteName.WarsStockpileLimit, HudLib.NotAvailableIcon, DssRef.todoLang.Hud_OutOfStock);
                        }

                        content.newParagraph();

                        content.h2(DssRef.lang.Hud_PurchaseTitle_CurrentlyOwn, HudLib.TitleColor_Label);
                        bool reachedBuffer = false;

                        city.GetGroupedResource(resourceType).toMenu(content, resourceType, ref reachedBuffer);
                        

                    }, count);
                }
            }
        }


    }
}
