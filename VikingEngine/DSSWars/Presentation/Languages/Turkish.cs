using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.PJ;
using VikingEngine.ToGG.HeroQuest.Players.Ai;

namespace VikingEngine.DSSWars.Presentation
{
    //Licenesed 2025 Sep 11, "Astro: you can remix our work freely, any platform"
    partial class Turkish : AbsLanguage
    {
        //Post mount update
        public override string StockPile_ItemsAreNotLost => "Depo sınırını aşarsanız eşyalar yok olmaz!";
        public override string SlaughterResult_PerAnimal => "Hayvan başına kesim verimi";
        public override string Settings_Mode_QuickBoss => "Hızlı boss";
        public override string Settings_Mode_QuickBoss_Description => "Birkaç saat hazırlık yapın, ardından son boss ile karşılaşın";
        public override string QuickBoss_TimeOption => "Boss süresi (saat)";

        //Binek güncellemesi
        public override string Leaderboards_title => "Leaderboards";
        public override string Leaderboards_domination => "Dünya hakimiyeti en iyi süresi, %{0} artı";
        public override string Leaderboards_victory => "Hikaye zaferi, en iyi % zorluk";
        public override string Leaderboards_CitySize => "Maksimum şehir boyutu, işçi sayısına göre";
        public override string Leaderboards_Survival => "%{0} zorlukta hayatta kalma süresi";

        public override string Message_CannotPayUpkeep => "Bakım masrafı ödenemiyor!";
        public override string Animals_ProductionStop => "Hayvan üretimi duracak";

        public override string Tutorial_ToCapture => "Yakalamak için";
        public override string Tutorial_ClickButton => "Butona tıkla";
        public override string Tutorial_MoveXToY => "{0}'i {1}'e taşı";

        public override string Workers_Description1_work => "İnşaat yapacak, kaynak toplayacak ve eşya craftlayacaklar.";
        public override string Workers_Description2_income => "Gelir için vergi öderler.";
        public override string Workers_Description3_soldiers => "Orduların için asker olarak silah altına alınabilirler.";

        public override string Hud_Time_ValuePerMinute => "Dakika başına değer";
        public override string Hud_Time_ValuePerSecond => "Saniye başına değer";
        public override string Hud_Lock => "Kilitle";
        public override string Hud_Maximum => "Maks";

        public override string Tutorial_SeeThisInThat => "{1} içinde {0}'i gör";
        public override string Conscript_SkillBonus => "Skill bonusu";
        public override string SoldierStats_UnitCount => "Birim sayısı";
        /// <summary>
        /// Alanlar; ova, orman, deniz ve kuşatmadır
        /// </summary>
        public override string Conscript_DamagePerSecondInAreaX => "Saniye başına hasar - {0}";
        public override string Conscript_BaseHealth => "Base HP";

        /// <summary>
        /// Haritada ilerleyebilme yeteneği için özet değer
        /// </summary>
        public override string Conscript_Mobility => "Mobilite";

        public override string Conscript_RiderMobility => "Binici mobilitesi";
        public override string Conscript_LightWagonMobility => "Hafif araba mobilitesi";
        public override string Conscript_HeavyWagonMobility => "Ağır araba mobilitesi";

        /// <summary>
        /// Yetenekler, kaynaklar ve binalar gibi herhangi bir obje için genelleştirilmiş
        /// </summary>
        public override string Culture_AffectedItems => "Etkilenen item'lar";
        //## Binek güncellemesi ##
        public override string Progress_ClosingCores => "CPU çekirdekleri kapatılıyor {0}";
        public override string Editor_ExportFrame => "Mevcut frame'i dışa aktar";
        public override string Editor_FistFrame => "İlk frame";
        public override string Editor_LastFrame => "Son frame";

        public override string Economy_AnimalPenUpkeep => "Ağıl bakım masrafı: {0}";
        public override string Work_SlaughterX => "{0} kes";

        public override string BuildCategory_Farming => "Tarım";
        public override string Resource_TypeName_ManType => "insan tipi";
        public override string Resource_TypeName_NobelMen => "soylular";
        public override string Resource_TypeName_ConservedFood => "konserve yiyecek";

        public override string UnitType_UnitOnMount => "{0} biniyor";
        public override string UnitType_UnitOnWagon => "{0} arabası";
        public override string UnitType_NobelUnit => "soylu {0}";

        /// <summary>
        /// 0: asker tipi, 1: hayvan
        /// </summary>
        public override string UnitType_LeashAnimalHandler => "{0} {1}-terbiyecisi";

        public override string Info_ArmyFood4 => "Konserve yiyecekler daha büyük bir erzak stoğuna izin verir";
        public override string Info_ArmyFood5 => "Önce taze yiyecekler tüketilir";

        public override string Resource_ConservedFood_Reserves => "Konserve yiyecek stokları";
        public override string Resource_TypeName_Clay => "kil";
        public override string Resource_TypeName_Brick => "tuğla";
        public override string Resource_TypeName_Container => "kutu";
        public override string Resource_TypeName_Meat => "et";
        public override string Resource_TypeName_Salt => "tuz";
        public override string Resource_TypeName_Vehicle => "araç";
        public override string Resource_TypeName_WagonClosed => "kapalı araba";
        public override string Resource_TypeName_WagonIron => "demir araba";
        public override string Resource_TypeName_WagonSteel => "çelik araba";
        public override string Resource_TypeName_Shield => "kalkan";
        public override string Resource_TypeName_BucklerShield => "buckler kalkanı";
        public override string Resource_TypeName_RoundShield => "yuvarlak kalkan";
        public override string Resource_TypeName_HeaterShield => "üçgen kalkan";
        public override string Resource_TypeName_TowerShield => "kule kalkanı";

        public override string Resource_TypeName_Mount => "binek";

        public override string Resource_TypeName_MountArmorTitle => "binek zırhı";

        /// <summary>
        /// 0: zırh tipi
        /// </summary>
        public override string Resource_TypeName_MountArmorX => "binek {0}";
        public override string Resource_TypeName_Animal => "hayvan";

        //public override string Resource_TypeName_WildAnimal => "vahşi hayvan";

        /// <summary>
        /// Vahşi hayvanların bulunduğu alan
        /// </summary>
        public override string Terrain_XAnimalHabitat => "{0} habitatı";

        public override string Resource_TypeName_Oxen => "öküz";
        public override string Resource_TypeName_KineOxen => "inek";

        /// <summary>
        /// Düşük tier tavuk (üretim için)
        /// </summary>
        public override string Resource_TypeName_Fowl => "kümes hayvanı";

        /// <summary>
        /// Düşük tier domuz (üretim için)
        /// </summary>
        public override string Resource_TypeName_Boar => "erkek domuz";
        public override string Resource_TypeName_Pig => "domuz";
        public override string Resource_TypeName_Hen => "tavuk";
        public override string Resource_TypeName_Dog => "köpek";
        public override string Resource_TypeName_Hound => "tazı";

        public override string Resource_TypeName_Pony => "midilli";
        public override string Resource_TypeName_Horse => "at";
        public override string Resource_TypeName_WarHorse => "savaş atı";
        public override string Resource_TypeName_DraftHorse => "yük atı";

        public override string Resource_TypeName_WildPig => "yabani domuz";
        public override string Resource_TypeName_WildHog => "yaban domuzu";
        public override string Resource_TypeName_WarHog => "savaş domuzu";
        public override string Resource_TypeName_StagHog => "boynuzlu domuz";

        public override string Resource_TypeName_Wolf => "kurt";
        public override string Resource_TypeName_Warg => "warg";
        public override string Resource_TypeName_AlphaWarg => "alfa warg";

        public override string Resource_TypeName_WildCat => "yaban kedisi";
        public override string Resource_TypeName_Lion => "aslan";
        public override string Resource_TypeName_WarLion => "savaş aslanı";

        public override string Resource_TypeName_Elephant => "fil";
        public override string Resource_TypeName_WarElephant => "savaş fili";
        public override string Resource_TypeName_Oliphant => "olifant";

        public override string BuildHud_Select => "Bina seç";
        public override string BuildHud_AreaRadius => "Alan yarıçapı";

        public override string NobleHouse_HousingCount => "{0} soyluya ev sahipliği yapacak";


        public override string BuildingType_GreatHall => "Büyük Salon";
        public override string BuildingType_GreatHall_Description => "Gelişmiş asker alımını açar";

        public override string BuildingType_ClayPit => "Kil Ocağı";
        public override string BuildingType_Butcher => "Kasap";
        public override string BuildingType_Butcher_Description => "Hayvanları yiyecek ve deriye dönüştürür";
        public override string BuildingType_Pottery => "Çömlekçi";
        public override string BuildingType_CraftX_Description => "{0} craft istasyonu";

        public override string BuildingType_GatherX_Description => "{0} topla";

        public override string BuildingType_Smoker => "Tütsüleyici";
        public override string BuildingType_Dryer => "Kurutucu";
        public override string BuildingType_Shieldmaker => "Kalkan Üreticisi";
        public override string BuildingType_DryingPan => "Kurutma Tavası";

        public override string BuildingType_TrapperHut => "Avcı Kulübesi";
        public override string BuildingType_TrapperHut_Description => "Vahşi hayvanların yakalanmasını sağlar";

        // --- Depolama ---
        public override string BuildingType_MaterialStorage => "Materyal Deposu";
        public override string BuildingType_FoodStorage => "Erzak Deposu";
        public override string BuildingType_WeaponStorage => "Silah Deposu";
        public override string BuildingType_ArmorStorage => "Zırh Deposu";
        public override string BuildingType_AnimalStorage => "Hayvan Deposu";

        public override string BuildingType_Storage_Description => "Maksimum stok limitini {0} artırır";

        public override string BuildingType_Cesspit => "Atık Çukuru";
        public override string BuildingType_Cesspit_Description => "Kaynakları yok eder";

        public override string BuildingType_Cesspit_Info1_StockPile => "Stok limitini aşan item'ları yok eder";
        public override string Info_XAmountIsConvertedToY => "{0}, {1}'e dönüştürülür";
        public override string Info_ProductionRestriction => "İtem üretimi şu şekilde sınırlandırıldı:";

        public override string BuildingType_FowlPen => "Kümes";
        public override string BuildingType_BoarPen => "Erkek Domuz Ağılı";

        // --- Öküz Ağılları ---
        public override string BuildingType_OxenPen => "Öküz Ağılı";
        public override string BuildingType_KineOxenPen => "İnek Ağılı";

        // --- Köpek Kafesleri ---
        public override string BuildingType_DogCage => "Köpek Kafesi";
        public override string BuildingType_HoundCage => "Tazı Kafesi";

        // --- At Ağılları ---
        public override string BuildingType_PonyPen => "Midilli Ağılı";
        public override string BuildingType_HorsePen => "At Ağılı";
        public override string BuildingType_WarHorsePen => "Savaş Atı Ağılı";
        public override string BuildingType_DraftHorsePen => "Yük Atı Ağılı";

        // --- Domuz Ağılları ---
        public override string BuildingType_WildPigPen => "Yabani Domuz Ağılı";
        public override string BuildingType_WildHogPen => "Yaban Domuzu Ağılı";
        public override string BuildingType_WarHogPen => "Savaş Domuzu Ağılı";
        public override string BuildingType_StagHogPen => "Boynuzlu Domuz Ağılı";

        // --- Kurt Kafesleri ---
        public override string BuildingType_WolfCage => "Kurt Kafesi";
        public override string BuildingType_WargCage => "Warg Kafesi";
        public override string BuildingType_AlphaWargCage => "Alfa Warg Kafesi";

        // --- Kedi Kafesleri ---
        public override string BuildingType_WildCatCage => "Yaban Kedisi Kafesi";
        public override string BuildingType_LionCage => "Aslan Kafesi";
        public override string BuildingType_WarLionCage => "Savaş Aslanı Kafesi";

        // --- Fil Kafesleri ---
        public override string BuildingType_ElephantCage => "Fil Ağılı";
        public override string BuildingType_WarElephantCage => "Savaş Fili Ağılı";
        public override string BuildingType_OliphantCage => "Olifant Ağılı";

        public override string BuildingDescription_Animals => "Asker alımı için hayvanlar üretir";
        public override string Pen_Breeding => "Hayvan yetiştiriciliği";
        public override string Pen_BreedUpChance => "Tier atlama şansı: %{0}";
        public override string Pen_BreedDownChance => "Tier düşme şansı: %{0}";


        public override string CityCulture_AnimalBreeder2_Description => "Daha yüksek başarılı yetiştirme şansı";

        public override string CityCulture_EnhancedProduction => "Gelişmiş {0} üretimi";
        public override string CityCulture_Production => "{0} üretimi";

        public override string CityCulture_Butchers => "Kasaplar";

        public override string CityCulture_Potters => "Çömlekçiler";

        public override string CityCulture_Wainwright => "Arabacılar";

        public override string CityCulture_Wheelwright => "Tekerlekçiler";
        public override string CityCulture_Wheelwright_Description => "Askere alınan arabalar için speed bonusu";

        public override string CityCulture_ShieldMaker => "Kalkan Üreticileri";


        //public override string CityCulture_Nomads_Description => "Düşük yerleşimci maliyeti";

        public override string CityCulture_Coopers => "Fıçıcılar";

        public override string CityCulture_Salters => "Tuzcular";


        public override string CityBiome_Title => "Biyom";
        public override string CityBiome_Description => "Biyomlar bazı kaynaklara ve binalara erişimi etkiler";

        public override string CityBiome_Fields => "Ovalar";
        public override string CityBiome_Frozen => "Buzul";
        public override string CityBiome_Forest => "Orman";
        public override string CityBiome_Mountain => "Dağlık";
        public override string CityBiome_Desolate => "Issız";
        public override string CityBiome_Desert => "Çöl";

        public override string Bonus_IncreaseSkin => "Artırılmış deri üretimi";
        public override string Bonus_FoodStorage => "Daha büyük erzak deposu";

        public override string StockPile_LimitTitle => "Stok limiti";


        public override string Help_Work_Automatic => "Çalışma otomatiktir";
        public override string Tutorial_SecondCity => "İkinci bir şehir ele geçir";
        //## Spring update

        public override string InputAction_SkipAutomated => "Otomatikleri atla";

        public override string Resource_WaterReason => "Su, destekleyebileceğiniz birlik sayısını ve üretim kapasitenizi sınırlar";
        public override string BuildingType_Orchard => "Meyve Bahçesi";
        public override string BuildingType_ManorLord => "Malikane Lordu";
        public override string BuildingType_ManorLord_Description => "Gıda işlemeyi açar";
        /// <summary>
        /// Will end diplomatic relations like alliance
        /// </summary>
        public override string Diplomacy_EndRelations => "İlişkileri sonlandır";

        /// <summary>
        /// Where a resource is produced or found
        /// </summary>
        public override string ItemSource => "Eşya kaynağı";

        public override string ItemSource_Terrain => "Arazi";
        public override string ItemSource_Farm => "Çiftlik";
        public override string ItemSource_CraftStation => "Zanaat istasyonu";
        public override string ItemSource_Gathering => "Toplayıcılık";

        public override string CityCulture_Nomad => "Göçebe";

        /// <summary>
        /// A generalized display of buffs and boons, example "+100%" or "Doubled"
        /// </summary>
        public override string Hud_ChangeFactor => "Değişim katsayısı: {0}";

        public override string Hud_Purchase_LowXCost => "Düşük {0} maliyeti";

        public override string WorkQueue_Title => "İş kuyruğu";
        public override string WorkQueue_Length => "Kalan iş hedefleri";
        public override string WorkQueue_ActiveWorkers => "Aktif iş ekipleri";
        public override string WorkQueue_IdleWorkers => "Boşta iş ekipleri";

        public override string WorkTeam_Size => "Köylüler {0} kişilik ekipler halinde çalışır";

        public override string ObjectUi_ViewOnMap => "Haritada göster";
        public override string ObjectUi_StuckBuildOrders => "Takılan inşa emirleri";
        public override string Hud_AllArmies => "Tüm ordular";

        public override string Hud_CurrentPage => "Mevcut sayfa";
        public override string Hud_AllPages => "Tüm sayfalar";
        public override string Hud_ToAllCities => "Tüm şehirlere";
        public override string Hud_ToFaction => "Fraksiyona";
        public override string Hud_FromFaction => "Fraksiyondan";
        public override string Hud_FactionWide => "Fraksiyon genelindeki ayarı kullan";
        /// <summary>
        /// This start a new city
        /// </summary>
        public override string Action_PlaceSettlement => "Yerleşim yeri kur";

        public override string Editor_Animation_RemoveAllFramesButThis => "Diğer tüm kareleri kaldır";

        //Winter patch 3
        public override string Hud_Purchase_AllBuildings => "Tüm binaları sıraya al";
        public override string Hud_Purchase_AllTech => "Tüm teknolojileri sıraya al";
        public override string BuildingType_CasualBarracks_Description => "Asker toplama süresi kışlalar arasında bölünür";

        //Winter update patch + spring
        /// <summary>
        /// How much of a resource that will be used, e.g. "5 gold". There will be a "cost" title above the text. 0: Resource, 1: cost
        /// </summary>
        public override string Language_ItemCount => "{1} {0}";

        //public override string DisplayMode => "Görüntü Modu";
        //public override string DisplayMode_Windowed => "Pencere";
        //public override string DisplayMode_BorderlessFullscreen => "Çerçevesiz Tam Ekran";

        //public override string GameSettings_RenderedMouseCursor => "Yazılımsal İmleç"; // "Software cursor" - standard tech term
        //public override string GameSettings_MuteControllerDisconnect => "Gamepad kopma uyarısını sustur";

        public override string Delivery_MaxDistance => "Maks. teslimat mesafesi: {0}";
        public override string Tutorial_WillTakeAWhile => "Bu biraz sürecek, daha sonra tekrar gel.";

        /// <summary>
        /// 0: name of building
        /// </summary>
        public override string Tutorial_WaitFor => "{0} tamamlanmasını bekle";
        public override string GameOverResults => "Oyun Geçmişi";

        public override string UnitType_UnclaimedLand => "Sahipsiz Topraklar";
        public override string UnitType_Settler => "Yerleşimci";
        public override string UnitType_Settler_Description => "Yeni bir şehir kur";
        public override string Resource_ConsumedProduced => "Tüketilen/Üretilen";
        public override string InputActionName_PlaceTarget => "Hedef yerleştir";

        public override string FactionStartSize => "Fraksiyon başlangıç boyutu";
        public override string FactionStartSize_Full => "Tam";
        public override string FactionStartSize_OneCity => "Tek şehir";
        public override string FactionStartSize_Settler => "Tek yerleşimci";


        //Winter update
        public override string Resource_StockpileLimit => "Depo limiti";
        public override string GameMode_QuickMatch => "Hızlı Eşleşme";
        public override string GameMode_QuickMatch_Description =>
            "Daha kısa süren bir oyun formatı. Rakip uluslara karşı topyekün savaşa girin.";
        public override string Lobby_PlayerCount => "Oyuncu sayısı";
        public override string Lobby_TwoTeams => "İki takım";
        public override string Hud_Produce => "Üret:";
        public override string Tutorial_WaitForWorkerLevel => "İşçi şu seviyeye ulaşana kadar bekle:";

        public override string Tutorial_PracticeOrSchool => "{0} üzerinde pratik yap veya bir {1} kullan";
        public override string Tutorial_AddTag => "Etiket ekle:";
        public override string Tutorial_AddPin => "işaret ekle:";
        public override string Tutorial_SelectMostTrees => "En çok ağaca sahip şehrini bul";
        public override string Tutorial_SelectACityWithX => "{0} bulunan bir şehir seç";

        public override string Tutorial_Select_NotCapital => ". Bu senin başkentin değil.";

        public override string Tutorial_SetXPriorityToY => "{0} önceliğini {1} olarak ayarla";
        public override string Tutorial_AdvisorMission => "Danışman görevi";

        public override string Tutorial_AdvisorDescription =>
            "Esas oyun başladı. Danışman, eğitimine yararlı görevler ekleyecek.";

        public override string Tutorial_EndAdvisor => "Danışman görevini bitir";

        public override string Tutorial_AdvisorCompleteTitle => "Danışman görevi tamamlandı!";
        public override string Tutorial_AdvisorCompleteMessage => "Güzel günler dileriz!";

        public override string Hud_Search => "Ara";

        public override string DifficultyDescription_ExtremeAggression => "Aşırı saldırganlık";

        public override string MapFilter => "Harita filtresi";

        public override string Settings_TechMultiplier => "Teknoloji araştırma hızı";

        public override string EndScreen_MatchComplete => "Maç sonucu";

        public override string FactionName_DragonGem => "Ejder Mücevheri";
        public override string FactionName_Tomten => "Tomten";
        public override string FactionName_Hælfolc => "Hælfolc";
        public override string FactionName_AerimAngren => "Aerim Angren";

        public override string HUD_NotAvailbleInX => "{0} içerisinde kullanılamaz";

        public override string InputActionName_MiniMap => "Mini-harita";
       
        //--
        public override string Error_SoundInitFailure => "Ses başlatılamadı";

        public override string GameMenu_ControllerDisconnected => "Kontrolcü bağlantısı kesildi";

        public override string Tutorial_HighPriority => "Askerlerin önce yüksek öncelikli görevleri tamamlayacak.";

        public override string BuildingType_Wall_Description => "Duvarlar birliklerini saldırılardan korur ve küçük bir saldırı boost'u sağlar.";

        public override string BuildingType_Wall_Siege => "Kuşatma silahları duvarların savunmasını azaltır.";

        public override string Conscript_BlockChance => "Bir saldırıyı bloklama olasılığı: %{0}";

        public override string Battle_DeclarWarReminder => "Saldırmadan önce savaş ilan etmelisin.";

        //--

        /// <summary>
        /// Name of this language
        /// </summary>
        public override string MyLanguage => "Türkçe";

        /// <summary>
        /// How to display a number of items. 0: item, 1:Number
        /// </summary>
        public override string Language_ItemCount_Colon => "{0}: {1}";

        /// <summary>
        /// Select language option
        /// </summary>
        public override string Lobby_Language => "Dil";

        /// <summary>
        /// Start playing the game
        /// </summary>
        public override string Lobby_Start => "BAŞLA";

        /// <summary>
        /// Button to select local mutiplayer count, 0:current player count
        /// </summary>
        public override string Lobby_LocalMultiplayerEdit => "Yerel çoklu oyuncu";

        /// <summary>
        /// Title for menu where you select split screen player count
        /// </summary>
        public override string Lobby_LocalMultiplayerTitle => "Oyuncu sayısını seç";

        /// <summary>
        /// Description for local multiplayer
        /// </summary>
        public override string Lobby_LocalMultiplayerControllerRequired => "Çoklu oyuncu Xbox kontrolcüsü gerektirir";

        /// <summary>
        /// Move to next split screen position
        /// </summary>
        public override string Lobby_NextScreen => "Diğer ekran konumu";

        /// <summary>
        /// Players can select visual appearance and store them in a profile
        /// </summary>
        public override string Lobby_FlagSelectTitle => "Bayrak seç";

        /// <summary>
        /// 0: Numbered 1 to 16
        /// </summary>
        public override string Lobby_FlagNumbered => "Bayrak {0}";

        /// <summary>
        /// Game name and version number
        /// </summary>
        //public override string Lobby_GameVersion => "DSS war party - ver {0}";

        public override string FlagEditor_Description => "Bayrağını oluştur ve ordun için bir renk seç.";

        /// <summary>
        /// Paint tool that fills an area with a color
        /// </summary>
        public override string FlagEditor_Bucket => "Kova";

        /// <summary>
        /// Opens flag profile editor
        /// </summary>
        public override string Lobby_FlagEdit => "Bayrağı düzenle";


        public override string Lobby_WarningTitle => "Uyarı";
        public override string Lobby_IgnoreWarning => "Uyarıyı görmezden gel";

        /// <summary>
        /// Warning when one player has no input selected.
        /// </summary>
        public override string Lobby_PlayerWithoutInputWarning => "Bir oyuncu girdi ataması yapmamış";

        /// <summary>
        /// Menu with content that are outside what most players will use.
        /// </summary>
        public override string Lobby_Extra => "Ekstra";

        /// <summary>
        /// The extra content is not translated or have full controller support.
        /// </summary>
        public override string Lobby_Extra_NoSupportWarning => "Uyarı! Bu içerik yerelleştirme kapsamı altında değil veya girdi/erişilebilirlik desteğine sahip değil";


        public override string Lobby_MapSizeTitle => "Harita boyutu";

        /// <summary>
        /// Map size 1 name
        /// </summary>
        public override string Lobby_MapSizeOptTiny => "Minnacık";

        /// <summary>
        /// Map size 2 name
        /// </summary>
        public override string Lobby_MapSizeOptSmall => "Küçük";

        /// <summary>
        /// Map size 3 name
        /// </summary>
        public override string Lobby_MapSizeOptMedium => "Orta";

        /// <summary>
        /// Map size 4 name
        /// </summary>
        public override string Lobby_MapSizeOptLarge => "Büyük";

        /// <summary>
        /// Map size 5 name
        /// </summary>
        public override string Lobby_MapSizeOptHuge => "Çok Büyük";

        /// <summary>
        /// Map size 6 name
        /// </summary>
        public override string Lobby_MapSizeOptEpic => "Devasa";

        /// <summary>
        /// Map size description X by Y kilometers. 0: Width, 1: Height
        /// </summary>
        public override string Lobby_MapSizeDesc => "{0}x{1} km";
        /// <summary>
        /// Close game application
        /// </summary>
        public override string Lobby_ExitGame => "Çıkış";

        /// <summary>
        /// Display local multiplayer name, 0: player number
        /// </summary>
        public override string Player_DefaultName => "Oyuncu {0}";

        /// <summary>
        /// In player profile editor. Opens menu with editor options
        /// </summary>
        public override string ProfileEditor_OptionsMenu => "Ayarlar";

        /// <summary>
        /// In player profile editor. Title for selecting flag colors
        /// </summary>
        public override string ProfileEditor_FlagColorsTitle => "Bayrak renkleri";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_MainColor => "Ana renk";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail1Color => "1. Detay rengi";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail2Color => "2. Detay rengi";

        /// <summary>
        /// In player profile editor. Title for selecting you soldiers colors
        /// </summary>
        public override string ProfileEditor_PeopleColorsTitle => "Halk";

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_SkinColor => "Ten rengi";

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_HairColor => "Saç rengi";

        /// <summary>
        /// In player profile editor. Open color palette and select color
        /// </summary>
        public override string ProfileEditor_PickColor => "Renk seç";

        /// <summary>
        /// In player profile editor. Adjust image position
        /// </summary>
        public override string ProfileEditor_MoveImage => "Resmi hareket ettir";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageLeft => "Sol";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageRight => "Sağ";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageUp => "Yukarı";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageDown => "Aşağı";

        /// <summary>
        /// In player profile editor. Close editor without saving
        /// </summary>
        public override string ProfileEditor_DiscardAndExit => "Çöpe at ve çık";

        /// <summary>
        /// In player profile editor. Tooltip for discarding
        /// </summary>
        public override string ProfileEditor_DiscardAndExitDescription => "Bütün değişikleri geri al";

        /// <summary>
        /// In player profile editor. Save changes and close editor
        /// </summary>
        public override string Hud_SaveAndExit => "Kaydet ve çık";

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Hue => "Ton";

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Lightness => "Parlaklık";

        /// <summary>
        /// In player profile editor. Move between flag and soldier color options.
        /// </summary>
        public override string ProfileEditor_NextColorType => "Diğer renk tipi";

        /// <summary>
        /// Current running speed of the game, compared to real time
        /// </summary>
        public override string Hud_GameSpeedLabel => "Oyun Hızı: {0}x";

        public override string Input_GameSpeed => "Oyun Hızı";

        /// <summary>
        /// Ingame display. Unit gold production
        /// </summary>
        public override string Hud_TotalIncome => "Saniye Başı Gelir: {0}";

        /// <summary>
        /// Unit gold cost.
        /// </summary>
        public override string Hud_Upkeep => "Bakım Maaliyeti";
        public override string Hud_ArmyUpkeep => "Ordu Bakım Maaliyeti: {0}";

        /// <summary>
        /// Ingame display. Soldiers protecting a building.
        /// </summary>
        public override string Hud_GuardCount => "Muhafızlar";

        public override string Hud_IncreaseMaxGuardCount => "Maks Muhafız Sayısı {0}";

        public override string Hud_GuardCount_MustExpandCityMessage => "Şehri Genişletmen Gerek.";

        public override string Hud_SoldierCount => "Asker Sayısı";

        public override string Hud_SoldierGroupsCount => "Grup Sayısı";

        /// <summary>
        /// Ingame display. Unit caculated battle strength.
        /// </summary>
        public override string Hud_StrengthRating => "Güç Oranı";

        /// <summary>
        /// Ingame display. Caculated battle strength for the whole nation.
        /// </summary>
        public override string Hud_TotalStrengthRating => "Askeri Güç: {0}";

        /// <summary>
        /// Ingame display. Extra men coming from outside the city state.
        /// </summary>
        public override string Hud_Immigrants => "Göçmenler";


        public override string Hud_CityCount => "Şehir Sayısı: {0}";
        public override string Hud_ArmyCount => "Ordu Sayısı: {0}";


        /// <summary>
        /// Mini button to repeat a purchase a number of times. E.G. "x5"
        /// </summary>
        public override string Hud_XTimes => "x{0}";

        public override string Hud_PurchaseTitle_Requirement => "Gereklilik";
        public override string Hud_PurchaseTitle_Cost => "Maaliyet";
        public override string Hud_PurchaseTitle_Gain => "Kazanç";

        /// <summary>
        /// How much of a resource that will be used, "5 gold. (Available: 10)". There will be a "cost" title above the text. 0: Resource, 1: cost, 2: available
        /// </summary>
        public override string Hud_Purchase_ResourceCostOfAvailable => "{1} {0}. (Mevcutta: {2})";

        public override string Hud_Purchase_CostWillIncreaseByX => "Maaliyet Artışı: {0}";

        public override string Hud_Purchase_MaxCapacity => "Maksimum kapasiteye ulaşıldı";

        public override string Hud_CompareMilitaryStrength_YourToOther => "Güç: Sen {0} - Rakip {1}";

        /// <summary>
        /// Display a short string of date as Year, Month, Day
        /// </summary>
        public override string Hud_Date => "Y{0} A{1} G{2}";

        /// <summary>
        /// Display a short string of timespan as Hour, Minutes, Seconds
        /// </summary>
        public override string Hud_TimeSpan => "S{0} D{1} Sn{2}";

        /// <summary>
        /// Battle between two armies, or army and city
        /// </summary>
        public override string Hud_Battle => "Muharebe";



        /// <summary>
        /// Describes button input. Pause.
        /// </summary>
        public override string Input_Pause => "Durdur";

        /// <summary>
        /// Describes button input. Resume from paused.
        /// </summary>
        public override string Input_ResumePaused => "Devam et";

        /// <summary>
        /// Generic money resource
        /// </summary>
        public override string ResourceType_Gold => "Sikke";

        /// <summary>
        /// Working men resource
        /// </summary>
        public override string ResourceType_Workers => "İşçiler";


        public override string ResourceType_Workers_Description => "İşçiler sikke getirir. Ve orduna asker olur";

        /// <summary>
        /// The resource used in diplomacy
        /// </summary>
        public override string ResourceType_DiplomacyPoints => "Diplomasi Puanı";

        /// <summary>
        /// 0: How many points you got, 1: Soft max value (will increase much slower after this), 2: Hard limit
        /// </summary>
        public override string ResourceType_DiplomacyPoints_WithSoftAndHardLimit => "Diplomasi Puanı: {0} / {1} ({2})";

        /// <summary>
        /// City building type. Building for knights and diplomats.
        /// </summary>
        public override string Building_NobleHouse => "Soylu Evi";

        public override string Building_NobleHouse_DiplomacyPointsAdd => "{0} saniye başı 1 diploma puanı";
        public override string Building_NobleHouse_DiplomacyPointsLimit => "+{0} puan, maks diplomasi puanı limitine eklenir";
        public override string Building_NobleHouse_UnlocksKnight => "Şövalye birliğinin kilidini açar";

        public override string Building_BuildAction => "İnşa et";
        public override string Building_IsBuilt => "İnşa edildi";

        /// <summary>
        /// City building type. Evil mass production.
        /// </summary>
        public override string Building_DarkFactory => "Kara fabrika";

        /// <summary>
        /// In game settings menu. Sums all difficulty options in percentage.
        /// </summary>
        public override string Settings_TotalDifficulty => "Toplam zorluk seviyesi %{0}";

        /// <summary>
        /// In game settings menu. Base difficulty option.
        /// </summary>
        public override string Settings_DifficultyLevel => "Zorluk seviyesi %{0}";


        /// <summary>
        ///  In game settings menu.Option for creating new maps instead of loading one
        /// </summary>
        public override string Settings_GenerateMaps => "Yeni harita yarat";

        /// <summary>
        ///  In game settings menu.Creating new maps has a longer loading time
        /// </summary>
        public override string Settings_GenerateMaps_SlowDescription => "Harita yaratmak, hazır bir haritayı açmaktan daha uzun sürer";

        /// <summary>
        ///  In game settings menu.Difficulty option. Block the ability to play the game while paused.
        /// </summary>
        public override string Settings_AllowPause => "Oyun duraksatılmışken emir verebilme";

        /// <summary>
        ///  In game settings menu.Difficulty option. Have bosses that enter the game.
        /// </summary>
        public override string Settings_BossEvents => "Baş Düşman etkinliği";

        /// <summary>
        ///  In game settings menu.Difficulty option. No Boss description.
        /// </summary>
        public override string Settings_BossEvents_SandboxDescription => "Baş düşman etkinliğini kapatmak oyunu sonu olmayan (serbest) moda sokacaktır";


        /// <summary>
        /// Options for automating game mechanics. Menu title.
        /// </summary>
        public override string Automation_Title => "Otomasyon";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_MaxWorkforce => "İşgücü maks olana kadar beklet";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_NegativeIncome => "Gelir eksiye düşene kadar beklet";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_Priority => "Büyük şehirleri önceliklendir";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_PurchaseSpeed => "Saniye başı maks bir satın alım gerçekleştir";


        /// <summary>
        /// Button caption for action. A specialized building for knights and diplomats.
        /// </summary>
        public override string HudAction_BuyItem => "Satın al {0}";

        /// <summary>
        /// The state of peace or war between two nations
        /// </summary>
        public override string Diplomacy_RelationType => "İlişki";

        /// <summary>
        /// Titel for list of relations other factions have with eachother
        /// </summary>
        public override string Diplomacy_RelationToOthers => "Diğerleri ile ilişkileri";

        /// <summary>
        /// Diplomatic relation. You are in direct control over the nations resources.
        /// </summary>
        public override string Diplomacy_RelationType_Servant => "Vasal";

        /// <summary>
        /// Diplomatic relation. Full co-operation.
        /// </summary>
        public override string Diplomacy_RelationType_Ally => "Müttefik";

        /// <summary>
        /// Diplomatic relation. Reduced chance of war.
        /// </summary>
        public override string Diplomacy_RelationType_Good => "İyi";

        /// <summary>
        /// Diplomatic relation. Peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Peace => "Barışta";

        /// <summary>
        /// Diplomatic relation. Have not yet made any contact.
        /// </summary>
        public override string Diplomacy_RelationType_Neutral => "Tarafsız";
        /// <summary>
        /// Diplomatic relation. Temporary peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Truce => "Ateşkeste";
        /// <summary>
        /// Diplomatic relation. War.
        /// </summary>
        public override string Diplomacy_RelationType_War => "Savaşta";
        /// <summary>
        /// Diplomatic relation. War with no chance of peace.
        /// </summary>
        public override string Diplomacy_RelationType_TotalWar => "Topyekün Savaşta";

        /// <summary>
        /// Diplomatic communication. How well you can discuss terms. 0: SpeakTerms
        /// </summary>
        public override string Diplomacy_SpeakTermIs => "İlişki Durumu";

        /// <summary>
        /// Diplomatic communication. Better than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Good => "İyi";

        /// <summary>
        /// Diplomatic communication. Normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Normal => "Normal";

        /// <summary>
        /// Diplomatic communication. Worse than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad => "Kötü";

        /// <summary>
        /// Diplomatic communication. Will not communicate.
        /// </summary>
        public override string Diplomacy_SpeakTerms_None => "İrtibat Yok";

        /// <summary>
        /// Diplomatic action. Make a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo => "Şununla ilişki kur: {0}";

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferPeace => "Barış Teklifi";

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferAlliance => "Müttefiklik Teklifi";

        /// <summary>
        /// Diplomatic title. Another player Suggested a new diplomatic relation. 0: player name
        /// </summary>
        public override string Diplomacy_PlayerOfferAlliance => "{0} oyuncusu yeni bir ilişki talep etti";

        /// <summary>
        /// Diplomatic action. Accept new diplomatic relation.
        /// </summary>
        public override string Diplomacy_AcceptRelationOffer => "Yeni ilişkiyi kabul et";

        /// <summary>
        /// Diplomatic description. Another player Suggested a new diplomatic relation. 0: relation type
        /// </summary>
        public override string Diplomacy_NewRelationOffered => "Önerilen yeni ilişki: {0}";

        /// <summary>
        /// Diplomatic action. Make another nation to serve you.
        /// </summary>
        public override string Diplomacy_AbsorbServant => "Vasalın yap";

        /// <summary>
        /// Diplomatic description. Is against evil.
        /// </summary>
        public override string Diplomacy_LightSide => "Aydınlık taraf müttefiği";

        /// <summary>
        /// Diplomatic description. How long the truce will last.
        /// </summary>
        public override string Diplomacy_TruceTimeLength => "{0} saniye içerisinde sona erecek";

        /// <summary>
        /// Diplomatic action. Make the truce last longer.
        /// </summary>
        public override string Diplomacy_ExtendTruceAction => "Ateşkesi uzat";

        /// <summary>
        /// Diplomatic description. How long the truce will be extended.
        /// </summary>
        public override string Diplomacy_TruceExtendTimeLength => "Ateşkes {0} saniye uzatılacak";

        /// <summary>
        /// Diplomatic description. Going against an agreed relation will cost diplomatic points.
        /// </summary>
        public override string Diplomacy_BreakingRelationCost => "İlişkiyi bozmak {0} diplomasi puanına mal olur";

        /// <summary>
        /// Diplomatic description for allies.
        /// </summary>
        public override string Diplomacy_AllyDescription => "Müttefikler, savaş ilanlarına ortak olur.";

        /// <summary>
        /// Diplomatic description for good relation.
        /// </summary>
        public override string Diplomacy_GoodRelationDescription => "Savaş ilanı yetkisini kısıtlar.";

        /// <summary>
        /// Diplomatic description. You must have a larger military force than your servant (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_XStrongerMilitary => "{0}x daha yüksek askeri güç";

        /// <summary>
        /// Diplomatic description. Servant must be stuck in a hopeless war (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_HopelessWar => "Vasal, daha güçlü bir düşmana karşı savaşta olmalı";

        /// <summary>
        /// Diplomatic description. A servant can't own too many cities (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_MaxCities => "Vasalın sahip olabileceği maks şehir sayısı: {0}";

        /// <summary>
        /// Diplomatic description. Const in diplomatic points will increase (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantPriceWillRise => "Bedel, her vasal başı artar";

        /// <summary>
        /// Diplomatic description. The result of servant relation, peaceful take over of another nation.
        /// </summary>
        public override string Diplomacy_ServantGainAbsorbFaction => "Diğer tarafın topraklarını ilhak et";

        /// <summary>
        /// Messaage when you recieve a war declaration
        /// </summary>
        public override string Diplomacy_WarDeclarationTitle => "Bu bir Savaş İlanı!";

        /// <summary>
        /// The truce timer har run out, and you go back to war
        /// </summary>
        public override string Diplomacy_TruceEndTitle => "Ateşkes sona erdi";

        /// <summary>
        /// Stats that are shown on the end game screen. Display title.
        /// </summary>
        public override string Statistics_Title => "İstatistikler";
        /// <summary>
        /// Stats that are shown on the end game screen. Total ingame time passed.
        /// </summary>
        public override string EndGameStatistics_Time => "Geçen Süre: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. How many soldiers you bought.
        /// </summary>
        public override string EndGameStatistics_SoldiersRecruited => "{0} asker eğitildi";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of your soldiers that died in battle.
        /// </summary>
        public override string EndGameStatistics_FriendlySoldiersLost => "Zayiat: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of opponent soldiers you killed in battle.
        /// </summary>
        public override string EndGameStatistics_EnemySoldiersKilled => "Düşman zayiatı: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of your soldiers that have left you.
        /// </summary>
        public override string EndGameStatistics_SoldiersDeserted => "Firar eden asker sayısı: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of cities won in battle.
        /// </summary>
        public override string EndGameStatistics_CitiesCaptured => "{0} şehir ele geçirildi";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of cities lost in battle.
        /// </summary>
        public override string EndGameStatistics_CitiesLost => "{0} şehir kaybedildi";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of battle win results.
        /// </summary>
        public override string EndGameStatistics_BattlesWon => "Kazanılan muharebe sayısı: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of battle lost results.
        /// </summary>
        public override string EndGameStatistics_BattlesLost => "Kaybedilen muharebe sayısı: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Diplomacy. War declarations made by you.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByYou => "Başlatılan savaş sayısı {0}";

        /// <summary>
        /// Stats that are shown on the end game screen.  Diplomacy. War declarations made toward you.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByEnemy => "Düşmanın başlattığı savaş sayısı: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Allies made through diplomacy.
        /// </summary>
        public override string EndGameStatistics_AlliedFactions => "Yapılan ittifak sayısı {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Servants made through diplomacy. Servants cities and armies become yours.
        /// </summary>
        public override string EndGameStatistics_ServantFactions => "Edinilen Vasal sayısı {0}";

        /// <summary>
        /// Collective unit type on the map. Army of soldiers.
        /// </summary>
        public override string UnitType_Army => "Ordu";

        /// <summary>
        /// Collective unit type on the map. Army of soldiers.
        /// </summary>
        public override string UnitType_SoldierGroup => "Grup";

        /// <summary>
        /// Collective unit type on the map. Common name for village or city.
        /// </summary>
        public override string UnitType_City => "Şehir";

        /// <summary>
        /// A group selection of armies
        /// </summary>
        public override string UnitType_ArmyCollectionAndCount => "Ordu grubu, sayısı: {0}";

        /// <summary>
        /// Name for a specialized type of soldier. Standard front line soldier.
        /// </summary>
        public override string UnitType_Soldier => "Asker";

        /// <summary>
        /// Name for a specialized type of soldier. Naval battle soldier.
        /// </summary>
        public override string UnitType_Sailor => "Denizci";

        /// <summary>
        /// Name for a specialized type of soldier. Drafted peasants.
        /// </summary>
        public override string UnitType_Folkman => "Milis";

        /// <summary>
        /// Name for a specialized type of soldier. Shield and spear unit.
        /// </summary>
        public override string UnitType_Spearman => "Mızrakçı";

        /// <summary>
        /// Name for a specialized type of soldier. Elite force, part of the Kings guard.
        /// </summary>
        public override string UnitType_HonorGuard => "Şan Muhafızı";

        /// <summary>
        /// Name for a specialized type of soldier. Anti cavalry, wears long two-handed spears.
        /// </summary>
        public override string UnitType_Pikeman => "Kargıcı";

        /// <summary>
        /// Name for a specialized type of soldier. Armored cavalry unit.
        /// </summary>
        public override string UnitType_Knight => "Şövalye";

        /// <summary>
        /// Name for a specialized type of soldier. Bow and arrow.
        /// </summary>
        public override string UnitType_Archer => "Okçu";

        /// <summary>
        /// Name for a specialized type of soldier. 
        /// </summary>
        public override string UnitType_Crossbow => "Arbeletçi";

        /// <summary>
        /// Name for a specialized type of soldier. Warmashine that slings large spears.
        /// </summary>
        public override string UnitType_Ballista => "Balista";

        /// <summary>
        /// Name for a specialized type of soldier. A fantasy troll wearing a cannon.
        /// </summary>
        public override string UnitType_Trollcannon => "Trol topu";

        /// <summary>
        /// Name for a specialized type of soldier. Soldier from the forest.
        /// </summary>
        public override string UnitType_GreenSoldier => "Orman Korucusu";

        /// <summary>
        /// Name for a specialized type of soldier. Naval unit from the north.
        /// </summary>
        public override string UnitType_Viking => "Viking";

        /// <summary>
        /// Name for a specialized type of soldier. The evil master boss.
        /// </summary>
        public override string UnitType_DarkLord => "Kara Lord";

        /// <summary>
        /// Name for a specialized type of soldier. Soldier that carries a large flag.
        /// </summary>
        public override string UnitType_Bannerman => "Sancaktar";

        /// <summary>
        /// Name for a military unit. Soldier carrying ship. 0: unit type it carries
        /// </summary>
        public override string UnitType_WarshipWithUnit => "{0} harp gemisi";

        public override string UnitType_Description_Soldier => "Çok amaçlı birim";
        public override string UnitType_Description_Sailor => "Deniz muharebesinde etkilidir";
        public override string UnitType_Description_Folkman => "Düşük maliyetli ve eğitimsiz askerler";
        public override string UnitType_Description_HonorGuard => "Bakım maaliyeti olmayan, seçkin askerler";
        public override string UnitType_Description_Knight => "Meydan muharebelerinde etkilidir";
        public override string UnitType_Description_Archer => "Korunmaya gerek duyar ve uzun menzilden destek ateşi sağlar";
        public override string UnitType_Description_Crossbow => "Güçlü, menzilli asker";
        public override string UnitType_Description_Ballista => "Şehirlere karşı etkili";
        public override string UnitType_Description_GreenSoldier => "Korkulan elf savaşçısı";

        public override string UnitType_Description_DarkLord => "Büyük kötü";

        /// <summary>
        /// Information about a soldier type
        /// </summary>
        public override string SoldierStats_Title => "Birim başı istatistik";

        /// <summary>
        /// How many groups of soldiers
        /// </summary>
        public override string SoldierStats_GroupCountAndSoldierCount => "{0} grup, toplam {1} birim";

        /// <summary>
        /// Soldiers will have different strengths depending if the attack on open field, from ships or attacking a settlement
        /// </summary>
        public override string SoldierStats_AttackStrengthLandSeaCity => "Saldırı gücü: Kara {0} | Deniz {1} | Şehir {2}";

        /// <summary>
        /// How many wounds a soldier can endure
        /// </summary>
        public override string SoldierStats_Health => "Sağlık";

        /// <summary>
        /// Some soldiers will increase the army movement speed
        /// </summary>
        public override string SoldierStats_SpeedBonusLand => "Karadaki ordu hız bonusu: {0}";

        /// <summary>
        /// Some soldiers will increase the ship movement speed
        /// </summary>
        public override string SoldierStats_SpeedBonusSea => "Denizdeki ordu hız bonusu: {0}";

        /// <summary>
        /// Purchased soliders will start as recruits and complete their training after a few minutes.
        /// </summary>
        public override string SoldierStats_RecruitTrainingTimeMinutes => "Eğitim süresi: {0} dakika. Acemi erler, şehir yakınında iki kat daha hızlı eğitilir.";

        /// <summary>
        /// Menu option to control an army. Make them stop moving.
        /// </summary>
        public override string ArmyOption_Halt => "Dur!";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_Disband => "Birimi terhis et";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_Divide => "Orduyu böl";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_RemoveX => "Şunu kaldır: {0}";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_DisbandAll => "Hepsini terhis et";

        /// <summary>
        /// Menu option to control an army. 0: Count, 1: Unit type
        /// </summary>
        public override string ArmyOption_XGroupsOfType => "{1} grubu: {0}";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendToX => "Birimleri şuraya gönder: {0}";

        public override string ArmyOption_MergeAllArmies => "Tüm orduları birleştir";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendToNewArmy => "Orduyu böl ve yeni bir ordu oluştur";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string Hud_SendX => "Şunu gönder: {0}";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendAll => "Hepsini gönder";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_DivideHalf => "Orduyu ikiye böl";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_MergeArmies => "Orduları birleştir";



        /// <summary>
        /// Purchase soldiers.
        /// </summary>
        public override string UnitType_Recruit => "Acemi Er";

        /// <summary>
        /// Purchase soldiers of type. 0:type
        /// </summary>
        public override string CityOption_RecruitType => "{0} Acemi Er";

        /// <summary>
        /// Number of paid soldiers
        /// </summary>
        public override string CityOption_XMercenaries => "Paralı Asker: {0}";


        /// <summary>
        /// Indicates the number of mercenaries currently available for hire from the market
        /// </summary>
        public override string Hud_MercenaryMarket => "İşe alınabilir paralı asker";

        /// <summary>
        /// Purchase a number of paid soldiers
        /// </summary>
        public override string CityOption_BuyXMercenaries => "{0} paralı asker işe al";

        public override string CityOption_Mercenaries_Description => "Askere alım, iş gücü yerine paralı askerler arasından yapılacak";

        /// <summary>
        /// Button caption for action. Create housing for more workers.
        /// </summary>
        public override string CityOption_ExpandWorkForce => "İş gücünü genişlet";
        public override string CityOption_ExpandWorkForce_IncreaseMax => "Maks iş gücü +{0}";
        public override string CityOption_ExpandGuardSize => "Muhafız sayısını arttır";

        public override string CityOption_Damages => "Hasar: {0}";
        public override string CityOption_Repair => "Bütün hasarları onar";
        public override string CityOption_RepairGain => "{0} hasarı onar";

        public override string CityOption_Repair_Description => "Hasar miktarı, atayabileceğin iş gücünü azaltır.";


        public override string CityOption_BurnItDown => "Yerle bir et";
        public override string CityOption_BurnItDown_Description => "İş gücünü kaldır ve tam hasar uygula";

        /// <summary>
        /// The main boss. Named after a glowing metal stone stuck in their forehead.
        /// </summary>
        public override string FactionName_DarkLord => "Kara Göz";

        /// <summary>
        /// Orc inspired faction. Works for the dark lord.
        /// </summary>
        public override string FactionName_DarkFollower => "Servants of Dread Korkunun Kulları";

        /// <summary>
        /// The largest faction, the old but corrupted kingdom.
        /// </summary>
        public override string FactionName_UnitedKingdom => "Birleşik Krallıklar";

        /// <summary>
        /// Elf inspired faction. Lives in harmony with the forest.
        /// </summary>
        public override string FactionName_Greenwood => "Yeşil Koru";

        /// <summary>
        /// Asian flavored faction to the east 
        /// </summary>
        public override string FactionName_EasternEmpire => "Doğu İmparatorluğu";

        /// <summary>
        /// Viking flavored kingdom in the north. The largest one.
        /// </summary>
        public override string FactionName_NordicRealm => "Nordik Krallığı";

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a bear claw symbol.
        /// </summary>
        public override string FactionName_BearClaw => "Ayı Pençesi";

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a cock symbol.
        /// </summary>
        public override string FactionName_NordicSpur => "Nord Sıradağları";

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a black raven symbol.
        /// </summary>
        public override string FactionName_IceRaven => "Beyaz Kuzgun";

        /// <summary>
        /// Faction famous for killing dragons with powerful ballistas.
        /// </summary>
        public override string FactionName_Dragonslayer => "Ejder Katili";

        /// <summary>
        /// A mercenary unit from the south. Arabic flavored.
        /// </summary>
        public override string FactionName_SouthHara => "Güney Hara";

        /// <summary>
        /// Name for neutral CPU controlled nations
        /// </summary>
        public override string FactionName_GenericAi => "YZ {0}";

        /// <summary>
        /// Display name for players and their numbers
        /// </summary>
        public override string FactionName_Player => "Oyuncu {0}";

        /// <summary>
        /// Message for when a miniboss is approaching on ships from the south.
        /// </summary>
        public override string EventMessage_HaraMercenaryTitle => "Düşman yaklaşıyor!";
        public override string EventMessage_HaraMercenaryText => "Güneyden gelen Hara paralı askerleri tespit edildi";

        /// <summary>
        /// First warning that the main boss will appear.
        /// </summary>
        public override string EventMessage_ProphesyTitle => "Karanlık kehanet";
        public override string EventMessage_ProphesyText => "Kara Göz yakında ortaya çıkacak ve düşmanların onun yanında saf tutacak!";

        /// <summary>
        /// Second warning that the main boss will appear.
        /// </summary>
        public override string EventMessage_FinalBossEnterTitle => "Zor zamanlar";
        public override string EventMessage_FinalBossEnterText => "Kara Göz haritaya giriş yaptı!";

        /// <summary>
        /// Message when the main boss will meet you on the battlefield.
        /// </summary>
        public override string EventMessage_FinalBattleTitle => "Çaresizce bir saldırı";
        public override string EventMessage_FinalBattleText => "Kara lord savaş meydanında. Artık onu yok etmek için bir şansın var!";

        /// <summary>
        /// Message when soldiers leave the army when you can't pay thier upkeep
        /// </summary>
        public override string EventMessage_DesertersTitle => "Firariler!";
        public override string EventMessage_DesertersText_Money => "Parası ödenmemiş askerler, ordundan firar ediyorlar";

        //public override string DifficultyDescription_AiAggression => "YZ agresifliği: {0}.";
        public override string DifficultyDescription_BossSize => "Baş düşman gücü: {0}.";
        public override string DifficultyDescription_BossEnterTime => "Baş düşmanın gelişine kalan süre: {0}.";
        public override string DifficultyDescription_AiEconomy => "YZ ekonomisi: %{0}.";
        public override string DifficultyDescription_AiDelay => "YZ gecikmesi: {0}.";
        public override string DifficultyDescription_DiplomacyDifficulty => "Diplomasi zorluğu: {0}.";
        public override string DifficultyDescription_MercenaryCost => "Paralı Asker maaliyeti: {0}.";
        public override string DifficultyDescription_HonorGuards => "Şan muhafızı: {0}.";


        /// <summary>
        /// Game has ended in success.
        /// </summary>
        public override string EndScreen_VictoryTitle => "Galibiyet!";

        /// <summary>
        /// Quotes from the leader character you play in the game
        /// </summary>
        public override List<string> EndScreen_VictoryQuotes => new List<string>
        {
            "Barış vakti şehitlerimize yas tutarız.",
            "Her galibiyet ile bizden biraz daha fazlası kopar.",
            "Bastığın yerleri 'toprak' diyerek geçme, tanı, düşün altındaki binlerce kefensiz yatanı.",
            "Gönlümüz zafer ile tüy gibi hafif; ancak kalbimiz, şehitlerimiz ile toprak altında."
        };

        public override string EndScreen_DominationVictoryQuote => "Bu cihanı hizaya getirmek, Tanrıların bana verdiği hükümdür!";

        /// <summary>
        /// Game has ended in failure.
        /// </summary>
        public override string EndScreen_FailTitle => "Mağlubiyet!";

        /// <summary>
        /// Quotes from the leader character you play in the game
        /// </summary>
        public override List<string> EndScreen_FailureQuotes => new List<string>
        {
            "Bedenlerimiz zayıf düşse de, geceler korku ile geçse de, yaklaşan sonumuzu selamlıyoruz. ",
            "Mağlubiyet vatan toprağını karartabilir belki, ama azmimiz ile yanan ışığı asla.",
            "Kalbimizdeki alevleri söndür; çocuklarımız, küllerimizden bir güneş gibi doğacaktır.",
            "Bizimle biten bu öykü yansın, yarın zafere gidecek yola ışık yaksın.",
        };

        /// <summary>
        /// A small cutscene at the end of the game
        /// </summary>
        public override string EndScreen_WatchEpilogue => "Kapanışı İzle";

        /// <summary>
        /// Cutscene title
        /// </summary>
        public override string EndScreen_Epilogue_Title => "Kapanış";

        /// <summary>
        /// Cutscene introduction
        /// </summary>
        public override string EndScreen_Epilogue_Text => "100 yıl önce";

        /// <summary>
        /// The Prologue is a short poem about the game's stroy
        /// </summary>
        public override string GameMenu_WatchPrologue => "Açılışı İzle";

        public override string Prologue_Title => "Açılış";

        /// <summary>
        /// The poem must be three lines, the fourth line will be pulled from the names translations to present the name of the boss
        /// </summary>
        public override List<string> Prologue_TextLines => new List<string>
        {
            "Kabuslar sana musallat oluyor",
            "Karanlık bir geleceğin kehanetidir bu",
            "Geliyor gelmekte olan",
        };

        /// <summary>
        /// Ingame menu when pausing
        /// </summary>
        public override string GameMenu_Title => "Oyun menüsü";

        /// <summary>
        /// Continue playing the game after end screen
        /// </summary>
        public override string GameMenu_ContinueGame => "Oyuna Devam Et";

        /// <summary>
        /// Continue playing the game
        /// </summary>
        public override string GameMenu_Resume => "Devam et";

        /// <summary>
        /// Exit to game lobby
        /// </summary>
        public override string GameMenu_ExitGame => "Oyundan Çık";

        public override string Hud_Save => "Kaydet";
        public override string GameMenu_SaveStateWarnings => "Dikkat! Kayıt dosyaları oyun güncellenince yok olacak.";
        public override string GameMenu_LoadState => "Yükle";
        public override string GameMenu_ContinueFromSave => "Kaldığın yerden devam et";

        public override string GameMenu_AutoSave => "Otomatik kayıt";

        public override string GameMenu_Load_PlayerCountError => "Kayıt dosyası için uyumlu oyuncu sayısı belirlemelisin: {0}";

        public override string Progressbar_MapLoadingState => "Harita yükleniyor {0}";

        public override string Progressbar_ProgressComplete => "tamamlandı";

        /// <summary>
        /// 0: progress in percentage, 1: fail count
        /// </summary>
        public override string Progressbar_MapLoadingState_GeneratingPercentage => "Oluşturulan: {0}%. (Hata {1})";


        /// <summary>
        /// 0: current part, 1: number of parts
        /// </summary>
        public override string Progressbar_MapLoadingState_LoadPart => "kısım {0}/{1}";

        /// <summary>
        /// 0: Percentage or Complete
        /// </summary>
        public override string Progressbar_SaveProgress => "Kaydediliyor: {0}";

        /// <summary>
        /// 0: Percentage or Complete
        /// </summary>
        public override string Progressbar_LoadProgress => "Yükleniyor: {0}";

        /// <summary>
        /// Progress done, waiting for player input
        /// </summary>
        public override string Progressbar_PressAnyKey => "Devam etmek için herhangi bir tuşa bas";


        /// <summary>
        /// A short tutorial where you are supposed to buy and move a soldier. All advanced controls are locked away until the tutorial is complete.
        /// </summary>
        public override string Tutorial_MenuOption => "Eğiticiyi başlat";
        public override string Tutorial_MissionsTitle => "Eğitici görevleri";
        public override string Tutorial_Mission_BuySoldier => "Bir şehir seç ve asker eğit";
        public override string Tutorial_Mission_MoveArmy => "Bir ordu seç ve hareket ettir";

        public override string Tutorial_CompleteTitle => "Eğitici tamamlandı!";
        public override string Tutorial_CompleteMessage => "Tam yakınlaştırma ve gelişmiş oyun seçenekleri artık kullanılabilir";

        /// <summary>
        /// Displays the button input
        /// </summary>
        public override string Tutorial_SelectInput => "Seç";
        public override string Tutorial_MoveInput => "İlerleme komutu";



        /// <summary>
        /// Versus. Text describing the two armies that will go into battle
        /// </summary>
        public override string Hud_Versus => "VS.";

        public override string Hud_WardeclarationTitle => "Savaş ilanı";

        public override string ArmyOption_Attack => "Saldır";



        //----
        /// <summary>
        /// In game settings menu. Change what keys and buttons do when pressed
        /// </summary>
        public override string Settings_ButtonMapping => "Tuş atamaları";



        /// <summary>
        /// Input type, standard PC input
        /// </summary>
        public override string Input_Source_Keyboard => "Klavye ve Fare";

        /// <summary>
        /// Input type, handheld controller like the xbox uses
        /// </summary>
        public override string Input_Source_Controller => "Kontrolcü";


        /* #### --------------- ##### */
        /* #### RESOURCE UPDATE ##### */
        /* #### --------------- ##### */
        public override string CityMenu_SalePricesTitle => "Satış fiyatları";
        public override string Blueprint_Title => "Şablon";
        public override string Resource_Tab_Overview => "Genel bakış";
        public override string Resource_Tab_Stockpile => "Depo";

        public override string Resource => "Kaynak";
        public override string Resource_StockPile_Info => "Hangi kaynağın ne kadar depolanacağını belirle; işçiler bu seçime göre diğer işlere yönelir.";
        public override string Resource_TypeName_Water => "su";
        public override string Resource_TypeName_Wood => "odun";
        public override string Resource_TypeName_Fuel => "yakıt";
        public override string Resource_TypeName_Stone => "taş";
        public override string Resource_TypeName_RawFood => "çiğ gıda";
        public override string Resource_TypeName_Food => "gıda";
        public override string Resource_TypeName_Beer => "bira";
        public override string Resource_TypeName_Wheat => "buğday";
        public override string Resource_TypeName_Linen => "keten";
        //public override string Resource_TypeName_SkinAndLinen => "skin and linen";
        public override string Resource_TypeName_IronOre => "demir Cevheri";
        public override string Resource_TypeName_GoldOre => "altın Cevheri";
        public override string Resource_TypeName_Iron => "demir";

        public override string Resource_TypeName_SharpStick => "Sivri çubuk";
        public override string Resource_TypeName_Sword => "Kılıç";
        public override string Resource_TypeName_KnightsLance => "Şövalye'nin mızrağı";
        public override string Resource_TypeName_TwoHandSword => "Zweihänder";
        public override string Resource_TypeName_Bow => "Yay";

        public override string Resource_TypeName_LightArmor => "Hafif zırh";
        public override string Resource_TypeName_MediumArmor => "Orta zırh";
        public override string Resource_TypeName_HeavyArmor => "Ağır zırh";

        public override string ResourceType_Children => "Çocuklar";

        public override string BuildingType_DefaultName => "Yapılar";
        public override string BuildingType_WorkerHut => "İşçi kulübesi";
        public override string BuildingType_Tavern => "Taverna";
        public override string BuildingType_Brewery => "Biracı";
        public override string BuildingType_Postal => "Postane";
        public override string BuildingType_Recruitment => "Asker alım merkezi";
        public override string BuildingType_Barracks => "Kışla";
        public override string BuildingType_PigPen => "Domuz çiftliği";
        public override string BuildingType_HenPen => "Tavuk çiftliği";
        public override string BuildingType_WorkBench => "Çalışma masası";
        public override string BuildingType_Carpenter => "Marangoz";
        public override string BuildingType_CoalPit => "Kömür çukuru";
        public override string DecorType_Statue => "Heykel";
        public override string DecorType_Pavement => "Kaldırım";
        public override string BuildingType_Smith => "Demirci";
        public override string BuildingType_Cook => "Aşçı";
        public override string BuildingType_Storehouse => "Depo";

        public override string BuildingType_ResourceFarm => "{0} çiftlik";

        public override string BuildingType_WorkerHut_DescriptionLimitX => "İşçi limitini şu kadar arttırır: {0}";
        public override string BuildingType_Tavern_Description => "İşçiler burada yiyebilir";
        public override string BuildingType_Tavern_Brewery => "Bira üretimi";
        public override string BuildingType_Postal_Description => "Diğer şehirlere kaynak gönder";
        public override string BuildingType_Recruitment_Description => "Diğer şehirlere insangücü gönder";
        public override string BuildingType_Barracks_Description => "Asker eğitmek için ekipman ve insangücü kullanır";
        public override string BuildingType_PigPen_Description => "Gıda ve deri için domuz yetiştirir";
        public override string BuildingType_HenPen_Description => "Et ve yumurta için tavuk yetiştirir";
        public override string BuildingType_Decor_Description => "Dekorasyon";
        public override string BuildingType_Farm_Description => "Bir kaynak üretir";

        public override string BuildingType_Cook_Description => "Gıda üretim tezgahı";
        public override string BuildingType_Bench_Description => "Eşya üretim tezgahı";

        public override string BuildingType_Smith_Description => "Metal işleme atölyesi";
        public override string BuildingType_Carpenter_Description => "Kereste atölyesi";

        public override string BuildingType_Nobelhouse_Description => "Şövalyeler ve diplomatlar için konaklama";
        public override string BuildingType_CoalPit_Description => "Yüksek verimle yakıt üretir";
        //public override string BuildingType_Storehouse_Description => "Kaynak stoklama noktası";

        public override string MenuTab_Info => "Bilgi";
        public override string MenuTab_Work => "İş";
        public override string MenuTab_Recruit => "Eğit";
        public override string MenuTab_Resources => "Kaynaklar";
        public override string MenuTab_Trade => "Ticaret";
        public override string MenuTab_Build => "İnşaat";
        public override string MenuTab_Economy => "Ekonomi";
        public override string MenuTab_Delivery => "Teslimat";

        public override string MenuTab_Build_Description => "Yapıları şehrin içine yerleştir";
        public override string MenuTab_BlackMarket_Description => "Yapıları şehrin içine yerleştir";
        public override string MenuTab_Resources_Description => "Yapıları şehrin içine yerleştir";
        public override string MenuTab_Work_Description => "Yapıları şehrin içine yerleştir";
        public override string MenuTab_Automation_Description => "Yapıları şehrin içine yerleştir";

        public override string BuildHud_OutsideCity => "Şehir bölgesinin dışında";
        public override string BuildHud_OutsideFaction => "Sınırlarının dışında!";

        public override string BuildHud_OccupiedTile => "Dolu alan/zemin";

        public override string Build_PlaceBuilding => "Yapı";
        public override string Build_DestroyBuilding => "Yık";
        public override string Build_ClearTerrain => "Zemini temizle";

        public override string Build_ClearOrders => "İnşa emirlerini iptal et";
        public override string Build_Order => "İnşa emirleri";
        public override string Build_OrderQue => "{0} inşa kuyruğu";
        public override string Build_AutoPlace => "Otomatik yerleştir";

        public override string Work_OrderPrioTitle => "İş önceliği";
        public override string Work_OrderPrioDescription => "Öncelik seviyesi 1'den (düşük) şuna çıkar: {0} (yüksek)";

        public override string Work_OrderPrio_No => "Öncelik atanmamış, iş yapılmayacak";
        public override string Work_OrderPrio_Min => "Minimum öncelik";
        public override string Work_OrderPrio_Max => "Maksimum öncelik";

        public override string Work_Move => "Eşyaları taşı";

        public override string Work_GatherXResource => "Topla: {0}";
        public override string Work_CraftX => "Üret: {0}";
        public override string Work_Farming => "Çiftçilik";
        public override string Work_Mining => "Madencilik";
        public override string Work_Trading => "Ticaret";

        public override string Work_AutoBuild => "Otomatik inşa ve genişleme";

        public override string WorkerHud_WorkType => "İş durumu: {0}";
        public override string WorkerHud_Carry => "Taşı: {0} {1}";
        public override string WorkerHud_Energy => "Enerji: {0}";
        public override string WorkerStatus_Exit => "İş gücünden ayrılıyor";
        public override string WorkerStatus_Eat => "Besleniyor";
        public override string WorkerStatus_Till => "Toprak işliyor";
        public override string WorkerStatus_Plant => "Bitki dikiyor";
        public override string WorkerStatus_Gather => "Topluyor";
        public override string WorkerStatus_PickUpResource => "Kaynak topluyor";
        public override string WorkerStatus_DropOff => "Teslim ediyor";
        public override string WorkerStatus_BuildX => "Inşa ediyor: {0}";
        public override string WorkerStatus_TrossReturnToArmy => "Orduya dön";

        public override string Hud_ToggleFollowFaction => "Taraf takibini aç/kapat";
        public override string Hud_FollowFaction_Yes => "Taraf genel ayarlarını kullanmaya ayarlı";
        public override string Hud_FollowFaction_No => "Yerel ayarları kullanıyor (Genel değer: {0})";

        public override string Hud_Idle => "Bekliyor";
        public override string Hud_NoLimit => "Sınır yok";

        public override string Hud_None => "Hiç";
        public override string Hud_ProductionQueue => "Üretim Kuyruğu";

        public override string Hud_EmptyList => "- Liste boş";

        public override string Hud_RequirementOr => "- ya da -";

        public override string Hud_BlackMarket => "Kara borsa";

        public override string Language_CollectProgress => "{0} / {1}";
        public override string Hud_SelectCity => "Şehir Seç";
        public override string Conscription_Title => "Askere alım";
        public override string Conscript_WeaponTitle => "Silah";
        public override string Conscript_ArmorTitle => "Zırh";
        public override string Conscript_TrainingTitle => "Eğitim";

        public override string Conscript_SpecializationTitle => "Uzmanlık";
        public override string Conscript_SpecializationDescription => "Tek bir bölge için saldırı gücünü arttırır, ancak diğer hepsi için düşürür {0}";
        public override string Conscript_SelectBuilding => "Kışla seç";

        public override string Conscript_WeaponDamage => "Silah hasarı";
        public override string Conscript_ArmorHealth => "Zırh Dayanıklılığı";
        public override string Conscript_AttackSpeed => "Saldırı hızı";
        public override string Conscript_TrainingTime => "Eğitim süresi";

        public override string Conscript_Training_Minimal => "Minimal";
        public override string Conscript_Training_Basic => "Temel";
        public override string Conscript_Training_Skillful => "Kıdemli";
        public override string Conscript_Training_Professional => "Profesyonel";

        public override string Conscript_Specialization_Field => "Meydan savaşı";
        public override string Conscript_Specialization_Sea => "Donanma";
        public override string Conscript_Specialization_Siege => "Kuşatma";
        public override string Conscript_Specialization_Traditional => "Geleneksel";
        public override string Conscript_Specialization_AntiCavalry => "Anti süvari";

        public override string Conscription_Status_CollectingEquipment => "Teçhizat toplanıyor: {0}";
        public override string Conscription_Status_CollectingMen => "Adam toplanıyor: {0}";
        public override string Conscription_Status_Training => "Eğitiliyor: {0}";

        public override string ArmyHud_Food_Reserves_X => "Gıda rezervi: {0}";
        public override string ArmyHud_Food_Upkeep_X => "Gıda tüketimi {0}";
        public override string ArmyHud_Food_Costs_X => "Gıda maliyeti {0}";

        public override string Deliver_WillSendXInfo => "Tek seferde {0} kadar gönderilecek";
        public override string Delivery_ListTitle => "Teslimat hizmetini seçin";
        public override string Delivery_DistanceX => "Mesafe: {0}";
        public override string Delivery_DeliveryTimeX => "Teslimat süresi: {0}";
        public override string Delivery_SenderMinimumCap => "Gönderen minimum sınırı";
        public override string Delivery_RecieverMaximumCap => "Alıcı maks sınırı";
        public override string Delivery_ItemsReady => "Eşyalar hazır";
        public override string Delivery_RecieverReady => "Alıcı hazır";
        public override string Hud_ThisCity => "Bu Şehir";
        public override string Hud_RecieveingCity => "Alıcı şehir";

        public override string Info_ButtonIcon => "i";

        public override string Info_ResourcePerSecond => "Saniye Başı Sergilenen Kaynak";

        public override string Info_MinuteAverage => "Bu değer, son bir dakikadaki ortalamanın sonucudur";

        public override string Message_OutOfFood_Title => "Gıda tükendi";
        public override string Message_CityOutOfFood_Text => "Pahalı gıdalar karaborsadan satın alınacak. Hazine tükendiğinde ise işçiler aç kalacak. ";

        public override string Hud_EndSessionIcon => "X";

        public override string TerrainType => "Zemin Türü";

        public override string Hud_EnergyUpkeepX => "{0} Enerji tüketimi";

        public override string Hud_EnergyAmount => "{0} enerji (kalan süre)";

        public override string Hud_CopySetup => "Düzeni kopyala";
        public override string Hud_Paste => "Yapıştır";

        public override string Hud_Available => "Müsait";

        public override string WorkForce_ChildBirthRequirements => "Doğum gereksinimleri";
        public override string WorkForce_AvailableHomes => "Müsait konaklar: {0} ";
        public override string WorkForce_Peace => "Refah";
        public override string WorkForce_ChildToManTime => "Büyüme süresi: {0} dakika";

        public override string Economy_TaxIncome => "Vergi geliri: {0}";
        public override string Economy_ImportCostsForResource => "{0} için ithalat maliyeti: {1} ";
        public override string Economy_BlackMarketCostsForResource => "{0} için karaborsa maliyeti: {1}";
        public override string Economy_GuardUpkeep => "Muhafız maliyeti: {0}";

        public override string Economy_LocalCityTrade_Export => "Şehir ticareti, ihracat: {0}";
        public override string Economy_LocalCityTrade_Import => "Şehir ticareti, ithalat {0}";

        public override string Economy_ResourceProduction => "{0} üretimi: {1}";
        public override string Economy_ResourceSpending => "{0} harcaması: {1}";

        public override string Economy_TaxDescription => "İşçi başına {0} alınan vergi";

        public override string Economy_SoldResources => "Satılan kaynaklar (altın cevheri): {0}";

        public override string UnitType_Cities => "Şehirler";
        public override string UnitType_Armies => "Ordular";
        public override string UnitType_Worker => "İşçiler";

        public override string UnitType_FootKnight => "Şövalye";
        public override string UnitType_CavalryKnight => "Süvari";

        public override string CityCulture_LargeFamilies => "Geniş aileler";
        public override string CityCulture_FertileGround => "Bereketli topraklar";
        public override string CityCulture_Archers => "Doğuştan nişancılar";
        public override string CityCulture_Warriors => "Savaşçılar";
        public override string CityCulture_AnimalBreeder => "Hayvan yetiştiricileri";
        public override string CityCulture_Miners => "Madenciler";
        public override string CityCulture_Woodcutters => "Keresteciler";
        public override string CityCulture_Builders => "İnşaatçılar";
        public override string CityCulture_CrabMentality => "Baskıcı";
        public override string CityCulture_DeepWell => "Bereketli su kaynakları";
        public override string CityCulture_Networker => "Sosyal kelebek";
        public override string CityCulture_PitMasters => "Kömürcü";

        public override string CityCulture_Culture => "Kültür";
        public override string CityCulture_LargeFamilies_Description => "Artan doğum oranları";
        public override string CityCulture_FertileGround_Description => "Ekinler daha fazla hasat verir";
        public override string CityCulture_Archers_Description => "Yetenekli okçular yetiştirilir";
        public override string CityCulture_Warriors_Description => "Yetenekli savaşçılar eğitilir";
        //public override string CityCulture_AnimalBreeder_Description => "Hayvanlar daha fazla ürün sağlar";
        public override string CityCulture_Miners_Description => "Madenden daha fazla cevher çıkarılır";
        public override string CityCulture_Woodcutters_Description => "Ağaçlardan daha fazla odun elde edilir";
        public override string CityCulture_Builders_Description => "Yapı inşaası daha hızlıdır";
        public override string CityCulture_CrabMentality_Description => "İş yapmak daha az enerji harcar fakat yetenekli asker yetiştirilemez";
        public override string CityCulture_DeepWell_Description => "Su deposu daha hızlı dolar";
        public override string CityCulture_Networker_Description => "Posta hizmeti daha verimlidir";
        public override string CityCulture_PitMasters_Description => "Yakıt üretimi daha fazladır";

        public override string CityOption_AutoBuild_Work => "İş gücünü otomatik genişlet";
        public override string CityOption_AutoBuild_Farm => "Çiftlikleri otomatik genişlet";

        public override string Hud_PurchaseTitle_Resources => "Kaynak satın al";
        public override string Hud_PurchaseTitle_CurrentlyOwn => "Sahipsin";

        public override string Tutorial_EndTutorial => "Öğreticiyi bitir";
        public override string Tutorial_MissionX => "Görev: {0}";
        public override string Tutorial_CollectXAmountOfY => "Topla {0} {1}";
        public override string Tutorial_SelectTabX => "Sekme seç: {0}";
        public override string Tutorial_IncreasePriorityOnX => "{0} için önceliği arttır";
        public override string Tutorial_PlaceBuildOrder => "{0} için inşa emri ver";
        public override string Tutorial_ZoomInput => "Yakınlaştır";

        public override string Tutorial_SelectACity => "Bir şehir seç";
        public override string Tutorial_ZoomInWorkers => "İşçileri görmek için yakınlaştır";
        public override string Tutorial_CreateSoldiers => "Şu ekipmanlar ile iki asker birimi oluştur: {0}. {1}.";
        public override string Tutorial_ZoomOutOverview => "Haritayı geniş olarak görmek için uzaklaştır";
        public override string Tutorial_ZoomOutDiplomacy => "Siyasi haritayı görmek için uzaklaştır";
        public override string Tutorial_ImproveRelations => "Komşu bir ülke ile ilişkini iyileştir";
        public override string Tutorial_MissionComplete_Title => "Görev Tamamlandı!";
        public override string Tutorial_MissionComplete_Unlocks => "Yeni yetkiler artık kullanılabilir";

        //patch1
        public override string Resource_ReachedStockpile => " Depo hedefine ulaşıldı";

        public override string BuildingType_ResourceMine => "{0} madeni";

        public override string Resource_TypeName_BogIron => "Bataklık demiri";

        public override string Resource_TypeName_Coal => "Kömür";

        public override string Language_XUpkeep => "{0} maaliyeti";
        public override string Language_XCountIsY => "{0} sayısı: {1}";

        public override string Message_ArmyOutOfFood_Text => "Yiyecekler karaborsadan pahalıya alınacak. Paran bittiğinde aç kalan askerler firar edecek.";

        public override string Info_ArmyFood1 => "Ordular, erzaklarını en yakın dost şehirden yeniler.";
        public override string Info_ArmyFood2 => "Diğer fraksiyonlardan erzak satın alınabilir.";
        public override string Info_ArmyFood3 => "Düşman bölgelerinde erzak yalnızca karaborsadan satın alınabilir.";
        public override string FactionName_Monger => "Monger";
        public override string FactionName_Hatu => "Hatu";
        public override string FactionName_Destru => "Destru";

        //patch2
        public override string Tutorial_BuildSomething => "{0} üreten bir yapı inşa et";
        public override string Tutorial_BuildCraft => "{0} için üretim alanı kur";
        public override string Tutorial_IncreaseBufferLimit => "{0} için depo sınırını arttır";

        /// <summary>
        /// 0: count, 1: item type
        /// </summary>
        public override string Tutorial_CollectItemStockpile => "{1} tane {0} depola";
        public override string Tutorial_LookAtFoodBlueprint => "Gıda şablonuna bak";
        public override string Tutorial_CollectFood_Info1 => "İşçiler yemek yemek için belediye binasına gidecek.";
        public override string Tutorial_CollectFood_Info2 => "Ordu, yiyecek toplamak için ikmal işçilerini gönderir.";
        public override string Tutorial_CollectFood_Info0 => "İşçileri tamamen kontrol etmek istiyorsan tüm iş önceliklerini sıfırla, sonra dilediklerini tek tek etkinleştir.";

        public override string EndGameStatistics_DecorsBuilt => "İnşa edilen dekorasyonlar: {0}";
        public override string EndGameStatistics_StatuesBuilt => "İnşa edilen heykeller: {0}";


        //############
        // XMAS UPDATE
        //############
        public override string Info_FoodAndDeliveryLocation => "Varsayılan olarak, işçiler yemek yemek veya eşya bırakmak için belediye binasına gider.";
        public override string GameMenu_UseSpeedX => "{0} hız seçenekleri";
        public override string GameMenu_LongerBuildQueue => "Uzatılmış inşa kuyruğu";

        public override string Diplomacy_RelationWithOthers => "Diğer taraflar ile ilişkileri";
        public override string Automation_queue_description => "Kuyruk bitene kadar sürekli tekrar eder";

        public override string BuildingType_Storehouse_Description => "İşçiler eşyaları buraya bırakabilir";
       
        public override string Resource_TypeName_Longbow => "uzun yay";
        public override string Resource_TypeName_Rapeseed => "kanola";
        public override string Resource_TypeName_Hemp => "enevir";

        public override string Resource_BogIronDescription => " Demir madenciliği, bataklık demiri kullanmaktan daha verimlidir.";


        public override string Resource_FoodSafeGuard_Description => "Gıda teminatı. Gıda miktarı {0} altına düşerse, gıda üretim zincirinin önceliği en üst düzeye çıkarılır.";
        public override string Resource_FoodSafeGuard_Active => "Gıda teminatı aktif";

        public override string GameMenu_NextSong => "Sonraki şarkı";

        public override string BuildingType_Bank => "Banka";
        public override string BuildingType_GoldDelivery_Description => "Diğer şehirlere sikke yolla";

        public override string BuildingType_Logistics => "Lojistik merkezi";
        public override string BuildingType_Logistics_Description => "Yapı inşa emri verme yeteneğini geliştirir";

        public override string BuildingType_Logistics_NationSizeRequirement => "Ülke çapı toplam iş gücü: {0} ";
        public override string Requirements_XItemStorageOfY => "{0} Şehrinin {1} stoğu";


        public override string XP_UnlockBuildQueue => "İnşa kuyruğu şuna yükseltildi: {0}";
        public override string XP_UnlockBuilding => "Yapıyı aç: ";
        public override string XP_Upgrade => "Geliştir";

        public override string XP_UpgradeBuildingX => "{0} yapısını geliştir";

        /// <summary>
        /// Title for describing the production cycle of farms
        /// </summary>
        public override string BuildHud_PerCycle => "Döngü başına";
        public override string BuildHud_MayCraft => "Üretebilir";
        public override string BuildHud_WorkTime => "Çalışma süresi: {0}";
        public override string BuildHud_GrowTime => "Büyüme süresi: {0}";
        public override string BuildHud_Produce => "Üretilen:";

        public override string BuildHud_Queue => " İzin verilen inşa kuyruğu: {0}/{1}";

        public override string LandType_Flatland => "Düz arazi";
        public override string LandType_Water => "Su";
        public override string BuildingType_Wall => "Sur";
        public override string Delivery_AutoReciever_Description => "Kaynaklar, stoğu en az olan şehre gönderilir.";

        public override string Hud_On => "Açık";
        public override string Hud_Off => "Kapalı";

        public override string Hud_Time_XSeconds => "{0} saniye";
        public override string Hud_Time_XMinutes => "{0} dakika";
        public override string Hud_Undo => "Geri al";
        public override string Hud_Redo => "Yinele";

        public override string Tag_ViewOnMap => "Etiketleri haritada göster";

        public override string MenuTab_Tag => "Etiket";

        public override string Input_Build => "İnşa et";

        public override string FlagEditor_ClearAll => "Hepsini temizle";


        public override string CityCulture_Stonemason => "Taş ustalığı";
        public override string CityCulture_Stonemason_Description => "Gelişmiş taş toplama";

        public override string CityCulture_Brewmaster => "Bira ustalığı";
        public override string CityCulture_Brewmaster_Description => "Bira üretimi artar";

        public override string CityCulture_Weavers => "Dokumacılık";
        public override string CityCulture_Weavers_Description => "Hafif zırh üretimi artar";

        public override string CityCulture_SiegeEngineer => "Kuşatma mühendisliği";
        public override string CityCulture_SiegeEngineer_Description => "Kuşatma silahları daha güçlü olur";

        public override string CityCulture_Armorsmith => "Zırh Ustalığı";
        public override string CityCulture_Armorsmith_Description => "Demir zırh üretimi artar";

        public override string CityCulture_Noblemen => "Asiller";
        public override string CityCulture_Noblemen_Description => "Şövalyeler daha güçlü olur";

        public override string CityCulture_Seafaring => "Denizcilik";
        public override string CityCulture_Seafaring_Description => "Denizcilik eğitimi alan askerlerin gemileri daha güçlü olur";

        public override string CityCulture_Backtrader => "Gizli Tüccarlar";
        public override string CityCulture_Backtrader_Description => "Kara borsa daha ucuzdur";

        public override string CityCulture_LawAbiding => "Örnek vatandaşlar";
        public override string CityCulture_LawAbiding_Description => "Vergi geliri artar, kara borsa yoktur";

        //##2##

        public override string Hud_Advanced => "Gelişmiş";
        public override string Hud_Loading => "Yükleniyor...";

        public override string CityOption_LowerGuardSize => "Muhafız sayısını azalt";
        public override string Hud_Purchase_MinCapacity => "Minimum kapasiteye ulaşıldı";
        public override string Settings_ResetToDefault => "Varsayılana geri dön";
        public override string Settings_NewGame => "Yeni oyun";

        public override string Settings_AdvancedGameSettings => "Gelişmiş Oyun Ayarları";
        public override string Settings_FoodMultiplier => "Gıda çarpanı";
        public override string Settings_FoodMultiplier_Description => "Bir işçi ya da asker, tok karnına ne kadar süre dayanır. Yüksek bir değer bilgisayar performansını düşürebilir.";

        public override string Settings_GameMode => "Oyun modu";

        public override string Settings_Mode_Story => "Hikaye modu";
        public override string Settings_Mode_IncludeBoss => "Baş düşman etkinliğini aktif et";
        public override string Settings_Mode_IncludeAttacks => "Rastgele saldırıları aktif et";
        public override string Settings_Mode_Sandbox => "Serbest mod";
        public override string Settings_Mode_Peaceful => "Barışçıl";
        public override string Settings_Mode_Peaceful_Description => "Bütün savaşlar oyuncu tarafından başlatılır";

        public override string Lobby_ImportSave => "Kaydı içe aktar";

        public override string Lobby_ExportSave => "Kaydı dışa aktar";
        public override string Lobby_ExportSave_Description => "Dosyanın bir kopyasını oluşturur ve içe aktarma klasörüne yerleştirir: {0}";

        public override string Resource_CurrentAmount => "Mevcut miktar: {0}";
        public override string Resource_MaxAmount_Soft => "Alt Sınır (Maks Limit): {0}";
        public override string Resource_MaxAmount => "Maks Limit: {0}";
        public override string Resource_AddPerSec => "Artış Hızı: saniyede {0}";

        public override string Resource_WaterAddLimit => "Su artış hızı değiştirilemez.";

        public override string Tutorial_Select_SubTab => "Ardından {0} kategorisini seçin";



        /* #### --------------- ##### */
        /* #### DSS 2 DEMO      ##### */
        /* #### --------------- ##### */


        public override string Tutorial_OpenGuardSubTab => "Bir kışla seç ve kategori belirle: {0}";
        public override string Tutorial_GuardToWall => "Muhafızı sura gönder";
        public override string Demo_MissionObjective_Title => "Görev Hedefi";
        public override string Demo_MissionObjective_Description => "Güneyden gelen saldırıya karşı savunma yap";
        public override string Demo_Complete_Title => "Deneme sürümü tamamlandı";
        public override string Demo_TimesUp_Title => "Süren Doldu!";
        public override string Demo_EndInOneMinuteDescription => "Deneme sürümü bir dakika içinde sona erecek";

        public override string ArmyOption_NewArmy => "Yeni ordu";
        public override string ProfileEditor_AltMain => "Alternatif ana profil";
        public override string Automation_CheckBoxTitle => "Otomatikleştirildi";

        public override string ArmyStructure_ColumnWidth => "Ordu sütun genişliği";
        public override string ArmyStructure_ArmyPlacement => "Ordu içi yerleşim";
        public override string ArmyStructure_Row_Front => "Ön hat";
        public override string ArmyStructure_Row_Body => "Orta hat";
        public override string ArmyStructure_Row_Second => "İkinci hat";
        public override string ArmyStructure_Row_Behind => "Arka hat";

        public override string Diplomacy_RelationType_Enemies => "Düşman";

        public override string EventMessage_EnemyAlliance_Title => "Hakimiyet Korkusu";
        public override string EventMessage_EnemyAlliance => "Uluslar, artan gücünden korkarak sana karşı bir ittifak kurdu.";

        public override string Settings_CentralGold => "Merkezi altın";
        public override string Settings_CentralGold_Description => "Açık: Tüm altınlar anında kullanılabilen ortak bir havuzda toplanır. Kapalı: Altın fiziksel bir kaynaktır ve taşınması gerekir.";





        public override string InputActionName_StopStart => "Durdur/Başlat";
        public override string InputActionName_ToggleHudDetail => "Arayüz Detayını Aç/Kapat";
        public override string InputActionName_NextCity => "Sonraki Şehir";
        public override string InputActionName_NextArmy => "Sonraki Ordu";
        public override string InputActionName_NextBattle => "Sonraki Muharebe";
        public override string InputActionName_Build => "İnşa et";
        public override string InputActionName_Copy => "Kopyala";
        public override string InputActionName_Paste => "Yapıştır";
        public override string InputActionName_Menu => "Menü";
        public override string InputActionName_FlagDesign_ToggleColor_Prev => "Önceki Renk";
        public override string InputActionName_FlagDesign_ToggleColor_Next => "Sonraki Renk";
        public override string InputActionName_FlagDesign_PaintBucket => "Boya Kovası";
        public override string InputActionName_Controller_FlagDesign_Colorpicker => "Renk Seçici";
        public override string InputActionName_ControllerFocus => "Kontrolcü Odağı";
        public override string InputActionName_ControllerCancel => "İptal";
        public override string InputActionName_ControllerMessageClick => "Mesajı Aç";
        public override string InputActionName_ControllerSelect => "Seç";
        public override string InputActionName_WASD_UP => "Yukarı";
        public override string InputActionName_WASD_DOWN => "Aşağı";
        public override string InputActionName_WASD_LEFT => "Sol";
        public override string InputActionName_WASD_RIGHT => "Sağ";
        public override string InputActionName_CameraTiltLeft => "Kamerayı Sola Eğ";
        public override string InputActionName_CameraTiltRight => "Kamerayı Sağa Eğ";
        public override string InputActionName_CameraTiltUp => "Kamerayı Yukarı Eğ";
        public override string InputActionName_ZoomInKey => "Yakınlaştır";
        public override string InputActionName_ZoomOutKey => "Uzaklaştır";




        public override string Settings_Title_Monitor => "Ekran Ayarları";
        public override string Settings_Title_Graphics => "Grafik Ayarları";
        public override string Settings_Title_Input => "Girdi";
        public override string Settings_Title_Gameplay => "Oynanış Ayarları";
        public override string Settings_PanOnZoom => "Yakınlaştırırken kaydır";
        public override string Settings_ScrollSensitivity_Game => "Oyun içi kaydırma hassasiyeti";
        public override string Settings_ScrollSensitivity_Menu => "Menü kaydırma hassasiyeti";
        public override string Settings_Blood => "Kan";

        public override string Settings_MasterVolume => "Genel Ses";
        public override string Settings_AmbienceVolume => "Ortam Sesleri";
        public override string Settings_BattleMelody => "Savaş Ezgileri";

        public override string Settings_ModelLight => "Model Üzerindeki ışık Efekti";
        public override string Settings_Particles => "Parçacık efektleri";
        public override string Settings_MapLoadSpeed => "Harita yüklenme hızı";
        public override string Lobby_Category_Options => "Seçenekler";
        public override string Lobby_Category_Editor => "Editör";
        public override string Lobby_Category_ExtraModes => "Ek modlar";

        public override string Lobby_Editor_MapEditor => "Harita düzenleyici";
        public override string Lobby_Editor_VoxelEditor => "Voksel düzenleyici";

        public override string Lobby_Mode_BattleLab => "Muharebe Test Alanı";
        public override string Lobby_Mode_BattleLab_Description => "İstediğin askerleri birbirine karşı dövüştür.";
        public override string Lobby_Mode_Commander => "Komutan Modu";
        public override string Lobby_Mode_Commander_Description => "Küçük, taktiksel masa oyunu.";
        public override string Lobby_MusicPlayList => "Çalma Listesi";

        public override string Lobby_GameSetup => "Oyun Tercihleri";
        public override string Lobby_PlayerSetup => "Oyuncu Tercihleri";
        public override string LobbyDemoMode_Demo => "Deneme sürümü";

        public override string Lobby_Tutorial => "Tutorial Öğretici";

        public override string LobbyDemoMode_ShortTutorial => "Hızlı Öğretici";
        public override string LobbyDemoMode_LongTutorial => "Detaylı Öğretici";

        /// <summary>
        /// Says wishlist on, followed by the STEAM logo
        /// </summary>
        public override string LobbyDemoMode_WishlistOn => "İstek listenize ekleyin!!";


        public override string BattleLab_StartHere => "Muharebeyi burada başlat";
        public override string BattleLab_Start => "Muharebeye başla";
        public override string BattleLab_Attacker => "Saldıran";



        public override string MapGenerator_Name => "Harita düzenleyici - oluştur";

        public override string MapType_CustomMap => "Özel Harita";
        public override string MapType_GenerateNewMap => "Yeni harita oluştur";
        public override string MapGenerator_GenerateAction => "Oluştur";
        public override string MapGenerator_Terrain_CustomSize => "Özel Boyut";
        public override string MapGenerator_Terrain_StartAs => "Başlangıç Türü";
        public override string MapGenerator_Terrain_ClearPass => "Temizleme aşamasını çalıştır";
        public override string MapGenerator_Terrain_BuildPass => "İnşa aşamasını çalıştır";
        public override string MapGenerator_Terrain_DigPass => "Kazma aşamasını çalıştır";
        public override string MapGenerator_Terrain_BuildDigLoops => "İnşa-Kazı döngü sayısı";
        public override string MapGenerator_Terrain_BuildStrokes => "İnşa fırça sayısı";
        public override string MapGenerator_Terrain_BuildStrokes_Description => "Her 100 karoda kullanılan fırça darbesinin sayısıdır.";
        public override string MapGenerator_Terrain_DigStrokes => "Kazı fırça sayısı";
        public override string MapGenerator_Terrain_CleanUp_Option => "Tekil karoların temizliği";
        public override string MapGenerator_Terrain_CleanUpPass => "Temizlik aşamasını çalıştır";



        public override string Economy_ServicemenUpkeep => "Hizmetli masrafı: {0}";
        public override string Economy_ServicemenUpkeep_Description => "Hizmetli başına altın gideri: {0}";
        public override string Economy_GuardUpkeep_Description => "Muhafız başına altın gideri: {0}";

        public override string EndScreen_TimeHasEndedTitle => "Zaman Doldu";

        public override string Hud_AdvancedSettings => "Gelişmiş Ayarlar";
        public override string Hud_Vector_X => "X";
        public override string Hud_Vector_Y => "Y";
        public override string Hud_Cancel => "İptal";
        public override string Hud_Delete => "Sil";
        public override string Hud_Next => "Sonraki";
        //public override string Hud_None => "None";
        public override string Hud_Apply => "AUygula";
        public override string Hud_AllCities => "ABütün Şehirler";
        public override string Hud_Time_Hours => "{0} saat";
        public override string Hud_AddX => "Ekle: {0}";
        public override string Hud_Both => "Her ikisi de";
        public override string Hud_Direction => "Yön";
        
        /// <summary>
        /// 0: object collection type name, 1: number of objects
        /// </summary>
        public override string Hud_ObjectsAndCount => "{0}, adeti: {1}";

        public override string Hud_EffectDoesNotStack => "Bu etki üst üste kullanılamaz.";

        public override string Work_SmeltX => "Erit {0}";

        public override string Info_TotalFoodProduction => "Toplam gıda üretimi";
        public override string Info_TotalFoodSpending => "Toplam gıda harcaması";

        public override string Info_FooodAndDeliveryLocation => "Varsayılan olarak, işçiler yemek yemek veya eşya bırakmak için belediye binasına gider.";

        public override string Delivery_SendChunk => "Teslimat başına eşya sayısı";
        public override string Delivery_SpeedBonus => "Hız Bonusu: ½{0} ";

        public override string Delivery_AutoResourceDescription => "Stok sınırına ulaşan eşyalar, ihtiyaç duyan şehirlere otomatik olarak gönderilir.";

        public override string Conscript_Soldiers_ArmyType => "Askerler";
        public override string Conscript_Soldiers_ArmyType_Description => "Yakındaki bir orduya asker topla.";
        public override string Conscript_Soldiers_GuardType => "Şehir muhafızları";
        public override string Conscript_Soldiers_GuardType_Description => "Muhafızlar surları takviye etmek için kullanılır";
        //-
        public override string Defence_Title => "Savunma";
        public override string Defence_GuardPost => "Muhafız karakolu";

        public override string Defence_WallDescription_Movement => "Düşman hareketini engeller.";
        public override string Defence_WallDescription_GuardPost => "Buraya muhafız yerleştirilebilir.";
        public override string Defence_AutoAssign => "Otomatik atama";
        public override string Defence_AutoAssign_Description => "Yeni muhafızlar bu noktaya otomatik olarak atanır.";
        public override string Conscript_SplashDamage => "Alan hasarı";
        public override string Conscript_HighSplashDamage => "Yüksek alan hasarı";

        public override string Conscript_Training_Champion => "Şampiyon";
        public override string Conscript_Training_Legendary => "Efsanevi";


        public override string Experience_Title => "Deneyim";
        public override string Experience_TopExperience => "En yüksek deneyim seviyeleri";

        public override string Experience_TimeReductionDescription => "Her seviye başına çalışma süresi ½{0} azalır ";

        public override string ExperienceType_Farm => "Çiftçilik";
        public override string ExperienceType_AnimalCare => "Hayvancılık";
        public override string ExperienceType_HouseBuilding => "İnşaat";
        public override string ExperienceType_WoodWork => "Odunculuk";
        public override string ExperienceType_StoneCutter => "Taş işçiliği";
        public override string ExperienceType_Mining => "Madencilik";
        public override string ExperienceType_Transport => "Nakliyat";
        public override string ExperienceType_Cook => "Aşçılık";
        public override string ExperienceType_Fletcher => "Ok üretimi";
        public override string ExperienceType_RefineOre => "Cevher dökümcülüğü";
        public override string ExperienceType_Casting => "Dökümcülük";
        public override string ExperienceType_CraftMetal => "Demircilik";
        public override string ExperienceType_CraftArmor => "Zırh üretimi";
        public override string ExperienceType_CraftWeapon => "Silah üretimi";
        public override string ExperienceType_CraftFuel => "Yakıt üretimi";
        public override string ExperienceType_Chemist => "Simyacılık";

        public override string ExperienceLevel_1 => "Çırak";
        public override string ExperienceLevel_2 => "Pratisyen";
        public override string ExperienceLevel_3 => "Uzman";
        public override string ExperienceLevel_4 => "Usta";
        public override string ExperienceLevel_5 => "Efsane";

        public override string ExperenceOrDistancePrio_Title => "İşçi seçimi";
        public override string ExperenceOrDistancePrio_Description => "Boştaki işçiler, uzaklığa veya deneyime göre görevlendirilir.";


        public override string Technology_Description => "Her şehrin bir teknoloji ağacı vardır. Her teknoloji, yeni binalar ve eşyaların kilidini açar.";
        public override string Experience_Description => "İşçiler deneyim kazanır ve gelişir";


        public override string Technology_Title => "Teknoloji";
        public override string Technology_ShareField => "Paylaşılan teknoloji alanı";

        public override string Technology_GainByNeigborRelation => "{0} Teknolojiye sahip her komşu şehir için, ilişkiniz";
        public override string Technology_ForEachMaster => "Bir {0}, {2} teknoloji alanında {1} deneyim seviyesine ulaştığında";
        public override string Technology_CitySpread => "Şehirleriniz bitişik olduğunda teknoloji paylaşır: {0}";
        public override string Technology_CityCapture => "Bir şehir savaşta ele geçirildiğinde teknolojilerin çoğu yok edilir.";

        public override string Technology_AdvancedBuildings => "Gelişmiş Yapılar";
        public override string Technology_AdvancedFarming => "Gelişmiş çiftçilik";
        public override string Technology_AdvancedCasting => "Gelişmiş Dökümcülük";

        public override string Help_Title => "Yardım";
        public override string Help_Work_Title => "Çalışma Başlamıyor";
        public override string Help_Work_Resources => "Binaların çalışması için gerekli kaynaklar hazır olmalıdır.";
        public override string Help_Work_Skill => "İşçinin gerekli (veya daha yüksek) beceri seviyesine sahip olması gerekir.";
        public override string Help_Work_Stockpile => "Depo doluysa daha fazla kaynak orada stoklanamaz.";
        public override string Help_Work_Priority => "İşin önceliği düşük ya da sıfır olabilir.";


        public override string Help_Soldiers_Title => "Asker üret";
        public override string Help_Soldiers_PlaceBuildingX => "Şu yapıyı yerleştir: {0}";
        public override string Help_Soldiers_Workers => "Askere alınabilecek uygun işçiler";
        public override string Help_Soldiers_Weapon => "Her asker için bir silah gerekir";
        public override string Help_Soldiers_StartX => "Başla: {0}";


        public override string Hud_SelectHistory => "Geçmişi Seç";

        public override string Hud_PointsPerMinute => "Dakika başı {0} puan";
        public override string Hud_PercentValueCost => "Hizmet bedeli %{0} idir";

        public override string Hud_Mixed => "Karışık";
        public override string Hud_Distance => "Mesafe";

        public override string Hud_Unlock => "Kilidini aç";
        public override string Hud_category => "Kategori";

        /// <summary>
        /// Sets the game speed to one frame at a time
        /// </summary>
        public override string Input_StepOneFrame => "1 kare ekle";

        public override string Resource_TypeName_Wagon2Wheel => "El arabası";
        public override string Resource_TypeName_Wagon4Wheel => "Vagon";
        public override string Resource_TypeName_Tin => "Kalay";
        public override string Resource_TypeName_TinOre => "Kalay cevheri";

        public override string Resource_TypeName_Copper => "Bakır";
        public override string Resource_TypeName_CopperOre => "Bakır cevheri";
        public override string Resource_TypeName_SilverOre => "Gümüş cevheri";
        public override string Resource_TypeName_Silver => "Gümüş";

        /// <summary>
        /// Mithril is a fantasy metal
        /// </summary>
        public override string Resource_TypeName_RawMithril => "Mitril cehveri";
        public override string Resource_TypeName_Mithril => "Mitril";

        public override string Resource_TypeName_BronzeSword => "Bronz kılıç";
        public override string Resource_TypeName_ShortSword => "Kısa kılıç";
        public override string Resource_TypeName_LongSword => "Uzun kılıç";
        public override string Resource_TypeName_HandSpear => "Kısa mızrak";
        public override string Resource_TypeName_Warhammer => "Savaş çekici";
        public override string Resource_TypeName_MithrilSword => "Mitril kılıç";
        public override string Resource_TypeName_SlingShot => "Sapan";
        public override string Resource_TypeName_ThrowingSpear => "Cirit";
        public override string Resource_TypeName_Crossbow => "Arbalet";
        public override string Resource_TypeName_MithrilBow => "Mitril Yay";

        public override string Resource_TypeName_CoolingFluid => "Soğutucu sıvı";
        public override string Resource_TypeName_Palisade => "Kazıklı sur";
        public override string Resource_TypeName_Toolkit => "Alet çantası";

        public override string Resource_TypeName_Sulfur => "Kükürt";
        public override string Resource_TypeName_LeadOre => "Kurşun cevheri";
        public override string Resource_TypeName_Lead => "Kurşun";
        public override string Resource_TypeName_Bronze => "Bronz";
        public override string Resource_TypeName_BloomIron => "Ocak demiri";
        public override string Resource_TypeName_Steel => "Çelik";
        public override string Resource_TypeName_CastIron => "Döküm demir";

        public override string Resource_TypeName_BlackPowder => "Kara barut";
        public override string Resource_TypeName_GunPowder => "Barut";
        public override string Resource_TypeName_LedBullet => "Mermi";

        public override string Resource_TypeName_HandCannon => "İlkel tüfek";
        public override string Resource_TypeName_HandCulverin => "Mini top";
        public override string Resource_TypeName_Rifle => "Tüfek";
        public override string Resource_TypeName_Blunderbuss => "Alaybozan";

        public override string Resource_TypeName_Manuballista => "Manubalista";
        public override string Resource_TypeName_Catapult => "Mancınık";
        public override string Resource_TypeName_BatteringRam => "Koçbaşı";
        public override string Resource_TypeName_SiegeCannonBronze => "Bazilika";
        public override string Resource_TypeName_ManCannonBronze => "Bombardıman topu";
        public override string Resource_TypeName_SiegeCannonIron => "Öbüs";
        public override string Resource_TypeName_ManCannonIron => "Top";

        public override string Resource_TypeName_PaddedArmor => "Kumaş zırh";
        public override string Resource_TypeName_HeavyPaddedArmor => "Ağır kumaş zırh";

        public override string Resource_TypeName_IronArmor => "Zincir zırh";
        public override string Resource_TypeName_HeavyIronArmor => "Ağır zincir zırh";

        public override string Resource_TypeName_BronzeArmor => "Bronz zırh";

        public override string Resource_TypeName_LightPlateArmor => "Plaka zırh";
        public override string Resource_TypeName_FullPlateArmor => "Ağır plaka zırh";
        public override string Resource_TypeName_MithrilArmor => "Mitril zırh";
        public override string Resource_TypeName_Coin => "Sikke";

        public override string UnitType_Warhammer => "Çekiçli şövalye";
        //public override string UnitType_MithrilKnight => "Ebedi şövalye";
        //public override string UnitType_MithrilArcher => "Ebedi okçu";
        public override string UnitType_SpearAndShield => "Hat askeri";

        public override string UnitType_CollectionOfSoldiers => "Asker destesi";
        public override string UnitType_CollectionOfArmies => "Ordu destesi";

        /// <summary>
        /// The id tag will be a unique number
        /// </summary>
        public override string UnitId => "(id {0})";

        public override string BuildHud_AreaEffectTitle => "Alan etkisi";
        public override string BuildHud_BonusRadius => "Bonus etki alanı: {0}";

        public override string BuildHud_BuildTime => "İnşa süresi";
        public override string SchoolHud_ToLevel => "Seviye atlamaya";
        public override string SchoolHud_TimeDescription => "Deneyimsiz başlangıç baz alınmıştır; deneyimle birkilte süre de azalır.";
        public override string SchoolHud_SelectSchool => "Okul seç";
        public override string Upgrade_Order => "Yükseltme sırası";

        public override string Building_ListDescription => "Bu kategorideki bütün yapıların listesi";

        public override string BuildingType_IsUpgraded => "{0} - yükseltildi";
        public override string BuildingType_WoodCutter => "Kereste atölyesi";
        public override string BuildingType_Workshop_Description => "Çevredeki çalışmaları iyileştirir";

        public override string BuildingType_WoodCutter_AreaAffect => "Ağaçlardan ½{0} daha fazla odun elde edilir";

        public override string BuildingType_StoneCutter_AreaAffect => "Kayalardan ½{0} daha fazla taş elde edilir";

        public override string BuildingType_StoneCutter => "Taş ocağı";

        public override string BuildingType_Embassy => "Büyükelçilik";
        public override string BuildingType_Embassy_Description => "Diplomatik ilişkiler için kullanılır";

        public override string BuildingType_SoldierBarracks => "Asker kışlası";
        public override string BuildingType_ArcherBarracks => "Okçu kışlası";
        public override string BuildingType_WarmachineBarracks => "Kuşatma silahı atölyesi";
        public override string BuildingType_GunBarracks => "Tüfekçi kışlası";
        public override string BuildingType_CannonBarracks => "Topçu kışlası";
        public override string BuildingType_KnightsBarracks => "Şövalye kışlası";

        public override string BuildingType_WaterResovoir => "Su deposu";
        public override string BuildingType_WaterResovoir_Description => "Daha fazla su depolanmasını sağlar";

        public override string BuildingType_SmeltingFurnace => "Eritme fırını";
        public override string BuildingType_SmeltingFurnace_Description => "Cevheri işleyip metale dönüştürür";

        public override string BuildingType_Foundry => "Dökümhane";
        public override string BuildingType_Foundry_Description => "Metal döküm istasyonu";

        public override string BuildingType_Armory => "Zırh atölyesi";
        public override string BuildingType_Armory_Description => "Zırh üretim istasyonu";
        public override string BuildingType_Chemist => "Kimya atölyesi";
        public override string BuildingType_Chemist_Description => "Kimyasal üretim istasyonu";
        public override string BuildingType_CoinMaker => "Darphane";
        public override string BuildingType_CoinMaker_Description => "Metali sikkeye dönüştürür";
        public override string BuildingType_Gunmaker => "Silah atölyesi";
        public override string BuildingType_Gunmaker_Description => "Tüfek ve top üretim istasyonu";

        public override string BuildingType_School_Tab => "Okul";
        public override string BuildingType_School => "Ustalar loncası";
        public override string BuildingType_School_Description => "İşçilerin beceri seviyesini artırır";

        public override string BuildingType_GoldDelivery => "Altın kuryesi";
        public override string BuildingType_Bank_Description => "Altın yönetimi";

        public override string DecorType_CobbleStones => "Taş döşeme";
        public override string DecorType_Square => "Şehir meydanı";

        public override string DecorType_Garden => "Bahçe";
        public override string DecorType_Flag => "Bayrak";
        public override string DecorType_Banner => "Sancak";

        public override string BuildingType_DirtRoad => "Toprak yol";
        public override string BuildingType_Palisade => "Kazıklı sur karakolu";

        public override string ResourceType_ServiceMen => "Hizmetliler";
        public override string BuildingType_ServiceHouse => "Hizmetli evi";
        public override string BuildingType_ServiceHouse_DescriptionAddX => "{0} hizmetli ekler";

        public override string BuildingType_GuardOffice => "Muhafız ofisi";
        public override string BuildingType_GuardOffice_DescriptionAddX => "Muhafız limitini şu kadar artırır: {0}";

        public override string BuildingType_DirtWall => "Kerpiç sur";
        public override string BuildingType_DirtTower => "Kerpiç kule";
        public override string BuildingType_WoodWall => "Ahşap sur";
        public override string BuildingType_WoodTower => "Ahşap kule";
        public override string BuildingType_StoneWall => "Taş sur";
        public override string BuildingType_StoneTower => "Taş kule";
        public override string BuildingType_StoneGate => "Taş geçit";
        public override string BuildingType_StoneHouse => "Taş geçit";


        /// <summary>
        /// When listing slight variations, like "Lamp A" and "Lamp B"
        /// </summary>
        public override string VariantType_A => "{0} A";
        public override string VariantType_B => "{0} B";
        public override string VariantType_C => "{0} C";
        public override string VariantType_D => "{0} D";
        public override string VariantType_E => "{0} E";
        public override string VariantType_F => "{0} F";
        public override string VariantType_G => "{0} G";
        public override string VariantType_H => "{0} H";

        public override string BuildingToolShape_Free => "Kalem";
        public override string BuildingToolShape_Area => "Dikdörtgen";
        public override string BuildingToolShape_Line => "Çizgi";
        public override string BuildingToolShape_LShape => "L-şekli";


        public override string CityHall_Upgrade => "Belediye binasını yükselt";

        /// <summary>
        /// A cap on how many workers the city can have
        /// </summary>
        public override string CityHall_MaxSupportedWorkers => "Maks işçi kapasitesi: {0}";

        public override string CityHall_Size_Small => "Köy";
        public override string CityHall_Size_Medium => "Kasaba";
        public override string CityHall_Size_Large => "Başkent";

        public override string GuardHousingCount => "Muhafız lojmanları";
        public override string ServicemenCount => "Hizmetliler: {0}";


        public override string Work_MiningResource => "Kazılan: {0}";

        public override string MenuTab_Progress => "İlerleme";

        public override string Automation_AutomateCity => "Şehri otomatik yönet";
        public override string Automation_AutomationFocus => "Otomatik mod odağı";
        public override string Automation_AutomationFocus_Grow => "Büyüt";
        public override string Automation_AutomationFocus_Export => "İhracat";
        public override string Automation_AutomationFocus_War => "Savaş";

        public override string CityCulture_Smelters_Description => "Daha iyi cevher eritme";
        public override string CityCulture_Smelters => "Eritme ustalığı";

        public override string CityCulture_Apprentices_Description => "Yeni işçiler, ustaların yanında deneyim kazanır";
        public override string CityCulture_Apprentices => "Çıraklık kültürü";

        public override string CityCulture_BronzeCasters_Description => "Bronz ve bronz eşya üretiminde verimi arttırır";
        public override string CityCulture_BronzeCasters => "Bronz ustalığı";

        //DEMO PATCH 1

        /// <summary>
        /// Evil orcs that roam on the map
        /// </summary>
        public override string FactionName_Barbarian => "Yağmacılar";
        public override string Tutorial_AttackAndDestroyX => "Saldır ve yok et: {0}";
        public override string Resource_TypeName_Pike => "Kargı";


        public override string BattleTrials_Title => "Muharebe deneme alanı";
        public override string BattleTrials_Description => "Taktiklerini, bu orduya-karşı-ordu karşılaşmasında test et";


        //DEMO PATCH 2
        public override string Conscript_BlockReducingAttack => "Bu saldırılar bloklama şansını azaltır";

        public override string Conscript_BlockPerSecond => "Saniyede {0} blok yapabilir";

        public override string Conscript_BlockDescription => "Askerler, ön taraftan gelen saldırıların çoğunu bloklar";

        public override string Map_CustomSeed => "Harita adı";

        public override string Settings_Mode_Spectator => "İzleyici modu";

        //public override string Settings_Mode_Spectator_Description => "Arkana yaslan ve izle";

        public override string Automation_AutomationFocus_NoFocus_Description => "Her şeyden biraz inşa eder";

        public override string Automation_AutomationFocus_WillProduce => "Ağırlıklı olarak şunları üretir:";

        public override string Help_Food_WhoEats => "Tüm askerler ve işçiler yemek tüketir";

        public override string Help_Food_BigArmy => "Kalabalık bir ordu, bulunduğu bölgedeki şehri aç bırakabilir";

        public override string Help_Food_DontBuild => "Yiyecek üretimi için sadece çiftlik yetmez; işçiler ve gıda üretim yapıları da gerekir.";

        public override string Help_Food_UseWater => "Gıda üretimi su gerektirir";

        public override string Help_Food_Postal => "Şehirler arası yiyecek desteği sağlamayı unutma";

        public override string Message_LostCity => "Şehir kaybedildi";

        public override string Demo_Description => "Örnek savunma senaryosu: Şehrini {0} dakika savun";


        //DEMO PATCH 3
        public override string Demo_EndInXMinuteDescription => "Deneme sürümü {0} dakika içerisinde sona erecek";

        public override string Experience_Required => "Gerekli deneyim";

        public override string InputActionName_ToggleMenu => "Menüyü aç/kapa";

        //DEMO PATCH 4
        public override string Work_BadValueDescription => "Kaynaklar sıfırın altına düşebilir veya stok sınırını bir miktar aşabilir. Bu sınırlar yalnızca iş kuyruğu oluşturulurken uygulanır.";

        public override string Work_SelectCategory => "Eşya kategorisi seç";
        public override string Hud_RemoveFromList => "Listeden çıkar";

        public override string Hud_ReturnToPrevious => "Geri al";
        public override string Hud_Close => "Kapat";

        public override string Hud_Low => "Düşük";
        public override string Hud_Medium => "Orta";
        public override string Hud_High => "Yüksek";

        public override string Hud_Copy => "Kopyala";
        //public override string Hud_Paste => "Yapıştır";
        public override string Hud_Cut => "Kes";
        public override string Hud_SaveCompleted => "Kayıt Başarılı";

        public override string Settings_WaterMultiplier => "Su Çarpanı";
        public override string Settings_WaterMultiplier_Description => "Su üretim ve depolama miktarını belirler. Yüksek değerler bilgisayar performansı etkileyebilir.";

        public override string Settings_ChildMultiplier => "Doğurganlık Çarpanı";
        //public override string Settings_CraftMultiplier => "Üretim Hızı Çarpanı";
        public override string Settings_CraftMultiplier_Description => "Daha düşük değerler, daha hızlı üretim sağlar.";

        public override string FastProduction => "Hızlı Üretim";
        public override string SlowProduction => "Yavaş Üretim";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public override string BlocksProduction => "Üretim Yapılmayacak";

        //public override string CityAutomation_WaitForMaxPopulation => "Wait for population to max out";
        public override string Automation_AutomationFocus_NoFocus => "Tümü";
        public override string CityAutomation_SoldierQuality => "Asker Kalifiyeliği";
        public override string CityAutomation_SoldierWeaponType => "Silah Türü";

        public override string WarsResourceGroup_Resources => "Kaynaklar";
        public override string WarsResourceGroup_Weapons => "Silahlar";

        public override string WarsResourceGroup_AllWeaponTypes => "Çeşitli";
        public override string WarsResourceGroup_MeleeHandWeapons => "Yakın dövüş";
        public override string WarsResourceGroup_RangedHandWeapons => "Menzilli";
        public override string WarsResourceGroup_Warmachines => "Kuşatma silahları";

        public override string FactionSettings_Titel => "Taraf Genel Ayarları";
        public override string FactionSettings_Description => "Tüm şehirleriniz için geçerlidir";

        public override string Conscript_MaxPopulation => "Maks nüfus";
        public override string Conscript_MaxPopulation_Description => "Yalnızca nüfus maksimum seviyedeyken askere alım yapılır";

        public override string Conscript_FoodAbundance => "Maks gıda stoğu";
        public override string Conscript_FoodAbundance_Description => "Yalnızca gıda stoğu maksimuma ulaştığında asker alınır";

        /// <summary>
        /// General settings will go through all items in a list and apply to all of them (to their checkbox)
        /// </summary>
        public override string GeneralSetting_On => "Durum: Açık";
        public override string GeneralSetting_Off => "Durum: Kapalı";
        public override string GeneralSetting_AllBuildingsDescription => "Tüm yapılar için geçerlidir";

        public override string GeneralSetting_ApplyMessage => "{0} yapıya uygulandı";

        public override string MustTurnOffSteamInput => "Kontrolcü kullanmak için Steam Girdi'sini kapatmanız gerekir.";

        public override string Technology_GainTitle => "Teknoloji edinme yolları";
        public override string Technology_LevelUp => "Seviye atla";
        public override string Technology_ForEachLevelUp => "Bir işçi, teknoloji alanında seviye atladığında: {0}";

        public override string VoxelEditor_Description => "Bloğumsu modeller oluştur";

        public override string Editor_Tool => "Araç";
        public override string Editor_SelectOptionsMenu => "Seçimler";
        public override string Editor_Continous => "Sürekli"; // corrected spelling
        public override string Editor_Tool_PencilSize => "Fırça Boyutu";
        public override string Editor_Tool_SizeTolerance => "Boyut Toleransı";
        public override string Editor_Tool_RoundPencil => "Yuvarlak Fırça";
        public override string Editor_Tool_EdgeSize => "Kenar Boyutu";
        public override string Editor_Tool_PercentFill => "Doluluk Oranı";
        public override string Editor_Tool_ClearAbove => "Üstü Temizle";
        public override string Editor_Tool_FillBelow => "Altı Doldur";
        public override string Editor_UserModels => "Kullanıcı Modelleri";
        public override string Editor_UserModels_Description => "Kaydettiğiniz modelleri görüntüleyin";

        public override string Editor_RetailModels => "Hazır Modeller";
        public override string Editor_RetailModels_Description => "Oyundaki hazır modelleri yükle";

        public override string Editor_ModTemplates => "Modlama Şablonları";
        public override string Editor_ExportAsOBJ => ".OBJ olarak dışa aktar";
        public override string Editor_SelectAll => "Tümünü Seç";

        public override string Editor_Canvas_Title => "Tuval";
        public override string Editor_Canvas_Size => "Boyut";
        public override string Editor_Canvas_Dimension_X => "X";
        public override string Editor_Canvas_Dimension_Y => "Y";
        public override string Editor_Canvas_Dimension_Z => "Z";
        public override string Editor_Canvas_SizePresets => "Hazır Boyutlar";
        public override string Editor_Canvas_Move => "Taşı";
        public override string Editor_Canvas_Move_Up => "Yukarı taşı";
        public override string Editor_Canvas_Move_Down => "Aşağı taşı";
        public override string Editor_Canvas_RotateClockwise => "Saat yönünde döndür";
        public override string Editor_Canvas_RotateCounterClockwise => "Saat yönünün tersine döndür"; // combined into one word
        public override string Editor_Canvas_Mirror => "Aynala";

        public override string Editor_Canvas_RotateFlip_Title => "Döndür/Yansıt";
        public override string Editor_Canvas_FlipVertical => "Yukarı/Aşağı Yansıt";
        public override string Editor_Canvas_FlipOrientation => "Yatay/Dikey Yansıt";
        public override string Editor_Canvas_ClearAll_Description => "Tüm blokları ve kareleri temizler";

        public override string Editor_Animation => "Animasyon";
        public override string Editor_Animation_RemoveCurrentFrame => "Geçerli Kareyi Sil";
        public override string Editor_Animation_AddFrameCopy => "Kopya Kare Ekle";
        public override string Editor_Animation_AddEmptyFrame => "Boş Kare Ekle";
        public override string Editor_Animation_MoveDescription => "Kare Konumunu Değiştir";
        public override string Editor_Animation_AllFrames => "Tüm Kareler";
        public override string Editor_Animation_AllFrames_ActionDescription => "Aynı işlemi tüm karelere uygula";

        public override string Editor_SettingsMenu => "Ayarlar";
        public override string Hud_Exit => "Çıkış";
        public override string Editor_Canvas_Clear => "Temizle";

        public override string Editor_Stamp => "Damga";
        public override string Editor_StampOtherFrames => "Diğer Karelere Damgala";
        public override string Editor_StampOtherFrames_Description => "Vokselleri şu karelere yapıştır"; // "this frames" → "these frames"
        public override string Editor_PasteToFrame => "Vokselleri bu kareye yapıştır";
        public override string Editor_ClearAllFrames => "Tüm Kareleri Temizle";
        public override string Editor_ClearOtherFrames => "Diğer Kareleri Temizle";

        public override string Editor_Settings_MoveSpeed => "Hareket Hızı";
        public override string Editor_Settings_BackgroundColor => "Arka Plan Rengi";
        public override string Editor_Settings_HideHUD => "Arayüzü Gizle";

        public override string Editor_Color => "Renk";
        public override string Editor_ColorsInUseLabel => "Kullaımdaki Renkler";
        public override string Editor_Color_BrighterPlus => "Çok Daha Açık";
        public override string Editor_Color_Brighter => "Daha Açık";
        public override string Editor_Color_Darker => "Daha Koyu";
        public override string Editor_Color_DarkerPlus => "Çok Daha Koyu";
        public override string Editor_Color_RedTint => "Kırmızı Ton";
        public override string Editor_Color_Tint => "Ton";
        public override string Editor_Color_GreenTint => "Yeşil Ton";
        public override string Editor_Color_BlueTint => "Mavi Ton";
        public override string Editor_Color_YellowTint => "Sarı Ton";
        public override string Editor_Color_PurpleTint => "Mor Ton";
        public override string Editor_NoColor => "Renksiz";

        public override string Editor_Material => "Malzeme";

        /// <summary>
        /// User may change one color to another across the model
        /// </summary>
        public override string Editor_Color_Recolor => "Renk Değiştir";
        public override string Editor_Color_RecolorTo => "Şu Renge Boya";

        public override string Editor_Material_Set => "Malzeme Belirle";

        public override string Editor_Preview => "Önizleme";
        public override string Editor_CombineWithCurrent => "Mevcut Modelle Birleştir";

        public override string Editor_PickedColor => "Seçilen Renk";
        public override string Editor_ColorRGBvalues => "K:{0} Y:{1} M:{2}";

        public override string BuildingType_ImmigrationTent => "Göçmen Çadırı";
        public override string BuildingType_ImmigrationTent_Description => "{0} Göçmeni barındırır";
        public override string BuildingType_ReseachCenter => "Araştırma Merkezi"; // fixed typo "Reseach"
        public override string BuildingType_Bookpress => "Matbaacı";
        public override string BuildingType_Bookpress_Description => "Bir araştırma alanında kazanılan tüm puanlar, diğer şehirlerinizdeki tüm {0} ile paylaşılır.";

        /// <summary>
        /// 0: beer, 1: chemistry, 2: gun powder
        /// </summary>
        public override string Technology_ReseachExample => "Örnek: Bir işçi {0} ürettiğinde, {1} becerisi gelişir. Seviye atladığında ise {1} alanını paylaştığı için {2} teknolojisine puan eklenir."; // fixed "Reseach" and plural

        public override string BuildingType_Research_BaseDescription => "Teknoloji araştırmasını arttırır";

        public override string BuildingType_ResearchCenter_Description => "Bir işçi aynı alanda seviye atladığında, {0} ek teknoloji puanı sağlar.";

        //DEMO PATCH 5

        public override string Editor_CropSelection => "Seçimi kırp";

        public override string Immigrants_DisbandedSoldiers => "Terhis edilen askerler göçmen olur";
        public override string Immigrants_RefillWorkers => "İş gücünü hızla doldurur";
        public override string Immigrants_UnhousedAreLost => "Barınaksız göçmenler bir süre sonra kaybolur";
        public override string Editor_VoxelCount => "{0} voksel";

        public override string Editor_Layers_Titel => "Katmanlar";
        public override string Editor_Layers_All => "Tüm katmanlar";
        public override string Editor_LayerNumber => "{0} katman";

        public override string Editor_Layer_AddEmpty => "Boş katman ekle";
        public override string Editor_Layer_AddCopy => "Katmanı kopyala";
        public override string Editor_Layer_Remove => "Katmanı sil";
        public override string Editor_Layer_MergeDown => "Alttakiyle birleştir";
        public override string Editor_IsAnimated => "Animasyonlu";
        public override string Editor_ToggleVisible => "Görünürlüğü değiştir";
        public override string Editor_ToggleAnimatedLayer => "Animasyon katmanını aç/kapat";
        public override string Editor_Projects => "Proje dosyaları";
        public override string ProfileEditor_ReplaceMaterial => "Profil rengi: {0}";

        public override string ProfileEditor_ProfileColors_Label => "Profil renkleri";
        public override string ProfileEditor_TunicColor => "Tunik rengi";
        public override string ProfileEditor_PantsColor => "Pantolon rengi";
        public override string ProfileEditor_LeaderColor => "Lider rengi";

        public override string MapStartAs_Water => "Su";
        public override string MapStartAs_Land => "Kara";
        public override string MapStartAs_Circle => "Daire";

        public override string Hud_NeedToBeAssigned => "Atama gerekiyor";
        public override string Hud_CommitAssignment => "Ata";
        public override string Technology_NoAvailableResearch => "Araştırılabilir teknoloji yok";

        public override string Research_Tab => "Araştırma";

        //5.2
        public override string BuildCategory_General => "Genel";
        public override string BuildCategory_Military => "Askeri";
        public override string BuildCategory_Decoration => "Dekorasyon";
        public override string BuildCategory_Upgrade => "Geliştir";
        public override string Work_NoMines => "Hiç maden yok";

        public override string HUD_DisplayName => "Görüntüleme adı";
        public override string HUD_Filter => "Filtre";
        public override string HUD_Scale => "Boyut";
        public override string HUD_Tags => "Etiketler";
        public override string HUD_ClickToCancel => "İptal etmek için tıkla";

        public override string ObjectTag_Description => "Haritaya bir sembol yerleştir";
        public override string HudPins => "Arayüz işaretleri";
        public override string HudPins_Description => "Bilgiyi ekrana sabitle";

        public override string Lobby_PlayerProfileNumbered => "Profil {0}";
        public override string Lobby_CharacterCreationNumbered => "Karakter {0}";
        public override string Lobby_PlayerProfileEdit => "Oyuncu profilini düzenle";

        //public override string ProfileEditor_TunicColor => "Tunik";
        //public override string ProfileEditor_PantsColor => "Pantolon";
        //public override string ProfileEditor_LeaderColor => "Lider";

        public override string Editor_ConvertAnimationToLayers => "Animasyonu katmanlara çevir";
        public override string Editor_StampAllFrames => "Tüm kareleri damgala";

        public override string Editor_DisplayOptions => "Görüntüleme seçenekleri";
        public override string Editor_CharacterCreator => "Karakter yaratıcısı";
        public override string Editor_CharacterCreator_Description => "Askeri model görünümü düzenleyicisi";
        public override string Editor_HatGenre => "Şapka görüntüleme modu";
        public override string Editor_HatGenre_FollowWeapon => "Silahı takip et";
        public override string Editor_HatGenre_Uniform => "Üniforma";
        public override string Editor_CopyPasteSelectedColor => "Seçili rengi yapıştır";

        public override string Character_Accessories => "Aksesuar";
        public override string Character_Hat => "Şapka";
        public override string Character_Head => "Kafa";
        public override string Character_Body => "Gövde";
        public override string Character_Arms => "Kollar";
        public override string Character_Back => "Sırt";
        public override string Character_Face => "Yüz";

        //public override string BuildingType_Tavern => "Taverna";

        public override string Settings_CraftMultiplier => "Üretim hızı çarpanı";
        public override string Settings_ChildMultiplier_Description => "Yeni işçilerin eklenme hızını artırır";

        public override string Settings_CasualControls => "Basit kontrol";
        public override string Settings_CasualControls_Description => "Kilit kararları azaltarak oynanışı basitleştirir. Yalnızca sikke bir kaynak olarak kalır.";

        public override string Settings_AdvancedControls => "Gelişmiş kontrol";
        public override string Settings_AdvancedControls_Description => "Detaylı kaynak yönetimi deneyimi.";

        public override string WarsResourceGroup_Metal => "Metal";
        public override string Work_Craft => "Üret";
        public override string Work_OnlyCraftOnFullStock => "Yalnızca depo dolu ise üretim yap";

        public override string ExperienceType_Smelting => "Dökümcülük";
        public override string Category_Optimize => "Verimli hale getir";
        public override string BuildCategory_Road => "Yol";
        public override string XP_UnlockBuildPrio => "{0} inşaatının öncelik kilidini açar";
        public override string Technology_ModernFarming => "Modern tarım";

        public override string ExportImportDescription => "Başka bir oyuncu ile kayıt dosyası paylaşmak içindir. Tüm dosyalar şu klasörde: {0}";

        public override string CityCultureDescription => "Kültür, bu şehire özel bir bonus verecek";

        public override string UnitType_CloseRangeRifle => "Arkebüzcü";
        public override string UnitType_LongRangeRifle => "Silahşör";
        public override string UnitType_Skirmisher => "Avcı";

        //From lumen (light)
        public override string UnitType_MithrilArcher => "Dolunay okçusu";
        public override string UnitType_MithrilSwordsman => "Dolunay şövalyesi";

        public override string Defence_AutoAssign_Towers => "Kulelere atama yap";

        public override string EventMessage_DesertersText_Food => "Aç kalan askerler ordundan firar ediyor";


        public override string Tutorial_CasualRecruitSoldiers => "Bir asker grubu alımı yap";


        //Shadow update
        public override string Technology_CannotReassign => "Araştırma tamamlanana kadar Teknoloji yeniden atanamaz";
        public override string Diplomacy_DeclareWarAgainst => "Şuna savaş ilan edeceksin:";
        public override string Diplomacy_AllyCount => "Müttefik sayısı";
        public override string Diplomacy_CostPerAlly => "Maliyet, müttefik başına {0} artar";

        public override string Event_ChanceOfFailure => "%{0} başarısızlık ihtimali";
        public override string EventMessage_Event_Title => "Olay";
        public override string EventMessage_TheCohalition => "Koalisyon";

        public override string EventMessage_DarkHorde => "Karanlık Ordu";
        public override string EventMessage_DarkHordeKiller_Title => "Karanlık Ordu Katili";
        public override string EventMessage_DarkHordeKiller_Message => "Şampiyon şövalyeler artık hizmetinde";

        public override string Settings_Mode_Spectator_Description => "Ya olanlara seyirci dur ya da İlahi Güçler ile müdahale et.";
        public override string GodPower => "İlahi Güçler";

        public override string Building_TreeSprout_Description => "Ağaç dik";
        public override string Building_TreeSprout_Soft => "Yumuşak ağaç fidanı";
        public override string Building_TreeSprout_Hard => "Sert ağaç fidanı";

        public override string GeneralSetting_SetAll => "Hepsine uygula";

        public override string Hud_All => "Tümü";

        public override string Hud_Previous => "Önceki";

        public override string Hud_EffectWillStack => "Birikecek etkiler";

        public override string Info_WhenFoodRunsOut => "Yemek bittiğinde, şehirler ve ordular onu otomatik olarak karaborsadan satın alır.";

        //Launch test


        public override string InputActionName_NextWar => "Savaşmakta olan sıradaki taraf";

        /// <summary>
        /// These symbols are needed to fit large numbers on the HUD,
        /// there will be a tooltip to explain what number it represents
        /// </summary>
        public override string EngineHud_SymbolFor100 => "c";
        public override string EngineHud_SymbolFor1000 => "k";
        public override string EngineHud_SymbolFor10000 => "10k";

        /// <summary>
        /// When loading files from other players, you won’t get their achievement progress
        /// </summary>
        public override string GameMenu_BlockImportAchievements => "İçe aktarılan dosyalardaki başarımları engelle";

        public override string EndScreen_PeaceVictoryQuote => "Kılıçlarımızı bırakalım ve daha iyi bir geleceğe sarılalım";

        public override string VictoryType_DefeatBoss => "Baş düşman yenildi";
        public override string VictoryType_Domination => "Hakimiyet";
        public override string VictoryType_WorldPeace => "Dünya barışı";



    }
}