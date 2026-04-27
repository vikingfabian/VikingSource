using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Build
{
    struct BuildingPosition
    {
        // --- Existing Fields ---
        public IntVector2 WorkerHuts_pos;
        public IntVector2 ServiceHouse_pos;
        public IntVector2 GuardHouse_pos;
        public IntVector2 Postal_pos;
        public IntVector2 Recruitment_pos;
        public IntVector2 SoldierBarracks_pos;
        public IntVector2 ImmigrationTent_pos;
        public IntVector2 Noblehouse_pos;
        public IntVector2 Tavern_pos;
        public IntVector2 Storehouse_pos;
        public IntVector2 Brewery_pos;
        public IntVector2 Cook_pos;
        public IntVector2 CoalPit_pos;
        public IntVector2 WorkBench_pos;
        public IntVector2 Smith_pos;
        public IntVector2 Carpenter_pos;
        public IntVector2 Orchard_pos;
        public IntVector2 WheatFarm_pos;
        public IntVector2 LinenFarm_pos;
        public IntVector2 HempFarm_pos;
        public IntVector2 RapeSeedFarm_pos;
        public IntVector2 TrapperHut_pos;
        public IntVector2 BoarPen_pos;
        public IntVector2 FowlPen_pos;
        public IntVector2 PigPen_pos;
        public IntVector2 HenPen_pos;
        public IntVector2 Statue_ThePlayer_pos;
        public IntVector2 Pavement_pos;
        public IntVector2 PavementFlower_pos;
        public IntVector2 Bank_pos;
        public IntVector2 CoinMinter_pos;
        public IntVector2 GoldDelivery_pos;
        public IntVector2 WoodCutter_pos;
        public IntVector2 StoneCutter_pos;
        public IntVector2 Embassy_pos;
        public IntVector2 WaterResovoir_pos;
        public IntVector2 ArcherBarracks_pos;
        public IntVector2 WarmachineBarracks_pos;
        public IntVector2 GunBarracks_pos;
        public IntVector2 CannonBarracks_pos;
        //public IntVector2 KnightsBarracks_pos;
        public IntVector2 Smelter_pos;
        public IntVector2 Foundry_pos;
        public IntVector2 Armory_pos;
        public IntVector2 Chemist_pos;
        public IntVector2 Gunmaker_pos;
        public IntVector2 School_pos;
        public IntVector2 ResearchCenter_pos;
        public IntVector2 BookPress_pos;

        // --- NEW Production Positions ---
        public IntVector2 Pottery_pos;
        public IntVector2 DryingPan_pos;
        public IntVector2 Butcher_pos;
        public IntVector2 Smoker_pos;
        public IntVector2 Dryer_pos;
        public IntVector2 ShieldMaker_pos;

        // --- NEW Storage Positions ---
        public IntVector2 MaterialStorage_pos;
        public IntVector2 FoodStorage_pos;
        public IntVector2 WeaponStorage_pos;
        public IntVector2 ArmorStorage_pos;
        public IntVector2 AnimalStorage_pos;

        // --- NEW Animal Pen Positions ---
        public IntVector2 OxenPen_pos;
        public IntVector2 KineOxenPen_pos;

        public IntVector2 DogCage_pos;
        public IntVector2 HoundCage_pos;

        public IntVector2 PonyPen_pos;
        public IntVector2 HorsePen_pos;
        public IntVector2 WarHorsePen_pos;
        public IntVector2 DraftHorsePen_pos;

        public IntVector2 WildPigPen_pos;
        public IntVector2 WildHogPen_pos;
        public IntVector2 WarHogPen_pos;
        public IntVector2 StagHogPen_pos;

        public IntVector2 WolfCage_pos;
        public IntVector2 WargCage_pos;
        public IntVector2 AlphaWargCage_pos;

        public IntVector2 WildCatCage_pos;
        public IntVector2 LionCage_pos;
        public IntVector2 WarLionCage_pos;

        public IntVector2 ElephantCage_pos;
        public IntVector2 WarElephantCage_pos;
        public IntVector2 OliphantCage_pos;


        public IntVector2 getPos(BuildAndExpandType type)
        {
            switch (type)
            {
                case BuildAndExpandType.WorkerTent:
                case BuildAndExpandType.WorkerHut:
                case BuildAndExpandType.WorkerHutLarge:
                    return WorkerHuts_pos;

                case BuildAndExpandType.ServiceHouse_Small:
                case BuildAndExpandType.ServiceHouse_Large:
                    return ServiceHouse_pos;

                case BuildAndExpandType.GuardHouse_Small:
                case BuildAndExpandType.GuardHouse_Large:
                    return GuardHouse_pos;

                case BuildAndExpandType.Postal:
                case BuildAndExpandType.PostalLevel2:
                case BuildAndExpandType.PostalLevel3:
                    return Postal_pos;

                case BuildAndExpandType.Recruitment:
                case BuildAndExpandType.RecruitmentLevel2:
                case BuildAndExpandType.RecruitmentLevel3:
                    return Recruitment_pos;

                case BuildAndExpandType.SoldierBarracks: return SoldierBarracks_pos;
                case BuildAndExpandType.Noblehouse: return Noblehouse_pos;
                case BuildAndExpandType.Tavern: return Tavern_pos;
                case BuildAndExpandType.Storehouse: return Storehouse_pos;
                case BuildAndExpandType.Brewery: return Brewery_pos;
                case BuildAndExpandType.Cook: return Cook_pos;
                case BuildAndExpandType.CoalPit: return CoalPit_pos;
                case BuildAndExpandType.WorkBench: return WorkBench_pos;
                case BuildAndExpandType.Smith: return Smith_pos;
                case BuildAndExpandType.Carpenter: return Carpenter_pos;

                case BuildAndExpandType.OrchardApple:
                case BuildAndExpandType.OrchidBanana:
                    return Orchard_pos;

                case BuildAndExpandType.WheatFarm:
                case BuildAndExpandType.WheatFarmUpgraded:
                    return WheatFarm_pos;

                case BuildAndExpandType.LinenFarm:
                case BuildAndExpandType.LinenFarmUpgraded:
                    return LinenFarm_pos;

                case BuildAndExpandType.HempFarm:
                case BuildAndExpandType.HempFarmUpgraded:
                    return HempFarm_pos;

                case BuildAndExpandType.RapeSeedFarm:
                case BuildAndExpandType.RapeSeedFarmUpgraded:
                    return RapeSeedFarm_pos;

                case BuildAndExpandType.BoarPen: return BoarPen_pos;
                case BuildAndExpandType.FowlPen: return FowlPen_pos;
                case BuildAndExpandType.PigPen: return PigPen_pos;
                case BuildAndExpandType.HenPen: return HenPen_pos;
                case BuildAndExpandType.Statue_ThePlayer: return Statue_ThePlayer_pos;
                case BuildAndExpandType.Pavement: return Pavement_pos;
                case BuildAndExpandType.PavementFlower: return PavementFlower_pos;
                case BuildAndExpandType.Bank: return Bank_pos;
                case BuildAndExpandType.CoinMinter: return CoinMinter_pos;

                case BuildAndExpandType.GoldDeliveryLvl1:
                case BuildAndExpandType.GoldDeliveryLvl2:
                case BuildAndExpandType.GoldDeliveryLvl3:
                    return GoldDelivery_pos;

                case BuildAndExpandType.WoodCutter: return WoodCutter_pos;
                case BuildAndExpandType.StoneCutter: return StoneCutter_pos;
                case BuildAndExpandType.Embassy: return Embassy_pos;
                case BuildAndExpandType.WaterResovoir: return WaterResovoir_pos;
                case BuildAndExpandType.ArcherBarracks: return ArcherBarracks_pos;
                case BuildAndExpandType.WarmachineBarracks: return WarmachineBarracks_pos;
                case BuildAndExpandType.GunBarracks: return GunBarracks_pos;
                case BuildAndExpandType.CannonBarracks: return CannonBarracks_pos;
                //case BuildAndExpandType.KnightsBarracks: return KnightsBarracks_pos;
                case BuildAndExpandType.Smelter: return Smelter_pos;
                case BuildAndExpandType.Foundry: return Foundry_pos;
                case BuildAndExpandType.Armory: return Armory_pos;
                case BuildAndExpandType.Chemist: return Chemist_pos;
                case BuildAndExpandType.Gunmaker: return Gunmaker_pos;
                case BuildAndExpandType.School: return School_pos;
                case BuildAndExpandType.ImmigrationTent: return ImmigrationTent_pos;

                // --- NEW Production ---
                case BuildAndExpandType.Pottery: return Pottery_pos;
                case BuildAndExpandType.DryingPan: return DryingPan_pos;
                case BuildAndExpandType.Butcher: return Butcher_pos;
                case BuildAndExpandType.Smoker: return Smoker_pos;
                case BuildAndExpandType.Dryer: return Dryer_pos;
                case BuildAndExpandType.ShieldMaker: return ShieldMaker_pos;

                // --- NEW Storage ---
                case BuildAndExpandType.MaterialStorage: return MaterialStorage_pos;
                case BuildAndExpandType.FoodStorage: return FoodStorage_pos;
                case BuildAndExpandType.WeaponStorage: return WeaponStorage_pos;
                case BuildAndExpandType.ArmorStorage: return ArmorStorage_pos;
                case BuildAndExpandType.AnimalStorage: return AnimalStorage_pos;

                // --- NEW Animals ---
                case BuildAndExpandType.OxenPen: return OxenPen_pos;
                case BuildAndExpandType.KineOxenPen: return KineOxenPen_pos;

                case BuildAndExpandType.DogCage: return DogCage_pos;
                case BuildAndExpandType.HoundCage: return HoundCage_pos;

                case BuildAndExpandType.PonyPen: return PonyPen_pos;
                case BuildAndExpandType.HorsePen: return HorsePen_pos;
                case BuildAndExpandType.WarHorsePen: return WarHorsePen_pos;
                case BuildAndExpandType.DraftHorsePen: return DraftHorsePen_pos;

                case BuildAndExpandType.WildPigPen: return WildPigPen_pos;
                case BuildAndExpandType.WildHogPen: return WildHogPen_pos;
                case BuildAndExpandType.WarHogPen: return WarHogPen_pos;
                case BuildAndExpandType.StagHogPen: return StagHogPen_pos;

                case BuildAndExpandType.WolfCage: return WolfCage_pos;
                case BuildAndExpandType.WargCage: return WargCage_pos;
                case BuildAndExpandType.AlphaWargCage: return AlphaWargCage_pos;

                case BuildAndExpandType.WildCatCage: return WildCatCage_pos;
                case BuildAndExpandType.LionCage: return LionCage_pos;
                case BuildAndExpandType.WarLionCage: return WarLionCage_pos;

                case BuildAndExpandType.ElephantCage: return ElephantCage_pos;
                case BuildAndExpandType.WarElephantCage: return WarElephantCage_pos;
                case BuildAndExpandType.OliphantCage: return OliphantCage_pos;

                case BuildAndExpandType.Logistics:
                    return IntVector2.NegativeOne;


                default:
#if DEBUG
                    throw new NotImplementedException($"getPos() not implemented for {type}");
#else
                    return IntVector2.NegativeOne;
#endif

            }
        }
    }
}
