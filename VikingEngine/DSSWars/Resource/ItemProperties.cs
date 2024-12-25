using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;

namespace VikingEngine.DSSWars.Resource
{
    static class ItemPropertyColl
    {
        public const int CarryStones = 5;
        public const int CarryFood = 20;
        public static ItemProperties[] items;

        static float DefaultWeight = 1f / 30;

        public static void Init()
        {
            items = new ItemProperties[(int)ItemResourceType.NUM];
                        
            items[(int)ItemResourceType.HardWood] = new ItemProperties(1f / 20, null, null);
            items[(int)ItemResourceType.SoftWood] = new ItemProperties(1f / 30, null, null);
            items[(int)ItemResourceType.DryWood] = new ItemProperties(1f / 60, null, null);

            items[(int)ItemResourceType.Stone_G] = new ItemProperties(1f / CarryStones, null, null);
            items[(int)ItemResourceType.IronOre_G] = new ItemProperties(1f / 10, null, null);
            items[(int)ItemResourceType.GoldOre] = new ItemProperties(1f / 10, null, null);
            //items[(int)ItemResourceType.Iron_G] = new ItemProperties(1f / 5, null, null);
            items[(int)ItemResourceType.Egg] = new ItemProperties(1f / 60, null, null);
            items[(int)ItemResourceType.Pig] = new ItemProperties(1f, null, null);
            items[(int)ItemResourceType.Hen] = new ItemProperties(1f / 4, null, null);
            items[(int)ItemResourceType.Wheat] = new ItemProperties(1f / 10, null, null);
            items[(int)ItemResourceType.Linen] = new ItemProperties(1f / 10, null, null);
            
            items[(int)ItemResourceType.Fuel_G] = new ItemProperties(DefaultWeight, CraftResourceLib.Fuel1, null);
            items[(int)ItemResourceType.Coal] = new ItemProperties(DefaultWeight, CraftResourceLib.Charcoal, null);
            items[(int)ItemResourceType.Food_G] = new ItemProperties(1f / CarryFood, CraftResourceLib.Food1, CraftResourceLib.Food2);
            items[(int)ItemResourceType.Beer] = new ItemProperties(DefaultWeight, CraftResourceLib.Beer, null);
            items[(int)ItemResourceType.CoolingFluid] = new ItemProperties(DefaultWeight, CraftResourceLib.CoolingFluid, null);

            items[(int)ItemResourceType.Copper] = new ItemProperties(DefaultWeight, CraftResourceLib.Copper, CraftResourceLib.Cupper_AndCooling);
            items[(int)ItemResourceType.Tin] = new ItemProperties(DefaultWeight, CraftResourceLib.Tin, null);
            items[(int)ItemResourceType.Lead] = new ItemProperties(DefaultWeight, CraftResourceLib.Lead, null);
            items[(int)ItemResourceType.Iron_G] = new ItemProperties(DefaultWeight, CraftResourceLib.Iron, CraftResourceLib.Iron_AndCooling);
            items[(int)ItemResourceType.Silver] = new ItemProperties(DefaultWeight, CraftResourceLib.Silver, CraftResourceLib.Silver_AndCooling);
            items[(int)ItemResourceType.Bronze] = new ItemProperties(DefaultWeight, CraftResourceLib.Bronze, null);
            items[(int)ItemResourceType.CastIron] = new ItemProperties(DefaultWeight, CraftResourceLib.CastIron, null);
            items[(int)ItemResourceType.BloomeryIron] = new ItemProperties(DefaultWeight, CraftResourceLib.BloomeryIron, null);
            items[(int)ItemResourceType.Steel] = new ItemProperties(DefaultWeight, CraftResourceLib.Steel, CraftResourceLib.Steel_AndCooling);
            items[(int)ItemResourceType.Mithril] = new ItemProperties(DefaultWeight, CraftResourceLib.Mithril, null);
            

            items[(int)ItemResourceType.PaddedArmor] = new ItemProperties(DefaultWeight, CraftResourceLib.PaddedArmor, null);
            items[(int)ItemResourceType.HeavyPaddedArmor] = new ItemProperties(DefaultWeight, CraftResourceLib.HeavyPaddedArmor, null);
            items[(int)ItemResourceType.BronzeArmor] = new ItemProperties(DefaultWeight, CraftResourceLib.BronzeArmor, null);
            items[(int)ItemResourceType.IronArmor] = new ItemProperties(DefaultWeight, CraftResourceLib.MailArmor, null);
            items[(int)ItemResourceType.HeavyIronArmor] = new ItemProperties(DefaultWeight, CraftResourceLib.HeavyMailArmor, null);
            items[(int)ItemResourceType.LightPlateArmor] = new ItemProperties(DefaultWeight, CraftResourceLib.PlateArmor, null);
            items[(int)ItemResourceType.FullPlateArmor] = new ItemProperties(DefaultWeight, CraftResourceLib.FullPlateArmor, null);
            items[(int)ItemResourceType.MithrilArmor] = new ItemProperties(DefaultWeight, CraftResourceLib.MithrilArmor, null);

            items[(int)ItemResourceType.Toolkit] = new ItemProperties(DefaultWeight, CraftResourceLib.Beer, null);
            items[(int)ItemResourceType.Wagon2Wheel] = new ItemProperties(DefaultWeight, CraftResourceLib.WagonLight, null);
            items[(int)ItemResourceType.Wagon4Wheel] = new ItemProperties(DefaultWeight, CraftResourceLib.WagonHeavy, null);
            items[(int)ItemResourceType.BlackPowder] = new ItemProperties(DefaultWeight, CraftResourceLib.BlackPowder, null);
            items[(int)ItemResourceType.GunPowder] = new ItemProperties(DefaultWeight, CraftResourceLib.GunPowder, null);
            items[(int)ItemResourceType.LedBullet] = new ItemProperties(DefaultWeight, CraftResourceLib.LedBullets, null);

            items[(int)ItemResourceType.SharpStick] = new ItemProperties(DefaultWeight, CraftResourceLib.SharpStick, null);
            items[(int)ItemResourceType.BronzeSword] = new ItemProperties(DefaultWeight, CraftResourceLib.BronzeSword, null);
            items[(int)ItemResourceType.ShortSword] = new ItemProperties(DefaultWeight, CraftResourceLib.ShortSword, null);
            items[(int)ItemResourceType.Sword] = new ItemProperties(DefaultWeight, CraftResourceLib.Sword, null);
            items[(int)ItemResourceType.LongSword] = new ItemProperties(DefaultWeight, CraftResourceLib.LongSword, null);
            items[(int)ItemResourceType.HandSpear] = new ItemProperties(DefaultWeight, CraftResourceLib.HandSpearIron, CraftResourceLib.HandSpearBronze);
            items[(int)ItemResourceType.MithrilSword] = new ItemProperties(DefaultWeight, CraftResourceLib.MithrilSword, null);

            items[(int)ItemResourceType.Warhammer] = new ItemProperties(DefaultWeight, CraftResourceLib.WarhammerIron, CraftResourceLib.WarhammerBronze);
            items[(int)ItemResourceType.TwoHandSword] = new ItemProperties(DefaultWeight, CraftResourceLib.TwoHandSword, null);
            items[(int)ItemResourceType.KnightsLance] = new ItemProperties(DefaultWeight, CraftResourceLib.KnightsLance, null);

            items[(int)ItemResourceType.SlingShot] = new ItemProperties(DefaultWeight, CraftResourceLib.Slingshot, null);
            items[(int)ItemResourceType.ThrowingSpear] = new ItemProperties(DefaultWeight, CraftResourceLib.ThrowingSpear1, CraftResourceLib.ThrowingSpear2);
            items[(int)ItemResourceType.Bow] = new ItemProperties(DefaultWeight, CraftResourceLib.Bow, null);
            items[(int)ItemResourceType.LongBow] = new ItemProperties(DefaultWeight, CraftResourceLib.LongBow, null);
            items[(int)ItemResourceType.Crossbow] = new ItemProperties(DefaultWeight, CraftResourceLib.CrossBow, null);
            items[(int)ItemResourceType.MithrilBow] = new ItemProperties(DefaultWeight, CraftResourceLib.MithrilBow, null);

            items[(int)ItemResourceType.HandCannon] = new ItemProperties(DefaultWeight, CraftResourceLib.BronzeHandCannon, null);
            items[(int)ItemResourceType.HandCulverin] = new ItemProperties(DefaultWeight, CraftResourceLib.BronzeHandCulverin, null);
            items[(int)ItemResourceType.Rifle] = new ItemProperties(DefaultWeight, CraftResourceLib.Rifle, null);
            items[(int)ItemResourceType.Blunderbus] = new ItemProperties(DefaultWeight, CraftResourceLib.Blunderbus, null);

            items[(int)ItemResourceType.Ballista] = new ItemProperties(DefaultWeight, CraftResourceLib.Ballista_Iron, CraftResourceLib.Ballista_Bronze);
            items[(int)ItemResourceType.Manuballista] = new ItemProperties(DefaultWeight, CraftResourceLib.ManuBallista, null);
            items[(int)ItemResourceType.Catapult] = new ItemProperties(DefaultWeight, CraftResourceLib.Catapult, null);
            items[(int)ItemResourceType.SiegeCannonBronze] = new ItemProperties(DefaultWeight, CraftResourceLib.SiegeCannonBronze, null);
            items[(int)ItemResourceType.ManCannonBronze] = new ItemProperties(DefaultWeight, CraftResourceLib.ManCannonBronze, null);
            items[(int)ItemResourceType.SiegeCannonIron] = new ItemProperties(DefaultWeight, CraftResourceLib.SiegeCannonIron, null);
            items[(int)ItemResourceType.ManCannonIron] = new ItemProperties(DefaultWeight, CraftResourceLib.ManCannonIron, null);

            items[(int)ItemResourceType.CupperCoin] = new ItemProperties(DefaultWeight, ResourceLib.CupperCoin, null);
            items[(int)ItemResourceType.BronzeCoin] = new ItemProperties(DefaultWeight, ResourceLib.BronzeCoin, null);
            items[(int)ItemResourceType.SilverCoin] = new ItemProperties(DefaultWeight, ResourceLib.SilverCoin, null);
            items[(int)ItemResourceType.ElfCoin] = new ItemProperties(DefaultWeight, ResourceLib.ElfCoin, null);


            for (int i = 0; i < items.Length; ++i)
            {
                if (items[i] == null)
                { 
                    items[i] = new ItemProperties(DefaultWeight, null, null);
                }
            }
        }

        public static int CarryAmount(ItemResourceType item, float maxWeight = 1f)
        {
            int carry = Convert.ToInt32(maxWeight / items[(int)item].weight);
            return carry;
        }

        public static void Blueprint(ItemResourceType item, out CraftBlueprint bp1, out CraftBlueprint bp2)
        {
            var properties = items[(int)item];
            bp1 = properties.bp1;
            bp2 = properties.bp2;
        }
    }

    class ItemProperties
    {
        /// <summary>
        /// Weight is measured in man-carry, 1 is a standard carry weight for a worker
        /// </summary>
        public float weight;
        public CraftBlueprint bp1;
        public CraftBlueprint bp2;

        public ItemProperties(float weight, CraftBlueprint bp1, CraftBlueprint bp2)
        {
            this.weight = weight;
            this.bp1 = bp1;
            this.bp2 = bp2;
        }
    }
}
