using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Presentation
{
    //GAME MANUAL
    partial class TodoTranslation
    {
        //static readonly string[] example = new string[] {
        //    "<h1>title",
        //    "<br>text",
        //    "continue text",
        //    "<p>new paragrapth",
        //    "<*>bullet point",
        //    "<h2>subtitle",
        //    "<br>text <img=InterfaceIconCamera> more text",
        //    "<br>Quotes are <name=ItemName>"
        //};
        public string GameManual => "Game manual";

        public string GameManualTitle_Work => "Work";

        public string[] manual_work => [
            "<h1><img=WarsHammer> Work",
            "<br>All resource collecting and production in your city is fully automatic.",
            "<br>The city will create a queue of all available tasks and sort them in order of priority.",
            "<br>As soon as a worker is available, he will pick the top task and carry it out.",
            
            "<h1>Work doesn't start",
            "<*><img=WarsBluePrint> Buildings and crafting need available resources.",
            "<*><img=WarsUnitLevelProfessional> The worker needs correct skill level (or higher).",
            "<*><img=WarsStockpileStop> Resource collection will be blocked by a full stockpile.",
            "<*>Work may have low or zero priority."
        ];


        public string GameManualTitle_Soldiers => "Soldiers";

        public string[] manual_soldiers => [
            "<h1>Produce soldiers",
            "<*><img=WarsBuild_Barracks> Place building: <name=barracks>",
            "<*><img=WarsWorker> Available workers to recruit.",
            "<*><img=WarsResource_Sword> A weapon for each soldier.",
            "<*><img=WarsHudIconProgress> Start: <name=queue>"
        ];

        public string[] manual_food => [
            "<h1><img=WarsResource_Food> Food",
            "<*>All soldiers and workers consume food.",
            "<*>A large army can starve out the city in its area.",
            "<*><img=WarsBuild_TreeApple> Building more orchards doesn't automatically increase food; you need available workers gather and process it.",
            "<*><img=WarsResource_Water> Food production requires water.",
            "<*>If you have problem with starvation, you are probably pushing the water limit too hard - scale down.",
            "<*><img=WarsBuild_Postal> Make sure your cities support each other by sending food.",
        ];
    }
}
