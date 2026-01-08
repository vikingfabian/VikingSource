using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Map;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.Resource
{
    static class ItemPropertyColl
    {
        public const int CarryStones = 5;
        public const int CarryFood = 20;
        public static ItemProperties[] items;

        static float DefaultWeight = 1f / 30;

        public static float ArmyFoodOrderSize;

        public static void Init()
        {
            ArmyFoodOrderSize = ItemPropertyColl.CarryFood * DssConst.Worker_TrossWorkerCarryWeight;
            items = new ItemProperties[(int)ItemResourceType.NUM];

            // wood variants
            new ItemProperties(ItemResourceType.HardWood, 1f / 20, null, null);
            new ItemProperties(ItemResourceType.SoftWood, 1f / 30, null, null);
            new ItemProperties(ItemResourceType.DryWood, 1f / 60, null, null);

            // basic resources
            new ItemProperties(ItemResourceType.Stone_G, 1f / CarryStones, null, null);
            new ItemProperties(ItemResourceType.IronOre_G, 1f / 10, null, null);
            new ItemProperties(ItemResourceType.GoldOre, 1f / 10, null, null);
            new ItemProperties(ItemResourceType.Egg, 1f / 60, null, null);
            new ItemProperties(ItemResourceType.Pig, 1f, null, null);
            new ItemProperties(ItemResourceType.Hen, 1f / 4, null, null);
            new ItemProperties(ItemResourceType.Wheat, 1f / 10, null, null);
            new ItemProperties(ItemResourceType.Linen, 1f / 10, null, null);

            // fuel & food
            new ItemProperties(ItemResourceType.Fuel_G, DefaultWeight, CraftResourceLib.Fuel1, null);
            new ItemProperties(ItemResourceType.Coal, DefaultWeight, CraftResourceLib.Charcoal, null);
            new ItemProperties(ItemResourceType.Food_G, 1f / CarryFood, CraftResourceLib.Food1, CraftResourceLib.Food2);
            new ItemProperties(ItemResourceType.Beer, DefaultWeight, CraftResourceLib.Beer, null);
            new ItemProperties(ItemResourceType.CoolingFluid, DefaultWeight, CraftResourceLib.CoolingFluid, null);

            // metals & alloys
            new ItemProperties(ItemResourceType.Copper, DefaultWeight, CraftResourceLib.Copper, CraftResourceLib.Cupper_AndCooling);
            new ItemProperties(ItemResourceType.Tin, DefaultWeight, CraftResourceLib.Tin, null);
            new ItemProperties(ItemResourceType.Lead, DefaultWeight, CraftResourceLib.Lead, null);
            new ItemProperties(ItemResourceType.Iron_G, DefaultWeight, CraftResourceLib.Iron, CraftResourceLib.Iron_AndCooling);
            new ItemProperties(ItemResourceType.Silver, DefaultWeight, CraftResourceLib.Silver, CraftResourceLib.Silver_AndCooling);
            new ItemProperties(ItemResourceType.Gold, DefaultWeight, Minting.ConvertGoldOre, null);
            new ItemProperties(ItemResourceType.Bronze, DefaultWeight, CraftResourceLib.Bronze, null);
            new ItemProperties(ItemResourceType.CastIron, DefaultWeight, CraftResourceLib.CastIron, null);
            new ItemProperties(ItemResourceType.BloomeryIron, DefaultWeight, CraftResourceLib.BloomeryIron, null);
            new ItemProperties(ItemResourceType.Steel, DefaultWeight, CraftResourceLib.Steel, CraftResourceLib.Steel_AndCooling);
            new ItemProperties(ItemResourceType.Mithril, DefaultWeight, CraftResourceLib.Mithril, null);

            // armors
            new ItemProperties(ItemResourceType.PaddedArmor, DefaultWeight, CraftResourceLib.PaddedArmor, null);
            new ItemProperties(ItemResourceType.HeavyPaddedArmor, DefaultWeight, CraftResourceLib.HeavyPaddedArmor, null);
            new ItemProperties(ItemResourceType.BronzeArmor, DefaultWeight, CraftResourceLib.BronzeArmor, null);
            new ItemProperties(ItemResourceType.IronArmor, DefaultWeight, CraftResourceLib.MailArmor, null);
            new ItemProperties(ItemResourceType.HeavyIronArmor, DefaultWeight, CraftResourceLib.HeavyMailArmor, null);
            new ItemProperties(ItemResourceType.LightPlateArmor, DefaultWeight, CraftResourceLib.PlateArmor, null);
            new ItemProperties(ItemResourceType.FullPlateArmor, DefaultWeight, CraftResourceLib.FullPlateArmor, null);
            new ItemProperties(ItemResourceType.MithrilArmor, DefaultWeight, CraftResourceLib.MithrilArmor, null);

            // buildings & tools
            new ItemProperties(ItemResourceType.Palisade, DefaultWeight, CraftResourceLib.Palisade, null);
            new ItemProperties(ItemResourceType.Toolkit, DefaultWeight, CraftResourceLib.Toolkit, null);
            new ItemProperties(ItemResourceType.Wagon2Wheel, DefaultWeight, CraftResourceLib.WagonLight, null);
            new ItemProperties(ItemResourceType.Wagon4Wheel, DefaultWeight, CraftResourceLib.WagonHeavy, null);

            // gunpowder & ballistics
            new ItemProperties(ItemResourceType.BlackPowder, DefaultWeight, CraftResourceLib.BlackPowder, null);
            new ItemProperties(ItemResourceType.GunPowder, DefaultWeight, CraftResourceLib.GunPowder, null);
            new ItemProperties(ItemResourceType.LedBullet, DefaultWeight, CraftResourceLib.LedBullets, null);

            // melee weapons
            new ItemProperties(ItemResourceType.SharpStick, DefaultWeight, CraftResourceLib.SharpStick, null);
            new ItemProperties(ItemResourceType.BronzeSword, DefaultWeight, CraftResourceLib.BronzeSword, null);
            new ItemProperties(ItemResourceType.ShortSword, DefaultWeight, CraftResourceLib.ShortSword, null);
            new ItemProperties(ItemResourceType.Sword, DefaultWeight, CraftResourceLib.Sword, null);
            new ItemProperties(ItemResourceType.LongSword, DefaultWeight, CraftResourceLib.LongSword, null);
            new ItemProperties(ItemResourceType.HandSpear, DefaultWeight, CraftResourceLib.HandSpearIron, CraftResourceLib.HandSpearBronze);
            new ItemProperties(ItemResourceType.MithrilSword, DefaultWeight, CraftResourceLib.MithrilSword, null);
            new ItemProperties(ItemResourceType.Warhammer, DefaultWeight, CraftResourceLib.WarhammerIron, CraftResourceLib.WarhammerBronze);
            new ItemProperties(ItemResourceType.TwoHandSword, DefaultWeight, CraftResourceLib.TwoHandSword, null);
            new ItemProperties(ItemResourceType.KnightsLance, DefaultWeight, CraftResourceLib.KnightsLance, null);

            // ranged weapons
            new ItemProperties(ItemResourceType.SlingShot, DefaultWeight, CraftResourceLib.Slingshot, null);
            new ItemProperties(ItemResourceType.ThrowingSpear, DefaultWeight, CraftResourceLib.ThrowingSpear1, CraftResourceLib.ThrowingSpear2);
            new ItemProperties(ItemResourceType.Bow, DefaultWeight, CraftResourceLib.Bow, null);
            new ItemProperties(ItemResourceType.LongBow, DefaultWeight, CraftResourceLib.LongBow, null);
            new ItemProperties(ItemResourceType.Crossbow, DefaultWeight, CraftResourceLib.CrossBow, null);
            new ItemProperties(ItemResourceType.MithrilBow, DefaultWeight, CraftResourceLib.MithrilBow, null);

            // firearms
            new ItemProperties(ItemResourceType.HandCannon, DefaultWeight, CraftResourceLib.BronzeHandCannon, null);
            new ItemProperties(ItemResourceType.HandCulverin, DefaultWeight, CraftResourceLib.BronzeHandCulverin, null);
            new ItemProperties(ItemResourceType.Rifle, DefaultWeight, CraftResourceLib.Rifle, null);
            new ItemProperties(ItemResourceType.Blunderbuss, DefaultWeight, CraftResourceLib.Blunderbus, null);

            // siege engines
            new ItemProperties(ItemResourceType.Ballista, DefaultWeight, CraftResourceLib.Ballista_Iron, CraftResourceLib.Ballista_Bronze);
            new ItemProperties(ItemResourceType.Manuballista, DefaultWeight, CraftResourceLib.ManuBallista, null);
            new ItemProperties(ItemResourceType.Catapult, DefaultWeight, CraftResourceLib.Catapult, null);
            new ItemProperties(ItemResourceType.SiegeCannonBronze, DefaultWeight, CraftResourceLib.SiegeCannonBronze, null);
            new ItemProperties(ItemResourceType.ManCannonBronze, DefaultWeight, CraftResourceLib.ManCannonBronze, null);
            new ItemProperties(ItemResourceType.SiegeCannonIron, DefaultWeight, CraftResourceLib.SiegeCannonIron, null);
            new ItemProperties(ItemResourceType.ManCannonIron, DefaultWeight, CraftResourceLib.ManCannonIron, null);

            // coins
            new ItemProperties(ItemResourceType.CopperCoin, DefaultWeight, Minting.CopperCoin, null);
            new ItemProperties(ItemResourceType.BronzeCoin, DefaultWeight, Minting.BronzeCoin, null);
            new ItemProperties(ItemResourceType.SilverCoin, DefaultWeight, Minting.SilverCoin, null);
            new ItemProperties(ItemResourceType.ElfCoin, DefaultWeight, Minting.ElfCoin, null);


            for (int i = 0; i < items.Length; ++i)
            {
                if (items[i] == null)
                {
                    new ItemProperties((ItemResourceType)i, DefaultWeight, null, null);
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


            //Init weapons
            {
                var weapon = Get(ItemResourceType.SharpStick);
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

            {
                var weapon = Get(ItemResourceType.KnightsLance);
                ref var soldier = ref weapon.soldierData;

                soldier.attackDamage = DssConst.WeaponDamage_KnigtsLance;
                soldier.attackSplashCount = 0;
                soldier.attackDamageStructure = Convert.ToInt32(30);// * skillBonus); // special override
                soldier.attackDamageSea = Convert.ToInt32(20);// * skillBonus);

                soldier.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 2.5f;
                soldier.attackRange = 0.06f;
                soldier.basehealth *= 3;
                soldier.mainAttack = AttackType.Melee;
                soldier.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 0.8f;

                soldier.modelName = LootFest.VoxelModelName.war_knight;
                soldier.modelVariationCount = 3;
                soldier.modelScale *= 0.75f;
                soldier.icon = SpriteName.WarsUnitIcon_Knight;

                soldier.upkeepMultiplier = 3;//DssLib.SoldierDefaultEnergyUpkeep * 3;
                soldier.rowWidth = 4;
                soldier.columnsDepth = 3;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 1.4f;
                soldier.workForcePerUnit = 2;
                soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                soldier.hasBannerMan = false;

                soldier.blockReducingAttack_Inv = DssConst.SmallBlockReduceAttack_Inv;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.KnightsLance;
                soldier.modelData.modelType = ModelType.Riding;
            }

            {
                var weapon = Get(ItemResourceType.MithrilSword);
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
                soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
                soldier.hasBannerMan = false;
                soldier.rotationSpeed = DssConst.WarmachineRotatingSpeed_NoWheels;
                soldier.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.Ballista;
                soldier.modelData.modelType = ModelType.Warmashine;
                weapon.Filter_IsSiegeWeapon = true;
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
                soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
                soldier.hasBannerMan = false;
                soldier.rotationSpeed = DssConst.WarmachineRotatingSpeed_NoWheels;
                soldier.blockReducingAttack_Inv = DssConst.HeavyBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.Manuballista;
                soldier.modelData.modelType = ModelType.Warmashine;
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
                soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 2.5f;
                soldier.hasBannerMan = false;
                soldier.rotationSpeed = DssConst.WarmachineRotatingSpeed_NoWheels;
                soldier.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.Catapult;
                soldier.modelData.modelType = ModelType.Warmashine;
                weapon.Filter_IsSiegeWeapon = true;
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
                soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
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
                soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 2.5f;
                soldier.hasBannerMan = false;
                soldier.attackStart = new Vector3(0, DssConst.Men_StandardModelScale * 0.4f, DssConst.Men_StandardModelScale * 1.1f);
                soldier.rotationSpeed = DssConst.WarmachineRotatingSpeed_Wheels;
                soldier.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.ManCannonBronze;
                soldier.modelData.modelType = ModelType.Warmashine;
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
                soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
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
                soldier.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                soldier.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
                soldier.hasBannerMan = false;
                soldier.attackStart = new Vector3(0, DssConst.Men_StandardModelScale * 0.4f, DssConst.Men_StandardModelScale * 1f);
                soldier.rotationSpeed = DssConst.WarmachineRotatingSpeed_Wheels;
                soldier.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
                soldier.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
                soldier.factionColoredModel = true;
                soldier.modelData.weapon = ItemResourceType.ManCannonIron;
                soldier.modelData.modelType = ModelType.Warmashine;
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

        public SoldierData soldierData = new SoldierData();
        public bool Filter_IsSiegeWeapon = false;

        public ItemComesFrom comesFrom = ItemComesFrom.NONE;
        public int comesFromId1 = -1, comesFromId2 = -1;

        public ItemProperties(ItemResourceType type, float weight, CraftBlueprint bp1, CraftBlueprint bp2)
        {   
            this.weight = weight;
            this.bp1 = bp1;
            this.bp2 = bp2;

            ItemPropertyColl.items[(int)type] = this;
        }

        public void SetComesFrom(TerrainMineType mineType)
        {
            comesFrom = ItemComesFrom.Mine;
            comesFromId1 = (int)mineType;
        }
    }

    enum ItemComesFrom
    { 
        NONE,
        Terrain,
        Mine,
        Farm,
        Crafting,
        NUM
    }
}
