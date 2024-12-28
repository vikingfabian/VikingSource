

using VikingEngine.DSSWars.GameObject;
using System;
using VikingEngine.ToGG.HeroQuest.Data;

namespace VikingEngine.DSSWars
{
    static class DssConst
    {
        //SOLDIER
        public static int Soldier_DefaultHealth = 400;
        public static float Soldier_StandardAttackAndCoolDownTime = 1600;
        public static int SoldierGroup_RowWidth = 6;
        public static int SoldierGroup_ColumnsDepth = 5;
        public static int SoldierGroup_DefaultCount = SoldierGroup_RowWidth * SoldierGroup_ColumnsDepth;

        //MEN
        public static float Men_StandardModelScale = 0.06f;
        public static float Men_StandardWalkingSpeed = 0.00018f;
        public static float Men_StandardShipSpeed = Men_StandardWalkingSpeed * 2f;

        //CONSCRIPT
        public static float SwordAttackRange = 0.04f;
        public static float MeleeAwareRange = SwordAttackRange * 3f;

        public static int WeaponDamage_Handspear = 10;
        public static int WeaponHealthAdd_Handspear = 200;
        public static int WeaponDamage_SharpStick = 20;
        public static int WeaponDamage_BronzeSword = 40;
        public static int WeaponDamage_ShortSword = 50;
        public static int WeaponDamage_Sword = 80;
        public static int WeaponDamage_LongSword = 120;

        public static int WeaponDamage_Pike = 80;
        public static int WeaponDamage_Warhammer = 100;
        public static int WeaponDamage_TwoHandSword = 140;
        public static int WeaponDamage_KnigtsLance = 200;
        public static int WeaponDamage_MithrilSword = 600;

        public static int WeaponDamage_Slingshot = 5;
        public static int WeaponDamage_Throwingspear = 30;
        public static int WeaponDamage_Bow = 50;
        public static int WeaponDamage_Longbow = 80;
        public static int WeaponDamage_CrossBow = 120;
        public static int WeaponDamage_MithrilBow = 300;

        public static int WeaponDamage_Handcannon = 130;
        public static int WeaponDamage_Handculvetin = 25;
        public static int WeaponDamage_Rifle = 180;
        public static int WeaponDamage_Blunderbus= 50;

        public static int WeaponDamage_Ballista = 350;
        public static int WeaponDamage_ManuBallista = 200;
        public static int WeaponDamage_Catapult = 400;

        public static int WeaponDamage_SiegeCannonBronze = 1000;
        public static int WeaponDamage_ManCannonBronze = 200;
        public static int WeaponDamage_SiegeCannonIron = 600;
        public static int WeaponDamage_ManCannonIron = 300;

        public static float AntiCavalryBonusMultiply = 2;
        public static float ArrowWeaknessBonusMultiply = 1.5f;

        public static int ArmorHealth_None = (int)(Soldier_DefaultHealth * 0.5);
        public static int ArmorHealth_Padded = Soldier_DefaultHealth;
        public static int ArmorHealth_HeavyPadded = (int)(Soldier_DefaultHealth * 1.25);
        public static int ArmorHealth_Bronze = (int)(Soldier_DefaultHealth * 1.4);
        public static int ArmorHealth_Mail = (int)(Soldier_DefaultHealth * 1.5);
        public static int ArmorHealth_HeavyMail = (int)(Soldier_DefaultHealth * 2);
        public static int ArmorHealth_Plate = (int)(Soldier_DefaultHealth * 2.2);
        public static int ArmorHealth_FullPlate = (int)(Soldier_DefaultHealth * 3);
        public static int ArmorHealth_Mithril = (int)(Soldier_DefaultHealth * 6);

        public static float TrainingAttackSpeed_Minimal = 0.5f;
        public static float TrainingAttackSpeed_Basic = 1f;
        public static float TrainingAttackSpeed_Skillful = 1.5f;
        public static float TrainingAttackSpeed_Professional = 2f;

        public static float TrainingTimeSec_Minimal = 30;
        public static float TrainingTimeSec_Basic = 60;
        public static float TrainingTimeSec_Skillful = 120;
        public static float TrainingTimeSec_Professional = 240;

        public static float TrainingTimeSec_NobelmenAdd = 120;

        public static float Conscript_SpecializePercentage = 0.5f;

        

        //OTHER
        public static float Livestock_WalkingSpeed = Men_StandardWalkingSpeed * 0.2f;
        public static int DeliveryMaxDistance = 80;

        //CITY
        public static float TaxPerWorker = 0.1f;
        public static int SmallCityStartMaxWorkForce = Convert.ToInt32(SoldierGroup_DefaultCount * 5);
        public static int LargeCityStartMaxWorkForce = Convert.ToInt32(SoldierGroup_DefaultCount * 7);
        public static int HeadCityStartMaxWorkForce = Convert.ToInt32(SoldierGroup_DefaultCount * 10);

        public static float WaterAdd_Average = 6f;
        public static float WaterAdd_SmallCity = 0.7f * WaterAdd_Average;
        public static float WaterAdd_LargeCity = 0.9f * WaterAdd_Average;
        public static float WaterAdd_HeadCity = 1.1f * WaterAdd_Average;
        public static float WaterAdd_RandomAdd = 0.25f * WaterAdd_Average; 

        public static int Maxwater = 60;
        public static int WaterResovoirWaterAdd = 20;
        public static int ExpandWorkForce = SoldierGroup_DefaultCount * 4;
        public static int ExpandGuardSize = SoldierGroup_DefaultCount;

        public static int CupperSellValue = 5;
        public static int BronzeSellValue = 10;
        public static int SilverSellValue = 25;
        public static int GoldOreSellValue = 100;
        public static int MithrilSellValue = 1000;
        

        public static int IronSellValue = 5;
        public static int FoodGoldValue = 2;
        public static int FoodGoldValue_BlackMarket = FoodGoldValue * 5;
        public static float MoneyCarryPerSoldier = FoodGoldValue_BlackMarket;

        public static int CityDeliveryChunkSize_Mini = 10;
        public static int CityDeliveryChunkSize_Level1 = 30;
        public static int CityDeliveryChunkSize_Level2 = 60;
        public static int CityDeliveryChunkSize_Level3 = 120;

        public const int ExpandGuardSizeCost = 12000;
        public const int ReleaseGuardSizeGain = ExpandGuardSizeCost / 2;


        //BUILDING
        public static int WoodCutter_BonusRadius = 8;
        public static byte WoodCutter_WoodBonus = 40;

        public static int StoneCutter_BonusRadius = 5;
        public static byte StoneCutter_StoneBonus = 25;

        public static int Harbour_BonusRadius = 6;
        public static byte Harbour_SpeedBonus = 5;

        //WORK
        public static float WorkTime_Eat = 10;
        public static float WorkTime_PickUpResource = 2;
        public static float WorkTime_PickUpProduce = 10;
        public static float WorkTime_TrossCityTrade = 4;
        public static float WorkTime_LocalTrade = WorkTime_TrossCityTrade;
        public static float WorkTime_GatherFoil_TreeSoft = 12;
        public static float WorkTime_GatherFoil_TreeHard = 15;
        public static float WorkTime_GatherFoil_DryWood = 6;
        public static float WorkTime_GatherFoil_FarmCulture = 22;
        public static float WorkTime_GatherFoil_Stones = 7;
        //public static float WorkTime_Till = 34;
        //public static float WorkTime_Till_Upgraded = 24;
        public static float WorkTime_Plant = 25;
        public static float WorkTime_Plant_Upgraded = 20;
        public static float WorkTime_Mine = 35;
        public static float WorkTime_BogIron = WorkTime_Mine * 2;
        public static float WorkTime_Craft = 10;
        public static float WorkTime_Building = 50;
        public static float WorkTime_UpgradeBuilding = 5;
        public static float WorkTime_Demolish = 10;

        public static int Worker_TrossWorkerCarryWeight = 4;
        public static int Worker_MaxEnergy = 500;
        public static int Worker_Starvation = -Worker_MaxEnergy;

        public static int WheatFoodAmount = 30;
        //public static int DefaultItemFuelAmount = 25;
        public static int RapeSeedFuelAmount = 15;
        public static int HempLinenAndFuelAmount = 8;
        public static int LinenHarvestAmount = 15;

        public static int HenRawFoodAmout = 6;
        public static int EggRawFoodAmout = 2;

        public static int PigRawFoodAmout = 4;
        public static int PigSkinAmount = 2;
        public static float ManDefaultEnergyCost = 1f;
        public static float WorkTeamEnergyCost = ManDefaultEnergyCost * City.WorkTeamSize;
        public static float WorkTeamEnergyCost_WhenIdle = WorkTeamEnergyCost * 0.5f;
        public static int FoodEnergy = 100;
        public static int PlantWaterCost = 1;
        //public static int PlantFoodCost = 6;

        public static int WorkSafeGuardAmount = 10;

        public const int StockPileMinBound = 0;
        public const int StockPileMaxBound = 20000;

        //XP
        public static byte WorkXpToLevel = 50;
        public static int WorkLevel_Expert = WorkXpToLevel * 3;
        public static int WorkLevel_Master = WorkXpToLevel * 4;
        public static byte DefaultWorkXpGain = 5;
        public static float XpLevelWorkTimePercReduction = 0.1f;

        public static int WorkQueue_Start = 3;
        public static int WorkQueue_LogisticsLevel1 = 6;
        public static int Logistics2_PopulationRequirement = 1200;

        public static int TechnologyGain_GoodRelation_PerMin = 1;
        public static int TechnologyGain_AllyRelation_PerMin = 2;
        public static int TechnologyGain_CitySpread = 4;
        public static int TechnologyGain_Expert = 2;
        public static int TechnologyGain_Master = 10;

        public static float Time_SchoolOneXP = 2;

        public static float DeliveryLevel2TimeReducePerc = 5;
        public static float DeliveryLevel3TimeReducePerc = 10;


        //BANK
        public static float GoldDeliveryFeePerc = 10;
        public static float BankTaxIncreasePercUnits = 0.05f;
        public static int GoldDeliveryChunkSize_Mini = 100;
        public static int GoldDeliveryChunkSize_Level1 = 500;
        public static int GoldDeliveryChunkSize_Level2 = 2000;
        public static int GoldDeliveryChunkSize_Level3 = 5000;

        //EVENTS
        public static Range DominationSizeIncrease = new Range(5, 7);
        public static IntervalF DominationWarTimeDelay_Minutes = new IntervalF(10, 20);
    }
}
