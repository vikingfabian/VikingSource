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
        /// How much of a resource that will be used, "5 gold". There will be a "cost" title above the text. 0: Resource, 1: cost
        /// </summary>
        public string Hud_Purchase_ResourceCost => "{1} {0}";
        
        /// <summary>
        /// Will end diplomatic relations like alliance
        /// </summary>
        public string Diplomacy_EndRelations => "End relations";
        public string DisplayMode => "Display mode";
        public string DisplayMode_Windowed => "Windowed";
        public string DisplayMode_BorderlessFullscreen => "Borderless fullscreen";

        public string GameSettings_RenderedMouseCursor => "Rendered cursor";
        public string GameSettings_MuteControllerDisconnect => "Mute disconnect messages";

        public string Delivery_MaxDistance => "Delivery max distance: {0}";
        //public string Error_SoundInitFailure => "Sound initialization failed";
        public string Tutorial_WillTakeAWhile => "This will take a while, come back later.";
        
        //##SPRING - settings##
        public string Settings_ControllerVibration = "Controller vibration";
        /// <summary>
        /// 0: name of building
        /// </summary>
        public string Tutorial_WaitFor => "Wait for {0} to complete";


        public string GameOverResults => "Game history log";

        //##SPRING##

        public string UnitType_UnclaimedLand => "Unclaimed land";
        public string UnitType_Settler => "Settler";
        public string UnitType_Settler_Description => "Found a new city";
        public string Resource_ConsumedProduced => "Consumed/Produced";
        public string InputActionName_PlaceTarget => "Place target";

        public string FactionStartSize => "Faction start size";
        public string FactionStartSize_Full => "Full";
        public string FactionStartSize_OneCity => "One city";
        public string FactionStartSize_Settler => "One settler";


        //## Mounted update ##

        public string Resource_TypeName_ConservedFood => "conserved food";
        public string Resource_TypeName_Clay => "clay";
        public string Resource_TypeName_StorageBox => "storage box";
        public string Resource_TypeName_Meat => "meat";
        public string Resource_TypeName_Salt => "salt";
        public string Resource_TypeName_Wagon=> "wagon";
        public string Resource_TypeName_WagonClosed => "closed wagon";
        public string Resource_TypeName_WagonIron => "Iron wagon";
        public string Resource_TypeName_WagonSteel => "Steel wagon";
        public string Resource_TypeName_Shield => "shield";
        public string Resource_TypeName_BucklerShield => "buckler shield";
        public string Resource_TypeName_RoundShield => "round shield";
        public string Resource_TypeName_HeaterShield => "heater shield";
        public string Resource_TypeName_TowerShield => "tower shield";

        public string Resource_TypeName_Mount => "mount";
        public string Resource_TypeName_Animal => "animal";

        public string Resource_TypeName_Oxen => "oxen";
        public string Resource_TypeName_KineOxen => "kine oxen";

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

        public string BuildingType_Butcher => "";
        public string BuildingType_Pottery => "";
        public string BuildingType_MeatGrill => "";
        public string BuildingType_Smoker => "";
        public string BuildingType_Dryer => "";
        public string BuildingType_ => "";
        public string BuildingType_ => "";


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


    //Not in use
    //public string Settings_Render3dScale_Title => "3D render scale";
    //public string Settings_Render3dScale_UpX => "Upscale {0}X";
    //public string Settings_Render3dScale_DownX => "Downscale {0}X";
}