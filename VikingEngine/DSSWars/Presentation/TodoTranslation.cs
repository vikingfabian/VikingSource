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
        public string StockPile_ItemsAreNotLost => "Items will not be destroyed if you exceed the stockpile!";

        public string SlaughterResult_PerAnimal => "Slaughter output, per animal";

        public string Settings_Mode_QuickBoss => "Quick boss";
        public string Settings_Mode_QuickBoss_Description => "Prepare for a few hours, then meet a final boss";

        public string QuickBoss_TimeOption => "Boss time (hours)";

    }
}