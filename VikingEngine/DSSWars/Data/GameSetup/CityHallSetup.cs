using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using VikingEngine.Core.EngineSpace.Engine.Extensions.Struct;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.Core.DSSWars.Data.GameSetup
{
    struct CityHallSetup
    {
        public static int CampHall_MaxWorkForce = 120;
        public static int VillageHall_MaxWorkForce = 250;
        public static int TownHall_MaxWorkForce = 500;
        public static int CapitalHall_MaxWorkForce = 800;


        public static int CampHall_GuardHousing = 5;
        public static int VillageHall_GuardHousing = 10;
        public static int TownHall_GuardHousing = 20;
        public static int CapitalHall_GuardHousing = 40;

        public static int VillageHall_RequiredStaff = 5;
        public static int TownHall_RequiredStaff = 20;
        public static int CapitalHall_RequiredStaff = 80;

        public static int CampHall_SoldierQuarters = 1;
        public static int VillageHall_SoldierQuarters = 3;
        public static int TownHall_SoldierQuarters = 6;
        public static int CapitalHall_SoldierQuarters = 8;

        const int ClaimRadius_Campsite = 10;
        const int ClaimRadius_Village = ClaimRadius_Campsite + 2;
        const int ClaimRadius_Town = ClaimRadius_Village + 2;
        const int ClaimRadius_Capital = ClaimRadius_Town + 2;

        public static int CampsiteCityStartMaxWorkForce = DssConst.HousingCount_WorkerTent;
        public static int SmallCityStartMaxWorkForce = DssConst.SoldierGroup_DefaultCount * 5;
        public static int LargeCityStartMaxWorkForce = DssConst.SoldierGroup_DefaultCount * 7;
        public static int HeadCityStartMaxWorkForce = DssConst.SoldierGroup_DefaultCount * 10;

        public static float WaterAdd_Average = 2.8f;
        public static float WaterAdd_SmallCity = 0.8f * WaterAdd_Average;
        public static float WaterAdd_LargeCity = 0.9f * WaterAdd_Average;
        public static float WaterAdd_HeadCity = 1f * WaterAdd_Average;
        public static float WaterAdd_RandomAdd = 0.2f * WaterAdd_Average;


        public EndlessInt maxSoldierQuarters;
        public int maxExpansions;
        public EndlessInt maxWorkForce;
        public IntervalF waterAdd;
        public int startWorkForceHousing;
        public int requiredStaff;
        public int defaultGuardHousing;

        public int claimRadius;

        public CityHallSetup(CityType cityType, bool foodModule)
        {
            switch (cityType)
            {
                case CityType.SiegeCamp:
                case CityType.Campsite:
                    maxSoldierQuarters = CampHall_SoldierQuarters;
                    maxWorkForce = CampHall_MaxWorkForce;
                    requiredStaff = 0;
                    defaultGuardHousing = CampHall_GuardHousing;
                    claimRadius = ClaimRadius_Campsite;
                    waterAdd = new IntervalF(WaterAdd_SmallCity, WaterAdd_HeadCity + WaterAdd_RandomAdd);
                    startWorkForceHousing = CampsiteCityStartMaxWorkForce;
                    maxExpansions = 0;
                    break;

                case CityType.Village:
                    maxSoldierQuarters = VillageHall_SoldierQuarters;
                    maxWorkForce = VillageHall_MaxWorkForce;
                    requiredStaff = VillageHall_RequiredStaff;
                    defaultGuardHousing = VillageHall_GuardHousing;
                    claimRadius = ClaimRadius_Village;
                    waterAdd = new IntervalF(WaterAdd_SmallCity, WaterAdd_SmallCity + WaterAdd_RandomAdd);
                    startWorkForceHousing = SmallCityStartMaxWorkForce;
                    maxExpansions = 2;
                    break;

                case CityType.Town:
                    maxSoldierQuarters = TownHall_SoldierQuarters;
                    maxWorkForce = TownHall_MaxWorkForce;
                    requiredStaff = TownHall_RequiredStaff;
                    defaultGuardHousing = TownHall_GuardHousing;
                    claimRadius = ClaimRadius_Town;
                    waterAdd = new IntervalF(WaterAdd_SmallCity, WaterAdd_LargeCity + WaterAdd_RandomAdd);
                    startWorkForceHousing = LargeCityStartMaxWorkForce;
                    maxExpansions = 4;
                    break;

                case CityType.Capital:                   
                    requiredStaff = CapitalHall_RequiredStaff;
                    defaultGuardHousing = CapitalHall_GuardHousing;
                    claimRadius = ClaimRadius_Capital;

                    if (foodModule)
                    {
                        maxSoldierQuarters = EndlessInt.NoLimit;
                        maxWorkForce = EndlessInt.NoLimit;
                    }
                    else
                    {
                        maxSoldierQuarters = CapitalHall_SoldierQuarters;
                        maxWorkForce = CapitalHall_MaxWorkForce;
                    }
                    waterAdd = new IntervalF(WaterAdd_SmallCity, WaterAdd_HeadCity + WaterAdd_RandomAdd);
                    startWorkForceHousing = HeadCityStartMaxWorkForce;
                    maxExpansions = 6;
                    break;


            }
        }
    }
}
