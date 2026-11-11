using VikingEngine.DSSWars.Build;

namespace VikingEngine.Core.DSSWars.Map.MapProcess.CityStructure
{    

    struct BuildingStructure
    {
        public bool gotUpdate;
        public bool manorLord;
        public bool greatHall;
        public int buildingLevel_logistics;
        public bool economics;
        public bool tingshouse;

        public int ExpansionHall_LowestLevel = 3;
        public int ExpansionHall_count;
        public int TentHuts_count;
        public int WorkerHuts_count;
        public int WorkerHuts_Large_count;
        public int ServiceMenHouse_count;
        public int ServiceMenHouse_Large_count;
        public int GuardOffice_count;
        public int GuardOffice_Large_count;
        public int SoldiersQuarter_Small_count;
        public int SoldiersQuarter_Large_count;
        public int ImmigrationTent_count;


        public int Postal_count;
        public int Recruitment_count;
        public int SoldierBarracks_count;
        public int Noblehouse_count;
        public int Tavern_count;
        public int Storehouse_count;
        public int Brewery_count;
        public int Cook_count;
        public int CoalPit_count;
        public int WorkBench_count;
        public int Smith_count;
        public int Carpenter_count;
        public int Orchard_count;
        public int WheatFarm_count;
        public int LinenFarm_count;
        public int HempFarm_count;
        public int RapeSeedFarm_count;
        public int TrapperHut_count;
        public int BoarPen_count;
        public int FowlPen_count;
        public int PigPen_count;
        public int HenPen_count;
        public int Statue_ThePlayer_count;
        public int Pavement_count;
        //public int PavementFlower_count;
        public int Bank_count;
        public int CoinMinter_count;
        public int GoldDelivery_count;
        public int WoodCutter_count;
        public int StoneCutter_count;
        public int Embassy_count;
        public int WaterWell_count;
        public int WaterResovoir_count;
        public int ArcherBarracks_count;
        public int WarmachineBarracks_count;
        public int GunBarracks_count;
        public int CannonBarracks_count;
        public int AnimalTrainer_count;
        //public int KnightsBarracks_count;
        public int Smelter_count;
        public int Foundry_count;
        public int Armory_count;
        public int Chemist_count;
        public int Gunmaker_count;
        public int School_count;
        public int ResearchCenter_count;
        public int BookPress_count;

        // --- NEW Production Buildings ---
        public int Pottery_count;
        public int DryingPan_count;
        public int Butcher_count;
        public int Smoker_count;
        public int Dryer_count;
        public int ShieldMaker_count;

        // --- NEW Storage Buildings ---
        public int MaterialStorage_count;
        public int FoodStorage_count;
        public int WeaponStorage_count;
        public int ArmorStorage_count;
        public int AnimalStorage_count;
        public int CessPit_count;

        // --- NEW Animal Pens & Cages ---
        public int OxenPen_count;
        public int KineOxenPen_count;

        public int DogCage_count;
        public int HoundCage_count;

        public int PonyPen_count;
        public int HorsePen_count;
        public int WarHorsePen_count;
        public int DraftHorsePen_count;

        public int WildPigPen_count;
        public int WildHogPen_count;
        public int WarHogPen_count;
        public int StagHogPen_count;

        public int WolfCage_count;
        public int WargCage_count;
        public int AlphaWargCage_count;

        public int WildCatCage_count;
        public int LionCage_count;
        public int WarLionCage_count;

        public int ElephantCage_count;
        public int WarElephantCage_count;
        public int OliphantCage_count;

        public int wallCount;

        public IntVector2 SuggestedTrapperPos;

        public BuildingStructure()
        { }
        public int getCount(BuildAndExpandType type)
        {
            switch (type)
            {
                case BuildAndExpandType.WorkerTent: return TentHuts_count;
                case BuildAndExpandType.WorkerHut: return WorkerHuts_count;
                case BuildAndExpandType.WorkerHutLarge: return WorkerHuts_Large_count;

                case BuildAndExpandType.ServiceHouse_Small: return ServiceMenHouse_count;
                case BuildAndExpandType.ServiceHouse_Large: return ServiceMenHouse_Large_count;

                case BuildAndExpandType.Postal: return Postal_count;
                case BuildAndExpandType.PostalLevel2: return Postal_count;
                case BuildAndExpandType.PostalLevel3: return Postal_count;

                case BuildAndExpandType.Recruitment: return Recruitment_count;
                case BuildAndExpandType.RecruitmentLevel2: return Recruitment_count;
                case BuildAndExpandType.RecruitmentLevel3: return Recruitment_count;

                case BuildAndExpandType.SoldierBarracks: return SoldierBarracks_count;
                case BuildAndExpandType.Noblehouse: return Noblehouse_count;
                case BuildAndExpandType.Tavern: return Tavern_count;
                case BuildAndExpandType.Storehouse: return Storehouse_count;
                case BuildAndExpandType.Brewery: return Brewery_count;
                case BuildAndExpandType.Cook: return Cook_count;
                case BuildAndExpandType.CoalPit: return CoalPit_count;
                case BuildAndExpandType.WorkBench: return WorkBench_count;
                case BuildAndExpandType.Smith: return Smith_count;
                case BuildAndExpandType.Carpenter: return Carpenter_count;

                case BuildAndExpandType.OrchardApple: return Orchard_count;
                case BuildAndExpandType.OrchidBanana: return Orchard_count;
                case BuildAndExpandType.WheatFarm: return WheatFarm_count;
                case BuildAndExpandType.WheatFarmUpgraded: return WheatFarm_count;
                case BuildAndExpandType.LinenFarm: return LinenFarm_count;
                case BuildAndExpandType.LinenFarmUpgraded: return LinenFarm_count;
                case BuildAndExpandType.HempFarm: return HempFarm_count;
                case BuildAndExpandType.HempFarmUpgraded: return HempFarm_count;
                case BuildAndExpandType.RapeSeedFarm: return RapeSeedFarm_count;
                case BuildAndExpandType.RapeSeedFarmUpgraded: return RapeSeedFarm_count;

                
                case BuildAndExpandType.Statue_ThePlayer: return Statue_ThePlayer_count;
                case BuildAndExpandType.Bank: return Bank_count;
                case BuildAndExpandType.CoinMinter: return CoinMinter_count;

                case BuildAndExpandType.GoldDeliveryLvl1: return GoldDelivery_count;
                case BuildAndExpandType.GoldDeliveryLvl2: return GoldDelivery_count;
                case BuildAndExpandType.GoldDeliveryLvl3: return GoldDelivery_count;

                case BuildAndExpandType.WoodCutter: return WoodCutter_count;
                case BuildAndExpandType.StoneCutter: return StoneCutter_count;
                case BuildAndExpandType.Embassy: return Embassy_count;
                case BuildAndExpandType.WaterWell: return WaterWell_count;
                case BuildAndExpandType.WaterResovoir: return WaterResovoir_count;
                case BuildAndExpandType.ArcherBarracks: return ArcherBarracks_count;
                case BuildAndExpandType.WarmachineBarracks: return WarmachineBarracks_count;
                case BuildAndExpandType.GunBarracks: return GunBarracks_count;
                case BuildAndExpandType.CannonBarracks: return CannonBarracks_count;
                case BuildAndExpandType.AnimalTrainer: return AnimalTrainer_count;
                //case BuildAndExpandType.KnightsBarracks: return KnightsBarracks_count;
                case BuildAndExpandType.Smelter: return Smelter_count;
                case BuildAndExpandType.Foundry: return Foundry_count;
                case BuildAndExpandType.Armory: return Armory_count;
                case BuildAndExpandType.Chemist: return Chemist_count;
                case BuildAndExpandType.Gunmaker: return Gunmaker_count;
                case BuildAndExpandType.School: return School_count;
                case BuildAndExpandType.ImmigrationTent: return ImmigrationTent_count;

                // --- NEW Production ---
                case BuildAndExpandType.Pottery: return Pottery_count;
                case BuildAndExpandType.DryingPan: return DryingPan_count;
                case BuildAndExpandType.Butcher: return Butcher_count;
                case BuildAndExpandType.Smoker: return Smoker_count;
                case BuildAndExpandType.Dryer: return Dryer_count;
                case BuildAndExpandType.ShieldMaker: return ShieldMaker_count;

                // --- NEW Storage ---
                case BuildAndExpandType.MaterialStorage: return MaterialStorage_count;
                case BuildAndExpandType.FoodStorage: return FoodStorage_count;
                case BuildAndExpandType.WeaponStorage: return WeaponStorage_count;
                case BuildAndExpandType.ArmorStorage: return ArmorStorage_count;
                case BuildAndExpandType.AnimalStorage: return AnimalStorage_count;
                case BuildAndExpandType.Cesspit: return CessPit_count;

                // --- NEW Animals ---


                case BuildAndExpandType.BoarPen: return BoarPen_count;
                case BuildAndExpandType.FowlPen: return FowlPen_count;
                case BuildAndExpandType.PigPen: return PigPen_count;
                case BuildAndExpandType.HenPen: return HenPen_count;

                case BuildAndExpandType.OxenPen: return OxenPen_count;
                case BuildAndExpandType.KineOxenPen: return KineOxenPen_count;

                case BuildAndExpandType.DogCage: return DogCage_count;
                case BuildAndExpandType.HoundCage: return HoundCage_count;

                case BuildAndExpandType.PonyPen: return PonyPen_count;
                case BuildAndExpandType.HorsePen: return HorsePen_count;
                case BuildAndExpandType.WarHorsePen: return WarHorsePen_count;
                case BuildAndExpandType.DraftHorsePen: return DraftHorsePen_count;

                case BuildAndExpandType.WildPigPen: return WildPigPen_count;
                case BuildAndExpandType.WildHogPen: return WildHogPen_count;
                case BuildAndExpandType.WarHogPen: return WarHogPen_count;
                case BuildAndExpandType.StagHogPen: return StagHogPen_count;

                case BuildAndExpandType.WolfCage: return WolfCage_count;
                case BuildAndExpandType.WargCage: return WargCage_count;
                case BuildAndExpandType.AlphaWargCage: return AlphaWargCage_count;

                case BuildAndExpandType.WildCatCage: return WildCatCage_count;
                case BuildAndExpandType.LionCage: return LionCage_count;
                case BuildAndExpandType.WarLionCage: return WarLionCage_count;

                case BuildAndExpandType.ElephantCage: return ElephantCage_count;
                case BuildAndExpandType.WarElephantCage: return WarElephantCage_count;
                case BuildAndExpandType.OliphantCage: return OliphantCage_count;

                case BuildAndExpandType.PavementLamp:
                case BuildAndExpandType.PavementStreet:
                case BuildAndExpandType.PavementFlower:
                case BuildAndExpandType.PavementFountain:
                case BuildAndExpandType.PavementRectFlower:
                    return Pavement_count;
                //case BuildAndExpandType.TreeHard:
                //case BuildAndExpandType.TreeSoft:
                //case BuildAndExpandType.TreeSeedlingSoft:
                //case BuildAndExpandType.TreeSeedlingHard:
                //case BuildAndExpandType.DirtRoad:
                //case BuildAndExpandType.GravelRoad:
                //case BuildAndExpandType.CobblestoneRoad:
                //case BuildAndExpandType.PavedRoad:



                
                //case BuildAndExpandType.GardenGrass:
                //case BuildAndExpandType.GardenFourBushes:
                //case BuildAndExpandType.GardenLongTree:
                //case BuildAndExpandType.GardenWalledBush:
                //case BuildAndExpandType.CitySquare:
                ////case BuildAndExpandType.CobbleStones:
                //case BuildAndExpandType.GardenBird:
                //case BuildAndExpandType.GardenMemoryStone:
                //case BuildAndExpandType.Statue_Leader:
                //case BuildAndExpandType.Statue_Lion:
                //case BuildAndExpandType.Statue_Horse:
                //case BuildAndExpandType.Statue_Pillar:
                //case BuildAndExpandType.DiplomaticStatue_ThumbsUpWest:
                //case BuildAndExpandType.DiplomaticStatue_ThumbsUpEast:
                //case BuildAndExpandType.DiplomaticStatue_InsultWest:
                //case BuildAndExpandType.DiplomaticStatue_InsultEast:
                //case BuildAndExpandType.DiplomaticStatue_GoldenPoop:

                //case BuildAndExpandType.FlagPole_LongBanner:
                //case BuildAndExpandType.FlagPole_Banner:
                //case BuildAndExpandType.FlagPole_SlimBanner:

                //case BuildAndExpandType.FlagPole_Flag:
                //case BuildAndExpandType.FlagPole_FlagRound:
                //case BuildAndExpandType.FlagPole_FlagLarge:
                //case BuildAndExpandType.FlagPole_Streamer:
                //case BuildAndExpandType.FlagPole_Triangle:
                //    return 0;

                case BuildAndExpandType.TrapperHut:
                    return TrapperHut_count;
                case BuildAndExpandType.GuardHouse_Small:
                    return GuardOffice_count;
                case BuildAndExpandType.GuardHouse_Large:
                    return GuardOffice_Large_count;
                case BuildAndExpandType.SoldiersQuarter_Small:
                    return SoldiersQuarter_Small_count;
                case BuildAndExpandType.SoldiersQuarter_Large:
                    return SoldiersQuarter_Large_count;

                case BuildAndExpandType.ResearchCenter:
                    return ResearchCenter_count;
                case BuildAndExpandType.BookPress:
                    return BookPress_count;

                case BuildAndExpandType.DirtWall:
                case BuildAndExpandType.DirtTower:
                case BuildAndExpandType.WoodWall:
                case BuildAndExpandType.WoodTower:
                case BuildAndExpandType.StoneWall:
                case BuildAndExpandType.StoneTower:
                case BuildAndExpandType.StoneGate:
                case BuildAndExpandType.StoneHouse:
                case BuildAndExpandType.StoneWallBlueRoof:
                case BuildAndExpandType.StoneWallGreen:
                case BuildAndExpandType.StoneWallWoodHouse:
                //case BuildAndExpandType.Logistics:
               
                case BuildAndExpandType.Palisade:
                    return wallCount;

                case BuildAndExpandType.ManorLord:
                    return lib.BoolToInt01(manorLord);

                case BuildAndExpandType.GreatHall:
                    return lib.BoolToInt01(greatHall);
                case BuildAndExpandType.Economics:
                    return lib.BoolToInt01(greatHall);
                case BuildAndExpandType.TingsHouse:
                    return lib.BoolToInt01(greatHall);

                case BuildAndExpandType.Logistics:
                    return buildingLevel_logistics;

#if DEBUG
                default: return 0;//throw new NotImplementedException(type.ToString());
#else
                default: return 0;
#endif
            }
        }

        public void addCount(BuildAndExpandType type, int add)
        {
            switch (type)
            {
                case BuildAndExpandType.WorkerHut: WorkerHuts_count += add; break;
                case BuildAndExpandType.WorkerHutLarge: WorkerHuts_Large_count += add; break;

                case BuildAndExpandType.ServiceHouse_Small: ServiceMenHouse_count += add; break;
                case BuildAndExpandType.ServiceHouse_Large: ServiceMenHouse_Large_count += add; break;

                case BuildAndExpandType.Postal:
                case BuildAndExpandType.PostalLevel2:
                case BuildAndExpandType.PostalLevel3:
                    Postal_count += add; break;

                case BuildAndExpandType.Recruitment:
                case BuildAndExpandType.RecruitmentLevel2:
                case BuildAndExpandType.RecruitmentLevel3:
                    Recruitment_count += add; break;

                case BuildAndExpandType.SoldierBarracks: SoldierBarracks_count += add; break;
                case BuildAndExpandType.Noblehouse: Noblehouse_count += add; break;
                case BuildAndExpandType.Tavern: Tavern_count += add; break;
                case BuildAndExpandType.Storehouse: Storehouse_count += add; break;
                case BuildAndExpandType.Brewery: Brewery_count += add; break;
                case BuildAndExpandType.Cook: Cook_count += add; break;
                case BuildAndExpandType.CoalPit: CoalPit_count += add; break;
                case BuildAndExpandType.WorkBench: WorkBench_count += add; break;
                case BuildAndExpandType.Smith: Smith_count += add; break;
                case BuildAndExpandType.Carpenter: Carpenter_count += add; break;

                case BuildAndExpandType.OrchardApple:
                case BuildAndExpandType.OrchidBanana:
                    Orchard_count += add; break;

                case BuildAndExpandType.WheatFarm:
                case BuildAndExpandType.WheatFarmUpgraded:
                    WheatFarm_count += add; break;

                case BuildAndExpandType.LinenFarm:
                case BuildAndExpandType.LinenFarmUpgraded:
                    LinenFarm_count += add; break;

                case BuildAndExpandType.HempFarm:
                case BuildAndExpandType.HempFarmUpgraded:
                    HempFarm_count += add; break;

                case BuildAndExpandType.RapeSeedFarm:
                case BuildAndExpandType.RapeSeedFarmUpgraded:
                    RapeSeedFarm_count += add; break;

                case BuildAndExpandType.PigPen: PigPen_count += add; break;
                case BuildAndExpandType.HenPen: HenPen_count += add; break;
                case BuildAndExpandType.Statue_ThePlayer: Statue_ThePlayer_count += add; break;

                case BuildAndExpandType.PavementStreet:
                case BuildAndExpandType.PavementFlower:
                case BuildAndExpandType.PavementLamp:
                case BuildAndExpandType.PavementFountain:
                case BuildAndExpandType.PavementRectFlower:
                    Pavement_count += add; break;
                //case BuildAndExpandType.PavementFlower: PavementFlower_count += add; break;
                case BuildAndExpandType.Bank: Bank_count += add; break;
                case BuildAndExpandType.CoinMinter: CoinMinter_count += add; break;

                case BuildAndExpandType.GoldDeliveryLvl1:
                case BuildAndExpandType.GoldDeliveryLvl2:
                case BuildAndExpandType.GoldDeliveryLvl3:
                    GoldDelivery_count += add; break;

                case BuildAndExpandType.WoodCutter: WoodCutter_count += add; break;
                case BuildAndExpandType.StoneCutter: StoneCutter_count += add; break;
                case BuildAndExpandType.Embassy: Embassy_count += add; break;
                case BuildAndExpandType.WaterResovoir: WaterResovoir_count += add; break;
                case BuildAndExpandType.ArcherBarracks: ArcherBarracks_count += add; break;
                case BuildAndExpandType.WarmachineBarracks: WarmachineBarracks_count += add; break;
                case BuildAndExpandType.GunBarracks: GunBarracks_count += add; break;
                case BuildAndExpandType.CannonBarracks: CannonBarracks_count += add; break;
                case BuildAndExpandType.AnimalTrainer: AnimalTrainer_count += add; break;
                //case BuildAndExpandType.KnightsBarracks: KnightsBarracks_count += add; break;
                case BuildAndExpandType.Smelter: Smelter_count += add; break;
                case BuildAndExpandType.Foundry: Foundry_count += add; break;
                case BuildAndExpandType.Armory: Armory_count += add; break;
                case BuildAndExpandType.Chemist: Chemist_count += add; break;
                case BuildAndExpandType.Gunmaker: Gunmaker_count += add; break;
                case BuildAndExpandType.School: School_count += add; break;
                case BuildAndExpandType.ImmigrationTent: ImmigrationTent_count += add; break;

                // --- NEW Production ---
                case BuildAndExpandType.Pottery: Pottery_count += add; break;
                case BuildAndExpandType.DryingPan: DryingPan_count += add; break;
                case BuildAndExpandType.Butcher: Butcher_count += add; break;
                case BuildAndExpandType.Smoker: Smoker_count += add; break;
                case BuildAndExpandType.Dryer: Dryer_count += add; break;

                // --- NEW Storage ---
                case BuildAndExpandType.MaterialStorage: MaterialStorage_count += add; break;
                case BuildAndExpandType.FoodStorage: FoodStorage_count += add; break;
                case BuildAndExpandType.WeaponStorage: WeaponStorage_count += add; break;
                case BuildAndExpandType.ArmorStorage: ArmorStorage_count += add; break;
                case BuildAndExpandType.AnimalStorage: AnimalStorage_count += add; break;
                case BuildAndExpandType.Cesspit: CessPit_count += add; break;

                // --- NEW Animals ---
                case BuildAndExpandType.OxenPen: OxenPen_count += add; break;
                case BuildAndExpandType.KineOxenPen: KineOxenPen_count += add; break;

                case BuildAndExpandType.DogCage: DogCage_count += add; break;
                case BuildAndExpandType.HoundCage: HoundCage_count += add; break;

                case BuildAndExpandType.PonyPen: PonyPen_count += add; break;
                case BuildAndExpandType.HorsePen: HorsePen_count += add; break;
                case BuildAndExpandType.WarHorsePen: WarHorsePen_count += add; break;
                case BuildAndExpandType.DraftHorsePen: DraftHorsePen_count += add; break;

                case BuildAndExpandType.WildPigPen: WildPigPen_count += add; break;
                case BuildAndExpandType.WildHogPen: WildHogPen_count += add; break;
                case BuildAndExpandType.WarHogPen: WarHogPen_count += add; break;
                case BuildAndExpandType.StagHogPen: StagHogPen_count += add; break;

                case BuildAndExpandType.WolfCage: WolfCage_count += add; break;
                case BuildAndExpandType.WargCage: WargCage_count += add; break;
                case BuildAndExpandType.AlphaWargCage: AlphaWargCage_count += add; break;

                case BuildAndExpandType.WildCatCage: WildCatCage_count += add; break;
                case BuildAndExpandType.LionCage: LionCage_count += add; break;
                case BuildAndExpandType.WarLionCage: WarLionCage_count += add; break;

                case BuildAndExpandType.ElephantCage: ElephantCage_count += add; break;
                case BuildAndExpandType.WarElephantCage: WarElephantCage_count += add; break;
                case BuildAndExpandType.OliphantCage: OliphantCage_count += add; break;

                default: break;
            }
        }


        public int getBarracksCount(BuildAndExpandType type)
        {
            switch (type)
            {
                
                case BuildAndExpandType.SoldierBarracks: return SoldierBarracks_count;
                case BuildAndExpandType.ArcherBarracks: return ArcherBarracks_count;
                case BuildAndExpandType.WarmachineBarracks: return WarmachineBarracks_count;
                case BuildAndExpandType.GunBarracks: return GunBarracks_count;
                case BuildAndExpandType.CannonBarracks: return CannonBarracks_count;
                //case BuildAndExpandType.KnightsBarracks: return KnightsBarracks_count;
               
                default: return 0; // Return 0 for NUM_NONE or any other undefined type
            }
        }

        public int AllBarracksCount()
        { 
            return SoldierBarracks_count + ArcherBarracks_count + WarmachineBarracks_count + GunBarracks_count + CannonBarracks_count;
        }
    }
}
