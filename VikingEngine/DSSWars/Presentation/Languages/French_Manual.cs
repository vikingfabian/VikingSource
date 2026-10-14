using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class French
    {
        public override string GameManual => "Manuel du jeu";

        public override string GameManualTitle_Work => "Travail";

        public override string[] manual_work => [
            "<h1><img=WarsHammer> Travail",
    "<br>La collecte de ressources et la production dans votre ville sont entièrement automatiques.",
    "<br>La ville crée une file d'attente de toutes les tâches disponibles et les trie par ordre de priorité.",
    "<br>Dès qu'un travailleur est disponible, il prend la première tâche de la liste et l'exécute.",

    "<h1>Le travail ne commence pas",
    "<*><img=WarsBluePrint> Les bâtiments et l'artisanat nécessitent des ressources disponibles.",
    "<*><img=WarsUnitLevelProfessional> Le travailleur a besoin du niveau de compétence requis (ou supérieur).",
    "<*><img=WarsStockpileStop> La collecte de ressources sera bloquée si votre stock est plein.",
    "<*>Le travail a peut-être une priorité faible ou nulle."
        ];


        public override string GameManualTitle_Soldiers => "Soldats";

        public override string[] manual_soldiers => [
            "<h1>Former des soldats",
    "<*><img=WarsBuild_Barracks> Placer le bâtiment : <name=barracks>",
    "<*><img=WarsWorker> Des travailleurs disponibles à recruter.",
    "<*><img=WarsResource_Sword> Une arme pour chaque soldat.",
    "<*><img=WarsHudIconProgress> Démarrer : <name=queue>"
        ];

        public override string[] manual_food => [
            "<h1><img=WarsResource_Food> Nourriture",
    "<*>Tous les soldats et travailleurs consomment de la nourriture.",
    "<*>Une grande armée peut affamer la ville dans sa zone.",
    "<*><img=WarsBuild_TreeApple> Construire plus de vergers n'augmente pas automatiquement la nourriture ; vous avez besoin de travailleurs disponibles pour la récolter et la traiter.",
    "<*><img=WarsResource_Water> La production de nourriture nécessite de l'eau.",
    "<*>Si vous avez un problème de famine, vous poussez probablement trop la limite d'eau - réduisez la cadence.",
    "<*><img=WarsBuild_Postal> Assurez-vous que vos villes s'entraident en envoyant de la nourriture.",
];
    }
}
