using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.Build
{
    

    struct TerrainStructure
    {
        public static readonly ItemResourceType[] AllTerrainResources = {
        ItemResourceType.Wood_Group,
        ItemResourceType.Stone_G,
        ItemResourceType.Brick,
        ItemResourceType.Coal,

        ItemResourceType.Clay,
        ItemResourceType.BogIron,
        ItemResourceType.IronOre_G,
        ItemResourceType.TinOre,
        ItemResourceType.CopperOre,
        ItemResourceType.LeadOre,
        ItemResourceType.SilverOre,
        ItemResourceType.GoldOre,
        ItemResourceType.RawMithril,
        ItemResourceType.Salt,
        ItemResourceType.Sulfur,
    };

        public int mineCount_bogIron;

        public int mineCount_iron;
        public int mineCount_tin;
        public int mineCount_copper;
        public int mineCount_lead;
        public int mineCount_silver;
        public int mineCount_gold;
        public int mineCount_mithril;
        public int mineCount_sulfur;
        public int mineCount_coal;

        // --- New Mine Count ---
        public int mineCount_salt;

        public int resourceCount_stone;
        public int resourceCount_wood;

        // --- New Resource Counts ---
        public int mineCount_stoneblock;
        public int resourceCount_clay;

        public int wildAnimalCount_Fowl;
        public int wildAnimalCount_Boar;
        public int wildAnimalCount_Dog;
        public int wildAnimalCount_Ox;

        public int wildAnimalCount_Pony;
        public int wildAnimalCount_Wolf;
        public int wildAnimalCount_Cat;
        public int wildAnimalCount_Elephant;


        static readonly SubTile TerrainType_wood = new SubTile(TerrainMainType.Foil, (int)TerrainSubFoilType.TreeSoft);
        static readonly SubTile TerrainType_stone = new SubTile(TerrainMainType.Foil, (int)TerrainSubFoilType.Stones);
        static readonly SubTile TerrainType_stoneblock = new SubTile(TerrainMainType.Mine, (int)TerrainMineType.StoneBlock); //New

        static readonly SubTile TerrainType_clay = new SubTile(TerrainMainType.Foil, (int)TerrainSubFoilType.ClayPit); //New
        static readonly SubTile TerrainType_bogiron = new SubTile(TerrainMainType.Foil, (int)TerrainSubFoilType.BogIron);
        static readonly SubTile TerrainType_iron = new SubTile(TerrainMainType.Mine, (int)TerrainMineType.IronOre);
        static readonly SubTile TerrainType_tin = new SubTile(TerrainMainType.Mine, (int)TerrainMineType.TinOre);
        static readonly SubTile TerrainType_copper = new SubTile(TerrainMainType.Mine, (int)TerrainMineType.CopperOre);
        static readonly SubTile TerrainType_lead = new SubTile(TerrainMainType.Mine, (int)TerrainMineType.LeadOre);
        static readonly SubTile TerrainType_silver = new SubTile(TerrainMainType.Mine, (int)TerrainMineType.SilverOre);
        static readonly SubTile TerrainType_gold = new SubTile(TerrainMainType.Mine, (int)TerrainMineType.GoldOre);
        static readonly SubTile TerrainType_mithril = new SubTile(TerrainMainType.Mine, (int)TerrainMineType.Mithril);

        static readonly SubTile TerrainType_salt = new SubTile(TerrainMainType.Mine, (int)TerrainMineType.Salt); //New
        static readonly SubTile TerrainType_sulfur = new SubTile(TerrainMainType.Mine, (int)TerrainMineType.Sulfur);
        static readonly SubTile TerrainType_coal = new SubTile(TerrainMainType.Mine, (int)TerrainMineType.Coal);

        static readonly SubTile TerrainType_fowlHabitat = new SubTile(TerrainMainType.Building, (int)TerrainBuildingType.FowlHabitat);
        static readonly SubTile TerrainType_boarHabitat = new SubTile(TerrainMainType.Building, (int)TerrainBuildingType.BoarHabitat);
        static readonly SubTile TerrainType_dogHabitat = new SubTile(TerrainMainType.Building, (int)TerrainBuildingType.DogHabitat);
        static readonly SubTile TerrainType_oxHabitat = new SubTile(TerrainMainType.Building, (int)TerrainBuildingType.OxHabitat);
        static readonly SubTile TerrainType_ponyHabitat = new SubTile(TerrainMainType.Building, (int)TerrainBuildingType.PonyHabitat);
        static readonly SubTile TerrainType_wolfHabitat = new SubTile(TerrainMainType.Building, (int)TerrainBuildingType.WolfHabitat);
        static readonly SubTile TerrainType_catHabitat = new SubTile(TerrainMainType.Building, (int)TerrainBuildingType.CatHabitat);
        static readonly SubTile TerrainType_elephantHabitat = new SubTile(TerrainMainType.Building, (int)TerrainBuildingType.ElephantHabitat);


        public bool HasIndependantResources()
        {
            return mineCount_bogIron + mineCount_bogIron >= 1 &&
                resourceCount_wood >= 3 &&
                resourceCount_stone >= 1;
        }

        public void miningOverviewHud(LocalPlayer player, RichBoxContent content)
        {
            content.newLine();

            content.Add(new RbImage(SpriteName.WarsWorkMine));
            content.space();

            int totalCount = 0;

            naturalResource(player, content, resourceCount_wood, ItemResourceType.Wood_Group, TerrainType_wood, ref totalCount);
            naturalResource(player, content, resourceCount_stone, ItemResourceType.Stone_G, TerrainType_stone, ref totalCount);
            
            naturalResource(player, content, resourceCount_clay, ItemResourceType.Clay, TerrainType_clay, ref totalCount);

            animalHabitat(player, content, wildAnimalCount_Fowl, ItemResourceType.Fowl, TerrainType_fowlHabitat);
            animalHabitat(player, content, wildAnimalCount_Boar, ItemResourceType.Boar, TerrainType_boarHabitat);
            animalHabitat(player, content, wildAnimalCount_Dog, ItemResourceType.Dog, TerrainType_dogHabitat);
            animalHabitat(player, content, wildAnimalCount_Ox, ItemResourceType.Oxen, TerrainType_oxHabitat);
            animalHabitat(player, content, wildAnimalCount_Pony, ItemResourceType.Pony, TerrainType_ponyHabitat);
            animalHabitat(player, content, wildAnimalCount_Wolf, ItemResourceType.Wolf, TerrainType_wolfHabitat);
            animalHabitat(player, content, wildAnimalCount_Cat, ItemResourceType.WildCat, TerrainType_catHabitat);
            animalHabitat(player, content, wildAnimalCount_Elephant, ItemResourceType.Elephant, TerrainType_elephantHabitat);


            mine(player, content, mineCount_coal, ItemResourceType.Coal, TerrainType_coal, ref totalCount);
            mine(player, content, mineCount_bogIron, ItemResourceType.BogIron, TerrainType_bogiron, ref totalCount);
            mine(player, content, mineCount_iron, ItemResourceType.Iron_G, TerrainType_iron, ref totalCount);
            mine(player, content, mineCount_tin, ItemResourceType.Tin, TerrainType_tin, ref totalCount);
            mine(player, content, mineCount_copper, ItemResourceType.Copper, TerrainType_copper, ref totalCount);
            mine(player, content, mineCount_lead, ItemResourceType.Lead, TerrainType_lead, ref totalCount);
            mine(player, content, mineCount_silver, ItemResourceType.Silver, TerrainType_silver, ref totalCount);
            mine(player, content, mineCount_gold, ItemResourceType.Gold, TerrainType_gold, ref totalCount);
            mine(player, content, mineCount_mithril, ItemResourceType.Mithril, TerrainType_mithril, ref totalCount);

            // Added Salt to mines
            mine(player, content, mineCount_salt, ItemResourceType.Salt, TerrainType_salt, ref totalCount);
            mine(player, content, mineCount_sulfur, ItemResourceType.Sulfur, TerrainType_sulfur, ref totalCount);
            mine(player, content, mineCount_stoneblock, ItemResourceType.Brick, TerrainType_stoneblock, ref totalCount);

            if (totalCount == 0)
            {
                content.Add(new RbText(DssRef.lang.Hud_EmptyList));
            }

        }



        public int Get(ItemResourceType type)
        {
            switch (type)
            {
                case ItemResourceType.BogIron:
                    return mineCount_bogIron;
                case ItemResourceType.IronOre_G:
                    return mineCount_iron;
                case ItemResourceType.TinOre:
                    return mineCount_tin;
                case ItemResourceType.CopperOre:
                    return mineCount_copper;
                case ItemResourceType.LeadOre:
                    return mineCount_lead;
                case ItemResourceType.SilverOre:
                    return mineCount_silver;
                case ItemResourceType.GoldOre:
                    return mineCount_gold;
                case ItemResourceType.RawMithril:
                    return mineCount_mithril;
                case ItemResourceType.Sulfur:
                    return mineCount_sulfur;
                case ItemResourceType.Coal:
                    return mineCount_coal;
                case ItemResourceType.Stone_G:
                    return resourceCount_stone;
                case ItemResourceType.Wood_Group:
                    return resourceCount_wood;

                // --- New Get Cases ---
                case ItemResourceType.Salt:
                    return mineCount_salt;
                case ItemResourceType.Clay:
                    return resourceCount_clay;
                case ItemResourceType.Brick:
                    return mineCount_stoneblock;

                default:
                    return 0;
            }
        }



        public void naturalResource(LocalPlayer player, RichBoxContent content, int count, ItemResourceType resource, SubTile terrainType, ref int totalCount)
        {
            resourceHoverButton(player, content, count, resource, DssRef.lang.Work_GatherXResource, SpriteName.WarsWorkCollect, terrainType, false, ref totalCount);
        }

        public void animalHabitat(LocalPlayer player, RichBoxContent content, int count, ItemResourceType resource, SubTile terrainType)
        {
            if (count > 0)
            {
                IconName.Item(resource, out SpriteName icon, out string resourceName);

                var infoContent = new RichBoxContent();

                infoContent.Add(new RbImage(icon));
                infoContent.space();
                var countText = new RbText(count.ToString());
                countText.overrideColor = Color.White;
                infoContent.Add(countText);

                var infoButton = new ArtButton(RbButtonStyle.Outline, infoContent, new RbAction1Arg<SubTile>(player.gameControls.map.terrainSearchClick, terrainType),
                    new RbTooltip((RichBoxContent content, object tag) =>
                    {
                        content.Add(new RbImage(icon));
                        content.space();
                        var habitatString = string.Format(DssRef.lang.Terrain_XAnimalHabitat, resourceName);
                        content.Add(new RbText(TextLib.LargeFirstLetter(string.Format(DssRef.lang.Language_XCountIsY, habitatString, count))));

                    }));

                content.Add(infoButton);
            }
        }

        public void mine(LocalPlayer player, RichBoxContent content, int count, ItemResourceType resource, SubTile terrainType, ref int totalCount)
        {
            resourceHoverButton(player, content, count, resource, DssRef.lang.BuildingType_ResourceMine, SpriteName.WarsWorkMine, terrainType, terrainType.mainTerrain != TerrainMainType.NUM, ref totalCount);
        }

        public void resourceHoverButton(LocalPlayer player, RichBoxContent content, int count, ItemResourceType resource, string categoryName, SpriteName workIcon, SubTile terrainType, bool clickable, ref int totalCount)
        {
            totalCount += count;
            if (count > 0)
            {
                IconName.Item(resource, out SpriteName icon, out string resourceName);
                
                var infoContent = new RichBoxContent();

                infoContent.Add(new RbOverlapImage(new RbImage(icon), workIcon, VectorExt.V2FromX(-0.2f), 0.8f));
                infoContent.space();
                var countText = new RbText(count.ToString());
                countText.overrideColor = Color.White;
                infoContent.Add(countText);

                var infoButton = new ArtButton(clickable? RbButtonStyle.Outline : RbButtonStyle.HoverArea, infoContent, clickable? new RbAction1Arg<SubTile>( player.gameControls.map.terrainSearchClick, terrainType) : null,
                    new RbTooltip((RichBoxContent content, object tag) =>
                    {
                        content.Add(new RbOverlapImage(new RbImage(icon), workIcon, Vector2.Zero, 0.8f));
                        content.space();
                        var mineString = string.Format(categoryName, resourceName);
                        content.Add(new RbText(TextLib.LargeFirstLetter(string.Format(DssRef.lang.Language_XCountIsY, mineString, count))));

                    }));

                content.Add(infoButton);
            }
        }
    }

    struct BuildingStructure
    {

        public bool manorLord;
        public bool greatHall;
        public int buildingLevel_logistics;
        public int TentHuts_count;
        public int WorkerHuts_count;
        public int WorkerHuts_Large_count;
        public int ServiceMenHouse_count;
        public int ServiceMenHouse_Large_count;
        public int GuardOffice_count;
        public int GuardOffice_Large_count;
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
        public int PavementFlower_count;
        public int Bank_count;
        public int CoinMinter_count;
        public int GoldDelivery_count;
        public int WoodCutter_count;
        public int StoneCutter_count;
        public int Embassy_count;
        public int WaterResovoir_count;
        public int ArcherBarracks_count;
        public int WarmachineBarracks_count;
        public int GunBarracks_count;
        public int CannonBarracks_count;
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
                case BuildAndExpandType.Pavement: return Pavement_count;
                case BuildAndExpandType.PavementFlower: return PavementFlower_count;
                case BuildAndExpandType.Bank: return Bank_count;
                case BuildAndExpandType.CoinMinter: return CoinMinter_count;

                case BuildAndExpandType.GoldDeliveryLvl1: return GoldDelivery_count;
                case BuildAndExpandType.GoldDeliveryLvl2: return GoldDelivery_count;
                case BuildAndExpandType.GoldDeliveryLvl3: return GoldDelivery_count;

                case BuildAndExpandType.WoodCutter: return WoodCutter_count;
                case BuildAndExpandType.StoneCutter: return StoneCutter_count;
                case BuildAndExpandType.Embassy: return Embassy_count;
                case BuildAndExpandType.WaterResovoir: return WaterResovoir_count;
                case BuildAndExpandType.ArcherBarracks: return ArcherBarracks_count;
                case BuildAndExpandType.WarmachineBarracks: return WarmachineBarracks_count;
                case BuildAndExpandType.GunBarracks: return GunBarracks_count;
                case BuildAndExpandType.CannonBarracks: return CannonBarracks_count;
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

                case BuildAndExpandType.TreeHard:
                case BuildAndExpandType.TreeSoft:
                case BuildAndExpandType.TreeSeedlingSoft:
                case BuildAndExpandType.TreeSeedlingHard:
                case BuildAndExpandType.DirtRoad:
                case BuildAndExpandType.PavementLamp:
                case BuildAndExpandType.PavemenFountain:
                case BuildAndExpandType.PavementRectFlower:
                case BuildAndExpandType.GardenGrass:
                case BuildAndExpandType.GardenFourBushes:
                case BuildAndExpandType.GardenLongTree:
                case BuildAndExpandType.GardenWalledBush:
                case BuildAndExpandType.CitySquare:
                case BuildAndExpandType.CobbleStones:
                case BuildAndExpandType.GardenBird:
                case BuildAndExpandType.GardenMemoryStone:
                case BuildAndExpandType.Statue_Leader:
                case BuildAndExpandType.Statue_Lion:
                case BuildAndExpandType.Statue_Horse:
                case BuildAndExpandType.Statue_Pillar:


                case BuildAndExpandType.FlagPole_LongBanner:
                case BuildAndExpandType.FlagPole_Banner:
                case BuildAndExpandType.FlagPole_SlimBanner:

                case BuildAndExpandType.FlagPole_Flag:
                case BuildAndExpandType.FlagPole_FlagRound:
                case BuildAndExpandType.FlagPole_FlagLarge:
                case BuildAndExpandType.FlagPole_Streamer:
                case BuildAndExpandType.FlagPole_Triangle:
                    return 0;

                case BuildAndExpandType.TrapperHut:
                    return TrapperHut_count;
                case BuildAndExpandType.GuardHouse_Small:
                    return GuardOffice_count;
                case BuildAndExpandType.GuardHouse_Large:
                    return GuardOffice_Large_count;

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
                case BuildAndExpandType.ManorLord:
                case BuildAndExpandType.GreatHall:
                case BuildAndExpandType.Palisade:
                    return wallCount;

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
                case BuildAndExpandType.Pavement: Pavement_count += add; break;
                case BuildAndExpandType.PavementFlower: PavementFlower_count += add; break;
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
