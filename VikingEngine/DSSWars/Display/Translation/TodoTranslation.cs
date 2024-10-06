using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    class TodoTranslation
    {
        public string CityMenu_SalePricesTitle => "Sale prices";

        public string Blueprint_Title = "Blueprint";
        //public string /*Blueprint_LetterSymbol*/ = "BP";
        public string Resource_Tab_Overview = "Overview";
        public string Resource_Tab_Stockpile = "Stockpile";
        public string Resource_TypeName_Water => "water";
        public string Resource_TypeName_Wood => "wood";
        public string Resource_TypeName_Fuel => "fuel";
        public string Resource_TypeName_Stone => "stone";
        public string Resource_TypeName_RawFood => "raw food";
        public string Resource_TypeName_Food => "food";
        public string Resource_TypeName_Beer => "beer";
        public string Resource_TypeName_Wheat => "wheat";
        public string Resource_TypeName_Linen => "linen";
        public string Resource_TypeName_SkinAndLinen => "skin and linen";
        public string Resource_TypeName_IronOre => "iron ore";
        public string Resource_TypeName_GoldOre => "gold ore";
        public string Resource_TypeName_Iron => "iron";

        
        public string Resource_TypeName_SharpStick => "Sharp stick";
        public string Resource_TypeName_Sword => "Sword";
        public string Resource_TypeName_KnightsLance => "Knight's lance";        
        public string Resource_TypeName_TwoHandSword => "Zweihänder";
        public string Resource_TypeName_Bow => "Bow";

        public string Resource_TypeName_LightArmor => "Light armor";
        public string Resource_TypeName_MediumArmor => "Medium armor";
        public string Resource_TypeName_HeavyArmor => "Heavy armor";

        public string ResourceType_Children => "Children";

        public string BuildingType_DefaultName => "Building";
        public string BuildingType_WorkerHut => "Worker hut";
        public string BuildingType_Tavern => "Tavern";
        public string BuildingType_Brewery => "Brewery";
        public string BuildingType_Postal => "Postal service";
        public string BuildingType_Recruitment => "Recruiting center";
        public string BuildingType_Barracks => "Barracks";
        public string BuildingType_PigPen => "Pig pen";
        public string BuildingType_HenPen => "Hen pen";
        public string BuildingType_WorkBench => "Work bench";
        public string BuildingType_Carpenter => "Carpenter";
        public string BuildingType_CoalPit => "Charcoal pit";
        public string DecorType_Statue => "Statue";
        public string DecorType_Pavement => "Pavement";
        public string BuildingType_Smith => "Smith";
        public string BuildingType_Cook => "Cook";

        public string BuildingType_ResourceFarm => "{0} farm";

        public string BuildingType_WorkerHut_DescriptionLimitX => "Expands worker limit with {0}";
        public string BuildingType_Tavern_Description => "Workers may eat here";
        public string BuildingType_Tavern_Brewery => "Beer production";
        public string BuildingType_Postal_Description => "Send resources to other cities";
        public string BuildingType_Recruitment_Description => "Send men to other cities";
        public string BuildingType_Barracks_Description => "Uses men and equipment to recruit soldiers";
        public string BuildingType_PigPen_Description => "Produces pigs, which give food and skin";
        public string BuildingType_HenPen_Description => "Produces hens and eggs, which give food";
        public string BuildingType_Decor_Description => "Decoration";
        public string BuildingType_Farm_Description => "Grow a resource";

        public string BuildingType_Cook_Description => "Food crafting station";
        public string BuildingType_Bench_Description => "Item crafting station";
        
        public string BuildingType_Smith_Description => "Metal crafting station";
        public string BuildingType_Carpenter_Description => "Wood crafting station";

        public string BuildingType_Nobelhouse_Description => "Home for knights and diplomats";
        public string BuildingType_CoalPit_Description => "Efficient fuel production";

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

        public string Work_OrderPrio_No => "No priority. Will not be worked on.";
        public string Work_OrderPrio_Min => "Minimum priority.";
        public string Work_OrderPrio_Max => "Maximum priority.";

        public string Work_Move => "Move items";

        public string Work_GatherXResource => "Gather {0}";
        public string Work_CraftX => "Craft {0}";
        public string Work_Farming => "Farming";
        public string Work_Mining => "Mining";
        public string Work_Trading => "Tradeing";

        public string Work_AutoBuild => "Auto build and expand";
        //public string Work_ExpandFarms => "Expand farms";

        public string Hud_ToggleFollowFaction => "Toggle follow faction settings";
        public string Hud_FollowFaction_Yes => "Is set to use faction global settings";
        public string Hud_FollowFaction_No => "Is set to use local settings (Global value is {0})";

        public string Hud_Idle => "Idle";
        public string Hud_NoLimit => "No limit";

        public string Hud_None => "None";
        public string Hud_Que => "Que";

        public string Hud_EmptyList => "- Empty list -";

        public string Hud_BlackMarket => "Black market";

        /// <summary>
        /// 0: current parts, 1: needed number of parts
        /// </summary>
        public string Language_CollectProgress => "{0} / {1}";
        public string Hud_SelectCity => "Select City";
        public string Conscription_Title => "Conscription";
        public string Conscript_WeaponTitle => "Weapon";
        public string Conscript_ArmorTitle => "Armor";
        public string Conscript_TrainingTitle => "Training";

        public string Conscript_SpecializationTitle => "Specialization";
        public string Conscript_SpecializationDescription => "Will increase attack in one area, and reduce all others, by {0}";
        public string Conscript_SelectBuilding => "Select barracks";

        public string Conscript_WeaponDamage = "Weapon damage: {0}";
        public string Conscript_ArmorHealth = "Armor health: {0}";
        public string Conscript_TrainingSpeed = "Attack speed: {0}";
        public string Conscript_TrainingTime = "Training time: {0}";

        public string Conscript_Training_Minimal => "Minimal";
        public string Conscript_Training_Basic => "Basic";
        public string Conscript_Training_Skillful => "Skillful";
        public string Conscript_Training_Professional => "Professional";

        public string Conscript_Specialization_Field => "Open field";
        public string Conscript_Specialization_Sea => "Ship";
        public string Conscript_Specialization_Siege => "Siege";
        public string Conscript_Specialization_Traditional => "Traditional";
        public string Conscript_Specialization_AntiCavalry => "Anti cavalry";


        public string Conscription_Status_CollectingEquipment => "Collecting equipment: {0}";
        public string Conscription_Status_CollectingMen => "Collecting men: {0}";
        public string Conscription_Status_Training => "Training: {0}";

        public string ArmyHud_Food_Reserves_X => "Food reserves: {0}";
        public string ArmyHud_Food_Upkeep_X => "Food upkeep: {0}";
        public string ArmyHud_Food_Costs_X => "Food costs: {0}";


        public string Delivery_ThisCity => "This city";
        public string Delivery_RecieveingCity => "Recieveing city";

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

        public string Hud_EnergyAmount => "{0} energy (seconds of work)";

        public string Hud_CopySetup => "Copy setup";
        public string Hud_Paste => "Paste";

        public string Hud_Available => "Available";


        public string Economy_TaxIncome = "Tax income: {0}";
        public string Economy_ImportCostsForResource = "Import costs for {0}: {1}";
        public string Economy_BlackMarketCostsForResource = "Black market costs for {0}: {1}";
        public string Economy_GuardUpkeep = "Guard upkeep: {0}";

        public string Economy_LocalCityTrade_Export = "City trade export: {0}";
        public string Economy_LocalCityTrade_Import = "City trade import: {0}";

        public string Economy_ResourceProduction = "{0} production: {1}";
        public string Economy_ResourceSpending = "{0} spending: {1}";

        public string Economy_TaxDescription = "Tax is {0} gold per worker";

        public string Economy_SoldResources = "Sold resources (gold ore): {0}";

        public string UnitType_Cities => "Cities";
        public string UnitType_Armies => "Armies";

        public string UnitType_FootKnight => "Longsword knight";
        public string UnitType_CavalryKnight => "Cavalry knight";

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
        public string CityCulture_PitMasters => "Pit masters";

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
        public string CityCulture_PitMasters_Description => "Higher fuel production";

        public string CityOption_AutoBuild_Work => "Auto expand workforce";
        public string CityOption_AutoBuild_Farm => "Auto expand farms";

        //public string CityOption_AutoBuild_Intelligent => "Intelligent, build only when needed";

        public string Hud_PurchaseTitle_Resources => "Buy resources";
        public string Hud_PurchaseTitle_CurrentlyOwn=> "You own";
    }
}
