using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.EntityComponent;
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
        public ResourceInfoTag(Faction faction, City city, ItemResourceType item)
        {
            this.faction = faction;
            this.city = city;
            this.item = item;
        }
        public Faction faction;
        public City city;
        public ItemResourceType item;
    }

    static class ResourceLib
    {
        public static readonly ItemResourceType[] MovableCityResource_Misc =
        {
            ItemResourceType.Wood_Group,
            ItemResourceType.Fuel_G,
            ItemResourceType.Stone_G,
        ItemResourceType.Clay,
        ItemResourceType.Brick,

            ItemResourceType.SkinLinen_Group,
            ItemResourceType.RawFood_Group,
        ItemResourceType.Salt,

            ItemResourceType.Food_G,
        ItemResourceType.ConservedFood,
            ItemResourceType.Beer,
            ItemResourceType.CoolingFluid,
            ItemResourceType.Container,
            ItemResourceType.Palisade,
            ItemResourceType.Toolkit,
            ItemResourceType.Wagon2Wheel,
            ItemResourceType.Wagon4Wheel,
            ItemResourceType.WagonClosed,
            ItemResourceType.WagonIron,
            ItemResourceType.WagonSteel,
            ItemResourceType.BlackPowder,
            ItemResourceType.GunPowder,
            ItemResourceType.LedBullet,
        };
        public static readonly ItemResourceType[] MovableCityResource_Metals =
       {
             ItemResourceType.IronOre_G,
             ItemResourceType.TinOre,
             ItemResourceType.CopperOre,
             ItemResourceType.LeadOre,
             ItemResourceType.SilverOre,
             ItemResourceType.GoldOre,

             ItemResourceType.Iron_G,
             ItemResourceType.Tin,
            ItemResourceType.Copper,
            ItemResourceType.Lead,
            ItemResourceType.Silver,
            ItemResourceType.RawMithril,
            ItemResourceType.Sulfur,

            ItemResourceType.Bronze,
            ItemResourceType.CastIron,
            ItemResourceType.BloomeryIron,
            ItemResourceType.Steel,
            ItemResourceType.Mithril,
        };
        public static readonly ItemResourceType[] MovableCityResource_WeaponMelee =
       {
            ItemResourceType.SharpStick,
            ItemResourceType.BronzeSword,
            ItemResourceType.ShortSword,
            ItemResourceType.Sword,
            ItemResourceType.LongSword,
            ItemResourceType.HandSpear,

            ItemResourceType.Warhammer,
            ItemResourceType.TwoHandSword,
             //ItemResourceType.KnightsLance,
            ItemResourceType.MithrilSword,


        };

        public static readonly ItemResourceType[] MovableCityResource_WeaponRanged =
         {
            ItemResourceType.SlingShot,
             ItemResourceType.ThrowingSpear,

             ItemResourceType.Bow,
             ItemResourceType.LongBow,
            ItemResourceType.Crossbow,
                ItemResourceType.MithrilBow,

            ItemResourceType.HandCannon,
            ItemResourceType.HandCulverin,
            ItemResourceType.Rifle,
            ItemResourceType.Blunderbuss,

            ItemResourceType.Ballista,
            ItemResourceType.Manuballista,
            ItemResourceType.Catapult,
            ItemResourceType.SiegeCannonBronze,
            ItemResourceType.ManCannonBronze,
            ItemResourceType.SiegeCannonIron,
            ItemResourceType.ManCannonIron,

        };

        public static readonly ItemResourceType[] MovableCityResource_Armor =
         {
             ItemResourceType.BronzeArmor,
             ItemResourceType.PaddedArmor,
             ItemResourceType.HeavyPaddedArmor,
             ItemResourceType.IronArmor,
             ItemResourceType.HeavyIronArmor,
             ItemResourceType.LightPlateArmor,
             ItemResourceType.FullPlateArmor,

             ItemResourceType.MithrilArmor,

            ItemResourceType.MountBronzeArmor,
            ItemResourceType.MountPaddedArmor,
            ItemResourceType.MountHeavyPaddedArmor,
            ItemResourceType.MountIronArmor,
            ItemResourceType.MountHeavyIronArmor,
            ItemResourceType.MountLightPlateArmor,
            ItemResourceType.MountFullPlateArmor,
            ItemResourceType.MountMithrilArmor,
        };

        public static readonly ItemResourceType[] MovableCityResource_Animals =
         {
            ItemResourceType.Hen,
            ItemResourceType.Pig,

            ItemResourceType.Dog,
            ItemResourceType.Hound,

            ItemResourceType.Oxen,
            ItemResourceType.KineOxen,

            ItemResourceType.Pony,
            ItemResourceType.Horse,
            ItemResourceType.WarHorse,
            ItemResourceType.DraftHorse,

            ItemResourceType.WildPig,
            ItemResourceType.WildHog,
            ItemResourceType.WarHog,
            ItemResourceType.StagHog,

            ItemResourceType.Wolf,
            ItemResourceType.Warg,
            ItemResourceType.AlphaWarg,

            ItemResourceType.WildCat,
            ItemResourceType.Lion,
            ItemResourceType.WarLion,

            ItemResourceType.Elephant,
            ItemResourceType.WarElephant,
            ItemResourceType.Oliphant,
        };

        public static ItemResourceType[] ResourceGroupList(ResourceGroupType group)
        {
            switch (group)
            {
                default: return MovableCityResource_Misc;
                case ResourceGroupType.Metals: return MovableCityResource_Metals; 
                case ResourceGroupType.Animals: return MovableCityResource_Animals; 
                case ResourceGroupType.Weapons: return MovableCityResource_WeaponMelee;
                case ResourceGroupType.Projectile: return MovableCityResource_WeaponRanged;
                case ResourceGroupType.Armor: return MovableCityResource_Armor;
            }
        }
        public static void ResourceIconCountDisplay(City city, ItemResourceType item, RichBoxContent content)
        {
            EntityComponent.GroupedResource resources = city.GetGroupedResource(item);
            IconName.Item(item, out SpriteName itemIcon, out string itemName);
            content.Add(new RbImage(itemIcon));
            content.space();
            content.Add(new RbText(TextLib.LargeFirstLetter(itemName) + ": ", HudLib.TitleColor_TypeName));
            content.space();
            content.Add(new RbText(TextLib.LargeNumber(resources.amount)));
        }

        public static void FullResourceInfo(RichBoxContent content, object tag)
        {
            ResourceInfoTag args = (ResourceInfoTag)tag;
            FullResourceInfo(args.faction, args.city, args.item, content); 
        }

        public static void FullResourceInfo(Faction faction, City city, ItemResourceType item, RichBoxContent content)
        {
            if (item == ItemResourceType.NONE)
            {
                content.Add(new RbText(DssRef.lang.Hud_None));
                return;
            }

            EntityComponent.GroupedResource resources = city != null? city.GetGroupedResource(item) : faction.GetRefResourceOverview(item);
            IconName.Item(item, out SpriteName itemIcon, out string itemName);
            var properties = ItemPropertyColl.Get(item);

            content.Add(new RbBeginTitle());

            content.Add(new RbImage(itemIcon));
            content.space();
            content.Add(new RbText(TextLib.LargeFirstLetter(itemName) + ": ", HudLib.TitleColor_TypeName));
            content.space();
            content.Add(new RbText(TextLib.LargeNumber(resources.amount)));

            //todo public ResourceOverview res_wood
            content.newLine();
            resources.changeRate.toMenu(content);

            if (properties.storageType != StorageType.NUM_NONE)
            {
                SpriteName stockIcon;
                if (resources.amount >= resources.stockPileLimit)
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
                content.Add(new RbText(TextLib.LargeNumber(resources.stockPileLimit)));


                content.newLine();
                IconName.Storage(properties.storageType, out var storeIcon, out var storeText);
                content.Add(new RbImage(storeIcon));
                content.space();
                content.Add(new RbText(storeText, HudLib.SecondaryTextColor));
            }
            bool hasPriority;
            Work.WorkPriority priority = city != null? city.workTemplate.GetWorkPriority(item, out hasPriority) : faction.workTemplate.GetWorkPriority(item, out hasPriority);
            if (hasPriority)
            {
                content.newLine();
                
                content.Add(new RbText(DssRef.lang.Work_OrderPrioTitle + ": ", HudLib.TitleColor_Label));
                content.space();
               
                content.space();
                IconName.Priority(priority.value, out SpriteName prioIcon, out _);
                content.Add(new RbOverlapImage(new RbImage(SpriteName.WarsHammer), prioIcon, new Vector2(0.4f, 0.1f), 0.75f));
                content.space(1.5f);
                content.Add(new RbText(priority.value.ToString(), priority.HasPrio() ? null : HudLib.NotAvailableColor));
            }

            content.Add(new RbSeperationLine());

            content.h1(DssRef.lang.ItemSource, HudLib.TitleColor_Head2);
            content.newLine();
            properties.ItemSourceToHud(content);

            bp(properties.bp1);
            bp(properties.bp2);

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

            if (resources.hasCesspit)
            {
                content.Add(new RbSeperationLine());
                content.Add(new RbBeginTitle());
                content.Add(new RbImage(SpriteName.WarsBuild_Cesspit));
                content.space();
                content.Add(new RbText(DssRef.todoLang.BuildingType_Cesspit, HudLib.TitleColor_Head2));
                content.text(".Todo info here");
            }

            if (properties.restrictedToBiom != Data.CityBiome.NUM_NONE)
            {
                content.newLine();/*.Add(new RbSeperationLine());*/
                content.Add(new RbText(TextLib.LabelColon( ".Item production restricted to"), HudLib.TitleColor_Label));
                content.space();
                content.Add(new RbText($"{DssRef.todoLang.CityBiome_Title} - {LangLib.Biome(properties.restrictedToBiom)}"));
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

        public static int Limit(StockpileLimitOption limitOption)
        {
            switch (limitOption)
            {
                case StockpileLimitOption.NoLimit:
                    return int.MaxValue;

                case StockpileLimitOption.Zero: return 0;
                //case StockpileLimitOption.Value100: return 100;
                case StockpileLimitOption.Value200: return 200;
                case StockpileLimitOption.Value4000: return 4000;

                default: return -1;
            }
        }



        //public static SpriteName Icon(ItemResourceType resource)
        //{
        //    switch (resource)
        //    {
        //        case ItemResourceType.Gold:
        //            return SpriteName.rtsMoney;
        //        case ItemResourceType.Men:
        //            return SpriteName.WarsWorker;
        //        case ItemResourceType.ServiceMen:
        //            return SpriteName.WarsServiceMen;

        //        case ItemResourceType.Ballista:
        //            return SpriteName.WarsResource_Ballista;
        //        case ItemResourceType.Beer:
        //            return SpriteName.WarsResource_Beer;
        //        case ItemResourceType.CoolingFluid:
        //            return SpriteName.WarsResource_CoolingFluid;
        //        case ItemResourceType.Bow:
        //            return SpriteName.WarsResource_Bow;
        //        case ItemResourceType.Crossbow:
        //            return SpriteName.WarsResource_Crossbow;
        //        case ItemResourceType.Egg:
        //            return SpriteName.WarsResource_Egg;
        //        case ItemResourceType.Food_G:
        //            return SpriteName.WarsResource_Food;
        //        case ItemResourceType.GoldOre:
        //            return SpriteName.WarsResource_GoldOre;
        //        case ItemResourceType.HeavyIronArmor:
        //            return SpriteName.WarsResource_HeavyIronArmor;
        //        case ItemResourceType.Iron_G:
        //            return SpriteName.WarsResource_Iron;
        //        case ItemResourceType.BogIron:
        //            return SpriteName.WarsBogIron;
        //        case ItemResourceType.IronOre_G:
        //            return SpriteName.WarsResource_IronOre;
        //        case ItemResourceType.IronArmor:
        //            return SpriteName.WarsResource_IronArmor;
        //        case ItemResourceType.PaddedArmor:
        //            return SpriteName.WarsResource_PaddedArmor;
        //        case ItemResourceType.Linen:
        //            return SpriteName.WarsResource_Linen;
        //        case ItemResourceType.LongBow:
        //            return SpriteName.WarsResource_Longbow;
        //        case ItemResourceType.Hemp:
        //            return SpriteName.WarsResource_Hemp;
        //        case ItemResourceType.Hen:
        //        case ItemResourceType.Pig:
        //            return SpriteName.WarsResource_RawMeat;
        //        case ItemResourceType.Rapeseed:
        //            return SpriteName.WarsResource_Rapeseed;
        //        case ItemResourceType.RawFood_Group:
        //            return SpriteName.WarsResource_RawFood;
        //        case ItemResourceType.SharpStick:
        //            return SpriteName.WarsResource_Sharpstick;
        //        case ItemResourceType.SkinLinen_Group:
        //            return SpriteName.WarsResource_LinenCloth;
        //        case ItemResourceType.Stone_G:
        //            return SpriteName.WarsResource_Stone;
        //        case ItemResourceType.Sword:
        //            return SpriteName.WarsResource_Sword;
        //        case ItemResourceType.Water_G:
        //            return SpriteName.WarsResource_Water;
        //        case ItemResourceType.Wheat:
        //            return SpriteName.WarsResource_Wheat;
        //        case ItemResourceType.DryWood:
        //        case ItemResourceType.SoftWood:
        //        case ItemResourceType.HardWood:
        //        case ItemResourceType.Wood_Group:
        //            return SpriteName.WarsResource_Wood;
        //        case ItemResourceType.Coal:
        //        case ItemResourceType.Fuel_G:
        //            return SpriteName.WarsResource_Fuel;
        //        case ItemResourceType.TwoHandSword:
        //            return SpriteName.WarsResource_TwoHandSword;
        //        case ItemResourceType.KnightsLance:
        //            return SpriteName.WarsResource_KnightsLance;


        //        case ItemResourceType.Wagon2Wheel:
        //            return SpriteName.WarsResource_Wagon2Wheel;
        //        case ItemResourceType.Wagon4Wheel:
        //            return SpriteName.WarsResource_Wagon4Wheel;
        //        case ItemResourceType.Tin:
        //            return SpriteName.WarsResource_Tin;
        //        case ItemResourceType.TinOre:
        //            return SpriteName.WarsResource_TinOre;
        //        case ItemResourceType.Bronze:
        //            return SpriteName.WarsResource_Bronze;
        //        case ItemResourceType.Copper:
        //            return SpriteName.WarsResource_Copper;
        //        case ItemResourceType.CopperOre:
        //            return SpriteName.WarsResource_CopperOre;
        //        case ItemResourceType.Silver:
        //            return SpriteName.WarsResource_Silver;
        //        case ItemResourceType.SilverOre:
        //            return SpriteName.WarsResource_SilverOre;
        //        case ItemResourceType.Mithril:
        //            return SpriteName.WarsResource_MithrilAlloy;
        //        case ItemResourceType.RawMithril:
        //            return SpriteName.WarsResource_Mithril;

        //        case ItemResourceType.BronzeSword:
        //            return SpriteName.WarsResource_BronzeSword;
        //        case ItemResourceType.ShortSword:
        //            return SpriteName.WarsResource_ShortSword;
        //        case ItemResourceType.LongSword:
        //            return SpriteName.WarsResource_Longsword;
        //        case ItemResourceType.HandSpear:
        //            return SpriteName.WarsResource_HandSpear;
        //        case ItemResourceType.Warhammer:
        //            return SpriteName.WarsResource_Warhammer;
        //        case ItemResourceType.MithrilSword:
        //            return SpriteName.WarsResource_MithrilSword;
        //        case ItemResourceType.SlingShot:
        //            return SpriteName.WarsResource_Slingshot;
        //        case ItemResourceType.ThrowingSpear:
        //            return SpriteName.WarsResource_ThrowSpear;
        //        case ItemResourceType.Pike:
        //            return SpriteName.WarsResource_Pike;
        //        case ItemResourceType.MithrilBow:
        //            return SpriteName.WarsResource_Mithrilbow;

        //        case ItemResourceType.Palisade:
        //            return SpriteName.WarsResource_Palisade;
        //        case ItemResourceType.Toolkit:
        //            return SpriteName.WarsResource_Toolkit;

        //        case ItemResourceType.Sulfur:
        //            return SpriteName.WarsResource_Sulfur;
        //        case ItemResourceType.LeadOre:
        //            return SpriteName.WarsResource_LeadOre;
        //        case ItemResourceType.Lead:
        //            return SpriteName.WarsResource_Lead;
        //        case ItemResourceType.BloomeryIron:
        //            return SpriteName.WarsResource_BloomeryIron;
        //        case ItemResourceType.Steel:
        //            return SpriteName.WarsResource_Steel;
        //        case ItemResourceType.CastIron:
        //            return SpriteName.WarsResource_CastIron;

        //        case ItemResourceType.BlackPowder:
        //            return SpriteName.WarsResource_BlackPowder;
        //        case ItemResourceType.GunPowder:
        //            return SpriteName.WarsResource_GunPowder;
        //        case ItemResourceType.LedBullet:
        //            return SpriteName.WarsResource_Bullets;

        //        case ItemResourceType.HandCannon:
        //            return SpriteName.WarsResource_BronzeRifle;
        //        case ItemResourceType.HandCulverin:
        //            return SpriteName.WarsResource_BronzeShotgun;
        //        case ItemResourceType.Rifle:
        //            return SpriteName.WarsResource_IronRifle;
        //        case ItemResourceType.Blunderbuss:
        //            return SpriteName.WarsResource_IronShotgun;

        //        case ItemResourceType.Manuballista:
        //            return SpriteName.WarsResource_Manuballista;
        //        case ItemResourceType.Catapult:
        //            return SpriteName.WarsResource_Catapult;
        //        case ItemResourceType.SiegeCannonBronze:
        //            return SpriteName.WarsResource_BronzeSiegeCannon;
        //        case ItemResourceType.ManCannonBronze:
        //            return SpriteName.WarsResource_BronzeManCannon;
        //        case ItemResourceType.SiegeCannonIron:
        //            return SpriteName.WarsResource_IronSiegeCannon;
        //        case ItemResourceType.ManCannonIron:
        //            return SpriteName.WarsResource_IronManCannon;

        //        case ItemResourceType.HeavyPaddedArmor:
        //            return SpriteName.WarsResource_HeavyPaddedArmor;


        //        case ItemResourceType.BronzeArmor:
        //            return SpriteName.WarsResource_BronzeArmor;

        //        case ItemResourceType.LightPlateArmor:
        //            return SpriteName.WarsResource_LightPlateArmor;
        //        case ItemResourceType.FullPlateArmor:
        //            return SpriteName.WarsResource_FullPlateArmor;

        //        case ItemResourceType.MithrilArmor:
        //            return SpriteName.WarsResource_MithrilArmor;
        //        case ItemResourceType.AutomatedItem:
        //            return SpriteName.AutomationGearIcon;

        //        case ItemResourceType.CopperCoin:
        //            return SpriteName.WarsResource_CopperCoin;
        //        case ItemResourceType.BronzeCoin:
        //            return SpriteName.WarsResource_BonzeCoin;
        //        case ItemResourceType.SilverCoin:
        //            return SpriteName.WarsResource_SilverCoin;
        //        case ItemResourceType.ElfCoin:
        //            return SpriteName.WarsResource_ElfCoin;

        //        case ItemResourceType.NONE:
        //            return SpriteName.BluePrintSquareFull;

        //        default:
        //            return SpriteName.NO_IMAGE;
        //    }
        //}


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

    enum ResourceGroupType
    {
        Resources,
        Metals,
        Weapons,
        Projectile,
        Armor,
        Animals,
        Mint,
        NUM,
        Auto,
    }

    enum ResourceManagementType
    {
        Overview,
        Work,
        Stockpile,
        Auto,
    }
}
