using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.DSSWars.Map.MapProcess.CityStructure;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Build
{
    static class AvailableBuildTypes
    {
        public static void Available(List<BuildAndExpandType> list, City city, bool autoBuild)
        {
            bool godPowers = city == null || (DssRef.state.GodPowers() || (StartupSettings.UnlockAllProgress && city.pfaction.TryGetLocalPlayer(out _))) && !autoBuild;

            bool devUnlockAll = StartupSettings.UnlockAllProgress;
            BuildingStructure buildingStructure = new BuildingStructure();
            XP.Unlocks unlocks = new XP.Unlocks();
            if (city != null)
            {
                unlocks = city.technology.GetUnlocks(false);
                buildingStructure = city.buildingStructure;
            }
            if (godPowers)
            {
                unlocks.unlockAll();
            }

            bool logistics1 = godPowers || city.buildingStructure.buildingLevel_logistics >= 1 ||
                 city.buildingStructure.tingshouse;

            bool logistics2 = godPowers || city.buildingStructure.buildingLevel_logistics >= 2 ||
                 city.buildingStructure.tingshouse;

            bool economics = godPowers || city.buildingStructure.economics ||
                 city.buildingStructure.tingshouse;

            bool manor = godPowers || city.buildingStructure.manorLord ||
                 city.buildingStructure.tingshouse;

            bool campSite = city != null && city.cityType == CityType.Campsite;

            if (!campSite && !logistics1)
            {
                list.Add(BuildAndExpandType.Logistics);
            }

            if (!campSite && !economics)
            {
                list.Add(BuildAndExpandType.Economics);
            }
            if (godPowers || (!campSite && !city.buildingStructure.tingshouse))
            {
                list.Add(BuildAndExpandType.TingsHouse);
            }

            if (logistics1)
            {
                if (!manor && DssRef.storage.ruleset_instance.module_foodAndWater)
                {
                    list.Add(BuildAndExpandType.ManorLord);
                }
                if (godPowers || !city.buildingStructure.greatHall)
                {
                    list.Add(BuildAndExpandType.GreatHall);
                }

                list.Add(BuildAndExpandType.ExpansionHall_Lvl1);
                list.Add(BuildAndExpandType.ExpansionHall_Lvl2);
                list.Add(BuildAndExpandType.ExpansionHall_Lvl3);
                list.Add(BuildAndExpandType.School);
                list.Add(BuildAndExpandType.ResearchCenter);
                list.Add(BuildAndExpandType.BookPress);
            }

            if (campSite)
            {
                list.Add(BuildAndExpandType.WorkerTent);
            }
            else
            {
                if (godPowers)
                {
                    list.Add(BuildAndExpandType.WorkerTent);
                }
                list.Add(BuildAndExpandType.WorkerHut);
                if (logistics1)
                {
                    list.Add(BuildAndExpandType.WorkerHutLarge);
                }
            }

            list.Add(BuildAndExpandType.ServiceHouse_Small);
            if (logistics1)
            {
                list.Add(BuildAndExpandType.ServiceHouse_Large);
            }

            list.Add(BuildAndExpandType.SoldiersQuarter_Small);
            if (logistics1)
            {
                list.Add(BuildAndExpandType.SoldiersQuarter_Large);
            }

            list.Add(BuildAndExpandType.GuardHouse_Small);
            if (logistics1)
            {
                list.Add(BuildAndExpandType.GuardHouse_Large);
                list.Add(BuildAndExpandType.ImmigrationTent);

            }

            if (DssRef.storage.ruleset_instance.module_foodAndWater)
            {
                list.Add(BuildAndExpandType.OrchardApple);
            }
            if (manor)
            {
                list.Add(BuildAndExpandType.WheatFarm);
                if (unlocks.building_upgradedFarm || godPowers)
                {
                    list.Add(BuildAndExpandType.WheatFarmUpgraded);
                }
            }

            list.Add(BuildAndExpandType.LinenFarm);
            if (unlocks.building_upgradedFarm)
            {
                list.Add(BuildAndExpandType.LinenFarmUpgraded);
            }

            list.Add(BuildAndExpandType.RapeSeedFarm);
            if (unlocks.building_upgradedFarm)
            {
                list.Add(BuildAndExpandType.RapeSeedFarmUpgraded);
            }

            if (manor)
            {
                if (unlocks.building_mixedFarms)
                {
                    list.Add(BuildAndExpandType.HempFarm);
                    if (unlocks.building_upgradedFarm)
                    {
                        list.Add(BuildAndExpandType.HempFarmUpgraded);
                    }
                }
            }

            if (logistics1)
            {
                list.Add(BuildAndExpandType.TrapperHut);

                addAnimalPen(BuildAndExpandType.FowlPen, CityResourceIndex.Fowl, buildingStructure.FowlPen_count);
                addAnimalPen(BuildAndExpandType.HenPen, CityResourceIndex.Hen, buildingStructure.HenPen_count);

                addAnimalPen(BuildAndExpandType.BoarPen, CityResourceIndex.Boar, buildingStructure.BoarPen_count);
                addAnimalPen(BuildAndExpandType.PigPen, CityResourceIndex.Pig, buildingStructure.PigPen_count);

                // --- Wild Pigs / Hogs ---
                if (biomRequirement(CityBiome.Mountain))
                {
                    addAnimalPen(BuildAndExpandType.WildPigPen, CityResourceIndex.WildPig, buildingStructure.WildPigPen_count);
                    addAnimalPen(BuildAndExpandType.WildHogPen, CityResourceIndex.WildHog, buildingStructure.WildHogPen_count);
                    addAnimalPen(BuildAndExpandType.WarHogPen, CityResourceIndex.WarHog, buildingStructure.WarHogPen_count);
                    addAnimalPen(BuildAndExpandType.StagHogPen, CityResourceIndex.StagHog, buildingStructure.StagHogPen_count);
                }

                // --- Oxen ---
                addAnimalPen(BuildAndExpandType.OxenPen, CityResourceIndex.Oxen, buildingStructure.OxenPen_count);
                addAnimalPen(BuildAndExpandType.KineOxenPen, CityResourceIndex.KineOxen, buildingStructure.KineOxenPen_count);

                // --- Dogs ---
                addAnimalPen(BuildAndExpandType.DogCage, CityResourceIndex.Dog, buildingStructure.DogCage_count);
                addAnimalPen(BuildAndExpandType.HoundCage, CityResourceIndex.Hound, buildingStructure.HoundCage_count);

                // --- Horses ---
                addAnimalPen(BuildAndExpandType.PonyPen, CityResourceIndex.Pony, buildingStructure.PonyPen_count);
                addAnimalPen(BuildAndExpandType.HorsePen, CityResourceIndex.Horse, buildingStructure.HorsePen_count);
                addAnimalPen(BuildAndExpandType.WarHorsePen, CityResourceIndex.WarHorse, buildingStructure.WarHorsePen_count);
                addAnimalPen(BuildAndExpandType.DraftHorsePen, CityResourceIndex.DraftHorse, buildingStructure.DraftHorsePen_count);


                // --- Wolves ---
                if (biomRequirement(CityBiome.Desolate))
                {
                    addAnimalPen(BuildAndExpandType.WolfCage, CityResourceIndex.Wolf, buildingStructure.WolfCage_count);
                    addAnimalPen(BuildAndExpandType.WargCage, CityResourceIndex.Warg, buildingStructure.WargCage_count);
                    addAnimalPen(BuildAndExpandType.AlphaWargCage, CityResourceIndex.AlphaWarg, buildingStructure.AlphaWargCage_count);
                }

                // --- Cats ---
                if (biomRequirement(CityBiome.Forest))
                {
                    addAnimalPen(BuildAndExpandType.WildCatCage, CityResourceIndex.WildCat, buildingStructure.WildCatCage_count);
                    addAnimalPen(BuildAndExpandType.LionCage, CityResourceIndex.Lion, buildingStructure.LionCage_count);
                    addAnimalPen(BuildAndExpandType.WarLionCage, CityResourceIndex.WarLion, buildingStructure.WarLionCage_count);
                }
                // --- Elephants ---
                if (biomRequirement(CityBiome.Desert))
                {
                    addAnimalPen(BuildAndExpandType.ElephantCage, CityResourceIndex.Elephant, buildingStructure.ElephantCage_count);
                    addAnimalPen(BuildAndExpandType.WarElephantCage, CityResourceIndex.WarElephant, buildingStructure.WarElephantCage_count);
                    addAnimalPen(BuildAndExpandType.OliphantCage, CityResourceIndex.Oliphant, buildingStructure.OliphantCage_count);
                }
            }


            if (buildingStructure.WoodCutter_count > 0 ||
                godPowers)
            {
                list.Add(BuildAndExpandType.TreeSeedlingSoft);
                list.Add(BuildAndExpandType.TreeSeedlingHard);
            }


            if (economics)
            {
                list.Add(BuildAndExpandType.Postal);
                list.Add(BuildAndExpandType.PostalLevel2);
                list.Add(BuildAndExpandType.PostalLevel3);
                list.Add(BuildAndExpandType.Recruitment);
                list.Add(BuildAndExpandType.RecruitmentLevel2);
                list.Add(BuildAndExpandType.RecruitmentLevel3);

                if (!DssRef.storage.ruleset_instance.module_physicalGold)
                {
                    list.Add(BuildAndExpandType.GoldDeliveryLvl1);
                    list.Add(BuildAndExpandType.GoldDeliveryLvl2);
                    list.Add(BuildAndExpandType.GoldDeliveryLvl3);
                }
            }

            if (logistics1)
            {
                list.Add(BuildAndExpandType.MaterialStorage);
                list.Add(BuildAndExpandType.FoodStorage);
                list.Add(BuildAndExpandType.WeaponStorage);
                list.Add(BuildAndExpandType.ArmorStorage);
                list.Add(BuildAndExpandType.AnimalStorage);
                list.Add(BuildAndExpandType.Cesspit);

                list.Add(BuildAndExpandType.Storehouse);
                list.Add(BuildAndExpandType.Tavern);
                if (manor)
                {
                    list.Add(BuildAndExpandType.Brewery);
                }
                list.Add(BuildAndExpandType.WaterResovoir);

                list.Add(BuildAndExpandType.CoalPit);
            }

            list.Add(BuildAndExpandType.WaterWell);
            //else
            //{
            //    list.Add(BuildAndExpandType.FoodStorage);
            //}

            list.Add(BuildAndExpandType.WorkBench);
            if (manor && DssRef.storage.ruleset_instance.module_foodAndWater)
            {
                list.Add(BuildAndExpandType.Cook);
            }

            if (logistics1)
            {
                list.Add(BuildAndExpandType.Butcher);
                if (DssRef.storage.ruleset_instance.module_foodAndWater)
                {
                    list.Add(BuildAndExpandType.Smoker);

                    if (biomRequirement(CityBiome.Desert))
                    {
                        list.Add(BuildAndExpandType.Dryer);
                        list.Add(BuildAndExpandType.DryingPan);
                    }
                }
            }
            //list.Add(BuildAndExpandType.Cook);
            list.Add(BuildAndExpandType.Smelter);
            list.Add(BuildAndExpandType.Foundry);
            list.Add(BuildAndExpandType.Smith);

            list.Add(BuildAndExpandType.Carpenter);

            list.Add(BuildAndExpandType.Pottery);



            if (logistics1)
            {
                list.Add(BuildAndExpandType.Armory);
                list.Add(BuildAndExpandType.ShieldMaker);
            }
            if (unlocks.building_chemist)
            {
                list.Add(BuildAndExpandType.Chemist);
            }
            if (unlocks.building_gunmaker)
            {
                list.Add(BuildAndExpandType.Gunmaker);
            }

            list.Add(BuildAndExpandType.SoldierBarracks);
            list.Add(BuildAndExpandType.ArcherBarracks);

            if (logistics1)
            {
                list.Add(BuildAndExpandType.WarmachineBarracks);
            }
            if (unlocks.building_gunBarrack || godPowers)
            {
                list.Add(BuildAndExpandType.GunBarracks);
            }
            if (unlocks.building_cannonBarrack ||
                godPowers)
            {
                list.Add(BuildAndExpandType.CannonBarracks);
            }

            list.Add(BuildAndExpandType.AnimalTrainer);

            if (logistics1)
            {
                list.Add(BuildAndExpandType.WoodCutter);
                list.Add(BuildAndExpandType.StoneCutter);
            }

            if (!campSite && unlocks.building_stoneBuildings)
            {
                list.Add(BuildAndExpandType.Noblehouse);

                if (buildingStructure.Noblehouse_count > 0 ||
                    godPowers)
                {
                    list.Add(BuildAndExpandType.Embassy);
                }

                list.Add(BuildAndExpandType.Bank);
                if (buildingStructure.Bank_count > 0 ||
                    godPowers)
                {
                    list.Add(BuildAndExpandType.CoinMinter);
                }
            }

            list.Add(BuildAndExpandType.DirtRoad);
            list.Add(BuildAndExpandType.GravelRoad);
            list.Add(BuildAndExpandType.CobblestoneRoad);
            list.Add(BuildAndExpandType.PavedRoad);

            if (logistics2)
            {
                list.Add(BuildAndExpandType.PavementStreet);
                list.Add(BuildAndExpandType.PavementFlower);
                list.Add(BuildAndExpandType.PavementRectFlower);
                list.Add(BuildAndExpandType.PavementLamp);
                list.Add(BuildAndExpandType.PavementFountain);

                list.Add(BuildAndExpandType.GardenGrass);
                list.Add(BuildAndExpandType.GardenBird);
                list.Add(BuildAndExpandType.GardenFourBushes);
                list.Add(BuildAndExpandType.GardenLongTree);
                list.Add(BuildAndExpandType.GardenWalledBush);

                list.Add(BuildAndExpandType.Statue_Leader);
                list.Add(BuildAndExpandType.Statue_Lion);
                list.Add(BuildAndExpandType.Statue_Horse);
                list.Add(BuildAndExpandType.Statue_Pillar);

            }
            if (unlocks.building_stoneBuildings ||
                godPowers)
            {
                list.Add(BuildAndExpandType.Statue_ThePlayer);
            }

            if (godPowers || city.GetGroupedResource(ItemResourceType.Palisade).amount > 0)
            {
                list.Add(BuildAndExpandType.Palisade);
            }

            list.Add(BuildAndExpandType.DirtWall);
            list.Add(BuildAndExpandType.DirtTower);

            if (logistics1)
            {
                list.Add(BuildAndExpandType.WoodWall);
                list.Add(BuildAndExpandType.WoodTower);
                list.Add(BuildAndExpandType.StoneWall);
                list.Add(BuildAndExpandType.StoneTower);
                list.Add(BuildAndExpandType.StoneWallGreen);
                list.Add(BuildAndExpandType.StoneWallBlueRoof);
                list.Add(BuildAndExpandType.StoneWallWoodHouse);
                list.Add(BuildAndExpandType.StoneGate);
                list.Add(BuildAndExpandType.StoneHouse);
                list.Add(BuildAndExpandType.CitySquare);
            }

            //list.Add(BuildAndExpandType.CobbleStones);
            list.Add(BuildAndExpandType.GardenMemoryStone);

            if (logistics1)
            {
                list.Add(BuildAndExpandType.FlagPole_LongBanner);
                list.Add(BuildAndExpandType.FlagPole_Banner);
                list.Add(BuildAndExpandType.FlagPole_SlimBanner);

                list.Add(BuildAndExpandType.FlagPole_Flag);
                list.Add(BuildAndExpandType.FlagPole_FlagRound);
                list.Add(BuildAndExpandType.FlagPole_FlagLarge);
                list.Add(BuildAndExpandType.FlagPole_Streamer);
                list.Add(BuildAndExpandType.FlagPole_Triangle);

                if (DssRef.DlcSupporter.owned)
                {
                    list.Add(BuildAndExpandType.DiplomaticStatue_ThumbsUpWest);
                    list.Add(BuildAndExpandType.DiplomaticStatue_ThumbsUpEast);
                    list.Add(BuildAndExpandType.DiplomaticStatue_InsultWest);
                    list.Add(BuildAndExpandType.DiplomaticStatue_InsultEast);
                    list.Add(BuildAndExpandType.DiplomaticStatue_GoldenPoop);
                }
            }


            if (DssRef.state.GodPowers())
            {
                list.Add(BuildAndExpandType.TreeSoft);
                list.Add(BuildAndExpandType.TreeHard);
                list.Add(BuildAndExpandType.CoalMine);
                list.Add(BuildAndExpandType.IronOreMine);
                list.Add(BuildAndExpandType.TinOreMine);
                list.Add(BuildAndExpandType.CopperOreMine);
                list.Add(BuildAndExpandType.SilverOreMine);
                list.Add(BuildAndExpandType.GoldOreMine);
                list.Add(BuildAndExpandType.LeadOreMine);
                list.Add(BuildAndExpandType.MithrilMine);
                list.Add(BuildAndExpandType.SulfurMine);
            }


            void addAnimalPen(BuildAndExpandType build, int cityResourceType, int buildingCount)
            {
                if (godPowers)
                {
                    list.Add(build);
                }
                else if (
                    (logistics1 && (buildingCount > 0 || city.GetGroupedResource(cityResourceType).amount > 0)) ||
                    StartupSettings.UnlockAllProgress)
                {
                    list.Add(build);
                }
            }

            bool biomRequirement(CityBiome biom)
            {
                return godPowers || biom == CityBiome.Default_Fields || city.cityBiome == biom || StartupSettings.UnlockAllProgress;
            }
        }
    }
}
