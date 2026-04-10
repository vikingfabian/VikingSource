using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.PJ;
using VikingEngine.PJ.GameState;
using VikingEngine.ToGG;
using VikingEngine.ToGG.HeroQuest.GO;

namespace VikingEngine.DSSWars
{
    static class IconName
    {
        public static void SpecializationTypeName(SpecializationType specialization, out SpriteName icon, out string name)
        {
            switch (specialization)
            {
                case SpecializationType.None:
                    icon = SpriteName.BluePrintSquareFull;
                    name = DssRef.lang.Hud_None;
                    break;
                case SpecializationType.Field:
                    icon = SpriteName.WarsSpecializeField;
                    name = DssRef.lang.Conscript_Specialization_Field;
                    break;
                case SpecializationType.Sea:
                    icon = SpriteName.WarsSpecializeSea;
                    name = DssRef.lang.Conscript_Specialization_Sea;
                    break;
                case SpecializationType.Siege:
                    icon = SpriteName.WarsSpecializeSiege;
                    name = DssRef.lang.Conscript_Specialization_Siege;
                    break;
                case SpecializationType.Viking:
                    icon = SpriteName.WarsUnitIcon_Viking;
                    name = DssRef.lang.UnitType_Viking;
                    break;
                case SpecializationType.HonorGuard:
                    icon = SpriteName.WarsUnitIcon_Honorguard;
                    name = DssRef.lang.UnitType_HonorGuard;
                    break;
                case SpecializationType.Green:
                    icon = SpriteName.WarsUnitIcon_Greensoldier;
                    name = DssRef.lang.UnitType_GreenSoldier;
                    break;
                case SpecializationType.Traditional:
                    icon = SpriteName.WarsSpecializeTradition;
                    name = DssRef.lang.Conscript_Specialization_Traditional;
                    break;
                case SpecializationType.AntiCavalry:

                    icon = SpriteName.WarsSpecializeAntiCavalry;
                    name = DssRef.lang.Conscript_Specialization_AntiCavalry;
                    break;
                case SpecializationType.CityGuard:

                    icon = SpriteName.WarsGuard;
                    name = DssRef.lang.Conscript_Soldiers_GuardType;
                    break;

                default:
                    icon = SpriteName.NO_IMAGE;
                    name = TextLib.Error;
                    break;
            }

        }
        public static void Storage(StorageType storage, out SpriteName storeIcon, out string storeText)
        {
            switch (storage)
            {
                default:
                case StorageType.MaterialStorage:
                    storeIcon = SpriteName.WarsBuild_MaterialStorage;
                    storeText = DssRef.todoLang.BuildingType_MaterialStorage;
                    break;
                case StorageType.FoodStorage:
                    storeIcon = SpriteName.WarsBuild_FoodStorage;
                    storeText = DssRef.todoLang.BuildingType_FoodStorage;
                    break;
                case StorageType.WeaponStorage:
                    storeIcon = SpriteName.WarsBuild_WeaponStorage;
                    storeText = DssRef.todoLang.BuildingType_WeaponStorage;
                    break;
                case StorageType.ArmorStorage:
                    storeIcon = SpriteName.WarsBuild_ArmorStorage;
                    storeText = DssRef.todoLang.BuildingType_ArmorStorage;
                    break;
                case StorageType.AnimalStorage:
                    storeIcon = SpriteName.WarsBuild_AnimalStorage;
                    storeText = DssRef.todoLang.BuildingType_AnimalStorage;
                    break;

            }
        }

        public static void Priority(int priority, out SpriteName prioIcon, out string prioText)
        {
            switch (priority)
            {
                case WorkTemplate.NoPrio:
                    prioIcon = SpriteName.WarsHudIconSpeed_Pause;
                    prioText = DssRef.lang.Work_OrderPrio_No;
                    break;

                case WorkTemplate.MinPrio:
                    prioIcon = SpriteName.WarsHudIconSpeed_Low;
                    prioText = DssRef.lang.Work_OrderPrio_Min;
                    break;

                default:
                    prioIcon = SpriteName.WarsHudIconSpeed_Medium;
                    prioText = null;
                    break;

                case WorkTemplate.MaxPrio:
                    prioIcon = SpriteName.WarsHudIconSpeed_High;
                    prioText = DssRef.lang.Work_OrderPrio_Max;
                    break;
            }
        }

        //new
        public static void Item(ItemResourceType item, out SpriteName itemIcon, out string itemName)
        {
            switch (item)
            {
                // --- Economy & Workers ---
                case ItemResourceType.Gold:
                    itemIcon = SpriteName.rtsMoney;
                    itemName = DssRef.lang.ResourceType_Gold;
                    break;
                case ItemResourceType.Men:
                    itemIcon = SpriteName.WarsWorker;
                    itemName = DssRef.lang.ResourceType_Workers;
                    break;
                case ItemResourceType.ServiceMen:
                    itemIcon = SpriteName.WarsServiceMen;
                    itemName = DssRef.lang.ResourceType_ServiceMen;
                    break;
                case ItemResourceType.NobelMen:
                    itemIcon = SpriteName.WarsNobelman;
                    itemName = DssRef.todoLang.Resource_TypeName_NobelMen;
                    break;
                case ItemResourceType.Settler:
                    itemIcon = SpriteName.WarsSettler; // Defaulting to worker icon
                    itemName = DssRef.lang.UnitType_Settler;
                    break;

                // --- Resources & Materials ---
                case ItemResourceType.Water_G:
                    itemIcon = SpriteName.WarsResource_Water;
                    itemName = DssRef.lang.Resource_TypeName_Water;
                    break;
                case ItemResourceType.Beer:
                    itemIcon = SpriteName.WarsResource_Beer;
                    itemName = DssRef.lang.Resource_TypeName_Beer;
                    break;
                case ItemResourceType.CoolingFluid:
                    itemIcon = SpriteName.WarsResource_CoolingFluid;
                    itemName = DssRef.lang.Resource_TypeName_CoolingFluid;
                    break;
                case ItemResourceType.Stone_G:
                    itemIcon = SpriteName.WarsResource_Stone;
                    itemName = DssRef.lang.Resource_TypeName_Stone;
                    break;
                case ItemResourceType.DryWood:
                case ItemResourceType.SoftWood:
                case ItemResourceType.HardWood:
                case ItemResourceType.Wood_Group:
                    itemIcon = SpriteName.WarsResource_Wood;
                    itemName = DssRef.lang.Resource_TypeName_Wood;
                    break;
                case ItemResourceType.Coal:
                case ItemResourceType.Fuel_G:
                    itemIcon = SpriteName.WarsResource_Fuel;
                    itemName = DssRef.lang.Resource_TypeName_Fuel;
                    break;
                case ItemResourceType.Sulfur:
                    itemIcon = SpriteName.WarsResource_Sulfur;
                    itemName = DssRef.lang.Resource_TypeName_Sulfur;
                    break;

                case ItemResourceType.Salt:
                    itemIcon = SpriteName.WarsResource_Salt;
                    itemName = DssRef.todoLang.Resource_TypeName_Salt;
                    break;
                case ItemResourceType.Clay:
                    itemIcon = SpriteName.WarsResource_Clay;
                    itemName = DssRef.todoLang.Resource_TypeName_Clay;
                    break;
                case ItemResourceType.Brick:
                    itemIcon = SpriteName.WarsResource_Brick;
                    itemName = DssRef.todoLang.Resource_TypeName_Brick;
                    break;

                // --- Food & Agriculture ---
                case ItemResourceType.Food_G:
                    itemIcon = SpriteName.WarsResource_Food;
                    itemName = DssRef.lang.Resource_TypeName_Food;
                    break;

                case ItemResourceType.ConservedFood:
                    itemIcon = SpriteName.WarsResource_ConservedFood;
                    itemName = DssRef.todoLang.Resource_TypeName_ConservedFood;
                    break;

                case ItemResourceType.Egg:
                case ItemResourceType.Wheat:
                case ItemResourceType.RawFood_Group:
                    itemIcon = SpriteName.WarsResource_RawFood;
                    itemName = DssRef.lang.Resource_TypeName_RawFood;
                    break;

                case ItemResourceType.Rapeseed:
                    itemIcon = SpriteName.WarsResource_Rapeseed;
                    itemName = DssRef.lang.Resource_TypeName_Rapeseed;
                    break;
                case ItemResourceType.Hemp:
                    itemIcon = SpriteName.WarsResource_Hemp;
                    itemName = DssRef.lang.Resource_TypeName_Hemp;
                    break;
                case ItemResourceType.Linen:
                case ItemResourceType.SkinLinen_Group:
                    itemIcon = SpriteName.WarsResource_LinenCloth;
                    itemName = DssRef.lang.Resource_TypeName_Linen;
                    break;
                // --- Animals ---
                case ItemResourceType.SlaughterFowl:
                case ItemResourceType.Fowl:
                    itemIcon = SpriteName.WarsResource_Fowl;
                    itemName = DssRef.todoLang.Resource_TypeName_Fowl;
                    break;

                case ItemResourceType.SlaughterHen:
                case ItemResourceType.Hen:
                    itemIcon = SpriteName.WarsResource_Hen;
                    itemName = DssRef.todoLang.Resource_TypeName_Hen;
                    break;

                case ItemResourceType.SlaughterBoar:
                case ItemResourceType.Boar:
                    itemIcon = SpriteName.WarsResource_Boar;
                    itemName = DssRef.todoLang.Resource_TypeName_Boar;
                    break;

                case ItemResourceType.SlaughterPig:
                case ItemResourceType.Pig:
                    itemIcon = SpriteName.WarsResource_Pig;
                    itemName = DssRef.todoLang.Resource_TypeName_Pig;
                    break;
                case ItemResourceType.SlaughterOxen:
                case ItemResourceType.Oxen:
                    itemIcon = SpriteName.WarsResource_Oxen;
                    itemName = DssRef.todoLang.Resource_TypeName_Oxen;
                    break;
                case ItemResourceType.SlaughterKineOxen:
                case ItemResourceType.KineOxen:
                    itemIcon = SpriteName.WarsResource_KineOxen;
                    itemName = DssRef.todoLang.Resource_TypeName_KineOxen;
                    break;

                case ItemResourceType.Dog:
                    itemIcon = SpriteName.WarsResource_Dog;
                    itemName = DssRef.todoLang.Resource_TypeName_Dog;
                    break;
                case ItemResourceType.Hound:
                    itemIcon = SpriteName.WarsResource_Hound;
                    itemName = DssRef.todoLang.Resource_TypeName_Hound;
                    break;

                // --- Horses ---
                case ItemResourceType.SlaughterPony:
                case ItemResourceType.Pony:
                    itemIcon = SpriteName.WarsResource_Pony;
                    itemName = DssRef.todoLang.Resource_TypeName_Pony;
                    break;
                case ItemResourceType.SlaughterHorse:
                case ItemResourceType.Horse:
                    itemIcon = SpriteName.WarsResource_Horse;
                    itemName = DssRef.todoLang.Resource_TypeName_Horse;
                    break;
                case ItemResourceType.SlaughterWarHorse:
                case ItemResourceType.WarHorse:
                    itemIcon = SpriteName.WarsResource_WarHorse;
                    itemName = DssRef.todoLang.Resource_TypeName_WarHorse;
                    break;
                case ItemResourceType.SlaughterDraftHorse:
                case ItemResourceType.DraftHorse:
                    itemIcon = SpriteName.WarsResource_DraftHorse;
                    itemName = DssRef.todoLang.Resource_TypeName_DraftHorse;
                    break;

                // --- Wild Pigs / Hogs ---
                case ItemResourceType.SlaughterWildPig:
                case ItemResourceType.WildPig:
                    itemIcon = SpriteName.WarsResource_WildPig;
                    itemName = DssRef.todoLang.Resource_TypeName_WildPig;
                    break;
                case ItemResourceType.SlaughterWildHog:
                case ItemResourceType.WildHog:
                    itemIcon = SpriteName.WarsResource_WildHog;
                    itemName = DssRef.todoLang.Resource_TypeName_WildHog;
                    break;
                case ItemResourceType.SlaughterWarHog:
                case ItemResourceType.WarHog:
                    itemIcon = SpriteName.WarsResource_WarHog;
                    itemName = DssRef.todoLang.Resource_TypeName_WarHog;
                    break;
                case ItemResourceType.SlaughterStagHog:
                case ItemResourceType.StagHog:
                    itemIcon = SpriteName.WarsResource_StagHog;
                    itemName = DssRef.todoLang.Resource_TypeName_StagHog;
                    break;

                // --- Wolves ---
                case ItemResourceType.Wolf:
                    itemIcon = SpriteName.WarsResource_Wolf;
                    itemName = DssRef.todoLang.Resource_TypeName_Wolf;
                    break;
                case ItemResourceType.Warg:
                    itemIcon = SpriteName.WarsResource_Warg;
                    itemName = DssRef.todoLang.Resource_TypeName_Warg;
                    break;
                case ItemResourceType.AlphaWarg:
                    itemIcon = SpriteName.WarsResource_AlphaWarg;
                    itemName = DssRef.todoLang.Resource_TypeName_AlphaWarg;
                    break;

                // --- Cats ---
                case ItemResourceType.WildCat:
                    itemIcon = SpriteName.WarsResource_WildCat;
                    itemName = DssRef.todoLang.Resource_TypeName_WildCat;
                    break;
                case ItemResourceType.Lion:
                    itemIcon = SpriteName.WarsResource_Lion;
                    itemName = DssRef.todoLang.Resource_TypeName_Lion;
                    break;
                case ItemResourceType.WarLion:
                    itemIcon = SpriteName.WarsResource_WarLion;
                    itemName = DssRef.todoLang.Resource_TypeName_WarLion;
                    break;

                // --- Elephants ---
                case ItemResourceType.SlaughterElephant:
                case ItemResourceType.Elephant:
                    itemIcon = SpriteName.WarsResource_Elephant;
                    itemName = DssRef.todoLang.Resource_TypeName_Elephant;
                    break;
                case ItemResourceType.SlaughterWarElephant:
                case ItemResourceType.WarElephant:
                    itemIcon = SpriteName.WarsResource_WarElephant;
                    itemName = DssRef.todoLang.Resource_TypeName_WarElephant;
                    break;
                case ItemResourceType.SlaughterOliphant:
                case ItemResourceType.Oliphant:
                    itemIcon = SpriteName.WarsResource_Oliphant;
                    itemName = DssRef.todoLang.Resource_TypeName_Oliphant;
                    break;


                // --- Ores & Metals ---
                case ItemResourceType.GoldOre:
                    itemIcon = SpriteName.WarsResource_GoldOre;
                    itemName = DssRef.lang.Resource_TypeName_GoldOre;
                    break;
                case ItemResourceType.IronOre_G:
                    itemIcon = SpriteName.WarsResource_IronOre;
                    itemName = DssRef.lang.Resource_TypeName_IronOre;
                    break;
                case ItemResourceType.BogIron:
                    itemIcon = SpriteName.WarsBogIron;
                    itemName = DssRef.lang.Resource_TypeName_BogIron;
                    break;
                case ItemResourceType.TinOre:
                    itemIcon = SpriteName.WarsResource_TinOre;
                    itemName = DssRef.lang.Resource_TypeName_TinOre;
                    break;
                case ItemResourceType.CopperOre:
                    itemIcon = SpriteName.WarsResource_CopperOre;
                    itemName = DssRef.lang.Resource_TypeName_CopperOre;
                    break;
                case ItemResourceType.LeadOre:
                    itemIcon = SpriteName.WarsResource_LeadOre;
                    itemName = DssRef.lang.Resource_TypeName_LeadOre;
                    break;
                case ItemResourceType.SilverOre:
                    itemIcon = SpriteName.WarsResource_SilverOre;
                    itemName = DssRef.lang.Resource_TypeName_SilverOre;
                    break;

                // --- Refined Metals ---
                case ItemResourceType.Iron_G:
                    itemIcon = SpriteName.WarsResource_Iron;
                    itemName = DssRef.lang.Resource_TypeName_Iron;
                    break;
                case ItemResourceType.BloomeryIron:
                    itemIcon = SpriteName.WarsResource_BloomeryIron;
                    itemName = DssRef.lang.Resource_TypeName_BloomIron;
                    break;
                case ItemResourceType.CastIron:
                    itemIcon = SpriteName.WarsResource_CastIron;
                    itemName = DssRef.lang.Resource_TypeName_CastIron;
                    break;
                case ItemResourceType.Steel:
                    itemIcon = SpriteName.WarsResource_Steel;
                    itemName = DssRef.lang.Resource_TypeName_Steel;
                    break;
                case ItemResourceType.Tin:
                    itemIcon = SpriteName.WarsResource_Tin;
                    itemName = DssRef.lang.Resource_TypeName_Tin;
                    break;
                case ItemResourceType.Copper:
                    itemIcon = SpriteName.WarsResource_Copper;
                    itemName = DssRef.lang.Resource_TypeName_Copper;
                    break;
                case ItemResourceType.Bronze:
                    itemIcon = SpriteName.WarsResource_Bronze;
                    itemName = DssRef.lang.Resource_TypeName_Bronze;
                    break;
                case ItemResourceType.Silver:
                    itemIcon = SpriteName.WarsResource_Silver;
                    itemName = DssRef.lang.Resource_TypeName_Silver;
                    break;
                case ItemResourceType.Lead:
                    itemIcon = SpriteName.WarsResource_Lead;
                    itemName = DssRef.lang.Resource_TypeName_Lead;
                    break;
                case ItemResourceType.RawMithril:
                    itemIcon = SpriteName.WarsResource_Mithril;
                    itemName = DssRef.lang.Resource_TypeName_RawMithril;
                    break;
                case ItemResourceType.Mithril:
                    itemIcon = SpriteName.WarsResource_MithrilAlloy;
                    itemName = DssRef.lang.Resource_TypeName_Mithril;
                    break;

                // --- Weapons (Melee) ---
                case ItemResourceType.SharpStick:
                    itemIcon = SpriteName.WarsResource_Sharpstick;
                    itemName = DssRef.lang.Resource_TypeName_SharpStick;
                    break;
                case ItemResourceType.Sword:
                    itemIcon = SpriteName.WarsResource_Sword;
                    itemName = DssRef.lang.Resource_TypeName_Sword;
                    break;
                case ItemResourceType.BronzeSword:
                    itemIcon = SpriteName.WarsResource_BronzeSword;
                    itemName = DssRef.lang.Resource_TypeName_BronzeSword;
                    break;
                case ItemResourceType.ShortSword:
                    itemIcon = SpriteName.WarsResource_ShortSword;
                    itemName = DssRef.lang.Resource_TypeName_ShortSword;
                    break;
                case ItemResourceType.LongSword:
                    itemIcon = SpriteName.WarsResource_Longsword;
                    itemName = DssRef.lang.Resource_TypeName_LongSword;
                    break;
                case ItemResourceType.TwoHandSword:
                    itemIcon = SpriteName.WarsResource_TwoHandSword;
                    itemName = DssRef.lang.Resource_TypeName_TwoHandSword;
                    break;
                case ItemResourceType.MithrilSword:
                    itemIcon = SpriteName.WarsResource_MithrilSword;
                    itemName = DssRef.lang.Resource_TypeName_MithrilSword;
                    break;
                case ItemResourceType.HandSpear:
                    itemIcon = SpriteName.WarsResource_HandSpear;
                    itemName = DssRef.lang.Resource_TypeName_HandSpear;
                    break;
                case ItemResourceType.Pike:
                    itemIcon = SpriteName.WarsResource_Pike; // Assumed SpriteName
                    itemName = DssRef.lang.Resource_TypeName_Pike;
                    break;
                case ItemResourceType.Warhammer:
                    itemIcon = SpriteName.WarsResource_Warhammer;
                    itemName = DssRef.lang.Resource_TypeName_Warhammer;
                    break;

                // --- Shields ---
                case ItemResourceType.BucklerShield:
                    itemIcon = SpriteName.WarsResource_BucklerShield;
                    itemName = DssRef.todoLang.Resource_TypeName_BucklerShield;
                    break;
                case ItemResourceType.RoundShield:
                    itemIcon = SpriteName.WarsResource_RoundShield;
                    itemName = DssRef.todoLang.Resource_TypeName_RoundShield;
                    break;
                case ItemResourceType.HeaterShield:
                    itemIcon = SpriteName.WarsResource_HeaterShield;
                    itemName = DssRef.todoLang.Resource_TypeName_HeaterShield;
                    break;
                case ItemResourceType.TowerShield:
                    itemIcon = SpriteName.WarsResource_TowerShield;
                    itemName = DssRef.todoLang.Resource_TypeName_TowerShield;
                    break;

                //case ItemResourceType.KnightsLance:
                //    itemIcon = SpriteName.WarsResource_KnightsLance;
                //    itemName = DssRef.lang.Resource_TypeName_KnightsLance;
                //    break;

                // --- Weapons (Ranged) ---
                case ItemResourceType.SlingShot:
                    itemIcon = SpriteName.WarsResource_Slingshot;
                    itemName = DssRef.lang.Resource_TypeName_SlingShot;
                    break;
                case ItemResourceType.ThrowingSpear:
                    itemIcon = SpriteName.WarsResource_ThrowSpear;
                    itemName = DssRef.lang.Resource_TypeName_ThrowingSpear;
                    break;
                case ItemResourceType.Bow:
                    itemIcon = SpriteName.WarsResource_Bow;
                    itemName = DssRef.lang.Resource_TypeName_Bow;
                    break;
                case ItemResourceType.LongBow:
                    itemIcon = SpriteName.WarsResource_Longbow;
                    itemName = DssRef.lang.Resource_TypeName_Longbow;
                    break;
                case ItemResourceType.Crossbow:
                    itemIcon = SpriteName.WarsResource_Crossbow;
                    itemName = DssRef.lang.Resource_TypeName_Crossbow;
                    break;
                case ItemResourceType.MithrilBow:
                    itemIcon = SpriteName.WarsResource_Mithrilbow;
                    itemName = DssRef.lang.Resource_TypeName_MithrilBow;
                    break;

                // --- Siege & Components ---
                case ItemResourceType.Ballista:
                    itemIcon = SpriteName.WarsResource_Ballista;
                    itemName = DssRef.lang.UnitType_Ballista;
                    break;
                case ItemResourceType.Manuballista:
                    itemIcon = SpriteName.WarsResource_Manuballista;
                    itemName = DssRef.lang.Resource_TypeName_Manuballista;
                    break;
                case ItemResourceType.Catapult:
                    itemIcon = SpriteName.WarsResource_Catapult;
                    itemName = DssRef.lang.Resource_TypeName_Catapult;
                    break;
                case ItemResourceType.SiegeCannonBronze:
                    itemIcon = SpriteName.WarsResource_BronzeSiegeCannon;
                    itemName = DssRef.lang.Resource_TypeName_SiegeCannonBronze;
                    break;
                case ItemResourceType.ManCannonBronze:
                    itemIcon = SpriteName.WarsResource_BronzeManCannon;
                    itemName = DssRef.lang.Resource_TypeName_ManCannonBronze;
                    break;
                case ItemResourceType.SiegeCannonIron:
                    itemIcon = SpriteName.WarsResource_IronSiegeCannon;
                    itemName = DssRef.lang.Resource_TypeName_SiegeCannonIron;
                    break;
                case ItemResourceType.ManCannonIron:
                    itemIcon = SpriteName.WarsResource_IronManCannon;
                    itemName = DssRef.lang.Resource_TypeName_ManCannonIron;
                    break;
                case ItemResourceType.Palisade:
                    itemIcon = SpriteName.WarsResource_Palisade;
                    itemName = DssRef.lang.Resource_TypeName_Palisade;
                    break;
                case ItemResourceType.Toolkit:
                    itemIcon = SpriteName.WarsResource_Toolkit;
                    itemName = DssRef.lang.Resource_TypeName_Toolkit;
                    break;
                case ItemResourceType.WoodContainer:
                case ItemResourceType.PotContainer:
                case ItemResourceType.Container:
                    itemIcon = SpriteName.WarsResource_Container;
                    itemName = DssRef.todoLang.Resource_TypeName_Container;
                    break;
                case ItemResourceType.Wagon2Wheel:
                    itemIcon = SpriteName.WarsResource_Wagon2Wheel;
                    itemName = DssRef.lang.Resource_TypeName_Wagon2Wheel;
                    break;
                case ItemResourceType.Wagon4Wheel:
                    itemIcon = SpriteName.WarsResource_Wagon4Wheel;
                    itemName = DssRef.lang.Resource_TypeName_Wagon4Wheel;
                    break;
                case ItemResourceType.WagonClosed:
                    itemIcon = SpriteName.WarsResource_WagonClosed;
                    itemName = DssRef.todoLang.Resource_TypeName_WagonClosed;
                    break;
                case ItemResourceType.WagonIron:
                    itemIcon = SpriteName.WarsResource_WagonIron;
                    itemName = DssRef.todoLang.Resource_TypeName_WagonIron;
                    break;
                case ItemResourceType.WagonSteel:
                    itemIcon = SpriteName.WarsResource_WagonSteel;
                    itemName = DssRef.todoLang.Resource_TypeName_WagonSteel;
                    break;

                // --- Firearms & Explosives ---
                case ItemResourceType.BlackPowder:
                    itemIcon = SpriteName.WarsResource_BlackPowder;
                    itemName = DssRef.lang.Resource_TypeName_BlackPowder;
                    break;
                case ItemResourceType.GunPowder:
                    itemIcon = SpriteName.WarsResource_GunPowder;
                    itemName = DssRef.lang.Resource_TypeName_GunPowder;
                    break;
                case ItemResourceType.LedBullet:
                    itemIcon = SpriteName.WarsResource_Bullets;
                    itemName = DssRef.lang.Resource_TypeName_LedBullet;
                    break;
                case ItemResourceType.HandCannon:
                    itemIcon = SpriteName.WarsResource_BronzeRifle;
                    itemName = DssRef.lang.Resource_TypeName_HandCannon;
                    break;
                case ItemResourceType.HandCulverin:
                    itemIcon = SpriteName.WarsResource_BronzeShotgun;
                    itemName = DssRef.lang.Resource_TypeName_HandCulverin;
                    break;
                case ItemResourceType.Rifle:
                    itemIcon = SpriteName.WarsResource_IronRifle;
                    itemName = DssRef.lang.Resource_TypeName_Rifle;
                    break;
                case ItemResourceType.Blunderbuss:
                    itemIcon = SpriteName.WarsResource_IronShotgun;
                    itemName = DssRef.lang.Resource_TypeName_Blunderbuss;
                    break;

                // --- Armor ---
                case ItemResourceType.PaddedArmor:
                    itemIcon = SpriteName.WarsResource_PaddedArmor;
                    itemName = DssRef.lang.Resource_TypeName_PaddedArmor;
                    break;
                case ItemResourceType.HeavyPaddedArmor:
                    itemIcon = SpriteName.WarsResource_HeavyPaddedArmor;
                    itemName = DssRef.lang.Resource_TypeName_HeavyPaddedArmor;
                    break;
                case ItemResourceType.BronzeArmor:
                    itemIcon = SpriteName.WarsResource_BronzeArmor;
                    itemName = DssRef.lang.Resource_TypeName_BronzeArmor;
                    break;
                case ItemResourceType.IronArmor:
                    itemIcon = SpriteName.WarsResource_IronArmor;
                    itemName = DssRef.lang.Resource_TypeName_IronArmor;
                    break;
                case ItemResourceType.HeavyIronArmor:
                    itemIcon = SpriteName.WarsResource_HeavyIronArmor;
                    itemName = DssRef.lang.Resource_TypeName_HeavyIronArmor;
                    break;
                case ItemResourceType.LightPlateArmor:
                    itemIcon = SpriteName.WarsResource_LightPlateArmor;
                    itemName = DssRef.lang.Resource_TypeName_LightPlateArmor;
                    break;
                case ItemResourceType.FullPlateArmor:
                    itemIcon = SpriteName.WarsResource_FullPlateArmor;
                    itemName = DssRef.lang.Resource_TypeName_FullPlateArmor;
                    break;
                case ItemResourceType.MithrilArmor:
                    itemIcon = SpriteName.WarsResource_MithrilArmor;
                    itemName = DssRef.lang.Resource_TypeName_MithrilArmor;
                    break;

                case ItemResourceType.MountPaddedArmor:
                    itemIcon = SpriteName.WarsResource_MountPaddedArmor;
                    itemName = string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_PaddedArmor.ToLower());
                    break;
                case ItemResourceType.MountHeavyPaddedArmor:
                    itemIcon = SpriteName.WarsResource_MountHeavyPaddedArmor;
                    itemName = string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_HeavyPaddedArmor.ToLower());
                    break;
                case ItemResourceType.MountBronzeArmor:
                    itemIcon = SpriteName.WarsResource_MountBronzeArmor;
                    itemName = string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_BronzeArmor.ToLower());
                    break;
                case ItemResourceType.MountIronArmor:
                    itemIcon = SpriteName.WarsResource_MountIronArmor;
                    itemName = string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_IronArmor.ToLower());
                    break;
                case ItemResourceType.MountHeavyIronArmor:
                    itemIcon = SpriteName.WarsResource_MountHeavyIronArmor;
                    itemName = string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_HeavyIronArmor.ToLower());
                    break;
                case ItemResourceType.MountLightPlateArmor:
                    itemIcon = SpriteName.WarsResource_MountLightPlateArmor;
                    itemName = string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_LightPlateArmor.ToLower());
                    break;
                case ItemResourceType.MountFullPlateArmor:
                    itemIcon = SpriteName.WarsResource_MountFullPlateArmor;
                    itemName = string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_FullPlateArmor.ToLower());
                    break;
                case ItemResourceType.MountMithrilArmor:
                    itemIcon = SpriteName.WarsResource_MountMithrilArmor;
                    itemName = string.Format(DssRef.todoLang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_MithrilArmor.ToLower());
                    break;

                // --- Currency ---
                case ItemResourceType.CopperCoin:
                    itemIcon = SpriteName.WarsResource_CopperCoin;
                    itemName = DssRef.lang.Resource_TypeName_Coin;
                    break;
                case ItemResourceType.BronzeCoin:
                    itemIcon = SpriteName.WarsResource_BonzeCoin;
                    itemName = DssRef.lang.Resource_TypeName_Coin;
                    break;
                case ItemResourceType.SilverCoin:
                    itemIcon = SpriteName.WarsResource_SilverCoin;
                    itemName = DssRef.lang.Resource_TypeName_Coin;
                    break;
                case ItemResourceType.ElfCoin:
                    itemIcon = SpriteName.WarsResource_ElfCoin;
                    itemName = DssRef.lang.Resource_TypeName_Coin;
                    break;

                // --- Misc ---
                case ItemResourceType.AutomatedItem:
                    itemIcon = SpriteName.AutomationGearIcon;
                    itemName = TextLib.Error; // No name provided in original
                    break;

                case ItemResourceType.NONE:
                    itemIcon = SpriteName.BluePrintSquareFull;
                    itemName = DssRef.lang.Hud_None;
                    break;

                default:
                    itemIcon = SpriteName.NO_IMAGE;
                    itemName = TextLib.Error;
                    break;
            }
        }

        public static void CityCulture(Data.CityCulture cityCulture, out string title, out string description)
        {
            switch (cityCulture)
            {
                case Data.CityCulture.Archers:
                    title = DssRef.lang.CityCulture_Archers;
                    description = DssRef.lang.CityCulture_Archers_Description;
                    break;

                case Data.CityCulture.Builders:
                    title = DssRef.lang.CityCulture_Builders;
                    description = DssRef.lang.CityCulture_Builders_Description;
                    break;

                case Data.CityCulture.CrabMentality:
                    title = DssRef.lang.CityCulture_CrabMentality;
                    description = DssRef.lang.CityCulture_CrabMentality_Description;
                    break;

                case Data.CityCulture.DeepWell:
                    title = DssRef.lang.CityCulture_DeepWell;
                    description = DssRef.lang.CityCulture_DeepWell_Description;
                    break;

                case Data.CityCulture.FertileGround:
                    title = DssRef.lang.CityCulture_FertileGround;
                    description = DssRef.lang.CityCulture_FertileGround_Description;
                    break;

                case Data.CityCulture.LargeFamilies:
                    title = DssRef.lang.CityCulture_LargeFamilies;
                    description = DssRef.lang.CityCulture_LargeFamilies_Description;
                    break;

                case Data.CityCulture.Miners:
                    title = DssRef.lang.CityCulture_Miners;
                    description = DssRef.lang.CityCulture_Miners_Description;
                    break;

                case Data.CityCulture.Warriors:
                    title = DssRef.lang.CityCulture_Warriors;
                    description = DssRef.lang.CityCulture_Warriors_Description;
                    break;

                case Data.CityCulture.Woodcutters:
                    title = DssRef.lang.CityCulture_Woodcutters;
                    description = DssRef.lang.CityCulture_Woodcutters_Description;
                    break;

                case Data.CityCulture.Networker:
                    title = DssRef.lang.CityCulture_Networker;
                    description = DssRef.lang.CityCulture_Networker_Description;
                    break;

                case Data.CityCulture.PitMasters:
                    title = DssRef.lang.CityCulture_PitMasters;
                    description = DssRef.lang.CityCulture_PitMasters_Description;
                    break;

                case Data.CityCulture.Stonemason:
                    title = DssRef.lang.CityCulture_Stonemason;
                    description = DssRef.lang.CityCulture_Stonemason_Description;
                    break;

                case Data.CityCulture.Brewmaster:
                    title = DssRef.lang.CityCulture_Brewmaster;
                    description = DssRef.lang.CityCulture_Brewmaster_Description;
                    break;

                case Data.CityCulture.Weavers:
                    title = DssRef.lang.CityCulture_Weavers;
                    description = DssRef.lang.CityCulture_Weavers_Description;
                    break;

                case Data.CityCulture.SiegeEngineer:
                    title = DssRef.lang.CityCulture_SiegeEngineer;
                    description = DssRef.lang.CityCulture_SiegeEngineer_Description;
                    break;

                case Data.CityCulture.Armorsmith:
                    title = DssRef.lang.CityCulture_Armorsmith;
                    description = DssRef.lang.CityCulture_Armorsmith_Description;
                    break;

                case Data.CityCulture.Noblemen:
                    title = DssRef.lang.CityCulture_Noblemen;
                    description = DssRef.lang.CityCulture_Noblemen_Description;
                    break;

                case Data.CityCulture.Seafaring:
                    title = DssRef.lang.CityCulture_Seafaring;
                    description = DssRef.lang.CityCulture_Seafaring_Description;
                    break;

                case Data.CityCulture.Backtrader:
                    title = DssRef.lang.CityCulture_Backtrader;
                    description = DssRef.lang.CityCulture_Backtrader_Description;
                    break;

                case Data.CityCulture.Lawbiding:
                    title = DssRef.lang.CityCulture_LawAbiding;
                    description = DssRef.lang.CityCulture_LawAbiding_Description;
                    break;

                case Data.CityCulture.Smelters:
                    title = DssRef.lang.CityCulture_Smelters;
                    description = DssRef.lang.CityCulture_Smelters_Description;
                    break;

                case Data.CityCulture.BronzeCasters:
                    title = DssRef.lang.CityCulture_BronzeCasters;
                    description = DssRef.lang.CityCulture_BronzeCasters_Description;
                    break;

                case Data.CityCulture.Apprentices:
                    title = DssRef.lang.CityCulture_Apprentices;
                    description = DssRef.lang.CityCulture_Apprentices_Description;
                    break;

                case Data.CityCulture.AnimalBreeder2:
                    title = DssRef.lang.CityCulture_AnimalBreeder;
                    description = DssRef.todoLang.CityCulture_AnimalBreeder2_Description;
                    break;

                case Data.CityCulture.Butchers:
                    title = DssRef.todoLang.CityCulture_Butchers;
                    description = string.Format( DssRef.todoLang.CityCulture_EnhancedProduction, DssRef.todoLang.Resource_TypeName_Meat);
                    break;
               

                case Data.CityCulture.Potters:
                    title = DssRef.todoLang.CityCulture_Potters;
                    description = string.Format(DssRef.todoLang.CityCulture_EnhancedProduction, DssRef.todoLang.CityCulture_Potters);
                    break;

                case Data.CityCulture.Wainwright:
                    title = DssRef.todoLang.CityCulture_Wainwright;
                    description = string.Format(DssRef.todoLang.CityCulture_EnhancedProduction, DssRef.todoLang.Resource_TypeName_Vehicle);
                    break;

                case Data.CityCulture.Wheelwright:
                    title = DssRef.todoLang.CityCulture_Wheelwright;
                    description = DssRef.todoLang.CityCulture_Wheelwright_Description;
                    break;

                case Data.CityCulture.ShieldMaker:
                    title = DssRef.todoLang.CityCulture_ShieldMaker;
                    description = string.Format(DssRef.todoLang.CityCulture_EnhancedProduction, DssRef.todoLang.Resource_TypeName_Shield);
                    break;

                case Data.CityCulture.Coopers:
                    title = DssRef.todoLang.CityCulture_Coopers;
                    description = string.Format(DssRef.todoLang.CityCulture_EnhancedProduction, DssRef.todoLang.Resource_TypeName_Container);
                    break;

                case Data.CityCulture.Salters:
                    title = DssRef.todoLang.CityCulture_Salters;
                    description = string.Format(DssRef.todoLang.CityCulture_EnhancedProduction, DssRef.todoLang.Resource_TypeName_ConservedFood);
                    break;

                case Data.CityCulture.Nomads:
                    title = DssRef.lang.CityCulture_Nomad;
                    description = string.Format(DssRef.lang.Hud_Purchase_LowXCost, DssRef.lang.UnitType_Settler);
                    break;

                default:
                    title = TextLib.Error;
                    description = TextLib.Error;
                    break;
            }
        }

        public static void BuildCategory(BuildCategoryTab tab, out SpriteName tabIcon, out string category)
        {
            //string category;
            //SpriteName tabIcon;
            switch (tab)
            {
                case BuildCategoryTab.Filter:
                    tabIcon = SpriteName.warsBuildCategorySearch;
                    category = DssRef.lang.HUD_Filter;
                    break;
                case BuildCategoryTab.General:
                    tabIcon = SpriteName.warsBuildCategoryHouse;
                    category = DssRef.lang.BuildCategory_General;
                    break;
                case BuildCategoryTab.Farming:
                    tabIcon = SpriteName.warsBuildCategoryFarm;
                    category = DssRef.todoLang.BuildCategory_Farming;
                    break;
                case BuildCategoryTab.Advanced:
                    tabIcon = SpriteName.warsBuildCategoryAdvanced;
                    category = DssRef.lang.Hud_Advanced;
                    break;
                case BuildCategoryTab.Military:
                    tabIcon = SpriteName.warsBuildCategoryMilitaryWall;
                    category = DssRef.lang.BuildCategory_Military;
                    break;
                case BuildCategoryTab.Decor:
                    tabIcon = SpriteName.warsBuildCategoryDecorTree;
                    category = DssRef.lang.BuildCategory_Decoration;
                    break;
                case BuildCategoryTab.Upgrade:
                    tabIcon = SpriteName.warsBuildCategoryUpgrades;
                    category = DssRef.lang.BuildCategory_Upgrade;
                    break;
                case BuildCategoryTab.GodPower:
                    tabIcon = SpriteName.WarsGodPowerIcon;
                    category = DssRef.lang.GodPower;
                    break;
                default:
                    tabIcon = SpriteName.warsBuildCategoryAutomation;
                    category = DssRef.lang.Automation_Title;
                    break;
            }
        }

        //New, previously called "category"
        public static void Tab(ResourceManagementType management, out SpriteName managementIcon, out string managementName)
        {
            switch (management)
            {
                case ResourceManagementType.Overview:
                    managementIcon = SpriteName.MenuPixelIconManual;
                    managementName = DssRef.lang.Resource_Tab_Overview;
                    break;

                case ResourceManagementType.Stockpile:
                    managementIcon = SpriteName.WarsStockpileAdd;
                    managementName = DssRef.lang.Resource_Tab_Stockpile;
                    break;

                case ResourceManagementType.Work:
                    managementIcon = SpriteName.WarsHammer;
                    managementName = DssRef.lang.MenuTab_Work;
                    break;

                //case ResourceManagementType.Auto:
                //    categoryIcon = SpriteName.MissingImage; // Assumed icon for Auto
                //    category = ".Auto"; // Placeholder or DssRef.lang.Auto
                //    break;

                default:
                    managementIcon = SpriteName.MissingImage;
                    managementName = TextLib.Error;
                    break;
            }
        }

        //New, previously called "tab"
        public static void Tab(ResourceGroupType group, out SpriteName groupIcon, out string groupName)
        {
            switch (group)
            {
                case ResourceGroupType.Resources:
                    groupIcon = SpriteName.WarsResource_Wood;
                    groupName = DssRef.lang.WarsResourceGroup_Resources;
                    break;
                                   
                case ResourceGroupType.Metals:
                    groupIcon = SpriteName.WarsResource_Iron;
                    groupName = DssRef.lang.WarsResourceGroup_Metal;
                    break;

                case ResourceGroupType.Weapons:
                    groupIcon = SpriteName.WarsResource_Sword;
                    groupName = DssRef.lang.WarsResourceGroup_MeleeHandWeapons;
                    break;

                case ResourceGroupType.Projectile:
                    groupIcon = SpriteName.WarsResource_Bow;
                    groupName = DssRef.lang.WarsResourceGroup_RangedHandWeapons;
                    break;

                case ResourceGroupType.Armor:
                    groupIcon = SpriteName.WarsResource_IronArmor;
                    groupName = DssRef.lang.Conscript_ArmorTitle;
                    break;

                case ResourceGroupType.Mint:
                    groupIcon = SpriteName.WarsResource_SilverCoin;
                    groupName = DssRef.lang.BuildingType_CoinMaker;
                    break;

                case ResourceGroupType.Animals:
                    groupIcon = SpriteName.WarsResource_Horse;
                    groupName = TextLib.LargeFirstLetter(DssRef.todoLang.Resource_TypeName_Animal); // Replace with DssRef.lang.Mounts if available
                    break;

                default:
                    groupIcon = SpriteName.MissingImage;
                    groupName = TextLib.Error;
                    break;
            }
        }

        ////Old, "ResourcesSubTab" is replaced by two new enums
        //public static void Tab(ResourcesSubTab tab, out SpriteName categoryIcon, out string category, out SpriteName tabIcon, out string tabName)
        //{
        //    switch (tab)
        //    {
        //        case ResourcesSubTab.Overview_Resources:
        //            categoryIcon = SpriteName.MenuPixelIconManual;
        //            tabIcon = SpriteName.WarsResource_Wood;
        //            category = DssRef.lang.Resource_Tab_Overview;
        //            tabName = DssRef.lang.WarsResourceGroup_Resources;
        //            break;
        //        case ResourcesSubTab.Overview_Metals:
        //            categoryIcon = SpriteName.MenuPixelIconManual;
        //            tabIcon = SpriteName.WarsResource_Iron;
        //            category = DssRef.lang.Resource_Tab_Overview;
        //            tabName = DssRef.lang.WarsResourceGroup_Metal;
        //            break;
        //        case ResourcesSubTab.Overview_Weapons:
        //            categoryIcon = SpriteName.MenuPixelIconManual;
        //            tabIcon = SpriteName.WarsResource_Sword;
        //            category = DssRef.lang.Resource_Tab_Overview;
        //            tabName = DssRef.lang.WarsResourceGroup_MeleeHandWeapons;
        //            break;
        //        case ResourcesSubTab.Overview_Projectile:
        //            categoryIcon = SpriteName.MenuPixelIconManual;
        //            tabIcon = SpriteName.WarsResource_Bow;
        //            category = DssRef.lang.Resource_Tab_Overview;
        //            tabName = DssRef.lang.WarsResourceGroup_RangedHandWeapons;
        //            break;
        //        case ResourcesSubTab.Overview_Armor:
        //            categoryIcon = SpriteName.WarsStockpileAdd;
        //            tabIcon = SpriteName.WarsResource_IronArmor;
        //            category = DssRef.lang.Resource_Tab_Overview;
        //            tabName = DssRef.lang.Conscript_ArmorTitle;
        //            break;

        //        case ResourcesSubTab.Stockpile_Resources:
        //            categoryIcon = SpriteName.WarsStockpileAdd;
        //            categoryIcon = SpriteName.MenuPixelIconManual;
        //            tabIcon = SpriteName.WarsResource_Wood;
        //            category = DssRef.lang.Resource_Tab_Stockpile;
        //            tabName = DssRef.lang.WarsResourceGroup_Resources;
        //            break;
        //        case ResourcesSubTab.Stockpile_Metals:
        //            categoryIcon = SpriteName.WarsStockpileAdd;
        //            tabIcon = SpriteName.WarsResource_Iron;
        //            category = DssRef.lang.Resource_Tab_Stockpile;
        //            tabName = DssRef.lang.WarsResourceGroup_Metal;
        //            break;
        //        case ResourcesSubTab.Stockpile_Weapons:
        //            categoryIcon = SpriteName.WarsStockpileAdd;
        //            tabIcon = SpriteName.WarsResource_Sword;
        //            category = DssRef.lang.Resource_Tab_Stockpile;
        //            tabName = DssRef.lang.WarsResourceGroup_MeleeHandWeapons;
        //            break;
        //        case ResourcesSubTab.Stockpile_Projectile:
        //            categoryIcon = SpriteName.WarsStockpileAdd;
        //            tabIcon = SpriteName.WarsResource_Bow;
        //            category = DssRef.lang.Resource_Tab_Stockpile;
        //            tabName = DssRef.lang.WarsResourceGroup_RangedHandWeapons;
        //            break;
        //        case ResourcesSubTab.Stockpile_Armor:
        //            categoryIcon = SpriteName.WarsStockpileAdd;
        //            tabIcon = SpriteName.WarsResource_IronArmor;
        //            category = DssRef.lang.Resource_Tab_Stockpile;
        //            tabName = DssRef.lang.Conscript_ArmorTitle;
        //            break;

        //        case ResourcesSubTab.Work_Resources:
        //            categoryIcon = SpriteName.WarsHammer;
        //            tabIcon = SpriteName.WarsResource_Wood;
        //            category = DssRef.lang.MenuTab_Work;
        //            tabName = DssRef.lang.WarsResourceGroup_Resources;
        //            break;
        //        case ResourcesSubTab.Work_Metals:
        //            categoryIcon = SpriteName.WarsHammer;
        //            tabIcon = SpriteName.WarsResource_Iron;
        //            category = DssRef.lang.MenuTab_Work;
        //            tabName = DssRef.lang.WarsResourceGroup_Metal;
        //            break;
        //        case ResourcesSubTab.Work_Weapons:
        //            categoryIcon = SpriteName.WarsHammer;
        //            tabIcon = SpriteName.WarsResource_Sword;
        //            category = DssRef.lang.MenuTab_Work;
        //            tabName = DssRef.lang.WarsResourceGroup_MeleeHandWeapons;
        //            break;
        //        case ResourcesSubTab.Work_Projectile:
        //            categoryIcon = SpriteName.WarsHammer;
        //            tabIcon = SpriteName.WarsResource_Bow;
        //            category = DssRef.lang.MenuTab_Work;
        //            tabName = DssRef.lang.WarsResourceGroup_RangedHandWeapons;
        //            break;
        //        case ResourcesSubTab.Work_Armor:
        //            categoryIcon = SpriteName.WarsHammer;
        //            tabIcon = SpriteName.WarsResource_IronArmor;
        //            category = DssRef.lang.MenuTab_Work;
        //            tabName = DssRef.lang.Conscript_ArmorTitle;
        //            break;

        //        case ResourcesSubTab.Work_Mint:
        //            categoryIcon = SpriteName.WarsHammer;
        //            tabIcon = SpriteName.WarsResource_SilverCoin;
        //            category = DssRef.lang.MenuTab_Work;
        //            tabName = DssRef.lang.BuildingType_CoinMaker;
        //            break;

        //        default:
        //            categoryIcon = SpriteName.MissingImage;
        //            tabIcon = SpriteName.MissingImage;
        //            category = TextLib.Error;
        //            tabName = TextLib.Error;
        //            break;
        //    }
        //}

        public static void Building(BuildAndExpandType buildingType, out SpriteName icon, out string name)
        {
            var build = BuildLib.BuildOptions[(int)buildingType];
            icon = build.sprite;
            Terrain(build.terrainType.mainTerrain, build.terrainType.subTerrain, out _, out name);
        }

        public static void Terrain(TerrainMainType mainType, int subType, out SpriteName icon, out string name)
        {
            icon = SpriteName.NO_IMAGE;
            name = null;

            switch (mainType)
            {
                case TerrainMainType.Building:
                    switch ((TerrainBuildingType)subType)
                    {
                        case TerrainBuildingType.Logistics:
                            icon = SpriteName.WarsBuild_Logistics;
                            name = DssRef.lang.BuildingType_Logistics;
                            break;
                        case TerrainBuildingType.ManorLord:
                            icon = SpriteName.WarsBuild_ManorLord;
                            name = DssRef.lang.BuildingType_ManorLord;
                            break;
                        case TerrainBuildingType.GreatHall:
                            icon = SpriteName.WarsBuild_GreatHall;
                            name = DssRef.todoLang.BuildingType_GreatHall;
                            break;
                        case TerrainBuildingType.SoldierBarracks:
                            icon = SpriteName.WarsBuild_SoldierBarracks;
                            name = DssRef.lang.BuildingType_SoldierBarracks;
                            break;
                        case TerrainBuildingType.Bank:
                            icon = SpriteName.WarsBuild_Bank;
                            name = DssRef.lang.BuildingType_Bank;
                            break;
                        case TerrainBuildingType.CoinMinter:
                            icon = SpriteName.WarsBuild_Coinminter;
                            name = DssRef.lang.BuildingType_CoinMaker;
                            break;
                        case TerrainBuildingType.Brewery:
                            icon = SpriteName.WarsBuild_Brewery;
                            name = DssRef.lang.BuildingType_Brewery;
                            break;
                        case TerrainBuildingType.Carpenter:
                            icon = SpriteName.WarsBuild_Carpenter;
                            name = DssRef.lang.BuildingType_Carpenter;
                            break;
                        case TerrainBuildingType.Work_CoalPit:
                            icon = SpriteName.WarsBuild_CoalPit;
                            name = DssRef.lang.BuildingType_CoalPit;
                            break;
                        case TerrainBuildingType.Work_Cook:
                            icon = SpriteName.WarsBuild_Cook;
                            name = DssRef.lang.BuildingType_Cook;
                            break;
                        case TerrainBuildingType.HenPen:
                            icon = SpriteName.WarsBuild_HenPen;
                            name = DssRef.lang.BuildingType_HenPen;
                            break;
                        case TerrainBuildingType.Nobelhouse:
                            icon = SpriteName.WarsBuild_Nobelhouse;
                            name = DssRef.lang.Building_NobleHouse;
                            break;
                        case TerrainBuildingType.ImmigrationTent:
                            icon = SpriteName.WarsBuild_Tent;
                            name = DssRef.lang.BuildingType_ImmigrationTent;
                            break;
                        case TerrainBuildingType.PigPen:
                            icon = SpriteName.WarsBuild_PigPen;
                            name = DssRef.lang.BuildingType_PigPen;
                            break;

                        case TerrainBuildingType.Postal:
                            icon = SpriteName.WarsBuild_Postal;
                            name = DssRef.lang.BuildingType_Postal;
                            break;
                        case TerrainBuildingType.PostalLevel2:
                            icon = SpriteName.WarsBuild_PostalLevel2;
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_Postal);
                            break;
                        case TerrainBuildingType.PostalLevel3:
                            icon = SpriteName.WarsBuild_PostalLevel3;
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_Postal);
                            break;

                        case TerrainBuildingType.GoldDeliveryLevel1:
                            icon = SpriteName.WarsBuild_GoldDeliver;
                            name = DssRef.lang.BuildingType_GoldDelivery;
                            break;
                        case TerrainBuildingType.GoldDeliveryLevel2:
                            icon = SpriteName.WarsBuild_GoldDeliverLevel2;
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_GoldDelivery);
                            break;
                        case TerrainBuildingType.GoldDeliveryLevel3:
                            icon = SpriteName.WarsBuild_GoldDeliverLevel3;
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_GoldDelivery);
                            break;

                        case TerrainBuildingType.Recruitment:
                            icon = SpriteName.WarsBuild_Recruitment;
                            name = DssRef.lang.BuildingType_Recruitment;
                            break;
                        case TerrainBuildingType.RecruitmentLevel2:
                            icon = SpriteName.WarsBuild_RecruitmentLevel2;
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_Recruitment);
                            break;
                        case TerrainBuildingType.RecruitmentLevel3:
                            icon = SpriteName.WarsBuild_RecruitmentLevel3;
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_Recruitment);
                            break;

                        case TerrainBuildingType.Work_Smith:
                            icon = SpriteName.WarsBuild_Smith;
                            name = DssRef.lang.BuildingType_Smith;
                            break;
                        case TerrainBuildingType.Storehouse:
                            icon = SpriteName.WarsBuild_Storehouse;
                            name = DssRef.lang.BuildingType_Storehouse;
                            break;
                        case TerrainBuildingType.Tavern:
                            icon = SpriteName.WarsBuild_Tavern;
                            name = DssRef.lang.BuildingType_Tavern;
                            break;
                        case TerrainBuildingType.Work_Bench:
                            icon = SpriteName.WarsBuild_WorkBench;
                            name = DssRef.lang.BuildingType_WorkBench;
                            break;

                        case TerrainBuildingType.WorkerTent:
                            icon = SpriteName.WarsBuild_TentHut;
                            name = DssRef.lang.BuildingType_WorkerHut;
                            break;
                        case TerrainBuildingType.WorkerHut:
                            icon = SpriteName.WarsBuild_WorkerHuts;
                            name = DssRef.lang.BuildingType_WorkerHut;
                            break;
                        case TerrainBuildingType.WorkerHutLarge:
                            icon = SpriteName.WarsBuild_WorkerHutLarge;
                            name = DssRef.lang.BuildingType_WorkerHut;
                            break;

                        case TerrainBuildingType.Smelter:
                            icon = SpriteName.WarsBuild_Smelter;
                            name = DssRef.lang.BuildingType_SmeltingFurnace;
                            break;
                        case TerrainBuildingType.WoodCutter:
                            icon = SpriteName.WarsBuild_WoodCutter;
                            name = DssRef.lang.BuildingType_WoodCutter;
                            break;
                        case TerrainBuildingType.StoneCutter:
                            icon = SpriteName.WarsBuild_StoneCutter;
                            name = DssRef.lang.BuildingType_StoneCutter;
                            break;
                        case TerrainBuildingType.Embassy:
                            icon = SpriteName.WarsBuild_Embassy;
                            name = DssRef.lang.BuildingType_Embassy;
                            break;
                        case TerrainBuildingType.WaterResovoir:
                            icon = SpriteName.WarsBuild_WaterReservoir;
                            name = DssRef.lang.BuildingType_WaterResovoir;
                            break;

                        case TerrainBuildingType.GuardHouse_Small:
                            icon = SpriteName.WarsBuild_GuardOffice;
                            name = DssRef.lang.BuildingType_GuardOffice;
                            break;
                        case TerrainBuildingType.GuardHouse_Large:
                            icon = SpriteName.WarsBuild_GuardOfficeLarge;
                            name = DssRef.lang.BuildingType_GuardOffice;
                            break;

                        case TerrainBuildingType.ArcherBarracks:
                            icon = SpriteName.WarsBuild_ArcherBarracks;
                            name = DssRef.lang.BuildingType_ArcherBarracks;
                            break;
                        case TerrainBuildingType.WarmachineBarracks:
                            icon = SpriteName.WarsBuild_WarmachineBarracks;
                            name = DssRef.lang.BuildingType_WarmachineBarracks;
                            break;
                        case TerrainBuildingType.GunBarracks:
                            icon = SpriteName.WarsBuild_GunBarracks;
                            name = DssRef.lang.BuildingType_GunBarracks;
                            break;
                        case TerrainBuildingType.CannonBarracks:
                            icon = SpriteName.WarsBuild_CannonBarracks;
                            name = DssRef.lang.BuildingType_CannonBarracks;
                            break;
                        //case TerrainBuildingType.KnightsBarracks:
                        //    name = DssRef.lang.BuildingType_KnightsBarracks;
                        //    break;

                        case TerrainBuildingType.Foundry:
                            icon = SpriteName.WarsBuild_Foundry;
                            name = DssRef.lang.BuildingType_Foundry;
                            break;
                        case TerrainBuildingType.Armory:
                            icon = SpriteName.WarsBuild_Armory;
                            name = DssRef.lang.BuildingType_Armory;
                            break;
                        case TerrainBuildingType.Chemist:
                            icon = SpriteName.WarsBuild_Chemist;
                            name = DssRef.lang.BuildingType_Chemist;
                            break;
                        case TerrainBuildingType.Gunmaker:
                            icon = SpriteName.WarsBuild_Gunmaker;
                            name = DssRef.lang.BuildingType_Gunmaker;
                            break;
                        case TerrainBuildingType.School:
                            icon = SpriteName.WarsBuild_School;
                            name = DssRef.lang.BuildingType_School;
                            break;
                        case TerrainBuildingType.ResearchCenter:
                            icon = SpriteName.WarsBuild_ResearchCenter;
                            name = DssRef.lang.BuildingType_ReseachCenter;
                            break;
                        case TerrainBuildingType.BookPress:
                            icon = SpriteName.WarsBuild_Bookpress;
                            name = DssRef.lang.BuildingType_Bookpress;
                            break;

                        case TerrainBuildingType.ServiceMenHouse_small:
                            icon = SpriteName.WarsBuild_SmallServiceHouse;
                            name = DssRef.lang.BuildingType_ServiceHouse;
                            break;
                        case TerrainBuildingType.ServiceMenHouse_Large:
                            icon = SpriteName.WarsBuild_BigServiceHouse;
                            name = DssRef.lang.BuildingType_ServiceHouse;
                            break;

                        // --- Everything below this line was already set up with icons in your provided block ---
                        case TerrainBuildingType.Pottery:
                            icon = SpriteName.WarsBuild_Pottery;
                            name = DssRef.todoLang.BuildingType_Pottery;
                            break;
                        case TerrainBuildingType.DryingPan:
                            icon = SpriteName.WarsBuild_DryingPan;
                            name = DssRef.todoLang.BuildingType_DryingPan;
                            break;
                        case TerrainBuildingType.Butcher:
                            icon = SpriteName.WarsBuild_Butcher;
                            name = DssRef.todoLang.BuildingType_Butcher;
                            break;
                        case TerrainBuildingType.Smoker:
                            icon = SpriteName.WarsBuild_Smoker;
                            name = DssRef.todoLang.BuildingType_Smoker;
                            break;
                        case TerrainBuildingType.Dryer:
                            icon = SpriteName.WarsBuild_Dryer;
                            name = DssRef.todoLang.BuildingType_Dryer;
                            break;

                        case TerrainBuildingType.MaterialStorage:
                            icon = SpriteName.WarsBuild_MaterialStorage;
                            name = DssRef.todoLang.BuildingType_MaterialStorage;
                            break;
                        case TerrainBuildingType.FoodStorage:
                            icon = SpriteName.WarsBuild_FoodStorage;
                            name = DssRef.todoLang.BuildingType_FoodStorage;
                            break;
                        case TerrainBuildingType.WeaponStorage:
                            icon = SpriteName.WarsBuild_WeaponStorage;
                            name = DssRef.todoLang.BuildingType_WeaponStorage;
                            break;
                        case TerrainBuildingType.ArmorStorage:
                            icon = SpriteName.WarsBuild_ArmorStorage;
                            name = DssRef.todoLang.BuildingType_ArmorStorage;
                            break;
                        case TerrainBuildingType.AnimalStorage:
                            icon = SpriteName.WarsBuild_AnimalStorage;
                            name = DssRef.todoLang.BuildingType_AnimalStorage;
                            break;

                        case TerrainBuildingType.Cesspit:
                            icon = SpriteName.WarsBuild_Cesspit;
                            name = DssRef.todoLang.BuildingType_Cesspit;
                            break;

                        case TerrainBuildingType.ShieldMaker:
                            icon = SpriteName.WarsBuild_Shieldmaker;
                            name = DssRef.todoLang.BuildingType_Shieldmaker;
                            break;

                        case TerrainBuildingType.TrappersHut:
                            icon = SpriteName.WarsBuild_Trapper;
                            name = DssRef.todoLang.BuildingType_TrapperHut;
                            break;

                        case TerrainBuildingType.FowlHabitat:
                            icon = SpriteName.WarsResource_Fowl;
                            name = TextLib.LargeFirstLetter( string.Format(DssRef.todoLang.Terrain_XAnimalHabitat, DssRef.todoLang.Resource_TypeName_Fowl));
                            break;
                        case TerrainBuildingType.BoarHabitat:
                            icon = SpriteName.WarsResource_Boar;
                            name = TextLib.LargeFirstLetter(string.Format(DssRef.todoLang.Terrain_XAnimalHabitat, DssRef.todoLang.Resource_TypeName_Boar));
                            break;
                        case TerrainBuildingType.OxHabitat:
                            icon = SpriteName.WarsResource_Oxen;
                            name = TextLib.LargeFirstLetter(string.Format(DssRef.todoLang.Terrain_XAnimalHabitat, DssRef.todoLang.Resource_TypeName_Oxen));
                            break;
                        case TerrainBuildingType.PonyHabitat:
                            icon = SpriteName.WarsResource_Pony;
                            name = TextLib.LargeFirstLetter(string.Format(DssRef.todoLang.Terrain_XAnimalHabitat, DssRef.todoLang.Resource_TypeName_Pony));
                            break;
                        case TerrainBuildingType.CatHabitat:
                            icon = SpriteName.WarsResource_WildCat;
                            name = TextLib.LargeFirstLetter(string.Format(DssRef.todoLang.Terrain_XAnimalHabitat, DssRef.todoLang.Resource_TypeName_WildCat));
                            break;
                        case TerrainBuildingType.DogHabitat:
                            icon = SpriteName.WarsResource_Dog;
                            name = TextLib.LargeFirstLetter(string.Format(DssRef.todoLang.Terrain_XAnimalHabitat, DssRef.todoLang.Resource_TypeName_Dog));
                            break;
                        case TerrainBuildingType.WolfHabitat:
                            icon = SpriteName.WarsResource_Wolf;
                            name = TextLib.LargeFirstLetter(string.Format(DssRef.todoLang.Terrain_XAnimalHabitat, DssRef.todoLang.Resource_TypeName_Wolf));
                            break;
                        case TerrainBuildingType.ElephantHabitat:
                            icon = SpriteName.WarsResource_Elephant;
                            name = TextLib.LargeFirstLetter(string.Format(DssRef.todoLang.Terrain_XAnimalHabitat, DssRef.todoLang.Resource_TypeName_Elephant));
                            break;

                        case TerrainBuildingType.FowlPen:
                            icon = SpriteName.WarsBuild_FowlPen;
                            name = DssRef.todoLang.BuildingType_FowlPen;
                            break;
                        case TerrainBuildingType.BoarPen:
                            icon = SpriteName.WarsBuild_BoarPen;
                            name = DssRef.todoLang.BuildingType_BoarPen;
                            break;

                           
                        case TerrainBuildingType.OxenPen:
                            icon = SpriteName.WarsBuild_OxenPen;
                            name = DssRef.todoLang.BuildingType_OxenPen;
                            break;
                        case TerrainBuildingType.KineOxenPen:
                            icon = SpriteName.WarsBuild_KineOxenPen;
                            name = DssRef.todoLang.BuildingType_KineOxenPen;
                            break;

                        case TerrainBuildingType.DogCage:
                            icon = SpriteName.WarsBuild_DogCage;
                            name = DssRef.todoLang.BuildingType_DogCage;
                            break;
                        case TerrainBuildingType.HoundCage:
                            icon = SpriteName.WarsBuild_HoundCage;
                            name = DssRef.todoLang.BuildingType_HoundCage;
                            break;

                        case TerrainBuildingType.PonyPen:
                            icon = SpriteName.WarsBuild_PonyPen;
                            name = DssRef.todoLang.BuildingType_PonyPen;
                            break;
                        case TerrainBuildingType.HorsePen:
                            icon = SpriteName.WarsBuild_HorsePen;
                            name = DssRef.todoLang.BuildingType_HorsePen;
                            break;
                        case TerrainBuildingType.WarHorsePen:
                            icon = SpriteName.WarsBuild_WarHorsePen;
                            name = DssRef.todoLang.BuildingType_WarHorsePen;
                            break;
                        case TerrainBuildingType.DraftHorsePen:
                            icon = SpriteName.WarsBuild_DraftHorsePen;
                            name = DssRef.todoLang.BuildingType_DraftHorsePen;
                            break;
                        case TerrainBuildingType.WildPigPen:
                            icon = SpriteName.WarsBuild_WildPigPen;
                            name = DssRef.todoLang.BuildingType_WildPigPen;
                            break;
                        case TerrainBuildingType.WildHogPen:
                            icon = SpriteName.WarsBuild_WildHogPen;
                            name = DssRef.todoLang.BuildingType_WildHogPen;
                            break;
                        case TerrainBuildingType.WarHogPen:
                            icon = SpriteName.WarsBuild_WarHogPen;
                            name = DssRef.todoLang.BuildingType_WarHogPen;
                            break;
                        case TerrainBuildingType.StagHogPen:
                            icon = SpriteName.WarsBuild_StagHogPen;
                            name = DssRef.todoLang.BuildingType_StagHogPen;
                            break;
                        case TerrainBuildingType.WolfCage:
                            icon = SpriteName.WarsBuild_WolfPen;
                            name = DssRef.todoLang.BuildingType_WolfCage;
                            break;
                        case TerrainBuildingType.WargCage:
                            icon = SpriteName.WarsBuild_AlphaWargPen;
                            name = DssRef.todoLang.BuildingType_WargCage;
                            break;
                        case TerrainBuildingType.AlphaWargCage:
                            icon = SpriteName.WarsBuild_AlphaWargPen;
                            name = DssRef.todoLang.BuildingType_AlphaWargCage;
                            break;
                        case TerrainBuildingType.WildCatCage:
                            icon = SpriteName.WarsBuild_WildCatPen;
                            name = DssRef.todoLang.BuildingType_WildCatCage;
                            break;
                        case TerrainBuildingType.LionCage:
                            icon = SpriteName.WarsBuild_LionPen;
                            name = DssRef.todoLang.BuildingType_LionCage;
                            break;
                        case TerrainBuildingType.WarLionCage:
                            icon = SpriteName.WarsBuild_WarLionPen;
                            name = DssRef.todoLang.BuildingType_WarLionCage;
                            break;
                        case TerrainBuildingType.ElephantCage:
                            icon = SpriteName.WarsBuild_ElephantPen;
                            name = DssRef.todoLang.BuildingType_ElephantCage;
                            break;
                        case TerrainBuildingType.WarElephantCage:
                            icon = SpriteName.WarsBuild_WarElephantPen;
                            name = DssRef.todoLang.BuildingType_WarElephantCage;
                            break;
                        case TerrainBuildingType.OliphantCage:
                            icon = SpriteName.WarsBuild_OliphantPen;
                            name = DssRef.todoLang.BuildingType_OliphantCage;
                            break;

                        default:
                            name = DssRef.lang.BuildingType_DefaultName;
                            break;
                    }
                    break;

                case TerrainMainType.Foil:
                    switch ((TerrainSubFoilType)subType)
                    {
                        default:
                            name = DssRef.lang.LandType_Flatland;
                            break;

                        case TerrainSubFoilType.TreeHard:
                        case TerrainSubFoilType.TreeSoft:
                        case TerrainSubFoilType.DryWood:
                            icon = SpriteName.WarsBuild_TreeSoft;
                            name = DssRef.lang.Resource_TypeName_Wood;
                            break;

                        case TerrainSubFoilType.TreeSoftSprout:
                            name = DssRef.lang.Building_TreeSprout_Soft;
                            break;
                        case TerrainSubFoilType.TreeHardSprout:
                            name = DssRef.lang.Building_TreeSprout_Hard;
                            break;

                        case TerrainSubFoilType.StoneBlock:
                        case TerrainSubFoilType.Stones:
                            icon = SpriteName.WarsResource_Stone;
                            name = DssRef.lang.Resource_TypeName_Stone;
                            break;

                        case TerrainSubFoilType.LinenFarm:
                            name = string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Linen);
                            break;
                        case TerrainSubFoilType.LinenFarmUpgraded:
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded,
                                string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Linen));
                            break;

                        case TerrainSubFoilType.TreeApple:
                            icon = SpriteName.WarsBuild_TreeApple;
                            name = DssRef.lang.BuildingType_Orchard;
                            break;
                        case TerrainSubFoilType.TreeBanana:
                            icon = SpriteName.WarsBuild_TreeBanana;
                            name = DssRef.lang.BuildingType_Orchard;
                            break;

                        case TerrainSubFoilType.WheatFarm:
                            name = string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Wheat);
                            break;
                        case TerrainSubFoilType.WheatFarmUpgraded:
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded,
                                string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Wheat));
                            break;

                        case TerrainSubFoilType.RapeSeedFarm:
                            name = string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Rapeseed);
                            break;
                        case TerrainSubFoilType.RapeSeedFarmUpgraded:
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded,
                                string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Rapeseed));
                            break;

                        case TerrainSubFoilType.HempFarm:
                            name = string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Hemp);
                            break;
                        case TerrainSubFoilType.HempFarmUpgraded:
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded,
                                string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Hemp));
                            break;

                        case TerrainSubFoilType.BogIron:
                            name = DssRef.lang.Resource_TypeName_BogIron;
                            break;

                        case TerrainSubFoilType.ClayPit:
                            icon = SpriteName.WarsClayPit;
                            name = DssRef.todoLang.BuildingType_ClayPit;
                            break;
                    }
                    break;

                case TerrainMainType.Mine:
                    icon = SpriteName.WarsWorkMine;
                    switch ((TerrainMineType)subType)
                    {
                        case TerrainMineType.StoneBlock:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Stone);
                            break;
                        case TerrainMineType.IronOre:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Iron);
                            break;
                        case TerrainMineType.Coal:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Coal);
                            break;
                        case TerrainMineType.GoldOre:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.ResourceType_Gold);
                            break;

                        case TerrainMineType.TinOre:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Tin);
                            break;
                        case TerrainMineType.CopperOre:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Copper);
                            break;
                        case TerrainMineType.SilverOre:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Silver);
                            break;
                        case TerrainMineType.LeadOre:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Lead);
                            break;
                        case TerrainMineType.Mithril:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Mithril);
                            break;
                        case TerrainMineType.Salt:
                            name = TextLib.LargeFirstLetter(string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.todoLang.Resource_TypeName_Salt));
                            break;
                        case TerrainMineType.Sulfur:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Sulfur);
                            break;
                    }
                    break;

                case TerrainMainType.Road:
                    switch ((TerrainRoadType)subType)
                    {
                        case TerrainRoadType.DirtRoad:
                            name = DssRef.lang.BuildingType_DirtRoad;
                            break;
                    }
                    break;

                case TerrainMainType.Decor:
                    switch ((TerrainDecorType)subType)
                    {
                        case TerrainDecorType.Pavement:
                            name = string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Pavement);
                            break;
                        case TerrainDecorType.PavementFlower:
                            name = string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Pavement);
                            break;
                        case TerrainDecorType.PavementRectFlower:
                            name = string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Pavement);
                            break;
                        case TerrainDecorType.PavementLamp:
                            name = string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Pavement);
                            break;
                        case TerrainDecorType.PavemenFountain:
                            name = string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Pavement);
                            break;

                        case TerrainDecorType.Statue_ThePlayer:
                            name = string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Statue);
                            break;

                        case TerrainDecorType.GardenFourBushes:
                            name = string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Garden);
                            break;
                        case TerrainDecorType.GardenLongTree:
                            name = string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Garden);
                            break;
                        case TerrainDecorType.GardenWalledBush:
                            name = string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Garden);
                            break;
                        case TerrainDecorType.GardenGrass:
                            name = string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Garden);
                            break;
                        case TerrainDecorType.GardenBird:
                            name = string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Garden);
                            break;

                        case TerrainDecorType.GardenMemoryStone:
                            name = string.Format(DssRef.lang.VariantType_F, DssRef.lang.DecorType_Garden);
                            break;

                        case TerrainDecorType.Statue_Leader:
                            name = string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Statue);
                            break;
                        case TerrainDecorType.Statue_Lion:
                            name = string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Statue);
                            break;
                        case TerrainDecorType.Statue_Horse:
                            name = string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Statue);
                            break;
                        case TerrainDecorType.Statue_Pillar:
                            name = string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Statue);
                            break;

                        case TerrainDecorType.FlagPole_LongBanner:
                            name = string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Banner);
                            break;
                        case TerrainDecorType.FlagPole_Banner:
                            name = string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Banner);
                            break;
                        case TerrainDecorType.FlagPole_SlimBanner:
                            name = string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Banner);
                            break;

                        case TerrainDecorType.FlagPole_Flag:
                            name = string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Flag);
                            break;
                        case TerrainDecorType.FlagPole_FlagRound:
                            name = string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Flag);
                            break;
                        case TerrainDecorType.FlagPole_FlagLarge:
                            name = string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Flag);
                            break;
                        case TerrainDecorType.FlagPole_Streamer:
                            name = string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Flag);
                            break;
                        case TerrainDecorType.FlagPole_Triangle:
                            name = string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Flag);
                            break;

                        case TerrainDecorType.CobbleStones:
                            name = DssRef.lang.DecorType_CobbleStones;
                            break;
                        case TerrainDecorType.Square:
                            name = DssRef.lang.DecorType_Square;
                            break;
                    }
                    break;

                case TerrainMainType.Destroyed:
                case TerrainMainType.DefaultLand:
                    name = DssRef.lang.LandType_Flatland;
                    break;

                case TerrainMainType.DefaultSea:
                    name = DssRef.lang.LandType_Water;
                    break;

                case TerrainMainType.Resourses:
                    name = DssRef.lang.Resource;
                    break;

                case TerrainMainType.Wall:
                    switch ((TerrainWallType)subType)
                    {
                        case TerrainWallType.Palisade:
                            name = DssRef.lang.BuildingType_Palisade;
                            break;
                        case TerrainWallType.DirtWall:
                            name = DssRef.lang.BuildingType_DirtWall;
                            break;
                        case TerrainWallType.DirtTower:
                            name = DssRef.lang.BuildingType_DirtTower;
                            break;
                        case TerrainWallType.WoodWall:
                            name = DssRef.lang.BuildingType_WoodWall;
                            break;
                        case TerrainWallType.WoodTower:
                            name = DssRef.lang.BuildingType_WoodTower;
                            break;
                        case TerrainWallType.StoneWall:
                            name = string.Format(DssRef.lang.VariantType_A, DssRef.lang.BuildingType_StoneWall);
                            break;
                        case TerrainWallType.StoneTower:
                            name = DssRef.lang.BuildingType_StoneTower;
                            break;
                        case TerrainWallType.StoneWallGreen:
                            name = string.Format(DssRef.lang.VariantType_B, DssRef.lang.BuildingType_StoneWall);
                            break;
                        case TerrainWallType.StoneWallBlueRoof:
                            name = string.Format(DssRef.lang.VariantType_C, DssRef.lang.BuildingType_StoneWall);
                            break;
                        case TerrainWallType.StoneWallWoodHouse:
                            name = string.Format(DssRef.lang.VariantType_D, DssRef.lang.BuildingType_StoneWall);
                            break;
                        case TerrainWallType.StoneGate:
                            name = DssRef.lang.BuildingType_StoneGate;
                            break;
                        case TerrainWallType.StoneHouse:
                            name = DssRef.lang.BuildingType_StoneHouse;
                            break;

                        default:
                            name = DssRef.lang.BuildingType_Wall;
                            break;
                    }
                    break;
            }

            if (name == null)
            {
                name = $"UNKNOWN ({mainType} {subType})";
            }
        }

    }
}
