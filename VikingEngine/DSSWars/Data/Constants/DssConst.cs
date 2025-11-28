

using VikingEngine.DSSWars.GameObject;
using System;
using VikingEngine.ToGG.HeroQuest.Data;

namespace VikingEngine.DSSWars
{
    static class DssConst
    {
        public const int MaxSpeedOption = 5;

        //DIPLOMACY
        public const int TruceTimeSec = 180;
        public static IntervalF PeaceSafeTimeSec = new IntervalF(10, 60) * TimeExt.MinuteInSeconds;
        public static float DiplomacyExtraCostPerAlly = 0.5f;
        //SOLDIER
        public static int Soldier_DefaultHealth = 400;
        public static float Soldier_StandardAttackAndCoolDownTime = 1600;
        public static int SoldierGroup_RowWidth = 6;
        public static int SoldierGroup_ColumnsDepth = 5;
        public static int SoldierGroup_GuardCount = 5;
        public static int SoldierGroup_DefaultCount = SoldierGroup_RowWidth * SoldierGroup_ColumnsDepth;

        public static float ShipBuildTimeSec = 5f;
        public static float ShipExitTimeSec = 3f;

        public static float GuardPostEnter_TimeSec = 3f;
        public static float GuardPostExit_TimeSec = 2f;

        public static float DefaultBlockChance = 0.75f;

        public static float DefaultBlockRefillTimeSec = 0.25f;
        public static float LowBlockRefillTimeSec = 0.5f;
        public static float BadBlockRefillTimeSec = 1f;

        public static float HeightAdvantageBlockReduce_multiply = 0.5f;
        public static float HeavyBlockReduceAttack_Inv = 0.2f;
        public static float MediumBlockReduceAttack_Inv = 0.5f;
        public static float SmallBlockReduceAttack_Inv = 0.75f;

        //MEN
        public static float SoldierGroupStandardRotatingSpeed = 6.5f;
        public static float WarmachineRotatingSpeed_NoWheels = SoldierGroupStandardRotatingSpeed * 0.1f;
        public static float WarmachineRotatingSpeed_Wheels = SoldierGroupStandardRotatingSpeed * 0.2f;
        public static float ShipRotatingSpeed = SoldierGroupStandardRotatingSpeed * 0.4f;

        public static float Men_StandardModelScale = 0.06f;
        public static float Men_ModCharacterScale = Men_StandardModelScale * 1.76f;
        public static float Men_StandardWalkingSpeed = 0.00018f;
        public static float Men_StandardShipSpeed = Men_StandardWalkingSpeed * 2f;

        //CONSCRIPT
        public static float SwordAttackRange = 0.04f;
        public static float MeleeAwareRange = SwordAttackRange * 3f;

        public static int WeaponDamage_Handspear = 10;
        public static int WeaponHealthAdd_Handspear = 200;
        public static int WeaponDamage_SharpStick = 20;
        public static int WeaponDamage_BronzeSword = 45;
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

        public static int ArmorHealth_None = (int)(Soldier_DefaultHealth * 0.25);
        public static int ArmorHealth_Padded = (int)(Soldier_DefaultHealth * 0.7f);
        public static int ArmorHealth_HeavyPadded = (int)(Soldier_DefaultHealth * 1.1f);
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
        public static float TrainingAttackSpeed_Champion = 2.5f;

        public static float TrainingTimeSec_Minimal = 20;
        public static float TrainingTimeSec_Basic = 90;
        public static float TrainingTimeSec_Skillful = 5 * TimeExt.MinuteInSeconds;
        public static float TrainingTimeSec_Professional = 10 * TimeExt.MinuteInSeconds;

        public static float TrainingTimeSec_NobelmenAdd = 120;

        public static float Conscript_SpecializePercentage = 0.5f;


        //OTHER
        public static float Livestock_WalkingSpeed = Men_StandardWalkingSpeed * 0.2f;
        public static int DeliveryMaxDistance = 80;

        //CITY
        public static int TaxPerWorker_copp = 10;
        public static int UpkeepPerServiceMan_copp = 10;
        public static int UpkeepPerGuard_copp = 10;
        public static int SmallCityStartMaxWorkForce = Convert.ToInt32(SoldierGroup_DefaultCount * 5);
        public static int LargeCityStartMaxWorkForce = Convert.ToInt32(SoldierGroup_DefaultCount * 7);
        public static int HeadCityStartMaxWorkForce = Convert.ToInt32(SoldierGroup_DefaultCount * 10);

        public static float WaterAdd_Average = 3.6f;
        public static float WaterAdd_SmallCity = 0.8f * WaterAdd_Average;
        public static float WaterAdd_LargeCity = 0.9f * WaterAdd_Average;
        public static float WaterAdd_HeadCity = 1f * WaterAdd_Average;
        public static float WaterAdd_RandomAdd = 0.2f * WaterAdd_Average; 

        public static int Maxwater = 60;
        public static int WaterResovoirWaterAdd = 20;
        public static int ExpandWorkForce = SoldierGroup_DefaultCount * 4;
        //public static int ExpandGuardSize = SoldierGroup_DefaultCount;

        public static int CopperSellValue = 5;
        public static int BronzeSellValue = 10;
        public static int SilverSellValue = 25;
        public static int GoldOreSellValue = 200;
        public static int MithrilSellValue = 1000;
        

        public static int IronSellValue = 5;
        public static int FoodGoldValue = 2;
        public static int FoodGoldValue_BlackMarket = FoodGoldValue * 5;
        public static float MoneyCarryPerSoldier = FoodGoldValue_BlackMarket * 2;

        public static int CityDeliveryChunkSize_Mini = 10;
        public static int CityDeliveryChunkSize_Level1 = 30;
        public static int CityDeliveryChunkSize_Level2 = 60;
        public static int CityDeliveryChunkSize_Level3 = 120;

        //public const int ExpandGuardSizeCost = 12000;
        //public const int ReleaseGuardSizeGain = ExpandGuardSizeCost / 2;

        public const int HousingCount_WorkerHut = 30;
        public const int HousingCount_WorkerHutLarge = 50;
        public const int HousingCount_GuardsOffice_Small = 30;
        public const int HousingCount_GuardsOffice_Large = 50;

        public const int HousingCount_ServiceHouse_Large = 10;
        public const int HousingCount_ServiceHouse_Small = 5;

        public static int VillageHall_MaxWorkForce = 250;
        public static int TownHall_MaxWorkForce = 500;

        public static int VillageHall_GuardHousing = 10;
        public static int TownHall_GuardHousing = 20;
        public static int CapitalHall_GuardHousing = 40;

        public static int VillageHall_RequiredStaff = 5;
        public static int TownHall_RequiredStaff = 20;
        public static int CapitalHall_RequiredStaff = 80;

        public const double ImmigrantsRemovePerSec = 0.1;
        public const int ImmigrantsTransfereSpeed = 5;
        public const int ImmigrantionTent_TransfereSpeedBonus = 2;
        public const int ImmigrantionTent_Capacity = 60;

        //DEFENCE
        public static float GuardPostDefenceChance_Palisade = 0.4f;
        public static float GuardPostDefenceChance_Dirt = 0.5f;
        public static float GuardPostDefenceChance_Wood = 0.7f;
        public static float GuardPostDefenceChance_Stone = 0.85f;


        //BUILDING
        public static int WoodCutter_BonusRadius = 8;
        public static byte WoodCutter_WoodBonus = 40;

        public static int StoneCutter_BonusRadius = 5;
        public static byte StoneCutter_StoneBonus = 25;

        public static int Harbour_BonusRadius = 6;
        public static byte Harbour_SpeedBonus = 5;

        //public const int NobleHouseCost = 4000;
        //public const int NobleHouseUpkeep_copp = 100;

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
        public static float WorkTime_Plant_Upgraded = 15;
        public static float WorkTime_Mine = 35;
        public static float WorkTime_BogIron = WorkTime_Mine * 4;
        public static float WorkTime_Craft = 10;

        public static float WorkTime_Building_Palisade = 10;
        public static float WorkTime_Building_Small = 30;
        public static float WorkTime_Building_Default = 50;
        public static float WorkTime_Building_Large = 100;
        public static float WorkTime_Building_Epic = 150;

        public static float WorkTime_CasualResearch_Level2_Minutes = 10;
        public static float WorkTime_CasualResearch_Level3_Minutes = 15;
        public static float WorkTime_CasualResearch_Level4_Minutes = 20;

        public static float WorkTime_UpgradeBuilding = 5;
        public static float WorkTime_Demolish = 10;

        public static int Worker_TrossWorkerCarryWeight = 4;
        public static int Worker_MaxEnergy = 500;
        public static int Worker_Starvation = -Worker_MaxEnergy;

        public static int WheatFoodAmount = 30;
        public static int AnimalFoodAmount = 60;

        //public static int DefaultItemFuelAmount = 25;
        public static int RapeSeedFuelAmount = 15;
        public static int HempLinenAndFuelAmount = 8;
        public static int LinenHarvestAmount = 15;

        public static int HenRawFoodAmout = 4;
        public static int EggRawFoodAmout = 2;

        public static int PigRawFoodAmout = 3;
        public static int PigSkinAmount = 2;
        public static float ManDefaultEnergyCost = 1f;
        public static float WorkTeamEnergyCost = ManDefaultEnergyCost * City.WorkTeamSize;
        public static float WorkTeamEnergyCost_WhenIdle = WorkTeamEnergyCost * 0.5f;
        public static int FoodEnergy = 100;
        public static int PlantWaterCost = 1;

        public static float CasualSoldierDefaultCost_Copp = 3f * TaxPerWorker_copp / SoldierGroup_DefaultCount;
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

        //public static int WorkQueue_Start = 3;
        //public static int WorkQueue_LogisticsLevel1 = 6;
        public static int BuildPrio_Start = 2;
        public static int BuildPrio_LogisticsLevel1 = 4;
        public static int Logistics1FoodStorage = 300;
        public static int Logistics2_PopulationRequirement = 1200;

        public static int TechnologyGain_GoodRelation_PerMin = 1;
        public static int TechnologyGain_AllyRelation_PerMin = 2;
        public static int TechnologyGain_CitySpread = 4;
        public static int TechnologyGain_AnyLevelUp = 2;
        public static int TechnologyGain_MasterLevelUp = 20;
        public static int TechnologyGain_ResearchCenter = 2;

        public static float Time_SchoolOneXPSec = 8;//100 per level

        public static float DeliveryLevel2TimeReducePerc = 5;
        public static float DeliveryLevel3TimeReducePerc = 10;


        //BANK
        public static float GoldDeliveryFeePerc = 10;
        public static int BankTaxIncreasePercUnits_copp = 5;
        public static int GoldDeliveryChunkSize_Mini = 100;
        public static int GoldDeliveryChunkSize_Level1 = 500;
        public static int GoldDeliveryChunkSize_Level2 = 2000;
        public static int GoldDeliveryChunkSize_Level3 = 5000;

        public static int Casual_Farm2TaxIncreasePercUnits_copp = 5;
        public static int Casual_Farm3TaxIncreasePercUnits_copp = 10;
        
        //EVENTS
        public static Range DominationSizeIncrease = new Range(5, 7);
        public static IntervalF DominationWarTimeDelay_Minutes = new IntervalF(10, 20);

        //SOUND
        public static float SoundChanceProjectile = 0.4f;
        public static float SoundChanceMachineProjectile = 0.6f;
        public static float SoundChanceSword = 0.3f;

        public static float SoundChanceDamageScream = 0.04f;
        public static float SoundChanceDeathScream = 0.12f;
        public static float SoundChanceDamageGore = 0.02f;
        public static float SoundChanceDeathGore = 0.2f;

        public static IntervalF ShipSoundTimeSec = new IntervalF(3, 4);
        public static float SoundChanceShip = 0.2f;

    }
}
