using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;

namespace VikingEngine.DSSWars.Resource
{
    struct ResourceInfoTag
    {
        public ResourceInfoTag(City city, ItemResourceType item)
        {
            this.city = city;
            this.item = item;
        }
        public City city;
        public ItemResourceType item;
    }

    static class ResourceLib
    {        

        public static void FullResourceInfo(RichBoxContent content, object tag)
        {
            ResourceInfoTag args = (ResourceInfoTag)tag;
            FullResourceInfo(args.city, args.item, content); 
        }

        public static void FullResourceInfo(City city, ItemResourceType item, RichBoxContent content)
        {
            var resources = city.GetGroupedResource(item);

            content.Add(new RbBeginTitle());

            content.Add(new RbImage(Icon(item)));
            content.space();
            content.Add(new RbText(TextLib.LargeFirstLetter(LangLib.Item(item)) + ": ", HudLib.TitleColor_TypeName));
            content.space();
            content.Add(new RbText(TextLib.LargeNumber(resources.amount)));

            //todo public ResourceOverview res_wood
            content.newLine();
            resources.changeRate.toMenu(content);

            SpriteName stockIcon;
            if (resources.amount >= resources.goalBuffer)
            {
                stockIcon = SpriteName.WarsStockpileStop;
            }
            else
            {
                stockIcon = SpriteName.WarsStockpileAdd;
            }
            content.newLine();
            
            content.Add(new RbText(DssRef.lang.Resource_StockpileLimit + ": ", HudLib.TitleColor_Label));
            content.space();
            content.Add(new RbImage(stockIcon));
            content.space();
            content.Add(new RbText(TextLib.LargeNumber(resources.goalBuffer)));

            var priority = city.workTemplate.GetWorkPriority(item, out bool hasPriority);
            if (hasPriority)
            {
                content.newLine();
                
                content.Add(new RbText(DssRef.lang.Work_OrderPrioTitle + ": ", HudLib.TitleColor_Label));
                content.space();
                content.Add(new RbImage(SpriteName.WarsHammer));
                content.space();
                content.Add(new RbText(priority.value.ToString(), priority.HasPrio() ? null : HudLib.NotAvailableColor));
            }

            ItemPropertyColl.Blueprint(item, out var bp1, out var bp2);

            bp(bp1);
            bp(bp2);

            //if (bp2 != null)
            //{

            //    content.newLine();
            //    bp2.toMenu(content, city, false, false);
            //}

            void bp(CraftBlueprint blueprint)
            {
                if (blueprint != null)
                {
                    content.Add(new RbSeperationLine());
                    content.Add(new RbBeginTitle());
                    content.Add(new RbImage(SpriteName.WarsBluePrint));
                    content.space();
                    content.Add(new RbText(DssRef.lang.Blueprint_Title, HudLib.TitleColor_Head2));
                    content.newLine();
                    blueprint.toMenu(content, city, false, false);
                }
            }
        }

        public static string Name(ResourceType resource)
        {
            switch (resource)
            {
                case ResourceType.Gold:
                    return DssRef.lang.ResourceType_Gold;

                case ResourceType.Worker:
                    return DssRef.lang.ResourceType_Workers;

                case ResourceType.DiplomaticPoint:
                    return DssRef.lang.ResourceType_DiplomacyPoints;

                case ResourceType.MercenaryOnMarket:
                    return DssRef.lang.Hud_MercenaryMarket;

                default:
                    return "Unknown resource";
            }
        }
        public static SpriteName PayIcon(ResourceType resource)
        {
            switch (resource)
            {
                case ResourceType.Gold:
                    return SpriteName.rtsUpkeep;

                case ResourceType.Worker:
                    return SpriteName.WarsWorker;

                case ResourceType.DiplomaticPoint:
                    return SpriteName.WarsDiplomaticSub;

                case ResourceType.MercenaryOnMarket:
                    return SpriteName.WarsGroupIcon;

                default:
                    return SpriteName.NO_IMAGE;
            }
        }



        public static SpriteName Icon(ItemResourceType resource)
        {
            switch (resource)
            {
                case ItemResourceType.Gold:
                    return SpriteName.rtsMoney;
                case ItemResourceType.Men:
                    return SpriteName.WarsWorker;
                case ItemResourceType.ServiceMen:
                    return SpriteName.WarsServiceMen;

                case ItemResourceType.Ballista:
                    return SpriteName.WarsResource_Ballista;
                case ItemResourceType.Beer:
                    return SpriteName.WarsResource_Beer;
                case ItemResourceType.CoolingFluid:
                    return SpriteName.WarsResource_CoolingFluid;
                case ItemResourceType.Bow:
                    return SpriteName.WarsResource_Bow;
                case ItemResourceType.Crossbow:
                    return SpriteName.WarsResource_Crossbow;
                case ItemResourceType.Egg:
                    return SpriteName.WarsResource_Egg;
                case ItemResourceType.Food_G:
                    return SpriteName.WarsResource_Food;
                case ItemResourceType.GoldOre:
                    return SpriteName.WarsResource_GoldOre;
                case ItemResourceType.HeavyIronArmor:
                    return SpriteName.WarsResource_HeavyIronArmor;
                case ItemResourceType.Iron_G:
                    return SpriteName.WarsResource_Iron;
                case ItemResourceType.BogIron:
                    return SpriteName.WarsBogIron;
                case ItemResourceType.IronOre_G:
                    return SpriteName.WarsResource_IronOre;
                case ItemResourceType.IronArmor:
                    return SpriteName.WarsResource_IronArmor;
                case ItemResourceType.PaddedArmor:
                    return SpriteName.WarsResource_PaddedArmor;
                case ItemResourceType.Linen:
                    return SpriteName.WarsResource_Linen;
                case ItemResourceType.LongBow:
                    return SpriteName.WarsResource_Longbow;
                case ItemResourceType.Hemp:
                    return SpriteName.WarsResource_Hemp;
                case ItemResourceType.Hen:
                case ItemResourceType.Pig:
                    return SpriteName.WarsResource_RawMeat;
                case ItemResourceType.Rapeseed:
                    return SpriteName.WarsResource_Rapeseed;
                case ItemResourceType.RawFood_Group:
                    return SpriteName.WarsResource_RawFood;
                case ItemResourceType.SharpStick:
                    return SpriteName.WarsResource_Sharpstick;
                case ItemResourceType.SkinLinen_Group:
                    return SpriteName.WarsResource_LinenCloth;
                case ItemResourceType.Stone_G:
                    return SpriteName.WarsResource_Stone;
                case ItemResourceType.Sword:
                    return SpriteName.WarsResource_Sword;
                case ItemResourceType.Water_G:
                    return SpriteName.WarsResource_Water;
                case ItemResourceType.Wheat:
                    return SpriteName.WarsResource_Wheat;
                case ItemResourceType.DryWood:
                case ItemResourceType.SoftWood:
                case ItemResourceType.HardWood:
                case ItemResourceType.Wood_Group:
                    return SpriteName.WarsResource_Wood;
                case ItemResourceType.Coal:
                case ItemResourceType.Fuel_G:
                    return SpriteName.WarsResource_Fuel;
                case ItemResourceType.TwoHandSword:
                    return SpriteName.WarsResource_TwoHandSword;
                case ItemResourceType.KnightsLance:
                    return SpriteName.WarsResource_KnightsLance;


                case ItemResourceType.Wagon2Wheel:
                    return SpriteName.WarsResource_Wagon2Wheel;
                case ItemResourceType.Wagon4Wheel:
                    return SpriteName.WarsResource_Wagon4Wheel;
                case ItemResourceType.Tin:
                    return SpriteName.WarsResource_Tin;
                case ItemResourceType.TinOre:
                    return SpriteName.WarsResource_TinOre;
                case ItemResourceType.Bronze:
                    return SpriteName.WarsResource_Bronze;
                case ItemResourceType.Copper:
                    return SpriteName.WarsResource_Copper;
                case ItemResourceType.CopperOre:
                    return SpriteName.WarsResource_CopperOre;
                case ItemResourceType.Silver:
                    return SpriteName.WarsResource_Silver;
                case ItemResourceType.SilverOre:
                    return SpriteName.WarsResource_SilverOre;
                case ItemResourceType.Mithril:
                    return SpriteName.WarsResource_MithrilAlloy;
                case ItemResourceType.RawMithril:
                    return SpriteName.WarsResource_Mithril;

                case ItemResourceType.BronzeSword:
                    return SpriteName.WarsResource_BronzeSword;
                case ItemResourceType.ShortSword:
                    return SpriteName.WarsResource_ShortSword;
                case ItemResourceType.LongSword:
                    return SpriteName.WarsResource_Longsword;
                case ItemResourceType.HandSpear:
                    return SpriteName.WarsResource_HandSpear;
                case ItemResourceType.Warhammer:
                    return SpriteName.WarsResource_Warhammer;
                case ItemResourceType.MithrilSword:
                    return SpriteName.WarsResource_MithrilSword;
                case ItemResourceType.SlingShot:
                    return SpriteName.WarsResource_Slingshot;
                case ItemResourceType.ThrowingSpear:
                    return SpriteName.WarsResource_ThrowSpear;
                case ItemResourceType.Pike:
                    return SpriteName.WarsResource_Pike;
                case ItemResourceType.MithrilBow:
                    return SpriteName.WarsResource_Mithrilbow;

                case ItemResourceType.Palisade:
                    return SpriteName.WarsResource_Palisade;
                case ItemResourceType.Toolkit:
                    return SpriteName.WarsResource_Toolkit;

                case ItemResourceType.Sulfur:
                    return SpriteName.WarsResource_Sulfur;
                case ItemResourceType.LeadOre:
                    return SpriteName.WarsResource_LeadOre;
                case ItemResourceType.Lead:
                    return SpriteName.WarsResource_Lead;
                case ItemResourceType.BloomeryIron:
                    return SpriteName.WarsResource_BloomeryIron;
                case ItemResourceType.Steel:
                    return SpriteName.WarsResource_Steel;
                case ItemResourceType.CastIron:
                    return SpriteName.WarsResource_CastIron;

                case ItemResourceType.BlackPowder:
                    return SpriteName.WarsResource_BlackPowder;
                case ItemResourceType.GunPowder:
                    return SpriteName.WarsResource_GunPowder;
                case ItemResourceType.LedBullet:
                    return SpriteName.WarsResource_Bullets;

                case ItemResourceType.HandCannon:
                    return SpriteName.WarsResource_BronzeRifle;
                case ItemResourceType.HandCulverin:
                    return SpriteName.WarsResource_BronzeShotgun;
                case ItemResourceType.Rifle:
                    return SpriteName.WarsResource_IronRifle;
                case ItemResourceType.Blunderbuss:
                    return SpriteName.WarsResource_IronShotgun;

                case ItemResourceType.Manuballista:
                    return SpriteName.WarsResource_Manuballista;
                case ItemResourceType.Catapult:
                    return SpriteName.WarsResource_Catapult;
                case ItemResourceType.SiegeCannonBronze:
                    return SpriteName.WarsResource_BronzeSiegeCannon;
                case ItemResourceType.ManCannonBronze:
                    return SpriteName.WarsResource_BronzeManCannon;
                case ItemResourceType.SiegeCannonIron:
                    return SpriteName.WarsResource_IronSiegeCannon;
                case ItemResourceType.ManCannonIron:
                    return SpriteName.WarsResource_IronManCannon;

                case ItemResourceType.HeavyPaddedArmor:
                    return SpriteName.WarsResource_HeavyPaddedArmor;

               
                case ItemResourceType.BronzeArmor:
                    return SpriteName.WarsResource_BronzeArmor;

                case ItemResourceType.LightPlateArmor:
                    return SpriteName.WarsResource_LightPlateArmor;
                case ItemResourceType.FullPlateArmor:
                    return SpriteName.WarsResource_FullPlateArmor;

                case ItemResourceType.MithrilArmor:
                    return SpriteName.WarsResource_MithrilArmor;
                case ItemResourceType.AutomatedItem:
                    return SpriteName.AutomationGearIcon;

                case ItemResourceType.CopperCoin:
                    return SpriteName.WarsResource_CopperCoin;
                case ItemResourceType.BronzeCoin:
                    return SpriteName.WarsResource_BonzeCoin;
                case ItemResourceType.SilverCoin:
                    return SpriteName.WarsResource_SilverCoin;
                case ItemResourceType.ElfCoin:
                    return SpriteName.WarsResource_ElfCoin;

                case ItemResourceType.NONE:
                    return SpriteName.BluePrintSquareFull;

                default:
                    return SpriteName.NO_IMAGE;
            }
        }

        
    }

    enum ResourceType
    {
        Gold,
        Worker,
        DiplomaticPoint,
        Item,
        MercenaryOnMarket,
        NUM
    }
}
