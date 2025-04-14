using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    class TodoTranslation
    {
        //## Ref.langOpt
        public string ReversedSterio = "Reversed stereo";
        public string Option_Low = "Low";
        public string Option_Medium = "Medium";
        public string Option_High = "High";
        public string MouseSettings_Title => "Mouse input";
        public string KeyboardSettings_Title => "Key mapping";
        public string MouseButtonAction_None => "No action";
        public string MouseButtonAction_Select => "Select";
        public string MouseButtonAction_Pan => "Pan";
        public string MouseButtonAction_PanAndOrder => "Pan and Order";
        public string MouseButtonAction_Order => "Order";
        public string MouseButtonAction_Cancel => "Cancel";

        public string MouseButton_Left => "Left Mouse";
        public string MouseButton_Right => "Right Mouse";
        public string MouseButton_Middle => "Middle Mouse";
        public string MouseButton_X1 => "X1 Button Mouse";
        public string MouseButton_X2 => "X2 Button Mouse";


        //##End Ref.langOpt

        public string Tutorial_OpenGuardSubTab => "Open a barracks and select category: {0}";
        public string Tutorial_GuardToWall => "Move a guard to a wall";
        public string Demo_MissionObjective_Title => "Mission Objective";
        public string Demo_MissionObjective_Description => "Defend against the attack from south";
        public string Demo_Complete_Title => "Demo complete";
        public string Demo_TimesUp_Title => "Times' up!";
        public string Demo_EndInOneMinuteDescription => "The demo will end in one minute";

        public string ArmyOption_NewArmy => "New army";
        public string ProfileEditor_AltMain => "Alternative main";
        public string Automation_CheckBoxTitle => "Automated";

        public string ArmyStructure_ColumnWidth = "Army column width";
        public string ArmyStructure_ArmyPlacement = "Placement in army";
        public string ArmyStructure_Row_Front = "Front";
        public string ArmyStructure_Row_Body = "Body";
        public string ArmyStructure_Row_Second = "Second";
        public string ArmyStructure_Row_Behind = "Behind";

        public string Diplomacy_RelationType_Enemies => "Enemies";

        public string EventMessage_EnemyAlliance_Title => "Fear of Domination";
        public string EventMessage_EnemyAlliance => "The nations, fearing your growing power, unite in an alliance against you.";
        
        public string Settings_CentralGold => "Central gold";
        public string Settings_CentralGold_Description => "On: all your gold is in a shared pool for instant use. Off: gold is physical and needs to be transported.";

        public string InputActionName_StopStart => "Stop/Start";
        public string InputActionName_ToggleHudDetail => "Toggle HUD Detail";
        public string InputActionName_NextCity => "Next City";
        public string InputActionName_NextArmy => "Next Army";
        public string InputActionName_NextBattle => "Next Battle";
        public string InputActionName_Build => "Build";
        public string InputActionName_Copy => "Copy";
        public string InputActionName_Paste => "Paste";
        public string InputActionName_Menu => "Menu";
        public string InputActionName_FlagDesign_ToggleColor_Prev => "Previous Color";
        public string InputActionName_FlagDesign_ToggleColor_Next => "Next Color";
        public string InputActionName_FlagDesign_PaintBucket => "Paint Bucket";
        public string InputActionName_Controller_FlagDesign_Colorpicker => "Color Picker";
        public string InputActionName_ControllerFocus => "Focus";
        public string InputActionName_ControllerCancel => "Cancel";
        public string InputActionName_ControllerMessageClick => "Message Click";
        public string InputActionName_ControllerSelect => "Select";
        public string InputActionName_WASD_UP => "Up";
        public string InputActionName_WASD_DOWN => "Down";
        public string InputActionName_WASD_LEFT => "Left";
        public string InputActionName_WASD_RIGHT => "Right";
        public string InputActionName_CameraTiltLeft => "Camera Tilt Left";
        public string InputActionName_CameraTiltRight => "Camera Tilt Right";
        public string InputActionName_CameraTiltUp => "Camera Tilt Up";
        public string InputActionName_ZoomInKey => "Zoom In";
        public string InputActionName_ZoomOutKey => "Zoom Out";

        public string Settings_Title_Monitor => "Monitor options";
        public string Settings_Title_Graphics => "Graphic options";
        public string Settings_Title_Input => "Input";
        public string Settings_Title_Gameplay => "Gameplay options";
        public string Settings_PanOnZoom => "Pan on zoom";
        public string Settings_ScrollSensitivity_Game => "Scroll sensitivity: game";
        public string Settings_ScrollSensitivity_Menu => "Scroll sensitivity: menu";
        public string Settings_Blood => "Blood";

        public string Settings_MasterVolume => "Master Volume";
        public string Settings_AmbienceVolume => "Ambience Volume";
        public string Settings_BattleMelody => "Battle Melody";

        public string Settings_ModelLight => "Model light effect";
        public string Settings_Particles => "Particle effects";
        public string Settings_MapLoadSpeed => "Map loading speed";
        public string Lobby_Category_Options => "Options";
        public string Lobby_Category_Editor => "Editor";
        public string Lobby_Category_ExtraModes => "Extra modes";

        public string Lobby_Editor_MapEditor => "Map editor";
        public string Lobby_Editor_VoxelEditor => "Voxel editor";

        public string Lobby_Mode_BattleLab => "Battle lab";
        public string Lobby_Mode_BattleLab_Description => "Pit any soldiers against eachother";
        public string Lobby_Mode_Commander => "Play Commander";
        public string Lobby_Mode_Commander_Description => "A small tactical board game";
        public string Lobby_MusicPlayList => "Music playlist";

        public string Lobby_GameSetup => "Game steup";
        public string Lobby_PlayerSetup => "Player steup";
        public string LobbyDemoMode_Demo => "Demo";
        public string LobbyDemoMode_ShortTutorial => "Quick Tutorial";
        public string LobbyDemoMode_LongTutorial => "Extended Tutorial";

        /// <summary>
        /// Says wishlist on, followed by the STEAM logo
        /// </summary>
        public string LobbyDemoMode_WishlistOn => "Wishlist on";


        public string BattleLab_StartHere => "Start battle here";
        public string BattleLab_Start => "Start battle";
        public string BattleLab_Attacker => "Attacker";



        public string MapGenerator_Name = "Map editor - generate";

        public string MapType_CustomMap = "Custom Map";
        public string MapType_GenerateNewMap = "Generate a new map";
        public string MapGenerator_GenerateAction = "Generate";
        public string MapGenerator_Terrain_CustomSize = "Custom size";
        public string MapGenerator_Terrain_StartAs = "Start as";
        public string MapGenerator_Terrain_ClearPass = "Run Clear Pass";
        public string MapGenerator_Terrain_BuildPass = "Run Build Pass";
        public string MapGenerator_Terrain_DigPass = "Run Dig Pass";
        public string MapGenerator_Terrain_BuildDigLoops = "Build-Dig loop count";
        public string MapGenerator_Terrain_BuildStrokes = "Build strokes count";
        public string MapGenerator_Terrain_BuildStrokes_Description = "Measured in paint strokes per 100 tiles";
        public string MapGenerator_Terrain_DigStrokes => "Dig strokes count";
        public string MapGenerator_Terrain_CleanUp_Option => "Cleanup of single tiles";
        public string MapGenerator_Terrain_CleanUpPass => "Run cleanup Pass";



        public string Economy_ServicemenUpkeep => "Servicemen upkeep: {0}";
        public string Economy_ServicemenUpkeep_Description => "Upkeep is {0} gold per serviceman";
        public string Economy_GuardUpkeep_Description => "Upkeep is {0} gold per guard";

        public string EndScreen_TimeHasEndedTitle => "Time's up";

        public  string Hud_AdvancedSettings => "Advanced settings";
        public string Hud_Vector_X => "X";
        public string Hud_Vector_Y => "Y";
        public string Hud_Cancel => "Cancel";
        public string Hud_Delete => "Delete";
        public string Hud_Next => "Next";
        public string Hud_None => "None";
        public string Hud_Apply => "Apply";
        public string Hud_AllCities => "All cities";
        public string Hud_Time_Hours => "{0} hours";
        public string Hud_AddX => "Add {0}";
        public string Hud_Both => "Both";
        public string Hud_Direction => "Direction";
        public string MusicIsBroken => "Music is currently broken";


        /// <summary>
        /// 0: object collection type name, 1: number of objects
        /// </summary>
        public string Hud_ObjectsAndCount => "{0}, count: {1}";

        public string Hud_EffectDoesNotStack => "This effect does not stack";

        public string Work_SmeltX => "Smelt {0}";

        public string Info_TotalFoodProduction => "Total food production";
        public string Info_TotalFoodSpending => "Total food spending";

        public string Info_FooodAndDeliveryLocation => "By default, workers go to the city hall to eat or drop off items";
        public string GameMenu_UseSpeedX => "{0} speed option";

        public string Delivery_SendChunk => "Items per Delivery";
        public string Delivery_SpeedBonus => "Speed bonus: {0}%";

        public string Delivery_AutoResourceDescription => "Delivers items that has reached the stockpile limit, to cities in need.";

        public string Conscript_Soldiers_ArmyType => "Army men";
        public string Conscript_Soldiers_ArmyType_Description => "Recruit soldiers to an adjacent army";
        public string Conscript_Soldiers_GuardType => "City guard";
        public string Conscript_Soldiers_GuardType_Description => "Guards are used to fortify walls";

        public string Defence_Title => "Defence";
        public string Defence_GuardPost => "Guard post";

        public string Defence_WallDescription_Movement => "Hinders enemy movement.";
        public string Defence_WallDescription_GuardPost => "Guard can be posted here.";
        public string Defence_AutoAssign => "Auto assign";
        public string Defence_AutoAssign_Description => "New guards will move to this post";
        public string Conscript_SplashDamage => "Splash damage";
        public string Conscript_HighSplashDamage => "High splash damage";

        public string Conscript_Training_Champion => "Champion";
        public string Conscript_Training_Legendary => "Legendary";


        public string Experience_Title => "Experience";
        public string Experience_TopExperience => "Top experience levels";

        public string Experience_TimeReductionDescription => "Work time is reduced by {0}% per level";

        public string ExperienceType_Farm => "Farmer";
        public string ExperienceType_AnimalCare => "Animal care";
        public string ExperienceType_HouseBuilding => "House builder";
        public string ExperienceType_WoodWork => "Wood worker";
        public string ExperienceType_StoneCutter => "Stone cutter";
        public string ExperienceType_Mining => "Miner";
        public string ExperienceType_Transport => "Transport";
        public string ExperienceType_Cook => "Cook";
        public string ExperienceType_Fletcher => "Fletcher";
        public string ExperienceType_RefineOre => "Smelter";
        public string ExperienceType_Casting => "Casting";
        public string ExperienceType_CraftMetal => "Smith";
        public string ExperienceType_CraftArmor => "Armorer";
        public string ExperienceType_CraftWeapon => "Weapon smith";
        public string ExperienceType_CraftFuel => "Collier";
        public string ExperienceType_Chemist => "Chemist";

        public string ExperienceLevel_1 => "Beginner";
        public string ExperienceLevel_2 => "Practitioner";
        public string ExperienceLevel_3 => "Expert";
        public string ExperienceLevel_4 => "Master";
        public string ExperienceLevel_5 => "Legendary";

        public string ExperenceOrDistancePrio_Title => "Worker selection";
        public string ExperenceOrDistancePrio_Description => "Idle workers will be selected to work either by distance or experience";


        public string Technology_Description = "Each city has a technology tree. Each technology will unlock buildings and items.";
        public string Experience_Description = "Wokers will gain experience and improve";
        

        public string Technology_Title = "Technology";
        public string Technology_ShareField = "Sharing technology field";

        public string Technology_GainByNeigborRelation => "For each neighbor city with the technology. And your relation is {0}: {1}";
        public string Technology_ForEachMaster=> "When a {0} reaches an experience level of {1}, in the technology field: {2}";
        public string Technology_CitySpread => "Your cities will share technology when adjacent: {0}";
        public string Technology_CityCapture => "Most technolgies are destroyed when a city is captured in battle";

        public string Technology_AdvancedBuildings = "Advanced buildings";
        public string Technology_AdvancedFarming = "Advanced farming";
        public string Technology_AdvancedCasting = "Advanced casting";

        public string Help_Title = "Help";
        public string Help_Work_Title = "Work doesn't start";
        public string Help_Work_Resources = "Buildings need available resources";
        public string Help_Work_Skill = "The worker need correct skill level (or higher)";
        public string Help_Work_Stockpile = "Collecting resources will be blocked by a full stockpile";
        public string Help_Work_Priority = "The work may have low or zero priority";


        public string Help_Soldiers_Title = "Produce soldiers";
        public string Help_Soldiers_PlaceBuildingX = "Place building: {0}";
        public string Help_Soldiers_Workers = "Available workers to recruit from";
        public string Help_Soldiers_Weapon = "A weapon to each soldier";
        public string Help_Soldiers_StartX = "Start: {0}";


        public string Hud_SelectHistory => "Select history";

        public string Hud_PointsPerMinute => "{0} points per minute";
        public string Hud_PercentValueCost => "The service costs {0}% of the value";

        public string Hud_Mixed => "Mixed";
        public string Hud_Distance => "Distance";

        public string Hud_Unlock => "Unlock";
        public string Hud_category => "Category";

        public string Resource_TypeName_Wagon2Wheel=> "Small wagon";
        public string Resource_TypeName_Wagon4Wheel=> "Large wagon";
        public string Resource_TypeName_Tin => "Tin";
        public string Resource_TypeName_TinOre=> "Tin ore";

        public string Resource_TypeName_Copper => "Copper";
        public string Resource_TypeName_CopperOre=> "Copper ore";
        public string Resource_TypeName_SilverOre => "Silver ore";
        public string Resource_TypeName_Silver => "Silver";

        /// <summary>
        /// Mithril is a fantasy metal
        /// </summary>
        public string Resource_TypeName_RawMithril=> "Raw mithril";
        public string Resource_TypeName_Mithril => "Mithril";

        public string Resource_TypeName_BronzeSword=> "Bronze sword";
        public string Resource_TypeName_ShortSword=> "Shortsword";
        public string Resource_TypeName_LongSword => "Longsword";
        public string Resource_TypeName_HandSpear => "Hand spear";
        public string Resource_TypeName_Warhammer=> "Warhammer";
        public string Resource_TypeName_MithrilSword=> "Mithril sword";
        public string Resource_TypeName_SlingShot=> "Slingshot";
        public string Resource_TypeName_ThrowingSpear=> "Javelin";
        public string Resource_TypeName_Crossbow=> "Crossbow";
        public string Resource_TypeName_MithrilBow=> "Mithril bow";

        public string Resource_TypeName_CoolingFluid=> "Cooling fluid";
        public string Resource_TypeName_Palisade => "Palisade";
        public string Resource_TypeName_Toolkit => "Tool kit";

        public string Resource_TypeName_Sulfur=> "Sulfur";
        public string Resource_TypeName_LeadOre=> "Lead ore";
        public string Resource_TypeName_Lead=> "Lead";
        public string Resource_TypeName_Bronze => "Bronze";
        public string Resource_TypeName_BloomIron => "Bloomery iron";
        public string Resource_TypeName_Steel => "Steel";
        public string Resource_TypeName_CastIron=> "Cast iron";

        public string Resource_TypeName_BlackPowder=> "Black powder";
        public string Resource_TypeName_GunPowder=> "Gunpowder";
        public string Resource_TypeName_LedBullet=> "Bullet";

        public string Resource_TypeName_HandCannon => "Hand cannon";
        public string Resource_TypeName_HandCulverin=> "Hand culverin";
        public string Resource_TypeName_Rifle=> "Rifle";
        public string Resource_TypeName_Blunderbus=> "Blunderbus";

        public string Resource_TypeName_Manuballista=> "Manuballista";
        public string Resource_TypeName_Catapult => "Catapult";
        public string Resource_TypeName_BatteringRam => "BatteringRam";
        public string Resource_TypeName_SiegeCannonBronze=> "Basilic";
        public string Resource_TypeName_ManCannonBronze=> "Bombard";
        public string Resource_TypeName_SiegeCannonIron=> "Haubitz";
        public string Resource_TypeName_ManCannonIron=> "Cannon";

        public string Resource_TypeName_PaddedArmor=> "Padded armor";
        public string Resource_TypeName_HeavyPaddedArmor=> "Heavy padded armor";

        public string Resource_TypeName_IronArmor=> "Mail armor";
        public string Resource_TypeName_HeavyIronArmor=> "Heavy mail armor";
        
        public string Resource_TypeName_BronzeArmor=> "Bronze armor";
        
        public string Resource_TypeName_LightPlateArmor=> "Plate armor";
        public string Resource_TypeName_FullPlateArmor => "Full plate armor";
        public string Resource_TypeName_MithrilArmor => "Mithril armor";
        public string Resource_TypeName_Coin => "Coin";

        public string UnitType_Warhammer => "Hammer knight";
        public string UnitType_MithrilKnight => "Immortal knight";
        public string UnitType_MithrilArcher=> "Immortal archer";
        public string UnitType_SpearAndShield => "Lineman";

        public string UnitType_CollectionOfSoldiers => "Soldier Bundle";
        public string UnitType_CollectionOfArmies => "Army Bundle";

        /// <summary>
        /// The id tag will be a unique number
        /// </summary>
        public string UnitId => "(id {0})";

        public string BuildHud_AreaAffectTitle => "Area affect";
        public string BuildHud_BonusRadius => "Bonus radius: {0}";

        public string BuildHud_BuildTime => "Build time";
        public string SchoolHud_ToLevel => "To level";
        public string SchoolHud_TimeDescription => "Time assumes zero experience; it decreases with experience.";
        public string SchoolHud_SelectSchool => "Select school";
        public string Upgrade_Order => "Upgrade order";

        public string Building_ListDescription => "A list of all buildings in this category";

        public string BuildingType_IsUpgraded => "{0} - upgraded";
        public string BuildingType_WoodCutter => "Lumber mill";
        public string BuildingType_Workshop_Description => "Improves work in the area";

        public string BuildingType_WoodCutter_AreaAffect => "Gain {0}% more wood from trees";

        public string BuildingType_StoneCutter_AreaAffect => "Gain {0}% more stone";

        public string BuildingType_StoneCutter => "Stone quarry";

        public string BuildingType_Embassy => "Embassy";
        public string BuildingType_Embassy_Description => "For diplomatic relations";

        public string BuildingType_SoldierBarracks => "Soldier barracks";
        public string BuildingType_ArcherBarracks => "Archer barracks";
        public string BuildingType_WarmashineBarracks => "Warmashine barracks";
        public string BuildingType_GunBarracks => "Gun barracks";
        public string BuildingType_CannonBarracks => "Cannon barracks";
        public string BuildingType_KnightsBarracks => "Knights barracks";

        public string BuildingType_WaterResovoir => "Water resovoir";
        public string BuildingType_WaterResovoir_Description => "Increase water storage";

        public string BuildingType_SmeltingFurnace => "Smelting furnace";
        public string BuildingType_SmeltingFurnace_Description => "Purify ore to metal";

        public string BuildingType_Foundry => "Foundry";
        public string BuildingType_Foundry_Description => "Metal casting station";

        public string BuildingType_Armory => "Armory";
        public string BuildingType_Armory_Description => "Armor crafting station";
        public string BuildingType_Chemist => "Chemist";
        public string BuildingType_Chemist_Description => "Chemicals crafting station";
        public string BuildingType_CoinMaker => "Coin minter";
        public string BuildingType_CoinMaker_Description => "Turn metals to money";
        public string BuildingType_Gunmaker => "Gunmaker";
        public string BuildingType_Gunmaker_Description => "Crafting station for guns and cannons";

        public string BuildingType_School_Tab => "School";
        public string BuildingType_School => "Masters guild";
        public string BuildingType_School_Description => "Increase the skill level of workers";

        public string BuildingType_GoldDelivery => "Gold courier";
        public string BuildingType_Bank_Description => "Gold management";

        public string DecorType_CobbleStones => "Cobble stones";
        public string DecorType_Square => "City square";

        public string DecorType_Garden =>"Garden";
        public string DecorType_Flag => "Flag";
        public string DecorType_Banner => "Banner";

        public string BuildingType_DirtRoad => "Dirt road";
        public string BuildingType_Palisade => "Palisade Fort";

        public string ResourceType_ServiceMen => "Servicemen";
        public string BuildingType_ServiceHouse => "Service house";
        public string BuildingType_ServiceHouse_DescriptionAddX => "Add {0} servicemen";

        public string BuildingType_GuardOffice => "Guards office";
        public string BuildingType_GuardOffice_DescriptionAddX => "Increase guard limit by {0}";

        public string BuildingType_DirtWall => "Dirt wall";
        public string BuildingType_DirtTower => "Dirt tower";
        public string BuildingType_WoodWall => "Wood wall";
        public string BuildingType_WoodTower => "Wood tower";
        public string BuildingType_StoneWall => "Stone wall";
        public string BuildingType_StoneTower => "Stone tower";
        public string BuildingType_StoneGate => "Stone gate";
        public string BuildingType_StoneHouse => "Stone gate";


        /// <summary>
        /// When listing slight variations, like "Lamp A" and "Lamp B"
        /// </summary>
        public string VariantType_A => "{0} A";
        public string VariantType_B => "{0} B";
        public string VariantType_C => "{0} C";
        public string VariantType_D => "{0} D";
        public string VariantType_E => "{0} E";
        public string VariantType_F => "{0} F";
        public string VariantType_G => "{0} G";
        public string VariantType_H => "{0} H";

        public string BuildingToolShape_Free => "Pen";
        public string BuildingToolShape_Area => "Rectangle";
        public string BuildingToolShape_Line => "Line";
        public string BuildingToolShape_LShape => "L Shape";


        public string CityHall_Upgrade => "Upgrade city hall";

        /// <summary>
        /// A cap on how many workers the city can have
        /// </summary>
        public string CityHall_MaxSupportedWorkers = "Max supported workers: {0}";

        public string CityHall_Size_Small = "Village";
        public string CityHall_Size_Medium = "Town";
        public string CityHall_Size_Large = "Capital";

        public string GuardHousingCount = "Guard office housing";
        public string ServicemenCount = "Servicemen: {0}";


        public string Work_MiningResource = "Mining {0}";

        public string MenuTab_Progress => "Progress";

        public string Automation_AutomateCity => "Automate city";
        public string Automation_AutomationFocus => "Automation focus";
        public string Automation_AutomationFocus_Grow => "Grow";
        public string Automation_AutomationFocus_Export => "Export";
        public string Automation_AutomationFocus_War => "War";

        public string CityCulture_Smelters_Description => "Improved ore smelting";
        public string CityCulture_Smelters => "Smelters";

        public string CityCulture_Apprentices_Description => "New workers will gain experience from active workers";
        public string CityCulture_Apprentices => "Apprentices";

        public string CityCulture_BronzeCasters_Description => "Improved production of bronze and bronze items";
        public string CityCulture_BronzeCasters => "Bronze casters";

        public string CityCulture_x_Description => "";
        public string CityCulture_x => "";

    }

}