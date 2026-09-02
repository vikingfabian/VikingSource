using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Polish
    {
        //QoL Update
        public override string GameManual => "Instrukcja gry";

        public override string GameManualTitle_Work => "Praca";

        public override string[] manual_work => [
            "<h1><img=WarsHammer> Praca",
    "<br>Zbieranie surowców i produkcja w twoim mieście są w pełni zautomatyzowane.",
    "<br>Miasto utworzy kolejkę wszystkich dostępnych zadań i posortuje je według priorytetu.",
    "<br>Gdy tylko robotnik będzie dostępny, wybierze pierwsze zadanie z listy i je wykona.",

    "<h1>Praca się nie rozpoczyna",
    "<*><img=WarsBluePrint> Budynki i rzemiosło wymagają dostępnych surowców.",
    "<*><img=WarsUnitLevelProfessional> Robotnik wymaga odpowiedniego poziomu umiejętności (lub wyższego).",
    "<*><img=WarsStockpileStop> Zbieranie surowców zostanie wstrzymane, jeśli magazyn jest pełny.",
    "<*>Praca może mieć niski lub zerowy priorytet."
        ];


        public override string GameManualTitle_Soldiers => "Żołnierze";

        public override string[] manual_soldiers => [
            "<h1>Szkolenie żołnierzy",
    "<*><img=WarsBuild_Barracks> Postaw budynek: <name=barracks>",
    "<*><img=WarsWorker> Dostępni robotnicy do zwerbowania.",
    "<*><img=WarsResource_Sword> Broń dla każdego żołnierza.",
    "<*><img=WarsHudIconProgress> Start: <name=queue>"
        ];

        public override string[] manual_food => [
            "<h1><img=WarsResource_Food> Żywność",
    "<*>Wszyscy żołnierze i robotnicy zużywają żywność.",
    "<*>Wielka armia może zagłodzić miasto w swoim regionie.",
    "<*><img=WarsBuild_TreeApple> Budowa większej liczby sadów nie zwiększa automatycznie żywności; potrzebujesz dostępnych robotników do jej zbioru i przetworzenia.",
    "<*><img=WarsResource_Water> Produkcja żywności wymaga wody.",
    "<*>Jeśli masz problem z głodem, prawdopodobnie zbytnio obciążasz limit wody - ogranicz produkcję.",
    "<*><img=WarsBuild_Postal> Upewnij się, że twoje miasta wspierają się nawzajem, wysyłając sobie żywność.",
];
        //-------

     
        //-------

    }
}
