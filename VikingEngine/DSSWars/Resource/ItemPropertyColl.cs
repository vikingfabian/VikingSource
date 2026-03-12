using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Work;

namespace VikingEngine.DSSWars.Resource
{
    static class ItemPropertyColl
    {
        public const int CarryStones = 5;
        public const int CarryBricks = 10;
        public const int CarryFood = 20;
        public const int CarryConservedFood = CarryFood * 2;
        public static ItemProperties[] items;

        public const int DefaultCarry = 30;
        static float DefaultWeight = 1f / DefaultCarry;

        public static float ArmyFoodOrderSize;

        public static void Init()
        {
            ArmyFoodOrderSize = ItemPropertyColl.CarryFood * DssConst.Worker_TrossWorkerCarryWeight;
            const int NoCityResource = -1;

            items = new ItemProperties[(int)ItemResourceType.NUM];

            //men
            new ItemProperties(ItemResourceType.Men, NoCityResource, 0, WorkPriorityType.NUM_NONE, null, null, StorageType.NUM_NONE).AddItemSource(new ItemSource(Build.BuildAndExpandType.WorkerHut));
            new ItemProperties(ItemResourceType.NobelMen, NoCityResource, 0, WorkPriorityType.NUM_NONE, null, null, StorageType.NUM_NONE).AddItemSource(new ItemSource(Build.BuildAndExpandType.Nobelhouse));

            // wood variants
            new ItemProperties(ItemResourceType.HardWood, CityResoureIndex.wood, 1f / 20, WorkPriorityType.NUM_NONE, null, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SoftWood, CityResoureIndex.wood, 1f / 30, WorkPriorityType.NUM_NONE, null, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.DryWood, CityResoureIndex.wood, 1f / 60, WorkPriorityType.NUM_NONE, null, null, StorageType.NUM_NONE);

            new ItemProperties(ItemResourceType.Wood_Group, CityResoureIndex.wood, DefaultWeight, WorkPriorityType.NUM_NONE, null, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(Map.TerrainSubFoilType.TreeSoft));
            new ItemProperties(ItemResourceType.Clay, CityResoureIndex.Clay, DefaultWeight, WorkPriorityType.collectClay, null, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(Map.TerrainSubFoilType.ClayPit));

            // basic resources
            new ItemProperties(ItemResourceType.Stone_G, CityResoureIndex.stone, 1f / CarryStones, WorkPriorityType.stone, null, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(Map.TerrainSubFoilType.Stones));
            new ItemProperties(ItemResourceType.Brick, CityResoureIndex.Brick, 1f / CarryBricks, WorkPriorityType.craftBrick, CraftResourceLib.Brick, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(Map.TerrainMineType.StoneBlock));
            new ItemProperties(ItemResourceType.Egg, CityResoureIndex.rawFood, 1f / 60, WorkPriorityType.craftFood, null, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.Wheat, CityResoureIndex.rawFood, 1f / 10, WorkPriorityType.craftFood, null, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.Meat, CityResoureIndex.Meat, DefaultWeight, WorkPriorityType.craftFood, null, null, StorageType.NUM_NONE);

            new ItemProperties(ItemResourceType.RawFood_Group, CityResoureIndex.rawFood, DefaultWeight, WorkPriorityType.NUM_NONE, null, null, StorageType.FoodStorage).AddItemSource(
                new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.WheatFarm), new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.HenPen), new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.PigPen));

            new ItemProperties(ItemResourceType.Linen, CityResoureIndex.skinLinnen, 1f / 10, WorkPriorityType.farmlinen, null, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.SkinLinen_Group, CityResoureIndex.skinLinnen, 1f / 10, WorkPriorityType.farmlinen, null, null, StorageType.MaterialStorage).AddItemSource(
                new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.LinenFarm), new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.PigPen));
            new ItemProperties(ItemResourceType.WoodContainer, CityResoureIndex.Container, DefaultWeight, WorkPriorityType.craftContainer, CraftResourceLib.Container_wood, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.PotContainer, CityResoureIndex.Container, DefaultWeight, WorkPriorityType.craftContainer, CraftResourceLib.Container_clay, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.Container, CityResoureIndex.Container, DefaultWeight, WorkPriorityType.craftContainer, CraftResourceLib.Container_wood, CraftResourceLib.Container_clay, StorageType.MaterialStorage);

            // fuel & food
            new ItemProperties(ItemResourceType.Fuel_G, CityResoureIndex.fuel, DefaultWeight, WorkPriorityType.craftFuel, CraftResourceLib.Fuel1, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(ItemSourceType.Crafting, Build.BuildAndExpandType.CoalPit), new ItemSource(Map.TerrainMineType.Coal));
            new ItemProperties(ItemResourceType.Coal, CityResoureIndex.fuel, DefaultWeight, WorkPriorityType.miningCoal, CraftResourceLib.Charcoal, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.Salt, CityResoureIndex.Salt, DefaultWeight, WorkPriorityType.miningSalt, null, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(Map.TerrainMineType.Salt), new ItemSource(ItemSourceType.Crafting, Build.BuildAndExpandType.DryingPan));
            new ItemProperties(ItemResourceType.Food_G, CityResoureIndex.food, 1f / CarryFood, WorkPriorityType.craftFood, CraftResourceLib.Food1, CraftResourceLib.Food2, StorageType.FoodStorage).AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.OrchardApple));
            new ItemProperties(ItemResourceType.ConservedFood, CityResoureIndex.ConservedFood, 1f / CarryConservedFood, WorkPriorityType.craftConservedFood, CraftResourceLib.ConservedFood_Barrel, CraftResourceLib.ConservedFood_Smoked, StorageType.FoodStorage).AddItemSource(new ItemSource(Build.BuildAndExpandType.Cook), new ItemSource(Build.BuildAndExpandType.Smoker), new ItemSource(Build.BuildAndExpandType.Dryer));
            new ItemProperties(ItemResourceType.Beer, CityResoureIndex.beer, DefaultWeight, WorkPriorityType.craftBeer, CraftResourceLib.Beer, null, StorageType.FoodStorage);
            new ItemProperties(ItemResourceType.CoolingFluid, CityResoureIndex.coolingfluid, DefaultWeight, WorkPriorityType.craftCoolingFluid, CraftResourceLib.CoolingFluid, null, StorageType.MaterialStorage);

            // metals & alloys
            new ItemProperties(ItemResourceType.IronOre_G, CityResoureIndex.ironore, 1f / 10, WorkPriorityType.miningIron, null, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(Map.TerrainMineType.IronOre));
            new ItemProperties(ItemResourceType.TinOre, CityResoureIndex.TinOre, 1f / 10, WorkPriorityType.miningTin, null, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(Map.TerrainMineType.TinOre));
            new ItemProperties(ItemResourceType.CopperOre, CityResoureIndex.CopperOre, 1f / 10, WorkPriorityType.miningCopper, null, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(Map.TerrainMineType.CopperOre));
            new ItemProperties(ItemResourceType.LeadOre, CityResoureIndex.LeadOre, 1f / 10, WorkPriorityType.miningLead, null, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(Map.TerrainMineType.LeadOre));
            new ItemProperties(ItemResourceType.SilverOre, CityResoureIndex.SilverOre, 1f / 10, WorkPriorityType.miningSilver, null, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(Map.TerrainMineType.SilverOre));
            new ItemProperties(ItemResourceType.GoldOre, CityResoureIndex.GoldOre, 1f / 10, WorkPriorityType.miningGold, null, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(Map.TerrainMineType.GoldOre));

            new ItemProperties(ItemResourceType.Iron_G, CityResoureIndex.iron, DefaultWeight, WorkPriorityType.smeltIron, CraftResourceLib.Iron, CraftResourceLib.Iron_AndCooling, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.Tin, CityResoureIndex.Tin, DefaultWeight, WorkPriorityType.smeltTin, CraftResourceLib.Tin, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.Copper, CityResoureIndex.Copper, DefaultWeight, WorkPriorityType.smeltCopper, CraftResourceLib.Copper, CraftResourceLib.Cupper_AndCooling, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.Lead, CityResoureIndex.Lead, DefaultWeight, WorkPriorityType.smeltLead, CraftResourceLib.Lead, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.RawMithril, CityResoureIndex.RawMithril, DefaultWeight, WorkPriorityType.miningMithril, null, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(Map.TerrainMineType.Mithril));
            new ItemProperties(ItemResourceType.Sulfur, CityResoureIndex.Sulfur, DefaultWeight, WorkPriorityType.miningSulfur, null, null, StorageType.MaterialStorage).AddItemSource(new ItemSource(Map.TerrainMineType.Sulfur));

            new ItemProperties(ItemResourceType.Silver, CityResoureIndex.Silver, DefaultWeight, WorkPriorityType.smeltSilver, CraftResourceLib.Silver, CraftResourceLib.Silver_AndCooling, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.Gold, NoCityResource, DefaultWeight, WorkPriorityType.smeltGold, Minting.ConvertGoldOre, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.Bronze, CityResoureIndex.Bronze, DefaultWeight, WorkPriorityType.craftBronze, CraftResourceLib.Bronze, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.CastIron, CityResoureIndex.CastIron, DefaultWeight, WorkPriorityType.craftCastIron, CraftResourceLib.CastIron, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.BloomeryIron, CityResoureIndex.BloomeryIron, DefaultWeight, WorkPriorityType.craftBloomeryIron, CraftResourceLib.BloomeryIron, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.Steel, CityResoureIndex.Steel, DefaultWeight, WorkPriorityType.craftSteel, CraftResourceLib.Steel, CraftResourceLib.Steel_AndCooling, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.Mithril, CityResoureIndex.Mithril, DefaultWeight, WorkPriorityType.craftMithril, CraftResourceLib.Mithril, null, StorageType.MaterialStorage);

            // --- Human Armor ---
            new ItemProperties(ItemResourceType.PaddedArmor, CityResoureIndex.paddedArmor, DefaultWeight, WorkPriorityType.craftPaddedArmor, CraftResourceLib.PaddedArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.HeavyPaddedArmor, CityResoureIndex.HeavyPaddedArmor, DefaultWeight, WorkPriorityType.craftHeavyPaddedArmor, CraftResourceLib.HeavyPaddedArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.BronzeArmor, CityResoureIndex.BronzeArmor, DefaultWeight, WorkPriorityType.craftBronzeArmor, CraftResourceLib.BronzeArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.IronArmor, CityResoureIndex.mailArmor, DefaultWeight, WorkPriorityType.craftMailArmor, CraftResourceLib.MailArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.HeavyIronArmor, CityResoureIndex.heavyMailArmor, DefaultWeight, WorkPriorityType.craftHeavyMailArmor, CraftResourceLib.HeavyMailArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.LightPlateArmor, CityResoureIndex.LightPlateArmor, DefaultWeight, WorkPriorityType.craftPlateArmor, CraftResourceLib.PlateArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.FullPlateArmor, CityResoureIndex.FullPlateArmor, DefaultWeight, WorkPriorityType.craftFullPlateArmor, CraftResourceLib.FullPlateArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.MithrilArmor, CityResoureIndex.MithrilArmor, DefaultWeight, WorkPriorityType.craftMithrilArmor, CraftResourceLib.MithrilArmor, null, StorageType.ArmorStorage);

            // --- Mount Armor ---
            new ItemProperties(ItemResourceType.MountPaddedArmor, CityResoureIndex.MountPaddedArmor, DefaultWeight, WorkPriorityType.craftMountPaddedArmor, CraftResourceLib.MountPaddedArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.MountHeavyPaddedArmor, CityResoureIndex.MountHeavyPaddedArmor, DefaultWeight, WorkPriorityType.craftMountHeavyPaddedArmor, CraftResourceLib.MountHeavyPaddedArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.MountBronzeArmor, CityResoureIndex.MountBronzeArmor, DefaultWeight, WorkPriorityType.craftMountBronzeArmor, CraftResourceLib.MountBronzeArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.MountIronArmor, CityResoureIndex.MountIronArmor, DefaultWeight, WorkPriorityType.craftMountMailArmor, CraftResourceLib.MountIronArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.MountHeavyIronArmor, CityResoureIndex.MountHeavyIronArmor, DefaultWeight, WorkPriorityType.craftMountHeavyMailArmor, CraftResourceLib.MountHeavyIronArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.MountLightPlateArmor, CityResoureIndex.MountLightPlateArmor, DefaultWeight, WorkPriorityType.craftMountPlateArmor, CraftResourceLib.MountLightPlateArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.MountFullPlateArmor, CityResoureIndex.MountFullPlateArmor, DefaultWeight, WorkPriorityType.craftMountFullPlateArmor, CraftResourceLib.MountFullPlateArmor, null, StorageType.ArmorStorage);
            new ItemProperties(ItemResourceType.MountMithrilArmor, CityResoureIndex.MountMithrilArmor, DefaultWeight, WorkPriorityType.craftMountMithrilArmor, CraftResourceLib.MountMithrilArmor, null, StorageType.ArmorStorage);

            // --- Shields ---
            new ItemProperties(ItemResourceType.BucklerShield, CityResoureIndex.BucklerShield, DefaultWeight, WorkPriorityType.craftBucklerShield, CraftResourceLib.BucklerShield, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.RoundShield, CityResoureIndex.RoundShield, DefaultWeight, WorkPriorityType.craftRoundShield, CraftResourceLib.RoundShield, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.HeaterShield, CityResoureIndex.HeaterShield, DefaultWeight, WorkPriorityType.craftHeaterShield, CraftResourceLib.HeaterShield, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.TowerShield, CityResoureIndex.TowerShield, DefaultWeight, WorkPriorityType.craftTowerShield, CraftResourceLib.TowerShield, null, StorageType.WeaponStorage);

            // --- Buildings & Tools ---
            new ItemProperties(ItemResourceType.Palisade, CityResoureIndex.Palisade, DefaultWeight, WorkPriorityType.craftPalisade, CraftResourceLib.Palisade, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.Toolkit, CityResoureIndex.Toolkit, DefaultWeight, WorkPriorityType.craftToolkit, CraftResourceLib.Toolkit, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.Wagon2Wheel, CityResoureIndex.Wagon2Wheel, DefaultWeight, WorkPriorityType.craftWagon2Wheel, CraftResourceLib.Wagon2Wheel, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.Wagon4Wheel, CityResoureIndex.Wagon4Wheel, DefaultWeight, WorkPriorityType.craftWagon4Wheel, CraftResourceLib.Wagon4Wheel, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.WagonClosed, CityResoureIndex.WagonClosed, DefaultWeight, WorkPriorityType.craftWagonClosed, CraftResourceLib.WagonClosed, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.WagonIron, CityResoureIndex.WagonIron, DefaultWeight, WorkPriorityType.craftWagonIron, CraftResourceLib.WagonIron, null, StorageType.MaterialStorage);
            new ItemProperties(ItemResourceType.WagonSteel, CityResoureIndex.WagonSteel, DefaultWeight, WorkPriorityType.craftWagonSteel, CraftResourceLib.WagonSteel, null, StorageType.MaterialStorage);

            // --- Gunpowder & Ballistics ---
            new ItemProperties(ItemResourceType.BlackPowder, CityResoureIndex.BlackPowder, DefaultWeight, WorkPriorityType.craftBlackPowder, CraftResourceLib.BlackPowder, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.GunPowder, CityResoureIndex.GunPowder, DefaultWeight, WorkPriorityType.craftGunPowder, CraftResourceLib.GunPowder, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.LedBullet, CityResoureIndex.LedBullet, DefaultWeight, WorkPriorityType.craftBullet, CraftResourceLib.LedBullets, null, StorageType.WeaponStorage);

            // --- Melee Weapons ---
            new ItemProperties(ItemResourceType.SharpStick, CityResoureIndex.sharpstick, DefaultWeight, WorkPriorityType.craftSharpStick, CraftResourceLib.SharpStick, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.BronzeSword, CityResoureIndex.BronzeSword, DefaultWeight, WorkPriorityType.craftBronzeSword, CraftResourceLib.BronzeSword, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.ShortSword, CityResoureIndex.shortsword, DefaultWeight, WorkPriorityType.craftShortSword, CraftResourceLib.ShortSword, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.Sword, CityResoureIndex.Sword, DefaultWeight, WorkPriorityType.craftSword, CraftResourceLib.Sword, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.LongSword, CityResoureIndex.LongSword, DefaultWeight, WorkPriorityType.craftLongSword, CraftResourceLib.LongSword, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.HandSpear, CityResoureIndex.HandSpear, DefaultWeight, WorkPriorityType.craftHandSpear, CraftResourceLib.HandSpearIron, CraftResourceLib.HandSpearBronze, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.MithrilSword, CityResoureIndex.MithrilSword, DefaultWeight, WorkPriorityType.craftMithrilSword, CraftResourceLib.MithrilSword, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.Warhammer, CityResoureIndex.Warhammer, DefaultWeight, WorkPriorityType.craftWarhammer, CraftResourceLib.WarhammerIron, CraftResourceLib.WarhammerBronze, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.TwoHandSword, CityResoureIndex.twohandsword, DefaultWeight, WorkPriorityType.craftTwoHandSword, CraftResourceLib.TwoHandSword, null, StorageType.WeaponStorage);
            //new ItemProperties(ItemResourceType.KnightsLance, CityResoureIndex.knightslance, DefaultWeight, WorkPriorityType.craftKnightsLance, CraftResourceLib.KnightsLance, null);

            // --- Ranged Weapons ---
            new ItemProperties(ItemResourceType.SlingShot, CityResoureIndex.SlingShot, DefaultWeight, WorkPriorityType.craftSlingshot, CraftResourceLib.Slingshot, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.ThrowingSpear, CityResoureIndex.ThrowingSpear, DefaultWeight, WorkPriorityType.craftThrowingspear, CraftResourceLib.ThrowingSpear1, CraftResourceLib.ThrowingSpear2, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.Bow, CityResoureIndex.bow, DefaultWeight, WorkPriorityType.craftBow, CraftResourceLib.Bow, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.LongBow, CityResoureIndex.longbow, DefaultWeight, WorkPriorityType.craftLongbow, CraftResourceLib.LongBow, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.Crossbow, CityResoureIndex.crossbow, DefaultWeight, WorkPriorityType.craftCrossbow, CraftResourceLib.CrossBow, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.MithrilBow, CityResoureIndex.MithrilBow, DefaultWeight, WorkPriorityType.craftMithrilbow, CraftResourceLib.MithrilBow, null, StorageType.WeaponStorage);

            // --- Firearms ---
            new ItemProperties(ItemResourceType.HandCannon, CityResoureIndex.HandCannon, DefaultWeight, WorkPriorityType.craftHandCannon, CraftResourceLib.BronzeHandCannon, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.HandCulverin, CityResoureIndex.HandCulvertin, DefaultWeight, WorkPriorityType.craftHandCulverin, CraftResourceLib.BronzeHandCulverin, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.Rifle, CityResoureIndex.Rifle, DefaultWeight, WorkPriorityType.craftRifle, CraftResourceLib.Rifle, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.Blunderbuss, CityResoureIndex.Blunderbuss, DefaultWeight, WorkPriorityType.craftBlunderbuss, CraftResourceLib.Blunderbuss, null, StorageType.WeaponStorage);

            // --- Siege Engines ---
            new ItemProperties(ItemResourceType.Ballista, CityResoureIndex.ballista, DefaultWeight, WorkPriorityType.craftBallista, CraftResourceLib.Ballista_Iron, CraftResourceLib.Ballista_Bronze, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.Manuballista, CityResoureIndex.Manuballista, DefaultWeight, WorkPriorityType.craftManuBallista, CraftResourceLib.ManuBallista, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.Catapult, CityResoureIndex.Catapult, DefaultWeight, WorkPriorityType.craftCatapult, CraftResourceLib.Catapult, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.SiegeCannonBronze, CityResoureIndex.SiegeCannonBronze, DefaultWeight, WorkPriorityType.craftSiegeCannonBronze, CraftResourceLib.SiegeCannonBronze, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.ManCannonBronze, CityResoureIndex.ManCannonBronze, DefaultWeight, WorkPriorityType.craftManCannonBronze, CraftResourceLib.ManCannonBronze, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.SiegeCannonIron, CityResoureIndex.SiegeCannonIron, DefaultWeight, WorkPriorityType.craftSiegeCannonIron, CraftResourceLib.SiegeCannonIron, null, StorageType.WeaponStorage);
            new ItemProperties(ItemResourceType.ManCannonIron, CityResoureIndex.ManCannonIron, DefaultWeight, WorkPriorityType.craftManCannonIron, CraftResourceLib.ManCannonIron, null, StorageType.WeaponStorage);

            // --- Coins ---
            new ItemProperties(ItemResourceType.CopperCoin, NoCityResource, DefaultWeight, WorkPriorityType.coinmaker_copper, Minting.CopperCoin, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.BronzeCoin, NoCityResource, DefaultWeight, WorkPriorityType.coinmaker_bronze, Minting.BronzeCoin, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SilverCoin, NoCityResource, DefaultWeight, WorkPriorityType.coinmaker_silver, Minting.SilverCoin, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.ElfCoin, NoCityResource, DefaultWeight, WorkPriorityType.coinmaker_mithril, Minting.ElfCoin, null, StorageType.NUM_NONE);


            new ItemProperties(ItemResourceType.Boar, CityResoureIndex.Boar, DefaultWeight, WorkPriorityType.SlaughterBoar, null, null, StorageType.AnimalStorage)
                .AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.BoarPen));
            
            new ItemProperties(ItemResourceType.Fowl, CityResoureIndex.Fowl, DefaultWeight, WorkPriorityType.SlaughterFowl, null, null, StorageType.AnimalStorage)
                .AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.FowlPen));

            {
                var animal = new ItemProperties(ItemResourceType.Pig, CityResoureIndex.Pig, DefaultWeight, WorkPriorityType.SlaughterPig, null, null, StorageType.AnimalStorage);
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.PigPen));
                ref var soldier = ref animal.soldierData;
                soldier.attackDamage = DssConst.WeaponDamage_Pig;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;
                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.02f;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.factionColoredModel = false;
                soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 1.2f;
                soldier.basehealth = DssConst.Soldier_DefaultHealth;
            }

            new ItemProperties(ItemResourceType.Hen, CityResoureIndex.Hen, DefaultWeight, WorkPriorityType.SlaughterHen, null, null, StorageType.AnimalStorage)
                .AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.HenPen));

            // --- Oxen ---
            {
                var animal = new ItemProperties(ItemResourceType.Oxen, CityResoureIndex.Oxen, DefaultWeight, WorkPriorityType.SlaughterOxen, null, null, StorageType.AnimalStorage);
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.OxenPen));
                animal.wagonPull = WagonPull.All;
                animal.armorCarry = ArmorCarry.All;
            }
            {
                var animal = new ItemProperties(ItemResourceType.KineOxen, CityResoureIndex.KineOxen, DefaultWeight, WorkPriorityType.SlaughterKineOxen, null, null, StorageType.AnimalStorage);
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.KineOxenPen));
                animal.wagonPull = WagonPull.All;
                animal.armorCarry = ArmorCarry.All;
            }
            // --- Dogs ---
            {
                var animal = new ItemProperties(ItemResourceType.Dog, CityResoureIndex.Dog, DefaultWeight, WorkPriorityType.NUM_NONE, null, null, StorageType.AnimalStorage);
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.DogCage));
                ref var soldier = ref animal.soldierData;
                soldier.attackDamage = DssConst.WeaponDamage_Dog;
                soldier.attackDamageStructure = soldier.attackDamage / 4;
                soldier.attackDamageSea = soldier.attackDamage / 2;
                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.03f;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.factionColoredModel = false;
                soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 2f;
                soldier.basehealth = DssConst.DogHealth;
            }
            {
                var animal = new ItemProperties(ItemResourceType.Hound, CityResoureIndex.Hound, DefaultWeight, WorkPriorityType.NUM_NONE, null, null, StorageType.AnimalStorage);
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.HoundCage));
                ref var soldier = ref animal.soldierData;
                soldier.attackDamage = DssConst.WeaponDamage_Hound;
                soldier.attackDamageStructure = soldier.attackDamage / 4;
                soldier.attackDamageSea = soldier.attackDamage / 2;
                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.03f;                
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.factionColoredModel = false;
                soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 1.7f;
                soldier.basehealth = DssConst.HoundHealth;
            }
            // --- Horses ---
            {
                var animal = new ItemProperties(ItemResourceType.Pony, CityResoureIndex.Pony, DefaultWeight, WorkPriorityType.SlaughterPony, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.PonyPen));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.LightOnly;
                animal.armorCarry = ArmorCarry.All;
            }

            {
                var animal = new ItemProperties(ItemResourceType.Horse, CityResoureIndex.Horse, DefaultWeight, WorkPriorityType.SlaughterHorse, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.HorsePen));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.All;
                animal.armorCarry = ArmorCarry.All;
            }

            {
                var animal = new ItemProperties(ItemResourceType.WarHorse, CityResoureIndex.WarHorse, DefaultWeight, WorkPriorityType.SlaughterWarHorse, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.WarHorsePen));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.All;
                animal.armorCarry = ArmorCarry.All;
            }

            {
                var animal = new ItemProperties(ItemResourceType.DraftHorse, CityResoureIndex.DraftHorse, DefaultWeight, WorkPriorityType.SlaughterDraftHorse, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.DraftHorsePen));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.All;
                animal.armorCarry = ArmorCarry.All;
            }

            // --- Pigs / Hogs ---
            {
                var animal = new ItemProperties(ItemResourceType.WildPig, CityResoureIndex.WildPig, DefaultWeight, WorkPriorityType.SlaughterWildPig, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.WildPigPen));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.LightOnly;
                animal.armorCarry = ArmorCarry.All;
            }

            {
                var animal = new ItemProperties(ItemResourceType.WildHog, CityResoureIndex.WildHog, DefaultWeight, WorkPriorityType.SlaughterWildHog, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.WildHogPen));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.LightOnly;
                animal.armorCarry = ArmorCarry.All;
            }

            {
                var animal = new ItemProperties(ItemResourceType.WarHog, CityResoureIndex.WarHog, DefaultWeight, WorkPriorityType.SlaughterWarHog, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.WarHogPen));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.LightOnly;
                animal.armorCarry = ArmorCarry.All;
            }

            {
                var animal = new ItemProperties(ItemResourceType.StagHog, CityResoureIndex.StagHog, DefaultWeight, WorkPriorityType.SlaughterStagHog, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.StagHogPen));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.All;
                animal.armorCarry = ArmorCarry.All;
            }

            // --- Wolves ---
            {
                var animal = new ItemProperties(ItemResourceType.Wolf, CityResoureIndex.Wolf, DefaultWeight, WorkPriorityType.SlaughterWolf, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.WolfCage));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.LightOnly;
                animal.armorCarry = ArmorCarry.LightOnly;
            }

            {
                var animal = new ItemProperties(ItemResourceType.Warg, CityResoureIndex.Warg, DefaultWeight, WorkPriorityType.SlaughterWarg, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.WargCage));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.LightOnly;
                animal.armorCarry = ArmorCarry.All;
            }

            {
                var animal = new ItemProperties(ItemResourceType.AlphaWarg, CityResoureIndex.AlphaWarg, DefaultWeight, WorkPriorityType.SlaughterAlphaWarg, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.AlphaWargCage));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.LightOnly;
                animal.armorCarry = ArmorCarry.All;
            }

            // --- Cats ---
            {
                var animal = new ItemProperties(ItemResourceType.WildCat, CityResoureIndex.WildCat, DefaultWeight, WorkPriorityType.SlaughterWildCat, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.WildCatCage));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.LightOnly;
                animal.armorCarry = ArmorCarry.LightOnly;
            }

            {
                var animal = new ItemProperties(ItemResourceType.Lion, CityResoureIndex.Lion, DefaultWeight, WorkPriorityType.SlaughterLion, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.LionCage));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.LightOnly;
                animal.armorCarry = ArmorCarry.All;
            }

            {
                var animal = new ItemProperties(ItemResourceType.WarLion, CityResoureIndex.WarLion, DefaultWeight, WorkPriorityType.SlaughterWarLion, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.WarLionCage));
                animal.soldierData.CavalrySetup();
                animal.wagonPull = WagonPull.LightOnly;
                animal.armorCarry = ArmorCarry.All;
            }

            // --- Elephants ---
            {
                var animal = new ItemProperties(ItemResourceType.Elephant, CityResoureIndex.Elephant, 1f / 2, WorkPriorityType.SlaughterElephant, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.ElephantCage));
                animal.soldierData.ElephantSetup();
                animal.wagonPull = WagonPull.Balcon;
                animal.armorCarry = ArmorCarry.All;
            }

            {
                var animal = new ItemProperties(ItemResourceType.WarElephant, CityResoureIndex.WarElephant, 1f / 2, WorkPriorityType.SlaughterWarElephant, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.WarElephantCage));
                animal.soldierData.ElephantSetup();
                animal.wagonPull = WagonPull.Balcon;
                animal.armorCarry = ArmorCarry.All;
            }

            {
                var animal = new ItemProperties(ItemResourceType.Oliphant, CityResoureIndex.Oliphant, 1f, WorkPriorityType.SlaughterOliphant, null, null, StorageType.AnimalStorage)
                {
                    Filter_IsRidingAnimal = true,
                };
                animal.AddItemSource(new ItemSource(ItemSourceType.Farm, Build.BuildAndExpandType.OliphantCage));
                animal.soldierData.ElephantSetup();
                animal.wagonPull = WagonPull.Balcon;
                animal.armorCarry = ArmorCarry.All;
            }


            new ItemProperties(ItemResourceType.SlaughterHen, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterHen, CraftResourceLib.SlaughterHen, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterPig, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterPig, CraftResourceLib.SlaughterPig, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterOxen, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterOxen, CraftResourceLib.SlaughterOxen, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterKineOxen, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterKineOxen, CraftResourceLib.SlaughterKineOxen, null, StorageType.NUM_NONE);

            new ItemProperties(ItemResourceType.SlaughterPony, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterPony, CraftResourceLib.SlaughterPony, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterHorse, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterHorse, CraftResourceLib.SlaughterHorse, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterWarHorse, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterWarHorse, CraftResourceLib.SlaughterWarHorse, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterDraftHorse, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterDraftHorse, CraftResourceLib.SlaughterDraftHorse, null, StorageType.NUM_NONE);

            new ItemProperties(ItemResourceType.SlaughterWildPig, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterWildPig, CraftResourceLib.SlaughterWildPig, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterWildHog, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterWildHog, CraftResourceLib.SlaughterWildHog, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterWarHog, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterWarHog, CraftResourceLib.SlaughterWarHog, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterStagHog, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterStagHog, CraftResourceLib.SlaughterStagHog, null, StorageType.NUM_NONE);

            new ItemProperties(ItemResourceType.SlaughterWolf, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterWolf, CraftResourceLib.SlaughterWolf, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterWarg, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterWarg, CraftResourceLib.SlaughterWarg, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterAlphaWarg, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterAlphaWarg, CraftResourceLib.SlaughterAlphaWarg, null, StorageType.NUM_NONE);

            new ItemProperties(ItemResourceType.SlaughterWildCat, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterWildCat, CraftResourceLib.SlaughterWildCat, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterLion, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterLion, CraftResourceLib.SlaughterLion, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterWarLion, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterWarLion, CraftResourceLib.SlaughterWarLion, null, StorageType.NUM_NONE);

            new ItemProperties(ItemResourceType.SlaughterElephant, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterElephant, CraftResourceLib.SlaughterElephant, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterWarElephant, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterWarElephant, CraftResourceLib.SlaughterWarElephant, null, StorageType.NUM_NONE);
            new ItemProperties(ItemResourceType.SlaughterOliphant, NoCityResource, DefaultWeight, WorkPriorityType.SlaughterOliphant, CraftResourceLib.SlaughterOliphant, null, StorageType.NUM_NONE);
            
            
            var craftList = CraftList.AllBuidings();
            foreach (var building_craftItems in craftList)
            {
                foreach (var item in building_craftItems.Value)
                {
                    items[(int)item].AddItemSource(new ItemSource(building_craftItems.Key));//AddCraftSource(building_craftItems.Key);
                }
            }

            for (int i = 0; i < items.Length; ++i)
            {
                if (items[i] == null)
                {
                    new ItemProperties((ItemResourceType)i, NoCityResource, DefaultWeight, WorkPriorityType.NUM_NONE, null, null, StorageType.NUM_NONE);
                }
            }

            //Init armor health
            // None
            {
                var armor = Get(ItemResourceType.NONE);
                ref var soldier = ref armor.soldierData;
                soldier.basehealth = DssConst.ArmorHealth_None;
                soldier.modelData.armor = ArmorLevel.None;
            }

            // Padded Armor → Leather
            {
                var armor = Get(ItemResourceType.PaddedArmor);
                ref var soldier = ref armor.soldierData;
                soldier.basehealth = DssConst.ArmorHealth_Padded;
                soldier.modelData.armor = ArmorLevel.Leather;
            }

            // Heavy Padded Armor → Leather
            {
                var armor = Get(ItemResourceType.HeavyPaddedArmor);
                ref var soldier = ref armor.soldierData;
                soldier.basehealth = DssConst.ArmorHealth_HeavyPadded;
                soldier.modelData.armor = ArmorLevel.Leather;
            }

            // Bronze Armor → Iron
            {
                var armor = Get(ItemResourceType.BronzeArmor);
                ref var soldier = ref armor.soldierData;
                soldier.basehealth = DssConst.ArmorHealth_Bronze;
                soldier.modelData.armor = ArmorLevel.Iron;
            }

            // Iron Armor → Iron
            {
                var armor = Get(ItemResourceType.IronArmor);
                ref var soldier = ref armor.soldierData;
                soldier.basehealth = DssConst.ArmorHealth_Mail;
                soldier.modelData.armor = ArmorLevel.Iron;
            }

            // Heavy Iron Armor → Iron
            {
                var armor = Get(ItemResourceType.HeavyIronArmor);
                ref var soldier = ref armor.soldierData;
                soldier.basehealth = DssConst.ArmorHealth_HeavyMail;
                soldier.modelData.armor = ArmorLevel.Iron;
            }

            // Light Plate Armor → Steel
            {
                var armor = Get(ItemResourceType.LightPlateArmor);
                ref var soldier = ref armor.soldierData;
                soldier.basehealth = DssConst.ArmorHealth_Plate;
                soldier.modelData.armor = ArmorLevel.Steel;
            }

            // Full Plate Armor → Steel
            {
                var armor = Get(ItemResourceType.FullPlateArmor);
                ref var soldier = ref armor.soldierData;
                soldier.basehealth = DssConst.ArmorHealth_FullPlate;
                soldier.modelData.armor = ArmorLevel.Steel;
            }

            // Mithril Armor → Masterful
            {
                var armor = Get(ItemResourceType.MithrilArmor);
                ref var soldier = ref armor.soldierData;
                soldier.basehealth = DssConst.ArmorHealth_Mithril;
                soldier.modelData.armor = ArmorLevel.Masterful;
            }

            {
                var weapon = Get(ItemResourceType.Settler);
                ref var soldier = ref weapon.soldierData;
                soldier.attackDamage = DssConst.WeaponDamage_SharpStick;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.03f;
                soldier.modelName = LootFest.VoxelModelName.war_folkman;
                soldier.icon = SpriteName.WarsUnitIcon_Folkman;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.factionColoredModel = true;

                soldier.modelData.weapon = ItemResourceType.Settler;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            //Init weapons
            {
                var weapon = Get(ItemResourceType.SharpStick);
                weapon.Filter_IsTwoHandWeapon = false;
                ref var soldier = ref weapon.soldierData;
                soldier.attackDamage = DssConst.WeaponDamage_SharpStick;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.03f;
                soldier.modelName = LootFest.VoxelModelName.war_folkman;
                soldier.icon = SpriteName.WarsUnitIcon_Folkman;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.factionColoredModel = true;

                soldier.modelData.weapon = ItemResourceType.SharpStick;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.BronzeSword);
                weapon.Filter_IsTwoHandWeapon = false;
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_BronzeSword;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.03f;
                soldier.modelName = LootFest.VoxelModelName.wars_soldier;
                soldier.modelVariationCount = 3;
                soldier.icon = SpriteName.WarsUnitIcon_Soldier;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.BronzeSword;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.ShortSword);
                weapon.Filter_IsTwoHandWeapon = false;
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_ShortSword;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.03f;
                soldier.modelName = LootFest.VoxelModelName.wars_soldier;
                soldier.modelVariationCount = 3;
                soldier.icon = SpriteName.WarsUnitIcon_Soldier;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.ShortSword;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.Sword);
                weapon.Filter_IsTwoHandWeapon = false;
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Sword;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = DssConst.SwordAttackRange;
                soldier.modelName = LootFest.VoxelModelName.wars_soldier;
                soldier.modelVariationCount = 3;
                soldier.icon = SpriteName.WarsUnitIcon_Soldier;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.Sword;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.LongSword);
                weapon.Filter_IsTwoHandWeapon = false;
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_LongSword;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.05f;
                soldier.modelName = LootFest.VoxelModelName.wars_longsword;
                soldier.icon = SpriteName.WarsUnitIcon_Longsword;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.LongSword;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.Pike);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Pike;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.arrowWeakness = true;
                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.055f;
                soldier.modelName = LootFest.VoxelModelName.wars_piker;
                soldier.modelVariationCount = 1;
                //soldier.modelScale *= 1.6f;
                soldier.icon = SpriteName.WarsUnitIcon_Pikeman;
                //soldier.specialization = SpecializationType.AntiCavalry;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.Pike;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.HandSpear);
                weapon.Filter_IsTwoHandWeapon = false;
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Handspear;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.arrowWeakness = true;
                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.05f;
                soldier.modelName = LootFest.VoxelModelName.wars_spearman;
                soldier.modelVariationCount = 1;
                //soldier.modelScale *= 1.0f;
                soldier.icon = SpriteName.LittleUnitIconSpearman;
                soldier.basehealth += DssConst.WeaponHealthAdd_Handspear;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.HandSpear;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.Warhammer);
                weapon.Filter_IsTwoHandWeapon = false;
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Warhammer;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.04f;
                soldier.modelName = LootFest.VoxelModelName.wars_hammer;
                soldier.modelScale *= 1.14f;
                soldier.icon = SpriteName.WarsUnitIcon_Hammerknight;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;

                soldier.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.Warhammer;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.TwoHandSword);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_TwoHandSword;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.arrowWeakness = true;
                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.08f;
                soldier.modelName = LootFest.VoxelModelName.wars_twohand;
                soldier.modelVariationCount = 1;
                soldier.modelScale *= 1.1f;
                soldier.icon = SpriteName.WarsUnitIcon_TwoHand;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;

                soldier.blockReducingAttack_Inv = DssConst.SmallBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.TwoHandSword;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            //{
            //    var weapon = Get(ItemResourceType.KnightsLance);
            //    ref var soldier = ref weapon.soldierData;

            //    soldier.attackDamage = DssConst.WeaponDamage_KnigtsLance;
            //    soldier.attackSplashCount = 0;
            //    soldier.attackDamageStructure = Convert.ToInt32(30);// * skillBonus); // special override
            //    soldier.attackDamageSea = Convert.ToInt32(20);// * skillBonus);

            //    soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 2.5f;
            //    soldier.attackRange = 0.06f;
            //    soldier.basehealth *= 3;
            //    soldier.mainAttack = AttackType.Melee;
            //    soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 0.8f;

            //    soldier.modelName = LootFest.VoxelModelName.war_knight;
            //    soldier.modelVariationCount = 3;
            //    soldier.modelScale *= 0.75f;
            //    soldier.icon = SpriteName.WarsUnitIcon_Knight;

            //    soldier.upkeepMultiplier = 3;//DssLib.SoldierDefaultEnergyUpkeep * 3;
            //    soldier.rowWidth = 4;
            //    soldier.columnsDepth = 3;
            //    soldier.groupSpacing = DssVar.DefaultGroupSpacing * 1.4f;
            //    soldier.workForcePerUnit = 2;
            //    soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
            //    soldier.hasBannerMan = false;

            //    soldier.blockReducingAttack_Inv = DssConst.SmallBlockReduceAttack_Inv;
            //    soldier.factionColoredModel = true;
            //    soldier.modelData.weapon = ItemResourceType.KnightsLance;
            //    soldier.modelData.modelType = ModelType.Riding;
            //}

            {
                var weapon = Get(ItemResourceType.MithrilSword);
                weapon.Filter_IsTwoHandWeapon = false;
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_MithrilSword;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.055f;
                soldier.modelScale *= 1.1f;
                soldier.modelName = LootFest.VoxelModelName.wars_mithrilman;
                soldier.icon = SpriteName.WarsUnitIcon_MithrilMan;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 0.8f;

                soldier.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.MithrilSword;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.SlingShot);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Slingshot;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 1.4f;
                soldier.mainAttack = AttackType.SlingShot;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                soldier.attackRange = 1.8f;
                soldier.modelName = LootFest.VoxelModelName.wars_slingman;
                soldier.icon = SpriteName.WarsUnitIcon_Slingshot;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
                soldier.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.SlingShot;
                soldier.modelData.modelType = ModelType.Soldier;
            }


            {
                var weapon = Get(ItemResourceType.ThrowingSpear);
                weapon.Filter_IsTwoHandWeapon = false;
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Throwingspear;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 1.3f;
                soldier.mainAttack = AttackType.Javelin;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                soldier.attackRange = 0.4f;
                soldier.modelName = LootFest.VoxelModelName.wars_javelin;
                soldier.icon = SpriteName.WarsUnitIcon_Javelin;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 9f;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.ThrowingSpear;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.Bow);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Bow;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Arrow;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                soldier.attackRange = 1.3f;
                soldier.modelName = LootFest.VoxelModelName.war_archer;
                soldier.modelVariationCount = 2;
                soldier.icon = SpriteName.WarsUnitIcon_Archer;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
                soldier.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.Bow;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.LongBow);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Longbow;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Arrow;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                soldier.attackRange = 1.7f;
                soldier.modelName = LootFest.VoxelModelName.war_archer;
                soldier.modelVariationCount = 2;
                soldier.icon = SpriteName.WarsUnitIcon_Archer;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
                soldier.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.LongBow;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.Crossbow);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_CrossBow;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Bolt;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                soldier.attackRange = 1.7f;
                soldier.modelName = LootFest.VoxelModelName.wars_crossbow;
                soldier.modelVariationCount = 1;
                soldier.icon = SpriteName.LittleUnitIconCrossBowman;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 15f;

                soldier.blockReducingAttack_Inv = DssConst.SmallBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.Crossbow;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.MithrilBow);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_MithrilBow;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Arrow;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                soldier.attackRange = 2.5f;
                soldier.modelName = LootFest.VoxelModelName.wars_mithrilarcher;
                soldier.modelScale *= 1.08f;
                soldier.icon = SpriteName.WarsUnitIcon_MithrilArcher;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 8f;

                soldier.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.MithrilBow;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.HandCannon);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Handcannon;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.GunShot;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                soldier.attackRange = 1.2f;
                soldier.modelName = LootFest.VoxelModelName.wars_handcannon;
                soldier.modelVariationCount = 1;
                soldier.icon = SpriteName.WarsUnitIcon_BronzeRifle;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 12f;

                soldier.blockReducingAttack_Inv = DssConst.SmallBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.HandCannon;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.HandCulverin);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Handculvetin;
                soldier.attackSplashCount = 7;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.GunBlast;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                soldier.attackRange = 0.4f;
                soldier.modelName = LootFest.VoxelModelName.wars_culvertin;
                soldier.modelVariationCount = 1;
                soldier.icon = SpriteName.WarsUnitIcon_BronzeRifle;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 12f;

                soldier.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.HandCulverin;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.Rifle);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Rifle;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.GunShot;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                soldier.attackRange = 1.5f;
                soldier.modelName = LootFest.VoxelModelName.wars_handcannon;
                soldier.modelVariationCount = 1;
                soldier.icon = SpriteName.WarsUnitIcon_BronzeRifle;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 12f;

                soldier.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.Rifle;
                soldier.modelData.modelType = ModelType.Soldier;
            }

            {
                var weapon = Get(ItemResourceType.Blunderbuss);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Blunderbus;
                soldier.attackSplashCount = 8;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.GunBlast;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                soldier.attackRange = 0.5f;
                soldier.modelName = LootFest.VoxelModelName.wars_culvertin;
                soldier.modelVariationCount = 1;
                soldier.icon = SpriteName.WarsUnitIcon_BronzeRifle;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 12f;

                soldier.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.Blunderbuss;
                soldier.modelData.modelType = ModelType.Soldier;
                soldier.boundRadius = DssVar.StandardBoundRadius * 2.2f;
            }

            {
                var weapon = Get(ItemResourceType.Ballista);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Ballista;
                soldier.attackSplashCount = 1;
                soldier.attackDamageStructure = Convert.ToInt32(1500); //* skillBonus);
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                soldier.attackRange = WarmachineProfile.BallistaRange;
                soldier.basehealth = MathExt.MultiplyInt(0.5, soldier.basehealth);
                soldier.mainAttack = AttackType.Ballista;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 32f;
                soldier.modelName = LootFest.VoxelModelName.war_ballista;
                soldier.modelVariationCount = 2;
                soldier.modelScale = DssConst.Men_StandardModelScale * 2f;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Second;
                soldier.icon = SpriteName.WarsUnitIcon_Ballista;
                soldier.upkeepMultiplier = 2;//DssLib.SoldierDefaultEnergyUpkeep * 2;
                soldier.rowWidth = 3;
                soldier.columnsDepth = 2;
                soldier.workForcePerUnit = 2;
                //soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
                soldier.hasBannerMan = false;
                soldier.rotationSpeed = DssConst.WarmachineRotatingSpeed_NoWheels;
                soldier.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.Ballista;
                soldier.modelData.modelType = ModelType.Warmashine;
                weapon.Filter_IsSiegeWeapon = true;
                soldier.boundRadius = DssVar.StandardBoundRadius * 2.2f;
            }

            {
                var weapon = Get(ItemResourceType.Manuballista);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_ManuBallista;
                soldier.attackSplashCount = 1;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                soldier.attackRange = 1.7f;
                soldier.basehealth = MathExt.MultiplyInt(0.5, soldier.basehealth);
                soldier.mainAttack = AttackType.Ballista;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 32f;
                soldier.modelName = LootFest.VoxelModelName.wars_manuballista;
                soldier.modelScale = DssConst.Men_StandardModelScale * 1.5f;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                soldier.icon = SpriteName.WarsResource_Manuballista;
                soldier.upkeepMultiplier = 2;//DssLib.SoldierDefaultEnergyUpkeep * 2;
                soldier.rowWidth = 3;
                soldier.columnsDepth = 2;
                soldier.workForcePerUnit = 2;
                //soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
                soldier.hasBannerMan = false;
                soldier.rotationSpeed = DssConst.WarmachineRotatingSpeed_NoWheels;
                soldier.blockReducingAttack_Inv = DssConst.HeavyBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.Manuballista;
                soldier.modelData.modelType = ModelType.Warmashine;
                soldier.boundRadius = DssVar.StandardBoundRadius * 2.2f;
            }
            {
                var weapon = Get(ItemResourceType.Catapult);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Catapult;
                soldier.attackSplashCount = 3;
                soldier.attackDamageStructure = Convert.ToInt32(2000); //* skillBonus);
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                soldier.attackRange = 2.6f;
                soldier.basehealth = MathExt.MultiplyInt(0.5, soldier.basehealth);
                soldier.mainAttack = AttackType.Catapult;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 32f;
                soldier.modelName = LootFest.VoxelModelName.wars_catapult;
                soldier.modelScale = DssConst.Men_StandardModelScale * 2.3f;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Second;
                soldier.icon = SpriteName.WarsUnitIcon_Catapult;
                soldier.upkeepMultiplier = 2;//DssLib.SoldierDefaultEnergyUpkeep * 2;
                soldier.rowWidth = 2;
                soldier.columnsDepth = 2;
                soldier.workForcePerUnit = 2;
                //soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 2.5f;
                soldier.hasBannerMan = false;
                soldier.rotationSpeed = DssConst.WarmachineRotatingSpeed_NoWheels;
                soldier.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.Catapult;
                soldier.modelData.modelType = ModelType.Warmashine;
                weapon.Filter_IsSiegeWeapon = true;
                soldier.boundRadius = DssVar.StandardBoundRadius * 2.2f;
            }
            {
                var weapon = Get(ItemResourceType.SiegeCannonBronze);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_SiegeCannonBronze;
                soldier.attackSplashCount = 12;
                soldier.attackDamageStructure = Convert.ToInt32(2000); // * skillBonus);
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.3f;
                soldier.attackRange = 2.4f;
                soldier.basehealth = MathExt.MultiplyInt(0.5, soldier.basehealth);
                soldier.mainAttack = AttackType.MassiveCannonball;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 100f;
                soldier.modelName = LootFest.VoxelModelName.wars_bronzesiegecannon;
                soldier.modelScale = DssConst.Men_StandardModelScale * 5f;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Second;
                soldier.icon = SpriteName.WarsUnitIcon_Catapult;
                soldier.upkeepMultiplier = 2;//DssLib.SoldierDefaultEnergyUpkeep * 2;
                soldier.rowWidth = 1;
                soldier.columnsDepth = 1;
                soldier.workForcePerUnit = 6;
                //soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 2.5f;
                soldier.hasBannerMan = false;
                soldier.attackStart = new Vector3(0, DssConst.Men_StandardModelScale * 0.4f, DssConst.Men_StandardModelScale * 2.4f);
                soldier.rotationSpeed = DssConst.WarmachineRotatingSpeed_NoWheels;
                soldier.blockReducingAttack_Inv = DssConst.HeavyBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.SiegeCannonBronze;
                soldier.modelData.modelType = ModelType.Warmashine;
                weapon.Filter_IsSiegeWeapon = true;
                soldier.boundRadius = DssVar.StandardBoundRadius * 2.2f;
            }
            {
                var weapon = Get(ItemResourceType.ManCannonBronze);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_ManCannonBronze;
                soldier.attackSplashCount = 5;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                soldier.attackRange = 2f;
                soldier.basehealth = MathExt.MultiplyInt(0.5, soldier.basehealth);
                soldier.mainAttack = AttackType.Cannonball;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 40f;
                soldier.modelName = LootFest.VoxelModelName.wars_bronzemancannon;
                soldier.modelScale = DssConst.Men_StandardModelScale * 2f;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                soldier.icon = SpriteName.WarsResource_BronzeManCannon;
                soldier.upkeepMultiplier = 2;//DssLib.SoldierDefaultEnergyUpkeep * 2;
                soldier.rowWidth = 3;
                soldier.columnsDepth = 2;
                soldier.workForcePerUnit = 2;
                //soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 2.5f;
                soldier.hasBannerMan = false;
                soldier.attackStart = new Vector3(0, DssConst.Men_StandardModelScale * 0.4f, DssConst.Men_StandardModelScale * 1.1f);
                soldier.rotationSpeed = DssConst.WarmachineRotatingSpeed_Wheels;
                soldier.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.ManCannonBronze;
                soldier.modelData.modelType = ModelType.Warmashine;
                soldier.boundRadius = DssVar.StandardBoundRadius * 2.2f;
            }
            {
                var weapon = Get(ItemResourceType.SiegeCannonIron);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_SiegeCannonIron;
                soldier.attackSplashCount = 2;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                soldier.attackRange = 2.2f;
                soldier.basehealth = MathExt.MultiplyInt(0.5, soldier.basehealth);
                soldier.mainAttack = AttackType.Haubitz;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 40f;
                soldier.modelName = LootFest.VoxelModelName.wars_ironsiegecannon;
                soldier.modelScale = DssConst.Men_StandardModelScale * 1f;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Second;
                soldier.icon = SpriteName.WarsResource_IronSiegeCannon;
                soldier.upkeepMultiplier = 4;//DssLib.SoldierDefaultEnergyUpkeep * 4;
                soldier.rowWidth = 3;
                soldier.columnsDepth = 2;
                soldier.workForcePerUnit = 2;
                //soldier.upkeepMultiplier = DssLib.SoldierDefaultUpkeep * 2;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
                soldier.hasBannerMan = false;
                soldier.attackStart = new Vector3(0, DssConst.Men_StandardModelScale * 0.4f, DssConst.Men_StandardModelScale * 0.3f);
                soldier.rotationSpeed = DssConst.WarmachineRotatingSpeed_NoWheels;
                soldier.blockReducingAttack_Inv = DssConst.HeavyBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.SiegeCannonIron;
                soldier.modelData.modelType = ModelType.Warmashine;
                weapon.Filter_IsSiegeWeapon = true;
                soldier.boundRadius = DssVar.StandardBoundRadius * 2.2f;
            }

            {
                var weapon = Get(ItemResourceType.ManCannonIron);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_ManCannonIron;
                soldier.attackSplashCount = 6;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                soldier.attackRange = 2.4f;
                soldier.basehealth = MathExt.MultiplyInt(0.5, soldier.basehealth);
                soldier.mainAttack = AttackType.Cannonball;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 40;
                soldier.modelName = LootFest.VoxelModelName.wars_ironmancannon;
                soldier.modelScale = DssConst.Men_StandardModelScale * 1.7f;
                soldier.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                soldier.icon = SpriteName.WarsUnitIcon_IronManCannon;
                soldier.upkeepMultiplier = 4;//DssLib.SoldierDefaultEnergyUpkeep * 2;
                soldier.rowWidth = 3;
                soldier.columnsDepth = 2;
                soldier.workForcePerUnit = 2;
                //soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
                soldier.hasBannerMan = false;
                soldier.attackStart = new Vector3(0, DssConst.Men_StandardModelScale * 0.4f, DssConst.Men_StandardModelScale * 1f);
                soldier.rotationSpeed = DssConst.WarmachineRotatingSpeed_Wheels;
                soldier.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.ManCannonIron;
                soldier.modelData.modelType = ModelType.Warmashine;
                soldier.boundRadius = DssVar.StandardBoundRadius * 2.2f;
            }

            {
                var weapon = Get(ItemResourceType.RoseWarrior_soldier);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_LongSword;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.05f;
                soldier.modelName = LootFest.VoxelModelName.wars_rosewarrior;
                soldier.factionColoredModel = false;
                soldier.icon = SpriteName.MissingImage;
                soldier.modelScale = DssConst.Men_StandardModelScale * 1.4f;
                soldier.hasBannerMan = false;
                soldier.rowWidth = 5;
                soldier.columnsDepth = 4;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 1.2f;
                soldier.groupSpacingRndOffset = DssVar.StandardBoundRadius * 1f;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.modelData.weapon = ItemResourceType.RoseWarrior_soldier;
                soldier.modelData.modelType = ModelType.Custom;
            }

            {
                var weapon = Get(ItemResourceType.RoseWarrior_dog);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_Sword;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.05f;
                soldier.modelName = LootFest.VoxelModelName.wars_rosedog;
                soldier.factionColoredModel = false;
                soldier.icon = SpriteName.MissingImage;
                soldier.modelScale = DssConst.Men_StandardModelScale * 1f;
                soldier.hasBannerMan = false;
                soldier.rowWidth = 5;
                soldier.columnsDepth = 4;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 1.2f;
                soldier.groupSpacingRndOffset = DssVar.StandardBoundRadius * 1f;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.modelData.weapon = ItemResourceType.RoseWarrior_dog;
                soldier.modelData.modelType = ModelType.Custom;
            }

            {
                var weapon = Get(ItemResourceType.RoseWarrior_tank);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_MithrilSword;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = soldier.attackDamage;
                soldier.attackDamageSea = soldier.attackDamage;

                soldier.mainAttack = AttackType.Melee;
                soldier.attackRange = 0.05f;
                soldier.modelName = LootFest.VoxelModelName.wars_rosetank;
                soldier.factionColoredModel = false;
                soldier.icon = SpriteName.MissingImage;
                soldier.modelScale = DssConst.Men_StandardModelScale * 2f;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
                soldier.hasBannerMan = false;
                soldier.rowWidth = 3;
                soldier.columnsDepth = 2;
                soldier.groupSpacingRndOffset = DssVar.StandardBoundRadius * 1f;
                soldier.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;
                soldier.modelData.weapon = ItemResourceType.RoseWarrior_tank;
                soldier.modelData.modelType = ModelType.Custom;
            }



        }

        public static ItemProperties Get(ItemResourceType type)
        {
            return items[(int)type];
        }

        public static int CityIndex(ItemResourceType type)
        {
            return items[(int)type].cityResourceIndex;
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
}
