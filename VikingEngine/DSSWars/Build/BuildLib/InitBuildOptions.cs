using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Build
{
    static class InitBuildOptions
    {
        public static void Init()
        {
            BuildLib.TerrainToBuildType.Clear();
            InitBuildings();
        }

        public static void InitRuleSet()
        {
            BuildLib.BuildOptions[(int)BuildAndExpandType.Logistics].specialBuildRequirement = DssRef.storage.ruleset_instance.module_foodAndWater ? SpecialBuildRequirement.FoodStorage : SpecialBuildRequirement.None;
        }

        public static void InitBuildings()
        {
            new BuildOption(BuildAndExpandType.Logistics, TerrainMainType.Building, (int)TerrainBuildingType.Logistics, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_Logistics, DssRef.lang.BuildingType_Logistics, DssRef.lang.BuildingType_Logistics_Description,
                CraftBuildingLib.CraftLogistics, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Upgrade, BuildFilterTag.Craft, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.JustOne, DssConst.WorkTime_Building_Default)
            {
                uniqueBuilding = true,
                //specialBuildRequirement = DssRef.storage.ruleset_instance.module_foodAndWater? SpecialBuildRequirement.FoodStorage : SpecialBuildRequirement.None,
            };

            new BuildOption(BuildAndExpandType.Economics, TerrainMainType.Building, (int)TerrainBuildingType.Economics, BuildingGroundRequirement.BuildMedium,
                SpriteName.MissingImage, DssRef.todoLang.BuildingType_Economics, null,
                CraftBuildingLib.CraftEconomics, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Upgrade, BuildFilterTag.Transport, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.JustOne, DssConst.WorkTime_Building_Default)
            {
                uniqueBuilding = true
            };

            new BuildOption(BuildAndExpandType.ManorLord, TerrainMainType.Building, (int)TerrainBuildingType.ManorLord, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_ManorLord, DssRef.lang.BuildingType_ManorLord, DssRef.lang.BuildingType_ManorLord_Description,
                CraftBuildingLib.ManorLord, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Upgrade, BuildFilterTag.Farm, BuildFilterTag.Food,
                MapPaintToolCategory.JustOne, DssConst.WorkTime_Building_Default)
            {
                uniqueBuilding = true
            };

            new BuildOption(BuildAndExpandType.GreatHall, TerrainMainType.Building, (int)TerrainBuildingType.GreatHall, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_GreatHall, DssRef.lang.BuildingType_GreatHall, DssRef.lang.BuildingType_GreatHall_Description,
                CraftBuildingLib.GreatHall, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Upgrade, BuildFilterTag.Military, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.JustOne, DssConst.WorkTime_Building_Default)
            {
                uniqueBuilding = true
            };

            new BuildOption(BuildAndExpandType.TingsHouse, TerrainMainType.Building, (int)TerrainBuildingType.TingsHouse, BuildingGroundRequirement.BuildMedium,
                SpriteName.MissingImage, DssRef.todoLang.BuildingType_TingsHouse, null,
                CraftBuildingLib.CraftTingsHouse, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Upgrade, BuildFilterTag.Transport, BuildFilterTag.Military,
                MapPaintToolCategory.JustOne, DssConst.WorkTime_Building_Large)
            {
                uniqueBuilding = true
            };

            new BuildOption(BuildAndExpandType.ExpansionHall_Lvl1, TerrainMainType.Building, (int)TerrainBuildingType.ExpansionHall_Lvl1, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsCityHall, DssRef.todoLang.BuildingType_ExpansionHall, null,
                CraftBuildingLib.ExpansionHall_Lvl1, false,
                BuildCategoryTab.Upgrade, BuildFilterTag.Upgrade, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { buildInArea = BuildInAreaOptions.OutsideBorders, specialBuildRequirement = SpecialBuildRequirement.MaxExpansions };

            new BuildOption(BuildAndExpandType.ExpansionHall_Lvl2, TerrainMainType.Building, (int)TerrainBuildingType.ExpansionHall_Lvl2, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsCityHall, DssRef.todoLang.BuildingType_ExpansionHall, null,
                CraftBuildingLib.ExpansionHall_Lvl2, false,
                BuildCategoryTab.Upgrade, BuildFilterTag.Upgrade, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { buildInArea = BuildInAreaOptions.OutsideBorders, specialBuildRequirement = SpecialBuildRequirement.MaxExpansions };

            new BuildOption(BuildAndExpandType.ExpansionHall_Lvl3, TerrainMainType.Building, (int)TerrainBuildingType.ExpansionHall_Lvl3, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsCityHall, DssRef.todoLang.BuildingType_ExpansionHall, null,
                CraftBuildingLib.ExpansionHall_Lvl3, false,
                BuildCategoryTab.Upgrade, BuildFilterTag.Upgrade, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large)
            { buildInArea = BuildInAreaOptions.OutsideBorders, specialBuildRequirement = SpecialBuildRequirement.MaxExpansions };

            new BuildOption(BuildAndExpandType.WorkerTent, TerrainMainType.Building, (int)TerrainBuildingType.WorkerTent, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_TentHut, DssRef.lang.BuildingType_WorkerHut, string.Format(DssRef.lang.BuildingType_WorkerHut_DescriptionLimitX, DssConst.HousingCount_WorkerTent),
                CraftBuildingLib.WorkerTent, true,
                BuildCategoryTab.General, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_WorkerTent);

            new BuildOption(BuildAndExpandType.WorkerHut, TerrainMainType.Building, (int)TerrainBuildingType.WorkerHut, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_WorkerHuts, DssRef.lang.BuildingType_WorkerHut, string.Format(DssRef.lang.BuildingType_WorkerHut_DescriptionLimitX, DssConst.HousingCount_WorkerHut),
                CraftBuildingLib.WorkerHut, true,
                BuildCategoryTab.General, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.WorkerHutLarge, TerrainMainType.Building, (int)TerrainBuildingType.WorkerHutLarge, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_WorkerHutLarge, DssRef.lang.BuildingType_WorkerHut, string.Format(DssRef.lang.BuildingType_WorkerHut_DescriptionLimitX, DssConst.HousingCount_WorkerHutLarge),
                CraftBuildingLib.WorkerHutLarge, false,
                BuildCategoryTab.General, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.ServiceHouse_Small, TerrainMainType.Building, (int)TerrainBuildingType.ServiceMenHouse_Small, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_SmallServiceHouse, DssRef.lang.BuildingType_ServiceHouse, string.Format(DssRef.lang.BuildingType_ServiceHouse_DescriptionAddX, DssConst.HousingCount_ServiceHouse_Small),
                CraftBuildingLib.ServiceHouse_Small, true,
                BuildCategoryTab.General, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.ServiceHouse_Large, TerrainMainType.Building, (int)TerrainBuildingType.ServiceMenHouse_Large, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_BigServiceHouse, DssRef.lang.BuildingType_ServiceHouse, string.Format(DssRef.lang.BuildingType_ServiceHouse_DescriptionAddX, DssConst.HousingCount_ServiceHouse_Large),
                CraftBuildingLib.ServiceHouse_Large, false,
                BuildCategoryTab.General, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.SoldiersQuarter_Small, TerrainMainType.Building, (int)TerrainBuildingType.SoldiersQuarter_Small, BuildingGroundRequirement.BuildLight,
                SpriteName.MissingImage, DssRef.todoLang.BuildingType_SoldiersQuarter, null,
                CraftBuildingLib.SoldiersQuarter_Small, true,
                BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.Upgrade, MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { specialBuildRequirement = SpecialBuildRequirement.MaxSoldierQuarters };

            new BuildOption(BuildAndExpandType.SoldiersQuarter_Large, TerrainMainType.Building, (int)TerrainBuildingType.SoldiersQuarter_Large, BuildingGroundRequirement.BuildMedium,
                SpriteName.MissingImage, DssRef.todoLang.BuildingType_SoldiersQuarter, null,
                CraftBuildingLib.SoldiersQuarter_Large, false,
                BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.Upgrade, MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { specialBuildRequirement = SpecialBuildRequirement.MaxSoldierQuarters };

            new BuildOption(BuildAndExpandType.GuardHouse_Small, TerrainMainType.Building, (int)TerrainBuildingType.GuardHouse_Small, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_GuardOffice, DssRef.lang.BuildingType_GuardOffice, string.Format(DssRef.lang.BuildingType_GuardOffice_DescriptionAddX, DssConst.HousingCount_GuardsOffice_Small),
                CraftBuildingLib.GuardHouse_Small, true,
                BuildCategoryTab.Military, BuildFilterTag.Guards, BuildFilterTag.Military, BuildFilterTag.NUM_NONE, MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GuardHouse_Large, TerrainMainType.Building, (int)TerrainBuildingType.GuardHouse_Large, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_GuardOfficeLarge, DssRef.lang.BuildingType_GuardOffice, string.Format(DssRef.lang.BuildingType_GuardOffice_DescriptionAddX, DssConst.HousingCount_GuardsOffice_Large),
                CraftBuildingLib.GuardHouse_Large, false,
                BuildCategoryTab.Military, BuildFilterTag.Guards, BuildFilterTag.Military, BuildFilterTag.NUM_NONE, MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.ImmigrationTent, TerrainMainType.Building, (int)TerrainBuildingType.ImmigrationTent, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Tent, DssRef.lang.BuildingType_ImmigrationTent, string.Format(DssRef.lang.BuildingType_ImmigrationTent_Description, DssConst.ImmigrantionTent_Capacity),
                CraftBuildingLib.ImmigrationTent, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Workers, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.Postal, TerrainMainType.Building, (int)TerrainBuildingType.Postal, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Postal, DssRef.lang.BuildingType_Postal, DssRef.lang.BuildingType_Postal_Description,
                CraftBuildingLib.Postal, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.PostalLevel2, TerrainMainType.Building, (int)TerrainBuildingType.PostalLevel2, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_PostalLevel2, string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_Postal), DssRef.lang.BuildingType_Postal_Description,
                CraftBuildingLib.Postal_Level2, false,
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.PostalLevel3, TerrainMainType.Building, (int)TerrainBuildingType.PostalLevel3, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_PostalLevel3, string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_Postal), DssRef.lang.BuildingType_Postal_Description,
                CraftBuildingLib.Postal_Level3, false,
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.Recruitment, TerrainMainType.Building, (int)TerrainBuildingType.Recruitment, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Recruitment, DssRef.lang.BuildingType_Recruitment, DssRef.lang.BuildingType_Recruitment_Description,
                CraftBuildingLib.Recruitment, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.RecruitmentLevel2, TerrainMainType.Building, (int)TerrainBuildingType.RecruitmentLevel2, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_RecruitmentLevel2, string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_Recruitment), DssRef.lang.BuildingType_Recruitment_Description,
                CraftBuildingLib.Recruitment_Level2, false,
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.RecruitmentLevel3, TerrainMainType.Building, (int)TerrainBuildingType.RecruitmentLevel3, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_RecruitmentLevel3, string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_Recruitment), DssRef.lang.BuildingType_Recruitment_Description,
                CraftBuildingLib.Recruitment_Level3, false,
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.GoldDeliveryLvl1, TerrainMainType.Building, (int)TerrainBuildingType.GoldDeliveryLevel1, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_GoldDeliver, DssRef.lang.BuildingType_GoldDelivery, DssRef.lang.BuildingType_GoldDelivery_Description,
                CraftBuildingLib.GoldDelivery, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GoldDeliveryLvl2, TerrainMainType.Building, (int)TerrainBuildingType.GoldDeliveryLevel2, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_GoldDeliverLevel2, string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_GoldDelivery), DssRef.lang.BuildingType_GoldDelivery_Description,
                CraftBuildingLib.GoldDelivery_Level2, false,
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.GoldDeliveryLvl3, TerrainMainType.Building, (int)TerrainBuildingType.GoldDeliveryLevel3, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_GoldDeliverLevel3, string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_GoldDelivery), DssRef.lang.BuildingType_GoldDelivery_Description,
                CraftBuildingLib.GoldDelivery_Level3, false,
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.SoldierBarracks, TerrainMainType.Building, (int)TerrainBuildingType.SoldierBarracks, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_SoldierBarracks, DssRef.lang.BuildingType_SoldierBarracks, DssRef.lang.BuildingType_Barracks_Description,
                CraftBuildingLib.SoldierBarracks, true,
                BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.ArcherBarracks, TerrainMainType.Building, (int)TerrainBuildingType.ArcherBarracks, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_ArcherBarracks, DssRef.lang.BuildingType_ArcherBarracks, DssRef.lang.BuildingType_Barracks_Description,
                CraftBuildingLib.ArcherBarracks, true,
                BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.WarmachineBarracks, TerrainMainType.Building, (int)TerrainBuildingType.WarmachineBarracks, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_WarmachineBarracks, DssRef.lang.BuildingType_WarmachineBarracks, DssRef.lang.BuildingType_Barracks_Description,
                CraftBuildingLib.WarmachineBarracks, true,
                BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.GunBarracks, TerrainMainType.Building, (int)TerrainBuildingType.GunBarracks, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_GunBarracks, DssRef.lang.BuildingType_GunBarracks, DssRef.lang.BuildingType_Barracks_Description,
                CraftBuildingLib.GunBarracks, true,
                BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.CannonBarracks, TerrainMainType.Building, (int)TerrainBuildingType.CannonBarracks, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_CannonBarracks, DssRef.lang.BuildingType_CannonBarracks, DssRef.lang.BuildingType_Barracks_Description,
                CraftBuildingLib.CannonBarracks, true,
                BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.AnimalTrainer, TerrainMainType.Building, (int)TerrainBuildingType.AnimalTrainer, BuildingGroundRequirement.BuildMedium,
                SpriteName.MissingImage, DssRef.todoLang.BuildingType_AnimalTrainer, DssRef.todoLang.BuildingType_AnimalTrainer_Description,
                CraftBuildingLib.AnimalTrainer, true,
                BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.Animals,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Noblehouse, TerrainMainType.Building, (int)TerrainBuildingType.Nobelhouse, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_Nobelhouse, DssRef.lang.Building_NobleHouse, DssRef.lang.BuildingType_Nobelhouse_Description,
                CraftBuildingLib.NobleHouse, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Upgrade, BuildFilterTag.Military, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Tavern, TerrainMainType.Building, (int)TerrainBuildingType.Tavern, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_Tavern, DssRef.lang.BuildingType_Tavern, DssRef.lang.BuildingType_Tavern_Description,
                CraftBuildingLib.Tavern, false,
                BuildCategoryTab.Advanced, BuildFilterTag.Food, BuildFilterTag.Workers, BuildFilterTag.Optimize,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.Storehouse, TerrainMainType.Building, (int)TerrainBuildingType.Storehouse, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_Storehouse, DssRef.lang.BuildingType_Storehouse, DssRef.lang.BuildingType_Storehouse_Description,
                CraftBuildingLib.Storehouse, false,
                BuildCategoryTab.Advanced, BuildFilterTag.Resources, BuildFilterTag.Workers, BuildFilterTag.Optimize,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Brewery, TerrainMainType.Building, (int)TerrainBuildingType.Brewery, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Brewery, DssRef.lang.BuildingType_Brewery, DssRef.lang.BuildingType_Tavern_Brewery,
                CraftBuildingLib.Brewery, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Water, BuildFilterTag.Optimize,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.Brewery_Bronze };

            new BuildOption(BuildAndExpandType.TrapperHut, TerrainMainType.Building, (int)TerrainBuildingType.TrappersHut, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Trapper, DssRef.lang.BuildingType_TrapperHut, DssRef.lang.BuildingType_TrapperHut_Description,
                CraftBuildingLib.TrapperHut, false,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Cook, TerrainMainType.Building, (int)TerrainBuildingType.Work_Cook, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Cook, DssRef.lang.BuildingType_Cook, DssRef.lang.BuildingType_Cook_Description,
                CraftBuildingLib.Cook, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.Cook_Copper };

            new BuildOption(BuildAndExpandType.CoalPit, TerrainMainType.Building, (int)TerrainBuildingType.Work_CoalPit, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_CoalPit, DssRef.lang.BuildingType_CoalPit, DssRef.lang.BuildingType_CoalPit_Description,
                CraftBuildingLib.CoalPit, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Fuel, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.WorkBench, TerrainMainType.Building, (int)TerrainBuildingType.Work_Bench, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_WorkBench, DssRef.lang.BuildingType_WorkBench, DssRef.lang.BuildingType_Bench_Description,
                CraftBuildingLib.WorkBench, true,
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.WorkBench_Bronze };

            new BuildOption(BuildAndExpandType.Smith, TerrainMainType.Building, (int)TerrainBuildingType.Work_Smith, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Smith, DssRef.lang.BuildingType_Smith, DssRef.lang.BuildingType_Smith_Description,
                CraftBuildingLib.Smith, true,
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Weapons, BuildFilterTag.Metals,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Carpenter, TerrainMainType.Building, (int)TerrainBuildingType.Carpenter, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Carpenter, DssRef.lang.BuildingType_Carpenter, DssRef.lang.BuildingType_Carpenter_Description,
                CraftBuildingLib.Carpenter, true,
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Weapons, BuildFilterTag.Resources,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.Carpenter_Bronze };

            new BuildOption(BuildAndExpandType.WheatFarm, TerrainMainType.Farm, (int)TerrainSubFarmType.WheatFarm, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_WheatFarms, string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Wheat), null,
                CraftBuildingLib.WheatFarm, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.WheatFarm_Gold };

            new BuildOption(BuildAndExpandType.WheatFarmUpgraded, TerrainMainType.Farm, (int)TerrainSubFarmType.WheatFarmUpgraded, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_WheatFarms, string.Format(DssRef.lang.BuildingType_IsUpgraded, string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Wheat)), null,
                CraftBuildingLib.WheatFarmUpgrade, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.LinenFarm, TerrainMainType.Farm, (int)TerrainSubFarmType.LinenFarm, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_LinenFarms, string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Linen), null,
                CraftBuildingLib.LinenFarm, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.LinenFarm_Gold };

            new BuildOption(BuildAndExpandType.LinenFarmUpgraded, TerrainMainType.Farm, (int)TerrainSubFarmType.LinenFarmUpgraded, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_LinenFarms, string.Format(DssRef.lang.BuildingType_IsUpgraded, string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Linen)), null,
                CraftBuildingLib.LinenFarmUpgrade, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.HempFarm, TerrainMainType.Farm, (int)TerrainSubFarmType.HempFarm, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_HempFarms, string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Hemp), null,
                CraftBuildingLib.HempFarm, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.Fuel,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.HempFarm_Gold };

            new BuildOption(BuildAndExpandType.HempFarmUpgraded, TerrainMainType.Farm, (int)TerrainSubFarmType.HempFarmUpgraded, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_HempFarms, string.Format(DssRef.lang.BuildingType_IsUpgraded, string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Hemp)), null,
                CraftBuildingLib.HempFarmUpgrade, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.Fuel,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.RapeSeedFarm, TerrainMainType.Farm, (int)TerrainSubFarmType.RapeSeedFarm, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_RapeseedFarms, string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Rapeseed), null,
                CraftBuildingLib.RapeseedFarm, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Fuel, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.RapeseedFarm_Gold };

            new BuildOption(BuildAndExpandType.RapeSeedFarmUpgraded, TerrainMainType.Farm, (int)TerrainSubFarmType.RapeSeedFarmUpgraded, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_RapeseedFarms, string.Format(DssRef.lang.BuildingType_IsUpgraded, string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Rapeseed)), null,
                CraftBuildingLib.RapeseedFarmUpgrade, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Fuel, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.TreeSeedlingSoft, TerrainMainType.Foil, (int)TerrainSubFoilType.TreeSoftSprout, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_TreeSeedlingSoft, DssRef.lang.Building_TreeSprout_Soft, null,
                CraftBuildingLib.TreeSeedlingSoft, false,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.TreeSeedlingSoft_Gold };

            new BuildOption(BuildAndExpandType.TreeSeedlingHard, TerrainMainType.Foil, (int)TerrainSubFoilType.TreeHardSprout, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_TreeSeedlingHard, DssRef.lang.Building_TreeSprout_Hard, null,
                CraftBuildingLib.TreeSeedlingHard, false,
               BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
               MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.TreeSeedlingHard_Gold };

            new BuildOption(BuildAndExpandType.OrchardApple, TerrainMainType.Farm, (int)TerrainSubFarmType.TreeApple, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_TreeApple, DssRef.lang.BuildingType_Orchard, null,
                CraftBuildingLib.Orchard, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.Orchard_Gold };

            new BuildOption(BuildAndExpandType.OrchidBanana, TerrainMainType.Farm, (int)TerrainSubFarmType.TreeBanana, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_TreeBanana, DssRef.lang.BuildingType_Orchard, null,
                CraftBuildingLib.Orchard, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.Orchard_Gold };

            new BuildOption(BuildAndExpandType.DirtRoad, TerrainMainType.Road, (int)TerrainRoadType.DirtRoad, BuildingGroundRequirement.BuildLight,
                SpriteName.warsFoliageDirtRoad, DssRef.lang.BuildingType_DirtRoad, null,
                CraftBuildingLib.DirtRoad, false,
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Road, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.GravelRoad, TerrainMainType.Road, (int)TerrainRoadType.GravelRoad, BuildingGroundRequirement.BuildLight,
                SpriteName.warsFoliageDirtRoad, DssRef.todoLang.BuildingType_GravelRoad, null,
                CraftBuildingLib.DirtRoad, false,
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Road, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.CobblestoneRoad, TerrainMainType.Road, (int)TerrainRoadType.CobblestoneRoad, BuildingGroundRequirement.BuildMedium,
                SpriteName.warsFoliageDirtRoad, DssRef.todoLang.BuildingType_CobblestoneRoad, null,
                CraftBuildingLib.DirtRoad, false,
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Road, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.PavedRoad, TerrainMainType.Road, (int)TerrainRoadType.PavedRoad, BuildingGroundRequirement.BuildMedium,
                SpriteName.warsFoliageDirtRoad, DssRef.todoLang.BuildingType_PavedRoad, null,
                CraftBuildingLib.DirtRoad, false,
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Road, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.PavementStreet, TerrainMainType.Road, (int)TerrainRoadType.PavementStreet, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_Pavement, string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Pavement), null,
                CraftBuildingLib.PavementStreet, false,
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Road, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.PavementFlower, TerrainMainType.Road, (int)TerrainRoadType.PavementFlower, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_PavementFlowers, string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Pavement), null,
                CraftBuildingLib.PavementFlower, false, //B
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Road, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.PavementRectFlower, TerrainMainType.Road, (int)TerrainRoadType.PavementRectFlower, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_PavementRectFlower, string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Pavement), null,
                CraftBuildingLib.PavementRectFlower, false,
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Road, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.PavementLamp, TerrainMainType.Road, (int)TerrainRoadType.PavementLamp, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_PavementLamp, string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Pavement), null,
                CraftBuildingLib.PavementLamp, false,
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE,
               MapPaintToolCategory.Road, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.PavementFountain, TerrainMainType.Road, (int)TerrainRoadType.PavemenFountain, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_PavemenFountain, string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Pavement), null,
                CraftBuildingLib.PavemenFountain, false,
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Road, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.Statue_ThePlayer, TerrainMainType.Decor, (int)TerrainDecorType.Statue_ThePlayer, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_Statue, string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Statue), null,
                CraftBuildingLib.Statue, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.Smelter, TerrainMainType.Building, (int)TerrainBuildingType.Smelter, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Smelter, DssRef.lang.BuildingType_SmeltingFurnace, DssRef.lang.BuildingType_SmeltingFurnace_Description,
                CraftBuildingLib.Smelter, true,
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Metals, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Armory, TerrainMainType.Building, (int)TerrainBuildingType.Armory, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Armory, DssRef.lang.BuildingType_Armory, DssRef.lang.BuildingType_Armory_Description,
                CraftBuildingLib.Armory, true,
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Weapons, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.WoodCutter, TerrainMainType.Building, (int)TerrainBuildingType.WoodCutter, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_WoodCutter, DssRef.lang.BuildingType_WoodCutter, DssRef.lang.BuildingType_Workshop_Description,
                CraftBuildingLib.WoodCutter, false,
                BuildCategoryTab.Advanced, BuildFilterTag.Optimize, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.StoneCutter, TerrainMainType.Building, (int)TerrainBuildingType.StoneCutter, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_StoneCutter, DssRef.lang.BuildingType_StoneCutter, DssRef.lang.BuildingType_Workshop_Description,
                CraftBuildingLib.StoneCutter, false,
                BuildCategoryTab.Advanced, BuildFilterTag.Optimize, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Bank, TerrainMainType.Building, (int)TerrainBuildingType.Bank, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_Bank, DssRef.lang.BuildingType_Bank, DssRef.lang.BuildingType_Bank_Description,
                CraftBuildingLib.Bank, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Upgrade, BuildFilterTag.Gold, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.CoinMinter, TerrainMainType.Building, (int)TerrainBuildingType.CoinMinter, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_Coinminter, DssRef.lang.BuildingType_CoinMaker, DssRef.lang.BuildingType_CoinMaker_Description,
                CraftBuildingLib.CoinMinter, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Gold, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Embassy, TerrainMainType.Building, (int)TerrainBuildingType.Embassy, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_Embassy, DssRef.lang.BuildingType_Embassy, DssRef.lang.BuildingType_Embassy_Description,
                CraftBuildingLib.Embassy, false,
                BuildCategoryTab.Advanced, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.WaterWell, TerrainMainType.Building, (int)TerrainBuildingType.WaterWell, BuildingGroundRequirement.BuildMedium,
                SpriteName.MissingImage, DssRef.todoLang.BuildingType_WaterWell, null,
                CraftBuildingLib.WaterWell, true,
                BuildCategoryTab.General, BuildFilterTag.Optimize, BuildFilterTag.Water, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.JustOne, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.WaterResovoir, TerrainMainType.Building, (int)TerrainBuildingType.WaterResovoir, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_WaterReservoir, DssRef.lang.BuildingType_WaterResovoir, DssRef.lang.BuildingType_WaterResovoir_Description,
                CraftBuildingLib.WaterResovoir, true,
                BuildCategoryTab.Farming, BuildFilterTag.Optimize, BuildFilterTag.Water, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.Foundry, TerrainMainType.Building, (int)TerrainBuildingType.Foundry, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Foundry, DssRef.lang.BuildingType_Foundry, DssRef.lang.BuildingType_Foundry_Description,
                CraftBuildingLib.Foundry, true,
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Metals, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Chemist, TerrainMainType.Building, (int)TerrainBuildingType.Chemist, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Chemist, DssRef.lang.BuildingType_Chemist, DssRef.lang.BuildingType_Chemist_Description,
                CraftBuildingLib.Chemist, true,
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Gunmaker, TerrainMainType.Building, (int)TerrainBuildingType.Gunmaker, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Gunmaker, DssRef.lang.BuildingType_Gunmaker, DssRef.lang.BuildingType_Gunmaker_Description,
                CraftBuildingLib.Gunmaker, true,
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Weapons, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.School, TerrainMainType.Building, (int)TerrainBuildingType.School, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_School, DssRef.lang.BuildingType_School, DssRef.lang.BuildingType_School_Description,
                CraftBuildingLib.School, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Optimize, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.ResearchCenter, TerrainMainType.Building, (int)TerrainBuildingType.ResearchCenter, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_ResearchCenter, DssRef.lang.BuildingType_ReseachCenter, DssRef.lang.BuildingType_Research_BaseDescription,
                CraftBuildingLib.ResearchCenter, false,
                BuildCategoryTab.Upgrade, BuildFilterTag.Optimize, BuildFilterTag.Research, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.BookPress, TerrainMainType.Building, (int)TerrainBuildingType.BookPress, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_Bookpress, DssRef.lang.BuildingType_Bookpress, DssRef.lang.BuildingType_Research_BaseDescription,
                CraftBuildingLib.BookPress, false,
                BuildCategoryTab.Upgrade, BuildFilterTag.Optimize, BuildFilterTag.Research, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Palisade, TerrainMainType.Wall, (int)TerrainWallType.Palisade, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Palisade, DssRef.lang.BuildingType_Palisade, null,
                CraftBuildingLib.Palisade, false,
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Palisade);

            new BuildOption(BuildAndExpandType.DirtWall, TerrainMainType.Wall, (int)TerrainWallType.DirtWall, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_DirtWall, DssRef.lang.BuildingType_DirtWall, null,
                CraftBuildingLib.DirtWall, false,
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.DirtTower, TerrainMainType.Wall, (int)TerrainWallType.DirtTower, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_DirtTower, DssRef.lang.BuildingType_DirtTower, null,
                CraftBuildingLib.DirtTower, false,
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.WoodWall, TerrainMainType.Wall, (int)TerrainWallType.WoodWall, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_WoodWall, DssRef.lang.BuildingType_WoodWall, null,
                CraftBuildingLib.WoodWall, false,
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.WoodTower, TerrainMainType.Wall, (int)TerrainWallType.WoodTower, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_WoodTower, DssRef.lang.BuildingType_WoodTower, null,
                CraftBuildingLib.WoodTower, false,
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.StoneWall, TerrainMainType.Wall, (int)TerrainWallType.StoneWall, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_StoneWall, string.Format(DssRef.lang.VariantType_A, DssRef.lang.BuildingType_StoneWall), null,
                CraftBuildingLib.StoneWall, false,
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.StoneTower, TerrainMainType.Wall, (int)TerrainWallType.StoneTower, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_StoneTower, DssRef.lang.BuildingType_StoneTower, null,
                CraftBuildingLib.StoneTower, false,
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.StoneWallGreen, TerrainMainType.Wall, (int)TerrainWallType.StoneWallGreen, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_StoneWallGreen, string.Format(DssRef.lang.VariantType_B, DssRef.lang.BuildingType_StoneWall), null,
                CraftBuildingLib.StoneWallGreen, false,
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.StoneWallBlueRoof, TerrainMainType.Wall, (int)TerrainWallType.StoneWallBlueRoof, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_StoneWallBlueRoof, string.Format(DssRef.lang.VariantType_C, DssRef.lang.BuildingType_StoneWall), null,
                CraftBuildingLib.StoneWallBlueRoof, false,
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
               MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.StoneWallWoodHouse, TerrainMainType.Wall, (int)TerrainWallType.StoneWallWoodHouse, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_StoneWallWoodHouse, string.Format(DssRef.lang.VariantType_D, DssRef.lang.BuildingType_StoneWall), null,
                CraftBuildingLib.StoneWallWoodHouse, false,
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
               MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.StoneGate, TerrainMainType.Wall, (int)TerrainWallType.StoneGate, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_StoneGate, DssRef.lang.BuildingType_StoneGate, null,
                CraftBuildingLib.StoneGate, false,
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.StoneHouse, TerrainMainType.Wall, (int)TerrainWallType.StoneHouse, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_StoneHouse, DssRef.lang.BuildingType_StoneHouse, null,
                CraftBuildingLib.StoneHouse, false,
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.GardenGrass, TerrainMainType.Decor, (int)TerrainDecorType.GardenGrass, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_GardenGrass, string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Garden), null,
                CraftBuildingLib.GardenGrass, false,
                BuildCategoryTab.Decor, BuildFilterTag.Garden, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GardenBird, TerrainMainType.Decor, (int)TerrainDecorType.GardenBird, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_GardenBird, string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Garden), null,
                CraftBuildingLib.GardenBird, false,
                BuildCategoryTab.Decor, BuildFilterTag.Garden, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GardenFourBushes, TerrainMainType.Decor, (int)TerrainDecorType.GardenFourBushes, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_GardenFourBushes, string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Garden), null,
                CraftBuildingLib.GardenFourBushes, false,
                BuildCategoryTab.Decor, BuildFilterTag.Garden, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GardenLongTree, TerrainMainType.Decor, (int)TerrainDecorType.GardenLongTree, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_GardenLongTree, string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Garden), null,
                CraftBuildingLib.GardenLongTree, false,
                BuildCategoryTab.Decor, BuildFilterTag.Garden, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GardenWalledBush, TerrainMainType.Decor, (int)TerrainDecorType.GardenWalledBush, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_GardenWalledBush, string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Garden), null,
                CraftBuildingLib.GardenWalledBush, false,
                BuildCategoryTab.Decor, BuildFilterTag.Garden, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.CitySquare, TerrainMainType.Road, (int)TerrainRoadType.Square, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_CitySquare, DssRef.lang.DecorType_Square, null,
                CraftBuildingLib.CitySquare, false,
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GardenMemoryStone, TerrainMainType.Decor, (int)TerrainDecorType.GardenMemoryStone, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_GardenMemoryStone, string.Format(DssRef.lang.VariantType_F, DssRef.lang.DecorType_Garden), null,
                CraftBuildingLib.GardenMemoryStone, false,
                BuildCategoryTab.Decor, BuildFilterTag.Garden, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Statue_Leader, TerrainMainType.Decor, (int)TerrainDecorType.Statue_Leader, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_Statue_Leader, string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Statue), null,
                CraftBuildingLib.Statue_Leader, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.Statue_Lion, TerrainMainType.Decor, (int)TerrainDecorType.Statue_Lion, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_Statue_Lion, string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Statue), null,
                CraftBuildingLib.Statue_Lion, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.Statue_Horse, TerrainMainType.Decor, (int)TerrainDecorType.Statue_Horse, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_Statue_Horse, string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Statue), null,
                CraftBuildingLib.Statue_Horse, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.Statue_Pillar, TerrainMainType.Decor, (int)TerrainDecorType.Statue_Pillar, BuildingGroundRequirement.BuildHeavy,
                SpriteName.WarsBuild_Statue_Pillar, string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Statue), null,
                CraftBuildingLib.Statue_Pillar, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.DiplomaticStatue_ThumbsUpWest, TerrainMainType.Decor, (int)TerrainDecorType.DiplomaticStatue_ThumbsUpWest, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_DiplomaticStatue_ThumbsUpWest, string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_DiplomaticStatue), null,
                CraftBuildingLib.DiplomaticStatueUpW, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.DiplomaticStatue_ThumbsUpEast, TerrainMainType.Decor, (int)TerrainDecorType.DiplomaticStatue_ThumbsUpEast, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_DiplomaticStatue_ThumbsUpEast, string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_DiplomaticStatue), null,
                CraftBuildingLib.DiplomaticStatueUpE, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.DiplomaticStatue_InsultWest, TerrainMainType.Decor, (int)TerrainDecorType.DiplomaticStatue_InsultWest, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_DiplomaticStatue_InsultWest, string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_DiplomaticStatue), null,
                CraftBuildingLib.DiplomaticStatueInsultW, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.DiplomaticStatue_InsultEast, TerrainMainType.Decor, (int)TerrainDecorType.DiplomaticStatue_InsultEast, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_DiplomaticStatue_InsultEast, string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_DiplomaticStatue), null,
                CraftBuildingLib.DiplomaticStatueInsultE, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.DiplomaticStatue_GoldenPoop, TerrainMainType.Decor, (int)TerrainDecorType.DiplomaticStatue_GoldenPoop, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_DiplomaticStatue_GoldenPoop, string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_DiplomaticStatue), null,
                CraftBuildingLib.DiplomaticStatue_GoldenPoop, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);


            new BuildOption(BuildAndExpandType.FlagPole_LongBanner, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_LongBanner, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsFlagType_LongBanner, string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Banner), null,
                CraftBuildingLib.FlagPole_LongBanner, false,
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_Banner, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_Banner, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsFlagType_Banner, string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Banner), null,
                CraftBuildingLib.FlagPole_Banner, false,
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_SlimBanner, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_SlimBanner, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsFlagType_SlimBanner, string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Banner), null,
                CraftBuildingLib.FlagPole_SlimBanner, false,
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_Flag, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_Flag, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsFlagType_Flag, string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Flag), null,
                CraftBuildingLib.FlagPole_Flag, false,
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_FlagRound, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_FlagRound, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsFlagType_FlagRound, string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Flag), null,
                CraftBuildingLib.FlagPole_FlagRound, false,
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_FlagLarge, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_FlagLarge, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsFlagType_FlagLarge, string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Flag), null,
                CraftBuildingLib.FlagPole_FlagLarge, false,
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_Streamer, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_Streamer, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsFlagType_Streamer, string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Flag), null,
                CraftBuildingLib.FlagPole_Streamer, false,
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_Triangle, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_Triangle, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsFlagType_Triangle, string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Flag), null,
                CraftBuildingLib.FlagPole_Triangle, false,
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);


            new BuildOption(BuildAndExpandType.TreeSoft, TerrainMainType.Foil, (int)TerrainSubFoilType.TreeSoft, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_TreeSoft, DssRef.lang.Resource_TypeName_Wood, null,
                CraftBuildingLib.TreeSoft, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.TreeHard, TerrainMainType.Foil, (int)TerrainSubFoilType.TreeHard, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_TreeHard, DssRef.lang.Resource_TypeName_Wood, null,
                CraftBuildingLib.TreeHard, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.CoalMine, TerrainMainType.Mine, (int)TerrainMineType.Coal, BuildingGroundRequirement.None,
                SpriteName.WarsResource_Fuel, string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Coal), null,
                CraftBuildingLib.CoalMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.StoneBlockMine, TerrainMainType.Mine, (int)TerrainMineType.StoneBlock, BuildingGroundRequirement.None,
                SpriteName.WarsResource_Stone, string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Stone), null,
                CraftBuildingLib.StoneBlockMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.IronOreMine, TerrainMainType.Mine, (int)TerrainMineType.IronOre, BuildingGroundRequirement.None,
                SpriteName.WarsResource_Iron, string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Iron), null,
                CraftBuildingLib.IronOreMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.TinOreMine, TerrainMainType.Mine, (int)TerrainMineType.TinOre, BuildingGroundRequirement.None,
                SpriteName.WarsResource_Tin, string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Tin), null,
                CraftBuildingLib.TinOreMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.CopperOreMine, TerrainMainType.Mine, (int)TerrainMineType.CopperOre, BuildingGroundRequirement.None,
                SpriteName.WarsResource_Copper, string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Copper), null,
                CraftBuildingLib.CopperOreMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.SilverOreMine, TerrainMainType.Mine, (int)TerrainMineType.SilverOre, BuildingGroundRequirement.None,
                SpriteName.WarsResource_Silver, string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Silver), null,
                CraftBuildingLib.SilverOreMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GoldOreMine, TerrainMainType.Mine, (int)TerrainMineType.GoldOre, BuildingGroundRequirement.None,
                SpriteName.WarsResource_Gold, string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.ResourceType_Gold), null,
                CraftBuildingLib.GoldOreMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.LeadOreMine, TerrainMainType.Mine, (int)TerrainMineType.LeadOre, BuildingGroundRequirement.None,
                SpriteName.WarsResource_Lead, string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Lead), null,
                CraftBuildingLib.LeadOreMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.MithrilMine, TerrainMainType.Mine, (int)TerrainMineType.Mithril, BuildingGroundRequirement.None,
                SpriteName.WarsResource_Mithril, string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Mithril), null,
                CraftBuildingLib.MithrilMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.SulfurMine, TerrainMainType.Mine, (int)TerrainMineType.Sulfur, BuildingGroundRequirement.None,
                SpriteName.WarsResource_Sulfur, string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Sulfur), null,
                CraftBuildingLib.SulfurMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.SaltMine, TerrainMainType.Mine, (int)TerrainMineType.Salt, BuildingGroundRequirement.None,
                SpriteName.WarsResource_Salt, TextLib.LargeFirstLetter(string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Salt)), null,
                CraftBuildingLib.SaltMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Pottery, TerrainMainType.Building, (int)TerrainBuildingType.Pottery, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Pottery, DssRef.lang.BuildingType_Pottery, TextLib.LargeFirstLetter(string.Format(DssRef.lang.BuildingType_CraftX_Description, DssRef.lang.Resource_TypeName_Clay)),
                CraftBuildingLib.Pottery, true,
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Storage, BuildFilterTag.Resources,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.ShieldMaker, TerrainMainType.Building, (int)TerrainBuildingType.ShieldMaker, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Shieldmaker, DssRef.lang.BuildingType_Shieldmaker, TextLib.LargeFirstLetter(string.Format(DssRef.lang.BuildingType_CraftX_Description, DssRef.lang.Resource_TypeName_Shield)),
                CraftBuildingLib.ShieldMaker, true,
               BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Weapons, BuildFilterTag.Soldiers,
               MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.DryingPan, TerrainMainType.Building, (int)TerrainBuildingType.DryingPan, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_DryingPan, DssRef.lang.BuildingType_DryingPan, TextLib.LargeFirstLetter(string.Format(DssRef.lang.BuildingType_GatherX_Description, DssRef.lang.Resource_TypeName_Salt)),
                CraftBuildingLib.DryingPan, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Butcher, TerrainMainType.Building, (int)TerrainBuildingType.Butcher, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Butcher, DssRef.lang.BuildingType_Butcher, DssRef.lang.BuildingType_Butcher_Description,
                CraftBuildingLib.Butcher, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Food, BuildFilterTag.Animals,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Smoker, TerrainMainType.Building, (int)TerrainBuildingType.Smoker, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Smoker, DssRef.lang.BuildingType_Smoker, TextLib.LargeFirstLetter(string.Format(DssRef.lang.BuildingType_CraftX_Description, DssRef.lang.Resource_TypeName_ConservedFood)),
                CraftBuildingLib.Smoker, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Dryer, TerrainMainType.Building, (int)TerrainBuildingType.Dryer, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_Dryer, DssRef.lang.BuildingType_Dryer, TextLib.LargeFirstLetter(string.Format(DssRef.lang.BuildingType_CraftX_Description, DssRef.lang.Resource_TypeName_ConservedFood)),
                CraftBuildingLib.Dryer, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.MaterialStorage, TerrainMainType.Building, (int)TerrainBuildingType.MaterialStorage, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_MaterialStorage, DssRef.lang.BuildingType_MaterialStorage, string.Format(DssRef.lang.BuildingType_Storage_Description, DssConst.StorageBuildingSizeAdd),
                CraftBuildingLib.MaterialStorage, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Storage, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FoodStorage, TerrainMainType.Building, (int)TerrainBuildingType.FoodStorage, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_FoodStorage, DssRef.lang.BuildingType_FoodStorage, string.Format(DssRef.lang.BuildingType_Storage_Description, DssConst.StorageBuildingSizeAdd),
                CraftBuildingLib.FoodStorage, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Storage, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.WeaponStorage, TerrainMainType.Building, (int)TerrainBuildingType.WeaponStorage, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_WeaponStorage, DssRef.lang.BuildingType_WeaponStorage, string.Format(DssRef.lang.BuildingType_Storage_Description, DssConst.StorageBuildingSizeAdd),
                CraftBuildingLib.WeaponStorage, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Storage, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.ArmorStorage, TerrainMainType.Building, (int)TerrainBuildingType.ArmorStorage, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_ArmorStorage, DssRef.lang.BuildingType_ArmorStorage, string.Format(DssRef.lang.BuildingType_Storage_Description, DssConst.StorageBuildingSizeAdd),
                CraftBuildingLib.ArmorStorage, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Storage, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.AnimalStorage, TerrainMainType.Building, (int)TerrainBuildingType.AnimalStorage, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_AnimalStorage, DssRef.lang.BuildingType_AnimalStorage, string.Format(DssRef.lang.BuildingType_Storage_Description, DssConst.StorageBuildingSizeAdd),
                CraftBuildingLib.AnimalStorage, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Storage, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Cesspit, TerrainMainType.Building, (int)TerrainBuildingType.Cesspit, BuildingGroundRequirement.None,
                SpriteName.WarsBuild_Cesspit, DssRef.lang.BuildingType_Cesspit, DssRef.lang.BuildingType_Cesspit_Description,
                CraftBuildingLib.Cesspit, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Storage, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.BoarPen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.BoarPen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_BoarPen, DssRef.lang.BuildingType_BoarPen, null,
                CraftBuildingLib.BoarPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.Resources,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 4) };

            new BuildOption(BuildAndExpandType.FowlPen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.FowlPen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_FowlPen, DssRef.lang.BuildingType_FowlPen, null,
                CraftBuildingLib.FowlPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 2) };

            new BuildOption(BuildAndExpandType.PigPen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.PigPen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_PigPen, DssRef.lang.BuildingType_PigPen, null,
                CraftBuildingLib.PigPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.Resources,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 4) };

            new BuildOption(BuildAndExpandType.HenPen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.HenPen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_HenPen, DssRef.lang.BuildingType_HenPen, null,
                CraftBuildingLib.HenPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 2) };

            new BuildOption(BuildAndExpandType.OxenPen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.OxenPen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_OxenPen, DssRef.lang.BuildingType_OxenPen, null,
                CraftBuildingLib.OxenPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 4) };

            new BuildOption(BuildAndExpandType.KineOxenPen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.KineOxenPen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_KineOxenPen, DssRef.lang.BuildingType_KineOxenPen, null,
                CraftBuildingLib.KineOxenPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 6) };

            new BuildOption(BuildAndExpandType.DogCage, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.DogCage, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_DogCage, DssRef.lang.BuildingType_DogCage, null,
                CraftBuildingLib.DogCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 2) };

            new BuildOption(BuildAndExpandType.HoundCage, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.HoundCage, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_HoundCage, DssRef.lang.BuildingType_HoundCage, null,
                CraftBuildingLib.HoundCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 2) };

            new BuildOption(BuildAndExpandType.PonyPen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.PonyPen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_PonyPen, DssRef.lang.BuildingType_PonyPen, null,
                CraftBuildingLib.PonyPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 4) };

            new BuildOption(BuildAndExpandType.HorsePen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.HorsePen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_HorsePen, DssRef.lang.BuildingType_HorsePen, null,
                CraftBuildingLib.HorsePen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 4) };

            new BuildOption(BuildAndExpandType.WarHorsePen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.WarHorsePen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_WarHorsePen, DssRef.lang.BuildingType_WarHorsePen, null,
                CraftBuildingLib.WarHorsePen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 4) };

            new BuildOption(BuildAndExpandType.DraftHorsePen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.DraftHorsePen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_DraftHorsePen, DssRef.lang.BuildingType_DraftHorsePen, null,
                CraftBuildingLib.DraftHorsePen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 4) };

            new BuildOption(BuildAndExpandType.WildPigPen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.WildPigPen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_WildPigPen, DssRef.lang.BuildingType_WildPigPen, null,
                CraftBuildingLib.WildPigPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 4) };

            new BuildOption(BuildAndExpandType.WildHogPen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.WildHogPen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_WildHogPen, DssRef.lang.BuildingType_WildHogPen, null,
                CraftBuildingLib.WildHogPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 4) };

            new BuildOption(BuildAndExpandType.WarHogPen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.WarHogPen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_WarHogPen, DssRef.lang.BuildingType_WarHogPen, null,
                CraftBuildingLib.WarHogPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 4) };

            new BuildOption(BuildAndExpandType.StagHogPen, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.StagHogPen, BuildingGroundRequirement.Plant,
                SpriteName.WarsBuild_StagHogPen, DssRef.lang.BuildingType_StagHogPen, null,
                CraftBuildingLib.StagHogPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 4) };

            new BuildOption(BuildAndExpandType.WolfCage, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.WolfCage, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_WolfPen, DssRef.lang.BuildingType_WolfCage, null,
                CraftBuildingLib.WolfCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 4) };

            new BuildOption(BuildAndExpandType.WargCage, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.WargCage, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_AlphaWargPen, DssRef.lang.BuildingType_WargCage, null,
                CraftBuildingLib.WargCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 6) };

            new BuildOption(BuildAndExpandType.AlphaWargCage, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.AlphaWargCage, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_AlphaWargPen, DssRef.lang.BuildingType_AlphaWargCage, null,
                CraftBuildingLib.AlphaWargCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 6) };

            new BuildOption(BuildAndExpandType.WildCatCage, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.WildCatCage, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_WildCatPen, DssRef.lang.BuildingType_WildCatCage, null,
                CraftBuildingLib.WildCatCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 4) };

            new BuildOption(BuildAndExpandType.LionCage, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.LionCage, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_LionPen, DssRef.lang.BuildingType_LionCage, null,
                CraftBuildingLib.LionCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 6) };

            new BuildOption(BuildAndExpandType.WarLionCage, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.WarLionCage, BuildingGroundRequirement.BuildLight,
                SpriteName.WarsBuild_WarLionPen, DssRef.lang.BuildingType_WarLionCage, null,
                CraftBuildingLib.WarLionCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 6) };

            new BuildOption(BuildAndExpandType.ElephantCage, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.ElephantCage, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_ElephantPen, DssRef.lang.BuildingType_ElephantCage, null,
                CraftBuildingLib.ElephantCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 16) };

            new BuildOption(BuildAndExpandType.WarElephantCage, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.WarElephantCage, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_WarElephantPen, DssRef.lang.BuildingType_WarElephantCage, null,
                CraftBuildingLib.WarElephantCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 20) };

            new BuildOption(BuildAndExpandType.OliphantCage, TerrainMainType.AnimalPen, (int)TerrainSubAnimalPenType.OliphantCage, BuildingGroundRequirement.BuildMedium,
                SpriteName.WarsBuild_OliphantPen, DssRef.lang.BuildingType_OliphantCage, null,
                CraftBuildingLib.OliphantCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_G, 40) };
        }
    }
}
