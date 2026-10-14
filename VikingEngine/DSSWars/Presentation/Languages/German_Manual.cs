using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class German
    {
        //QoL Update
        public override string GameManual => "Spielhandbuch";

        public override string GameManualTitle_Work => "Arbeit";

        public override string[] manual_work => [
            "<h1><img=WarsHammer> Arbeit",
    "<br>Das Sammeln von Ressourcen und die Produktion in deiner Stadt verlaufen vollautomatisch.",
    "<br>Die Stadt erstellt eine Warteschlange aller verfügbaren Aufgaben und sortiert sie nach Priorität.",
    "<br>Sobald ein Arbeiter verfügbar ist, nimmt er sich die oberste Aufgabe und führt sie aus.",

    "<h1>Arbeit beginnt nicht",
    "<*><img=WarsBluePrint> Gebäude und Handwerk benötigen verfügbare Ressourcen.",
    "<*><img=WarsUnitLevelProfessional> Der Arbeiter benötigt das richtige Fähigkeitslevel (oder höher).",
    "<*><img=WarsStockpileStop> Das Sammeln von Ressourcen wird blockiert, wenn das Lager voll ist.",
    "<*>Die Arbeit hat möglicherweise eine niedrige oder gar keine Priorität."
        ];


        public override string GameManualTitle_Soldiers => "Soldaten";

        public override string[] manual_soldiers => [
            "<h1>Soldaten ausbilden",
    "<*><img=WarsBuild_Barracks> Gebäude platzieren: <name=barracks>",
    "<*><img=WarsWorker> Verfügbare Arbeiter zum Rekrutieren.",
    "<*><img=WarsResource_Sword> Eine Waffe für jeden Soldaten.",
    "<*><img=WarsHudIconProgress> Start: <name=queue>"
        ];

        public override string[] manual_food => [
            "<h1><img=WarsResource_Food> Nahrung",
    "<*>Alle Soldaten und Arbeiter verbrauchen Nahrung.",
    "<*>Eine große Armee kann die Stadt in ihrem Gebiet aushungern.",
    "<*><img=WarsBuild_TreeApple> Mehr Obstgärten zu bauen, erhöht nicht automatisch die Nahrung; du brauchst verfügbare Arbeiter, um sie zu ernten und zu verarbeiten.",
    "<*><img=WarsResource_Water> Die Nahrungsproduktion benötigt Wasser.",
    "<*>Wenn du Probleme mit Hunger hast, reizt du wahrscheinlich das Wasser-Limit zu sehr aus – fahre die Produktion zurück.",
    "<*><img=WarsBuild_Postal> Stelle sicher, dass deine Städte sich gegenseitig unterstützen, indem sie Nahrung schicken.",
];
     
    }
}
