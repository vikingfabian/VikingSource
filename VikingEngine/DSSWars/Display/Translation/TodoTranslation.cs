﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    class TodoTranslation
    {
        public string CityMenu_SalePricesTitle => "Sale prices";

        public string Resource_TypeName_Water => "water";
        public string Resource_TypeName_Wood => "wood";
        public string Resource_TypeName_Stone => "stone";
        public string Resource_TypeName_RawFood => "raw food";
        public string Resource_TypeName_Food => "food";
        public string Resource_TypeName_SkinAndLinnen => "skin and linnen";
        public string Resource_TypeName_Ore => "ore";
        public string Resource_TypeName_Iron => "iron";

        
        public string Resource_TypeName_SharpStick => "Sharp stick";
        public string Resource_TypeName_Sword => "Sword";
        public string Resource_TypeName_Bow => "Bow";

        public string Resource_TypeName_LightArmor => "Light armor";
        public string Resource_TypeName_MediumArmor => "Medium armor";
        public string Resource_TypeName_HeavyArmor => "Heavy armor";

        public string ResourceType_Children => "Children";

        public string BuildingType_WorkerHut => "Worker hut";
        public string BuildingType_Tavern => "Tavern";
        public string BuildingType_Postal => "Postal service";
        public string BuildingType_Recruitment => "Recruiting center";
        public string BuildingType_Barracks => "Barracks";
        public string BuildingType_PigPen => "Pig pen";
        public string BuildingType_HenPen => "Hen pen";

        public string BuildingType_WorkerHut_DescriptionLimitX => "Expands worker limit with {0}";
        public string BuildingType_Tavern_Description => "Workers may eat here";
        public string BuildingType_Postal_Description => "Send resources to other cities";
        public string BuildingType_Recruitment_Description => "Send men to other cities";
        public string BuildingType_Barracks_Description => "Uses men and equipment to recruit soldiers";
        public string BuildingType_PigPen_Description => "Produces pigs, which give food and skin";
        public string BuildingType_HenPen_Description => "Produces hens and eggs, which give food";


        public string MenuTab_Info => "Info";
        public string MenuTab_Work => "Work";
        public string MenuTab_Recruit => "Recruit";
        public string MenuTab_Resources => "Resources";
        public string MenuTab_Trade => "Trade";
        public string MenuTab_Build => "Build";
        public string MenuTab_Economy => "Economy";
        public string MenuTab_Delivery => "Delivery";

        public string BuildHud_OutsideCity => "Outside city region";
        public string BuildHud_OutsideFaction => "Outside your borders!";
        
        /// <summary>
        /// Info when the square is covoered with a building or blocking terrain
        /// </summary>
        public string BuildHud_OccupiedTile => "Occupied tile";

        public string Build_PlaceBuilding => "Building";
        public string Build_DestroyBuilding => "Destroy";
        public string Build_ClearTerrain => "Clear terrain";

        public string Work_OrderPrioTitle => "Work priority";
        public string Work_OrderPrioDescription => "Priority goes from 1 (low) to {0} (high)";

        public string Work_Move => "Move items";

        public string Work_GatherXResource => "Gather {0}";
        public string Work_CraftX => "Craft {0}";
        public string Work_Farming => "Farming";
        public string Work_Mining => "Mining";
        public string Work_Trading => "Tradeing";

        public string Work_ExpandHousing => "Expand housing";
        public string Work_ExpandFarms => "Expand farms";

        public string Hud_ToggleFollowFaction => "Toggle follow faction settings";
        public string Hud_FollowFaction_Yes => "Is set to use faction global settings";
        public string Hud_FollowFaction_No => "Is set to use local settings (Global value is {0})";

        public string Hud_Idle => "Idle";
        public string Hud_NoLimit => "No limit";


        public string Hud_Que => "Que";

        public string Hud_EmptyList => "- Empty list -";

        /// <summary>
        /// 0: current parts, 1: needed number of parts
        /// </summary>
        public string Language_CollectProgress => "{0}/{1}";
        public string Hud_SelectCity => "Select City";
        public string Conscription_Title => "Conscription";
        public string Conscript_WeaponTitle => "Weapon";
        public string Conscript_ArmorTitle => "Weapon";
        public string Conscript_TrainingTitle => "Weapon";
        public string Conscript_SelectBuilding => "Select barracks";

        public string Conscript_WeaponDamage = "Weapon damage: {0}";
        public string Conscript_ArmorHealth = "Armor health: {0}";
        public string Conscript_TrainingSpeed = "Attack speed: {0}";
        public string Conscript_TrainingTime = "Training time: {0}";

        public string Conscription_Status_CollectingEquipment => "Collecting equipment: {0}";
        public string Conscription_Status_CollectingMen => "Collecting men: {0}";
        public string Conscription_Status_Training => "Training: {0}";

        public string ArmyHud_Food_Reserves_X => "Food reserves: {0}";
        public string ArmyHud_Food_Upkeep_X => "Food upkeep: {0}";
        public string ArmyHud_Food_Costs_X => "Food costs: {0}";

        //public string _Title => "Conscription";

        /// <summary>
        /// A small symbol for buttons containing extra information
        /// </summary>
        public string Info_ButtonIcon => "i";

        public string Info_PerSecond => "Displayed in Resource Per Second.";

        public string Info_MinuteAverage => "The value is an average from the last minute";

        /// <summary>
        /// A small symbol for buttons that will end/close an editor
        /// </summary>
        public string Hud_EndSessionIcon => "X";

        public string TerrainType => "Terrain type";

        public string Hud_EnergyUpkeepX => "Food energy upkeep {0}";

        public string Economy_TaxIncome = "Tax income: {0}";
        public string Economy_ImportCostsForResource = "Import costs for {0}: {1}";
        public string Economy_BlackMarketCostsForResource = "Black market costs for {0}: {1}";
        public string Economy_GuardUpkeep = "Guard upkeep: {0}";

        public string Economy_LocalCityTrade_Export = "City trade export: {0}";
        public string Economy_LocalCityTrade_Import = "City trade import: {0}";

        public string Economy_ResourceProduction = "{0} production: {1}";
        public string Economy_ResourceSpending = "{0} spending: {1}";

        public string Economy_TaxDescription = "Tax is {0} gold per worker";

        public string Economy_SoldResources = "Sold resources (iron/gold): {0}";

        public string UnitType_Cities => "Cities";
        public string UnitType_Armies => "Armies";

        public string CityCulture_LargeFamilies => "Large families";
        public string CityCulture_FertileGround => "Fertile grounds";
        public string CityCulture_Archers => "Skilled archers";
        public string CityCulture_Warriors => "Warriors";
        public string CityCulture_AnimalBreeder => "Animal breeders";
        public string CityCulture_Miners => "Miner";
        public string CityCulture_Woodcutters => "Lumbermen";
        public string CityCulture_Builders => "Builders";
        public string CityCulture_CrabMentality => "Crab mentality"; //ingen vill bli expert
        public string CityCulture_DeepWell => "Deep well";
        public string CityCulture_Networker => "Networker";

        public string CityCulture_LargeFamilies_Description => "Increased child birth";
        public string CityCulture_FertileGround_Description => "Crops give more";
        public string CityCulture_Archers_Description => "Produces skilled archers";
        public string CityCulture_Warriors_Description => "Produces skilled melee fighters";
        public string CityCulture_AnimalBreeder_Description => "Animals give more resources";
        public string CityCulture_Miners_Description => "Mines more ore";
        public string CityCulture_Woodcutters_Description => "Trees give more wood";
        public string CityCulture_Builders_Description => "Fast at building";
        public string CityCulture_CrabMentality_Description => "Work cost less energy. Cannot produce high skill soldiers."; //ingen vill bli expert
        public string CityCulture_DeepWell_Description => "Water replenish faster";
        public string CityCulture_Networker_Description => "Efficient postal service";

    }
}
