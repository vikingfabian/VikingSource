
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameObject;
using System;

namespace VikingEngine.DSSWars
{
    static class DssConst
    {
        //SOLDIER
        public static int Soldier_DefaultHealth = 200;
        public static float Soldier_StandardAttackAndCoolDownTime = 1600;
        public static int SoldierGroup_RowWidth = 6;
        public static int SoldierGroup_ColumnsDepth = 5;
        public static int SoldierGroup_DefaultCount = SoldierGroup_RowWidth * SoldierGroup_ColumnsDepth;

        //MEN
        public static float Men_StandardModelScale = 0.06f;
        public static float Men_StandardWalkingSpeed = 0.00018f;
        public static float Men_StandardShipSpeed = Men_StandardWalkingSpeed * 2f;

        //CONSCRIPT
        public static int WeaponDamage_SharpStick = 30;
        public static int WeaponDamage_Sword = 50;
        public static int WeaponDamage_Pike = 50;
        public static int WeaponDamage_TwoHandSword = 100;
        public static int WeaponDamage_KnigtsLance = 120;

        public static int WeaponDamage_Bow = 30;
        public static int WeaponDamage_Longbow = 50;
        public static int WeaponDamage_CrossBow = 100;
        public static int WeaponDamage_Ballista = 300;

        public static float AntiCavalryBonusMultiply = 2;
        public static float ArrowWeaknessBonusMultiply = 1.5f;

        public static int ArmorHealth_None = (int)(Soldier_DefaultHealth * 0.5);
        public static int ArmorHealth_Light = Soldier_DefaultHealth;
        public static int ArmorHealth_Medium = (int)(Soldier_DefaultHealth * 1.5);
        public static int ArmorHealth_Heavy = (int)(Soldier_DefaultHealth * 2);

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

        //CITY
        public static float TaxPerWorker = 0.1f;
        public static int SmallCityStartMaxWorkForce = Convert.ToInt32(SoldierGroup_DefaultCount * 5);
        public static int LargeCityStartMaxWorkForce = Convert.ToInt32(SoldierGroup_DefaultCount * 7);
        public static int HeadCityStartMaxWorkForce = Convert.ToInt32(SoldierGroup_DefaultCount * 10);

        //public static int Maxwater_SmallCity = 5;
        //public static int Maxwater_LargeCity= 8;
        //public static int Maxwater_HeadCity = 10;

        //public static int Maxwater_RandomAdd = 2;

        public static float WaterAdd_SmallCity = 0.5f;
        public static float WaterAdd_LargeCity = 0.8f;
        public static float WaterAdd_HeadCity = 1f;
        public static float WaterAdd_RandomAdd = 0.25f; 

        public static int Maxwater = 10;
        public static int ExpandWorkForce = SoldierGroup_DefaultCount * 4;
        public static int ExpandGuardSize = SoldierGroup_DefaultCount;

        public static int GoldOreSellValue = 100;
        public static int IronSellValue = 5;
        public static int FoodGoldValue = 2;
        public static int FoodGoldValue_BlackMarket = FoodGoldValue * 5;
        public static int CityDeliveryCount = 30;

        //WORK
        public static float WorkTime_Eat = 10;
        public static float WorkTime_PickUpResource = 2;
        public static float WorkTime_PickUpProduce = 3;
        public static float WorkTime_TrossCityTrade = 4;
        public static float WorkTime_LocalTrade = WorkTime_TrossCityTrade;
        public static float WorkTime_GatherFoil_TreeSoft = 10;
        public static float WorkTime_GatherFoil_TreeHard = 12;
        public static float WorkTime_GatherFoil_DryWood = 5;
        public static float WorkTime_GatherFoil_FarmCulture = 20;
        public static float WorkTime_GatherFoil_Stones = 5;
        public static float WorkTime_Till = 30;
        public static float WorkTime_Plant = 20;
        public static float WorkTime_Mine = 30;
        public static float WorkTime_BogIron = WorkTime_Mine * 2;
        public static float WorkTime_Craft = 2;
        public static float WorkTime_Building = 40;

        public static int Worker_TrossWorkerCarryWeight = 4;
        public static int Worker_MaxEnergy = 500;
        public static int Worker_Starvation = -Worker_MaxEnergy;

        public static int DefaultItemRawFoodAmout = 8;
        public static int PigRawFoodAmout = 5 * DefaultItemRawFoodAmout;
        public static float ManDefaultEnergyCost = 1f;
        public static float WorkTeamEnergyCost = ManDefaultEnergyCost * City.WorkTeamSize;
        public static float WorkTeamEnergyCost_WhenIdle = WorkTeamEnergyCost * 0.5f;
        public static int FoodEnergy = 100;


    }
}
