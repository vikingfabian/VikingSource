using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Resource
{
    static class BlackMarketResources
    {
        static readonly ItemResourceType[] Resources =
        {
            ItemResourceType.Wood_Group,
            ItemResourceType.Stone_G,
            ItemResourceType.RawFood_Group,
            ItemResourceType.SkinLinen_Group,
            ItemResourceType.Food_G,
            ItemResourceType.Iron_G,
        };
        static readonly int[] PurchaseCount = { 20, 100, 500 };

        static int Cost_RawFood = DssConst.FoodGoldValue_BlackMarket - 4;
        static int Cost_Food = DssConst.FoodGoldValue_BlackMarket;
        static int Cost_Wood = 50;
        static int Cost_Stone = 60;
        static int Cost_SkinAndLinnen = 50;
        static int Cost_Iron = 500;

        public static void AiPurchaseWood(City city, Faction faction)
        {
            int count = 5;
            if (faction.payMoney(count * Cost_Wood, false, city))
            {
                city.res_wood.amount += count;
            }
        }
        public static bool AiPurchaseIron(City city, Faction faction)
        {
            int count = CraftBuildingLib.CraftSmith_IronUse;
            if (faction.payMoney(count * Cost_Iron, false, city))
            {
                city.res_iron.amount += count;
                return true;
            }
            return false;
        }

        static int CostMultiply(City city, int cost)
        {
            if (city.Culture == CityCulture.Backtrader)
            {
                return cost / 2;
            }
            return cost;
        }

        public static void ToHud(LocalPlayer player, RichBoxContent content, City city)
        {
            if (city.Culture == CityCulture.Lawbiding)
            {
                city.cultureToHud(player, content, false);
                return;
            }

            content.h2(DssRef.lang.Hud_PurchaseTitle_Resources).overrideColor = HudLib.TitleColor_Label;

            foreach (var r in Resources)
            {
                ResourceToHud(r, player, content, city);
            }
            //Resource(CostMultiply(city, Cost_RawFood), ItemResourceType.RawFood_Group, DssRef.lang.Resource_TypeName_RawFood);
            //Resource(CostMultiply(city, Cost_Food), ItemResourceType.Food_G, DssRef.lang.Resource_TypeName_Food);
            //Resource(CostMultiply(city, Cost_Wood), ItemResourceType.Wood_Group, DssRef.lang.Resource_TypeName_Wood);
            //Resource(CostMultiply(city, Cost_Stone), ItemResourceType.Stone_G, DssRef.lang.Resource_TypeName_Stone);
            //Resource(CostMultiply(city, Cost_SkinAndLinnen), ItemResourceType.SkinLinen_Group, DssRef.lang.Resource_TypeName_Linen);
            //Resource(CostMultiply(city, Cost_Iron), ItemResourceType.Iron_G, DssRef.lang.Resource_TypeName_Iron);


        }

        public static void ResourceToHud(ItemResourceType item, LocalPlayer player, RichBoxContent content, City city)
        {
            switch (item)
            {
                case ItemResourceType.RawFood_Group:
                    Resource(CostMultiply(city, Cost_RawFood), ItemResourceType.RawFood_Group, DssRef.lang.Resource_TypeName_RawFood);
                    break;
                case ItemResourceType.Food_G:
                    Resource(CostMultiply(city, Cost_Food), ItemResourceType.Food_G, DssRef.lang.Resource_TypeName_Food);
                    break;
                case ItemResourceType.Wood_Group:
                    Resource(CostMultiply(city, Cost_Wood), ItemResourceType.Wood_Group, DssRef.lang.Resource_TypeName_Wood);
                    break;
                case ItemResourceType.Stone_G:
                    Resource(CostMultiply(city, Cost_Stone), ItemResourceType.Stone_G, DssRef.lang.Resource_TypeName_Stone);
                    break;
                case ItemResourceType.SkinLinen_Group:
                    Resource(CostMultiply(city, Cost_SkinAndLinnen), ItemResourceType.SkinLinen_Group, DssRef.lang.Resource_TypeName_Linen);
                    break;
                case ItemResourceType.Iron_G:
                    Resource(CostMultiply(city, Cost_Iron), ItemResourceType.Iron_G, DssRef.lang.Resource_TypeName_Iron);
                    break;
            }
            void Resource(int cost, ItemResourceType resourceType, string name)
            {
                int count = 1;
                int non = 0;

                content.newLine();

                content.Add(new RbImage(SpriteName.rtsUpkeep));
                content.Add(new RbText(cost.ToString()));
                content.space();

                RbButton button = new RbButton(new List<AbsRichBoxMember>
                    {
                        new RbImage(ResourceLib.Icon(resourceType)),
                        new RbText(name),
                    },
                new RbAction3Arg<ItemResourceType, int, int>(city.blackMarketPurchase, resourceType, count, cost, SoundLib.menuBuy),
                tooltip(count), player.faction.calcCost(cost, ref non, city));

                content.Add(button);
                content.space();

                foreach (var c in PurchaseCount)
                {
                    count = c;
                    RbButton xbutton = new RbButton(new List<AbsRichBoxMember>
                        {
                            new RbText(string.Format(DssRef.lang.Hud_XTimes, count)),
                        },
                    new RbAction3Arg<ItemResourceType, int, int>(city.blackMarketPurchase, resourceType, count, cost, SoundLib.menuBuy),
                    tooltip(count), player.faction.calcCost(cost * count, ref non, city));
                    content.Add(xbutton);
                    content.space();
                }



                AbsRbAction tooltip(int count)
                {
                    return new RbAction(() =>
                    {
                        RichBoxContent content = new RichBoxContent();
                        content.h2(DssRef.lang.Hud_PurchaseTitle_Cost).overrideColor = HudLib.TitleColor_Label;
                        content.newLine();
                        HudLib.ResourceCost(content, ResourceType.Gold, cost * count, player.faction.gold);

                        content.newParagraph();

                        content.h2(DssRef.lang.Hud_PurchaseTitle_CurrentlyOwn).overrideColor = HudLib.TitleColor_Label;
                        bool reachedBuffer = false;
                        bool safeGuard = city.foodSafeGuardIsActive(resourceType);
                        city.GetGroupedResource(resourceType).toMenu(content, resourceType, safeGuard, ref reachedBuffer);
                        //if (reachedBuffer)
                        //{
                        //    GroupedResource.BufferIconInfo(content);
                        //}
                        //content.text(name + " " + city.GetGroupedResource(resourceType).amount.ToString());

                        player.hud.tooltip.create(player, content, true);

                    });
                }
            }
        }


    }
}
