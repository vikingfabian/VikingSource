using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Japanese
    {
        //QoL Update
        public override string GameManual => "ゲームマニュアル";

        public override string GameManualTitle_Work => "作業";

        public override string[] manual_work => [
            "<h1><img=WarsHammer> 作業",
    "<br>都市での資源収集と生産はすべて完全に自動化されています。",
    "<br>都市は利用可能なすべての作業のキューを作成し、優先順位に従って並べ替えます。",
    "<br>労働者が空き次第、リストの一番上の作業から順に実行します。",

    "<h1>作業が始まらない場合",
    "<*><img=WarsBluePrint> 建設や作成には利用可能な資源が必要です。",
    "<*><img=WarsUnitLevelProfessional> 労働者が適切なスキルレベル（またはそれ以上）を持っている必要があります。",
    "<*><img=WarsStockpileStop> 備蓄がいっぱいだと資源の収集が停止します。",
    "<*>作業の優先度が低い、またはゼロに設定されている可能性があります。"
        ];


        public override string GameManualTitle_Soldiers => "兵士";

        public override string[] manual_soldiers => [
            "<h1>兵士の訓練",
    "<*><img=WarsBuild_Barracks> 建物を配置する: <name=barracks>",
    "<*><img=WarsWorker> 徴兵可能な待機中の労働者。",
    "<*><img=WarsResource_Sword> 各兵士用の武器。",
    "<*><img=WarsHudIconProgress> 開始: <name=queue>"
        ];

        public override string[] manual_food => [
            "<h1><img=WarsResource_Food> 食料",
    "<*>すべての兵士と労働者は食料を消費します。",
    "<*>大軍が駐留すると、その地域の都市が飢餓に陥る可能性があります。",
    "<*><img=WarsBuild_TreeApple> 果樹園を増やしても自動的に食料は増えません。収穫や加工を行うための労働者が必要です。",
    "<*><img=WarsResource_Water> 食料の生産には水が必要です。",
    "<*>飢餓の問題が発生している場合、水不足の限界を超えている可能性が高いです。規模を縮小してください。",
    "<*><img=WarsBuild_Postal> 食料を送って、都市同士が確実に支援し合えるようにしましょう。",
];
        //-------

        //-------

    }
}
