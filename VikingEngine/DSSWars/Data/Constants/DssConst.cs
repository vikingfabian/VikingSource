
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars
{
    static class DssConst
    {
        
        //CITY
        public static int Maxwater = 10;

        public static int GoldOreSellValue = 100;
        public static int IronSellValue = 5;
        public static float FoodGoldValue = 2f;
        public static float FoodGoldValue_BlackMarket = FoodGoldValue * 5;

        //WORK
        public static float WorkTime_Eat = 20;
        public static float WorkTime_PickUpResource = 2;
        public static float WorkTime_PickUpProduce = 3;
        public static float WorkTime_TrossCityTrade = 4;
        public static float WorkTime_LocalTrade = WorkTime_TrossCityTrade;
        public static float WorkTime_GatherFoil_TreeSoft = 10;
        public static float WorkTime_GatherFoil_TreeHard = 12;
        public static float WorkTime_GatherFoil_FarmCulture = 20;
        public static float WorkTime_GatherFoil_Stones = 5;
        public static float WorkTime_Till = 30;
        public static float WorkTime_Plant = 20;
        public static float WorkTime_Mine = 30;
        public static float WorkTime_Craft = 5;
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
