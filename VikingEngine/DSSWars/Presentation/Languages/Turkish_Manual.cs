using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Turkish
    {
        //QoL Update
        public override string GameManual => "Oyun kılavuzu";

        public override string GameManualTitle_Work => "Çalışma";

        public override string[] manual_work => [
            "<h1><img=WarsHammer> Çalışma",
    "<br>Şehrinizdeki tüm kaynak toplama ve üretim işlemleri tamamen otomatiktir.",
    "<br>Şehir, mevcut tüm görevlerden bir sıra oluşturur ve bunları önceliğe göre sıralar.",
    "<br>Boşta bir işçi olur olmaz, listedeki en üstteki görevi seçer ve yerine getirir.",

    "<h1>Çalışma başlamıyor",
    "<*><img=WarsBluePrint> Binalar ve üretim için mevcut kaynaklar gereklidir.",
    "<*><img=WarsUnitLevelProfessional> İşçinin doğru beceri seviyesine (veya daha yüksek bir seviyeye) sahip olması gerekir.",
    "<*><img=WarsStockpileStop> Depo doluysa kaynak toplama işlemi engellenir.",
    "<*>Çalışmanın önceliği düşük veya sıfır olabilir."
        ];


        public override string GameManualTitle_Soldiers => "Askerler";

        public override string[] manual_soldiers => [
            "<h1>Asker üret</h1>",
    "<*><img=WarsBuild_Barracks> Bina yerleştir: <name=barracks>",
    "<*><img=WarsWorker> İşe alınabilecek boşta işçiler.",
    "<*><img=WarsResource_Sword> Her asker için bir silah.",
    "<*><img=WarsHudIconProgress> Başlat: <name=queue>"
        ];

        public override string[] manual_food => [
            "<h1><img=WarsResource_Food> Gıda",
    "<*>Tüm askerler ve işçiler gıda tüketir.",
    "<*>Büyük bir ordu, bölgesindeki şehri açlığa sürükleyebilir.",
    "<*><img=WarsBuild_TreeApple> Daha fazla meyve bahçesi kurmak gıdayı otomatik olarak artırmaz; toplamak ve işlemek için boşta işçilere ihtiyacınız var.",
    "<*><img=WarsResource_Water> Gıda üretimi su gerektirir.",
    "<*>Açlık sorunu yaşıyorsanız, muhtemelen su sınırını çok fazla zorluyorsunuzdur – ölçeği küçültün.",
    "<*><img=WarsBuild_Postal> Şehirlerinizin gıda göndererek birbirini desteklediğinden emin olun.",
];
        //-------


    }
}
