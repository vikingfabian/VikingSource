using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    class TodoTranslation
    {

        //public string Tutorial_BuildSomething => "Build something that produces {0}";
        //public string Tutorial_BuildCraft => "Build a crafting station for: {0}";
        //public string Tutorial_IncreaseBufferLimit => "Increase buffer limit for: {0}";

        ///// <summary>
        ///// 0: count=> ""; 1: item type
        ///// </summary>
        //public string Tutorial_CollectItemStockpile => "Reach a stockpile of {0} {1}";
        //public string Tutorial_LookAtFoodBlueprint => "Look at the food blueprint";
        //public string Tutorial_CollectFood_Info1 => "The workers will walk to the city hall to eat";
        //public string Tutorial_CollectFood_Info2 => "The army sends tross workers to collect food";
        //public string Tutorial_CollectFood_Info0 => "Want full control of the workers? Set all work priorities to zero=> ""; and then just activate one at a time.";

        //public string EndGameStatistics_DecorsBuilt => "Decorations built: {0}";
        //public string EndGameStatistics_StatuesBuilt => "Statues built: {0}";

        public string Automation_queue_description => "Will keep repeating until the que is empty";

        public string BuildingType_Storehourse_Description => "Workers may drop items here";

        public string Resource_TypeName_Longbow => "longbow";
        public string Resource_TypeName_Rapeseed => "rapeseed";
        public string Resource_TypeName_Hemp => "hemp";


        public string Resource_FoodSafeGuard_Description => "Safe guard. Will maximize the priority of the food production chain, if it falls below {0}.";
        public string Resource_FoodSafeGuard_Active => "Safe guard is active.";

        public string GameMenu_NextSong => "Next song";

        //MAYBE
        public string BuildingType_Bank => "Bank";
        public string BuildingType_Bank_Description => "Send gold to other cities";

        /// <summary>
        /// Title for describing the production cycle of farms
        /// </summary>
        public string BuildHud_PerCycle => "Per cycle";
        public string BuildHud_MayCraft => "May craft";
        public string BuildHud_WorkTime => "Work time: {0}";
        public string BuildHud_GrowTime => "Grow time: {0}";
        public string BuildHud_Produce => "Poduce:";

        public string Delivery_AutoReciever_Description => "Will send to the city with lowest amount of resources";

        public string Hud_On => "On";
        public string Hud_Off => "Off";

        public string Hud_Time_Seconds => "{0} seconds";
        public string Hud_Time_Minutes => "{0} minutes";


        public string Input_Build => "Build";

        public string CityCulture_Stonemason => "Stonemason";
        public string CityCulture_Stonemason_Description => "Improved stone collecting";

        public string CityCulture_Brewmaster => "Brewmaster";
        public string CityCulture_Brewmaster_Description => "Better beer production";

        public string CityCulture_Weavers => "Weavers";
        public string CityCulture_Weavers_Description => "Improved light armor production";

        public string CityCulture_SiegeEngineer => "SiegeEngineer";
        public string CityCulture_SiegeEngineer_Description => "More powerful warmashines";

        public string CityCulture_Armorsmith => "Armorsmith";
        public string CityCulture_Armorsmith_Description => "Improved iron armor production";

        public string CityCulture_Nobelmen => "Nobelmen";
        public string CityCulture_Nobelmen_Description => "More powerful knights";

        public string CityCulture_Seafaring => "Seafaring";
        public string CityCulture_Seafaring_Description => "Soldiers with sea specialzation, have stronger ships";

        public string CityCulture_Backtrader => "Backtrader";
        public string CityCulture_Backtrader_Description => "Cheaper black market";



        /// <summary>
        /// The faction names are fantasy names designed to sound historic, they don't have to be directly translated as long as they keep some of their essense
        /// </summary>
        public string FactionName_Starshield=> "Starshield";
        public string FactionName_Bluepeak=> "Blue Peak";
        public string FactionName_Hoft=> "Hoft";
        public string FactionName_RiverStallion=> "River Stallion";
        public string FactionName_Sivo=> "Sivo";

        public string FactionName_AelthrenConclave=> "Aelthren Conclave";
        public string FactionName_VrakasundEnclave=> "Vrakasund Enclave";
        public string FactionName_Tormürd=> "Tormürd";
        public string FactionName_ElderysFyrd=> "Elderys Fyrd";
        public string FactionName_Hólmgar=> "Hólmgar";
        public string FactionName_RûnothalOrder=> "Rûnothal Order";
        public string FactionName_GrimwardEotain=> "Grimward Eotain";
        public string FactionName_SkaeldraHaim=> "Skaeldra Haim";
        public string FactionName_MordwynnCompact=> "Mordwynn Compact";
        public string FactionName_AethmireSovren=> "Aethmire Sovren";

        public string FactionName_ThurlanKin=> "Thurlan Kin";
        public string FactionName_ValestennOrder=> "Valestenn Order";
        public string FactionName_Mournfold=> "Mournfold";
        public string FactionName_OrentharTribes=> "Orenthar Tribes";
        public string FactionName_SkarnVael=> "Skarn Vael";
        public string FactionName_Glimmerfell=> "Glimmerfell";
        public string FactionName_BleakwaterFold=> "Bleakwater Fold";
        public string FactionName_Oathmaeren=> "Oathmaeren";
        public string FactionName_Elderforge=> "Elderforge";
        public string FactionName_MarhollowCartel=> "Marhollow Cartel";
        
        public string FactionName_TharvaniDominion=> "Tharvani Dominion";
        public string FactionName_KystraAscendancy=> "Kystra Ascendancy";
        public string FactionName_GildenmarkUnion=> "Gildenmark Union";
        public string FactionName_AurecanEmpire=> "Aurecan Empire";
        public string FactionName_BronzeReach=> "Bronze Reach";
        public string FactionName_ElbrethGuild=> "Elbreth Guild";
        public string FactionName_ValosianSenate=> "Valosian Senate";
        public string FactionName_IronmarchCompact=> "Ironmarch Compact";
        public string FactionName_KaranthCollective=> "Karanth Collective";
        public string FactionName_VerdicAlliance=> "Verdic Alliance";

        public string FactionName_OrokhCircles=> "Orokh Circles";
        public string FactionName_TannagHorde=> "Tannag Horde";
        public string FactionName_BraghkRaiders=> "Braghk Raiders";
        public string FactionName_ThurvanniStonekeepers=> "Thurvanni Stonekeepers";
        public string FactionName_KolvrenHunters=> "Kolvren Hunters";
        public string FactionName_JorathBloodbound=> "Jorath Bloodbound";
        public string FactionName_UlrethSkycallers=> "Ulreth Skycallers";
        public string FactionName_GharjaRavagers=> "Ghar'ja Ravagers";
        public string FactionName_RavkanShield=> "Ravkan Shield";
        public string FactionName_FenskaarTidewalkers=> "Fenskaar Tidewalkers";
    }

}