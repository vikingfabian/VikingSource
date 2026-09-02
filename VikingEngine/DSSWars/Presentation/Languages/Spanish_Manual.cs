using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Spanish
    {
        //QoL Update
        public override string GameManual => "Manual del juego";

        public override string GameManualTitle_Work => "Trabajo";

        public override string[] manual_work => [
            "<h1><img=WarsHammer> Trabajo",
    "<br>Toda la recolección de recursos y producción en tu ciudad es completamente automática.",
    "<br>La ciudad creará una cola con todas las tareas disponibles y las ordenará por prioridad.",
    "<br>Tan pronto como un trabajador esté disponible, tomará la tarea superior y la llevará a cabo.",

    "<h1>El trabajo no empieza",
    "<*><img=WarsBluePrint> Los edificios y la artesanía necesitan recursos disponibles.",
    "<*><img=WarsUnitLevelProfessional> El trabajador necesita el nivel de habilidad correcto (o superior).",
    "<*><img=WarsStockpileStop> La recolección de recursos se bloqueará si el almacén está lleno.",
    "<*>El trabajo puede tener prioridad baja o nula."
        ];


        public override string GameManualTitle_Soldiers => "Soldados";

        public override string[] manual_soldiers => [
            "<h1>Producir soldados",
    "<*><img=WarsBuild_Barracks> Colocar edificio: <name=barracks>",
    "<*><img=WarsWorker> Trabajadores disponibles para reclutar.",
    "<*><img=WarsResource_Sword> Un arma para cada soldado.",
    "<*><img=WarsHudIconProgress> Iniciar: <name=queue>"
        ];

        public override string[] manual_food => [
            "<h1><img=WarsResource_Food> Comida",
    "<*>Todos los soldados y trabajadores consumen comida.",
    "<*>Un gran ejército puede hacer pasar hambre a la ciudad en su área.",
    "<*><img=WarsBuild_TreeApple> Construir más huertos no aumenta automáticamente la comida; necesitas trabajadores disponibles para recolectarla y procesarla.",
    "<*><img=WarsResource_Water> La producción de comida requiere agua.",
    "<*>Si tienes un problema de hambre, probablemente estás forzando demasiado el límite de agua; reduce la escala.",
    "<*><img=WarsBuild_Postal> Asegúrate de que tus ciudades se apoyen mutuamente enviando comida.",
];
        //-------

    }
}
