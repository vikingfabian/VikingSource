using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.Presentation
{
    class TodoTranslation
    {
        public string BuildingType_Orchard => "Orchard";
        public string BuildingType_ManorLord => "Manor lord";
        public string BuildingType_ManorLord_Description => "Unlock food processing";
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

        public string CityCulture_Nomad => "Nomad";

        /// <summary>
        /// A generalized display of buffs and boons, example "+100%" or "Doubled"
        /// </summary>
        public string Hud_ChangeFactor => "By change factor: {0}";

        public string Hud_Purchase_LowXCost => "Low {0} cost";

    }
}