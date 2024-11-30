using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.PJ.Joust;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        /// <remarks>
        /// Changing the list will corrupt the save files!
        /// </remarks>
        public static readonly ItemResourceType[] MovableCityResourceTypes =
        {
             ItemResourceType.Wood_Group,
             ItemResourceType.Fuel_G,
             ItemResourceType.Stone_G,
             ItemResourceType.RawFood_Group,
             ItemResourceType.Food_G,
             ItemResourceType.Beer,
             ItemResourceType.SkinLinen_Group,

             ItemResourceType.IronOre_G,
             ItemResourceType.TinOre,
             ItemResourceType.CupperOre,
             ItemResourceType.LeadOre,
             ItemResourceType.SilverOre,

             ItemResourceType.Iron_G,
             ItemResourceType.Tin,
            ItemResourceType.Cupper,
            ItemResourceType.Lead,
            ItemResourceType.Silver,
            ItemResourceType.RawMithril,
            ItemResourceType.Sulfur,

            ItemResourceType.Bronze,
            ItemResourceType.CastIron,
            ItemResourceType.BloomeryIron,
            ItemResourceType.Mithril,

            ItemResourceType.Toolkit,
            ItemResourceType.Wagon2Wheel,
            ItemResourceType.Wagon4Wheel,
            ItemResourceType.BlackPowder,
            ItemResourceType.GunPowder,
            ItemResourceType.LedBullet,

            ItemResourceType.SharpStick,
            ItemResourceType.BronzeSword,
            ItemResourceType.ShortSword,
            ItemResourceType.Sword,
            ItemResourceType.LongSword,
            ItemResourceType.HandSpear,
            ItemResourceType.MithrilSword,

            ItemResourceType.Warhammer,
             ItemResourceType.TwoHandSword,
             ItemResourceType.KnightsLance,

             ItemResourceType.SlingShot,
             ItemResourceType.ThrowingSpear,
             ItemResourceType.Bow,
             ItemResourceType.LongBow,
            ItemResourceType.MithrilBow,

            ItemResourceType.Ballista,
            ItemResourceType.Manuballista,
            ItemResourceType.SiegeCannonBronze,
            ItemResourceType.ManCannonBronze,
            ItemResourceType.SiegeCannonIron,
            ItemResourceType.ManCannonIron,

             ItemResourceType.PaddedArmor,
             ItemResourceType.HeavyPaddedArmor,
             ItemResourceType.BronzeArmor,
             ItemResourceType.IronArmor,
             ItemResourceType.HeavyIronArmor,
             ItemResourceType.LightPlateArmor,
             ItemResourceType.FullPlateArmor,
        };

        MinuteStats blackMarketCosts_food = new MinuteStats();
        public MinuteStats foodProduction = new MinuteStats();
        public MinuteStats foodSpending = new MinuteStats();
        public MinuteStats soldResources = new MinuteStats();

        //public int water = Maxwater;
        int waterBuffer = 2;
        int waterSpendOrders = 0;

        public int maxWaterBase = DssConst.Maxwater;
        public int maxWaterTotal = DssConst.Maxwater;
        FloatingInt nextWater = new FloatingInt();
        public float waterAddPerSec;
        static readonly GroupedResource Res_Nothing = new GroupedResource() { amount = 100000 };

        public GroupedResource res_water = new GroupedResource();
        public GroupedResource res_wood = new GroupedResource() { amount = 20, goalBuffer = 300 };
        public GroupedResource res_fuel = new GroupedResource() { amount = 20, goalBuffer = 300 };
        public GroupedResource res_stone = new GroupedResource() { amount = 20, goalBuffer = 100 };
        public GroupedResource res_rawFood = new GroupedResource() { amount = 50, goalBuffer = 200 };
        public GroupedResource res_food = new GroupedResource() { amount = 200, goalBuffer = 500 };
        public GroupedResource res_beer = new GroupedResource() { amount = 0, goalBuffer = 200 };
        public GroupedResource res_skinLinnen = new GroupedResource() { goalBuffer = 100 };

        public GroupedResource res_ironore = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_TinOre = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_CupperOre = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_LeadOre = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_SilverOre = new GroupedResource() { goalBuffer = 100 };

        public GroupedResource res_iron = new GroupedResource() { amount = 10, goalBuffer = 100 };
        public GroupedResource res_Tin = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_Cupper = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_Lead = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_Silver = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_RawMithril = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_Sulfur = new GroupedResource() { goalBuffer = 100 };

        public GroupedResource res_Bronze = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_Steel = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_CastIron = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_BloomeryIron = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_Mithril = new GroupedResource() { goalBuffer = 100 };

        public GroupedResource res_Toolkit = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_Wagon2Wheel = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_Wagon4Wheel = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_BlackPowder = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_GunPowder = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_LedBullet = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_sharpstick = new GroupedResource() { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
        public GroupedResource res_BronzeSword = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_shortsword = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_Sword = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_LongSword = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_HandSpear = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_MithrilSword = new GroupedResource() { goalBuffer = 100 };

        public GroupedResource res_Warhammer = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_twohandsword = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_knightslance = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_SlingShot = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_ThrowingSpear = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_bow = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_longbow = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_crossbow = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_MithrilBow = new GroupedResource() { goalBuffer = 100 };

        public GroupedResource res_HandCannon = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_HandCulvertin = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_Rifle = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_Blunderbus = new GroupedResource() { goalBuffer = 100 };

        public GroupedResource res_BatteringRam = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_ballista = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_Manuballista = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_Catapult = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_SiegeCannonBronze = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_ManCannonBronze = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_SiegeCannonIron = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_ManCannonIron = new GroupedResource() { goalBuffer = 100 };

        public GroupedResource res_paddedArmor = new GroupedResource() { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
        public GroupedResource res_HeavyPaddedArmor = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_BronzeArmor = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_mailArmor = new GroupedResource() { amount = 2, goalBuffer = 100 };
        public GroupedResource res_heavyMailArmor = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_LightPlateArmor = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_FullPlateArmor = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_MithrilArmor = new GroupedResource() { goalBuffer = 100 };

        public bool res_food_safeguard = true;

        public bool foodSafeGuardIsActive(ItemResourceType item)
        {
            bool food = foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard);
            switch (item)
            {
                case ItemResourceType.Food_G:
                    return food;
                case ItemResourceType.Fuel_G:
                    return fuelSafeGuard;
                case ItemResourceType.RawFood_Group:
                    return rawFoodSafeGuard;
                case ItemResourceType.Wood_Group:
                    return woodSafeGuard;
            }

            return false;
        }

        public bool foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard)
        {
            if (res_food_safeguard && res_food.amount <= DssConst.WorkSafeGuardAmount)
            {
                fuelSafeGuard = res_fuel.amount <= DssConst.WorkSafeGuardAmount;
                rawFoodSafeGuard = res_rawFood.amount <= DssConst.WorkSafeGuardAmount;
                woodSafeGuard = fuelSafeGuard && res_wood.amount <= DssConst.WorkSafeGuardAmount;
                return true;
            }
            else
            {
                fuelSafeGuard = false;
                rawFoodSafeGuard = false;
                woodSafeGuard = false;
                return false;
            }
        }


        public TradeTemplate tradeTemplate = new TradeTemplate();
        public const int DefaultFoodBuffer = 500;
        public const int Logistics1FoodStorage = 300;
        public void defaultResourceBuffer()
        {
            res_wood.goalBuffer = 300;
            res_fuel.goalBuffer = 300;
            res_stone.goalBuffer = 200;
            res_rawFood.goalBuffer = 200;
            res_food.goalBuffer = DefaultFoodBuffer;
            res_beer.goalBuffer = 200;
            res_skinLinnen.goalBuffer = 100;

            res_ironore.goalBuffer = 100;
            res_TinOre.goalBuffer = 100;
            res_CupperOre.goalBuffer = 100;
            res_LeadOre.goalBuffer = 100;
            res_SilverOre.goalBuffer = 100;

            res_iron.goalBuffer = 100;
            res_Tin.goalBuffer = 100;
            res_Cupper.goalBuffer = 100;
            res_Lead.goalBuffer = 100;
            res_Silver.goalBuffer = 100;
            res_RawMithril.goalBuffer = 100;
            res_Sulfur.goalBuffer = 100;

            res_Steel.goalBuffer = 100;
            res_Bronze.goalBuffer = 100;
            res_CastIron.goalBuffer = 100; 
            res_BloomeryIron.goalBuffer = 100; 
            res_Mithril.goalBuffer = 100;

            res_sharpstick.goalBuffer = 100;
            res_shortsword.goalBuffer = 100;
            res_Sword.goalBuffer = 100;
            res_LongSword.goalBuffer = 100;
            res_HandSpear.goalBuffer = 100;

            res_Warhammer.goalBuffer = 100;
            res_twohandsword.goalBuffer = 100;
            res_knightslance.goalBuffer = 100;

            res_SlingShot.goalBuffer = 100;
            res_ThrowingSpear.goalBuffer = 100;
            res_bow.goalBuffer = 100;
            res_longbow.goalBuffer = 100;
            res_crossbow.goalBuffer = 100;
            res_MithrilBow.goalBuffer = 100;
            
            res_HandCannon.goalBuffer = 100;
            res_HandCulvertin.goalBuffer = 100;
            res_Rifle.goalBuffer = 100;
            res_Blunderbus.goalBuffer = 100;

            res_ballista.goalBuffer = 100;
            res_Manuballista.goalBuffer = 100;
            res_Catapult.goalBuffer = 100;
            res_BatteringRam.goalBuffer = 100;

            res_SiegeCannonBronze.goalBuffer = 100;
            res_ManCannonBronze.goalBuffer = 100;
            res_SiegeCannonIron.goalBuffer = 100;
            res_ManCannonIron.goalBuffer = 100;

            res_paddedArmor.goalBuffer = 100;
            res_HeavyPaddedArmor.goalBuffer = 100;
            res_BronzeArmor.goalBuffer = 100;
            res_mailArmor.goalBuffer = 100;
            res_heavyMailArmor.goalBuffer = 100;
            res_LightPlateArmor.goalBuffer = 100;
            res_FullPlateArmor.goalBuffer = 100;

        }

        public void AddGroupedResource(ItemResourceType type, int add)
        {
            if (add == 0)
            { 
                lib.DoNothing();
            }

            switch (type)
            {
                case ItemResourceType.Gold:
                    faction.gold += add;
                    break;
                case ItemResourceType.Water_G:
                    res_water.amount += add;
                    break;
                case ItemResourceType.Food_G:
                    res_food.amount += add;
                    break;
                case ItemResourceType.Beer:
                    res_beer.amount += add;
                    break;
                case ItemResourceType.Stone_G:
                    res_stone.amount += add;
                    break;
                case ItemResourceType.Wood_Group:
                    res_wood.amount += add;
                    break;
                case ItemResourceType.Fuel_G:
                    res_fuel.amount += add;
                    break;
                case ItemResourceType.RawFood_Group:
                    res_rawFood.amount += add;
                    break;
                case ItemResourceType.SkinLinen_Group:
                    res_skinLinnen.amount += add;
                    break;
                case ItemResourceType.Toolkit:
                    res_Toolkit.amount += add;
                    break;
                case ItemResourceType.Wagon2Wheel:
                    res_Wagon2Wheel.amount += add;
                    break;
                case ItemResourceType.Wagon4Wheel:
                    res_Wagon4Wheel.amount += add;
                    break;
                case ItemResourceType.BlackPowder:
                    res_BlackPowder.amount += add;
                    break;
                case ItemResourceType.GunPowder:
                    res_GunPowder.amount += add;
                    break;
                case ItemResourceType.LedBullet:
                    res_LedBullet.amount += add;
                    break;

                case ItemResourceType.IronOre_G:
                    res_ironore.amount += add;
                    break;
                case ItemResourceType.TinOre:
                    res_TinOre.amount += add;
                    break;
                case ItemResourceType.CupperOre:
                    res_CupperOre.amount += add;
                    break;
                case ItemResourceType.LeadOre:
                    res_LeadOre.amount += add;
                    break;
                case ItemResourceType.SilverOre:
                    res_SilverOre.amount += add;
                    break;

                case ItemResourceType.Iron_G:
                    res_iron.amount += add;
                    break;
                case ItemResourceType.Tin:
                    res_Tin.amount += add;
                    break;
                case ItemResourceType.Cupper:
                    res_Cupper.amount += add;
                    break;
                case ItemResourceType.Lead:
                    res_Lead.amount += add;
                    break;
                case ItemResourceType.Silver:
                    res_Silver.amount += add;
                    break;
                case ItemResourceType.RawMithril:
                    res_RawMithril.amount += add;
                    break;
                case ItemResourceType.Sulfur:
                    res_Sulfur.amount += add;
                    break;

                case ItemResourceType.Steel:
                    res_Steel.amount += add;
                    break;
                case ItemResourceType.Bronze:
                    res_Bronze.amount += add;
                    break;
                case ItemResourceType.CastIron:
                    res_CastIron.amount += add;
                    break;
                case ItemResourceType.BloomeryIron:
                    res_BloomeryIron.amount += add;
                    break;
                case ItemResourceType.Mithril:
                    res_Mithril.amount += add;
                    break;
                                 

                case ItemResourceType.SharpStick:
                    res_sharpstick.amount += add;
                    break;
                case ItemResourceType.BronzeSword:
                    res_BronzeSword.amount += add;
                    break;
                case ItemResourceType.ShortSword:
                    res_shortsword.amount += add;
                    break;
                case ItemResourceType.Sword:
                    res_Sword.amount += add;
                    break;
                case ItemResourceType.LongSword:
                    res_LongSword.amount += add;
                    break;
                case ItemResourceType.HandSpear:
                    res_HandSpear.amount += add;
                    break;

                case ItemResourceType.Warhammer:
                    res_Warhammer.amount += add;
                    break;
                case ItemResourceType.TwoHandSword:
                    res_twohandsword.amount += add;
                    break;
                case ItemResourceType.KnightsLance:
                    res_knightslance.amount += add;
                    break;

                case ItemResourceType.SlingShot:
                    res_SlingShot.amount += add;
                    break;
                case ItemResourceType.ThrowingSpear:
                    res_ThrowingSpear.amount += add;
                    break;
                case ItemResourceType.Bow:
                    res_bow.amount += add;
                    break;
                case ItemResourceType.LongBow:
                    res_longbow.amount += add;
                    break;
                case ItemResourceType.Crossbow:
                    res_crossbow.amount += add;
                    break;
                case ItemResourceType.MithrilBow:
                    res_MithrilBow.amount += add;
                    break;

                case ItemResourceType.Ballista:
                    res_ballista.amount += add;
                    break;
                case ItemResourceType.Manuballista:
                    res_Manuballista.amount += add;
                    break;
                case ItemResourceType.Catapult:
                    res_Catapult.amount += add;
                    break;
                case ItemResourceType.UN_BatteringRam:
                    res_BatteringRam.amount += add;
                    break;
                case ItemResourceType.SiegeCannonBronze:
                    res_SiegeCannonBronze.amount += add;
                    break;
                case ItemResourceType.ManCannonBronze:
                    res_ManCannonBronze.amount += add;
                    break;
                case ItemResourceType.SiegeCannonIron:
                    res_SiegeCannonIron.amount += add;
                    break;
                case ItemResourceType.ManCannonIron:
                    res_ManCannonIron.amount += add;
                    break;

                case ItemResourceType.PaddedArmor:
                    res_paddedArmor.amount += add;
                    break;
                case ItemResourceType.HeavyPaddedArmor:
                    res_HeavyPaddedArmor.amount += add;
                    break;
                case ItemResourceType.BronzeArmor:
                    res_BronzeArmor.amount += add;
                    break;
                case ItemResourceType.IronArmor:
                    res_mailArmor.amount += add;
                    break;
                case ItemResourceType.HeavyIronArmor:
                    res_heavyMailArmor.amount += add;
                    break;
                case ItemResourceType.LightPlateArmor:
                    res_LightPlateArmor.amount += add;
                    break;
                case ItemResourceType.FullPlateArmor:
                    res_FullPlateArmor.amount += add;
                    break;

                case ItemResourceType.NONE:
                    return;

                default:
                    throw new NotImplementedException();
            }
        }

                case ItemResourceType.Men:
                    return workForce;

        public bool needMore(ItemResourceType type, bool rawfoodSafeGuard, bool woodSafeGuard, out bool usesSafeGuard)
        {
            usesSafeGuard = false;
            switch (type)
            {
                case ItemResourceType.RawFood_Group:
                case ItemResourceType.Wheat:
                case ItemResourceType.Egg:
                case ItemResourceType.Hen:
                    if (rawfoodSafeGuard)
                    {
                        usesSafeGuard = true;
                        return true;
                    }
                    return res_rawFood.needMore();

                case ItemResourceType.Pig:
                    if (rawfoodSafeGuard)
                    {
                        usesSafeGuard = true;
                        return true;
                    }
                    return res_food.needMore() || res_skinLinnen.needMore();

                case ItemResourceType.Wood_Group:
                case ItemResourceType.DryWood:
                case ItemResourceType.SoftWood:
                case ItemResourceType.HardWood:
                    if (woodSafeGuard)
                    {
                        usesSafeGuard = true;
                        return true;
                    }
                    return res_wood.needMore();

                case ItemResourceType.NONE:
                    return false;

                default:
//#if DEBUG
                    return GetGroupedResource(type).needMore();
                    //throw new NotImplementedException();
//#else
//                    return false;
//#endif
            }
        }


        public GroupedResource GetGroupedResource(ItemResourceType type)
        {
            switch (type)
            {
                case ItemResourceType.Gold:
                    return new GroupedResource() { amount = faction.gold };
                case ItemResourceType.GoldOre:
                    return new GroupedResource() { amount = 1 };

                case ItemResourceType.Water_G: return res_water;

                case ItemResourceType.Beer: return res_beer;
                case ItemResourceType.Food_G: return res_food;
                case ItemResourceType.Stone_G: return res_stone;
                case ItemResourceType.Wood_Group: return res_wood;
                case ItemResourceType.Fuel_G: return res_fuel;
                case ItemResourceType.RawFood_Group: return res_rawFood;
                case ItemResourceType.SkinLinen_Group: return res_skinLinnen;

                case ItemResourceType.Toolkit: return res_Toolkit;
                case ItemResourceType.Wagon2Wheel: return res_Wagon2Wheel;
                case ItemResourceType.Wagon4Wheel: return res_Wagon4Wheel;
                case ItemResourceType.BlackPowder: return res_BlackPowder;
                case ItemResourceType.GunPowder: return res_GunPowder;
                case ItemResourceType.LedBullet: return res_LedBullet;

                case ItemResourceType.IronOre_G: return res_ironore;
                case ItemResourceType.TinOre: return res_TinOre;
                case ItemResourceType.CupperOre: return res_CupperOre;
                case ItemResourceType.LeadOre: return res_LeadOre;
                case ItemResourceType.SilverOre: return res_SilverOre;

                case ItemResourceType.Iron_G: return res_iron;
                case ItemResourceType.Tin: return res_Tin;
                case ItemResourceType.Cupper: return res_Cupper;
                case ItemResourceType.Lead: return res_Lead;
                case ItemResourceType.Silver: return res_Silver;
                case ItemResourceType.RawMithril: return res_RawMithril;
                case ItemResourceType.Sulfur: return res_Sulfur;

                case ItemResourceType.Steel: return res_Steel;
                case ItemResourceType.Bronze: return res_Bronze;
                case ItemResourceType.CastIron: return res_CastIron;
                case ItemResourceType.BloomeryIron: return res_BloomeryIron;
                case ItemResourceType.Mithril: return res_Mithril;

                case ItemResourceType.SharpStick: return res_sharpstick;
                case ItemResourceType.BronzeSword: return res_BronzeSword;
                case ItemResourceType.ShortSword: return res_shortsword;
                case ItemResourceType.Sword: return res_Sword;
                case ItemResourceType.LongSword: return res_LongSword;
                case ItemResourceType.HandSpear: return res_HandSpear;
                case ItemResourceType.MithrilSword: return res_MithrilSword;

                case ItemResourceType.Warhammer: return res_Warhammer;
                case ItemResourceType.TwoHandSword: return res_twohandsword;
                case ItemResourceType.KnightsLance: return res_knightslance;

                case ItemResourceType.SlingShot: return res_SlingShot;
                case ItemResourceType.ThrowingSpear: return res_ThrowingSpear;
                case ItemResourceType.Bow: return res_bow;
                case ItemResourceType.LongBow: return res_longbow;
                case ItemResourceType.Crossbow: return res_crossbow;
                case ItemResourceType.MithrilBow: return res_MithrilBow;

                case ItemResourceType.HandCannon: return res_HandCannon;
                case ItemResourceType.HandCulverin: return res_HandCulvertin;
                case ItemResourceType.Rifle: return res_Rifle;
                case ItemResourceType.Blunderbus: return res_Blunderbus;

                case ItemResourceType.Ballista: return res_ballista;
                case ItemResourceType.Manuballista: return res_Manuballista;
                case ItemResourceType.Catapult: return res_Catapult;
                case ItemResourceType.UN_BatteringRam: return res_BatteringRam;

                case ItemResourceType.SiegeCannonBronze: return res_SiegeCannonBronze;
                case ItemResourceType.ManCannonBronze: return res_ManCannonBronze;
                case ItemResourceType.SiegeCannonIron: return res_SiegeCannonIron;
                case ItemResourceType.ManCannonIron: return res_ManCannonIron;

                case ItemResourceType.PaddedArmor: return res_paddedArmor;
                case ItemResourceType.HeavyPaddedArmor: return res_HeavyPaddedArmor;
                case ItemResourceType.BronzeArmor: return res_BronzeArmor;
                case ItemResourceType.IronArmor: return res_mailArmor;
                case ItemResourceType.HeavyIronArmor: return res_heavyMailArmor;
                case ItemResourceType.LightPlateArmor: return res_LightPlateArmor;
                case ItemResourceType.FullPlateArmor: return res_FullPlateArmor;
                case ItemResourceType.MithrilArmor: return res_MithrilArmor;

                case ItemResourceType.NONE: return Res_Nothing;

                default:
                    throw new NotImplementedException();
            }
        }


        public void SetGroupedResource(ItemResourceType type, GroupedResource resource)
        {
            switch (type)
            {
                case ItemResourceType.Water_G:
                    res_water = resource;
                    break;
                case ItemResourceType.Food_G:
                    res_food = resource;
                    break;
                case ItemResourceType.Beer:
                    res_beer = resource;
                    break;
                case ItemResourceType.Stone_G:
                    res_stone = resource;
                    break;
                case ItemResourceType.Wood_Group:
                    res_wood = resource;
                    break;
                case ItemResourceType.Fuel_G:
                    res_fuel = resource;
                    break;
                case ItemResourceType.RawFood_Group:
                    res_rawFood = resource;
                    break;
                case ItemResourceType.SkinLinen_Group:
                    res_skinLinnen = resource;
                    break;
                case ItemResourceType.Toolkit:
                    res_Toolkit = resource;
                    break;
                case ItemResourceType.Wagon2Wheel:
                    res_Wagon2Wheel = resource;
                    break;
                case ItemResourceType.Wagon4Wheel:
                    res_Wagon4Wheel = resource;
                    break;
                case ItemResourceType.BlackPowder:
                    res_BlackPowder = resource;
                    break;
                case ItemResourceType.GunPowder:
                    res_GunPowder = resource;
                    break;
                case ItemResourceType.LedBullet:
                    res_LedBullet = resource;
                    break;
                case ItemResourceType.IronOre_G:
                    res_ironore = resource;
                    break;
                case ItemResourceType.TinOre:
                    res_TinOre = resource;
                    break;
                case ItemResourceType.CupperOre:
                    res_CupperOre = resource;
                    break;
                case ItemResourceType.LeadOre:
                    res_LeadOre = resource;
                    break;
                case ItemResourceType.SilverOre:
                    res_SilverOre = resource;
                    break;
                case ItemResourceType.Iron_G:
                    res_iron = resource;
                    break;
                case ItemResourceType.Tin:
                    res_Tin = resource;
                    break;
                case ItemResourceType.Cupper:
                    res_Cupper = resource;
                    break;
                case ItemResourceType.Lead:
                    res_Lead = resource;
                    break;
                case ItemResourceType.Silver:
                    res_Silver = resource;
                    break;
                case ItemResourceType.RawMithril:
                    res_RawMithril = resource;
                    break;
                case ItemResourceType.Sulfur:
                    res_Sulfur = resource;
                    break;
                case ItemResourceType.Steel:
                    res_Steel = resource;
                    break;
                case ItemResourceType.Bronze:
                    res_Bronze = resource;
                    break;
                case ItemResourceType.CastIron:
                    res_CastIron = resource;
                    break;
                case ItemResourceType.BloomeryIron:
                    res_BloomeryIron = resource;
                    break;
                case ItemResourceType.Mithril:
                    res_Mithril = resource;
                    break;
                case ItemResourceType.SharpStick:
                    res_sharpstick = resource;
                    break;
                case ItemResourceType.BronzeSword:
                    res_BronzeSword = resource;
                    break;
                case ItemResourceType.ShortSword:
                    res_shortsword = resource;
                    break;
                case ItemResourceType.Sword:
                    res_Sword = resource;
                    break;
                case ItemResourceType.LongSword:
                    res_LongSword = resource;
                    break;
                case ItemResourceType.HandSpear:
                    res_HandSpear = resource;
                    break;
                case ItemResourceType.MithrilSword:
                    res_MithrilSword = resource;
                    break;
                case ItemResourceType.Warhammer:
                    res_Warhammer = resource;
                    break;
                case ItemResourceType.TwoHandSword:
                    res_twohandsword = resource;
                    break;
                case ItemResourceType.KnightsLance:
                    res_knightslance = resource;
                    break;
                case ItemResourceType.SlingShot:
                    res_SlingShot = resource;
                    break;
                case ItemResourceType.ThrowingSpear:
                    res_ThrowingSpear = resource;
                    break;
                case ItemResourceType.Bow:
                    res_bow = resource;
                    break;
                case ItemResourceType.LongBow:
                    res_longbow = resource;
                    break;
                case ItemResourceType.Crossbow:
                    res_crossbow = resource;
                    break;
                case ItemResourceType.MithrilBow:
                    res_MithrilBow = resource;
                    break;
                case ItemResourceType.HandCannon:
                    res_HandCannon = resource;
                    break;
                case ItemResourceType.HandCulverin:
                    res_HandCulvertin = resource;
                    break;
                case ItemResourceType.Rifle:
                    res_Rifle = resource;
                    break;
                case ItemResourceType.Blunderbus:
                    res_Blunderbus = resource;
                    break;
                case ItemResourceType.Ballista:
                    res_ballista = resource;
                    break;
                case ItemResourceType.Manuballista:
                    res_Manuballista = resource;
                    break;
                case ItemResourceType.Catapult:
                    res_Catapult = resource;
                    break;
                case ItemResourceType.UN_BatteringRam:
                    res_BatteringRam = resource;
                    break;
                case ItemResourceType.SiegeCannonBronze:
                    res_SiegeCannonBronze = resource;
                    break;
                case ItemResourceType.ManCannonBronze:
                    res_ManCannonBronze = resource;
                    break;
                case ItemResourceType.SiegeCannonIron:
                    res_SiegeCannonIron = resource;
                    break;
                case ItemResourceType.ManCannonIron:
                    res_ManCannonIron = resource;
                    break;
                case ItemResourceType.PaddedArmor:
                    res_paddedArmor = resource;
                    break;
                case ItemResourceType.HeavyPaddedArmor:
                    res_HeavyPaddedArmor = resource;
                    break;
                case ItemResourceType.BronzeArmor:
                    res_BronzeArmor = resource;
                    break;
                case ItemResourceType.IronArmor:
                    res_mailArmor = resource;
                    break;
                case ItemResourceType.HeavyIronArmor:
                    res_heavyMailArmor = resource;
                    break;
                case ItemResourceType.LightPlateArmor:
                    res_LightPlateArmor = resource;
                    break;
                case ItemResourceType.FullPlateArmor:
                    res_FullPlateArmor = resource;
                    break;
                case ItemResourceType.NONE:
                case ItemResourceType.Gold:
                    // No action needed for these types
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public int SellCost(ItemResourceType itemResourceType)
        {
            TradeResource resource;
            switch (itemResourceType)
            {
                case ItemResourceType.HardWood:
                case ItemResourceType.SoftWood:
                    resource = tradeTemplate.wood;
                    break;
                case ItemResourceType.Stone_G:
                    resource = tradeTemplate.stone;
                    break;
                case ItemResourceType.Food_G:
                    resource = tradeTemplate.food;
                    break;
                case ItemResourceType.Iron_G:
                    resource = tradeTemplate.iron;
                    break;

                default:
                    throw new NotImplementedException();
            }

            int goldCost = (int)Math.Ceiling( ItemPropertyColl.CarryAmount(itemResourceType) * resource.price);

            return goldCost;
        }

        public ItemResource MakeTrade(ItemResourceType itemResourceType, int payment, float maxWeight = 1f)
        {
            int carry = ItemPropertyColl.CarryAmount(itemResourceType, maxWeight);
            switch (itemResourceType)
            {
                case ItemResourceType.SoftWood:
                    res_wood.amount -= carry;
                    break;
                case ItemResourceType.Stone_G:
                    res_stone.amount -= carry;
                    break;
                case ItemResourceType.Food_G:
                    res_food.amount -= carry;
                    break;
                case ItemResourceType.Iron_G:
                    res_iron.amount -= carry;
                    break;

                default:
                    throw new NotImplementedException();
            }

            return new ItemResource(itemResourceType, 1, payment, carry);
        }

        public void dropOffItem(ItemResource item, out ItemResource convert1, out ItemResource convert2)
        {
            convert1 = item;
            convert2 = ItemResource.Empty;

            switch (item.type)
            {
                case ItemResourceType.DryWood:
                case ItemResourceType.SoftWood:
                case  ItemResourceType.HardWood:
                    convert1.type = ItemResourceType.Wood_Group;
                    break;
                
                case ItemResourceType.Coal:
                    convert1.type = ItemResourceType.Fuel_G;
                    break;

                case ItemResourceType.Wheat:
                    convert1.type = ItemResourceType.RawFood_Group;
                    convert1.amount = DssConst.WheatFoodAmount;
                    break;

                case ItemResourceType.Egg:                                   
                case ItemResourceType.Hen:
                    convert1.type = ItemResourceType.RawFood_Group;
                    convert1.amount = DssConst.HenRawFoodAmout;
                    animalResourceBonus(ref item);
                    break;

                case ItemResourceType.Pig:
                    convert1.type = ItemResourceType.RawFood_Group;
                    convert1.amount = DssConst.PigRawFoodAmout;
                    animalResourceBonus(ref item);

                    convert2 = new ItemResource(ItemResourceType.SkinLinen_Group, 1, 1, DssConst.PigSkinAmount);
                    break;

                case ItemResourceType.Linen:
                    convert1.type = ItemResourceType.SkinLinen_Group;
                    convert1.amount = DssConst.LinenHarvestAmount;
                    break;

                case ItemResourceType.Rapeseed:
                    convert1.type = ItemResourceType.Fuel_G;
                    convert1.amount = DssConst.RapeSeedFuelAmount;
                    break;

                case ItemResourceType.Hemp:
                    convert1.type = ItemResourceType.SkinLinen_Group;
                    convert1.amount = DssConst.HempLinenAndFuelAmount;

                    convert2.type = ItemResourceType.Fuel_G;
                    convert2.amount = DssConst.HempLinenAndFuelAmount;
                    break;

                case ItemResourceType.GoldOre:
                    {
                        var price = convert1.amount * DssConst.GoldOreSellValue;
                        faction.gold += price;
                        soldResources.add(price);

                        convert1.type = ItemResourceType.Gold;
                        convert1.amount = price;
                    }
                    break;
            }

            if (Ref.rnd.Chance(DssRef.difficulty.resourceMultiplyChance) &&
                faction.player.IsAi())
            {
                if (DssRef.difficulty.resourceMultiplyDecrease)
                {
                    return;
                }
                else
                {
                    convert1.amount *= 2;
                    convert2.amount *= 2;
                }
            }

            AddGroupedResource(convert1.type, convert1.amount);
            if (convert2.amount > 0)
            {
                AddGroupedResource(convert2.type, convert2.amount);
            }
        }

        void animalResourceBonus(ref ItemResource item)
        {
            if (Culture == CityCulture.AnimalBreeder)
            {
                item.amount *= 2;
            }
        }
        
        public void tradeTab()
        { 
            
        }

        public void blackMarketPurchase(ItemResourceType resourceType, int count, int cost)
        {
            if (faction.payMoney(cost * count, false))
            {
                AddGroupedResource(resourceType, count);
            }
        }
    }   


    struct GroupedResource
    {
        public int amount;
        //public int backOrder;
        public int goalBuffer;
        //public int orderQueCount;
        public int deliverCount;

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(amount);
            w.Write((ushort)goalBuffer);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            amount = r.ReadInt32();
            goalBuffer = r.ReadUInt16();
        }

        //public int freeAmount()
        //{ 
        //    return amount - backOrder;
        //}

        public bool needMore()
        {
            return amount < goalBuffer;
        }

        public bool needToImport()
        {
            return amount < goalBuffer;
        }

        public bool canTradeAway()
        {
            return amount >= goalBuffer;
        }

        public int amountPlusDelivery()
        {
            return amount + deliverCount;
        }

        public void add(ItemResource item, int multiply = 1)
        {
            amount += item.amount * multiply;
        }

        public void toMenu(RichBoxContent content, ItemResourceType item, bool safeGuard, ref bool reachedBuffer)
        {
            content.newLine();
            
            content.Add(new RichBoxImage(ResourceLib.Icon(item)));
            content.space();
            content.Add(new RichBoxText( LangLib.Item(item) + ": " + TextLib.LargeNumber(amount)));

            if (item != ItemResourceType.Water_G && 
                item != ItemResourceType.Gold &&
                item != ItemResourceType.Men)
            {
                bool reached = amount >= goalBuffer;
                reachedBuffer |= reached;
                SpriteName stockIcon;
                if (safeGuard)
                {
                    stockIcon = SpriteName.WarsStockpileAdd_Protected;
                }
                else if (reached)
                {
                    stockIcon = SpriteName.WarsStockpileStop;
                }
                else
                {
                    stockIcon = SpriteName.WarsStockpileAdd;
                }
                var icon = new RichBoxImage(stockIcon);
                content.Add(icon);
            }
            
        }

        public static void BufferIconInfo(RichBoxContent content, bool safeguard)
        {
            content.newLine();
            SpriteName sprite;
            string textstring;
            if (safeguard)
            {
                sprite = SpriteName.WarsStockpileAdd_Protected;
                textstring = DssRef.lang.Resource_FoodSafeGuard_Active;
            }
            else
            {
                sprite = SpriteName.WarsStockpileStop;
                textstring = DssRef.lang.Resource_ReachedStockpile;
            }


            var icon = new RichBoxImage(sprite);
            content.Add(icon);

            var text = new RichBoxText(": " + textstring);
            text.overrideColor = HudLib.InfoYellow_Light;
            content.Add(text);
        }

        //public void clearOrders()
        //{ 
        //    backOrder = 0;
        //    //orderQueCount = 0;
        //}

        public override string ToString()
        {
            return $"Grouped resource {amount}/{goalBuffer}";
        }
    }

    
}
