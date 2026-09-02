using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class SimplifiedChinese
    {
        //QoL Update
        public override string GameManual => "游戏手册";

        public override string GameManualTitle_Work => "工作";

        public override string[] manual_work => [
            "<h1><img=WarsHammer> 工作",
    "<br>你城市中的所有资源收集和生产完全自动进行。",
    "<br>城市将为所有可用任务创建一个队列，并按优先级排序。",
    "<br>一旦有工人空闲，他们就会挑选排在最前面的任务并执行。",

    "<h1>工作未开始的原因",
    "<*><img=WarsBluePrint> 建筑和制造需要有可用的资源。",
    "<*><img=WarsUnitLevelProfessional> 工人需要达到正确的技能等级（或更高）。",
    "<*><img=WarsStockpileStop> 仓库满载时将阻止资源收集。",
    "<*>工作的优先级可能较低或为零。"
        ];


        public override string GameManualTitle_Soldiers => "士兵";

        public override string[] manual_soldiers => [
            "<h1>生产士兵",
    "<*><img=WarsBuild_Barracks> 放置建筑：<name=barracks>",
    "<*><img=WarsWorker> 可用于招募的工人。",
    "<*><img=WarsResource_Sword> 每个士兵需要一件武器。",
    "<*><img=WarsHudIconProgress> 开始：<name=queue>"
        ];

        public override string[] manual_food => [
            "<h1><img=WarsResource_Food> 食物",
    "<*>所有士兵和工人都会消耗食物。",
    "<*>庞大的军队可能会让其所在区域的城市陷入饥荒。",
    "<*><img=WarsBuild_TreeApple> 建造更多果园不会自动增加食物；你需要有可用的工人来收集和加工它们。",
    "<*><img=WarsResource_Water> 食物生产需要水。",
    "<*>如果你遇到了饥荒问题，说明你可能把水资源额度压榨得太紧了——请缩减规模。",
    "<*><img=WarsBuild_Postal> 确保你的城市之间通过运送食物来互相支持。",
];
        //-------

        

    }
}
