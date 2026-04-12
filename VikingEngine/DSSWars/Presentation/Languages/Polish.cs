using System;
using System.Collections.Generic;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Polish : AbsLanguage
    {
        //Aktualizacja wierzchowców
        public override string Leaderboards_title => "Rankingi";
        public override string Leaderboards_domination => "Rekord dominacji nad światem, {0}% plus";
        public override string Leaderboards_victory => "Zwycięstwo fabularne, top % trudności";
        public override string Leaderboards_CitySize => "Największe miasto, w robotnikach";
        public override string Leaderboards_Survival => "Czas przetrwania na poziomie trudności {0}%";

        public override string Message_CannotPayUpkeep => "Nie możesz opłacić kosztów utrzymania!";
        public override string Animals_ProductionStop => "Produkcja zwierząt zostanie wstrzymana";

        public override string Tutorial_ToCapture => "Aby schwytać";
        public override string Tutorial_ClickButton => "Kliknij przycisk";
        public override string Tutorial_MoveXToY => "Przenieś {0} do {1}";

        public override string Workers_Description1_work => "Będą budować, zbierać zasoby i wytwarzać przedmioty.";
        public override string Workers_Description2_income => "Są opodatkowani, co zapewnia dochód.";
        public override string Workers_Description3_soldiers => "Mogą zostać powołani do wojska jako żołnierze.";

        public override string Hud_Time_ValuePerMinute => "Wartość na minutę";
        public override string Hud_Time_ValuePerSecond => "Wartość na sekundę";
        public override string Hud_Lock => "Zablokuj";
        public override string Hud_Maximum => "Maks.";

        public override string Tutorial_SeeThisInThat => "Zobacz {0} w {1}";
        public override string Conscript_SkillBonus => "Bonus do umiejętności";
        public override string SoldierStats_UnitCount => "Liczba jednostek";
        /// <summary>
        /// Areas are field, forest, sea and siege
        /// </summary>
        public override string Conscript_DamagePerSecondInAreaX => "Obrażenia na sekundę - {0}";
        public override string Conscript_BaseHealth => "Bazowe zdrowie";

        /// <summary>
        /// Summary value for the ability to get across the map
        /// </summary>
        public override string Conscript_Mobility => "Mobilność";

        public override string Conscript_RiderMobility => "Mobilność jeźdźca";
        public override string Conscript_LightWagonMobility => "Mobilność lekkiego wozu";
        public override string Conscript_HeavyWagonMobility => "Mobilność ciężkiego wozu";

        /// <summary>
        /// Generelized for any object, like skills, resources and buildings
        /// </summary>
        public override string Culture_AffectedItems => "Przedmioty objęte efektem";
        //## Mounted update ##
        public override string Progress_ClosingCores => "Zamykanie rdzeni CPU {0}";
        public override string Editor_ExportFrame => "Eksportuj bieżącą klatkę";
        public override string Editor_FistFrame => "Pierwsza klatka";
        public override string Editor_LastFrame => "Ostatnia klatka";

        public override string Economy_AnimalPenUpkeep => "Utrzymanie zagrody: {0}";
        public override string Work_SlaughterX => "Ubij {0}";

        public override string BuildCategory_Farming => "Rolnictwo";
        public override string Resource_TypeName_ManType => "typ człowieka";
        public override string Resource_TypeName_NobelMen => "szlachta";
        public override string Resource_TypeName_ConservedFood => "żywność konserwowana";

        public override string UnitType_UnitOnMount => "ujeżdża {0}";
        public override string UnitType_UnitOnWagon => "wóz {0}";
        public override string UnitType_NobelUnit => "szlachecki {0}";

        /// <summary>
        /// 0: soldier type, 1: animal
        /// </summary>
        public override string UnitType_LeashAnimalHandler => "{0} trener {1}ów";

        public override string Info_ArmyFood4 => "Konserwowana żywność pozwala na większe rezerwy jedzenia";
        public override string Info_ArmyFood5 => "Świeża żywność zostanie zużyta w pierwszej kolejności";

        public override string Resource_ConservedFood_Reserves => "Zapasy konserwowanej żywności";
        public override string Resource_TypeName_Clay => "glina";
        public override string Resource_TypeName_Brick => "cegła";
        public override string Resource_TypeName_Container => "pojemnik";
        public override string Resource_TypeName_Meat => "mięso";
        public override string Resource_TypeName_Salt => "sól";
        public override string Resource_TypeName_Vehicle => "pojazd";
        public override string Resource_TypeName_WagonClosed => "zakryty wóz";
        public override string Resource_TypeName_WagonIron => "żelazny powóz";
        public override string Resource_TypeName_WagonSteel => "stalowy powóz";
        public override string Resource_TypeName_Shield => "tarcza";
        public override string Resource_TypeName_BucklerShield => "puklerz";
        public override string Resource_TypeName_RoundShield => "okrągła tarcza";
        public override string Resource_TypeName_HeaterShield => "tarcza trójkątna";
        public override string Resource_TypeName_TowerShield => "pawęż";

        public override string Resource_TypeName_Mount => "wierzchowiec";

        public override string Resource_TypeName_MountArmorTitle => "pancerz wierzchowca";

        /// <summary>
        /// 0: armor type
        /// </summary>
        public override string Resource_TypeName_MountArmorX => "wierzchowiec {0}";
        public override string Resource_TypeName_Animal => "zwierzę";

        //public override string Resource_TypeName_WildAnimal => "dzikie zwierzę";

        /// <summary>
        /// Area with wild animals
        /// </summary>
        public override string Terrain_XAnimalHabitat => "Siedlisko: {0}";

        public override string Resource_TypeName_Oxen => "wół";
        public override string Resource_TypeName_KineOxen => "bydło hodowlane";

        /// <summary>
        /// Low tier hen (for breeding)
        /// </summary>
        public override string Resource_TypeName_Fowl => "ptactwo";

        /// <summary>
        /// Low tier pig (for breeding)
        /// </summary>
        public override string Resource_TypeName_Boar => "knur";
        public override string Resource_TypeName_Pig => "świnia";
        public override string Resource_TypeName_Hen => "kura";
        public override string Resource_TypeName_Dog => "pies";
        public override string Resource_TypeName_Hound => "ogar";

        public override string Resource_TypeName_Pony => "kuc";
        public override string Resource_TypeName_Horse => "koń";
        public override string Resource_TypeName_WarHorse => "koń bojowy";
        public override string Resource_TypeName_DraftHorse => "koń pociągowy";

        public override string Resource_TypeName_WildPig => "dzika świnia";
        public override string Resource_TypeName_WildHog => "dzik";
        public override string Resource_TypeName_WarHog => "dzik bojowy";
        public override string Resource_TypeName_StagHog => "rogowy wieprz";

        public override string Resource_TypeName_Wolf => "wilk";
        public override string Resource_TypeName_Warg => "warg";
        public override string Resource_TypeName_AlphaWarg => "warg alfa";

        public override string Resource_TypeName_WildCat => "żbik";
        public override string Resource_TypeName_Lion => "lew";
        public override string Resource_TypeName_WarLion => "lew bojowy";

        public override string Resource_TypeName_Elephant => "słoń";
        public override string Resource_TypeName_WarElephant => "słoń bojowy";
        public override string Resource_TypeName_Oliphant => "olifant";

        public override string BuildHud_Select => "Wybierz budynek";
        public override string BuildHud_AreaRadius => "Promień obszaru";

        public override string NobleHouse_HousingCount => "Pomieści {0} szlachciców";


        public override string BuildingType_GreatHall => "Wielka sala";
        public override string BuildingType_GreatHall_Description => "Odblokuj zaawansowany pobór wojskowy";

        public override string BuildingType_ClayPit => "Glinianka";
        public override string BuildingType_Butcher => "Rzeźnik";
        public override string BuildingType_Butcher_Description => "Przetwarzaj zwierzęta na żywność i skóry";
        public override string BuildingType_Pottery => "Garncarz";
        public override string BuildingType_CraftX_Description => "Stanowisko rzemieślnicze: {0}";

        public override string BuildingType_GatherX_Description => "Zbieraj: {0}";

        public override string BuildingType_Smoker => "Wędzarnia";
        public override string BuildingType_Dryer => "Suszarnia";
        public override string BuildingType_Shieldmaker => "Tarczownik";
        public override string BuildingType_DryingPan => "Misa do suszenia";

        public override string BuildingType_TrapperHut => "Chata trapera";
        public override string BuildingType_TrapperHut_Description => "Pozwala na chwytanie dzikich zwierząt";

        // --- Storage ---
        public override string BuildingType_MaterialStorage => "Skład materiałów";
        public override string BuildingType_FoodStorage => "Skład żywności";
        public override string BuildingType_WeaponStorage => "Zbrojownia";
        public override string BuildingType_ArmorStorage => "Magazyn pancerzy";
        public override string BuildingType_AnimalStorage => "Zagroda dla zwierząt";

        public override string BuildingType_Storage_Description => "Zwiększa maks. zapasy o {0}";

        public override string BuildingType_Cesspit => "Dół na odpady";
        public override string BuildingType_Cesspit_Description => "Niszcz zasoby";

        public override string BuildingType_Cesspit_Info1_StockPile => "Niszczy przedmioty przekraczające limit zapasów";
        public override string Info_XAmountIsConvertedToY => "{0} zmienia się w {1}";
        public override string Info_ProductionRestriction => "Produkcja przedmiotów ograniczona do";

        public override string BuildingType_FowlPen => "Zagroda dla ptactwa";
        public override string BuildingType_BoarPen => "Zagroda dla knurów";

        // --- Oxen Pens ---
        public override string BuildingType_OxenPen => "Zagroda dla wołów";
        public override string BuildingType_KineOxenPen => "Zagroda dla bydła";

        // --- Dog Cages ---
        public override string BuildingType_DogCage => "Kojec dla psów";
        public override string BuildingType_HoundCage => "Kojec dla ogarów";

        // --- Horse Pens ---
        public override string BuildingType_PonyPen => "Wybieg dla kuców";
        public override string BuildingType_HorsePen => "Stajnia";
        public override string BuildingType_WarHorsePen => "Stajnia koni bojowych";
        public override string BuildingType_DraftHorsePen => "Stajnia koni pociągowych";

        // --- Pig/Hog Pens ---
        public override string BuildingType_WildPigPen => "Zagroda dla dzikich świń";
        public override string BuildingType_WildHogPen => "Zagroda dla dzików";
        public override string BuildingType_WarHogPen => "Zagroda dla dzików bojowych";
        public override string BuildingType_StagHogPen => "Zagroda dla rogowych wieprzy";

        // --- Wolf Cages ---
        public override string BuildingType_WolfCage => "Klatka dla wilków";
        public override string BuildingType_WargCage => "Klatka dla wargów";
        public override string BuildingType_AlphaWargCage => "Klatka dla wargów alfa";

        // --- Cat Cages ---
        public override string BuildingType_WildCatCage => "Klatka dla żbików";
        public override string BuildingType_LionCage => "Klatka dla lwów";
        public override string BuildingType_WarLionCage => "Klatka dla lwów bojowych";

        // --- Elephant Cages ---
        public override string BuildingType_ElephantCage => "Zagroda dla słoni";
        public override string BuildingType_WarElephantCage => "Zagroda dla słoni bojowych";
        public override string BuildingType_OliphantCage => "Zagroda dla olifantów";

        public override string BuildingDescription_Animals => "Produkuje zwierzęta do poboru wojskowego";
        public override string Pen_Breeding => "Hodowla zwierząt";
        public override string Pen_BreedUpChance => "{0}% szansy na wyższą rangę";
        public override string Pen_BreedDownChance => "{0}% szansy na niższą rangę";


        public override string CityCulture_AnimalBreeder2_Description => "Większa szansa na udaną hodowlę";

        public override string CityCulture_EnhancedProduction => "Zwiększona produkcja: {0}";
        public override string CityCulture_Production => "Produkcja: {0}";

        public override string CityCulture_Butchers => "Rzeźnicy";

        public override string CityCulture_Potters => "Garncarze";

        public override string CityCulture_Wainwright => "Kołodzieje";

        public override string CityCulture_Wheelwright => "Kołodzieje";
        public override string CityCulture_Wheelwright_Description => "Bonus do szybkości dla wozów z poboru";

        public override string CityCulture_ShieldMaker => "Tarczownicy";


        //public override string CityCulture_Nomads_Description => "Niski koszt osadników";

        public override string CityCulture_Coopers => "Bednarze";

        public override string CityCulture_Salters => "Mielnicy soli";


        public override string CityBiome_Title => "Biom";
        public override string CityBiome_Description => "Biomy wpływają na dostęp do niektórych zasobów i budynków";

        public override string CityBiome_Fields => "Pola";
        public override string CityBiome_Frozen => "Mroźny";
        public override string CityBiome_Forest => "Las";
        public override string CityBiome_Mountain => "Góry";
        public override string CityBiome_Desolate => "Pustkowia";
        public override string CityBiome_Desert => "Pustynia";

        public override string Bonus_IncreaseSkin => "Zwiększona produkcja skór";
        public override string Bonus_FoodStorage => "Większy skład żywności";

        public override string StockPile_LimitTitle => "Limit zapasów";

        public override string Help_Work_Automatic => "Praca odbywa się automatycznie";
        public override string Tutorial_SecondCity => "Zdobądź drugie miasto";
        public override string InputAction_SkipAutomated => "Pomiń automatyzację";

        public override string Resource_WaterReason => "Woda ogranicza liczbę jednostek, które możesz utrzymać, oraz wielkość Twojej produkcji";
        public override string BuildingType_Orchard => "Sad";
        public override string BuildingType_ManorLord => "Dwór Seniora";
        public override string BuildingType_ManorLord_Description => "Odblokowuje przetwarzanie żywności";
        /// <summary>
        /// Will end diplomatic relations like alliance
        /// </summary>
        public override string Diplomacy_EndRelations => "Zerwij relacje";

        /// <summary>
        /// Where a resource is produced or found
        /// </summary>
        public override string ItemSource => "Źródło przedmiotu";

        public override string ItemSource_Terrain => "Teren";
        public override string ItemSource_Farm => "Farma";
        public override string ItemSource_CraftStation => "Warsztat rzemieślniczy";
        public override string ItemSource_Gathering => "Zbieractwo";

        public override string CityCulture_Nomad => "Nomada";

        /// <summary>
        /// A generalized display of buffs and boons, example "+100%" or "Doubled"
        /// </summary>
        public override string Hud_ChangeFactor => "Współczynnik zmiany: {0}";

        public override string Hud_Purchase_LowXCost => "Niski koszt: {0}";

        public override string WorkQueue_Title => "Kolejka prac";
        public override string WorkQueue_Length => "Pozostałe cele prac";
        public override string WorkQueue_ActiveWorkers => "Aktywne zespoły robocze";
        public override string WorkQueue_IdleWorkers => "Wolne zespoły robocze";

        public override string WorkTeam_Size => "Mieszkańcy pracują w zespołach po {0}";

        public override string ObjectUi_ViewOnMap => "Pokaż na mapie";
        public override string ObjectUi_StuckBuildOrders => "Zablokowane rozkazy budowy";
        public override string Hud_AllArmies => "Wszystkie armie";

        public override string Hud_CurrentPage => "Bieżąca strona";
        public override string Hud_AllPages => "Wszystkie strony";
        public override string Hud_ToAllCities => "Do wszystkich miast";
        public override string Hud_ToFaction => "Do frakcji";
        public override string Hud_FromFaction => "Od frakcji";
        public override string Hud_FactionWide => "Użyj ustawień całej frakcji";
        /// <summary>
        /// This start a new city
        /// </summary>
        public override string Action_PlaceSettlement => "Załóż osadę";

        public override string Editor_Animation_RemoveAllFramesButThis => "Usuń wszystkie pozostałe klatki";


        //Winter patch 3
        public override string Hud_Purchase_AllBuildings => "Kolejkuj wszystkie budynki";
        public override string Hud_Purchase_AllTech => "Kolejkuj wszystkie technologie";
        public override string BuildingType_CasualBarracks_Description => "Czas rekrutacji żołnierzy jest dzielony między koszary";

        //Winter update patch + spring

        /// <summary>
        /// How much of a resource that will be used, e.g. "5 gold". There will be a "cost" title above the text. 0: Resource, 1: cost
        /// </summary>
        public override string Language_ItemCount => "{1} {0}";

        //public override string DisplayMode => "Tryb wyświetlania";
        //public override string DisplayMode_Windowed => "W oknie";
        //public override string DisplayMode_BorderlessFullscreen => "Pełny ekran bez ramek (Borderless)";

        //public override string GameSettings_RenderedMouseCursor => "Renderowany kursor";
        //public override string GameSettings_MuteControllerDisconnect => "Wycisz komunikaty o rozłączeniu";

        public override string Delivery_MaxDistance => "Maks. dystans dostawy: {0}";
        public override string Tutorial_WillTakeAWhile => "To zajmie chwilę, wróć później.";

        /// <summary>
        /// 0: name of building
        /// </summary>
        public override string Tutorial_WaitFor => "Poczekaj na ukończenie: {0}";
        public override string GameOverResults => "Dziennik historii gry";

        public override string UnitType_UnclaimedLand => "Ziemia niczyja";
        public override string UnitType_Settler => "Osadnik";
        public override string UnitType_Settler_Description => "Załóż nowe miasto";
        public override string Resource_ConsumedProduced => "Zużycie/Produkcja";
        public override string InputActionName_PlaceTarget => "Ustaw cel";

        public override string FactionStartSize => "Początkowa wielkość frakcji";
        public override string FactionStartSize_Full => "Pełna";
        public override string FactionStartSize_OneCity => "Jedno miasto";
        public override string FactionStartSize_Settler => "Jeden osadnik";


        //Winter update
        public override string Resource_StockpileLimit => "Limit zapasów";
        public override string GameMode_QuickMatch => "Szybki mecz";
        public override string GameMode_QuickMatch_Description => "Krótszy format gry. Wejdź do wojny na pełną skalę przeciwko rywalom.";
        public override string Lobby_PlayerCount => "Liczba graczy";
        public override string Lobby_TwoTeams => "Dwie drużyny";
        public override string Hud_Produce => "Produkcja:";
        public override string Tutorial_WaitForWorkerLevel => "Poczekaj, aż pracownik osiągnie:";

        /// <summary>
        /// 0: Production item, 1: School
        /// </summary>
        public override string Tutorial_PracticeOrSchool => "Trenuj na: {0} lub użyj: {1}";
        public override string Tutorial_AddTag => "Dodaj tag:";
        public override string Tutorial_AddPin => "Dodaj pin:";
        public override string Tutorial_SelectMostTrees => "Znajdź swoje miasto z największą ilością drzew";
        public override string Tutorial_SelectACityWithX => "Wybierz miasto z {0}";

        /// <summary>
        /// Will continue on another sentence "Select a city"
        /// </summary>
        public override string Tutorial_Select_NotCapital => ". Nie Twoją stolicę.";

        public override string Tutorial_SetXPriorityToY => "Ustaw priorytet {0} na {1}";
        public override string Tutorial_AdvisorMission => "Misja doradcy";

        public override string Tutorial_AdvisorDescription => "Rozpoczęła się pełna gra. Doradca wspomoże Cię dodatkowymi misjami pomocniczymi.";

        public override string Tutorial_EndAdvisor => "Zakończ doradztwo";


        public override string Tutorial_AdvisorCompleteTitle => "Zadania doradcy ukończone!";
        public override string Tutorial_AdvisorCompleteMessage => "Niech Twój kolejny dzień będzie błogosławiony!";

        public override string Hud_Search => "Szukaj";


        public override string DifficultyDescription_ExtremeAggression => "Ekstremalna agresja";

        public override string MapFilter => "Filtr mapy";

        public override string Settings_TechMultiplier => "Szybkość badań technologicznych";

        public override string EndScreen_MatchComplete => "Wynik meczu";

        /// <summary>
        /// Motyw: Symbol czterogłowego smoka. Znany z posiadania niezdobytego zamku.
        /// </summary>
        public override string FactionName_DragonGem => "Dragon Gem";

        /// <summary>
        /// Motyw: Easter egg na grudzień. „Tomten” to stare nordyckie imię Świętego Mikołaja
        /// </summary>
        public override string FactionName_Tomten => "Tomten";

        /// <summary>
        /// Motyw: Błogosławiony lud. Frakcja rolników przypominająca hordę.
        /// </summary>
        public override string FactionName_Hælfolc => "Hælfolc";

        /// <summary>
        /// Żelaźni Święci, ludzie strzegący przełęczy górskiej przed złem.
        /// </summary>
        public override string FactionName_AerimAngren => "Aerim Angren";

        public override string HUD_NotAvailbleInX => "Niedostępne w: {0}";

        public override string InputActionName_MiniMap => "Minimapa";

        //--
        public override string Error_SoundInitFailure => "Inicjalizacja dźwięku nie powiodła się";

        public override string GameMenu_ControllerDisconnected => "Kontroler odłączony";

        public override string Tutorial_HighPriority => "Twoi ludzie najpierw wykonają zadania o wysokim priorytecie.";

        public override string BuildingType_Wall_Description => "Mury chronią ludzi przed atakami i dają lekki bonus do ataku.";

        public override string BuildingType_Wall_Siege => "Machiny oblężnicze osłabiają obronę murów.";

        public override string Conscript_BlockChance => "{0}% szansy na zablokowanie ataku.";

        public override string Battle_DeclarWarReminder => "Musisz wypowiedzieć wojnę, aby zaatakować.";

        //--

        /// <summary>
        /// Nazwa tego języka
        /// </summary>
        public override string MyLanguage => "Polski";

        /// <summary>
        /// Jak wyświetlać liczbę przedmiotów. 0: przedmiot, 1: Liczba
        /// </summary>
        public override string Language_ItemCount_Colon => "{0}: {1}";

        /// <summary>
        /// Opcja wyboru języka
        /// </summary>
        public override string Lobby_Language => "Język";

        /// <summary>
        /// Rozpocznij grę
        /// </summary>
        public override string Lobby_Start => "START";

        /// <summary>
        /// Przycisk wyboru liczby graczy lokalnych, 0: obecna liczba graczy
        /// </summary>
        public override string Lobby_LocalMultiplayerEdit => "Lokalny multiplayer";

        /// <summary>
        /// Tytuł menu wyboru liczby graczy na podzielonym ekranie
        /// </summary>
        public override string Lobby_LocalMultiplayerTitle => "Wybierz liczbę graczy";

        /// <summary>
        /// Opis dla trybu lokalnego
        /// </summary>
        public override string Lobby_LocalMultiplayerControllerRequired => "Multiplayer wymaga kontrolerów Xbox";

        /// <summary>
        /// Przejdź do następnej pozycji ekranu
        /// </summary>
        public override string Lobby_NextScreen => "Następna pozycja ekranu";

        /// <summary>
        /// Gracze mogą wybrać wygląd i zapisać go w profilu
        /// </summary>
        public override string Lobby_FlagSelectTitle => "Wybierz flagę";

        /// <summary>
        /// 0: Numerowane od 1 do 16
        /// </summary>
        public override string Lobby_FlagNumbered => "Flaga {0}";

        /// <summary>
        /// Nazwa gry i numer wersji
        /// </summary>
        //public override string Lobby_GameVersion => "DSS War Party - wer. {0}";

        public override string FlagEditor_Description => "Zaprojektuj flagę i wybierz kolory dla swoich oddziałów.";

        /// <summary>
        /// Narzędzie malarskie wypełniające obszar kolorem
        /// </summary>
        public override string FlagEditor_Bucket => "Wiaderko";

        /// <summary>
        /// Opens flag profile editor
        /// </summary>
        public override string Lobby_FlagEdit => "Edytuj flagę";


        public override string Lobby_WarningTitle => "Ostrzeżenie";
        public override string Lobby_IgnoreWarning => "Ignoruj ostrzeżenie";

        /// <summary>
        /// Warning when one player has no input selected.
        /// </summary>
        public override string Lobby_PlayerWithoutInputWarning => "Jeden z graczy nie ma przypisanego sterowania";

        /// <summary>
        /// Menu with content that are outside what most players will use.
        /// </summary>
        public override string Lobby_Extra => "Extra";

        /// <summary>
        /// The extra content is not translated or have full controller support.
        /// </summary>
        public override string Lobby_Extra_NoSupportWarning => "Uwaga! Ta zawartość nie posiada lokalizacji ani pełnego wsparcia dla kontrolera i ułatwień dostępu";


        public override string Lobby_MapSizeTitle => "Wielkość mapy";

        /// <summary>
        /// Map size 1 name
        /// </summary>
        public override string Lobby_MapSizeOptTiny => "Malutka";

        /// <summary>
        /// Map size 2 name
        /// </summary>
        public override string Lobby_MapSizeOptSmall => "Mała";

        /// <summary>
        /// Map size 3 name
        /// </summary>
        public override string Lobby_MapSizeOptMedium => "Średnia";

        /// <summary>
        /// Map size 4 name
        /// </summary>
        public override string Lobby_MapSizeOptLarge => "Duża";

        /// <summary>
        /// Map size 5 name
        /// </summary>
        public override string Lobby_MapSizeOptHuge => "Ogromna";

        /// <summary>
        /// Map size 6 name
        /// </summary>
        public override string Lobby_MapSizeOptEpic => "Epicka";

        /// <summary>
        /// Map size description X by Y kilometers. 0: Width, 1: Height
        /// </summary>
        public override string Lobby_MapSizeDesc => "{0}x{1} km";
        /// <summary>
        /// Close game application
        /// </summary>
        public override string Lobby_ExitGame => "Wyjdź";

        /// <summary>
        /// Display local multiplayer name, 0: player number
        /// </summary>
        public override string Player_DefaultName => "Gracz {0}";

        /// <summary>
        /// In player profile editor. Opens menu with editor options
        /// </summary>
        public override string ProfileEditor_OptionsMenu => "Opcje";

        /// <summary>
        /// In player profile editor. Title for selecting flag colors
        /// </summary>
        public override string ProfileEditor_FlagColorsTitle => "Kolory flagi";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_MainColor => "Główny kolor";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail1Color => "Kolor detali 1";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail2Color => "Kolor detali 2";

        /// <summary>
        /// In player profile editor. Title for selecting you soldiers colors
        /// </summary>
        public override string ProfileEditor_PeopleColorsTitle => "Ludzie";

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_SkinColor => "Kolor skóry";

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_HairColor => "Kolor włosów";

        /// <summary>
        /// In player profile editor. Open color palette and select color
        /// </summary>
        public override string ProfileEditor_PickColor => "Wybierz kolor";

        /// <summary>
        /// In player profile editor. Adjust image position
        /// </summary>
        public override string ProfileEditor_MoveImage => "Przesuń obraz";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageLeft => "Lewo";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageRight => "Prawo";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageUp => "Góra";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageDown => "Dół";

        /// <summary>
        /// In player profile editor. Close editor without saving
        /// </summary>
        public override string ProfileEditor_DiscardAndExit => "Odrzuć i wyjdź";

        /// <summary>
        /// In player profile editor. Tooltip for discarding
        /// </summary>
        public override string ProfileEditor_DiscardAndExitDescription => "Cofnij wszystkie zmiany";

        /// <summary>
        /// In player profile editor. Save changes and close editor
        /// </summary>
        public override string Hud_SaveAndExit => "Zapisz i wyjdź";

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Hue => "Barwa";

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Lightness => "Jasność";

        /// <summary>
        /// In player profile editor. Move between flag and soldier color options.
        /// </summary>
        public override string ProfileEditor_NextColorType => "Następny typ koloru";

        /// <summary>
        /// Bieżąca prędkość gry w porównaniu do czasu rzeczywistego
        /// </summary>
        public override string Hud_GameSpeedLabel => "Prędkość gry: {0}x";

        public override string Input_GameSpeed => "Prędkość gry";

        /// <summary>
        /// Wyświetlanie w grze. Produkcja złota jednostki.
        /// </summary>
        public override string Hud_TotalIncome => "Łączny dochód/sek: {0}";

        /// <summary>
        /// Koszt złota jednostki.
        /// </summary>
        public override string Hud_Upkeep => "Utrzymanie";
        public override string Hud_ArmyUpkeep => "Utrzymanie armii: {0}";

        /// <summary>
        /// Wyświetlanie w grze. Żołnierze chroniący budynek.
        /// </summary>
        public override string Hud_GuardCount => "Strażnicy";

        public override string Hud_IncreaseMaxGuardCount => "Maks. liczba strażników: {0}";

        public override string Hud_GuardCount_MustExpandCityMessage => "Musisz rozbudować miasto.";

        public override string Hud_SoldierCount => "Liczba żołnierzy";

        public override string Hud_SoldierGroupsCount => "Liczba grup";

        /// <summary>
        /// Wyświetlanie w grze. Obliczona siła bitewna jednostki.
        /// </summary>
        public override string Hud_StrengthRating => "Wskaźnik siły";

        /// <summary>
        /// Wyświetlanie w grze. Obliczona siła bitewna dla całego narodu.
        /// </summary>
        public override string Hud_TotalStrengthRating => "Siła militarna: {0}";

        /// <summary>
        /// Wyświetlanie w grze. Dodatkowi ludzie przybywający spoza miasta-państwa.
        /// </summary>
        public override string Hud_Immigrants => "Imigranci";


        public override string Hud_CityCount => "Liczba miast: {0}";
        public override string Hud_ArmyCount => "Liczba armii: {0}";


        /// <summary>
        /// Mały przycisk do powtórzenia zakupu określoną liczbę razy. Np. "x5"
        /// </summary>
        public override string Hud_XTimes => "x{0}";

        public override string Hud_PurchaseTitle_Requirement => "Wymagania";
        public override string Hud_PurchaseTitle_Cost => "Koszt";
        public override string Hud_PurchaseTitle_Gain => "Zysk";

        /// <summary>
        /// Ile surowca zostanie zużyte, "5 złota. (Dostępne: 10)". Nad tekstem będzie tytuł "koszt". 0: Surowiec, 1: koszt, 2: dostępne
        /// </summary>
        public override string Hud_Purchase_ResourceCostOfAvailable => "{1} {0}. (Dostępne: {2})";

        public override string Hud_Purchase_CostWillIncreaseByX => "Koszt wzrośnie o {0}";

        public override string Hud_Purchase_MaxCapacity => "Osiągnięto maksymalną wydajność";

        public override string Hud_CompareMilitaryStrength_YourToOther => "Siła: Twoja {0} – Ich {1}";

        /// <summary>
        /// Wyświetla krótki ciąg daty jako Rok, Miesiąc, Dzień
        /// </summary>
        public override string Hud_Date => "R{0} M{1} D{2}";

        /// <summary>
        /// Wyświetla krótki przedział czasu jako Godziny, Minuty, Sekundy
        /// </summary>
        public override string Hud_TimeSpan => "G{0} M{1} S{2}";

        /// <summary>
        /// Bitwa między dwiema armiami lub armią a miastem
        /// </summary>
        public override string Hud_Battle => "Bitwa";



        /// <summary>
        /// Opisuje przycisk sterowania. Pauza.
        /// </summary>
        public override string Input_Pause => "Pauza";

        /// <summary>
        /// Opisuje przycisk sterowania. Wznowienie z pauzy.
        /// </summary>
        public override string Input_ResumePaused => "Wznów";

        /// <summary>
        /// Ogólny surowiec pieniężny
        /// </summary>
        public override string ResourceType_Gold => "Złoto";

        /// <summary>
        /// Surowiec: pracujący ludzie
        /// </summary>
        public override string ResourceType_Workers => "Pracownicy";


        public override string ResourceType_Workers_Description => "Pracownicy generują dochód i są powoływani jako żołnierze do Twoich armii";

        /// <summary>
        /// Surowiec używany w dyplomacji
        /// </summary>
        public override string ResourceType_DiplomacyPoints => "Punkty dyplomacji";

        /// <summary>
        /// 0: Liczba posiadanych punktów, 1: Miękki limit (po nim przyrost zwalnia), 2: Twardy limit
        /// </summary>
        public override string ResourceType_DiplomacyPoints_WithSoftAndHardLimit => "Punkty dyplomacji: {0} / {1} ({2})";

        /// <summary>
        /// Typ budynku miejskiego. Budynek dla rycerzy i dyplomatów.
        /// </summary>
        public override string Building_NobleHouse => "Dom szlachecki";

        public override string Building_NobleHouse_DiplomacyPointsAdd => "1 punkt dyplomacji co {0} sekund";
        public override string Building_NobleHouse_DiplomacyPointsLimit => "+{0} do limitu punktów dyplomacji";
        public override string Building_NobleHouse_UnlocksKnight => "Odblokowuje jednostkę rycerzy";

        public override string Building_BuildAction => "Buduj";
        public override string Building_IsBuilt => "Zbudowano";

        /// <summary>
        /// Typ budynku miejskiego. Masowa produkcja zła.
        /// </summary>
        public override string Building_DarkFactory => "Mroczna fabryka";

        /// <summary>
        /// W menu ustawień gry. Sumuje wszystkie opcje trudności w procentach.
        /// </summary>
        public override string Settings_TotalDifficulty => "Całkowity poziom trudności: {0}%";

        /// <summary>
        /// W menu ustawień gry. Podstawowa opcja trudności.
        /// </summary>
        public override string Settings_DifficultyLevel => "Poziom trudności: {0}%";


        /// <summary>
        /// W menu ustawień gry. Opcja tworzenia nowych map zamiast ładowania gotowych.
        /// </summary>
        public override string Settings_GenerateMaps => "Generuj nowe mapy";

        /// <summary>
        /// W menu ustawień gry. Tworzenie nowych map wydłuża czas ładowania.
        /// </summary>
        public override string Settings_GenerateMaps_SlowDescription => "Generowanie trwa dłużej niż ładowanie gotowych map";

        /// <summary>
        /// W menu ustawień gry. Opcja trudności. Możliwość wydawania rozkazów podczas pauzy.
        /// </summary>
        public override string Settings_AllowPause => "Zezwalaj na pauzę i rozkazy";

        /// <summary>
        /// W menu ustawień gry. Opcja trudności. Bossowie pojawiający się w grze.
        /// </summary>
        public override string Settings_BossEvents => "Wydarzenia bossów";

        /// <summary>
        /// W menu ustawień gry. Opis trybu bez bossów.
        /// </summary>
        public override string Settings_BossEvents_SandboxDescription => "Wyłączenie bossów przełączy grę w tryb piaskownicy (sandbox) bez zakończenia.";


        /// <summary>
        /// Opcje automatyzacji mechanik gry. Tytuł menu.
        /// </summary>
        public override string Automation_Title => "Automatyzacja";
        /// <summary>
        /// Informacja o działaniu automatyzacji.
        /// </summary>
        public override string Automation_InfoLine_MaxWorkforce => "Poczeka na maksymalną liczbę pracowników";
        /// <summary>
        /// Informacja o działaniu automatyzacji.
        /// </summary>
        public override string Automation_InfoLine_NegativeIncome => "Zatrzyma się, jeśli dochód będzie ujemny";
        /// <summary>
        /// Informacja o działaniu automatyzacji.
        /// </summary>
        public override string Automation_InfoLine_Priority => "Duże miasta mają priorytet";
        /// <summary>
        /// Informacja o działaniu automatyzacji.
        /// </summary>
        public override string Automation_InfoLine_PurchaseSpeed => "Wykonuje maksymalnie jeden zakup na sekundę";

        /// <summary>
        /// Podpis przycisku akcji. Specjalistyczny budynek dla rycerzy i dyplomatów.
        /// </summary>
        public override string HudAction_BuyItem => "Kup {0}";

        /// <summary>
        /// Stan pokoju lub wojny między dwoma narodami
        /// </summary>
        public override string Diplomacy_RelationType => "Relacja";

        /// <summary>
        /// Tytuł listy relacji innych frakcji między sobą
        /// </summary>
        public override string Diplomacy_RelationToOthers => "Ich relacje z innymi";

        /// <summary>
        /// Relacja dyplomatyczna. Masz bezpośrednią kontrolę nad zasobami narodu.
        /// </summary>
        public override string Diplomacy_RelationType_Servant => "Wasal";

        /// <summary>
        /// Relacja dyplomatyczna. Pełna współpraca.
        /// </summary>
        public override string Diplomacy_RelationType_Ally => "Sojusznik";

        /// <summary>
        /// Relacja dyplomatyczna. Zmniejszona szansa na wojnę.
        /// </summary>
        public override string Diplomacy_RelationType_Good => "Dobra";

        /// <summary>
        /// Relacja dyplomatyczna. Porozumienie pokojowe.
        /// </summary>
        public override string Diplomacy_RelationType_Peace => "Pokój";

        /// <summary>
        /// Relacja dyplomatyczna. Brak kontaktu.
        /// </summary>
        public override string Diplomacy_RelationType_Neutral => "Neutralna";
        /// <summary>
        /// Relacja dyplomatyczna. Tymczasowe porozumienie pokojowe.
        /// </summary>
        public override string Diplomacy_RelationType_Truce => "Rozejm";
        /// <summary>
        /// Relacja dyplomatyczna. Wojna.
        /// </summary>
        public override string Diplomacy_RelationType_War => "Wojna";
        /// <summary>
        /// Relacja dyplomatyczna. Wojna bez szansy na pokój.
        /// </summary>
        public override string Diplomacy_RelationType_TotalWar => "Wojna totalna";

        /// <summary>
        /// Komunikacja dyplomatyczna. Jak dobrze możecie omawiać warunki. 0: SpeakTerms
        /// </summary>
        public override string Diplomacy_SpeakTermIs => "Stosunki dyplomatyczne: {0}";

        /// <summary>
        /// Komunikacja dyplomatyczna. Lepsza niż normalnie.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Good => "Dobre";

        /// <summary>
        /// Komunikacja dyplomatyczna. Normalna.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Normal => "Normalne";

        /// <summary>
        /// Komunikacja dyplomatyczna. Gorsza niż normalnie.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad => "Złe";

        /// <summary>
        /// Komunikacja dyplomatyczna. Brak komunikacji.
        /// </summary>
        public override string Diplomacy_SpeakTerms_None => "Brak";

        /// <summary>
        /// Akcja dyplomatyczna. Nawiąż nową relację dyplomatyczną.
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo => "Nawiąż relacje z: {0}";

        /// <summary>
        /// Akcja dyplomatyczna. Zaproponuj nową relację dyplomatyczną.
        /// </summary>
        public override string Diplomacy_OfferPeace => "Zaproponuj pokój";

        /// <summary>
        /// Akcja dyplomatyczna. Zaproponuj nową relację dyplomatyczną.
        /// </summary>
        public override string Diplomacy_OfferAlliance => "Zaproponuj sojusz";

        /// <summary>
        /// Tytuł dyplomatyczny. Inny gracz zaproponował nową relację. 0: nazwa gracza
        /// </summary>
        public override string Diplomacy_PlayerOfferAlliance => "{0} proponuje nowe relacje";

        /// <summary>
        /// Akcja dyplomatyczna. Zaakceptuj nową relację dyplomatyczną.
        /// </summary>
        public override string Diplomacy_AcceptRelationOffer => "Zaakceptuj nową relację";

        /// <summary>
        /// Opis dyplomatyczny. Inny gracz zaproponował nową relację. 0: typ relacji
        /// </summary>
        public override string Diplomacy_NewRelationOffered => "Zaproponowano nową relację: {0}";

        /// <summary>
        /// Akcja dyplomatyczna. Spraw, by inny naród Ci służył.
        /// </summary>
        public override string Diplomacy_AbsorbServant => "Wciel wasala";

        /// <summary>
        /// Opis dyplomatyczny. Przeciwstawia się złu.
        /// </summary>
        public override string Diplomacy_LightSide => "Jest sojusznikiem sił światła";

        /// <summary>
        /// Opis dyplomatyczny. Jak długo potrwa rozejm.
        /// </summary>
        public override string Diplomacy_TruceTimeLength => "Kończy się za {0} sekund";

        /// <summary>
        /// Akcja dyplomatyczna. Przedłuż rozejm.
        /// </summary>
        public override string Diplomacy_ExtendTruceAction => "Przedłuż rozejm";

        /// <summary>
        /// Opis dyplomatyczny. O ile rozejm zostanie przedłużony.
        /// </summary>
        public override string Diplomacy_TruceExtendTimeLength => "Przedłuża rozejm o {0} sekund";

        /// <summary>
        /// Opis dyplomatyczny. Złamanie uzgodnionej relacji będzie kosztować punkty dyplomacji.
        /// </summary>
        public override string Diplomacy_BreakingRelationCost => "Zerwanie relacji będzie kosztować {0} pkt. dyplomacji";

        /// <summary>
        /// Opis dyplomatyczny dla sojuszników.
        /// </summary>
        public override string Diplomacy_AllyDescription => "Sojusznicy wspólnie wypowiadają wojny.";

        /// <summary>
        /// Opis dyplomatyczny dla dobrych relacji.
        /// </summary>
        public override string Diplomacy_GoodRelationDescription => "Ogranicza możliwość wypowiedzenia wojny.";

        /// <summary>
        /// Opis dyplomatyczny. Musisz mieć większą siłę militarną niż wasal.
        /// </summary>
        public override string Diplomacy_ServantRequirement_XStrongerMilitary => "{0}x silniejsza potęga militarna";

        /// <summary>
        /// Opis dyplomatyczny. Wasal musi tkwić w beznadziejnej wojnie.
        /// </summary>
        public override string Diplomacy_ServantRequirement_HopelessWar => "Wasal musi być w stanie wojny z silniejszym wrogiem";

        /// <summary>
        /// Opis dyplomatyczny. Wasal nie może posiadać zbyt wielu miast.
        /// </summary>
        public override string Diplomacy_ServantRequirement_MaxCities => "Wasal może mieć maksymalnie {0} miast";

        /// <summary>
        /// Opis dyplomatyczny. Koszt w punktach dyplomacji wzrośnie.
        /// </summary>
        public override string Diplomacy_ServantPriceWillRise => "Cena wzrośnie za każdego wasala";

        /// <summary>
        /// Opis dyplomatyczny. Wynik relacji wasalnej.
        /// </summary>
        public override string Diplomacy_ServantGainAbsorbFaction => "Wchłoń drugą frakcję";

        /// <summary>
        /// Wiadomość po otrzymaniu deklaracji wojny
        /// </summary>
        public override string Diplomacy_WarDeclarationTitle => "Wojna została wypowiedziana!";

        /// <summary>
        /// Licznik rozejmu dobiegł końca, powrót do stanu wojny
        /// </summary>
        public override string Diplomacy_TruceEndTitle => "Rozejm dobiegł końca";

        /// <summary>
        /// Statystyki wyświetlane na ekranie końcowym gry.
        /// </summary>
        public override string Statistics_Title => "Statystyki";
        /// <summary>
        /// Statystyki końcowe. Całkowity czas gry.
        /// </summary>
        public override string EndGameStatistics_Time => "Czas gry: {0}";

        /// <summary>
        /// Statystyki końcowe. Liczba kupionych żołnierzy.
        /// </summary>
        public override string EndGameStatistics_SoldiersRecruited => "Zrekrutowani żołnierze: {0}";

        /// <summary>
        /// Statystyki końcowe. Liczba Twoich żołnierzy poległych w bitwie.
        /// </summary>
        public override string EndGameStatistics_FriendlySoldiersLost => "Żołnierze straceni w bitwie: {0}";

        /// <summary>
        /// Statystyki końcowe. Liczba wrogich żołnierzy zabitych w bitwie.
        /// </summary>
        public override string EndGameStatistics_EnemySoldiersKilled => "Wrogowie zabici w bitwie: {0}";

        /// <summary>
        /// Statystyki końcowe. Liczba żołnierzy, którzy Cię opuścili.
        /// </summary>
        public override string EndGameStatistics_SoldiersDeserted => "Dezerterzy: {0}";

        /// <summary>
        /// Statystyki końcowe. Liczba miast zdobytych w bitwie.
        /// </summary>
        public override string EndGameStatistics_CitiesCaptured => "Zdobyte miasta: {0}";

        /// <summary>
        /// Statystyki końcowe. Liczba miast straconych w bitwie.
        /// </summary>
        public override string EndGameStatistics_CitiesLost => "Stracone miasta: {0}";

        /// <summary>
        /// Statystyki końcowe. Liczba wygranych bitew.
        /// </summary>
        public override string EndGameStatistics_BattlesWon => "Wygrane bitwy: {0}";

        /// <summary>
        /// Statystyki końcowe. Liczba przegranych bitew.
        /// </summary>
        public override string EndGameStatistics_BattlesLost => "Przegrane bitwy: {0}";

        /// <summary>
        /// Statystyki końcowe. Dyplomacja. Wypowiedziane przez Ciebie wojny.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByYou => "Wypowiedziane wojny: {0}";

        /// <summary>
        /// Statystyki końcowe. Dyplomacja. Wojny wypowiedziane Tobie.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByEnemy => "Otrzymane deklaracje wojny: {0}";

        /// <summary>
        /// Statystyki końcowe. Sojusze nawiązane przez dyplomację.
        /// </summary>
        public override string EndGameStatistics_AlliedFactions => "Sojusze dyplomatyczne: {0}";

        /// <summary>
        /// Statystyki końcowe. Wasale pozyskani przez dyplomację.
        /// </summary>
        public override string EndGameStatistics_ServantFactions => "Wasale dyplomatyczni: {0}";

        /// <summary>
        /// Zbiorczy typ jednostki na mapie. Armia żołnierzy.
        /// </summary>
        public override string UnitType_Army => "Armia";

        /// <summary>
        /// Zbiorczy typ jednostki na mapie. Armia żołnierzy.
        /// </summary>
        public override string UnitType_SoldierGroup => "Grupa";

        /// <summary>
        /// Zbiorczy typ jednostki na mapie. Pospolita nazwa dla wioski lub miasta.
        /// </summary>
        public override string UnitType_City => "Miasto";

        /// <summary>
        /// Wybór grupy armii
        /// </summary>
        public override string UnitType_ArmyCollectionAndCount => "Grupa armii, liczba: {0}";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Standardowy żołnierz liniowy.
        /// </summary>
        public override string UnitType_Soldier => "Żołnierz";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Żołnierz bitwy morskiej.
        /// </summary>
        public override string UnitType_Sailor => "Marynarz";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Powołani chłopi.
        /// </summary>
        public override string UnitType_Folkman => "Pospolity żołnierz";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Jednostka z tarczą i włócznią.
        /// </summary>
        public override string UnitType_Spearman => "Włócznik";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Oddziały elitarne, część gwardii królewskiej.
        /// </summary>
        public override string UnitType_HonorGuard => "Gwardia honorowa";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Jednostka przeciwpancerna, używa długich włóczni dwuręcznych.
        /// </summary>
        public override string UnitType_Pikeman => "Pikinier";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Opancerzona jednostka kawalerii.
        /// </summary>
        public override string UnitType_Knight => "Rycerz";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Łuk i strzały.
        /// </summary>
        public override string UnitType_Archer => "Łucznik";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza.
        /// </summary>
        public override string UnitType_Crossbow => "Kusznik";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Machina wojenna miotająca wielkie bełty.
        /// </summary>
        public override string UnitType_Ballista => "Balista";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Fantastyczny troll z armatą.
        /// </summary>
        public override string UnitType_Trollcannon => "Troll-armata";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Żołnierz z lasu.
        /// </summary>
        public override string UnitType_GreenSoldier => "Leśny żołnierz";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Jednostka morska z północy.
        /// </summary>
        public override string UnitType_Viking => "Wiking";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Główny zły władca.
        /// </summary>
        public override string UnitType_DarkLord => "Mroczny Władca";

        /// <summary>
        /// Nazwa specjalistycznego typu żołnierza. Żołnierz niosący wielką flagę.
        /// </summary>
        public override string UnitType_Bannerman => "Chorąży";

        /// <summary>
        /// Nazwa jednostki wojskowej. Statek transportujący żołnierzy. 0: typ transportowanej jednostki
        /// </summary>
        public override string UnitType_WarshipWithUnit => "Okręt wojenny ({0})";

        public override string UnitType_Description_Soldier => "Jednostka ogólnego przeznaczenia.";
        public override string UnitType_Description_Sailor => "Silny podczas walk na morzu.";
        public override string UnitType_Description_Folkman => "Tani, niewyszkoleni żołnierze.";
        public override string UnitType_Description_HonorGuard => "Elitarni żołnierze bez kosztów utrzymania.";
        public override string UnitType_Description_Knight => "Silny w bitwach na otwartym polu.";
        public override string UnitType_Description_Archer => "Silny tylko pod osłoną.";
        public override string UnitType_Description_Crossbow => "Potężny strzelec dystansowy.";
        public override string UnitType_Description_Ballista => "Skuteczna przeciwko miastom.";
        public override string UnitType_Description_GreenSoldier => "Budzący grozę elficki wojownik.";

        public override string UnitType_Description_DarkLord => "Finałowy boss.";

        /// <summary>
        /// Informacje o typie żołnierza
        /// </summary>
        public override string SoldierStats_Title => "Statystyki jednostki";

        /// <summary>
        /// Liczba grup żołnierzy
        /// </summary>
        public override string SoldierStats_GroupCountAndSoldierCount => "{0} grup, łącznie {1} jednostek";

        /// <summary>
        /// Żołnierze mają różną siłę w zależności od tego, czy atakują na lądzie, z morza czy oblegają osadę
        /// </summary>
        public override string SoldierStats_AttackStrengthLandSeaCity => "Siła ataku: Ląd {0} | Morze {1} | Miasto {2}";

        /// <summary>
        /// Ile ran może wytrzymać żołnierz
        /// </summary>
        public override string SoldierStats_Health => "Zdrowie";

        /// <summary>
        /// Niektórzy żołnierze zwiększają prędkość poruszania się armii
        /// </summary>
        public override string SoldierStats_SpeedBonusLand => "Bonus do prędkości armii na lądzie: {0}";

        /// <summary>
        /// Niektórzy żołnierze zwiększają prędkość poruszania się floty
        /// </summary>
        public override string SoldierStats_SpeedBonusSea => "Bonus do prędkości armii na morzu: {0}";

        /// <summary>
        /// Kupieni żołnierze zaczynają jako rekrci i kończą szkolenie po kilku minutach.
        /// </summary>
        public override string SoldierStats_RecruitTrainingTimeMinutes => "Czas szkolenia: {0} min. Przebiega dwa razy szybciej, jeśli rekrci stacjonują przy mieście.";

        /// <summary>
        /// Opcja menu do sterowania armią. Zatrzymanie ruchu.
        /// </summary>
        public override string ArmyOption_Halt => "Zatrzymaj (Halt)";

        /// <summary>
        /// Opcja menu do sterowania armią. Usuwanie żołnierzy.
        /// </summary>
        public override string ArmyOption_Disband => "Rozwiąż jednostki";

        /// <summary>
        /// Opcja menu do sterowania armią. Przesyłanie żołnierzy między armiami.
        /// </summary>
        public override string ArmyOption_Divide => "Podziel armię";

        /// <summary>
        /// Opcja menu do sterowania armią. Usuwanie żołnierzy.
        /// </summary>
        public override string ArmyOption_RemoveX => "Usuń {0}";

        /// <summary>
        /// Opcja menu do sterowania armią. Usuwanie żołnierzy.
        /// </summary>
        public override string ArmyOption_DisbandAll => "Rozwiąż wszystkich";

        /// <summary>
        /// Opcja menu do sterowania armią. 0: Liczba, 1: Typ jednostki
        /// </summary>
        public override string ArmyOption_XGroupsOfType => "{1} grup: {0}";

        /// <summary>
        /// Opcja menu do sterowania armią. Przesyłanie żołnierzy między armiami.
        /// </summary>
        public override string ArmyOption_SendToX => "Wyślij jednostki do {0}";

        public override string ArmyOption_MergeAllArmies => "Połącz wszystkie armie";

        /// <summary>
        /// Opcja menu do sterowania armią. Przesyłanie żołnierzy między armiami.
        /// </summary>
        public override string ArmyOption_SendToNewArmy => "Wydziel jednostki do nowej armii";

        /// <summary>
        /// Opcja menu do sterowania armią. Przesyłanie żołnierzy między armiami.
        /// </summary>
        public override string ArmyOption_SendX => "Wyślij {0}";

        /// <summary>
        /// Opcja menu do sterowania armią. Przesyłanie żołnierzy między armiami.
        /// </summary>
        public override string ArmyOption_SendAll => "Wyślij wszystko";

        /// <summary>
        /// Opcja menu do sterowania armią. Przesyłanie żołnierzy między armiami.
        /// </summary>
        public override string ArmyOption_DivideHalf => "Podziel armię na pół";

        /// <summary>
        /// Opcja menu do sterowania armią. Przesyłanie żołnierzy między armiami.
        /// </summary>
        public override string ArmyOption_MergeArmies => "Połącz armie";



        /// <summary>
        /// Kupowanie żołnierzy.
        /// </summary>
        public override string UnitType_Recruit => "Rekrut";

        /// <summary>
        /// Kupowanie żołnierzy konkretnego typu. 0: typ
        /// </summary>
        public override string CityOption_RecruitType => "Rekrutuj: {0}";

        /// <summary>
        /// Liczba płatnych żołnierzy
        /// </summary>
        public override string CityOption_XMercenaries => "Najemnicy: {0}";


        /// <summary>
        /// Wskazuje liczbę najemników obecnie dostępnych do wynajęcia na rynku
        /// </summary>
        public override string Hud_MercenaryMarket => "Dostępni najemnicy na rynku";

        /// <summary>
        /// Zakup płatnych żołnierzy
        /// </summary>
        public override string CityOption_BuyXMercenaries => "Sprowadź {0} najemników";

        public override string CityOption_Mercenaries_Description => "Żołnierze zostaną powołani spośród najemników zamiast Twoich pracowników";

        /// <summary>
        /// Podpis przycisku akcji. Tworzenie miejsc mieszkalnych dla większej liczby pracowników.
        /// </summary>
        public override string CityOption_ExpandWorkForce => "Zwiększ liczbę pracowników";
        public override string CityOption_ExpandWorkForce_IncreaseMax => "Maks. liczba pracowników +{0}";
        public override string CityOption_ExpandGuardSize => "Zwiększ straż";

        public override string CityOption_Damages => "Uszkodzenia: {0}";
        public override string CityOption_Repair => "Napraw uszkodzenia";
        public override string CityOption_RepairGain => "Napraw {0} pkt. uszkodzeń";

        public override string CityOption_Repair_Description => "Uszkodzenia ograniczają liczbę pracowników, których możesz pomieścić.";


        public override string CityOption_BurnItDown => "Spal doszczętnie";
        public override string CityOption_BurnItDown_Description => "Usuń pracowników i zadaj maksymalne uszkodzenia miastu";

        /// <summary>
        /// Główny boss. Nazwany od świecącego metalicznego kamienia utkwionego w czole.
        /// </summary>
        public override string FactionName_DarkLord => "Oko Zagłady";

        /// <summary>
        /// Frakcja inspirowana orkami. Służy Mrocznemu Władcy.
        /// </summary>
        public override string FactionName_DarkFollower => "Słudzy Grozy";

        /// <summary>
        /// Największa frakcja, stare, ale skorumpowane królestwo.
        /// </summary>
        public override string FactionName_UnitedKingdom => "Zjednoczone Królestwa";

        /// <summary>
        /// Frakcja inspirowana elfami. Żyje w harmonii z lasem.
        /// </summary>
        public override string FactionName_Greenwood => "Zielony Las";

        /// <summary>
        /// Frakcja o wschodnim klimacie, położona na wschodzie.
        /// </summary>
        public override string FactionName_EasternEmpire => "Cesarstwo Wschodnie";

        /// <summary>
        /// Królestwo w klimacie wikingów na północy. Największe z nich.
        /// </summary>
        public override string FactionName_NordicRealm => "Królestwa Północy";

        /// <summary>
        /// Królestwo w klimacie wikingów na północy. Używa symbolu niedźwiedziego pazura.
        /// </summary>
        public override string FactionName_BearClaw => "Niedźwiedzi Pazur";

        /// <summary>
        /// Królestwo w klimacie wikingów na północy. Używa symbolu koguciej ostrogi.
        /// </summary>
        public override string FactionName_NordicSpur => "Ostroga Północy";

        /// <summary>
        /// Królestwo w klimacie wikingów na północy. Używa symbolu czarnego kruka.
        /// </summary>
        public override string FactionName_IceRaven => "Lodowy Kruk";

        /// <summary>
        /// Frakcja słynąca z zabijania smoków za pomocą potężnych balist.
        /// </summary>
        public override string FactionName_Dragonslayer => "Pogromca Smoków";

        /// <summary>
        /// Jednostka najemników z południa. Klimat arabski.
        /// </summary>
        public override string FactionName_SouthHara => "Południowa Hara";

        /// <summary>
        /// Nazwa dla neutralnych nacji kontrolowanych przez komputer.
        /// </summary>
        public override string FactionName_GenericAi => "AI {0}";

        /// <summary>
        /// Wyświetlana nazwa graczy i ich numerów.
        /// </summary>
        public override string FactionName_Player => "Gracz {0}";

        /// <summary>
        /// Wiadomość, gdy miniboss nadpływa na statkach z południa.
        /// </summary>
        public override string EventMessage_HaraMercenaryTitle => "Wróg nadciąga!";
        public override string EventMessage_HaraMercenaryText => "Najemnicy z Hary zostali dostrzeżeni na południu";

        /// <summary>
        /// Pierwsze ostrzeżenie o przybyciu głównego bossa.
        /// </summary>
        public override string EventMessage_ProphesyTitle => "Mroczna przepowiednia";
        public override string EventMessage_ProphesyText => "Oko Zagłady wkrótce się objawi, a Twoi wrogowie staną po jego stronie!";

        /// <summary>
        /// Drugie ostrzeżenie o przybyciu głównego bossa.
        /// </summary>
        public override string EventMessage_FinalBossEnterTitle => "Mroczne czasy";
        public override string EventMessage_FinalBossEnterText => "Oko Zagłady wkroczyło na mapę!";

        /// <summary>
        /// Wiadomość, gdy główny boss spotyka Cię na polu bitwy.
        /// </summary>
        public override string EventMessage_FinalBattleTitle => "Desperacki atak";
        public override string EventMessage_FinalBattleText => "Mroczny Władca dołączył do bitwy. To Twoja szansa, by go zgładzić!";

        /// <summary>
        /// Wiadomość, gdy żołnierze opuszczają armię, bo nie możesz opłacić ich utrzymania.
        /// </summary>
        public override string EventMessage_DesertersTitle => "Dezercja!";
        public override string EventMessage_DesertersText_Money => "Nieopłaceni żołnierze uciekają z Twoich armii";

        public override string DifficultyDescription_AiAggression => "Agresja AI: {0}.";
        public override string DifficultyDescription_BossSize => "Wielkość bossa: {0}.";
        public override string DifficultyDescription_BossEnterTime => "Czas przybycia bossa: {0}.";
        public override string DifficultyDescription_AiEconomy => "Ekonomia AI: {0}%.";
        public override string DifficultyDescription_AiDelay => "Opóźnienie AI: {0}.";
        public override string DifficultyDescription_DiplomacyDifficulty => "Trudność dyplomacji: {0}.";
        public override string DifficultyDescription_MercenaryCost => "Koszt najemników: {0}.";
        public override string DifficultyDescription_HonorGuards => "Gwardia honorowa: {0}.";


        /// <summary>
        /// Gra zakończona sukcesem.
        /// </summary>
        public override string EndScreen_VictoryTitle => "Zwycięstwo!";

        /// <summary>
        /// Cytaty lidera, w którego wciela się gracz.
        /// </summary>
        public override List<string> EndScreen_VictoryQuotes => new List<string>
        {
            "W czasach pokoju opłakujemy zmarłych.",
            "Każdy triumf niesie ze sobą cień poświęcenia.",
            "Pamiętaj o drodze, która nas tu przywiodła, usłanej duszami dzielnych wojowników.",
            "Nasze umysły promieniują zwycięstwem, lecz serca są ciężkie od żalu po poległych."
        };

        public override string EndScreen_DominationVictoryQuote => "Zostałem wybrany przez Bogów, by władać światem!";

        /// <summary>
        /// Gra zakończona porażką.
        /// </summary>
        public override string EndScreen_FailTitle => "Porażka!";

        /// <summary>
        /// Cytaty lidera, w którego wciela się gracz.
        /// </summary>
        public override List<string> EndScreen_FailureQuotes => new List<string>
        {
            "Z ciałami wycieńczonymi marszem i po nocach pełnych trwogi, witamy koniec.",
            "Porażka może spowić nasze ziemie mrokiem, lecz nie zgasi światła naszej determinacji.",
            "Zduście płomienie w naszych sercach; z ich popiołów nasze dzieci wykują nowy świt.",
            "Niech nasze opowieści będą żarem, który roznieci jutrzejsze zwycięstwo.",
        };

        /// <summary>
        /// Krótka scenka na końcu gry.
        /// </summary>
        public override string EndScreen_WatchEpilogue => "Obejrzyj epilog";

        /// <summary>
        /// Tytuł scenki.
        /// </summary>
        public override string EndScreen_Epilogue_Title => "Epilog";

        /// <summary>
        /// Wprowadzenie do scenki.
        /// </summary>
        public override string EndScreen_Epilogue_Text => "160 lat temu";

        /// <summary>
        /// Prolog to krótki wiersz o historii gry.
        /// </summary>
        public override string GameMenu_WatchPrologue => "Obejrzyj prolog";

        public override string Prologue_Title => "Prolog";

        /// <summary>
        /// Wiersz musi mieć trzy linie, czwarta zostanie pobrana z nazw bossów.
        /// </summary>
        public override List<string> Prologue_TextLines => new List<string>
        {
            "Sny nawiedzają cię nocą,",
            "Przepowiednia mrocznej przyszłości,",
            "Przygotuj się na jego przybycie,",
        };

        /// <summary>
        /// Menu w grze podczas pauzy.
        /// </summary>
        public override string GameMenu_Title => "Menu gry";

        /// <summary>
        /// Kontynuuj grę po ekranie końcowym.
        /// </summary>
        public override string GameMenu_ContinueGame => "Kontynuuj";

        /// <summary>
        /// Kontynuuj rozgrywkę.
        /// </summary>
        public override string GameMenu_Resume => "Wznów";

        /// <summary>
        /// Wyjście do lobby.
        /// </summary>
        public override string GameMenu_ExitGame => "Wyjdź z gry";

        public override string Hud_Save => "Zapisz";
        public override string GameMenu_SaveStateWarnings => "Uwaga! Pliki zapisu zostaną utracone po aktualizacji gry.";
        public override string GameMenu_LoadState => "Wczytaj";
        public override string GameMenu_ContinueFromSave => "Kontynuuj z zapisu";

        public override string GameMenu_AutoSave => "Autozapis";

        public override string GameMenu_Load_PlayerCountError => "Musisz ustawić liczbę graczy zgodną z plikiem zapisu: {0}";

        public override string Progressbar_MapLoadingState => "Ładowanie mapy: {0}";

        public override string Progressbar_ProgressComplete => "ukończono";

        /// <summary>
        /// 0: postęp w procentach, 1: liczba błędów
        /// </summary>
        public override string Progressbar_MapLoadingState_GeneratingPercentage => "Generowanie: {0}%. (Błędy: {1})";


        /// <summary>
        /// 0: obecna część, 1: liczba części
        /// </summary>
        public override string Progressbar_MapLoadingState_LoadPart => "część {0}/{1}";

        /// <summary>
        /// 0: Procenty lub Ukończono
        /// </summary>
        public override string Progressbar_SaveProgress => "Zapisywanie: {0}";

        /// <summary>
        /// 0: Procenty lub Ukończono
        /// </summary>
        public override string Progressbar_LoadProgress => "Wczytywanie: {0}";

        /// <summary>
        /// Postęp zakończony, oczekiwanie na gracza.
        /// </summary>
        public override string Progressbar_PressAnyKey => "Naciśnij dowolny klawisz, aby kontynuować";


        /// <summary>
        /// Krótki samouczek.
        /// </summary>
        public override string Tutorial_MenuOption => "Uruchom samouczek";
        public override string Tutorial_MissionsTitle => "Misje samouczka";
        public override string Tutorial_Mission_BuySoldier => "Wybierz miasto i zrekrutuj żołnierza";
        public override string Tutorial_Mission_MoveArmy => "Wybierz armię i wydaj rozkaz ruchu";

        public override string Tutorial_CompleteTitle => "Samouczek ukończony!";
        public override string Tutorial_CompleteMessage => "Odblokowano pełny zoom i zaawansowane opcje gry.";

        /// <summary>
        /// Wyświetla przycisk sterowania.
        /// </summary>
        public override string Tutorial_SelectInput => "Wybierz";
        public override string Tutorial_MoveInput => "Rozkaz ruchu";

        /// <summary>
        /// Versus. Text describing the two armies that will go into battle
        /// </summary>
        public override string Hud_Versus => "VS.";

        public override string Hud_WardeclarationTitle => "Deklaracja wojny";

        public override string ArmyOption_Attack => "Atakuj";



        //----
        /// <summary>
        /// In game settings menu. Change what keys and buttons do when pressed
        /// </summary>
        public override string Settings_ButtonMapping => "Przypisanie klawiszy";



        /// <summary>
        /// Input type, standard PC input
        /// </summary>
        public override string Input_Source_Keyboard => "Klawiatura i mysz";

        /// <summary>
        /// Input type, handheld controller like the xbox uses
        /// </summary>
        public override string Input_Source_Controller => "Kontroler";


        /* #### --------------- ##### */
        /* #### RESOURCE UPDATE ##### */
        /* #### --------------- ##### */
        public override string CityMenu_SalePricesTitle => "Ceny sprzedaży";
        public override string Blueprint_Title => "Projekt";
        public override string Resource_Tab_Overview => "Przegląd";
        public override string Resource_Tab_Stockpile => "Zapasy";

        public override string Resource => "Surowiec";
        public override string Resource_StockPile_Info => "Ustal docelową ilość surowców w magazynie; poinformuje to pracowników, kiedy powinni zająć się innymi zasobami.";
        public override string Resource_TypeName_Water => "woda";
        public override string Resource_TypeName_Wood => "drewno";
        public override string Resource_TypeName_Fuel => "opał";
        public override string Resource_TypeName_Stone => "kamień";
        public override string Resource_TypeName_RawFood => "surowa żywność";
        public override string Resource_TypeName_Food => "żywność";
        public override string Resource_TypeName_Beer => "piwo";
        public override string Resource_TypeName_Wheat => "pszenica";
        public override string Resource_TypeName_Linen => "len";
        //public override string Resource_TypeName_SkinAndLinen => "skóra i len";
        public override string Resource_TypeName_IronOre => "ruda żelaza";
        public override string Resource_TypeName_GoldOre => "ruda złota";
        public override string Resource_TypeName_Iron => "żelazo";

        public override string Resource_TypeName_SharpStick => "Zaostrzony kij";
        public override string Resource_TypeName_Sword => "Miecz";
        public override string Resource_TypeName_KnightsLance => "Kopia rycerska";
        public override string Resource_TypeName_TwoHandSword => "Zweihänder";
        public override string Resource_TypeName_Bow => "Łuk";

        public override string Resource_TypeName_LightArmor => "Lekki pancerz";
        public override string Resource_TypeName_MediumArmor => "Średni pancerz";
        public override string Resource_TypeName_HeavyArmor => "Ciężki pancerz";

        public override string ResourceType_Children => "Dzieci";

        public override string BuildingType_DefaultName => "Budynek";
        public override string BuildingType_WorkerHut => "Chata pracownicza";
        public override string BuildingType_Brewery => "Browar";
        public override string BuildingType_Postal => "Poczta";
        public override string BuildingType_Recruitment => "Punkt rekrutacyjny";
        public override string BuildingType_Barracks => "Koszary";
        public override string BuildingType_PigPen => "Zagroda dla świń";
        public override string BuildingType_HenPen => "Kurnik";
        public override string BuildingType_WorkBench => "Warsztat";
        public override string BuildingType_Carpenter => "Zakład ciesielski";
        public override string BuildingType_CoalPit => "Mielerz";
        public override string DecorType_Statue => "Posąg";
        public override string DecorType_Pavement => "Bruk";
        public override string BuildingType_Smith => "Kuźnia";
        public override string BuildingType_Cook => "Kuchnia";
        public override string BuildingType_Storehouse => "Skład";

        public override string BuildingType_ResourceFarm => "Farma ({0})";

        public override string BuildingType_WorkerHut_DescriptionLimitX => "Zwiększa limit pracowników o {0}";
        public override string BuildingType_Tavern_Description => "Miejsce posiłków dla pracowników";
        public override string BuildingType_Tavern_Brewery => "Produkcja piwa";
        public override string BuildingType_Postal_Description => "Wysyłaj surowce do innych miast";
        public override string BuildingType_Recruitment_Description => "Wysyłaj ludzi do innych miast";
        public override string BuildingType_Barracks_Description => "Wykorzystuje ludzi i ekwipunek do szkolenia żołnierzy";
        public override string BuildingType_PigPen_Description => "Hodowla świń dostarczająca żywność i skóry";
        public override string BuildingType_HenPen_Description => "Hodowla kur i jaj dostarczająca żywność";
        public override string BuildingType_Decor_Description => "Dekoracja";
        public override string BuildingType_Farm_Description => "Uprawa surowców";

        public override string BuildingType_Cook_Description => "Stanowisko przygotowywania żywności";
        public override string BuildingType_Bench_Description => "Stanowisko rzemieślnicze";

        public override string BuildingType_Smith_Description => "Stanowisko obróbki metalu";
        public override string BuildingType_Carpenter_Description => "Stanowisko obróbki drewna";

        public override string BuildingType_Nobelhouse_Description => "Siedziba rycerzy i dyplomatów";
        public override string BuildingType_CoalPit_Description => "Wydajna produkcja opału";
        //public override string BuildingType_Storehouse_Description => "Punkt zrzutu surowców";

        public override string MenuTab_Info => "Info";
        public override string MenuTab_Work => "Praca";
        public override string MenuTab_Recruit => "Rekrutuj";
        public override string MenuTab_Resources => "Surowce";
        public override string MenuTab_Trade => "Handel";
        public override string MenuTab_Build => "Buduj";
        public override string MenuTab_Economy => "Ekonomia";
        public override string MenuTab_Delivery => "Dostawy";

        public override string MenuTab_Build_Description => "Wznosi budynki w Twoim mieście";
        public override string MenuTab_BlackMarket_Description => "Dostęp do budynków czarnego rynku";
        public override string MenuTab_Resources_Description => "Zarządzaj surowcami miasta";
        public override string MenuTab_Work_Description => "Ustaw zadania dla pracowników";
        public override string MenuTab_Automation_Description => "Ustawienia automatyzacji miasta";

        public override string BuildHud_OutsideCity => "Poza regionem miasta";
        public override string BuildHud_OutsideFaction => "Poza Twoimi granicami!";

        public override string BuildHud_OccupiedTile => "Pole zajęte";

        public override string Build_PlaceBuilding => "Budynek";
        public override string Build_DestroyBuilding => "Zburz";
        public override string Build_ClearTerrain => "Oczyść teren";

        public override string Build_ClearOrders => "Wyczyść rozkazy budowy";
        public override string Build_Order => "Rozkaz budowy";
        public override string Build_OrderQue => "Kolejka budowy: {0}";
        public override string Build_AutoPlace => "Automatyczne ustawianie";

        public override string Work_OrderPrioTitle => "Priorytet pracy";
        public override string Work_OrderPrioDescription => "Priorytet od 1 (niski) do {0} (wysoki)";

        public override string Work_OrderPrio_No => "Brak priorytetu. Zadanie nie będzie wykonywane.";
        public override string Work_OrderPrio_Min => "Minimalny priorytet.";
        public override string Work_OrderPrio_Max => "Maksymalny priorytet.";

        public override string Work_Move => "Przenieś przedmioty";

        public override string Work_GatherXResource => "Zbierz {0}";
        public override string Work_CraftX => "Wytwórz {0}";
        public override string Work_Farming => "Rolnictwo";
        public override string Work_Mining => "Górnictwo";
        public override string Work_Trading => "Handel";

        public override string Work_AutoBuild => "Automatyczna budowa i ekspansja";

        public override string WorkerHud_WorkType => "Status pracy: {0}";
        public override string WorkerHud_Carry => "Niesie: {0} {1}";
        public override string WorkerHud_Energy => "Energia: {0}";
        public override string WorkerStatus_Exit => "Opuść siłę roboczą";
        public override string WorkerStatus_Eat => "Jedzenie";
        public override string WorkerStatus_Till => "Uprawa roli";
        public override string WorkerStatus_Plant => "Sadzenie";
        public override string WorkerStatus_Gather => "Zbieranie";
        public override string WorkerStatus_PickUpResource => "Podnoszenie surowca";
        public override string WorkerStatus_DropOff => "Składowanie";
        public override string WorkerStatus_BuildX => "Budowanie: {0}";
        public override string WorkerStatus_TrossReturnToArmy => "Powrót do armii";

        public override string Hud_ToggleFollowFaction => "Przełącz ustawienia globalne frakcji";
        public override string Hud_FollowFaction_Yes => "Używa ustawień globalnych frakcji";
        public override string Hud_FollowFaction_No => "Używa ustawień lokalnych (Globalna wartość: {0})";

        public override string Hud_Idle => "Bezczynny";
        public override string Hud_NoLimit => "Bez limitu";

        public override string Hud_None => "Brak";
        public override string Hud_ProductionQueue => "Kolejka produkcji";

        public override string Hud_EmptyList => "- Pusta lista -";

        public override string Hud_RequirementOr => "- lub -";

        public override string Hud_BlackMarket => "Czarny rynek";

        public override string Language_CollectProgress => "{0} / {1}";
        public override string Hud_SelectCity => "Wybierz miasto";

        public override string Conscription_Title => "Pobór";
        public override string Conscript_WeaponTitle => "Broń";
        public override string Conscript_ArmorTitle => "Pancerz";
        public override string Conscript_TrainingTitle => "Szkolenie";

        public override string Conscript_SpecializationTitle => "Specjalizacja";
        public override string Conscript_SpecializationDescription => "Zwiększy atak w jednym obszarze i zmniejszy we wszystkich pozostałych o {0}";
        public override string Conscript_SelectBuilding => "Wybierz koszary";

        public override string Conscript_WeaponDamage => "Obrażenia broni";
        public override string Conscript_ArmorHealth => "Wytrzymałość pancerza";
        public override string Conscript_AttackSpeed => "Szybkość ataku";
        public override string Conscript_TrainingTime => "Czas szkolenia";

        public override string Conscript_Training_Minimal => "Minimalne";
        public override string Conscript_Training_Basic => "Podstawowe";
        public override string Conscript_Training_Skillful => "Biegłe";
        public override string Conscript_Training_Professional => "Profesjonalne";

        public override string Conscript_Specialization_Field => "Otwarte pole";
        public override string Conscript_Specialization_Sea => "Statek";
        public override string Conscript_Specialization_Siege => "Oblężenie";
        public override string Conscript_Specialization_Traditional => "Tradycyjna";
        public override string Conscript_Specialization_AntiCavalry => "Przeciw kawalerii";

        public override string Conscription_Status_CollectingEquipment => "Zbieranie ekwipunku: {0}";
        public override string Conscription_Status_CollectingMen => "Pobór ludzi: {0}";
        public override string Conscription_Status_Training => "Szkolenie: {0}";

        public override string ArmyHud_Food_Reserves_X => "Zapasy żywności: {0}";
        public override string ArmyHud_Food_Upkeep_X => "Utrzymanie (jedzenie): {0}";
        public override string ArmyHud_Food_Costs_X => "Koszty żywności: {0}";

        public override string Deliver_WillSendXInfo => "Wyśle {0} naraz";
        public override string Delivery_ListTitle => "Wybierz usługę dostawy";
        public override string Delivery_DistanceX => "Dystans: {0}";
        public override string Delivery_DeliveryTimeX => "Czas dostawy: {0}";
        public override string Delivery_SenderMinimumCap => "Minimalny limit nadawcy";
        public override string Delivery_RecieverMaximumCap => "Maksymalny limit odbiorcy";
        public override string Delivery_ItemsReady => "Przedmioty gotowe";
        public override string Delivery_RecieverReady => "Odbiorca gotowy";
        public override string Hud_ThisCity => "To miasto";
        public override string Hud_RecieveingCity => "Miasto docelowe";

        public override string Info_ButtonIcon => "i";

        public override string Info_ResourcePerSecond => "Wyświetlane jako Surowce na sekundę.";

        public override string Info_MinuteAverage => "Wartość jest średnią z ostatniej minuty";

        public override string Message_OutOfFood_Title => "Brak żywności";
        public override string Message_CityOutOfFood_Text => "Kosztowna żywność zostanie kupiona na czarnym rynku. Pracownicy zaczną głodować, gdy zabraknie Ci pieniędzy.";

        public override string Hud_EndSessionIcon => "X";

        public override string TerrainType => "Typ terenu";

        public override string Hud_EnergyUpkeepX => "Utrzymanie energii (jedzenie): {0}";

        public override string Hud_EnergyAmount => "{0} energii (sekundy pracy)";

        public override string Hud_CopySetup => "Kopiuj ustawienia";
        public override string Hud_Paste => "Wklej";

        public override string Hud_Available => "Dostępne";

        public override string WorkForce_ChildBirthRequirements => "Wymagania narodzin dzieci";
        public override string WorkForce_AvailableHomes => "Dostępne domy: {0}";

        /// <summary>
        /// workers require peace to grow(make babies)
        /// </summary>
        public override string WorkForce_Peace => "Pokój";
        public override string WorkForce_ChildToManTime => "Wiek dorosłości: {0} min";

        public override string Economy_TaxIncome => "Przychód z podatków: {0}";
        public override string Economy_ImportCostsForResource => "Koszty importu ({0}): {1}";
        public override string Economy_BlackMarketCostsForResource => "Koszty czarnorynkowe ({0}): {1}";
        public override string Economy_GuardUpkeep => "Utrzymanie straży: {0}";

        public override string Economy_LocalCityTrade_Export => "Eksport handlowy miasta: {0}";
        public override string Economy_LocalCityTrade_Import => "Import handlowy miasta: {0}";

        public override string Economy_ResourceProduction => "{0} - produkcja: {1}";
        public override string Economy_ResourceSpending => "{0} - wydatki: {1}";

        public override string Economy_TaxDescription => "Podatek wynosi {0} złota od każdego pracownika";

        public override string Economy_SoldResources => "Sprzedane surowce (ruda złota): {0}";

        public override string UnitType_Cities => "Miasta";
        public override string UnitType_Armies => "Armie";
        public override string UnitType_Worker => "Pracownik";

        public override string UnitType_FootKnight => "Rycerz z mieczem długim";
        public override string UnitType_CavalryKnight => "Rycerz konny";

        public override string CityCulture_LargeFamilies => "Wielodzietne rodziny";
        public override string CityCulture_FertileGround => "Żyzne ziemie";
        public override string CityCulture_Archers => "Wyborni łucznicy";
        public override string CityCulture_Warriors => "Wojownicy";
        public override string CityCulture_AnimalBreeder => "Hodowcy zwierząt";
        public override string CityCulture_Miners => "Górnicy";
        public override string CityCulture_Woodcutters => "Drwale";
        public override string CityCulture_Builders => "Budowniczowie";

        /// <summary>
        /// Crab mentality: culture where you suppress those who are better at something
        /// </summary>
        public override string CityCulture_CrabMentality => "Krabia mentalność";
        public override string CityCulture_DeepWell => "Głęboka studnia";
        public override string CityCulture_Networker => "Organizator";

        /// <summary>
        /// Pit master: someone who is good at burning work (char coal) 
        /// </summary>
        public override string CityCulture_PitMasters => "Mistrzowie mielerzy";

        public override string CityCulture_Culture => "Kultura";
        public override string CityCulture_LargeFamilies_Description => "Zwiększony przyrost naturalny";
        public override string CityCulture_FertileGround_Description => "Obfitsze plony z upraw";
        public override string CityCulture_Archers_Description => "Pozwala szkolić wybitnych łuczników";
        public override string CityCulture_Warriors_Description => "Pozwala szkolić wybitnych wojowników";
        //public override string CityCulture_AnimalBreeder_Description => "Zwierzęta dają więcej surowców";
        public override string CityCulture_Miners_Description => "Wydajniejsze wydobycie rudy";
        public override string CityCulture_Woodcutters_Description => "Drzewa dają więcej drewna";
        public override string CityCulture_Builders_Description => "Szybsze budowanie";
        public override string CityCulture_CrabMentality_Description => "Praca kosztuje mniej energii. Brak możliwości szkolenia elitarnych jednostek.";
        public override string CityCulture_DeepWell_Description => "Zasoby wody odnawiają się szybciej";
        public override string CityCulture_Networker_Description => "Wydajniejsza poczta i komunikacja";
        public override string CityCulture_PitMasters_Description => "Wyższa produkcja opału";

        public override string CityOption_AutoBuild_Work => "Auto-rozbudowa siły roboczej";
        public override string CityOption_AutoBuild_Farm => "Auto-rozbudowa farm";

        public override string Hud_PurchaseTitle_Resources => "Kup surowce";
        public override string Hud_PurchaseTitle_CurrentlyOwn => "Posiadasz";

        public override string Tutorial_EndTutorial => "Zakończ tutorial";
        public override string Tutorial_MissionX => "Misja {0}";
        public override string Tutorial_CollectXAmountOfY => "Zbierz {0} {1}";
        public override string Tutorial_SelectTabX => "Wybierz zakładkę: {0}";
        public override string Tutorial_IncreasePriorityOnX => "Zwiększ priorytet dla: {0}";
        public override string Tutorial_PlaceBuildOrder => "Wydaj rozkaz budowy: {0}";
        public override string Tutorial_ZoomInput => "Zoom";

        public override string Tutorial_SelectACity => "Wybierz miasto";
        public override string Tutorial_ZoomInWorkers => "Przybliż widok (Zoom), by zobaczyć pracowników";
        public override string Tutorial_CreateSoldiers => "Stwórz dwa oddziały żołnierzy z tym ekwipunkiem: {0}. {1}.";
        public override string Tutorial_ZoomOutOverview => "Oddal widok do podglądu mapy";
        public override string Tutorial_ZoomOutDiplomacy => "Oddal widok do widoku dyplomacji";
        public override string Tutorial_ImproveRelations => "Popraw swoje relacje z sąsiednią frakcją";
        public override string Tutorial_MissionComplete_Title => "Misja ukończona!";
        public override string Tutorial_MissionComplete_Unlocks => "Odblokowano nowe opcje sterowania";

        //patch1
        public override string Resource_ReachedStockpile => "Osiągnięto docelowy bufor zapasów";

        public override string BuildingType_ResourceMine => "Kopalnia ({0})";

        public override string Resource_TypeName_BogIron => "ruda darniowa";

        public override string Resource_TypeName_Coal => "węgiel";

        public override string Language_XUpkeep => "Utrzymanie ({0})";
        public override string Language_XCountIsY => "Liczba ({0}): {1}";

        public override string Message_ArmyOutOfFood_Text => "Droga żywność zostanie kupiona na czarnym rynku. Głodni żołnierze zaczną dezerterować, gdy zabraknie Ci złota.";

        public override string Info_ArmyFood1 => "Armie będą uzupełniać prowiant w najbliższym przyjaznym mieście.";
        public override string Info_ArmyFood2 => "Prowiant można kupić od innych frakcji.";
        public override string Info_ArmyFood3 => "Na wrogich terytoriach prowiant można kupić tylko na czarnym rynku.";
        public override string FactionName_Monger => "Monger";
        public override string FactionName_Hatu => "Hatu";
        public override string FactionName_Destru => "Destru";

        //patch2
        public override string Tutorial_BuildSomething => "Zbuduj coś, co produkuje: {0}";
        public override string Tutorial_BuildCraft => "Zbuduj warsztat rzemieślniczy dla: {0}";
        public override string Tutorial_IncreaseBufferLimit => "Zwiększ limit bufora dla: {0}";

        /// <summary>
        /// 0: count, 1: item type
        /// </summary>
        public override string Tutorial_CollectItemStockpile => "Zgromadź zapasy w ilości: {0} {1}";
        public override string Tutorial_LookAtFoodBlueprint => "Spójrz na projekt (blueprint) żywności";
        public override string Tutorial_CollectFood_Info1 => "Pracownicy udają się do ratusza, aby zjeść";
        public override string Tutorial_CollectFood_Info2 => "Armia wysyła tabor (tross), aby zebrać żywność";
        public override string Tutorial_CollectFood_Info0 => "Chcesz pełnej kontroli nad pracownikami? Ustaw wszystkie priorytety pracy na zero, a potem aktywuj je po kolei.";

        public override string EndGameStatistics_DecorsBuilt => "Zbudowane dekoracje: {0}";
        public override string EndGameStatistics_StatuesBuilt => "Zbudowane posągi: {0}";


        //############
        // XMAS UPDATE
        //############
        public override string Info_FoodAndDeliveryLocation => "Domyślnie pracownicy jedzą i składowują przedmioty w ratuszu";
        public override string GameMenu_UseSpeedX => "Opcja prędkości: {0}";
        public override string GameMenu_LongerBuildQueue => "Wydłużona kolejka budowy";

        public override string Diplomacy_RelationWithOthers => "Ich relacje z innymi";
        public override string Automation_queue_description => "Będzie powtarzać, aż kolejka zostanie opróżniona";

        public override string BuildingType_Storehouse_Description => "Pracownicy mogą tu składować przedmioty";

        public override string Resource_TypeName_Longbow => "długi łuk";
        public override string Resource_TypeName_Rapeseed => "rzepak";
        public override string Resource_TypeName_Hemp => "konopie";

        public override string Resource_BogIronDescription => "Wydobycie żelaza w kopalni jest bardziej efektywne niż zbieranie rudy darniowej.";


        public override string Resource_FoodSafeGuard_Description => "Zabezpieczenie. Ustawi maksymalny priorytet dla łańcucha produkcji żywności, jeśli jej ilość spadnie poniżej {0}.";
        public override string Resource_FoodSafeGuard_Active => "Zabezpieczenie żywności jest aktywne.";

        public override string GameMenu_NextSong => "Następny utwór";

        public override string BuildingType_Bank => "Bank";
        public override string BuildingType_GoldDelivery_Description => "Wysyłaj złoto do innych miast";

        public override string BuildingType_Logistics => "Logistyka";
        public override string BuildingType_Logistics_Description => "Ulepsz swoje możliwości zlecania budowy";

        public override string BuildingType_Logistics_NationSizeRequirement => "Całkowita siła robocza narodu: {0}";
        public override string Requirements_XItemStorageOfY => "Magazyn miasta {0}: {1}";


        public override string XP_UnlockBuildQueue => "Odblokuj kolejkę budowy do: {0}";
        public override string XP_UnlockBuilding => "Odblokuj budynek: ";
        public override string XP_Upgrade => "Ulepszenie";

        public override string XP_UpgradeBuildingX => "Ulepsz budynek: {0}";

        /// <summary>
        /// Title for describing the production cycle of farms
        /// </summary>
        public override string BuildHud_PerCycle => "Na cykl";
        public override string BuildHud_MayCraft => "Może wytworzyć";
        public override string BuildHud_WorkTime => "Czas pracy: {0}";
        public override string BuildHud_GrowTime => "Czas wzrostu: {0}";
        public override string BuildHud_Produce => "Produkcja:";

        public override string BuildHud_Queue => "Dozwolona kolejka budowy: {0}/{1}";

        public override string LandType_Flatland => "Równina";
        public override string LandType_Water => "Woda";
        public override string BuildingType_Wall => "Mur";
        public override string Delivery_AutoReciever_Description => "Wyśle do miasta z najmniejszą ilością zasobów";

        public override string Hud_On => "Wł.";
        public override string Hud_Off => "Wył.";

        public override string Hud_Time_Seconds => "{0} sekund";
        public override string Hud_Time_Minutes => "{0} minut";
        public override string Hud_Undo => "Cofnij (Undo)";
        public override string Hud_Redo => "Ponów (Redo)";

        public override string Tag_ViewOnMap => "Pokaż tagi na mapie";

        public override string MenuTab_Tag => "Tag";

        public override string Input_Build => "Buduj";

        public override string FlagEditor_ClearAll => "Wyczyść wszystko";

        public override string CityCulture_Stonemason => "Kamieniarstwo";
        public override string CityCulture_Stonemason_Description => "Ulepszone wydobycie kamienia";

        public override string CityCulture_Brewmaster => "Piwowarstwo";
        public override string CityCulture_Brewmaster_Description => "Zwiększona produkcja piwa";

        public override string CityCulture_Weavers => "Tkactwo";
        public override string CityCulture_Weavers_Description => "Zwiększona produkcja lekkich pancerzy";

        public override string CityCulture_SiegeEngineer => "Inżynieria oblężnicza";
        public override string CityCulture_SiegeEngineer_Description => "Potężniejsze machiny wojenne";

        public override string CityCulture_Armorsmith => "Płatnerstwo";
        public override string CityCulture_Armorsmith_Description => "Ulepszona produkcja żelaznych pancerzy";

        public override string CityCulture_Noblemen => "Szlachta";
        public override string CityCulture_Noblemen_Description => "Silniejsi rycerze";

        public override string CityCulture_Seafaring => "Żeglarstwo";
        public override string CityCulture_Seafaring_Description => "Żołnierze ze specjalizacją morską dysponują silniejszymi okrętami";

        public override string CityCulture_Backtrader => "Pokątny handel";
        public override string CityCulture_Backtrader_Description => "Tańszy czarny rynek";

        public override string CityCulture_LawAbiding => "Praworządność";
        public override string CityCulture_LawAbiding_Description => "Wyższe wpływy z podatków. Brak dostępu do czarnego rynku.";

        //##2##

        public override string Hud_Advanced => "Zaawansowane";
        public override string Hud_Loading => "Wczytywanie...";

        public override string CityOption_LowerGuardSize => "Zwolnij strażników";
        public override string Hud_Purchase_MinCapacity => "Osiągnięto minimalną wydajność";
        public override string Settings_ResetToDefault => "Resetuj do domyślnych";
        public override string Settings_NewGame => "Nowa gra";

        public override string Settings_AdvancedGameSettings => "Zaawansowane ustawienia gry";
        public override string Settings_FoodMultiplier => "Mnożnik żywności";
        public override string Settings_FoodMultiplier_Description => "Określa, na jak długo pracownikowi lub żołnierzowi wystarcza pełny żołądek. Wysoka wartość obniży wydajność komputera.";

        public override string Settings_GameMode => "Tryb gry";

        public override string Settings_Mode_Story => "Pełna opowieść";
        public override string Settings_Mode_IncludeBoss => "Włącz wydarzenia z Bossami";
        public override string Settings_Mode_IncludeAttacks => "Włącz losowe ataki";
        public override string Settings_Mode_Sandbox => "Sandbox (Piaskownica)";
        public override string Settings_Mode_Peaceful => "Tryb pokojowy";
        public override string Settings_Mode_Peaceful_Description => "Wszystkie wojny są inicjowane przez gracza";

        public override string Lobby_ImportSave => "Importuj zapis";

        public override string Lobby_ExportSave => "Eksportuj zapis";
        public override string Lobby_ExportSave_Description => "Tworzy kopię pliku i umieszcza ją w folderze importu: {0}";

        public override string Resource_CurrentAmount => "Obecna ilość: {0}";
        public override string Resource_MaxAmount_Soft => "Miękki limit (Maks.): {0}";
        public override string Resource_MaxAmount => "Limit maksymalny: {0}";
        public override string Resource_AddPerSec => "Tempo przyrostu: {0} na sekundę";

        public override string Resource_WaterAddLimit => "Tempo przyrostu wody nie może być zmienione";

        public override string Tutorial_Select_SubTab => "I wybierz kategorię: {0}";

        public override string Tutorial_OpenGuardSubTab => "Otwórz koszary i wybierz kategorię: {0}";
        public override string Tutorial_GuardToWall => "Przemieść strażnika na mur";
        public override string Demo_MissionObjective_Title => "Cel misji";
        public override string Demo_MissionObjective_Description => "Obroń się przed atakiem z południa";
        public override string Demo_Complete_Title => "Demo ukończone";
        public override string Demo_TimesUp_Title => "Czas minął!";
        public override string Demo_EndInOneMinuteDescription => "Demo zakończy się za minutę";

        public override string ArmyOption_NewArmy => "Nowa armia";
        public override string ProfileEditor_AltMain => "Alternatywny główny";
        public override string Automation_CheckBoxTitle => "Zautomatyzowane";

        public override string ArmyStructure_ColumnWidth => "Szerokość kolumny armii";
        public override string ArmyStructure_ArmyPlacement => "Rozmieszczenie w armii";
        public override string ArmyStructure_Row_Front => "Front";
        public override string ArmyStructure_Row_Body => "Centrum";
        public override string ArmyStructure_Row_Second => "Druga linia";
        public override string ArmyStructure_Row_Behind => "Tyły";

        public override string Diplomacy_RelationType_Enemies => "Wrogowie";

        public override string EventMessage_EnemyAlliance_Title => "Strach przed dominacją";
        public override string EventMessage_EnemyAlliance => "Narody, obawiając się Twojej rosnącej potęgi, jednoczą się w sojuszu przeciwko Tobie.";

        public override string Settings_CentralGold => "Centralne złoto";
        public override string Settings_CentralGold_Description => "Wł.: całe złoto trafia do wspólnej puli do natychmiastowego użycia. Wył.: złoto ma formę fizyczną i musi być transportowane.";

        public override string InputActionName_StopStart => "Stop/Start";
        public override string InputActionName_ToggleHudDetail => "Przełącz szczegóły HUD";
        public override string InputActionName_NextCity => "Następne miasto";
        public override string InputActionName_NextArmy => "Następna armia";
        public override string InputActionName_NextBattle => "Następna bitwa";
        public override string InputActionName_Build => "Buduj";
        public override string InputActionName_Copy => "Kopiuj";
        public override string InputActionName_Paste => "Wklej";
        public override string InputActionName_Menu => "Menu";
        public override string InputActionName_FlagDesign_ToggleColor_Prev => "Poprzedni kolor";
        public override string InputActionName_FlagDesign_ToggleColor_Next => "Następny kolor";
        public override string InputActionName_FlagDesign_PaintBucket => "Wiaderko";
        public override string InputActionName_Controller_FlagDesign_Colorpicker => "Selektor kolorów (Color Picker)";
        public override string InputActionName_ControllerFocus => "Focus";
        public override string InputActionName_ControllerCancel => "Anuluj";
        public override string InputActionName_ControllerMessageClick => "Kliknięcie wiadomości";
        public override string InputActionName_ControllerSelect => "Wybierz";
        public override string InputActionName_WASD_UP => "Góra";
        public override string InputActionName_WASD_DOWN => "Dół";
        public override string InputActionName_WASD_LEFT => "Lewo";
        public override string InputActionName_WASD_RIGHT => "Prawo";
        public override string InputActionName_CameraTiltLeft => "Pochyl kamerę w lewo";
        public override string InputActionName_CameraTiltRight => "Pochyl kamerę w prawo";
        public override string InputActionName_CameraTiltUp => "Pochyl kamerę w górę";
        public override string InputActionName_ZoomInKey => "Przybliż (Zoom In)";
        public override string InputActionName_ZoomOutKey => "Oddal (Zoom Out)";

        public override string Settings_Title_Monitor => "Opcje monitora";
        public override string Settings_Title_Graphics => "Opcje graficzne";
        public override string Settings_Title_Input => "Sterowanie";
        public override string Settings_Title_Gameplay => "Opcje rozgrywki";
        public override string Settings_PanOnZoom => "Przesuń przy zoomie";
        public override string Settings_ScrollSensitivity_Game => "Czułość przewijania: gra";
        public override string Settings_ScrollSensitivity_Menu => "Czułość przewijania: menu";
        public override string Settings_Blood => "Krew";

        public override string Settings_MasterVolume => "Głośność ogólna";
        public override string Settings_AmbienceVolume => "Głośność otoczenia";
        public override string Settings_BattleMelody => "Muzyka bitewna";

        public override string Settings_ModelLight => "Oświetlenie modeli";
        public override string Settings_Particles => "Efekty cząsteczkowe";
        public override string Settings_MapLoadSpeed => "Szybkość ładowania mapy";
        public override string Lobby_Category_Options => "Opcje";
        public override string Lobby_Category_Editor => "Edytor";
        public override string Lobby_Category_ExtraModes => "Dodatkowe tryby";

        public override string Lobby_Editor_MapEditor => "Edytor map";
        public override string Lobby_Editor_VoxelEditor => "Edytor voxeli";

        public override string Lobby_Mode_BattleLab => "Battle Lab";
        public override string Lobby_Mode_BattleLab_Description => "Wystaw dowolne jednostki przeciwko sobie";
        public override string Lobby_Mode_Commander => "Graj w Commander";
        public override string Lobby_Mode_Commander_Description => "Mała, taktyczna gra planszowa";
        public override string Lobby_MusicPlayList => "Lista odtwarzania muzyki";

        public override string Lobby_GameSetup => "Konfiguracja gry";
        public override string Lobby_PlayerSetup => "Ustawienia gracza";
        public override string LobbyDemoMode_Demo => "Demo";

        public override string Lobby_Tutorial => "Tutorial";

        public override string LobbyDemoMode_ShortTutorial => "Szybki Tutorial";
        public override string LobbyDemoMode_LongTutorial => "Rozszerzony Tutorial";

        /// <summary>
        /// Says wishlist on, followed by the STEAM logo
        /// </summary>
        public override string LobbyDemoMode_WishlistOn => "Dodaj do Wishlisty na";

        public override string BattleLab_StartHere => "Rozpocznij bitwę tutaj";
        public override string BattleLab_Start => "Start bitwy";
        public override string BattleLab_Attacker => "Atakujący";



        public override string MapGenerator_Name => "Generator mapy - generowanie";

        public override string MapType_CustomMap => "Mapa Custom (własna)";
        public override string MapType_GenerateNewMap => "Generuj nową mapę";
        public override string MapGenerator_GenerateAction => "Generuj";
        public override string MapGenerator_Terrain_CustomSize => "Własny rozmiar";
        public override string MapGenerator_Terrain_StartAs => "Zacznij jako";
        public override string MapGenerator_Terrain_ClearPass => "Uruchom fazę czyszczenia";
        public override string MapGenerator_Terrain_BuildPass => "Uruchom fazę budowania";
        public override string MapGenerator_Terrain_DigPass => "Uruchom fazę kopania";
        public override string MapGenerator_Terrain_BuildDigLoops => "Liczba cykli buduj-kop";
        public override string MapGenerator_Terrain_BuildStrokes => "Liczba pociągnięć budujących";
        public override string MapGenerator_Terrain_BuildStrokes_Description => "Mierzone w pociągnięciach pędzla na 100 pól";
        public override string MapGenerator_Terrain_DigStrokes => "Liczba pociągnięć kopiących";
        public override string MapGenerator_Terrain_CleanUp_Option => "Sprzątanie pojedynczych pól";
        public override string MapGenerator_Terrain_CleanUpPass => "Uruchom fazę sprzątania";



        public override string Economy_ServicemenUpkeep => "Utrzymanie personelu: {0}";
        public override string Economy_ServicemenUpkeep_Description => "Koszt utrzymania to {0} złota za pracownika";
        public override string Economy_GuardUpkeep_Description => "Koszt utrzymania to {0} złota za strażnika";

        public override string EndScreen_TimeHasEndedTitle => "Czas minął";

        public override string Hud_AdvancedSettings => "Ustawienia zaawansowane";
        public override string Hud_Vector_X => "X";
        public override string Hud_Vector_Y => "Y";
        public override string Hud_Cancel => "Anuluj";
        public override string Hud_Delete => "Usuń";
        public override string Hud_Next => "Dalej";
        //public override string Hud_None => "None";
        public override string Hud_Apply => "Zastosuj";
        public override string Hud_AllCities => "Wszystkie miasta";
        public override string Hud_Time_Hours => "{0} godz.";
        public override string Hud_AddX => "Dodaj {0}";
        public override string Hud_Both => "Oba";
        public override string Hud_Direction => "Kierunek";


        /// <summary>
        /// 0: object collection type name, 1: number of objects
        /// </summary>
        public override string Hud_ObjectsAndCount => "{0}, liczba: {1}";

        public override string Hud_EffectDoesNotStack => "Ten efekt nie kumuluje się";

        public override string Work_SmeltX => "Przetapiaj: {0}";

        public override string Info_TotalFoodProduction => "Całkowita produkcja żywności";
        public override string Info_TotalFoodSpending => "Całkowite wydatki na żywność";

        public override string Info_FooodAndDeliveryLocation => "Domyślnie pracownicy jedzą lub składowują przedmioty w ratuszu";

        public override string Delivery_SendChunk => "Liczba przedmiotów na dostawę";
        public override string Delivery_SpeedBonus => "Bonus do szybkości: {0}%";

        public override string Delivery_AutoResourceDescription => "Dostarcza surowce, które osiągnęły limit zapasów, do miast w potrzebie.";

        public override string Conscript_Soldiers_ArmyType => "Żołnierze armii";
        public override string Conscript_Soldiers_ArmyType_Description => "Rekrutuj żołnierzy do pobliskiej armii";
        public override string Conscript_Soldiers_GuardType => "Straż miejska";
        public override string Conscript_Soldiers_GuardType_Description => "Strażnicy służą do obsadzania i wzmacniania murów";
        //-
        public override string Defence_Title => "Obrona";
        public override string Defence_GuardPost => "Posterunek straży";

        public override string Defence_WallDescription_Movement => "Utrudnia ruch wroga.";
        public override string Defence_WallDescription_GuardPost => "Można tu wystawić strażnika.";
        public override string Defence_AutoAssign => "Auto-przypisanie";
        public override string Defence_AutoAssign_Description => "Nowi strażnicy będą automatycznie trafiać na ten post";
        public override string Conscript_SplashDamage => "Obrażenia obszarowe (Splash)";
        public override string Conscript_HighSplashDamage => "Wysokie obrażenia obszarowe";

        public override string Conscript_Training_Champion => "Czempion";
        public override string Conscript_Training_Legendary => "Legendarny";

        public override string Experience_Title => "Doświadczenie";
        public override string Experience_TopExperience => "Najwyższe poziomy doświadczenia";

        public override string Experience_TimeReductionDescription => "Czas pracy skrócony o {0}% na poziom";

        public override string ExperienceType_Farm => "Rolnik";
        public override string ExperienceType_AnimalCare => "Hodowca zwierząt";
        public override string ExperienceType_HouseBuilding => "Budowniczy domów";
        public override string ExperienceType_WoodWork => "Obróbka drewna";
        public override string ExperienceType_StoneCutter => "Kamieniarz";
        public override string ExperienceType_Mining => "Górnik";
        public override string ExperienceType_Transport => "Tragarz";
        public override string ExperienceType_Cook => "Kucharz";
        public override string ExperienceType_Fletcher => "Łuczarz";
        public override string ExperienceType_RefineOre => "Hutnik";
        public override string ExperienceType_Casting => "Odlewnik";
        public override string ExperienceType_CraftMetal => "Kowal";
        public override string ExperienceType_CraftArmor => "Płatnerz";
        public override string ExperienceType_CraftWeapon => "Miecznik";
        public override string ExperienceType_CraftFuel => "Węglarz";
        public override string ExperienceType_Chemist => "Chemik";

        public override string ExperienceLevel_1 => "Początkujący";
        public override string ExperienceLevel_2 => "Praktykant";
        public override string ExperienceLevel_3 => "Ekspert";
        public override string ExperienceLevel_4 => "Mistrz";
        public override string ExperienceLevel_5 => "Legenda";

        public override string ExperenceOrDistancePrio_Title => "Wybór pracowników";
        public override string ExperenceOrDistancePrio_Description => "Wolni pracownicy będą wybierani do pracy na podstawie dystansu lub doświadczenia";


        public override string Technology_Description => "Każde miasto posiada drzewko technologiczne. Każda technologia odblokuje budynki i przedmioty.";
        public override string Experience_Description => "Pracownicy będą zdobywać doświadczenie i stawać się lepsi";


        public override string Technology_Title => "Technologia";
        public override string Technology_ShareField => "Wspólna dziedzina technologii";

        public override string Technology_GainByNeigborRelation => "Za każde sąsiednie miasto z tą technologią. Twoja relacja to {0}: {1}";
        public override string Technology_ForEachMaster => "Gdy {0} osiągnie poziom doświadczenia {1} w dziedzinie: {2}";
        public override string Technology_CitySpread => "Twoje miasta będą dzielić się technologią, gdy sąsiadują ze sobą: {0}";
        public override string Technology_CityCapture => "Większość technologii ulega zniszczeniu po zdobyciu miasta w bitwie";

        public override string Technology_AdvancedBuildings => "Zaawansowane budynki";
        public override string Technology_AdvancedFarming => "Zaawansowane rolnictwo";
        public override string Technology_AdvancedCasting => "Zaawansowane odlewnictwo";

        public override string Help_Title => "Pomoc";
        public override string Help_Work_Title => "Praca nie rusza";
        public override string Help_Work_Resources => "Budynki potrzebują dostępnych surowców";
        public override string Help_Work_Skill => "Pracownik potrzebuje odpowiedniego poziomu umiejętności (lub wyższego)";
        public override string Help_Work_Stockpile => "Zbieranie surowców zostanie przerwane, gdy magazyn będzie pełny";
        public override string Help_Work_Priority => "Zadanie może mieć niski lub zerowy priorytet";


        public override string Help_Soldiers_Title => "Produkcja żołnierzy";
        public override string Help_Soldiers_PlaceBuildingX => "Wznieś budynek: {0}";
        public override string Help_Soldiers_Workers => "Pracownicy dostępni do rekrutacji";
        public override string Help_Soldiers_Weapon => "Broń dla każdego żołnierza";
        public override string Help_Soldiers_StartX => "Start: {0}";


        public override string Hud_SelectHistory => "Wybierz historię";

        public override string Hud_PointsPerMinute => "{0} punktów na minutę";
        public override string Hud_PercentValueCost => "Usługa kosztuje {0}% wartości";

        public override string Hud_Mixed => "Mieszane";
        public override string Hud_Distance => "Dystans";

        public override string Hud_Unlock => "Odblokuj";
        public override string Hud_category => "Kategoria";

        /// <summary>
        /// Sets the game speed to one frame at a time
        /// </summary>
        public override string Input_StepOneFrame => "Przesuń o 1 klatkę";

        public override string Resource_TypeName_Wagon2Wheel => "Mały wóz";
        public override string Resource_TypeName_Wagon4Wheel => "Duży wóz";
        public override string Resource_TypeName_Tin => "Cyna";
        public override string Resource_TypeName_TinOre => "Ruda cyny";

        public override string Resource_TypeName_Copper => "Miedź";
        public override string Resource_TypeName_CopperOre => "Ruda miedzi";
        public override string Resource_TypeName_SilverOre => "Ruda srebra";
        public override string Resource_TypeName_Silver => "Srebro";

        /// <summary>
        /// Mithril is a fantasy metal
        /// </summary>
        public override string Resource_TypeName_RawMithril => "Surowy mithril";
        public override string Resource_TypeName_Mithril => "Mithril";

        public override string Resource_TypeName_BronzeSword => "Miecz z brązu";
        public override string Resource_TypeName_ShortSword => "Krótki miecz";
        public override string Resource_TypeName_LongSword => "Długi miecz";
        public override string Resource_TypeName_HandSpear => "Włócznia krótka";
        public override string Resource_TypeName_Warhammer => "Młot bojowy";
        public override string Resource_TypeName_MithrilSword => "Mithrilowy miecz";
        public override string Resource_TypeName_SlingShot => "Proca";
        public override string Resource_TypeName_ThrowingSpear => "Oszczep";
        public override string Resource_TypeName_Crossbow => "Kusza";
        public override string Resource_TypeName_MithrilBow => "Mithrilowy łuk";

        public override string Resource_TypeName_CoolingFluid => "Chłodziwo";
        public override string Resource_TypeName_Palisade => "Palisada";
        public override string Resource_TypeName_Toolkit => "Zestaw narzędzi";

        public override string Resource_TypeName_Sulfur => "Siarka";
        public override string Resource_TypeName_LeadOre => "Ruda ołowiu";
        public override string Resource_TypeName_Lead => "Ołów";
        public override string Resource_TypeName_Bronze => "Brąz";
        public override string Resource_TypeName_BloomIron => "Żelazo dymarkowe";
        public override string Resource_TypeName_Steel => "Stal";
        public override string Resource_TypeName_CastIron => "Żeliwo";

        public override string Resource_TypeName_BlackPowder => "Czarny proch";
        public override string Resource_TypeName_GunPowder => "Proch strzelniczy";
        public override string Resource_TypeName_LedBullet => "Ołowiany pocisk";

        public override string Resource_TypeName_HandCannon => "Ręczna armata";
        public override string Resource_TypeName_HandCulverin => "Ręczna kolubryna";
        public override string Resource_TypeName_Rifle => "Muszkiet";
        public override string Resource_TypeName_Blunderbuss => "Garłacz";

        public override string Resource_TypeName_Manuballista => "Manuballista";
        public override string Resource_TypeName_Catapult => "Katapulta";
        public override string Resource_TypeName_BatteringRam => "Taran";
        public override string Resource_TypeName_SiegeCannonBronze => "Bazyliszek";
        public override string Resource_TypeName_ManCannonBronze => "Bombarda";
        public override string Resource_TypeName_SiegeCannonIron => "Haubica";
        public override string Resource_TypeName_ManCannonIron => "Armata";

        public override string Resource_TypeName_PaddedArmor => "Przeszywanica";
        public override string Resource_TypeName_HeavyPaddedArmor => "Ciężka przeszywanica";

        public override string Resource_TypeName_IronArmor => "Kolczuga";
        public override string Resource_TypeName_HeavyIronArmor => "Ciężka kolczuga";

        public override string Resource_TypeName_BronzeArmor => "Pancerz z brązu";

        public override string Resource_TypeName_LightPlateArmor => "Zbroja płytowa";
        public override string Resource_TypeName_FullPlateArmor => "Pełna zbroja płytowa";
        public override string Resource_TypeName_MithrilArmor => "Mithrilowa zbroja";
        public override string Resource_TypeName_Coin => "Moneta";

        public override string UnitType_Warhammer => "Rycerz z młotem";

        public override string UnitType_SpearAndShield => "Liniowiec";

        public override string UnitType_CollectionOfSoldiers => "Zestaw żołnierzy";
        public override string UnitType_CollectionOfArmies => "Zestaw armii";

        /// <summary>
        /// The id tag will be a unique number
        /// </summary>
        public override string UnitId => "(id {0})";

        public override string BuildHud_AreaEffectTitle => "Efekt obszarowy";
        public override string BuildHud_BonusRadius => "Promień bonusu: {0}";

        public override string BuildHud_BuildTime => "Czas budowy";
        public override string SchoolHud_ToLevel => "Na poziom";
        public override string SchoolHud_TimeDescription => "Czas zakłada zerowe doświadczenie; maleje ono wraz ze wzrostem doświadczenia.";
        public override string SchoolHud_SelectSchool => "Wybierz szkołę";
        public override string Upgrade_Order => "Rozkaz ulepszenia";

        public override string Building_ListDescription => "Lista wszystkich budynków w tej kategorii";

        public override string BuildingType_IsUpgraded => "{0} - ulepszony";
        public override string BuildingType_WoodCutter => "Tartak";
        public override string BuildingType_Workshop_Description => "Ulepsza pracę w okolicy";

        public override string BuildingType_WoodCutter_AreaAffect => "Zdobądź {0}% więcej drewna z drzew";

        public override string BuildingType_StoneCutter_AreaAffect => "Zdobądź {0}% więcej kamienia";

        public override string BuildingType_StoneCutter => "Kamieniołom";

        public override string BuildingType_Embassy => "Ambasada";
        public override string BuildingType_Embassy_Description => "Do relacji dyplomatycznych";

        public override string BuildingType_SoldierBarracks => "Koszary żołnierzy";
        public override string BuildingType_ArcherBarracks => "Koszary łuczników";
        public override string BuildingType_WarmachineBarracks => "Koszary machin wojennych";
        public override string BuildingType_GunBarracks => "Koszary strzelców";
        public override string BuildingType_CannonBarracks => "Koszary artyleryjskie";
        public override string BuildingType_KnightsBarracks => "Koszary rycerzy";

        public override string BuildingType_WaterResovoir => "Zbiornik wody";
        public override string BuildingType_WaterResovoir_Description => "Zwiększa zapasy wody";

        public override string BuildingType_SmeltingFurnace => "Piec hutniczy";
        public override string BuildingType_SmeltingFurnace_Description => "Oczyszcza rudę w metal";

        public override string BuildingType_Foundry => "Odlewnia";
        public override string BuildingType_Foundry_Description => "Stanowisko odlewania metalu";

        public override string BuildingType_Armory => "Zbrojownia";
        public override string BuildingType_Armory_Description => "Stanowisko wytwarzania pancerzy";
        public override string BuildingType_Chemist => "Pracownia chemiczna";
        public override string BuildingType_Chemist_Description => "Stanowisko wytwarzania chemikaliów";
        public override string BuildingType_CoinMaker => "Mennica";
        public override string BuildingType_CoinMaker_Description => "Przetwarza metale w monety";
        public override string BuildingType_Gunmaker => "Rusznikarz";
        public override string BuildingType_Gunmaker_Description => "Stanowisko wytwarzania broni palnej i armat";

        public override string BuildingType_School_Tab => "Szkoła";
        public override string BuildingType_School => "Gildia mistrzów";
        public override string BuildingType_School_Description => "Zwiększa poziom umiejętności pracowników";

        public override string BuildingType_GoldDelivery => "Kurier złota";
        public override string BuildingType_Bank_Description => "Zarządzanie złotem";

        public override string DecorType_CobbleStones => "Bruk";
        public override string DecorType_Square => "Plac miejski";

        public override string DecorType_Garden => "Ogród";
        public override string DecorType_Flag => "Flaga";
        public override string DecorType_Banner => "Sztandar";

        public override string BuildingType_DirtRoad => "Droga gruntowa";
        public override string BuildingType_Palisade => "Fort palisadowy";

        public override string ResourceType_ServiceMen => "Personel";
        public override string BuildingType_ServiceHouse => "Budynek personelu";
        public override string BuildingType_ServiceHouse_DescriptionAddX => "Dodaje {0} pracowników personelu";

        public override string BuildingType_GuardOffice => "Biuro straży";
        public override string BuildingType_GuardOffice_DescriptionAddX => "Zwiększa limit straży o {0}";

        public override string BuildingType_DirtWall => "Wał ziemny";
        public override string BuildingType_DirtTower => "Wieża ziemna";
        public override string BuildingType_WoodWall => "Drewniany mur";
        public override string BuildingType_WoodTower => "Drewniana wieża";
        public override string BuildingType_StoneWall => "Kamienny mur";
        public override string BuildingType_StoneTower => "Kamienna wieża";
        public override string BuildingType_StoneGate => "Kamienna brama";
        public override string BuildingType_StoneHouse => "Kamienny dom";


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

        public override string BuildingToolShape_Free => "Pióro";
        public override string BuildingToolShape_Area => "Prostokąt";
        public override string BuildingToolShape_Line => "Linia";
        public override string BuildingToolShape_LShape => "Kształt L";

        public override string CityHall_Upgrade => "Rozbuduj ratusz";

        /// <summary>
        /// A cap on how many workers the city can have
        /// </summary>
        public override string CityHall_MaxSupportedWorkers => "Maks. liczba pracowników: {0}";

        public override string CityHall_Size_Small => "Wioska";
        public override string CityHall_Size_Medium => "Miasteczko";
        public override string CityHall_Size_Large => "Stolica";

        public override string GuardHousingCount => "Kwatery biura straży";
        public override string ServicemenCount => "Personel: {0}";


        public override string Work_MiningResource => "Wydobycie: {0}";

        public override string MenuTab_Progress => "Postęp";

        public override string Automation_AutomateCity => "Automatyzacja miasta";
        public override string Automation_AutomationFocus => "Cel automatyzacji";
        public override string Automation_AutomationFocus_Grow => "Rozwój";
        public override string Automation_AutomationFocus_Export => "Eksport";
        public override string Automation_AutomationFocus_War => "Wojna";

        public override string CityCulture_Smelters_Description => "Ulepszone wytapianie rudy";
        public override string CityCulture_Smelters => "Hutnicy";

        public override string CityCulture_Apprentices_Description => "Nowi pracownicy będą zdobywać doświadczenie od aktywnych pracowników";
        public override string CityCulture_Apprentices => "Uczniowie";

        public override string CityCulture_BronzeCasters_Description => "Ulepszona produkcja brązu i wyrobów z brązu";
        public override string CityCulture_BronzeCasters => "Odlewnicy brązu";

        //DEMO PATCH 1

        /// <summary>
        /// Evil orcs that roam on the map
        /// </summary>
        public override string FactionName_Barbarian => "Mroczna horda";
        public override string Tutorial_AttackAndDestroyX => "Atakuj i zniszcz: {0}";
        public override string Resource_TypeName_Pike => "pika";


        public override string BattleTrials_Title => "Próby bitewne";
        public override string BattleTrials_Description => "Przetestuj swoją taktykę w bezpośrednim starciu armia przeciw armii.";


        //DEMO PATCH 2
        public override string Conscript_BlockReducingAttack => "Te ataki zmniejszają szansę na blok";

        public override string Conscript_BlockPerSecond => "Może blokować {0} razy na sekundę";

        public override string Conscript_BlockDescription => "Żołnierze będą blokować większość ataków nadchodzących z przodu";

        public override string Map_CustomSeed => "Ziarno mapy";

        public override string Settings_Mode_Spectator => "Obserwator";

        //public override string Settings_Mode_Spectator_Description => "Just watch";

        public override string Automation_AutomationFocus_NoFocus_Description => "Będzie budować po trochu wszystkiego";

        public override string Automation_AutomationFocus_WillProduce => "Będzie produkować głównie:";

        public override string Help_Food_WhoEats => "Wszyscy żołnierze i pracownicy konsumują żywność";

        public override string Help_Food_BigArmy => "Wielka armia może doprowadzić do głodu w mieście w swoim zasięgu";

        public override string Help_Food_DontBuild => "Budowanie kolejnych farm nie zwiększa automatycznie ilości żywności; potrzebujesz wolnych pracowników i kuchni, aby ją zbierać i przetwarzać";

        public override string Help_Food_UseWater => "Produkcja żywności wymaga wody";

        public override string Help_Food_Postal => "Upewnij się, że Twoje miasta wspierają się nawzajem, wysyłając żywność";

        public override string Message_LostCity => "Miasto utracone";

        public override string Demo_Description => "Krótki scenariusz: broń miasta przez {0} min";


        //DEMO PATCH 3
        public override string Demo_EndInXMinuteDescription => "Demo zakończy się za {0} min";

        public override string Experience_Required => "Wymagane doświadczenie";

        public override string InputActionName_ToggleMenu => "Przełącz menu";

        //DEMO PATCH 4
        public override string Work_BadValueDescription => "Ilość surowców może spaść poniżej zera i nieznacznie przekroczyć limit zapasów. Granice te są egzekwowane tylko podczas tworzenia kolejki prac.";

        public override string Work_SelectCategory => "Wybierz kategorię przedmiotów";
        public override string Hud_RemoveFromList => "Usuń z listy";

        public override string Hud_ReturnToPrevious => "Wróć";
        public override string Hud_Close => "Zamknij";

        public override string Hud_Low => "Niski";
        public override string Hud_Medium => "Średni";
        public override string Hud_High => "Wysoki";

        public override string Hud_Copy => "Kopiuj";
        //public override string Hud_Paste => "Paste";
        public override string Hud_Cut => "Wytnij";
        public override string Hud_SaveCompleted => "Zapisywanie zakończone";

        public override string Settings_WaterMultiplier => "Mnożnik wody";
        public override string Settings_WaterMultiplier_Description => "Określa, ile wody produkują i przechowują miasta. Wyższe wartości obniżają wydajność komputera.";

        public override string Settings_ChildMultiplier => "Mnożnik narodzin";

        public override string Settings_CraftMultiplier_Description => "Niższe wartości skutkują szybszą produkcją.";

        public override string FastProduction => "Szybka produkcja";
        public override string SlowProduction => "Powolna produkcja";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public override string BlocksProduction => "Nie będzie produkować";

        //public override string CityAutomation_WaitForMaxPopulation => "Wait for population to max out";
        public override string Automation_AutomationFocus_NoFocus => "Wszystko";
        public override string CityAutomation_SoldierQuality => "Jakość żołnierzy";
        public override string CityAutomation_SoldierWeaponType => "Typ broni";

        public override string WarsResourceGroup_Resources => "Surowce";
        public override string WarsResourceGroup_Weapons => "Broń";

        public override string WarsResourceGroup_AllWeaponTypes => "Mieszana";
        public override string WarsResourceGroup_MeleeHandWeapons => "Broń biała";
        public override string WarsResourceGroup_RangedHandWeapons => "Broń dystansowa";
        public override string WarsResourceGroup_Warmachines => "Machiny wojenne";

        public override string FactionSettings_Titel => "Ustawienia całej frakcji";
        public override string FactionSettings_Description => "Ma zastosowanie do wszystkich Twoich miast";

        public override string Conscript_MaxPopulation => "Maksymalna populacja";
        public override string Conscript_MaxPopulation_Description => "Rekrutuje tylko wtedy, gdy populacja osiągnie maksimum";

        public override string Conscript_FoodAbundance => "Maksimum zapasów żywności";
        public override string Conscript_FoodAbundance_Description => "Rekrutuje tylko wtedy, gdy żywność osiągnie maksymalny poziom zapasów";

        /// <summary>
        /// General settings will go through all items in a list and apply to all of them (to their checkbox)
        /// </summary>
        public override string GeneralSetting_On => "Ustaw na: Wł.";
        public override string GeneralSetting_Off => "Ustaw na: Wył.";
        public override string GeneralSetting_AllBuildingsDescription => "Dotyczy wszystkich budynków";

        public override string GeneralSetting_ApplyMessage => "Zastosowano zmiany w {0} budynkach";

        public override string MustTurnOffSteamInput => "Aby używać kontrolerów, musisz wyłączyć funkcję Steam Input.";

        public override string Technology_GainTitle => "Sposoby zdobywania technologii";
        public override string Technology_LevelUp => "Awans poziomu";
        public override string Technology_ForEachLevelUp => "Gdy pracownik awansuje w dziedzinie technologii: {0}";

        public override string VoxelEditor_Description => "Twórz modele z bloków (wokseli)";

        public override string Editor_Tool => "Narzędzie";
        public override string Editor_SelectOptionsMenu => "Opcje zaznaczania";
        public override string Editor_Continous => "Ciągły";
        public override string Editor_Tool_PencilSize => "Rozmiar ołówka";
        public override string Editor_Tool_SizeTolerance => "Tolerancja rozmiaru";
        public override string Editor_Tool_RoundPencil => "Okrągły ołówek";
        public override string Editor_Tool_EdgeSize => "Rozmiar krawędzi";
        public override string Editor_Tool_PercentFill => "Procent wypełnienia";
        public override string Editor_Tool_ClearAbove => "Wyczyść powyżej";
        public override string Editor_Tool_FillBelow => "Wypełnij poniżej";
        public override string Editor_UserModels => "Modele użytkownika";
        public override string Editor_UserModels_Description => "Przeglądaj zapisane modele";

        public override string Editor_RetailModels => "Modele z gry";
        public override string Editor_RetailModels_Description => "Wczytaj modele z plików gry";

        public override string Editor_ModTemplates => "Szablony moderskie";
        public override string Editor_ExportAsOBJ => "Eksportuj jako .OBJ";
        public override string Editor_SelectAll => "Zaznacz wszystko";

        public override string Editor_Canvas_Title => "Płótno (Canvas)";
        public override string Editor_Canvas_Size => "Rozmiar";
        public override string Editor_Canvas_Dimension_X => "X";
        public override string Editor_Canvas_Dimension_Y => "Y";
        public override string Editor_Canvas_Dimension_Z => "Z";
        public override string Editor_Canvas_SizePresets => "Gotowe rozmiary";
        public override string Editor_Canvas_Move => "Przesuń";
        public override string Editor_Canvas_Move_Up => "Góra";
        public override string Editor_Canvas_Move_Down => "Dół";
        public override string Editor_Canvas_RotateClockwise => "Obróć w prawo";
        public override string Editor_Canvas_RotateCounterClockwise => "Obróć w lewo";
        public override string Editor_Canvas_Mirror => "Lustro";

        public override string Editor_Canvas_RotateFlip_Title => "Obrót/Odbicie";
        public override string Editor_Canvas_FlipVertical => "Odbij góra-dół";
        public override string Editor_Canvas_FlipOrientation => "Przełącz leżący/stojący";
        public override string Editor_Canvas_ClearAll_Description => "Usuwa wszystkie bloki i klatki";

        public override string Editor_Animation => "Animacja";
        public override string Editor_Animation_RemoveCurrentFrame => "Usuń bieżącą klatkę";
        public override string Editor_Animation_AddFrameCopy => "Dodaj klatkę jako kopię";
        public override string Editor_Animation_AddEmptyFrame => "Dodaj pustą klatkę";
        public override string Editor_Animation_MoveDescription => "Zmień pozycję klatki";
        public override string Editor_Animation_AllFrames => "Wszystkie klatki";
        public override string Editor_Animation_AllFrames_ActionDescription => "Wykonaj to samo działanie na wszystkich klatkach";

        public override string Editor_SettingsMenu => "Ustawienia";
        public override string Hud_Exit => "Wyjdź";
        public override string Editor_Canvas_Clear => "Wyczyść";

        public override string Editor_Stamp => "Stempel";
        public override string Editor_StampOtherFrames => "Stempluj w innych klatkach";
        public override string Editor_StampOtherFrames_Description => "Wklej woksele w tych klatkach";
        public override string Editor_PasteToFrame => "Wklej woksele w tej klatce";
        public override string Editor_ClearAllFrames => "Wyczyść we wszystkich klatkach";
        public override string Editor_ClearOtherFrames => "Wyczyść pozostałe klatki";

        public override string Editor_Settings_MoveSpeed => "Prędkość ruchu";
        public override string Editor_Settings_BackgroundColor => "Kolor tła";
        public override string Editor_Settings_HideHUD => "Ukryj HUD";

        public override string Editor_Color => "Kolor";
        public override string Editor_ColorsInUseLabel => "Używane kolory";
        public override string Editor_Color_BrighterPlus => "Jaśniej +";
        public override string Editor_Color_Brighter => "Jaśniej";
        public override string Editor_Color_Darker => "Ciemniej";
        public override string Editor_Color_DarkerPlus => "Ciemniej +";
        public override string Editor_Color_RedTint => "Czerwony odcień";
        public override string Editor_Color_Tint => "Odcień";
        public override string Editor_Color_GreenTint => "Zielony odcień";
        public override string Editor_Color_BlueTint => "Niebieski odcień";
        public override string Editor_Color_YellowTint => "Żółty odcień";
        public override string Editor_Color_PurpleTint => "Fioletowy odcień";
        public override string Editor_NoColor => "Puste";

        public override string Editor_Material => "Materiał";

        /// <summary>
        /// User may change one color to another across the model
        /// </summary>
        public override string Editor_Color_Recolor => "Przemaluj";
        public override string Editor_Color_RecolorTo => "Przemaluj na";

        public override string Editor_Material_Set => "Ustaw materiał";

        public override string Editor_Preview => "Podgląd";
        public override string Editor_CombineWithCurrent => "Połącz z bieżącym modelem";

        public override string Editor_PickedColor => "Wybrany";
        public override string Editor_ColorRGBvalues => "R:{0} G:{1} B:{2}";

        public override string BuildingType_ImmigrationTent => "Namiot imigracyjny";
        public override string BuildingType_ImmigrationTent_Description => "Mieści {0} imigrantów";
        public override string BuildingType_ReseachCenter => "Centrum badawcze";
        public override string BuildingType_Bookpress => "Prasa drukarska";
        public override string BuildingType_Bookpress_Description => "W danej dziedzinie badań wszystkie zdobyte punkty będą dzielone ze wszystkimi {0} w Twoich pozostałych miastach.";

        /// <summary>
        /// 0: beer, 1: chemistry, 2: gun powder
        /// </summary>
        public override string Technology_ReseachExample => "Przykład: Kiedy pracownik produkuje {0}, zwiększa swoją umiejętność {1}. Przy awansie doda to punkty do technologii {2}, ponieważ dzielą one dziedzinę {1}.";

        public override string BuildingType_Research_BaseDescription => "Zwiększa postęp badań technologicznych.";

        public override string BuildingType_ResearchCenter_Description => "Dodaje {0} dodatkowych punktów badań technologicznych, gdy pracownik awansuje w tej samej dziedzinie.";

        //DEMO PATCH 5

        public override string Editor_CropSelection => "Kadruj do zaznaczenia";

        public override string Immigrants_DisbandedSoldiers => "Rozwiązane oddziały będą emigrować";
        public override string Immigrants_RefillWorkers => "Szybko uzupełnia siłę roboczą";
        public override string Immigrants_UnhousedAreLost => "Imigranci bez dachu nad głową znikną po pewnym czasie";
        public override string Editor_VoxelCount => "{0} wokseli";

        public override string Editor_Layers_Titel => "Warstwy";
        public override string Editor_Layers_All => "Wszystkie warstwy";
        public override string Editor_LayerNumber => "Warstwa {0}";

        public override string Editor_Layer_AddEmpty => "Dodaj pustą warstwę";
        public override string Editor_Layer_AddCopy => "Duplikuj warstwę";
        public override string Editor_Layer_Remove => "Usuń warstwę";
        public override string Editor_Layer_MergeDown => "Scal w dół";
        public override string Editor_IsAnimated => "Animowane";
        public override string Editor_ToggleVisible => "Przełącz widoczność";
        public override string Editor_ToggleAnimatedLayer => "Przełącz warstwę animowaną";
        public override string Editor_Projects => "Pliki projektów";
        public override string ProfileEditor_ReplaceMaterial => "Kolor profilu: {0}";

        public override string ProfileEditor_ProfileColors_Label => "Kolory profilu";
        public override string ProfileEditor_TunicColor => "Kolor tuniki";
        public override string ProfileEditor_PantsColor => "Kolor spodni";
        public override string ProfileEditor_LeaderColor => "Kolor dowódcy";

        public override string MapStartAs_Water => "Woda";
        public override string MapStartAs_Land => "Ląd";
        public override string MapStartAs_Circle => "Koło";

        public override string Hud_NeedToBeAssigned => "Wymaga przypisania";
        public override string Hud_CommitAssignment => "Przypisz";
        public override string Technology_NoAvailableResearch => "Brak dostępnych badań";

        public override string Research_Tab => "Badania";

        public override string BuildCategory_General => "Ogólne";
        public override string BuildCategory_Military => "Militarne";
        public override string BuildCategory_Decoration => "Dekoracje";
        public override string BuildCategory_Upgrade => "Ulepszenia";
        public override string Work_NoMines => "Brak kopalni";

        //NEXT FEST DEMO
        public override string HUD_DisplayName => "Nazwa wyświetlana";
        public override string HUD_Filter => "Filtr";
        public override string HUD_Scale => "Skala";
        public override string HUD_Tags => "Tagi";
        public override string HUD_ClickToCancel => "Kliknij, aby anulować";

        public override string ObjectTag_Description => "Dodaj symbol na mapie";
        public override string HudPins => "Piny HUD";
        public override string HudPins_Description => "Przypnij informacje do ekranu";

        public override string Lobby_PlayerProfileNumbered => "Profil {0}";
        public override string Lobby_CharacterCreationNumbered => "Postać {0}";
        public override string Lobby_PlayerProfileEdit => "Edytuj profil gracza";

        public override string Editor_ConvertAnimationToLayers => "Konwertuj animację na warstwy";
        public override string Editor_StampAllFrames => "Stempluj na wszystkich klatkach";

        public override string Editor_DisplayOptions => "Opcje wyświetlania";
        public override string Editor_CharacterCreator => "Kreator postaci";
        public override string Editor_CharacterCreator_Description => "Edytor wyglądu modeli wojskowych";
        public override string Editor_HatGenre => "Tryb wyświetlania nakrycia głowy";
        public override string Editor_HatGenre_FollowWeapon => "Zależnie od broni";
        public override string Editor_HatGenre_Uniform => "Mundur";
        public override string Editor_CopyPasteSelectedColor => "Kopiuj z wybranego koloru";

        public override string Character_Accessories => "Akcesoria";
        public override string Character_Hat => "Nakrycie głowy";
        public override string Character_Head => "Głowa";
        public override string Character_Body => "Tułów";
        public override string Character_Arms => "Ramiona";
        public override string Character_Back => "Plecy";
        public override string Character_Face => "Twarz";

        public override string BuildingType_Tavern => "Wielka hala";

        public override string Settings_CraftMultiplier => "Mnożnik czasu rzemiosła";
        public override string Settings_ChildMultiplier_Description => "Zwiększa szybkość przybywania nowych pracowników";

        public override string Settings_CasualControls => "Sterowanie uproszczone";
        public override string Settings_CasualControls_Description => "Upraszcza rozgrywkę, redukując wybory do kluczowych decyzji. Jedynym surowcem są pieniądze.";

        public override string Settings_AdvancedControls => "Sterowanie zaawansowane";
        public override string Settings_AdvancedControls_Description => "Pełne doświadczenie zarządzania surowcami.";

        public override string WarsResourceGroup_Metal => "Metal";
        public override string Work_Craft => "Rzemiosło";
        public override string Work_OnlyCraftOnFullStock => "Wytwarzaj tylko przy pełnych zapasach";

        public override string ExperienceType_Smelting => "Hutnictwo";
        public override string Category_Optimize => "Optymalizuj";
        public override string BuildCategory_Road => "Droga";
        public override string XP_UnlockBuildPrio => "Odblokuj priorytet budowy: {0}";
        public override string Technology_ModernFarming => "Nowoczesne rolnictwo";

        public override string ExportImportDescription => "Aby udostępnić pliki zapisu innym graczom, skopiuj pliki z tego folderu: {0}";

        public override string CityCultureDescription => "Kultura zapewnia miastu specjalny bonus";

        public override string UnitType_CloseRangeRifle => "Arkebuzer";
        public override string UnitType_LongRangeRifle => "Muszkieter";
        public override string UnitType_Skirmisher => "Harcownik";

        //From lumen (light)
        public override string UnitType_MithrilArcher => "Łucznik Lunari";
        public override string UnitType_MithrilSwordsman => "Rycerz Lunari";

        public override string Defence_AutoAssign_Towers => "Przypisz wieże";

        public override string EventMessage_DesertersText_Food => "Głodni żołnierze dezerterują z Twojej armii";

        public override string Tutorial_CasualRecruitSoldiers => "Kup jedną grupę żołnierzy";


        //Shadow update
        public override string Technology_CannotReassign => "Technologii nie można zmienić przed ukończeniem badań";
        public override string Diplomacy_DeclareWarAgainst => "Wypowiesz wojnę przeciwko";
        public override string Diplomacy_AllyCount => "Liczba sojuszników";
        public override string Diplomacy_CostPerAlly => "Koszt wzrasta o {0} na każdego sojusznika";

        public override string Event_ChanceOfFailure => "Szansa na niepowodzenie: {0}%";
        public override string EventMessage_Event_Title => "Wydarzenie";
        public override string EventMessage_TheCohalition => "Koalicja";

        public override string EventMessage_DarkHorde => "Mroczna Horda";
        public override string EventMessage_DarkHordeKiller_Title => "Pogromca Mrocznej Hordy";
        public override string EventMessage_DarkHordeKiller_Message => "Rycerze czempioni dołączyli do Twojej służby";

        public override string Settings_Mode_Spectator_Description => "Tylko obserwuj lub ingeruj za pomocą Boskich Mocy.";
        public override string GodPower => "Boska Moc";

        public override string Building_TreeSprout_Description => "Zasadź drzewo";
        public override string Building_TreeSprout_Soft => "Sadzonka miękkiego drewna";
        public override string Building_TreeSprout_Hard => "Sadzonka twardego drewna";

        public override string GeneralSetting_SetAll => "Zastosuj do wszystkich";

        public override string Hud_All => "Wszystkie";

        public override string Hud_Previous => "Poprzedni";

        public override string Hud_EffectWillStack => "Efekt będzie się kumulować";

        public override string Info_WhenFoodRunsOut => "Gdy skończy się żywność, miasta i armie automatycznie kupią ją na czarnym rynku.";


        //Launch test


        public override string InputActionName_NextWar => "Następna wroga frakcja";

        /// <summary>
        /// Te symbole są potrzebne, aby zmieścić duże liczby w HUD, pojawi się tooltip wyjaśniający, jaką liczbę reprezentują
        /// </summary>
        public override string EngineHud_SymbolFor100 => "c";
        public override string EngineHud_SymbolFor1000 => "k";
        public override string EngineHud_SymbolFor10000 => "10k";

        /// <summary>
        /// Podczas ładowania plików od innych graczy nie otrzymasz ich postępów w osiągnięciach.
        /// </summary>
        public override string GameMenu_BlockImportAchievements => "Blokuj osiągnięcia w importowanych plikach";

        public override string EndScreen_PeaceVictoryQuote => "Złóżmy miecze i powitajmy lepszą przyszłość";

        public override string VictoryType_DefeatBoss => "Boss pokonany";
        public override string VictoryType_Domination => "Dominacja";
        public override string VictoryType_WorldPeace => "Pokój na świecie";





    }
}
