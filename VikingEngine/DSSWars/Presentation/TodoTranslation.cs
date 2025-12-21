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

    }
}