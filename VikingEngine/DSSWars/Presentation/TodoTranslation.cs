using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.Presentation
{
    class TodoTranslation
    {
        //options
        public string InputSteam = "Steam input";
        public string Input_SimulateMouse = "Simulate mouse";
        public string Input_LockMouseToWindow => "Lock mouse to window";
        public string Input_MouseEdgePush_Title=> "Mouse edge push";
        public string Input_NoControl => "None";
        public string Input_ActiveControl => "Active";
        public string Input_PassiveControl => "Passive";
        public string Setting_MinimapScale => "Mini map scale";


        //regular
        public string Hud_Time_ValuePerMinute => "Value per minute";

        public string Tutorial_SeeThisInThat = "See {0} in {1}";

        public string Conscript_SkillBonus => "Skill bonus";

        /// <summary>
        /// Generelized for any object, like skills, resources and buildings
        /// </summary>
        public string Culture_AffectedItems => "Affected items";
        //## Mounted update ##
        public string Progress_ClosingCores => "Closing CPU cores {0}";
        public string Editor_ExportFrame => "Export current frame";

        public string Economy_AnimalPenUpkeep => "Pen upkeep: {0}";
        public string Work_SlaughterX => "Slaughter {0}";

        public string BuildCategory_Farming => "Farming";
        public string Resource_TypeName_ManType => "man type";
        public string Resource_TypeName_NobelMen => "nobelmen";
        public string Resource_TypeName_ConservedFood => "conserved food";

        public string Resource_ConservedFood_Reserves => "Conserved food reserves";
        public string Resource_TypeName_Clay => "clay";
        public string Resource_TypeName_Brick => "brick";
        public string Resource_TypeName_Container => "container";
        public string Resource_TypeName_Meat => "meat";
        public string Resource_TypeName_Salt => "salt";
        public string Resource_TypeName_Vehicle=> "vehicle";
        public string Resource_TypeName_WagonClosed => "closed wagon";
        public string Resource_TypeName_WagonIron => "iron coach";
        public string Resource_TypeName_WagonSteel => "steel coach";
        public string Resource_TypeName_Shield => "shield";
        public string Resource_TypeName_BucklerShield => "buckler shield";
        public string Resource_TypeName_RoundShield => "round shield";
        public string Resource_TypeName_HeaterShield => "heater shield";
        public string Resource_TypeName_TowerShield => "tower shield";

        public string Resource_TypeName_Mount => "mount";

        public string Resource_TypeName_MountArmorTitle => "mount armor";

        /// <summary>
        /// 0: armor type
        /// </summary>
        public string Resource_TypeName_MountArmorX => "mount {0}";
        public string Resource_TypeName_Animal => "animal";

        //public string Resource_TypeName_WildAnimal => "wild animal";
        
        /// <summary>
        /// Area with wild animals
        /// </summary>
        public string Terrain_XAnimalHabitat => "{0} habitat";

        public string Resource_TypeName_Oxen => "oxen";
        public string Resource_TypeName_KineOxen => "kine oxen";

        /// <summary>
        /// Low tier hen (for breeding)
        /// </summary>
        public string Resource_TypeName_Fowl => "fowl";

        /// <summary>
        /// Low tier pig (for breeding)
        /// </summary>
        public string Resource_TypeName_Boar => "boar";
        public string Resource_TypeName_Pig => "pig";
        public string Resource_TypeName_Hen => "hen";
        public string Resource_TypeName_Dog => "dog";
        public string Resource_TypeName_Hound => "hound";

        public string Resource_TypeName_Pony => "pony";
        public string Resource_TypeName_Horse => "horse";
        public string Resource_TypeName_WarHorse => "war horse";
        public string Resource_TypeName_DraftHorse => "draft horse";

        public string Resource_TypeName_WildPig => "wild pig";
        public string Resource_TypeName_WildHog => "wild hog";
        public string Resource_TypeName_WarHog => "war hog";
        public string Resource_TypeName_StagHog => "stag hog";

        public string Resource_TypeName_Wolf => "wolf";
        public string Resource_TypeName_Warg => "warg";
        public string Resource_TypeName_AlphaWarg => "alpha warg";

        public string Resource_TypeName_WildCat => "wild cat";
        public string Resource_TypeName_Lion => "lion";
        public string Resource_TypeName_WarLion => "war lion";

        public string Resource_TypeName_Elephant => "elephant";
        public string Resource_TypeName_WarElephant => "war elephant";
        public string Resource_TypeName_Oliphant => "oliphant";

        public string NobelHouse_HousingCount => "Will house {0} nobelmen";

        public string BuildingType_GreatHall => "Great hall";
        public string BuildingType_GreatHall_Description => "Unlock advanced conscripting";

        public string BuildingType_ClayPit => "Clay pit";
        public string BuildingType_Butcher => "Butcher";
        public string BuildingType_Butcher_Description => "Turn animals to food and skin";
        public string BuildingType_Pottery => "Pottery";
        public string BuildingType_CraftX_Description => "{0} crafting station";

        public string BuildingType_GatherX_Description => "Gather {0}";

        public string BuildingType_Smoker => "Smoker";
        public string BuildingType_Dryer => "Dryer";
        public string BuildingType_Shieldmaker => "Shield maker";
        public string BuildingType_DryingPan => "Drying pan";

        public string BuildingType_TrapperHut => "Trapper's hut";
        public string BuildingType_TrapperHut_Description => "Allows capture of wild animals";
        
        // --- Storage ---
        public string BuildingType_MaterialStorage => "Material Storage";
        public string BuildingType_FoodStorage => "Food Storage";
        public string BuildingType_WeaponStorage => "Weapon Storage";
        public string BuildingType_ArmorStorage => "Armor Storage";
        public string BuildingType_AnimalStorage => "Animal Storage";

        public string BuildingType_Storage_Description => "Increase max stockpile by {0}";

        public string BuildingType_Cesspit => "Cesspit";
        public string BuildingType_Cesspit_Description => "Destroy resources";

        public string BuildingType_FowlPen => "Fowl Pen";
        public string BuildingType_BoarPen => "Boar Pen";

        // --- Oxen Pens ---
        public string BuildingType_OxenPen => "Oxen Pen";
        public string BuildingType_KineOxenPen => "Kine Oxen Pen";

        // --- Dog Cages ---
        public string BuildingType_DogCage => "Dog Cage";
        public string BuildingType_HoundCage => "Hound Cage";

        // --- Horse Pens ---
        public string BuildingType_PonyPen => "Pony Pen";
        public string BuildingType_HorsePen => "Horse Pen";
        public string BuildingType_WarHorsePen => "War Horse Pen";
        public string BuildingType_DraftHorsePen => "Draft Horse Pen";

        // --- Pig/Hog Pens ---
        public string BuildingType_WildPigPen => "Wild Pig Pen";
        public string BuildingType_WildHogPen => "Wild Hog Pen";
        public string BuildingType_WarHogPen => "War Hog Pen";
        public string BuildingType_StagHogPen => "Stag Hog Pen";

        // --- Wolf Cages ---
        public string BuildingType_WolfCage => "Wolf Cage";
        public string BuildingType_WargCage => "Warg Cage";
        public string BuildingType_AlphaWargCage => "Alpha Warg Cage";

        // --- Cat Cages ---
        public string BuildingType_WildCatCage => "Wild Cat Cage";
        public string BuildingType_LionCage => "Lion Cage";
        public string BuildingType_WarLionCage => "War Lion Cage";

        // --- Elephant Cages ---
        public string BuildingType_ElephantCage => "Elephant Cage";
        public string BuildingType_WarElephantCage => "War Elephant Cage";
        public string BuildingType_OliphantCage => "Oliphant Cage";

        public string BuildingDescription_Animals => "Produces animals for soldier conscript";
        public string Pen_Breeding => "Animal breeding";
        public string Pen_BreedUpChance => "{0}% chance to increase tier";
        public string Pen_BreedDownChance => "{0}% chance to decrease tier";


        public string CityCulture_AnimalBreeder2_Description => "Higher chance of successful breeding";

        public string CityCulture_EnhancedProduction => "Enhanced {0} production";
        public string CityCulture_Production => "{0} production";

        public string CityCulture_Butchers => "Butchers";

        public string CityCulture_Potters => "Potters";

        public string CityCulture_Wainwright => "Wainwright";

        public string CityCulture_Wheelwright => "Wheelwright";
        public string CityCulture_Wheelwright_Description => "Speed bonus to conscripted carts";

        public string CityCulture_ShieldMaker => "Shield Maker";

        
        //public string CityCulture_Nomads_Description => "Low settler cost";

        public string CityCulture_Coopers => "Coopers";

        public string CityCulture_Salters => "Salters";


        public string CityBiome_Title => "Biome";
        public string CityBiome_Description => "Biomes affect access to some resources and buildings";

        public string CityBiome_Fields => "Fields";
        public string CityBiome_Frozen => "Frozen";
        public string CityBiome_Forest => "Forest";
        public string CityBiome_Mountain => "Mountain";
        public string CityBiome_Desolate => "Desolate";
        public string CityBiome_Desert => "Desert";

        public string Bonus_IncreaseSkin => "Increased skin production";
        public string Bonus_FoodStorage => "Larger food storage";

        public string StockPile_LimitTitle => "Limit stockpile";

    }
}