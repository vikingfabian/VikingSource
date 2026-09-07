using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Korean
    {
        //QoL Update
        public override string GameManual => "게임 매뉴얼";

        public override string GameManualTitle_Work => "작업";

        public override string[] manual_work => [
            "<h1><img=WarsHammer> 작업",
    "<br>도시의 모든 자원 수집 및 생산은 완전히 자동으로 이루어집니다.",
    "<br>도시는 가능한 모든 작업의 대기열을 만들고 우선순위에 따라 정렬합니다.",
    "<br>대기 중인 일꾼이 생기면 즉시 목록의 가장 위에 있는 작업을 맡아 수행합니다.",

    "<h1>작업이 시작되지 않을 때",
    "<*><img=WarsBluePrint> 건물 건설 및 제작에는 사용 가능한 자원이 필요합니다.",
    "<*><img=WarsUnitLevelProfessional> 일꾼이 적절한 숙련도(또는 그 이상)를 갖춰야 합니다.",
    "<*><img=WarsStockpileStop> 저장소가 꽉 차면 자원 수집이 중단됩니다.",
    "<*>작업의 우선순위가 낮거나 0으로 설정되어 있을 수 있습니다."
        ];


        public override string GameManualTitle_Soldiers => "병사";

        public override string[] manual_soldiers => [
            "<h1>병사 훈련",
    "<*><img=WarsBuild_Barracks> 건물 배치: <name=barracks>",
    "<*><img=WarsWorker> 징집 가능한 대기 중인 일꾼.",
    "<*><img=WarsResource_Sword> 각 병사를 위한 무기.",
    "<*><img=WarsHudIconProgress> 시작: <name=queue>"
        ];

        public override string[] manual_food => [
            "<h1><img=WarsResource_Food> 식량",
    "<*>모든 병사와 일꾼은 식량을 소비합니다.",
    "<*>대규모 군대는 해당 지역의 도시를 굶주리게 할 수 있습니다.",
    "<*><img=WarsBuild_TreeApple> 과수원을 더 짓는다고 식량이 자동으로 늘어나지는 않습니다. 수확하고 가공할 수 있는 일꾼이 필요합니다.",
    "<*><img=WarsResource_Water> 식량 생산에는 물이 필요합니다.",
    "<*>기아 문제가 발생했다면 식수 한계를 초과했을 가능성이 높습니다. 규모를 축소하세요.",
    "<*><img=WarsBuild_Postal> 식량을 보내 도시들이 서로 지원하도록 하세요.",
];
        //-------

   
        //-------

    }
}
