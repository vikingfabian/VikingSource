using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Portuguese
    {
        //QoL Update
        public override string GameManual => "Manual do jogo";

        public override string GameManualTitle_Work => "Trabalho";

        public override string[] manual_work => [
            "<h1><img=WarsHammer> Trabalho",
    "<br>Toda a coleta de recursos e produção na sua cidade é totalmente automática.",
    "<br>A cidade criará uma fila com todas as tarefas disponíveis e as organizará por ordem de prioridade.",
    "<br>Assim que um trabalhador estiver disponível, ele pegará a primeira tarefa da lista e a executará.",

    "<h1>O trabalho não começa",
    "<*><img=WarsBluePrint> Construções e fabricações precisam de recursos disponíveis.",
    "<*><img=WarsUnitLevelProfessional> O trabalhador precisa do nível de habilidade correto (ou superior).",
    "<*><img=WarsStockpileStop> A coleta de recursos será bloqueada se o estoque estiver cheio.",
    "<*>O trabalho pode estar com prioridade baixa ou zero."
        ];


        public override string GameManualTitle_Soldiers => "Soldados";

        public override string[] manual_soldiers => [
            "<h1>Treinar soldados",
    "<*><img=WarsBuild_Barracks> Coloque a construção: <name=barracks>",
    "<*><img=WarsWorker> Trabalhadores disponíveis para recrutar.",
    "<*><img=WarsResource_Sword> Uma arma para cada soldado.",
    "<*><img=WarsHudIconProgress> Iniciar: <name=queue>"
        ];

        public override string[] manual_food => [
            "<h1><img=WarsResource_Food> Comida",
    "<*>Todos os soldados e trabalhadores consomem comida.",
    "<*>Um grande exército pode fazer a cidade da sua área passar fome.",
    "<*><img=WarsBuild_TreeApple> Construir mais pomares não aumenta a comida automaticamente; você precisa de trabalhadores disponíveis para coletar e processar.",
    "<*><img=WarsResource_Water> A produção de comida exige água.",
    "<*>Se você está com problemas de fome, provavelmente está forçando muito o limite de água - reduza a produção.",
    "<*><img=WarsBuild_Postal> Certifique-se de que suas cidades apoiam umas às outras enviando comida.",
];
        //-------

       
    }
}
