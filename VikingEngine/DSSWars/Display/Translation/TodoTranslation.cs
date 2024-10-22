﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    class TodoTranslation
    {
        
        public string BuildSomething = "Build something that produces {0}";
        public string BuildCraft = "Build a crafting station for: {0}";
        public string IncreaseBufferLimit = "Increase buffer limit for: {0}";

        /// <summary>
        /// 0: count, 1: item type
        /// </summary>
        public string CollectItemStockpile = "Reach a stockpile of {0} {1}";
        public string LookAtFoodBlueprint = "Look at the food blueprint";
        public string CollectFood_Info1 = "The workers will walk to the city hall to eat";
        public string CollectFood_Info2 = "The army sends tross workers to collect food";
        public string CollectFood_Info0 = "Want full control of the workers? Set all work priorities to zero, and then just activate one at a time.";

        public string EndGameStatistics_DecorsBuilt => "Decorations built: {0}";
        public string EndGameStatistics_StatuesBuilt => "Statues built: {0}";
        
        public string BuildingType_Storehourse_Description => "Workers may drop items here";

        public string Resource_TypeName_Longbow => "longbow";


        //MAYBE
        public string BuildingType_Bank => "Bank";
        public string BuildingType_Bank_Description => "Send gold to other cities";

        public string Delivery_AutoReciever_Description => "Will send to the city with lowest amount of resources";

        public string Hud_On => "On";
        public string Hud_Off => "Off";

    }

}