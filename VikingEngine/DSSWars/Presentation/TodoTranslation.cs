using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.Presentation
{
    class TodoTranslation
    {
        /// <summary>
        /// Will end diplomatic relations like alliance
        /// </summary>
        public string Diplomacy_EndRelations => "End relations";

        /// <summary>
        /// Where a resource is produced or found
        /// </summary>
        public string ItemSource => "Item source";

        public string ItemSource_Terrain => "Terrain";
        public string ItemSource_Farm => "Farm";
        public string ItemSource_CraftStation => "Craft station";
        public string ItemSource_Gathering => "Gathering";

        //## Mounted update ##
     
        public string Economy_AnimalPenUpkeep => "Pen upkeep: {0}";
        public string Work_SlaughterX => "Slaughter {0}";

        public string Resource_TypeName_NobelMen => "nobelmen";
        public string Resource_TypeName_ConservedFood => "conserved food";
        public string Resource_TypeName_Clay => "clay";
        public string Resource_TypeName_Brick => "brick";
        public string Resource_TypeName_Container => "container";
        public string Resource_TypeName_Meat => "meat";
        public string Resource_TypeName_Salt => "salt";
        public string Resource_TypeName_Wagon=> "wagon";
        public string Resource_TypeName_WagonClosed => "closed wagon";
        public string Resource_TypeName_WagonIron => "iron coach";
        public string Resource_TypeName_WagonSteel => "steel coach";
        public string Resource_TypeName_Shield => "shield";
        public string Resource_TypeName_BucklerShield => "buckler shield";
        public string Resource_TypeName_RoundShield => "round shield";
        public string Resource_TypeName_HeaterShield => "heater shield";
        public string Resource_TypeName_TowerShield => "tower shield";

        public string Resource_TypeName_Mount => "mount";

        /// <summary>
        /// 0: armor type
        /// </summary>
        public string Resource_TypeName_MountArmorX => "mount {0}";
        public string Resource_TypeName_Animal => "animal";

        public string Resource_TypeName_Oxen => "oxen";
        public string Resource_TypeName_KineOxen => "kine oxen";

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

        public string BuildingType_ClayPit => "Clay pit";
        public string BuildingType_Butcher => "Butcher";
        public string BuildingType_Pottery => "Pottery";
        
        public string BuildingType_Smoker => "Smoker";
        public string BuildingType_Dryer => "Dryer";
        public string BuildingType_Shieldmaker => "Shield maker";
        public string BuildingType_DryingPan => "Drying pan";

        // --- Storage ---
        public string BuildingType_MaterialStorage => "Material Storage";
        public string BuildingType_FoodStorage => "Food Storage";
        public string BuildingType_WeaponStorage => "Weapon Storage";
        public string BuildingType_ArmorStorage => "Armor Storage";
        public string BuildingType_AnimalStorage => "Animal Storage";

        public string BuildingType_Cesspit => "Cesspit";

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



        public string CityCulture_AnimalBreeder2_Description => "Higher chance of successful breeding";
        
        public string CityCulture_EnhancedProduction => "Enhanced {0} production";

        public string CityCulture_Butchers => "Butchers";

        public string CityCulture_Potters => "Potters";

        public string CityCulture_Wainwright => "Wainwright";

        public string CityCulture_Wheelwright => "Wheelwright";
        public string CityCulture_Wheelwright_Description => "Speed bonus to conscripted carts";

        public string CityCulture_ShieldMaker => "Shield Maker";

        public string CityCulture_Nomads => "Nomads";
        public string CityCulture_Nomads_Description => "Low settler cost";

        public string CityCulture_Coopers => "Coopers";

        public string CityCulture_Salters => "Salters";
    }
}