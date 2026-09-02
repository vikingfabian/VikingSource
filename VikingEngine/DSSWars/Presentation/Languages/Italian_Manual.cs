using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Italian
    {
        //QoL Update
        public override string GameManual => "Manuale di gioco";

        public override string GameManualTitle_Work => "Lavoro";

        public override string[] manual_work => [
            "<h1><img=WarsHammer> Lavoro",
    "<br>La raccolta di risorse e la produzione nella tua città sono completamente automatiche.",
    "<br>La città creerà una coda di tutte le attività disponibili e le ordinerà per priorità.",
    "<br>Non appena un lavoratore è disponibile, prenderà la prima attività e la eseguirà.",

    "<h1>Il lavoro non inizia",
    "<*><img=WarsBluePrint> Edifici e artigianato richiedono risorse disponibili.",
    "<*><img=WarsUnitLevelProfessional> Il lavoratore ha bisogno del livello di abilità corretto (o superiore).",
    "<*><img=WarsStockpileStop> La raccolta di risorse sarà bloccata se il magazzino è pieno.",
    "<*>Il lavoro potrebbe avere priorità bassa o nulla."
        ];


        public override string GameManualTitle_Soldiers => "Soldati";

        public override string[] manual_soldiers => [
            "<h1>Reclutare soldati",
    "<*><img=WarsBuild_Barracks> Piazza edificio: <name=barracks>",
    "<*><img=WarsWorker> Lavoratori disponibili da reclutare.",
    "<*><img=WarsResource_Sword> Un'arma per ogni soldato.",
    "<*><img=WarsHudIconProgress> Avvia: <name=queue>"
        ];

        public override string[] manual_food => [
            "<h1><img=WarsResource_Food> Cibo",
    "<*>Tutti i soldati e i lavoratori consumano cibo.",
    "<*>Un grande esercito può affamare la città nella sua area.",
    "<*><img=WarsBuild_TreeApple> Costruire più frutteti non aumenta automaticamente il cibo; ti servono lavoratori disponibili per raccoglierlo e lavorarlo.",
    "<*><img=WarsResource_Water> La produzione di cibo richiede acqua.",
    "<*>Se hai un problema di carestia, probabilmente stai spingendo troppo il limite dell'acqua: riduci la produzione.",
    "<*><img=WarsBuild_Postal> Assicurati che le tue città si supportino a vicenda inviando cibo.",
];
     

    }
}
