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
        public string Tutorial_SeeThisInThat = "See {0} in {1}";


        /// <summary>
        /// Generelized for any object, like skills, resources and buildings
        /// </summary>
        public string Culture_AffectedItems => "Affected items";
    }
}