using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;
using VikingEngine.DSSWars.XP;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        public CasualCityProfile casualCityProfile = new CasualCityProfile();
        CityCasualProgress casualProgress = null;

        public void CasualBuild(CasualBuildType type, int count)
        {
            GetCasualProgress().AddBuild(this, new CasualBuildQueueItem() { build = type, count = count });
        }
        public void FinishCasualBuild(CasualBuildType casualBuildType)
        {
            switch (casualBuildType)
            {
                case CasualBuildType.Tent:
                    queuePlaceBuilding(BuildAndExpandType.ImmigrationTent);
                    break;
                case CasualBuildType.WorkerHut:
                    queuePlaceBuilding(BuildAndExpandType.WorkerHut);
                    queuePlaceBuilding(BuildAndExpandType.WheatFarm);
                    queuePlaceBuilding(BuildAndExpandType.LinenFarm);
                    break;
                case CasualBuildType.Barracks:
                    queuePlaceBuilding(BuildAndExpandType.SoldierBarracks);
                    queuePlaceBuilding(BuildAndExpandType.ArcherBarracks);
                    break;
                case CasualBuildType.StartUpBarracks:
                    queuePlaceBuilding(BuildAndExpandType.ArcherBarracks);
                    queuePlaceBuilding(BuildAndExpandType.WarmachineBarracks);
                    break;

                case CasualBuildType.GuardTower_Wood:
                    queuePlaceBuilding(BuildAndExpandType.WoodTower);
                    if (defenceBuildings.Count +1 >= HousingCount_Guard)
                    {
                        queuePlaceBuilding(BuildAndExpandType.GuardHouse_Small);
                    }
                    break;
                case CasualBuildType.GuardTower_Stone:
                    queuePlaceBuilding(BuildAndExpandType.StoneTower);
                    if (defenceBuildings.Count +1 >= HousingCount_Guard)
                    {
                        queuePlaceBuilding(BuildAndExpandType.GuardHouse_Large);
                    }
                    break;

                case CasualBuildType.Logistics:
                    casualCityProfile.unlock_logistics = true;
                    queuePlaceBuilding(BuildAndExpandType.Logistics);
                    break;

                case CasualBuildType.ResearchCenter:
                    casualCityProfile.unlock_research = true;
                    queuePlaceBuilding(BuildAndExpandType.ResearchCenter);
                    technology.advancedBuilding.points = TechnologyTemplate.AdvancedBuildingUnlock;
                    break;

                case CasualBuildType.UnlockIronArmor:
                    casualCityProfile.unlock_armor = 1;
                    casualCityProfile.refreshTech();
                    queuePlaceBuilding(BuildAndExpandType.Armory);
                    queuePlaceBuilding(BuildAndExpandType.Smith);
                    queuePlaceBuilding(BuildAndExpandType.Smelter);

                    break;
                case CasualBuildType.UnlockSteelArmor:
                    casualCityProfile.unlock_armor = 2;
                    casualCityProfile.refreshTech();
                    queuePlaceBuilding(BuildAndExpandType.Armory);
                    queuePlaceBuilding(BuildAndExpandType.Smith);
                    queuePlaceBuilding(BuildAndExpandType.Smelter);

                    technology.steel.points = TechnologyTemplate.SteelUnlock;
                    break;

                case CasualBuildType.UnlockSword:
                    casualCityProfile.unlock_sword = 1;
                    casualCityProfile.refreshTech();
                    queuePlaceBuilding(BuildAndExpandType.Smith);
                    technology.iron.points = TechnologyTemplate.IronUnlock;
                    break;

                case CasualBuildType.UnlockSteelSword:
                    casualCityProfile.unlock_sword = 2;
                    casualCityProfile.refreshTech();
                    queuePlaceBuilding(BuildAndExpandType.Smith);
                    queuePlaceBuilding(BuildAndExpandType.Smelter);

                    technology.steel.points = TechnologyTemplate.SteelUnlock;
                    break;

                case CasualBuildType.UnlockCatapult:
                    casualCityProfile.unlock_projectile = 1;
                    queuePlaceBuilding(BuildAndExpandType.Carpenter);
                    casualCityProfile.refreshTech();

                    technology.catapult.points = TechnologyTemplate.CatapultUnlock;
                    break;
                case CasualBuildType.UnlockBlackPower:
                    casualCityProfile.unlock_projectile = 2;
                    queuePlaceBuilding(BuildAndExpandType.Chemist);
                    queuePlaceBuilding(BuildAndExpandType.Gunmaker);
                    queuePlaceBuilding(BuildAndExpandType.GunBarracks);
                    casualCityProfile.refreshTech();

                    technology.blackPowder.points = TechnologyTemplate.BlackPowderUnlock;
                    break;
                case CasualBuildType.UnlockGunPower:
                    casualCityProfile.unlock_projectile = 3;
                    queuePlaceBuilding(BuildAndExpandType.GunBarracks);
                    casualCityProfile.refreshTech();

                    technology.gunPowder.points = TechnologyTemplate.GunPowderUnlock;
                    break;

                case CasualBuildType.UnlockFarming2:
                    casualCityProfile.unlock_farming = 1;
                    queuePlaceBuilding(BuildAndExpandType.Bank);
                    casualCityProfile.refreshTech();
                    technology.advancedFarming.points = TechnologyTemplate.AdvancedFarmingUnlock;
                    break;
                case CasualBuildType.UnlockFarming3:
                    casualCityProfile.unlock_farming = 2;
                    queuePlaceBuilding(BuildAndExpandType.CoinMinter);
                    casualCityProfile.refreshTech();
                    break;
            }

            void queuePlaceBuilding(BuildAndExpandType build)
            {
                DssRef.state.resources.editSubTilesActionQueue.Enqueue(new RbAction1Arg<BuildAndExpandType>(placeBuilding, build));
            }

            void placeBuilding(BuildAndExpandType build)
            {
                var buildData = BuildLib.BuildOptions[(int)build];
                IntVector2 buildPos = IntVector2.NegativeOne;

                if (CityStructure.Find(this, buildData.mainType, buildData.subType, out IntVector2 sameBuilding))
                {
                    findAdjacentFreeSpot(sameBuilding, ref buildPos);
                }

                if (buildPos.X < 0)
                {
                    if (!CityStructure.FindEmpty(this, out buildPos))
                    {
                        return;
                    }
                }

                var subTile = DssRef.world.subTileGrid.Get(buildPos);
                bool upgrade = false;

                //var dist = cityHallSubtilePos.SideLength(buildPos);
                if (buildData.execute_async(this, buildPos, ref subTile, upgrade))
                {
                    EditSubTile edit = new EditSubTile(buildPos, subTile, true, true, false);
                    edit.ExecuteEdit();
                }
            }

        }

        public CityCasualProgress GetCasualProgress()
        {
            if (casualProgress == null)
            {
                casualProgress = new CityCasualProgress(this);
            }

            return casualProgress;
        }

        public int casualRecruitTime_sec(CasualSoldierType soldierType)
        {
            int barracksCount = getCount(CasualBuildType.Barracks);
            if (barracksCount == 0)
            {
                barracksCount = 1;
            }

            return Convert.ToInt32(ConscriptProfile.TrainingTime(soldierType) / barracksCount);
        }

        public int getCount(CasualBuildType casualType)
        {
            switch (casualType)
            {
                case CasualBuildType.Tent:
                    return buildingStructure.ImmigrationTent_count;

                case CasualBuildType.WorkerHut:
                    return buildingStructure.WorkerHuts_count + buildingStructure.WorkerHuts_Large_count;

                case CasualBuildType.Barracks:
                    return Math.Min(buildingStructure.SoldierBarracks_count, buildingStructure.ArcherBarracks_count);

                case CasualBuildType.GuardTower_Wood:
                case CasualBuildType.GuardTower_Stone:
                    return defenceBuildings.Count;

                case CasualBuildType.Logistics:
                    return lib.BoolToInt01(casualCityProfile.unlock_logistics);
                case CasualBuildType.ResearchCenter:
                    return lib.BoolToInt01(casualCityProfile.unlock_research);

                case CasualBuildType.UnlockIronArmor:
                    return lib.BoolToInt01(casualCityProfile.unlock_armor >= 1);
                case CasualBuildType.UnlockSteelArmor:
                    return lib.BoolToInt01(casualCityProfile.unlock_armor >= 2);

                case CasualBuildType.UnlockSword:
                    return lib.BoolToInt01(casualCityProfile.unlock_sword >= 1);
                case CasualBuildType.UnlockSteelSword:
                    return lib.BoolToInt01(casualCityProfile.unlock_sword >= 2);

                case CasualBuildType.UnlockCatapult:
                    return lib.BoolToInt01(casualCityProfile.unlock_projectile >= 1);
                case CasualBuildType.UnlockBlackPower:
                    return lib.BoolToInt01(casualCityProfile.unlock_projectile >= 2);
                case CasualBuildType.UnlockGunPower:
                    return lib.BoolToInt01(casualCityProfile.unlock_projectile >= 3);

                case CasualBuildType.UnlockFarming2:
                    return lib.BoolToInt01(casualCityProfile.unlock_farming >= 1);
                case CasualBuildType.UnlockFarming3:
                    return lib.BoolToInt01(casualCityProfile.unlock_farming >= 2);

                default: return 0;
            }
        }

        public int getMaxCount(CasualBuildType casualType)
        {
            switch (casualType)
            {
                case CasualBuildType.WorkerHut:
                    return casualCityProfile.maxHuts;
                case CasualBuildType.Barracks:
                    return 8;
                case CasualBuildType.Tent:
                    return 8;

                case CasualBuildType.GuardTower_Wood:
                case CasualBuildType.GuardTower_Stone:
                    return 99;

                default: return 1;
            }
        }        
    }
}
