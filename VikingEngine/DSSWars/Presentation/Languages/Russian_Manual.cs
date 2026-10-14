using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Russian
    {
        //QoL Update
        public override string GameManual => "Игровое руководство";

        public override string GameManualTitle_Work => "Работа";

        public override string[] manual_work => [
            "<h1><img=WarsHammer> Работа",
    "<br>Вся добыча ресурсов и производство в вашем городе происходят полностью автоматически.",
    "<br>Город создаст очередь из всех доступных задач и отсортирует их по приоритету.",
    "<br>Как только появится свободный работник, он возьмет верхнюю задачу из списка и выполнит ее.",

    "<h1>Работа не начинается",
    "<*><img=WarsBluePrint> Для зданий и ремесел требуются доступные ресурсы.",
    "<*><img=WarsUnitLevelProfessional> Работнику нужен соответствующий уровень навыка (или выше).",
    "<*><img=WarsStockpileStop> Добыча ресурсов заблокируется, если склад переполнен.",
    "<*>У работы может быть низкий или нулевой приоритет."
        ];


        public override string GameManualTitle_Soldiers => "Солдаты";

        public override string[] manual_soldiers => [
            "<h1>Найм солдат",
    "<*><img=WarsBuild_Barracks> Построить здание: <name=barracks>",
    "<*><img=WarsWorker> Свободные работники для найма.",
    "<*><img=WarsResource_Sword> Оружие для каждого солдата.",
    "<*><img=WarsHudIconProgress> Старт: <name=queue>"
        ];

        public override string[] manual_food => [
            "<h1><img=WarsResource_Food> Еда",
    "<*>Все солдаты и работники потребляют еду.",
    "<*>Большая армия может истощить запасы продовольствия города в своем регионе.",
    "<*><img=WarsBuild_TreeApple> Строительство новых садов не увеличивает количество еды автоматически; вам нужны свободные работники для ее сбора и переработки.",
    "<*><img=WarsResource_Water> Для производства еды требуется вода.",
    "<*>Если у вас проблемы с голодом, вы, вероятно, слишком сильно исчерпали лимит воды — снизьте масштабы.",
    "<*><img=WarsBuild_Postal> Убедитесь, что ваши города поддерживают друг друга, отправляя еду.",
];
        //-------

    
    }
}
