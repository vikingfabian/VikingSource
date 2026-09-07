using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class English
    {
        //QoL Update
        public override string GameManual => "Game manual";

        public override string GameManualTitle_Work => "Work";

        public override string[] manual_work => [
            "<h1><img=WarsHammer> Work",
            "<br>All resource collection and production in your city is fully automatic.",
            "<br>The city will create a queue of all available tasks and sort them in order of priority.",
            "<br>As soon as a worker is available, they will pick the top task and carry it out.",

            "<h1>Work doesn't start",
            "<*><img=WarsBluePrint> Buildings and crafting need available resources.",
            "<*><img=WarsUnitLevelProfessional> The worker needs the correct skill level (or higher).",
            "<*><img=WarsStockpileStop> Resource collection will be blocked by a full stockpile.",
            "<*>Work may have low or zero priority."
        ];


        public override string GameManualTitle_Soldiers => "Soldiers";

        public override string[] manual_soldiers => [
            "<h1>Produce soldiers",
            "<*><img=WarsBuild_Barracks> Place building: <name=barracks>",
            "<*><img=WarsWorker> Available workers to recruit.",
            "<*><img=WarsResource_Sword> A weapon for each soldier.",
            "<*><img=WarsHudIconProgress> Start: <name=queue>"
        ];

        public override string[] manual_food => [
            "<h1><img=WarsResource_Food> Food",
            "<*>All soldiers and workers consume food.",
            "<*>A large army can starve out the city in its area.",
            "<*><img=WarsBuild_TreeApple> Building more orchards doesn't automatically increase food; you need available workers to gather and process it.",
            "<*><img=WarsResource_Water> Food production requires water.",
            "<*>If you have a problem with starvation, you are probably pushing the water limit too hard - scale down.",
            "<*><img=WarsBuild_Postal> Make sure your cities support each other by sending food.",
        ];
    }
}
