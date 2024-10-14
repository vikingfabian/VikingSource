using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    abstract class AbsTodoTranslation
    {
        public abstract string CityMenu_SalePricesTitle { get; }
        public abstract string Blueprint_Title { get; }
        public abstract string Resource_Tab_Overview { get; }
        public abstract string Resource_Tab_Stockpile { get; }

        public abstract string Resource { get; }
        public abstract string Resource_StockPile_Info { get; }
        public abstract string Resource_TypeName_Water { get; }
        public abstract string Resource_TypeName_Wood { get; }
        public abstract string Resource_TypeName_Fuel { get; }
        public abstract string Resource_TypeName_Stone { get; }
        public abstract string Resource_TypeName_RawFood { get; }
        public abstract string Resource_TypeName_Food { get; }
        public abstract string Resource_TypeName_Beer { get; }
        public abstract string Resource_TypeName_Wheat { get; }
        public abstract string Resource_TypeName_Linen { get; }
        public abstract string Resource_TypeName_SkinAndLinen { get; }
        public abstract string Resource_TypeName_IronOre { get; }
        public abstract string Resource_TypeName_GoldOre { get; }
        public abstract string Resource_TypeName_Iron { get; }

        public abstract string Resource_TypeName_SharpStick { get; }
        public abstract string Resource_TypeName_Sword { get; }
        public abstract string Resource_TypeName_KnightsLance { get; }
        public abstract string Resource_TypeName_TwoHandSword { get; }
        public abstract string Resource_TypeName_Bow { get; }

        public abstract string Resource_TypeName_LightArmor { get; }
        public abstract string Resource_TypeName_MediumArmor { get; }
        public abstract string Resource_TypeName_HeavyArmor { get; }

        public abstract string ResourceType_Children { get; }

        public abstract string BuildingType_DefaultName { get; }
        public abstract string BuildingType_WorkerHut { get; }
        public abstract string BuildingType_Tavern { get; }
        public abstract string BuildingType_Brewery { get; }
        public abstract string BuildingType_Postal { get; }
        public abstract string BuildingType_Recruitment { get; }
        public abstract string BuildingType_Barracks { get; }
        public abstract string BuildingType_PigPen { get; }
        public abstract string BuildingType_HenPen { get; }
        public abstract string BuildingType_WorkBench { get; }
        public abstract string BuildingType_Carpenter { get; }
        public abstract string BuildingType_CoalPit { get; }
        public abstract string DecorType_Statue { get; }
        public abstract string DecorType_Pavement { get; }
        public abstract string BuildingType_Smith { get; }
        public abstract string BuildingType_Cook { get; }
        public abstract string BuildingType_Storage { get; }

        public abstract string BuildingType_ResourceFarm { get; }

        public abstract string BuildingType_WorkerHut_DescriptionLimitX { get; }
        public abstract string BuildingType_Tavern_Description { get; }
        public abstract string BuildingType_Tavern_Brewery { get; }
        public abstract string BuildingType_Postal_Description { get; }
        public abstract string BuildingType_Recruitment_Description { get; }
        public abstract string BuildingType_Barracks_Description { get; }
        public abstract string BuildingType_PigPen_Description { get; }
        public abstract string BuildingType_HenPen_Description { get; }
        public abstract string BuildingType_Decor_Description { get; }
        public abstract string BuildingType_Farm_Description { get; }

        public abstract string BuildingType_Cook_Description { get; }
        public abstract string BuildingType_Bench_Description { get; }

        public abstract string BuildingType_Smith_Description { get; }
        public abstract string BuildingType_Carpenter_Description { get; }

        public abstract string BuildingType_Nobelhouse_Description { get; }
        public abstract string BuildingType_CoalPit_Description { get; }
        public abstract string BuildingType_Storage_Description { get; }

        public abstract string MenuTab_Info { get; }
        public abstract string MenuTab_Work { get; }
        public abstract string MenuTab_Recruit { get; }
        public abstract string MenuTab_Resources { get; }
        public abstract string MenuTab_Trade { get; }
        public abstract string MenuTab_Build { get; }
        public abstract string MenuTab_Economy { get; }
        public abstract string MenuTab_Delivery { get; }

        public abstract string MenuTab_Build_Description { get; }
        public abstract string MenuTab_BlackMarket_Description { get; }
        public abstract string MenuTab_Resources_Description { get; }
        public abstract string MenuTab_Work_Description { get; }
        public abstract string MenuTab_Automation_Description { get; }

        public abstract string BuildHud_OutsideCity { get; }
        public abstract string BuildHud_OutsideFaction { get; }

        public abstract string BuildHud_OccupiedTile { get; }

        public abstract string Build_PlaceBuilding { get; }
        public abstract string Build_DestroyBuilding { get; }
        public abstract string Build_ClearTerrain { get; }

        public abstract string Build_ClearOrders { get; }
        public abstract string Build_Order { get; }
        public abstract string Build_OrderQue { get; }
        public abstract string Build_AutoPlace { get; }

        public abstract string Work_OrderPrioTitle { get; }
        public abstract string Work_OrderPrioDescription { get; }

        public abstract string Work_OrderPrio_No { get; }
        public abstract string Work_OrderPrio_Min { get; }
        public abstract string Work_OrderPrio_Max { get; }

        public abstract string Work_Move { get; }

        public abstract string Work_GatherXResource { get; }
        public abstract string Work_CraftX { get; }
        public abstract string Work_Farming { get; }
        public abstract string Work_Mining { get; }
        public abstract string Work_Trading { get; }

        public abstract string Work_AutoBuild { get; }

        public abstract string WorkerHud_WorkType { get; }
        public abstract string WorkerHud_Carry { get; }
        public abstract string WorkerHud_Energy { get; }
        public abstract string WorkerStatus_Exit { get; }
        public abstract string WorkerStatus_Eat { get; }
        public abstract string WorkerStatus_Till { get; }
        public abstract string WorkerStatus_Plant { get; }
        public abstract string WorkerStatus_Gather { get; }
        public abstract string WorkerStatus_PickUpResource { get; }
        public abstract string WorkerStatus_DropOff { get; }
        public abstract string WorkerStatus_BuildX { get; }
        public abstract string WorkerStatus_TrossReturnToArmy { get; }

        public abstract string Hud_ToggleFollowFaction { get; }
        public abstract string Hud_FollowFaction_Yes { get; }
        public abstract string Hud_FollowFaction_No { get; }

        public abstract string Hud_Idle { get; }
        public abstract string Hud_NoLimit { get; }

        public abstract string Hud_None { get; }
        public abstract string Hud_Queue { get; }

        public abstract string Hud_EmptyList { get; }

        public abstract string Hud_RequirementOr { get; }

        public abstract string Hud_BlackMarket { get; }

        public abstract string Language_CollectProgress { get; }
        public abstract string Hud_SelectCity { get; }
        public abstract string Conscription_Title { get; }
        public abstract string Conscript_WeaponTitle { get; }
        public abstract string Conscript_ArmorTitle { get; }
        public abstract string Conscript_TrainingTitle { get; }

        public abstract string Conscript_SpecializationTitle { get; }
        public abstract string Conscript_SpecializationDescription { get; }
        public abstract string Conscript_SelectBuilding { get; }

        public abstract string Conscript_WeaponDamage { get; }
        public abstract string Conscript_ArmorHealth { get; }
        public abstract string Conscript_TrainingSpeed { get; }
        public abstract string Conscript_TrainingTime { get; }

        public abstract string Conscript_Training_Minimal { get; }
        public abstract string Conscript_Training_Basic { get; }
        public abstract string Conscript_Training_Skillful { get; }
        public abstract string Conscript_Training_Professional { get; }

        public abstract string Conscript_Specialization_Field { get; }
        public abstract string Conscript_Specialization_Sea { get; }
        public abstract string Conscript_Specialization_Siege { get; }
        public abstract string Conscript_Specialization_Traditional { get; }
        public abstract string Conscript_Specialization_AntiCavalry { get; }

        public abstract string Conscription_Status_CollectingEquipment { get; }
        public abstract string Conscription_Status_CollectingMen { get; }
        public abstract string Conscription_Status_Training { get; }

        public abstract string ArmyHud_Food_Reserves_X { get; }
        public abstract string ArmyHud_Food_Upkeep_X { get; }
        public abstract string ArmyHud_Food_Costs_X { get; }

        public abstract string Deliver_WillSendXInfo { get; }
        public abstract string Delivery_ListTitle { get; }
        public abstract string Delivery_DistanceX { get; }
        public abstract string Delivery_DeliveryTimeX { get; }
        public abstract string Delivery_SenderMinimumCap { get; }
        public abstract string Delivery_RecieverMaximumCap { get; }
        public abstract string Delivery_ItemsReady { get; }
        public abstract string Delivery_RecieverReady { get; }
        public abstract string Hud_ThisCity { get; }
        public abstract string Hud_RecieveingCity { get; }

        public abstract string Info_ButtonIcon { get; }

        public abstract string Info_PerSecond { get; }

        public abstract string Info_MinuteAverage { get; }

        public abstract string Message_CityOutOfFood_Title { get; }
        public abstract string Message_CityOutOfFood_Text { get; }

        public abstract string Hud_EndSessionIcon { get; }

        public abstract string TerrainType { get; }

        public abstract string Hud_EnergyUpkeepX { get; }

        public abstract string Hud_EnergyAmount { get; }

        public abstract string Hud_CopySetup { get; }
        public abstract string Hud_Paste { get; }

        public abstract string Hud_Available { get; }

        public abstract string WorkForce_ChildBirthRequirements { get; }
        public abstract string WorkForce_AvailableHomes { get; }
        public abstract string WorkForce_Peace { get; }
        public abstract string WorkForce_ChildToManTime { get; }

        public abstract string Economy_TaxIncome { get; }
        public abstract string Economy_ImportCostsForResource { get; }
        public abstract string Economy_BlackMarketCostsForResource { get; }
        public abstract string Economy_GuardUpkeep { get; }

        public abstract string Economy_LocalCityTrade_Export { get; }
        public abstract string Economy_LocalCityTrade_Import { get; }

        public abstract string Economy_ResourceProduction { get; }
        public abstract string Economy_ResourceSpending { get; }

        public abstract string Economy_TaxDescription { get; }

        public abstract string Economy_SoldResources { get; }

        public abstract string UnitType_Cities { get; }
        public abstract string UnitType_Armies { get; }
        public abstract string UnitType_Worker { get; }

        public abstract string UnitType_FootKnight { get; }
        public abstract string UnitType_CavalryKnight { get; }

        public abstract string CityCulture_LargeFamilies { get; }
        public abstract string CityCulture_FertileGround { get; }
        public abstract string CityCulture_Archers { get; }
        public abstract string CityCulture_Warriors { get; }
        public abstract string CityCulture_AnimalBreeder { get; }
        public abstract string CityCulture_Miners { get; }
        public abstract string CityCulture_Woodcutters { get; }
        public abstract string CityCulture_Builders { get; }
        public abstract string CityCulture_CrabMentality { get; }
        public abstract string CityCulture_DeepWell { get; }
        public abstract string CityCulture_Networker { get; }
        public abstract string CityCulture_PitMasters { get; }

        public abstract string CityCulture_CultureIsX { get; }
        public abstract string CityCulture_LargeFamilies_Description { get; }
        public abstract string CityCulture_FertileGround_Description { get; }
        public abstract string CityCulture_Archers_Description { get; }
        public abstract string CityCulture_Warriors_Description { get; }
        public abstract string CityCulture_AnimalBreeder_Description { get; }
        public abstract string CityCulture_Miners_Description { get; }
        public abstract string CityCulture_Woodcutters_Description { get; }
        public abstract string CityCulture_Builders_Description { get; }
        public abstract string CityCulture_CrabMentality_Description { get; }
        public abstract string CityCulture_DeepWell_Description { get; }
        public abstract string CityCulture_Networker_Description { get; }
        public abstract string CityCulture_PitMasters_Description { get; }

        public abstract string CityOption_AutoBuild_Work { get; }
        public abstract string CityOption_AutoBuild_Farm { get; }

        public abstract string Hud_PurchaseTitle_Resources { get; }
        public abstract string Hud_PurchaseTitle_CurrentlyOwn { get; }

        public abstract string Tutorial_EndTutorial { get; }
        public abstract string Tutorial_MissionX { get; }
        public abstract string Tutorial_CollectXAmountOfY { get; }
        public abstract string Tutorial_SelectTabX { get; }
        public abstract string Tutorial_IncreasePriorityOnX { get; }
        public abstract string Tutorial_PlaceBuildOrder { get; }
        public abstract string Tutorial_ZoomInput { get; }

        public abstract string Tutorial_SelectACity { get; }
        public abstract string Tutorial_ZoomInWorkers { get; }
        public abstract string Tutorial_CreateSoldiers { get; }
        public abstract string Tutorial_ZoomOutOverview { get; }
        public abstract string Tutorial_ZoomOutDiplomacy { get; }
        public abstract string Tutorial_ImproveRelations { get; }
        public abstract string Tutorial_MissionComplete_Title { get; }
        public abstract string Tutorial_MissionComplete_Unlocks { get; }
    }
    class EnglishTodo : AbsTodoTranslation
    {
        public override string CityMenu_SalePricesTitle => "Sale prices";
        public override string Blueprint_Title => "Blueprint";
        public override string Resource_Tab_Overview => "Overview";
        public override string Resource_Tab_Stockpile => "Stockpile";

        public override string Resource => "Resource";
        public override string Resource_StockPile_Info => "Set a goal amount for storage of resources; this will inform the workers when to work on another resource.";
        public override string Resource_TypeName_Water => "water";
        public override string Resource_TypeName_Wood => "wood";
        public override string Resource_TypeName_Fuel => "fuel";
        public override string Resource_TypeName_Stone => "stone";
        public override string Resource_TypeName_RawFood => "raw food";
        public override string Resource_TypeName_Food => "food";
        public override string Resource_TypeName_Beer => "beer";
        public override string Resource_TypeName_Wheat => "wheat";
        public override string Resource_TypeName_Linen => "linen";
        public override string Resource_TypeName_SkinAndLinen => "skin and linen";
        public override string Resource_TypeName_IronOre => "iron ore";
        public override string Resource_TypeName_GoldOre => "gold ore";
        public override string Resource_TypeName_Iron => "iron";

        public override string Resource_TypeName_SharpStick => "Sharp stick";
        public override string Resource_TypeName_Sword => "Sword";
        public override string Resource_TypeName_KnightsLance => "Knight's lance";
        public override string Resource_TypeName_TwoHandSword => "Zweihänder";
        public override string Resource_TypeName_Bow => "Bow";

        public override string Resource_TypeName_LightArmor => "Light armor";
        public override string Resource_TypeName_MediumArmor => "Medium armor";
        public override string Resource_TypeName_HeavyArmor => "Heavy armor";

        public override string ResourceType_Children => "Children";

        public override string BuildingType_DefaultName => "Building";
        public override string BuildingType_WorkerHut => "Worker hut";
        public override string BuildingType_Tavern => "Tavern";
        public override string BuildingType_Brewery => "Brewery";
        public override string BuildingType_Postal => "Postal service";
        public override string BuildingType_Recruitment => "Recruitment center";
        public override string BuildingType_Barracks => "Barracks";
        public override string BuildingType_PigPen => "Pig pen";
        public override string BuildingType_HenPen => "Hen pen";
        public override string BuildingType_WorkBench => "Work bench";
        public override string BuildingType_Carpenter => "Carpenter";
        public override string BuildingType_CoalPit => "Charcoal pit";
        public override string DecorType_Statue => "Statue";
        public override string DecorType_Pavement => "Pavement";
        public override string BuildingType_Smith => "Smith";
        public override string BuildingType_Cook => "Cook";
        public override string BuildingType_Storage => "Storehouse";

        public override string BuildingType_ResourceFarm => "{0} farm";

        public override string BuildingType_WorkerHut_DescriptionLimitX => "Expands worker limit with {0}";
        public override string BuildingType_Tavern_Description => "Workers may eat here";
        public override string BuildingType_Tavern_Brewery => "Beer production";
        public override string BuildingType_Postal_Description => "Send resources to other cities";
        public override string BuildingType_Recruitment_Description => "Send men to other cities";
        public override string BuildingType_Barracks_Description => "Uses men and equipment to recruit soldiers";
        public override string BuildingType_PigPen_Description => "Produces pigs, which give food and skin";
        public override string BuildingType_HenPen_Description => "Produces hens and eggs, which give food";
        public override string BuildingType_Decor_Description => "Decoration";
        public override string BuildingType_Farm_Description => "Grow a resource";

        public override string BuildingType_Cook_Description => "Food crafting station";
        public override string BuildingType_Bench_Description => "Item crafting station";

        public override string BuildingType_Smith_Description => "Metal crafting station";
        public override string BuildingType_Carpenter_Description => "Wood crafting station";

        public override string BuildingType_Nobelhouse_Description => "Home for knights and diplomats";
        public override string BuildingType_CoalPit_Description => "Efficient fuel production";
        public override string BuildingType_Storage_Description => "Dropoff point for resources";

        public override string MenuTab_Info => "Info";
        public override string MenuTab_Work => "Work";
        public override string MenuTab_Recruit => "Recruit";
        public override string MenuTab_Resources => "Resources";
        public override string MenuTab_Trade => "Trade";
        public override string MenuTab_Build => "Build";
        public override string MenuTab_Economy => "Economy";
        public override string MenuTab_Delivery => "Delivery";

        public override string MenuTab_Build_Description => "Place buildings in your city";
        public override string MenuTab_BlackMarket_Description => "Place buildings in your city";
        public override string MenuTab_Resources_Description => "Place buildings in your city";
        public override string MenuTab_Work_Description => "Place buildings in your city";
        public override string MenuTab_Automation_Description => "Place buildings in your city";

        public override string BuildHud_OutsideCity => "Outside city region";
        public override string BuildHud_OutsideFaction => "Outside your borders!";

        public override string BuildHud_OccupiedTile => "Occupied tile";

        public override string Build_PlaceBuilding => "Building";
        public override string Build_DestroyBuilding => "Destroy";
        public override string Build_ClearTerrain => "Clear terrain";

        public override string Build_ClearOrders => "Clear build orders";
        public override string Build_Order => "Build order";
        public override string Build_OrderQue => "Build order que: {0}";
        public override string Build_AutoPlace => "Auto place";

        public override string Work_OrderPrioTitle => "Work priority";
        public override string Work_OrderPrioDescription => "Priority goes from 1 (low) to {0} (high)";

        public override string Work_OrderPrio_No => "No priority. Will not be worked on.";
        public override string Work_OrderPrio_Min => "Minimum priority.";
        public override string Work_OrderPrio_Max => "Maximum priority.";

        public override string Work_Move => "Move items";

        public override string Work_GatherXResource => "Gather {0}";
        public override string Work_CraftX => "Craft {0}";
        public override string Work_Farming => "Farming";
        public override string Work_Mining => "Mining";
        public override string Work_Trading => "Tradeing";

        public override string Work_AutoBuild => "Auto build and expand";

        public override string WorkerHud_WorkType => "Work status: {0}";
        public override string WorkerHud_Carry => "Carry: {0} {1}";
        public override string WorkerHud_Energy => "Energy: {0}";
        public override string WorkerStatus_Exit => "Leave workforce";
        public override string WorkerStatus_Eat => "Eat";
        public override string WorkerStatus_Till => "Till";
        public override string WorkerStatus_Plant => "Plant";
        public override string WorkerStatus_Gather => "Gather";
        public override string WorkerStatus_PickUpResource => "Pick up resource";
        public override string WorkerStatus_DropOff => "Drop off";
        public override string WorkerStatus_BuildX => "Build {0}";
        public override string WorkerStatus_TrossReturnToArmy => "Return to army";

        public override string Hud_ToggleFollowFaction => "Toggle follow faction settings";
        public override string Hud_FollowFaction_Yes => "Is set to use faction global settings";
        public override string Hud_FollowFaction_No => "Is set to use local settings (Global value is {0})";

        public override string Hud_Idle => "Idle";
        public override string Hud_NoLimit => "No limit";

        public override string Hud_None => "None";
        public override string Hud_Queue => "Queue";

        public override string Hud_EmptyList => "- Empty list -";

        public override string Hud_RequirementOr => "- or -";

        public override string Hud_BlackMarket => "Black market";

        public override string Language_CollectProgress => "{0} / {1}";
        public override string Hud_SelectCity => "Select City";
        public override string Conscription_Title => "Conscription";
        public override string Conscript_WeaponTitle => "Weapon";
        public override string Conscript_ArmorTitle => "Armor";
        public override string Conscript_TrainingTitle => "Training";

        public override string Conscript_SpecializationTitle => "Specialization";
        public override string Conscript_SpecializationDescription => "Will increase attack in one area, and reduce all others, by {0}";
        public override string Conscript_SelectBuilding => "Select barracks";

        public override string Conscript_WeaponDamage => "Weapon damage: {0}";
        public override string Conscript_ArmorHealth => "Armor health: {0}";
        public override string Conscript_TrainingSpeed => "Attack speed: {0}";
        public override string Conscript_TrainingTime => "Training time: {0}";

        public override string Conscript_Training_Minimal => "Minimal";
        public override string Conscript_Training_Basic => "Basic";
        public override string Conscript_Training_Skillful => "Skillful";
        public override string Conscript_Training_Professional => "Professional";

        public override string Conscript_Specialization_Field => "Open field";
        public override string Conscript_Specialization_Sea => "Ship";
        public override string Conscript_Specialization_Siege => "Siege";
        public override string Conscript_Specialization_Traditional => "Traditional";
        public override string Conscript_Specialization_AntiCavalry => "Anti cavalry";

        public override string Conscription_Status_CollectingEquipment => "Collecting equipment: {0}";
        public override string Conscription_Status_CollectingMen => "Collecting men: {0}";
        public override string Conscription_Status_Training => "Training: {0}";

        public override string ArmyHud_Food_Reserves_X => "Food reserves: {0}";
        public override string ArmyHud_Food_Upkeep_X => "Food upkeep: {0}";
        public override string ArmyHud_Food_Costs_X => "Food costs: {0}";

        public override string Deliver_WillSendXInfo => "Will send {0} at a time";
        public override string Delivery_ListTitle => "Select delivery service";
        public override string Delivery_DistanceX => "Distance: {0}";
        public override string Delivery_DeliveryTimeX => "Delivery time: {0}";
        public override string Delivery_SenderMinimumCap => "Sender minimum cap";
        public override string Delivery_RecieverMaximumCap => "Receiver maximum cap";
        public override string Delivery_ItemsReady => "Items ready";
        public override string Delivery_RecieverReady => "Receiver ready";
        public override string Hud_ThisCity => "This city";
        public override string Hud_RecieveingCity => "Receiving city";

        public override string Info_ButtonIcon => "i";

        public override string Info_PerSecond => "Displayed in Resource Per Second.";

        public override string Info_MinuteAverage => "The value is an average from the last minute";

        public override string Message_CityOutOfFood_Title => "Out of food";
        public override string Message_CityOutOfFood_Text => "Expensive food will be purchased from the black market. Workers will starve when your money runs out.";

        public override string Hud_EndSessionIcon => "X";

        public override string TerrainType => "Terrain type";

        public override string Hud_EnergyUpkeepX => "Food energy upkeep {0}";

        public override string Hud_EnergyAmount => "{0} energy (seconds of work)";

        public override string Hud_CopySetup => "Copy setup";
        public override string Hud_Paste => "Paste";

        public override string Hud_Available => "Available";

        public override string WorkForce_ChildBirthRequirements => "Child birth requirements:";
        public override string WorkForce_AvailableHomes => "Available homes: {0}";
        public override string WorkForce_Peace => "Peace";
        public override string WorkForce_ChildToManTime => "Grown up age: {0} minutes";

        public override string Economy_TaxIncome => "Tax income: {0}";
        public override string Economy_ImportCostsForResource => "Import costs for {0}: {1}";
        public override string Economy_BlackMarketCostsForResource => "Black market costs for {0}: {1}";
        public override string Economy_GuardUpkeep => "Guard upkeep: {0}";

        public override string Economy_LocalCityTrade_Export => "City trade export: {0}";
        public override string Economy_LocalCityTrade_Import => "City trade import: {0}";

        public override string Economy_ResourceProduction => "{0} production: {1}";
        public override string Economy_ResourceSpending => "{0} spending: {1}";

        public override string Economy_TaxDescription => "Tax is {0} gold per worker";

        public override string Economy_SoldResources => "Sold resources (gold ore): {0}";

        public override string UnitType_Cities => "Cities";
        public override string UnitType_Armies => "Armies";
        public override string UnitType_Worker => "Worker";

        public override string UnitType_FootKnight => "Longsword knight";
        public override string UnitType_CavalryKnight => "Cavalry knight";

        public override string CityCulture_LargeFamilies => "Large families";
        public override string CityCulture_FertileGround => "Fertile grounds";
        public override string CityCulture_Archers => "Skilled archers";
        public override string CityCulture_Warriors => "Warriors";
        public override string CityCulture_AnimalBreeder => "Animal breeders";
        public override string CityCulture_Miners => "Miners";
        public override string CityCulture_Woodcutters => "Lumbermen";
        public override string CityCulture_Builders => "Builders";
        public override string CityCulture_CrabMentality => "Crab mentality";
        public override string CityCulture_DeepWell => "Deep well";
        public override string CityCulture_Networker => "Networker";
        public override string CityCulture_PitMasters => "Pit masters";

        public override string CityCulture_CultureIsX => "Culture: {0}";
        public override string CityCulture_LargeFamilies_Description => "Increased child birth";
        public override string CityCulture_FertileGround_Description => "Crops give more";
        public override string CityCulture_Archers_Description => "Produces skilled archers";
        public override string CityCulture_Warriors_Description => "Produces skilled melee fighters";
        public override string CityCulture_AnimalBreeder_Description => "Animals give more resources";
        public override string CityCulture_Miners_Description => "Mines more ore";
        public override string CityCulture_Woodcutters_Description => "Trees give more wood";
        public override string CityCulture_Builders_Description => "Fast at building";
        public override string CityCulture_CrabMentality_Description => "Work cost less energy. Cannot produce high-skill soldiers.";
        public override string CityCulture_DeepWell_Description => "Water replenish faster";
        public override string CityCulture_Networker_Description => "Efficient postal service";
        public override string CityCulture_PitMasters_Description => "Higher fuel production";

        public override string CityOption_AutoBuild_Work => "Auto expand workforce";
        public override string CityOption_AutoBuild_Farm => "Auto expand farms";

        public override string Hud_PurchaseTitle_Resources => "Buy resources";
        public override string Hud_PurchaseTitle_CurrentlyOwn => "You own";

        public override string Tutorial_EndTutorial => "End tutorial";
        public override string Tutorial_MissionX => "Mission {0}";
        public override string Tutorial_CollectXAmountOfY => "Collect {0} {1}";
        public override string Tutorial_SelectTabX => "Select tab: {0}";
        public override string Tutorial_IncreasePriorityOnX => "Increase the priority on: {0}";
        public override string Tutorial_PlaceBuildOrder => "Place build order: {0}";
        public override string Tutorial_ZoomInput => "Zoom";

        public override string Tutorial_SelectACity => "Select a city";
        public override string Tutorial_ZoomInWorkers => "Zoom in to see the workers";
        public override string Tutorial_CreateSoldiers => "Create two soldier units with this equipment: {0}. {1}.";
        public override string Tutorial_ZoomOutOverview => "Zoom out, to map overview";
        public override string Tutorial_ZoomOutDiplomacy => "Zoom out, to diplomacy view";
        public override string Tutorial_ImproveRelations => "Improve your relations with a neighbor faction";
        public override string Tutorial_MissionComplete_Title => "Mission complete!";
        public override string Tutorial_MissionComplete_Unlocks => "New controls have been unlocked";
    }


    class TodoTranslation
    {
        //public string CityMenu_SalePricesTitle => "Sale prices";

        //public string Blueprint_Title = "Blueprint";
        //public string Resource_Tab_Overview = "Overview";
        //public string Resource_Tab_Stockpile = "Stockpile";

        //public string Resource => "Resource";
        //public string Resource_StockPile_Info => "Set a goal amount for storage of resources; this will inform the workers when to work on another resource.";
        //public string Resource_TypeName_Water => "water";
        //public string Resource_TypeName_Wood => "wood";
        //public string Resource_TypeName_Fuel => "fuel";
        //public string Resource_TypeName_Stone => "stone";
        //public string Resource_TypeName_RawFood => "raw food";
        //public string Resource_TypeName_Food => "food";
        //public string Resource_TypeName_Beer => "beer";
        //public string Resource_TypeName_Wheat => "wheat";
        //public string Resource_TypeName_Linen => "linen";
        //public string Resource_TypeName_SkinAndLinen => "skin and linen";
        //public string Resource_TypeName_IronOre => "iron ore";
        //public string Resource_TypeName_GoldOre => "gold ore";
        //public string Resource_TypeName_Iron => "iron";


        //public string Resource_TypeName_SharpStick => "Sharp stick";
        //public string Resource_TypeName_Sword => "Sword";
        //public string Resource_TypeName_KnightsLance => "Knight's lance";        
        //public string Resource_TypeName_TwoHandSword => "Zweihänder";
        //public string Resource_TypeName_Bow => "Bow";

        //public string Resource_TypeName_LightArmor => "Light armor";
        //public string Resource_TypeName_MediumArmor => "Medium armor";
        //public string Resource_TypeName_HeavyArmor => "Heavy armor";

        //public string ResourceType_Children => "Children";

        //public string BuildingType_DefaultName => "Building";
        //public string BuildingType_WorkerHut => "Worker hut";
        //public string BuildingType_Tavern => "Tavern";
        //public string BuildingType_Brewery => "Brewery";
        //public string BuildingType_Postal => "Postal service";
        //public string BuildingType_Recruitment => "Recruitment center";
        //public string BuildingType_Barracks => "Barracks";
        //public string BuildingType_PigPen => "Pig pen";
        //public string BuildingType_HenPen => "Hen pen";
        //public string BuildingType_WorkBench => "Work bench";
        //public string BuildingType_Carpenter => "Carpenter";
        //public string BuildingType_CoalPit => "Charcoal pit";
        //public string DecorType_Statue => "Statue";
        //public string DecorType_Pavement => "Pavement";
        //public string BuildingType_Smith => "Smith";
        //public string BuildingType_Cook => "Cook";
        //public string BuildingType_Storage => "Storehouse";

        //public string BuildingType_ResourceFarm => "{0} farm";

        //public string BuildingType_WorkerHut_DescriptionLimitX => "Expands worker limit with {0}";
        //public string BuildingType_Tavern_Description => "Workers may eat here";
        //public string BuildingType_Tavern_Brewery => "Beer production";
        //public string BuildingType_Postal_Description => "Send resources to other cities";
        //public string BuildingType_Recruitment_Description => "Send men to other cities";
        //public string BuildingType_Barracks_Description => "Uses men and equipment to recruit soldiers";
        //public string BuildingType_PigPen_Description => "Produces pigs, which give food and skin";
        //public string BuildingType_HenPen_Description => "Produces hens and eggs, which give food";
        //public string BuildingType_Decor_Description => "Decoration";
        //public string BuildingType_Farm_Description => "Grow a resource";

        //public string BuildingType_Cook_Description => "Food crafting station";
        //public string BuildingType_Bench_Description => "Item crafting station";

        //public string BuildingType_Smith_Description => "Metal crafting station";
        //public string BuildingType_Carpenter_Description => "Wood crafting station";

        //public string BuildingType_Nobelhouse_Description => "Home for knights and diplomats";
        //public string BuildingType_CoalPit_Description => "Efficient fuel production";
        //public string BuildingType_Storage_Description => "Dropoff point for resources";

        //public string MenuTab_Info => "Info";
        //public string MenuTab_Work => "Work";
        //public string MenuTab_Recruit => "Recruit";
        //public string MenuTab_Resources => "Resources";
        //public string MenuTab_Trade => "Trade";
        //public string MenuTab_Build => "Build";
        //public string MenuTab_Economy => "Economy";
        //public string MenuTab_Delivery => "Delivery";

        //public string MenuTab_Build_Description => "Place buildings in your city";
        //public string MenuTab_BlackMarket_Description => "Place buildings in your city";
        //public string MenuTab_Resources_Description => "Place buildings in your city";
        //public string MenuTab_Work_Description => "Place buildings in your city";
        //public string MenuTab_Automation_Description => "Place buildings in your city";

        //public string BuildHud_OutsideCity => "Outside city region";
        //public string BuildHud_OutsideFaction => "Outside your borders!";

        ///// <summary>
        ///// Info when the square is covoered with a building or blocking terrain
        ///// </summary>
        //public string BuildHud_OccupiedTile => "Occupied tile";

        //public string Build_PlaceBuilding => "Building";
        //public string Build_DestroyBuilding => "Destroy";
        //public string Build_ClearTerrain => "Clear terrain";

        //public string Build_ClearOrders => "Clear build orders";
        //public string Build_Order => "Build order";
        //public string Build_OrderQue => "Build order que: {0}";
        //public string Build_AutoPlace => "Auto place";

        //public string Work_OrderPrioTitle => "Work priority";
        //public string Work_OrderPrioDescription => "Priority goes from 1 (low) to {0} (high)";

        //public string Work_OrderPrio_No => "No priority. Will not be worked on.";
        //public string Work_OrderPrio_Min => "Minimum priority.";
        //public string Work_OrderPrio_Max => "Maximum priority.";

        //public string Work_Move => "Move items";

        //public string Work_GatherXResource => "Gather {0}";
        //public string Work_CraftX => "Craft {0}";
        //public string Work_Farming => "Farming";
        //public string Work_Mining => "Mining";
        //public string Work_Trading => "Tradeing";

        //public string Work_AutoBuild => "Auto build and expand";
        ////public string Work_ExpandFarms => "Expand farms";


        //public string WorkerHud_WorkType = "Work status: {0}";
        //public string WorkerHud_Carry = "Carry: {0} {1}";
        //public string WorkerHud_Energy = "Energy: {0}";
        //public string WorkerStatus_Exit => "Leave workforce";
        //public string WorkerStatus_Eat => "Eat";
        //public string WorkerStatus_Till => "Till";
        //public string WorkerStatus_Plant => "Plant";
        //public string WorkerStatus_Gather => "Gather";
        //public string WorkerStatus_PickUpResource => "Pick up resource";
        //public string WorkerStatus_DropOff => "Drop off";
        //public string WorkerStatus_BuildX => "Build {0}";
        //public string WorkerStatus_TrossReturnToArmy => "Return to army";

        //public string Hud_ToggleFollowFaction => "Toggle follow faction settings";
        //public string Hud_FollowFaction_Yes => "Is set to use faction global settings";
        //public string Hud_FollowFaction_No => "Is set to use local settings (Global value is {0})";

        //public string Hud_Idle => "Idle";
        //public string Hud_NoLimit => "No limit";

        //public string Hud_None => "None";
        //public string Hud_Queue => "Queue";

        //public string Hud_EmptyList => "- Empty list -";

        ///// <summary>
        ///// To view optional requirement in a list
        ///// </summary>
        //public string Hud_RequirementOr => "- or -";

        //public string Hud_BlackMarket => "Black market";

        ///// <summary>
        ///// 0: current parts, 1: needed number of parts
        ///// </summary>
        //public string Language_CollectProgress => "{0} / {1}";
        //public string Hud_SelectCity => "Select City";
        //public string Conscription_Title => "Conscription";
        //public string Conscript_WeaponTitle => "Weapon";
        //public string Conscript_ArmorTitle => "Armor";
        //public string Conscript_TrainingTitle => "Training";

        //public string Conscript_SpecializationTitle => "Specialization";
        //public string Conscript_SpecializationDescription => "Will increase attack in one area, and reduce all others, by {0}";
        //public string Conscript_SelectBuilding => "Select barracks";

        //public string Conscript_WeaponDamage = "Weapon damage: {0}";
        //public string Conscript_ArmorHealth = "Armor health: {0}";
        //public string Conscript_TrainingSpeed = "Attack speed: {0}";
        //public string Conscript_TrainingTime = "Training time: {0}";

        //public string Conscript_Training_Minimal => "Minimal";
        //public string Conscript_Training_Basic => "Basic";
        //public string Conscript_Training_Skillful => "Skillful";
        //public string Conscript_Training_Professional => "Professional";

        //public string Conscript_Specialization_Field => "Open field";
        //public string Conscript_Specialization_Sea => "Ship";
        //public string Conscript_Specialization_Siege => "Siege";
        //public string Conscript_Specialization_Traditional => "Traditional";
        //public string Conscript_Specialization_AntiCavalry => "Anti cavalry";


        //public string Conscription_Status_CollectingEquipment => "Collecting equipment: {0}";
        //public string Conscription_Status_CollectingMen => "Collecting men: {0}";
        //public string Conscription_Status_Training => "Training: {0}";

        //public string ArmyHud_Food_Reserves_X => "Food reserves: {0}";
        //public string ArmyHud_Food_Upkeep_X => "Food upkeep: {0}";
        //public string ArmyHud_Food_Costs_X => "Food costs: {0}";


        //public string Deliver_WillSendXInfo => "Will send {0} at a time";
        //public string Delivery_ListTitle => "Select delivery service";
        //public string Delivery_DistanceX => "Distance: {0}";
        //public string Delivery_DeliveryTimeX => "Delivery time: {0}";
        //public string Delivery_SenderMinimumCap => "Sender minimum cap";
        //public string Delivery_RecieverMaximumCap => "Receiver maximum cap";
        //public string Delivery_ItemsReady => "Items ready";
        //public string Delivery_RecieverReady => "Receiver ready";
        //public string Hud_ThisCity => "This city";
        //public string Hud_RecieveingCity => "Receiving city";

        ////public string _Title => "Conscription";

        ///// <summary>
        ///// A small symbol for buttons containing extra information
        ///// </summary>
        //public string Info_ButtonIcon => "i";

        //public string Info_PerSecond => "Displayed in Resource Per Second.";

        //public string Info_MinuteAverage => "The value is an average from the last minute";


        //public string Message_CityOutOfFood_Title => "Out of food";
        //public string Message_CityOutOfFood_Text => "Expensive food will be purchased from the black market. Workers will starve when your money runs out.";

        ///// <summary>
        ///// A small symbol for buttons that will end/close an editor
        ///// </summary>
        //public string Hud_EndSessionIcon => "X";

        //public string TerrainType => "Terrain type";

        //public string Hud_EnergyUpkeepX => "Food energy upkeep {0}";

        //public string Hud_EnergyAmount => "{0} energy (seconds of work)";

        //public string Hud_CopySetup => "Copy setup";
        //public string Hud_Paste => "Paste";

        //public string Hud_Available => "Available";

        //public string WorkForce_ChildBirthRequirements = "Child birth requirements:";
        //public string WorkForce_AvailableHomes = "Available homes: {0}";
        //public string WorkForce_Peace = "Peace";
        //public string WorkForce_ChildToManTime = "Grown up age: {0} minutes";

        //public string Economy_TaxIncome = "Tax income: {0}";
        //public string Economy_ImportCostsForResource = "Import costs for {0}: {1}";
        //public string Economy_BlackMarketCostsForResource = "Black market costs for {0}: {1}";
        //public string Economy_GuardUpkeep = "Guard upkeep: {0}";

        //public string Economy_LocalCityTrade_Export = "City trade export: {0}";
        //public string Economy_LocalCityTrade_Import = "City trade import: {0}";

        //public string Economy_ResourceProduction = "{0} production: {1}";
        //public string Economy_ResourceSpending = "{0} spending: {1}";

        //public string Economy_TaxDescription = "Tax is {0} gold per worker";

        //public string Economy_SoldResources = "Sold resources (gold ore): {0}";

        //public string UnitType_Cities => "Cities";
        //public string UnitType_Armies => "Armies";
        //public string UnitType_Worker => "Worker";

        //public string UnitType_FootKnight => "Longsword knight";
        //public string UnitType_CavalryKnight => "Cavalry knight";

        //public string CityCulture_LargeFamilies => "Large families";
        //public string CityCulture_FertileGround => "Fertile grounds";
        //public string CityCulture_Archers => "Skilled archers";
        //public string CityCulture_Warriors => "Warriors";
        //public string CityCulture_AnimalBreeder => "Animal breeders";
        //public string CityCulture_Miners => "Miners";
        //public string CityCulture_Woodcutters => "Lumbermen";
        //public string CityCulture_Builders => "Builders";
        //public string CityCulture_CrabMentality => "Crab mentality"; //ingen vill bli expert
        //public string CityCulture_DeepWell => "Deep well";
        //public string CityCulture_Networker => "Networker";
        //public string CityCulture_PitMasters => "Pit masters";


        //public string CityCulture_CultureIsX => "Culture: {0}";
        //public string CityCulture_LargeFamilies_Description => "Increased child birth";
        //public string CityCulture_FertileGround_Description => "Crops give more";
        //public string CityCulture_Archers_Description => "Produces skilled archers";
        //public string CityCulture_Warriors_Description => "Produces skilled melee fighters";
        //public string CityCulture_AnimalBreeder_Description => "Animals give more resources";
        //public string CityCulture_Miners_Description => "Mines more ore";
        //public string CityCulture_Woodcutters_Description => "Trees give more wood";
        //public string CityCulture_Builders_Description => "Fast at building";
        //public string CityCulture_CrabMentality_Description => "Work cost less energy. Cannot produce high-skill soldiers."; //ingen vill bli expert
        //public string CityCulture_DeepWell_Description => "Water replenish faster";
        //public string CityCulture_Networker_Description => "Efficient postal service";
        //public string CityCulture_PitMasters_Description => "Higher fuel production";

        //public string CityOption_AutoBuild_Work => "Auto expand workforce";
        //public string CityOption_AutoBuild_Farm => "Auto expand farms";

        ////public string CityOption_AutoBuild_Intelligent => "Intelligent, build only when needed";

        //public string Hud_PurchaseTitle_Resources => "Buy resources";
        //public string Hud_PurchaseTitle_CurrentlyOwn=> "You own";

        //public string Tutorial_EndTutorial => "End tutorial";
        //public string Tutorial_MissionX => "Mission {0}";
        //public string Tutorial_CollectXAmountOfY => "Collect {0} {1}";
        //public string Tutorial_SelectTabX => "Select tab: {0}";
        //public string Tutorial_IncreasePriorityOnX => "Increase the priority on: {0}";
        //public string Tutorial_PlaceBuildOrder => "Place build order: {0}";
        //public string Tutorial_ZoomInput => "Zoom";

        //public string Tutorial_SelectACity => "Select a city";
        //public string Tutorial_ZoomInWorkers => "Zoom in to see the workers";
        //public string Tutorial_CreateSoldiers => "Create two soldier units with this equipment: {0}. {1}.";
        //public string Tutorial_ZoomOutOverview => "Zoom out, to map overview";
        //public string Tutorial_ZoomOutDiplomacy => "Zoom out, to diplomacy view";
        //public string Tutorial_ImproveRelations => "Improve your relations with a neighbor faction";
        //public string Tutorial_MissionComplete_Title => "Mission complete!";
        //public string Tutorial_MissionComplete_Unlocks => "New controls have been unlocked";
    }

}