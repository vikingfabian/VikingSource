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
    struct BuildingPosition
    {
        // --- Existing Fields ---
        public IntVector2 WorkerHuts_pos;
        public IntVector2 ServiceHouse_pos;
        public IntVector2 Postal_pos;
        public IntVector2 Recruitment_pos;
        public IntVector2 SoldierBarracks_pos;
        public IntVector2 ImmigrationTent_pos;
        public IntVector2 Nobelhouse_pos;
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

        public IntVector2 SuggestedTrapperPos;

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

                case BuildAndExpandType.Postal:
                case BuildAndExpandType.PostalLevel2:
                case BuildAndExpandType.PostalLevel3:
                    return Postal_pos;

                case BuildAndExpandType.Recruitment:
                case BuildAndExpandType.RecruitmentLevel2:
                case BuildAndExpandType.RecruitmentLevel3:
                    return Recruitment_pos;

                case BuildAndExpandType.SoldierBarracks: return SoldierBarracks_pos;
                case BuildAndExpandType.Nobelhouse: return Nobelhouse_pos;
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

                default:
                    throw new NotImplementedException($"getPos() not implemented for {type}");
            }
        }
    }

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
        static readonly SubTile TerrainType_stoneblock = new SubTile(TerrainMainType.Foil, (int)TerrainSubFoilType.StoneBlock); //New

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

            // Added StoneBlock (Brick) and Clay to natural resources
            naturalResource(player, content, mineCount_stoneblock, ItemResourceType.Brick, TerrainType_stoneblock, ref totalCount);
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
                        var habitatString = string.Format(DssRef.todoLang.Terrain_XAnimalHabitat, resourceName);
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
        public int Nobelhouse_count;
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
                case BuildAndExpandType.Nobelhouse: return Nobelhouse_count;
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

                case BuildAndExpandType.PigPen: return PigPen_count;
                case BuildAndExpandType.HenPen: return HenPen_count;
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

                // --- NEW Animals ---
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

                default: return 0;
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
                case BuildAndExpandType.Nobelhouse: Nobelhouse_count += add; break;
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
