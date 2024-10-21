using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    static class BlackMarketResources
    {
        static readonly int[] PurchaseCount = { 20, 100, 500 };

        static int Cost_RawFood = DssConst.FoodGoldValue_BlackMarket - 4;
        static int Cost_Food = DssConst.FoodGoldValue_BlackMarket;
        static int Cost_Wood = 50;
        static int Cost_Stone = 30;
        static int Cost_SkinAndLinnen = 50;
        static int Cost_Iron = 500;

        public static void AiPurchaseWood(City city, Faction faction)
        {
            int count = 5;
            if (faction.payMoney(count * Cost_Wood, false))
            {
                city.res_wood.amount += count;
            }
        }
        public static bool AiPurchaseIron(City city, Faction faction)
        {
            int count = ResourceLib.CraftSmith_IronUse;
            if (faction.payMoney(count * Cost_Iron, false))
            {
                city.res_iron.amount += count;
                return true;
            }
            return false;
        }

        public static void ToHud(LocalPlayer player, RichBoxContent content, City city)
        {
            content.h2(DssRef.lang.Hud_PurchaseTitle_Resources).overrideColor = HudLib.TitleColor_Label;

            Resource(Cost_RawFood, ItemResourceType.RawFood_Group, DssRef.lang.Resource_TypeName_RawFood);
            Resource(Cost_Food, ItemResourceType.Food_G, DssRef.lang.Resource_TypeName_Food);
            Resource(Cost_Wood, ItemResourceType.Wood_Group, DssRef.lang.Resource_TypeName_Wood);
            Resource(Cost_Stone, ItemResourceType.Stone_G, DssRef.lang.Resource_TypeName_Stone);
            Resource(Cost_SkinAndLinnen, ItemResourceType.SkinLinen_Group, DssRef.lang.Resource_TypeName_Linen);
            Resource(Cost_Iron, ItemResourceType.Iron_G, DssRef.lang.Resource_TypeName_Iron);

            void Resource(int cost, ItemResourceType resourceType, string name)
            {
                int count = 1;
                int non =0;

                content.newLine();

                content.Add(new RichBoxImage(SpriteName.rtsUpkeep));
                content.Add(new RichBoxText(cost.ToString()));
                content.space();

                RichboxButton button = new RichboxButton(new List<AbsRichBoxMember>
                {
                    new RichBoxImage(ResourceLib.Icon(resourceType)),
                    new RichBoxText(name),
                },
                new RbAction3Arg<ItemResourceType, int, int>(city.blackMarketPurchase, resourceType, count, cost, SoundLib.menuBuy),
                tooltip(count), player.faction.calcCost(cost, ref non));

                content.Add(button);
                content.space();

                foreach (var c in PurchaseCount)
                {
                    count = c;
                    RichboxButton xbutton = new RichboxButton(new List<AbsRichBoxMember>
                    {   
                        new RichBoxText(string.Format(DssRef.lang.Hud_XTimes, count)),
                    },
                    new RbAction3Arg<ItemResourceType, int, int>(city.blackMarketPurchase, resourceType, count, cost, SoundLib.menuBuy),
                    tooltip(count), player.faction.calcCost(cost * count, ref non));
                    content.Add(xbutton);
                    content.space();
                }



                AbsRbAction tooltip(int count)
                {
                    return new RbAction(() =>
                    {
                        RichBoxContent content = new RichBoxContent();
                        content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
                        content.newLine();
                        HudLib.ResourceCost(content, ResourceType.Gold, cost * count, player.faction.gold);
                        
                        content.newParagraph();

                        content.h2(DssRef.lang.Hud_PurchaseTitle_CurrentlyOwn);
                        bool reachedBuffer = false;
                        city.GetGroupedResource(resourceType).toMenu(content, resourceType, ref reachedBuffer);
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
