using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;

namespace VikingEngine.Core.DSSWars.Work
{
    struct WorkProperty
    {
        public WorkPriorityType workType;
        public SpriteName icon1;
        public SpriteName icon2;
        public string name;

        public WorkViewMode workView;
        public ItemResourceType relatedResource1, relatedResource2;
        public ItemResourceType? relatedMine;
        public bool waitForFullStockOption;

        public WorkProperty(WorkPriorityType workType, SpriteName icon1, SpriteName icon2, string name, ItemResourceType relatedResource1, ItemResourceType relatedResource2)
        {
            this.workType = workType;
            this.icon1 = icon1;
            this.icon2 = icon2;
            this.name = name;

            this.relatedResource1 = relatedResource1;
            this.relatedResource2 = relatedResource2;
        }
    }

    static class WorkPropertyColl
    {
        static WorkProperty[] workProperties = new WorkProperty[(int)WorkPriorityType.NUM_NONE];

        public static WorkProperty Get(WorkPriorityType workType)
        {
            return workProperties[(int)workType];
        }

        public static void Init()
        {
            // --- Resources ---
            workProperties[(int)WorkPriorityType.move] = new WorkProperty(WorkPriorityType.move, SpriteName.WarsWorkMove, SpriteName.WarsBuild_Storehouse, DssRef.lang.Work_Move, ItemResourceType.NONE, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.wood] = new WorkProperty(WorkPriorityType.wood, SpriteName.WarsWorkCollect, SpriteName.WarsResource_Wood, string.Format(DssRef.lang.Work_GatherXResource, DssRef.lang.Resource_TypeName_Wood.ToLowerInvariant()), ItemResourceType.Wood_G, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.stone] = new WorkProperty(WorkPriorityType.stone, SpriteName.WarsWorkCollect, SpriteName.WarsResource_Stone, string.Format(DssRef.lang.Work_GatherXResource, DssRef.lang.Resource_TypeName_Stone.ToLowerInvariant()), ItemResourceType.Stone_G, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.collectClay] = new WorkProperty(WorkPriorityType.collectClay, SpriteName.WarsWorkCollect, SpriteName.WarsResource_Clay, string.Format(DssRef.lang.Work_GatherXResource, DssRef.lang.Resource_TypeName_Clay), ItemResourceType.Clay, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.miningBrick] = new WorkProperty(WorkPriorityType.miningBrick, SpriteName.WarsWorkMine, SpriteName.WarsResource_Brick, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Brick), ItemResourceType.Brick, ItemResourceType.NONE)
            {
                relatedMine = ItemResourceType.Brick
            };
            workProperties[(int)WorkPriorityType.craftBrick] = new WorkProperty(WorkPriorityType.craftBrick, SpriteName.WarsHammer, SpriteName.WarsResource_Brick, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Brick), ItemResourceType.Brick, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.farmFood] = new WorkProperty(WorkPriorityType.farmFood, SpriteName.WarsWorkFarm, SpriteName.WarsResource_Food, DssRef.lang.Work_Farming + ": " + DssRef.lang.BuildingType_Orchard, ItemResourceType.Food_G, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.farmRawFood] = new WorkProperty(WorkPriorityType.farmRawFood, SpriteName.WarsWorkFarm, SpriteName.WarsResource_RawFood, DssRef.lang.Work_Farming + ": " + DssRef.lang.Resource_TypeName_Food.ToLowerInvariant(), ItemResourceType.RawFood_G, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.miningSalt] = new WorkProperty(WorkPriorityType.miningSalt, SpriteName.WarsWorkMine, SpriteName.WarsResource_Salt, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Salt.ToLowerInvariant()), ItemResourceType.Salt, ItemResourceType.NONE)
            {
                relatedMine = ItemResourceType.Salt
            };

            workProperties[(int)WorkPriorityType.farmfuel] = new WorkProperty(WorkPriorityType.farmfuel, SpriteName.WarsWorkFarm, SpriteName.WarsResource_Fuel, DssRef.lang.Work_Farming + ": " + DssRef.lang.Resource_TypeName_Fuel.ToLowerInvariant(), ItemResourceType.Fuel_G, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.farmlinen] = new WorkProperty(WorkPriorityType.farmlinen, SpriteName.WarsWorkFarm, SpriteName.WarsResource_LinenCloth, DssRef.lang.Work_Farming + ": " + DssRef.lang.Resource_TypeName_Linen.ToLowerInvariant(), ItemResourceType.SkinLinen_G, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.craftFood] = new WorkProperty(WorkPriorityType.craftFood, SpriteName.WarsHammer, SpriteName.WarsResource_Food, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Food.ToLowerInvariant()), ItemResourceType.Food_G, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftConservedFood] = new WorkProperty(WorkPriorityType.craftConservedFood, SpriteName.WarsHammer, SpriteName.WarsResource_ConservedFood, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_ConservedFood.ToLowerInvariant()), ItemResourceType.ConservedFood, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftFuel] = new WorkProperty(WorkPriorityType.craftFuel, SpriteName.WarsHammer, SpriteName.WarsResource_Fuel, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Fuel.ToLowerInvariant()), ItemResourceType.Fuel_G, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftBeer] = new WorkProperty(WorkPriorityType.craftBeer, SpriteName.WarsHammer, SpriteName.WarsResource_Beer, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Beer.ToLowerInvariant()), ItemResourceType.Beer, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftCoolingFluid] = new WorkProperty(WorkPriorityType.craftCoolingFluid, SpriteName.WarsHammer, SpriteName.WarsResource_CoolingFluid, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_CoolingFluid.ToLowerInvariant()), ItemResourceType.CoolingFluid, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.craftContainer] = new WorkProperty(WorkPriorityType.craftContainer, SpriteName.WarsHammer, SpriteName.WarsResource_Container, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Container.ToLowerInvariant()), ItemResourceType.Container, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftPalisade] = new WorkProperty(WorkPriorityType.craftPalisade, SpriteName.WarsHammer, SpriteName.WarsResource_Palisade, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Palisade.ToLowerInvariant()), ItemResourceType.Palisade, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftToolkit] = new WorkProperty(WorkPriorityType.craftToolkit, SpriteName.WarsHammer, SpriteName.WarsResource_Toolkit, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Toolkit.ToLowerInvariant()), ItemResourceType.Toolkit, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.craftWagon2Wheel] = new WorkProperty(WorkPriorityType.craftWagon2Wheel, SpriteName.WarsHammer, SpriteName.WarsResource_Wagon2Wheel, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Wagon2Wheel.ToLowerInvariant()), ItemResourceType.Wagon2Wheel, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftWagon4Wheel] = new WorkProperty(WorkPriorityType.craftWagon4Wheel, SpriteName.WarsHammer, SpriteName.WarsResource_Wagon4Wheel, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Wagon4Wheel.ToLowerInvariant()), ItemResourceType.Wagon4Wheel, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftWagonClosed] = new WorkProperty(WorkPriorityType.craftWagonClosed, SpriteName.WarsHammer, SpriteName.WarsResource_WagonClosed, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_WagonClosed.ToLowerInvariant()), ItemResourceType.WagonClosed, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftWagonIron] = new WorkProperty(WorkPriorityType.craftWagonIron, SpriteName.WarsHammer, SpriteName.WarsResource_WagonIron, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_WagonIron.ToLowerInvariant()), ItemResourceType.WagonIron, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftWagonSteel] = new WorkProperty(WorkPriorityType.craftWagonSteel, SpriteName.WarsHammer, SpriteName.WarsResource_WagonSteel, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_WagonSteel.ToLowerInvariant()), ItemResourceType.WagonSteel, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.craftBlackPowder] = new WorkProperty(WorkPriorityType.craftBlackPowder, SpriteName.WarsHammer, SpriteName.WarsResource_BlackPowder, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BlackPowder.ToLowerInvariant()), ItemResourceType.BlackPowder, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftGunPowder] = new WorkProperty(WorkPriorityType.craftGunPowder, SpriteName.WarsHammer, SpriteName.WarsResource_GunPowder, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_GunPowder.ToLowerInvariant()), ItemResourceType.GunPowder, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftBullet] = new WorkProperty(WorkPriorityType.craftBullet, SpriteName.WarsHammer, SpriteName.WarsResource_Bullets, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_LedBullet.ToLowerInvariant()), ItemResourceType.LedBullet, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.autoBuild] = new WorkProperty(WorkPriorityType.autoBuild, SpriteName.AutomationGearIcon, SpriteName.warsBuildCategoryHouse, DssRef.lang.Work_AutoBuild, ItemResourceType.NONE, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.buildOrders] = new WorkProperty(WorkPriorityType.buildOrders, SpriteName.WarsHammer, SpriteName.warsBuildCategoryHouse, DssRef.lang.Build_Order, ItemResourceType.NONE, ItemResourceType.NONE);


            // --- Metals ---
            workProperties[(int)WorkPriorityType.bogiron] = new WorkProperty(WorkPriorityType.bogiron, SpriteName.WarsWorkCollect, SpriteName.WarsResource_IronOre, DssRef.lang.Resource_TypeName_BogIron, ItemResourceType.IronOre_G, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.miningIron] = new WorkProperty(WorkPriorityType.miningIron, SpriteName.WarsWorkMine, SpriteName.WarsResource_Iron, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Iron.ToLowerInvariant()), ItemResourceType.IronOre_G, ItemResourceType.NONE)
            { relatedMine = ItemResourceType.IronOre_G };

            workProperties[(int)WorkPriorityType.miningTin] = new WorkProperty(WorkPriorityType.miningTin, SpriteName.WarsWorkMine, SpriteName.WarsResource_Tin, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Tin.ToLowerInvariant()), ItemResourceType.TinOre, ItemResourceType.NONE)
            { relatedMine = ItemResourceType.TinOre };

            workProperties[(int)WorkPriorityType.miningCopper] = new WorkProperty(WorkPriorityType.miningCopper, SpriteName.WarsWorkMine, SpriteName.WarsResource_Copper, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Copper.ToLowerInvariant()), ItemResourceType.CopperOre, ItemResourceType.NONE)
            { relatedMine = ItemResourceType.CopperOre };

            workProperties[(int)WorkPriorityType.miningLead] = new WorkProperty(WorkPriorityType.miningLead, SpriteName.WarsWorkMine, SpriteName.WarsResource_Lead, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Lead.ToLowerInvariant()), ItemResourceType.LeadOre, ItemResourceType.NONE)
            { relatedMine = ItemResourceType.LeadOre };

            workProperties[(int)WorkPriorityType.miningSilver] = new WorkProperty(WorkPriorityType.miningSilver, SpriteName.WarsWorkMine, SpriteName.WarsResource_Silver, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Silver.ToLowerInvariant()), ItemResourceType.SilverOre, ItemResourceType.NONE)
            { relatedMine = ItemResourceType.SilverOre };

            workProperties[(int)WorkPriorityType.miningGold] = new WorkProperty(WorkPriorityType.miningGold, SpriteName.WarsWorkMine, SpriteName.WarsResource_Gold, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.ResourceType_Gold.ToLowerInvariant()), ItemResourceType.GoldOre, ItemResourceType.NONE)
            { relatedMine = ItemResourceType.GoldOre };

            workProperties[(int)WorkPriorityType.miningMithril] = new WorkProperty(WorkPriorityType.miningMithril, SpriteName.WarsWorkMine, SpriteName.WarsResource_Mithril, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Mithril.ToLowerInvariant()), ItemResourceType.RawMithril, ItemResourceType.NONE)
            { relatedMine = ItemResourceType.RawMithril };

            workProperties[(int)WorkPriorityType.miningSulfur] = new WorkProperty(WorkPriorityType.miningSulfur, SpriteName.WarsWorkMine, SpriteName.WarsResource_Sulfur, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Sulfur.ToLowerInvariant()), ItemResourceType.Sulfur, ItemResourceType.NONE)
            { relatedMine = ItemResourceType.Sulfur };

            workProperties[(int)WorkPriorityType.miningCoal] = new WorkProperty(WorkPriorityType.miningCoal, SpriteName.WarsWorkMine, SpriteName.WarsResource_Fuel, string.Format(DssRef.lang.Work_MiningResource, DssRef.lang.Resource_TypeName_Coal.ToLowerInvariant()), ItemResourceType.Fuel_G, ItemResourceType.NONE)
            { relatedMine = ItemResourceType.Fuel_G };

            workProperties[(int)WorkPriorityType.smeltIron] = new WorkProperty(WorkPriorityType.smeltIron, SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Iron, string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Iron.ToLowerInvariant()), ItemResourceType.Iron_G, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.smeltTin] = new WorkProperty(WorkPriorityType.smeltTin, SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Tin, string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Tin.ToLowerInvariant()), ItemResourceType.Tin, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.smeltCopper] = new WorkProperty(WorkPriorityType.smeltCopper, SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Copper, string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Copper.ToLowerInvariant()), ItemResourceType.Copper, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.smeltLead] = new WorkProperty(WorkPriorityType.smeltLead, SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Lead, string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Lead.ToLowerInvariant()), ItemResourceType.Lead, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.smeltSilver] = new WorkProperty(WorkPriorityType.smeltSilver, SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Silver, string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.Resource_TypeName_Silver.ToLowerInvariant()), ItemResourceType.Silver, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.smeltGold] = new WorkProperty(WorkPriorityType.smeltGold, SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Gold, string.Format(DssRef.lang.Work_SmeltX, DssRef.lang.ResourceType_Gold.ToLowerInvariant()), ItemResourceType.Gold, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.craftBronze] = new WorkProperty(WorkPriorityType.craftBronze, SpriteName.WarsHammer, SpriteName.WarsResource_Bronze, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Bronze.ToLowerInvariant()), ItemResourceType.Bronze, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftCastIron] = new WorkProperty(WorkPriorityType.craftCastIron, SpriteName.WarsHammer, SpriteName.WarsResource_CastIron, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_CastIron.ToLowerInvariant()), ItemResourceType.CastIron, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftBloomeryIron] = new WorkProperty(WorkPriorityType.craftBloomeryIron, SpriteName.WarsHammer, SpriteName.WarsResource_BloomeryIron, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BloomIron.ToLowerInvariant()), ItemResourceType.BloomeryIron, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftSteel] = new WorkProperty(WorkPriorityType.craftSteel, SpriteName.WarsHammer, SpriteName.WarsResource_Steel, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Steel.ToLowerInvariant()), ItemResourceType.Steel, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftMithril] = new WorkProperty(WorkPriorityType.craftMithril, SpriteName.WarsHammer, SpriteName.WarsResource_MithrilAlloy, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Mithril.ToLowerInvariant()), ItemResourceType.Mithril, ItemResourceType.NONE);


            // --- Weapons ---
            workProperties[(int)WorkPriorityType.craftSharpStick] = new WorkProperty(WorkPriorityType.craftSharpStick, SpriteName.WarsHammer, SpriteName.WarsResource_Sharpstick, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SharpStick.ToLowerInvariant()), ItemResourceType.SharpStick, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftBronzeSword] = new WorkProperty(WorkPriorityType.craftBronzeSword, SpriteName.WarsHammer, SpriteName.WarsResource_BronzeSword, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BronzeSword.ToLowerInvariant()), ItemResourceType.BronzeSword, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftShortSword] = new WorkProperty(WorkPriorityType.craftShortSword, SpriteName.WarsHammer, SpriteName.WarsResource_ShortSword, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_ShortSword.ToLowerInvariant()), ItemResourceType.ShortSword, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftSword] = new WorkProperty(WorkPriorityType.craftSword, SpriteName.WarsHammer, SpriteName.WarsResource_Sword, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Sword.ToLowerInvariant()), ItemResourceType.Sword, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftLongSword] = new WorkProperty(WorkPriorityType.craftLongSword, SpriteName.WarsHammer, SpriteName.WarsResource_Longsword, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_LongSword.ToLowerInvariant()), ItemResourceType.LongSword, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftHandSpear] = new WorkProperty(WorkPriorityType.craftHandSpear, SpriteName.WarsHammer, SpriteName.WarsResource_HandSpear, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HandSpear.ToLowerInvariant()), ItemResourceType.HandSpear, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.craftWarhammer] = new WorkProperty(WorkPriorityType.craftWarhammer, SpriteName.WarsHammer, SpriteName.WarsResource_Warhammer, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Warhammer.ToLowerInvariant()), ItemResourceType.Warhammer, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftTwoHandSword] = new WorkProperty(WorkPriorityType.craftTwoHandSword, SpriteName.WarsHammer, SpriteName.WarsResource_TwoHandSword, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_TwoHandSword.ToLowerInvariant()), ItemResourceType.TwoHandSword, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftMithrilSword] = new WorkProperty(WorkPriorityType.craftMithrilSword, SpriteName.WarsHammer, SpriteName.WarsResource_MithrilSword, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_MithrilSword.ToLowerInvariant()), ItemResourceType.MithrilSword, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.craftBucklerShield] = new WorkProperty(WorkPriorityType.craftBucklerShield, SpriteName.WarsHammer, SpriteName.WarsResource_BucklerShield, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BucklerShield), ItemResourceType.BucklerShield, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftRoundShield] = new WorkProperty(WorkPriorityType.craftRoundShield, SpriteName.WarsHammer, SpriteName.WarsResource_RoundShield, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_RoundShield), ItemResourceType.RoundShield, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftHeaterShield] = new WorkProperty(WorkPriorityType.craftHeaterShield, SpriteName.WarsHammer, SpriteName.WarsResource_HeaterShield, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HeaterShield), ItemResourceType.HeaterShield, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftTowerShield] = new WorkProperty(WorkPriorityType.craftTowerShield, SpriteName.WarsHammer, SpriteName.WarsResource_TowerShield, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_TowerShield), ItemResourceType.TowerShield, ItemResourceType.NONE);


            // --- Projectile ---
            workProperties[(int)WorkPriorityType.craftSlingshot] = new WorkProperty(WorkPriorityType.craftSlingshot, SpriteName.WarsHammer, SpriteName.WarsResource_Slingshot, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SlingShot.ToLowerInvariant()), ItemResourceType.SlingShot, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftThrowingspear] = new WorkProperty(WorkPriorityType.craftThrowingspear, SpriteName.WarsHammer, SpriteName.WarsResource_ThrowSpear, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_ThrowingSpear.ToLowerInvariant()), ItemResourceType.ThrowingSpear, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftBow] = new WorkProperty(WorkPriorityType.craftBow, SpriteName.WarsHammer, SpriteName.WarsResource_Bow, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Bow.ToLowerInvariant()), ItemResourceType.Bow, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftLongbow] = new WorkProperty(WorkPriorityType.craftLongbow, SpriteName.WarsHammer, SpriteName.WarsResource_Longbow, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Longbow.ToLowerInvariant()), ItemResourceType.LongBow, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftCrossbow] = new WorkProperty(WorkPriorityType.craftCrossbow, SpriteName.WarsHammer, SpriteName.WarsResource_Crossbow, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Crossbow.ToLowerInvariant()), ItemResourceType.Crossbow, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftMithrilbow] = new WorkProperty(WorkPriorityType.craftMithrilbow, SpriteName.WarsHammer, SpriteName.WarsResource_Mithrilbow, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_MithrilBow.ToLowerInvariant()), ItemResourceType.MithrilBow, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.craftHandCannon] = new WorkProperty(WorkPriorityType.craftHandCannon, SpriteName.WarsHammer, SpriteName.WarsResource_BronzeRifle, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HandCannon.ToLowerInvariant()), ItemResourceType.HandCannon, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftHandCulverin] = new WorkProperty(WorkPriorityType.craftHandCulverin, SpriteName.WarsHammer, SpriteName.WarsResource_BronzeShotgun, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HandCulverin.ToLowerInvariant()), ItemResourceType.HandCulverin, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftRifle] = new WorkProperty(WorkPriorityType.craftRifle, SpriteName.WarsHammer, SpriteName.WarsResource_IronRifle, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Rifle.ToLowerInvariant()), ItemResourceType.Rifle, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftBlunderbuss] = new WorkProperty(WorkPriorityType.craftBlunderbuss, SpriteName.WarsHammer, SpriteName.WarsResource_IronShotgun, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Blunderbuss.ToLowerInvariant()), ItemResourceType.Blunderbuss, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.craftBallista] = new WorkProperty(WorkPriorityType.craftBallista, SpriteName.WarsHammer, SpriteName.WarsResource_Ballista, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.UnitType_Ballista.ToLowerInvariant()), ItemResourceType.Ballista, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftManuBallista] = new WorkProperty(WorkPriorityType.craftManuBallista, SpriteName.WarsHammer, SpriteName.WarsResource_Manuballista, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Manuballista.ToLowerInvariant()), ItemResourceType.Manuballista, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftCatapult] = new WorkProperty(WorkPriorityType.craftCatapult, SpriteName.WarsHammer, SpriteName.WarsResource_Catapult, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Catapult.ToLowerInvariant()), ItemResourceType.Catapult, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.craftSiegeCannonBronze] = new WorkProperty(WorkPriorityType.craftSiegeCannonBronze, SpriteName.WarsHammer, SpriteName.WarsResource_BronzeSiegeCannon, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SiegeCannonBronze.ToLowerInvariant()), ItemResourceType.SiegeCannonBronze, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftManCannonBronze] = new WorkProperty(WorkPriorityType.craftManCannonBronze, SpriteName.WarsHammer, SpriteName.WarsResource_BronzeManCannon, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_ManCannonBronze.ToLowerInvariant()), ItemResourceType.ManCannonBronze, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftSiegeCannonIron] = new WorkProperty(WorkPriorityType.craftSiegeCannonIron, SpriteName.WarsHammer, SpriteName.WarsResource_IronSiegeCannon, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SiegeCannonIron.ToLowerInvariant()), ItemResourceType.SiegeCannonIron, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftManCannonIron] = new WorkProperty(WorkPriorityType.craftManCannonIron, SpriteName.WarsHammer, SpriteName.WarsResource_IronManCannon, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_ManCannonIron.ToLowerInvariant()), ItemResourceType.ManCannonIron, ItemResourceType.NONE);


            // --- Armor ---
            workProperties[(int)WorkPriorityType.craftPaddedArmor] = new WorkProperty(WorkPriorityType.craftPaddedArmor, SpriteName.WarsHammer, SpriteName.WarsResource_PaddedArmor, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_PaddedArmor.ToLowerInvariant()), ItemResourceType.PaddedArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftHeavyPaddedArmor] = new WorkProperty(WorkPriorityType.craftHeavyPaddedArmor, SpriteName.WarsHammer, SpriteName.WarsResource_HeavyPaddedArmor, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HeavyPaddedArmor.ToLowerInvariant()), ItemResourceType.HeavyPaddedArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftBronzeArmor] = new WorkProperty(WorkPriorityType.craftBronzeArmor, SpriteName.WarsHammer, SpriteName.WarsResource_BronzeArmor, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_BronzeArmor.ToLowerInvariant()), ItemResourceType.BronzeArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftMailArmor] = new WorkProperty(WorkPriorityType.craftMailArmor, SpriteName.WarsHammer, SpriteName.WarsResource_IronArmor, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_IronArmor.ToLowerInvariant()), ItemResourceType.IronArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftHeavyMailArmor] = new WorkProperty(WorkPriorityType.craftHeavyMailArmor, SpriteName.WarsHammer, SpriteName.WarsResource_HeavyIronArmor, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HeavyIronArmor.ToLowerInvariant()), ItemResourceType.HeavyIronArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftPlateArmor] = new WorkProperty(WorkPriorityType.craftPlateArmor, SpriteName.WarsHammer, SpriteName.WarsResource_LightPlateArmor, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_LightPlateArmor.ToLowerInvariant()), ItemResourceType.LightPlateArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftFullPlateArmor] = new WorkProperty(WorkPriorityType.craftFullPlateArmor, SpriteName.WarsHammer, SpriteName.WarsResource_FullPlateArmor, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_FullPlateArmor.ToLowerInvariant()), ItemResourceType.FullPlateArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftMithrilArmor] = new WorkProperty(WorkPriorityType.craftMithrilArmor, SpriteName.WarsHammer, SpriteName.WarsResource_MithrilArmor, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_MithrilArmor.ToLowerInvariant()), ItemResourceType.MithrilArmor, ItemResourceType.NONE);

            workProperties[(int)WorkPriorityType.craftMountPaddedArmor] = new WorkProperty(WorkPriorityType.craftMountPaddedArmor, SpriteName.WarsHammer, SpriteName.WarsResource_MountPaddedArmor, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.lang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_PaddedArmor.ToLowerInvariant())), ItemResourceType.MountPaddedArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftMountHeavyPaddedArmor] = new WorkProperty(WorkPriorityType.craftMountHeavyPaddedArmor, SpriteName.WarsHammer, SpriteName.WarsResource_MountHeavyPaddedArmor, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.lang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_HeavyPaddedArmor.ToLowerInvariant())), ItemResourceType.MountHeavyPaddedArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftMountBronzeArmor] = new WorkProperty(WorkPriorityType.craftMountBronzeArmor, SpriteName.WarsHammer, SpriteName.WarsResource_MountBronzeArmor, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.lang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_BronzeArmor.ToLowerInvariant())), ItemResourceType.MountBronzeArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftMountMailArmor] = new WorkProperty(WorkPriorityType.craftMountMailArmor, SpriteName.WarsHammer, SpriteName.WarsResource_MountIronArmor, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.lang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_IronArmor.ToLowerInvariant())), ItemResourceType.MountIronArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftMountHeavyMailArmor] = new WorkProperty(WorkPriorityType.craftMountHeavyMailArmor, SpriteName.WarsHammer, SpriteName.WarsResource_MountHeavyIronArmor, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.lang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_HeavyIronArmor.ToLowerInvariant())), ItemResourceType.MountHeavyIronArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftMountPlateArmor] = new WorkProperty(WorkPriorityType.craftMountPlateArmor, SpriteName.WarsHammer, SpriteName.WarsResource_MountLightPlateArmor, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.lang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_LightPlateArmor.ToLowerInvariant())), ItemResourceType.MountLightPlateArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftMountFullPlateArmor] = new WorkProperty(WorkPriorityType.craftMountFullPlateArmor, SpriteName.WarsHammer, SpriteName.WarsResource_MountFullPlateArmor, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.lang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_FullPlateArmor.ToLowerInvariant())), ItemResourceType.MountFullPlateArmor, ItemResourceType.NONE);
            workProperties[(int)WorkPriorityType.craftMountMithrilArmor] = new WorkProperty(WorkPriorityType.craftMountMithrilArmor, SpriteName.WarsHammer, SpriteName.WarsResource_MountMithrilArmor, string.Format(DssRef.lang.Work_CraftX, string.Format(DssRef.lang.Resource_TypeName_MountArmorX, DssRef.lang.Resource_TypeName_MithrilArmor.ToLowerInvariant())), ItemResourceType.MountMithrilArmor, ItemResourceType.NONE);


            // --- Animals (Requires WorkViewMode and fullStock options) ---
            workProperties[(int)WorkPriorityType.SlaughterFowl] = new WorkProperty(WorkPriorityType.SlaughterFowl, SpriteName.WarsSlaughter, SpriteName.WarsResource_Fowl, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_Fowl), ItemResourceType.Fowl, ItemResourceType.SlaughterFowl)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterHen] = new WorkProperty(WorkPriorityType.SlaughterHen, SpriteName.WarsSlaughter, SpriteName.WarsResource_Hen, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_Hen), ItemResourceType.Hen, ItemResourceType.SlaughterHen)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterBoar] = new WorkProperty(WorkPriorityType.SlaughterBoar, SpriteName.WarsSlaughter, SpriteName.WarsResource_Boar, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_Boar), ItemResourceType.Boar, ItemResourceType.SlaughterBoar)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterPig] = new WorkProperty(WorkPriorityType.SlaughterPig, SpriteName.WarsSlaughter, SpriteName.WarsResource_Pig, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_Pig), ItemResourceType.Pig, ItemResourceType.SlaughterPig)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterOxen] = new WorkProperty(WorkPriorityType.SlaughterOxen, SpriteName.WarsSlaughter, SpriteName.WarsResource_Oxen, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_Oxen), ItemResourceType.Oxen, ItemResourceType.SlaughterOxen)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterKineOxen] = new WorkProperty(WorkPriorityType.SlaughterKineOxen, SpriteName.WarsSlaughter, SpriteName.WarsResource_KineOxen, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_KineOxen), ItemResourceType.KineOxen, ItemResourceType.SlaughterKineOxen)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterPony] = new WorkProperty(WorkPriorityType.SlaughterPony, SpriteName.WarsSlaughter, SpriteName.WarsResource_Pony, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_Pony), ItemResourceType.Pony, ItemResourceType.SlaughterPony)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterHorse] = new WorkProperty(WorkPriorityType.SlaughterHorse, SpriteName.WarsSlaughter, SpriteName.WarsResource_Horse, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_Horse), ItemResourceType.Horse, ItemResourceType.SlaughterHorse)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterWarHorse] = new WorkProperty(WorkPriorityType.SlaughterWarHorse, SpriteName.WarsSlaughter, SpriteName.WarsResource_WarHorse, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_WarHorse), ItemResourceType.WarHorse, ItemResourceType.SlaughterWarHorse)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterDraftHorse] = new WorkProperty(WorkPriorityType.SlaughterDraftHorse, SpriteName.WarsSlaughter, SpriteName.WarsResource_DraftHorse, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_DraftHorse), ItemResourceType.DraftHorse, ItemResourceType.SlaughterDraftHorse)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterWildPig] = new WorkProperty(WorkPriorityType.SlaughterWildPig, SpriteName.WarsSlaughter, SpriteName.WarsResource_WildPig, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_WildPig), ItemResourceType.WildPig, ItemResourceType.SlaughterWildPig)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterWildHog] = new WorkProperty(WorkPriorityType.SlaughterWildHog, SpriteName.WarsSlaughter, SpriteName.WarsResource_WildHog, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_WildHog), ItemResourceType.WildHog, ItemResourceType.SlaughterWildHog)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterWarHog] = new WorkProperty(WorkPriorityType.SlaughterWarHog, SpriteName.WarsSlaughter, SpriteName.WarsResource_WarHog, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_WarHog), ItemResourceType.WarHog, ItemResourceType.SlaughterWarHog)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterStagHog] = new WorkProperty(WorkPriorityType.SlaughterStagHog, SpriteName.WarsSlaughter, SpriteName.WarsResource_StagHog, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_StagHog), ItemResourceType.StagHog, ItemResourceType.SlaughterStagHog)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterWolf] = new WorkProperty(WorkPriorityType.SlaughterWolf, SpriteName.WarsSlaughter, SpriteName.WarsResource_Wolf, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_Wolf), ItemResourceType.Wolf, ItemResourceType.SlaughterWolf)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterWarg] = new WorkProperty(WorkPriorityType.SlaughterWarg, SpriteName.WarsSlaughter, SpriteName.WarsResource_Warg, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_Warg), ItemResourceType.Warg, ItemResourceType.SlaughterWarg)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterAlphaWarg] = new WorkProperty(WorkPriorityType.SlaughterAlphaWarg, SpriteName.WarsSlaughter, SpriteName.WarsResource_AlphaWarg, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_AlphaWarg), ItemResourceType.AlphaWarg, ItemResourceType.SlaughterAlphaWarg)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterWildCat] = new WorkProperty(WorkPriorityType.SlaughterWildCat, SpriteName.WarsSlaughter, SpriteName.WarsResource_WildCat, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_WildCat), ItemResourceType.WildCat, ItemResourceType.SlaughterWildCat)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterLion] = new WorkProperty(WorkPriorityType.SlaughterLion, SpriteName.WarsSlaughter, SpriteName.WarsResource_Lion, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_Lion), ItemResourceType.Lion, ItemResourceType.SlaughterLion)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterWarLion] = new WorkProperty(WorkPriorityType.SlaughterWarLion, SpriteName.WarsSlaughter, SpriteName.WarsResource_WarLion, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_WarLion), ItemResourceType.WarLion, ItemResourceType.SlaughterWarLion)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterElephant] = new WorkProperty(WorkPriorityType.SlaughterElephant, SpriteName.WarsSlaughter, SpriteName.WarsResource_Elephant, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_Elephant), ItemResourceType.Elephant, ItemResourceType.SlaughterElephant)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterWarElephant] = new WorkProperty(WorkPriorityType.SlaughterWarElephant, SpriteName.WarsSlaughter, SpriteName.WarsResource_WarElephant, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_WarElephant), ItemResourceType.WarElephant, ItemResourceType.SlaughterWarElephant)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.SlaughterOliphant] = new WorkProperty(WorkPriorityType.SlaughterOliphant, SpriteName.WarsSlaughter, SpriteName.WarsResource_Oliphant, string.Format(DssRef.lang.Work_SlaughterX, DssRef.lang.Resource_TypeName_Oliphant), ItemResourceType.Oliphant, ItemResourceType.SlaughterOliphant)
            { workView = WorkViewMode.Slaughter, waitForFullStockOption = true };


            // --- Minting (Requires fullStock option) ---
            workProperties[(int)WorkPriorityType.coinmaker_copper] = new WorkProperty(WorkPriorityType.coinmaker_copper, SpriteName.WarsHammer, SpriteName.WarsResource_CopperCoin, string.Format("{0} - {1}", string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Coin), DssRef.lang.Resource_TypeName_Copper), ItemResourceType.NONE, ItemResourceType.NONE)
            { waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.coinmaker_bronze] = new WorkProperty(WorkPriorityType.coinmaker_bronze, SpriteName.WarsHammer, SpriteName.WarsResource_BonzeCoin, string.Format("{0} - {1}", string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Coin), DssRef.lang.Resource_TypeName_Bronze), ItemResourceType.NONE, ItemResourceType.NONE)
            { waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.coinmaker_silver] = new WorkProperty(WorkPriorityType.coinmaker_silver, SpriteName.WarsHammer, SpriteName.WarsResource_SilverCoin, string.Format("{0} - {1}", string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Coin), DssRef.lang.Resource_TypeName_Silver), ItemResourceType.NONE, ItemResourceType.NONE)
            { waitForFullStockOption = true };

            workProperties[(int)WorkPriorityType.coinmaker_mithril] = new WorkProperty(WorkPriorityType.coinmaker_mithril, SpriteName.WarsHammer, SpriteName.WarsResource_ElfCoin, string.Format("{0} - {1}", string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Coin), DssRef.lang.Resource_TypeName_Mithril), ItemResourceType.NONE, ItemResourceType.NONE)
            { waitForFullStockOption = true };
        }
    }

}
